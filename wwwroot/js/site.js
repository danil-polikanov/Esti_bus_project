document.addEventListener("DOMContentLoaded", function () {
    const timeDisplay = document.getElementById('timeDisplay');

    function updateTime() {
        const now = new Date();
        const hours = now.getHours().toString().padStart(2, '0');
        const minutes = now.getMinutes().toString().padStart(2, '0');
        const seconds = now.getSeconds().toString().padStart(2, '0');

        timeDisplay.textContent = `${hours}:${minutes}:${seconds}`;
    }

    setInterval(updateTime, 1000);
    updateTime(); 

    const regionInput = document.getElementById("region-input");
    const regionDropdown = document.getElementById("region-dropdown");
    const submitBtn = document.getElementById("submit-btn");
    const responseMessage = document.getElementById("response-message");
    const regionCurrent = document.getElementById("region-current");
    const stopCurrent = document.getElementById("stop-current");
    const ulRegionElement = document.getElementsByClassName('region-dropdown')[0]

    const busNumber = document.getElementById("bus-group");

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
                        optionInside.addEventListener('click', function () {
                            regionInput.value = optionInside.textContent;
                        });
                        option.appendChild(optionInside);
                        regionDropdown.appendChild(option);
                      
                    });
                }
            })
            .catch(error => {
                console.error("Error fetching regions:", error);
            });
    });
    const stopInput = document.getElementById("stop-input");
    const stopDropdown = document.getElementById("stop-dropdown");
    const submitStopBtn = document.getElementById("submit-stop-btn");
    const responseStopMessage = document.getElementById("response-message");
    const ulStopElement = document.getElementsByClassName('stop-dropdown')[0];
    submitBtn.addEventListener("click", function () {
        const selectedRegion = regionInput.value;
        if (!selectedRegion) {
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
                        optionInside.value = stop.stopId;
                        optionInside.textContent = stop.name;
                        optionInside.addEventListener('click', function () {
                            stopInput.value = optionInside.textContent;
                            bus_stop_id = optionInside.value

                        });
                        option.appendChild(optionInside);
                        stopDropdown.appendChild(option);              
                    });
                }
            }).catch(error => {
                console.error("Error submitting region:", error);
                responseMessage.textContent = "Error submitting region.";
            });
        regionCurrent.textContent = regionInput.value;
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


    submitStopBtn.addEventListener("click", function () {
        const selectedStop = stopInput.value;
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
                const routes = data.routes;
                const stopId = data.stopId;
                busNumber.innerHTML = "";
                for (var key in routes) {
                    if (routes.hasOwnProperty(key)) {
                        const option = document.createElement("button");
                        option.classList.add("btn");
                        option.classList.add("btn-primary");
                        option.classList.add("mx-2");
                        option.type = "button";
                        option.value = key;
                        option.textContent = routes[key];
                        option.addEventListener('click', function () {
                            fetch(`/Home/GetBusArrivalsWithDirection?busNumber=${option.textContent}&routeId=${option.value}&time=${timeDisplay.textContent}&stopId=${stopId}`)
                                .then(response => response.json())
                                .then(data => {
                                    displayBusArrivals(data);
                                })
                                .catch(error => {
                                    console.error("Error fetching bus arrivals:", error);
                                });
                        });
                        busNumber.appendChild(option)
                    }

                };
            })
            .catch(error => {
                console.error("Error fetching route short names:", error);
            });
        stopCurrent.textContent = stopInput.value;
    });


    function displayBusArrivals(data) {
        const scheduleTableBody = document.querySelector("#bus-schedule tbody");
        const errorMessage = document.getElementById("error-message");
        scheduleTableBody.innerHTML = "";
        errorMessage.style.display = "none";

        if (data.length < 1) {
            errorMessage.textContent = "No bus arrivals found for this route.";
            errorMessage.style.display = "block";
            return;
        }

        data.forEach((arrival) => {
            const row = document.createElement("tr");
            row.id = "bus-list";
            const busNum = document.createElement("td");
            busNum.textContent = arrival.bus;
            row.appendChild(busNum);

            const arrivalTimeCell = document.createElement("td");
            arrivalTimeCell.textContent = arrival.arrivals;
            row.appendChild(arrivalTimeCell);

            const trip = document.createElement("td");
            trip.textContent = arrival.tripName;
            row.appendChild(trip);

            scheduleTableBody.appendChild(row);
        });
        

        document.getElementById('clear-button').addEventListener('click', function () { clearFields(); });
        function clearFields() {
            document.querySelectorAll('input')
                .forEach(input => input.value = '');
            regionCurrent.textContent == "";
            stopCurrent.textContent == "";
            const bus_list = document.querySelector("#bus-schedule tbody");
            if (bus_list.hasChildNodes()) {
                while (bus_list.firstChild) {
                    bus_list.removeChild(bus_list.firstChild);
                }
            }
            if (busNumber.hasChildNodes) {
                while (busNumber.firstChild) {
                    busNumber.removeChild(busNumber.firstChild);
                }
            }


        }
    }
})