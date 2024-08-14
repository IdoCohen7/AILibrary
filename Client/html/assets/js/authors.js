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
  let booksAuthorContainer = document.createElement("div");

  // Display books in the booksContainer div
  GetBooksSuccess(booksByAuthor, "booksContainer");

  // Create and add return button
  const returnButton = document.createElement("button");
  returnButton.textContent = "Return";
  returnButton.onclick = () => {
    window.location.href = "13_community.html";
  };

  // Add the return button to the booksAuthorContainer
  booksAuthorContainer.appendChild(returnButton);

  // Select the mainDiv and booksContainer
  const mainDiv = document.getElementById("mainDiv");
  const booksContainer = document.getElementById("booksContainer");

  // Clear all content in mainDiv except for booksContainer
  if (booksContainer) {
    // Store the booksContainer's content
    const booksContainerContent = booksContainer.innerHTML;

    // Clear mainDiv
    mainDiv.innerHTML = "";

    // Reinsert booksContainer and its content
    mainDiv.innerHTML = booksContainerContent;
    mainDiv.appendChild(booksAuthorContainer);
  } else {
    // If booksContainer does not exist, just clear the mainDiv and add the booksAuthorContainer
    mainDiv.innerHTML = "";
    mainDiv.appendChild(booksAuthorContainer);
  }
}

function ShowBooksWrittenByECB(Error) {
  alert("Error: " + Error);
}
GetAuthors();
