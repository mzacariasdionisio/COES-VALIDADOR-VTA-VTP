var markers = [];
var map;


iniciarMapa = function () {

    google.maps.event.addDomListener(window, 'load', initAutocomplete);
}

function iniciarMapa() {
    //Creamos el punto a partir de las coordenadas:
    var punto = new google.maps.LatLng(-22.187404991398775, -72.9052734375);

    //Configuramos las opciones indicando Zoom, punto(el que hemos creado) y tipo de mapa
    var myOptions = {
        zoom: 5,
        center: punto,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    //Creamos el mapa e indicamos en qué caja queremos que se muestre
    map = new google.maps.Map($("#mostrarMapa")[0], myOptions);
    map.setOptions({ draggableCursor: "url(../../../content/images/ubicador.gif) 25 25, default" });


    if ($('#txtCoordenadaX').val() != "" && $('#txtCoordenadaY').val() != "") {
       
        var latEquipo = { lat: parseFloat($('#txtCoordenadaX').val()), lng: parseFloat($('#txtCoordenadaY').val()) };
        addMarker(latEquipo);
    }


    map.addListener('mousemove', function (e) {
        $("#latitud").text(e.latLng.lat());
        $("#longitud").text(e.latLng.lng());

    });

    map.addListener('click', function (e) {
        $("#txtCoordenadaX").val(e.latLng.lat());
        $("#txtCoordenadaY").val(e.latLng.lng());
        deleteMarkers();
        addMarker(e.latLng);
    });
    
    // Create the search box and link it to the UI element.
    var input = document.getElementById('pac-input');
    var searchBox = new google.maps.places.SearchBox(input);
    map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);

    // Bias the SearchBox results towards current map's viewport.
    map.addListener('bounds_changed', function () {
        searchBox.setBounds(map.getBounds());
    });


    // [START region_getplaces]
    // Listen for the event fired when the user selects a prediction and retrieve
    // more details for that place.
    searchBox.addListener('places_changed', function () {
        var places = searchBox.getPlaces();

        if (places.length == 0) {
            return;
        }

        // Clear out the old markers.
        markers.forEach(function (marker) {
            marker.setMap(null);
        });
      
        // For each place, get the icon, name and location.
        var bounds = new google.maps.LatLngBounds();
        places.forEach(function (place) {
            var icon = {
                url: place.icon,
                size: new google.maps.Size(71, 71),
                origin: new google.maps.Point(0, 0),
                anchor: new google.maps.Point(17, 34),
                scaledSize: new google.maps.Size(25, 25)
            };

            // Create a marker for each place.
            markers.push(new google.maps.Marker({
                map: map,
                icon: icon,
                title: place.name,
                position: place.geometry.location
            }));

            if (place.geometry.viewport) {
                // Only geocodes have viewport.
                bounds.union(place.geometry.viewport);
            } else {
                bounds.extend(place.geometry.location);
            }
        });
        map.fitBounds(bounds);
    });

}
