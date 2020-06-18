using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace OWSO_Sync_Service
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            this.AfterInstall += new InstallEventHandler(ServiceInstaller_AfterInstall);
        }

        void ServiceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            ProjectInstaller serviceInstaller = (ProjectInstaller)sender;

            using (ServiceController sc = new ServiceController(serviceInstaller.OWSOServiceInstaller.ServiceName))
            {
                sc.Start();
            }
        }
    }
}
