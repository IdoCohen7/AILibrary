$(document).ready(function () {
  if (user != null) {
    LoadPage();
  } else {
    if (
      confirm(
        "You must log in to view this page, shall we take you to the login page?"
      )
    ) {
      window.location.href = "01_login-in.html";
    }
    let header = document.createElement("h3");
    header.innerHTML = "There's no content here for guest users";
    let div = document.getElementById("mainContainer");
    div.innerHTML = "";
    div.appendChild(header);
  }
});

function LoadPage() {
  // fill details
  let profilePic = document.getElementById("profilePic");
  profilePic.src = user.profilePic;
  let header = document.getElementById("userHeader");
  header.classList.add("uk-text-lead");
  header.innerHTML = user.name + "'s Account";
  let emailDiv = document.getElementById("emailDiv");
  emailDiv.innerHTML = user.email;
  let statusDiv = document.getElementById("statusDiv");
  if (user.isActive == true) {
    statusDiv.innerHTML = "Activated";
  } else {
    statusDiv.innerHTML = "Banned";
  }
  let dateDiv = document.getElementById("dateDiv");
  dateDiv.innerHTML = user.registrationDate;

  // give functions

  let passwordBttn = document.getElementById("changePasswordBttn");
  passwordBttn.addEventListener("click", function () {
    UIkit.modal("#modal-password").show();
  });

  let nameBttn = document.getElementById("changeNameBttn");
  nameBttn.addEventListener("click", function () {
    UIkit.modal("#modal-username").show();
  });

  document
    .getElementById("changePasswordForm")
    .addEventListener("submit", function (event) {
      event.preventDefault();
      let oldPassword = document.getElementById("old-password");
      let newPassword = document.getElementById("new-password");
      let confirmPassword = document.getElementById("confirm-password");
      const passwordPattern = /^.{3,}$/;

      if (oldPassword.value == user.password) {
        if (passwordPattern.test(newPassword.value)) {
          if (newPassword.value == confirmPassword.value) {
            ChangePassword(user.id, newPassword.value);
          } else {
            alert("You didn't repeat the new password correctly");
          }
        } else {
          alert("password needs to be than 2 characters!");
        }
      } else {
        alert("Old password is not correct");
      }
    });

  document
    .getElementById("changeUsernameForm")
    .addEventListener("submit", function (event) {
      event.preventDefault();
      let newUsername = document.getElementById("new-username");
      if (newUsername.value == user.name) {
        alert("This is already your username");
      } else {
        ChangeUsername(user.id, newUsername.value);
      }
    });
}

function ChangePassword(userId, newPassword) {
  let api =
    "https://proj.ruppin.ac.il/cgroup75/test2/tar6/api/User/ChangePassword?userId=" +
    userId +
    "&password=" +
    newPassword;
  ajaxCall("PUT", api, null, ChangePasswordSCB, AjaxECB);
}

function ChangePasswordSCB(message) {
  if (message == 1) {
    alert("Password changed successfuly, please log in once again");
    Logout();
  } else {
    alert("Unable to change password");
  }
}

function ChangeUsername(userId, newName) {
  let api =
    "https://proj.ruppin.ac.il/cgroup75/test2/tar6/api/User/ChangeUsername?userId=" +
    userId +
    "&name= " +
    newName;
  ajaxCall("PUT", api, null, ChangeUsernameSCB, AjaxECB);
}

function ChangeUsernameSCB(message) {
  if (message == 1) {
    alert("Username changed successfuly, please reconnect");
    Logout();
  } else {
    alert("Unable to change username");
  }
}

$(document).ready(function () {
  // Show file input dialog when profile picture is clicked
  $("#changePicture").on("click", function () {
    $("#fileInput").click();
  });

  // Handle file selection and upload
  $("#fileInput").on("change", function () {
    var files = $(this).get(0).files;
    if (files.length > 0) {
      var data = new FormData();
      for (var f = 0; f < files.length; f++) {
        data.append("files", files[f]);
      }

      var api = "https://proj.ruppin.ac.il/cgroup75/test2/tar6/api/Upload";

      // Ajax upload
      $.ajax({
        type: "POST",
        url: api,
        contentType: false,
        processData: false,
        data: data,
        success: ChangeProfilePicture,
        error: error,
      });
    }
  });
});

function showImages(data) {
  var imgStr = "";
  var imageFolder = "https://localhost:7063/Images/"; // Set the path to your image folder

  if (Array.isArray(data)) {
    for (var i = 0; i < data.length; i++) {
      var src = imageFolder + data[i];
      imgStr += `<img src='${src}' style="width: 80px;"/>`;
      uploadedImage = src;
    }
  } else {
    var src = imageFolder + data;
    imgStr = `<img src='${src}'/>`;
  }

  // Update profile picture
  $("#profilePic").attr("src", imgStr);
}

function error(data) {
  console.log(data);
}

var userNewPicture;

function ChangeProfilePicture(data) {
  var imageFolder = "https://localhost:7063/Images/";
  let src = imageFolder + data;
  userNewPicture = src;
  let api =
    "https://proj.ruppin.ac.il/cgroup75/test2/tar6/api/User/ChangeProfilePicture?userId=" +
    user.id +
    "&picture=" +
    src;
  ajaxCall("PUT", api, null, ChangeProfilePictureSCB, AjaxECB);
}

function ChangeProfilePictureSCB(message) {
  if (message == 1) {
    let userJsonString = localStorage.getItem("user");
    if (userJsonString) {
      let userObject = JSON.parse(userJsonString);

      userObject.profilePic = userNewPicture;

      let updatedUserJsonString = JSON.stringify(userObject);

      localStorage.setItem("user", updatedUserJsonString);
    }
    window.location.reload();
  }
}
