function GetBooks() {
  let api = "https://localhost:7063/api/Book";
  ajaxCall("GET", api, null, GetBooksSCB, GetBooksECB);
}

function GetBooksSCB(allBooks) {
  let booksContainer = document.getElementById("booksContainer");
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
    if (book.authors[0] == "") {
      genreDiv.textContent = "N/A"; // Replace with the actual genre if available
    } else {
      genreDiv.textContent = book.authors;
    }

    let ratingAndPriceDiv = document.createElement("div");
    ratingAndPriceDiv.classList.add("game-card__rating-and-price");

    let ratingDiv = document.createElement("div");
    ratingDiv.classList.add("game-card__rating");

    let ratingSpan = document.createElement("span");
    ratingSpan.textContent = book.ratingAverage; // Replace with the actual rating if available
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
    priceSpan.textContent = book.price + "$"; // Replace with the actual price if available

    priceDiv.appendChild(priceSpan);

    ratingAndPriceDiv.appendChild(ratingDiv);
    ratingAndPriceDiv.appendChild(priceDiv);

    let bottomDiv = document.createElement("div");
    bottomDiv.classList.add("game-card__bottom");

    let platformDiv = document.createElement("div");
    platformDiv.classList.add("game-card__platform");

    if (book.epubLink != "N/A") {
      let anchor = document.createElement("a");
      anchor.href = book.epubLink;
      anchor.target = "_blank";

      let platformWindowsIcon = document.createElement("img");
      platformWindowsIcon.src = "assets/img/icons/epub.png";
      platformWindowsIcon.style.width = "36px";
      platformWindowsIcon.style.height = "48px";

      anchor.appendChild(platformWindowsIcon); // Append the image to the anchor
      platformDiv.appendChild(anchor); // Append the anchor to the platformDiv
    }

    if (book.pdfLink != "") {
      let anchor = document.createElement("a");
      anchor.href = book.pdfLink;
      anchor.target = "_blank";

      let platformAppleIcon = document.createElement("img");
      platformAppleIcon.src = "assets/img/icons/pdf.png";
      platformAppleIcon.style.width = "36px";
      platformAppleIcon.style.height = "48px";

      anchor.appendChild(platformAppleIcon); // Append the image to the anchor
      platformDiv.appendChild(anchor); // Append the anchor to the platformDiv
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

  let bookAmount = document.getElementById("bookSum");
  bookAmount.innerText = allBooks.length + " books";
}

function GetBooksECB(ERROR) {
  alert("ERROR: " + ERROR);
}

GetBooks();

let searchButton = document.getElementById("searchBooksBttn");
searchButton.addEventListener("click", function () {
  searchByParameters();
});

let refreshButton = document.getElementById("refreshBooksBttn");
refreshButton.addEventListener("click", function () {
  GetBooks();
});

function searchByParameters() {
  let val = document.getElementById("parameter");
  let textBox = document.getElementById("bookTextInput");
  console.log("CONTENT: " + textBox.value);
  if (val.value == "" || textBox.value == "") {
    alert("ERROR: no search input or parameter supplied");
    return;
  }

  let api = "https://localhost:7063/api/Book/Text";
  ajaxCall(
    "GET",
    api,
    { parameter: val.value, text: textBox.value },
    searchByParametersSCB,
    searchByParametersECB
  );
}

function searchByParametersSCB(fitBooks) {
  GetBooksSCB(fitBooks);
}

function searchByParametersECB(ERROR) {
  alert("ERROR fetching books: " + ERROR);
}

function openModal(book) {
  var modal = document.getElementById("myModal");
  var modalContent = document.getElementById("modalBookContent");
  modalContent.innerHTML = `
    <div style="display: flex;">
      <div style="flex: 1; padding: 10px; color: black;">
        <h2>${book.title}</h2>
        <h3>${book.subtitle}</h3>
        <p>${book.authors.join(", ")}</p>
        <p>Genre: ${book.category}</p>
        <p>Length: ${book.pageCount} pages</p>
        <p>Publish date: ${book.publishedDate}</p>
        <p>Print Type: ${book.isMagazine ? "Magazine" : "Book"}</p>
        <p>Reading Format: ${book.isEbook ? "Digital" : "Physical"}</p>
        <p>Language: ${book.language}</p>
        <p>Price: ${book.price}$</p>
        <p>Description (if available): ${book.description}</p>
        <p>Text Snippet (if available): ${book.textSnippet}</p>
      </div>
      <div style="flex: 1; display: flex; flex-direction: column; align-items: center; padding: 10px;">
        <img src="${
          book.thumbnail
        }" style="max-width: 100%; max-height: 400px;">
                <p style="color: red;">${
                  book.isMature
                    ? "This book is intended for mature audiences. Our site is not responsible for verifying the user's age."
                    : ""
                }</p>
      </div>
    </div>
  `;
  modal.style.display = "block";
}

function closeModal() {
  var modal = document.getElementById("myModal");
  modal.style.display = "none";
}

// Get the <span> element that closes the modal
var span = document.getElementsByClassName("close")[0];

// When the user clicks on <span> (x), close the modal
span.onclick = function () {
  closeModal();
};

// When the user clicks anywhere outside of the modal, close it
window.onclick = function (event) {
  var modal = document.getElementById("myModal");
  if (event.target == modal) {
    closeModal();
  }
};
