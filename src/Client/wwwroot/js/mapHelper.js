window.mapHelper = {
    init: function (lat, lng) {
        if (!lat || !lng) {
            lat = 37.7749; lng = -122.4194;
        }
        if (window._lastMap) {
            window._lastMap.remove();
            window._lastMap = null;
        }
        // var container = document.getElementById('map');
        // if (container) { delete container._leaflet_id; }
        var map = L.map('map').setView([lat, lng], 13);
        window._lastMap = map;
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '© OpenStreetMap contributors'
        }).addTo(map);
    },
    addLocationMarker: function (lat, lng) {
        L.circleMarker([lat, lng], {
            radius: 10,
            fillColor: '#e74c3c',
            color: '#c0392b',
            weight: 2,
            fillOpacity: 0.9
        }).addTo(window._lastMap).bindPopup('Your search location');
    },
    addMarker: function (lat, lng, text) {
        L.marker([lat, lng]).addTo(window._lastMap)
            .bindPopup(text || 'Food Truck');
    },
    getLocation: function () {
        return new Promise(function (resolve, reject) {
            if (!navigator.geolocation) {
                reject('Geolocation not supported');
                return;
            }
            navigator.geolocation.getCurrentPosition(
                function (pos) {
                    resolve({ latitude: pos.coords.latitude, longitude: pos.coords.longitude });
                },
                function (err) {
                    reject(err.message);
                }
            );
        });
    }
};
