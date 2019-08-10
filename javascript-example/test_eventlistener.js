//debugger;

var img1 = document.querySelector(".img-1");

img1.addEventListener('error', function() {
    console.log("image1 failed to load.")
    return false;
});

img1.addEventListener('load', function() {
    console.log("image1 loaded.")
});

function testImage(url) {
    //var tester = new Image();
    console.log("Update image url to " + url);
    img1.src = url;
}

setTimeout(testImage, 1000, "image1.jpg");
setTimeout(testImage, 2000, "image2.jpg");
setTimeout(testImage, 3000, "image1.jpg");