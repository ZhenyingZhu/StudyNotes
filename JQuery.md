# JQuery

## w3school Turtorial
[src](https://www.w3schools.com/jquery/)

[test](https://www.w3schools.com/jquery/tryit.asp?filename=tryjquery_hide)

Include jQuery from a CDN (Content Delivery Network)
```
<head>
<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
</head>
```

Syntax: `$(selector).action()`
- tailor-made for selecting HTML elements and performing some action on the elements
- uses CSS syntax to select elements
- `$(this).hide()`: hides the current element.
- `$("p").hide()`: hides all `<p>` elements.
- `$(".test").hide()`: hides all elements with `class="test"`.
- `$("#test").hide()`: hides the element with `id="test"`.

document ready event is to prevent any jQuery code from running before the document is finished loading
```
$(document).ready(function(){
   ...
});
```
or
```
$(function(){
    ...
});
```

[Selector](https://www.w3schools.com/jquery/jquery_selectors.asp)
- `$("*")`: all elements
- `$("p.intro")`: Selects all `<p>` elements with `class="intro"`
- `$("p:first")`
- `$("ul li:first")`
- `$("ul li:first-child")`
- `$("[href]")`: select element with attribute
- `$("a[target='_blank']")`, `$("a[target!='_blank']")`
- `$(":button")`: Selects all `<button>` elements and `<input>` elements of `type="button"`
- `$("tr:even")`, `$("tr:odd")`

[Event Methods](https://www.w3schools.com/jquery/jquery_events.asp)

DOM events
- Mouse Events
  - click
  - dblclick
  - mouseenter
  - mouseleave
  - hover: takes two functions
- Keyboard Events
  - keypress
  - keydown
  - keyup
- Form Events
  - submit
  - change
  - focus
  - blur: when leave the field
- Document and Window Events
  - load
  - resize
  - scroll
  - unload
  - ready

Click a paragh element makes it hide
```
$("p").click(function(){
    $(this).hide();
});
```

hover takes two functions
```
$("#p1").hover(function(){
    alert("You entered p1!");
},
function(){
    alert("Bye! You now leave p1!");
});
```

For elements
```
Name: <input type="text" name="fullname"><br>
Email: <input type="text" name="email">
```
Change color
```
$(document).ready(function(){
    $("input").focus(function(){
        $(this).css("background-color", "#cccccc");
    });
    $("input").blur(function(){
        $(this).css("background-color", "#ffffff");
    });
});
```

Attach multiple event handlers to a `<p>` element using `on()`:
```
$("p").on({
    mouseenter: function(){
        $(this).css("background-color", "lightgray");
    }, 
    mouseleave: function(){
        $(this).css("background-color", "lightblue");
    }, 
    click: function(){
        $(this).css("background-color", "yellow");
    } 
});
```

[jQuery Effects](https://www.w3schools.com/jquery/jquery_hide_show.asp)
- Syntax `$(selector).hide(speed,callback);`, where speed can be `"slow"`, `"fast"` and milliseconds
- `hide`, `show`, `toggle`
- `fadeIn()`, `fadeOut()`, `fadeToggle()`, `fadeTo()`
- `slideDown()`, `slideUp()`, `slideToggle()`
- `$(selector).animate({params},speed,callback);`. [More details](https://www.w3schools.com/jquery/jquery_animate.asp)
- `$(selector).stop(stopAll,goToEnd);` stop animate

Callback: A callback function is executed after the current effect is 100% finished.

Without callback:
```
$("button").click(function(){
    $("p").hide(1000);
    alert("The paragraph is now hidden"); # before hide complete the alert shows
});
```

With callback:
```
$("button").click(function(){
    $("p").hide("slow", function(){
        alert("The paragraph is now hidden");
    });
});
```

Method Chaining
```
$("#p1").css("color", "red").slideUp(2000).slideDown(2000);
```

[jQuery HTML](https://www.w3schools.com/jquery/jquery_dom_get.asp)
- `text()`: Sets or returns the text content of selected elements
- `html()`: Sets or returns the content of selected elements (including HTML markup)
- `val()`: Sets or returns the value of form fields
- `attr("href")`

return
```
$("#btn1").click(function(){
    alert("Text: " + $("#test").text());
});
```

set first 3:
```
$("#test1").text("Hello world!");
```

`text()`, `html()`, and `val()` comes with Callback functions that has 2 args: index and old value

```
$("#btn1").click(function(){
    $("p").text(function(i, origText){
        return "Old text: " + origText + " New text: Hello world!
        (index: " + i + ")"; 
    });
});
```
Can change
```
<p>paragh 1</p>
<p>paragh 2</p>
```
to
```
<p>Old text: paragh 1 New text: Hello world! index: 0</p>
<p>Old text: paragh 2 New text: Hello world! index: 1</p>
```

set attr
```
$("#w3s").attr({
    "href" : "https://www.w3schools.com/jquery",
    "title" : "W3Schools jQuery Tutorial"
});
```

Add New HTML Content
- `append()`: Inserts content at the end of the selected elements
- `prepend()`: Inserts content at the beginning of the selected elements
- `after()`: Inserts content after the selected elements
- `before()`: Inserts content before the selected elements

Append can append any number of any var
```
function appendText() {
    var txt1 = "<p>Text.</p>";               // Create element with HTML  
    var txt2 = $("<p></p>").text("Text.");   // Create with jQuery
    var txt3 = document.createElement("p");  // Create with DOM
    txt3.innerHTML = "Text.";
    $("body").append(txt1, txt2, txt3);      // Append the new elements 
}
```

Remove Elements
- `remove()`: Removes the selected element (and its child elements)
- `empty()`: Removes the child elements from the selected element

Accept filter
```
$("p").remove(".test, .demo");
```

[Get and Set CSS Classes](https://www.w3schools.com/jquery/jquery_css_classes.asp)
- `addClass()`: Adds one or more classes to the selected elements
- `removeClass()`: Removes one or more classes from the selected elements
- `toggleClass()`: Toggles between adding/removing classes from the selected elements
- `css()`: Sets or returns the style attribute

CSS style
```
<style>
.important {
    font-weight: bold;
    font-size: xx-large;
}

.blue {
    color: blue;
}
</style>

```
Add to elements
```
$(document).ready(function(){
    $("button").click(function(){
        $("h1, h2, p").addClass("blue");
        $("div").addClass("important blue");
    });
});
```

Return a CSS Property of the first matched element
```
$("p").css("background-color");
```

Set of all matched elements
```
$("p").css({"background-color": "yellow", "font-size": "200%"});
```

[jQuery Dimension Methods](https://www.w3schools.com/jquery/jquery_dimensions.asp)
- `width()`
- `height()`
- `innerWidth()`
- `innerHeight()`
- `outerWidth()`
- `outerHeight()`

For css
```
<style>
#div1 {
    height: 100px;
    width: 300px;
    padding: 10px;
    margin: 3px;
    border: 1px solid blue;
    background-color: lightblue;
}
</style>

<div id="div1"></div>
<button>Display dimensions of div</button>
```
Display height in the div
```
$(document).ready(function(){
    $("button").click(function(){
        var txt = "";
        txt += "Width of div: " + $("#div1").width() + "</br>";
        $("#div1").html(txt);
    });
});
```


HTML document: `$(document).width()`

browser viewport: `$(window).height()`

[Traversing](https://www.w3schools.com/jquery/jquery_traversing.asp)

Traversing a DOM tree. move up (ancestors), down (descendants) and sideways (siblings)

Traverse Up:
- `parent()`
- `parents()`: all the way up to the document's root element `<html>`
- `parentsUntil()`: `$("span").parentsUntil("div");`

Filter
```
$(document).ready(function(){
    $("span").parents("ul").css({"color": "red", "border": "2px solid red"});
});
```

Traverse Down:
- `children()`: single level down
- `find()`: returns descendant elements of the selected element, all the way down to the last descendant. All: `$("div").find("*");`

Filter
```
$(document).ready(function(){
    $("div").children("p.first");
});
```

Traversing Sideways
- `siblings()`: all sibling elements of the selected element, not include itself.
- `next()`
- `nextAll()`
- `nextUntil()`: not include the target.
- `prev()`
- `prevAll()`
- `prevUntil()`

Filtering
- `first()`: `$("div").first();` the first `<div>`
- `last()`
- `eq()`: specify index number. `$("p").eq(1);`
- `filter()`: `$("p").filter(".intro");` select all `<p class="intro">`
- `not()`: `$("p").not(".intro");`

[jQuery AJAX](https://www.w3schools.com/jquery/jquery_ajax_intro.asp)

With the jQuery AJAX methods, you can request text, HTML, XML, or JSON from a remote server using both HTTP Get and HTTP Post. And you can load the external data directly into the selected HTML elements of your web page!

Because different browsers have different syntax for AJAX implementation. This means that you will have to write extra code to test for different browsers. However, the jQuery team has taken care of this for us.

AJAX `load()` Method
- Syntax: `$(selector).load(URL,data,callback);`
- `$("#div1").load("demo_test.txt #p1");` load the element with id `p1` in this URL
- Callback: 
  - `responseTxt`: contains the resulting content if the call succeeds
  - `statusTxt`: contains the status of the call
  - `xhr`: contains the XMLHttpRequest object

Get the status from load method
```
$("button").click(function(){
    $("#div1").load("demo_test.txt", function(responseTxt, statusTxt, xhr){
        if(statusTxt == "success")
            alert("External content loaded successfully!");
        if(statusTxt == "error")
            alert("Error: " + xhr.status + ": " + xhr.statusText);
    });
});
```

GET and POST
- the POST method NEVER caches data, and is often used to send data along with the request.
- GET Syntax: `$.get(URL,callback);`
- POST Syntax: `$.post(URL,data,callback);`


GET
```
$("button").click(function(){
    $.get("demo_test.asp", function(data, status){
        alert("Data: " + data + "\nStatus: " + status);
    });
});
```

POST
```
$("button").click(function(){
    $.post("demo_test_post.asp",
    {
        name: "Donald Duck",
        city: "Duckburg"
    },
    function(data, status){
        alert("Data: " + data + "\nStatus: " + status);
    });
});
```

[The `noConflict()` Method](https://www.w3schools.com/jquery/jquery_noconflict.asp)

If two different frameworks are using the same shortcut, one of them might stop working. The `noConflict()` method releases the hold on the $ shortcut identifier, so that other scripts can use it.

