var controlador = siteRoot + "PotenciaFirmeRemunerable/Configuracion/";

$(document).ready(function () {


    $('#cbAnio').change(function () {
        listadoPeriodo();
    });

    $('#cbPeriodo').change(function () {
        cargarRevisiones();
        //buscarGeneracion();
    });

    $('#cbRevision').change(function () {
        buscarGeneracion();
    });

    buscarGeneracion();
});


//consulta
function buscarGeneracion() {
    var pericodi = parseInt($("#cbPeriodo").val()) || 0;
    var recacodi = parseInt($("#cbRevision").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "ListadoGeneracion",
        data: {
            pfrpercodi: pericodi,
            recacodi: recacodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#listado_generacion').html(evt);
                $('#tabla_generacion').dataTable({
                    "sDom": 'ft',
                    "ordering": true,
                    "iDisplayLength": -1,
                    "order": [[2, "asc"]]
                });
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }

            //$('#listado_generacion').html(evt);
            //$('#tabla_generacion').dataTable({
            //    "sDom": 'ft',
            //    "ordering": true,
            //    "iDisplayLength": -1,
            //    "order": [[2, "asc"]]
            //});
        },
        error: function (err) {
            alert('Ha ocurrido un error: ' + err);
        }
    });
};

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
                        $('#cbPeriodo').get(0).options[$('#cbPeriodo').get(0).options.length] = new Option(item.Pfrpernombre, item.Pfrpercodi);
                    });
                } else {
                    $('#cbPeriodo').get(0).options[0] = new Option("--", "0");
                }

                //buscarGeneracion();
                cargarRevisiones();
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function cargarRevisiones() {

    var pfrpericodi = parseInt($("#cbPeriodo").val()) || 0;

    $("#cbRevision").empty();

    $.ajax({
        type: 'POST',
        url: controlador + "CargarRevisiones",
        data: {
            pfrpercodi: pfrpericodi,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.ListaRecalculo.length > 0) {
                    $.each(evt.ListaRecalculo, function (i, item) {
                        $('#cbRevision').get(0).options[$('#cbRevision').get(0).options.length] = new Option(item.Pfrrecnombre, item.Pfrreccodi);
                    });
                } else {
                    $('#cbRevision').get(0).options[0] = new Option("--", "0");
                }

                //mostrarTabFactorIndisponibilidad();
                buscarGeneracion();
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}
