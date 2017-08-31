using LegacyApplication.Database.Infrastructure;
using LegacyApplication.Models.LightManage;

namespace LegacyApplication.Repositories.LightManage
{
    public interface ILightInfoRepository : IEntityBaseRepository<LightInfo>
    {
    }

    public class LightInfoRepository : EntityBaseRepository<LightInfo>, ILightInfoRepository
    {
        public LightInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
