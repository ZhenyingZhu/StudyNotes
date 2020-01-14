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
- <https://app.pluralsight.com/library/courses/typescript>
- <https://app.pluralsight.com/library/courses/angular-2-getting-started-update>
- <https://app.pluralsight.com/library/courses/angular-cli>

- <https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-3.0>

- <https://fullstackmark.com/post/13/jwt-authentication-with-aspnet-core-2-web-api-angular-5-net-core-identity-and-facebook-login>

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

[dotnet cmdlet](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-new?tabs=netcore22)

Dotnet can create

- console app
- class lib
- UT
- Razor page: A slim version of MVC. Model and Controller code is included in Razor Page itself. So it is a Model-View-ViewModel (MVVM) framework. If just simple pages with basic writes, use it.
- MVC: If have a lot of dynamic server views, use it.
- ASP.NET Core Web app + FE frameworks
- Razor Class Lib
- ASP.NET Core Web API

MVC:

- Model: a data structure
- View: a web form for input + output (data 2 way binding)
- Controller: how to get the data and represent in View.

### Installing Visual Studio

1. ASP.NET Core Web Application: Big Project
2. MVC
3. Change Authenticate: Individual user accounts

Web Application: Use Razor web pages.

Visual Studio Web Application options:

[HTTPS intro](https://love2dev.com/blog/how-https-works/)

- Wraps an encrypted layer around HTTP.
- HTTP: async communicate protocol.
  1. init a connection
  2. data passed between server and client are through TCP with multi round trips
  3. TCP slow start: first packet size is 16kb, keep doubling on success until 4MB or the bandwidth of the client reachs
- Use TLS to encrypt. It has replaced SSL. Its certs are free.
  - a cert is a public key with the id of the owner.
  - TLS cert is installed on server.
  - public and private keys are stored on server.
  - Based on public/private-key cryptography: public key is used for encryption, private key is for decryption.
- HTTPS Handshake
  1. browser connect to an HTTPS server with a SYNC
  2. server returns a SYNC ACK
  3. brower sends an ACK + client hello
  4. server answers a server hello + the cert with public key
  5. browser checks 1. the owner matches the server, 2. the cert is signed by a trusted authority
  6. brower and server exchange a secret, which means both sides send others a public key and keep a private key??
  7. then start communicate with app data and encrypted with public keys
  8. only the domain is not encrypted. Even the full URL is encrypted.

Config HTTPS

- The HTTPS url will appear in the Properties/launchSettings.json.
- [HSTS](https://aka.ms/aspnetcore-hsts) service will be injected. It provides an opt-in security enhancement by returning a response header client can use.
- In develop mode, IIS Express generates [self-signed cert](https://docs.microsoft.com/en-us/aspnet/visual-studio/overview/2013/aspnet-and-web-tools-20132-preview-for-visual-studio-2013-release-notes#ssl). This is a root cert. Can find it in Cert Management under Personal/Certificates/localhost.

Config Auth method: If choose [Authentication](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/individual?view=aspnetcore-2.1#win), the auth option will appear in the Properties/launchSettings.json. The requests will contain the caller's user info. With different auth method the user info will be passed in different ways. For example Windows Auth will use AAD to encrypt username and password, so it is good for intranet websites, so that all the cx will be login as AAD user before using the website.

Project dependencies are AspNetCore.App and Razor.Design.

[Empty](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/?view=aspnetcore-2.2&tabs=windows)

- Program.cs: CreateWebHostBuilder and run. It is using [Kestrel server](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/?view=aspnetcore-2.2&tabs=windows#servers).
- Startup.cs: Config required services and HTTP request pipeline, which is a series of middleware components.
- appsettings.json and appsetttings.Development.json: contains key-value pairs. By default it defines logging and host. Can use env vars to override them. If need manage confidential config data, can use [Secret Manager tool](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.2&tabs=windows)

[API](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-2.2&tabs=visual-studio)

- Can choose Authentication.
- Startup.cs: Config service inject MVC. Runtime pipeline use HttpsRedirection and use MVC.
- ValuesController.cs

[Web Application MVC](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-2.2&tabs=visual-studio)

- wwwroot with jquery + bootstrap.
- Areas: Looks like it is for identity.
- Data: for DB connection.
- Pages: Razor pages. An index.cshtml is created. Login and cookie logics are also created.
- In appsettings.json, a connection string is created to connect to localdb.
- In Startup.cs, injected cookie, DB, identity.
- HomeController: 3 default controllers: Index, Privacy and Error.
- Models: 1 default model: Error.
- Views: Index and Privacy.

[Razor class library](https://docs.microsoft.com/en-us/aspnet/core/razor-pages/ui-class?view=aspnetcore-2.2&tabs=visual-studio)

[Angular](https://github.com/aspnet/JavaScriptServices)

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

In head

- `<meta charset="utf-8" />`
- `<title>`
- CSS: `<link rel="stylesheet" href="{css path}" />`.

In body

- header: can put navigation bar `<nav>`.
- page content
- footer: put copyright info
- JS: `<script src="~/lib/jquery/dist/jquery.js"></script>`

An element can have attributes with different values separate by spaces:

`data-*` attrs are data attributes, which can be used to store custom data in HTML attr so that CSS selector can find it.

```html
<li data-quantity="700g" data-vegetable="not spicy like chili">Red pepper</li>
```

Common Elements

- a: doesn't necessary to have href attr. It might be used for ASP routing.
- div: a section, so that 1. it can be styled with CSS, 2. can perform tasks with JS.
- img
- ul, li
- button
- br
- form: label, textarea, input

### HTML Forms

`form` element is to get info from the user.

`input` has type. The special type is `submit`.

Browser is trying its best to get what to display. It won't throw parsing errors.

### CSS Basics

Seperate the structure of web application from the design elements or the rules for how to draw it.

CSS rule order: the more specific rule wins the more general rule. It is not based on the order of rules.

Styles:

- font-family: cursive;
- font-size: 18px;
- font-weight: bold;
- text-decoration: line-through;
- text-shadow: 1px 1px 1px black;
- text-align: center;
- width: 200px;
- max-width: 150px;
- background: linear-gradient(to bottom, rgba(0,0,0,0.25), rgba(0,0,0,0.1));
- background: url("") 5px center no-repeat;
- color: white;
- list-style-type: none;
- display: inline-block;
- float: right;
- cursor: pointer;
- text-transform: uppercase;
- content: ' |'; Add content. Need work with selector after.
- opacity: 0.5
- border-collapse: collapse;
- border-spacing: 0;

### CSS Naming

Use for CSS selector.

`<li class="a b">` means this element belongs to both class a and b.

### CSS Classes

ID is unique. Class is to group same style elements together.

[CSS selector](https://developer.mozilla.org/en-US/docs/Learn/CSS/Introduction_to_CSS/Selectors)

Simple selector

- `#` for ID
- `.` for class
- `*` for all elements
- space for child,
- `ele.class` get the elements that are of the class.

Attr selector

- `[attr]`: all elements with `attr`.
- `[attr=val]`: if `attr` eq val.
- `[attr~=vall]`: if one of the attr value is val.
- `[attr^=val]`: start with val.
- `[attr$=val]`: end with val.
- `[attr*=val]`: val is a substr.
- `[attr|=val]`: used to match lang such as `en` or `en-US`.

Pseudo classes

Random access an element in a list

- [:first-child](https://developer.mozilla.org/en-US/docs/Web/CSS/:first-child): represents the first element among a group of sibling elements.
- [:first-of-type](https://developer.mozilla.org/en-US/docs/Web/CSS/:first-of-type): represents the first element of its type among a group of sibling elements.
- [:last-child](https://developer.mozilla.org/en-US/docs/Web/CSS/:last-child): represents the last element among a group of sibling elements.
- [:last-of-type](https://developer.mozilla.org/en-US/docs/Web/CSS/:last-of-type): represents the last element of its type among a group of sibling elements.
- [:nth-child()](https://developer.mozilla.org/en-US/docs/Web/CSS/:nth-child): matches elements based on their position in a group of siblings.
- [:nth-last-child()](https://developer.mozilla.org/en-US/docs/Web/CSS/:nth-last-child): matches elements based on their position among a group of siblings, counting from the end.
- [:nth-last-of-type()](https://developer.mozilla.org/en-US/docs/Web/CSS/:nth-last-of-type): matches elements of a given type, based on their position among a group of siblings, counting from the end.
- [:nth-of-type()](https://developer.mozilla.org/en-US/docs/Web/CSS/:nth-of-type): matches elements of a given type, based on their position among a group of siblings.

Select based on user behavior

- [:active](https://developer.mozilla.org/en-US/docs/Web/CSS/:active): an element (such as a button) that is being activated by the user.
- [:enabled](https://developer.mozilla.org/en-US/docs/Web/CSS/:enabled): represents any enabled element.
- [:disabled](https://developer.mozilla.org/en-US/docs/Web/CSS/:disabled): represents any disabled element.
- [:checked](https://developer.mozilla.org/en-US/docs/Web/CSS/:checked): radio, checkbox or option element that is checked or toggled to an on state.
- [:focus](https://developer.mozilla.org/en-US/docs/Web/CSS/:focus): represents an element that has received focus.
- [:focus-within](https://developer.mozilla.org/en-US/docs/Web/CSS/:focus-within): represents an element that has received focus or contains an element that has received focus.
- [:hover](https://developer.mozilla.org/en-US/docs/Web/CSS/:hover): matches when the user interacts with an element with a pointing device
- [:fullscreen](https://developer.mozilla.org/en-US/docs/Web/CSS/:fullscreen): matches every element which is currently in full-screen mode.

User input validation

- [:in-range](https://developer.mozilla.org/en-US/docs/Web/CSS/:in-range): represents an `<input>` element whose current value is within the range limits specified by the min and max attributes.
- [:out-of-range](https://developer.mozilla.org/en-US/docs/Web/CSS/:out-of-range): represents an `<input>` element whose current value is outside the range limits specified by the min and max attributes.
- [:invalid](https://developer.mozilla.org/en-US/docs/Web/CSS/:invalid): represents any `<input>` or other `<form>` element whose contents fail to validate.
- [:indeterminate](https://developer.mozilla.org/en-US/docs/Web/CSS/:indeterminate): represents any form element whose state is indeterminate.
- [:valid](https://developer.mozilla.org/en-US/docs/Web/CSS/:valid): represents any `<input>` or other `<form>` element whose contents validate successfully.
- [:visited](https://developer.mozilla.org/en-US/docs/Web/CSS/:visited): represents links that the user has already visited. For privacy reasons, the styles that can be modified using this selector are very limited.

Based on element defination

- [:default](https://developer.mozilla.org/en-US/docs/Web/CSS/:default): form elements that are the default in a group of related elements.
- [:dir()](https://developer.mozilla.org/en-US/docs/Web/CSS/:dir): matches elements based on the directionality of the text contained in them.
- [:empty](https://developer.mozilla.org/en-US/docs/Web/CSS/:empty): represents any element that has no children, neither element nodes nor text (including whitespace).
- [:lang()](https://developer.mozilla.org/en-US/docs/Web/CSS/:lang): matches elements based on the language they are determined to be in.
- [:link](https://developer.mozilla.org/en-US/docs/Web/CSS/:link): It matches every unvisited `<a>`, `<area>`, or `<link>` element that has an href attribute.
- [:only-child](https://developer.mozilla.org/en-US/docs/Web/CSS/:only-child): represents an element without any siblings.
- [:only-of-type](https://developer.mozilla.org/en-US/docs/Web/CSS/:only-of-type): represents an element that has no siblings of the same type.
- [:read-only](https://developer.mozilla.org/en-US/docs/Web/CSS/:read-only): represents an element that is not editable by the user.
- [:read-write](https://developer.mozilla.org/en-US/docs/Web/CSS/:read-write): represents an element that is editable by the user.
- [:required](https://developer.mozilla.org/en-US/docs/Web/CSS/:required): represents any `<input>`, `<select>` or `<textarea>` element that has the required attribute set on it.
- [:optional](https://developer.mozilla.org/en-US/docs/Web/CSS/:optional): represents any `<input>`, `<select>`, or `<textarea>` element that does not have the required attribute set on it.
- [:root](https://developer.mozilla.org/en-US/docs/Web/CSS/:root): matches the root element of a tree representing the document.
- [:scope](https://developer.mozilla.org/en-US/docs/Web/CSS/:scope): represents elements that are a reference point for selectors to match against.
- [:target](https://developer.mozilla.org/en-US/docs/Web/CSS/:target): represents the target element with an id matching the URL's fragment.

Print

- [:first](https://developer.mozilla.org/en-US/docs/Web/CSS/:first): used with the `@page` at-rule, represents the first page of a printed document.
- [:left](https://developer.mozilla.org/en-US/docs/Web/CSS/:left): used with the `@page` at-rule, represents all left-hand pages of a printed document.
- [:right](https://developer.mozilla.org/en-US/docs/Web/CSS/:right): used with the `@page` at-rule, represents all right-hand pages of a printed document.

Expression

- [:is](https://developer.mozilla.org/en-US/docs/Web/CSS/:is): takes a selector list as its argument, and selects any element that can be selected by one of the selectors in that list.
- [:not()](https://developer.mozilla.org/en-US/docs/Web/CSS/:not): elements that do not match a list of selectors.

Combine several selectors

```css
a:hover,
a:active,
a:focus {
  color: darkred;
  text-decoration: none;
}
```

Find odd/even:

```css
li:nth-of-type(2n) {
  background-color: #ccc;
}

li:nth-of-type(2n+1) {
  background-color: #eee;
}
```

Pseudo elements

- `::after`
- `::before`
- `::first-letter`
- `::first-line`
- `::selection`
- `::backdrop`

```css
[href^=http]::after {
  content: '⤴';
}
```

Combinator

- A, B
- A B: Matching B and is a descendant of A
- A > B: Matching B and is direct child of A
- A + B: matching B and is the next sibling of A
- A ~ B: matching B and is one of the next sibling of A

### The Box Model

margin, border, padding, content

- margin is between two elements.
- padding is between border and content.
- direction: left, right, top, bottom.
- properties: height, width. actual-height/width: border.

By default, div is displayed in a block, which take as much horizontal space and as little vertical space as possible.

- border: 1px solid grey;
- padding: 8px 2px;
- margin: 2px;
- width: 450px;
- border-radius: 5px;

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
- To use jQuery, add `<script src="/node_modules/jquery/dist/jquery.min.js"></script>` to html. (Or just use `lib/jquery/dist/jquery.min.js` since the visual studio MVC project template has jquery included by default). Another way is to use a CDN.
- Add a nuget package `odetocode` which introduce middle tier `app.UseNodeModules(env)` to Startup.cs.

Or call command line tool: `npm install bootstrap`.

Note that [odetocode](https://github.com/OdeToCode/UseNodeModules) can serve files from `node_modules` directory in the project root, but when publish, if this folder is not copied, the server would fail to start. Need follow steps below to solve the issue.

[Correct way to setup the project](https://stackoverflow.com/questions/37935524/how-to-use-npm-with-asp-net-core)

- Need copy the `node_module` folder to wwwroot using gulp.

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

In visual studio, can create 2 kinds of controllers: MVC Controller, API controller.

- Both require to select Model and DataContext.
- The default Context is ApplicationDbContext.
- The Model needs have a primary key, using `[key]` annotation.

The default controller created by visual studio has

- List API: used by Index.
- GET by ID API: used by Detail.
- Create: Use ValidateAntiForgeryToken to prevent [Cross site request forgery](https://en.wikipedia.org/wiki/Cross-site_request_forgery)
- POST Create with binding input
- Edit by ID: used by Edit.
- POST Edit by id with binding input
- Delete
- DeleteConfirmed

For Create/Update call, when successfully write, it `return RedirectToAction(nameof(Index));` to go back to index view.

For update, it handles concurrency exception.

After creating the model, need use Package Manager Console to apply pending migrations to the database:

```cmd
PM> Add-Migration <a note as the name>
PM> Update-Database
```

See all the commands [here](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)

Or use dotnet cmdlet:

```cmd
dotnet ef database update
```

### Enabling MVC 6

In Startup.cs add `app.UseMvc()` to set up the routes.

Routes: from pattern of the URL, find out which controller to send the request.

URL pattern "/{controller}/{action}/{id?}": `id?` indicates it is an optional field.

`cfg.MapRoute("Default", "/{controller}/{action}/{id?}", new { controller = "App", Action = "Index" });` means if no controller or action pass in, go to AppController.Index.

Default route specifies controller, action, and id segments. After `?` is the URL of query string.

ASP.NET Core requires to use dependency injection.

`app.UseDeveloperExceptionPage();` to show the error with call stack.

Using `IHostingEnvironment env` to figure out if the env is a prod or a staging or development. In project property Debug page, can set the environment.

### Creating a Layout

Layout page: the common elements on multiple pages. It is a view shares across controllers.

Put `_Layout.cshtml` under `Views\Shared` folder.

- Use`@ViewBag` to get properties
- `@RenderBody()` can put body of a cshtml to the layout.
- `<environment name="">` tag can write different contents for different env.
- `@if()` can access ASP C# classes, like `User.Identity.IsAuthenticated`
- `@RenderSection("secName")`: if child defines `@section secName`, this part will be randered differently. I think it is used for things other than body.

Add the Views folder, add `_ViewStart.cshtml` (Razor View Start), which is act as a base class.

### Adding More Views

CSS selector `ele1>ele2` can select direct children.

### Using Tag Helpers

[TagHelper](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/tag-helpers/intro?view=aspnetcore-3.0)

- enable server-side code to participate in creating and rendering HTML elements in Razor files
- `asp-for` is one tag helper. This attribute can extract a property name of a model. `<label asp-for="Movie.Title"></label>` becomes `<label for="Movie_Title">Title</label>`.
- anchor tag helper: `<a asp-controller="AppTestChildModels" asp-action="Details" asp-route-id="@c.Id">@c.Name</a>`
- [Built-in helpers](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/tag-helpers/intro?view=aspnetcore-3.0#built-in-aspnet-core-tag-helpers)

`_ViewImports.cshtml` is a page that provides a way to add things that are appear on every page. Like import classes to all pages.

`@addTagHelper` is a decoration of adding a set of TagHelpers.

`[HttpGet("contact")]` on an action can change the action link.

### Razor Pages

`app.UseExceptionHandler("/error");` can specify what the error handling path to use. When error happens, show this view.

Razor page start with `@page` decoration. It makes the file into an MVC action, and don't go through a controller. In this case error page is a Razor page.

Razor page and view are different world. If just want display some simple text, use page is enough.

Need copy `_ViewStart.cshtml` to Pages folder so that the Razor pages can also use layout.

### Implementing a Contact Page

Create a contact view page. Use `@model` to pick the model. Use `@section Scripts` to pick the js to use.

Add a form in the contact page with post method. Each field of the form need has a name property. In the future can use `asp-for` and `asp-validation-for` to bind data.

In the controller, add `[HttpPost('contact')]` to the contact action. The action should accept `object model`. We need model binding to get the post result.

When debug hit a breakpoint in the controller, in the Watch tab, type in `this.Request` and can find `Form` in its properties.

### Model Binding

Create a view model to represend the data structure of the post form with same name of properties.

In the Razor page, use decorate `@model`, and use tag helper `asp-for` to get the model properties.

Label can also have `asp-for` so that taps the label can set the focus on the input.

Each group of form elements, for example `<label>` and `<input>`, can be put in a div.

In the controller action, a parameter can have attribute `[Bind("propertyName")]`, to prevent cross-site request forgery. [detail](https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/implementing-basic-crud-functionality-with-the-entity-framework-in-asp-net-mvc-application#overpost)

Form attr `asp-action`: the value is the action name in the controller.

`<a asp-action="Create">Create New</a>` can create a link to that view.

### Using Validation

In view model, add `[Required]` or other validation annotations from `System.ComponentModel.DataAnnotations`.

In the controller, call `ModelState.IsValid` to validate. `ModelState` contains all errors.

[ModelState](https://docs.microsoft.com/en-us/dotnet/api/system.web.mvc.modelstate?view=aspnet-mvc-5.2) is used to check validation rules for data binding.

In the Razor page, add `asp-validation-summary` and `asp-validation-for` to get the model validation error. Those are [Tag helpers](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/working-with-forms?view=aspnetcore-2.2).

Need add "jquery-validation" and "jquery-validation-unobtrusive" to npm. But visual studio project template for MVC also contains them.

In Layout, add a `@RenderSection("scripts", false)` so that each razor page can define its own scripts.

`ValidationSummary`: `All` vs `ModelOnly`.

- All shows all the errors including the property errors, but on the page, property errors usually show around the property.
- ModelOnly only tells if the model is wrong.

But both frontend and backend needs validation.

### Adding a Service

Use visual studio to create a `NullMailService` and an interface in `Services` folder.

Add the service in `ConfigureServices` of `Startup`.

```C#
services.AddTransient<IMailService, NullMailService>();
```

3 types of services:

- Transient: no data on the service. It is a method.
- Scoped: a little expensive to create, but keep around in a connection (most common scope is a length of a request from a client).
- Singleton: kept the lifetime of the server being up.

Dependency injection:

- in the controller, add a field store the service instance.
- in the controller ctor, add the dependency of an interface.
- In Startup add the real service implementation. The controller can be created by the factory pattern with the dependency.

After email is sent, call `ModelState.Clear();` to clear the form.

There should be a ASP.NET Core Web Server output in Visual Studio. In debugging mode, those outputs are in Debug window.

### Adding Bootstrap

Bootstrap is based on CSS/SASS. Uses javascript on its components.

Being modular and skinnable (so that it looks more personality).

Bootstrap need be placed before site.css (the personal css) so that site.css can override settings from bootstrap.

Bootstrap.bundle.min.js bundled all its dependencies. It requires jQuery so should be placed after jQuery.

Add bootstrap classes to elements. Class `container` is for sections. `<section class="container">`

Element can belong to several CSS classes. `class="btn btn-success"` means it is a `btn`, but change color to `success`.

[Official Doc](https://getbootstrap.com/docs/4.3/getting-started/introduction/)

- requirements: bootstrap.min.css, jquery-3.3.1.slim.min.js, popper.min.js, bootstrap.min.js.
- Add HTML5 viewport tag in header" `<meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">`
- Put the dochead to avoid issues.

```html
<!doctype html>
<html lang="en">
  ...
</html>
```

Layout

- breakpoint: the size of the device is one of the xs, sm, md, lg and xl. They are breakpoints.
- Default grid system needs `<div class="container">` (resize on every responsive breakpoint) or `<div class="container-fluid">` (always full width).
- Bootstrap use flex box. Attributes of flex box: flex-direction, flex-wrap, flex-flow, justify-content, align-items, align-content, order, flex-grow, flex-shrink, flex-basis, align-self
- Cols are children of Rows. Contents are in Cols.
- `.col-4` uses 1/3 of the 12-grid system. `.col-sm` evenly seperates colmns.
- Five grid (responsive) breakpoints: extra small, small, medium, large, and extra large.
- `.col-sm-4` applies to sm, md, lg, and xl devices, but not xs breakpoint.
- `.col-md-auto` auto adject col width based on content.
- `.w-100` break the col follow by this to another row. There is no padding in between two rows.
- `.px-lg-5` this is for spacing. `p` is padding/`m` is margin. `t` is top/`b` is bottom/`l` left/`r` right/`x` is left and right/`y` is top and bottom.
- `.align-items-start/center/end` can add to container, row and col. Is for vertical alignment.
- `.justify-content-start` is for horizontal alignment.
- `.order-12`, `.order-first` to control when this col appears.
- `.offset-md-4`
- `.ml-md-auto` put margin with auto width.
- In a col, can add a `.row` with the 12-col grid.
- sizing: `.w-25`: 25%. `.w-auto`. `.h-100`. `.mw-100`: max width. `.vw-100`: relative to viewport.
- spacing: `.p` and `.m` , with `t/b/l/r/x/y` define loc, and `0-5/auto` define size.
- `.stretched-link`, the upper level elements are also clickable.
- `.text-justify`, `.text-sm-left`, `.text-wrap`, `.text-break`, `.text-lowercase`, `.text-capitalize`, `.font-weight-bold`, `.font-weight-bolder`, `.font-italic`, `.text-monospace`

Content

- code: `<pre><code></code></pre>` for multi-line codes.
- image: `.img-fluid`, `.img-thumbnail`. Add attr `alt` to show message when img cannot be loaded.
- table: `.table`. `<th>` can have attr `scope=row/col` to determine this header is the header of row/col. Use `<table class="table table-dark">` to select theme. `.table-striped` zebra. `.table-bordered`.
- `.bg-active/primary/secondary/success/danger/warning/info/light/dark` some predefine themes.

Components

- `.alert`, `.alert-link`
- `.badge`. Can be put in a `<span>` element or `<a>` element.
- `.btn`, `.btn-lg`, `.btn-block`. Only add `.active` to force show the button as active.
- `.btn-group`.
- `.card-body`, `.card-title`, `.card-text`, `.card-link`, `.card-img-top`.
- `.list-group`, `.list-group-item`.
- `.carousel`, need a `.active` first.
- `.collapse`
- `.dropdown`
- `.form-group`, `.form-control`, `.form-text`, `.form-check`, `.form-check-input`, `.form-check-label`
- `.input-group`
- `.jumbotron`
- `.list-group`
- `.media`
- `.modal`, `.modal-dialog`
- `.nav`, `.nav-item`, `.nav-link`
- `.navbar`, `.navbar-nav`
- `.pagination`, `.page-item`
- attr `data-toggle="popover"` for btn.
- `.progress`, `.progress-bar`
- attr `data-spy="scroll"` for a div.
- `.spinner-border`, `.sr-only`
- `.toast`
- `.mr-auto`

Utilities

- `.border-top-0`, `.rounded-pill`, `.rounded-lg`
- `.clearfix`
- `.close`
- Colors: `primary/secondary/success/danger/warning/info/light/dark/body/muted/white-50/black`
- `.d-{breakpoint}-{value}`, values: `none/inline/inline-block/block/table/table-cell/table-row/flex/inline-flex`
- `.embed-responsive` around a `<iframe>`.
- `.d-flex`
- `.float-left`, `.float-sm-right`, `.float-none`.
- `.text-hide`
- `.overflow-auto`
- `.position-static`, `.position-relative`, `.position-absolute`, `.position-fixed`, `.position-sticky`. `.fixed-top`, `.sticky-bottom`.
- `.sr-only`: screen reader utils.
- `.shadow-none`, `.shadow-lg`
- `.align-baseline/top/middle/bottom/text-top/text-bottom`
- `.visible`, `.invisible`

An example of using javascript:

```html
<div class="alert alert-warning alert-dismissible fade show" role="alert">
  <strong>Holy guacamole!</strong> You should check in on some of those fields below.
  <button type="button" class="close" data-dismiss="alert" aria-label="Close">
    <span aria-hidden="true">&times;</span>
  </button>
</div>
```

A tool bar with button groups: Notice the `role` attr:

```html
<div class="btn-toolbar" role="toolbar" aria-label="Toolbar with button groups">
  <div class="btn-group mr-2" role="group" aria-label="First group">
    <button type="button" class="btn btn-secondary">1</button>
    <button type="button" class="btn btn-secondary">2</button>
    <button type="button" class="btn btn-secondary">3</button>
    <button type="button" class="btn btn-secondary">4</button>
  </div>
</div>
```

An example of form:

```html
<div class="row">
    <div class="col-md-6 offset-md-3">
        <form method="post">
            <div asp-validation-summary="ModelOnly"></div>
            <div class="form-group">
                <label asp-for="Username">Username</label>
                <input asp-for="Username" class="form-control" />
                <span asp-validation-for="Username" class="text-warning"></span>
            </div>
            <div class="form-group">
                <label asp-for="Password">Password</label>
                <input asp-for="Password" type="password" class="form-control" />
                <span asp-validation-for="Password" class="text-warning"></span>
            </div>
            <div class="form-group">
                <div class="form-check">
                    <input asp-for="RememberMe" type="checkbox" class="form-check-input" />
                    <label asp-for="RememberMe" class="form-check-label">Remember Me?</label>
                </div>
                <span asp-validation-for="RememberMe" class="text-warning"></span>
            </div>
            <div class="form-group">
                <input type="submit" value="Login" class="btn btn-success" />
            </div>
        </form>
    </div>
</div>
```

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

[Font awesome](https://fontawesome.com/how-to-use/on-the-web/referencing-icons/basic-use)

[w3school](https://www.w3schools.com/icons/fontawesome_icons_intro.asp)

[CDN](https://www.bootstrapcdn.com/fontawesome/)

### Creating Entities

Entity Framework Core: compare to Entity Framework, it removes the requirement of Relational DBs.

EF6 is still more mature than EF core. But to use EF6, you need .NET 4.x.

Create `Data` folder for DB interfaces. Create `Data\Entities` for entites, which are shapes of the data.

Every entity class has `Id` as primary key.

Or under `Models` folder create models for entites, and use [DataAnnotations](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=netframework-4.8) to define field restricts.

Use `ICollection<AnotherEntity>` to create a parent-child/one-to-many relationship.

Create class `DutchContext` inherit from `DbContext` for establishing connection. Create `DbSet` in it for entities that need to be queryable.

### Using Entity Framework Core Tooling

Can also use package manager. See "First Controller/View".

Under `dotnet-example\DutchTreat\DutchTreat`, run `dotnet ef database update`.

After add the model, call `dotnet ef migrations add <a migration name>`.

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

**The integrated security should be replaced with actual creds when deploy to prod.**

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

Use [PropertyBuilder](https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.metadata.builders.propertybuilder-1?view=efcore-2.1) to define property restricts.

Create data in `modelBuilder.Entity<Order>().HasData()`. It is embedded into migration for seeding data. After add this line, create a new migration: `dotnet ef migrations add SeedData` and check `Migrations` folder.

Note the PM cmdlet for migration `Add-Migration <Migration Name>` is not for a model, but for all the models and seeding data.

Then need to run `Update-Database` to actually apply the change to DB. Notice the seeding data is also created at this time.

[How to reset](https://stackoverflow.com/questions/38192450/how-to-unapply-a-migration-in-asp-net-core-with-ef-core)

- If want to remove the latest migration: `remove-migration`. But it doesn't work if the migration is applied to DB.
- Reset DB to an old version: `Update-Database <previous-migration-name>`
- If want to reset database totally: `update-database 0`

`HadData` has limitation that it can create only simple entity without relationship.

[A good blog about how to use EF](https://code-maze.com/migrations-and-seed-data-efcore/)

[Entity vs Model](https://stackoverflow.com/questions/8743995/what-is-difference-between-a-model-and-an-entity)

Another way is to create a Seeder class as a service, and start it in Program.

- Create a Seeder Class.
- Inject the DBContext
- Inject `IHostingEnvironment`, where its `ContentRootPath`  is the project root folder.
- Inject `userManager` if want to use NET Core Identity.
- The main method is `SeedAsync`.
- First call `dbContext.Database.EnsureCreated()` before DB operations.
- Then create all seeding data.
- Then in program.cs, using `IServiceScopeFactory` to create a scope, and run the seeder service. (If I put it in the startup, what happens??)

```c#
dbContext.Database.EnsureCreated();
dbContext.Products.AddRange(products);
dbContext.SaveChanges();
```

`host.Services.GetService<DutchSeeder>();` can get services that are set up via Startup. It creates an instance and tries to fullfill all of its dependencies.

`scopeFactory`: during every request this factory creates a scope of the lifetime of the request.

[Scopes](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.0#service-lifetimes)

[Use the NET Core way](https://stackoverflow.com/questions/50785009/how-to-seed-an-admin-user-in-ef-core-2-1-0)

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

HTML tag

- `dl`: description list, outer element
- `dt`: term of a list
- `dd`: description

[With native EF and MVC](https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/crud?view=aspnetcore-3.0)

- Notice EF with Razor Pages are the new suggested way.
- [AsNoTracking](https://docs.microsoft.com/en-us/ef/core/querying/tracking): the update would not be saved and change tracker would not be created.

### Logging errors

In cmd, run `set ASPNETCORE_ENVIRONMENT=Development`

`dotnet run` already print logs.

Inject `ILogger` to DutchRepository, and let logger type to be `DutchRepository`, so that logs log where are they come from.

Can define different log level for defferent namespaces.

Set logging level in config.json.

### Create an API Controller

Use Postman to send request to API.

Web API is a set of endpoints to expose your APIs.

It expose data, which is similar to AppController, which also expose data.

Notice error "Microsoft.AspNetCore.Server.Kestrel[17] Connection id "0HLQ8IO277ERG" bad request data: Invalid request line: \x16\x03\x01\" might caused by SSL. Go to properties/launchSettings.json to confirm the application URL is with HTTPS or not.

Add attribute `Route("api/[Controller]")` to the controller class.

The API has a verb, `Get`. Implement it with Repository.

Run `http://localhost:10120/api/products` to call it.

The Get API can return an IEnumerable. But then if an exception happens, it cannot return properly.

Return Json result can wrap the bad request to a Json object, but it tied the MVC to json.

Return `IActionResult` is the best. Return `Ok` or `BadRequest` wrap with the results.

Can use Swagger to document public APIs.

To use the new document way in .NET Core 2.1, use ControllerBase instead of Controller, and remove `Ok`.

For VS 2017, install [ASP.NET core 2.2 109](https://dotnet.microsoft.com/download/thank-you/dotnet-sdk-2.2.109-windows-x64-installer)

ActionResult returns implicit operator, so that concrete types can be specified and converted. But interfaces cannot.

Add attribute `[ApiController]` to the class.

In startup, `services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);` to opt in the new feature.

### Returning Data

[How to return data that is lazy loading (foreign key)](https://stackoverflow.com/questions/50397105/return-collection-in-asp-net-core-api)

- To make either API or View to show the lazy loaded data, such as the foreign key refered properties stored in another table, use [Include](https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.entityframeworkqueryableextensions.include?view=efcore-3.0)
- An example to return all the parents with their children: `var parents = await _context.Parents.Include(p => p.Children).ToListAsync();`
- Notice to make View works, also need to update the Razor page (cshtml) to show the data.

An example for multiple level foreign data:

```C#
context.Blogs.Include(blog => blog.Posts).ThenInclude(post => post.Tags);
```

Use `this._ctx.Orders.Include(o => o.Items).ThenInclude(i => i.Product).ToList()` to get both Order, and item and product.

Self referencing loop

- OrderItem refer back to Order.
- Set json option to decide how to handle reference loop.
- This is a [JSON option](https://www.newtonsoft.com/json/help/html/ReferenceLoopHandlingIgnore.htm)

Without this, an exception would throw:

```html
<b>fail</b>: Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware[1]<br/>
      An unhandled exception has occurred while executing the request.<br/>
Newtonsoft.Json.JsonSerializationException: Self referencing loop detected for property 'parent' with type 'WebApplicationMVC.Models.AppTestModel'. Path 'children[0]'.
```

```c#
services.AddMvc().AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);`
```

Use `[HttpGet("{id:int}")]` attribute to create a Get method for getting order by id.

The method body is `this._ctx.Orders.Find(id)` if only need get an order, but to get item and product as well, use fluent/LINQ syntax `Where`.

### Implementing POST

Post Order with query string: `http://localhost:17661/api/Orders?OrderDate=2017-5-5` can set the OrderDate? Doesn't seem working.

If not add `[FromBody]` attribute to the input model, the action takes property values (CLR object) from query string? doesn't seems necessary.

Return `Created($"api/orders/{model.Id}", model)` action result for 201.

When call `SaveAll`, the model has been updated with all properties.

### Validation and View Models

The view model can also used to validate APIs.

Use DataAnnotations to add constrains:

- AssociationAttribute, DisplayColumnAttribute
- BindableTypeAttribute
- ConcurrencyCheckAttribute
- CustomValidationAttribute
- DataTypeAttribute
- EditableAttribute
- EmailAddressAttribute, PhoneAttribute
- EnumDataTypeAttribute
- FileExtensionsAttribute
- KeyAttribute
- MaxLengthAttribute, MinLengthAttribute, RangeAttribute
- RegularExpressionAttribute
- RequiredAttribute
- StringLengthAttribute
- TimestampAttribute
- UrlAttribute

Change the Post request to use the view model instead of the real object.

Use `ModelState.IsValid` and `return BadRequest(ModelState);` to do the validation and error handling? Looks like upper level handled it.

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

Without AutoMapper:

- From the child model, to easily set a Parent, make the foreign key ParentID explicit.
- Then use this property to set the value. EF auto set the reference?

### Using Query Strings for APIs

URL is used to describe what of resouces are looking for. Query string can change the behavior.

In the controller, get method, add a bool parameter with default value.

Send the request with URL like `http://localhost:5000/api/orders?includeItems=false`.

[EF Core LINQ](https://docs.microsoft.com/en-us/ef/core/querying/)

EntityFrameworkCore.EntityFrameworkQueryableExtensions

- ToListAsync
- Include, ThenInclude
- SingleAsync: if there are more than 1 in DB, throw.
- SingleOrDefaultAsync: if couldn't find, throw.
- FirstOrDefaultAsync: if there are more than 1, not throw.

`EntityFrameworkCore.DbSet<TEntity>`

- FindAsync

Can also use `from` clause. [basic LINQ query ops](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/basic-linq-query-operations)

### Authorizing Actions

Entities: those are directly store in DB

- Order: A collection of OrderItems. Bind to a StoreUser.
- OrderItem: An instance of a Product.
- Product
- StoreUser: Inherit from IdentityUser.

ViewModels: A layer between Controller and DB.

- ContactViewModel: for contact page.
- LoginViewModel: for login page.
- OrderItemViewModel: for order item page.
- OrderViewModel: for order page.

Controllers

- Account: Inject userManager and signInManager to interact with StoreUser.
- App: for contact and about me page.
- OrderItems: for API.
- Orders: for API.
- Products: for API.

Views

- Account: not in use.
- App: Index, About and Contact pages are in use.
- `Shared/_Layout`: Login and Logout changes the nav-bar.

ClientApp

- checkout
- login
- shop

In the API controller, add the annotation `Authorize` from `Microsoft.AspNetCore.Authorization` to the controller class.

Using Bearer token: `[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]`.

### Storing Identities in the Database

Create an entity inherit IdentityUser.

Derive DutchContext from `IdentityDbContext<StoreUser>`, where `StoreUser` is the user type.

Need migrate by `dotnet ef migrations add Identity`.

Drop the table and rebuild it since there is too much changes: `dotnet ef database drop`.

`await` vs `.Wait()`.

In the seeder, inject UserManager, and use it to create a StoreUser. Notice it is async.

[Config Identity with Auth Token](https://developer.okta.com/blog/2018/03/23/token-authentication-aspnetcore-complete-guide)

- Token AuthN: client attach a token to HTTP requests for the server side to authN.
- If token is missing or invalid, server returns 401
- Used in the context of OAuth 2.0 or OpenID connect.
- In the startup.cs `ConfigureServices` method, after `services.AddIdentity`, add `services.AddAuthentication`.
- The option is `Authority`, `Audience`, `Issuer`, `IssuerSigningKey`
- In the `configure` method, before `UseMvc`, add `app.UseAuthentication();`
- Add the `[Authorize]` attribute on your controllers or routes

```c#
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.Authority = "{yourAuthorizationServerAddress}";
    options.Audience = "{yourAudience}";
});
```

- When JwtBearer middleware handles a request first time, it retrieves some metadata from AuthZ server (also calls Authority or issuer)
- In OpenID Connect terminology those metadata call discovery document.
- They contain public key and other properties for validating the token.
- The audience of a token is the intended recipient of the token. i.e. the Resource Servers that should accept the token, such as `https://contoso.com`.

- The generated token will be signed by authZ server, either use HS256 (symmetric key) or RS256 (asymmetric key).
- Symmetric key, a.k.a. shared secret, is kept on both the authZ server and the application. Like a password, server signs the token, application validate it.
- Asymmetric key is a public+private key pair. Server signs token with private key, and publish the public key to anyone that needs validate the token.

- To generate a token, need to get an authZ server. One server is [IdentityServer4](https://developer.okta.com/blog/2018/03/23/token-authentication-aspnetcore-complete-guide#identityserver4)
- In this example, uses localhost as the authZ server with a random string as the key.
- ValidIssuer is localhost, ValidAudience is users, IssuerSigningKey is a random string.
- In the generated token, there is a claim with user email, name and a random guid. There is also a signingCredentials that is generated based on the key as a SymmetricSecurityKey.

[ASP.NET Core Security](https://docs.microsoft.com/en-us/aspnet/core/security/?view=aspnetcore-3.0)

Common Vulnerabilities

- [Cross site scripting (XSS)](https://docs.microsoft.com/en-us/aspnet/core/security/cross-site-scripting?view=aspnetcore-3.0): an application takes user input and outputs it to a page without validating, encoding and escaping. e.g., attacker writes a blog which contains a script to steal cookie. When visitor opens the blog, the script runs.
- [SQL injection](https://docs.microsoft.com/en-us/ef/core/querying/raw-sql): validate the user input. use parameterization which sends the values separate from the SQL text.
- [Cross-Site Request Forgery (XSRF/CSRF)](https://docs.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-3.0): browsers auto send authN token with every request to a website. The malicious website can use a form which submits POST request in a form to the good website, if the user has already signed in to the good website.
- [Open redirect attacks](https://docs.microsoft.com/en-us/aspnet/core/security/preventing-open-redirects?view=aspnetcore-3.0): when user redirects to login page, append the return url with a malicious website login page, that is identical to the good page.

[ASP NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-3.0)

- The middleware `IAuthenticationService` handled authN.
- authN handlers and options are called "schemes", i.e., a scheme refer to an authN mechanism. Write in `Startup.ConfigureServices`.
- Then call `services.AddAuthentication` with scheme-specific extension methods like `AddJwtBearer` or `AddCookie`. Those methods call `AddScheme` to register schemes with appropriate settings.
- In `Startup.Configure`, call `IApplicationBuilder.UseAuthentication`. It should call before any middleware that depends on users being authN. After `UseRouting`, before `UseEndpoints`.
- When use `Identity`, `AddAuthentication` is called internally.

```c#
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => Configuration.Bind("JwtSettings", options))
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => Configuration.Bind("CookieSettings", options));
```

AuthN Concepts

- An authZ policy can pick multiple schemes to authN the user.
- authentication handler: implement the behavior to authN users.
- Authenticate: construt the user's identity. From request context, it returns a scheme from either cookie or token.
- Challenge: if the user is not authN, either redirect user to login page or return 401 with a `www-authenticate: bearer` header.
- Forbid: an authNed user access resources not permitted, return forbidden cookie or 403 result.

[Introduction to Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-3.0&tabs=visual-studio)

- it is a membership system.
- identity can be stored in a SQL Server DB with username, password and profile data. Azure Table Storage is also supported as a persistent store.
- to secure web APIs and SPAs, use either [AAD](https://docs.microsoft.com/en-us/azure/api-management/api-management-howto-protect-backend-with-aad), AAD B2C or [IdentityServer4](https://identityserver.io/).

[Example project](https://github.com/aspnet/AspNetCore.Docs/tree/master/aspnetcore/security/authentication/identity/sample)

- While creating the web app, select "Individual User Accounts".
- The project creates with folder `Area/Identity/Account` which refers to a Razor class library.
- Need call `Update-Database` to let it take effect.
- In `Views\Shared\` there is a `_LoginPartial.cshtml` which appears in `_Layout.cshtml` in the nav bar.
- Username: zhenying@webapp.com, zhenying2@webapp.com, Password: `P@ssw0rd`
- Several tables are created: AspNetUsers, AspNetRoles, AspNetUserLogins, AspNetUserRoles, AspNetUserTokens
- Now users can register and be added to DB.
- Check `services.AddDefaultIdentity()` in `ConfigureServices`, should already be there.
- To config identity (such as password, lockout and username policy), `services.Configure<IdentityOptions>(options => ...)`
- Same for `services.ConfigureApplicationCookie()`
- In the `Configure`, `app.UseAuthentication();` should be after `app.UseRouting();` but before `app.UseAuthorization();`.

- Create identity Scaffold files can change the login views. But note if the build doesn't pass it won't work!
- `Views\Shared\_LoginPartial.cshtml` refers to `asp-area` Identity and pages under `Areas\Identity\Pages\Account\`. Those are razor pages.
- Each of those pages has a cshtml and a cs file. The cs file defines a `InputModel` for input values validation, and `OnGetAsync`, `OnPostAsync` for render views.
- The bind property is an `InputModel`, which is the model that defines all the allowed input values.
- The property `IList<AuthenticationScheme> ExternalLogins` is the schemes.
- The property `ErrorMessage` is annotated as `TempData`, so that it can be store. It is displayed in `OnGetAsync`.
- Since most of the display contents are already written in the cshtml, `OnGetAsync` is mainly for external login contents.
- `OnGetAsync` first clean up the cookie `HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);`, then get all schemes `_signInManager.GetExternalAuthenticationSchemesAsync()`.
- `OnPostAsync` calls `_signInManager.PasswordSignInAsync(...)` to pass the input to sign in.
- `Register` page uses email sender. Need set it up in the startup.cs

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

Create a Controller, `AccountController`, that is not using EF for the Login and Logout.

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

Add `[Authorize]` to controller (those are API controllers) classes. When sending a request to the API before get authed, the response returns 302 with redirect URL. It is auth with cookie.

- For normal MVC controllers, each action/request returns either a `ViewResult` or a `StatusCodeResult` which can be rendered to a page. If the user is not authed, then it redirect to the login page.
- For API controllers, each action returns either a `ActionResult<T>` or a `StatusCodeResult`. But if the user is not authed, it returns a login page as well, but in html format. So the request should send with the cookie grab from the web browser.

Replace `[Authorize]` with `[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]`, so that when user is not authed, instead of returning 302, returns 401.

See GenerateEncodedToken

Using postman, adding a header with `Authorization` key and `Bearer` value.

Create a REST call (i.e. it is not resolve to a view), `CreateToken`, in Account Controller. It is a POST.

`SignInManager.PasswordSignInAsync` is actually using a cookie.

Inject UserManager to the account controller, so that we can get a user and call `SignManager.CheckSignInAsync`.

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

### Use Identity in Read Operations

Since OrdersController uses auth, can get the user in all its APIs.

APIs can be used by angular.

### Use Identity in Write Operations

User.Identity is just a list of claims. To get the user object, need use UserManager.

In the OrdersController, add the logic to Post method.

### First TypeScript Class

Angular requires TypeScript.

Transpiling in visual studio 2017.

Create ts folder under wwwroot.

In JavaScript, everything is a class.

In TypeScript, properties can be specified a type, like `public visits:number = 0`. The type safety check happens when compiling. There is no running time check.

A function:

```typescript
    public showName(name:string):boolean {
        alert(name);
    }
```

In TypeScript: `this` is required for fields.

```typescript
    private ourName: string;
    set name(val) {
        this.ourName = val;
    }
    get name() {
        return this.ourName;
    }
```

Constructor can have private arguments, which is auto create and wired to private properties.

```typescript
    constructor(private firstName: string, private lastName: string) {
    }
    // No need to define firstName and lastName but they are private properties.
```

The class need to be exported so others can use it.

```typescript
export class StoreCustomer {
    ...
}
```

To use it, need import the type. But TypeScript only import classes instead of namespaces.

```typescript
import { StoreCustomer, OtherThing } from "./StoreCustomer";

let shopper = new StoreCustomer("Zhenying", "Zhu");
shopper.showName();
```

Import and export works well when using a loader, such as WebPack and Browserify. But if not using a loader, then the typescript is used as a javascript. Need specify the loading order of them.

### Compiling TypeScript

Compile = transpile.

Add a tsconfig.json to the project root.

`"include": [ "wwwroot/ts/**/*.ts" ]`. Double star means search all sub folders.

Add `compilerOptions`.

- Target is the ECMAScript version. Target es5 because Accessors are not available in es3.
- sourceMap: create map file for debugging.

### Debugging in the Browser

Add a scripts sections in Shop view, and drag the js files to it.

In the browser debugger, the typescripts are showing up because the map files.

### What is Angular

It is successor to AngularJS.

It is an Open Source JavaScript Framework. It supplies basic app services. It can be used to build a Single Page Application.

Angular is build with TypeScript.

### Installing the Angular-CLI

Use npm to install Angular-CLI. npm is distributed with Node.js.

`npm -g` install tools globally. CLI tools are better to be installed globally, such as TypeScript, Gulp, Grunt.

To install angular cli: `npm install -g @angular/cli`.

To check the CLI version: `ng --version`. `ng -h` list help.

### Generate your first project

`ng new dutch-app --dry-run --skip-git --inline-template --inline-style --skip-tests`

Note: NOT use route. It will generate app/app-routing.module.ts, which is different fron the tutorial.

Scaffolding: write the definition, and compiler of the framework generate code.

Skip tests is actually just skip UTs. Test scaffolding files are still generated.

Under dutch-app folder, run `ng build`. It uses Webpack to package files. It packs 5 files

- our code is in main.js file.
- vendor.js contains all angular stuff.
- styples.js is css.
- runtime.js includes some plumbing.
- polyfills.js is for old browsers compatibility.

`ng serve` start the app without running backend. It runs `src\app\app.component.ts`.

- It uses `Component` decorator.
- The class sets 3 attributes: selector, template, styles.
- selector defines the name of the elment used in the body in index.html.

### Copying the Project

We don't want the client code lives in dutch-app folder.

- Move angular.json and replace tsconfig.json under the solution root.
- Create ClientApp folder under the solution root.
- Move all files under dutch-app/src to ClientApp folder.
- Merge package.json from the one in the dutch-app to the one in the solution.
- Then the dutch-app folder can be deleted. The node_modules sub folder has a lot of files already and they should be merged to the solution by visual studio.
- add `"exclude": ["./node_modules/"]` to tsconfig.json so that TypeScript won't compile them.
- Replace the outDir to `"outDir": "./wwwroot/ClientApp/out-tsc",`
- angular.json, change root and sourceRoot valut to `ClientApp`.
- `"outputPath": "wwwroot/clientapp/dist",`. This is where JS files are created.
- replace all `src/` with `ClientApp/`

After moved and updated, under DutchTraet\DutchTreat, run `npm install` to reintall packages.

### Integrating the Project

Use the client app to build shop page.

`ng build` creates the dist folder.

Include wwwroot/ClientApp/dist scripts in order:

1. runtime.js
1. polyfills.js
1. styles.js
1. vendor.js
1. main.js

In the ClientApp/app/app.component.ts, rename the selector to the-shop.

In Shop.cshtml, add the-shop element.

### Using External Templates

Can use HTML and View as external template to replace the default template in app.component.ts.

Replace `template` with `templateUrl: "./app.component.html",`

Create the html under ClientApp\app.

In the template, use `{{}}` to do data binding. It can bind to the property or the method of the corresponding typescript class.

Need to do a build to make the change takes affect. Can run `ng build --watch` to let the build keep running with only compiling the changed part.

The HTML is not retrived on the server. It is embedded in the angular app as a resource.

### Your First Anuglar Component

Getting data directly from the API instead of generate a view.

Create `app\shop\productList.component.ts` under `ClientApp` for the class to represent the lists of products.

Need add `import { Component } from '@angular/core';` to import the Component key word.

Then we can use `@Component` as an decorator. It is like C# attribute of a class.

Define templateUrl as productList.component.html.

Create the html. Use directive. The directive are attributes that can be applied to elements.

Use directive `*ngFor` to do a for loop. `<li *ngFor="let p of products">{{ p.title }}</li>`. `let` here is the javascript keyword,

In `app.module.ts` import the new class `import { ProductList } from "./shop/productlist.component";` and add to the NgModel.declarations. It makes each part of the html a composition.

The data binding can use pipe: `{{ p.price | currency:"USD":true }}` [currency](https://angular.io/api/common/CurrencyPipe) is a decorator.

Pipes specify what the data type is. It is Model-View-ViewModel.

### Creating a Service

Create a shared\dataService.ts file. It is used to share data.

In the productList.omponent.ts, inject the data from dataService. Create a ctor of the ProductList `constructor(private data: DataService)`. This also set the private member of the class.

Also include the service in the app.module. Add to providers instead of declarations.

### Calling the API

Inject HttpClient into dataService, and add the HttpClientModule to the app.module.ts under imports.

Need decorate the DataService class to make the injection chain (Since it is inejected to the ProductList class) knows it also has its own dependencies.

Add a loadProduct function. It calls http.get. The `subscribe()` method is where the request is send, and it returns the result when receives the response.

To make some changes before return to the customer, need use some interceptors by putting them into method `pipe()`. Call rxjs operatior `map`.

To let client (ProductList) call the loadProduct, let it implments OnInit.

### Using Type Safety

In shared folder, create product.ts. This is the expected data contract from the server.

To create the contract, get a real product object from thhe API response, and copy to json2ts.com. Copy the generated typescript back.

In the productList.component.ts, set the type for products.

In dataService.ts, add `loadProducts(): Observable<boolean>`.

rxjs: reactive extensions for JS.

Type safety is part of compile environment.

### Implementing a Template

Create the template productList.component.html by copy the product view from index.html. Notice the reference folder is changed, so the image url need to change to root.

When dealing with an entire attribute, can use `[alt]="p.title"`. e.g.: . It is a write only binding.

We can use component css as well. Use `styleUrls` in productList.component.ts.

### Creating another component

Create the cart.component.ts + html.

Add it to app.component.html.

Add the cart to app.model.ts.

### Sharing Data Across Components

Create order.ts under shared as an interface.

Get the order use postman and use the json2ts to create interfaces of Order and OrderItem.

Change the interface to class. Cannot create an instance of an interface in TS.

Add default value to orderDate.

Convert `[]` array to use `Array.

In dataService.ts, create a method `addToOrder`.

Try import all the classes inside order.ts as OrderNS: `import * as OrderNS from './order';`.

In productList.component.html, make the buyButton work by adding `(click)="addProduct(p)"`. The parentheses means it is a callback.

`()` is readonly, and `[]` is writeonly.

In productList.component.ts, create the `addProduct()` method.

### Building the cart

Add a table to cart.component.html.

Table is bad for general layout. But here we use if for table.

In dataService.addToOrder, add the logic to merge existing items in the cart.

### Using calculated data

Calculated subtotal of the cart on FE instead of BE.

In order.ts, add a property `subtotal()` as a number.

Add `lodash` to package.json dependencies and order.ts. It can do map reduce. It is LINQ for JS.

Use `_.sum(_.map(arr, i => some logic))`.

### Add Routing to the Project

Import routerModel in app.module.ts. In the imports, add `RouterModule.forRoot()`.

They are inter-page routes.

Add routes array. Each route is a path and a component.

The config for RouteModule

- `forChild` allows nested routing.
- `userHash` add a hash tag to URL. It can enhance single page?

Create shop/shop.component.ts + html. Move all content from app.component.html to shop.component.html because app.component.html is just a place holder. It only needs router-outlet element, so that the content could be changed with different components.

Create checkout/checkout.component.ts + css + html.

Import Checkout and Shop to app.module.ts.

In cart.component.html, add `a` element with `routerLink` attr.

Navigate between routes won't loss state, because dataService stores them on client.

### Support Login

In dataService.ts, add properties token, tokenExpiration, and loginRequired.

We want to make it not required to login to shop, but need login before checkout. So remove the auth of AppController.Shop.

In cart.component.html, make the checkout gated, so if login, can checkout, otherwise go to login page.

Angular: `{}` inject value, `[]` evaluate and put value in the form into attribute of the class, `()` to mark it as an event handler.

Make the checkout button handle click event with `onCheckout` method.

Use `router.navigate` to route to login page.

Create Login comonent.

### Use Form Binding

In the login.component.ts, use a public property `creds` to store user input.

Add `FormsModel` to `app.module.ts`.

In the form of login.component.html, use `ngModel`. Use both `[]` and `()` because it both push values to the form and handle event as a call back. It is two way binding.

Updatet the username in the callback should affect the value in the text box.

Add `submit` event to the form and handle it by `onLogin`.

### Add Validation

In login.component.html, use the `required` validation that build in in browsers.

Use Angular to validate if the constrain is met. `#` can make angular bond the element to an object. Add `#username="ngModel"` to the form input, and use `*ngIf="username.invalid"` to control if a error message appears.

Add `#theForm="ngForm"` with `theForm.invalid` as well.

Add `novalidate` to the form, so that browser won't validate itself except the validation we specified.

### Use Token Authentication

In dataService.ts, add a method `login` which returns `Observable<boolean>`, to call AccountController.CreateToken.

In login.component.ts, call `login` and `subscribe` to the result to do routing logic.

In login.component.html, show `errorMessage` if not empty.

### Implement Server-side Checkout

In dataService.ts, implement a `checkout()` method.

In checkout.component.ts, let `onCheckoout()` call `dataService.checkout()`.

Put error message to checkout.component.html.

In `dataService.checkout()`, need set headers of the post to include token. The third parameter of the post should be

```javascript
this.http.post("api/orders", this.order, {
    headers: new HttpHeaders().set("Authorization", "Bearer " + this.token)
})
```

The client side validation might pass, but the server side validation can still fail.

Check Network XHR response, can see the `OrderNumber` is missing. So add it in the `checkout()`.

Fail again because in server side log, Cannot update `Product` while update `Order`.

In `DutchRepository`, create a method `AddOrder()`, which first look up `Product`. Let `OrdersController` call this method.

### Minifying your JavaScript

In the package.json, add `gulp`, `gulp-uglify` and `gulp-concat` to devDependencies, which are used when build project.

Gulp automate build time tooling using js. It takes javascripts, concat into a large file, uglify (minify) it.

Add gulp config file `gulpfile.js` to the root of the project.

Create tasks and group tasks. First task is `minify`.

`gulp.src("wwwroot/js/**/*.js")` gets all the subfolders js files. `gulp.dest("wwwroot/dist")` save it.

In a cmd, run `gulp`. It runs the default task. Or run `gulp minify`.

In VS 2017, right click gulpfile.js, start Task Runner Explorer, and bind task to after build.

### Environment Tag Helpers

In the `Views\Shared\_Layout.cshtml`, set `environment` element to define envs.

Use minified js in the staging and prod envs.

The `script` element can have an attr `asp-append-version`. When set it to true, each build will have different minify script name, so that client doesn't need to clear their cache to use the new script.

For common lib, use `asp-fallback-src` to define my local path, and use CDN if client has already load it for other websites.

Use envs for CSS as well.

In the project properties, Debug, can change `ASPNETCORE_ENVIRONMENT` to change envs.

### Setting up Deployment Scripts

In visual studio, can publish a project onto a remote server. Since the build process will be different, need set up deployment envs.

In the csproj file, add `Target` element. It can let MSBuild do something during the build.

```xml
  <Target Name="MyPublishScripts" BeforeTargets="BeforePublish">
    <Exec Command="npm install" />
    <Exec Command="gulp" />
    <Exec Command="ng build" />
    <Exec Command="dir" />
  </Target>
```

`Exec` can run console commands. Notice order matters.

### Publishing to a Directory

Publish to `bin\Debug\netcoreapp2.2\publish\`.

In the folder, the `DutchTreat.dll` is the starting point.

Use `dotnet DutchTreat.dll` can start the server.

Weirdly the `npm install` didn't run with just publish. I might missed some thing.

Looks like I hit this [issue](https://github.com/dotnet/cli/issues/4062)

But add this

```xml
<ItemGroup>
  <Content Include="node_modules\foo\**" CopyToPublishDirectory="PreserveNewest" />
</ItemGroup>
```

Can cause build failure.

### Publishing to Azure

1. Publish
2. New Profile
3. Creating new
4. Pick the right Azure subscription
5. Create a Resource Group `DutchTreatSite`

A App Service Plan is created.

In visual studio 2017's Web Publish Activity tab, can see the details.

It runs on the azurewebsites.net with HTTPS.

[Build an ASP.NET Core and SQL Database app](https://docs.microsoft.com/en-us/azure/app-service/app-service-web-tutorial-dotnetcore-sqldb)

- To run an developped repo locally:

```powershell
dotnet restore
dotnet ef database update
dotnet run
```

- Start an [Azure powershell](https://shell.azure.com/)
- `az group create --name myResourceGroup --location "West US"`
- `az sql server create --name <server_name> --resource-group myResourceGroup --location "West US" --admin-user <db_username> --admin-password <db_password>`

# HERE

https://docs.microsoft.com/en-us/azure/app-service/app-service-web-tutorial-dotnetcore-sqldb#configure-a-server-firewall-rule

### Publishing to IIS

In IIS, create a new Website. Change the port to 81 because Default Website take 80 already.

Change the Application pool to Core, so CLR is not needed.

In the `C:\inetpub`, create a new folder `dutchtreat`.

Restart VS in admin mode to connect to IIS.

Choose `Web Deploy`, Server is localhost, Site name is dutchtreat, URL is `http://localhost:81`.

### Publishing Using the CLI

`dotnet build`, `dotnet run`, `dotnet publish`.

```cmd
dotnet publish -o C:\Downloads\pub
```

It uses MSBuild which is a build engine.

Does `node_modules` required?? The course example doesn't seem to have it. Check:

<https://stackoverflow.com/questions/37935524/how-to-use-npm-with-asp-net-core>

### Publishing with Runtime

ASP.NET Core has Runtime Identifiers(RID).

In DutchTreat.csproj, add `<RuntimeIdentifier>win10-x64</RuntimeIdentifier>` for TargetFramework.

Use `dotnet publish -o <PATH> --self-contained`. It will generate DutchTreat.exe. Run `DutchTreat` will start the server.

To support multiple platform, add `<RuntimeIdentifiers>win10-x64,OSX.10.10-x64</RuntimeIdentifiers>` instead.

Use `dotnet publish -o <PATH> --runtime osx.10.10-x64`.

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

## ASP.NET Core 3.0

<https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-3.0>

## Other notes

### Entity Framework(EF)

<https://docs.microsoft.com/en-us/aspnet/entity-framework>

`dotnet ef migrations --help` can see the options.

Use `[Column(TypeName = "decimal(18,2)")]` before a property to define its restrict.

`modelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(18, 2);` should work samely, but this API is not found.

### RESTful

<https://www.tutorialspoint.com/restful/index.htm>

### WCF

Service oriented: <https://docs.microsoft.com/en-us/dotnet/framework/wcf/whats-wcf>

### LocalDB

[MSSQLLOCALDB databases aren't listed](https://stackoverflow.com/questions/34029337/mssqllocaldb-databases-arent-listed)
