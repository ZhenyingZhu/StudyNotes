function displayDate() {
    document.getElementById("my-date").innerHTML = Date();
}

$(document).ready(function () {
    console.log("jQuery is running");

    $("#my-button2").on("click", function () {
        $("#my-date").text("The date is?");
    });
});

