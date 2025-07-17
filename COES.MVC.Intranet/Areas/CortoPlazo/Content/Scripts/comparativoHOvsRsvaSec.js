var controlador = siteRoot + 'CortoPlazo/Comparativo/';
var FILTRO_FECHA_INICIO = '01/01/2000';
var FILTRO_FECHA_HOY = '';

$(function () {
    FILTRO_FECHA_HOY = $("#fechaPeriodo").val();

    $('#fechaPeriodo').Zebra_DatePicker({
        direction: [FILTRO_FECHA_INICIO, FILTRO_FECHA_HOY],
        onSelect: function () {
            cargarComparativoHOvsRsvaSec();
        },
    });

    //
    $('#btnConsultar').click(function () {
        cargarComparativoHOvsRsvaSec();
    });
    $('#btnExportarExcel').click(function () {
        generarExcelComparativoHOvsRsvaSec();
    });

    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').bind('easytabs:after', function () {
        refrehDatatable();
    });

    cargarComparativoHOvsRsvaSec();
});

///////////////////////////
/// web 
///////////////////////////
function cargarComparativoHOvsRsvaSec() {
    $("#mensaje").hide();
    $("#vistaRsf .content-tabla").html('');
    $("#vistaHo .content-tabla").html('');

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarComparativoHOvsRsvaSec',
        dataType: 'json',
        data: {
            strFecha: $("#fechaPeriodo").val(),
        },
        cache: false,
        success: function (data) {
            if (data.Resultado != "-1") {
                if (data.Resultado == "0") {
                    $("#mensaje").show();
                    mostrarMensaje("mensaje", "No se encontraron inconsistencias en las fuentes de datos para la fecha seleccionada", $tipoMensajeAlerta, $modoMensajeCuadro);
                }
                var listaObj = data.ListaRsvaSec;

                $("#vistaRsf .content-tabla").html(listaObj[0].ReporteHtml);
                $("#vistaHo .content-tabla").html(listaObj[1].ReporteHtml);

                refrehDatatable();
            } else {
                alert("Ha ocurrido un error: " + data.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function refrehDatatable() {
    $('.tbl_comparativo').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "ordering": false,
        "searching": false,
        "iDisplayLength": -1,
        "info": false,
        "paging": false,
        "scrollX": false,
    });

}

///////////////////////////
/// excel 
///////////////////////////

function generarExcelComparativoHOvsRsvaSec() {
    $("#mensaje").hide();

    $.ajax({
        type: 'POST',
        url: controlador + "GenerarExcelComparativoHOvsRsvaSec",
        data: {
            strFecha: $("#fechaPeriodo").val(),
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "Exportar?file_name=" + evt.Resultado;
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}