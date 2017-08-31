using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using LegacyApplication.Database.Infrastructure;
using LegacyApplication.Models.LightManage;
using LegacyApplication.Repositories.HumanResources;
using LegacyApplication.Repositories.LightManage;
using LegacyApplication.Shared.Features.Pagination;
using LegacyApplication.ViewModels.LightManage;
using LegacyStandalone.Web.Controllers.Bases;
using Newtonsoft.Json.Linq;

namespace LegacyStandalone.Web.Controllers.LightManage
{
    [RoutePrefix("api/TempTimeSchDetail")]
    public class TempTimeSchDetailController : ApiControllerBase
    {
        private readonly ITempTimeSchDetailRepository _TempTimeSchDetailRepository;

        public TempTimeSchDetailController(
            ITempTimeSchDetailRepository TempTimeSchDetailRepository,
            IUnitOfWork unitOfWork,
            IDepartmentRepository departmentRepository) : base(unitOfWork, departmentRepository)
        {
            _TempTimeSchDetailRepository = TempTimeSchDetailRepository;
        }

        public async Task<IEnumerable<TempTimeSchDetailViewModel>> Get()
        {
            var models = await _TempTimeSchDetailRepository.AllIncluding(x => x.Group, x => x.TempTimeSchMain).ToListAsync();
            var viewModels = Mapper.Map<IEnumerable<TempTimeSchDetail>, IEnumerable<TempTimeSchDetailViewModel>>(models);
            return viewModels;
        }

        public async Task<IHttpActionResult> GetOne(int id)
        {
            var model = await _TempTimeSchDetailRepository.GetSingleAsync(id);
            if (model != null)
            {
                var viewModel = Mapper.Map<TempTimeSchDetail, TempTimeSchDetailViewModel>(model);
                return Ok(viewModel);
            }
            return NotFound();
        }

        [Route("SelcetDetail/{mid}")]
        public async Task<IEnumerable<TempTimeSchDetailViewModel>> SelcetDetail(int mid)
        {
            var models = await _TempTimeSchDetailRepository.AllIncluding(x => x.Group, x => x.TempTimeSchMainId==mid).ToListAsync();
            var viewModels = Mapper.Map<IEnumerable<TempTimeSchDetail>, IEnumerable<TempTimeSchDetailViewModel>>(models);
            return viewModels;
        }

        [Route("ByPage/{pageIndex}/{pageSize}/{id?}")]
        public async Task<PaginatedItemsViewModel<TempTimeSchDetailViewModel>> GetByPage(int pageIndex, int pageSize, int? id = null)
        {
            var exp = _TempTimeSchDetailRepository.All.AsQueryable();
            if (id != null)
            {
                exp = exp.Where(x => x.Id == id);
            }
            var users = await exp.OrderBy(x => x.Order)
                .Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            var count = await exp.CountAsync();
            var vms = Mapper.Map<IEnumerable<TempTimeSchDetail>, List<TempTimeSchDetailViewModel>>(users);
            var result = new PaginatedItemsViewModel<TempTimeSchDetailViewModel>(pageIndex, pageSize, count, vms);
            return result;
        }

        [HttpPost]
        [Route("BySelect")]
        public async Task<PaginatedItemsViewModel<TempTimeSchDetailViewModel>> BySelect(JObject jObj)
        {
            var exp = _TempTimeSchDetailRepository.All.AsQueryable();

            var pageIndex = jObj["pageIndex"].ToObject<int>();
            var pageSize = jObj["pageSize"].ToObject<int>();
            if (jObj["id"] != null)
            {
                if (jObj["id"].Value<string>() != "" && jObj["id"].Value<int?>() != null)
                {
                    var id = jObj["id"].ToObject<int?>();
                    exp = exp.Where(x => x.Id == id);
                }
            }
            if (jObj["startDate1"] != null && jObj["startDate2"] != null)
            {
                if (jObj["startDate1"].Value<DateTime?>() != null && jObj["startDate2"].Value<DateTime?>() != null)
                {
                    var startDate1 = jObj["startDate1"].ToObject<DateTime?>();
                    var startDate2 = jObj["startDate2"].ToObject<DateTime?>();
                    exp = exp.Where(x => x.StartTime > startDate1 && x.StartTime < startDate2);
                }
            }

            var users = await exp.OrderBy(x => x.Order)
                .Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            var count = await exp.CountAsync();
            var vms = Mapper.Map<IEnumerable<TempTimeSchDetail>, List<TempTimeSchDetailViewModel>>(users);
            var result = new PaginatedItemsViewModel<TempTimeSchDetailViewModel>(pageIndex, pageSize, count, vms);
            return result;
        }

        public async Task<IHttpActionResult> Post([FromBody]TempTimeSchDetailViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newModel = Mapper.Map<TempTimeSchDetailViewModel, TempTimeSchDetail>(viewModel);
            newModel.CreateUser = newModel.UpdateUser = User.Identity.Name;
            _TempTimeSchDetailRepository.Add(newModel);
            await UnitOfWork.SaveChangesAsync();

            return RedirectToRoute("", new { controller = "TempTimeSchDetail", id = newModel.Id });
        }

        public async Task<IHttpActionResult> Put(int id, [FromBody]TempTimeSchDetailViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            viewModel.UpdateUser = User.Identity.Name;
            viewModel.UpdateTime = Now;
            viewModel.LastAction = "更新";
            var model = Mapper.Map<TempTimeSchDetailViewModel, TempTimeSchDetail>(viewModel);

            _TempTimeSchDetailRepository.AttachAsModified(model);

            await UnitOfWork.SaveChangesAsync();

            return Ok(viewModel);
        }

        public async Task<IHttpActionResult> Delete(int id)
        {
            var model = await _TempTimeSchDetailRepository.GetSingleAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            _TempTimeSchDetailRepository.Delete(model);
            await UnitOfWork.SaveChangesAsync();
            return Ok();
        }
    }
}