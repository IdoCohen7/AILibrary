jQuery(document).ready(function ($) {
  if (user != null) {
    LoadRequests();
  } else {
    if (
      confirm(
        "You must log in to view this page, shall we take you to the login page?"
      )
    ) {
      window.location.href = "01_login-in.html";
    }
  }
});

function LoadRequests() {
  let api =
    "https://proj.ruppin.ac.il/cgroup75/test2/tar1/api/User/GetPending?userId=" +
    user.id;
  ajaxCall("GET", api, null, LoadRequestsSCB, AjaxECB);
}

function LoadRequestsSCB(allRequests) {
  let container = document.getElementById("requestContainer");

  if (allRequests.length == 0) {
    let div = document.createElement("div");
    div.innerHTML = "There are no pending requests at the moment.";
    container.appendChild(div);
  }
  allRequests.forEach((request) => {
    let outerDiv = document.createElement("div");
    outerDiv.classList.add("user-item", "--active");

    let imgDiv = document.createElement("div");
    imgDiv.classList.add("user-item__avatar");
    let img = document.createElement("img");
    img.src = request.buyerProfilePic;
    img.alt = "user";
    imgDiv.appendChild(img);

    let userDiv = document.createElement("div");
    userDiv.classList.add("user-item__box");

    let usernameDiv = document.createElement("div");
    usernameDiv.classList.add("user-item__name");
    let usernameLink = document.createElement("a");
    usernameLink.textContent = request.buyerName;
    usernameDiv.appendChild(usernameLink);

    let playingDiv = document.createElement("div");
    playingDiv.classList.add("user-item__playing");
    playingDiv.innerHTML = `Requesting <b>${request.title}</b>`; // Assuming request.gameName contains the game name
    let detailsDiv = document.createElement("div");
    detailsDiv.classList.add("user-item__playing");
    detailsDiv.innerHTML = `Price: <b>${request.price}</b>`; // Assuming request.gameName contains the game name
    let dateDiv = document.createElement("div");
    dateDiv.classList.add("user-item__playing");
    let dateOnly = request.requestDate.split(" ")[0]; // Splits the date and time, keeping only the date part
    dateDiv.innerHTML = `Requested on: <b>${dateOnly}</b>`;
    /*
    let thumbnailDiv = document.createElement("div");
    let thumbnail = document.createElement("img");
    thumbnail.src = request.thumbnail;
    thumbnailDiv.appendChild(thumbnail);
    thumbnailDiv.classList.add("thumbnail");
    */

    userDiv.appendChild(usernameDiv);
    userDiv.appendChild(playingDiv);
    userDiv.appendChild(dateDiv);
    userDiv.appendChild(detailsDiv);

    let buttonDiv = document.createElement("div");
    buttonDiv.classList.add("buttonRequests");
    let acceptButton = document.createElement("button");
    acceptButton.textContent = "Accept";
    acceptButton.addEventListener("click", function () {
      if (
        confirm("Do you want to hand this book to " + request.buyerName + "?")
      ) {
        AcceptBookRequest(request.buyerId, request.id);
      }
    });
    let declineButton = document.createElement("button");
    declineButton.textContent = "Decline";
    declineButton.addEventListener("click", function () {
      if (
        confirm("Do you want to decline " + request.buyerName + "'s offer?")
      ) {
        CancelBookRequest(request.buyerId, request.id);
      }
    });

    buttonDiv.appendChild(acceptButton);
    buttonDiv.appendChild(declineButton);

    outerDiv.appendChild(imgDiv);
    outerDiv.appendChild(userDiv);

    outerDiv.appendChild(buttonDiv);

    container.appendChild(outerDiv);
  });
}

function AcceptBookRequest(buyerId, bookId) {
  let api =
    "https://proj.ruppin.ac.il/cgroup75/test2/tar1/api/User/AcceptRequest?buyerId=" +
    buyerId +
    "&sellerId=" +
    user.id +
    "&bookId=" +
    bookId;
  ajaxCall("PUT", api, null, AcceptBookRequestSCB, AjaxECB);
}

function AcceptBookRequestSCB(message) {
  if (message == 3) {
    alert("Book exchanged successfuly");
    window.location.reload();
  } else
    alert("Error: Buyer already owns the book.\n You may cancel the request.");
}

function CancelBookRequest(buyerId, bookId) {
  let api =
    "https://proj.ruppin.ac.il/cgroup75/test2/tar1/api/User/CancelRequest?buyerId=" +
    buyerId +
    "&sellerId=" +
    user.id +
    "&bookId=" +
    bookId;
  ajaxCall("PUT", api, null, CancelBookRequestSCB, AjaxECB);
}

function CancelBookRequestSCB(message) {
  if (message == 1) {
    window.location.reload();
  } else {
    alert("Error: can't exchange book.");
  }
}
