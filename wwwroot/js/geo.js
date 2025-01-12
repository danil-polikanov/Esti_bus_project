document.addEventListener('DOMContentLoaded', () => {
    const regionInput = document.getElementById("region-input");
    const regionDropdown = document.getElementById("region-dropdown");
    const submitBtn = document.getElementById("submit-btn");
    const responseMessage = document.getElementById("response-message");
    const regionCurrent = document.getElementById("region-current");
    const stopCurrent = document.getElementById("stop-current");
    const stopInput = document.getElementById("stop-input");
    const stopDropdown = document.getElementById("stop-dropdown");
    const submitStopBtn = document.getElementById("submit-stop-btn");
    const responseStopMessage = document.getElementById("response-message");
    const busNumber = document.getElementById("bus-group");

    bus_stop_id = "";
    function calculateDistance(
        lat1, lon1, lat2, lon2) {
        const EarthRadiusKm = 6371;
        function toRadians(degrees) {
            return degrees * Math.PI / 180;
        }

        const dLat = toRadians(lat2 - lat1);
        const dLon = toRadians(lon2 - lon1);
        const a = Math.pow(Math.sin(dLat / 2), 2) +
            Math.cos(toRadians(lat1)) * Math.cos(toRadians(lat2)) *
            Math.pow(Math.sin(dLon / 2), 2);
        const c = 2 * Math.asin(Math.sqrt(a));
        return EarthRadiusKm * c;
    }

     navigator.geolocation.getCurrentPosition(
        (position) => {
            const latitude = position.coords.latitude;
            const longitude = position.coords.longitude;
             const accuracy = position.coords.accuracy;

            fetch(`/Home/GetNearestStop?latitude=${latitude}&longitude=${longitude}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                }
            }).then(response => {
                if (!response.ok) {
                    throw new Error('Failed to fetch nearest stop.');
                }
                return response.json();
            })
                .then(stops => {
                    stops.forEach(stop => {
                        stop.distance = calculateDistance(
                            latitude,
                            longitude,
                            parseFloat(stop.stopLat.replace(',', '.')),
                            parseFloat(stop.stopLon.replace(',', '.'))
                        );
                    });

                    stops.sort((a, b) => a.distance - b.distance);

                    const nearestStop = stops[0];
                    regionCurrent.textContent = nearestStop.stopArea;
                    stopCurrent.textContent = nearestStop.stopName;
                    regionInput.value = nearestStop.stopArea;
                    stopInput.value = nearestStop.stopName;
                    fetch(`/Home/SubmitRegion`, {
                        method: "POST",
                        headers: { "Content-Type": "application/json" },
                        body: JSON.stringify(regionInput.value)
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
                                    });
                                    option.appendChild(optionInside);
                                    stopDropdown.appendChild(option);
                                });
                            }
                            if (!stopInput.value) {
                                responseStopMessage.textContent = "Please select or enter a stop.";
                                return;
                            }
                            fetch(`/Home/GetRouteShortNames`, {
                                method: "POST",
                                headers: { "Content-Type": "application/json" },
                                body: JSON.stringify(stopInput.value)
                            }).then(response => response.json())
                                .then(data => {
                                    console.log('Ответ от сервера:', data);
                                    const routes = data.routes;
                                    const stopId = data.stopId;
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
                                });
                        }).catch(error => {
                            console.error("Error submitting region:", error);
                            responseMessage.textContent = "Error submitting region.";
                        });
                })
                .catch(error => {
                    console.error("Error fetching nearest stop:", error);
                    alert('Failed to fetch nearest stop.');
                });
        },
        (error) => {
            console.error("Error retrieving location:", error);
            alert('Failed to retrieve location.');
        }
    );
    function displayBusArrivals(data) {
        const scheduleTableBody = document.querySelector("#bus-schedule tbody");
        const errorMessage = document.getElementById("error-message");
        scheduleTableBody.innerHTML = "";
        errorMessage.style.display = "none";

        if (data.length <1) {
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
    }

}); 