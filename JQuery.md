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

[HERE](https://www.w3schools.com/jquery/jquery_events.asp) jQuery Syntax For Event Methods