using LegacyApplication.Database.Infrastructure;
using LegacyApplication.Models.LightManage;

namespace LegacyApplication.Repositories.LightManage
{
    public interface ITempTimeSchDetailRepository : IEntityBaseRepository<TempTimeSchDetail>
    {
    }

    public class TempTimeSchDetailRepository : EntityBaseRepository<TempTimeSchDetail>, ITempTimeSchDetailRepository
    {
        public TempTimeSchDetailRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
