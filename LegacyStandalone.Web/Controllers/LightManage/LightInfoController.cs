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

namespace LegacyStandalone.Web.Controllers.LightManage
{
    [RoutePrefix("api/LightInfo")]
    public class LightInfoController: ApiControllerBase
    {
        private readonly ILightInfoRepository _LightInfoRepository;

        public LightInfoController(
            ILightInfoRepository LightInfoRepository,
            IUnitOfWork unitOfWork, 
            IDepartmentRepository departmentRepository) : base(unitOfWork, departmentRepository)
        {
            _LightInfoRepository = LightInfoRepository;
        }

        public async Task<IEnumerable<LightInfoViewModel>> Get()
        {
            var models = await _LightInfoRepository.All.ToListAsync();
            var viewModels = Mapper.Map<IEnumerable<LightInfo>, IEnumerable<LightInfoViewModel>>(models);
            return viewModels;
        }

        [Route("ByPage/{pageIndex}/{pageSize}/{controlNo?}")]
        public async Task<PaginatedItemsViewModel<LightInfoViewModel>> GetByPage(int pageIndex, int pageSize, string controlNo = null)
        {
            var exp = _LightInfoRepository.All.AsQueryable();
            if (!string.IsNullOrEmpty(controlNo))
            {
                exp = exp.Where(x => x.ControlNo.Contains(controlNo) || x.LightNo.Contains(controlNo)
                || x.Describe.Contains(controlNo) || x.Address.Contains(controlNo));
            }
            var users = await exp.OrderBy(x => x.ControlNo)
                .Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            var count = await exp.CountAsync();
            var vms = Mapper.Map<IEnumerable<LightInfo>, List<LightInfoViewModel>>(users);
            var result = new PaginatedItemsViewModel<LightInfoViewModel>(pageIndex, pageSize, count, vms);
            return result;
        }

        public async Task<IHttpActionResult> GetOne(int id)
        {
            var model = await _LightInfoRepository.GetSingleAsync(id);
            if (model != null)
            {
                var viewModel = Mapper.Map<LightInfo, LightInfoViewModel>(model);
                return Ok(viewModel);
            }
            return NotFound();
        }

        public async Task<IHttpActionResult> Post([FromBody]LightInfoViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newModel = Mapper.Map<LightInfoViewModel, LightInfo>(viewModel);
            newModel.CreateUser = newModel.UpdateUser = User.Identity.Name;
            _LightInfoRepository.Add(newModel);
            await UnitOfWork.SaveChangesAsync();

            return RedirectToRoute("", new { controller = "LightInfo", id = newModel.Id });
        }

        public async Task<IHttpActionResult> Put(int id, [FromBody]LightInfoViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            viewModel.UpdateUser = User.Identity.Name;
            viewModel.UpdateTime = Now;
            viewModel.LastAction = "更新";
            var model = Mapper.Map<LightInfoViewModel, LightInfo>(viewModel);

            _LightInfoRepository.AttachAsModified(model);

            await UnitOfWork.SaveChangesAsync();

            return Ok(viewModel);
        }

        public async Task<IHttpActionResult> Delete(int id)
        {
            var model = await _LightInfoRepository.GetSingleAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            _LightInfoRepository.Delete(model);
            await UnitOfWork.SaveChangesAsync();
            return Ok();
        }
    }
}