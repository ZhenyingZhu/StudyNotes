# JQuery

## Resource

<https://www.w3schools.com/jquery/default.asp>

## w3school Turtorial

[src1](https://www.w3schools.com/jquery/default.asp); 
[src2](http://www.w3school.com.cn/js/js_library_jquery.asp)

jQuery:

- is a lightweight, "write less, do more", JavaScript library.
- contains the following features:
  - HTML/DOM manipulation
  - CSS manipulation
  - HTML event methods
  - Effects and animations
  - AJAX
  - Utilities

JavaScript框架：库，高级应用程序设计。特别针对浏览器差异处理。常用的有jQuery、Prototype、MooTools。

To use jQuery:

- can download `jquery-3.3.1.min.js` from [jQuery.com](http://jquery.com/download/) and add to the webpage as a script.
- or include jQuery from a CDN (Content Delivery Network). If the client already download it while read another webpage, it is in the cache.

```html
<head>
<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
</head>
<script>
mycode
...
</script>
```

CDN (Content Delivery Network)把框架库存放在一个通用的位置供每个网页分享就变得很有意义了。

使用前需要引用jQuery 库。

Syntax: `$(selector).action()`:

- tailor-made for selecting HTML elements and performing some action on the elements
- uses CSS syntax to select elements
- `$(this).hide()`: hides the current element.
- `$("p").hide()`: hides all `<p>` elements.
- `$(".test").hide()`: hides all elements with `class="test"`.
- `$("#test").hide()`: hides the element with `id="test"`.

```javascript
function myFunction() {
  $("#h01").html("Hello jQuery"); // element id "h01", innerHTML set to "Hello jQuery"
}

$(document).ready(myFunction); // onload
```

document ready event is to prevent any jQuery code from running before the document is finished loading

```javascript
$(document).ready(function(){
   ...
});
```

or a shorter form:

```javascript
$(function(){
    ...
});
```

测试 JavaScript 框架库:

- 主要的jQuery 函数是`$()` 函数(jQuery 函数)。
- 如果您向该函数传递DOM 对象，它会返回jQuery 对象，带有向其添加的jQuery 功能。
- 可以通过CSS 选择器来选取元素
- jQuery 函数返回jQuery 对象。

DOM 对象$(document) 传递到jQuery。返回的jQuery对象有ready 方法。

[Selector](https://www.w3schools.com/jquery/jquery_selectors.asp):

- `$(this)`
- `$("#id")`: id should be unique within a page.
- `$("*")`: all elements
- `$("p.intro")`: Selects all `<p>` elements with `class="intro"`
- `$("p:first")`, `$("tr:even")`, `$("tr:odd")`
- `$("ul li:first")`: Selects the first `<li>` element of the first `<ul>`.
- `$("ul li:first-child")`: Selects the first `<li>` element of every `<ul>`.
- `$("[href]")`: select element with attribute.
- `$("a[target='_blank']")`, `$("a[target!='_blank']")`
- `$(":button")`: Selects all `<button>` elements and `<input>` elements of `type="button"`

[Other selectors](https://www.w3schools.com/jquery/jquery_ref_selectors.asp) [Tester](https://www.w3schools.com/jquery/trysel.asp)

[Event Methods](https://www.w3schools.com/jquery/jquery_events.asp)

Event:

- jQuery is tailor-made to respond to events in an HTML page.
- All the different visitors' actions that a web page can respond to are called events.
- An event represents the precise moment when something happens.
- Most DOM events have an equivalent jQuery method.
- Syntax: `$(<element selector>).<event>(<action>)`.

JQuery event methods:

- Mouse Events
  - click
  - dblclick
  - mouseenter
  - mouseleave
  - hover: equals to `mouseenter()` and `mouseleave()`, so it can take two functions.
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

[All events](https://www.w3schools.com/jquery/jquery_ref_events.asp)

Click a paragh element makes it hide:

```javascript
$("p").click(function(){
    $(this).hide();
});
```

hover takes two functions:

```javascript
$("#p1").hover(function(){
    alert("You entered p1!");
},
function(){
    alert("Bye! You now leave p1!");
});
```

For elements:

```javascript
Name: <input type="text" name="fullname"><br>
Email: <input type="text" name="email">
```

Change color: Notice it is a nested jQuery action.

```javascript
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

```javascript
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

[jQuery Effects](https://www.w3schools.com/jquery/jquery_hide_show.asp):

- Syntax `$(selector).hide(speed,callback);`, where speed can be `"slow"`, `"fast"` and milliseconds
- `hide`, `show`, `toggle`
- `fadeIn()`, `fadeOut()`, `fadeToggle()`, `fadeTo()`
- `slideDown()`, `slideUp()`, `slideToggle()`
- `$(selector).animate({params},speed,callback);`. [More details](https://www.w3schools.com/jquery/jquery_animate.asp)
- `$(selector).stop(stopAll,goToEnd);` stop animate

Callback: A callback function is executed after the current effect is 100% finished.

Without callback:

```javascript
$("button").click(function(){
    $("p").hide(1000);
    alert("The paragraph is now hidden"); # before hide complete the alert shows
});
```

With callback:

```javascript
$("button").click(function(){
    $("p").hide("slow", function(){
        alert("The paragraph is now hidden");
    });
});
```

[All effects](https://www.w3schools.com/jquery/jquery_ref_effects.asp)

[Animate: Skipped](https://www.w3schools.com/jquery/jquery_animate.asp)

[Stop: Skipped](https://www.w3schools.com/jquery/jquery_stop.asp)

[Callback](https://www.w3schools.com/jquery/jquery_callback.asp)

Syntax: `$(selector).hide(speed,callback);` Where the callback is a function.

[Chaining](https://www.w3schools.com/jquery/jquery_chaining.asp)

Run multiple jQuery methods (on the same element) within a single statement, so that the same element doesn't need to be find by the brower again.

Method Chaining:

```javascript
$("#p1").css("color", "red")
    .slideUp(2000)
    .slideDown(2000);
```

[jQuery HTML Get](https://www.w3schools.com/jquery/jquery_dom_get.asp):

- `text()`: Sets or returns the text content of selected elements
- `html()`: Sets or returns the content of selected elements (including HTML markup)
- `val()`: Sets or returns the value of form fields
- `attr("href")`

return:

```javascript
$("#btn1").click(function(){
    alert("Text: " + $("#test").text());
});
```

[jQuery HTML Set](https://www.w3schools.com/jquery/jquery_dom_set.asp):

set first text, html and val:

```javascript
$("#test1").text("Hello world!");
```

`text()`, `html()`, and `val()` comes with Callback functions that has 2 args: index and old value.

```javascript
$("#btn1").click(function(){
    $("p").text(function(i, origText){
        return "Old text: " + origText + " New text: Hello world! (index: " + i + ")";
    });
});
```

Index is the index of the element in the selected element list.

Can change:

```html
<p>paragh 1</p>
<p>paragh 2</p>
```

to:

```html
<p>Old text: paragh 1 New text: Hello world! index: 0</p>
<p>Old text: paragh 2 New text: Hello world! index: 1</p>
```

set attr:

```javascript
$("#w3s").attr("href", "https://www.w3schools.com/jquery");

$("#w3s").attr({
    "href" : "https://www.w3schools.com/jquery",
    "title" : "W3Schools jQuery Tutorial"
});
```

Callback:

```javascript
$("button").click(function(){
    $("#w3s").attr("href", function(i, origValue){
        return origValue + "/jquery/";
    });
});
```

[jQuery HTML Add](https://www.w3schools.com/jquery/jquery_dom_add.asp):

- `append()`: Inserts content at the end of the selected elements
- `prepend()`: Inserts content at the beginning of the selected elements
- `after()`: Inserts content after the selected elements
- `before()`: Inserts content before the selected elements

Append can append any number of any var:

```javascript
function appendText() {
    var txt1 = "<p>Text.</p>";               // Create element with HTML
    var txt2 = $("<p></p>").text("Text.");   // Create with jQuery
    var txt3 = document.createElement("p");  // Create with DOM
    txt3.innerHTML = "Text.";
    $("body").append(txt1, txt2, txt3);      // Append the new elements
}
```

JQuery create element: `$("<html element tag></tag>").text()`

`after` and `before` add the text outside the selected element, while `append` and `prepend` add inside the element.

[jQuery HTML Remove](https://www.w3schools.com/jquery/jquery_dom_remove.asp):

Remove Elements:

- `remove()`: Removes the selected element (and its child elements)
- `empty()`: Removes the child elements from the selected element

Accept filter: it means remove all the "p" elements that has class "test" and "demo".

```javascript
$("p").remove(".test, .demo");
```

[Get and Set CSS Classes](https://www.w3schools.com/jquery/jquery_css_classes.asp):

Classes are used to define css styles for elements.

- `addClass()`: Adds one or more classes to the selected elements
- `removeClass()`: Removes one or more classes from the selected elements
- `toggleClass()`: Toggles between adding/removing classes from the selected elements
- `css()`: Sets or returns the style attribute

CSS style:

```html
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

Add to elements:

```javascript
$(document).ready(function(){
    $("button").click(function(){
        $("h1, h2, p").addClass("blue");
        $("div").addClass("important blue");
    });
});
```

# HERE https://www.w3schools.com/jquery/jquery_css.asp

Return a CSS Property of the first matched element:

```javascript
$("p").css("background-color");
```

Set of all matched elements:

```javascript
$("p").css({"background-color": "yellow", "font-size": "200%"});
```

[jQuery Dimension Methods](https://www.w3schools.com/jquery/jquery_dimensions.asp):

- `width()`
- `height()`
- `innerWidth()`
- `innerHeight()`
- `outerWidth()`
- `outerHeight()`

For css:

```html
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

Display height in the div:

```javascript
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

```javascript
$(document).ready(function(){
    $("span").parents("ul").css({"color": "red", "border": "2px solid red"});
});
```

Traverse Down:

- `children()`: single level down
- `find()`: returns descendant elements of the selected element, all the way down to the last descendant. All: `$("div").find("*");`

Filter:

```javascript
$(document).ready(function(){
    $("div").children("p.first");
});
```

Traversing Sideways:

- `siblings()`: all sibling elements of the selected element, not include itself.
- `next()`
- `nextAll()`
- `nextUntil()`: not include the target.
- `prev()`
- `prevAll()`
- `prevUntil()`

Filtering:

- `first()`: `$("div").first();` the first `<div>`
- `last()`
- `eq()`: specify index number. `$("p").eq(1);`
- `filter()`: `$("p").filter(".intro");` select all `<p class="intro">`
- `not()`: `$("p").not(".intro");`

[jQuery AJAX](https://www.w3schools.com/jquery/jquery_ajax_intro.asp)

With the jQuery AJAX methods, you can request text, HTML, XML, or JSON from a remote server using both HTTP Get and HTTP Post. And you can load the external data directly into the selected HTML elements of your web page!

Because different browsers have different syntax for AJAX implementation. This means that you will have to write extra code to test for different browsers. However, the jQuery team has taken care of this for us.

AJAX `load()` Method:

- Syntax: `$(selector).load(URL,data,callback);`
- `$("#div1").load("demo_test.txt #p1");` load the element with id `p1` in this URL
- Callback:
  - `responseTxt`: contains the resulting content if the call succeeds
  - `statusTxt`: contains the status of the call
  - `xhr`: contains the XMLHttpRequest object

Get the status from load method:

```javascript
$("button").click(function(){
    $("#div1").load("demo_test.txt", function(responseTxt, statusTxt, xhr){
        if(statusTxt == "success")
            alert("External content loaded successfully!");
        if(statusTxt == "error")
            alert("Error: " + xhr.status + ": " + xhr.statusText);
    });
});
```

GET and POST:

- the POST method NEVER caches data, and is often used to send data along with the request.
- GET Syntax: `$.get(URL,callback);`
- POST Syntax: `$.post(URL,data,callback);`

GET:

```javascript
$("button").click(function(){
    $.get("demo_test.asp", function(data, status){
        alert("Data: " + data + "\nStatus: " + status);
    });
});
```

POST:

```javascript
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
