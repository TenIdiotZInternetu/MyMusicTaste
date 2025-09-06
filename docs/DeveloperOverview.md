# MyMusicTaste project overview

MyMusicTaste is a server-side Web Application built with ASP.NET and Blazor front-end network. It uses MongoDB, but it is designed to be database-agnostic through dependency injections.

The architecture was inspired by MVC, where the Blazor's razor pages are the views, their codebehinds are the controllers, and models are very simple objects that contain nothing but data. Controllers use injected database operations to retrieve models, and show the data in the view.

## File structure

The relevant source code is divided into these root directiories:

### Components
All blazor pages and components are present here. At the root of the directory are pregenerated `App.razor` and `Routes.razor` for HTML header definition and routing. Other pregenerated files are in `Layout`. The `Page-*` contain components only required by the specific page. `Comments`, `Dialogs` and `Misc` directories contain components used throughout the whole app.

Vast majority of components are comprised of a `.razor` file containing markup of the page with no logic, and its complementary `.razor.cs` codebehind, which includes initialization, loading dependencies, event handling and user interaction.

At times a component uses a `.razor.css` file, that defines styles scoped for that specific component. By default styles are defined in `/wwwroot/app.css` and with Bootstrap.

Some pages, like authentication pages cannot fully work with Blazor alone, so they use standard MVC controllers from ASP.NET with request handling.

### Models
Here are all the models used by the app. They are simple data-only objects either directly retrieved from the database or computed from the database data. They all inherit from the `Model` class which holds the unique id of any model.

### Database
Here are defined database-agnostic interfaces for operating with database. The database-specific implementations are all contained in the `Contexts` subdirectory. 

Right now `MongoDb` is the only database context. It's setup is contained in the `Contexts/MongoDb/MongoDbContext`.

In the root of the directory is `IDbRepository` and `IIdentityProvider`.

`IDbRepository` provides generic interface for basic CRUD operations (Create, Read, Update, Delete) on any model stored directly in the database. They can be retrieved by their Id and created/update/deleted by providing a model with the desired data. For MongoDb, the valid models are the ones that are mapped to MongoCollections in `MongoCollectionFactory`.

`IIdentityProvider` is responsible for handling sensitive user data in the database when signing up/logging. It is also responsible for authenticating/authorizing users, when page behavior changes based on who visits it. `AspNetCore.Identity.MongoDbCore` nuget package was used for this purpose.

Other more specific interfaces, such as search and stats calculation are in the `Operations` subdirectory.

### Startup
This directory is to keep `Program.cs` from getting bloated, and such methods from here should only be run before the actual application. They are mostly middleware, services, dependency injections and identity setups.

### Utils
Here are helper functions that didn't fit into any other 