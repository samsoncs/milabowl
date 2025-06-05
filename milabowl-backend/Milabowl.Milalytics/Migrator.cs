using System.Reflection;
using DbUp;

namespace Milabowl.Milalytics;

public static class Migrator
{
    public static bool Migrate()
    {
        EnsureDatabase.For.SqlDatabase(DbConnection.CONNECTION_STRING);

        var upgrader =
            DeployChanges.To
                .SqlDatabase(DbConnection.CONNECTION_STRING)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

        var result = upgrader.PerformUpgrade();
        return result.Successful;
    }
}
