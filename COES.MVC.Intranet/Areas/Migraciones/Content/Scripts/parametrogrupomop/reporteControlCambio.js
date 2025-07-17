var controlador = siteRoot + 'migraciones/parametro/';

$(function () {
    $('#btnConsultar').click(function () {
        cargarListado();
    });
    $('#btnExportar').click(function () {
        exportarReporte();
    });

    $('#txtFechaData').Zebra_DatePicker({
        onSelect: function () {
            cargarListado();
        }
    });

    $('#cbAgrupacion').change(function () {
        cargarListado();
    });

    $('#cbCategoria').change(function () {
        cargarListado();
    });

    cargarListado();
});

function cargarListado() {
    var fecha = $("#txtFechaData").val();
    var agrp = $("#cbAgrupacion").val();
    var central = $("#cbCategoria").val()

    $.ajax({
        type: 'POST',
        url: controlador + "ListarReporteControlCambios",
        data: {
            strfecha: fecha,
            idAgrup: agrp,
            catecodi: central
        },
        success: function (evt) {
            $('#listado').html(evt.Resultado);

            $('#tabla').dataTable({
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": -1
            });
        },
        error: function (err) {
            alert('Ha ocurrido un error');
        }
    });
}

function exportarReporte() {
    var fecha = $("#txtFechaData").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarReporteControlCambios',
        data: {
            strfecha: fecha
        },
        success: function (evt) {
            if (evt.Error == undefined) {
                window.location.href = controlador + 'DescargarExcelReporte?archivo=' + evt[0] + '&nombre=' + evt[1];
            }
            else {
                alert("Error:" + evt.Descripcion);
            }
        },
        error: function (result) {
            alert('Ha ocurrido un error al descargar el archivo excel. ' + result.status + ' - ' + result.statusText + '.');
        }
    });
}