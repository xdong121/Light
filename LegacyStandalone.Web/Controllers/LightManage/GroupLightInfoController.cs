using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using LegacyApplication.Database.Infrastructure;
using LegacyApplication.Models.HumanResources;
using LegacyApplication.Models.LightManage;
using LegacyApplication.Repositories.HumanResources;
using LegacyApplication.Repositories.LightManage;
using LegacyApplication.Shared.ByModule.LightManage.Enums;
using LegacyApplication.Shared.Features.Pagination;
using LegacyApplication.Shared.Features.Tree;
using LegacyApplication.ViewModels.HumanResources;
using LegacyApplication.ViewModels.LightManage;
using LegacyStandalone.Web.Controllers.Bases;
using LegacyStandalone.Web.Controllers.LightManage.PublicControllers;
using Newtonsoft.Json.Linq;
using NLog;
using TcpCommands.Exceptions;
using TcpCommands.Extensions;
using TcpCommands.Models;

namespace LegacyStandalone.Web.Controllers.LightManage
{

    [RoutePrefix("api/GroupLightInfo")]
    public class GroupLightInfoController : ApiControllerBase
    {
        private readonly IGroupLightInfoRepository _GroupLightInfoRepository;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ITeleInfoRepository _TeleInfoRepository;

        public GroupLightInfoController(
            IGroupLightInfoRepository GroupLightInfoRepository,
            ITeleInfoRepository teleInfoRepository,
            IUnitOfWork unitOfWork,
            IDepartmentRepository departmentRepository) : base(unitOfWork, departmentRepository)
        {
            _GroupLightInfoRepository = GroupLightInfoRepository;
            _TeleInfoRepository = teleInfoRepository;
        }

        public async Task<IEnumerable<GroupLightInfoViewModel>> Get()
        {
            var models = await _GroupLightInfoRepository.All.ToListAsync();
            var viewModels = Mapper.Map<IEnumerable<GroupLightInfo>, IEnumerable<GroupLightInfoViewModel>>(models);
            return viewModels;
        }

        public async Task<IHttpActionResult> GetOne(int id)
        {
            var model = await _GroupLightInfoRepository.GetSingleAsync(id);
            if (model != null)
            {
                var viewModel = Mapper.Map<GroupLightInfo, GroupLightInfoViewModel>(model);
                return Ok(viewModel);
            }
            return NotFound();
        }

        public async Task<IHttpActionResult> Post([FromBody]GroupLightInfoViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newModel = Mapper.Map<GroupLightInfoViewModel, GroupLightInfo>(viewModel);
            newModel.CreateUser = newModel.UpdateUser = User.Identity.Name;
            if (newModel.ParentId > 0)
            {
                var parent = await _GroupLightInfoRepository.GetSingleAsync(newModel.ParentId.Value);
                if (parent != null)
                {
                    newModel.AncestorIds = parent.GetAncestorIdsAsParent();
                }
            }

            _GroupLightInfoRepository.Add(newModel);
            await UnitOfWork.SaveChangesAsync();

            return RedirectToRoute("", new { controller = "GroupLightInfo", id = newModel.Id });
        }

        public async Task<IHttpActionResult> Put(int id, [FromBody]GroupLightInfoViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = _GroupLightInfoRepository.GetSingle(id);
            if (model == null)
            {
                return NotFound();
            }
            if (model.ParentId != viewModel.ParentId)
            {
                model.ParentId = viewModel.ParentId;
                if (model.ParentId.HasValue)
                {
                    var parent = await _GroupLightInfoRepository.GetSingleAsync(model.ParentId.Value);
                    if (parent != null)
                    {
                        model.AncestorIds = parent.GetAncestorIdsAsParent();
                    }
                }
                else
                {
                    model.AncestorIds = null;
                }
            }

            model.Describe = viewModel.Describe;
            model.Address = viewModel.Address;
            model.Longitude = viewModel.Longitude;
            model.Latitude = viewModel.Latitude;
            model.ReMark = viewModel.ReMark;
            model.GroupInfoId = viewModel.GroupInfoId;
            model.UpdateUser = User.Identity.Name;
            model.UpdateTime = Now;
            model.LastAction = "更新";

            _GroupLightInfoRepository.AttachAsModified(model);

            await UnitOfWork.SaveChangesAsync();

            viewModel = Mapper.Map<GroupLightInfo, GroupLightInfoViewModel>(model);

            return Ok(viewModel);
        }

        [Route("ToGroup/{id}/{obj}")]
        public async Task<IHttpActionResult> GetToGroup(int id, JObject obj)
        {
            var exp = _GroupLightInfoRepository.All.AsQueryable();
            var ids = obj["id"].ToObject<List<int>>();
            //foreach (var o in ids)
            //{

            //}
            //var users = await exp.OrderBy(x => x.Order)
            //    .Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            //var count = await exp.CountAsync();
            //var vms = Mapper.Map<IEnumerable<GroupLightInfo>, List<GroupLightInfoViewModel>>(users);
            //var result = new PaginatedItemsViewModel<GroupLightInfoViewModel>(pageIndex, pageSize, count, vms);
            //return result;
            return Ok();
        }



        public async Task<IHttpActionResult> Delete(int id)
        {
            var model = await _GroupLightInfoRepository.GetSingleAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            _GroupLightInfoRepository.Delete(model);
            await UnitOfWork.SaveChangesAsync();
            return Ok();
        }

        // ---------------------------------------------------------------------------------------------

        [Route("Tree")]
        public async Task<IEnumerable<TreeEntityBase<GroupLightInfoViewModel>>> GetTree()
        {
            var models = await _GroupLightInfoRepository.All.ToListAsync();
            var viewModels = Mapper.Map<IEnumerable<GroupLightInfo>, IEnumerable<GroupLightInfoViewModel>>(models);
            var roots = viewModels.ToMultipleRoots();
            return roots;
        }

        //查询“控制器时间”查询命令，类型码0xAB.
        [HttpPost]
        [Route("SelectNoTime/{id}")]
        public async Task<IHttpActionResult> SelectNoTime(int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = _GroupLightInfoRepository.GetSingle(id);
            if (model == null)
            {
                return NotFound();
            }

            //控制器时间查询命令
            var controlNo = Int32.Parse(model.No);
            var command = new ControlTimeQuery(controlNo);
            //将生成的帧保存到TeleInfo电文信息表。
            var TeleInfoModel = new TeleInfoViewModel();
            var newModel = Mapper.Map<TeleInfoViewModel, TeleInfo>(TeleInfoModel);
            newModel.CreateUser = newModel.UpdateUser = User.Identity.Name;
            newModel.Content = command.Command;
            newModel.Type = "in";
            _TeleInfoRepository.Add(newModel);
            //将返回的帧保存到TeleInfo电文信息表。
            var backCommand = new ControlTimeQueryBack();
            Send:
            try
            {
                backCommand = await command.SendAsync<ControlTimeQueryBack>(SendController.SendServer, SendController.SendPort);
            }
            catch (TcpCommandReadTimeoutException e)
            {
                //返回的命令如果超时，发送“控制器在线查询命令”。
                var command1 = new ControlOnlineQuery(0);
                var backCommand1 = await command1.SendAsync<ControlOnlineQueryBack>(SendController.SendServer, SendController.SendPort);
                //如果联通继续发送之前的命令，跳转到Send，不联通抛异常。
                goto Send;
            }
            catch (CheckBitNotMatchException e)
            {
                Logger.Error(e);
                //return InternalServerError(e);
            }
            catch (ArgumentNullException e)
            {
                Logger.Error(e);
                //throw;
                //return InternalServerError(e);
            }
            catch (SocketException e)
            {
                Logger.Error(e);
                //return InternalServerError(e);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                //return InternalServerError(e);
            }
            //var backCommand = await command.SendAsync<ControlTimeQueryBack>(SendController.SendServer, SendController.SendPort);
            var backTeleInfoModel = new TeleInfoViewModel();
            var backNewModel = Mapper.Map<TeleInfoViewModel, TeleInfo>(backTeleInfoModel);
            backNewModel.CreateUser = backNewModel.UpdateUser = User.Identity.Name;
            backNewModel.Content = backCommand.Command;
            backNewModel.Type = "in";
            _TeleInfoRepository.Add(backNewModel);
            //用back2字段存储控制器时间
            model.Back2 = backCommand.Year + "-" + backCommand.Month + "-" + backCommand.Day + " " + backCommand.Hour + ":" + backCommand.Minute + ":" + backCommand.Second;
            model.UpdateUser = User.Identity.Name;
            model.UpdateTime = Now;
            model.LastAction = "更新";

            _GroupLightInfoRepository.AttachAsModified(model);

            await UnitOfWork.SaveChangesAsync();

            var viewModel = Mapper.Map<GroupLightInfo, GroupLightInfoViewModel>(model);

            return Ok(viewModel);
        }

        //设置控制器时间
        [HttpPost]
        [Route("SetNoTime/{id}")]
        public async Task<IHttpActionResult> SetNoTime(int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = _GroupLightInfoRepository.GetSingle(id);
            if (model == null)
            {
                return NotFound();
            }

            //控制器时间查询命令
            var dt = DateTime.Now;
            var controlNo = Int32.Parse(model.No);
            var command = new ControlTime(controlNo, dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
            //将生成的帧保存到TeleInfo电文信息表。
            var TeleInfoModel = new TeleInfoViewModel();
            var newModel = Mapper.Map<TeleInfoViewModel, TeleInfo>(TeleInfoModel);
            newModel.CreateUser = newModel.UpdateUser = User.Identity.Name;
            newModel.Content = command.Command;
            newModel.Type = "in";
            _TeleInfoRepository.Add(newModel);
            //将返回的帧保存到TeleInfo电文信息表。
            var backCommand = new ControlTimeBack();
            Send:
            try
            {
                backCommand = await command.SendAsync<ControlTimeBack>(SendController.SendServer, SendController.SendPort);
            }
            catch (TcpCommandReadTimeoutException e)
            {
                //返回的命令如果超时，发送“控制器在线查询命令”。
                var command1 = new ControlOnlineQuery(0);
                var backCommand1 = await command1.SendAsync<ControlOnlineQueryBack>(SendController.SendServer, SendController.SendPort);
                //如果联通继续发送之前的命令，跳转到Send，不联通抛异常。
                goto Send;
            }
            catch (CheckBitNotMatchException e)
            {
                Logger.Error(e);
                //return InternalServerError(e);
            }
            catch (ArgumentNullException e)
            {
                Logger.Error(e);
                //throw;
                //return InternalServerError(e);
            }
            catch (SocketException e)
            {
                Logger.Error(e);
                //return InternalServerError(e);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                //return InternalServerError(e);
            }
            //var backCommand = await command.SendAsync<ControlTimeQueryBack>(SendController.SendServer, SendController.SendPort);
            var backTeleInfoModel = new TeleInfoViewModel();
            var backNewModel = Mapper.Map<TeleInfoViewModel, TeleInfo>(backTeleInfoModel);
            backNewModel.CreateUser = backNewModel.UpdateUser = User.Identity.Name;
            backNewModel.Content = backCommand.Command;
            backNewModel.Type = "in";
            _TeleInfoRepository.Add(backNewModel);
            //用back2字段存储控制器时间
            //model.Back2 =  + "-" + backCommand.Month + "-" + backCommand.Day + " " + backCommand.Hour + ":" + backCommand.Minute + ":" + backCommand.Second;
            //model.UpdateUser = User.Identity.Name;
            //model.UpdateTime = Now;
            //model.LastAction = "更新";

            //_GroupLightInfoRepository.AttachAsModified(model);

            await UnitOfWork.SaveChangesAsync();

            //var viewModel = Mapper.Map<GroupLightInfo, GroupLightInfoViewModel>(model);

            if (backCommand.SuccessfulFlag == 1)
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpPost]
        [Route("BySelect1/{controlNo}/{lightNo}/{lightSonNo}")]
        public async Task<IHttpActionResult> BySelect1(int controlNo, int lightNo, int lightSonNo)
        {
            if (controlNo == 0)
            {
                return null;
            }

            var exp = _GroupLightInfoRepository.AllIncluding(x => x.Parent).AsQueryable();
            var controlNoStr = controlNo.ToString();
            var lightNoStr = lightNo.ToString();
            var lightSonNoStr = lightSonNo.ToString();
            if (lightNo != 0)
            {
                if (lightSonNo != 0)
                {
                    exp = exp.Where(x => x.No == lightSonNoStr && x.Parent != null && x.Parent.No == lightNoStr &&
                                         x.Parent.Parent != null && x.Parent.Parent.No == controlNoStr);
                }
                else
                {
                    exp = exp.Where(x => x.Parent != null && x.Parent.No == lightNoStr &&
                                         x.Parent.Parent != null && x.Parent.Parent.No == controlNoStr);
                }
            }
            else
            {
                exp = exp.Where(x => x.Parent != null && x.Parent.Parent != null
                                     && x.Parent.Parent.No == controlNoStr);
            }
            if (exp.Any())
            {
                var models = await exp.ToListAsync();
                var vms = Mapper.Map<IEnumerable<GroupLightInfo>, List<GroupLightInfoTempViewModel>>(models);

                foreach (var vm in vms)
                {
                    //获取ParentNo
                    var ParentModel = _GroupLightInfoRepository.GetSingle((int)vm.ParentId);
                    vm.ControlNo = controlNo.ToString();
                    vm.LightNo = ParentModel.No;
                    //LED状态
                    var command = new LedStatus(controlNo, Int32.Parse(ParentModel.No), Int32.Parse(vm.No));
                    //将生成的帧保存到TeleInfo电文信息表。
                    var TeleInfoModel = new TeleInfoViewModel();
                    var newModel = Mapper.Map<TeleInfoViewModel, TeleInfo>(TeleInfoModel);
                    newModel.CreateUser = newModel.UpdateUser = User.Identity.Name;
                    newModel.Content = command.Command;
                    newModel.Type = "in";
                    _TeleInfoRepository.Add(newModel);
                    //将返回的帧保存到TeleInfo电文信息表。
                    var backCommand = new LedStatusBack();
                    Send:
                    try
                    {
                        backCommand =
                            await command.SendAsync<LedStatusBack>(SendController.SendServer, SendController.SendPort);
                    }
                    catch (TcpCommandReadTimeoutException e)
                    {
                        //返回的命令如果超时，发送“控制器在线查询命令”。
                        var command1 = new ControlOnlineQuery(0);
                        var backCommand1 =
                            await command1.SendAsync<ControlOnlineQueryBack>(SendController.SendServer,
                                SendController.SendPort);
                        //如果联通继续发送之前的命令，跳转到Send，不联通抛异常。
                        goto Send;
                    }
                    catch (CheckBitNotMatchException e)
                    {
                        Logger.Error(e);
                        //return InternalServerError(e);
                    }
                    catch (ArgumentNullException e)
                    {
                        Logger.Error(e);
                        //throw;
                        //return InternalServerError(e);
                    }
                    catch (SocketException e)
                    {
                        Logger.Error(e);
                        //return InternalServerError(e);
                    }
                    catch (Exception e)
                    {
                        Logger.Error(e);
                        //return InternalServerError(e);
                    }
                    //var backCommand = await command.SendAsync<ControlTimeQueryBack>(SendController.SendServer, SendController.SendPort);
                    var backTeleInfoModel = new TeleInfoViewModel();
                    var backNewModel = Mapper.Map<TeleInfoViewModel, TeleInfo>(backTeleInfoModel);
                    backNewModel.CreateUser = backNewModel.UpdateUser = User.Identity.Name;
                    backNewModel.Content = backCommand.Command;
                    backNewModel.Type = "in";
                    _TeleInfoRepository.Add(backNewModel);

                    //获得返回状态和亮度
                    vm.LightState = backCommand.Status.ToString();
                    vm.LightPower = backCommand.Brightness.ToString();
                }

                await UnitOfWork.SaveChangesAsync();

                return Ok(new
                {
                    result = vms
                });
            }
            return null;
        }


        //LED状态
        [HttpGet]
        [Route("BySelect/{controlNo}/{lightNo}/{lightSonNo}")]
        public async Task<List<GroupLightInfoTempViewModel>> BySelect(int controlNo, int lightNo, int lightSonNo)
        {
            if (controlNo == 0)
            {
                return null;
            }

            var exp = _GroupLightInfoRepository.AllIncluding(x => x.Parent).AsQueryable();
            var controlNoStr = controlNo.ToString();
            var lightNoStr = lightNo.ToString();
            var lightSonNoStr = lightSonNo.ToString();
            if (lightNo != 0)
            {
                if (lightSonNo != 0)
                {
                    exp = exp.Where(x => x.No == lightSonNoStr && x.Parent != null && x.Parent.No == lightNoStr &&
                                         x.Parent.Parent != null && x.Parent.Parent.No == controlNoStr);
                }
                else
                {
                    exp = exp.Where(x => x.Parent != null && x.Parent.No == lightNoStr &&
                                         x.Parent.Parent != null && x.Parent.Parent.No == controlNoStr);
                }
            }
            else
            {
                exp = exp.Where(x => x.Parent != null && x.Parent.Parent != null
                                     && x.Parent.Parent.No == controlNoStr);
            }
            if (exp.Any())
            {
                var models = await exp.ToListAsync();
                var vms = Mapper.Map<IEnumerable<GroupLightInfo>, List<GroupLightInfoTempViewModel>>(models);

                foreach (var vm in vms)
                {
                    //获取ParentNo
                    var ParentModel = _GroupLightInfoRepository.GetSingle((int) vm.ParentId);
                    vm.ControlNo = controlNo.ToString();
                    vm.LightNo = ParentModel.No;
                    //LED状态
                    var command = new LedStatus(controlNo, Int32.Parse(ParentModel.No), Int32.Parse(vm.No));
                    //将生成的帧保存到TeleInfo电文信息表。
                    var TeleInfoModel = new TeleInfoViewModel();
                    var newModel = Mapper.Map<TeleInfoViewModel, TeleInfo>(TeleInfoModel);
                    newModel.CreateUser = newModel.UpdateUser = User.Identity.Name;
                    newModel.Content = command.Command;
                    newModel.Type = "in";
                    _TeleInfoRepository.Add(newModel);
                    //将返回的帧保存到TeleInfo电文信息表。
                    var backCommand = new LedStatusBack();
                    Send:
                    try
                    {
                        backCommand =
                            await command.SendAsync<LedStatusBack>(SendController.SendServer, SendController.SendPort);
                    }
                    catch (TcpCommandReadTimeoutException e)
                    {
                        //返回的命令如果超时，发送“控制器在线查询命令”。
                        var command1 = new ControlOnlineQuery(0);
                        var backCommand1 =
                            await command1.SendAsync<ControlOnlineQueryBack>(SendController.SendServer,
                                SendController.SendPort);
                        //如果联通继续发送之前的命令，跳转到Send，不联通抛异常。
                        goto Send;
                    }
                    catch (CheckBitNotMatchException e)
                    {
                        Logger.Error(e);
                        //return InternalServerError(e);
                    }
                    catch (ArgumentNullException e)
                    {
                        Logger.Error(e);
                        //throw;
                        //return InternalServerError(e);
                    }
                    catch (SocketException e)
                    {
                        Logger.Error(e);
                        //return InternalServerError(e);
                    }
                    catch (Exception e)
                    {
                        Logger.Error(e);
                        //return InternalServerError(e);
                    }
                    //var backCommand = await command.SendAsync<ControlTimeQueryBack>(SendController.SendServer, SendController.SendPort);
                    var backTeleInfoModel = new TeleInfoViewModel();
                    var backNewModel = Mapper.Map<TeleInfoViewModel, TeleInfo>(backTeleInfoModel);
                    backNewModel.CreateUser = backNewModel.UpdateUser = User.Identity.Name;
                    backNewModel.Content = backCommand.Command;
                    backNewModel.Type = "in";
                    _TeleInfoRepository.Add(backNewModel);

                    //获得返回状态和亮度
                    vm.LightState = backCommand.Status.ToString();
                    vm.LightPower = backCommand.Brightness.ToString();
                }

                await UnitOfWork.SaveChangesAsync();

                return vms;
            }
            return null;

        }


    }
}