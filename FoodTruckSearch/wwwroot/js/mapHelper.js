window.mapHelper = {
    init: function (lat, lng) {
        if (!lat || !lng) {
            lat = 37.7749; lng = -122.4194; // default San Francisco
        }
        var map = L.map('map').setView([lat, lng], 13);
        window._lastMap = map;

        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '© OpenStreetMap contributors'
        }).addTo(map);

        return map;
    },
    addMarker: function (lat, lng, text) {
        L.marker([lat, lng]).addTo(window._lastMap)
            .bindPopup(text || 'Food Truck');
    },
    setMapRef: function () {
        window._lastMap = L.map('map');
    }
};
