
# OWSO Sync Service
OWSO Sync Service is an service application which running periodically to sync the local MS. SQL database to web service.

## Configuration parameters
Edit OWSO_Sync_Service.exe.config as administrator role, and configure the following parameters for your service and database connection.

        <add key="base_url" value="" />
        <add key="database_sync_api" value="" />
        <add key="health_status_update_api" value="" />
        <add key="connection_time_out" value="15"/> <!-- unit in seconds -->
    
        <add key="site_code" value="" />
        <add key="interval" value="" />

        <add key="database_url" value="" />
        <add key="query" value="" />
        <add key="query_max_timestamp" value=""/>
		
        <add key="sentry_dsn" value="" />
	<add key="access_token" value="" />
        

- base_url: The web service hosting server address. Ex. http://100.100.100.100
- database_sync_api: the api for syncing the database. Ex. http://100.100.100.100/api/database, so the value should be "/api/database". It will concatenate with base_url.
- health_status_udpate_api: the api for syncing the health status
- connection_time_out： is in seconds, it is used to set the time that waiting for the response. If the response from server is longer than the set time, it will cancel and throw error.
- site_code: the code to identify the area that the application deploy at
- interval: the interval that the service will run in schedule. This interval's unit is minute.
- database_url: The database connection url which access to mssql server
- query: The query that user wants to trigger and sync with web service, it should include the compare date and time that user want to compare in the interval and should be set as parameter {0}
- query_max_timestamp: This is the sql statement to select the max timestamp of the table which depends on the timestamp data type, and is used to compare the sync record.
- sentry_dsn: is the sentry dsn to report the error to
- access_token: access api backend token


## Development

### IDE
- Visual Studio 2019

### Extension:
- Microsoft Visual Studio Installer Project

### Build and Release
- The solution consists of 2 projects, the development project and the installer. To build and release project:
    1. Build the development project
    2. Build the project installer project, then you will be able to find an msi file inside the project installer's bin folder.

### How to Debug?
1. Build the service project in debug mode
2. Then start command prompt with administrative credential and run command
        installutil OWSO_Sync_Service.exe
3. Start Visual Studio with administrative credentials
4. Then go to Tools -> Attach to Process
5. Select the Show processes from all users check box
6. Choose "OWSO Sync Service", then choose Attach

For more detail information about how to debug Windows Service, please refer to https://docs.microsoft.com/en-us/dotnet/framework/windows-services/how-to-debug-windows-service-applications
