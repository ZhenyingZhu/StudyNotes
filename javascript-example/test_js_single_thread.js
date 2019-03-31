debugger;

var cnt = 0;

function increaseCount() {
	cnt += 1;
	console.log(cnt);
	document.getElementById("demo").innerHTML = cnt;
}

(function () {
	for (var i = 0; i < 5; i++) {
		setInterval(increaseCount, 1000);
	}
})()
