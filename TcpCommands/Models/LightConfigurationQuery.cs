using TcpCommands.Extensions;
using TcpCommands.Models.Base;

namespace TcpCommands.Models
{
    public class LightConfigurationQuery : CommandBase
    {
        public LightConfigurationQuery(int controlNo)
        {
            ControlNo = controlNo;
            Data = new[] { controlNo };
        }

        public override string Type => "A8";

    }

    public class LightConfigurationQueryBack : CommandBase, ICommandBack
    {
        public override string Type => "A8";
        
        public string Response { get; set; }

        public void Resolve()
        {
            var arr = this.ParseCommandBack();
            Data = this.ExtractCommandData(arr);

            ControlNo = Data[0];

            Illumination1 = (Data[2] << 8) | Data[1];
            Illumination2 = (Data[4] << 8) | Data[3];
            Illumination3 = (Data[6] << 8) | Data[5];
            Illumination4 = (Data[8] << 8) | Data[7];
            Illumination5 = (Data[10] << 8) | Data[9];
            Brightness1 = (Data[12] << 8) | Data[11];
            Brightness2 = (Data[14] << 8) | Data[13];
            Brightness3 = (Data[16] << 8) | Data[15];
            Brightness4 = (Data[18] << 8) | Data[17];
            Brightness5 = (Data[20] << 8) | Data[19];
            
            Check(arr);
        }

        public int Illumination1 { get; set; }
        public int Illumination2 { get; set; }
        public int Illumination3 { get; set; }
        public int Illumination4 { get; set; }
        public int Illumination5 { get; set; }

        public int Brightness1 { get; set; }
        public int Brightness2 { get; set; }
        public int Brightness3 { get; set; }
        public int Brightness4 { get; set; }
        public int Brightness5 { get; set; }
    }
}
