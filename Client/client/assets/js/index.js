// Function to fetch recommended (Top Rated) books
function GetRecommendedBooks() {
  let api = "https://proj.ruppin.ac.il/cgroup75/test2/tar6/api/Book/TopRated";
  ajaxCall("GET", api, null, GetTopRatedBooksSCB, GetBooksECB);
}

// Success callback for recommended books
function GetTopRatedBooksSCB(allBooks) {
  GetBooksSuccess(allBooks, "booksContainer");
  RemoveLinks("booksContainer");
}

// Error callback for both types of books
function GetBooksECB(ERROR) {
  alert("ERROR: " + ERROR);
}

// Function to fetch the most new books
function GetMostNewBooks() {
  let api = "https://proj.ruppin.ac.il/cgroup75/test2/tar6/api/Book/MostNew";
  ajaxCall("GET", api, null, GetMostNewBooksSCB, GetBooksECB);
}

// Success callback for most new books
function GetMostNewBooksSCB(allBooks) {
  GetBooksSuccess(allBooks, "newBooksContainer");
  RemoveLinks("newBooksContainer");
}

// Function to open the modal for a specific book (for recommended books)
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

// Function to close the modal for a specific book (for recommended books)
function closeModal() {
  var modal = document.getElementById("myModal");
  modal.style.display = "none";
}

// Function to open the modal for a specific book (for most new books)
function openNewBookModal(book) {
  var modal = document.getElementById("newBooksModal");
  var modalContent = document.getElementById("modalNewBookContent");
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

// Function to close the modal for a specific book (for most new books)
function closeNewBookModal() {
  var modal = document.getElementById("newBooksModal");
  modal.style.display = "none";
}

// Event listener to close the recommended book modal
var span = document.getElementsByClassName("close")[0];
span.onclick = function () {
  closeModal();
};

// Event listener to close the most new book modal
var spanNew = document.getElementsByClassName("close-new")[0];
spanNew.onclick = function () {
  closeNewBookModal();
};

// Event listener to close the modal when clicking outside of it
window.onclick = function (event) {
  var modal = document.getElementById("myModal");
  var newBooksModal = document.getElementById("newBooksModal");
  if (event.target == modal) {
    closeModal();
  }
  if (event.target == newBooksModal) {
    closeNewBookModal();
  }
};

// Function to remove links from the books container
function RemoveLinks(containerId) {
  let booksContainer = document.getElementById(containerId);
  let links = booksContainer.getElementsByClassName("readingLinks");

  for (let i = 0; i < links.length; i++) {
    links[i].innerHTML = "";
  }
}

// Initialize fetching of books
GetRecommendedBooks();
GetMostNewBooks();
