# Connect-Us

To run on a development environment:
### On First Start 
    $ docker-compose -f docker-compose.debug.yml build
    
### After First Start
    $ docker-compose -f docker-compose.debug.yml up -d

### To stop containers
     $ docker-compose stop

### To remove containers
     $ docker-compose down

### To compile SASS after running containers
    $ docker start connect-us_node_1

### Dealing with migrations 
###### To add a migration (note - replace MyTest with migration name)
    $ docker stop connect-us_app_1 && docker run -v ${PWD}:/app --rm --entrypoint /bin/sh connect-us_app -c 'dotnet ef migrations add MyTest' && docker start connect-us_app_1
###### To apply the migration to database:
    $ docker-compose stop && docker-compose -f docker-compose.debug.yml up -d

