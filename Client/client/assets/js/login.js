// Toggle between Sign In and Sign Up forms
const signInButton = document.querySelector("#signInButton");
const signUpButton = document.querySelector("#signUpButton");
const signInForm = document.querySelector("#logInForm");
const signUpForm = document.querySelector("#signUpForm");

if (signInButton && signUpButton && signInForm && signUpForm) {
  signInButton.addEventListener("click", () => {
    signInForm.style.display = "block";
    signUpForm.style.display = "none";
  });

  signUpButton.addEventListener("click", () => {
    signUpForm.style.display = "block";
    signInForm.style.display = "none";
  });
}

$(document).ready(function () {
  const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
  const passwordPattern = /^.{3,}$/;

  $("#logInForm").submit(function (event) {
    if (
      emailPattern.test($("#email2TB").val()) &&
      passwordPattern.test($("#password2TB").val())
    ) {
      event.preventDefault(); // Prevent default form submission
      Login($("#email2TB").val(), $("#password2TB").val());
    } else {
      alert(
        "ERROR: Password shorter than 3 characters or invalid email format"
      );
    }
  });

  $("#signUpForm").submit(function (event) {
    if (
      emailPattern.test($("#emailTB").val()) &&
      passwordPattern.test($("#passwordTB").val())
    ) {
      event.preventDefault(); // Prevent default form submission

      SignUp(
        $("#nameTB").val(),
        $("#emailTB").val(),
        $("#passwordTB").val(),
        uploadedImage
      );
    } else {
      alert(
        "ERROR: Password shorter than 3 characters or invalid email format"
      );
    }
  });
});

function ajaxCall(method, api, data, successCB, errorCB) {
  $.ajax({
    type: method,
    url: api,
    data: data,
    cache: false,
    contentType: "application/json",
    dataType: "json",
    success: successCB,
    error: errorCB,
  });
}

function Login(email, password) {
  let api = "https://proj.ruppin.ac.il/cgroup75/test2/tar6/api/User/Login";
  ajaxCall(
    "GET",
    api,
    { email: email, password: password },
    LoginSCB,
    LoginECB
  );
}

function LoginSCB(user) {
  if (!user || user.id === 0) {
    alert("ERROR: Password or email mismatch");
  } else {
    var userJson = JSON.stringify(user);
    alert("Welcome " + user.name);
    localStorage.setItem("loggedIn", true);
    localStorage.setItem("user", userJson);
    $("#loginStatus").text("logged in as " + user.name); // Update login status on the UI
    window.location.href = "index.html";
  }
}

function LoginECB() {
  alert("ERROR: login unsuccessful");
  localStorage.setItem("loggedIn", false);
  localStorage.removeItem("user");
}

function SignUp(name, email, password, profilePic) {
  if (profilePic == null) {
    profilePic =
      "https://upload.wikimedia.org/wikipedia/commons/7/7c/Profile_avatar_placeholder_large.png";
  }
  let api = "https://proj.ruppin.ac.il/cgroup75/test2/tar6/api/User/Register";
  let newUser = {
    name: name,
    email: email,
    password: password,
    profilePic: profilePic,
    registrationDate: "",
  };
  ajaxCall("POST", api, JSON.stringify(newUser), SignUPSCB, SignUPECB);
}

function SignUPSCB(status) {
  if (status == -1) {
    alert("There's already a user signed up with this email");
    return;
  }
  alert("Sign-Up completed successfully");
  let email = $("#emailTB").val();
  let password = $("#passwordTB").val();
  Login(email, password);
}

function SignUPECB(err) {
  alert("ERROR: " + err.responseText || "An error occurred during sign-up.");
}

// Global variables to store the image data and folder path
var uploadedImage;
var imageFolder = "https://proj.ruppin.ac.il/cgroup75/test2/tar6/Images/";

$(document).ready(function () {
  $("#buttonUpload").on("click", function () {
    var data = new FormData();
    var files = $("#files").get(0).files;

    // Add the uploaded file to the form data collection
    if (files.length > 0) {
      for (f = 0; f < files.length; f++) {
        data.append("files", files[f]);
      }
    }

    var api = "https://proj.ruppin.ac.il/cgroup75/test2/tar6/api/Upload";

    // Ajax upload
    $.ajax({
      type: "POST",
      url: api,
      contentType: false,
      processData: false,
      data: data,
      success: showImages,
      error: error,
    });

    return false;
  });
});

function showImages(data) {
  var bttn = document.getElementById("buttonUpload");
  bttn.style.display = "none";
  var imgStr = "";

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
  document.getElementById("ph").innerHTML = imgStr;
}

// Now you can access uploadedImages anywhere in your script

function error(data) {
  console.log(data);
}
