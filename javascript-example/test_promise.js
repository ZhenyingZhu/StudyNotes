var img1 = document.querySelector('.img-1');

function loaded() {
  console.log("image loaded");
}

if (img1.complete) {
  loaded();
}
else {
  img1.addEventListener('load', loaded);
}

img1.addEventListener('error', function() {
  console.log("image failed");
});