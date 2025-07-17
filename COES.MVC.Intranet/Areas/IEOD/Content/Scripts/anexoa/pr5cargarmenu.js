var controlador = siteRoot + 'IEOD/AnexoA/';
var parametro1 = '';
var parametro2 = '';
var parametro3 = '';
var parametro4 = '';

$(function () {
    cargarMenuInfo();

    $('#openMenuPR5').click(function () {
        $('#contenedorMenuPR5').slideToggle("slow");
    });

    $('#closeMenuPR5').click(function () {
        $('#contenedorMenuPR5').css("display", "none");
    });

    $('#txtFechaInicio').Zebra_DatePicker({
        pair: $('#txtFechaFin'),
        onSelect: function (date) {
            $('#txtFechaFin').val(date);

            var date1 = getFecha(date);
            var date2 = getFecha($('#txtFechaFin').val());
            if (date1 > date2) {
                $('#txtFechaFin').val(date);
            }
        }
    });

    $('#txtFechaFin').Zebra_DatePicker({
        direction: true
    });

    $('#btnSearch').click(function () {
        mostrarReporteByFiltros();
    });

    $('#btnExportExcel').click(function () {
        exportarExcel();
    });

    $('#btnRegresar').click(function () {
        var fecha = $("#hdFechaAnexoA").val().replaceAll("/", '-');
        document.location.href = siteRoot + 'IEOD/AnexoA/MenuAnexoA?fecha=' + fecha;
    });

});

function fnClick(x) {
    var fecha = $("#hdFechaAnexoA").val().replaceAll("/", '-');
    document.location.href = controlador + x + "?fecha=" + fecha;
}

function exportarExcel() {
    var reporcodi = parseInt($("#hdReporcodi").val());
    var idEmpresa = getEmpresa();
    var idCentral = getCentral();
    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarReporteAnexoAByItem',
        data: {
            reporcodi: reporcodi,
            fec1: $('#txtFechaInicio').val(),
            fec2: $('#txtFechaFin').val(),
            param1: parametro1,
            param2: parametro2,
            param3: parametro3,
            param4: parametro4,
            idEmpresa: idEmpresa,
            idCentral: idCentral
        },
        dataType: 'json',
        success: function (result) {
            switch (result.Total) {
                case 1: window.location = controlador + "ExportarReporteXls?nameFile=" + result.Resultado; break;// si hay elementos
                case 2: alert("No existen registros !"); break;// sino hay elementos
                case -1: alert(result.Mensaje); break;// Error en C#
            }
        },
        error: function (err) {
            alert("Error en reporte");;
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

            //sombrear el item seleccionado
            $(".item_pr5 a").css("font-weight", "normal");
            var codigoItem = $("#hdReporcodi").val();
            $("#repor_" + codigoItem + ".item_pr5 a").css("font-weight", "bold");

            $('#CodiMenu').val(1);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}