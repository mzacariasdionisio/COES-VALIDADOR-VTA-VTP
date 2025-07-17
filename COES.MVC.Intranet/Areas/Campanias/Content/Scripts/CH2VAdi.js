var seccionCHVAdicional = 41;
var descripcion = "InfoAdicional";

$(function () {
    var ruta_interna = obtenerRutaProyecto();  
    crearUploaderGeneral('btnSubirFormatoInfBasica', '#tablaLinInfoAdicional', seccionCHVAdicional, descripcion, ruta_interna);
});


function cargarAdicionalCHV() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        cargarArchivosRegistradosDesc(seccionCHVAdicional, '#tablaLinInfoAdicional');
    }
    if (modoModel == "consultar") {
        desactivarCamposFormulario('HVAD');
        $('#btnSubirFormatoInfBasica').hide();
    }
}

