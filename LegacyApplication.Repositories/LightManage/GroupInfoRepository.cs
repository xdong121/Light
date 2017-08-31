using LegacyApplication.Database.Infrastructure;
using LegacyApplication.Models.LightManage;

namespace LegacyApplication.Repositories.LightManage
{
    public interface IGroupInfoRepository : IEntityBaseRepository<GroupInfo>
    {
    }

    public class GroupInfoRepository : EntityBaseRepository<GroupInfo>, IGroupInfoRepository
    {
        public GroupInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
