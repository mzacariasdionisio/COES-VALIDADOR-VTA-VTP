var controlador = siteRoot + 'InformeEjecutivoMen/reporteejecutivo/';
var controladorbarras = siteRoot + 'InformeEjecutivoMen/Barra/';

var TipoEstado = Object.freeze(
    {
        OK: 1,
        ERROR: 0
    }
);

$(function () {
    cargarMenu();

    $('#openMenuSiosein').click(function () {
        $('#contenedorMenuSiosein').slideToggle("slow");
    });
    
    $('#closeMenuSiosein').click(function () {
        $('#contenedorMenuSiosein').css("display", "none");
    });

    $('#PickerMes').Zebra_DatePicker({
        format: 'Y-m',
        onSelect: function () {
        }
    });

    $('#btnSearch').click(function () {
        mostrarReporteByFiltros();
    });

    $('#btnExportExcelIndividual').click(function () {
        exportarExcelIndividual();
    }); 

    $('#btnRegresar').click(function () {
        var mesConsulta = $("#PickerMes").val();
        document.location.href = siteRoot + 'InformeEjecutivoMen/ReporteEjecutivo/index?mes=' + mesConsulta;
    });

    //configurar barras
    $('#btnAdminBarras').on('click', function () {
        document.location.href = controladorbarras + "index";
    });
});

function fnClick(x) {
    var codigoVersion = getCodigoVersion();
    document.location.href = controlador + x + '?codigoVersion=' + codigoVersion;    
}

function cargarMenu() {

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarMenu',
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
            var codigoItem = $("#hdIdNumeral").val();
            $("#repor_" + codigoItem + ".item_pr5 a").css("font-weight", "bold");
            $("#repor_" + codigoItem + ".item_pr5 a").css("color", "blue");

            $('#CodiMenu').val(1);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function exportarExcelIndividual() {
    var reporcodi = parseInt($("#hdReportecodi").val());
    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarReporteMensual',
        data: {
            reporcodi: reporcodi, versi: $("#cboVersion").val()
        },
        dataType: 'json',
        success: function (e) {

            switch (e.Total) {
                case 1: window.location = controlador + "ExportarReporteXlsIndividual?nameFile=" + e.Resultado; break;// si hay elementos
                case -1: alert(e.Mensaje); break;
                //alert(e.Resultado2); break;// Error en C#
            }
        },
        error: function () {
            alert("Error en reporte");;
        }
    });
}