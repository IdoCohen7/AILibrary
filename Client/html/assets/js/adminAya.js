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

  html2canvas(table)
    .then((canvas) => {
      const imgData = canvas.toDataURL("image/png");
      const imgWidth = 210; // A4 width in mm
      const pageHeight = 295; // A4 height in mm
      const imgHeight = (canvas.height * imgWidth) / canvas.width;
      let heightLeft = imgHeight;

      let position = 0;

      doc.addImage(imgData, "PNG", 0, position, imgWidth, imgHeight);
      heightLeft -= pageHeight;

      while (heightLeft > 0) {
        position -= pageHeight;
        doc.addPage();
        doc.addImage(imgData, "PNG", 0, position, imgWidth, imgHeight);
        heightLeft -= pageHeight;
      }

      doc.save("Users.pdf");
    })
    .catch((error) => {
      console.error("Error generating PDF:", error);
    });
}

function GetUsers() {
  let api = "https://localhost:7063/api/User";
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

// Initial fetch of users

function GetUsersLibraries(userId) {
  let api = "https://localhost:7063/api/User/LibraryDetails/" + userId;
  ajaxCall("GET", api, null, GetUserLibrariesSCB, AjaxECB);
}

function GetUserLibrariesSCB(data) {
  console.log(data);
}

GetUsers();

function BanUser(id) {
  let api = "https://localhost:7063/api/User/BanUser?userId=" + id;
  ajaxCall("PUT", api, null, BanUserSCB, AjaxECB);
}

function BanUserSCB(status) {
  alert("User is banned");
}

function UnbanUser(id) {
  let api = "https://localhost:7063/api/User/UnbanUser?userId=" + id;
  ajaxCall("PUT", api, null, UnbanUserSCB, AjaxECB);
}

function UnbanUserSCB(status) {
  alert("User is unbanned");
}
