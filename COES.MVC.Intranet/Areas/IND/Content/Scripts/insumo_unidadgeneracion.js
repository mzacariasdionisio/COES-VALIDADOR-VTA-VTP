var controlador = siteRoot + 'IND/UnidadGeneracion/';
var LISTA_UNIDAD = [];

$(function () {
    $('#cntMenu').css("display", "none");

    $('#cbAnio').change(function () {
        listadoPeriodo();
    });
    $('#cbPeriodo').change(function () {
        mostrarListado();
    });
    $('#cbFamilia').change(function () {
        mostrarListado();
    });

    $('#btnBuscar').click(function () {
        mostrarListado();
    });

    mostrarListado();
});

function mostrarListado() {
    $('#listado').html('');

    var famcodi = parseInt($("#cbFamilia").val()) || 0;
    var pericodi = parseInt($("#cbPeriodo").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "CargarListaOperacionComercial",
        data: {
            pericodi: pericodi,
            famcodi: famcodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                $('#listado').css("width", ($('#mainLayout').width() - 50) + "px");
                $('#listado').html(evt.Resultado);

                var altotabla = parseInt($('#listado').height()) || 0;
                $('#tabla_data').dataTable({
                    "sPaginationType": "full_numbers",
                    "stripeClasses": [],
                    "destroy": "true",
                    "ordering": false,
                    "searching": true,
                    "iDisplayLength": 15,
                    "info": false,
                    "paging": false,
                    "scrollX": true,
                    "scrollY": altotabla > 355 || altotabla == 0 ? 355 + "px" : "100%"
                });
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function listadoPeriodo() {

    var anio = parseInt($("#cbAnio").val()) || 0;

    $("#cbPeriodo").empty();

    $.ajax({
        type: 'POST',
        url: controlador + "PeriodoListado",
        data: {
            anio: anio,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.ListaPeriodo.length > 0) {
                    $.each(evt.ListaPeriodo, function (i, item) {
                        $('#cbPeriodo').get(0).options[$('#cbPeriodo').get(0).options.length] = new Option(item.Iperinombre, item.Ipericodi);
                    });
                } else {
                    $('#cbPeriodo').get(0).options[0] = new Option("--", "0");
                }

                mostrarListado();
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}
