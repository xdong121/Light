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
    [RoutePrefix("api/GroupInfo")]
    public class GroupInfoController: ApiControllerBase
    {
        private readonly IGroupInfoRepository _GroupInfoRepository;

        public GroupInfoController(
            IGroupInfoRepository GroupInfoRepository,
            IUnitOfWork unitOfWork, 
            IDepartmentRepository departmentRepository) : base(unitOfWork, departmentRepository)
        {
            _GroupInfoRepository = GroupInfoRepository;
        }

        public async Task<IEnumerable<GroupInfoViewModel>> Get()
        {
            var models = await _GroupInfoRepository.All.ToListAsync();
            var viewModels = Mapper.Map<IEnumerable<GroupInfo>, IEnumerable<GroupInfoViewModel>>(models);
            return viewModels;
        }

        //[Route("ByPage/{pageIndex}/{pageSize}/{content?}")]
        //public async Task<PaginatedItemsViewModel<GroupInfoViewModel>> GetByPage(int pageIndex, int pageSize, string content = null)
        //{
        //    var exp = _GroupInfoRepository.All.AsQueryable();
        //    if (!string.IsNullOrEmpty(content))
        //    {
        //        exp = exp.Where(x => x.Content.Contains(content));
        //    }
        //    var users = await exp.OrderBy(x => x.Order)
        //        .Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
        //    var count = await exp.CountAsync();
        //    var vms = Mapper.Map<IEnumerable<GroupInfo>, List<GroupInfoViewModel>>(users);
        //    var result = new PaginatedItemsViewModel<GroupInfoViewModel>(pageIndex, pageSize, count, vms);
        //    return result;
        //}

        public async Task<IHttpActionResult> GetOne(int id)
        {
            var model = await _GroupInfoRepository.GetSingleAsync(id);
            if (model != null)
            {
                var viewModel = Mapper.Map<GroupInfo, GroupInfoViewModel>(model);
                return Ok(viewModel);
            }
            return NotFound();
        }

        public async Task<IHttpActionResult> Post([FromBody]GroupInfoViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newModel = Mapper.Map<GroupInfoViewModel, GroupInfo>(viewModel);
            newModel.CreateUser = newModel.UpdateUser = User.Identity.Name;
            _GroupInfoRepository.Add(newModel);
            await UnitOfWork.SaveChangesAsync();

            return RedirectToRoute("", new { controller = "GroupInfo", id = newModel.Id });
        }

        public async Task<IHttpActionResult> Put(int id, [FromBody]GroupInfoViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            viewModel.UpdateUser = User.Identity.Name;
            viewModel.UpdateTime = Now;
            viewModel.LastAction = "更新";
            var model = Mapper.Map<GroupInfoViewModel, GroupInfo>(viewModel);

            _GroupInfoRepository.AttachAsModified(model);

            await UnitOfWork.SaveChangesAsync();

            return Ok(viewModel);
        }

        public async Task<IHttpActionResult> Delete(int id)
        {
            var model = await _GroupInfoRepository.GetSingleAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            _GroupInfoRepository.Delete(model);
            await UnitOfWork.SaveChangesAsync();
            return Ok();
        }
    }
}