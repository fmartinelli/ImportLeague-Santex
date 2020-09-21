## Task
# Import League

> The goal is to make a project that exposes an API with an HTTP GET in this URI:
> /import-league/{leagueCode} . E.g., it must be possible to invoke the service using this URL:
> http://localhost:<port>/import-league/CL.
The service implementation must get data using the given {leagueCode}, by making requests to the http://www.football-data.org/API (you can see the documentation entering to the site, use the API v2),  and import the data into a DB (MS SQL Server is suggested, but you can use any DB of your preference). 

> The data requested is:
> Competition ("name", "code", "areaName")
> Team ("name", "tla", "shortName", "areaName", "email")
> Player("name", "position", "dateOfBirth", "countryOfBirth", "nationality")

> Feel free to add to this data structure any other field that you might need (for the foreign keys relationship).

 
#  Total Players

> Additionally, expose an HTTP GET in URI /total-players/{leagueCode}  , with a simple JSON response like this:
{"total" : N } and HTTP Code 200.

>where N is the total amount of players belonging to all teams that participate in the given league (leagueCode). This service must rely exclusively on the data saved inside the DB (it must not access the API football-data.org). If the given leagueCode is not present into the DB, it should respond an HTTP Code 404.

# Remarks
 

> You are allowed to use any library related to the language in which you are implementing the project.
You must provide the SQL for data structure creation; it is a plus that the project automatically creates the structure (if it doesn't exist) when it runs the first time.
All the mentioned DB entities must keep their proper relationships (the players with which team they belong to; the teams in which leagues participate).

The API responses for /import-league/{leagueCode} are:
| Code | Response | x |
| ------ | ------ | ------ |
| HttpCode 201 | {"message": "Successfully imported"} | When the leagueCode was successfully imported.|
| HttpCode 409 | {"message": "League already imported"} | If the given leagueCode was already imported into the DB (and in this case, it doesn't need to be imported again). |
| HttpCode 404 | {"message": "Not found" } | if the leagueCode was not found. |
| HttpCode 504 | {"message": "Server Error" } | If there is any connectivity issue either with the football API or the DB server.|

> It might happen that when a given leagueCode is being imported, the league has participant teams that are already imported (because each team might belong to one or more leagues). For these cases, it must add the relationship between the league and the team(s) (and omit the process of the preexistent teams and their players).
 
# Solution

* Net Core 3.1
* Entity Framework Core.
* Migrations
* Dependency Injection
* Async Calls
* Swagger Documentation
* Unit Tests (xUnit) using Moq
* A mapper library (Automapper)
* Patterns:
    * Unit of Work
    * Repository
* Easy project setup
    * Docker Compose startup project
    * SLQ Server on Docker Container
    * Database auto-creation

### Todos

 - Write MORE Tests
 - Increase Connection Resiliency on football-data api using Polly
 - Add Apikeys to Swagger
 - Database Project (Migrations are difficult to merge along big teams. DACPAC files lets the developers merge the sql files using normal GIT tools and deploy using DACPAC generated file.)

### Build & Run

Before build: replace the word "token" at the environment variable assignment line ("FootballServiceApiKey=token") with your football-data api token. You will find this line at "\<repository>\ImportLeague\docker-compose.override.yml".

On Visual Studio with Docker Desktop installed:
 - Setup Docker Compose as default startup project
 - Run
     
> Swagger: http://localhost:5105/swagger
