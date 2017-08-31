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
    [RoutePrefix("api/LightSchDetail")]
    public class LightSchDetailController : ApiControllerBase
    {
        private readonly ILightSchDetailRepository _LightSchDetailRepository;

        public LightSchDetailController(
            ILightSchDetailRepository LightSchDetailRepository,
            IUnitOfWork unitOfWork,
            IDepartmentRepository departmentRepository) : base(unitOfWork, departmentRepository)
        {
            _LightSchDetailRepository = LightSchDetailRepository;
        }

        public async Task<IEnumerable<LightSchDetailViewModel>> Get()
        {
            var models = await _LightSchDetailRepository.AllIncluding(x => x.LightSchMain).ToListAsync();
            var viewModels = Mapper.Map<IEnumerable<LightSchDetail>, IEnumerable<LightSchDetailViewModel>>(models);
            return viewModels;
        }

        public async Task<IHttpActionResult> GetOne(int id)
        {
            var model = await _LightSchDetailRepository.GetSingleAsync(id);
            //var model = await _LightSchDetailRepository.AllIncluding(x=> x.Id==id, x => x.LightSchMain).ToListAsync();
            if (model != null)
            {
                var viewModel = Mapper.Map<LightSchDetail, LightSchDetailViewModel>(model);
                //var viewModel = Mapper.Map<IEnumerable<LightSchDetail>, IEnumerable<LightSchDetailViewModel>>(model);
                return Ok(viewModel);
            }
            return NotFound();
        }

        [Route("SelcetDetail/{mid}")]
        public async Task<IEnumerable<LightSchDetailViewModel>> SelcetDetail(int mid)
        {
            var models = await _LightSchDetailRepository.AllIncluding(x => x.LightSchMainId==mid).ToListAsync();
            var viewModels = Mapper.Map<IEnumerable<LightSchDetail>, IEnumerable<LightSchDetailViewModel>>(models);
            return viewModels;
        }

        [Route("ByPage/{pageIndex}/{pageSize}/{id?}")]
        public async Task<PaginatedItemsViewModel<LightSchDetailViewModel>> GetByPage(int pageIndex, int pageSize, int? id = null)
        {
            var exp = _LightSchDetailRepository.All.AsQueryable();
            if (id != null)
            {
                exp = exp.Where(x => x.Id == id);
            }
            var users = await exp.OrderBy(x => x.Order)
                .Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            var count = await exp.CountAsync();
            var vms = Mapper.Map<IEnumerable<LightSchDetail>, List<LightSchDetailViewModel>>(users);
            var result = new PaginatedItemsViewModel<LightSchDetailViewModel>(pageIndex, pageSize, count, vms);
            return result;
        }

        [HttpPost]
        [Route("BySelect")]
        public async Task<PaginatedItemsViewModel<LightSchDetailViewModel>> BySelect(JObject jObj)
        {
            var exp = _LightSchDetailRepository.All.AsQueryable();

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
           

            var users = await exp.OrderBy(x => x.Order)
                .Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            var count = await exp.CountAsync();
            var vms = Mapper.Map<IEnumerable<LightSchDetail>, List<LightSchDetailViewModel>>(users);
            var result = new PaginatedItemsViewModel<LightSchDetailViewModel>(pageIndex, pageSize, count, vms);
            return result;
        }

        public async Task<IHttpActionResult> Post([FromBody]LightSchDetailViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newModel = Mapper.Map<LightSchDetailViewModel, LightSchDetail>(viewModel);
            newModel.CreateUser = newModel.UpdateUser = User.Identity.Name;
            _LightSchDetailRepository.Add(newModel);
            await UnitOfWork.SaveChangesAsync();

            return RedirectToRoute("", new { controller = "LightSchDetail", id = newModel.Id });
        }

        public async Task<IHttpActionResult> Put(int id, [FromBody]LightSchDetailViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            viewModel.UpdateUser = User.Identity.Name;
            viewModel.UpdateTime = Now;
            viewModel.LastAction = "更新";
            var model = Mapper.Map<LightSchDetailViewModel, LightSchDetail>(viewModel);

            _LightSchDetailRepository.AttachAsModified(model);

            await UnitOfWork.SaveChangesAsync();

            return Ok(viewModel);
        }

        public async Task<IHttpActionResult> Delete(int id)
        {
            var model = await _LightSchDetailRepository.GetSingleAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            _LightSchDetailRepository.Delete(model);
            await UnitOfWork.SaveChangesAsync();
            return Ok();
        }
    }
}