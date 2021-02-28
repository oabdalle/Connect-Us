# Connect-Us



Useful Commands: 
### To compile SASS after running containers
    $ docker start connect-us_node_1

### To stop containers
     $ docker-compose stop

### To remove containers
     $ docker-compose down



To run on a development environment:
### On First Start 
    $ docker-compose build
    
### After First Start 
    $ docker-compose up -d

### Dealing with migrations 
###### To add a migration (note - replace MyTest with migration name)
    $ docker stop connect-us_app_1 && docker run -v ${PWD}:/app --rm --entrypoint /bin/sh connect-us_app -c 'dotnet ef migrations add MyTest' && docker start connect-us_app_1
###### To apply the migration to database:
    $ docker-compose stop && docker-compose up -d



To run on staging environment:
### On First Start 
    $ docker-compose -f docker-compose.staging.yml build
    
### After First Start 
    $ docker-compose -f docker-compose.staging.yml up -d

### Compiling SASS, Adding Migration, Stopping and Removing containers is the same
###### To apply the migration to database:
    $ docker-compose stop && docker-compose -f docker-compose.staging.yml up -d

