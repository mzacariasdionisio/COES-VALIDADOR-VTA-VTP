var seccionSolAdicional = 35;
var descripcion = "InfoAdicional";

$(function () {
    var ruta_interna = obtenerRutaProyecto();  
    crearUploaderGeneral('btnSubirFormatoSol', '#tablaAdiSol', seccionSolAdicional, descripcion, ruta_interna);
});

function cargarAdicionalSol() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        cargarArchivosRegistradosDesc(seccionSolAdicional, '#tablaAdiSol');
    }
    if (modoModel == "consultar") {
        desactivarCamposFormulario('FichaAdi');
        $('#btnSubirFormatoSol').hide();
    }
}