$.message = "";
var controler = siteRoot + "home/";

$(function () {
    $('#btnEnviar').click(function () {
        autenticar();
    });

    $('#Usuario').keyup(function (e) {
        if (e.keyCode == 13) {
            autenticar();
        }
    });

    $('#Clave').keyup(function (e) {
        if (e.keyCode == 13) {
            autenticar();
        }
    });
});

autenticar = function () {
    var recodar = ($('#cbRecordar').is(':checked') == true) ? 1 : 0;

    if (ValidarIngreso()) {
        $.ajax({
            type: "POST",
            url: controler + "autenticar",
            dataType: 'json',
            data: {
                usuario: $('#Usuario').val(), password: $('#Clave').val(), indicador: recodar
            },
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    location.href = "/";
                }
                if (resultado == 2) {
                    $('#accionResultado').addClass("action-alert");
                    $('#accionResultado').html("El usuario ingresado no existe.");
                }
                if (resultado == 3) {
                    $('#accionResultado').addClass("action-alert");
                    $('#accionResultado').html("El usuario o clave son incorrectos.");
                }
                if (resultado == -1) {
                    $('#accionResultado').addClass("action-error");
                    $('#accionResultado').html("Ha ocurrido un error.");
                }
            },
            error: function (req, status, error) {
                $('#accionResultado').addClass("action-error");
                $('#accionResultado').html("Ha ocurrido un error.");
            }
        });
    }
    else {
        $('#accionResultado').addClass("action-alert");
        $('#accionResultado').html($.message);
    }
}

ValidarIngreso = function () {
    var flag = true;
    $.message = "";
    $.message += "<ul>";
    if ($('#Usuario').val() == "") {
        $.message += "<li>Ingrese usuario.</li>";
        flag = false;
    }
    if ($('#Clave').val() == "") {
        $.message += "<li>Ingrese clave.</li>";
        flag = false;
    }
    $.message += "</ul>";
    return flag;
}