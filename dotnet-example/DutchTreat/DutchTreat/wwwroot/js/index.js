var x = 0;
var s = "";

console.log("Hello world");

var theForm = document.getElementById("theForm");
if (theForm != null) {
    theForm.hidden = true;
}

// $("#theForm").show(); // opposite to hide.

var button = document.getElementById("buyButton");
// same as $("#buyButton").on("click", function (){...})
if (button != null) {
    button.addEventListener("click", function () {
        console.log("Buying item");

        //var productInfo = document.getElementsByClassName("product-props");
        //var listItems = productInfo.item(0).children;
        //console.log(listItems);
    });
}

var productInfo = $(".product-props li");
productInfo.on("click", function () {
    console.log("You clicked on " + $(this).text());
})

$(document).ready(function () {
    var $loginToggle = $("#loginToggle");
    var $popupForm = $(".popup-form");

    $loginToggle.on("click", function () {
        $popupForm.slideToggle(500);
    });
});