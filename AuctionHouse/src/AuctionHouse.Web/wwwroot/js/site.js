(function () {
    function renderCountdown(element) {
        var end = element.getAttribute("data-end");
        if (!end) {
            return;
        }

        var endTime = new Date(end).getTime();
        if (Number.isNaN(endTime)) {
            element.textContent = "Invalid end time";
            return;
        }

        var now = Date.now();
        var diff = endTime - now;

        if (diff <= 0) {
            element.textContent = "Auction Ended";
            element.classList.add("ended");
            return;
        }

        var totalSeconds = Math.floor(diff / 1000);
        var days = Math.floor(totalSeconds / 86400);
        totalSeconds %= 86400;
        var hours = Math.floor(totalSeconds / 3600);
        totalSeconds %= 3600;
        var minutes = Math.floor(totalSeconds / 60);
        var seconds = totalSeconds % 60;

        element.textContent = days + "d " + String(hours).padStart(2, "0") + "h " + String(minutes).padStart(2, "0") + "m " + String(seconds).padStart(2, "0") + "s";
    }

    function startCountdowns() {
        var elements = document.querySelectorAll(".countdown[data-end]");
        if (!elements.length) {
            return;
        }

        elements.forEach(renderCountdown);

        setInterval(function () {
            elements.forEach(renderCountdown);
        }, 1000);
    }

    startCountdowns();
})();
