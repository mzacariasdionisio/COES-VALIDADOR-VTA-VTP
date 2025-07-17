function getEmpresa() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]" || empresa.length == 0) empresa = "-1";
    $('#hfEmpresa').val(empresa);
    var idEmpresa = $('#hfEmpresa').val();

    return idEmpresa;
}

function getCentral() {
    var central = $('#cbCentral').multipleSelect('getSelects');
    if (central == "[object Object]" || central.length == 0) central = "-1";
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
    if (tipocentral == "[object Object]") central = "-1";
    $('#hfTipoCentral').val(tipocentral);
    var idTCentral = $('#hfTipoCentral').val();

    return idTCentral;
}

function getTipoCombustible() {
    var tcombustible = $('#cbTipoCombustible').multipleSelect('getSelects');
    if (tcombustible == "[object Object]") tcombustible = "-1";
    $('#hfTipoCombustible').val(tcombustible);
    var idTCombustible = $('#hfTipoCombustible').val();

    return idTCombustible;
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
    if (subestacion == "[object Object]" || subestacion.length == 0) subestacion = "-1";
    $('#hfSubEstacion').val(subestacion);
    var idSubestacion = $('#hfSubEstacion').val();

    return idSubestacion;
}

function getAreaOperativa() {
    var obj = $('#cbAreaOperativa').multipleSelect('getSelects');
    if (obj == "[object Object]") obj = "-1";
    $('#hfAreaOperativa').val(obj);
    var idobj = $('#hfAreaOperativa').val();

    return idobj;
}


function getEquipo() {
    var equipo = $('#cbEquipo').multipleSelect('getSelects');
    if (equipo == "[object Object]" || central.length == 0) equipo = "-1";
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