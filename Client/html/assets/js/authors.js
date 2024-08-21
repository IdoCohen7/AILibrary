$(document).ready(function () {
  GetAuthors();
});

function GetAuthors() {
  let api = "https://localhost:7063/api/Author";
  ajaxCall("GET", api, null, GetAuthorsSCB, GetAuthorsECB);
}

function GetAuthorsSCB(allAuthors) {
  let div = document.getElementById("mainDiv");
  let headOne = document.createElement("h1");
  headOne.innerHTML = "Our site features creations by:";
  headOne.classList.add("uk-text-lead");
  let remainingAuthors = allAuthors.length;
  let authorsList = [];
  let authorPromises = [];

  allAuthors.forEach((author, index) => {
    const authorName = encodeURIComponent(author.name);

    // Create promises for both Wikipedia and Open Library API calls
    const wikiUrl = `https://en.wikipedia.org/w/api.php?action=query&prop=extracts|pageimages&format=json&titles=${authorName}&exintro=1&origin=*`;
    const wikiPromise = new Promise((resolve) => {
      ajaxCall(
        "GET",
        wikiUrl,
        null,
        (data) => {
          const page = Object.values(data.query.pages)[0];
          const biography = page.extract || "No biography available.";
          const imageUrl = page.thumbnail ? page.thumbnail.source : null;

          // filter biographies that are not related to one person
          const isDisambiguation =
            biography.includes("may refer to") ||
            biography.includes("disambiguation") ||
            biography.includes("is the name of");

          resolve({
            biography: isDisambiguation ? "No biography available." : biography,
            imageUrl,
          });
        },
        () => {
          resolve({
            biography: "No biography available.",
            imageUrl: null,
          });
        }
      );
    });

    const openLibraryUrl = `https://openlibrary.org/search/authors.json?q=${authorName}`;
    const openLibraryPromise = fetch(openLibraryUrl)
      .then((response) => response.json())
      .then((data) => {
        const doc = data.docs[0];
        return {
          birthDate: doc?.birth_date
            ? `<p><strong>Birth Date:</strong> ${doc.birth_date}</p>`
            : "",
          deathDate: doc?.death_date
            ? `<p><strong>Death Date:</strong> ${doc.death_date}</p>`
            : "",
          topSubjects:
            doc?.top_subjects && doc.top_subjects.length > 0
              ? `<p><strong>Top Subjects:</strong> ${doc.top_subjects.join(
                  ", "
                )}</p>`
              : "",
          topWork: doc?.top_work
            ? `<p><strong>Top Work:</strong> ${doc.top_work}</p>`
            : "",
        };
      })
      .catch((error) => {
        console.error("Error fetching Open Library data:", error);
        return {
          birthDate: "",
          deathDate: "",
          topSubjects: "",
          topWork: "",
        };
      });

    authorPromises.push(
      Promise.all([wikiPromise, openLibraryPromise]).then(
        ([wikiData, openLibraryData]) => {
          const { biography, imageUrl } = wikiData;
          const { birthDate, deathDate, topSubjects, topWork } =
            openLibraryData;

          return `
          <div class="author" style="margin-bottom: 80px;">
            <h3 class="uk-text-lead">${author.name}</h3>
            ${
              imageUrl
                ? `<img src="${imageUrl}" alt="${author.name}" style="max-width: 200px; height: auto;">`
                : ""
            }
            <p>${biography}</p>
            <div class="additional-info">
              ${birthDate}
              ${deathDate}
              ${topSubjects}
              ${topWork}
            </div>
            <button class="uk-button uk-button-secondary" value="${
              author.id
            }">View Books by ${author.name}</button>
          </div>`;
        }
      )
    );
  });

  Promise.all(authorPromises).then((authorsHtml) => {
    div.appendChild(headOne);
    $(".page-main").append(authorsHtml.join(""));

    // Now attach event listeners to the buttons
    let buttons = document.querySelectorAll(".author button");
    buttons.forEach((button) => {
      button.addEventListener("click", function () {
        ShowBooksWrittenBy(button.value);
      });
    });
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
  // clear main div
  const mainDiv = document.getElementById("mainDiv");
  mainDiv.innerHTML = "";

  // add return button
  const returnButton = document.createElement("button");
  returnButton.textContent = "Return";
  returnButton.style.margin = "20px";
  returnButton.classList.add("uk-button");
  returnButton.classList.add("uk-button-secondary");
  returnButton.onclick = () => {
    window.location.href = "09_authors.html";
  };

  // Append return button
  mainDiv.appendChild(returnButton);

  // Create a new booksContainer div
  const booksContainer = document.createElement("div");
  booksContainer.className =
    "uk-grid uk-child-width-1-6@xl uk-child-width-1-4@l uk-child-width-1-3@s uk-flex-middle uk-grid-small";
  booksContainer.setAttribute("data-uk-grid", "");
  booksContainer.id = "booksContainer";

  // Append the booksContainer to the mainDiv
  mainDiv.appendChild(booksContainer);

  // Display books in the new booksContainer div
  GetBooksSuccess(booksByAuthor, "booksContainer");
}

function ShowBooksWrittenByECB(Error) {
  alert("Error: " + Error);
}
