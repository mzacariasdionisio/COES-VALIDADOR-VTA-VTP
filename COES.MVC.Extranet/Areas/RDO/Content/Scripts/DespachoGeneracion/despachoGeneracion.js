var controlador = siteRoot + 'RDO/DespachoGeneracion/';
var hojaPrincipal = '';

$(function () {
    $('#cbTipoFormato').unbind('change');
    $('#cbTipoFormato').change(function () {
        CANTIDAD_CLICK_TIPO_FORMATO++;
        cargarFormatoSinTabs();
    });
    cargarFormatoSinTabs();
});

function cargarFormatoSinTabs() {

    var fmt1 = parseInt($('#cbTipoFormato').val()) || 0;

    var hoja1 = '1';
    //inicializar variables
    LISTA_OBJETO_HOJA = { '1': crearObjetoHoja() };

    setVerUltimoEnvio(NO_VER_ULTIMO_ENVIO, hoja1);

    getErrores(hoja1)[ERROR_NO_NUMERO].validar = true;
    getErrores(hoja1)[ERROR_LIM_INFERIOR].validar = true;
    getErrores(hoja1)[ERROR_LIM_SUPERIOR].validar = true;
    getErrores(hoja1)[ERROR_BLANCO].validar = false;
    //getErrores(hoja1)[ERROR_NUMERO_NEGATIVO].validar = true;
    getErrores(hoja1)[ERROR_UNIDAD].validar = false;
    getErrores(hoja1)[ERROR_DATA_DESPACHO].validar = false;

    setTieneGrafico(true, hoja1);

    //crear vistas
    cargarHoja(hoja1, fmt1, 'viewHojaFormato');
}

function generaFiltroGrafico(numHoja) { }

//function mostrarGrafico(numHoja) {
//    generarFiltroGraficoFormatoIEOD(numHoja);
//    graficoFormatoCentralUnidadMedidaEje(numHoja);

//    setTimeout(function () {
//        $(getIdGrafico(numHoja)).bPopup({
//            easing: 'easeOutBack',
//            speed: 450,
//            transition: 'slideDown',
//            modalClose: false
//        });
//    }, 50);
//}