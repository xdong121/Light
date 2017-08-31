using System.Globalization;
using System.Linq;
using TcpCommands.Exceptions;
using TcpCommands.Extensions;
using TcpCommands.Models.Base;

namespace TcpCommands.Models
{
    public class GroupQuery : CommandBase
    {
        public GroupQuery(int controlNo, int lineNo)
        {
            ControlNo = controlNo;
            LineNo = lineNo;
            Data = new[] { ControlNo, LineNo };
        }

        public override string Type => "A1";
    }

    public class GroupQueryBack : CommandBase, ICommandBack
    {
        public override string Type => "A1";

        public string Response { get; set; }

        public void Resolve()
        {
            var arr = this.ParseCommandBack();
            Data = this.ExtractCommandData(arr);

            ControlNo = Data[0];
            LineNo = Data[1];

            Check(arr);
        }
    }
}
