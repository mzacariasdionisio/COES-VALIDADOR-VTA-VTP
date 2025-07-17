var controlador = siteRoot + 'hidrologia/';
var periodo = {}
$(function () {
    getPeriodoDefault();

    $('#cbEmpresa').change(function () {
        //mostrarReporteCumplimiento();
    });

    $('#txtMes').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            //mostrarReporteCumplimiento()
        }
    });

    $('#Anho').val(periodo.anho);

    $('#btnBuscar').click(function () {
        mostrarReporteCumplimiento();
    });

    $('#btnExportar').click(function () {
        exportarReporteCumplimiento();
    });

});

function actualizarPantalla() {
    $(window).on('resize', function () {
        var anchoActual = $(".search-content").width();

        $(".panel-container").css({
            width: anchoActual
        });
    });
}

function mostrarReporteCumplimiento() {
    $.ajax({
        type: 'POST',
        url: controlador + "Cumplimiento/ViewReporteCumplimientoByEmpresa",
        data: {
            empresacodi: $("#cbEmpresa").val(),
            mes: $('#txtMes').val()
        },
        success: function (dataHtml) {
            var anchoActual = $(".search-content").width();
            $('#listado').html(dataHtml);

            $(".panel-container").css({
                width: anchoActual
            });

            actualizarPantalla();
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function exportarReporteCumplimiento() {
    $.ajax({
        type: 'POST',
        url: controlador + 'Cumplimiento/ExportarExcelReporteCumplimiento',
        data: {
            mes: $('#txtMes').val(),
            estado: $('#checkFecha').prop('checked')
        },
        success: function (result) {
            if (result.Resultado != "-1") {
                window.location.href = controlador + 'Cumplimiento/DescargarExcelReporteCumplimiento?archivo=' + result.Resultado + '&nombre=' + result.TituloReporteXLS;
            }
            else {
                alert("Error en descargar el archivo");
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error al descargar el archivo excel.');
        }
    });
}

function horizonte() {
    $('#cntFecha').css("display", "none");
    $('#cntSemana').css("display", "none");
    $('#fechasSemana').css("display", "none");
    $('#cntMes').css("display", "block");
    $('#cntFecha2').css("display", "none");
    $('#cntSemana2').css("display", "none");
    $('#fechasSemana2').css("display", "none");
    $('#cntMes2').css("display", "block");
}

/// Setea por defecto el año mes y dia al iniciar el aplicativo
function getPeriodoDefault() {
    var hoy = new Date();
    var dd = hoy.getDate();
    var mm = hoy.getMonth() + 1; //hoy es 0!
    var yyyy = hoy.getFullYear();

    if (dd < 10) {
        dd = '0' + dd
    }

    if (mm < 10) {
        mm = '0' + mm
    }
    periodo.anho = yyyy;
    periodo.mes = $("#hfMes").val();

}
