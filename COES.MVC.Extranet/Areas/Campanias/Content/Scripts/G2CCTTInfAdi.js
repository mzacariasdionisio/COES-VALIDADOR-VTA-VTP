var seccionCCTTAdicional = 34;
var descripcion = "InfoAdicional";
$(function () {
    var ruta_interna = obtenerRutaProyecto();  
    crearUploaderGeneral('btnSubirCCTTAdicional', '#tablaInfoAdicional', seccionCCTTAdicional, descripcion, ruta_interna);
    if (modoModel == "consultar") {
        desactivarCamposFormulario('FichaAdi');
        $('#btnSubirCCTTAdicional').hide();
    }
});

function cargarAdicionalCCTT() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        cargarArchivosRegistradosDesc(seccionCCTTAdicional, '#tablaInfoAdicional');
    }
}