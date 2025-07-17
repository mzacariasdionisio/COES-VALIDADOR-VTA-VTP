var controlador = siteRoot + 'Subastas/ConfgSubasta/';

$(document).ready(function () {

    $('#txtFechaData').Zebra_DatePicker({
        onSelect: function () {
        }
    });

    $('#btnProcesar').click(function () {
        procesar();
    });


    $('#txtFechaData2').Zebra_DatePicker({
        onSelect: function () {
        }
    });
    $('#btnProcesar2').click(function () {
        procesarInicial();
    });
});

function procesar() {
    var strFechaOferta = $("#txtFechaData").val();
    var tipoOferta = parseInt($('input[name=cbTipo]:checked').val()) || 0;

    var mensaje = '¿Desea desencriptar las Ofertas ' + (tipoOferta == 0 ? "Defecto" : "Diaria") + ' para la fecha ' + strFechaOferta;
    if (confirm(mensaje)) {
        $.ajax({
            type: 'POST',
            url: controlador + "ProcesarOferta",
            data: {
                strFechaOferta: strFechaOferta,
                tipoOferta: tipoOferta,
            },
            dataType: 'json',
            cache: false,
            success: function (model) {
                if (model.Resultado != -1) {
                    alert("El proceso se ha ejecutado correctamente.");
                } else {
                    alert("Ha ocurrido un error: " + model.Mensaje);
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error.");
            }
        });
    }
}

function procesarInicial() {
    var strFechaOferta = $("#txtFechaData2").val();

    var mensaje = '¿Desea desencriptar las Ofertas de envíos históricos anteriores?';
    if (confirm(mensaje)) {
        $.ajax({
            type: 'POST',
            url: controlador + "ProcesarOfertaInicial",
            data: {
                strMaxFechaOferta: strFechaOferta,
            },
            dataType: 'json',
            cache: false,
            success: function (model) {
                if (model.Resultado != -1) {
                    alert("El proceso se ha ejecutado correctamente.");
                } else {
                    alert("Ha ocurrido un error: " + model.Mensaje);
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error.");
            }
        });
    }
}