var controlador = siteRoot + 'Combustibles/gestionGas/';

const ESTADO_SOLICITADO = 1;
const ESTADO_PROCESADO = 2;
const ESTADO_APROBADO = 3;
const ESTADO_DESAPROBADO = 4;
const ESTADO_FUERA_PLAZO = 5;
const ESTADO_OBSERVADO = 6;
const ESTADO_SUBSANADO = 7;
const ESTADO_CANCELADO = 8;
const ESTADO_APROBADO_PARCIAL = 10;
const ESTADO_SOLICITUD_ASIGNACION = 11;
const ESTADO_ASIGNADO = 12;

const FORMATO3 = 0;
const ARCHIVOS = 1;

const GUARDADO_TEMPORAL = 1;
const GUARDADO_OFICIAL = 2;

function buscarAutoguardados() {
    var idEnvio = $("#hfIdEnvio").val() || 0;
    var tipoCentral = $("#hdTipoCentral").val();
    var mesVigencia = $("#cbMes").val();
    var idEmpresa = $("#cbEmpresa").val();
    var estenvcodi = $("#hdIdEstado").val() || 0;

    $.ajax({
        type: 'POST',
        url: controlador + 'BuscarAutoguardados',
        dataType: 'json',
        data: {
            idEnvio: idEnvio,
            tipoCentral: tipoCentral,
            mesVigencia: mesVigencia,
            idEmpresa: idEmpresa,
            estenvcodi: estenvcodi
        },
        cache: false,
        success: function (evt) {
            //if (evt.Resultado != "-1") {
            //    alert('Se efectuó la aprobación parcial correctamente.');
            //    regresarListaPrincipal(ESTADO_APROBADO_PARCIAL);
            //} else {
            //    mostrarMensaje('mensaje_popupAprobarParcialmente', 'error', 'Ha ocurrido un error: ' + evt.Mensaje);
            //}
        },
        error: function (err) {
            //mostrarMensaje('mensaje_popupAprobarParcialmente', 'error', 'Ha ocurrido un error.');
        }
    });
}

function cerrarPopup(id) {
    $("#" + id).bPopup().close()
}

function abrirPopup(contentPopup) {
    setTimeout(function () {
        $("#" + contentPopup).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}

function mostrarMensaje(id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

}

function limpiarBarraMensaje(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
}

//Usado en envios con estado = Aprobados Parcialmente
function obtenerDatosCentrales() {

    var listaCentrales = MODELO_LISTA_CENTRAL; //variable global de solicitudExcelWeb.js
    var numCentrales = listaCentrales.length;

    var listaDataCentrales = [];

    if (numCentrales > 0) {
        for (var i = 0; i < numCentrales; i++) {

            var central = listaCentrales[i];

            var objCentralX = {
                Equicodi: central.Equicodi, //codigo
                NombCentral: central.Central, //nombre central
                Tipo: $("#hdTipoCentral").val(), //'N':Nueva, 'E':Existente
                DataFormularioF3: ObtenerDataHandson(central.Equicodi, central.ArrayItemObs, FORMATO3),
                DataFormularioArchivos: ObtenerDataHandson(central.Equicodi, central.ArrayItemObs, ARCHIVOS)
            };

            listaDataCentrales.push(objCentralX);
        }
    }

    return listaDataCentrales;
}

//Usado en envios con estado = Aprobados Parcialmente
function ObtenerDataHandson(equicodi, listaItemObs, tipo) {
    var listaDataSeccion = [];

    if (tipo == FORMATO3) {
        var dataHandson = LISTA_OBJETO_HOJA[equicodi].hot.getData();
        for (var i = 0; i < listaItemObs.length; i++) {
            var objItemObs = listaItemObs[i];
            if (objItemObs.EsColEstado) {
                var codigoEstado = 0;
                var cadenaEstado = (dataHandson[objItemObs.PosRow][objItemObs.PosCol] ?? "").toUpperCase();
                switch (cadenaEstado) {
                    case "CONFORME":
                        codigoEstado = 1;
                        break;
                    case "OBSERVADO":
                        codigoEstado = 2;
                        break;
                    case "SUBSANADO":
                        codigoEstado = 3;
                        break;
                    case "NO SUBSANADO":
                        codigoEstado = 4;
                        break;
                }

                var objSeccion = {
                    NumSeccion: objItemObs.NumeralSeccion, //devuelve num de la seccion
                    //ValorObs: seccion.Valor, //devuelve valor (no es necesario)
                    Estado: codigoEstado, //devuelve estado  (entero)  // 1:Conforme, 2:Observado, 3:Subsanado y 4:No subsanado
                };

                listaDataSeccion.push(objSeccion);
            }
        }
    }

    if (tipo == ARCHIVOS) {
        //pendiente
    }
    return listaDataSeccion;
}

//////////////////////////////////////////////////////////////////////////////
function mostrarInformativo(alerta) {
    $('#mensajeEvento').removeClass("action-message");
    $('#mensajeEvento').removeClass("action-alert");
    $('#mensajeEvento').removeClass("action-exito");
    $('#mensajeEvento').addClass("action-message");
    $('#mensajeEvento').html(alerta);
    $('#mensajeEvento').css("display", "block");
}

function mostrarAlerta(alerta) {
    $('#mensajeEvento').removeClass("action-message");
    $('#mensajeEvento').removeClass("action-alert");
    $('#mensajeEvento').removeClass("action-exito");
    $('#mensajeEvento').addClass("action-alert");
    $('#mensajeEvento').html(alerta);
    $('#mensajeEvento').css("display", "block");
}

function mostrarError(alerta) {
    $('#mensajeEvento').removeClass("action-message");
    $('#mensajeEvento').removeClass("action-alert");
    $('#mensajeEvento').removeClass("action-exito");
    $('#mensajeEvento').addClass("action-error");
    $('#mensajeEvento').html(alerta);
    $('#mensajeEvento').css("display", "block");
}
