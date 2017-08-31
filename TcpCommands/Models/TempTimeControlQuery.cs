using TcpCommands.Extensions;
using TcpCommands.Models.Base;

namespace TcpCommands.Models
{
    public class TempTimeControlQuery : CommandBase
    {
        public TempTimeControlQuery(int controlNo)
        {
            ControlNo = controlNo;
            Data = new[] { controlNo };
        }
        public override string Type => "A3";
    }

    public class TempTimeControlQueryBack : CommandBase, ICommandBack
    {
        public override string Type => "A3";

        public string Response { get; set; }

        public int No { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Group { get; set; }
        public int Level { get; set; }
        public int StartStop { get; set; }

        public void Resolve()
        {
            var arr = this.ParseCommandBack();
            Data = this.ExtractCommandData(arr);

            ControlNo = Data[0];
            No = Data[1];
            Hour = Data[2];
            Minute = Data[3];
            Group = Data[4];
            Level = Data[5];
            StartStop = Data[6];

            Check(arr);
        }
    }
}
