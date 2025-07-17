var STEP_FORMULARIO = 0;
var STEP_EQUIPO = 1;
var STEP_PROCESAR = 2;

var TIPO_MIGR_DUPLICIDAD = 1;
var TIPO_MIGR_INSTALACION_NO_CORRESPONDEN = 2;
var TIPO_MIGR_CAMBIO_RAZON_SOCIAL = 3;
var TIPO_MIGR_FUSION = 4;
var TIPO_MIGR_TRANSFERENCIA = 5;

var NOTA_MIGR_DUPLICIDAD = `
Se utiliza cuando la empresa destino y origen son realmente la misma empresa y <b>existió un error de registro de la razón social o nombre de la empresa origen</b>. A esta última se le dará de baja como paso final del proceso.
`;
var NOTA_MIGR_INSTALACION_NO_CORRESPONDEN = `
Se utiliza cuando <b>existió un registro erróneo de equipos asociados a la empresa origen</b>. En esta operación no se requiere la Fecha de Corte o también llamada Fecha de transferencia.
`;
var NOTA_MIGR_CAMBIO_RAZON_SOCIAL = `
Se utiliza cuando se ha hecho el cambio de la razón social desde una fecha de Corte y es necesario realizarla desde la empresa con la antigua razón social (empresa origen) a la que posee la nueva razón social (empresa destino). A la primera mencionada se le dará de baja como paso final del proceso, pero contará con la información histórica hasta la fecha de transferencia.
<br/><br/>
Antes de realizar la transferencia se debe ingresar al módulo de Administración de Empresas y crear un nuevo registro (este debe tener la nueva Razón Social y el mismo RUC).
`;
var NOTA_MIGR_FUSION = `
Una empresa absorbe a otra y se requiere transferir todos los equipos, puntos de medición y toda la información asociada de la empresa que será absorbida en la fecha de la fusión (fecha de corte o transferencia). A la empresa absorbida se le dará de baja como paso final del proceso, pero contará con la información histórica hasta la fecha de corte o transferencia.
`;
var NOTA_MIGR_TRANSFERENCIA = `
Se utiliza cuando se producen transferencia parcial de equipos de una empresa a otra o escisión de empresas a una fecha de transferencia.
`;

var TIPO_EMPRESA_ORIGEN = 1;
var TIPO_EMPRESA_DESTINO = 2;
var PREFIJO_EMPRESA_ORIGEN = 'orig_';
var PREFIJO_EMPRESA_DESTINO = 'dest_';

var ESTADO_EMP_BAJA = 'B';
var ESTADO_EMP_ACTIVO = 'A';
var ESTADO_EMP_ELIMINADO = 'E';

var ESTADO_EQ_ELIMINADO = "X";
var ESTADO_EQ_NO_ELIMINADO = "NX";
var ESTADO_EQ_TODOS = "-1";

var OBJETO_DATA = {
    origen: generarObjTransf(TIPO_EMPRESA_ORIGEN),
    destino: generarObjTransf(TIPO_EMPRESA_DESTINO),
    tipoMigraOp: 0,
    emprcodiOrigen: 0,
    emprcodi: 0,
    strFechaCorte: '',
    validado: false,
    actualizarInstalaciones: false
};

function generarObjTransf(tipo) {
    var obj = {
        empresa : null,
        tipoObj: tipo,
        DATA_INICIAL_AREA: [],
        DATA_INICIAL_FAMILIA: [],
        DATA_INICIAL_ORIGEN_LECTURA: [],
        DATA_INICIAL_EQUIPO: [],

        DATA_SELECT_AREA: [],
        DATA_SELECT_FAMILIA: [],
        DATA_SELECT_EQUIPO: [],

        SELECT_CODIGO_AREA: -1,
        SELECT_CODIGO_FAMILIA: -1,
    };

    return obj;
}

function tieneFechaCorte(tipoMigraOp) {
    return TIPO_MIGR_CAMBIO_RAZON_SOCIAL == tipoMigraOp
        || TIPO_MIGR_FUSION == tipoMigraOp
        || TIPO_MIGR_TRANSFERENCIA == tipoMigraOp;
}

function tieneTransferirEmpresa(tipoMigraOp) {
    return TIPO_MIGR_DUPLICIDAD == tipoMigraOp
        || TIPO_MIGR_CAMBIO_RAZON_SOCIAL == tipoMigraOp
        || TIPO_MIGR_FUSION == tipoMigraOp;
}

//
function mostrarError(msj) {
    var msjHtml = msj !== undefined && msj != null ? msj : "Ha ocurrido un error";
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').html(msjHtml);
    $('#mensaje').css("display", "block");
};

function mostrarMensaje(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};

function mostrarExitoOperacion() {
    $('#mensaje').removeClass("action-error");
    $('#mensaje').addClass("action-exito");
    $('#mensaje').text("La operación se realizó con exito...!");
    $('#mensaje').css("display", "block");
};

//
function mostrarDetalleTransferencia(idTransferencia) {
    document.location.href = controlador + 'DetalleTransferencia?idTransferencia=' + idTransferencia;
};