var seccionLinAdicional = 33;
var descripcion = "InfoAdicional";

$(function () {
    var ruta_interna = obtenerRutaProyecto();  
    crearUploaderGeneral('btnSubirHidroAdicional', '#tablaInfoAdicional', seccionLinAdicional, descripcion, ruta_interna);
});

function cargarAdicionalLin() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        cargarArchivosRegistradosDesc(seccionLinAdicional, '#tablaInfoAdicional');
    }
    if (modoModel == "consultar") {
        desactivarCamposFormulario('FichaAdi');
        $('#btnSubirHidroAdicional').hide();
    }
}