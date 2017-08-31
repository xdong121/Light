using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using LegacyApplication.Database.Infrastructure;
using LegacyApplication.Models.HumanResources;
using LegacyApplication.Repositories.HumanResources;
using LegacyApplication.ViewModels.HumanResources;
using LegacyStandalone.Web.Controllers.Bases;
using LegacyApplication.Shared.Features.Pagination;


namespace LegacyStandalone.Web.Controllers.HumanResources
{
    [RoutePrefix("api/Nationality")]
    public class NationalityController: ApiControllerBase
    {
        private readonly INationalityRepository _NationalityRepository;

        public NationalityController(
            INationalityRepository NationalityRepository,
            IUnitOfWork unitOfWork, 
            IDepartmentRepository departmentRepository) : base(unitOfWork, departmentRepository)
        {
            _NationalityRepository = NationalityRepository;
        }

        public async Task<IEnumerable<NationalityViewModel>> Get()
        {
            var models = await _NationalityRepository.All.ToListAsync();
            var viewModels = Mapper.Map<IEnumerable<Nationality>, IEnumerable<NationalityViewModel>>(models);
            return viewModels;
        }

        [Route("ByPage/{pageIndex}/{pageSize}/{name?}")]
        public async Task<PaginatedItemsViewModel<NationalityViewModel>> GetByPage(int pageIndex, int pageSize, string name = null)
        {
            var exp = _NationalityRepository.All.AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                exp = exp.Where(x => x.Name.Contains(name));
            }
            var users = await exp.OrderBy(x => x.Name)
                .Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            var count = await exp.CountAsync();
            var vms = Mapper.Map<IEnumerable<Nationality>, List<NationalityViewModel>>(users);
            var result = new PaginatedItemsViewModel<NationalityViewModel>(pageIndex, pageSize, count, vms);
            return result;
        }

        public async Task<IHttpActionResult> GetOne(int id)
        {
            var model = await _NationalityRepository.GetSingleAsync(id);
            if (model != null)
            {
                var viewModel = Mapper.Map<Nationality, NationalityViewModel>(model);
                return Ok(viewModel);
            }
            return NotFound();
        }

        public async Task<IHttpActionResult> Post([FromBody]NationalityViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newModel = Mapper.Map<NationalityViewModel, Nationality>(viewModel);
            newModel.CreateUser = newModel.UpdateUser = User.Identity.Name;
            _NationalityRepository.Add(newModel);
            await UnitOfWork.SaveChangesAsync();

            return RedirectToRoute("", new { controller = "Nationality", id = newModel.Id });
        }

        public async Task<IHttpActionResult> Put(int id, [FromBody]NationalityViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            viewModel.UpdateUser = User.Identity.Name;
            viewModel.UpdateTime = Now;
            viewModel.LastAction = "更新";
            var model = Mapper.Map<NationalityViewModel, Nationality>(viewModel);

            _NationalityRepository.AttachAsModified(model);

            await UnitOfWork.SaveChangesAsync();

            return Ok(viewModel);
        }

        public async Task<IHttpActionResult> Delete(int id)
        {
            var model = await _NationalityRepository.GetSingleAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            _NationalityRepository.Delete(model);
            await UnitOfWork.SaveChangesAsync();
            return Ok();
        }
    }
}