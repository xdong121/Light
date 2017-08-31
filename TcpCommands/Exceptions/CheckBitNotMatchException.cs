using System;

namespace TcpCommands.Exceptions
{
    public class CheckBitNotMatchException : Exception
    {
        public CheckBitNotMatchException(string message = "校验位不符")
            : base(message)
        {
        }

        public CheckBitNotMatchException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
