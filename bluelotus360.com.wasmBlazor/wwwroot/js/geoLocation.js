function getCurrentPosition() {
    return new Promise((resolve, reject) => {
        navigator.geolocation.getCurrentPosition(
            (position) => {
                var locationModel = {
                    Latitude: position.coords.latitude,
                    Longitude: position.coords.longitude,
                    Altitude: position.coords.altitude,
                    Accuracy: position.coords.accuracy,
                    isMock: position.coords.isMock
                };
                resolve(locationModel);
            },
            (error) => {
                reject(error.message);
            }
        );
    });
}