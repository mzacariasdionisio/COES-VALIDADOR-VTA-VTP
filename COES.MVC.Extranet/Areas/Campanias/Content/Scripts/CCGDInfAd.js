var seccionLinAdicional = 24;
var descripcion = "InfoAdicional";

$(function () {
    var ruta_interna = obtenerRutaProyecto();  
    crearUploaderGeneral('btnSubirCCGDAdicional', '#tablaAdicionalCCGD', seccionLinAdicional, descripcion, ruta_interna);
});

function cargarAdicionalCCGD() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        cargarArchivosRegistradosDesc(seccionLinAdicional, '#tablaAdicionalCCGD');
    }

    if (modoModel == "consultar") {
        desactivarCamposFormulario('FichaAdi');
        $('#btnSubirFormatoInfBasica').hide();
    }
}
