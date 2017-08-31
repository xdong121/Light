namespace TcpCommands.Models.Base
{
    public interface ILight
    {
        int ControlNo { get; set; }
        int LineNo { get; set; }
        int SubNo { get; set; }
        int GroupNo { get; set; }
    }
}