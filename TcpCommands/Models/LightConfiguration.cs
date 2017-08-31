using TcpCommands.Extensions;
using TcpCommands.Models.Base;

namespace TcpCommands.Models
{
    public class LightConfiguration : CommandBase
    {
        public LightConfiguration(int controlNo, int illumination1, int illumination2, int illumination3, int illumination4, int illumination5, int brightness1, int brightness2, int brightness3, int brightness4, int brightness5)
        {
            ControlNo = controlNo;
            Illumination1 = illumination1;
            Illumination2 = illumination2;
            Illumination3 = illumination3;
            Illumination4 = illumination4;
            Illumination5 = illumination5;
            Brightness1 = brightness1;
            Brightness2 = brightness2;
            Brightness3 = brightness3;
            Brightness4 = brightness4;
            Brightness5 = brightness5;

            byte i10 = (byte)Illumination1, i11 = (byte)(Illumination1 >> 8);
            byte i20 = (byte)Illumination2, i21 = (byte)(Illumination2 >> 8);
            byte i30 = (byte)Illumination3, i31 = (byte)(Illumination3 >> 8);
            byte i40 = (byte)Illumination4, i41 = (byte)(Illumination4 >> 8);
            byte i50 = (byte)Illumination5, i51 = (byte)(Illumination5 >> 8);
            
            byte b10 = (byte)Brightness1, b11 = (byte)(Brightness1 >> 8);
            byte b20 = (byte)Brightness2, b21 = (byte)(Brightness2 >> 8);
            byte b30 = (byte)Brightness3, b31 = (byte)(Brightness3 >> 8);
            byte b40 = (byte)Brightness4, b41 = (byte)(Brightness4 >> 8);
            byte b50 = (byte)Brightness5, b51 = (byte)(Brightness5 >> 8);

            Data = new[] { controlNo, i10, i11, i20, i21, i30, i31, i40, i41, i50, i51, b10, b11, b20, b21, b30, b31, b40, b41, b50, b51 };
        }

        public override string Type => "08";

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

    public class LightConfigurationBack : CommandBase, ICommandBack
    {
        public override string Type => "08";

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
