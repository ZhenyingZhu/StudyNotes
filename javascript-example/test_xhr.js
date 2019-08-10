function get(url) {
    return new Promise(function(resolve, reject) {
        var req = new XMLHttpRequest();
        req.open('GET', url);

        req.onload = function() {
            if (req.status == 200) {
                resolve(req.response);
            } else {
                reject(Error(req.statusText));
            }
        };

        req.onerror = function() {
            reject(Error('Network Error'));
        };
        
        req.send();
    });
}

get('story.json').then(function(response) {
    console.log("Success!", response);
    var obj = JSON.parse(response);
    document.getElementById("demo").innerHTML = obj.title;
}, function(error) {
    console.error("Failed!", error);
});