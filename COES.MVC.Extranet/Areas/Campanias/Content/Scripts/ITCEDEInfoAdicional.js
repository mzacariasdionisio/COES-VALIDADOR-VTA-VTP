var seccionLinAdicional = 10;
var descripcion = "InfoAdicional";

$(function () {
    var ruta_interna = obtenerRutaProyecto();  
    crearUploaderGeneral('btnSubirItcEDEAdicional', '#tablaAdicionalITCEDE', seccionLinAdicional, descripcion, ruta_interna);
});

function cargarAdicionalLin() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        cargarArchivosRegistradosDesc(seccionLinAdicional, '#tablaAdicionalITCEDE');
    }
    if (modoModel == "consultar") {
        desactivarCamposFormulario('FichaAdi');
        $('#btnSubirItcEDEAdicional').hide();
    }
}