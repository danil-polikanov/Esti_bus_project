document.addEventListener("DOMContentLoaded", function () {
    const timeDisplay = document.getElementById('timeDisplay');

    function updateTime() {
        const now = new Date();
        const hours = now.getHours().toString().padStart(2, '0');
        const minutes = now.getMinutes().toString().padStart(2, '0');
        const seconds = now.getSeconds().toString().padStart(2, '0');

        timeDisplay.textContent = `Текущее время: ${hours}:${minutes}:${seconds}`;
    }

    setInterval(updateTime, 1000);
    updateTime(); 

    


    const regionInput = document.getElementById("region-input");
    const regionDropdown = document.getElementById("region-dropdown");
    const submitBtn = document.getElementById("submit-btn");
    const responseMessage = document.getElementById("response-message");
    const regionCurrent = document.getElementById("region-current");
    const stopCurrent = document.getElementById("stop-current");

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
            body: JSON.stringify({ stopName: selectedStop })
        }).then(response => response.json())
            .then(data => {
                stopDropdown.innerHTML = ""; // Очистка предыдущих данных
                const stopIds = data.StopIds; // Массив идентификаторов остановок

                data.RouteShortNames.forEach((bus, index) => {
                    const option = document.createElement("button");
                    option.className = "btn btn-primary mx-2";
                    option.type = "button";
                    option.textContent = bus; 
                    option.id = stopIds[index]; 
                    stopDropdown.appendChild(option);
                });
            })
            .catch(error => {
                console.error("Error fetching route short names:", error);
            });
    });


    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(
            (position) => {
                const latitude = position.coords.latitude;  
                const longitude = position.coords.longitude;
                const accuracy = position.coords.accuracy; 
                fetch(`/api/nearest-stop?latitude=${latitude}&longitude=${longitude}`, {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
                    .then(response => {
                        if (!response.ok) {
                            throw new Error('Failed to fetch nearest stop.');
                        }
                        return response.json();
                    })
                    .then(data => {
                        regionCurrent.textContent = data.Region;
                        stopCurrent.textContent = data.StopName;
                        regionInput.textContent = data.Region;
                        stopInput.textContent = data.StopName;
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
                                            option.type = "button"
                                            option.textContent = bus.name;
                                            stopDropdown.appendChild(option);
                                        });
                                    });
                            }).catch(error => {
                                console.error("Error submitting region:", error);
                                responseMessage.textContent = "Error submitting region.";
                            });

                    });
            });
    };
    document.querySelectorAll('.bus-button').forEach(button => {
        button.addEventListener('click', function () {
            const busNumber = this.textContent.trim();
            const stopId = this.id; // Получите ID выбранной остановки
            fetch(`/Home/GetBusArrivals?busNumber=${busNumber}&stopId=${stopId}`)
                .then(response => response.json())
                .then(data => {
                    displayBusArrivals(data);
                })
                .catch(error => {
                    console.error("Error fetching bus arrivals:", error);
                });
        });
    });

    function displayBusArrivals(data) {
        const directionElement = document.getElementById("bus-direction");
        const scheduleTableBody = document.querySelector("#bus-schedule tbody");
        const errorMessage = document.getElementById("error-message");

        directionElement.textContent = "";
        scheduleTableBody.innerHTML = "";
        errorMessage.style.display = "none";

        if (!data || !data.Arrivals || data.Arrivals.length === 0) {
            errorMessage.textContent = "No bus arrivals found for this route.";
            errorMessage.style.display = "block";
            return;
        }

        directionElement.textContent = `Direction: ${data.Direction}`;

        data.Arrivals.forEach((arrival) => {
            const row = document.createElement("tr");

            const stopSequenceCell = document.createElement("td");
            stopSequenceCell.textContent = arrival.stopSequence;
            row.appendChild(stopSequenceCell);

            const arrivalTimeCell = document.createElement("td");
            arrivalTimeCell.textContent = arrival.arrivalTime;
            row.appendChild(arrivalTimeCell);

            const departureTimeCell = document.createElement("td");
            departureTimeCell.textContent = arrival.departureTime;
            row.appendChild(departureTimeCell);

            scheduleTableBody.appendChild(row);
        });
    }



    document.getElementById('clear-button').addEventListener('click', function () { clearFields(); });
    function clearFields() {
        document.querySelectorAll('input')
            .forEach(input => input.value = '');
        document.getElementById('arrivals-list').innerHTML = "";
    }
});