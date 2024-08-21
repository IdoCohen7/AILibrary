jQuery(document).ready(function ($) {
  $(".js-select").niceSelect();
  $(document).on("click", ".menu-btn", function () {
    $(this).toggleClass("is-active");
    $(".sidebar").toggleClass("is-show");
  });
  const mediaHeader = window.matchMedia("(max-width: 959px)");

  function handleHeader(e) {
    if (e.matches) {
      $(".menu-btn").removeClass("is-active");
      $(".sidebar").removeClass("is-show");
      $(document).on("click", ".menu-btn", function () {
        $("body").toggleClass("no-scroll");
      });
    } else {
      $(".menu-btn").addClass("is-active");
      $(".sidebar").addClass("is-show");
      $("body").removeClass("no-scroll");
    }
  }

  /////////////////////////////////////////////////////////////////
  // Preloader
  /////////////////////////////////////////////////////////////////

  var $preloader = $("#page-preloader"),
    $spinner = $preloader.find(".spinner-loader");
  $spinner.fadeOut();
  $preloader.delay(250).fadeOut("slow");

  mediaHeader.addListener(handleHeader);
  handleHeader(mediaHeader);
  const recommendSlider = new Swiper(".js-recommend .swiper", {
    slidesPerView: 1,
    spaceBetween: 40,
    loop: true,
    watchOverflow: true,
    observeParents: true,
    observeSlideChildren: true,
    observer: true,
    speed: 800,
    autoplay: {
      delay: 5000,
    },
    navigation: {
      nextEl: ".js-recommend .swiper-button-next",
      prevEl: ".js-recommend .swiper-button-prev",
    },
    pagination: {
      el: ".js-recommend .swiper-pagination",
      type: "bullets",
      // 'bullets', 'fraction', 'progressbar'
      clickable: true,
    },
  });
  const trendingSlider = new Swiper(".js-trending .swiper", {
    slidesPerView: 1,
    spaceBetween: 40,
    loop: true,
    watchOverflow: true,
    observeParents: true,
    observeSlideChildren: true,
    observer: true,
    speed: 800,
    autoplay: {
      delay: 5000,
    },
    navigation: {
      nextEl: ".js-trending .swiper-button-next",
      prevEl: ".js-trending .swiper-button-prev",
    },
    pagination: {
      el: ".js-trending .swiper-pagination",
      type: "bullets",
      // 'bullets', 'fraction', 'progressbar'
      clickable: true,
    },
  });
  const popularSlider = new Swiper(".js-popular .swiper", {
    slidesPerView: 1,
    spaceBetween: 25,
    loop: true,
    watchOverflow: true,
    observeParents: true,
    observeSlideChildren: true,
    observer: true,
    speed: 800,
    autoplay: {
      delay: 5000,
    },
    navigation: {
      nextEl: ".js-popular .swiper-button-next",
      prevEl: ".js-popular .swiper-button-prev",
    },
    pagination: {
      el: ".js-popular .swiper-pagination",
      type: "bullets",
      // 'bullets', 'fraction', 'progressbar'
      clickable: true,
    },
    breakpoints: {
      575: {
        slidesPerView: 2,
        spaceBetween: 25,
      },
      1199: {
        slidesPerView: 4,
        spaceBetween: 25,
      },
      1599: {
        slidesPerView: 6,
        spaceBetween: 25,
      },
    },
  });
  const gallerySmall = new Swiper(".js-gallery-small .swiper", {
    slidesPerView: 1,
    spaceBetween: 20,
    loop: true,
    watchOverflow: true,
    observeParents: true,
    observeSlideChildren: true,
    observer: true,
    speed: 800,
    pagination: {
      el: ".js-gallery-small .swiper-pagination",
      type: "bullets",
      // 'bullets', 'fraction', 'progressbar'
      clickable: true,
    },
    breakpoints: {
      575: {
        slidesPerView: 2,
        spaceBetween: 20,
      },
      767: {
        slidesPerView: 3,
        spaceBetween: 20,
      },
      1599: {
        slidesPerView: 4,
        spaceBetween: 20,
      },
    },
  });
  const galleryBig = new Swiper(".js-gallery-big .swiper", {
    slidesPerView: 1,
    spaceBetween: 20,
    loop: true,
    watchOverflow: true,
    observeParents: true,
    observeSlideChildren: true,
    observer: true,
    speed: 800,
    thumbs: {
      swiper: gallerySmall,
    },
  });

  SetProfilePicture();
  const logoutBttn = document.getElementById("logout");
  if (logoutBttn != null) {
    logoutBttn.addEventListener("click", function () {
      Logout();
    });
  }

  if (user != null) {
    GetNotificationCount();
    if (user.isAdmin == true) {
      // Create admin sidebar
      let sidebar = document.getElementById("sidebar");

      let adminHeader = document.createElement("li");
      adminHeader.classList.add("uk-nav-header");
      adminHeader.textContent = "Admin";

      let managementItem = document.createElement("li");
      managementItem.id = "managementLi";
      let managementLink = document.createElement("a");
      managementLink.href = "11_managment.html";
      let managementIcon = document.createElement("i");
      managementIcon.classList.add("ico_clipboard-text");
      let managementSpan = document.createElement("span");
      managementSpan.textContent = "Management";

      managementLink.appendChild(managementIcon);
      managementLink.appendChild(managementSpan);
      managementItem.appendChild(managementLink);

      let librariesItem = document.createElement("li");
      librariesItem.id = "librariesLi";
      let librariesLink = document.createElement("a");
      librariesLink.href = "12_libraries.html";
      let librariesIcon = document.createElement("i");
      librariesIcon.classList.add("ico_search");
      let librariesSpan = document.createElement("span");
      librariesSpan.textContent = "Libraries";

      librariesLink.appendChild(librariesIcon);
      librariesLink.appendChild(librariesSpan);
      librariesItem.appendChild(librariesLink);

      let sidebarNav = sidebar.querySelector(".uk-nav");
      sidebarNav.appendChild(adminHeader);
      sidebarNav.appendChild(managementItem);
      sidebarNav.appendChild(librariesItem);
    }
  }

  if (user != null && user.isActive == 0) {
    alert("You are banned, please contact management to discuss your status");
    window.location.href = "01_login-in.html";
  }
});

function Logout() {
  if (user != null) {
    localStorage.removeItem("user");
    window.location.href = "01_login-in.html"; // Reload the page after removing the item
  }
}

let user = JSON.parse(localStorage.getItem("user"));

function ajaxCall(method, api, data, successCB, errorCB) {
  $.ajax({
    type: method, // Get/Post/Put/Delete/Patch
    url: api, // routing to the server
    data: data, // the data we pass in the body (if any…)
    cache: false, // allow caching
    contentType: "application/json", // the data format we expect back
    dataType: "json", // the data format that we send
    success: successCB, // the success callback function
    error: errorCB, // the error callback function
  });
}

function SetProfilePicture() {
  const userPic = document.getElementById("userPic");
  let user = JSON.parse(localStorage.getItem("user"));

  if (user != null) {
    userPic.src = user.profilePic;
    userPic.alt = user.name + "'s profile picture";
    const dashboard = document.getElementById("userDashboard");
    let anchor = document.createElement("a");
    let disconnectBttn = document.createElement("img");
    disconnectBttn.src = "assets/img/disconnect.png";
    anchor.appendChild(disconnectBttn);
    anchor.classList.add("profile");
    anchor.id = "logout";

    dashboard.appendChild(anchor);
  } else {
    if (userPic != null) {
      userPic.src =
        "https://upload.wikimedia.org/wikipedia/commons/7/7c/Profile_avatar_placeholder_large.png";
    }
  }
}

function SortBooks(array) {
  return array.sort((a, b) => a.title.localeCompare(b.title));
}

function GetBooksSuccess(allBooks, containerId, callback) {
  allBooks = SortBooks(allBooks);

  let booksContainer = document.getElementById(containerId);

  if (!booksContainer) return; // Ensure the container exists
  booksContainer.innerHTML = "";

  if (allBooks.length == 0) {
    booksContainer.innerHTML = "No books to display.";
    return;
  }

  allBooks.forEach((book) => {
    let outerDiv = document.createElement("div");

    let middleDiv = document.createElement("div");
    middleDiv.classList.add("game-card");

    let innerDiv = document.createElement("div");
    innerDiv.classList.add("game-card__box");

    let mediaDiv = document.createElement("div");
    mediaDiv.classList.add("game-card__media");

    let imgAnchor = document.createElement("a");
    let mediaImg = document.createElement("img");
    mediaImg.style.width = "100%";
    mediaImg.style.objectFit = "contain";
    mediaImg.src = book.thumbnail;
    mediaImg.alt = book.title;
    mediaImg.addEventListener("click", function () {
      // Populate modal with book details
      // Open the modal
      UIkit.modal("#modal-book").show();
      FillModalContent(book);
    });
    imgAnchor.appendChild(mediaImg);
    mediaDiv.appendChild(imgAnchor);

    let infoDiv = document.createElement("div");
    infoDiv.classList.add("game-card__info");

    let titleLink = document.createElement("a");
    titleLink.classList.add("game-card__title");
    titleLink.href = book.infoLink;
    titleLink.textContent = book.title;

    let genreDiv = document.createElement("div");
    genreDiv.classList.add("game-card__genre");
    genreDiv.textContent = book.authors[0] === "" ? "N/A" : book.authors;

    let ratingAndPriceDiv = document.createElement("div");
    ratingAndPriceDiv.classList.add("game-card__rating-and-price");

    let ratingAnchor = document.createElement("a");
    let ratingDiv = document.createElement("div");
    ratingDiv.classList.add("game-card__rating");
    ratingDiv.addEventListener("click", function () {
      UIkit.modal("#modal-reviews").show();
      GetBookReviews(book.id);
    });
    ratingAnchor.appendChild(ratingDiv);

    let ratingSpan = document.createElement("span");
    ratingSpan.textContent = book.ratingAverage;
    let amountSpan = document.createElement("span");
    amountSpan.textContent = book.ratingCount + " reviews";

    let ratingIcon = document.createElement("i");
    ratingIcon.classList.add("ico_star");

    let starDiv = document.createElement("div");
    starDiv.appendChild(ratingSpan);
    starDiv.appendChild(ratingIcon);

    ratingDiv.appendChild(starDiv);
    ratingDiv.appendChild(amountSpan);

    let priceDiv = document.createElement("div");
    priceDiv.classList.add("game-card__price");

    let priceSpan = document.createElement("span");
    priceSpan.textContent = book.price + "$";

    priceDiv.appendChild(priceSpan);

    ratingAndPriceDiv.appendChild(ratingAnchor);
    ratingAndPriceDiv.appendChild(priceDiv);

    let bottomDiv = document.createElement("div");
    bottomDiv.classList.add("game-card__bottom");
    bottomDiv.classList.add("mainInteract");

    let platformDiv = document.createElement("div");
    platformDiv.classList.add("game-card__platform");
    platformDiv.classList.add("readingLinks");

    if (book.epubLink !== "N/A") {
      let anchor = document.createElement("a");
      anchor.href = book.epubLink;
      anchor.target = "_blank";

      let platformWindowsIcon = document.createElement("img");
      platformWindowsIcon.src = "assets/img/icons/epub.png";
      platformWindowsIcon.style.width = "36px";
      platformWindowsIcon.style.height = "48px";

      anchor.appendChild(platformWindowsIcon);
      platformDiv.appendChild(anchor);
    }

    if (book.pdfLink !== "") {
      let anchor = document.createElement("a");
      anchor.href = book.pdfLink;
      anchor.target = "_blank";

      let platformAppleIcon = document.createElement("img");
      platformAppleIcon.src = "assets/img/icons/pdf.png";
      platformAppleIcon.style.width = "36px";
      platformAppleIcon.style.height = "48px";

      anchor.appendChild(platformAppleIcon);
      platformDiv.appendChild(anchor);
    }

    let usersDiv = document.createElement("div");
    usersDiv.classList.add("game-card__platform");
    usersDiv.classList.add("interact");
    usersDiv.id = book.id;

    let heartAnchor = document.createElement("a");
    heartAnchor.classList.add("heartFavorite");

    heartAnchor.addEventListener("click", function AddBookToFavorite() {
      if (user == null) {
        alert("you must log in first");
        return;
      } else {
        if (confirm("Buy this book for " + book.price + "$?")) {
          AddToFavorite(book.id);
        }
      }
    });
    let heart = document.createElement("img");
    heart.src = "assets/img/icons/addFavorite.png";
    heart.style.width = "40px";
    heart.style.height = "40px";
    heartAnchor.appendChild(heart);
    usersDiv.appendChild(heartAnchor);

    bottomDiv.appendChild(platformDiv);
    bottomDiv.appendChild(usersDiv);

    infoDiv.appendChild(titleLink);
    infoDiv.appendChild(genreDiv);
    infoDiv.appendChild(ratingAndPriceDiv);
    infoDiv.appendChild(bottomDiv);

    innerDiv.appendChild(mediaDiv);
    innerDiv.appendChild(infoDiv);

    middleDiv.appendChild(innerDiv);
    outerDiv.appendChild(middleDiv);

    booksContainer.appendChild(outerDiv);
  });

  // Optional callback for additional functionality
  if (callback && typeof callback === "function") {
    callback();
  }
}

function AddToFavorite(bookId) {
  let api =
    "https://proj.ruppin.ac.il/cgroup75/test2/tar1/api/User/Favorite?userId=" +
    user.id +
    "&bookId=" +
    bookId;
  ajaxCall("POST", api, null, AddToFavoriteSCB, AjaxECB);
}

function AddToFavoriteSCB(Message) {
  if (Message == 1) {
    alert("Book added to favorites!");
  } else {
    alert("Error: this book is already in your favorites");
  }
}

function AjaxECB(xhr) {
  let title = xhr.getResponseHeader("X-Title") || "No Title";
  let errorDetails = xhr.responseText || "No error details available";
  alert("Error occurred : " + title + "\nInfo: " + errorDetails);
}

function GetNotificationCount() {
  let api =
    "https://proj.ruppin.ac.il/cgroup75/test2/tar1/api/User/Notification?userId=" +
    user.id;
  ajaxCall("GET", api, null, GetNotificationCountSCB, AjaxECB);
}

function GetNotificationCountSCB(number) {
  if (number > 0) {
    let note = document.getElementById("requestNote");
    let span = document.createElement("span");
    span.classList.add("count");
    span.innerHTML = number;
    note.appendChild(span);
  }
}

function FillModalContent(book) {
  let modalBody = document.querySelector("#modal-book .uk-modal-body");

  // Update the title

  // Prepare content for each property
  let content = `
  <h2 id="bookTitle" class="uk-modal-title">${book.title}</h2>
  <h3 id="bookSubtitle" class="uk-modal-title">${book.subtitle}</h3>
  ${
    book.isMature
      ? '<span class="uk-form-label price-down">This book is intended for mature audiences. Our site is not responsible for verifying the user\'s age.</span><br><br>'
      : ""
  }
      <span class="uk-form-label uk-text-lead">Authors:</span> ${book.authors.join(
        ", "
      )}<br>
      <span class="uk-form-label uk-text-lead">Genre:</span> ${
        book.category
      }<br>
      <span class="uk-form-label uk-text-lead">Length:</span> ${
        book.pageCount
      } pages<br>
      <span class="uk-form-label uk-text-lead">Publish date:</span> ${
        book.publishedDate
      }<br>
      <span class="uk-form-label uk-text-lead">Print Type:</span> ${
        book.isMagazine ? "Magazine" : "Book"
      }<br>
      <span class="uk-form-label uk-text-lead">Reading Format:</span> ${
        book.isEbook ? "Digital" : "Physical"
      }<br>
      <span class="uk-form-label uk-text-lead">Language:</span> ${
        book.language
      }<br>
      <span class="uk-form-label uk-text-lead">Price:</span> ${book.price}$<br>
      <span class="uk-form-label uk-text-lead">Description (if available):</span> ${
        book.description
      }<br>
      <span class="uk-form-label uk-text-lead">Text Snippet (if available):</span> ${
        book.textSnippet
      }<br>
  `;

  // Insert content into the modal body
  modalBody.innerHTML = content;

  // Optionally, you can add other elements or update existing ones if needed
}

function GetBookReviews(bookId) {
  let api =
    "https://proj.ruppin.ac.il/cgroup75/test2/tar1/api/Book/Reviews?bookId=" +
    bookId;
  ajaxCall("GET", api, null, GetBookReviewsSCB, AjaxECB);
}

async function query(data) {
  const response = await fetch(
    "https://api-inference.huggingface.co/models/avichr/heBERT_sentiment_analysis",
    {
      headers: {
        Authorization: "Bearer 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", // Replace with your API key
        "Content-Type": "application/json",
      },
      method: "POST",
      body: JSON.stringify(data),
    }
  );
  const result = await response.json();
  return result;
}

function GetBookReviewsSCB(reviews) {
  let modalBody = document.getElementById("reviewModalHeader");
  modalBody.innerHTML = ""; // Clear previous content if any

  // Add the h2 element at the top
  let title = document.createElement("h2");
  title.classList.add("uk-modal-title");
  title.textContent = "Reviews";
  modalBody.appendChild(title);

  // Add the reviews or the "no reviews" message
  if (reviews.length === 0) {
    let noReviewsDiv = document.createElement("div");
    noReviewsDiv.innerHTML = "There are no reviews available.";
    modalBody.appendChild(noReviewsDiv);
  } else {
    reviews.forEach((review, index) => {
      let outerDiv = document.createElement("div");
      outerDiv.classList.add("user-item", "--active");

      let imgDiv = document.createElement("div");
      imgDiv.classList.add("user-item__avatar");
      let img = document.createElement("img");
      img.src = review.profilePic;
      img.alt = "user";
      imgDiv.appendChild(img);

      let userDiv = document.createElement("div");
      userDiv.classList.add("user-item__box", "review-content");

      let detailsDiv = document.createElement("div");
      detailsDiv.classList.add("review-details");

      let usernameDiv = document.createElement("div");
      usernameDiv.classList.add("user-item__name");
      let usernameLink = document.createElement("a");
      usernameLink.textContent = review.userName;
      usernameDiv.appendChild(usernameLink);

      let reviewTextDiv = document.createElement("div");
      reviewTextDiv.classList.add("user-item__playing");
      reviewTextDiv.textContent = "Review: " + review.text;

      let ratingDiv = document.createElement("div");
      ratingDiv.classList.add("user-item__playing");
      ratingDiv.innerHTML = `Rating: <b>${review.rating}/5</b>`;

      let dateDiv = document.createElement("div");
      dateDiv.classList.add("user-item__playing");
      dateDiv.innerHTML = `Date: <b>${review.date}</b>`;

      let sentimentButton = document.createElement("button");
      sentimentButton.textContent = "Analyze Sentiment";
      sentimentButton.classList.add("sentiment-button");

      sentimentButton.addEventListener("click", async () => {
        try {
          const sentimentResult = await query({ inputs: review.text });

          const label = sentimentResult[0][0]?.label || "Unknown";
          const score = sentimentResult[0][0]?.score || 0;
          const capitalizedLabel =
            label.charAt(0).toUpperCase() + label.slice(1);
          const formattedScore = score.toFixed(2);

          alert(`Sentiment: ${capitalizedLabel}\n(Score: ${formattedScore})`);
        } catch (error) {
          console.error("Error analyzing sentiment:", error);
          alert("Error analyzing sentiment.");
        }
      });

      detailsDiv.appendChild(usernameDiv);
      detailsDiv.appendChild(reviewTextDiv);
      detailsDiv.appendChild(ratingDiv);
      detailsDiv.appendChild(dateDiv);

      userDiv.appendChild(detailsDiv);
      userDiv.appendChild(sentimentButton);

      outerDiv.appendChild(imgDiv);
      outerDiv.appendChild(userDiv);

      modalBody.appendChild(outerDiv);
    });
  }

  // Add the p element at the bottom
  let infoParagraph = document.createElement("p");
  infoParagraph.id = "reviewBookId";
  infoParagraph.classList.add("uk-form-label");
  infoParagraph.style.textAlign = "center";
  infoParagraph.textContent =
    "Rating data is also based on external rating sites!";
  modalBody.appendChild(infoParagraph);
}
