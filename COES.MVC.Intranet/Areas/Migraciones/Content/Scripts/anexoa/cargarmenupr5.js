var controlador = siteRoot + 'Migraciones/AnexoA/';
var parametro1 = '';

$(function () {
    cargarMenuInfo();

    $('#txtFec1').Zebra_DatePicker({
        pair: $('#txtFec2'),
        onSelect: function (date) {
            if ($('#txtFec2').val() === undefined) {
                $('#txtFec2').val(date);
            }
            var date1 = getFecha(date);
            var date2 = getFecha($('#txtFec2').val());
            if (date1 > date2) {
                $('#txtFec2').val(date);
            }
        }
    });

    $('#txtFec2').Zebra_DatePicker({
        direction: true
    });

    $('#btnFilter').click(function () {
        $('#tbFilter').toggle("slow");
    });

    $('#btnExportExcel').click(function () {
        exportarExcel();
    });

    $('#btnSearch').click(function () {
        setearFechas();
    });
});

function exportarExcel() {
    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarReporteAnexoA',
        data: {
            fec1: $('#txtFec1').val(), fec2: $('#txtFec2').val(), url: "/", param1: parametro1
        },
        dataType: 'json',
        success: function (result) {
            switch (result.Total) {
                case 1: window.location = controlador + "ExportarReporteXls?nameFile=" + result.Resultado; break;// si hay elementos
                case 2: alert("No existen registros !"); break;// sino hay elementos
                case -1: alert("Error en reporte result"); break;// Error en C#
            }
        },
        error: function () {
            alert("Error en reporte");;
        }
    });
}

function setearFechas() {
    $.ajax({
        type: 'POST',
        url: controlador + 'SetearFechaFilterA',
        data: { fec1: $('#txtFec1').val(), fec2: $('#txtFec2').val() },
        dataType: 'json',
        success: function (e) {
            if (e.Total == 1) {
                window.location.href = controlador + e.Resultado;
            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarMenuInfo() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarMenu',
        data: { id: -1 },
        dataType: 'json',
        success: function (e) {
            $('#MenuID').html(e.Menu);
            $('#myTable').DataTable({
                "paging": false,
                "lengthChange": false,
                "pagingType": false,
                "ordering": false,
                "info": false
            });
            $('#CodiMenu').val(1);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}