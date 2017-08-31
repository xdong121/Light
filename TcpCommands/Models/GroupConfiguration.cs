using System.Globalization;
using TcpCommands.Exceptions;
using TcpCommands.Extensions;
using TcpCommands.Models.Base;

namespace TcpCommands.Models
{
    public class GroupConfiguration : CommandBase
    {
        public GroupConfiguration(int controlNo, int lineNo, int subNo, int groupNo)
        {
            ControlNo = controlNo;
            LineNo = lineNo;
            SubNo = subNo;
            GroupNo = groupNo;
            Data = new[] { ControlNo, LineNo, SubNo, GroupNo };
        }

        public override string Type => "01";
    }

    public class GroupConfigurationBack : CommandBase, ICommandBack
    {
        public override string Type => "01";

        public int SuccessfulFlag { get; set; } //0x00 成功，0xFF 失败

        public string Response { get; set; }

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
