var controlador = siteRoot + 'Ensayo/';

$(function () {
    $('#btnCancelar').click(function () {
        cancelar();
    });

})

cancelar = function () {
    document.location.href = controlador + "genera/index/";
}

function abrirArchivo(archivo) {
    window.location = controlador + 'genera/DescargarArchivoEnvio?archivo=' + archivo;
}

loadInfoFile = function (fileName) {
    $('#fileInfo').html(fileName);
}

loadValidacionFile = function (mensaje) {
    alert(mensaje);
}

mostrarProgreso = function (porcentaje) {
    $('#progreso').text(porcentaje + "%");
}

mostrarError = function () {
    alert("Ha ocurrido un error.");
}

mostrarMensaje = function (mensaje) {
    alert(mensaje);

}

limpiarMensaje = function () {
    $('#mensaje').removeClass("action-alert");
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-message");
    $('#mensaje').html("Complete los datos, seleccione y procese archivo.");
}

mensajeExito = function () {
    $('#popupFormatoEnviadoOk').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

cerrarpopupFormatoEnviadoOk = function () {
    $('#popupFormatoEnviadoOk').bPopup().close();
}

function mostrarListado(ensayocodi, rowChange) {       
    $.ajax({
        type: 'POST',
        url: controlador + "genera/ListaFormato",
        data: {
            Ensayocodi: ensayocodi, iRowChange: rowChange
        },
        success: function (evt) {           
            $('#listado').html(evt);                               
        },
        error: function () {
            alert("Ha ocurrido un error mostrar listado");
        }
    });
}

function popupHistorialFormato(idCodi, idFormato) {

    $.ajax({
        type: 'POST',
        url: controlador + "genera/HistorialFormato",
        data: { iensayocodi: idCodi, iformatocodi: idFormato },
        success: function (evt) {
            $('#HistorialFormato').html(evt);
            setTimeout(function () {
                $('#popupEnsayoHistorialFormato').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function () {
            alert("Ha ocurrido un error en ingresar Historial Formato");
        }
    });
}

