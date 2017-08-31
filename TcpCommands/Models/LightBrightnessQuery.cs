using TcpCommands.Extensions;
using TcpCommands.Models.Base;

namespace TcpCommands.Models
{
    public class LightBrightnessQuery : CommandBase
    {
        public LightBrightnessQuery(int controlNo, int groupNo)
        {
            ControlNo = controlNo;
            GroupNo = groupNo;

            Data = new[] { controlNo, GroupNo };
        }

        public override string Type => "09";

    }

    public class LightBrightnessQueryBack : CommandBase, ICommandBack
    {
        public override string Type => "09";

        public string Response { get; set; }

        public void Resolve()
        {
            var arr = this.ParseCommandBack();
            Data = this.ExtractCommandData(arr);

            ControlNo = Data[0];
            GroupNo = Data[1];
            Brightness = Data[2];

            Check(arr);
        }

        public int Brightness { get; set; }

    }
}
