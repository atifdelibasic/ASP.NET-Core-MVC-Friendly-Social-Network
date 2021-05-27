let spinnerGrow = `<div class="text-primary text-center">
                        <div class="spinner-grow text-primary" role="status">
                        <span class="sr-only">Loading...</span>
                       </div>
                        <div>Loading</div>
                        </div>`;

var loaderSmall = `<div class="d-flex justify-content-center m-1">
                  <div class="spinner-border spinner-border-sm" role="status">
                 </div>
                 </div>`;

function ConnectBtnLoad(userId) {
    var url = "/UserProfile/FriendConnect/" + userId;
    $("#" + userId).html(loaderSmall);
    $.ajax({
        type: 'GET',
        url: url,
        success: function (data) {
            $("#" + userId).html(data);
        }
    })
}

function AjaxPost(form, url, id = null) {
    $.ajax({
        type: 'POST',
        url: url,
        data: form.serialize(),
        success: function (data) {
            if (id != null) {
                $("#" + id).html(data);
            }
        }
    });
}

function Toggle(a) {
    console.log("#respondRequest " + a);
    var b = document.getElementById('respondRequest ' + a);
    $(b).toggle();
}

function Cancel(userId) {
    var url = "/UserProfile/CancelRequest";
    var form = $("#form" + userId);
    AjaxPost(form, url, userId);
}

function addFriend(userId, element) {
    element.innerHTML = loaderSmall;

    // Send friend request
    var url = "/UserProfile/AddFriend";
    var form = $("#form" + userId);
    AjaxPost(form, url, userId);

    // Send push notification to user
    var url2 = "/UserProfile/Testiranje";
    var form2 = $("#form" + userId);
    AjaxPost(form2, url2);
}

function ConfirmRequest(userId) {
    var url = "/UserProfile/EstablishConnection";
    var form = $("#form" + userId);
    AjaxPost(form, url, userId);

    var url2 = "/UserProfile/Testiranje";
    var form2 = $("#form" + userId);
    AjaxPost(form2, url2);
}

function RespondRequest() {
    var menu = document.querySelector("#menu");
    menu.classList.toggle('toggle-display');
}

function EditPost(id) {

    $.ajax({
        type:'GET',
        url: '/Feed/GetPost/' + id,
        success: function (data) {
            let postId = document.getElementById('PostId');
            let postText = document.getElementById('PostText');
            let hobbiesCat = document.getElementById('hobbiesCat');
            let hobbies = document.getElementById('hobbies');
            postId.value = data.id;
            postText.value = data.text;
            hobbiesCat.value = data.hobby.hobbyCategoryId;
            LoadHobbies(hobbiesCat.value, data.hobbyId);
            //hobbies.value = data.hobbyId;
        }
    });

}


function TogglePostMenu(id) {

    let postMenu = $("#postMenu" + id);
    postMenu.toggle();


}

function DeletePost(id) {
    if (id == 0 || id == null)
        return;

    let url = '/Feed/DeletePost/' + id;
    if (confirm('Are you sure to delete this post?') == true) {

        $.ajax({
            type: 'POST',
            url: url,
            success: function (res) {
                if (res.id != undefined) {
                    RemovePostFromUI(res.id);
                }
            }
        });
    }
}

function RemovePostFromUI(id) {
    let post = $("#post" + id);
    post.remove();
}

