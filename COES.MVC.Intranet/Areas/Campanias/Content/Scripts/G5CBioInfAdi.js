var seccionBioAdicional = 26;
var descripcion = "InfoAdicional";

$(function () {
    var ruta_interna = obtenerRutaProyecto();  
    crearUploaderGeneral('btnSubirBioAdicional', '#tablaInfoAdicional', seccionBioAdicional, descripcion, ruta_interna);
});


function cargarAdicionalBio() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        cargarArchivosRegistradosDesc(seccionBioAdicional, '#tablaInfoAdicional');
    }
    if (modoModel == "consultar") {
        desactivarCamposFormulario('FichaAdi');
        $('#btnSubirBioAdicional').hide();
    }
}

