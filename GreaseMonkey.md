# Userscript

## Turtorial

[Applying JavaScript: User Scripts](https://medium.freecodecamp.com/applying-javascript-user-scripts-2e505643644d)

Add jQuery
```
// @require http://code.jquery.com/jquery-latest.js
```

Change css:
- `.athing` pick up all `<tr class='athing' /tr>`
- `:odd` pick the elements one skip by one

```
$(document).ready(function() {
  $('.athing:odd').css('background-color', '#DDD');
});
```

### Pending
http://hayageek.com/greasemonkey-tutorial/#install-greasemonkey

https://hibbard.eu/tampermonkey-tutorial/

https://github.com/OpenUserJs/OpenUserJS.org/wiki/Userscript-Beginners-HOWTO

## Example

### Steam Trading Cards Bulk Buyer
[src](https://bitbucket.org/Doctor_McKay/steam-trading-card-bulk-buyer/raw/tip/badgebuy.user.js)

Header contains match and require.
```
// ==UserScript==
// @match			*://steamcommunity.com/*/gamecards/*
// @require			https://ajax.googleapis.com/ajax/libs/jquery/2.0.3/jquery.min.js
// ==/UserScript==
```

[XHR](https://en.wikipedia.org/wiki/XMLHttpRequest)

Send creds. More [info](https://stackoverflow.com/questions/2054316/sending-credentials-with-cross-domain-posts):
```
$.ajaxSetup({
	xhrFields: {
		withCredentials: true
	}
});

```

To click a button:
```
$('.badge_craft_button').click();
```

Where the button is
```
<div class="badge_craft_button" onclick="Profile_CraftGameBadge();" />
```

Reload the page after 100 ms
```
setTimeout(function() {
    window.location.reload();
}, 100);
```

## InfoQ Grapper
The last page:
http://www.infoq.com/cn/cloud-computing/news/1675

Test here: https://www.w3schools.com/jsref/tryit.asp?filename=tryjsref_document_getelementsbyclassname
```
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
    var output = document.getElementsByClassName("example");
    
    var header2 = document.getElementsByTagName('h2');
    var atts = header2.attributes;
    var result = "1";
    for (var i = 0; i < 4; i++)
    {
    	result += "1";
    }

    output[0].innerHTML = result;
}
</script>

</body>
</html>
```