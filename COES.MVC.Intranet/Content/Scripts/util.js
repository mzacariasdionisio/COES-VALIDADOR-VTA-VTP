/* ------------------------------------------------------------------------
Class: util
Uso: Contiene funciones genéricas
Fecha creación: 28.10.2015
Version: 1.0.0
-------------------------------------------------------------------------*/

$mensajeExitoGeneral = "La operación se ha ejecutado exitósamente";
$mensajeErrorGeneral = "Se ha producido un error, por favor inténtelo nuevamente.";

//Tipos de mensajes
$tipoMensajeError = 1;
$tipoMensajeExito = 2;
$tipoMensajeAlerta = 3;
$tipoMensajeMensaje = 4;

//Modo en que se visualizan los mensajes
$modoMensajePopup = 1;
$modoMensajeCuadro = 2;


mostrarMensaje = function (identificador, mensaje, tipo, modo) {

    if (modo == $modoMensajeCuadro) {

        var estilo = "";

        $('#' + identificador).removeClass();
        $('#' + identificador).html(mensaje);

        switch (tipo) {
            case $tipoMensajeError:
                {
                    estilo = "action-error";
                    break;
                }
            case $tipoMensajeExito:
                {
                    estilo = "action-exito";
                    break;
                }
            case $tipoMensajeAlerta:
                {
                    estilo = "action-alert";
                    break;
                }
            case $tipoMensajeMensaje:
                {
                    estilo = "action-message";
                    break;
                }
        }

        $('#' + identificador).addClass(estilo);
    }
}

mostrarConfirmacion = function () {

    mensaje = arguments[0];
    operacion = arguments[1];
    var parametros = [];

    if (arguments.length > 2) {

        for (var i = 2; i < arguments.length; i++) {
            parametros.push(arguments[i]);
        }

        if (mensaje == '') mensaje = 'Por favor confirme la operación';
        $('#mensajeConfirmacion').html(mensaje);
        $('#popupConfirmarOperacion').bPopup({
            follow: [false, false], //x, y
            position: [580, 220] //x, y
        });

        $("#btnAceptarConfirmacionOpe").unbind("click");
        $('#btnAceptarConfirmacionOpe').click(function () {
            operacion(parametros);
        });

        $('#btnCancelarConfirmacionOpe').click(function () {
            $('#popupConfirmarOperacion').bPopup().close();
        })
    }
}

mostrarMensajePopup = function () {
    mensaje = arguments[0];
    tipomensaje = arguments[1];

    var img = "";
    //if (tipomensaje == 1) {
    //    img = "<img src='" + siteRoot + "content/images/ic_check.png' />"
    //} else if (tipomensaje == 2) {
    //    img = "<img src='" + siteRoot + "content/images/ic_error.png' />"
    //} else if (tipomensaje == 3) {
    //    img = "<img src='" + siteRoot + "content/images/ic_info.png' />"
    //}

    $('#cmensaje').html(img + " " + mensaje);
    $('#popupMensajeZ').bPopup({
        follow: [false, false], //x, y
        position: [550, 190] //x, y
    });

    $('#btnAceptarMsj').click(function () {
        $('#popupMensajeZ').bPopup().close();
    })
}

validarLetras = function (e) { // 1

    tecla = (document.all) ? e.keyCode : e.which;
    if (tecla == 8) return true; // backspace
    if (tecla == 32) return true; // espacio
    if (e.ctrlKey && tecla == 86) { return true; } //Ctrl v
    if (e.ctrlKey && tecla == 67) { return true; } //Ctrl c
    if (e.ctrlKey && tecla == 88) { return true; } //Ctrl x

    patron = /[a-zA-Z0-9]/; //patron

    te = String.fromCharCode(tecla);
    return patron.test(te); // prueba de patron
}


validarDecimal = function (n) {
    if (n == "")
        return false;

    var count = 0;
    var strCheck = "0123456789.";
    var i;

    for (i in n) {
        if (strCheck.indexOf(n[i]) == -1)
            return false;
        else {
            if (n[i] == '.') {
                count = count + 1;
            }
        }
    }
    if (count > 1) return false;
    return true;
}

validarHora = function (hora) {
    if (!hora || hora.length < 1) { return false; }
    var time = hora.split(':');
    return (time.length === 2
           && parseInt(time[0], 10) >= 0
           && parseInt(time[0], 10) <= 23
           && parseInt(time[1], 10) >= 0
           && parseInt(time[1], 10) <= 59) ||
           (time.length === 3
           && parseInt(time[0], 10) >= 0
           && parseInt(time[0], 10) <= 23
           && parseInt(time[1], 10) >= 0
           && parseInt(time[1], 10) <= 59
           && parseInt(time[2], 10) >= 0
           && parseInt(time[2], 10) <= 59)
}

validarEmail = function (email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}

getFecha = function (date) {
    var parts = date.split("/");
    var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date.getTime();
}

getFechaDate = function (date) {
    var parts = date.split("/");
    var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date;
}

parseJsonDate = function (jsonDateString) {
    return new Date(parseInt(jsonDateString.replace('/Date(', '')));
}

validarNumero = function (item, evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;

    if (charCode == 46) {
        var regex = new RegExp(/\./g)
        var count = $(item).val().match(regex).length;
        if (count > 1) {
            return false;
        }
    }

    if (charCode == 45) {
        var regex = new RegExp(/\-/g)
        var count = $(item).val().match(regex).length;
        if (count > 0) {
            return false;
        }
    } 
    if (charCode > 31 && charCode != 45 && (charCode < 48 || charCode > 57)) {
         
        return false;
    }

    return true;
}

$.fn.extend({
    persisteBusqueda: async function (selector, callback, options) {
        let filtro = "input[type=text],select";
        let ruta = window.location.href;
        let storage = "filtro"

        let filtrador = localStorage.getItem(storage);
        let objFiltro = JSON.parse(filtrador);

        if (!objFiltro || !objFiltro.hasOwnProperty("ruta") || objFiltro["ruta"] != ruta) {
            objFiltro = null;
            localStorage.removeItem(storage, null);
        }

        let objetos = $(selector).find(filtro);
        $(this).click(() => {
            let objetoValor = new Object();
            for (let i = 0; i < objetos.length; i++) {
                objetoValor[objetos[i].id] = $(objetos[i]).val();
            }
            objetoValor.ruta = window.location.href;
            let json = JSON.stringify(objetoValor);
            localStorage.setItem(storage, json);
        });
        if (objFiltro) {
            let hechos = new Array();
            if (options) {
                for (option of options) {
                    $("#" + option.principal).val(objFiltro[option.principal]);
                    await option.accion();
                    hechos.push(option.principal);
                }
            }
            for (let i = 0; i < objetos.length; i++) {
                $(objetos[i]).val(objFiltro[objetos[i].id]);
            }
            callback();
        }
        else {
            if (options) {
                for (option of options) {
                    option.accion();
                }
            }
            callback();
        }
    }
});




$(document).on("keypress", '.numeroDecimal', function (a) {

    return validarNumero(a.currentTarget,a);
});

$(document).on("keypress", '.numero', function (evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
});

