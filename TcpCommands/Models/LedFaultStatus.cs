using TcpCommands.Extensions;
using TcpCommands.Models.Base;

namespace TcpCommands.Models
{
    public class LedFaultStatus : CommandBase
    {
        public LedFaultStatus(int controlNo, int lineNo, int subNo, int status)
        {
            ControlNo = controlNo;
            LineNo = lineNo;
            SubNo = subNo;
            Status = status;
            Data = new[] { controlNo, LineNo, SubNo, Status };
        }

        public override string Type => "06";

        public int Status { get; set; }
    }

    public class LedFaultStatusBack : CommandBase, ICommandBack
    {
        public override string Type => "06";

        public string Response { get; set; }

        public int Status { get; set; }

        public void Resolve()
        {
            var arr = this.ParseCommandBack();
            Data = this.ExtractCommandData(arr);

            ControlNo = Data[0];
            LineNo = Data[1];
            SubNo = Data[2];
            Status = Data[3];

            Check(arr);
        }
    }
}
