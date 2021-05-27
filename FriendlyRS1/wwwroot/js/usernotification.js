"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/NotificationUserHub?userId=" + userId).build();
connection.on("sendToUser", (actorName, notificationMessage, userId, profileImage) => {

    if (profileImage == null) {
        profileImage = "/avatar.png";
    } else {
        profileImage = "data:image/*;base64," + profileImage;
    }

    var alertNotification = `<div class="alert alert-light alert-dismissible fade show shadow p-3 not mb-2" id="alobee">
                <div class="d-flex justify-content-between">
                    <h5 class="p-0">New notification</h5>
                    <button class="close" data-dismiss="alert">×</button>
                </div>
                <hr/>
                <div onclick="openProfile(${userId})" class="m-0 d-flex p-0 align-items-center">
                   <img class="rounded-circle border-dark" style="height:56px; width:56px" id="ItemPreview" src="${profileImage}">
                   <div class="pl-2"> ${actorName + " " + notificationMessage} </div>
                </div>
                </div>`;

    var num = +$('#notif-count').attr("data-count");
    if ($("#mainNotif").is(":hidden")) {
        num = num + 1;
    }
    $('#notif-count').attr('data-count', num);

    document.getElementById("notifications").innerHTML += alertNotification;

    setTimeout(function () {
        $(".alert").alert('close');
    }, 10000);

    let bell = `<div class="d-flex align-items-center py-2 not">
                    <img class="rounded-circle border-dark notif-img" style="height:56px; width:56px" id="ItemPreview" src="${profileImage}">
                <div class="d-flex flex-column pl-2">
                    <div class="messagge">
                        <span class="font-weight-bold">${ actorName }</span>
                        <span>${ notificationMessage }</span>
                    </div>
                <div class="pt-1 time">Few seconds ago</div>
                </div>
                </div >`;

    // append notification to the bell notifications list 
    if ($("#mainNotif").is(":visible")) {
        $("#notifDiv").prepend(bell);
    }
});


function openProfile(id) {
    if (id != null) {
        let url = '/UserProfile/Index/' + id;
        window.location.replace(url);
    } else {
        console.log('Cannot load profile, user id is undefined or null');
    }
}

function GetNotification(id) {
    let url = '/Feed/GetNotification/' + id;

    $.ajax({
        url: url,
        success: function (d) {
            console.log(d);
        }
    });
}

connection.start().catch(function (err) {
    return console.error(err.toString());
}).then(function () {
    connection.invoke('GetConnectionId');
});