var controlador = siteRoot + 'PMPO/ModificacionCentralSDDP/';

var AGREGAR_MODIF = 1;
var EDITAR_MODIF = 2;
var DETALLES_MODIF = 3;

var DE_LISTADO = 1;
var DE_POPUP = 2;

var validarCambioDePestaña = true;

$(function () {

    $('#tab-container').easytabs({
        animate: false,
        select: '#vistaListado'
    });

    $('#tab-container').bind('easytabs:before', function () {
        var esTabDetalle = $("#tab-container .tab.active").html().toLowerCase().includes('detalle');
        var existeHtmlTabDetalle = $("#vistaDetalle").html().trim() != '';
        var esEditarCrear = parseInt($("#hfAccionDetalle").val()) != DETALLES_MODIF;

        if (validarCambioDePestaña) {
            if (esTabDetalle && existeHtmlTabDetalle && esEditarCrear) {
                if (confirm('¿Desea cambiar de pestaña? Si selecciona "Aceptar" se perderán los cambios. Si selecciona "Cancelar" se mantendrá en la misma pestaña')) {
                    $("#vistaDetalle").html(''); //limpiar tab Detalle
                    limpiarBarraMensaje('mensaje');
                } else {
                    return false;
                }
            }
        }
        validarCambioDePestaña = true;
    });

    mostrarListadoModificacionCentral(); 

    $('#btnAgregarModificacion').click(function () {
        var topcodi = parseInt($("#hdTopcodi").val()) || 1;
        mantenerModificacionCentral(AGREGAR_MODIF, topcodi, null, DE_LISTADO);
    });

    //Exportar Informacion 
    $('#btnExportar').click(function () {
        var topcodi = parseInt($("#hdTopcodi").val()) || 1;
        exportarInformacion(topcodi);
    });
});

function eliminarModificacionCentral(topcodi, recurcodi) {

    var msgConfirmacion = '¿Esta seguro que desea eliminar la Modificación de Central SDDP?';

    if (confirm(msgConfirmacion)) {
        $.ajax({
            url: controlador + "EliminarModificacionCentralSDDP",
            data: {
                topcodi: topcodi,
                recurcodi: recurcodi
            },
            type: 'POST',
            success: function (result) {

                if (result.Resultado === "-1") {
                    mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al eliminar la Modificación de Central SDDP: ' + result.Mensaje);
                } else {
                    mostrarListadoModificacionCentral();
                    mostrarMensaje_('mensaje', 'exito', 'Eliminación de Modificación Central SDDP realizada con éxito.');
                }
            },
            error: function (xhr, status) {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function mostrarListadoModificacionCentral() {

    var topcodi = parseInt($("#hdTopcodi").val()) || 1;
    var action = parseInt($("#hdAction").val()) || 0;
    limpiarBarraMensaje('mensaje');
    $.ajax({
        type: 'POST',
        url: controlador + "ListarModificacionCentraleSDDP",
        dataType: 'json',
        data: {
            topcodi: topcodi,
            action: action
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                $("#cuadroModificaciones").html(evt.HtmlListadoModificacionCentral);
                refrehDatatable();

            } else {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error: ' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function refrehDatatable() {
    $('#tabla_ModificacionCentral').dataTable({
        "scrollY": 430,
        "scrollX": true,
        "sDom": 't',
        "ordering": false,
        "iDisplayLength": 50
    });
}

function guardarModificacionCentral(accion) {

    var objModificacion = {};
    objModificacion = getObjetoCreacionJson(accion);

    var mensaje = validarDatos(objModificacion);
    if (mensaje == "") {
        limpiarBarraMensaje('mensaje');
        var dataJson = JSON.stringify(objModificacion).replace(/\/Date/g, "\\\/Date").replace(/\)\//g, "\)\\\/");

        $.ajax({
            type: 'POST',
            url: controlador + "RegistarModificacionCentralSDDP",
            dataType: 'json',
            data: {
                dataJson: dataJson,
                accion: accion
            },
            success: function (resultado) {
                if (resultado.Resultado == "1") {
                    $("#vistaDetalle").html('');//limpiar tab Detalle
                    validarCambioDePestaña = false;
                    $('#tab-container').easytabs('select', '#vistaListado');

                    mostrarMensaje_('mensaje', 'exito', 'Modificación Central SDDP registrada con éxito.');
                    mostrarListadoModificacionCentral();
                }
                if (resultado.Resultado == "-1") {
                    mostrarMensaje_('mensaje', 'error', resultado.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error:' + err);
            }
        });
    }
    else {
        mostrarAlerta('mensaje', 'error', mensaje);
    }

}

function getObjetoCreacionJson(accion) {
    var objTop = {};
    var objRecurso = {};
    //var objPropRecurso = {};
    objRecurso.dataPropRecursoSDDP = [];

    if (accion == AGREGAR_MODIF) {

        //topologia
        var fechafutura = $("#txtFechaFutura").val();
        var sDia = fechafutura.substring(0, 2);
        var sMes = fechafutura.substring(3, 5) - 1;
        var sAnio = fechafutura.substring(6, 10);
        objTop.Mtopfechafutura = new Date(sAnio, sMes, sDia);
        //objTop.Mtopcodi = $("#hdtopcodi").val();
        objTop.Mtopcodi = $("#hdTopcodi").val();
        objTop.Mtopcodipadre = $("#hdTopcodi").val();
        //recurso
        //objRecurso.Mtopcodi = $("#hdtopcodi").val();
        objRecurso.Mtopcodi = $("#hdTopcodi").val();
        objRecurso.Mrecurcodi = $("#cboRecurso").val();
        objRecurso.Mrecurnomb = $("#txtDescripcionSddp").val();
        objRecurso.NumeroSDDP = $("#txtCodigoSddp").val() || "";
    }
    if (accion == EDITAR_MODIF) {
        //topologia
        var fechafutura = $("#txtFechaFutura").val();
        var sDia = fechafutura.substring(0, 2);
        var sMes = fechafutura.substring(3, 5) - 1;
        var sAnio = fechafutura.substring(6, 10);
        objTop.Mtopfechafutura = new Date(sAnio, sMes, sDia);
        objTop.Mtopcodi = $("#hdtopcodi").val();
        objTop.Mtopcodipadre = $("#hdTopcodi").val();
        //recurso
        objRecurso.Mtopcodi = $("#hdtopcodi").val();
        objRecurso.Mrecurcodi = $("#cboRecurso").val();
        objRecurso.Mrecurnomb = $("#txtDescripcionSddp").val();
        objRecurso.NumeroSDDP = $("#txtCodigoSddp").val() || "";
    }

    //Lista propiedades recurso
    var arrayPropSddp = [];
    var potencia = {
        Mpropcodi: $("#propcodiPotencia").val(),
        Mprvalvalor: $("#txtPotencia").val(),
    }
    arrayPropSddp.push(potencia);
    var coefProduccion = {
        Mpropcodi: $("#propcodiCoefProd").val(),
        Mprvalvalor: $("#txtCoefProduccion").val(),
    }
    arrayPropSddp.push(coefProduccion);
    var caudalMinTur = {
        Mpropcodi: $("#propcodiCaudalMinTur").val(),
        Mprvalvalor: $("#txtCaudalMinTur").val(),
    }
    arrayPropSddp.push(caudalMinTur);
    var caudalMaxTur = {
        Mpropcodi: $("#propcodiCaudalMaxTur").val(),
        Mprvalvalor: $("#txtCaudalMaxTur").val(),
    }
    arrayPropSddp.push(caudalMaxTur);
    var defluenciaTotMin = {
        Mpropcodi: $("#propcodiDefluenciaTotMin").val(),
        Mprvalvalor: $("#txtDefluenciaTotMin").val(),
    }
    arrayPropSddp.push(defluenciaTotMin);
    var icp = {
        Mpropcodi: $("#propcodiICP").val(),
        Mprvalvalor: $("#txtICP").val(),
    }
    arrayPropSddp.push(icp);
    var ih = {
        Mpropcodi: $("#propcodiIH").val(),
        Mprvalvalor: $("#txtIH").val(),
    }
    arrayPropSddp.push(ih);
    var volumenMax = {
        Mpropcodi: $("#propcodiVolumenMax").val(),
        Mprvalvalor: $("#txtVolumenMax").val(),
    }
    arrayPropSddp.push(volumenMax);

    //radio
    var indicadorSI = $("#rdCHSiEA").is(':checked');
    var indicadorNO = $("#rdCHNoEA").is(':checked');
    if (indicadorSI) {
        //objPropRecurso.IndicadorEA = 1;
        var indicadorEASi = {
            Mpropcodi: $("#propcodiIndicadorEA").val(),
            Mprvalvalor: 1,
        }
        arrayPropSddp.push(indicadorEASi);
    }
    if (indicadorNO) {
        //objPropRecurso.IndicadorEA = 0;
        var indicadorEANo = {
            Mpropcodi: $("#propcodiIndicadorEA").val(),
            Mprvalvalor: 0,
        }
        arrayPropSddp.push(indicadorEANo);
    }

    objTop.RecursoSddp = objRecurso;
    objRecurso.ListaPropRecursoSddp = arrayPropSddp;

    return objTop;
}

function validarDatos(obj) {

    var objRecurso = obj.RecursoSddp;

    var validacion = "<ul>";
    var flag = true;

    if (objRecurso.Mrecurcodi <= 0) {
        validacion = validacion + "<li>Nombre SDDP: campo requerido.</li>";//Campo Requerido
        flag = false;
    }
    if (objRecurso.NumeroSDDP == null || objRecurso.NumeroSDDP == '') {
        validacion = validacion + "<li>Código SDDP: campo requerido.</li>";//Campo Requerido
        flag = false;
    }
    //if (objRecurso.Mrecurnomb == null || objRecurso.Mrecurnomb == '') {
    //    validacion = validacion + "<li>Descripción: campo requerido.</li>";
    //    flag = false;
    //}
    if (obj.Mtopfechafutura == null || obj.Mtopfechafutura == '') {
        validacion = validacion + "<li>Fecha Modificación: campo requerido.</li>";//Campo Requerido
        flag = false;
    }

    if ($("#txtPotencia").val() == null || $("#txtPotencia").val() == '') {
        validacion = validacion + "<li>Potencia: campo requerido.</li>";//Campo Requerido
        flag = false;
    }
    else {
        if (validarNumero($("#txtPotencia").val())) {
            if (parseFloat($("#txtPotencia").val()) < 0) {
                validacion = validacion + "<li>Potencia: Debe ser un número mayor o igual a cero.</li>";
                flag = false;
            }
        }
        else {
            validacion = validacion + "<li>Potencia: Debe ser un número.</li>";
            flag = false;
        }
    }

    if ($("#txtCoefProduccion").val() == null || $("#txtCoefProduccion").val() == '') {
        validacion = validacion + "<li>Coef. de Producción: campo requerido.</li>";//Campo Requerido
        flag = false;
    }
    else {
        if (validarNumero($("#txtCoefProduccion").val())) {
            if (parseFloat($("#txtCoefProduccion").val()) < 0) {
                validacion = validacion + "<li>Coef. de Producción: Debe ser un número mayor o igual a cero.</li>";
                flag = false;
            }
        }
        else {
            validacion = validacion + "<li>Coef. de Producción: Debe ser un número.</li>";
            flag = false;
        }
    }
    if ($("#txtCaudalMinTur").val() == null || $("#txtCaudalMinTur").val() == '') {
        validacion = validacion + "<li>Caudal Min. Turbinante: campo requerido.</li>";//Campo Requerido
        flag = false;
    }
    else {
        if (validarNumero($("#txtCaudalMinTur").val())) {
            if (parseFloat($("#txtCaudalMinTur").val()) < 0) {
                validacion = validacion + "<li>Caudal Min. Turbinante: Debe ser un número mayor o igual a cero.</li>";
                flag = false;
            }
        }
        else {
            validacion = validacion + "<li>Caudal Min. Turbinante: Debe ser un número.</li>";
            flag = false;
        }
    }
    if ($("#txtCaudalMaxTur").val() == null || $("#txtCaudalMaxTur").val() == '') {
        validacion = validacion + "<li>Caudal Max. Turbinante: campo requerido.</li>";//Campo Requerido
        flag = false;
    }
    else {
        if (validarNumero($("#txtCaudalMaxTur").val())) {
            if (parseFloat($("#txtCaudalMaxTur").val()) < 0) {
                validacion = validacion + "<li>Caudal Max. Turbinante: Debe ser un número mayor o igual a cero.</li>";
                flag = false;
            }
        }
        else {
            validacion = validacion + "<li>Caudal Max. Turbinante: Debe ser un número.</li>";
            flag = false;
        }
    }
    if ($("#txtDefluenciaTotMin").val() == null || $("#txtDefluenciaTotMin").val() == '') {
        validacion = validacion + "<li>Defluencia Total Min: campo requerido.</li>";//Campo Requerido
        flag = false;
    }
    else {
        if (validarNumero($("#txtDefluenciaTotMin").val())) {
            if (parseFloat($("#txtDefluenciaTotMin").val()) < 0) {
                validacion = validacion + "<li>Defluencia Total Min: Debe ser un número mayor o igual a cero.</li>";
                flag = false;
            }
        }
        else {
            validacion = validacion + "<li>Defluencia Total Min: Debe ser un número.</li>";
            flag = false;
        }
    }
    if ($("#txtICP").val() == null || $("#txtICP").val() == '') {
        validacion = validacion + "<li>ICP: campo requerido.</li>";//Campo Requerido
        flag = false;
    }
    else {
        if (validarNumero($("#txtICP").val())) {
            if (parseFloat($("#txtICP").val()) < 0) {
                validacion = validacion + "<li>ICP: Debe ser un número mayor o igual a cero.</li>";
                flag = false;
            }
        }
        else {
            validacion = validacion + "<li>ICP: Debe ser un número.</li>";
            flag = false;
        }
    }
    if ($("#txtIH").val() == null || $("#txtIH").val() == '') {
        validacion = validacion + "<li>IH: campo requerido.</li>";//Campo Requerido
        flag = false;
    }
    else {
        if (validarNumero($("#txtIH").val())) {
            if (parseFloat($("#txtIH").val()) < 0) {
                validacion = validacion + "<li>IH: Debe ser un número mayor o igual a cero.</li>";
                flag = false;
            }
        }
        else {
            validacion = validacion + "<li>IH: Debe ser un número.</li>";
            flag = false;
        }
    }
    if ($("#txtVolumenMax").val() == null || $("#txtVolumenMax").val() == '') {
        validacion = validacion + "<li>Volumen Max: campo requerido.</li>";//Campo Requerido
        flag = false;
    }
    else {
        if (validarNumero($("#txtVolumenMax").val())) {
            if (parseFloat($("#txtVolumenMax").val()) < 0) {
                validacion = validacion + "<li>Volumen Max: Debe ser un número mayor o igual a cero.</li>";
                flag = false;
            }
        }
        else {
            validacion = validacion + "<li>Volumen Max: Debe ser un número.</li>";
            flag = false;
        }
    }

    validacion = validacion + "</ul>";

    if (flag == true) validacion = "";

    return validacion;
}

function validarNumero(texto) {
    return /^-?[\d.]+(?:e-?\d+)?$/.test(texto);
};

function mantenerModificacionCentral(accion, topcodi, recurcodi, origen) {
    $('#tab-container').easytabs('select', '#vistaDetalle');
    mostrarVistaDetalles(accion, topcodi, recurcodi);
    //if (origen == DE_POPUP)
    //    $('#historialMODIF').bPopup().close();
}

function mostrarVistaDetalles(accion, topcodi, recurcodi) {
    $("#vistaDetalle").html('');
    topcodi = topcodi || 0;
    recurcodi = recurcodi || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "CargarDetalles",
        data: {
            accion: accion,
            topcodi: topcodi,
            recurcodi: recurcodi
        },
        success: function (evt) {
            $('#vistaDetalle').html(evt);
            limpiarBarraMensaje('mensaje');
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            completarDatosRecurso(accion, topcodi, recurcodi);
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            //llenarCamposVistaDetalle(accion, evt);

            $('#txtFechaFutura').Zebra_DatePicker();

            $('#cboRecurso').change(function () {
                var recurcodiSeleccionado = $("#cboRecurso").val() || 0;
                completarDatosRecurso(AGREGAR_MODIF, topcodi, recurcodiSeleccionado);
            });

            //Guardar
            $("#btnGrabarModificacion").click(function () {
                guardarModificacionCentral(AGREGAR_MODIF);
            });

            $("#btnActualizarModificacion").click(function () {
                guardarModificacionCentral(EDITAR_MODIF);
            });

            $("#btnCancelarModifiacion").click(function () {
                $("#vistaDetalle").html(''); //limpiar tab Detalle

                $('#tab-container').easytabs('select', '#vistaListado');
                mostrarListadoModificacionCentral();
            });

        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error: ' + err);
        }
    });
}

function completarDatosRecurso(accion, topcodi, recurcodi) {

    if (recurcodi > 0) {

        $.ajax({
            url: controlador + "ObtenerRecurso",
            data: {
                topcodi: topcodi,
                recurcodi: recurcodi,
                accion: accion
            },
            type: 'POST',
            success: function (result) {
                if (result.Resultado === "-1") {
                    mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al obtener el recurso: ' + result.Mensaje);
                } else {
                    llenarCamposVistaDetalle(accion, result);
                }
            },
            error: function (xhr, status) {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function llenarCamposVistaDetalle(accion, result) {

    var modificacion = result.ModificacionCentral;

    if (accion == AGREGAR_MODIF) {
        $('#cboRecurso').val(modificacion.RecursoSddp.Mrecurcodi);
        $('#txtCodigoSddp').val(modificacion.RecursoSddp.NumeroSDDP);
        $('#txtDescripcionSddp').val(modificacion.RecursoSddp.Mrecurnomb);
        $('#txtFechaFutura').val(modificacion.FechaFuturaDesc);
        $('#txtPotencia').val(modificacion.RecursoSddp.Potencia);
        $('#txtCoefProduccion').val(modificacion.RecursoSddp.CoefProduccion);
        $('#txtCaudalMinTur').val(modificacion.RecursoSddp.CaudalMinTur);
        $('#txtCaudalMaxTur').val(modificacion.RecursoSddp.CaudalMaxTur);
        $('#txtDefluenciaTotMin').val(modificacion.RecursoSddp.DefluenciaTotMin);
        $('#txtICP').val(modificacion.RecursoSddp.ICP);
        $('#txtIH').val(modificacion.RecursoSddp.IH);
        $('#txtVolumenMax').val(modificacion.RecursoSddp.VolumenMax);

        $("#txtCodigoSddp").prop('disabled', 'disabled');
        $("#txtDescripcionSddp").prop('disabled', 'disabled');
    }

    if (accion == EDITAR_MODIF) {
        $('#cboRecurso').val(modificacion.RecursoSddp.Mrecurcodi);
        $('#txtCodigoSddp').val(modificacion.RecursoSddp.NumeroSDDP);
        $('#txtDescripcionSddp').val(modificacion.RecursoSddp.Mrecurnomb);
        $('#txtFechaFutura').val(modificacion.FechaFuturaDesc);
        $('#txtPotencia').val(modificacion.RecursoSddp.Potencia);
        $('#txtCoefProduccion').val(modificacion.RecursoSddp.CoefProduccion);
        $('#txtCaudalMinTur').val(modificacion.RecursoSddp.CaudalMinTur);
        $('#txtCaudalMaxTur').val(modificacion.RecursoSddp.CaudalMaxTur);
        $('#txtDefluenciaTotMin').val(modificacion.RecursoSddp.DefluenciaTotMin);
        $('#txtICP').val(modificacion.RecursoSddp.ICP);
        $('#txtIH').val(modificacion.RecursoSddp.IH);
        $('#txtVolumenMax').val(modificacion.RecursoSddp.VolumenMax);

        $("#hdtopcodi").val(modificacion.Mtopcodi);
        $("#cboRecurso").val(modificacion.RecursoSddp.Mrecurcodi);

        if (modificacion.RecursoSddp.IndicadorEA == 1) {
            $("#rdCHSiEA").prop('checked', true);
            $("#rdCHNoEA").prop('checked', false);
        }
        else {
            $("#rdCHSiEA").prop('checked', false);
            $("#rdCHNoEA").prop('checked', true);
        }

        $("#cboRecurso").prop('disabled', 'disabled');
        //$("#txtCodigoSddp").prop('disabled', 'disabled');
        //$("#txtDescripcionSddp").prop('disabled', 'disabled');
    }

    if (accion == DETALLES_MODIF) {

        $('#txtCodigoSddp').val(modificacion.RecursoSddp.NumeroSDDP);
        $('#txtDescripcionSddp').val(modificacion.RecursoSddp.Mrecurnomb);
        $('#txtFechaFutura').val(modificacion.FechaFuturaDesc);
        $('#txtPotencia').val(modificacion.RecursoSddp.Potencia);
        $('#txtCoefProduccion').val(modificacion.RecursoSddp.CoefProduccion);
        $('#txtCaudalMinTur').val(modificacion.RecursoSddp.CaudalMinTur);
        $('#txtCaudalMaxTur').val(modificacion.RecursoSddp.CaudalMaxTur);
        $('#txtDefluenciaTotMin').val(modificacion.RecursoSddp.DefluenciaTotMin);
        $('#txtICP').val(modificacion.RecursoSddp.ICP);
        $('#txtIH').val(modificacion.RecursoSddp.IH);
        $('#txtVolumenMax').val(modificacion.RecursoSddp.VolumenMax);

        $("#cboRecurso").val(modificacion.RecursoSddp.Mrecurcodi);
        if (modificacion.RecursoSddp.IndicadorEA == 1) {
            $("#rdCHSiEA").prop('checked', true);
            $("#rdCHNoEA").prop('checked', false);
        }
        else {
            $("#rdCHSiEA").prop('checked', false);
            $("#rdCHNoEA").prop('checked', true);
        }

        $("#cboRecurso").prop('disabled', 'disabled');
        document.getElementById('cboRecurso').style.backgroundColor = '';
        $("#txtCodigoSddp").prop('disabled', 'disabled');
        $("#txtDescripcionSddp").prop('disabled', 'disabled');
        $("#txtFechaFutura").prop('disabled', 'disabled');
        $(".Zebra_DatePicker_Icon")[0].classList.add('Zebra_DatePicker_Icon_Disabled');
        document.getElementById('txtFechaFutura').style.backgroundColor = '';

        $("#txtPotencia").prop('disabled', 'disabled');
        $("#txtCoefProduccion").prop('disabled', 'disabled');
        $("#txtCaudalMinTur").prop('disabled', 'disabled');
        $("#txtCaudalMaxTur").prop('disabled', 'disabled');
        $("#txtDefluenciaTotMin").prop('disabled', 'disabled');
        $("#txtICP").prop('disabled', 'disabled');
        $("#txtIH").prop('disabled', 'disabled');
        $("#txtVolumenMax").prop('disabled', 'disabled');

        $("#rdCHSiEA").prop('disabled', 'disabled');
        $("#rdCHNoEA").prop('disabled', 'disabled');
    }
}

function exportarInformacion(topcodi) {

    $.ajax({
        type: 'POST',
        url: controlador + "DescargarModificacionCentralSDDP",
        data: {
            topcodi: topcodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "Exportar?file_name=" + evt.Resultado;
            } else {
                mostrarMensaje_('mensaje', 'error', "Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
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

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

async function mostrarMensaje_(id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

    await sleep(6000);

    limpiarBarraMensaje(id);
}

function limpiarBarraMensaje(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
}

mostrarAlerta = function (id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}