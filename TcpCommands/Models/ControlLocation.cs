using System.Linq;
using TcpCommands.Extensions;
using TcpCommands.Models.Base;

namespace TcpCommands.Models
{
    public class ControlLocation : CommandBase
    {
        public ControlLocation(int controlNo, float longitude, float latitude)
        {
            ControlNo = controlNo;

            Longitude = longitude;
            Latitude = latitude;

            var lo = Longitude.ToString("000.00");
            var los = lo.ToCharArray().Where(x => x != '.').Select(x => int.Parse(x.ToString())).ToArray();
            var la = Latitude.ToString("000.00");
            var las = la.ToCharArray().Where(x => x != '.').Select(x => int.Parse(x.ToString())).ToArray();

            Data = new[]
            {
                controlNo, los[0], los[1], los[2], los[3], los[4]
                , las[0], las[1], las[2], las[3], las[4]
            };
        }

        public override string Type => "0C";

        public float Longitude { get; set; }
        public float Latitude { get; set; }
    }

    public class ControlLocationBack : CommandBase, ICommandBack
    {
        public override string Type => "0C";

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
