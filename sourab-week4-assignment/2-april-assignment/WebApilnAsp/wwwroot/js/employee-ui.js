window.employeeUi = (function () {
    function escapeHtml(value) {
        if (value === null || value === undefined) {
            return "";
        }

        return String(value)
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#39;");
    }

    function normalizeImage(imagePath) {
        return imagePath || "/uploads/default.jpg";
    }

    function fullName(employee) {
        const firstName = employee.firstName || "";
        const lastName = employee.lastName || "";
        const name = `${firstName} ${lastName}`.trim();
        return name || "Employee";
    }

    function buildEmployeeCard(employee, bodyContent) {
        const imagePath = normalizeImage(employee.imagePath || employee.imageUrl);

        return `
            <div class="card employee-detail-card border-0 shadow-lg">
                <div class="card-header employee-card-header">
                    <div>
                        <p class="panel-kicker mb-1">Employee profile</p>
                        <h2 class="h4 mb-0">${escapeHtml(fullName(employee))}</h2>
                    </div>
                </div>
                <div class="card-body">
                    <div class="row g-4 align-items-start">
                        <div class="col-lg-4">
                            <img src="${imagePath}" alt="${escapeHtml(fullName(employee))}" class="employee-detail-image" />
                        </div>
                        <div class="col-lg-8">
                            ${bodyContent}
                        </div>
                    </div>
                </div>
            </div>`;
    }

    function infoItem(label, value) {
        return `
            <div class="info-chip">
                <span class="info-chip-label">${escapeHtml(label)}</span>
                <span class="info-chip-value">${escapeHtml(value)}</span>
            </div>`;
    }

    function buildRoute(template, id) {
        return template.replace("__id__", id);
    }

    function renderAlert(selector, type, message) {
        $(selector).html(`<div class="alert alert-${type}" role="alert">${message}</div>`);
    }

    function readError(xhr, fallbackMessage) {
        if (xhr.responseJSON) {
            if (typeof xhr.responseJSON === "string") {
                return escapeHtml(xhr.responseJSON);
            }

            if (xhr.responseJSON.title) {
                return escapeHtml(xhr.responseJSON.title);
            }
        }

        if (xhr.responseText) {
            return escapeHtml(xhr.responseText);
        }

        return escapeHtml(fallbackMessage);
    }

    function readValidation(xhr) {
        const response = xhr.responseJSON;
        if (response && response.errors) {
            const messages = [];

            Object.keys(response.errors).forEach(function (key) {
                response.errors[key].forEach(function (message) {
                    messages.push(`<li>${escapeHtml(message)}</li>`);
                });
            });

            return `<div>Please fix the following errors:</div><ul class="mb-0 mt-2">${messages.join("")}</ul>`;
        }

        return readError(xhr, "The request could not be completed.");
    }

    function escapeAttribute(value) {
        return escapeHtml(value);
    }

    return {
        buildEmployeeCard: buildEmployeeCard,
        buildRoute: buildRoute,
        escapeAttribute: escapeAttribute,
        escapeHtml: escapeHtml,
        infoItem: infoItem,
        normalizeImage: normalizeImage,
        readError: readError,
        readValidation: readValidation,
        renderAlert: renderAlert
    };
})();
