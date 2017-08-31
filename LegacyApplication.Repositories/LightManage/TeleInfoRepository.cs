using LegacyApplication.Database.Infrastructure;
using LegacyApplication.Models.LightManage;

namespace LegacyApplication.Repositories.LightManage
{
    public interface ITeleInfoRepository : IEntityBaseRepository<TeleInfo>
    {
    }

    public class TeleInfoRepository : EntityBaseRepository<TeleInfo>, ITeleInfoRepository
    {
        public TeleInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
