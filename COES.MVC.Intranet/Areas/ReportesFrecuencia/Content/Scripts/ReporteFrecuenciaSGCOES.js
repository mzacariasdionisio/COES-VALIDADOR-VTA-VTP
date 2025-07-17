var controler = siteRoot + "ReportesFrecuencia/ReporteFrecuenciaSGCOES/"

function exportarExcel()  ///para exportar arhivos excel 
{
    console.log('exportarExcel');
    console.log(controler);
    var intIdEquipo = 0;
    if ($("#cboEquipo").val() == '') {
        intIdEquipo = 0;
    } else {
        intIdEquipo = $("#cboEquipo").val();
    }
    $.ajax({
        type: 'POST',
        url: controler + 'GenerarArchivoReporteXls',
        data: {
            fechaIni: $('#FechaInicial').val(), fechaFin: $('#FechaFinal').val(), IdEquipo: intIdEquipo, IndOficial: $("#cboOficial").val()
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controler + "ExportarReporte";
            }
            if (result == -1) {
                alert("Ha ocurrido un error al generar reporte excel");
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error al generar archivo excel");
            console.log(err);
        }
    });
}