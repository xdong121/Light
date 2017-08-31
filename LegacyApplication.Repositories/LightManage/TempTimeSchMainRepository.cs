using LegacyApplication.Database.Infrastructure;
using LegacyApplication.Models.LightManage;

namespace LegacyApplication.Repositories.LightManage
{
    public interface ITempTimeSchMainRepository : IEntityBaseRepository<TempTimeSchMain>
    {
    }

    public class TempTimeSchMainRepository : EntityBaseRepository<TempTimeSchMain>, ITempTimeSchMainRepository
    {
        public TempTimeSchMainRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
