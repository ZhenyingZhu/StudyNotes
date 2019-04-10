function t1() {
	return new Promise(function(resolve) {
		setTimeout(function() {
			console.log("Running task1.");
			resolve("Task done.");
		}, 1000);
	});
}

function t2() {
	return new Promise(function(resolve) {
		setTimeout(function() {
			console.log("Running task2.");
			resolve("Task done.");
		}, 1000);
	});
}

var promise = t1();
promise.then(function(res) {
	console.log(res);
	return t2();
}).then(function(res) {
	console.log(res);
});