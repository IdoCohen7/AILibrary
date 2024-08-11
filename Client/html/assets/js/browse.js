function GetBooks() {
  let api = "https://localhost:7063/api/Book";
  ajaxCall("GET", api, null, GetBooksSCB, GetBooksECB);
}

function GetBooksSCB(allBooks) {
  GetBooksSuccess(allBooks, "booksContainer");
  let bookAmount = document.getElementById("bookSum");
  bookAmount.innerText = allBooks.length + " books";
  RemoveLinks();
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

function RemoveLinks() {
  let booksContainer = document.getElementById("booksContainer");
  let links = booksContainer.getElementsByClassName("readingLinks");

  for (let i = 0; i < links.length; i++) {
    links[i].innerHTML = "";
  }
}
