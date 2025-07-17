var controlador = siteRoot + 'PrimasRER/cuadro/';
var ancho = 1000;

$(function () {
    $('#cntMenu').css("display", "none");

    $('#tab-container').easytabs({
        animate: false
    });

    $('#tab-container').bind('easytabs:after', function () {
        refrehDatatable();
    });

    $('#tab-container').easytabs('select', '#vistaVersion');

    $("#btnProcesar").click(function () {
        procesarEvaluacion();
    });

    listarEvaluaciones();

});

function listarEvaluaciones() {
    $('#listado').html('');

    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 1100;
    $('#tab-container').easytabs('select', '#vistaVersion');

    let rerrevcodi = parseInt(window.parent.$("#cbRevisionFiltroGeneral").val()) || 0

    $.ajax({
        type: 'POST',
        url: controlador + "ListarEvaluaciones",
        data: {
            rerrevcodi: rerrevcodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#cantidadEvaluacionValidado').val(evt.CantidadEvaluacionValidado);
                $('#listado').html(evt.Resultado);

                $("#listado").css("width", (ancho) + "px");
                $('#tabla_version_x_revision').dataTable({
                    "sPaginationType": "full_numbers",
                    "destroy": "true",
                    "ordering": false,
                    "searching": false,
                    "iDisplayLength": -1,
                    "info": false,
                    "paging": false,
                    "scrollX": true,
                    "scrollY": "100%"
                });
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function editarEvaluacion(rerevacodi) {
    window.location.href = controlador + "ViewEvaluacion?rerevacodi=" + rerevacodi;
}

function verEvaluacion(rerevacodi) {
    window.location.href = controlador + "ViewEvaluacion?rerevacodi=" + rerevacodi;
}

function descargarEvaluacion(rerevacodi) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ExportaraExcelEvaluacion',
        contentType: 'application/json;',
        data: JSON.stringify({
            rerevacodi: rerevacodi,
        }),
        datatype: 'json',
        success: function (evt) {
            if (evt.Resultado == "-1") {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
            else {
                window.location = controlador + 'abrirarchivo?tipo=' + 1 + '&nombreArchivo=' + evt.Resultado;
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error.");
        }
    });
}

function procesarEvaluacion() {

    let cantidadEvaluacionValidado = parseInt($("#cantidadEvaluacionValidado").val()) || 0;
    if (cantidadEvaluacionValidado > 0)
    {
        if (!confirm("Ya existe una versión con estado 'Validado' para este periodo y revisión 'Mensual'. Si decide continuar, el estado de dicha versión cambiará a 'Generado'. ¿Está seguro que desea continuar?"))
        {
            return;
        }
    }

    let rerrevcodi = parseInt($("#rev_rerrevcodi").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + 'ProcesarEvaluacion',
        data: {
            rerrevcodi: rerrevcodi
        },
        cache: false,
        success: function (result) {
            if (result.Resultado == '-1') {
                alert('Ha ocurrido un error: ' + result.Mensaje);
            } else {
                alert('Se procesó correctamente.');
                listarEvaluaciones();
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function refrehDatatable() {
    $('.tabla_version_x_revision').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "ordering": false,
        "searching": false,
        "iDisplayLength": -1,
        "info": false,
        "paging": false,
        "scrollX": true,
        "scrollY": "100%"
    });
}
