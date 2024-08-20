
function GetUsers() {
    let api = "https://localhost:7063/api/User/AllUsersWithBookCount"; // כתובת ה-API הנכונה
    ajaxCall("GET", api, null, GetUsersSCB, GetUsersECB);
}

// Success Callback
function GetUsersSCB(allUsers) {
    let tableBody = document.querySelector("#userTable tbody");
    tableBody.innerHTML = ""; // נקה את השורות הקיימות

    allUsers.forEach(user => {
        let row = document.createElement("tr");

        row.innerHTML = `
            <td>${user.id || 'N/A'}</td>
            <td>${user.name || 'N/A'}</td>
            <td>${user.email || 'N/A'}</td>
            <td><img src="${user.profilePictureUrl}" alt="Profile Picture" width="50" height="50" /></td>
            <td>${user.isActive ? 'Yes' : 'No'}</td>
            <td>${user.totalBooksPurchased || 0}</td>
        `;

        tableBody.appendChild(row);
    });
}


// Error Callback
function GetUsersECB(ERROR) {
    alert("ERROR: " + ERROR);
}

// Initial fetch of users
GetUsers();
