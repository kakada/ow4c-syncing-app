using Sentry;
using System.ServiceProcess;

namespace OWSO_Sync_Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            Setting setting = new Setting();
            using (SentrySdk.Init(setting.sentryDSN))
            {
                // App code
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new OWSOService(setting)
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
