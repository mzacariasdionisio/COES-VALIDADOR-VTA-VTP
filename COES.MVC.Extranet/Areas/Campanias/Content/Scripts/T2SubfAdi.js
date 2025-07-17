var seccionSubAdicional = 39;
var descripcion = "InfoAdicional";

$(function () {
    var ruta_interna = obtenerRutaProyecto();  
    crearUploaderGeneral('btnSubirTSubAdicional', '#tablaTSubInfoAdicional', seccionSubAdicional, descripcion, ruta_interna);
});

function cargarAdicionalTSub() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        cargarArchivosRegistradosDesc(seccionSubAdicional, '#tablaTSubInfoAdicional');
    }
    if (modoModel == "consultar") {
        desactivarCamposFormulario('FichaAdi');
        $('#btnSubirTSubAdicional').hide();
    }
}