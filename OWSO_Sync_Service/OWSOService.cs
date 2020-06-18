using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Collections.Specialized;
using System.IO;
using Sentry;

namespace OWSO_Sync_Service
{
    public partial class OWSOService : ServiceBase
    {
        private const String KEY_BASE_URL = "base_url";
        private const String KEY_DATABASE_SYNC_API = "database_sync_api";
        private const String KEY_HEALTH_STATUS_UPDATE_API = "health_status_update_api";

        private const String KEY_AREA_CODE = "area_code";
        private const String KEY_DURATION = "interval";

        private const String KEY_DB_URL = "database_url";
        private const String KEY_QUERY = "query";

        // Configuration Properties
        private String _baseUrl;
        private String _databaseSyncAPI;
        private String _healthStatusAPI;

        private String _areaCode;
        private int _syncInterval; // in minutes

        private String _databaseUrl;
        private String _query;

        private readonly APICommand apiCommand;
        private readonly Database _database;

        private readonly LastSyncUpdateStorage lastupdateStorage;

        private readonly Logger _logger;
        private Timer timer;

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(System.IntPtr handle, ref ServiceStatus serviceStatus);

        public OWSOService()
        {
            InitializeComponent();
            readConfiguration();
            _logger = Logger.Initialize(_baseUrl, _databaseSyncAPI, _healthStatusAPI, _areaCode, _syncInterval, _databaseUrl, _query);

            _database = new Database(_databaseUrl, _query);
            // apiCommand = new APICommand(_baseUrl, _databaseSyncAPI, _healthStatusAPI);
            lastupdateStorage = new LastSyncUpdateStorage();

            timer = new Timer(_syncInterval * 60000) { AutoReset = true };
            timer.Elapsed += OnTimer;
        }

        private void readConfiguration()
        {
            NameValueCollection settings = ConfigurationManager.AppSettings;
            _baseUrl = settings.Get(KEY_BASE_URL);
            _databaseSyncAPI = settings.Get(KEY_DATABASE_SYNC_API);
            _healthStatusAPI = settings.Get(KEY_HEALTH_STATUS_UPDATE_API);

            _areaCode = settings.Get(KEY_AREA_CODE);
            String durationText = settings.Get(KEY_DURATION);
            _syncInterval = Int32.Parse(durationText);

            _databaseUrl = settings.Get(KEY_DB_URL);
            _query = settings.Get(KEY_QUERY);
        }

        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            _logger.log(this, "Start service");

            // Set up a timer that triggers every minute.
            timer.Start();

            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            int currentTime = DateTime.Now.Millisecond;
            _logger.log(this, "Timer is triggered");
            long lastupdated = lastupdateStorage.getLastUpdateSync();
            String json = _database.readUpdatedDataInJson(lastupdated);
            _logger.log(this, "JSON String: " + json);

            _logger.log(this, "Store current time: " + currentTime);
            lastupdateStorage.storeLastUpdateSync(currentTime);
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
