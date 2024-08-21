$(document).ready(function () {
  let managementLi = document.getElementById("managementLi");
  managementLi.classList.add("uk-active");
});

function exportToExcel() {
  const ws = XLSX.utils.table_to_sheet(document.querySelector("#userTable"));
  const wb = XLSX.utils.book_new();
  XLSX.utils.book_append_sheet(wb, ws, "Users");
  XLSX.writeFile(wb, "Users.xlsx");
}

function exportToPDF() {
  const { jsPDF } = window.jspdf;
  const doc = new jsPDF();

  const table = document.querySelector("#userTable");
  const rows = table.querySelectorAll("tr");

  let yPosition = 20; // Start position for the first row

  // Iterate through each row of the table
  rows.forEach((row) => {
    const cells = row.querySelectorAll("td, th"); // Get all cells (both headers and data cells)
    let rowText = [];

    // Extract the text from each cell and add it to the rowText array
    cells.forEach((cell) => {
      rowText.push(cell.innerText);
    });

    // Add the extracted row to the PDF
    doc.text(rowText.join("  "), 10, yPosition); // Adjust the position (x=10, y=yPosition)
    yPosition += 10; // Move down for the next row

    // Add a new page if the yPosition exceeds the page height (A4 height in mm is 295)
    if (yPosition > 280) {
      doc.addPage();
      yPosition = 20; // Reset y position for the new page
    }
  });

  // Save the generated PDF
  doc.save("Users.pdf");
}

function GetUsers() {
  let api = "https://proj.ruppin.ac.il/cgroup75/test2/tar1/api/User";
  ajaxCall("GET", api, null, GetUsersSCB, GetUsersECB);
}

function GetUsersSCB(allUsers) {
  let tableBody = document.querySelector("#userTable tbody");
  tableBody.innerHTML = "";

  allUsers.forEach((user) => {
    let row = document.createElement("tr");

    row.innerHTML = `
      <td>${user.id || "N/A"}</td>
      <td>${user.name || "N/A"}</td>
      <td>${user.email || "N/A"}</td>
      <td>${user.password || "N/A"}</td>
      <td><img src="${
        user.profilePic || ""
      }" alt="Profile Picture" width="50" height="50" /></td>
      <td>
        <input type="checkbox" ${
          user.isActive ? "checked" : ""
        } data-user-id="${user.id}" />
      </td>
      <td>${user.registrationDate || "N/A"}</td>
    `;

    tableBody.appendChild(row);
  });

  // Initialize the DataTable
  $("#userTable").DataTable({
    responsive: true,
    paging: true,
    searching: true,
    ordering: true,
    destroy: true, // Reinitialize the table if it already exists
  });

  // Add event listener to checkboxes
  document
    .querySelectorAll("#userTable input[type='checkbox']")
    .forEach((checkbox) => {
      checkbox.addEventListener("change", handleCheckboxChange);
    });
}

function handleCheckboxChange(event) {
  const checkbox = event.target;
  const userId = checkbox.getAttribute("data-user-id");

  if (checkbox.checked) {
    // User is being unbanned
    UnbanUser(userId);
  } else {
    // User is being banned
    BanUser(userId);
  }
}

function GetUsersECB(ERROR) {
  alert("ERROR: " + ERROR);
}

GetUsers();

function BanUser(id) {
  let api =
    "https://proj.ruppin.ac.il/cgroup75/test2/tar1/api/User/BanUser?userId=" +
    id;
  ajaxCall("PUT", api, null, BanUserSCB, AjaxECB);
}

function BanUserSCB(status) {
  alert("User is banned");
}

function UnbanUser(id) {
  let api =
    "https://proj.ruppin.ac.il/cgroup75/test2/tar1/api/User/UnbanUser?userId=" +
    id;
  ajaxCall("PUT", api, null, UnbanUserSCB, AjaxECB);
}

function UnbanUserSCB(status) {
  alert("User is unbanned");
}
