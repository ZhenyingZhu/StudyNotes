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
- A `ValidateModelState` attribute is deifined
- Controller name convension: `{Entity}ApiController`
- Should the auth really pass in the apikey **??**
- When use EF, can the properties that are another object saved **??**
- FindPetsByTags() has a `FromQuery` parameter, how to use it **??**
- How to store the data would need to be handled by the developer
- `[Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]` The API key seems not need to passed in **??**
- `[FromRoute][Required][Range(1, 10)]long? orderId` the way to define restriction for a parameter
- `IOperationFilter` and `IDocumentFilter` are for swagger
- Models implement `IEquatable`
- Need to know how to use the response model in code **??**
- How to hook up with EF **??**

## Entity Framework Core

## Identity

OAuth

## React

## Bootstrap
