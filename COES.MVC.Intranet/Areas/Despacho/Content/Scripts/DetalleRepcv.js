var controlador = siteRoot + 'Despacho/';
var anchoListado = 900;

$(function () {
    mostrarParametros();
    mostrarCostosVariables();
    $('#btnRegresar').click(function () {
        verCostoVar();
    });

    $("#cbTipo").val($("#hdnTipo").val());

    anchoListado = $("#mainLayout").width() - 30;

    $('#btnActualizar').click(function () {
        var repcodi = $('#hdnRepCodi').val();
        actualizarParametro(repcodi);
    });

});

function verCostoVar() {
    location.href = siteRoot + "Despacho/CostosVariables/Index";
};

function actualizarParametro(repCodi) {
    location.href = siteRoot + "Migraciones/Parametro/Index?repCodi=" + repCodi; 
};

mostrarError = function () {
    alert('Ha ocurrido un error.');
};
function mostrarParametros() {
    $.ajax({
        type: "POST",
        url: controlador + "CostosVariables/ParametrosPorRepCv",
        data: {
            repcodi: $('#hdnRepCodi').val()
        },
        success: function (evt) {
            $('#listadoParametros').html(evt);
            $("#listadoParametros").css("width", anchoListado + "px");
            $('#tabla').dataTable({
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 1000
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
};
function mostrarCostosVariables() {
    $.ajax({
        type: "POST",
        url: controlador + "CostosVariables/CostosVariablesPorRepCv",
        data: {
            repcodi: $('#hdnRepCodi').val()
        },
        success: function (evt) {
            $('#listadoCV').html(evt);
            $("#listadoCV").css("width", anchoListado + "px");
            $('#tablaCostos').dataTable({
                "scrollY": 300,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 1000
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
};
