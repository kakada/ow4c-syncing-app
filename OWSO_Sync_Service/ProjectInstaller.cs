using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

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
