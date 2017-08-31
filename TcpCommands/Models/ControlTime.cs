using TcpCommands.Extensions;
using TcpCommands.Models.Base;

namespace TcpCommands.Models
{
    public class ControlTime : CommandBase
    {
        public ControlTime(int controlNo, int year, int month, int day, int hour, int minute, int second)
        {
            ControlNo = controlNo;

            Year = year;
            byte y0 = (byte)year, y1 = (byte)(year >> 8);
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;

            Data = new[] { controlNo, y0, y1, Month, Day, Hour, Minute, Second };
        }

        public override string Type => "0B";

        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
    }

    public class ControlTimeBack : CommandBase, ICommandBack
    {
        public override string Type => "0B";

        public string Response { get; set; }

        public void Resolve()
        {
            var arr = this.ParseCommandBack();
            Data = this.ExtractCommandData(arr);

            ControlNo = Data[0];
            SuccessfulFlag = Data[1];

            Check(arr);
        }

        public int SuccessfulFlag { get; set; }
    }
}
