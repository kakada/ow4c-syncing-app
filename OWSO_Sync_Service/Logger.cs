using Sentry;
using System;
using System.Diagnostics;

namespace OWSO_Sync_Service
{
    class Logger
    {
        private static Logger logger;

        private readonly Setting _setting;

        private readonly EventLog _log;

        private Logger(Setting setting)
        {
            _setting = setting;

            _log = new EventLog();
            if (!EventLog.SourceExists("OWSO"))
            {
                EventLog.CreateEventSource("OWSO", "OWSOLog");
            }
            _log.Source = "OWSO";
            _log.Log = "OWSOLog";
        }
        /**
         * This method use for the first time instantiate the object to store the basic properties for Logger
         */
        public static Logger Initialize(Setting config)
        {
            logger = new Logger(config);
            return logger;
        }
        public static Logger getInstance()
        {
            return logger;
        }

        public void log(Object obj, String message)
        {
            String errorMessage = formatMessage(obj, message, "INFORMATION");
            _log.WriteEntry(errorMessage);
        }

        public void logError(Object obj, Exception e)
        {
            logError(obj, e.Message);
        }

        public void logError(Object obj, String message)
        {
            String errorMessage = formatMessage(obj, message, "ERROR");
            _log.WriteEntry(errorMessage);
            SentrySdk.CaptureMessage(errorMessage);
        }

        private String formatMessage(Object obj, String message, String type)
        {
            return String.Format("AREA ID: {0}" +
                "\nBaseUrl: {1}" +
                "\nDatabaseSyncAPI: {2}" +
                "\nHealthCheckAPI: {3}" +
                "\nInterval: {4}" +
                "\nDatabaseUrl: {5}" +
                "\nQuery: {6}" +
                "\nWeb Token: {7}" +
                "\nSentry DSN: {8}" +
                "\ntype: {9}" +
                "\nClass: {10}" +
                "\nMessage: {11}",
                _setting.areaCode,
                _setting.baseUrl,
                _setting.databaseSyncAPI,
                _setting.healthStatusAPI,
                _setting.syncInterval,
                _setting.databaseUrl,
                _setting.query,
                _setting.webToken,
                _setting.sentryDSN,
                type,
                obj.GetType().Name,
                message);
        }
    }
}
