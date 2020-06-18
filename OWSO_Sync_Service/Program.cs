using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace OWSO_Sync_Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            using (SentrySdk.Init("https://c1ec46e28c844aca90752d754fc7a975@o408810.ingest.sentry.io/5280159"))
            {
                // App code
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new OWSOService()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
