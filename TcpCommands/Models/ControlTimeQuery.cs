using TcpCommands.Extensions;
using TcpCommands.Models.Base;

namespace TcpCommands.Models
{
    public class ControlTimeQuery : CommandBase
    {
        public ControlTimeQuery(int controlNo)
        {
            ControlNo = controlNo;

            Data = new[] { controlNo };
        }

        public override string Type => "AB";

    }

    public class ControlTimeQueryBack : CommandBase, ICommandBack
    {
        public override string Type => "AB";

        public string Response { get; set; }

        public void Resolve()
        {
            var arr = this.ParseCommandBack();
            Data = this.ExtractCommandData(arr);

            ControlNo = Data[0];
            Year = (Data[2] << 8) | Data[1];
            Month = Data[3];
            Day = Data[4];
            Hour = Data[5];
            Minute = Data[6];
            Second = Data[7];

            Check(arr);
        }

        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
    }
}
