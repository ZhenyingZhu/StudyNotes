function t(taskId) {
	return new Promise(function(resolve) {
		setTimeout(function() {
			console.log("Running task " + taskId);
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

var id = 0;
var promise = t(id);

setTimeout(function() {
	console.log("outside id " + id);
	id += 1;
}, 500);

promise.then(function(res) {
	setTimeout(function() {
		console.log(res);
		id += 1;
		//return t(id);
		console.log("First then returns.");
	}, 750);
	return id;
}).then(function(res) {
	console.log(res);
	id++;
//	return t(id);
});