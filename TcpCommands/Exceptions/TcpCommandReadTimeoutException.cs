using System;

namespace TcpCommands.Exceptions
{
    public class TcpCommandReadTimeoutException : Exception
    {
        public TcpCommandReadTimeoutException(string message = "返回命令超时")
            : base(message)
        {
        }

        public TcpCommandReadTimeoutException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
