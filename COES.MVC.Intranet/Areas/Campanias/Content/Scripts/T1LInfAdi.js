var seccionLinAdicional = 38;
var descripcion = "InfoAdicional";

$(function () {
    // var ruta_interna = obtenerRutaProyecto();  
    // crearUploaderGeneral('btnSubirLinAdicional', '#tablaLinInfoAdicional', seccionLinAdicional, descripcion, ruta_interna);
});

function cargarAdicionalTLin() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        cargarArchivosRegistradosDesc(seccionLinAdicional, '#tablaLinInfoAdicional');
    }
    if (modoModel == "consultar") {
        desactivarCamposFormulario('FichaAdi');
        $('#btnSubirLinAdicional').hide();
    }
}