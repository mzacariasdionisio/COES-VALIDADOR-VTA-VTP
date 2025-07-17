$.message = "";
var controler = siteRoot + "home/";

$(document).ready(function () {

    $(document).ajaxStart(function () {
        $('#loading').bPopup({
            fadeSpeed: 'fast',
            opacity: 0.4,
            followSpeed: 500, //can be a string ('slow'/'fast') or int
            modalColor: '#000000'
        });
    });

    $(document).ajaxStop(function () {
        $('#loading').bPopup().close();
    });

    $('#btnEnviar').click(function () {
        Autenticar();
    });

    $('#loginForm').keypress(function (e) {
        if (e.keyCode == '13') {
            $('#btnEnviar').click();
        }
    });

    $('#Usuario').focus();
});

function Autenticar() {
    if (ValidarIngreso()) {
        $.ajax({
            type: "POST",
            url: controler + "autenticar",
            dataType: 'json',
            data: {
                usuario: $('#Usuario').val(), password: $('#Clave').val()
            },
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    location.href = controler + "default";
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

function ValidarIngreso() {

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

