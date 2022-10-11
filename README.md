# Task 5. React.js application

As a result of task 5, the functionality of events and the user profile was transferred to react js.   
The react application uses the api developed in task 4. Moved the functionality of creating, updating, deleting events. The user profile and work with it has been transferred: editing, replenishing the balance, changing the password.

## Future flag

Future flag is used as switching between applications, appsettings.json  used as a source of feature flag value. To change the client, you need to change the following in appsettings.json, which is located in the presentation:  
"FeatureManagement": {  
"PresentationUI": ...    
},    
Instead of ... you need to insert:  
- true to launch presentation ui.  
- false to run react js application.

## Application launch

To launch the application correctly, you need to repeat all the steps from 4 tasks:  
You need to right-click on the solution, then open the properties, then select: several startup projects, mark the launch for: TicketManagement.VenueAPI, TicketManagement.EventAPI, TicketManagement.UserAPI, TicketManagement.TicketAPI, TicketManagement.Presentation
- ### Just so that presentation was the last project to be launched!  
To change the database, you need to change the value in appsettings.json in each project for  
"ConnectionStrings": {  
"DefaultConnection": "……[your connection string]…."  
}  
- #### To run the React.js application, you need to go to the application folder (...training\IrynaKhadanovich \src\ticket-management-react-ui)  on the command line using the cd command. Next, call the “npm install react-router-dom” command. Next, call “npm start” command


## Microservice architecture

The ticket management application created in Task #2 has been refactored. Solution architecture updated. The following APIs have been created:
VenueAPI is responsible for stealing operations and working with all entities associated with venue: venue, areas, sets, layouts.
UserAPI is the authentication endpoint responsible for custom CRUD operations. Use JWT as your approach to authentication and authorization. Responsible for user login and registration. The user is added branding for authorization in the system with a specific role.
TicketAPI is responsible for the history of purchases and the user's work with his personal account.
The EventAPI is responsible for all the entities related to and managing events, as well as the third party event provider.
Added documentation for each API(Swagger). All components are logged. Logging created with Serilog.

## Presentation layer changes

Removed identity from presentation layer. Removed all references to BLL and DAL.Clients are created on the presentation layer. The rest easy library is used. All units, integration tests run and passed. 

## Event provider functionality.  

During the execution of the task, an event provider was created. New project added into decisions. Added CRUD operations for ThirdPartyEvent entity. Autofac was used as a DI container in Global.asax. As data source used Json file, which has the following location: App_Data/EventsJson/ThirdPartyEvent.json. The pictures used in the application are located in the following path: App_Data/Images/. Images are converted to base64. A controller and views have been created that make it possible to perform CRUD operations. And also create files for import. Import files are located in: App_Data/EventsToImport/. Created a project for testing the logic of services with unit tests. Moq were used for testing. Data logging and caught exceptions can be found in the following path: App_Data/LoggerData/logger.txt.

## Some value information about files path.

Pictures must be loaded from App Data/Images. If you want to select a new image, add it to App Data/Images
All folder paths are set using Web.config file in appSettings section:  
key="MyDatabase" value="/App_Data/EventsJson/ThirdPartyEvent.json" => (data source(json file))  
key="MyImport" value="/App_Data/EventsToImport/" => (path to store imported files)  
key="ImgPath" value="/App_Data/Images/" => (path to store images)  
key="FileFormat" value=".json" => (format for saving imported files)  
key="LoggerFile" value="/App_Data/LoggerData/logger.txt" => (file for logging data and exceptions)

## Interaction with Ticket Management.

To import files into the main application, an ImportController controller and views were created, which also localized. To download new events, you need to log in as an event manager. When registering in the application, the user assumes the user role, so in order to check the functionality, you need to log into the application as an administrator and assign the user the role of eventManager.  
Login: admin@mail.ru  
Password: admin  
Imported files are checked for compliance, pictures are converted to format ‘.png’. When all file exceptions occur the user receives a warning message that not all files have been added or not all have been added. In the event that any files are added, but some are not, the same thing happens. For example, in case of intersection of dates or the wrong venue. Data is stored in wwwroot. Folder names are written in appsettings.json  
"ImgFolder": "images" => (folder to store images)  
"FileFolder": "importFiles" => (folder to store import files)

## Database connection.

Integration and unit tests have been created. Unit tests test the BLL layer and the integration layer DAL. Examined the process of creating tests using Moq and integration tests, as well as friendly builds. The functionality of the application has been tested. The test database is deployed. To work with the application and test the tests, you need to deploy the database script and specify database name and server in the appsettings.json files in the test project and presentation. To run the tests, you need to deploy the database, then in the appsettings.json file in the project TicketManagement.IntegrationTests, you must specify a connection string in the following form:
  
  "ConnectionStrings": {  
    "TestDatabase": "connection string should be here"  
  }  

In presentation layer:  

"ConnectionStrings": {  
 "DefaultConnection": "Data Source=[...server name...];Initial Catalog=[...database name...];Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;MultipleActiveResultSets=true"  
 },  

## Application functionality.

The application has different roles: admin, user, eventManager and venueManager. Created an initializer to create initial roles and the first user (administrator). Authentication and authorization added with Identity. Localization in three languages has been added to the application: Russian, Belarusian and English. Depending on the time zone chosen by the user during registration, the time of events on the main page and the time of ticket purchase change. The user has the right to refuse the ticket if the event has not yet passed and return money to his balance.

## Application use.

For authorization in the application, you must register. Registered user receives the role of the "user" and has the ability to access the personal account and purchase a ticket. To assign a user the role of a manager, you must log in under the role of admin.  
  
  Login: admin@mail.ru   
  Password: admin  
    
After that, changing user roles will be available. The event manager has the right to change, add and delete events. Venue manager has the right to edit, add and delete venues, layouts, areas, user management. An anonymous user has no options and can only view events.
