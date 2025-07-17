var controlador = siteRoot + 'Medidores/Parametro/';

////////////////////////////////////////////////////////////////////////
/// Util
////////////////////////////////////////////////////////////////////////

mostrarExitoOperacion = function () {
    $('#mensaje').removeClass("action-error");
    $('#mensaje').addClass("action-exito");
    $('#mensaje').text("La operación se realizó con exito...!");
    $('#mensaje').css("display", "block");
};

mostrarError = function (msj) {
    var mensaje = "Ha ocurrido un error";
    if (msj != undefined && msj != null && msj != -1) {
        mensaje = msj;
    }

    alert(msj);

    //$('#mensaje').removeClass("action-exito");
    //$('#mensaje').addClass("action-error");
    //$('#mensaje').text(mensaje);
    //$('#mensaje').css("display", "block");
};

function dibujarHora(id, time) {
    var hora = time.substring(0, 2);
    var min = time.substring(3, 5);

    $('#' + id + ' .hour').html(hora);
    $('#' + id + ' .minute').html(min);
}