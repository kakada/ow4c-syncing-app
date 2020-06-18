using Sentry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWSO_Sync_Service
{
    class Logger
    {
        private static Logger logger;

        private String _baseUrl;
        private String _databaseSyncAPI;
        private String _healthStatusAPI;

        private String _areaCode;
        private int _syncInterval; // in minutes

        private String _databaseUrl;
        private String _query;

        private readonly EventLog _log;

        private Logger(String baseUrl, String databaseSyncAPI, String healthStatusAPI, String areaCode, int syncInterval, String databaseUrl, String query)
        {
            _baseUrl = baseUrl;
            _databaseSyncAPI = databaseSyncAPI;
            _healthStatusAPI = healthStatusAPI;
            _areaCode = areaCode;
            _syncInterval = syncInterval;
            _databaseUrl = databaseUrl;
            _query = query;

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
        public static Logger Initialize(String baseUrl, String databaseSyncAPI, String healthStatusAPI, String areaCode, int syncInterval, String databaseUrl, String query)
        {
            logger = new Logger(baseUrl, databaseSyncAPI, healthStatusAPI, areaCode, syncInterval, databaseUrl, query);
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
            return String.Format("AREA ID: {0}\nBaseUrl: {1}\nDatabaseSyncAPI: {2}\nHealthCheckAPI: {3}\nInterval: {4}\nDatabaseUrl: {5}\nQuery: {6}\ntype: {7}\nClass: {8}\nMessage: {9}",
                _areaCode, _baseUrl, _databaseSyncAPI, _healthStatusAPI, _syncInterval, _databaseUrl, _query, type, obj.GetType().Name, message);
        }
    }
}
