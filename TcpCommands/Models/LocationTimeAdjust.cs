using TcpCommands.Extensions;
using TcpCommands.Models.Base;

namespace TcpCommands.Models
{
    public class LocationTimeAdjust : CommandBase
    {
        public LocationTimeAdjust(int controlNo, int sunriseMode, int sunriseTime, int sunsetMode, int sunsetTime)
        {
            ControlNo = controlNo;
            SunriseMode = sunriseMode;
            SunriseTime = sunriseTime;
            SunsetMode = sunsetMode;
            SunsetTime = sunsetTime;

            Data = new[] { controlNo, sunriseMode, sunriseTime, sunsetMode, sunsetTime };
        }

        public override string Type => "04";

        public int SunriseMode { get; set; }
        public int SunriseTime { get; set; }
        public int SunsetMode { get; set; }
        public int SunsetTime { get; set; }
    }

    public class LocationTimeAdjustBack : CommandBase, ICommandBack
    {
        public override string Type => "04";

        public string Response { get; set; }

        public int SuccessfulFlag { get; set; }

        public void Resolve()
        {
            var arr = this.ParseCommandBack();
            Data = this.ExtractCommandData(arr);

            ControlNo = Data[0];
            SuccessfulFlag = Data[1];

            Check(arr);
        }
    }
}
