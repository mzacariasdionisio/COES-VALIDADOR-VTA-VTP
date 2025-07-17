var controlador = siteRoot + 'Despacho/';
var anchoListado = 900;
$(function () {
    $('#btnRegresar').click(function () {
        location.href = controlador + "CostosVariables/Index";
    });
    $('#btnReporte').click(function () {
        exportarReporteCostosVariablesRepCv();
    });
    mostrarCostosVariables();
    mostrarParametrosGenerales();
    $('#tab-container-principal').easytabs({
        animate: false
    });

    anchoListado = $("#mainLayout").width() - 30;

    $('#tab-container-principal').bind('easytabs:after', function () {
        var oTable = $('#tablaCostos').dataTable();
        if (oTable.length > 0) {
            oTable.fnAdjustColumnSizing();
        }
        var oTable1 = $('#tablaParametros').dataTable();
        if (oTable1.length > 0) {
            oTable1.fnAdjustColumnSizing();
        }
    });
});
function mostrarCostosVariables() {
    $.ajax({
        type: "POST",
        url: controlador + "CostosVariables/ListaReporteCostosVariables",
        data: {
            repcodi: $('#hdnRepCodi').val()
        },
        success: function (evt) {
            $('#listaCostoVariables').html(evt);
            $("#listaCostoVariables").css("width", anchoListado + "px");
            $('#tablaCostos').dataTable({
                "scrollY": 400,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 1000
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
};
function mostrarParametrosGenerales() {
    $.ajax({
        type: "POST",
        url: controlador + "CostosVariables/ListaParametrosGeneralesRepCv",
        data: {
            repcodi: $('#hdnRepCodi').val()
        },
        success: function (evt) {
            $('#listaParametrosGen').html(evt);
            $("#listaParametrosGen").css("width", anchoListado + "px");
            $('#tablaParametros').dataTable({
                "scrollY": 400,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 1000,

            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
};
exportarReporteCostosVariablesRepCv = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "CostosVariables/ExportarReporteCostosVariablesPorRepCv",
        data: {
            repcodi: $('#hdnRepCodi').val()
        },
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                location.href = controlador + "CostosVariables/DescargarRerporteCostosVariablesPorRepCv";
            } else {
                alert("Ha ocurrido un error");
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
};

function mostrarError(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}
