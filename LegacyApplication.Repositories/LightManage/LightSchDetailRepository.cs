using LegacyApplication.Database.Infrastructure;
using LegacyApplication.Models.LightManage;

namespace LegacyApplication.Repositories.LightManage
{
    public interface ILightSchDetailRepository : IEntityBaseRepository<LightSchDetail>
    {
    }

    public class LightSchDetailRepository : EntityBaseRepository<LightSchDetail>, ILightSchDetailRepository
    {
        public LightSchDetailRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
