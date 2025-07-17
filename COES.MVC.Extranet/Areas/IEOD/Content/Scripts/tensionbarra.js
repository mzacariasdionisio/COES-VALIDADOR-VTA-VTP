var controlador = siteRoot + 'IEOD/TensionBarra/';

$(function () {
    var hoja1 = '1';
    //inicializar variables
    LISTA_OBJETO_HOJA = { '1': crearObjetoHoja() };

    getErrores(hoja1)[ERROR_NO_NUMERO].validar = true;
    getErrores(hoja1)[ERROR_LIM_INFERIOR].validar = true;
    getErrores(hoja1)[ERROR_LIM_SUPERIOR].validar = true;

    setTieneGrafico(false, hoja1);

    var fmt1 = $("#hfFormato1").val();

    //crear vistas
    cargarHoja(hoja1, fmt1, 'viewHojaFormato');
});