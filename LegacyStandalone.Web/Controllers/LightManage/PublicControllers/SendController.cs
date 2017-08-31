using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Timers;
using NLog;
using NLog.Fluent;

namespace LegacyStandalone.Web.Controllers.LightManage.PublicControllers

{

    public class SendController : System.Web.HttpApplication
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public static readonly string SendServer = "169.254.153.214";
        public static readonly int SendPort = 8951;

        protected void Application_Start(object sender, EventArgs e)

        {

            //定义定时器  

            System.Timers.Timer myTimer = new System.Timers.Timer(5000);

            myTimer.Elapsed += new ElapsedEventHandler(myTimer_Elapsed);

            myTimer.Enabled = true;

            myTimer.AutoReset = true;

        }

        void myTimer_Elapsed(object source, ElapsedEventArgs e)

        {

            try

            {

                Logger.Info(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":AutoTask is Working!");

                YourTask();

            }

            catch (Exception ee)

            {

               // Log.SaveException(ee);

            }

        }

        void YourTask()

        {

            //在这里写你需要执行的任务  

        }

        protected void Application_End(object sender, EventArgs e)

        {

            //Log.SaveNote(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":Application End!");

            //下面的代码是关键，可解决IIS应用程序池自动回收的问题  

            Thread.Sleep(1000);

            //这里设置你的web地址，可以随便指向你的任意一个aspx页面甚至不存在的页面，目的是要激发Application_Start  

            string url = "http://www.shaoqun.com";  
      
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

            Stream receiveStream = myHttpWebResponse.GetResponseStream();//得到回写的字节流  

        }

    }

}