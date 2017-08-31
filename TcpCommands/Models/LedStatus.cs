using TcpCommands.Extensions;
using TcpCommands.Models.Base;

namespace TcpCommands.Models
{
    public class LedStatus : CommandBase
    {
        public LedStatus(int controlNo, int lineNo, int subNo)
        {
            ControlNo = controlNo;
            LineNo = lineNo;
            SubNo = subNo;
            Data = new[] { controlNo, LineNo, SubNo };
        }

        public override string Type => "07";
    }

    public class LedStatusBack : CommandBase, ICommandBack
    {
        public override string Type => "07";

        public string Response { get; set; }

        public int Status { get; set; }
        public int Brightness { get; set; }

        public void Resolve()
        {
            var arr = this.ParseCommandBack();
            Data = this.ExtractCommandData(arr);

            ControlNo = Data[0];
            LineNo = Data[1];
            SubNo = Data[2];
            Status = Data[3];
            Brightness = Data[4];

            Check(arr);
        }
    }
}
