# Create migration

Create migration: `dotnet ef migrations add MIGRATION_NAME`    
Apply migration: `dotnet ef database update MIGRATION_NAME --connection CONNECTION_STRING`    

For example,
```
dotnet ef migrations add WaitForReviewFiledAdd
dotnet ef database update WaitForReviewFiledAdd --connection "Data Source=bin/debug/net7.0/app.db"
```

Please, remember that in some cases you maybe need edit migration code himself.

And a good practics is send `--active_telegram_bot false` in `dotnet ef database update`, like this:
```
dotnet ef database update AddRatingToPosts --connection "Data Source=bin/debug/net7.0/app.db" -- --active_telegram_bot false
```