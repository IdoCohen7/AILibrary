$(document).ready(function () {
  let librariesLi = document.getElementById("librariesLi");
  librariesLi.classList.add("uk-active");
});

var searchVal;

document
  .getElementById("searchLibraryBttn")
  .addEventListener("click", function () {
    // Get the selected search type
    let searchType = document.getElementById("searchType").value;

    // Define the API endpoint based on the search type
    let apiEndpoint = "";
    switch (searchType) {
      case "User":
        apiEndpoint = "https://proj.ruppin.ac.il/cgroup75/test2/tar1/api/User";
        searchVal = 1;
        break;
      case "Book":
        apiEndpoint = "https://proj.ruppin.ac.il/cgroup75/test2/tar1/api/Book";
        searchVal = 2;
        break;
      case "Author":
        apiEndpoint =
          "https://proj.ruppin.ac.il/cgroup75/test2/tar1/api/Author";
        searchVal = 3;
        break;
      default:
        console.error("Invalid search type");
        return;
    }

    // Clear the existing options and remove any previous select and button
    let listDiv = document.getElementById("listDiv");
    listDiv.innerHTML = "";

    // Create new select element and button
    let listSelect = document.createElement("select");
    listSelect.classList.add("nice-select");
    listSelect.classList.add("js-select");
    listSelect.id = "list";

    let pickButton = document.createElement("button");
    pickButton.className = "uk-button uk-button-secondary";
    pickButton.id = "pickBttn";
    pickButton.textContent = "Pick";

    pickButton.addEventListener("click", function () {
      LoadData(searchVal, listSelect.value);
    });

    // Append new elements to the listDiv
    listDiv.appendChild(listSelect);
    listDiv.appendChild(pickButton);

    // Use ajaxCall to fetch data from the API
    ajaxCall(
      "GET",
      apiEndpoint,
      null,
      function (response) {
        // Clear existing options from the select element
        listSelect.innerHTML = "";

        // Assuming response is an array of objects with an 'id' and 'name' property
        response.forEach((item) => {
          let option = document.createElement("option");
          option.value = item.id;
          option.textContent = item.name !== undefined ? item.name : item.title;
          listSelect.appendChild(option);
        });
      },
      function (error) {
        console.error("Error fetching data:", error);
      }
    );
  });

function LoadData(val, id) {
  let api;
  switch (val) {
    case 1:
      api =
        "https://proj.ruppin.ac.il/cgroup75/test2/tar1/api/User/LibraryDetails?userId=" +
        id;
      ajaxCall("GET", api, null, FillBookTable, AjaxECB);
      break;

    case 2:
      api =
        "https://proj.ruppin.ac.il/cgroup75/test2/tar1/api/Book/BookLibraryDetails?bookId=" +
        id;
      ajaxCall("GET", api, null, FillUsersTab, AjaxECB);
      break;

    case 3:
      api =
        "https://proj.ruppin.ac.il/cgroup75/test2/tar1/api/Author/AuthorLibraryDetails?authorId=" +
        id;
      ajaxCall("GET", api, null, FillAuthorTab, AjaxECB);
      break;

    default:
      alert("Error occured");
      return;
  }
}

function FillBookTable(data) {
  // Select the tableDiv and clear its content
  let tableDiv = document.getElementById("tableDiv");
  tableDiv.innerHTML = ""; // Clear existing content

  // Check if data is an array
  if (!Array.isArray(data)) {
    console.error("Data is not an array:", data);
    return;
  }

  // Create the table
  let table = document.createElement("table");
  table.id = "userTable";
  table.className = "user-table";

  // Create table header
  let thead = document.createElement("thead");
  let headerRow = document.createElement("tr");

  // Define headers
  let headers = [
    "title",
    "authors",
    "price",
    "thumbnail",
    "rating",
    "category",
    "finishReadingDate",
  ];
  headers.forEach((header) => {
    let th = document.createElement("th");
    th.textContent = header;
    headerRow.appendChild(th);
  });

  thead.appendChild(headerRow);
  table.appendChild(thead);

  // Create table body
  let tbody = document.createElement("tbody");

  data.forEach((item) => {
    let row = document.createElement("tr");

    // Create and append table cells for each property
    let titleCell = document.createElement("td");
    titleCell.textContent = item.title || "N/A";
    row.appendChild(titleCell);

    let authorsCell = document.createElement("td");
    let authors = Array.isArray(item.authors) ? item.authors : [];
    authorsCell.textContent = authors.join(", ") || "N/A";
    row.appendChild(authorsCell);

    let priceCell = document.createElement("td");
    priceCell.textContent = item.price || "N/A";
    row.appendChild(priceCell);

    let thumbnailCell = document.createElement("td");
    let thumbnailImg = document.createElement("img");
    thumbnailImg.src = item.thumbnail || "";
    thumbnailImg.alt = item.title || "No Title";
    thumbnailImg.style.width = "100px";
    thumbnailCell.appendChild(thumbnailImg);
    row.appendChild(thumbnailCell);

    let ratingCell = document.createElement("td");
    ratingCell.textContent = item.ratingAverage || "N/A";
    row.appendChild(ratingCell);

    let categoryCell = document.createElement("td");
    categoryCell.textContent = item.category || "N/A";
    row.appendChild(categoryCell);

    let finishReadingDateCell = document.createElement("td");
    finishReadingDateCell.textContent = item.finishReadingDate || "N/A";
    row.appendChild(finishReadingDateCell);

    tbody.appendChild(row);
  });

  table.appendChild(tbody);

  // Append the table to the tableDiv
  tableDiv.appendChild(table);

  // Initialize DataTables to make the table responsive
  $(document).ready(function () {
    $("#userTable").DataTable({
      responsive: true,
      paging: true,
      searching: true,
      ordering: true,
      destroy: true, // Reinitialize the table if it already exists
    });
  });
}

function FillUsersTab(data) {
  // Select the tableDiv and clear its content
  let tableDiv = document.getElementById("tableDiv");
  tableDiv.innerHTML = ""; // Clear existing content

  // Create the table
  let table = document.createElement("table");
  table.id = "userTable";
  table.className = "display"; // Use 'display' class for DataTables styling

  // Create table header
  let thead = document.createElement("thead");
  let headerRow = document.createElement("tr");

  // Define headers
  let headers = ["Name", "Profile Picture", "Email", "Finish Reading Date"];
  headers.forEach((header) => {
    let th = document.createElement("th");
    th.textContent = header;
    headerRow.appendChild(th);
  });

  thead.appendChild(headerRow);
  table.appendChild(thead);

  // Create table body
  let tbody = document.createElement("tbody");

  // Check if data is an array
  if (Array.isArray(data)) {
    data.forEach((item) => {
      let row = document.createElement("tr");

      // Create and append table cells for each property
      let nameCell = document.createElement("td");
      nameCell.textContent = item.name || "N/A";
      row.appendChild(nameCell);

      let profilePicCell = document.createElement("td");
      let profilePicImg = document.createElement("img");
      profilePicImg.src = item.profilePic || "";
      profilePicImg.alt = item.name || "No Name";
      profilePicImg.style.width = "100px"; // Adjust size as needed
      profilePicCell.appendChild(profilePicImg);
      row.appendChild(profilePicCell);

      let emailCell = document.createElement("td");
      emailCell.textContent = item.email || "N/A";
      row.appendChild(emailCell);

      let finishReadingDateCell = document.createElement("td");
      finishReadingDateCell.textContent = item.finishReadingDate || "N/A";
      row.appendChild(finishReadingDateCell);

      tbody.appendChild(row);
    });
  } else {
    console.error("Data is not an array:", data);
  }

  table.appendChild(tbody);

  // Append the table to the tableDiv
  tableDiv.appendChild(table);

  // Initialize DataTables to make the table responsive
  $(document).ready(function () {
    $("#userTable").DataTable({
      responsive: true,
      paging: true,
      searching: true,
      ordering: true,
      destroy: true, // Reinitialize the table if it already exists
    });
  });
}

function FillAuthorTab(data) {
  // Clear the existing content in the tableDiv
  let tableDiv = document.getElementById("tableDiv");
  tableDiv.innerHTML = ""; // Clear existing content

  // Create a table element
  let table = $('<table id="authorTable" class="display"></table>');

  // Define the table headers
  let thead = $(
    "<thead><tr><th>Profile Picture</th><th>Name</th><th>Email</th><th>Book Title</th><th>Thumbnail</th><th>Finish Reading Date</th></tr></thead>"
  );
  table.append(thead);

  // Create table body
  let tbody = $("<tbody></tbody>");

  // Check if data is an array
  if (Array.isArray(data)) {
    data.forEach((item) => {
      let row = $("<tr></tr>");

      // Profile Picture Cell
      let profilePicCell = $("<td></td>");
      if (item.profilePic) {
        let img = $("<img>")
          .attr("src", item.profilePic)
          .attr("alt", item.name || "No Name")
          .css("width", "50px"); // Adjust size as needed
        profilePicCell.append(img);
      } else {
        profilePicCell.text("N/A");
      }

      // Other Cells
      let nameCell = $("<td></td>").text(item.name || "N/A");
      let emailCell = $("<td></td>").text(item.email || "N/A");
      let titleCell = $("<td></td>").text(item.title || "N/A");

      let thumbnailCell = $("<td></td>");
      if (item.thumbnail) {
        let img = $("<img>")
          .attr("src", item.thumbnail)
          .attr("alt", item.title || "No Title")
          .css("width", "100px"); // Adjust size as needed
        thumbnailCell.append(img);
      } else {
        thumbnailCell.text("N/A");
      }

      let finishReadingDateCell = $("<td></td>").text(
        item.finishReadingDate || "N/A"
      );

      // Append cells to the row
      row.append(
        profilePicCell,
        nameCell,
        emailCell,
        titleCell,
        thumbnailCell,
        finishReadingDateCell
      );
      tbody.append(row);
    });
  } else {
    console.error("Data is not an array:", data);
  }

  table.append(tbody);
  tableDiv.appendChild(table[0]);

  // Initialize DataTable for responsive behavior
  $("#authorTable").DataTable({
    responsive: true,
    paging: true,
    searching: true,
    ordering: true,
    destroy: true, // Reinitialize the table if it already exists
  });
}
