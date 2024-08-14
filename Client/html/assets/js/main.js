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
      if (user != null) {
        localStorage.removeItem("user");
        window.location.reload(); // Reload the page after removing the item
      }
    });
  }

  if (user != null) {
    GetNotificationCount();
  }
});

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

  if (user != null && userPic != null) {
    userPic.src = user.profilePic;
    userPic.alt = user.name + "'s profile picture";
  }
}

function GetBooksSuccess(allBooks, containerId, callback) {
  let booksContainer = document.getElementById(containerId);
  if (!booksContainer) return; // Ensure the container exists
  booksContainer.innerHTML = "";

  allBooks.forEach((book) => {
    let outerDiv = document.createElement("div");

    let middleDiv = document.createElement("div");
    middleDiv.classList.add("game-card");

    let innerDiv = document.createElement("div");
    innerDiv.classList.add("game-card__box");

    let mediaDiv = document.createElement("div");
    mediaDiv.classList.add("game-card__media");

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
    mediaDiv.appendChild(mediaImg);

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

    let ratingDiv = document.createElement("div");
    ratingDiv.classList.add("game-card__rating");

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

    ratingAndPriceDiv.appendChild(ratingDiv);
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
      if (user != null && confirm("Buy this book for " + book.price + "?")) {
        AddToFavorite(book.id);
      } else alert("you must log in first");
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
    "https://localhost:7063/api/User/Favorite?userId=" +
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

function AjaxECB(response) {
  alert("Error: " + response);
}

function GetNotificationCount() {
  let api = "https://localhost:7063/api/User/Notification?userId=" + user.id;
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
      <span class="uk-form-label">Authors:</span> ${book.authors.join(", ")}<br>
      <span class="uk-form-label">Genre:</span> ${book.category}<br>
      <span class="uk-form-label">Length:</span> ${book.pageCount} pages<br>
      <span class="uk-form-label">Publish date:</span> ${book.publishedDate}<br>
      <span class="uk-form-label">Print Type:</span> ${
        book.isMagazine ? "Magazine" : "Book"
      }<br>
      <span class="uk-form-label">Reading Format:</span> ${
        book.isEbook ? "Digital" : "Physical"
      }<br>
      <span class="uk-form-label">Language:</span> ${book.language}<br>
      <span class="uk-form-label">Price:</span> ${book.price}$<br>
      <span class="uk-form-label">Description (if available):</span> ${
        book.description
      }<br>
      <span class="uk-form-label">Text Snippet (if available):</span> ${
        book.textSnippet
      }<br>
  `;

  // Insert content into the modal body
  modalBody.innerHTML = content;

  // Optionally, you can add other elements or update existing ones if needed
}
