var controlador = siteRoot + 'YupanaContinuo/Parametro/';

$(function () {
    $('#txtFecha').Zebra_DatePicker({
        format: 'd/m/Y',
        onSelect: function () {
        }
    });

    $('#btnEjecManual').click(function () {
        iniciarSimulacion('M');
    });
    $('#btnEjecAutomatica').click(function () {
        iniciarSimulacion('A');
    });

    $('#btnFinalizarEjec').click(function () {
        finalizarEjecucionGams();
    });
});

function iniciarSimulacion(tipo) {
    var jsonResult = tipo == 'M' ? 'SimularEjecucionManual' : 'SimularEjecucionAutomatica';

    if (confirm('¿Desea generar una simulación?')) {
        $.ajax({
            url: controlador + jsonResult,
            data: {
                fecha: $("#txtFecha").val(),
                hora: $("#cbHora").val(),
            },
            type: 'POST',
            success: function (result) {
                if (result.Resultado == "1") {
                }
                else {
                    alert('Se ha producido un error al iniciar simulación. Error: ' + result.Mensaje);
                }
            },
            error: function (xhr, status) {
                alert('Se ha producido un error al simular árbol.');
            }
        });
    }
}

function finalizarEjecucionGams() {

    if (confirm('¿Desea finalizar ejecución del árbol y procesos Gams?')) {

        $.ajax({
            url: controlador + "FinalizarEjecucionGams",
            data: {

            },
            type: 'POST',
            success: function (result) {
                if (result.Resultado == "1") {
                    alert("Terminó de la ejecución del árbol y procesos Gams.");
                }
                else {
                    alert('Se ha producido un error: ' + result.Mensaje);
                }
            },
            error: function (xhr, status) {
                alert('Se ha producido un error.');
            }
        });
    }
}
