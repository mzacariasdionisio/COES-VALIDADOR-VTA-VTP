var markers = [];
var map;

$(function () {
    mostrarGoogleMaps();
});

mostrarGoogleMaps= function() {
    //Creamos el punto a partir de las coordenadas:
    var punto = new google.maps.LatLng(-12.104832, -77.0384679);

    //Configuramos las opciones indicando Zoom, punto(el que hemos creado) y tipo de mapa
    var myOptions = {
        zoom: 14,
        center: punto,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    //Creamos el mapa e indicamos en qué caja queremos que se muestre
    map = new google.maps.Map($("#mostrarMapa")[0], myOptions);
    map.setOptions({ draggableCursor: "url(../../content/images/ubicador.gif) 25 25, default" });
    
    map.addListener('mousemove', function (e) {
        $("#latitud").text(e.latLng.lat());
        $("#longitud").text(e.latLng.lng());
               
    });

    map.addListener('click', function (e) {
        $("#latitud").text(e.latLng.lat());
        $("#longitud").text(e.latLng.lng());

        deleteMarkers();
        addMarker(e.latLng);
    });
}


function addMarker(location) {
    var marker = new google.maps.Marker({
        position: location,
        map: map
    });
    markers.push(marker);
}

function setMapOnAll(map) {
    for (var i = 0; i < markers.length; i++) {
        markers[i].setMap(map);
    }
}

function clearMarkers() {
    setMapOnAll(null);
}

function showMarkers() {
    setMapOnAll(map);
}

function deleteMarkers() {
    clearMarkers();
    markers = [];
}