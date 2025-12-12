### Database Migrations

* `--project Infrastructure` (optional if in root folder)
* `--startup-project WebApi`
* `--output-dir Migrations`

For example, to add a new migration from the root folder:
 
dotnet ef migrations add "SampleMigration" --project src\Common\Infrastructure --startup-project src\Apps\WebApi --output-dir Migrations

