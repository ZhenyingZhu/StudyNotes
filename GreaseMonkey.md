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


