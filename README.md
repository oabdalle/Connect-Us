# Connect-Us

To run on a development environment:
### On First Start 
    $ docker-compose -f docker-compose.debug.yml up -d
    
### After containers have been created, apply migrations
    $ docker run --rm --entrypoint /bin/bash connect-us_app -c 'dotnet ef database update`

### To stop containers
     $ docker-compose stop

### Dealing with migrations 
###### To add a migration (note - left and right of && stop and start app container if running)
    $ docker stop connect-us_app_1 && docker run -v ${PWD}:/app --rm --entrypoint /bin/bash connect-us_app -c 'dotnet ef migrations add MyTest' && docker start connect-us_app_1
###### To apply the migration to database (note - ensure db container has started):
    $ docker run --rm --entrypoint /bin/bash connect-us_app -c 'dotnet ef database update'

#### To compile SASS after running containers:
    $ docker start connect-us_node_1