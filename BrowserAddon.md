# Userscript

Tampermonkey for chrome and Greasemonkey for firefox.

## Resources

[Tempermonkey document](https://tampermonkey.net/documentation.php)

[DOM reference](https://devdocs.io/dom/)

## Turtorial

[Tempermonkey welcome](https://tampermonkey.net/index.php?version=4.7&ext=dhdg&updated=true)

Tempermonkey icon > Dashboard > Setting > Config mode: change to Advenced would show much more settings.

[Applying JavaScript: User Scripts](https://medium.freecodecamp.com/applying-javascript-user-scripts-2e505643644d)

Add jQuery header

```javascript
// ==UserScript==
// @require https://code.jquery.com/jquery-latest.js
// ==/UserScript==
```

Pop a message box

```javascript
alert('WINNING');
```

Target is to change the background color of the Hacker News links.

Using debug tool, found links html is

```html
<tr class="athing" id="18321884">
  <td align="right" valign="top" class="title"><span class="rank">1.</span></td>
  <td valign="top" class="votelinks">
    <center><a id="up_18321884" href="vote?id=18321884&amp;how=up&amp;goto=news"><div class="votearrow" title="upvote"></div></a></center>
  </td>
  <td class="title">
    <a href="https://www.redhat.com/en/blog/red-hat-ibm-creating-leading-hybrid-cloud-provider" class="storylink">IBM acquires Red Hat</a>
    <span class="sitebit comhead"> (<a href="from?site=redhat.com"><span class="sitestr">redhat.com</span></a>)</span>
  </td>
</tr>
```

To change the css

- `.athing` pick up all `<tr class='athing' /tr>`
- `:odd` pick the elements one skip by one

```javascript
// ==UserScript==
// @name         Zhenying Test
// @namespace    http://tampermonkey.net/
// @version      0.1
// @description  Learning Applying JavaScript: User Scripts
// @author       You
// @match        https://news.ycombinator.com/
// @grant        none
// @require      https://code.jquery.com/jquery-latest.js
// ==/UserScript==

var $ = window.jQuery;

$(document).ready(function() {
    'use strict';

    // Your code here...
    $('.athing:odd').css('background-color', '#DDD');
})();
```

### Pending

http://hayageek.com/greasemonkey-tutorial/#install-greasemonkey

https://hibbard.eu/tampermonkey-tutorial/

https://github.com/OpenUserJs/OpenUserJS.org/wiki/Userscript-Beginners-HOWTO

## Debugger

<https://www.w3schools.com/js/js_debugging.asp>

```javascript
console.log(c);
```

Adding `debugger;` on the first line.

## Example

### Steam Trading Cards Bulk Buyer

[src](https://bitbucket.org/Doctor_McKay/steam-trading-card-bulk-buyer/raw/tip/badgebuy.user.js)

Header contains match and require.

```javascript
// ==UserScript==
// @match *://steamcommunity.com/*/gamecards/*
// @require https://ajax.googleapis.com/ajax/libs/jquery/2.0.3/jquery.min.js
// ==/UserScript==
```

[XHR](https://en.wikipedia.org/wiki/XMLHttpRequest)

Send creds. More [info](https://stackoverflow.com/questions/2054316/sending-credentials-with-cross-domain-posts):

```javascript
$.ajaxSetup({
  xhrFields: {
    withCredentials: true
  }
});

```

To click a button:

```javascript
$('.badge_craft_button').click();
```

Where the button is:

```html
<div class="badge_craft_button" onclick="Profile_CraftGameBadge();" />
```

Reload the page after 100 ms:

```javascript
setTimeout(function() {
    window.location.reload();
}, 100);
```

## InfoQ Grapper

The last page:

<http://www.infoq.com/cn/cloud-computing/news/1675>

Test here: <https://www.w3schools.com/jsref/tryit.asp?filename=tryjsref_document_getelementsbyclassname>

```html
<!DOCTYPE html>
<html>
<body>

<div class="example">First div element with class="example".</div>

<div class="news_type_block">
  <span class="followable primaryTopic-grid">
    <a href="/"></a>
      <span>
        <span class="follow__what"></span>
        <span class="followers" style="display: inline;"> 他的粉丝</span>
      </span>
  </span>
  <h2>
  <a href="/cn/news/2007/07/amazon-arch-example" title="用Amazon Web Service实现视频文件转换程序">用Amazon Web Service实现视频文件转换程序</a>
  </h2>
  <span class="author">作者
    <span class="authors-list">
      <span class="followable">
        <a class="editorlink f_taxonomyEditor" style="color: #286ab2" href="/cn/profile/Gavin-Terrill">Gavin Terrill</a>
        <span>
          <script type="text/javascript">window['isFollowedUser101764189']= false;</script>
        </span>
      </span>
    </span>
  </span>
</div>

<button onclick="myFunction()">Try it</button>

<p><strong>Note:</strong> The getElementsByClassName() method is not supported in Internet Explorer 8 and earlier versions.</p>

<script>
function myFunction() {
    var output = document.getElementsByClassName("news_type_block");
    var header2 = output[0].getElementsByTagName('h2')[0];
    var link = header2.getElementsByTagName('a')[0].href;
    console.log(link);
    window.open(link,"_self");
}
</script>

</body>
</html>
```

Maybe use <http://www.netinstructions.com/how-to-make-a-simple-web-crawler-in-javascript-and-node-js/>