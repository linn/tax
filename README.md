# Tax (VAT) Service

## Solution summary
Application able to view outstanding VAT obligations, generate figures and submit quarterly VAT returns via the HMRC API on behalf of users with a Goverment Gateway login.

## Component technologies
* The backend service is dotnet core C# using NancyFx web framework.
* The GUI client is built with React/Redux and managed with npm and webpack.
* Persistence is to Oracle database via EF Core.
* Continuous deployment via Docker container to AWS ECS using Travis CI.

## Local running and Testing
### C# service
* Restore nuget packages.  
* Run C# tests as preferred. 
* Run or debug the Service.Host project to start the backend. To interact with the HMRC API you'll need a client id and secret which can be obtained from their developer hub. They have a sandbox environment which allows you to interact with test data during development: https://developer.service.hmrc.gov.uk/api-documentation/docs/testing 

### Client
* `npm install` to install npm packages.
* `npm start` to run client locally on port 3000.
* `npm test` to run javascript tests.

