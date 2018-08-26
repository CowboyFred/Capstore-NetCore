// Map js
function initialize() {
    var mapOptions = {
        zoom: 16,
        center: new google.maps.LatLng(-36.848450, 174.762192),
        scrollwheel: false
    };
    var map = new google.maps.Map(document.getElementById("map-canvas"),
        mapOptions);
    var myLatLng = new google.maps.LatLng(-36.848450, 174.762192);
    var beachMarker = new google.maps.Marker({
        position: myLatLng,
        map: map
    });
}
google.maps.event.addDomListener(window, 'load', initialize);
