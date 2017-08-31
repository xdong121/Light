using System.Linq;
using TcpCommands.Extensions;
using TcpCommands.Models.Base;

namespace TcpCommands.Models
{
    public class ControlLocationQuery : CommandBase
    {
        public ControlLocationQuery(int controlNo)
        {
            ControlNo = controlNo;

            Data = new[] { controlNo };
        }

        public override string Type => "AC";

    }

    public class ControlLocationQueryBack : CommandBase, ICommandBack
    {
        public override string Type => "AC";

        public string Response { get; set; }

        public void Resolve()
        {
            var arr = this.ParseCommandBack();
            Data = this.ExtractCommandData(arr);

            ControlNo = Data[0];
            var lo = $"{Data[1]}{Data[2]}{Data[3]}.{Data[4]}{Data[5]}";
            Longitude = float.Parse(lo);
            var la = $"{Data[6]}{Data[7]}{Data[8]}.{Data[9]}{Data[10]}";
            Latitude = float.Parse(la);

            Check(arr);
        }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
    }

}
