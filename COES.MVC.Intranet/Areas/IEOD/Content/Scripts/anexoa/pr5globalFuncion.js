////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Funciones de validación

//variables globales
var resultFiltro = false;

//validar filtros por cadenas con formart "," y "/"
function valida1(str) {
    var filtro = filtro || [];
    str = str.split("|");

    $.each(str, function (i, l) {
        var str2 = l.split(";");

        filtro.push({ id: str2[0], mensaje: str2[1] })
    })

    for (var i in filtro) {

        if (filtro[i].id.trim() != "") {
            resultFiltro = true;
        }
        else {
            alert(filtro[i].mensaje);
            resultFiltro = false;
            break;
        }
    }
    filtro.push();
}

//validar filtros por Arrays
function validarFiltros(filtro) {
    for (var i in filtro) {
        if (filtro[i].id != "") {
            resultFiltro = true;
        }
        else {
            alert(filtro[i].mensaje);
            resultFiltro = false;

            break;
        }
    }
    filtro = [];
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Funciones para obtener el valor de los inputs

function getEmpresa() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if ($('#cbEmpresa').multipleSelect('isAllSelected') == "S" || empresa == "[object Object]" || empresa.length == 0) empresa = "-1";
    $('#hfEmpresa').val(empresa);
    var idEmpresa = $('#hfEmpresa').val();

    return idEmpresa;
}

function getUbicacion() {
    var ubicacion = $('#cbUbicacion').multipleSelect('getSelects');
    if ($('#cbUbicacion').multipleSelect('isAllSelected') == "S" || ubicacion == "[object Object]" || ubicacion.length == 0) ubicacion = "-1";
    $('#hfUbicacion').val(ubicacion);
    var idUbicacion = $('#hfUbicacion').val();

    return idUbicacion;
}

function getTipoEquipo() {
    var tipoequipo = $('#cbTipoEquipo').multipleSelect('getSelects');
    if ($('#cbTipoEquipo').multipleSelect('isAllSelected') == "S" || tipoequipo == "[object Object]" || tipoequipo.length == 0) tipoequipo = "-1";
    $('#hfTipoEquipo').val(tipoequipo);
    idTipoEquipo = $('#hfTipoEquipo').val();

    return idTipoEquipo;
}

function getCentral() {
    var central = $('#cbCentral').multipleSelect('getSelects');
    if ($('#cbCentral').multipleSelect('isAllSelected') == "S" || central == "[object Object]" || central.length == 0) central = "-1";
    $('#hfCentral').val(central);
    var idEmpresa = $('#hfCentral').val();

    return idEmpresa;
}

function getFechaInicio() {
    var fechaInicio = $('#txtFechaInicio').val();
    return fechaInicio;
}

function getFechaFin() {
    var fechaFin = $('#txtFechaFin').val();
    if (fechaFin === undefined || fechaFin === null || fechaFin == "")
        return getFechaInicio();
    return fechaFin;
}

function getTipoOperacion() {
    var toperacion = $('#cbTipoOperacion').multipleSelect('getSelects');
    if (toperacion == "[object Object]") toperacion = "-1";
    $('#hfTipoOpe').val(toperacion);
    var idTOperacion = $('#hfTipoOpe').val();

    return idTOperacion;
}

function getTipocentral() {
    var tipocentral = $('#cbTipoCentral').multipleSelect('getSelects');
    if (tipocentral == "[object Object]" || tipocentral.length == 0) central = "-1";
    $('#hfTipoCentral').val(tipocentral);
    var idTCentral = $('#hfTipoCentral').val();

    return idTCentral;
}

function getTipoRecurso() {
    var recurso = $('#cbTipoRecurso').multipleSelect('getSelects');
    if (recurso == "[object Object]" || recurso.length == 0) recurso = "-1";
    $('#hfRecurso').val(recurso);
    var idTipoRecurso = $('#hfTipoCentral').val();

    return idTipoRecurso;
}

function getTipoGeneracion() {
    var generacion = $('#cbTipoGeneracion').multipleSelect('getSelects');
    if (generacion == undefined || generacion == "[object Object]" || generacion.length == 0) generacion = "-1";
    $('#hfTipoGeneracion').val(generacion);
    var idTipoGeneracion = $('#hfTipoGeneracion').val();

    return idTipoGeneracion;
}

function getPtoMedicion() {
    var ptomedicion = $('#cbPtoMedicion').multipleSelect('getSelects');
    if (ptomedicion == "[object Object]" || ptomedicion.length == 0 || ptomedicion == "") ptomedicion = "-1";
    $('#hfPtoMedicion').val(ptomedicion);

    var idPtoMedicion = $('#hfPtoMedicion').val();

    return idPtoMedicion;
}

function getCuenca() {
    var cuenca = $('#cbCuenca').multipleSelect('getSelects');
    if ($('#cbCuenca').multipleSelect('isAllSelected') == "S" || cuenca == "[object Object]" || cuenca.length == 0 || cuenca == "") cuenca = "-1";
    $('#hfCuenca').val(cuenca);

    var idCuenca = $('#hfCuenca').val();

    return idCuenca;
}

function getFamilia() {
    var familia = $('#cbFamilia').multipleSelect('getSelects');
    if (familia == "[object Object]" || familia.length == 0 || familia == "") familia = "-1";
    $('#hfFamilia').val(familia);

    var idFamilia = $('#hfFamilia').val();

    return idFamilia;
}

function getCentrales() {

}

function getTipoCombustible() {
    var tcombustible = $('#cbTipoCombustible').multipleSelect('getSelects');
    if ($('#cbTipoCombustible').multipleSelect('isAllSelected') == "S" || tcombustible == "[object Object]" || tcombustible.length == 0) tcombustible = "-1";
    $('#hfTipoCombustible').val(tcombustible);
    var idTCombustible = $('#hfTipoCombustible').val();

    return idTCombustible;
}

function getTipoCombustibleXcentral() {
    var tcombustible = $('#cbTipoCombustibleXcentral').multipleSelect('getSelects');
    if (tcombustible == "[object Object]" || tcombustible.length == 0) tcombustible = "-1";
    $('#hfTipoCombustibleXcentral').val(tcombustible);
    var idTCombustibleXcentral = $('#hfTipoCombustibleXcentral').val();

    return idTCombustibleXcentral;
}

function getTipoCombustibleXTipoCentral() {
    var tcombustible = $('#cbTipoCombustibleXTipoCentral').multipleSelect('getSelects');
    if (tcombustible == "[object Object]" || tcombustible.length == 0) tcombustible = "-1";
    $('#hfTipoCombustibleXTipoCentral').val(tcombustible);
    var idTCombustibleXTipoCentral = $('#hfTipoCombustibleXTipoCentral').val();

    return idTCombustibleXTipoCentral;
}

function getTipoCombustibleXmodo() {
    var tcombustible = $('#cbTipoCombustibleXmodo').multipleSelect('getSelects');
    if (tcombustible == "[object Object]" || tcombustible.length == 0) tcombustible = "-1";
    $('#hfTipoCombustibleXmodo').val(tcombustible);
    var idTCombustibleXmodo = $('#hfTipoCombustibleXmodo').val();

    return idTCombustibleXmodo;
}

function getModoOpeGrupos() {
    var modo = $('#cbModosOpeGrupos').multipleSelect('getSelects');
    if (modo == "[object Object]") modo = "-1";
    $('#hfModo').val(modo);
    var idModo = $('#hfModo').val();

    return idModo;
}

function getSistemaA() {
    var sistemaA = $('#cbSistemaA').multipleSelect('getSelects');
    if (sistemaA == "[object Object]") sistemaA = "-1";
    $('#hfSistemaA').val(sistemaA);
    var idSistemaA = $('#hfSistemaA').val();

    return idSistemaA;
}

function getTOperacion() {
    var tipoOpe = $('#cbTipoOpe').multipleSelect('getSelects');
    if (tipoOpe == "[object Object]") tipoOpe = "-1";
    $('#hfTipoOpe').val(tipoOpe);
    var idtipoOpe = $('#hfTipoOpe').val();

    return idtipoOpe;
}

function getOtraClasificacion() {
    var clasificacion = $('#cbClasificacion').multipleSelect('getSelects');
    if (clasificacion == "[object Object]") clasificacion = "-1";
    $('#hfClasificacion').val(clasificacion);
    var idClasificacion = $('#hfClasificacion').val();

    return idClasificacion;
}

function getTipoOperacion() {
    var tipoOpe = $('#cbTipoOperacion').multipleSelect('getSelects');
    if (tipoOpe == "[object Object]") tipoOpe = "-1";
    $('#hfTipoOperacion').val(tipoOpe);
    var idtipoOpe = $('#hfTipoOperacion').val();

    return idtipoOpe;
}

function getSubestacion() {
    var subestacion = $('#cbSubEstacion').multipleSelect('getSelects');
    if ($('#cbSubEstacion').multipleSelect('isAllSelected') == "S" || subestacion == "[object Object]" || subestacion.length == 0) subestacion = "-1";
    $('#hfSubEstacion').val(subestacion);
    var idSubestacion = $('#hfSubEstacion').val();

    return idSubestacion;
}

function getAreaOperativa() {
    var obj = $('#cbAreaOperativa').multipleSelect('getSelects');
    if (obj == "[object Object]") obj = "-1";
    $('#hfAreaOperativa').val(obj);
    var idAreaOperativa = $('#hfAreaOperativa').val();

    return idAreaOperativa;
}

function getEquipo() {
    var equipo = $('#cbEquipo').multipleSelect('getSelects');
    if ($('#cbEquipo').multipleSelect('isAllSelected') == "S" || equipo == "[object Object]" || central.length == 0) equipo = "-1";
    $('#hfEquipo').val(equipo);
    idEquipo = $('#hfEquipo').val();

    return idEquipo;
}

function getGPS() {
    var obj = $('#cbGPS').multipleSelect('getSelects');
    if (obj == "[object Object]") obj = "-1";
    $('#hfGPS').val(obj);
    var idobj = $('#hfGPS').val();

    return idobj;
}

function getTipoLectura48() {
    return parseInt($('input[name=cbLectura48]:checked').val()) || 0;
}

//
//funcion que calcula el ancho disponible para la tabla reporte
function getHeightTablaListado() {
    return $(window).height()
        - $(".header").outerHeight(true)
        - $("#cntTitulo").outerHeight(true)
        - $("#divGeneral div.form-title").outerHeight(true)
        - 15 //padding-top #mainLayout
        - $("#mainLayout.content-hijo div.search-content").outerHeight(true)
        //- tamaño de la cabecera de la tabla se define en el js que lo usa 
        - 10 //faltante
        - 15 //padding-top #mainLayout
        - $(".footer").outerHeight(true)
        ;
}