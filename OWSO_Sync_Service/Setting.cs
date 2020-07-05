using System;
using System.Collections.Specialized;
using System.Configuration;

namespace OWSO_Sync_Service
{
    public class Setting
    {
        private const String KEY_BASE_URL = "base_url";
        private const String KEY_DATABASE_SYNC_API = "database_sync_api";
        private const String KEY_HEALTH_STATUS_UPDATE_API = "health_status_update_api";
        private const String KEY_CONNECTION_TIME_OUT = "connection_time_out";

        private const String KEY_SITE_CODE = "site_code";
        private const String KEY_DURATION = "interval";

        private const String KEY_DB_URL = "database_url";
        private const String KEY_QUERY = "query";
        private const String KEY_COMPARE_DATE_TIME_FORMAT = "compare_date_time_format";
        private const String KEY_ACCESS_TOKEN = "access_token";

        private const String KEY_SENTRY_DSN = "sentry_dsn";

        // Configuration Properties
        public String baseUrl { get; }
        public String databaseSyncAPI { get; }
        public String healthStatusAPI { get; }
        public int connectionTimeout { get; } // in seconds

        public String siteCode { get; }
        public int syncInterval { get; } // in minutes

        public String databaseUrl { get; }
        public String query { get; }

        public String compareDateTimeFormat { get; }
        public String accessToken { get; }

        public String sentryDSN { get; }

        public Setting()
        {
            NameValueCollection settings = ConfigurationManager.AppSettings;
            baseUrl = settings.Get(KEY_BASE_URL);
            databaseSyncAPI = settings.Get(KEY_DATABASE_SYNC_API);
            healthStatusAPI = settings.Get(KEY_HEALTH_STATUS_UPDATE_API);
            connectionTimeout = Int32.Parse(settings.Get(KEY_CONNECTION_TIME_OUT));

            siteCode = settings.Get(KEY_SITE_CODE);
            String durationText = settings.Get(KEY_DURATION);
            syncInterval = Int32.Parse(durationText);

            databaseUrl = settings.Get(KEY_DB_URL);
            query = settings.Get(KEY_QUERY);
            compareDateTimeFormat = settings.Get(KEY_COMPARE_DATE_TIME_FORMAT);

            accessToken = settings.Get(KEY_ACCESS_TOKEN);
            sentryDSN = settings.Get(KEY_SENTRY_DSN);
        }

    }
}
