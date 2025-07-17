var controlador = siteRoot + "mediciones/generacionrer/";
var FORMATO_DIARIO;

$(function () {

    $('#cbHorizonte').change(function () {
        horizonte();
        consultarDatos();
    });
    FORMATO_DIARIO = $('#cbHorizonte').val();
    horizonte();

    $('#txtFecha').Zebra_DatePicker({
        onSelect: function () {
            consultarDatos();
        }
    });
    $('#cbAnio').Zebra_DatePicker({
        format: 'Y',
        onSelect: function () {
            cargarSemanaAnho($('#cbAnio').val());
        }
    });

    $('#cbSemana').change(function () {
        mostrarFechas();
        consultarDatos();
    });
    mostrarFechas();
    consultarDatos();

});

horizonte = function () {

    if ($('#cbHorizonte').val() == FORMATO_DIARIO) {
        $('.cntFecha').css("display", "table-cell");
        $('.cntSemana').css("display", "none");
        $('#fechasSemana').css("display", "none");
    } else {
        $('.cntFecha').css("display", "none");
        $('.cntSemana').css("display", "table-cell");
        $('#fechasSemana').css("display", "table-cell");
    }
};

mostrarFechas = function () {
    if ($('#cbSemana').val() != "" && $('#cbAnio').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerfechasanio',
            dataType: 'json',
            data: { nroSemana: $('#cbSemana').val(), anio: $('#cbAnio').val() },
            success: function (result) {
                $('#txtFechaInicio').text(result.FechaInicio);
                $('#txtFechaFin').text(result.FechaFin);
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        $('#txtFechaInicio').text("");
        $('#txtFechaFin').text("");
    }
}

consultarDatos = function () {

    $.ajax({
        type: 'POST',
        url: controlador + "cumplimiento",
        data: {
            horizonte: $('#cbHorizonte').val(), fecha: $('#txtFecha').val(), nroSemana: $('#cbSemana').val(), anio: $('#cbAnio').val()
        },
        success: function (evt) {
            $('#listado').html(evt);
            $('#tabla_reporte').dataTable({
                "scrollY": 520,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "bPaginate": false,
                "iDisplayLength": -1,
                "language": {
                    "emptyTable": "¡No existen registros!"
                }
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

ejecutarProceso = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "EjecutarProcesoEnvio",
        dataType: 'json',
        data: {
        },
        success: function (model) {
            if (model.Resultado != "-1") {
                alert("Proceso ejecutado correctamente.");
            } else {
                alert("Ha ocurrido un error: " + model.Mensaje);
            }
        },
        error: function () {
            mostrarError()
        }
    });
}

function cargarSemanaAnho(anho) {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarSemanas',
        async: false,
        data: {
            idAnho: anho
        },
        success: function (aData) {
            $("#divSemana").html(aData);

            $('#cbSemana').unbind('change');
            $('#cbSemana').change(function () {
                consultarDatos();
                mostrarFechas();
            });

            consultarDatos();
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}