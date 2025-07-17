var MODELO_LISTA_CENTRAL = [];
var MODELO_DOCUMENTO = [];

var MODELO_LISTA_CENTRAL_JSON = [];
var MODELO_DOCUMENTO_JSON = [];

var LISTA_OBJETO_HOJA = [];

var LISTA_ERRORES = [];
var LISTA_VERSIONES = [];
var LISTA_LOG = [];

var HEIGHT_MINIMO = 500;
var numArchivosS2 = 0;
var URL_IMG_POPUP_GAS = siteRoot + 'Content/Images/btn-properties.png';
var IMG_EDITAR = '<img src="' + siteRoot + 'Content/Images/btn-edit.png" alt="" width="19" height="19" style="">';

var CODIGO_SECCION_SUMINISTRO = 177;
var CODIGO_SECCION_TRANSPORTE = 182;
var CODIGO_SECCION_DISTRIBUCION = 187;

var ESTADO_ENVIO_FORMULARIO = ESTADO_SOLICITADO;

// Variables para almacenar la posición de las celdas seleccionadas
var TIPO_OPERACION_DECIMAL = 1;
var TIPO_OPERACION_NO_APLICA = 2;
var objPosSelecTmp = _inicializarObjetoPosicion();
var objPosSelecOrigen = _inicializarObjetoPosicion();

function mostrarGrilla() {
    ESTADO_ENVIO_FORMULARIO = parseInt($("#hdIdEstado").val()) || 0;
    $("#barra_herramienta_envio").hide();

    //eliminar handsontables
    for (var i = 0; i < MODELO_LISTA_CENTRAL.length; i++) {
        var objTabCentral = MODELO_LISTA_CENTRAL[i];
        if (LISTA_OBJETO_HOJA[objTabCentral.Equicodi].hot != null) {
            LISTA_OBJETO_HOJA[objTabCentral.Equicodi].hot.destroy();
        }
    }

    $('#div_formato3 #html_formulario').html('');

    var idEnvio = parseInt($("#hfIdEnvio").val()) || 0;
    var idVersion = parseInt($("#hfIdVersion").val()) || 0;
    var idEmpresa = $("#cbEmpresa").val();
    var tipoCentral = $("#hdTipoCentral").val();
    var tipoOpcion = $("#hdTipoOpcion").val();
    var tipoAccionForm = $("#hdTipoAccion").val();
    var mesVigencia = $("#cbMes").val();

    $.ajax({
        type: 'POST',
        url: controlador + "MostrarGrilla",
        dataType: 'json',
        data: {
            idEnvio: idEnvio,
            idVersion: idVersion,
            idEmpresa: idEmpresa,
            tipoCentral: tipoCentral,
            tipoOpcion: tipoOpcion,
            tipoAccionForm: tipoAccionForm,
            mesVigencia: mesVigencia
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                MODELO_LISTA_CENTRAL = evt.ListaFormularioCentral;
                MODELO_DOCUMENTO = evt.FormularioSustento;
                LISTA_VERSIONES = evt.ListadoVersiones;
                LISTA_LOG = evt.ListaLog;
                MODELO_LISTA_CENTRAL_JSON = MODELO_LISTA_CENTRAL;
                MODELO_DOCUMENTO_JSON = MODELO_DOCUMENTO;

                //crear Tab
                cargarHtmlTabCentral(MODELO_LISTA_CENTRAL);
                $('#tab-container-central').easytabs();

                //crear handson en cada Tab
                LISTA_OBJETO_HOJA = {};
                for (var i = 0; i < MODELO_LISTA_CENTRAL.length; i++) {
                    var objTabCentral = MODELO_LISTA_CENTRAL[i];

                    LISTA_OBJETO_HOJA[objTabCentral.Equicodi] = {
                        hot: null,
                        evtHot: null,
                        idTab: "central_" + objTabCentral.Equicodi,
                        posTab: i
                    };
                    cargarHandsontableXTab(objTabCentral, i, objTabCentral.Equicodi);
                }

                //correcion bug, tab no visible
                $('#tab-container-central').bind('easytabs:midTransition', async function () {
                    var id = document.activeElement.id;
                    await sleep(500); //espera 2 seg hasta que termine de cargar handson
                    document.querySelector('#' + id).click();
                });

                //tab archivos
                cargarHtmlDocumentoXEnvioTabSustento(MODELO_DOCUMENTO);

                //mostrar botones del administrador
                visibilidadBotones();

                //barra de herramienta
                if (evt.TienePermisoEditar) {
                    if (evt.IdEnvio <= 0)
                        $("#mensajeEvento").html('Debe ingresar la información, favor de completar los datos. <b>Plazo del envío</b>: Permitido hasta las 23:59:59 del día 20 de cada mes.');

                    $("#barra_herramienta_envio").show();
                    $("#btnEnviarDatos").parent().css("display", "table-cell");
                    $("#btnMostrarErrores").parent().css("display", "table-cell");
                    $("#btnLeyenda").parent().css("display", "table-cell");
                    $("#btnExpandirRestaurar").parent().css("display", "table-cell");

                    $("#barra_herramienta_formato3").show();
                    $("#btnHistorialAutoguardado").parent().css("display", "table-cell");
                    $("#btnAutoguardar").parent().css("display", "table-cell");
                    $("#btnDescargar").parent().css("display", "table-cell");
                    $("#btnImportar").parent().css("display", "table-cell");
                    $("#btnMasDecimales").parent().css("display", "table-cell");
                    $("#btnMenosDecimales").parent().css("display", "table-cell");
                    $("#btnNoAplica").parent().css("display", "table-cell");
                    $("#btnLimpiar").parent().css("display", "table-cell");

                } else {
                    if (evt.IdEnvio <= 0) {
                        if (evt.FlagExisteEnvio) {
                            $("#mensajeEvento").show();
                            $("#mensajeEvento").html('<b>Ya existe una solicitud para el periodo seleccionado. No está permitido realizar una nueva solicitud.');
                        } else {
                            $("#mensajeEvento").show();
                            $("#mensajeEvento").html('<b>Plazo del envío</b>: No permitido.');
                        }
                    }
                    else {
                        $("#barra_herramienta_envio").show();
                        $("#btnVerEnvio").parent().css("display", "table-cell");

                        $("#barra_herramienta_formato3").show();
                        $("#btnDescargar").parent().css("display", "table-cell");
                    }
                }

                //etiquetas
                if (evt.IdEnvio <= 0) {

                } else {
                    $("#container .content-titulo").html(`Envío de Costo de Combustibles - Central ${evt.Envio.CbenvtipocentralDesc} - Solicitud N° ${evt.IdEnvio}`);
                }

                if (ESTADO_ENVIO_FORMULARIO == ESTADO_OBSERVADO) {
                    $("#container .content-titulo").append(" <span style='font-weight: bold; color: #ffd400;'>(OBSERVADO)<span>");

                    var msj = '';
                    if (evt.Envio.CbenvfecfinsubsanarobsDesc != null && evt.Envio.CbenvfecfinsubsanarobsDesc != '') {
                        msj = `<b>El COES ha observado la solicitud</b>. Tiene plazo hasta <b>${evt.Envio.CbenvfecfinsubsanarobsDesc}</b> para subsanar la solicitud. `;
                        if (evt.EsExtranet) msj += "Se permite la edición del formulario";
                    }

                    if (evt.Envio.CbenvfecamplDesc != null && evt.Envio.CbenvfecamplDesc != '') {
                        msj = `El COES ha ampliado el plazo hasta <b>${evt.Envio.CbenvfecamplDesc}</b> para la subsanación de observaciones. `;
                        if (evt.EsExtranet) msj += "Se permite la edición del formulario";
                    }

                    if (msj != '') {
                        $("#div_dia_habil").html(msj);
                        $("#div_dia_habil").show();
                    }
                }

                if (ESTADO_ENVIO_FORMULARIO == ESTADO_SUBSANADO) {
                    $("#container .content-titulo").append(" <span style='font-weight: bold; color: #ffd400;'>(SUBSANACIÓN DE OBSERVACIONES)<span>");
                }

                if (ESTADO_ENVIO_FORMULARIO == ESTADO_CANCELADO) {
                    $("#container .content-titulo").append(" <span style='font-weight: bold; color: #9a9a9a;'>(CANCELADO)<span>");

                    $("#div_dia_habil").html(`Motivo de Cancelación: <br/> <b style="">"${evt.Envio.Cbenvobs}"</b>`);
                    $("#div_dia_habil").show();
                }

                if (ESTADO_ENVIO_FORMULARIO == ESTADO_APROBADO) {
                    $("#container .content-titulo").append(" <span style='font-weight: bold; color: #37bd3c'>(APROBADO)<span>");
                }

                if (ESTADO_ENVIO_FORMULARIO == ESTADO_DESAPROBADO) {
                    $("#container .content-titulo").append(" <span style='font-weight: bold; color: #FF7979'>(DESAPROBADO)<span>");
                }

                if (evt.Envio.Cbenvusucarga != null && evt.Envio.Cbenvusucarga != "") {
                    var tipoCarga = '';
                    if (evt.Envio.Cbenvtipocarga == "IPC") tipoCarga = "Primera Carga";
                    if (evt.Envio.Cbenvtipocarga == "NI") tipoCarga = "Nuevo envío";
                    if (evt.Envio.Cbenvtipocarga == "SI") tipoCarga = "Subsanar envío";

                    $("#div_creado_intranet").html(`Tipo de carga: <span style='font-weight: bold; color: blue;'>${tipoCarga}</span>. Registrado por el usuario COES ${evt.Envio.Cbenvusucarga} con fecha ${evt.Envio.CbenvfecsolicitudDesc}.`);
                    $("#div_creado_intranet").show();
                }
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Error al cargar Excel Web");
        }
    });
}

function validarEnvio() {
    LISTA_ERRORES = [];

    //errores de cada tab
    for (var i = 0; i < MODELO_LISTA_CENTRAL.length; i++) {
        var objTabCentral = MODELO_LISTA_CENTRAL[i];

        //agregar errores propios de celda
        LISTA_ERRORES = LISTA_ERRORES.concat(objTabCentral.ListaErrores);
    }

    //errores de archivo
    var listaArchivo = MODELO_DOCUMENTO.SeccionCombustible.ListaArchivo;
    if (listaArchivo.length == 0) {
        LISTA_ERRORES.push({
            Tab: "Informe sustentatorio",
            Celda: '',
            Valor: '',
            Mensaje: 'Debe subir archivos sustentatorios.'
        });
    }

    if (LISTA_ERRORES.length > 0) {
        return false;
    }
    else {
        return true;
    }
}

function actualizarDimensionExcelWeb(equicodi, cbftitcodi1, cbftitcodi0, tipoInfo, numCol, numColDesp, mesAnterior) {
    actualizarModeloMemoria();

    var idEnvio = $("#hfIdEnvio").val();
    var tipoAccionForm = $("#hdTipoAccion").val();

    var formulario = {
        IdEnvio: idEnvio,
        ListaFormularioCentral: MODELO_LISTA_CENTRAL_JSON,
        Equicodi: equicodi,
        CnpSeccion1: cbftitcodi1,
        CnpSeccion0: cbftitcodi0,
        NumCol: numCol,
        NumColDesp: numColDesp,
        TipoOpcionSeccion: tipoInfo,
        TipoAccionForm: tipoAccionForm,
        MesAnteriorCentralNueva: mesAnterior
    };

    _actualizarGrillaExcelWeb(formulario);
}

function _actualizarGrillaExcelWeb(formulario) {

    var dataJson = {
        data: JSON.stringify(formulario)
    };

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: controlador + "ActualizarGrilla",
        contentType: 'application/json; charset=UTF-8',
        data: JSON.stringify(dataJson),
        beforeSend: function () {
            //mostrarExito("Enviando Información ..");
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                MODELO_LISTA_CENTRAL = evt.ListaFormularioCentral;
                MODELO_LISTA_CENTRAL_JSON = evt.ListaFormularioCentral;

                //volver a renderizar handsontable
                if (formulario.CnpSeccion1 > 0) {

                    //eliminar handsontables
                    for (var i = 0; i < MODELO_LISTA_CENTRAL.length; i++) {
                        var objTabCentral = MODELO_LISTA_CENTRAL[i];
                        if (LISTA_OBJETO_HOJA[objTabCentral.Equicodi].hot != null) {
                            LISTA_OBJETO_HOJA[objTabCentral.Equicodi].hot.destroy();
                        }
                    }

                    for (var i = 0; i < MODELO_LISTA_CENTRAL.length; i++) {
                        var objTabCentral = MODELO_LISTA_CENTRAL[i];

                        cargarHandsontableXTab(objTabCentral, i, objTabCentral.Equicodi);
                    }
                } else {
                    //solo actualizar las celdas
                    for (var c = 0; c < MODELO_LISTA_CENTRAL.length; c++) {
                        var objTabCentral = MODELO_LISTA_CENTRAL[c];

                        var matriz = objTabCentral.Handson.ListaExcelData;
                        var filaIni = objTabCentral.FilaIni;
                        var numMaxColData = objTabCentral.NumMaxColData;

                        var colNumSeccion = 0;
                        var colNumItem = colNumSeccion + 1;
                        var colProp = colNumItem + 1;
                        var colUnidad = colProp + 1;
                        var colValorIni = colUnidad + 1;
                        var colValorFin = colValorIni + numMaxColData - 1;
                        var colInstruc = colValorFin + 1;
                        var colConf = colInstruc + 1;

                        //setear booleanos a columna confidencial
                        var columnaCheckConf = [matriz.length];
                        if (matriz.length > 0) {
                            for (var i = filaIni + 1; i < matriz.length; i++) {
                                columnaCheckConf[i] = matriz[i][colConf] != ""; //la celda tiene check o no

                                if (matriz[i][colConf] != "") {
                                    matriz[i][colConf] = (matriz[i][colConf] == "1"); //1: check, 2: no check
                                }
                            }
                        }

                        LISTA_OBJETO_HOJA[objTabCentral.Equicodi].hot.updateSettings({ data: matriz });
                    }
                }

                $('#popupFormularioSeccion').bPopup().close();
            } else {
                //alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error.");
        }
    });
}

function actualizarModeloMemoria() {
    for (var i = 0; i < MODELO_LISTA_CENTRAL.length; i++) {
        var equicodi = MODELO_LISTA_CENTRAL[i].Equicodi;
        MODELO_LISTA_CENTRAL[i].Handson.ListaExcelData = LISTA_OBJETO_HOJA[equicodi].hot.getData();

        var matriz = MODELO_LISTA_CENTRAL[i].Handson.ListaExcelData;
        var filaIni = MODELO_LISTA_CENTRAL[i].FilaIni;
        var numMaxColData = MODELO_LISTA_CENTRAL[i].NumMaxColData;

        //LISTA_OBJETO_HOJA[ MODELO_LISTA_CENTRAL[i].Equicodi].hot.updateSettings({ data: matriz });

        var listaArrayItem = MODELO_LISTA_CENTRAL[i].ArrayItem;
        for (var j = 0; j < listaArrayItem.length; j++) {
            var objItem = listaArrayItem[j];
            objItem.Cbftitfeccreacion = null;
            objItem.Cbftitfecmodificacion = null;
            //objItem.Cbftitinstructivo = '';
            //objItem.Cbftitnombre = '';
            objItem.Ccombnombre = '';

            if (objItem.ListaItemXSeccion != null) {
                for (var m = 0; m < objItem.ListaItemXSeccion.length; m++) {
                    var objItem2 = objItem.ListaItemXSeccion[m];
                    objItem2.Cbftitfeccreacion = null;
                    objItem2.Cbftitfecmodificacion = null;
                }
            }
        }

        var listaArrayItemObs = MODELO_LISTA_CENTRAL_JSON[i].ArrayItemObs;
        if (listaArrayItemObs != null) {
            for (var j = 0; j < listaArrayItemObs.length; j++) {
                var objItem = listaArrayItem[j];
                objItem.Cbftitfeccreacion = null;
                objItem.Cbftitfecmodificacion = null;
                //objItem.Cbftitinstructivo = '';
                //objItem.Cbftitnombre = '';
                objItem.Ccombnombre = '';

                if (objItem.ListaItemXSeccion != null) {
                    for (var m = 0; m < objItem.ListaItemXSeccion.length; m++) {
                        var objItem2 = objItem.ListaItemXSeccion[m];
                        objItem2.Cbftitfeccreacion = null;
                        objItem2.Cbftitfecmodificacion = null;
                    }
                }
            }
        }
    }

    MODELO_LISTA_CENTRAL_JSON = _clonarModeloListaCentral(MODELO_LISTA_CENTRAL);
}

function _clonarModeloListaCentral(modeloCentral) {
    //clonar objeto y quitar html
    var modeloCloneCentral = JSON.parse(JSON.stringify(modeloCentral));
    for (var i = 0; i < modeloCloneCentral.length; i++) {
        var equicodi = modeloCloneCentral[i].Equicodi;

        var matriz = modeloCloneCentral[i].Handson.ListaExcelData;
        var filaIni = modeloCloneCentral[i].FilaIni;
        var numMaxColData = modeloCloneCentral[i].NumMaxColData;

        var colNumSeccion = 0;
        var colNumItem = colNumSeccion + 1;
        var colProp = colNumItem + 1;
        var colUnidad = colProp + 1;
        var colValorIni = colUnidad + 1;
        var colValorFin = colValorIni + numMaxColData - 1;
        var colInstruc = colValorFin + 1;
        var colConf = colInstruc + 1;
        var colObsCOES = colConf + 1;
        var colSubsGen = colObsCOES + 1;
        var colRptaCOES = colSubsGen + 1;
        var colEstado = colRptaCOES + 1;

        //
        var arrayItem = modeloCloneCentral[i].ArrayItem;
        var arrayItemObs = modeloCloneCentral[i].ArrayItemObs;

        //quitar el html
        if (matriz.length > 0) {
            for (var c = 0; c < arrayItem.length; c++) {
                var objItem = arrayItem[c];
                if (objItem.EsSeccion) {
                    for (var col = 0; col < matriz[0].length; col++) {
                        //matriz[n][colProp] = "";
                        //matriz[n][colInstruc] = "";
                        if (col >= colObsCOES && col <= colRptaCOES) {
                            matriz[objItem.PosRow + 1][col] = "";
                        }
                    }
                }
            }
        }

    }

    return modeloCloneCentral;
}

function consultarMedidor(mesConsulta, idEmpresa, equicodi) {
    $("#div_ENO").hide();

    var valorEnergia = consultaMedidorXTipocentral(mesConsulta, idEmpresa, equicodi);
    if (valorEnergia > 0) {
        $("#span_mwh").html(valorEnergia);
        $("#div_ENO").show();
    }
}

function consultaMedidorXTipocentral(mesConsulta, idEmpresa, equicodi) {
    var tipoCentral = $("#hdTipoCentral").val();

    var valor = 0;
    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerEnergiaXCentralYPeriodo",
        dataType: 'json',
        async: false,
        data: {
            mesConsulta: mesConsulta,
            idEmpresa: idEmpresa,
            equicodi: equicodi,
            tipoCentral: tipoCentral
        },
        success: function (evt) {
            valor = evt.ValorEnergia;
        },
        error: function (err) {
            valor = 0;
        }
    });

    return valor;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Handson
////////////////////////////////////////////////////////////////////////////////////////////////////////////
function cargarHtmlTabCentral(listaTab) {
    var htmlTabNombre = '';
    var htmlTabDiv = '';

    for (var i = 0; i < listaTab.length; i++) {
        var objCentral = listaTab[i];
        htmlTabNombre += `  <li class='tab'><a id="tab_central_${objCentral.Equicodi}" href="#central_${objCentral.Equicodi}">${objCentral.Central}</a></li>  `;
        htmlTabDiv += `           

            <div id="central_${objCentral.Equicodi}" style="background-color:white">
                <div style="height: 30px;" id="estadoCentralTermica_${objCentral.Equicodi}"></div>
                <div class="content-tabla" style="display:block;">
                    <div class="bodyexcel" id='detalleFormato_${objCentral.Equicodi}'></div>
                </div>

            </div>

        `;
    }

    var htmlTab = `

    <div id="tab-container-central" class='tab-container' style="display: block;padding-top: 15px;">
        <ul class='etabs'>
            ${htmlTabNombre}
        </ul>
        <div class="panel-container">            
            ${htmlTabDiv}
        </div>
    </div>
    `;

    $("#html_formulario").html(htmlTab);
}

function cargarHandsontableXTab(objTabCentral, posTab, equicodi) {

    //variable segun Número de hoja
    var idHandsontable = 'detalleFormato_' + objTabCentral.Equicodi;

    var matriz = objTabCentral.Handson.ListaExcelData;
    var matrizTipoEstado = objTabCentral.Handson.MatrizTipoEstado;
    var listaItem = objTabCentral.ArrayItem;
    var listaItemObs = objTabCentral.ArrayItemObs;
    var filaIni = objTabCentral.FilaIni;
    var numMaxColData = objTabCentral.NumMaxColData;
    var soloLectura = !objTabCentral.EsEditable;
    var esEditable = objTabCentral.EsEditable;
    var esEditableObs = objTabCentral.EsEditableObs;

    var colNumSeccion = 0;
    var colNumItem = colNumSeccion + 1;
    var colProp = colNumItem + 1;
    var colUnidad = colProp + 1;
    var colValorIni = colUnidad + 1;
    var colValorFin = colValorIni + numMaxColData - 1;
    var colInstruc = colValorFin + 1;
    var colConf = colInstruc + 1;
    var colObsCOES = colConf + 1;
    var colSubsGen = colObsCOES + 1;
    var colRptaCOES = colSubsGen + 1;
    var colEstado = colRptaCOES + 1;

    function rendererFilaSeccion(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.color = 'White';
        td.style.textAlign = 'center';
        td.style.background = '#0D6AB7';
    }

    function rendererFilaPropiedadColNumeral(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.color = '#3075B5';
        td.style.textAlign = 'center';
        td.style.background = '#F2F2F2';
    }

    function rendererFilaPropiedadColNombre(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.color = '#45587D';
        td.style.textAlign = 'left';
        td.style.background = '#F2F2F2';
    }

    function rendererFilaPropiedadColUnidad(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.color = '#45587D';
        td.style.textAlign = 'center';
        td.style.background = '#F2F2F2';
    }

    function rendererFilaPropiedadColValor(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.color = '#696969';
        td.style.textAlign = 'center';
        td.style.background = !cellProperties.readOnly ? _getColorCelda(posTab, row, col) : '#FFE4C4';
    }

    function rendererFilaPropiedadColValorFormula(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.color = '#000000';
        td.style.textAlign = 'center';
        td.style.background = '#A6A6A6';
    }

    function rendererFilaPropiedadColValorNoEditable(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.color = '#000000';
        td.style.textAlign = 'center';
        td.style.background = '#9BC2E6'; //'#EDEDED';
    }

    function rendererFilaPropiedadColInstructivo(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.color = '#777';
        td.style.textAlign = 'left';
        td.style.background = '#F2F2F2';
        td.style.fontSize = '10px';
    }

    function rendererConfidencialCheck(instance, td, row, col, prop, value, cellProperties) {
        td.style.textAlign = 'center';
        td.style.background = '#F2F2F2';
        td.style.height = '5px';
        td.style.lineHeight = '0px';
        td.style.verticalAlign = 'middle';
        td.innerHTML = _getHtmlCheckConfidencial(posTab, row, col, value);
    }

    function rendererConfidencialSinCheck(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.textAlign = 'center';
        td.style.background = '#F2F2F2';
    }

    function rendererConfidencialCheckGrupal(instance, td, row, col, prop, value, cellProperties) {
        td.style.textAlign = 'center';
        td.style.background = '#0D6AB7';
        td.style.height = '5px';
        td.style.lineHeight = '0px';
        td.style.verticalAlign = 'middle';
        td.innerHTML = _getHtmlCheckConfidencial(posTab, row, col, value);
    }

    function rendererFilaPropiedadColValorHtml(instance, td, row, col, prop, value, cellProperties) {
        var esEditableCelda = _getCeldaEsEditableObservacionF3(posTab, row, col);
        td.style.background = esEditableCelda ? '#FFFFFF' : '#FFE4C4';
        td.innerHTML = _getHtmlObservacion(posTab, row, col, esEditableCelda);
    }

    function rendererFilaCeldaReadonly(instance, td, row, col, prop, value, cellProperties) {
        td.style.background = !cellProperties.readOnly ? '#FFFFFF' : '#FFE4C4';
    }

    function rendererPopupGas(instance, td, row, col, prop, value, cellProperties) {
        $(td).html(''); //$(td).children('.btn').remove();

        var elementoImg = document.createElement('img');
        elementoImg.className = 'btn';
        elementoImg.src = URL_IMG_POPUP_GAS;

        td.appendChild(elementoImg);
        $(td).unbind();
        $(td).on('click', function () {
            mostrarPopupGas(posTab, row);
        });
        $(td).addClass("estiloPopupGas");
        return td;
    }

    function calculateSize() {
        var offset = Handsontable.Dom.offset(container);

        //Verificar en que ventana esta, expandida -> 2 o extranet normal -> 1
        if (offset.top == 222) {
            availableHeight = $(window).height() - 140 - offset.top - 20;
        }
        else {
            availableHeight = $(window).height() - 140 - offset.top - 20;
        }

        if (offset.left > 0)
            availableWidth = $(window).width() - 2 * offset.left; //$("#divGeneral").width() - 50; //
        if (offset.top > 0) {
            availableHeight = availableHeight < HEIGHT_MINIMO ? HEIGHT_MINIMO : availableHeight;
            container.style.height = availableHeight + 'px';
        }
        if (LISTA_OBJETO_HOJA[equicodi] != undefined && getVariableHot(equicodi) != undefined) {
            getVariableHot(equicodi).render();
        }
    }

    //setear booleanos a columna confidencial
    var columnaCheckConf = [matriz.length];
    if (matriz.length > 0) {
        for (var i = filaIni + 1; i < matriz.length; i++) {
            columnaCheckConf[i] = matriz[i][colConf] != ""; //la celda tiene check o no

            if (matriz[i][colConf] != "") {
                matriz[i][colConf] = (matriz[i][colConf] == "1"); //1: check, 2: no check
            }
        }
    }

    //
    var listaOpcionEstado = [];
    if (ESTADO_ENVIO_FORMULARIO == ESTADO_SOLICITADO) {
        listaOpcionEstado.push("Observado");
        listaOpcionEstado.push("Conforme");
    }
    if (ESTADO_ENVIO_FORMULARIO == ESTADO_OBSERVADO) {
        listaOpcionEstado.push("Observado");
        listaOpcionEstado.push("Subsanado");
    }
    if (ESTADO_ENVIO_FORMULARIO == ESTADO_SUBSANADO) {
        listaOpcionEstado.push("No Subsanado");
        listaOpcionEstado.push("Conforme");
    }

    //definicion
    var container = document.getElementById(idHandsontable);
    //Handsontable.Dom.addEvent(window, 'resize', calculateSize());
    hotOptions = {
        data: matriz,
        mergeCells: objTabCentral.Handson.ListaMerge,
        maxCols: matriz[0].length,
        maxRows: matriz.length,//52,
        height: HEIGHT_MINIMO,
        colHeaders: false,
        rowHeaders: false,
        fillHandle: true,
        columnSorting: false,
        colWidths: objTabCentral.Handson.ListaColWidth,
        readOnly: soloLectura,
        cells: function (row, col, prop) {
            if (row >= 0 && col >= 0) {
                this.readOnly = false;

                //celda no editables
                if (col < colConf) {
                    if (esEditable)
                        this.readOnly = matrizTipoEstado[row][col] <= 0; //-1: no editable, 0: solo lectura, -2: readonly
                    else
                        this.readOnly = true;
                } else {
                    if (col >= colObsCOES && col <= colRptaCOES)
                        this.readOnly = true;
                    if (col == colEstado)
                        this.readOnly = matrizTipoEstado[row][col] <= 0;
                }

                if (col == colConf)
                    this.readOnly = true;

                //renderizar "confidencial", observacion
                if (row == filaIni) {
                    if (col == colConf || col == colObsCOES)
                        this.renderer = rendererFilaSeccion; //color azul
                }

                //renderizar filas de datos
                if (row > filaIni) {
                    var objItem = listaItem[row - filaIni - 1];
                    if (objItem.EsSeccion) {
                        if (col == 1 && objItem.Cbftitcnp1 > 0) {//solo mostrar boton popup cuando sea editable
                            this.renderer = rendererPopupGas;
                        }
                        else
                            this.renderer = rendererFilaSeccion;

                        if (col == colConf) {
                            if (columnaCheckConf[row]) {
                                this.renderer = rendererConfidencialCheckGrupal;
                            }
                            else
                                this.renderer = rendererFilaSeccion;
                        }
                    }
                    else {
                        if (col == 0 || col == 1)
                            this.renderer = rendererFilaPropiedadColNumeral;

                        if (col == 2)
                            this.renderer = rendererFilaPropiedadColNombre;

                        if (col == 3)
                            this.renderer = rendererFilaPropiedadColUnidad;

                        //valor
                        if (col >= colValorIni && col <= colValorFin) {
                            if (matrizTipoEstado[row][col] == -1) //nunca editable
                                this.renderer = rendererFilaPropiedadColValorNoEditable;
                            else {
                                if (matrizTipoEstado[row][col] == 0) //dato fórmula
                                    this.renderer = rendererFilaPropiedadColValorFormula;
                                else {
                                    this.renderer = rendererFilaPropiedadColValor;
                                }
                            }
                        }

                        if (col == colInstruc)
                            this.renderer = rendererFilaPropiedadColInstructivo;

                        if (col == colConf) {
                            if (columnaCheckConf[row]) {
                                this.renderer = rendererConfidencialCheck;
                            }
                            else
                                this.renderer = rendererConfidencialSinCheck;
                        }

                        if (col >= colObsCOES && col <= colEstado) {
                            if (matrizTipoEstado[row][col] == -1) //nunca editable
                                this.renderer = rendererFilaPropiedadColValorNoEditable;
                            else {
                                if (col == colObsCOES)
                                    this.renderer = rendererFilaPropiedadColValorHtml;

                                if (col == colSubsGen)
                                    this.renderer = rendererFilaPropiedadColValorHtml;

                                if (col == colRptaCOES)
                                    this.renderer = rendererFilaPropiedadColValorHtml;

                                if (col == colEstado) {
                                    var objItemObs = _getItemObsEstadoByFila(listaItemObs, row);
                                    if (objItemObs != null) {
                                        this.type = 'dropdown';
                                        this.source = listaOpcionEstado;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        },
        //afterSelection: function (actualFila, actualColumna, finalFila, finalColumna) {
        afterChange: (changes) => {
            if (changes != null && changes.length > 0) {
                var columna = changes[0][1];
                var fila = changes[0][0];
                if (columna >= colValorIni && columna <= colValorFin)
                    actualizarDimensionExcelWeb(equicodi);

                //solo cuando cambia la columna estados
                if (columna == colEstado) {
                    var estadoEnvioEnPantalla = parseInt($("#hdIdEstado").val()) || 0;

                    //solo para envios subsanados
                    if (estadoEnvioEnPantalla == ESTADO_SUBSANADO) {
                        pintarEstadosPorCentral();
                    }
                }

                //solo cuando cambia la columna confidencial
                if (columna == colConf) {
                    valorCheckGrupal = changes[0][3];
                    _setCheckedGrupal(posTab, fila, colConf, valorCheckGrupal);
                }
            }
        },
        afterSelection: function (actualFila, actualColumna, finalFila, finalColumna) {

            //obtener filas y columnas consistentes
            var vFila = actualFila > finalFila ? finalFila : actualFila;
            var vFilaFinal = finalFila > actualFila ? finalFila : actualFila;
            var vColumna = actualColumna > finalColumna ? finalColumna : actualColumna;
            var vColumnaFinal = finalColumna > actualColumna ? finalColumna : actualColumna;

            //generar array temporal
            objPosSelecTmp = {};
            objPosSelecTmp.rowIni = vFila;
            objPosSelecTmp.colIni = vColumna;
            objPosSelecTmp.rowFin = vFilaFinal;
            objPosSelecTmp.colFin = vColumnaFinal;
            objPosSelecTmp.equicodi = equicodi;

        },
        afterInit: async function () {
            var estadoEnvioEnPantalla = parseInt($("#hdIdEstado").val()) || 0;
            //solo para envios subsanados
            if (estadoEnvioEnPantalla == ESTADO_APROBADO_PARCIAL) {
                await sleep(2000); //espera 2 seg hasta que termine de cargar handson
                pintarEstadosPorCentral();
            }
        },
        afterRenderer: function (td, row, column, prop, value, cellProperties) {
            if (column == colEstado) {
                td.style.border = '0px solid'; //ocultamos bordes en la columna estado
            }
        },
        afterLoadData: function () {
            this.render();
        },
    };
    setVariableHot(new Handsontable(container, hotOptions), equicodi);
    calculateSize(1);
}

function updateDimensionHandson(equicodi) {
    var idHandsontable = 'detalleFormato_' + equicodi;

    var container = document.getElementById(idHandsontable)
    var hot = getVariableHot(equicodi);
    var offset = {};
    try {
        offset = Handsontable.Dom.offset(container);
    }
    catch (err) {
        console.log(err);
    }

    if (offset.length != 0) {
        var widthHT;
        var heightHT;

        if (offset.top == 222) {
            heightHT = $(window).height() - 140 - offset.top - 20;
        }
        else {
            heightHT = $(window).height() - 140 - offset.top - 20;
        }

        heightHT = heightHT < HEIGHT_MINIMO ? HEIGHT_MINIMO : heightHT;
        if (offset.left > 0 && offset.top > 0) {
            widthHT = $(window).width() - 2 * offset.left; // $("#divGeneral").width() - 50; //
            hot.updateSettings({
                width: widthHT,
                height: heightHT
            });
        }
    }
}

//hot
function setVariableHot(hotVar, equicodi) {
    LISTA_OBJETO_HOJA[equicodi].hot = hotVar;
}
function getVariableHot(equicodi) {
    return LISTA_OBJETO_HOJA[equicodi].hot;
}

function _setCheckedGrupal(posTab, fila, colConf, valorCheckGrupal) {
    var arrayCambios = [];

    var objTabCentral = MODELO_LISTA_CENTRAL[posTab];
    var listaItem = objTabCentral.ArrayItem;

    for (var i = 0; i < listaItem.length; i++) {
        if (listaItem[i].EsSeccion && listaItem[i].PosRow == fila && listaItem[i].TieneCheckConf) {//seccion seleccionada
            var listaItemXSeccion = listaItem[i].ListaItemXSeccion;
            for (var j = 0; j < listaItemXSeccion.length; j++) { //items que pueden tener check
                var objItem = listaItemXSeccion[j];
                if (objItem.TieneCheckConf) {
                    var arrayCeldaCambio = [];
                    arrayCeldaCambio.push(objItem.PosRow); //row
                    arrayCeldaCambio.push(colConf); //col
                    arrayCeldaCambio.push(valorCheckGrupal); //value

                    //agregar array
                    arrayCambios.push(arrayCeldaCambio);
                }
            }
        }
    }

    //Set new value to a cell. To change many cells at once (recommended way), pass an array of changes in format [[row, col, value],...] as the first argument.
    LISTA_OBJETO_HOJA[objTabCentral.Equicodi].hot.setDataAtCell(arrayCambios);
}

function _getColorCelda(posTab, row, col) {
    var objTabCentral = MODELO_LISTA_CENTRAL[posTab];
    for (var i = 0; i < objTabCentral.ListaErrores.length; i++) {
        var objError = objTabCentral.ListaErrores[i];
        if (objError.Row == row && objError.Col == col) {
            var colorError = objError.ColorError;
            switch (colorError) {
                case 1:
                    return "#FEC3E1";
                    break;
            }
        }
    }

    return '#FFFFFF';
}

function _getHtmlCheckConfidencial(posTab, row, col, valor) {
    var objTabCentral2 = MODELO_LISTA_CENTRAL[posTab];
    var filaIni2 = objTabCentral2.FilaIni;
    var listaArrayItem2 = objTabCentral2.ArrayItem;
    var objItem2 = listaArrayItem2[row - filaIni2 - 1];
    var esEditable = objItem2.TieneCheckConfEditable;

    if (row == 11 && col == 6) {
        console.log("111");
    }

    var htmlDiv = '';
    if (objItem2.TieneCheckConf) {
        var htmlChecked = "";
        if (valor) htmlChecked = " checked ";

        var htmlDisabled = "";
        if (!objTabCentral2.EsEditable || !esEditable) htmlDisabled = " disabled ";

        var htmlBtn = `
            <input type="checkbox" id='check_conf_postab_${posTab}_row_${row}_col_${col}' ${htmlChecked} ${htmlDisabled} style='margin: 0px;' onclick="_setearValorConfidencial(${posTab}, ${row}, ${col})">
        `;

        htmlDiv += `
                ${htmlBtn}
        `;
    }

    return htmlDiv;
}

function _setearValorConfidencial(posTab, row, col) {
    var objTabCentral = MODELO_LISTA_CENTRAL[posTab];

    var valorOrigen = LISTA_OBJETO_HOJA[objTabCentral.Equicodi].hot.getDataAtCell(row, col);

    var arrayCambios = [];
    var arrayCeldaCambio = [];
    arrayCeldaCambio.push(row); //row
    arrayCeldaCambio.push(col); //col
    arrayCeldaCambio.push(!valorOrigen); //value

    arrayCambios.push(arrayCeldaCambio);

    LISTA_OBJETO_HOJA[objTabCentral.Equicodi].hot.setDataAtCell(arrayCambios);
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Asignacion
////////////////////////////////////////////////////////////////////////////////////////////////////////////
function mostrarGrillaNuevaAsignacion() {

    $("#barra_herramienta_envio").show();
    $("#btnEnviarDatos").parent().css("display", "table-cell");

    $('#div_formato3 #html_formulario').html('');

    var idEnvio = parseInt($("#hfIdEnvio").val()) || 0;
    var idVersion = parseInt($("#hfIdVersion").val()) || 0;
    var idEmpresa = $("#cbEmpresa").val();
    var tipoCentral = $("#hdTipoCentral").val();
    var tipoOpcion = $("#hdTipoOpcion").val();
    var mesVigencia = $("#cbMes").val();

    $.ajax({
        type: 'POST',
        url: controlador + "MostrarGrilla",
        dataType: 'json',
        data: {
            idEnvio: idEnvio,
            idVersion: idVersion,
            idEmpresa: idEmpresa,
            tipoCentral: tipoCentral,
            tipoOpcion: tipoOpcion,
            mesVigencia: mesVigencia
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                MODELO_LISTA_CENTRAL = evt.ListaFormularioCentral;

                //crear Tab
                cargarHtmlTabCentral(MODELO_LISTA_CENTRAL);
                $('#tab-container-central').easytabs();

                //crear handson en cada Tab
                LISTA_OBJETO_HOJA = {};
                for (var i = 0; i < MODELO_LISTA_CENTRAL.length; i++) {
                    var objTabCentral = MODELO_LISTA_CENTRAL[i];
                    LISTA_OBJETO_HOJA[objTabCentral.Equicodi] = {
                        idTab: "central_" + objTabCentral.Equicodi,
                    };
                    cargarHtmlFormularioXEnvioNuevaSolicitud(objTabCentral, objTabCentral.Equicodi);
                }
                $('#btnVerCostos').click(function () {
                    var url = siteRoot + 'Migraciones/Parametro/Index';
                    window.open(url, '_blank').focus();
                });

                //mostrar botones del administrador
                visibilidadBotones();

            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Error al cargar Excel Web");
        }
    });
}

function cargarHtmlFormularioXEnvioNuevaSolicitud(objTabCentral) {

    //variable segun Número de hoja
    var idHandsontable = 'detalleFormato_' + objTabCentral.Equicodi;

    var costo = objTabCentral.Handson.ListaExcelData[0][0];

    var htmlDiv = '';
    htmlDiv += `
        <div style='font-size: 18px; color: #3D90CB; margin-top: 11px; margin: 0 auto; width: 100px;font-weight: bold;'>
            Asignación
        </div>

        <div style='margin-top: 30px; margin-bottom: 50px;'>
            <div style='margin: 0 auto;font-size: 15px; width: 384px;font-weight: bold;'>
                <span>Costo de Combustible Gaseoso (*): </span>
    `;
    if (true) { //solo en intranet muestro con caja de texto
        htmlDiv += `
                <input type="text" style="width:60px;font-weight: bold;color: blue;font-size:17px;text-align:center;" value="${costo}" disabled />                
                <span>USD/GJ</span>
            </div>
        `;
    } else {
        htmlDiv += `
                <span style='font-weight: bold;color: blue;'>${costo}</span>
                <span>USD/GJ</span>
            </div>
        `;
    }
    if (true) { //solo en intranet
        htmlDiv += `
            <div style = "text-align:center; padding-top: 20px;">
                <input type="button" id="btnVerCostos" value="Ver Costos" />
            </div>
        </div>
        `;
    }
    htmlDiv += `
        <div style="width: 900px; margin: 0 auto;">
            (*) Referido al poder calorífico inferior
        </div>

        <div style='font-weight: bold;width: 900px; margin: 0 auto;'>
            Mayor precio del combustible gaseoso definido por el Osinergmin para efectos tarifarios considerando el (100/90) de la tarifa de transporte y distribución.
        </div>
    `;
    if (false) { //solo en extranet
        htmlDiv += `
        <div style='font-style: oblique;padding-top: 10px; width: 900px; margin: 0 auto;'>
            <b>Nota:</b> Al momento de aprobar la solicitud de asignación, dicho valor del costo puede cambiar por el valor del costo de combustible vigente al dia de la aprobación.
        </div>
    `;
    }

    $("#" + idHandsontable).html(htmlDiv);
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Ventana emergente
////////////////////////////////////////////////////////////////////////////////////////////////////////////
function mostrarPopupGas(posTab, fila) {
    var tipoCentral = $("#hdTipoCentral").val();

    if (tipoCentral == 'E')
        popupFormularioSeccionExistente(posTab, fila);
    else
        popupFormularioSeccionNueva(posTab, fila);
}

function popupFormularioSeccionExistente(posTab, fila) {
    var objTab = MODELO_LISTA_CENTRAL[posTab];
    var objItem = objTab.ArrayItem[fila - 1];

    if (!objTab.FlagConsultoEnergiaMesAnterior) {
        objTab.FlagConsultoEnergiaMesAnterior = true;
        objTab.EnergiaMesAnterior = consultaMedidorXTipocentral($("#cbMes").val(), parseInt($("#cbEmpresa").val()) || 0, objTab.Equicodi);
    }

    $("#cbInfoSeccion").unbind();
    $('input[type=radio][name=radio_num_col]').unbind();
    $('#btnAceptarFormularioSeccion').unbind();
    $('#popupFormularioSeccion').html('');

    var htmlCombo = '';
    var htmlTxtInfo = '';
    var htmlTxtNum = '';
    var htmlDivESI = '';
    var htmlDivENO = '';
    var numCol = objItem.NumColSeccion > 0 && objItem.TipoOpcionSeccion != null ? objItem.NumColSeccion : "";
    var tieneDesp = objItem.TieneListaDesplegable ? 1 : 0;

    switch (objItem.Cbftitcnp1) {
        case CODIGO_SECCION_SUMINISTRO:
            htmlTxtInfo = 'Información del servicio de suministro';
            htmlTxtNum = 'Número de proveedores por suministro';
            htmlCombo = `
                    <option value='NA'>No aplica</option>
                    <option value='ESI'>Consumió energía durante el mes anterior</option>
                    <option value='ENO'>Energía consumida igual a cero (0) durante el mes anterior</option>
            `;
            if (objTab.EnergiaMesAnterior == 0) htmlDivESI = `
                <div id='div_ESI' style='width: 650px;display: none;font-weight: bold; margin-top: 30px; margin-bottom: 15px;'>
                    Se detectó que la energía generada es de 0 MWh ¿está seguro de escoger la opción "Consumió energía durante el mes anterior"?
                </div>
            `;
            if (objTab.EnergiaMesAnterior > 0) htmlDivENO = `
                <div id='div_ENO' style='width: 650px;display: none;font-weight: bold; margin-top: 30px; margin-bottom: 15px;'>
                    Se detectó la energía generada de ${objTab.EnergiaMesAnterior} MWh ¿está seguro de escoger la opción Energía consumida igual a cero (0) durante el mes anterior?
                </div>
            `;
            break;
        case CODIGO_SECCION_TRANSPORTE:
            htmlTxtInfo = 'Información del servicio de transporte';
            htmlTxtNum = 'Número de proveedores por transporte';
            htmlCombo = `
                    <option value='NA'>No aplica</option>
                    <option value='ESI'>Cuenta con tipo de servicio firme y/o registró consumo durante el mes anterior</option>
                    <option value='ENO'>No cuenta con tipo de servicio firme y no registró consumo durante el mes anterior</option>
            `;
            if (objTab.EnergiaMesAnterior == 0) htmlDivESI = `
                <div id='div_ESI' style='width: 650px;display: none;font-weight: bold; margin-top: 30px; margin-bottom: 15px;'>
                    Se detectó que la energía generada es de 0 MWh, ¿está seguro de escoger la opción "Cuenta con tipo de servicio firme y/o registró consumo durante el mes anterior?
                </div>
            `;
            if (objTab.EnergiaMesAnterior > 0) htmlDivENO = `
                <div id='div_ENO' style='width: 650px;display: none;font-weight: bold; margin-top: 30px; margin-bottom: 15px;'>
                    Se detectó la energía generada de ${objTab.EnergiaMesAnterior} MWh ¿está seguro de escoger la opción No cuenta con tipo de servicio firme y no registró consumo durante el mes anterior?
                </div>
            `;
            break;
        case CODIGO_SECCION_DISTRIBUCION:
            htmlTxtInfo = 'Información del servicio de distribución';
            htmlTxtNum = 'Número de proveedores por distribución';
            htmlCombo = `
                    <option value='NA'>No aplica</option>
                    <option value='ESI'>Cuenta con tipo de servicio firme y/o registró consumo durante el mes anterior</option>
                    <option value='ENO'>No cuenta con tipo de servicio firme y no registró consumo durante el mes anterior</option>
                    <option value='EDS'>Cuenta con la aplicación del mecanismo de compensación regulado por el Decreto Supremo N° 035-2013-EM y/o sus modificatorias</option>
            `;
            if (objTab.EnergiaMesAnterior == 0) htmlDivESI = `
                <div id='div_ESI' style='width: 650px;display: none;font-weight: bold; margin-top: 30px; margin-bottom: 15px;'>
                    Se detectó que la energía generada es de 0 MWh, ¿está seguro de escoger la opción "Cuenta con tipo de servicio firme y/o registró consumo durante el mes anterior?
                </div>
            `;
            if (objTab.EnergiaMesAnterior > 0) htmlDivENO = `
                <div id='div_ENO' style='width: 650px;display: none;font-weight: bold; margin-top: 30px; margin-bottom: 15px;'>
                    Se detectó la energía generada de ${objTab.EnergiaMesAnterior} MWh ¿está seguro de escoger la opción: No cuenta con tipo de servicio firme y no registró consumo durante el mes anterior?
                </div>
            `;
            break;
    }

    var htmlDivNumProv = '';
    if (objItem.TieneListaDesplegable) {
        for (var j = 0; j < objItem.ListaOpcionDesplegable.length; j++) {
            var descDesp = objItem.ListaOpcionDesplegable[j];

            htmlDivNumProv += `
                <tr>
                    <td></td>
                    <td>
                        <input type="text" id="txtNumProvDesp_${j}" name='txtNumProvDesp' value="" style='width: 22px;' autocomplete="off" >
                    </td>
                    <td>${descDesp}</td>
                </tr>
            `;
        }
    }

    var htmlDiv = `
            <span class="button b-close"><span>X</span></span>
            <div class="popup-title">
                <span>${objItem.Cbftitnumeral} ${objItem.Cbftitnombre}</span>&nbsp;&nbsp;
            </div>

            <input type='hidden' id='hdPopupTab' value='${posTab}' />
            <input type='hidden' id='hdPopupFila' value='${fila}' />

            <div id="idFormSeccion" style="width: 680px;margin-bottom: 15px;margin-top: 15px;">
                ${htmlTxtInfo}:
                <select id='cbInfoSeccion' style='width: 442px;'>
                    ${htmlCombo}
                </select>
            </div>

            ${htmlDivESI}
            ${htmlDivENO}
            
            <div id='div_num_col' style="width: 680px;margin-bottom: 15px;margin-top: 15px; display: none">
                <fieldset style="padding-bottom: 15px;">
                    <legend>Definición del número de columnas del Formato 3</legend>
                    <table style='margin: 0 auto;'>
                        <tr>
                            <td style='text-align: right;'>Igual al del mes anterior: </td>
                            <td colspan="2">
                                <input type="radio" id="rd1" name="radio_num_col" value="S">Sí 
                                <input type="radio" id="rd2" name="radio_num_col" value="N" checked="checked">No
                            </td>
                        </tr>
                        <tr>
                            <td>${htmlTxtNum}: </td>
                            <td>
                                <span id="lblNumProv" style='margin-left: 11px;'>${numCol}</span>
                                <input type="text" id="txtNumProv" value="${numCol}" style='width: 22px;display: none'>
                                <input type="hidden" id="txtNumProv_MesAnt" value="${objItem.NumColSeccionMesAnt}" >
                                <input type="hidden" id="txtNumProv_MesAct" value="${numCol}" >
                                <input type="hidden" id="txtNumProv_TieneDesp" value="${tieneDesp}" >
                            </td>
                        </tr>
                        ${htmlDivNumProv}
                    </table>
                </fieldset>
            </div>

            <table class="table-form-vertical">
                <tr>
                    <td style="text-align: center; padding-top: 20px; padding-bottom: 20px;">
                        <input type="button" id="btnAceptarFormularioSeccion" value="Guardar" style='' />
                    </td>
                </tr>
            </table>
        `;

    $('#popupFormularioSeccion').html(htmlDiv);
    $("#cbInfoSeccion").change(function () {
        _popupEcambioInfoSeccion($("#cbInfoSeccion").val());
    });
    $('input[type=radio][name=radio_num_col]').change(function () {
        _popupEcambioRadioButtonSeccion(this.value == 'S');
    });
    $('input[type=text][name=txtNumProvDesp]').change(function () {

        var totalDesp = 0;
        $('input[type=text][name=txtNumProvDesp]').each(function (i, obj) {
            var valor = $(obj).val();
            totalDesp += (parseInt(valor) || 0);
        });
        totalDesp = totalDesp > 0 ? totalDesp : "";

        $("#txtNumProv").val(totalDesp);
        $("#lblNumProv").html(totalDesp);
    });
    $('#btnAceptarFormularioSeccion').click(function () {
        var posTab = parseInt($("#hdPopupTab").val()) || 0;
        var fila = parseInt($("#hdPopupFila").val()) || 0;

        //item seleccionado
        var objTab = MODELO_LISTA_CENTRAL[posTab];
        var objItem = objTab.ArrayItem[fila - 1];

        //validacion
        var esValidoForm = true;
        var numCol = 1;
        var tipoInfo = $("#cbInfoSeccion").val();
        var numColDesp = '';

        if (tipoInfo == 'ESI') {
            numCol = parseInt($("#txtNumProv").val()) || 0;
            if (numCol <= 0) {
                esValidoForm = false;
                alert("Debe ingresar valor válido de número de columnas.");
            } else {
                var arryNumColDesp = [];
                $('input[type=text][name=txtNumProvDesp]').each(function (i, obj) {
                    var valor = $(obj).val();
                    arryNumColDesp.push(parseInt(valor) || 0);
                });
                numColDesp = arryNumColDesp.join(",");;
            }
        }

        if (tipoInfo == 'ENO') {
            if (objItem.Cbftitnumeral == '2.0') {
                if (objItem.CbcentcodiMesUltimoComb == 0) {
                    esValidoForm = false;
                    alert("No existe solicitud previa que tenga seleccionada la opción 'Consumió energía durante el mes anterior', para esta solicitud no es posible seleccionar 'Energía consumida igual a cero (0) durante el mes anterior'.");
                }
            }

            if (objItem.Cbftitnumeral == '3.0') {
                if (objItem.CbcentcodiMesUltimoComb == 0) {
                    esValidoForm = false;
                    alert("No existe solicitud previa que tenga seleccionada la opción 'Cuenta con tipo de servicio firme y/o registró consumo durante el mes anterior', para esta solicitud no es posible seleccionar 'No cuenta con tipo de servicio firme y no registró consumo durante el mes anterior'.");
                }
            }

            if (objItem.Cbftitnumeral == '4.0') {
                if (objItem.CbcentcodiMesUltimoComb == 0) {
                    esValidoForm = false;
                    alert("No existe solicitud previa que tenga seleccionada la opción 'Cuenta con tipo de servicio firme y/o registró consumo durante el mes anterior', para esta solicitud no es posible seleccionar 'No cuenta con tipo de servicio firme y no registró consumo durante el mes anterior'.");
                }
            }
        }

        if (esValidoForm)
            actualizarDimensionExcelWeb(objTab.Equicodi, objItem.Cbftitcnp1, objItem.Cbftitcnp0, tipoInfo, numCol, numColDesp, '');
    });

    setTimeout(function () {
        $('#popupFormularioSeccion').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });

        //setear valores defecto
        if (objItem.TipoOpcionSeccion != null && objItem.TipoOpcionSeccion != "") {
            $("#cbInfoSeccion").val(objItem.TipoOpcionSeccion);
            _popupEcambioInfoSeccion($("#cbInfoSeccion").val());

            var numProMesAnt = parseInt($("#txtNumProv_MesAct").val()) || 0;
            numProMesAnt = numProMesAnt > 0 ? numProMesAnt : "";

            //determinar check mes anterior
            var tieneCheckSec = numProMesAnt == objItem.NumColSeccionMesAnt;

            if (objItem.TieneListaDesplegable) {
                for (var j = 0; j < objItem.ListaOpcionDesplegable.length; j++) {
                    var numColDesp = objItem.ListaOpcionNumcolDesplegableMesActual[j];
                }
            }
            if (tieneCheckSec) $("#rd1").prop('checked', true);
            _popupEcambioRadioButtonSeccion(tieneCheckSec);
            $("#txtNumProv").val(numProMesAnt);
        }

        if (!objItem.EsSeccionSoloLectura) {
        } else {
            $("#cbInfoSeccion").prop('disabled', 'disabled');
            $("input[type=radio][name=radio_num_col]").prop('disabled', 'disabled');
            $("#txtNumProv").prop('disabled', 'disabled');
            $("input[type=text][name=txtNumProvDesp]").prop('disabled', 'disabled');
            $("#btnAceptarFormularioSeccion").hide();
        }

    }, 50);
}

function _popupEcambioInfoSeccion(valor) {
    $("#div_num_col").hide();
    $("#txtNumProv").hide();
    $("#lblNumProv").hide();
    $("#div_ESI").hide();
    $("#div_ENO").hide();

    var tieneDesp = parseInt($("#txtNumProv_TieneDesp").val()) || 0;

    var tipoInfo = valor;
    if (tipoInfo == 'ESI') {
        $("#div_ESI").show();
        $("#div_num_col").show();

        if (tieneDesp == 1)
            $("#lblNumProv").show();
        else
            $("#txtNumProv").show();
    } if (tipoInfo == 'ENO') {
        $("#div_ENO").show();
    }
}

function _popupEcambioRadioButtonSeccion(tieneCheck) {
    var posTab = parseInt($("#hdPopupTab").val()) || 0;
    var fila = parseInt($("#hdPopupFila").val()) || 0;

    //item seleccionado
    var objTab = MODELO_LISTA_CENTRAL[posTab];
    var objItem = objTab.ArrayItem[fila - 1];

    if (tieneCheck) {
        //mes anterior
        $("#txtNumProv").prop('disabled', 'disabled');

        var numProMesAnt = parseInt($("#txtNumProv_MesAnt").val()) || 0;
        $("#txtNumProv").val(numProMesAnt);

        if (objItem.TieneListaDesplegable) {
            for (var j = 0; j < objItem.ListaOpcionDesplegable.length; j++) {

                var descDesp = objItem.ListaOpcionDesplegable[j];
                var numColDesp = objItem.ListaOpcionNumcolDesplegableMesAnterior[j];
                numColDesp = numColDesp > 0 ? numColDesp : "";

                $("#txtNumProvDesp_" + j).val(numColDesp);
                $("#txtNumProvDesp_" + j).prop('disabled', 'disabled');
            }
        }
    }
    else {
        //mes actual
        $("#txtNumProv").removeAttr("disabled");
        $("#txtNumProv").val("");

        if (objItem.TieneListaDesplegable) {
            for (var j = 0; j < objItem.ListaOpcionDesplegable.length; j++) {

                var descDesp = objItem.ListaOpcionDesplegable[j];
                var numColDesp = objItem.ListaOpcionNumcolDesplegableMesActual[j];
                numColDesp = numColDesp > 0 ? numColDesp : "";

                $("#txtNumProvDesp_" + j).val(numColDesp);
                $("#txtNumProvDesp_" + j).removeAttr("disabled");
            }
        }
    }
}

function popupFormularioSeccionNueva(posTab, fila) {
    var objTab = MODELO_LISTA_CENTRAL[posTab];
    var objItem = objTab.ArrayItem[fila - 1];

    if (!objTab.FlagConsultoEnergiaMesAnterior) {
        objTab.FlagConsultoEnergiaMesAnterior = true;
        objTab.EnergiaMesAnterior = consultaMedidorXTipocentral($("#cbMes").val(), parseInt($("#cbEmpresa").val()) || 0, objTab.Equicodi);
    }

    $("#cbInfoSeccion").unbind();
    $('#mesUltEnerg').unbind();
    $('input[type=radio][name=radio_num_col_ENO]').unbind();
    $('#btnAceptarFormularioSeccion').unbind();
    $('#popupFormularioSeccion').html('');

    var htmlCombo = '';
    var htmlTxtInfo = '';
    var htmlTxtNum = '';
    var htmlMesUltimo = '';
    var htmlDivESI = '';
    var htmlDivENO = '';
    var numCol = objItem.NumColSeccion > 0 && objItem.TipoOpcionSeccion != null ? objItem.NumColSeccion : "";
    var mesVigenciaCentralNueva = objItem.MesUltimoCentralNuevaSeccion != null ? objItem.MesUltimoCentralNuevaSeccion : "";
    var tieneDesp = objItem.TieneListaDesplegable ? 1 : 0;

    switch (objItem.Cbftitcnp1) {
        case CODIGO_SECCION_SUMINISTRO:
            htmlTxtInfo = 'Información del servicio de suministro';
            htmlTxtNum = 'Número de proveedores por suministro';
            htmlMesUltimo = 'Último mes que consumió energía';
            htmlCombo = `
                    <option value='NA'>No aplica</option>
                    <option value='ESI'>Consumió energía durante el mes anterior</option>
                    <option value='ENO'>Energía consumida igual a cero (0) durante el mes anterior</option>
            `;
            if (objTab.EnergiaMesAnterior == 0) htmlDivESI = `
                <div id='div_ESI' style='width: 650px;display: none;font-weight: bold; margin-top: 30px; margin-bottom: 15px;'>
                    Se detectó que la energía generada es de 0 MWh ¿está seguro de escoger la opción "Consumió energía durante el mes anterior"?
                </div>
            `;
            htmlDivENO = `
                <div id='div_ENO' style='width: 650px;display: none;font-weight: bold; margin-top: 30px; margin-bottom: 15px;'>
                    Se detectó la energía generada de <span id='span_mwh'>${objTab.EnergiaMesAnterior}</span> MWh, ¿está seguro de escoger la opción "Energía consumida igual a cero (0) durante el mes anterior?"
                </div>
            `;
            break;
        case CODIGO_SECCION_TRANSPORTE:
            htmlTxtInfo = 'Información del servicio de transporte';
            htmlTxtNum = 'Número de proveedores por transporte';
            htmlMesUltimo = 'Último mes que utilizó el servicio de transporte';
            htmlCombo = `
                    <option value='NA'>No aplica</option>
                    <option value='ESI'>Cuenta con tipo de servicio firme y/o registró consumo durante el mes anterior</option>
                    <option value='ENO'>No cuenta con tipo de servicio firme y no registró consumo durante el mes anterior</option>
            `;
            if (objTab.EnergiaMesAnterior == 0) htmlDivESI = `
                <div id='div_ESI' style='width: 650px;display: none;font-weight: bold; margin-top: 30px; margin-bottom: 15px;'>
                    Se detectó que la energía generada es de 0 MWh, ¿está seguro de escoger la opción "Cuenta con tipo de servicio firme y/o registró consumo durante el mes anterior"?
                </div>
            `;
            htmlDivENO = `
                <div id='div_ENO' style='width: 650px;display: none;font-weight: bold; margin-top: 30px; margin-bottom: 15px;'>
                    Se detectó la energía generada de <span id='span_mwh'>${objTab.EnergiaMesAnterior}</span> MWh, ¿está seguro de escoger la opción "No cuenta con tipo de servicio firme y no registró consumo durante el mes anterior"?
                </div>
            `;
            break;
        case CODIGO_SECCION_DISTRIBUCION:
            htmlTxtInfo = 'Información del servicio de distribución';
            htmlTxtNum = 'Número de proveedores por distribución';
            htmlMesUltimo = 'Último mes que utilizó el servicio de distribución ';
            htmlCombo = `
                    <option value='NA'>No aplica</option>
                    <option value='ESI'>Cuenta con tipo de servicio firme y/o registró consumo durante el mes anterior</option>
                    <option value='ENO'>No cuenta con tipo de servicio firme y no registró consumo durante el mes anterior</option>
                    <option value='EDS'>Cuenta con la aplicación del mecanismo de compensación regulado por el Decreto Supremo N° 035-2013-EM y/o sus modificatorias</option>
            `;
            if (objTab.EnergiaMesAnterior == 0) htmlDivESI = `
                <div id='div_ESI' style='width: 650px;display: none;font-weight: bold; margin-top: 30px; margin-bottom: 15px;'>
                    Se detectó que la energía generada es de 0 MWh, ¿está seguro de escoger la opción "Cuenta con tipo de servicio firme y/o registró consumo durante el mes anterior?
                </div>
            `;
            htmlDivENO = `
                <div id='div_ENO' style='width: 650px;display: none;font-weight: bold; margin-top: 30px; margin-bottom: 15px;'>
                    Se detectó la energía generada de <span id='span_mwh'>${objTab.EnergiaMesAnterior}</span> MWh, ¿está seguro de escoger la opción "No cuenta con tipo de servicio firme y no registró consumo durante el mes anterior"?
                </div>
            `;
            break;
    }

    var htmlDivNumProvESI = '';
    var htmlDivNumProvENO = '';
    if (objItem.TieneListaDesplegable) {
        for (var j = 0; j < objItem.ListaOpcionDesplegable.length; j++) {
            var descDesp = objItem.ListaOpcionDesplegable[j];
            var numColDesp = objItem.ListaOpcionNumcolDesplegableMesActual[j];
            numColDesp = numColDesp > 0 ? numColDesp : "";

            htmlDivNumProvESI += `
                <tr class='tr_txtNumProvDesp_ESI' style='display: none'>
                    <td></td>
                    <td>
                        <input type="text" id="txtNumProvDesp_ESI_${j}" name='txtNumProvDesp_ESI' value="${numColDesp}" style='width: 22px;'>
                    </td>
                    <td>${descDesp}</td>
                </tr>
            `;
            htmlDivNumProvENO += `
                <tr class='tr_txtNumProvDesp_ENO' style='display: none'>
                    <td></td>
                    <td>
                        <input type="text" id="txtNumProvDesp_ENO_${j}" name='txtNumProvDesp_ENO' value="${numColDesp}" style='width: 22px;'>
                    </td>
                    <td>${descDesp}</td>
                </tr>
            `;
        }
    }

    var htmlDiv = `
            <span class="button b-close"><span>X</span></span>
            <div class="popup-title">
                <span>${objItem.Cbftitnumeral} ${objItem.Cbftitnombre}</span>&nbsp;&nbsp;
            </div>

            <div id="idFormSeccion" style="width: 650px;margin-bottom: 15px;margin-top: 15px;">
                ${htmlTxtInfo}:
                <select id='cbInfoSeccion' style='width: 350px;'>
                    ${htmlCombo}
                </select>
            </div>

            ${htmlDivESI}
            ${htmlDivENO}
            
            <div id='div_num_col_ESI' style="width: 650px;margin-bottom: 15px;margin-top: 15px; display: none">
                <fieldset style="padding-bottom: 15px;">
                    <legend>Definición del número de columnas del Formato 3</legend>
                    <table style='margin: 0 auto;'>
                       <tr>
                            <td>${htmlTxtNum}: </td>
                            <td>
                                <span id="lblNumProv_ESI" style='margin-left: 11px;'>${numCol}</span>
                                <input type="text" id="txtNumProv_ESI" value="${numCol}" style='width: 22px;display: none'>
                                <input type="hidden" id="txtNumProv_TieneDesp" value="${tieneDesp}" >
                            </td>
                        </tr>
                        ${htmlDivNumProvESI}
                    </table>
                </fieldset>
            </div>

            <div id='div_num_col_ENO' style="width: 650px;margin-bottom: 15px;margin-top: 15px; display: none">
                <fieldset style="padding-bottom: 15px;">
                    <legend>Definición del número de columnas del Formato 3</legend>
                    <table style='margin: 0 auto;'>
                        <tr>
                            <td style='text-align: right;'>¿Consumió energía en los meses pasados?: </td>
                            <td colspan="2">
                                <input type="radio" id="rd1" name="radio_num_col_ENO" value="S">Sí
                                <input type="radio" id="rd2" name="radio_num_col_ENO" value="N" checked="checked">No
                            </td>
                        </tr>
                        <tr id='tr_mes_energ' style='display: none'>
                            <td>${htmlMesUltimo}: </td>
                            <td>
                                <input type="text" id="mesUltEnerg" style="width: 77px;" value="${mesVigenciaCentralNueva}" />
                                <input type="hidden" id="equipoUltEnerg" style="width: 77px;" value="${objTab.Equicodi}" />
                            </td>
                        </tr>
                        <tr id='tr_num_energ' style='display: none'>
                            <td>${htmlTxtNum}: </td>
                            <td>
                                <span id="lblNumProv_ENO" style='margin-left: 11px;'>${numCol}</span>
                                <input type="text" id="txtNumProv_ENO" value="${numCol}" style='width: 22px;display: none'>
                            </td>
                        </tr>
                        ${htmlDivNumProvENO}
                    </table>
                </fieldset>
            </div>

            <table class="table-form-vertical">
                <tr>
                    <td style="text-align: center; padding-top: 20px; padding-bottom: 20px;">
                        <input type="button" id="btnAceptarFormularioSeccion" value="Guardar" style='' />
                    </td>
                </tr>
            </table>
        `;

    $('#popupFormularioSeccion').html(htmlDiv);
    $("#cbInfoSeccion").change(function () {
        _popupNcambioInfoSeccion($("#cbInfoSeccion").val());
    });
    $('#mesUltEnerg').Zebra_DatePicker({
        format: "m-Y",
        onSelect: function (date) {
            consultarMedidor($("#mesUltEnerg").val(), $("#cbEmpresa").val(), $("#equipoUltEnerg").val());
        }
    });

    $('input[type=radio][name=radio_num_col_ENO]').change(function () {
        _popupNcambioRadioButtonSeccion(this.value == 'S');
    });
    $('input[type=text][name=txtNumProvDesp_ENO]').change(function () {

        var totalDesp = 0;
        $('input[type=text][name=txtNumProvDesp_ENO]').each(function (i, obj) {
            var valor = $(obj).val();
            totalDesp += (parseInt(valor) || 0);
        });
        totalDesp = totalDesp > 0 ? totalDesp : "";

        $("#txtNumProv_ENO").val(totalDesp);
        $("#lblNumProv_ENO").html(totalDesp);
    });
    $('input[type=text][name=txtNumProvDesp_ESI]').change(function () {

        var totalDesp = 0;
        $('input[type=text][name=txtNumProvDesp_ESI]').each(function (i, obj) {
            var valor = $(obj).val();
            totalDesp += (parseInt(valor) || 0);
        });
        totalDesp = totalDesp > 0 ? totalDesp : "";

        $("#txtNumProv_ESI").val(totalDesp);
        $("#lblNumProv_ESI").html(totalDesp);
    });

    $('#btnAceptarFormularioSeccion').click(function () {
        var esValidoForm = true;
        var numCol = 1;
        var tipoInfo = $("#cbInfoSeccion").val();
        var numColDesp = '';
        var mesAnterior = '';

        if (tipoInfo == 'ESI') {
            numCol = parseInt($("#txtNumProv_ESI").val()) || 0;
            if (numCol <= 0) {
                esValidoForm = false;
                alert("Debe ingresar valor válido de número de columnas.");
            } else {
                var arryNumColDesp = [];
                $('input[type=text][name=txtNumProvDesp_ESI]').each(function (i, obj) {
                    var valor = $(obj).val();
                    arryNumColDesp.push(parseInt(valor) || 0);
                });
                numColDesp = arryNumColDesp.join(",");;
            }
        }
        if (tipoInfo == 'ENO') {
            var opcionTieneEnergia = $('input[name="radio_num_col_ENO"]:checked').val();
            if (opcionTieneEnergia == 'N') {
                alert("Error, falta comprobante de pago");
                esValidoForm = false;
            } else {
                numCol = parseInt($("#txtNumProv_ENO").val()) || 0;
                if (numCol <= 0) {
                    esValidoForm = false;
                    alert("Debe ingresar valor válido de número de columnas.");
                } else {
                    var arryNumColDesp = [];
                    $('input[type=text][name=txtNumProvDesp_ENO]').each(function (i, obj) {
                        var valor = $(obj).val();
                        arryNumColDesp.push(parseInt(valor) || 0);
                    });
                    numColDesp = arryNumColDesp.join(",");;
                }

                mesAnterior = $("#mesUltEnerg").val();
                if (mesAnterior == null || mesAnterior == "")
                    alert("Debe seleccionar mes.");
            }
        }

        if (esValidoForm)
            actualizarDimensionExcelWeb(objTab.Equicodi, objItem.Cbftitcnp1, objItem.Cbftitcnp0, tipoInfo, numCol, numColDesp, mesAnterior);
    });

    setTimeout(function () {
        $('#popupFormularioSeccion').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });

        //setear valores defecto
        if (objItem.TipoOpcionSeccion != null && objItem.TipoOpcionSeccion != "") {
            $("#cbInfoSeccion").val(objItem.TipoOpcionSeccion);
            _popupNcambioInfoSeccion($("#cbInfoSeccion").val());

            var tieneCheck = objItem.MesUltimoCentralNuevaSeccion != null && objItem.MesUltimoCentralNuevaSeccion != "";
            if (tieneCheck) {
                $("#rd1").prop('checked', true);
                $('#mesUltEnerg').val(objItem.MesUltimoCentralNuevaSeccion);
                _popupNcambioRadioButtonSeccion(tieneCheck);
            }
        }

        if (!objItem.EsSeccionSoloLectura) {
        } else {
            $("#cbInfoSeccion").prop('disabled', 'disabled');
            $("input[type=radio][name=radio_num_col_ENO]").prop('disabled', 'disabled');
            $("#mesUltEnerg").prop('disabled', 'disabled');
            $("#txtNumProv_ESI").prop('disabled', 'disabled');
            $("#txtNumProv_ENO").prop('disabled', 'disabled');
            $("input[type=text][name=txtNumProvDesp_ENO]").prop('disabled', 'disabled');
            $("input[type=text][name=txtNumProvDesp_ESI]").prop('disabled', 'disabled');
            $("#btnAceptarFormularioSeccion").hide();
        }
    }, 50);
}

function _popupNcambioInfoSeccion(valor) {
    $("#div_num_col_ESI").hide();
    $("#div_num_col_ENO").hide();
    $("#txtNumProv_ENO").hide();
    $("#lblNumProv_ENO").hide();
    $("#txtNumProv_ESI").hide();
    $("#lblNumProv_ESI").hide();
    $("#div_ESI").hide();
    $("#div_ENO").hide();

    var tieneDesp = parseInt($("#txtNumProv_TieneDesp").val()) || 0;

    var tipoInfo = valor;
    if (tipoInfo == 'ESI') {
        $("#div_ESI").show();
        $("#div_num_col_ESI").show();
        $(".tr_txtNumProvDesp_ESI").show();

        if (tieneDesp == 1)
            $("#lblNumProv_ESI").show();
        else
            $("#txtNumProv_ESI").show();
    } if (tipoInfo == 'ENO') {
        $("#div_ENO").show();
        $("#div_num_col_ENO").show();

        if (tieneDesp == 1)
            $("#lblNumProv_ENO").show();
        else
            $("#txtNumProv_ENO").show();
    }
}

function _popupNcambioRadioButtonSeccion(tieneCheck) {
    if (tieneCheck) {
        $("#tr_mes_energ").show();
        $("#tr_num_energ").show();
        $(".tr_txtNumProvDesp_ENO").show();
    }
    else {
        $("#tr_mes_energ").hide();
        $("#tr_num_energ").hide();
        $(".tr_txtNumProvDesp_ENO").hide();
    }
}

/////////////////////////////////////////////
//No aplica, Decimales
function actualizarHandsonValorCelda(tipoOperacion, valor, numDecimal) {
    //existe celda seleccionada
    if (objPosSelecTmp.equicodi > 0) {
        for (var i = 0; i < MODELO_LISTA_CENTRAL.length; i++) {
            var objTabCentral = MODELO_LISTA_CENTRAL[i];

            //tab seleccionado
            if (objTabCentral.Equicodi == objPosSelecTmp.equicodi) {
                var matriz = objTabCentral.Handson.ListaExcelData;
                var matrizTipoEstado = objTabCentral.Handson.MatrizTipoEstado;
                var matrizDigitoDecimal = objTabCentral.Handson.MatrizDigitoDecimal;

                var filaIni = objTabCentral.FilaIni;
                var numMaxColData = objTabCentral.NumMaxColData;

                var colNumSeccion = 0;
                var colNumItem = colNumSeccion + 1;
                var colProp = colNumItem + 1;
                var colUnidad = colProp + 1;
                var colValorIni = colUnidad + 1;
                var colValorFin = colValorIni + numMaxColData - 1;

                var arrayCambios = [];
                if (objPosSelecTmp.colIni >= colValorIni && objPosSelecTmp.colFin <= colValorFin) {

                    //recorrer celdas seleccionadas
                    for (var conti = objPosSelecTmp.rowIni; conti <= objPosSelecTmp.rowFin; conti++) {
                        var objItem = objTabCentral.ArrayItem[conti - 1];

                        for (var contj = objPosSelecTmp.colIni; contj <= objPosSelecTmp.colFin; contj++) {

                            //No Aplica
                            if (TIPO_OPERACION_NO_APLICA == tipoOperacion) {
                                if (matrizTipoEstado[conti][contj] == 1) { //si la celda es editable

                                    var arrayCeldaCambio = [];
                                    arrayCeldaCambio.push(conti); //row
                                    arrayCeldaCambio.push(contj); //col
                                    arrayCeldaCambio.push(valor); //value

                                    //agregar array
                                    arrayCambios.push(arrayCeldaCambio);
                                }
                            }

                            //decimales
                            if (TIPO_OPERACION_DECIMAL == tipoOperacion) {
                                var valorCelda = (matriz[conti][contj] ?? "").trim();
                                var numDecCelda = matrizDigitoDecimal[conti][contj];
                                var numDecTmp = numDecCelda + numDecimal;

                                if (numDecTmp >= 4 && numDecTmp <= 10) {
                                    if ((objItem.Cbftittipodato == "FORMULA") && valorCelda.toUpperCase() != "NO APLICA") { //si la celda es editable

                                        var valorCeldaTmp = (parseFloat(valorCelda) || 0).toFixed(numDecTmp);

                                        var arrayCeldaCambio = [];
                                        arrayCeldaCambio.push(conti); //row
                                        arrayCeldaCambio.push(contj); //col
                                        arrayCeldaCambio.push(valorCeldaTmp); //value

                                        //agregar array
                                        arrayCambios.push(arrayCeldaCambio);

                                        objTabCentral.Handson.MatrizDigitoDecimal[conti][contj] = numDecTmp;
                                    }
                                }
                            }
                        }
                    }
                }

                //Set new value to a cell. To change many cells at once (recommended way), pass an array of changes in format [[row, col, value],...] as the first argument.
                LISTA_OBJETO_HOJA[objTabCentral.Equicodi].hot.setDataAtCell(arrayCambios);
            }
        }
    }
}

function _inicializarObjetoPosicion() {
    var objPos = { rowIni: 0, colIni: 0, rowFin: 0, colFin: 0, equicodi: 0 };
    return objPos;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Errores
////////////////////////////////////////////////////////////////////////////////////////////////////////////
function mostrarDetalleErrores() {

    $("#idTerrores").html('');

    var htmlTerrores = dibujarTablaError();
    $("#idTerrores").html(htmlTerrores);

    setTimeout(function () {
        $('#validaciones').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });

        $('#tablaError').dataTable({
            "scrollY": 300,
            "scrollX": false,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
            , "oLanguage": { "sZeroRecords": "", "sEmptyTable": "" }
        });

    }, 200);
}

//////////////////////////////////////////////////////////
//// popup para ver envios
//////////////////////////////////////////////////////////
function mostrarEnviosAnteriores() {
    $('#idEnviosAnteriores').html(dibujarTablaEnvios());
    setTimeout(function () {
        $('#enviosanteriores').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
        $('#tablalenvio').dataTable({
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });
    }, 50);
}

function mostrarEnvioExcelWeb(envio) {
    $('#enviosanteriores').bPopup().close();
    //setIdEnvio(numHoja, envio);
    //mostrarFormulario(numHoja, OPCION_ENVIO_ANTERIOR);
    $("#hfIdVersion").val(envio);
    mostrarGrilla();
}

function dibujarTablaEnvios() {

    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablalenvio' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Versión</th><th>Fecha Hora</th><th>Usuario</th></tr></thead>";
    cadena += "<tbody>";

    for (key in LISTA_VERSIONES) {
        cadena += `
            <tr onclick='mostrarEnvioExcelWeb(${LISTA_VERSIONES[key].Cbvercodi});' style='cursor:pointer'>
                <td>${LISTA_VERSIONES[key].Cbvernumversion}</td>
                <td>${LISTA_VERSIONES[key].CbverfeccreacionDesc} </td>
                <td>${LISTA_VERSIONES[key].Cbverusucreacion}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    cadena += `
         <table id='tabla_log_envio' border='1' class='pretty tabla-adicional' cellspacing='0' style='width: auto;'>
            <caption>Acciones</caption>
            <thead>
                <tr>
                    <th>Acción</th>
                    <th>Descripción</th>
                    <th>Usuario</th>
                    <th>Fecha</th>
                </tr>
            </thead>
            <tbody>
    `;

    for (key in LISTA_LOG) {
        var reg = LISTA_LOG[key];
        cadena += `
                <tr>
                    <td style=''>${reg.Estenvnomb}</td>
                    <td style='white-space: break-spaces; text-align: left;'>${reg.Logenvobservacion}</td>
                    <td style='text-align: center'>${reg.Logenvusucreacion}</td>
                    <td style=''>${reg.LogenvfeccreacionDesc}</td>
                </tr>
        `;
    }

    cadena += `
            <tbody>
        </table>
    `;

    return cadena;

}

function getFormattedDate(date) {
    if (date instanceof Date) {
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var ampm = hours >= 12 ? 'pm' : 'am';
        hours = hours % 12;
        hours = hours ? hours : 12; // the hour '0' should be '12'
        hours = hours < 10 ? '0' + hours : hours;
        minutes = minutes < 10 ? '0' + minutes : minutes;
        var strTime = hours + ':' + minutes + ' ' + ampm;

        return year + '/' + month + '/' + day + " " + strTime;
    }
    else {
        return "No es fecha";
    }
}

function dibujarTablaError() {
    var cadena = `
        <div style='clear:both; height:5px'></div>
            <table id='tablaError' border='1' class='pretty tabla-adicional' cellspacing='0' style='width: 760px'>
                <thead>
                    <tr>
                        <th>Pestaña</th>
                        <th>Item</th>
                        <th>Valor</th>
                        <th>Tipo Error</th>
                    </tr>
                </thead>
                <tbody>
    `;

    for (var i = 0; i < LISTA_ERRORES.length; i++) {
        var item = LISTA_ERRORES[i];
        cadena += `
                    <tr>
                        <td>${item.Tab}</td>
                        <td style='text-align: left; white-space: break-spaces;'>${item.Celda}</td>
                        <td>${item.Valor}</td>
                        <td>${item.Mensaje}</td>
                    </tr>
        `;
    }

    cadena += `
                </tbody>
            </table>
        </div>
    `;

    return cadena;
}


function pintarEstadosPorCentral() {

    var lstDatosCentrales = obtenerDatosCentrales();

    for (var i = 0; i < lstDatosCentrales.length; i++) {
        var centralX = lstDatosCentrales[i];

        var dataF3 = centralX.DataFormularioF3;
        var dataArch = centralX.DataFormularioArchivos;

        var strCentralF3X = "";
        var strCentralAX = "";

        //para las secciones del F3
        for (var y = 0; y < dataF3.length; y++) {
            var seccionF3X = dataF3[y];

            var estadoX = seccionF3X.Estado;
            var letra = "";

            if (estadoX == 1)//"CONFORME"
                letra = "U";
            if (estadoX == 2)//"OBSERVADO"
                letra = "D";
            if (estadoX == 3)//"SUBSANADO"
                letra = "T";
            if (estadoX == 4)//"NO SUBSANADO"
                letra = "C";

            strCentralF3X += letra;

        }

        var estadoHtml = ``;
        var estadoColor = '';
        //Si todos son CONFORME PINTO "APROBADO"
        if (strCentralF3X == "UUUU") {
            estadoColor = '#99FEA3'; //verde
            estadoHtml = `
                                <div style="font-size: 16px; font-weight:bold; color:#38CB33;">
                                    Aprobado
                                </div>
                            `;

        } else {
            if (strCentralF3X.includes("C") || strCentralF3X.includes("D")) {//Si hay 1 NO_SUBNADO/OBSERVADO PINTO "DESAPROBADO"                
                estadoColor = '#FCB0B0'; //rojo
                estadoHtml = `
                                <div style="font-size: 16px; font-weight:bold; color:#C20C21;">
                                    Desaprobado
                                </div>
                            `;
            } else {
                estadoHtml = ``;
                estadoColor = '#FFFFFF'; //blanco
            }
        }
        var miIdTexto = "estadoCentralTermica_" + centralX.Equicodi;
        var miIdPestania = "tab_central_" + centralX.Equicodi;
        $("#" + miIdTexto).html(estadoHtml);
        $("#" + miIdPestania).css('background', estadoColor);

    }

}

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

//////////////////////////////////////////////////////////
//// btnVerLeyenda
//////////////////////////////////////////////////////////

var LISTA_VALIDACION = [
    { tipo: '1', Descripcion: 'Celda con información correcta', BackgroundColor: 'white', Color: 'black', },
    { tipo: '2', Descripcion: 'Celda de solo lectura', BackgroundColor: '#C8D8EA', Color: 'black', },
    { tipo: '3', Descripcion: 'Error: El dato es menor que el límite inferior', BackgroundColor: '#FFF200', Color: 'black', },
    { tipo: '4', Descripcion: "Error: El dato es mayor que el límite superior", BackgroundColor: '#FF7F27', Color: 'black', },
    { tipo: '5', Descripcion: 'Error: valor negativo', BackgroundColor: '#FEC3E1', Color: 'black', },
    { tipo: '6', Descripcion: 'Error: Separador decimal coma (,)', BackgroundColor: '#FEC3E1', Color: 'black', },
    { tipo: '7', Descripcion: 'Error: El ítem 3.14 no puede ser mayor al ítem 3.6', BackgroundColor: '#FEC3E1', Color: 'black', },
    { tipo: '8', Descripcion: 'Error: Celda vacía en las celdas no bloqueadas', BackgroundColor: '#FEC3E1', Color: 'black', },
    { tipo: '8', Descripcion: 'Error: Falta adjuntar informe sustentatorio', BackgroundColor: '#FEC3E1', Color: 'black', },
    { tipo: '8', Descripcion: 'Error: Celda con el texto “No aplica” incorrecto', BackgroundColor: '#FEC3E1', Color: 'black', },
    { tipo: '8', Descripcion: 'Error: Ítem no subsanado', BackgroundColor: '#FEC3E1', Color: 'black', },
];

function popupLeyenda() {
    $('#idLeyenda').html(dibujarTablaLeyenda(LISTA_VALIDACION));
    setTimeout(function () {
        $('#leyenda').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
        $('#tablaLeyenda').dataTable({
            "scrollY": 330,
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });

    }, 50);
}

function dibujarTablaLeyenda(validacionApp) {

    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<div style='height:115px'>";
    cadena += "<table id='tablaLeyenda' border='1' class='pretty tabla-adicional' cellspacing='0' >";
    cadena += "<thead><tr><th>Celda</th><th>Descripción</th></tr></thead>";
    cadena += "<div>";

    var len = validacionApp.length;
    for (var i = 0; i < len; i++) {
        cadena += "<tr>";
        cadena += '     <td style="background-color: ' + validacionApp[i].BackgroundColor + ' !important; color: white;"></td>';
        cadena += "<td style='text-align: left;'>" + validacionApp[i].Descripcion + "</td></tr>";
    }
    cadena += "</div></table></div>";

    return cadena;
}
