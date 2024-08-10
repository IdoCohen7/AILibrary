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
            alert("ERROR: Password shorter than 4 characters or invalid email format");
        }
    });

    $("#signUpForm").submit(function (event) {
        if (
            emailPattern.test($("#emailTB").val()) &&
            passwordPattern.test($("#passwordTB").val())
        ) {
            event.preventDefault(); // Prevent default form submission
            SignUp($("#nameTB").val(), $("#emailTB").val(), $("#passwordTB").val());
        } else {
            alert("ERROR: Password shorter than 4 characters or invalid email format");
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
    let api = "https://localhost:7063/api/User/Login";
    ajaxCall(
        "POST",
        api,
        JSON.stringify({ email: email, password: password }),
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

function SignUp(name, email, password) {
    let api = "https://localhost:7063/api/User/Register";
    let newUser = { name: name, email: email, password: password };
    ajaxCall("POST", api, JSON.stringify(newUser), SignUPSCB, SignUPECB);
}

function SignUPSCB(status) {
    alert("Sign-Up completed successfully");
    let email = $("#emailTB").val();
    let password = $("#passwordTB").val();
    Login(email, password);
}

function SignUPECB(err) {
    alert(err.responseText || "An error occurred during sign-up.");
}
