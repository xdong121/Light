using TcpCommands.Extensions;
using TcpCommands.Models.Base;

namespace TcpCommands.Models
{
    public class ControlOnlineQuery : CommandBase
    {
        public ControlOnlineQuery(int controlNo)
        {
            ControlNo = controlNo;

            Data = new[] { controlNo };
        }

        public override string Type => "0A";

    }

    public class ControlOnlineQueryBack : CommandBase, ICommandBack
    {
        public override string Type => "0A";

        public string Response { get; set; }

        public void Resolve()
        {
            var arr = this.ParseCommandBack();
            Data = this.ExtractCommandData(arr);

            ControlNo = Data[0];

            Check(arr);
        }
        
    }
}
