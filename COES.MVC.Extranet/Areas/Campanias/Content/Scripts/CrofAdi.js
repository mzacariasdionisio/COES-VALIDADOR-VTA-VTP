var seccionCroAdicional = 46;
var descripcion = "InfoAdicional";

$(function () {
    var ruta_interna = obtenerRutaProyecto();  
    crearUploaderGeneral('btnSubirCroAdicional', '#tablaCroInfoAdicional', seccionCroAdicional, descripcion, ruta_interna);
});

function cargarAdicionalCro() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        cargarArchivosRegistradosDesc(seccionCroAdicional, '#tablaCroInfoAdicional');
    }
}