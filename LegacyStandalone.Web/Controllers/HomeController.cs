using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web.Mvc;
using NLog;
using TcpCommands.Exceptions;
using TcpCommands.Extensions;
using TcpCommands.Models;

namespace LegacyStandalone.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public async Task<ActionResult> Index()
        {
            await Send();


            //////////////////////////////////////
            return Redirect("/Help");
        }

        private async Task Send()
        {
            Send:
            try
            {
                var command = new TempTimeControlQuery(0);
                var c = command.Command;
                var backCommand = await command.SendAsync<TempTimeControlQueryBack>("169.254.153.214", 8951);
                var b = backCommand.Command;
            }
            catch (TcpCommandReadTimeoutException e)
            {
                var command = new ControlOnlineQuery(0);
                var backCommand = await command.SendAsync<ControlOnlineQueryBack>("169.254.153.214", 8951);
                goto Send;
            }
            catch (CheckBitNotMatchException e)
            {
                Logger.Error(e);
                //return InternalServerError(e);
            }
            catch (ArgumentNullException e)
            {
                Logger.Error(e);
                //throw;
                //return InternalServerError(e);
            }
            catch (SocketException e)
            {
                Logger.Error(e);
                //return InternalServerError(e);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                //return InternalServerError(e);
            }
        }
    }
}
