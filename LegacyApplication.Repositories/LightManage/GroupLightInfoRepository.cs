using LegacyApplication.Database.Infrastructure;
using LegacyApplication.Models.LightManage;

namespace LegacyApplication.Repositories.LightManage
{
    public interface IGroupLightInfoRepository : IEntityBaseRepository<GroupLightInfo>
    {
        
    }

    public class GroupLightInfoRepository : EntityBaseRepository<GroupLightInfo>, IGroupLightInfoRepository
    {
        public GroupLightInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
