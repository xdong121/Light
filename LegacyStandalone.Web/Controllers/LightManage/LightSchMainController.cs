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
    [RoutePrefix("api/LightSchMain")]
    public class LightSchMainController: ApiControllerBase
    {
        private readonly ILightSchMainRepository _LightSchMainRepository;
        private readonly ILightSchDetailRepository _LightSchDetailRepository;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ITeleInfoRepository _TeleInfoRepository;

        public LightSchMainController(
            ILightSchMainRepository LightSchMainRepository,
            ILightSchDetailRepository LightSchDetailRepository,
            ITeleInfoRepository teleInfoRepository,
            IUnitOfWork unitOfWork, 
            IDepartmentRepository departmentRepository) : base(unitOfWork, departmentRepository)
        {
            _LightSchMainRepository = LightSchMainRepository;
            _LightSchDetailRepository = LightSchDetailRepository;
            _TeleInfoRepository = teleInfoRepository;
        }

        public async Task<IEnumerable<LightSchMainViewModel>> Get()
        {
            //var models = await _LightSchMainRepository.All.ToListAsync();
            var models = await _LightSchMainRepository.AllIncluding(x => x.GroupLightInfo).ToListAsync();
            var viewModels = Mapper.Map<IEnumerable<LightSchMain>, IEnumerable<LightSchMainViewModel>>(models);
            return viewModels;
        }

        public async Task<IHttpActionResult> GetOne(int id)
        {
            var model = await _LightSchMainRepository.GetSingleAsync(id);
            //var model = await _LightSchMainRepository.AllIncluding(x => x.Id==id, x => x.GroupLightInfo).ToListAsync();
            if (model != null)
            {
                var viewModel = Mapper.Map<LightSchMain, LightSchMainViewModel>(model);
                //var viewModel = Mapper.Map<IEnumerable<LightSchMain>, IEnumerable<LightSchMainViewModel>>(model);
                return Ok(viewModel);
            }
            return NotFound();
        }

        [Route("ByPage/{pageIndex}/{pageSize}/{id?}")]
        public async Task<PaginatedItemsViewModel<LightSchMainViewModel>> GetByPage(int pageIndex, int pageSize, int? id =null)
        {
            var exp = _LightSchMainRepository.All.AsQueryable();
            if (id!=null)
            {
                exp = exp.Where(x => x.Id==id);
            }
            var users = await exp.OrderBy(x => x.Order)
                .Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            var count = await exp.CountAsync();
            var vms = Mapper.Map<IEnumerable<LightSchMain>, List<LightSchMainViewModel>>(users);
            var result = new PaginatedItemsViewModel<LightSchMainViewModel>(pageIndex, pageSize, count, vms);
            return result;
        }

        [HttpPost]
        [Route("BySelect")]
        public async Task<PaginatedItemsViewModel<LightSchMainViewModel>> BySelect(JObject jObj)
        {
            var exp = _LightSchMainRepository.All.AsQueryable();

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
                    exp = exp.Where(x => x.MakeTime > startDate1 && x.MakeTime < startDate2);
                }
            }

            var users = await exp.OrderBy(x => x.Order)
                .Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            var count = await exp.CountAsync();
            var vms = Mapper.Map<IEnumerable<LightSchMain>, List<LightSchMainViewModel>>(users);
            var result = new PaginatedItemsViewModel<LightSchMainViewModel>(pageIndex, pageSize, count, vms);
            return result;
        }

        

        public async Task<IHttpActionResult> Post([FromBody]LightSchMainViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newModel = Mapper.Map<LightSchMainViewModel, LightSchMain>(viewModel);
            newModel.CreateUser = newModel.UpdateUser = User.Identity.Name;
            _LightSchMainRepository.Add(newModel);
            await UnitOfWork.SaveChangesAsync();

            return RedirectToRoute("", new { controller = "LightSchMain", id = newModel.Id });
        }

        public async Task<IHttpActionResult> Put(int id, [FromBody]LightSchMainViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            viewModel.UpdateUser = User.Identity.Name;
            viewModel.UpdateTime = Now;
            viewModel.LastAction = "更新";
            var model = Mapper.Map<LightSchMainViewModel, LightSchMain>(viewModel);

            _LightSchMainRepository.AttachAsModified(model);

            await UnitOfWork.SaveChangesAsync();

            return Ok(viewModel);
        }

        public async Task<IHttpActionResult> Delete(int id)
        {
            var model = await _LightSchMainRepository.GetSingleAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            _LightSchMainRepository.Delete(model);
            await UnitOfWork.SaveChangesAsync();
            return Ok();
        }

        //---------------------------------------------------------------------

        //光控设置
        [HttpPost]
        [Route("LightConfiguration/{id}")]
        public async Task<IHttpActionResult> LightConfiguration(int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //主档表
            var model = _LightSchMainRepository.GetSingle(id);
            if (model == null)
            {
                return NotFound();
            }
            //明细表
            var details = _LightSchDetailRepository.All.AsQueryable().Where(x => x.LightSchMainId == id);
            //必须5条记录
            if (details.Count() == 5)
            {
                //取值
                int[] levels = new int[5];
                int[] lights = new int[5];
                int i = 0;
                foreach (var detail in details)
                {
                    levels[i] = detail.Start;
                    lights[i] = detail.Power;
                    i++;
                }
                var command = new LightConfiguration(Int32.Parse(model.ControlNo), levels[0], levels[1], levels[2],
                    levels[3], levels[4], lights[0], lights[1], lights[2], lights[3], lights[4]);
                //将生成的帧保存到TeleInfo电文信息表。
                var TeleInfoModel = new TeleInfoViewModel();
                var newModel = Mapper.Map<TeleInfoViewModel, TeleInfo>(TeleInfoModel);
                newModel.CreateUser = newModel.UpdateUser = User.Identity.Name;
                newModel.Content = command.Command;
                newModel.Type = "in";
                _TeleInfoRepository.Add(newModel);
                //将返回的帧保存到TeleInfo电文信息表。
                var backCommand = new LightConfigurationBack();
                Send:
                try
                {
                    backCommand =
                        await command.SendAsync<LightConfigurationBack>(SendController.SendServer,
                            SendController.SendPort);
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
                //用back2字段存储控制器时间
                model.MakeTime = DateTime.Now;
                model.Result =(LightSchMainResult) backCommand.SuccessfulFlag;
                model.UpdateUser = User.Identity.Name;
                model.UpdateTime = Now;
                model.LastAction = "更新";

                _LightSchMainRepository.AttachAsModified(model);


                await UnitOfWork.SaveChangesAsync();

                var viewModel = Mapper.Map<LightSchMain, LightSchMainViewModel>(model);

                return Ok(viewModel);
            }
            return NotFound();
        }


    }
}