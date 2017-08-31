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
    [RoutePrefix("api/TeleInfo")]
    public class TeleInfoController: ApiControllerBase
    {
        private readonly ITeleInfoRepository _TeleInfoRepository;

        public TeleInfoController(
            ITeleInfoRepository TeleInfoRepository,
            IUnitOfWork unitOfWork, 
            IDepartmentRepository departmentRepository) : base(unitOfWork, departmentRepository)
        {
            _TeleInfoRepository = TeleInfoRepository;
        }

        public async Task<IEnumerable<TeleInfoViewModel>> Get()
        {
            var models = await _TeleInfoRepository.All.ToListAsync();
            var viewModels = Mapper.Map<IEnumerable<TeleInfo>, IEnumerable<TeleInfoViewModel>>(models);
            return viewModels;
        }

        [Route("ByPage/{pageIndex}/{pageSize}/{content?}")]
        public async Task<PaginatedItemsViewModel<TeleInfoViewModel>> GetByPage(int pageIndex, int pageSize, string content = null)
        {
            var exp = _TeleInfoRepository.All.AsQueryable();
            if (!string.IsNullOrEmpty(content))
            {
                exp = exp.Where(x => x.Content.Contains(content));
            }
            var users = await exp.OrderBy(x => x.Order)
                .Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            var count = await exp.CountAsync();
            var vms = Mapper.Map<IEnumerable<TeleInfo>, List<TeleInfoViewModel>>(users);
            var result = new PaginatedItemsViewModel<TeleInfoViewModel>(pageIndex, pageSize, count, vms);
            return result;
        }

        public async Task<IHttpActionResult> GetOne(int id)
        {
            var model = await _TeleInfoRepository.GetSingleAsync(id);
            if (model != null)
            {
                var viewModel = Mapper.Map<TeleInfo, TeleInfoViewModel>(model);
                return Ok(viewModel);
            }
            return NotFound();
        }

        public async Task<IHttpActionResult> Post([FromBody]TeleInfoViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newModel = Mapper.Map<TeleInfoViewModel, TeleInfo>(viewModel);
            newModel.CreateUser = newModel.UpdateUser = User.Identity.Name;
            _TeleInfoRepository.Add(newModel);
            await UnitOfWork.SaveChangesAsync();

            return RedirectToRoute("", new { controller = "TeleInfo", id = newModel.Id });
        }

        public async Task<IHttpActionResult> Put(int id, [FromBody]TeleInfoViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            viewModel.UpdateUser = User.Identity.Name;
            viewModel.UpdateTime = Now;
            viewModel.LastAction = "更新";
            var model = Mapper.Map<TeleInfoViewModel, TeleInfo>(viewModel);

            _TeleInfoRepository.AttachAsModified(model);

            await UnitOfWork.SaveChangesAsync();

            return Ok(viewModel);
        }

        public async Task<IHttpActionResult> Delete(int id)
        {
            var model = await _TeleInfoRepository.GetSingleAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            _TeleInfoRepository.Delete(model);
            await UnitOfWork.SaveChangesAsync();
            return Ok();
        }
    }
}