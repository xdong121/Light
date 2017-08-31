using System;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NLog;
using TcpCommands.Exceptions;
using TcpCommands.Models.Base;

namespace TcpCommands.Extensions
{
    public static class TransmitterExtension
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        //超时时间
        private static int timeout = 1000 * 30;

        public static async Task<T> SendAsync<T>(this CommandBase command, string server, int port)
            where T : CommandBase, ICommandBack, new()
        {
            TcpClient client = null;
            NetworkStream stream = null;
            try
            {
                client = new TcpClient(server, port);
                var data = Encoding.ASCII.GetBytes(command.Command);
                stream = client.GetStream();
                stream.Write(data, 0, data.Length);

                Logger.Info("Sent: {0}", command.Command);

                data = new byte[256];

                

                var bytes = 0;
                var receiveTask = Task.Run(async () => { bytes = await stream.ReadAsync(data, 0, data.Length); });
                var isReceived = await Task.WhenAny(receiveTask, Task.Delay(timeout)) == receiveTask;
                if (!isReceived)
                {
                    throw new TcpCommandReadTimeoutException();
                }
                var responseData = Encoding.ASCII.GetString(data, 0, bytes);

                Logger.Info("Received: {0}", responseData);
                var t = new T
                {
                    ControlNo = command.ControlNo,
                    LineNo = command.LineNo,
                    SubNo = command.SubNo,
                    GroupNo = command.GroupNo,
                    Response = responseData
                };
                t.Resolve();
                return t;
            }
            catch (ArgumentNullException e)
            {
                Logger.Error(e);
                throw;
            }
            catch (SocketException e)
            {
                Logger.Error(e);
                throw;
            }
            catch (CheckBitNotMatchException e)
            {
                Logger.Error(e);
                throw;
            }
            catch (TcpCommandReadTimeoutException e)
            {
                Logger.Error(e);
                throw;
            }
            finally
            {
                stream?.Close();
                client?.Close();
            }
        }
        
        public static string[] ParseCommandBack<T>(this T commandBack)
            where T : CommandBase, ICommandBack, new()
        {
            return commandBack.Response.Split(' ').Where(x => !string.IsNullOrEmpty(x) && x.Length > 0).ToArray();
        }

        public static int[] ExtractCommandData<T>(this T commandBack, string[] command)
        {
            return command.Skip(3).Take(command.Length - 5).Select(x => int.Parse(x, NumberStyles.HexNumber)).ToArray(); ;
        }

    }
}
