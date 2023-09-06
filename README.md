# SubredditStats

Backend Web API application to collect and serve statistics about a given subreddit's posts with a console client that continually updates and displays the statistics fetched from the Web API.

## "Installation"

1. Clone repository
1. Set client credentials environment variables:
    1. `REDDIT_API_CLIENT_ID`
    1. `REDDIT_API_CLIENT_SECRET`
1. Open Visual Studio solutions:
    1. `\Backend\Backend.sln`
    1. `\Frontend\Frontend.sln`
1. Build Solutions
    1. Use `Release`` configuration for both
    1. Use `http` launch configuration for Backend
1. Specify the subreddit you would like to collect stats on:
    1. Edit the `SubredditName` value in the `appsettings.json` file in the Backend project. (`\Backend\WebApi\appsettings.json`).

>Remember to set the environment variables **before** you run the command prompt or open Visual Studio. If they are already open, you **must close and restart them** for them to be able to see the changes to your envionment variables!

### Backend

.NET Core 7 ASP.NET Core Web API application.

Build and run this first. It will start the Web API server listening at <http://localhost:5159>. Once it starts, it begins collecting statistics for the subreddit specified. Select the `Release` configuration and run the `WebApi` project.

>You may need to set the Web Api project as the startup project before the first time you run it.

If you have not set the client credentials environment variables, you will get an exception message in the console output.

#### Swagger & ReDoc API Docs

Once the Web Api project starts, you should see, or can navigate to the following addresses to see the Swagger API UI and ReDoc API documentation:

* Swagger: <http://localhost:5159/swagger>
* ReDoc: <http://localhost:5159/api-docs>

### Frontend

.NET Core 7 Console client application that fetches the statistics from the Web API and displays them in the console, updating continuously. Run this after the Backend WebApi project has started. Select the `Release` configuration and then build and run the `ConsoleClient` project.

>It will verify the Web Api server connection when it starts up. If that fails, it will print an error exception message. In this case, ensure that the Web Api is running, and listening for requests at the Url specificed in the first line of the console client output.

## Running (Recap)

Once environment variables are set and both solutions are open:

1. Run the Backend solution Web Api project first.
    1. The Swagger page will open
1. Then run the Frontend solution ConsoleClient project.
    1. The console client will begin displaying statistics.
