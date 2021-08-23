#Run migrations on database
dotnet ./out/Migrations.dll "$TASK"
dotnet test ./Milabowl.Test/Milabowl.Test.csproj --logger trx --results-directory /var/temp