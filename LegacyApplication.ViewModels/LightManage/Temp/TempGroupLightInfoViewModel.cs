using System.Collections.Generic;

namespace LegacyApplication.ViewModels.LightManage.Temp
{
    public class TempGroupLightInfoViewModel<TEntity> where TEntity : class
    {
        public int LightState { get; }

        public int LightPower { get; }

        public IEnumerable<TEntity> Data { get; }

        public TempGroupLightInfoViewModel(int lightState, int lightPower, IEnumerable<TEntity> data)
        {
            LightState = lightState;
            LightPower = lightPower;
            Data = data;
        }
    }
}

