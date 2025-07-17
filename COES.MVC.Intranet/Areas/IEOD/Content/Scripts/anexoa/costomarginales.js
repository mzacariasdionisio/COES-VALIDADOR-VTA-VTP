var anchoListado = 900;

$(function () {
    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').bind('easytabs:after', function () {
        refrehDatatable();
    });

    anchoListado = $("#mainLayout").width() - 40;

    cargarLista();
});

function mostrarReporteByFiltros() {
    cargarLista();
}
function cargarLista() {
    $("#div_barra").html("");
    $("#div_energ").html("");
    $("#div_conge").html("");

    var fechaInicio = getFechaInicio();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaCostoMarginalesCP',
        data: {
            fechaInicio: fechaInicio
        },
        success: function (model) {
            $('.filtro_fecha_desc').html(model.FiltroFechaDesc);

            $("#div_barra").html(model.Resultado);
            $("#div_energ").html(model.Resultado2);
            $("#div_conge").html(model.Resultado3);

            $("#div_reporte_1,#div_reporte_2,#div_reporte_3").css("width", anchoListado + "px");
            refrehDatatable();
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}


function refrehDatatable() {
    $('#reporte_1,#reporte_2,#reporte_3').dataTable({
        "scrollX": true,
        "scrollCollapse": true,
        "destroy": "true",
        "sDom": 't',
        "ordering": false,
        paging: false,
        fixedColumns: {
            leftColumns: 1
        }
    });
}