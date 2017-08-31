using TcpCommands.Extensions;
using TcpCommands.Models.Base;

namespace TcpCommands.Models
{
    public class TempTimeConfiguration : CommandBase
    {
        public TempTimeConfiguration(int controlNo, int startMonth, int startDay, int endMonth, int endDay)
        {
            ControlNo = controlNo;
            StartMonth = startMonth;
            StartDay = startDay;
            EndMonth = endMonth;
            EndDay = endDay;
            Data = new[] { controlNo, startMonth, startDay, endMonth, endDay };
        }

        public override string Type => "02";

        public int StartMonth { get; set; }
        public int StartDay { get; set; }
        public int EndMonth { get; set; }
        public int EndDay { get; set; }
    }

    public class TempTimeConfigurationBack : CommandBase, ICommandBack
    {
        public override string Type => "02";

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
