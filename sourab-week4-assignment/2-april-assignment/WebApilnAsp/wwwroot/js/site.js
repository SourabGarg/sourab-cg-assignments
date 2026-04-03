(function () {
    function getToken() {
        return sessionStorage.getItem("jwt");
    }

    function getAuthHeaders() {
        const token = getToken();
        return token ? { Authorization: `Bearer ${token}` } : {};
    }

    window.appAuth = {
        getToken: getToken,
        getAuthHeaders: getAuthHeaders
    };

    if (!window.jQuery) {
        return;
    }

    $.ajaxSetup({
        beforeSend: function (xhr, settings) {
            const token = getToken();
            if (!token) {
                return;
            }

            const requestUrl = settings && settings.url ? settings.url.toString() : "";
            const isApiRequest = requestUrl.startsWith("/api/") ||
                requestUrl.startsWith("api/") ||
                requestUrl.indexOf(`${window.location.origin}/api/`) === 0;

            if (isApiRequest) {
                xhr.setRequestHeader("Authorization", `Bearer ${token}`);
            }
        }
    });
})();
