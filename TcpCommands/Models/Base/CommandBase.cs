using System;
using System.Globalization;
using System.Linq;
using TcpCommands.Exceptions;

namespace TcpCommands.Models.Base
{
    public abstract class CommandBase : ILight
    {
        public string Head => "7E";
        public abstract string Type { get; }
        public int Length => Data.Length + 2;
        public int CheckBit => (int.Parse(Type, NumberStyles.HexNumber) + Length + Data.Sum()) % 256;
        public string Tail => "7E";

        public int[] Data { get; set; }
        public string HexData => string.Join(" ", Data.Select(x => x.ToString("X")).ToArray());

        public string Command => $"{Head} {Type} {Length:X} {HexData} {CheckBit:X} {Tail}";

        public int ControlNo { get; set; }
        public int LineNo { get; set; }
        public int SubNo { get; set; }
        public int GroupNo { get; set; }

        protected void Check(string[] arr)
        {
            var backCheckBit = arr[arr.Length - 2];
            if (CheckBit != int.Parse(backCheckBit, NumberStyles.HexNumber))
            {
                throw new CheckBitNotMatchException();
            }
        }
    }
}
