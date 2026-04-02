document.addEventListener("DOMContentLoaded", function () {
	const list = document.querySelector("#movie-list ul");
	const addForm = document.getElementById("add-movie");
	const input = addForm.querySelector("input[type='text']");

	addForm.addEventListener("submit", function (event) {
		event.preventDefault();

		const movieName = input.value.trim();
		if (!movieName) {
			return;
		}

		const li = document.createElement("li");

		const nameSpan = document.createElement("span");
		nameSpan.className = "name";
		nameSpan.textContent = movieName;

		const deleteSpan = document.createElement("span");
		deleteSpan.className = "delete";
		deleteSpan.textContent = "delete";

		li.appendChild(nameSpan);
		li.appendChild(deleteSpan);
		list.appendChild(li);

		input.value = "";
		input.focus();
	});

	// Use event delegation so delete also works for newly added items.
	list.addEventListener("click", function (event) {
		if (event.target.classList.contains("delete")) {
			const li = event.target.closest("li");
			if (li) {
				li.remove();
			}
		}
	});
});
