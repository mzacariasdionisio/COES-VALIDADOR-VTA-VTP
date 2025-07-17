
var seccionLinAdicional = 11;
var descripcion = "InfoAdicional";

$(function () {
    var ruta_interna = obtenerRutaProyecto();  
    crearUploaderGeneral('btnSubirItcSEPAdicional', '#tablaAdicionalITCSEP', seccionLinAdicional, descripcion, ruta_interna);
});

function cargarAdicionalLin() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        cargarArchivosRegistradosDesc(seccionLinAdicional, '#tablaAdicionalITCSEP');
    }
    if (modoModel == "consultar") {
        desactivarCamposFormulario('FichaAdi');
        $('#btnSubirItcSEPAdicional').hide();
    }
}