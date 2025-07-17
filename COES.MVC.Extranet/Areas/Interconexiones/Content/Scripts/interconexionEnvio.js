var controlador = siteRoot + 'Interconexiones/Envio/';

$(function () {
    var hojaPrincipal = $("#hfHojaMain").val();
    var hoja1 = $("#hfHojaPri").val();
    var hoja2 = $("#hfHojaSec").val();

    //inicializar variables
    LISTA_OBJETO_HOJA = {};
    LISTA_OBJETO_HOJA[hojaPrincipal] = crearObjetoHoja();
    LISTA_OBJETO_HOJA[hoja1] = crearObjetoHoja();
    LISTA_OBJETO_HOJA[hoja2] = crearObjetoHoja();

    getErrores(hoja1)[ERROR_NO_NUMERO].validar = true;
    getErrores(hoja1)[ERROR_LIM_INFERIOR].validar = true;
    getErrores(hoja1)[ERROR_LIM_SUPERIOR].validar = true;
    getErrores(hoja1)[ERROR_DATA_INTERCONEXION].validar = true;

    getErrores(hoja2)[ERROR_NO_NUMERO].validar = true;
    getErrores(hoja2)[ERROR_LIM_INFERIOR].validar = true;
    getErrores(hoja2)[ERROR_LIM_SUPERIOR].validar = true;
    getErrores(hoja2)[ERROR_DATA_INTERCONEXION].validar = true;

    var listaHojaView = [];
    listaHojaView.push(crearHojaView(hoja1, 'viewMedidorPrincipal', getHoja(hoja1)));
    listaHojaView.push(crearHojaView(hoja2, 'viewMedidorSecundario', getHoja(hoja2)));

    setListaHoja(listaHojaView, hojaPrincipal);
    setTieneFiltro(false, hojaPrincipal);
    setEsHojaPadre(true, hojaPrincipal);

    //crear vistas
    inicializarHojaView(hojaPrincipal);
});