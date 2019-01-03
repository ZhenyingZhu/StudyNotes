// ==UserScript==
// @name         Steam Wishlist Parser
// @namespace    https://github.com/shenjihehe/steamdiscount-/blob/master/steamwhlistspider.py
// @version      0.1
// @description  The original script is poorly written. Trying to convert it into a tampermonkey script. Note Steam already support it.
// @author       shenjihehe
// @match        https://store.steampowered.com/wishlist/*
// @grant        None
// ==/UserScript==

debugger;

(function() {
    'use strict';
})();
// It doesn't work. The issue is the the page load after the script executes.
window.onload(function exec() {
    var textMessage = "",
        appId;
    var info = $J(".wishlist_row:has(.discount_pct)");

    info.filter(function() {
        appId = $(this).getAttribute('data-app-id');
        textMessage += appId;
    });

    alert(textMessage);
})();
