const apiBase = window.API_BASE_URL ?? "https://localhost:7192/api/Product";

const productForm = document.getElementById("productForm");
const productId = document.getElementById("productId");
const nameInput = document.getElementById("name");
const priceInput = document.getElementById("price");
const categoryInput = document.getElementById("category");
const rows = document.getElementById("productRows");
const message = document.getElementById("message");
const formTitle = document.getElementById("formTitle");
const cancelBtn = document.getElementById("cancelBtn");

productForm.addEventListener("submit", onSave);
cancelBtn.addEventListener("click", resetForm);

loadProducts();

async function loadProducts() {
    clearMessage();
    try {
        const response = await fetch(apiBase);
        const products = await response.json();
        renderRows(products);
    } catch {
        showMessage("Unable to load products. Make sure backend is running and CORS is enabled.", true);
    }
}

function renderRows(products) {
    rows.innerHTML = "";

    if (!products || products.length === 0) {
        rows.innerHTML = `<tr><td colspan="5">No products found.</td></tr>`;
        return;
    }

    for (const product of products) {
        const row = document.createElement("tr");
        row.innerHTML = `
            <td>${product.id}</td>
            <td>${escapeHtml(product.name)}</td>
            <td>${Number(product.price).toFixed(2)}</td>
            <td>${escapeHtml(product.category)}</td>
            <td>
                <button type="button" data-edit="${product.id}">Edit</button>
                <button type="button" class="danger" data-delete="${product.id}">Delete</button>
            </td>`;
        rows.appendChild(row);
    }

    rows.querySelectorAll("[data-edit]").forEach(btn => {
        btn.addEventListener("click", () => editProduct(btn.dataset.edit));
    });

    rows.querySelectorAll("[data-delete]").forEach(btn => {
        btn.addEventListener("click", () => deleteProduct(btn.dataset.delete));
    });
}

async function onSave(event) {
    event.preventDefault();
    clearMessage();

    const payload = {
        name: nameInput.value.trim(),
        price: Number(priceInput.value),
        category: categoryInput.value.trim()
    };

    const id = productId.value;
    const isEdit = id !== "";
    const url = isEdit ? `${apiBase}/${id}` : apiBase;
    const method = isEdit ? "PUT" : "POST";

    try {
        const response = await fetch(url, {
            method,
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
        });

        if (!response.ok) {
            const error = await tryReadError(response);
            showMessage(error ?? "Save failed.", true);
            return;
        }

        showMessage(isEdit ? "Product updated." : "Product added.");
        resetForm();
        await loadProducts();
    } catch {
        showMessage("Save failed.", true);
    }
}

async function editProduct(id) {
    clearMessage();
    try {
        const response = await fetch(`${apiBase}/${id}`);
        if (!response.ok) {
            showMessage("Product not found.", true);
            return;
        }

        const product = await response.json();
        productId.value = product.id;
        nameInput.value = product.name;
        priceInput.value = product.price;
        categoryInput.value = product.category;

        formTitle.textContent = "Edit Product";
        cancelBtn.classList.remove("hidden");
    } catch {
        showMessage("Unable to load product.", true);
    }
}

async function deleteProduct(id) {
    if (!confirm("Delete this product?")) {
        return;
    }

    clearMessage();
    try {
        const response = await fetch(`${apiBase}/${id}`, { method: "DELETE" });
        if (!response.ok) {
            showMessage("Delete failed.", true);
            return;
        }

        showMessage("Product deleted.");
        if (productId.value === id) {
            resetForm();
        }
        await loadProducts();
    } catch {
        showMessage("Delete failed.", true);
    }
}

function resetForm() {
    productForm.reset();
    productId.value = "";
    formTitle.textContent = "Add Product";
    cancelBtn.classList.add("hidden");
}

function showMessage(text, isError = false) {
    message.textContent = text;
    message.classList.toggle("error", isError);
}

function clearMessage() {
    message.textContent = "";
    message.classList.remove("error");
}

function escapeHtml(value) {
    if (value === null || value === undefined) return "";
    return String(value)
        .replaceAll("&", "&amp;")
        .replaceAll("<", "&lt;")
        .replaceAll(">", "&gt;")
        .replaceAll('"', "&quot;")
        .replaceAll("'", "&#039;");
}

async function tryReadError(response) {
    try {
        const payload = await response.json();
        if (payload?.message) return payload.message;
        if (payload?.title) return payload.title;
    } catch {
        return null;
    }
    return null;
}
