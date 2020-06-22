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

        private const String KEY_AREA_CODE = "area_code";
        private const String KEY_DURATION = "interval";

        private const String KEY_DB_URL = "database_url";
        private const String KEY_QUERY = "query";
        private const String KEY_WEB_TOKEN = "access_token";

        private const String KEY_SENTRY_DSN = "sentry_dsn";

        // Configuration Properties
        public String baseUrl { get; }
        public String databaseSyncAPI { get; }
        public String healthStatusAPI { get; }

        public String areaCode { get; }
        public int syncInterval { get; } // in minutes

        public String databaseUrl { get; }
        public String query { get; }
        public String webToken { get; }

        public String sentryDSN { get; }

        public Setting()
        {
            NameValueCollection settings = ConfigurationManager.AppSettings;
            baseUrl = settings.Get(KEY_BASE_URL);
            databaseSyncAPI = settings.Get(KEY_DATABASE_SYNC_API);
            healthStatusAPI = settings.Get(KEY_HEALTH_STATUS_UPDATE_API);

            areaCode = settings.Get(KEY_AREA_CODE);
            String durationText = settings.Get(KEY_DURATION);
            syncInterval = Int32.Parse(durationText);

            databaseUrl = settings.Get(KEY_DB_URL);
            query = settings.Get(KEY_QUERY);

            webToken = settings.Get(KEY_WEB_TOKEN);
            sentryDSN = settings.Get(KEY_SENTRY_DSN);
        }

    }
}
