# Web App

[ASP.NET Tech Options](https://docs.microsoft.com/en-us/aspnet/core/tutorials/choose-web-ui?view=aspnetcore-5.0)

- ASP.NET Core Razor Pages: server rendered. Better than MVC. But FE and BE are not seperated
- Blazor: rendered with Razor components. Hosting options:
  - Blazor Server: rendered on server side. Using SignalR
  - WebAssembly: client rendered
- React SPA: client rendered

[MVC vs. Web API](https://www.dotnettricks.com/learn/webapi/difference-between-aspnet-mvc-and-aspnet-web-api)

[Use the React project template with ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/client-side/spa/react?view=aspnetcore-5.0&tabs=visual-studio)

[Swagger](https://swagger.io/about/)

[axios vs. fetch](https://betterprogramming.pub/rest-api-consumption-in-react-with-fetch-axios-and-axios-hooks-d9dd14b43c8b): Smaller app use fetch

[bower](https://bower.io/): package managenent

[npm](https://www.npmjs.com/): node.js package manager

[node.js](https://nodejs.org/en/): chrome JS engine runtime

[gulp](https://gulpjs.com/): a toolkit to automate JS workflows

[Bootstrap](https://getbootstrap.com/): themeing

**Decision**: Backend use EF and SQLServer. Server use Web API with Swagger to develop. Client use JS to interact with Web API. Bootstrap for basic UI, and later switch to React for better UI

[Prototype](https://github.com/ZhenyingZhu/StudyNotes/tree/master/dotnet-example/TodoApi)

## Swagger

[Getting Start](https://swagger.io/tools/open-source/getting-started/)

- [demo](https://www.youtube.com/watch?v=6kwmW_p_Tig). **[HERE]**: 20:50
- [swagger file path](Github\ToDoAndTracker\swagger.yaml)

- Use the `Swagger Editor` to create your OAS definition and then use `Swagger Codegen` to generate server implementation.
- `Swagger UI` is a collection of HTML, Javascript, and CSS assets that dynamically generate beautiful documentation from an OAS-compliant API.

[Swagger Editor Doc](https://swagger.io/docs/open-source-tools/swagger-editor/): Design API

[Swagger Editor Web Version](https://editor.swagger.io/?_ga=2.220049180.464638247.1646109938-1608438930.1646109938)

[Swagger CodeGen](https://github.com/swagger-api/swagger-codegen#getting-started)

[OpenAPI Spec](https://swagger.io/specification/)

[petstore](https://petstore.swagger.io/#/pet/uploadFile)

Swagger can generate code with doc `AddSwaggerGen`.

Server

- Need to comment out the lagacy source in `swagger\server\NuGet.Config`
- Need to comment out the `TermsOfService` in `Startup.cs`
- It uses ASP.NET Core MVC. What's the difference and how to change it to Web API **??**
- A `ValidateModelState` attribute is deifined
- Controller name convension: `{Entity}ApiController`
- Should the auth really pass in the apikey **??**
- When use EF, can the properties that are another object saved **??**
- FindPetsByTags() has a `FromQuery` parameter, how to use it **??**
- How to store the data would need to be handled by the developer
- `[Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]` The API key seems not need to passed in. How to use it? **??**
- `[FromRoute][Required][Range(1, 10)]long? orderId` the way to define restriction for a parameter
- `IOperationFilter` and `IDocumentFilter` are for swagger
- Models implement `IEquatable`
- Need to know how to use the response model in code **??**
- How to hook up with EF **??**
- when start, the `index.html` is retrieved in the npm [swagger-ui](https://github.com/swagger-api/swagger-ui) **??**

C# Client

- A C# program to call the RESTful APIs. Might be used in some programs talking to the RESTful APIs
- Uses [RestSharp](https://restsharp.dev/) as the HTTP client
  - How to use `InterceptRequest` and `InterceptResponse` **??**
- Each API has an async and a sync version
- Has a config class. Contains:
  - base URL
  - Default header
  - ApiKey, ApiKeyPrefix
  - username, password
  - access token
  - timeout
  - userAgent
- `{api}WithHttpInfo` is setting `Path`, `PathParams`, `QueryParams`, `HeaderParams`, `FormParams`, `FileParams`, `PostBody`, `HttpContentTypes`, `HttpHeaderAccepts`
- How to enforce both `apiKey` and `AccessToken` are needed **??**
- `PostBody` will be set from the input `localVarPostBody = this.Configuration.ApiClient.Serialize(body);`
- For writes where are the `Form` passed in **??**
- When upload file, the file content is `this.Configuration.ApiClient.ParameterToFile("file", file)`. how to call it **??**
- `ExceptionFactory`: just a delegate. From the method name and response status code, generate `ApiException`
- `IApiAccessor`: how API endpoints interact with config
- `SwaggerDateConverter`: handling date time value type
- All the models are re-defined same as them on the server side with `IEquatable` and `IValidatableObject` interface
- build with C# compiler (CSC) directly
- has a test project using mono.  **TODO: give it a try**

## RESTful API Design

[API Design Pattern](https://blog.stoplight.io/api-design-patterns-for-rest-web-services)

HTTP Methods

- POST: return 201.
  - can set cache control in header
- PUT: update existing data. return 204
- PATCH: update subset of existing data. return 204
- GET: result is cached. should not be used to sensitive data. return 200.

Parameters

- Filtering: GET /users?age=30
- Pagination: /users?page=3&results_per_page=20
- Sorting: GET /users?sort_by=first_name&order=asc

[REST API Design Standard](https://blog.stoplight.io/rest-api-standards-do-they-even-exist)

- OAuth
- JSON PATCH

[CRUD API Design](https://blog.stoplight.io/crud-api-design)

- RPC: another design paradigm. executing commands instead of performing actions on resources
- Paths can indicate a hierarchy of subresources (/contacts/22/addresses), but should not be more than 2 levels of nesting
- resources should be lower case. use hyphen to seperate words (/legal-documents)
- use HTTP Accept header to config json or xml is supported
- actions break CRUD but can still be useful

Visual Studio Template

- GET api/{resources}: 200 with list of resources
- GET api/{resources}/{id}: 404 or 200 with the resource. Not return 401 or 403 so the user won't be able to tell if an id exist
- PUT api/{resources}/{id}: 404, 400 with error (e.g., if the id in the request doesn't match the url), or 204
- POST api/{resources}: 201 with the created resource. Not return 400 might because the validation is done in UI
- DELETE api/{resources}/{id}: 404 or 200

[API Key](https://swagger.io/docs/specification/authentication/api-keys/)

- can be in URL query string, or in header, or in cookie
- defined scheme in `securitySchemes` using type `apiKey`. Define where it is in and what is the name

OpenAPI trival notes

- `x-codegen-request-body-name`: define the name of the request body. PUT and POST need it
- `application/x-www-form-urlencoded`: HTML form

**TODO**: Read

- [RESTful Web Services Cookbook](https://learning.oreilly.com/library/view/restful-web-services/9780596809140/)

## ASP.NET Core Web API

ASP.NET Core configs

- default launch URL: `https://localhost:5001/swagger`. How to launch in public **??**
- How to config [HttpPlatformHandler](https://docs.microsoft.com/en-us/iis/extensions/httpplatformhandler/httpplatformhandler-configuration-reference) in web.config **??**
- logging: `appsettings.json` how to setup and how to see **??**
- [HSTS](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Strict-Transport-Security)

[Tutorial: Create a web API with ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-6.0&tabs=visual-studio)

- `CreatedAtAction`: return 201 with the location in the response header points to the URI of the newly created resource
- `dotnet tool install -g Microsoft.dotnet-httprepl`
- `ActionResult` ctor contains `actionName`, `routeValue`, `objectValue`
- [ActionResult vs IActionResult](https://docs.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-6.0#actionresult-vs-iactionresult). If return `NoContent`, use `IActionResult`, otherwise use `ActionResult` so the results can be converted to the HTTP response
- PUT method needs `_context.Entry(input).State = EntityState.Modified;` so the `DbUpdateConcurrencyException` can be thrown
- Data Transfer Object (DTO): hide some properties in the model. More details in [Preventing mass assignment or over posting in ASP.NET Core](https://andrewlock.net/preventing-mass-assignment-or-over-posting-in-asp-net-core/#:~:text=Mass%20assignment%2C%20also%20known%20as%20over-posting%2C%20is%20an,a%20developer%20did%20not%20expect%20to%20be%20set.)
- While handling `DbUpdateConcurrencyException` during PUT, check item exists is to deal with the case that the item is deleted by another caller.

Compare the WebAPI vs. WebApp

- WebApp: has Individual account auth support
- WebApp: has IIS support
- WebAPI: with swagger build in
- WebApp: has `Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore` that shows the EF error page
- WebApp: has a Index.cshtml with `IndeModel` randered
- WebApp: has `builder.Services.AddDatabaseDeveloperPageExceptionFilter();`
- WebApp: has `builder.Services.AddRazorPages();`
- WebAPI: has `builder.Services.AddControllers();` and `builder.Services.AddEndpointsApiExplorer();`
- WebApp: has `app.UseStaticFiles();`, `app.UseRouting();` and `app.MapRazorPages();`

**TODO**: Read

- [Create web APIs with ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-6.0)

## Entity Framework Core

[Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)

- object-relational mapper (O/RM)
- context object: represents a session with the database
- avoid common pitfalls:
  - need knowledge of primary and foreign keys, constraints, indexes, normalization, DML and DDL statements, data types, profiling, etc. **??**
  - test frameworks upgrade like ASP.NET Core, OData, or AutoMapper **??**
  - Performance and stress testing to catch inefficient operations: multiple collections Includes, heavy use of lazy loading, conditional queries on non-indexed columns, massive updates and inserts with store-generated values, lack of concurrency handling, large models, inadequate cache policy **??**
  - Security review: connection strings, secrets, database permissions, input validation for raw SQL, encryption for sensitive data **??**
  - failure scenarios: version rollback, fallback servers, scale-out and load balancing, DoS mitigation, and data backups **??**
  - App deployment/migration: data migration during app start might hit concurrency issue. Staging.

[Getting Started with EF Core](https://docs.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=visual-studio)

- `Install-Package Microsoft.EntityFrameworkCore.Tools` to manage DB migration locally
  - `Add-Migration InitialCreate`
  - `Update-Database`
- The context class can inherit `IdentityDbContext` instead of `DBContext`. When use Identity scaffolding

- In `Program.CreateWebHostBuilder()`, calls `Startup.ConfigureServices(IServiceCollection)` to setup the DB.
  - SqlServer: `services.AddDbContext<SchoolContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));`
  - SQLite: `services.AddDbContext<MyDatabaseContext>(options => options.UseSqlite("Data Source=localdatabase.db"));`
  - With Identity:
    - `services.AddIdentity<StoreUser, IdentityRole>().AddEntityFrameworkStores<DutchContext>();` and `services.AddAuthentication().AddCookie().AddJwtBearer(cfg => { cfg.TokenValidationParameters = new TokenValidationParameters() });`
    - or `services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();`
    - then `services.AddDbContext()`
  - In memory: `services.AddDbContext<TodoContext>(options => options.UseInMemoryDatabase("TodoList"));`

- `DBContext.OnConfiguring(DbContextOptionsBuilder)`: can be used to sets the DB context (e.g., connection string), but the best place is in the `Startup.ConfigureServices(IServiceCollection)`
  - [DbContextOptionsBuilder](https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbcontextoptionsbuilder?view=efcore-6.0): set options with predefined setups
- `DBContext.OnModelCreating(ModelBuilder)` do actions when models are creating
  - `modelBuilder.Entity<Course>().ToTable("Course");`
  - Create compound index: `modelBuilder.Entity<CourseAssignment>().HasKey(c => new { c.CourseID, c.InstructorID });`
  - `modelBuilder.Entity<Product>().Property(p => p.Title).HasMaxLength(250);`
  - Seed an entry: `modelBuilder.Entity<AppTestModel>().HasData(new AppTestModel() { Id = 10, AppTestInput = "Seeding Test1" });`

[Managing Database Schemas](https://docs.microsoft.com/en-us/ef/core/managing-schemas/)

- Migrations: when EF Core model is SOT
- Reverse Engineering: when DB schema is SOT
- [Create and Drop APIs](https://docs.microsoft.com/en-us/ef/core/managing-schemas/ensure-created): `dbContext.Database.EnsureCreated();`. only for testing. A lightweight version of Migrations, but can only use one

[Migrations Overview](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=vs)

- EF core compares the current model against a snapshot of the old model to generate migration file
- migration file can be applied to DB
- EF core records the applied migration in a history table
- Excluding parts of your model: can have the app getting data from multiple DB context. To avoid conflict, exclude the entity from current context
- In the Migrations folder, {timestamp}_{migration name}.cs are calling `migrationBuilder` to interact with DB
- `migrationBuilder.Sql(sqlQuery)`
- Generate SQL script to be applied in Prod: `Script-Migration <FromMigration> <ToMigration>`. If from migration is newer than to migration, it generates a rollback script
- `Script-Migration -Idempotent` can use the history table to generate SQL script automatically
- `Bundle-Migration` and then run the generated `.\efbundle.exe --connection 'Data Source=(local)\MSSQLSERVER;Initial Catalog=Blogging;User ID=myUsername;Password=myPassword'`
- App migration should not be used in Prod: In the `Main()`, `using (var scope = host.Services.CreateScope()) { var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>(); db.Database.Migrate(); }`

[Error: "There was an error running the selected code generator: Package restore failed"](https://stackoverflow.com/questions/44509694/error-there-was-an-error-running-the-selected-code-generator-package-restore)

- The scaffold needs the latest NuGet package versions

[EF Core - adding/updating entity and adding/updating/removing child entities in one request](https://stackoverflow.com/questions/48359363/ef-core-adding-updating-entity-and-adding-updating-removing-child-entities-in)

Models:

- Master
  - Id
  - SomeProperty
  - Children
  - SuperMaster: `[ForeignKey("SuperMasterId")]`
  - SuperMasterId
- Child
  - Id
  - SomeDescription
  - Count
  - Master: `[ForeignKey("MasterId")]`
  - MasterId
  - RelatedEntity: `[ForeignKey("RelatedEntityId")]`
  - RelatedEntityId

Controllers:

- `Master entity = Context.Masters.Include(x => x.SuperMaster).Include(x => x.Children).ThenInclude(x => x.RelatedEntity).FirstOrDefault(x => x.id == id)`
- `entity.Children.Add(Mapper.Map<Child>(child));`: // Mapper.Map uses AutoMapper to map from the input DTO to entity
- Child is created/updated directly in Child table, instead of letting EF updating the Master entity. If child.id is 0, create new one, is -1, delete, otherwise is updating. But this is bad pratice to update primary key as it messed up the EF tracking. Create a seperate list for recording them

- **HERE**

**TODO**: Read

- [Connection Strings](https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-strings)
  - [ProjectV13 vs. MSSqlLocalDB](https://stackoverflow.com/questions/43211082/purpose-of-projectsv13-localdb-instance#:~:text=According%20to%20this%20answer%2C%20SQL%20Server%20Data%20Tools,should%20use%20MSSQLLocalDB%20or%20your%20own%20private%20instance.)
- [Creating and configuring a model](https://docs.microsoft.com/en-us/ef/core/modeling/)

## Identity

[ASP.NET Core security topics](https://docs.microsoft.com/en-us/aspnet/core/security/?view=aspnetcore-6.0)

- **TODO**: go over below security topics
  - [XSRF/CSRF prevention](https://docs.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-6.0)
  - [CORS](https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-6.0)
  - [XSS attacks](https://docs.microsoft.com/en-us/aspnet/core/security/cross-site-scripting?view=aspnetcore-6.0)
  - [SQL injection attacks](https://docs.microsoft.com/en-us/ef/core/querying/raw-sql)
  - [Open redirect attacks](https://docs.microsoft.com/en-us/aspnet/core/security/?view=aspnetcore-6.0)

[What Is Federated Identity?](https://www.okta.com/identity-101/what-is-federated-identity/)

- Federated identity: linking a user’s identity across multiple separate identity management systems
- Build on single sign-on (SSO) techniques
- agree on a set of shared principals
- Service providers trust Identity provider (IdP)
- Federated identity: an agreement between entities about the definition and use of those attributes
- laws of identity
  1. User control and consent
  2. Minimal disclosure: stored securely and deleted quickly
  3. Justification
  4. Directed identity: one company cannot have a permanent view of a user
  5. Competition: support multiple identity providers
  6. Human integration: need a real person to involve
  7. Consistency
- Federated identity management relies on strong agreements: what attributes are representative of who you are online. technologies
  - Security Assertion Markup Language (SAML)
  - OAuth
  - OpenID
- security tokens: JWT (JSON Web Token) tokens and SAML assertions, to pass permissions from one platform to another
  - Pull OAuth credentials from Google's API: choose data like client id and client secrets that both Google and the company knows
  - Grab an access token from the Google Authorization Server
  - Compare the access scopes: compare that your request matches their willingness to share
  - Send the token to an API: Users are ready to gain access, as long as the token is included in an HTTP authorization request header

[strong agreements](https://www.networkworld.com/article/2285444/understanding-federated-identity.html)

- defining an identity for each user, associating attributes with the identity and enforcing a means by which a user can verify identity
- standardized means of representing attributes
- a user may have multiple identifiers associated with multiple roles, each with its own access permissions
- identity mapping: map identities and attributes of a user in one domain to the requirements of another domain
- IdP acquires attribute information through dialog and protocol exchanges with users and administrators
- Service providers: entities that obtain and employ data maintained and provided by identity providers, to support authorization decisions and to collect audit information
- management tasks: configuring systems to perform attribute transfers and identity mapping, and performing logging and auditing functions
- SAML: The principal standard for federated identity

[The Differences Between Standards](https://www.okta.com/identity-101/whats-the-difference-between-oauth-openid-connect-and-saml/#:~:text=The%20main%20differentiator%20between%20these,industry%20standards%20for%20federated%20authentication.)

- Federated Identity: a method of linking a user’s identity across multiple separate identity management systems
- OAuth 2.0: a framework that controls authZ to a protected resource
- OpenID Connect: industry standard for federated authN. Built on the OAuth 2.0 protocol and uses an additional JSON Web Token (JWT)
- SAML: another industry standard for federated authN. Independent of OAuth, relying on an exchange of messages to authenticate in XML SAML format

[Overview of ASP.NET Core authentication](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-6.0)

- schemes: The registered authentication handlers and their configuration options
  - `builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => builder.Configuration.Bind("JwtSettings", options)).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => builder.Configuration.Bind("CookieSettings", options));`
  - `AddJwtBearer` and `AddCookie` are scheme specific extentions. The first parameter specify the scheme name
  - AuthZ policies can specify schemes to authN a user
  - Call UseAuthentication before any middleware that depends on users being authenticated
- AuthN provides the claims principal for AuthZ
- Multiple AuthN scheme approaches to select which authN handler to generate claims:
  - AuthN scheme
  - default authN scheme
  - directly set HttpContext.User
- AuthN handler: implements the behavior of a scheme. It creates `AuthenticationTicket` for a user when authN succeed. Also has methods to challenge (unauthN) and forbid (unauthZ)
- RemoteAuthenticationHandler: Async. OAuth 2.0 and OIDC both use this pattern. JWT and cookies don't
- Challenge: A cookie authentication scheme redirecting the user to a login page. A JWT bearer scheme returning a 401 result with a `www-authenticate: bearer` header.

[Introduction to Identity on ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-6.0&tabs=visual-studio)

- Adds UI and SQL server for ASP.NET Core web app. Work with Azure AD and [Duende IdentityServer4](https://duendesoftware.com/products/identityserver)
- [src code](https://github.com/dotnet/AspNetCore/tree/main/src/Identity)
- A razor class lib. Use UserManager and SignInManager. Need to [disable default account verification](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-6.0&tabs=visual-studio#disable-default-account-verification) in Prod **TODO**: How to add a real email sender
- Can use `builder.Services.Configure<IdentityOptions>(options => )` to config identity and `builder.Services.ConfigureApplicationCookie(options => )` to config cookie. [Options](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.identityoptions?view=aspnetcore-6.0)
- Add `[Authorize]` to a razor model or an api controller
- [Microsoft identity platform](https://docs.microsoft.com/en-us/azure/active-directory/develop/): An evolution of the Azure AD developer platform. But not related to ASP.NET Core Identity

[Mapping, customizing, and transforming claims in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/claims?view=aspnetcore-6.0)

- claim: a name value pair represents what the subject is (like email, role) for a user/identity data. Issued by a trusted identity provider (ASP.NET Core identity)
- OpenId connect client: config and map claims

[Authentication and authorization for SPAs](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity-api-authorization?view=aspnetcore-6.0)

- Duende IdentityServer implements OpenID Connect
- `services.AddIdentityServer().AddApiAuthorization<ApplicationUser, ApplicationDbContext>();` to use IdentityServer
- `app.UseIdentityServer();` to expose OpenID connect endpoints
- Use `services.Configure<JwtBearerOptions>(schemeName, options => {})` to config API authN handler
- API's JWT handler raises `JwtBearerEvents`. AddIdentityServerJwt registers its own event handlers.
- For client: `const token = await authService.getAccessToken();`, then add the token to header 'Authorization': `Bearer ${token}`.
- When deploy to prod, need resources to be provisioned:
  - a DB to store identity user accounts and IdentityServer grants
  - A production certificate to use for signing tokens

**HERE**: <https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity-api-authorization?view=aspnetcore-6.0#example-deploy-to-a-non-azure-web-hosting-provider>

[Scaffold Identity in ASP.NET Core projects](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-6.0&tabs=visual-studio)

- Visual studio 2022 -> Add -> Identity
- Then do `Add-Migration`
- Must do it before adding a DB context, otherwise the Identity DB context cannot add-migration. **TODO**: figure out why
- The Identity webpage link is under url: `https://localhost:{port}/Identity/Account/Login`
- [Identity not working in .NET Core project](https://stackoverflow.com/questions/60099787/identity-not-working-in-net-core-project)

- [Protect a web API with AAD](https://docs.microsoft.com/en-us/azure/api-management/api-management-howto-protect-backend-with-aad)
- [Overview of ASP.NET Core authentication](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-6.0)
- [Creating ASP.NET Core projects with Microsoft identity platform](https://github.com/AzureAD/microsoft-identity-web/blob/master/tools/app-provisioning-tool/vs2019-16.9-how-to-use.md)

Cannot find Identity pages. Need compare to [Dotnet6WebAppBoilerPlate](https://github.com/ZhenyingZhu/StudyNotes/tree/master/dotnet-example/Dotnet6WebAppBoilerPlate)

- OAuth2?
- JWT?

- Claim?
- identity?
- principal?
- ticket?

## Javascript

[Tutorial: Call an ASP.NET Core web API with JavaScript](https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-javascript?view=aspnetcore-6.0)

- [What Does javascript:void(0); Mean?](https://www.freecodecamp.org/news/javascript-void-keyword-explained/)
- flows
  - view: a table. `getItems()` is called at the end of rendering the html
    - `getItems()` calls `GET uri` and gets the response json, then call `_displayItems()` that sets the view table and the counter
    - `_displayItems()` first clean the view table, then create a row for each todo item, then stores the todo items in an array. `cloneNode` is faster than `createElement`, so use `cloneNode` as much as possible
    - `Edit` button `onclick` calls `displayEditForm(id)`. It reads an todo item from the array, then binds the fields to the edit form, then unhides the form
    - `Delete` button `onclick` calls `deleteItem(id)`. It calls `DELETE uri/id`, then `getItems()`
  - add: a form, `onsubmit` calls `addItem()` with `POST` method. Submit button `Add`
    - `addItem()` reads input from the form, calls `trim()` for the text input, then calls `POST uri`, but not uses the result, and instead calls `getItems()`, then cleans the input form
  - update: a form, normally hidden. `onsubmit` calls `updateItem()` without set method. Submit button `Save`
    - `aria-label` is set for the inavtive button `X` (`&#10006;`). It calls `closeInput()` when `onclick`
    - `updateItem()` reads input from the form, then `parseInt()` on int and `trim()` on string, then calls `PUT uri/id`, and then calls `getItems()`. `closeInput()` is called outside the async, because even the call fails, it also hides the form. Return false to stop default form submission
    - [What is the meaning of onsubmit="return false"?](https://stackoverflow.com/questions/35037069/what-is-the-meaning-of-onsubmit-return-false-javascript-jquery)
  - counter: a `<p>`

[Handling status 500 with try and catch](https://stackoverflow.com/questions/70709770/handling-status-500-with-try-and-catch#:~:text=1%20Take%20a%20look%20at%20the%20Fetch%20docs%3A,the%20promise%20resolves%20and%20no%20error%20is%20thrown.)

- `fetch` returns 500 doesn't go to `catch` block.

## React

- Form input validation?
- what input pre-process? `parseInt`, `string.trim()`?

## Bootstrap

## Azure

<https://docs.microsoft.com/en-us/aspnet/core/tutorials/publish-to-azure-webapp-using-vs?view=aspnetcore-6.0>

How to use docker **??**
