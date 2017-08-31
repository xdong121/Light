using TcpCommands.Extensions;
using TcpCommands.Models.Base;

namespace TcpCommands.Models
{
    public class TempTimeControl : CommandBase
    {
        public TempTimeControl(int controlNo, int no, int hour, int minute, int @group, int level, int startStop)
        {
            ControlNo = controlNo;
            No = no;
            Hour = hour;
            Minute = minute;
            Group = @group;
            Level = level;
            StartStop = startStop;
            Data = new[] { controlNo, No, Hour, Minute, Group, Level, StartStop };
        }
        public override string Type => "03";

        public int No { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Group { get; set; }
        public int Level { get; set; }
        public int StartStop { get; set; }
    }

    public class TempTimeControlBack : CommandBase, ICommandBack
    {
        public override string Type => "03";

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
