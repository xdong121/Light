using TcpCommands.Extensions;
using TcpCommands.Models.Base;

namespace TcpCommands.Models
{
    public class LocationTimeAdjustQuery : CommandBase
    {
        public LocationTimeAdjustQuery(int controlNo)
        {
            ControlNo = controlNo;
            Data = new[] { controlNo};
        }

        public override string Type => "A4";
    }

    public class LocationTimeAdjustQueryBack : CommandBase, ICommandBack
    {
        public override string Type => "A4";

        public string Response { get; set; }
        
        public int SunriseMode { get; set; }
        public int SunriseTime { get; set; }
        public int SunsetMode { get; set; }
        public int SunsetTime { get; set; }

        public void Resolve()
        {
            var arr = this.ParseCommandBack();
            Data = this.ExtractCommandData(arr);

            ControlNo = Data[0];
            SunriseMode = Data[1];
            SunriseTime = Data[2];
            SunsetMode = Data[3];
            SunsetTime = Data[4];

            Check(arr);
        }
    }
}
