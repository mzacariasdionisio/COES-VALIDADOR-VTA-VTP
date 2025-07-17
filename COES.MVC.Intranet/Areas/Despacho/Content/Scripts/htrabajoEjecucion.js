var controlador = siteRoot + 'Despacho/HTrabajo/';

$(function () {
    $('#txtFecha').Zebra_DatePicker({
        format: 'd-m-Y',
        onSelect: function () {
        }
    });

    $('#btnEjecManual').click(function () {
        iniciarSimulacion('M');
    });
    $('#btnEjecAutomatica').click(function () {
        iniciarSimulacion('A');
    });
});

function iniciarSimulacion(tipo) {
    var jsonResult = tipo == 'M' ? 'EjecucionManual' : 'EjecucionAutomatica';

    if (confirm('¿Desea ejecutar carga de archivo?')) {
        $.ajax({
            url: controlador + jsonResult,
            data: {
                fecha: $("#txtFecha").val(),
                h: $("#cbMediaHora").val(),
            },
            type: 'POST',
            success: function (result) {
                if (result.Resultado == "1") {
                    alert("¡Ejecución exitosa! \nVerificar notificación en la bandeja del correo electrónico.");
                }
                else {
                    alert('Se ha producido un error al iniciar ejecución. \nError: ' + result.Mensaje);
                }
            },
            error: function (xhr, status) {
                alert('No se pudo conectar al servidor COES');
            }
        });
    }
}
