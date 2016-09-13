AutoClutch Templates

1) Error		This project references NuGet package(s) that are missing on this computer. Enable NuGet Package Restore to download them.  For more information, see http://go.microsoft.com/fwlink/?LinkID=322105. The missing file is R:\dev3\MyTestProjectB\\.nuget\NuGet.targets.	MyTestProjectB.Core	R:\dev3\MyTestProjectB\MyTestProjectB.Core\MyTestProjectB.Core.csproj	181	

run https://github.com/owen2/AutomaticPackageRestoreMigrationScript/blob/master/migrateToAutomaticPackageRestore.ps1

2) Resolve references

3) Add user to database (or make sure user you will use has database rights)

4) Edit the web.config connectionstring by modifying the database name and user the username of the user that has login permission to the database.

3) Package Manager Console -> select [Your Project Name].Data as the default project -> enable-migrations

4) Add-migration Initial

7) update-database

Test

