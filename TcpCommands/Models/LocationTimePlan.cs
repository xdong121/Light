using TcpCommands.Extensions;
using TcpCommands.Models.Base;

namespace TcpCommands.Models
{
    public class LocationTimePlan : CommandBase
    {
        public LocationTimePlan(int controlNo, int no, int hour, int minute, int @group, int level, int mode)
        {
            ControlNo = controlNo;
            No = no;
            Hour = hour;
            Minute = minute;
            Group = @group;
            Level = level;
            Mode = mode;

            Data = new[] { controlNo, No, Hour, Minute, Group, Level, Mode };
        }

        public override string Type => "05";

        public int No { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Group { get; set; }
        public int Level { get; set; }
        public int Mode { get; set; }
    }

    public class LocationTimePlanBack : CommandBase, ICommandBack
    {
        public override string Type => "05";

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
