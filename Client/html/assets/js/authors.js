function GetAuthors() {
  let api = "https://localhost:7063/api/Author";
  ajaxCall("GET", api, null, GetAuthorsSCB, GetAuthorsECB);
}

function GetAuthorsSCB(allAuthors) {
  let div = document.getElementById("mainDiv");
  let headOne = document.createElement("h1");
  headOne.innerHTML = "Our site features creations by:";
  let remainingAuthors = allAuthors.length;
  let authorsList = [];

  function handleAuthorResponse(index, html) {
    authorsList[index] = html;
    remainingAuthors--;

    div.appendChild(headOne);
    // Once all authors are processed, append to the page
    if (remainingAuthors === 0) {
      $(".page-main").append(authorsList.join(""));

      // Now attach event listeners to the buttons
      let buttons = document.querySelectorAll(".author button");
      buttons.forEach((button) => {
        button.addEventListener("click", function () {
          ShowBooksWrittenBy(button.value);
        });
      });
    }
  }

  allAuthors.forEach((author, index) => {
    const authorName = author.name.split(" ").join("_");
    const apiUrl = `https://en.wikipedia.org/w/api.php?action=query&prop=extracts|pageimages&format=json&titles=${authorName}&exintro=1&origin=*`;

    ajaxCall(
      "GET",
      apiUrl,
      null,
      (data) => {
        const page = Object.values(data.query.pages)[0];
        const biography = page.extract || "No biography available.";
        const imageUrl = page.thumbnail ? page.thumbnail.source : null;

        // Check if the biography seems to be about multiple people or not about a person
        const isDisambiguation =
          biography.includes("may refer to") ||
          biography.includes("disambiguation") ||
          biography.includes("is the name of");

        const html = `
              <div class="author" style="margin-bottom: 80px;">
                <h3>${author.name}</h3>
                ${
                  imageUrl
                    ? `<img src="${imageUrl}" alt="${author.name}" style="max-width: 200px; height: auto;">`
                    : ""
                }
                <p>${
                  isDisambiguation ? "No biography available." : biography
                }</p>
                <button class="uk-button uk-button-secondary" value="${
                  author.id
                }">View Books by ${author.name}</button>
              </div>`;

        handleAuthorResponse(index, html);
      },
      () => {
        const html = `
              <div class="author">
                <h3>${author.name}</h3>
                <p>No biography available.</p>
              </div>`;

        handleAuthorResponse(index, html);
      }
    );
  });
}

function GetAuthorsECB(error) {
  alert("Error: " + error);
}

function ShowBooksWrittenBy(authorId) {
  let api = "https://localhost:7063/api/Author/Written?id=" + authorId;
  ajaxCall("GET", api, null, ShowBooksWrittenBySCB, ShowBooksWrittenByECB);
}

function ShowBooksWrittenBySCB(booksByAuthor) {
  let page = document.getElementById("mainDiv");
  page.innerHTML = ""; // Clear existing content

  // Create header
  let headerDiv = document.createElement("div");
  headerDiv.style.display = "flex";
  headerDiv.style.justifyContent = "space-between";
  headerDiv.style.alignItems = "center";
  headerDiv.style.marginBottom = "20px";

  let head = document.createElement("h2");
  head.textContent =
    "Creations by " + (booksByAuthor[0] ? booksByAuthor[0].authors : "Unknown");
  head.style.margin = "0"; // Remove default margin

  let returnButton = document.createElement("button");
  returnButton.textContent = "Return";
  returnButton.classList.add("uk-button", "uk-button-secondary");
  returnButton.addEventListener("click", function () {
    location.reload();
  });

  headerDiv.appendChild(head);
  headerDiv.appendChild(returnButton);

  page.appendChild(headerDiv);

  // Create container for books
  let booksContainer = document.createElement("div");
  booksContainer.className =
    "uk-grid uk-child-width-1-6@xl uk-child-width-1-4@l uk-child-width-1-3@s uk-flex-middle uk-grid-small";
  booksContainer.setAttribute("data-uk-grid", "");

  // Append books to container
  booksByAuthor.forEach((book) => {
    let outerDiv = document.createElement("div");

    let middleDiv = document.createElement("div");
    middleDiv.classList.add("game-card");

    let innerDiv = document.createElement("div");
    innerDiv.classList.add("game-card__box");

    let mediaDiv = document.createElement("div");
    mediaDiv.classList.add("game-card__media");

    let mediaImg = document.createElement("img");
    mediaImg.style.width = "100%";
    mediaImg.style.objectFit = "contain"; // Ensure the image is not cropped
    mediaImg.src = book.thumbnail;
    mediaImg.alt = book.title;
    mediaImg.addEventListener("click", function () {
      openModal(book);
    });

    mediaDiv.appendChild(mediaImg);

    let infoDiv = document.createElement("div");
    infoDiv.classList.add("game-card__info");

    let titleLink = document.createElement("a");
    titleLink.classList.add("game-card__title");
    titleLink.href = book.infoLink; // Replace with the actual book profile link
    titleLink.textContent = book.title; // Use the book title

    let genreDiv = document.createElement("div");
    genreDiv.classList.add("game-card__genre");
    genreDiv.textContent = book.authors[0] === "" ? "N/A" : book.authors;

    let ratingAndPriceDiv = document.createElement("div");
    ratingAndPriceDiv.classList.add("game-card__rating-and-price");

    let ratingDiv = document.createElement("div");
    ratingDiv.classList.add("game-card__rating");

    let ratingSpan = document.createElement("span");
    ratingSpan.textContent = book.ratingAverage || "N/A";
    let amountSpan = document.createElement("span");
    amountSpan.textContent = (book.ratingCount || 0) + " reviews";

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
    priceSpan.textContent = (book.price || "N/A") + "$";

    priceDiv.appendChild(priceSpan);

    ratingAndPriceDiv.appendChild(ratingDiv);
    ratingAndPriceDiv.appendChild(priceDiv);

    let bottomDiv = document.createElement("div");
    bottomDiv.classList.add("game-card__bottom");

    let platformDiv = document.createElement("div");
    platformDiv.classList.add("game-card__platform");

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

    let heartAnchor = document.createElement("a");
    heartAnchor.href = "";
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

    booksContainer.appendChild(outerDiv); // Append the outerDiv to the booksContainer
  });

  page.appendChild(booksContainer); // Append the booksContainer to the mainDiv

  // Update book amount display if necessary
  let bookAmount = document.getElementById("bookSum");
  if (bookAmount) {
    bookAmount.innerText = booksByAuthor.length + " books";
  }
}

function ShowBooksWrittenByECB(Error) {
  alert("Error: " + Error);
}
GetAuthors();
