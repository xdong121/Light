using TcpCommands.Extensions;
using TcpCommands.Models.Base;

namespace TcpCommands.Models
{
    public class TempTimeQuery : CommandBase
    {
        public TempTimeQuery(int controlNo)
        {
            ControlNo = controlNo;
            Data = new[] { controlNo };
        }

        public override string Type => "A2";
    }

    public class TempTimeQueryBack : CommandBase, ICommandBack
    {
        public override string Type => "A2";

        public string Response { get; set; }

        public int StartMonth { get; set; }
        public int StartDay { get; set; }
        public int EndMonth { get; set; }
        public int EndDay { get; set; }

        public void Resolve()
        {
            var arr = this.ParseCommandBack();
            Data = this.ExtractCommandData(arr);

            ControlNo = Data[0];
            StartMonth = Data[1];
            StartDay = Data[2];
            EndMonth = Data[3];
            EndDay = Data[4];

            Check(arr);
        }
    }
}
