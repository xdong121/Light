namespace TcpCommands.Models.Base
{
    public interface ICommandBack
    {
        string Response { get; set; }

        void Resolve();
    }
}