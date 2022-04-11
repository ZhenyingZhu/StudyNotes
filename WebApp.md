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

**Decision**: Server use Web API. Client use Swagger to develop and later switch to React for better UI

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

## ASP.NET Core Web API

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

**TODO**: Read

- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Getting Started with EF Core](https://docs.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=visual-studio)
- [Protect a web API with AAD](https://docs.microsoft.com/en-us/azure/api-management/api-management-howto-protect-backend-with-aad)
- [Create web APIs with ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-6.0)

## Entity Framework Core

## Identity

OAuth2? JWT?

- Claim?
- identity?
- principal?
- ticket?

## React

- Form input validation?
- what input pre-process? `parseInt`, `string.trim()`?

## Bootstrap

## Azure

<https://docs.microsoft.com/en-us/aspnet/core/tutorials/publish-to-azure-webapp-using-vs?view=aspnetcore-6.0>

How to use docker **??**
