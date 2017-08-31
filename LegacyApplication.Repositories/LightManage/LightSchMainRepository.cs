using LegacyApplication.Database.Infrastructure;
using LegacyApplication.Models.LightManage;

namespace LegacyApplication.Repositories.LightManage
{
    public interface ILightSchMainRepository : IEntityBaseRepository<LightSchMain>
    {
    }

    public class LightSchMainRepository : EntityBaseRepository<LightSchMain>, ILightSchMainRepository
    {
        public LightSchMainRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
