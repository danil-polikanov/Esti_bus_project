document.addEventListener("DOMContentLoaded", function () {
    const regionInput = document.getElementById("region-input");
    const regionDropdown = document.getElementById("region-dropdown");
    const submitBtn = document.getElementById("submit-btn");
    const responseMessage = document.getElementById("response-message");

    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(
            (position) => {
                const latitude = position.coords.latitude;  // Широта
                const longitude = position.coords.longitude; // Долгота
                const accuracy = position.coords.accuracy; // Точность в метрах
            });

        
        };
    regionInput.addEventListener("input", function () {
        const query = regionInput.value.trim();
        fetch(`/Home/GetRegions?query=${encodeURIComponent(query)}`)
            .then(response => response.json())
            .then(data => {
                regionDropdown.innerHTML = "";
                if (data.length === 0) {
                    const noResultOption = document.createElement("li");
                    noResultOption.textContent = "No finding";
                    regionDropdown.appendChild(noResultOption);
                } else {
                    data.forEach(region => {
                        const option = document.createElement("li");
                        const optionInside = document.createElement("a");
                        optionInside.className = "dropdown-item";
                        optionInside.value = region.id;
                        optionInside.textContent = region.name;
                        option.appendChild(optionInside);
                        regionDropdown.appendChild(option);
                    });
                }
            })
            .catch(error => {
                console.error("Error fetching regions:", error);
            });
    });
    const ulRegionElement = document.getElementsByClassName('region-dropdown')[0];
    if (ulRegionElement.querySelectorAll('li').length != 0) {
        ulRegionElement.querySelectorAll('li').forEach(li => {
            li.addEventListener('click', function () {
                regionInput.value = li.value;
                regionInput.textContent = li.textContent;
            });
        });
    };


    const stopInput = document.getElementById("stop-input");
    const stopDropdown = document.getElementById("stop-dropdown");
    const submitStopBtn = document.getElementById("submit-stop-btn");
    const responseStopMessage = document.getElementById("response-message");

    submitBtn.addEventListener("click", function () {
        const selectedRegion = regionInput.textContent;
        if (!selectedRegion)
        {
            responseMessage.textContent = "Please select or enter a region.";
            return;
        }
        fetch(`/Home/SubmitRegion`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(selectedRegion)
        }).then(response => response.json())
            .then(data => {
                stopDropdown.innerHTML = "";
                if (data.length === 0) {
                    const noResultOption = document.createElement("li");
                    noResultOption.textContent = "No finding";
                    stopDropdown.appendChild(noResultOption);
                } else {
                    data.forEach(stop => {
                        const option = document.createElement("li");
                        const optionInside = document.createElement("a");
                        optionInside.className = "dropdown-item";
                        optionInside.value = stop.id;
                        optionInside.textContent = stop.name;
                        option.appendChild(optionInside);
                        stopDropdown.appendChild(option);
                    });
                }
            }).catch(error => {
                console.error("Error submitting region:", error);
                responseMessage.textContent = "Error submitting region.";
            });
    });

    stopInput.addEventListener('click', function () {
        const stops = stopDropdown.getElementsByTagName('li');
        for (let i = 0; i < stops.length; i++) {
            const stopName = stops[i].textContent.toLowerCase();
            if (stopName.includes(stopInput)) {
                stops[i].classList.remove('hidden');
            } else {
                stops[i].classList.add('hidden');
            }
        }
    });
    const ulStopElement = document.getElementsByClassName('stop-dropdown')[0];
    if (ulStopElement.querySelectorAll('li').length != 0) {
        ulStopElement.querySelectorAll('li').forEach(li => {
            li.addEventListener('click', function () {
                stopInput.value = li.value;
                stopInput.textContent = li.textContent;
            });
        });
    };

    submitStopBtn.addEventListener("click", function () {
        const selectedStop = stopInput.textContent;
        if (!selectedStop) {
            responseStopMessage.textContent = "Please select or enter a stop.";
            return;
        }
        fetch(`/Home/GetRouteShortNames`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(selectedStop)
        }).then(response => response.json())
            .then(data => {
                data.forEach(bus => {
                    const option = document.createElement("button");
                    option.className = "btn btn-primary mx-2";
                    option.type="button"
                    option.textContent = bus.name;
                    stopDropdown.appendChild(option);
                });
            });
    });


});
