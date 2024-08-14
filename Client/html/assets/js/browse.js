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

function RemoveLinks() {
  let booksContainer = document.getElementById("booksContainer");
  let links = booksContainer.getElementsByClassName("readingLinks");

  for (let i = 0; i < links.length; i++) {
    links[i].innerHTML = "";
  }
}

if ("webkitSpeechRecognition" in window) {
  // Create a new instance of SpeechRecognition
  const recognition = new webkitSpeechRecognition();

  // Set properties
  recognition.continuous = false; // Stops after a single result
  recognition.interimResults = false; // Only final results
  recognition.lang = "en-US"; // Set the language

  // Event handler for when the speech recognition returns a result
  recognition.onresult = (event) => {
    const transcript = event.results[0][0].transcript;
    document.getElementById("bookTextInput").value = transcript; // Insert the text into the input field
  };

  // Error handling
  recognition.onerror = (event) => {
    console.error("Speech recognition error detected: ", event.error);
  };

  // Start recognition when the microphone button is clicked
  document.getElementById("micBtn").addEventListener("click", () => {
    recognition.start();
  });
} else {
  alert("Speech recognition is not supported in this browser.");
}
