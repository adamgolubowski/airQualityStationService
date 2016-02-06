var map = L.map('map', { zoomControl: false }).setView([53.134699, 23.157905], 13);
L.tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token={accessToken}', {
    attribution: 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, <a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="http://mapbox.com">Mapbox</a>',
    maxZoom: 18,
    id: 'adamgolubowski.og5h8gk5',
    accessToken: 'pk.eyJ1IjoiYWRhbWdvbHVib3dza2kiLCJhIjoiY2lpZ2ljNXNjMDAzYndvbTBvY3M2cmEzbiJ9.muPbMbiGcgo2W7H9I-TEpQ'
}).addTo(map);


function addMarker(id, name, lon, lat) {
    
    //baseUrl = "localhost:60480/";
    //controllerMethod = "stations/details/";
    //url = controllerMethod + id;
    popupContent = '<p>'+name+'</p>';
    //popupContent = '<a href="stations/details/1">this</a>';

    marker = new L.marker([lon, lat])
        .addTo(map)
        .bindPopup(popupContent);
}

addMarker("1","Białystok - Centrum", 53.1346, 23.1579);