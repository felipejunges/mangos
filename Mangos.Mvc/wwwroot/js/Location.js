function getPosition(minAccuracy, timeout, clearAfter, callback) {
    if (navigator.geolocation) {

        var dataHora = new Date();

        var options = {
            timeout: timeout * 1000,
            enableHighAccuracy: true,
            maximumAge: 0
        };

        $loading.show();

        var watch = navigator.geolocation.watchPosition(
            function (position) {
                var estorouTimeout = (new Date() - dataHora) > timeout * 1000;

                if (position.coords.accuracy <= minAccuracy || estorouTimeout) {
                    $loading.hide();
                    
                    if (clearAfter) {
                        navigator.geolocation.clearWatch(watch);
                    }

                    callback(position);
                }
            },
            function (error) {
                console.log(error);
            },
            options
        );

    }
}

function distance(lat1, lon1, lat2, lon2) {
    var R = 6371;
    var dLat = (lat2 - lat1) * Math.PI / 180;
    var dLon = (lon2 - lon1) * Math.PI / 180;
    var a = Math.sin(dLat / 2) * Math.sin(dLat / 2)
        + Math.cos(lat1 * Math.PI / 180) * Math.cos(lat2 * Math.PI / 180)
        * Math.sin(dLon / 2) * Math.sin(dLon / 2);
    var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
    var d = R * c;
    return d;
}

function distanceFormated(lat1, lon1, lat2, lon2) {
    var d = distance(lat1, lon1, lat2, lon2);
    if (d > 1) return d.toFixed(1).toString().replace('.', ',') + "km";
    else if (d <= 1) return Math.round(d * 1000) + "m";
    return d;
}
