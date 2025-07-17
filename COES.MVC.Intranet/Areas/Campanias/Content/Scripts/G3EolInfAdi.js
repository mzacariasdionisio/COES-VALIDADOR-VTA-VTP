var seccionEolAdicional = 14;
var descripcion = "InfoAdicional";

$(function () {
    var ruta_interna = obtenerRutaProyecto();  
    crearUploaderGeneral('btnSubirEolAdicional', '#tablaInfoAdicional', seccionEolAdicional, descripcion, ruta_interna);

    if (modoModel == "consultar") {
        desactivarCamposFormulario('FichaAdi');
        $('#btnSubirEolAdicional').hide();
    }
});
function cargarAdicionalEol() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        cargarArchivosRegistradosDesc(seccionEolAdicional, '#tablaInfoAdicional');
    }
}

function eliminarFile(id) {
    document.getElementById("contenidoPopup").innerHTML = '¿Está seguro de realizar esta operación?';
    $('#popupProyectoGeneral').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    $('#btnConfirmarPopup').off('click').on('click', function() {
        $.ajax({
            type: "POST",
            url: controladorFichas + "EliminarFile",
            data: {
                id: id,
            },
            dataType: "json",
            success: function (result) {
                if (result == 1) {
                    $("#fila" + id).remove();
                    mostrarMensaje(
                        "mensajeFicha",
                        "exito",
                        "El archivo se eliminó correctamente."
                    );
                } else {
                    mostrarMensaje("mensajeFicha", "error", "Se ha producido un error.");
                }
            },
            error: function () {
                mostrarMensaje("mensajeFicha", "error", "Se ha producido un error.");
            },
        });
        popupClose('popupProyectoGeneral');
    });
}