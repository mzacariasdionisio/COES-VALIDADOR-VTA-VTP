var controlador = siteRoot + "GMM/garantias/";

$(document).ready(function () {

    $('#tab-container').easytabs({
        animate: false
    });

    $('#rpt1').click(function () {
        exportarReporte('rpt1');
    });

    $('#rpt2').click(function () {
        exportarReporte('rpt2');
    });

    $('#rpt3').click(function () {
        exportarReporte('rpt3');
    });

    $('#rpt4').click(function () {
        exportarReporte('rpt4');
    });

    $('#rpt5').click(function () {
        exportarReporte('rpt5');
    });

    $('#rpt6').click(function () {
        exportarReporte('rpt6');
    });

    setAnio("anhorpt");
    var vd = new Date();
    var vanho = vd.getFullYear();
    var vmes = vd.getMonth() +1;

    $('#anhorpt').val(vanho);
    $('#mesrpt').val(vmes);

});

function setAnio(aselect) {

    var d = new Date();
    var n = d.getFullYear();
    var select = document.getElementById(aselect);
    for (var i = 2020; i <= n; i++) {
        var opc = document.createElement("option");
        opc.text = i;
        opc.value = i;
        select.add(opc);
    }
}

function mostrarExito(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}
function mostrarError(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}
function mostrarAlerta(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}
function mostrarMensaje(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
}
function ocultarMensaje(){
	$('#mensaje').removeClass();
	$('#mensaje').html('');
}

exportarReporte = function (ptipo) {
    var panio = $('#anhorpt').val();
    var pmes = $('#mesrpt').val();
        $.ajax({
            type: 'POST',
            url: controlador + 'exportarReporte',
            dataType: 'json',
            data: { anio: panio, mes: pmes, tipo: ptipo},
            success: function (result) {
                if (result == "1") {
                	ocultarMensaje('');
                    window.location = controlador + ptipo;
                }
                else if (result == "2") {
                    mostrarError('No existen garantías calculadas para el perido: ' + pmes + '/' + panio);
                }
                else if (result == "-1") {
                    mostrarError();
                }
                else {
                    alert(result);
                }
            },
            error: function () {
                alert("Lo sentimos, se ha producido un error");
            }
        });
}

