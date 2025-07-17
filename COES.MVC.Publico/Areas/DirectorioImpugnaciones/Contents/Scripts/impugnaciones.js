var controlador = siteRoot + 'DirectorioImpugnaciones/Impugnaciones/';

$(function () {
    obtenerListaImpugnaciones();

    $('#btnBuscar').click(function () {
        obtenerListaImpugnaciones();
    });
});

obtenerListaImpugnaciones = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListaImpugnaciones',
        data: {
            tipoImpugnacion: tipoImpugnacion,
            fecAnio: $('#fecAnio').val(),
            fecMes: $('#fecMes').val()
        },
        success: function (evt){
            $('#listaImpugnaciones').html(evt);
            $('#tablaImpugnaciones').dataTable({
                paging: false,                
                "columns": [
                    { "orderable": false }, null, null, null, null, null, null, null, null, null, null, null, null
                ],
                "info": false,
                "iDisplayLength": 50
            });
            obtenerCuadroEstadistico();
        }
    });
}

obtenerCuadroEstadistico = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'CuadroEstadistico',
        data: {
            maximo: $('#maximo').val(),
            minimo: $('#minimo').val(),
            promedio: $('#promedio').val()
        },
        success: function (evt) {
            $('#cuadroEstadistico').html(evt);
        }
    });
}

downloadBlob = function (url) {
    document.location.href = controlador + "Download?id=" + url;
}