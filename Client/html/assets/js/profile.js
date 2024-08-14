jQuery(document).ready(function ($) {
  if (user != null) {
    SetLibraryHeader();
    GetFavoriteBooks();
    GetReadBooks();
    GetBoughtBooks();
    SetStarSystem();
    SetRatingSystem();
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

function GetReadBooks() {
  let api = "https://localhost:7063/api/User/GetHistory?userId=" + user.id;
  ajaxCall("GET", api, null, GetReadBooksSCB, AjaxECB);
}

function GetReadBooksSCB(readBooks) {
  GetBooksSuccess(readBooks, "readContainer");
  AddReviewLogo("readContainer");
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

function AddReviewLogo(containerName) {
  let booksContainer = document.getElementById(containerName);
  let hearts = booksContainer.getElementsByClassName("interact");

  for (let i = 0; i < hearts.length; i++) {
    let bookId = hearts[i].id;
    hearts[i].innerHTML = "";

    // Create and configure the "remove from favorites" anchor
    let heartAnchor = document.createElement("a");
    heartAnchor.classList.add("heartFavorite");

    let heart = document.createElement("img");
    heart.src = "assets/img/icons/review.png";
    heart.style.width = "40px";
    heart.style.height = "40px";
    heartAnchor.appendChild(heart);
    heartAnchor.href = "#modal-review";
    heartAnchor.setAttribute("data-uk-toggle", "");
    heartAnchor.addEventListener("click", function () {
      let idp = document.getElementById("bookId");
      idp.innerHTML = "";
      idp.innerHTML = bookId;
    });
    hearts[i].appendChild(heartAnchor);
  }
}

function SetStarSystem() {
  document.querySelectorAll(".star-rating .star").forEach(function (star) {
    star.addEventListener("mouseover", function () {
      // Add hover effect to the current star and all stars to the right
      let currentStar = this;
      while (currentStar) {
        currentStar.classList.add("hover");
        currentStar = currentStar.nextElementSibling;
      }
    });

    star.addEventListener("mouseout", function () {
      // Remove hover effect from all stars
      this.parentNode.querySelectorAll(".star").forEach(function (s) {
        s.classList.remove("hover");
      });
    });

    star.addEventListener("click", function () {
      // Remove selected class from all stars
      this.parentNode.querySelectorAll(".star").forEach(function (s) {
        s.classList.remove("selected");
      });

      // Add selected class to the current star and all stars to the right
      let currentStar = this;
      while (currentStar) {
        currentStar.classList.add("selected");
        currentStar = currentStar.nextElementSibling;
      }

      // Store the selected rating value
      const ratingValue = this.getAttribute("data-value");
      console.log("Selected rating:", ratingValue);

      // Store the rating value for further use (e.g., form submission)
      document
        .querySelector(".uk-form-controls")
        .setAttribute("data-rating", ratingValue);
    });
  });
}

function SetRatingSystem() {
  let bookId = document.getElementById("bookId");
  let text = document.getElementById("reviewText");
  document
    .getElementById("ratingForm")
    .addEventListener("submit", function (event) {
      event.preventDefault();
      let reviewText = text.value;
      if (reviewText == null) {
        reviewText = "Empty";
      }
      let api =
        "https://localhost:7063/api/User/AddReview?userId=" +
        user.id +
        "&bookId=" +
        bookId.innerHTML +
        "&text=" +
        reviewText +
        "&rating= " +
        GetSelectedRating();
      ajaxCall("POST", api, null, AddReviewSCB, AddReviewECB);
    });
}

function AddReviewECB(Error) {
  alert("Error: Review form was not filled");
}

function GetSelectedRating() {
  // Find the star element with the 'selected' class
  const selectedStar = document.querySelector(".star-rating .star.selected");

  // Check if a selected star exists
  if (selectedStar) {
    // Get the value from the data-value attribute
    return selectedStar.getAttribute("data-value");
  } else {
    // Return null or a default value if no star is selected
    return 0;
  }
}

function AddReviewSCB(message) {
  if (message == 2) {
    window.location.reload();
  } else {
    alert("You already added a review for this book!");
  }
}
