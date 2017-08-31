using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using LegacyApplication.Database.Infrastructure;
using LegacyApplication.Models.LightManage;
using LegacyApplication.Repositories.HumanResources;
using LegacyApplication.Repositories.LightManage;
using LegacyApplication.Shared.ByModule.LightManage.Enums;
using LegacyApplication.Shared.Features.Pagination;
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
    [RoutePrefix("api/TempTimeSchMain")]
    public class TempTimeSchMainController: ApiControllerBase
    {
        private readonly ITempTimeSchMainRepository _TempTimeSchMainRepository;
        private readonly ITempTimeSchDetailRepository _TempTimeSchDetailRepository;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ITeleInfoRepository _TeleInfoRepository;

        public TempTimeSchMainController(
            ITempTimeSchMainRepository TempTimeSchMainRepository,
            ITempTimeSchDetailRepository TempTimeSchDetailRepository,
            ITeleInfoRepository teleInfoRepository,
            IUnitOfWork unitOfWork, 
            IDepartmentRepository departmentRepository) : base(unitOfWork, departmentRepository)
        {
            _TempTimeSchMainRepository = TempTimeSchMainRepository;
            _TempTimeSchDetailRepository = TempTimeSchDetailRepository;
            _TeleInfoRepository = teleInfoRepository;
        }

        public async Task<IEnumerable<TempTimeSchMainViewModel>> Get()
        {
            var models = await _TempTimeSchMainRepository.All.ToListAsync();
            var viewModels = Mapper.Map<IEnumerable<TempTimeSchMain>, IEnumerable<TempTimeSchMainViewModel>>(models);
            return viewModels;
        }

        [Route("ByPage/{pageIndex}/{pageSize}/{id?}")]
        public async Task<PaginatedItemsViewModel<TempTimeSchMainViewModel>> GetByPage(int pageIndex, int pageSize, int? id =null)
        {
            var exp = _TempTimeSchMainRepository.All.AsQueryable();
            if (id!=null)
            {
                exp = exp.Where(x => x.Id==id);
            }
            var users = await exp.OrderBy(x => x.Order)
                .Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            var count = await exp.CountAsync();
            var vms = Mapper.Map<IEnumerable<TempTimeSchMain>, List<TempTimeSchMainViewModel>>(users);
            var result = new PaginatedItemsViewModel<TempTimeSchMainViewModel>(pageIndex, pageSize, count, vms);
            return result;
        }

        [HttpPost]
        [Route("BySelect")]
        public async Task<PaginatedItemsViewModel<TempTimeSchMainViewModel>> BySelect(JObject jObj)
        {
            var exp = _TempTimeSchMainRepository.All.AsQueryable();

            var pageIndex = jObj["pageIndex"].ToObject<int>();
            var pageSize = jObj["pageSize"].ToObject<int>();
            if (jObj["id"]!=null)
            {
                if (jObj["id"].Value<string>() != "" && jObj["id"].Value<int?>() != null)
                {
                    var id = jObj["id"].ToObject<int?>();
                    exp = exp.Where(x => x.Id == id);
                }
            }
            if (jObj["startDate1"]!= null && jObj["startDate2"]!= null)
            {
                if (jObj["startDate1"].Value<DateTime?>() != null && jObj["startDate2"].Value<DateTime?>() != null)
                {
                    var startDate1 = jObj["startDate1"].ToObject<DateTime?>();
                    var startDate2 = jObj["startDate2"].ToObject<DateTime?>();
                    exp = exp.Where(x => x.StartDate > startDate1 && x.StartDate < startDate2);
                }
            }

            var users = await exp.OrderBy(x => x.Order)
                .Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            var count = await exp.CountAsync();
            var vms = Mapper.Map<IEnumerable<TempTimeSchMain>, List<TempTimeSchMainViewModel>>(users);
            var result = new PaginatedItemsViewModel<TempTimeSchMainViewModel>(pageIndex, pageSize, count, vms);
            return result;
        }

        public async Task<IHttpActionResult> GetOne(int id)
        {
            var model = await _TempTimeSchMainRepository.GetSingleAsync(id);
            if (model != null)
            {
                var viewModel = Mapper.Map<TempTimeSchMain, TempTimeSchMainViewModel>(model);
                return Ok(viewModel);
            }
            return NotFound();
        }

        public async Task<IHttpActionResult> Post([FromBody]TempTimeSchMainViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newModel = Mapper.Map<TempTimeSchMainViewModel, TempTimeSchMain>(viewModel);
            newModel.CreateUser = newModel.UpdateUser = User.Identity.Name;
            _TempTimeSchMainRepository.Add(newModel);
            await UnitOfWork.SaveChangesAsync();

            return RedirectToRoute("", new { controller = "TempTimeSchMain", id = newModel.Id });
        }

        public async Task<IHttpActionResult> Put(int id, [FromBody]TempTimeSchMainViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            viewModel.UpdateUser = User.Identity.Name;
            viewModel.UpdateTime = Now;
            viewModel.LastAction = "更新";
            var model = Mapper.Map<TempTimeSchMainViewModel, TempTimeSchMain>(viewModel);

            _TempTimeSchMainRepository.AttachAsModified(model);

            await UnitOfWork.SaveChangesAsync();

            return Ok(viewModel);
        }

        public async Task<IHttpActionResult> Delete(int id)
        {
            var model = await _TempTimeSchMainRepository.GetSingleAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            _TempTimeSchMainRepository.Delete(model);
            await UnitOfWork.SaveChangesAsync();
            return Ok();
        }

        //---------------------------------------------------------------------

        //临时时间方案-起止年月
        [HttpPost]
        [Route("TempTimeConfiguration/{id}")]
        public async Task<IHttpActionResult> TempTimeConfiguration(int id )
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = _TempTimeSchMainRepository.GetSingle(id);
            if (model == null)
            {
                return NotFound();
            }

            //更新主档表
            //控制器默认0
            var controlNo = 0;
            var command = new TempTimeConfiguration(controlNo, model.StartDate.Month, model.StartDate.Day, model.EndDate.Month, model.EndDate.Day);
            //将生成的帧保存到TeleInfo电文信息表。
            var TeleInfoModel = new TeleInfoViewModel();
            var newModel = Mapper.Map<TeleInfoViewModel, TeleInfo>(TeleInfoModel);
            newModel.CreateUser = newModel.UpdateUser = User.Identity.Name;
            newModel.Content = command.Command;
            newModel.Type = "in";
            _TeleInfoRepository.Add(newModel);
            //将返回的帧保存到TeleInfo电文信息表。
            var backCommand = new TempTimeConfigurationBack();
            Send:
            try
            {
                backCommand = await command.SendAsync<TempTimeConfigurationBack>(SendController.SendServer, SendController.SendPort);
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
            model.MakeTime = DateTime.Now;
            model.Result = (TempTimeSchMainResult) backCommand.SuccessfulFlag;
            model.UpdateUser = User.Identity.Name;
            model.UpdateTime = Now;
            model.LastAction = "更新";

            _TempTimeSchMainRepository.AttachAsModified(model);

            //更新明细表
            int no = 0;
            var details = _TempTimeSchDetailRepository.All.AsQueryable().Where(x=> x.TempTimeSchMainId==id);
            foreach (TempTimeSchDetail detail in details)
            {
                //控制器默认0
                ++no;
                var commandDetail = new TempTimeControl(controlNo, no, detail.StartTime.Hour, detail.StartTime.Minute, (int)detail.GroupInfoId,detail.Light,(int)detail.IsUse);
                //将生成的帧保存到TeleInfo电文信息表。
                var TeleInfoModelDetail = new TeleInfoViewModel();
                var newModelDetail = Mapper.Map<TeleInfoViewModel, TeleInfo>(TeleInfoModelDetail);
                newModelDetail.CreateUser = newModelDetail.UpdateUser = User.Identity.Name;
                newModelDetail.Content = commandDetail.Command;
                _TeleInfoRepository.Add(newModelDetail);
                //将返回的帧保存到TeleInfo电文信息表。
                var backCommandDetail = new TempTimeControlBack();
                SendDetail:
                try
                {
                    backCommandDetail = await commandDetail.SendAsync<TempTimeControlBack>(SendController.SendServer, SendController.SendPort);
                }
                catch (TcpCommandReadTimeoutException e)
                {
                    //返回的命令如果超时，发送“控制器在线查询命令”。
                    var command1 = new ControlOnlineQuery(0);
                    var backCommand1 = await command1.SendAsync<ControlOnlineQueryBack>(SendController.SendServer, SendController.SendPort);
                    //如果联通继续发送之前的命令，跳转到Send，不联通抛异常。
                    goto SendDetail;
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
                var backTeleInfoModelDetail = new TeleInfoViewModel();
                var backNewModelDetail = Mapper.Map<TeleInfoViewModel, TeleInfo>(backTeleInfoModelDetail);
                backNewModelDetail.CreateUser = backNewModelDetail.UpdateUser = User.Identity.Name;
                backNewModelDetail.Content = backCommandDetail.Command;
                _TeleInfoRepository.Add(backNewModelDetail);
                //用back2字段存储控制器时间
                detail.MakeTime = DateTime.Now;
                detail.Result = backCommandDetail.Command.Contains("0x00") ? TempTimeSchDetailResult.成功 : TempTimeSchDetailResult.失败;
                detail.UpdateUser = User.Identity.Name;
                detail.UpdateTime = Now;
                detail.LastAction = "更新";

                _TempTimeSchDetailRepository.AttachAsModified(detail);
            }
            await UnitOfWork.SaveChangesAsync();

            var viewModel = Mapper.Map<TempTimeSchMain, TempTimeSchMainViewModel>(model);

            return Ok(viewModel);
        }


    }
}