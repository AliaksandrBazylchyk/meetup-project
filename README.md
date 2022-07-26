# Meetup project for Modsen

The project demonstrates my skills in developing a microservice application based on CRUD web api using the following technologies:  

 - .NET 6.0
 - Entity Framework Core
 - PostgreSQL
 - AutoMapper
 - Authentication via Bearer (Duende Identity Server)
 - Swagger UI

# Description
The application has 5 requests for working with events and 1 for authorization.  All requests related to events are public, except for deletion.  Removal requires authorization through a login request.

# Deploy
Install Docker Desktop and run it. After that clone project from this repository to any folder. 

    git clone https://github.com/AliaksandrBazylchyk/meetup-project.git

Open folder in terminal and run this command:

    docker-compose up -d --build
 
 That command execute docker-compose.yml file and run all containers automatically. Flag -d stand for detached. This means that once launched, the containers will run in the background. --build flag force to rebuild all solutions from containers.
 ## Database connection
 After all containers starts you can login into PostgreSQL Control panel (PgAdmin) by your web-browser, just go to address:

    http://localhost:5050
Password for access: "root". Now you can add PostgreSQL server to view your databases. For that just right click to Servers category (left part of the screen) -> Register -> Server.   
Into Name field insert whatever you want, go to "connection" and into hostname/address and paswword fields insert "postgres-database" and "root". Click "Save" button. After that you can see 4 databases (3 for project and 1 default). 

 - IdentityDb contain all configurations for Identity Server (migrate and seed after first run Identity Server container)
 - AspIdentityDb containt all information about users, their logins, claims, etc.
 - Events contain only one table with events information

## Swagger 
 After all containers starts you can open Swagger UI panel, just go to address:
 

    http://localhost:8000/swagger/index.html

You can see 5 request for events and 1 for login.

### Event Controller

 - GET with {id} - Getting Event with GUID(id) id what you insert
 - PATCH with {id} - Update info about existed event by GUID(id). Query has a lot of properties for updated. Only GUID property requested and unchangable.
 - DELETE with {id} - Delete event record by GUID(id) (Secured request, need authorization)
 - GET - Getting array with all events from database
 - POST - Create record about new event. Request body. All params requested

### Login Controller

 - POST with Body (login-password pair) - For authorization.
Only one creds exist and seed when application started. For login use:

    {
  "username": "Test",
  "password": "root"
}

	If you try make DELETE request without authorization you will get 401 status code with "Unauthorized" message. If you try make POST login request with another creds you will get "invalid pass or username". If you login with good creds server respone you your access token. Copy him and find on top of page green "Authorize" button. Click it and insert your Access Token into "Value" field. Click "Authorize". If your token valid, not exipred you may delete record by id. If your token invalid you will get error with different message error.
