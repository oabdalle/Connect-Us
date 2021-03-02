# Connect-Us



Useful Commands: 
### To compile SASS after running containers
    $ docker start connect-us_node_1

### To stop containers
     $ docker-compose stop

### To remove containers
     $ docker-compose down

### To remove Database to start from scratch
     $ docker rm connect-us_db_1 -f && docker volume rm connect-us_mysqldata -f



To run on a development environment:
### On First Start 
    $ docker-compose build
    
### After First Start
    $ docker-compose up 

### To add a Database Migration (note - replace MyTest with useful migration name)
    $ docker-compose stop && docker run -v ${PWD}:/app --rm --entrypoint /bin/sh connect-us_app -c 'dotnet ef migrations add MyTest' && docker-compose up



To run on staging environment:
### On First Start 
    $ docker-compose -f docker-compose.staging.yml build
    
### After First Start 
    $ docker-compose -f docker-compose.staging.yml up 

### To add a Database Migration
    $ docker-compose stop && docker run -v ${PWD}:/app --rm --entrypoint /bin/sh connect-us_app -c 'dotnet ef migrations add MyTest' && docker-compose -f docker-compose.staging.yml up

