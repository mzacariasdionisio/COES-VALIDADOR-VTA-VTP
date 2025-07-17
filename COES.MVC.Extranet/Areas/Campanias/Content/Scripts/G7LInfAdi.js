var seccionLinAdicional = 37;
var descripcion = "InfoAdicional";

$(function () {
    var ruta_interna = obtenerRutaProyecto();  
    crearUploaderGeneral('btnSubirLinAdicional', '#tablaLinInfoAdicional', seccionLinAdicional, descripcion, ruta_interna);
});

function cargarAdicionalLin() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        cargarArchivosRegistradosDesc(seccionLinAdicional, '#tablaLinInfoAdicional');
    }
    if (modoModel == "consultar") {
        desactivarCamposFormulario('FichaAdi');
        $('#btnSubirLinAdicional').hide();
   
        $('.btnEliminar').hide();
    }
}
if (modoModel == "consultar") {
    desactivarCamposFormulario('FichaAdi');
    $('#btnSubirLinAdicional').hide();

    $('.btnEliminar').hide();
}