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
using NLog;
using TcpCommands.Exceptions;
using TcpCommands.Extensions;
using TcpCommands.Models;

namespace LegacyStandalone.Web.Controllers.LightManage
{
    [RoutePrefix("api/LightAlarm")]
    public class LightAlarmController: ApiControllerBase
    {
        private readonly ILightAlarmRepository _LightAlarmRepository;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ITeleInfoRepository _TeleInfoRepository;

        public LightAlarmController(
            ILightAlarmRepository LightAlarmRepository,
            ITeleInfoRepository teleInfoRepository,
            IUnitOfWork unitOfWork, 
            IDepartmentRepository departmentRepository) : base(unitOfWork, departmentRepository)
        {
            _LightAlarmRepository = LightAlarmRepository;
            _TeleInfoRepository = teleInfoRepository;
        }

        public async Task<IEnumerable<LightAlarmViewModel>> Get()
        {
            var models = await _LightAlarmRepository.All.ToListAsync();
            var viewModels = Mapper.Map<IEnumerable<LightAlarm>, IEnumerable<LightAlarmViewModel>>(models);
            return viewModels;
        }

        [Route("ByPage/{pageIndex}/{pageSize}/{controlNo?}")]
        public async Task<PaginatedItemsViewModel<LightAlarmViewModel>> GetByPage(int pageIndex, int pageSize, string controlNo = null)
        {
            var exp = _LightAlarmRepository.All.AsQueryable();
            if (!string.IsNullOrEmpty(controlNo))
            {
                exp = exp.Where(x => x.ControlNo.Contains(controlNo) 
                || x.LightNo.Contains(controlNo)
                || x.LightSonNo.Contains(controlNo) );
            }
            var users = await exp.OrderBy(x => x.ControlNo)
                .Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            var count = await exp.CountAsync();
            var vms = Mapper.Map<IEnumerable<LightAlarm>, List<LightAlarmViewModel>>(users);
            var result = new PaginatedItemsViewModel<LightAlarmViewModel>(pageIndex, pageSize, count, vms);
            return result;
        }

        public async Task<IHttpActionResult> GetOne(int id)
        {
            var model = await _LightAlarmRepository.GetSingleAsync(id);
            if (model != null)
            {
                var viewModel = Mapper.Map<LightAlarm, LightAlarmViewModel>(model);
                return Ok(viewModel);
            }
            return NotFound();
        }

        public async Task<IHttpActionResult> Post([FromBody]LightAlarmViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newModel = Mapper.Map<LightAlarmViewModel, LightAlarm>(viewModel);
            newModel.CreateUser = newModel.UpdateUser = User.Identity.Name;
            _LightAlarmRepository.Add(newModel);
            await UnitOfWork.SaveChangesAsync();

            return RedirectToRoute("", new { controller = "LightAlarm", id = newModel.Id });
        }

        public async Task<IHttpActionResult> Put(int id, [FromBody]LightAlarmViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            viewModel.UpdateUser = User.Identity.Name;
            viewModel.UpdateTime = Now;
            viewModel.LastAction = "更新";
            var model = Mapper.Map<LightAlarmViewModel, LightAlarm>(viewModel);

            _LightAlarmRepository.AttachAsModified(model);

            await UnitOfWork.SaveChangesAsync();

            return Ok(viewModel);
        }

        public async Task<IHttpActionResult> Delete(int id)
        {
            var model = await _LightAlarmRepository.GetSingleAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            _LightAlarmRepository.Delete(model);
            await UnitOfWork.SaveChangesAsync();
            return Ok();
        }

        //---------------------------------------------------------------------

        [HttpPost]
        [Route("ByUpdate/{controlNo}/{lightNo}/{lightSonNo}")]
        public async Task<IHttpActionResult> ByUpdate(int controlNo, int lightNo, int lightSonNo)
        {
            if (controlNo == 0 || lightNo == 0 || lightSonNo == 0)
            {
                return NotFound();
            }
            var exp = _LightAlarmRepository.All.AsQueryable();
            exp = exp.Where(x => x.ControlNo.Contains(controlNo.ToString())
                                 && x.LightNo.Contains(lightNo.ToString())
                                 && x.LightSonNo.Contains(lightSonNo.ToString()));
            var models = await exp.ToListAsync();
            var model = models.First<LightAlarm>();

            if (model != null)
            {
                //LED状态
                var command = new LedStatus(controlNo, lightNo, lightSonNo);
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
                if (backCommand.Status == 0)
                {
                    model.IsRepair = LightAlarmIsRepair.已修复;
                    _LightAlarmRepository.AttachAsModified(model);
                }

                await UnitOfWork.SaveChangesAsync();

                var vm = Mapper.Map<LightAlarm, LightAlarmViewModel>(model);

                return Ok(new
                {
                    result = vm
                });
            }
            return NotFound();
        }


        [HttpPost]
        [Route("LedFaultStatus/{controlNo}/{lightNo}/{lightSonNo}")]
        public async Task<IHttpActionResult> LedFaultStatus(int controlNo, int lightNo, int lightSonNo)
        {
            if (controlNo == 0 || lightNo == 0 || lightSonNo == 0)
            {
                return NotFound();
            }
            var exp = _LightAlarmRepository.All.AsQueryable();
            exp = exp.Where(x => x.ControlNo.Contains(controlNo.ToString())
                                 && x.LightNo.Contains(lightNo.ToString())
                                 && x.LightSonNo.Contains(lightSonNo.ToString()));
            var models = await exp.ToListAsync();
            var model = models.First<LightAlarm>();

            if (model != null)
            {
                //LED状态
                var command = new LedFaultStatus(controlNo, lightNo, lightSonNo,0);
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
                if (backCommand.Status == 0)
                {
                    model.IsRepair = LightAlarmIsRepair.已修复;
                    _LightAlarmRepository.AttachAsModified(model);
                }

                await UnitOfWork.SaveChangesAsync();

                var vm = Mapper.Map<LightAlarm, LightAlarmViewModel>(model);

                return Ok(new
                {
                    result = vm
                });
            }
            return NotFound();
        }




    }
}