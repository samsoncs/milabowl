This console app is used to load FPL and Milabowl data into a SQL database
for analytics purposes. For this you need a SQL Server database available to you.
The connection string used is hardcoded in the console app, so make sure that
the connection string matches your database.

The recommended approach is to
spin up a SQL server docker image using the following command:

```
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=!5omeSup3rF4ncyPwd!" -p 1431:1433
-d mcr.microsoft.com/mssql/server:2022-latest
```

Once the SQL Server instance is up and running, simply run the Milabowl.Milalytics
project, and the data should be imported.
