var informacionFrecuencia = "AnalisisInformacion/";
var controler = siteRoot + "ReportesFrecuencia/" + informacionFrecuencia;

//Funciones de busqueda
$(document).ready(function () {

    $('#tabla').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "aaSorting": [[0, "asc"], [1, "asc"]]
    });

    


});

function enviarCorreosAlertasDesvFrec() {
    $.ajax({
        type: 'POST',
        url: controler + "EnvioAlertasCorreo",
        data: {
        },
        success: function (evt) {
            if (evt == "1") {
                alert('Correo enviado satisfactoriamente.');
            }
        },
        error: function () {
            reject("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function enviarCorreosAlertasEventosFrec() {
    $.ajax({
        type: 'POST',
        url: controler + "EnvioAlertasCorreoEventosFrec",
        data: {
        },
        success: function (evt) {
            if (evt == "1") {
                alert('Correo enviado satisfactoriamente.');
            }
        },
        error: function () {
            reject("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function enviarCorreosReporteSegFaltantes() {
    $.ajax({
        type: 'POST',
        url: controler + "EnvioAlertasCorreoRepSegFaltantes",
        data: {
        },
        success: function (evt) {
            if (evt == "1") {
                alert('Correo enviado satisfactoriamente.');
            }
        },
        error: function () {
            reject("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

