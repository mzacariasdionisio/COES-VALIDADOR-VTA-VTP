var seccionD1Adicional = 40;
var descripcion = "InfoAdicional";
$(function () {
    var ruta_interna = obtenerRutaProyecto();  
    crearUploaderGeneral('btnSubirD1Adicional', '#tablaInfoAdicional', seccionD1Adicional, descripcion, ruta_interna);
});
function cargarAdicionalD1() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        cargarArchivosRegistradosDesc(seccionD1Adicional, '#tablaInfoAdicional');
    }

    if (modoModel == "consultar") {
        desactivarCamposFormulario('FichaAdi');
        $('#btnSubirD1Adicional').hide();
    }
}