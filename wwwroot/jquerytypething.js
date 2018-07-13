$(document).ready(function () {
	//your code here
	$("#transactionTable").tabulator({
		height: 205,
		layout: "fitColumns",
		columns: [
			{ title: "id", field: "id", editor: "input", width: 150 },
			{ title: "userID", field: "userID", editor: "input", width: 150 },
			{ title: "categoryID", field: "categoryID", width: 160, editor: "input" },
			{ title: "amount", field: "amount", width: 90, editor: "input" },
			{ title: "description", field: "description", align: "center", width: 170, editor: "input" },
			{ title: "dateOfPurchase", field: "dateOfPurchase", width: 200, editor: "input" },
			{ title: "update/delete", align: "center" }
		],
		rowClick: function (e, row) {

			if (confirm("save?")) {
				putTransactions(row);
			} else if (confirm("delete?")) {
				deleteTransactions(row);
			}

		},
	});



});

function putTransactions(row) {
	var xhr = new XMLHttpRequest();


	xhr.open("PUT", "https://localhost:5001/api/users/" + row.getData().userID + "/transactions/" + row.getData().id, true);
	xhr.setRequestHeader("Content-type", "application/json");
	xhr.withCredentials = true;
	xhr.send(JSON.stringify(row.getData()));

	xhr.onreadystatechange = function () {
		if (this.readyState == 4 && this.status == 200) {
			alert("transaction updated");
		}

		if (this.status >= 400) {
			alert("transaction failed to update");
		}
	};
}

function deleteTransactions(row) {
	var xhr = new XMLHttpRequest();


	xhr.open("DELETE", "https://localhost:5001/api/users/" + row.getData().userID + "/transactions/" + row.getData().id, true);
	xhr.setRequestHeader("Content-type", "application/json");
	xhr.withCredentials = true;
	console.log(row.getData())
	xhr.send();

	xhr.onreadystatechange = function () {
		if (this.readyState == 4 && this.status == 200) {
			alert("transaction updated");
		}

		if (this.status >= 400) {
			alert("transaction failed to update");
		}
	};
}

