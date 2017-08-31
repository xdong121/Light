using LegacyApplication.Database.Infrastructure;
using LegacyApplication.Models.LightManage;

namespace LegacyApplication.Repositories.LightManage
{
    public interface ILightAlarmRepository : IEntityBaseRepository<LightAlarm>
    {
    }

    public class LightAlarmRepository : EntityBaseRepository<LightAlarm>, ILightAlarmRepository
    {
        public LightAlarmRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
