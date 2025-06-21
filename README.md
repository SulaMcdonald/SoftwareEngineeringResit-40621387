# StarterApp app version 4

This version of the StarterApp app uses SQL Server for data storage with Entity Framework Core.

Note that the file appsettings.json is required, but it is not included in the repo because it contains local IP addresses.
Add the appsettings.json to the root folder of the Startapp.Database project. It has the following format:

```
 {
     "ConnectionStrings": {
         "DevelopmentConnection": "Server=<IP Address>;Database=<Database name>;User Id=<Username>;Password=<Password>;TrustServerCertificate=True;Encrypt=True;"
     }
 }
 ```

Replace &lt;IP Address>, &lt;Database name>, &lt;Username> and &lt;Password> with your values.