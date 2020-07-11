using System;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Timers;

namespace OWSO_Sync_Service
{
    public partial class OWSOService : ServiceBase
    {
        private readonly APICommand apiCommand;
        private readonly Database _database;

        private readonly LastSyncUpdateStorage lastupdateStorage;

        private readonly Logger _logger;
        private Timer timer;

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(System.IntPtr handle, ref ServiceStatus serviceStatus);

        public OWSOService(Setting setting)
        {
            InitializeComponent();
            _logger = Logger.Initialize(setting);

            _database = new Database(setting);
            apiCommand = new APICommand(setting);
            lastupdateStorage = new LastSyncUpdateStorage();

            timer = new Timer(setting.syncInterval * 60000) { AutoReset = true };
            timer.Elapsed += OnTimer;
        }


        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            // Set up a timer that triggers every minute.
            timer.Start();

            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            int newTimestamp = _database.readMaxTimestamp();
            int lastupdated = lastupdateStorage.getLastUpdateSync();
            Tuple<STATUS, String> result = _database.readUpdatedDataInJson(lastupdated);

            apiCommand.SubmitData(result.Item1, result.Item2, newTimestamp);
        }

        protected override void OnStop()
        {
            _logger.log(this, "Service stop");
            timer.Stop();

            // Update the service state to Stop Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            // Update the service state to Stopped.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }
    }

    public enum ServiceState
    {
        SERVICE_STOPPED = 0x00000001,
        SERVICE_START_PENDING = 0x00000002,
        SERVICE_STOP_PENDING = 0x00000003,
        SERVICE_RUNNING = 0x00000004,
        SERVICE_CONTINUE_PENDING = 0x00000005,
        SERVICE_PAUSE_PENDING = 0x00000006,
        SERVICE_PAUSED = 0x00000007,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
        public int dwServiceType;
        public ServiceState dwCurrentState;
        public int dwControlsAccepted;
        public int dwWin32ExitCode;
        public int dwServiceSpecificExitCode;
        public int dwCheckPoint;
        public int dwWaitHint;
    };
}
