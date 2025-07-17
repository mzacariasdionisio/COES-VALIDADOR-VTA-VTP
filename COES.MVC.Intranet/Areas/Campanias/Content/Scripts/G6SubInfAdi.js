var seccionSubAdicional = 36;
var descripcion = "InfoAdicional";
$(function () {
    var ruta_interna = obtenerRutaProyecto();  
    crearUploaderGeneral('btnSubirSubAdicional', '#tablaSubInfoAdicional', seccionSubAdicional, descripcion, ruta_interna);
});
function cargarAdicionalSub() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        cargarArchivosRegistradosDesc(seccionSubAdicional, '#tablaSubInfoAdicional');
    }
    if (modoModel == "consultar") {
        desactivarCamposFormulario('FichaAdi');
        $('#btnSubirSubAdicional').hide();
    }
}
