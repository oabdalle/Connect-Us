# Connect-Us

To run on development environment:

- On First Start - 
Run Commands: 
    docker-compose -f docker-compose.debug.yml build && docker-compose -f docker-compose.debug.yml up -d
    docker exec -it connect-us_app_1 dotnet ef database update

To run after building containers completed:
    docker-compose -f docker-compose.debug.yml up -d

To add a migration:
    docker exec -it connect-us_app_1 dotnet ef migrations add MyTestMigration

To apply the migration to database:
    docker exec -it connect-us_app_1 dotnet ef database update