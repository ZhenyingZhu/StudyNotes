# ASP NET Web API

[What is ASP.NET](https://en.wikipedia.org/wiki/ASP.NET)

- ASP.NET is an open-source server-side web application (a client–server computer program) framework.
- the successor to Microsoft's Active Server Pages (ASP) technology. Successor is ASP.NET Core.
- together with other frameworks like Entity Framework.

ASP.NET offers programming models

- Web Forms: building modular pages out of components, with UI events being processed server-side.
- MVC: Model–view–controller
- Web Pages: adding dynamic code and data access directly inside HTML markup.
- Web API: building RESTful applications on the .NET Framework.
- Webhooks: subscribing to and publishing events via HTTP.
- SignalR: real-time communications framework for bi-directional communication between client and server.

[ASP.NET Core vs ASP.NET](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/choose-aspnet-framework?view=aspnetcore-2.2)

- ASP.NET Core is a redesign of ASP.NET 4.x.

## Resources

- <https://app.pluralsight.com/library/courses/front-end-web-app-html5-javascript-css/table-of-contents>
- <https://app.pluralsight.com/library/courses/css3-in-depth/table-of-contents>
- <https://app.pluralsight.com/library/courses/js4cs/table-of-contents>
- <https://app.pluralsight.com/library/courses/entity-framework-core-getting-started>
- <https://app.pluralsight.com/library/courses/aspdotnetcore-implementing-securing-api>
- <https://app.pluralsight.com/library/courses/web-api-design>
- <https://app.pluralsight.com/library/courses/aspdotnetcore-implementing-securing-api>

## Building a Web App with ASP.NET Core, MVC, Entity Framework Core, Bootstrap, and Angular

<https://app.pluralsight.com/library/courses/aspnetcore-mvc-efcore-bootstrap-angular-web/table-of-contents>

- ASP.NET Core 2.1
- Entity Framework Core 2.1
- Bootstrap 4.1
- Angular v6

### What Is ASP.NET Core

ASP.NET Core is sitting on .NET Core CLR (Common Language Runtime), which can be on IIS/Self hosted (dotnet cli) on either windows or linux.

Frameworks are the bootstrap and CLR. Anything above Core CLR is a package from Nuget. E.g. MVC, logging, Identity.

Command line:

- `dotnet --version`
- `dotnet new --help`
- `dotnet new console`: does a restore as well for getting nuget packages. The csproj is not same as the ones visual studio created, but work with MSBuild.
- `dotnet run` (which include `dotnet build`)
- `dotnet add package Newtonsoft.Json`

### Installing Visual Studio

1. ASP.NET Core Web Application: Big Project
2. MVC
3. Change Authenticate: Individual user accounts

Web Application: Use Razor web pages.

### Creating a project with visual studio

1. ASP.NET Core Web Application: DutchTreat.
2. Choose Empty project.
3. Uncheck Configure for HTTPS.
4. Only Program.cs and Startup.cs is created. They are same as a console app, which is listening on port 80.
5. Choose IIS Express to run it. The DutchTreat option is same as run `dotnet run`.
6. Choose the web browser (Edge).
7. Check project properties: Debug: App URL: find the port.

Program.cs: `WebHost.CreateDefaultBuilder`

### Serving Your First File

Startup.cs: In `Configure()`, `app.UseStaticFiles();` It serve static files in folder `wwwroot`. If it doesn't exsit, create one.

The folders in `wwwroot` is actually the path needed in the URL.

`app.UseDefaultFiles();` replace default html urls such as `domain/` to `domain/index.html`.

They are all middlewares. Orders matter (Order is so call Pipe). `UseDefaultFiles` need come before `UseStaticFiles()`.

### What Is HTML

HTML != XML

- HTML is not case sensitive.
- Some HTML elements need always use a closing tag instead of self-closing.

### HTML Basics

Anchor: `<a href="https://microsoft.com">` with an attribute forms a link.

Div: is a block that can be formatted.

Img tag need self closing.

<https://github.com/psauthor/BuildingASPNETCore2>

HTML by default is drawn top-down.

### HTML Forms

`form` element is to get info from the user.

`input` has type. The special type is `submit`.

Browser is trying its best to get what to display. It won't throw parsing error.

### CSS Basics

Seperate the structure of web application from the design elements or the rules for how to draw it.

CSS rule order: the more specific rule wins the more general rule. It is not based on the order of rules.

### CSS Naming

Use for CSS selector.

### CSS Classes

ID is unique. Class is to group same style elements together.

CSS selector: `#` for ID, `.` for class. space for child, `ele.class` get the elements that are of the class.

### The Box Model

margin, border, padding, content

- margin is between two elements.
- padding is between border and content.
- direction: left, right, top, bottom.
- properties: height, width. actual-height/width: border.

By default, div is displayed in a block, which take as much horizontal space and as little vertical space as possible.

### What is JavaScript

Object Oriented: prototypical inheritance

Dynamic type: each object itself has the notion of a type.

Compiled: Just-in-time compiled without an intermediate format.

### Adding a JavaScript File

`alert` blocks all the other page rendering.

HTML is rendered line by line. JavaScript is executed as soon as it excuted. So need call JavaScript after the page is fully loaded.

### JavaScript Events

There could be multiple event listener for an element.

JavaScript uses anonymous functions a lot.

JQuery makes all browsers can use same code.

### Using NPM

- Add package.json to the root of the project.
- Add "dependencies" to package.json for runtime dependencies. While "devDependencies" is development dependencies.
- A new node_modules folder would appear with jquery in it. dist folder is for distributions. See which dist you want to use.
- Add `<script src="/node_modules/jquery/dist/jquery.min.js"></script>` to html.
- Add a nuget package `odetocode` which introduce middle tier `app.UseNodeModules(env)` to Startup.cs.

Or call command line tool: `npm install bootstrap`.

### Introducing jQuery

jQuery can be treat as an object in a javascript file.

jQuery is good at find elements in the document.

Change `this` to `$(this)` make the javascript wrapped up to a jQyery object, so it gets cross browser support.

### Practical jQuery

JavaScript issue: Global scope. To make functions scoped, common to wrap all codes inside an anonymous function and let it execute at once.

```JavaScript
(function () {
  ...
})();
```

For jQuery: execute when document is ready.

```jQuery
$(document).ready(function() {});
```

When define a jQuery object, convention is to name it start with `$`.

### What Is MVC

Model-View-Controller framework for applications.

- Model: data
- Controller: logic
- View: markup to display

Request route to a controller class, controller get some data from model, then send back to controller to do some logic, and then controller send data to view, view render and return the response.

### First Controller/View

Create a Controller class inherit from AspNetCore.Mvc.Controller under a folder calls controllers.

Controller maps a request to an action. Action is where the real logic happens.

Views can be returned from action. Convention is that AppController is controller for App, and return views in Views/App folder. View name is same as the action name.

View represents Razor (A syntax for generation/modify view code in C#), which is not html (cshtml).

ViewBag is a bag of properties. With `@` it can be used in html code.

Path in html should start with `~/` indicates it is the root of the project.

### Enabling MVC 6

In Startup.cs add `app.UseMvc()` to set up the routes.

Routes: from pattern of the URL, find out which controller to send the request.

URL pattern "/{controller}/{action}/{id?}": `id?` indicates it is an optional field.

`cfg.MapRoute("Default", "/{controller}/{action}/{id?}", new { controller = "App", Action = "Index" });` means if no controller or action pass in, go to AppController.Index.

ASP.NET Core requires to use dependency injection.

`app.UseDeveloperExceptionPage();` to show the error with call stack.

Using `IHostingEnvironment env` to figure out if the env is a prod or a staging or development. In project property Debug page, can set the environment.

### Creating a Layout

Layout page: the common elements on multiple pages. It is a view shares across controllers.

`@RenderBody()` can put body of a cshtml to the layout.

Add the Views folder, add `_ViewStart.cshtmml` (Razor View Start), which is act as a base class.

### Adding More Views

CSS selector `ele1>ele2` can select direct children.

### Using Tag Helpers

ViewImports is a way to add things that are appear on every page. Like import classes to all pages.

`@addTagHelper` is a decoration of adding a set of TagHelpers.

`[HttpGet("contact")]` on an action can change the action link.

### Razor Pages

`app.UseExceptionHandler("/error");` can specify what the error handling path to use.

Razor page start with `@page` decoration.

Razor page and view are different world. If just want display some simple text, use page is enough.

Need copy `_ViewStart.cshtml` to Pages folder so that the Razor pages can also use layout.

### Implementing a Contact Page

Add a form in the contact page with post method. Each field of the form need has a name property.

In the controller, add `[HttpPost('contact')]` to the contact action. The action should accept `object model`. We need model binding to get the post result.

When debug, in the Watch tab, type in `this.Request` and can find `Form` in its properties.

### Model Binding

Create a view model to represend the data structure of the post form with same name of properties.

In the Razor page, use decorate `@model`, and use tag helper `asp-for` to get the model properties.

Label can also have `asp-for` so that taps the label can set the focus on the input.

### Using Validation

In view model, add `[Required]` or other validation annotations from `System.ComponentModel.DataAnnotations`.

In the controller, call `ModelState.IsValid` to validate. `ModelState` contains all errors.

In the Razor page, add `asp-validation-summary` and `asp-validation-for` to get the error.

Need add "jquery-validation" and "jquery-validation-unobtrusive" to npm.

In Layout, add a `@RenderSection("scripts", false)` so that each razor page can define its own scripts.

`All` vs `ModelOnly`.

But both frontend and backend needs validation.

### Adding a Service

Use visual studio to create a `NullMailService` and an interface in `Services` folder.

Add the service in `ConfigureServices` of `Startup`.

3 types of services:

- Transient: no data on the service. It is a method.
- Scoped: a little expensive to create, but keep around in a connection (most common scope is a length of a request from a client).
- Singleton: kept the lifetime of the server being up.

Dependency injection: in the ctor add the dependency of an interface. In Startup add the real service implementation.

After email is sent, call `ModelState.Clear();` to clear the form.

There should be a ASP.NET Core Web Server output in Visual Stuio **?? I cannot find it**

### Adding Bootstrap

Bootstrap is based on CSS/SASS. Uses javascript on its components.

Being modular and skinnable (so that it looks more personality).

Bootstrap need placed before site.css so that site.css can override settings from bootstrap.

Bootstrap.bundle.min.js bundled all its dependencies. It requires jQuery so should be placed after jQuery.

Add bootstrap classes to elements. `container` is for sections.

Element can belong to several CSS classes. `class="btn btn-success"` means it is a `btn`, but change color to `success`.

### Building a NavBar

Bootstrap NavBar can show correctly on both desktop and cellphone.

In HTML5 `nav` element is for navbar.

### Boostrap's Grid System

Bootstrap use 12-column grids system.

`.col-8 .offset-2`: take 8 columns, and offset 2 columns from the last seen grid.

If out-of-boundary, move to the next line.

Size: `.col-xl-xx` > `.col-lg-xx` > `.col-md-xx` > `.col-sm-xx` > `.col-xx`

`.col-lg-3 .col-md-6 .col-3`

### Using the Grid System

`<div class="row"><div class="col-md-6"></div></div>` to use grid.

`<div class="card card-body bg-light"></div>` to wrap a content into a card.

### Bootstrap Forms

Wrap each form elment related html code into `form-group` class div.

button, submit and anchor tags can be classed as `btn`.

6 colors in bootstrap:

- danger
- primary
- success
- warning
- default

### Using Font Awesome

Font Awesome is the successor of glyph icons.

Use NPM to install it, and put `font-awesome.min.css` before `site.css` so that `site.css` can override it, so document and graph DBs can also be used.

Use `<i>` element with class to define what icon to use.

### Creating Entities

Entity Framework Core: compare to Entity Framework, it removes the requirement of Relational DBs.

EF6 is still more mature than EF core. But to use EF6, you need .NET 4.x.

Create `Data` folder for DB interfaces. Create `Data\Entities` for entites, which are shapes of the data.

Every entity class has `Id` as primary key.

Use `ICollection<AnotherEntity>` to create a parent-child/one-to-many relationship.

Create class `DutchContext` inherit from `DbContext` for establishing connection. Create `DbSet` in it for entities that need to be queryable.

### Using Entity Framework Core Tooling

Under `dotnet-example\DutchTreat\DutchTreat`, run `dotnet ef database update`.

Context isn't tied to a specific database.

Cosmos DB: document store.

In `startup.ConfigureServices`, add `services.AddDbContext<DutchContext>();`. It is added to service collection, and they are injected into different services.

EntityFrameworkCore.DBContextOptionsBuilder provides

- in mem DB: for testing
- SqlServer
- SQLite

### Using Configuration

Startup ctor can inject interfaces that are setup in the Program.

Pass in `Microsoft.Extensions.Configuration.Iconfiguration`, which has `GetConnectionString` method.

In Program.cs, the `WebHost.CreateDefaultBuilder` sets up a default configuration file we can use.

ASP.NET uses a WebConfig file (XML), while ASP.NET Core supports different ecosystem.

```c#
builder.AddJsonFile("config.json", false, true)
    .AddXmlFile("config.xml", true) // not exist.
    .AddEnvironmentVariables(); // not exist.
```

The later config override previous ones if there are conflicts.

Config supports name-value pair and hirerarchy.

DB ProjectsV13 is come by default.

Connection string: `"DutchConnectionString":  "server=(localdb)\\ProjectsV13;Database=DutchTreatDb;Integrated Security=true;MultipleActiveResultSets=true;"`

The integrated security should be replaced with actual creds when deploy to prod.

`MultipleActiveResultSets` allows retriving multiple steams of data at the same time.

In the DutchContext, add ctor `public DutchContext(DbContextOptions<DutchContext> options): base(options) {}`.

Then run `dotnet ef database update` to create a DB folder, and then run `dotnet ef migrations add InitialDb` to generate some migration class. Move the `Migrations` folder under `Data` because it more belongs there. run `dotnet ef database update` again to build schemas.

### Using DbContext

In AppController, inject DutchContext into the ctor. And then create an action, which call `_context.Products.ToList()` (fluent syntax) to query the result.

Or use LINQ query `var results = from p in _context.Products orderby p.Category select p;`.

Create a view, and add the model `IEnumerable<Product>`. This is not necessary but it can let Intellisense works.

`@Model` is the model passed in into the view.

### Seeding the Database

In DbContext, override `OnModelCreating`. It specifies how is the mapping between entities in the DB. For example set a string property to be at most 50 length.

Create data in `modelBuilder.Entity<Order>().HasData()`. It is embedded into migration. `dotnet ef migrations add SeedData` and check `Migrations` folder.

`HadData` has limitation that it can create only simple entity without relationship.

Another way is to create a Seeder class.

Call `dbContext.Database.EnsureCreated()` before DB operations.

```c#
dbContext.Database.EnsureCreated();
dbContext.Products.AddRange(products);
dbContextt.SaveChanges();
```

`host.Services.GetService<DutchSeeder>();` can get services that are set up via Startup. It create an instance and tries to fullfill all of its dependencies.

`scopeFactory`: during every request this factory creates a scope of the lifetime of the request.

### The Repository Pattern

Create a class `DutchRepository` and inject `DutchContext`. Create different methods to expost different calls to DB we want.

Extract an interface of it so that testing can mock it.

Another pattern is use generic. It makes the testing too complicate.

Inject it to Startup by `services.AddScoped<IDutchRepository, DutchRepository>();`. In the same request scope the repository is not created again and again.

### Showing the Products

Need put `div class=col-md-3` in a `div class=row` to make them show side by side.

Need put parenthese around `img src="~/img/@(p.ArtId).jpg"` because otherwise razor would think jpg as a property.

Use `class="img-fluid"` to make picture auto size.

Give a border by `div class="border bg-light rounded p-1"`, p-1 gives a little padding.

### Logging errors

In cmd, run `set ASPNETCORE_ENVIRONMENT=Development`

`dotnet run` already print logs.

Inject `ILogger` to DutchRepository, and let logger type to be `DutchRepository`, so that logs log where are they come from.

Can define different log level for defferent namespaces.

Set logging level in config.json.

### Create an API Controller

Use Postman to send request to API.

Web API is a set of endpoints to expose your APIs.

It expose data, which is similar to AppController, which expose data.

Add attribute `Route("api/[Controller]")` to the controller class.

The API has a verb, `Get`. Implement it with Repository.

Run `http://localhost:10120/api/products` to call it.

The Get API can return an IEnumerable. But then if exception happens, it cannot return properly.

Return Json result can wrap the bad request to a Json object, but it tied the MVC to json.

Return `IActionResult` is the best. Return `Ok` or `BadRequest` wrap with the results.

Can use Swagger to document public APIs.

To use the new document way in .NET Core 2.1, use ControllerBase instead of Controller, and remove `Ok`.

ActionResult returns implicit operator, so that concrete types can be specified and converted. But interfaces cannot.

Add attribute `[ApiController]` to the class.

In startup, `services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);` to opt in the new feature.

### Returning Data

Use `this._ctx.Orders.Include(o => o.Items).ThenInclude(i => i.Product).ToList()` to get both Order, and item and product.

Self referencing loop

- OrderItem refer back to Order.
- Set json option to decide how to handle reference loop.

```c#
services.AddMvc().AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);`
```

Use `[HttpGet("{id:int}")]` attribute to create a Get method for getting order by id.

The method body is `this._ctx.Orders.Find(id)` if only need get an order, but to get item and product as well, use fluent/LINQ syntax `Where`.

### Implementing POST

Post Order with query string: `http://localhost:17661/api/Orders?OrderDate=2017-5-5` can set the OrderDate.

If not add `[FromBody]` attribute to the input model, the action takes property values (CLR object) from query string.

Return `Created($"api/orders/{model.Id}", model)` action result for 201.

When call `SaveAll`, the model has been updated with all properties.s

### Validation and View Models

The view model can also used to validate APIs.

Use DataAnnotations to add constrains.

Change the Post request to use the view model instead of the real object.

Use `ModelState.IsValid` and `return BadRequest(ModelState);` to do the validation and error handling.

### Using AutoMapper

Add Nuget package AutoMapper and AutoMapper.extensions.Microsoft.depdencyInjection..

In `Startup.cs`, inject `services.AddAutoMapper();`

In the controller, inject `IMapper`.

In the controller, `return Ok(this._mapper.Map<Order, OrderViewModel>(order));`.

Add a `DutchMappingProfile` to define the mapping.

### Creating association controllers

The association controllers are to add collection to ViewModel.

AutoMapper will trying to map the object to view model as best as it can. Don't need to define how to map each item in a collection in a class.
i.e. Order has a collection of OrderItem as a property. If define Order to OrderViewModel map, and OrderItem to OrderItemViewModel map, then there is no need to define Order.OrderItem view model map.

Assicoation controller is to deal with urls like orders/1/items.

We can flatting Product into OrderItem view model using auto mapper convention. If the proudct is read-only, no need to create a single view model for it.

Just add the Product as a prefix for all proerties in OrderItemViewModel.

### Using Query Strings for APIs

URL is used to describe what of resouces are looking for. Query string can change the behavior.

In the controller, get method, add a bool parameter with default value.

Send the request with URL like `http://localhost:5000/api/orders?includeItems=false`.

### Authorizing Actions

In the controller, add the attribute `Authorize` to the view.

### Storing Identities in the Database

Create an entity inherit IdentityUser.

Derive DutchContext from `IdentityDbContext<StoreUser>`, where `StoreUser` is the user type.

Need migrate by `dotnet ef migrations add Identity`.

Drop the table and rebuild it since there is too much changes: `dotnet ef database drop`.

`await` vs `.Wait()`.

In the seeder, inject UserManager, and use it to create a StoreUser. Notice it is async.

### Configuring Identity

In startup ConfigureServices, call `services.AddIdentity<StoreUser, IdentityRole>(cfg => {}).AddEntityFrameworkStores<DutchContext>();`.

IdentityRole can used to config roles.

Config can define rules.

Call `AddEntityFrameworkStores` to decide where to store the user. Store users info and product info into 2 DB can help reduce the risk of compromise.

Call `app.UseAuthentication()` to turn the identity on. It needs to be called before `UseMvc`.

Calls are redirect to `Account/Login` page.

### Designing the Login View

Create an `AccountController`.

Use `this.User.Identity.IsAuthenticated` to call into identity to check if the user is logged in.

If the user is login, `return RedirectToAction("Index", "app");`.

To create the View, first create a LoginViewModel with properties and validations. Then create the View cshtml.

### Implementing Login and Logout

Inject `SignInManager` to AccountController.

Create a Login view with Post method. In the cshtml page, add the method post to the form.

Use `signInManager.PasswordSignInAsync` to get the login result. Don't need to access the store user manually.

To add `ModelState.AddModelError("", "Failed to login");`, need add `<div asp-validation-summary="ModelOnly"></div>` in the cshtml.

Username: zhenying@dutchtreat.com, Password: P@ssw0rd!

In the Layout.cshtml, add Login and Logout nav-link.

### Use Identity in the API

When login, calling the APIs shouldn't need to re-auth.

- Using cookies are easiest, but least secure. Also it cannot resolve the issue if other clients other than browser want access APIs.
- Open Id
- OAuth2
- JWT Tokens: The one that used in the course.

Using Identity in ASP NET Core without setting security is by default using cookies.

Inject authentication service: `services.AddAuthentication().AddCookie().AddJwtBearer();`

Add `[Authorize]` to controller classes. When sending a request to the API before get authed, the response returns 302 with redirect URL. It is auth with cookie.

Replace it with `[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]`, so that it returns 401.

Using postman, adding a header with `Authorization` key and `Bearer` value.

Create a REST call (i.e. it is not resolve to a view), `CreateToken`, in Account Controller. It is a POST.

SignInManager.PasswordSignInAsync is actually using a cookie.

Inject UserManager to the account controller, so that we can get a user and call SignManager.CheckSignInAsync.

Claims are a set of well-known keys with values.

- Sub is the subject name.
- Jti is the unique id of each token.
- UniqueName, the user uniq Id.

Key: the secret to encrypt the token. Some part of the token is encrpted but not necessary to encrypt all parts.

Use SymmetricSecurityKey as the key. It needs a string, which we should read it from config, so that IT can replace it with a value that is not in the src code.

Inject Extension.IConfiguration to the controller to read the config.

Create a creds SigningCredentials with the key and SHA256 algorithms.

With the creds we can create JwtSecurityToken.

- Issuer: who create the token.
- Audience: who can use this token.

Return HTTP.Created(src, token).

In startup, add config for AddAuthentication to create a token based on the bearer string in the request header.

In the postman, call the action CreateToken to get the token, and then add this to API call header.

### 

# HERE

<https://app.pluralsight.com/library/courses/aspnetcore-mvc-efcore-bootstrap-angular-web/table-of-contents>

## ASP.NET Web API

Use HTTP methods.

Create a controller, which defines:

- `GetAllProducts` method returns an `IEnumerable<Product>` type, url `/api/products`.
- The `GetProduct` method: url `/api/products/id`.

Web API controller action can return

- void: no content 204.
- HttpResponseMessage: Convert directly to an HTTP response message.
- IHttpActionResult: Call `ExecuteAsync` to create an `HttpResponseMessage` from a factory, then convert to an HTTP response message.
- Some other type: Write the serialized return value into the response body; return 200.

Single-Page Applications (SPAs): the entire page is loaded in the browser after the initial request, but subsequent interactions take place through Ajax requests.

The Web API framework is part of the ASP.NET Stack and is designed to make it easy to implement HTTP services, generally sending and receiving JSON- or XML-formatted data through a RESTful API.

Entity Framework (EF) is an object-relational mapper (ORM) that enables you to create data access applications by programming with a conceptual application model instead of programming directly using a relational storage schema.

Initial the DB: in HTTPApplication, add `System.Data.Entity.Database.SetInitializer(new TriviaDatabaseInitializer());` to `Application_Start()`.

Global config like fommater and routes: `WebApiConfig`.

ASP.NET Scaffolding is a code generation framework for ASP.NET Web applications. The controller generated by it has

- inherit from `ApiController`.
- Context is the db of EF.
- override `Dispose` to release all the resource of Context instance.

Use Code Snippet to add help methods, for example AsyncTask.

Web API routing is very similar to MVC routing. The main difference is that Web API uses the HTTP method, not the URI path, to select the action.

Controller handles HTTP requests. The public methods of the controller are called action methods or simply actions.

When the Web API framework receives a request, it routes the request to an action. To determine which action to invoke, the framework uses a routing table: `WebApiConfig`.

convention:

- "/contacts" go to an MVC controller, and "/api/contacts" go to a Web API controller.
- Route request to Action whose name begins with that HTTP method name for GET, POST, PUT, and DELETE methods.

Parameter Bindings: how Web API creates a value for a parameter.

- Simple types are taken from the URI.
- Complex types are taken from the request body.

Attribute Routing in Web API 2.

- solving the routing problem that resources often contain child resources.
- need decorating http methods: Delete, Get, Head, Options, Patch, Post, Put.
- need controller methods return a data transfer object (DTO) instead of the EF model.

AJAX isn't new, but today there are JavaScript frameworks that make it easier to build and maintain a large sophisticated SPA application. This tutorial uses `Knockout.js`。

building blocks

- ASP.NET MVC creates the HTML page.
- ASP.NET Web API handles the AJAX requests and returns JSON data.
- Knockout.js data-binds the HTML elements to the JSON data.
- Entity Framework talks to the database.

"Code First" approach to EF: write C# classes that correspond to database tables, and EF creates the database.

Write domain objects as POCOs (plain-old CLR objects).

CRUD operations: Create, read, update and delete.

circular navigation properties: EF models can have navigation properties, which are another model. If both entities have reference to the other, a circular created. Use DTOs or change the JSON/XML formatters to solve the problem.

three ways to load related data in EF

- eager loading: load while init DB query, use `System.Data.Entity.Include`.
- lazy loading: automatically loads when the navigation property for that entity is dereferenced. make the navigation property virtual. It can cause serialization problems. Can serialize data transfer objects (DTOs) instead of entity objects to solve it.
- explicit loading: write personal codes to do lazy loading.

DTOs:

- change the database schema to return the client.
- Remove circular references.
- Hide particular properties.
- Flatten object graphs that contain nested objects.
- Avoid "over-posting" vulnerabilities: user posts some properties that are read-only/not exist.
- Decouple your service layer from your database layer.

Open Data Protocol (OData)

- a uniform way to query and manipulate data sets through CRUD operations.
- can have a v4 endpoint that runs side-by-side with a v3 endpoint.

In `Web.config`, add `connectionStrings` for setup the DB connection.

In `App_Start/WebApiConfig.cs`, update `Register` method to setup Entity Data Model (EDM) and route.

- Route setup endpoints.

The `[EnableQuery]` attribute enables clients to modify the query, by using query options such as $filter, $sort, and $page.

OData supports two different semantics for updating

- `PATCH` performs a partial update. The client specifies just the properties to update.
- `PUT` replaces the entire entity.

Using OData, clients can navigate over entity relations.

OData supports creating or removing reference(Odata4)/link(OData3), which is relationships, between two existing entities.

The URI of the reference: `http:/host/Products(1)/Supplier/$ref`

- PUT if the navigation property is a single entity.
- POST if the navigation property is a collection.

actions and functions are a way to add server-side behaviors that are not easily defined as CRUD operations on entities.

- actions have side effects but functions don't.
- actions can used to Complex transactions, Manipulating several entities at once, Allowing updates only to certain properties of an entity, Sending data that is not an entity.
- Functions are useful for returning information that does not correspond directly to an entity or collection.
- binding: An action (or function) can target a single entity or a collection. So it is an action for some entity.
- "unbound" actions/functions: static operations on the service. It is not for some specific entity.

A complex type is a structured type without a key.

[Service Metadata Document](https://docs.microsoft.com/en-us/aspnet/web-api/overview/odata-support-in-aspnet-web-api/odata-v3/creating-an-odata-endpoint#service-metadata-document)

- describes the data model of the service.
- using an XML language called the Conceptual Schema Definition Language (CSDL).
- The metadata document shows the structure of the data in the service, and can be used to generate client code.
- To get the metadata document, send a GET request to `http://localhost:port/odata/$metadata`.
- EntityContainer and EntitySet are defined if an entity can be get in groups.

To add an entity:

1. Create a class.
2. Set up Context class to make EF include the table.
3. Update csdl so client knows the data structure.
4. Write Controller codes to support CRUD.

`[FromOdataUri]` attribute in the key parameter: tells Web API to use OData syntax rules when it parses the key from the request URI.

links:

- uri: `entity/$links/entity`.

Generate Service Proxy for client

- proxy is a .NET class that defines methods for accessing the OData service.
- The proxy translates method calls into HTTP requests.
- setup uri.

Apply Query options

- use LINQ expressions.
- define a method.

`where` clause: GET `http://localhost/odata/Products()?$filter=Category eq 'apparel'`

`orderby` clause: GET `http://localhost/odata/Products()?$orderby=Price desc`

Client-Side Paging

- client might want to limit the number of results.
- `Skip` and `Take`: GET `http://localhost/odata/Products()?$orderby=Price desc&$skip=40&$top=10`

`DataServiceQuery<t>.Expand`: GET `http://localhost/odata/Products()?$expand=Supplier`

`select`: GET `http://localhost/odata/Products()?$select=Name`

A `select` clause can include related entities. In that case, do not call `expand`.

Query options: the parameters the client send in the query string.

- expand.
- filter: based on a boolean condition.
- inlinecount: include the total count of matching entities in the response. used for server-side paging.
- orderby.
- select.
- skip.
- top.

Need `EnableQuerySupport` in the startup `HttpConfiguration`, which enables Query options for any controller action that returns an IQueryable type. Or add `[Queryable]` attribute to the action method.

Filter example

- `http://localhost/Products?$filter=Category eq 'Toys'`
- `http://localhost/Products?$filter=Price ge 5 and Price le 15`
- `http://localhost/Products?$filter=substringof('zz',Name)`
- `http://localhost/Products?$filter=year(ReleaseDate) gt 2005`

[Server-Driven Paging](https://docs.microsoft.com/en-us/aspnet/web-api/overview/odata-support-in-aspnet-web-api/supporting-odata-query-options)

- `[Queryable(PageSize=10)]`
- response will have `"odata.nextLink":"http://localhost/Products?$skip=10"`
- client can see total number of results `http://localhost/Products?$inlinecount=allpages`

Allow ordering only by certain properties, to prevent sorting on properties that are not indexed in the database:

- `[Queryable(AllowedOrderByProperties="Id")]`

`Web.Http.OData.Query.Validators` can validate queries.

Expand example:

- `http://localhost/odata/Products(1)?$expand=Category,Supplier`

Web API limits the maximum expansion depth to 2, to avoid creating large responses.

You can combine `$select` and `$expand` in the same query. Make sure to include the expanded property in the `$select` option.

- `http://localhost/odata/Products?$select=Name,Supplier&$expand=Supplier`

select the properties within an expanded property, e.g. Name is Product property:

- `http://localhost/odata/Categories?$expand=Products&$select=Name,Products/Name`

Raw value of the property substruct the value:

- `http://localhost/odata/Products(1)/Name/$value` returns "Hat", but otherwise it will have odata.metadata includes.

When Web API gets an OData request, it maps the request to a controller name and an action name.

OData URI consists of

- The service root: `https://example.com/odata`
- The resource path: `/Products(1)/Supplier/`
- Query options: `?$top=2`

resource path is divided into segments.

- `Products`
- `1`
- `Supplier`

Controller name is the root of the resource path, which is `ProductsController`.

Action Name is the path segments plus the entity data model(EDM):

- `/Products`: GetProducts
- `Products(1)`: GetProduct
- `Products(1)/Models.Book`: GetBook
- `/Products(1)/Supplier` where Supplier is navigation property: GetSupplierFromProduct
- `/Products(1)/$links/Supplier`: CreateLink
- `/Products(1)/Name` where name is a property: GetNameFromProduct
- `/Products(1)/Rate` where Rate is an action: RateOnProduct

Method Signature rule:

- If the path contains a key, the action should have a parameter named key.
- If the path contains a key into a navigation property, the action should have a parameter named relatedKey.
- Decorate key and relatedKey parameters with the `[FromODataUri]` parameter.
- POST and PUT requests take a parameter of the entity type.
- PATCH requests take a parameter of type `Delta<T>`, where T is the entity type.

[examples](https://docs.microsoft.com/en-us/aspnet/web-api/overview/odata-support-in-aspnet-web-api/odata-routing-conventions)

Exlude a property from the EDM

- Set the `[IgnoreDataMember]`
- `employees.EntityType.Ignore(emp => emp.Salary);`

Allow/disable filter methods

- `[Queryable(PageSize=10)]`
- `[Queryable(AllowedQueryOptions=AllowedQueryOptions.Skip | AllowedQueryOptions.Top)]`
- `[Queryable(AllowedOrderByProperties="Id,Name")]`
- `[Queryable(MaxNodeCount=20)]`
- `[Queryable(AllowedFunctions= AllowedFunctions.AllFunctions & ~AllowedFunctions.All & ~AllowedFunctions.Any)]`
- `[Queryable(AllowedFunctions=AllowedFunctions.AllFunctions & ~AllowedFunctions.AllStringFunctions)]`

Filtering on navigation properties can result in a join, if not indexed.

MIME type: media type. Consists of two strings, a type and a subtype.

- text/html
- image/png
- application/json

response:

```http
HTTP/1.1 200 OK
Content-Length: 95267
Content-Type: image/png
```

request:

```http
Accept: text/html,application/xhtml+xml,application/xml
```

Web API has built-in support for XML, JSON, BSON, and form-urlencoded data, and you can support additional media types by writing a `media formatter`.

JSON and XML formatters serialize objects.

Circular Object References

- If two properties refer to the same object, or if the same object appears twice in a collection, the formatter will serialize the object twice.
- `json.SerializerSettings.PreserveReferencesHandling` to prevent it.

BSON

- BSON is a binary serialization format.
- stands for "Binary JSON"
- BSON and JSON are serialized very differently.
- numeric data types are stored as bytes, not strings.
- designed to be lightweight, easy to scan, and fast to encode/decode.

content negotiation

- HTTP specification (RFC 2616)
- the process of selecting the best representation for a given response when there are multiple representations available.
- Accept, Accept-Charset, Accept-Encoding, Accept-Language.

Validate rules for properties on the model

- use attributes in `System.ComponentModel.DataAnnotations`
- `Required`
- `Range(0, 999)`
- without over-posting, Web API ignore additional attributes client sents.
- to validate over-posting, create a model class that only has attributes that client is allow to send.

Web API does not automatically return an error to the client when validation fails. It is up to the controller action to check the model state and respond appropriately.

Parameter binding

- primitive types can be put in the URI, such as int, bool, double, TimeSpan, DateTime, Guid, decimal, and string. `[FromUri]`.
- complex types using a media-type formatter. `[FromBody]`.

`ExceptionFilterAttribute`: implement a `OnException` to catch unhandled exceptions. Then register it `GlobalConfiguration.Configuration.Filters.Add(new ProductStore.NotImplExceptionFilterAttribute())`.

`IExceptionLogger` and `IExceptionHandler`, to log and handle unhandled exceptions.

- registering multiple exception loggers but only a single exception handler.
- Exception loggers always get called

Use `Microsoft.AspNet.WebApi.Tracing` to add tracking.

To write UT, use a pattern called dependency injection. Basically add interface for the real classes and create mock classes.

Web API assumes that authentication happens in the host. For web-hosting, the host is IIS, which uses HTTP modules for authentication. When the host authenticates the user, it creates a principal, which is an IPrincipal object that represents the security context under which code is running.

Instead of using the host for authentication, you can put authentication logic into an HTTP message handler. In that case, the message handler examines the HTTP request and sets the principal.

- An HTTP module sees all requests that go through the ASP.NET pipeline. A message handler only sees requests that are routed to Web API.
- You can set per-route message handlers, which lets you apply an authentication scheme to a specific route.
- HTTP modules are specific to IIS. Message handlers are host-agnostic, so they can be used with both web-hosting and self-hosting.
- HTTP modules participate in IIS logging, auditing, and so on.
- HTTP modules run earlier in the pipeline. If you handle authentication in a message handler, the principal does not get set until the handler runs. Moreover, the principal reverts back to the previous principal when the response leaves the message handler.

Web API project templates have three options for authentication

- Individual accounts. The app uses a membership database.
- Organizational accounts. Users sign in with their Azure Active Directory, Office 365, or on-premise Active Directory credentials.
- Windows authentication. This option is intended for Intranet applications, and uses the Windows Authentication IIS module.

ASP.NET 4.5.1 expand the security options for Single Page Applications (SPA) and Web API services to integrate with external authentication services, which include several OAuth/OpenID and social media authentication services: Microsoft Accounts, Twitter, Facebook, and Google.

Cross-Site Request Forgery (CSRF) is an attack where a malicious site sends a request to a vulnerable site where the user is currently logged in.

To help prevent CSRF attacks, ASP.NET MVC uses anti-forgery tokens, also called request verification tokens. Malicious page cannot read the user's tokens, due to same-origin policies. (Same-origin policies prevent documents hosted on two different sites from accessing each other's content.

Cross Origin Resource Sharing (CORS)

- a W3C standard
- allows a server to relax the same-origin policy
- a server can explicitly allow some cross-origin requests while rejecting others
- similar to JSONP

Authentication filters let you set an authentication scheme for individual controllers or actions. That way, your app can support different authentication mechanisms for different HTTP resources.

Authenticated doesn't mean authorized to perform an action.

[Basic authentication](http://www.ietf.org/rfc/rfc2617.txt)

- If a request requires authentication, the server returns 401 (Unauthorized). The response includes a WWW-Authenticate header, indicating the server supports Basic authentication. The client sends another request, with the client credentials in the Authorization header. The credentials are formatted as the string "name:password", base64-encoded. The credentials are not encrypted.
- Basic authentication is performed within the context of a "realm".
- Internet standard.
- Supported by all major browsers.
- Relatively simple protocol.
- User credentials are sent in the request.
- Credentials are sent as plaintext.
- Credentials are sent with every request.
- No way to log out, except by ending the browser session.
- Vulnerable to cross-site request forgery (CSRF); requires anti-CSRF measures.

Forms Authentication

- not an Internet standard.
- Easy to implement: Built into ASP.NET.

Integrated Windows authentication

- using Kerberos or NTLM.
- best suited for an intranet environment.
- Built into IIS.
- Does not send the user credentials in the request.
- If the client computer belongs to the domain (for example, intranet application), the user does not need to enter credentials.
- Not recommended for Internet applications.
- Requires Kerberos or NTLM support in the client.
- Client must be in the Active Directory domain.

Open Web Interface for .NET (OWIN)

- defines an abstraction between .NET web servers and web applications.
- decouples the web application from the server, which makes OWIN ideal for self-hosting a web application in your own process, outside of IIS, inside an Azure worker role.

Web API configuration: in the HttpConfiguration class.

- DependencyResolver: Enables dependency injection for controllers.
- Filters: Action filters.
- Formatters: Media-type formatters.
- IncludeErrorDetailPolicy: Specifies whether the server should include error details, such as exception messages and stack traces, in HTTP response messages.
- Initializer: A function that performs final initialization of the HttpConfiguration.
- MessageHandlers: HTTP message handlers.
- ParameterBindingRules: A collection of rules for binding parameters on controller actions.
- Properties: A generic property bag.
- Routes: The collection of routes.
- Services: The collection of services.

Dependency Injection

- constructor injection
- Web API Dependency Resolver
- IoC(inversion of control) container is a software component that is responsible for managing dependencies

Web API example:

- GET /api/products/id
- POST /api/products
- PUT /api/products/id # update
- DELETE /api/products/id

A message handler is a class that receives an HTTP request and returns an HTTP response. Typically, a series of message handlers are chained together. The first handler receives an HTTP request, does some processing, and gives the request to the next handler. At some point, the response is created and goes back up the chain. This pattern is called a delegating handler.

HTML form

```html
<form name="form1" method="post" enctype="multipart/form-data" action="api/upload">
    <div>
        <label for="caption">Image Caption</label>
        <input name="caption" type="text" />
    </div>
    <div>
        <label for="image1">Image File</label>
        <input name="image1" type="file" />
    </div>
    <div>
        <input type="submit" value="Submit" />
    </div>
</form>
```

enctype: form attribute specifies the format of request body.

- application/x-www-form-urlencoded: Form data is encoded as name/value pairs, similar to a URI query string. This is the default format for POST.
- multipart/form-data: Form data is encoded as a multipart MIME message. Use this format if you are uploading a file to the server.

Cookie attributes

- Domain
- Path
- Expires
- Max-Age

CRUD

- GET retrieves the representation of the resource at a specified URI. GET should have no side effects on the server.
- PUT updates a resource at a specified URI. PUT can also be used to create a new resource at a specified URI, if the server allows clients to specify new URIs. For this tutorial, the API will not support creation through PUT.
- POST creates a new resource. The server assigns the URI for the new object and returns this URI as part of the response message.
- DELETE deletes a resource at a specified URI.

Note: The PUT method replaces the entire product entity. That is, the client is expected to send a complete representation of the updated product. If you want to support partial updates, the PATCH method is preferred. This tutorial does not implement PATCH.

Get a product by category: `/api/products?category=category`

Entity Framework translates the C# models used by Web API into database entities.

wire format: transmit data to the client via HTTP.

POCOs(plain-old CLR objects) do not carry any extra properties that describe database state, they can easily be serialized to JSON or XML. However, that does not mean you should always expose your Entity Framework models directly to clients, as we'll see later in the tutorial.

Knockout.js is a Javascript library that makes it easy to bind HTML controls to data. Knockout.js uses the Model-View-ViewModel (MVVM) pattern.

- The model is the server-side representation of the data in the business domain (in our case, products and orders).
- The view is the presentation layer (HTML).
- The view-model is a Javascript object that holds the model data. The view-model is a code abstraction of the UI. It has no knowledge of the HTML representation. Instead, it represents abstract features of the view, such as "a list of items".
- Updates to the view-model are automatically reflected in the view.
- The view-model also gets events from the view.

self-host a web API in your own host process: without IIS.

Code samples: <https://docs.microsoft.com/en-us/aspnet/web-api/samples-list>

Razor syntax is a simple programming syntax for embedding server-based code in a web page.

It is something like HTML

```html
<!-- Single statement blocks  -->
@{ var total = 7; }
@{ var myMessage = "Hello World"; }

<!-- Inline expressions -->
<p>The value of your account is: @total </p>
<p>The value of myMessage is: @myMessage</p>

<!-- Multi-statement block -->
@{
    var greeting = "Welcome to our site!";
    var weekDay = DateTime.Now.DayOfWeek;
    var greetingMessage = greeting + " Today is: " + weekDay;
}
<p>The greeting is: @greetingMessage</p>
```

Client content is the stuff you're used to in web pages.

Razor syntax lets you add server code to this client content. If there's server code in the page, the server runs that code first, before it sends the page to the browser.

Can define variable type, but not necessary to.

```powershell
@{
    var greeting = "Welcome!";
    string name = "Joe";
    DateTime tomorrow = DateTime.Now.AddDays(1);
}
```

[Conditional check](https://docs.microsoft.com/en-us/aspnet/web-pages/overview/getting-started/introducing-razor-syntax-c#conditional-logic-and-loops)

`this` is a page object. `this.Request` shows request collection.

WebMatrix is a tool that integrates a web page editor, a database utility, a web server for testing pages, and features for publishing your website to the Internet. (Which has been abandoned already.)

NuGet: the package manager.

The Gravatar helper.
[MSDN subscription](https://azure.microsoft.com/pricing/member-offers/msdn-benefits-details/?WT.mc_id=A443DD604)

The @ character tells ASP.NET that what follows is Razor code, not HTML. ASP.NET will treat everything after the @ character as code until it runs into some HTML again.

HERE:
<https://docs.microsoft.com/en-us/aspnet/web-pages/overview/getting-started/introducing-aspnet-web-pages-2/layouts>

[Filter with any](https://stackoverflow.com/questions/15475593/webapi-odata-filter-any-or-all-query-not-working)
`~/api/Blogs?$filter=Tags/any(tag: tag/Name eq 'csharp')`

<https://help.nintex.com/en-us/insight/OData/HE_CON_ODATAQueryCheatSheet.htm>

[OData operation vs function vs action?](https://blogs.sap.com/2013/04/26/what-is-the-difference-between-a-operation-a-function-and-an-action/)

- Operations: llow the client to be able to execute a business process on the server.
- Operations are the base for two specialized forms of operations, functions and actions.
- Functions must always return data and must never alter data on the backend so that there are no observable side effects to the function call. This means they are almost always GET HTTP operations.
- Actions on the other hand are a superset of functions in that the two criteria of a function are relaxed – actions do not have to always return a result and executing the action can cause changes to the backend data. actions are performed using the POST.
- Since functions are idempotent they can be used in $filter and $sortby system queries to provide a better fidelity in filtering and sorting queries.
- operations parameters are handled much like querying for an entity with a compound key. The parameters are wrapped in brackets in the format “name = value” seperated by a comma.

## Entity Framework(EF)

<https://docs.microsoft.com/en-us/aspnet/entity-framework>

`dotnet ef migrations --help` can see the options.

Use `[Column(TypeName = "decimal(18,2)")]` before a property to define its restrict.

`modelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(18, 2);` should work samely, but this API is not found.

## RESTful

<https://www.tutorialspoint.com/restful/index.htm>

## WCF

Service oriented: <https://docs.microsoft.com/en-us/dotnet/framework/wcf/whats-wcf>

## LocalDB

[MSSQLLOCALDB databases aren't listed](https://stackoverflow.com/questions/34029337/mssqllocaldb-databases-arent-listed)