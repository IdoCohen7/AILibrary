jQuery(document).ready(function ($) {
  if (user != null) {
    SetLibraryHeader();
    GetFavoriteBooks();
    GetReadBooks();
    GetBoughtBooks();
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

function SetLibraryHeader() {
  const libraryHeader = document.getElementById("libraryHeader");
  libraryHeader.innerText = user.name + "'s Library";
}

function GetFavoriteBooks() {
  let api = "https://localhost:7063/api/User/GetFavorites?userId=" + user.id;
  ajaxCall("GET", api, null, GetFavoriteBooksSCB, AjaxECB);
}

function GetFavoriteBooksSCB(favoriteBooks) {
  GetBooksSuccess(favoriteBooks, "booksContainer");
  ReplaceHearts("booksContainer");
}

function ReplaceHearts(containerName) {
  let booksContainer = document.getElementById(containerName);
  let hearts = booksContainer.getElementsByClassName("interact");

  for (let i = 0; i < hearts.length; i++) {
    let bookId = hearts[i].id;
    hearts[i].innerHTML = "";

    // Create and configure the "remove from favorites" anchor
    let heartAnchor = document.createElement("a");
    heartAnchor.classList.add("heartFavorite");

    let heart = document.createElement("img");
    heart.src = "assets/img/icons/removeFavorite.png";
    heart.style.width = "40px";
    heart.style.height = "40px";
    heartAnchor.appendChild(heart);
    heartAnchor.addEventListener("click", function () {
      RemoveFromFavorites(bookId);
    });
    hearts[i].appendChild(heartAnchor);

    // Create and configure the "mark as read" anchor
    let doneAnchor = document.createElement("a");
    doneAnchor.classList.add("heartFavorite");

    let done = document.createElement("img");
    done.src = "assets/img/icons/done.png";
    done.style.width = "40px";
    done.style.height = "40px";
    doneAnchor.appendChild(done);
    doneAnchor.addEventListener("click", function () {
      MarkAsRead(bookId);
    });
    hearts[i].appendChild(doneAnchor);
  }
}

function AjaxECB(Error) {
  alert("Error: " + Error.responseText);
}

function GetReadBooks() {
  let api = "https://localhost:7063/api/User/GetHistory?userId=" + user.id;
  ajaxCall("GET", api, null, GetReadBooksSCB, AjaxECB);
}

function GetReadBooksSCB(readBooks) {
  GetBooksSuccess(readBooks, "readContainer");
  RemoveHearts("readContainer");
}

function RemoveHearts(containerName) {
  let readContainer = document.getElementById(containerName);
  let hearts = readContainer.getElementsByClassName("interact");

  for (let i = 0; i < hearts.length; i++) {
    hearts[i].innerHTML = "";
  }
}

function RemoveFromFavorites(bookId) {
  let api =
    "https://localhost:7063/api/User/RemoveFavorite?userId=" +
    user.id +
    "&bookId=" +
    bookId;
  ajaxCall("DELETE", api, null, RemoveFromFavoritesSCB, AjaxECB);
}

function RemoveFromFavoritesSCB(Message) {
  if (Message == 1) {
    window.location.reload();
  } else {
    alert("Error occured");
  }
}

function MarkAsRead(bookId) {
  if (confirm("Have you finished reading this book?")) {
    let api =
      "https://localhost:7063/api/User/Mark?userId=" +
      user.id +
      "&bookId=" +
      bookId;
    ajaxCall("POST", api, null, MarkAsReadSCB, AjaxECB);
  } else {
  }
}

function MarkAsReadSCB(Message) {
  if (Message == 2) {
    window.location.reload();
  } else {
    // Show an error alert if Message is not 2
    alert("Book is already read");
  }
}

function GetBoughtBooks() {
  let api = "https://localhost:7063/api/User/GetAccepted?userId=" + user.id;
  ajaxCall("GET", api, null, GetBoughtBooksSCB, AjaxECB);
}

function GetBoughtBooksSCB(boughtBooks) {
  GetBooksSuccess(boughtBooks, "boughtContainer");
  ReplaceHearts("boughtContainer");
}
