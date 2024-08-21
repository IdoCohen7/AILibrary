jQuery(document).ready(function ($) {
  if (user != null) {
    LoadMarketplace();
  } else {
    let div = document.getElementById("booksContainer");
    div.innerHTML = "In order to view our marketplace, you need to log in.";
  }
});

function LoadMarketplace() {
  let api = "https://proj.ruppin.ac.il/cgroup75/test2/tar1/api/Book/Market";
  ajaxCall("GET", api, null, LoadMarketplaceSCB, AjaxECB);
}

let marketPlaceArray = [];

function LoadMarketplaceSCB(allBooks) {
  marketPlaceArray = [];

  for (let i = 0; i < allBooks.length; i++) {
    if (allBooks[i].userId != user.id) {
      marketPlaceArray.push(allBooks[i]); // Add the book to marketPlaceArray if it meets the condition
    }
  }

  GetBooksSuccess(marketPlaceArray, "booksContainer", AddSellerDetails);
}

function formatDate(dateString) {
  if (!dateString) return "Not available";
  const date = new Date(dateString);
  return date.toLocaleDateString(); // Format the date to a readable format without time
}

function AddSellerDetails() {
  let interactionDivs = document.getElementsByClassName("mainInteract");
  for (let i = 0; i < interactionDivs.length; i++) {
    if (i < marketPlaceArray.length) {
      let seller = marketPlaceArray[i].username || "Unknown seller";
      let finishedDate = marketPlaceArray[i].finishedDate;

      // Create the HTML content
      let content = document.createElement("p");
      content.innerHTML = `Up for sale by ${seller}<br>Finished reading on ${finishedDate}`;
      content.style.fontSize = "11px";
      content.classList.add("game-card__genre");

      // Create the "request to buy" link
      let requestLink = document.createElement("a");
      requestLink.textContent = "Request";
      requestLink.addEventListener("click", function () {
        let api =
          "https://proj.ruppin.ac.il/cgroup75/test2/tar1/api/User/SendRequest?buyerId=" +
          user.id +
          "&sellerId=" +
          marketPlaceArray[i].userId +
          "&bookId=" +
          marketPlaceArray[i].id;
        ajaxCall("POST", api, null, SendRequestSCB, AjaxECB);
      });
      requestLink.classList.add("request-to-buy"); // Add a class for styling if needed

      // Append the link to the content
      interactionDivs[i].innerHTML = "";
      interactionDivs[i].appendChild(content);
      interactionDivs[i].appendChild(requestLink);
    } else {
      console.warn("No corresponding book for interactionDiv index:", i);
    }
  }
}

function SendRequestSCB(Message) {
  if (Message == -1) {
    alert("Error: Book already owned / request already sent");
  } else
    alert(
      "Request sent, if the seller accepts, book will appear in your library"
    );
}
