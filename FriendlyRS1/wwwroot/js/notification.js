"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/NotificationHub").build();
connection.on("sendToUser", (articleHeading, articleContent) => {
    //var heading = document.createElement("h3");
    //heading.textContent = articleHeading;
    //var p = document.createElement("p");
    //p.innerText = articleContent;

    //var div = document.createElement("div");
    //div.appendChild(heading);
    //div.appendChild(p);
    //alert();

    //document.getElementById("articleList").appendChild(div);
});
connection.start(function () {
}).catch(function (err) {
    return console.error(err.toString());
});
