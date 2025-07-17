var EVT_GLOBAL = null;
var MODELO_WEB = null;
var numArchivosS2 = 0;

function mostrarGrilla() {
    $('#barraConsumo').css("display", "block");
    var idEnvio = parseInt($("#hfIdEnvio").val()) || 0;
    var empresa = $("#IdEmpresa").val();
    var equicodi = $("#hdIdEquipo").val();
    var grupocodi = $("#hdIdGrupo").val();
    var combustible = $("#hdIdCombustible").val();
    var idFenergcodi = $("#hdIdFenerg").val();

    $.ajax({
        type: 'POST',
        url: controlador + "MostrarGrilla",
        dataType: 'json',
        data: {
            idEnvio: idEnvio,
            idEmpresa: empresa,
            equicodi: equicodi,
            grupocodi: grupocodi,
            idFenergcodi: idFenergcodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                MODELO_WEB = evt.ModeloWeb;
                EVT_GLOBAL = evt;

                //tab grilla
                cargarHtmlFormularioXEnvio(MODELO_WEB.ListaSeccion, EVT_GLOBAL.AccionEditar);

                //tab archivos
                cargarHtmlDocumentoXEnvio(MODELO_WEB.ListaSeccionDocumento, EVT_GLOBAL.AccionEditar);

                //
                cargarHtmlLogXEnvio();

                cargarHtmlCostosVariables();

                //MENSAJES
                var mensajeObsHtml = MODELO_WEB.Observacion != null ? MODELO_WEB.Observacion : '';
                mensajeObsHtml = mensajeObsHtml.replace(/(?:\r\n|\r|\n)/g, '<br>');

                if (MODELO_WEB.Estenvcodi == ESTADO_SOLICITUD) {
                    if (idEnvio > 0) {
                        var msj = '';

                        if (MODELO_WEB.EsExtranet) {
                            if (EVT_GLOBAL.AccionEditar)
                                msj += "Si necesita actualizar la información del envío, tiene hasta el día de hoy para realizar la acción.";
                            else
                                msj += "No se permite la edición del formulario.";
                        }


                        $("#div_dia_habil").html(msj);
                        $("#div_dia_habil").show();
                    }
                }

                if (MODELO_WEB.Estenvcodi == ESTADO_OBSERVADO) {
                    $("#container .content-titulo").append(" <span style='font-weight: bold; color: #ffd400;'>(OBSERVADO)<span>");

                    var msj = '';
                    if (MODELO_WEB.Fecfinsubsanarobs != null && MODELO_WEB.Fecfinsubsanarobs != '') {
                        msj = `<b>El COES ha observado la solicitud</b>. Tiene plazo hasta <b>${MODELO_WEB.Fecfinsubsanarobs}</b> para subsanar la solicitud. `;
                        if (MODELO_WEB.EsExtranet) msj += "Se permite la edición del formulario";
                    }

                    if (MODELO_WEB.Fecfinampliacion != null && MODELO_WEB.Fecfinampliacion != '') {
                        msj = `El COES ha ampliado el plazo hasta <b>${MODELO_WEB.Fecfinampliacion}</b> para la subsanación de observaciones. `;
                        if (MODELO_WEB.EsExtranet) msj += "Se permite la edición del formulario";
                    }

                    if (MODELO_WEB.EsEditableItem106) {
                        msj += `<br/>El COES ha habilitado la edición ítem 1.06 para su edición.`;
                    }

                    if (msj != '') {
                        $("#div_dia_habil").html(msj);
                        $("#div_dia_habil").show();
                    }

                    //observacion
                    if (mensajeObsHtml) {
                        $("#idobservacion").html(`Observación COES: <br/> <b style="color: red;">"${mensajeObsHtml}"</b>`);
                        $("#idobservacion").show();
                    }
                }

                if (MODELO_WEB.Estenvcodi == ESTADO_LEVANTAMIENTO_OBS) {
                    $("#container .content-titulo").append(" <span style='font-weight: bold; color: #ffd400;'>(SUBSANACIÓN DE OBSERVACIONES)<span>");

                    var msj = '';
                    if (!MODELO_WEB.EsExtranet && MODELO_WEB.EsEditableItem106) {
                        msj += `<br/>El COES habilitó la edición ítem 1.06 para la subsanación de observaciones.`;
                    }

                    $("#div_dia_habil").html(msj);
                    $("#div_dia_habil").show();

                    //observacion
                    if (mensajeObsHtml) {
                        $("#idobservacion").html(`Observación COES: <br/> <b style="color: red;">"${mensajeObsHtml}"</b>`);
                        $("#idobservacion").show();
                    }
                }

                if (MODELO_WEB.Estenvcodi == ESTADO_CANCELADO) {
                    $("#container .content-titulo").append(" <span style='font-weight: bold; color: #9a9a9a;'>(CANCELADO)<span>");

                    $("#idobservacion").html(`Motivo de Cancelación: <br/> <b style="">"${mensajeObsHtml}"</b>`);
                    $("#idobservacion").show();
                }

                if (MODELO_WEB.Estenvcodi == ESTADO_APROBADO) {
                    $("#container .content-titulo").append(" <span style='font-weight: bold; color: #37bd3c'>(APROBADO)<span>");

                    $("#div_dia_habil").html('');
                    $("#div_dia_habil").show();
                }

                if (MODELO_WEB.Estenvcodi == ESTADO_DESAPROBADO) {
                    $("#container .content-titulo").append(" <span style='font-weight: bold; color: #FF7979'>(DESAPROBADO)<span>");

                    if (mensajeObsHtml) {
                        var msj = `Mensaje al generador: <br/> <b style="">${mensajeObsHtml}</b>`;
                        $("#div_dia_habil").html(msj);
                        $("#div_dia_habil").show();
                    }

                    if (MODELO_WEB.HabilitarDesaprobar)
                        $(".btn_desaprobar").parent().css("display", "table-cell");
                }

                if (EVT_GLOBAL.AccionEditar) {
                    $("#barra_herramienta_envio").show();
                    $("#leyenda_alerta").show();
                    $("#btnEnviarDatos").parent().css("display", "table-cell");
                    $("#btnMostrarErrores").parent().css("display", "table-cell");

                    if (MODELO_WEB.Estenvcodi != ESTADO_OBSERVADO) {
                        $(".btn_desaprobar").parent().css("display", "table-cell");
                        $(".btn_aprobar").parent().css("display", "table-cell");
                        $(".btn_observar").parent().css("display", "table-cell");
                    }

                    //errores o datos incompletos
                    if (MODELO_WEB.ListaErrores.length > 0) {
                        mostrarInformativo(MENSAJE_FALTA_DATOS);
                    }
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

function getTipoCambio(fecha, idTdCambio) {
    $.ajax({
        type: 'POST',
        data: {
            fecha: fecha
        },
        dataType: 'json',
        url: controlador + "ObtenerTipoCambio",
        success: function (evt) {
            if (evt.Resultado != "-1")
                $("#" + idTdCambio).val(evt.TipoCambio);
            else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function getVolumenRecepcion(fecha, sufijo) {

    var id_volumen = 'i_volumen_' + sufijo;
    var id_msj = 'i_msj_val_' + sufijo;
    var id_html_msj = 'html_val_recep_' + sufijo;

    var equicodi = $("#hdIdEquipo").val();
    var grupocodi = $("#hdIdGrupo").val();
    var fenergcodi = $("#hdIdFenerg").val();
    var idEnvio = parseInt($("#hfIdEnvio").val()) || 0;

    $.ajax({
        type: 'POST',
        data: {
            fecha: fecha,
            grupocodi: grupocodi,
            fenergcodi: fenergcodi,
            equicodi: equicodi,
            cbenvcodi: idEnvio
        },
        dataType: 'json',
        url: controlador + "ObtenerVolumenRecepcion",
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $("#" + id_volumen).val(evt.CombustibleAlmacenado);
                $("#" + id_msj).val(evt.MensajeFechaRecepcion);

                $("#" + id_html_msj).html(evt.MensajeFechaRecepcion);
            }
            else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function validarEnvio() {
    var seccionAcrchivos2 = MODELO_WEB.ListaSeccionDocumento[0];
    var archivosSec2 = seccionAcrchivos2.ListaArchivo;
    numArchivosS2 = archivosSec2.length;

    if (MODELO_WEB.ListaErrores.length > 0 || numArchivosS2 == 0) {
        alert("Error al Grabar, hay errores en celdas");
        mostrarError(MENSAJE_FALTA_DATOS);

        mostrarDetalleErrores();
        return false;
    }
    else {
        return true;
    }
}

function actualizarExcelWeb() {
    $("#mensajeEvento").hide();

    MODELO_WEB.ListaErrores = [];
    var dataJson = JSON.stringify(MODELO_WEB);

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: controlador + "ActualizarGrilla",
        data: {
            dataJson: dataJson
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                MODELO_WEB = evt.ModeloWeb;

                cargarHtmlFormularioXEnvio(MODELO_WEB.ListaSeccion, EVT_GLOBAL.AccionEditar);
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error.");
        }
    });
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Handson
////////////////////////////////////////////////////////////////////////////////////////////////////////////
function cargarHtmlFormularioXEnvio(listaSeccion, tienePermisoEditar) {
    var altoHanson = MODELO_WEB.Estcomcodi == TIPO_COMB_LIQUIDO ? 1100 : 1675;
    var html = `
            <div class="bodyexcel handsontable htRowHeaders" id="hot" 
                                style="display: inline-block; height: ${altoHanson}px; overflow: hidden; width: 1020px;" data-initialstyle="display:inline-block;" 
                                data-originalstyle="display: inline-block; height: ${altoHanson}px; overflow: hidden; width: 1020px;">
                <div class="ht_master handsontable" style="position: relative;">
                    <div class="wtHolder" style="position: relative; width: 1020px; height: ${altoHanson}px;">
                        <table class="htCore">
                            <colgroup>
                                <col style="width: 54px;">
                                <col style="width: 54px;">
                                <col style="width: 555px;">
                                <col style="width: 107px;">
                                <col style="width: 71px;">
                                <col style="width: 107px;">
                                <col style="width: 71px;">
                            </colgroup>
                            <thead></thead>
                            <tbody>
    `;

    for (var i = 0; i < listaSeccion.length; i++) {
        var regSeccion = listaSeccion[i];
        html += generarHtmlTablaFormulario(regSeccion.Seccion, regSeccion.ListaItem, tienePermisoEditar);
    }

    html += `
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
    `;

    $("#html_formulario").html(html);
}

function generarHtmlTablaFormulario(seccion, listaItem, tienePermisoEditar) {

    var html = '';

    if (!seccion.EsFilaResultado) {
        var tdhtml_uni = tienePermisoEditar ? `<img onclick="popupFormularioSeccion(${seccion.Ccombcodi})" src="../../Content/images/btn-properties.png" style="cursor:pointer; width:18px;">` : '';

        html = `
                <tr>
                    <td class="htDimmed td_seccion_titulo">${seccion.Numeral}</td>
                    <td class="htDimmed td_seccion_titulo" colspan="5">${seccion.Ccombnombre}</td>
                    <td class="htDimmed td_seccion_titulo" title='${MENSAJE_VENTANA_SECCION}' >${tdhtml_uni}</td>
                </tr>
        `;
    } else {
        html = `
                <tr>
                    <td class="htDimmed td_seccion_titulo" >${seccion.Numeral}</td>
                    <td class="htDimmed td_seccion_titulo" colspan="2">${seccion.Ccombnombre}</td>
                    <td class="htDimmed td_seccion_titulo" colspan="2">${seccion.NombreTotalSoles}</td>
                    <td class="htDimmed td_seccion_titulo" colspan="2">${seccion.NombreTotalDolares}</td>
                </tr>
        `;
    }

    for (var i = 0; i < listaItem.length; i++) {
        var item = listaItem[i];
        var idrow = "row";
        var tdValor = generarHtmlColumnaValorYUnidad(item, item.ItemDato);

        html += `
                <tr id="${idrow}">
                    <td class="htDimmed"></td>
                    <td class="htDimmed td_item_numeral" >${item.Numeral}</td>
                    <td class="htDimmed td_item_nombre">${item.Ccombnombre}</td>
                    ${tdValor}
                </tr >
        `;
    }

    return html;
}

function generarHtmlColumnaValorYUnidad(objCnp, objDato) {
    var tdhtmlFila = '';
    if (!objCnp.EsFilaResultado) {
        //estilo celda 1.05
        var celda105style = '';
        //columna valor
        var tdhtml = '';
        var tdcolor = objCnp.Ccombreadonly == 'y' ? 'td_item_valor_lectura' : '';
        var tdstyle = 'text-align: left;';

        switch (objCnp.Ccombtipo) {
            case 't': //texto
                tdhtml = objDato.Cbevdavalor;
                var arrayDeTextos = tdhtml.split(";");
                if (arrayDeTextos.length >= 3 && objCnp.Ccombnumeral >= 201)
                    tdhtml = "Varios";
                break;
            case 'f': //fecha
                tdhtml = objDato.Cbevdavalor;
                break;
            case 'n':
                tdhtml = formatFloat(objDato.Cbevdavalornumerico, objCnp.Ccombnumdecimal, '.', ' ');
                tdstyle = 'text-align: right;';
                break;
            default:
                tdhtml = item.ItemDato.Cbevdavalor;
                break;
        }


        if (objCnp.Numeral == "1.05") {
            if (tdhtml == '') celda105style = 'margin:0;';
            else celda105style = 'margin: 0; word-break: break-all; text-indent: 0px; padding-left: 68px;';
            tdhtmlFila += `<td class="htDimmed ${tdcolor}" colspan="3" style='${tdstyle}'> <div class="action-message-celda" style='${celda105style}' title='Corresponde a la(s) fecha(s) de recepción(es) de combustible que permitirá la actualización del costo'> ${tdhtml}</td>`;
        }
        else {
            if (objCnp.Numeral == "1.06") tdhtmlFila += `<td class="htDimmed ${tdcolor}" colspan="3" style='${tdstyle}'> <div class="action-message-celda" style="margin:0; padding: 2px;" title='Corresponde al (stock “Final Declarado” día ayer) – (recepción(es) de combustible hasta la fecha más antigua ítem 1.05).'> ${tdhtml}</td>`;
            else {
                tdhtmlFila += `<td class="htDimmed ${tdcolor}" colspan="3" style='${tdstyle}'>${tdhtml}</td>`;
            }
        }

        //columna unidad
        var tdhtml_uni = objDato.Cbevdatipo;
        var tdcolor_uni = 'td_item_unidad_lectura';

        tdhtmlFila += `<td class="htDimmed ${tdcolor_uni}">${tdhtml_uni}</td>`;

    } else {
        //items de resultado
        var tdcolor = 'td_item_valor_lectura';
        var tdstyle1 = 'text-align: right;font-weight: bold;';
        var tdstyle2 = 'text-align: left;';
        var tdhtml1 = formatFloat(objDato.Cbevdavalor, objCnp.Ccombnumdecimal, '.', ' ');
        var tdhtml2 = formatFloat(objDato.Cbevdavalor2, objCnp.Ccombnumdecimal, '.', ' ');

        tdhtmlFila += `<td class="htDimmed ${tdcolor}" style='${tdstyle1}'>${tdhtml1}</td>`;
        tdhtmlFila += `<td class="htDimmed ${tdcolor}" style='${tdstyle2}'>${objDato.Cbevdatipo}</td>`;

        tdhtmlFila += `<td class="htDimmed ${tdcolor}" style='${tdstyle1}'>${tdhtml2}</td>`;
        tdhtmlFila += `<td class="htDimmed ${tdcolor}" style='${tdstyle2}'>${objDato.Cbevdatipo2}</td>`;
    }

    return tdhtmlFila;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Facturas
////////////////////////////////////////////////////////////////////////////////////////////////////////////
function popupFormularioSeccion(concepcodiSeccion) {
    $('#divformSeccion').html('');
    $('#tablaFactura').html('');

    var titulo = obtenerSeccionXcnp(MODELO_WEB.ListaSeccion, concepcodiSeccion).Seccion.Ccombnombre;
    var numeral = obtenerSeccionXcnp(MODELO_WEB.ListaSeccion, concepcodiSeccion).Seccion.Numeral;
    var htmlSeccion = generarHtmlTablaSeccion(concepcodiSeccion);
    var htmlTablaFactura = generarHtmlTablaFactura(concepcodiSeccion);
    var htmlTablaDmrg = "";
    //var htmlTablaDmrg = generarHtmlDemurrage(concepcodiSeccion);
    var htmlTablaMerma = generarHtmlMerma(concepcodiSeccion);

    var htmlDiv = `
            <span class="button b-close"><span>X</span></span>
            <div class="popup-title">
                <span>${numeral} ${titulo}</span>&nbsp;&nbsp;
            </div>

            <div id="idFormSeccion" style="width: 850px;margin-bottom: 15px;">
                ${htmlSeccion}
            </div>
            
            <div id="idFacturas" style="width: 850px;margin-bottom: 15px;">
                ${htmlTablaFactura}
            </div>

            <div id="idDmrg" style="width: 210px;margin-bottom: 15px;">
                ${htmlTablaDmrg}
            </div>

            <div id="idMerma" style="width: 210px;margin-bottom: 15px;">
                ${htmlTablaMerma}
            </div>
        `;

    if (!MODELO_WEB.Readonly) {
        htmlDiv += ` 
            <div style="text-align:right;">
                <input onclick="guardarFormSeccion(${concepcodiSeccion})" type='button' value='Guardar' />
            </div>
        `;
    }

    $('#divformSeccion').html(htmlDiv);
    setEventoFecha();

    setTimeout(function () {
        $('#divformSeccion').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });

        $('#tablaFactura').dataTable({
            "scrollY": 170,
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
            , "oLanguage": { "sZeroRecords": "", "sEmptyTable": "" }
        });

        $('#tablaRecepcion').dataTable({
            "scrollY": 170,
            "scrollX": false,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
            , "oLanguage": { "sZeroRecords": "", "sEmptyTable": "" }
        });

    }, 50);
}

function generarHtmlTablaSeccion(concepcodi) {
    var tipCant = MODELO_WEB.UnidadDesc;
    var seccion = obtenerSeccionXcnp(MODELO_WEB.ListaSeccion, concepcodi).Seccion;

    var cadena = ``;

    //seccion 1.0 BD COES
    if (seccion.Ccombnumeral == 100) {
        cadena += `
                    <div style='clear:both; height:5px'></div> 
                    <input  type="hidden" id="concepcodi_Popup" value="${concepcodi}" />
        `;

        //si el envio se encuentra observado, el agente puede ingresar manualmente el valor de combustible
        var strItem106 = MODELO_WEB.Estcomcodi == TIPO_COMB_LIQUIDO ? "Volumen de combustible en almacén" : "Cantidad de carbón en almacén";
        if (MODELO_WEB.EsEditableItem106) {
            cadena += `
                    <table>
                        <tbody>
                            <tr>
                                <td>${strItem106}:</td>
                                <td><input  type="text" id="txt_volumen_combustible" value="${seccion.CombustibleAlmacenado}" style='width: 95px;' autocomplete="off" /></td>
                            </tr>
                        </tbody>
                    </table>
            `;
        }
        if (!MODELO_WEB.Readonly) {
            cadena += `
                    <div class="popup-title">
                        <span>Agregar fecha</span>&nbsp;&nbsp;
                        <img onclick="agregarFilaRecepcion(${concepcodi})" src="../../Content/Images/btn-add.png" title="Agregar" style="cursor:pointer" />
                    </div>

                `;
        }

        cadena += `<div style='width: auto;'>
                        <table id='tablaRecepcion' class='pretty tabla-adicional' cellspacing='0' style=''>
                            <thead>
                                <tr>
                                    <th>Fecha de Recepción (*)</th>
                                    <th>Recepción (${tipCant}) (**)</th>
                                    <th>Mensaje adicional</th>
                `;
        if (!MODELO_WEB.Readonly) {
            cadena += `
                                    <th style='width:40px'></th>
                `;
        }
        cadena += `             </tr>
                            </thead>
                            <tbody id='tbody_recepcion'>
                `;

        var listaRecepcion = obtenerListaFacturaXItem(concepcodi, seccion.ConcepcodiCombAlmacenado);
        listaRecepcion = listaRecepcion != null && listaRecepcion.length > 0 ? listaRecepcion : []; //por defecto lista vacía

        for (var i = 0; i < listaRecepcion.length; i++) {
            var factura = listaRecepcion[i];

            var idTr = 'tr_' + i;
            cadena += agregarHtmlFilaRecepcion(idTr, factura);
        }

        cadena += `
                            </tbody>
                        </table>
                    </div>

                    <div>Stock Final Declarado (${tipCant}) (**): <input type="text" disabled value="${MODELO_WEB.StockFinalDeclarado}" style='width: 80px; text-align: center;    color: dimgray !important;    background-color: silver !important;'></div>

                    <div style="font-size: 11px; padding-top: 5px;">
                            (*): Corresponde a la(s) fecha(s) de recepción(es) de combustible que permitirá la actualización del costo. <br/>
                            (**): Información del módulo Stock y Consumo de Combustibles.
                    </div>
                `;

    } else {
        cadena = `
            <table>
                <tbody>
            `;
    }


    //otros campos de la seccion
    if (MODELO_WEB.Estcomcodi == TIPO_COMB_LIQUIDO) {
        switch (seccion.Ccombnumeral) {
            case 200:
                cadena += `
                <tr>
                    <td>Proveedor del combustible:</td>
                    <td><input  type="text" id="txt_proveedor" value="${seccion.Proveedor}" autocomplete="off"/></td>
                </tr>
                <tr>
                    <td>Sitio de entrega del proveedor:</td>
                    <td><input  type="text" id="txt_sitio" value="${seccion.Sitio}" autocomplete="off"/></td>
                </tr>
                `;
                break;

            case 300:
                cadena += `
                <tr>
	                <td>Proveedor del transporte: </td>
	                <td><input  type="text" id="txt_proveedor" value="${seccion.Proveedor}" autocomplete="off"/></td>
                </tr>
                <tr>
	                <td>Sitio de carga del combustible: </td>
	                <td><input  type="text" id="txt_sitio" value="${seccion.Sitio}" autocomplete="off"/></td>
                </tr>
                <tr>
	                <td>Sitio de descarga del combustible: </td>
	                <td><input  type="text" id="txt_sitio2" value="${seccion.Sitio2}" autocomplete="off"/></td>
                </tr>
                `;
                break;
        }
    } else {
        switch (seccion.Ccombnumeral) {
            case 200:
                cadena += `
                <tr>
	                <td>Procedencia del carbón: </td>
	                <td><input  type="text" id="txt_procedencia" value="${seccion.Procedencia}" autocomplete="off"/></td>
                </tr>
                <tr>
	                <td>Proveedor del carbón: </td>
	                <td><input  type="text" id="txt_proveedor" value="${seccion.Proveedor}" autocomplete="off"/></td>
                </tr>
                <tr>
	                <td>Sitio de entrega del proveedor:</td>
	                <td><input  type="text" id="txt_sitio" value="${seccion.Sitio}" autocomplete="off"/></td>
                </tr>
                <tr>
	                <td>Poder Calorífico Superior del carbón (kJ/kg): </td>
	                <td><input  type="text" id="txt_pcSup" value="${seccion.PcSup}" class='onlyNumber' onkeypress='return validaNum(event,this.id);' autocomplete="off"/></td>
                </tr>
                <tr>
	                <td>Poder Calorífico Inferior del carbón (kJ/kg): </td>
	                <td><input  type="text" id="txt_pcInf" value="${seccion.PcInf}" class='onlyNumber' onkeypress='return validaNum(event,this.id);' autocomplete="off"/></td>
                </tr>
                `;
                break;

            case 300:
                cadena += `
                <tr>
	                <td>Proveedor del transporte: </td>
	                <td><input  type="text" id="txt_proveedor" value="${seccion.Proveedor}" autocomplete="off"/> </td>
                </tr>
                <tr>
	                <td>Puerto de embarque del combustible: </td>
	                <td><input  type="text" id="txt_puerto" value="${seccion.Puerto}" autocomplete="off"/> </td>
                </tr>
                <tr>
	                <td>Puerto de desembarque en el Perú: </td>
	                <td><input  type="text" id="txt_puerto2" value="${seccion.Puerto2}" autocomplete="off"/> </td>
                </tr>
                `;
                break;

            case 700:
                cadena += `
                <tr>
	                <td>Proveedor del transporte: </td>
	                <td><input  type="text" id="txt_proveedor" value="${seccion.Proveedor}" autocomplete="off"/> </td>
                </tr>
                <tr>
	                <td>Sitio de cargue del carbón: </td>
	                <td><input  type="text" id="txt_sitio" value="${seccion.Sitio}" autocomplete="off"/> </td>
                </tr>
                <tr>
	                <td>Sitio de descargue del combustible: </td>
	                <td><input  type="text" id="txt_sitio2" value="${seccion.Sitio2}" autocomplete="off"/> </td>
                </tr>
                `;
                break;
        }
    }

    cadena += `
            </tbody>
        </table>
    `;

    return cadena;
}

function generarHtmlTablaFactura(concepcodi) {
    var tipCant = MODELO_WEB.UnidadDesc;
    var seccion = obtenerSeccionXcnp(MODELO_WEB.ListaSeccion, concepcodi).Seccion;

    //si es la primera seccion, esta no tiene tabla de facturas

    if (seccion.ConcepcodiFactura <= 0)
        return '';

    var cadena = `
        <div style='clear:both; height:5px'></div> 
        <input  type="hidden" id="concepcodi_Popup" value="${concepcodi}" />

        <div class="popup-title">
            <span>Detalle de Facturas</span>&nbsp;&nbsp;`;

    if (!MODELO_WEB.Readonly) {
        cadena += `
            <img onclick="agregarFilaFactura(${concepcodi})" src="../../Content/Images/btn-add.png" title="Agregar Factura" style="cursor:pointer" />
    `;
    }

    cadena += `
        </div>
    `;

    //tabla de facturas
    cadena += `
        <table id='tablaFactura' class='pretty tabla-adicional' cellspacing='0'>
            <thead>
                <tr>
                    <th style='width:180px'>Comp. Pago</th>
                    <th style='width:100px'>F. Emision</th>
                    <th style='width:20px'>T. Cambio</th>
                    <th style='width:20px'>Moneda</th>
    `;

    if (MODELO_WEB.Estcomcodi == TIPO_COMB_LIQUIDO) {
        switch (seccion.Ccombnumeral) {
            case 200:
            case 300:
                cadena += `
                    <th style='width:90px'>Cantidad (${tipCant})</th>
                    <th style='width:90px'>Costo</th>
                    <th style='width:90px'>Impuesto</th>`;
                break;
            case 400:
                cadena += `
                    <th style='width:90px'>Costo</th>`;
                break;
        }
    } else {
        switch (seccion.Ccombnumeral) {
            case 200:
            case 300:
                cadena += `
                    <th style='width:90px'>Cantidad (${tipCant})</th>
                    <th style='width:90px'>Costo</th>`;
                break;
            case 400: cadena += `
                    <th style='width:90px'>Costo</th>`;
                break;
            case 500: cadena += `
                    <th style='width:90px'>Costo</th>
                    <th style='width:90px'>Impuesto</th>`;
                break;
            case 600: cadena += `
                    <th style='width:90px'>C. Embarque</th>
                    <th style='width:90px'>C. Desembarque</th>`;
                break;
            case 700:
                cadena += `
                    <th style='width:90px'>Cantidad (${tipCant})</th>
                    <th style='width:90px'>Costo</th>
                    <th style='width:90px'>Impuesto</th>`;
                break;
            case 800:
                cadena += `
                    <th style='width:90px'>Costo</th>`;
                break;
        }
    }


    if (!MODELO_WEB.Readonly) {
        cadena += `
                    <th style='width:40px'></th>
    `;
    }

    cadena += `
                </tr>
            </thead>
            <tbody id='tbody_factura'>
    `;

    var listaFactura = obtenerListaFacturaXItem(concepcodi, seccion.ConcepcodiFactura);
    listaFactura = listaFactura != null && listaFactura.length > 0 ? listaFactura : [objDefaultFactura(seccion.Moneda)];

    for (var i = 0; i < listaFactura.length; i++) {
        var factura = listaFactura[i];

        var idTr = 'tr_' + i;
        var htmlTr = generarHtmlFilaFactura(idTr, factura, seccion);
        cadena += htmlTr;
    }

    cadena += `
            </tbody>
        </table>
    `;

    cadena += `<table id="tblEquivalencia" class="pretty tabla-adicional dataTable no-footer" style="width: 50%; margin-bottom: 20px; ">
	            <caption>TABLA DE EQUIVALENCIAS</caption>
	            <thead>
	            <tr>
		            <th>N</th>
		            <th>Dimensión</th>
		            <th>Unidad</th>
		            <th>Equivalencia</th>
		            <th>Unidad</th>
	            </tr>
	            </thead>
	            <tbody>
	            <tr>
		            <td>1</td>
		            <td rowspan="3">Volumen</td>
		            <td>gal</td>
		            <td>3.785412</td>
		            <td>l</td>
	            </tr>
	            <tr>
		            <td>2</td>
		            <td>pie³</td>
		            <td>0.02831685</td>
		            <td>m³</td>
	            </tr>
	            <tr>
		            <td>3</td>
		            <td>bbl</td>
		            <td>0.1589873</td>
		            <td>m³</td>
	            </tr>
	            <tr>
		            <td>4</td>
		            <td>Masa</td>
		            <td>lb</td>
		            <td>0.45359237</td>
		            <td>kg</td>
	            </tr>
	            <tr>
		            <td>5</td>
		            <td rowspan="2">Energia</td>
		            <td>BTU</td>
		            <td>1.05506</td>
		            <td>kJ</td>
	            </tr>
	            <tr>
		            <td>6</td>
		            <td>kcal</td>
		            <td>4.1868</td>
		            <td>kJ</td>
	            </tr>
	            </tbody>
            </table>`;


    return cadena;
}

function generarHtmlDemurrage(concepcodi) {
    var seccion = obtenerSeccionXcnp(MODELO_WEB.ListaSeccion, concepcodi).Seccion;

    if (seccion.ConcepcodiDemurrage <= 0)
        return '';

    var listaDmr = obtenerListaFacturaXItem(concepcodi, seccion.ConcepcodiDemurrage);
    listaDmr = listaDmr != null && listaDmr.length > 0 ? listaDmr : [objDefaultDemurrage(seccion.Moneda)];

    var objDmr = listaDmr[0];

    var selectedSoles = objDmr.Cbdetmoneda == MONEDA_SOLES ? " selected" : "";
    var selectedDolar = objDmr.Cbdetmoneda == MONEDA_DOLAR ? " selected" : "";

    var cadena = `
        <div style='clear:both; height:5px'></div> 

        <div class="popup-title">
            <span>Pago por demurrage</span>&nbsp;&nbsp;
        </div>
    `;

    cadena += `
        <table id='tablaDmr' class='pretty tabla-adicional' cellspacing='0'>
            <thead>
                <tr>
                    <th style='width:20px'>Moneda</th>
                    <th style='width:90px'>Costo</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>
                        <select id='dmrg_i_moneda' style='width: 80px' >
                            <option value='S' ${selectedSoles}>Soles</option>
                            <option value='D' ${selectedDolar}>Dólares</option>
                        </select>
                    </td>
                    <td>
                        <input type='text' id='dmrg_i_costo' value="${objDmr.Cbdetcosto}" style='width:80px; text-align: right; padding-right: 5px;' maxlength='10' 
                                                            onkeypress='return validaNum(event,this.id);' autocomplete="off" />
                    </td>
                </tr>
            </tbody>
        </table>
    `;


    return cadena;
}

function generarHtmlMerma(concepcodi) {
    var seccion = obtenerSeccionXcnp(MODELO_WEB.ListaSeccion, concepcodi).Seccion;

    if (seccion.ConcepcodiMerma <= 0)
        return '';

    var listaMerma = obtenerListaFacturaXItem(concepcodi, seccion.ConcepcodiMerma);
    listaMerma = listaMerma != null && listaMerma.length > 0 ? listaMerma : [objDefaultMerma(seccion.Moneda)];

    var objMerma = listaMerma[0];

    var selectedSoles = objMerma.Cbdetmoneda == MONEDA_SOLES ? " selected" : "";
    var selectedDolar = objMerma.Cbdetmoneda == MONEDA_DOLAR ? " selected" : "";

    var solXUnidad = MODELO_WEB.Estcomcodi == TIPO_COMB_LIQUIDO ? 'S//l' : 'S//kg';
    var dolarXUnidad = MODELO_WEB.Estcomcodi == TIPO_COMB_LIQUIDO ? 'USD/l' : 'USD/kg';

    var cadena = `
        <div style='clear:both; height:5px'></div> 

        <div class="popup-title">
            <span>Costos por Mermas</span>&nbsp;&nbsp;
        </div>
    `;

    cadena += `
        <table id='tablaMerma' class='pretty tabla-adicional' cellspacing='0'>
            <thead>
                <tr>
                    <th style='width:20px'>Moneda//Unidad</th>
                    <th style='width:90px'>Costo</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>
                        <select id='merma_i_moneda' style='width: 80px' >
                            <option value='S' ${selectedSoles}>${solXUnidad}</option>
                            <option value='D' ${selectedDolar}>${dolarXUnidad}</option>
                        </select>
                    </td>
                    <td>
                        <input type='text' id='merma_i_costo' value="${objMerma.Cbdetcosto}" style='width:80px; text-align: right; padding-right: 5px;' maxlength='10' 
                                                            onkeypress='return validaNum(event,this.id);' autocomplete="off" />
                    </td>
                </tr>
            </tbody>
        </table>
    `;


    return cadena;
}

function agregarFilaFactura(concepcodiSeccion) {
    var seccion = obtenerSeccionXcnp(MODELO_WEB.ListaSeccion, concepcodiSeccion).Seccion;

    var factura = objDefaultFactura(seccion.Moneda);

    var idTr = 'tr_' + Date.now();
    var htmlTr = generarHtmlFilaFactura(idTr, factura, seccion);

    $("#tbody_factura").append(htmlTr);
    setEventoFecha();
}

function agregarFilaRecepcion() {
    var factura = objDefaultVolCombustible();

    var idTr = 'tr_' + Date.now();
    var htmlTr = agregarHtmlFilaRecepcion(idTr, factura);

    $("#tbody_recepcion").append(htmlTr);
    setEventoFecha();

    getVolumenRecepcion(factura.CbdetfechaemisionDesc, idTr);
}

function generarHtmlFilaFactura(idTr, factura, seccion) {
    var selectedSoles = factura.Cbdetmoneda == MONEDA_SOLES ? " selected" : "";
    var selectedDolar = factura.Cbdetmoneda == MONEDA_DOLAR ? " selected" : "";


    //Comp. Pago
    //F. Emision
    //T. Cambio
    //Moneda
    var title = "Valor venta de la SBS, según fecha de emisión del comprobante de pago. Si la moneda del comprobante de pago es igual al formato principal, no es necesario completar";
    var visibilityTc = factura.Cbdetmoneda == seccion.Moneda ? 'hidden' : 'visible';
    if (seccion.EsObligatorioTC) visibilityTc = 'visible';

    var cadena = `
                <tr id='${idTr}'>
                    <td>
                        <input type='text' name='i_factura' value="${factura.Cbdetcompago}" style='width:180px; margin-top: 3px; margin-bottom: 3px;' maxlength='40' 
                                        onkeypress='return validaTexto(event,this.id);' onkeydown='return validaTexto(event,this.id);' autocomplete="off"/>
                    </td>
                    <td>
                        <input type='text' id='i_fecha_${idTr}' name='i_fecha' class='i_fecha' value="${factura.CbdetfechaemisionDesc}" style='width:90px' />
                    </td>
                    <td style='visibility: ${visibilityTc};'>
                        <div class="action-message-celda" style="margin:0; padding: 2px;" title='${title}'>
                        <input  title='${title}' type='text' class='onlyNumber' id='i_tipocambio_${idTr}' name='i_tipocambio' VALUE="${factura.Cbdettipocambio}" style='width:80px; text-align: center;' maxlength='10' 
                                        onkeypress='return validaNum(event,this.id);' autocomplete="off"/>
                        </div>
                    </td>
                    <td>
                        <select name='i_moneda' style='width: 80px' onchange='monedaTipoCambio(this, "${seccion.Moneda}", "${seccion.EsObligatorioTC}")' >
                            <option data-moneda='${MONEDA_SOLES}' value='S' ${selectedSoles}>Soles</option>
                            <option data-moneda='${MONEDA_DOLAR}' value='D' ${selectedDolar}>Dólares</option>
                        </select>
                    </td>
        `;

    if (MODELO_WEB.Estcomcodi == TIPO_COMB_LIQUIDO) {
        switch (seccion.Ccombnumeral) {

            //Cantidad ()
            //Costo
            //Impuesto
            case 200:
            case 300: cadena += `
                    <td>
                        <input type='text' id='i_cantidad_${idTr}' name='i_cantidad' value="${factura.Cbdetvolumen}" style='width:80px; text-align: right; padding-right: 5px;' maxlength='10'
                                        onkeypress='return validaNum(event,this.id);' class='onlyNumber' autocomplete="off"/>
                    </td>
                    <td>
                        <input type='text' id='i_costo_${idTr}' name='i_costo' value="${factura.Cbdetcosto}" style='width:80px; text-align: right; padding-right: 5px;' maxlength='10'
                                        onkeypress='return validaNum(event,this.id);' class='onlyNumber' autocomplete="off"/>
                    </td>
                    <td>
                        <input type='text' id='i_impuesto_${idTr}' name='i_impuesto' value="${factura.Cbdetimpuesto}" style='width:80px; text-align: right; padding-right: 5px;' maxlength='10'
                                        onkeypress='return validaNum(event,this.id);' class='onlyNumber' autocomplete="off"/>
                    </td>`;
                break;

            //Costo
            case 400: cadena += `
                    <td>
                        <input type='text' id='i_costo_${idTr}' name='i_costo' value="${factura.Cbdetcosto}" style='width:80px; text-align: right; padding-right: 5px;' maxlength='10'
                                        onkeypress='return validaNum(event,this.id);' class='onlyNumber' autocomplete="off"/>
                    </td>`;
                break;
        }
    } else {
        switch (seccion.Ccombnumeral) {
            //Cantidad ()
            //Costo
            case 200:
            case 300:
                cadena += `
                    <td>
                        <input type='text' id='i_cantidad_${idTr}' name='i_cantidad' value="${factura.Cbdetvolumen}" style='width:80px; text-align: right; padding-right: 5px;' maxlength='10'
                                        onkeypress='return validaNum(event,this.id);' class='onlyNumber' autocomplete="off"/>
                    </td>
                    <td>
                        <input type='text' id='i_costo_${idTr}' name='i_costo' value="${factura.Cbdetcosto}" style='width:80px; text-align: right; padding-right: 5px;' maxlength='10'
                                        onkeypress='return validaNum(event,this.id);' class='onlyNumber' autocomplete="off"/>
                    </td>`;
                break;
            //Costo
            case 400: cadena += `
                    <td>
                        <input type='text' id='i_costo_${idTr}' name='i_costo' value="${factura.Cbdetcosto}" style='width:80px; text-align: right; padding-right: 5px;' maxlength='10'
                                        onkeypress='return validaNum(event,this.id);' class='onlyNumber' autocomplete="off"/>
                    </td>`;
                break;
            //Costo
            //Impuesto
            case 500: cadena += `
                    <td>
                        <input type='text' id='i_costo_${idTr}' name='i_costo' value="${factura.Cbdetcosto}" style='width:80px; text-align: right; padding-right: 5px;' maxlength='10'
                                        onkeypress='return validaNum(event,this.id);' class='onlyNumber' autocomplete="off"/>
                    </td>
                    <td>
                        <input type='text' id='i_impuesto_${idTr}' name='i_impuesto' value="${factura.Cbdetimpuesto}" style='width:80px; text-align: right; padding-right: 5px;' maxlength='10'
                                        onkeypress='return validaNum(event,this.id);' class='onlyNumber' autocomplete="off"/>
                    </td>`;
                break;
            //C. Embarque
            //C. Desembarque
            case 600: cadena += `
                    <td>
                        <input type='text' id='i_costo_${idTr}' name='i_costo' value="${factura.Cbdetcosto}" style='width:80px; text-align: right; padding-right: 5px;' maxlength='10'
                                        onkeypress='return validaNum(event,this.id);' class='onlyNumber' autocomplete="off"/>
                    </td>
                    <td>
                        <input type='text' id='i_costo2_${idTr}' name='i_costo2' value="${factura.Cbdetcosto2}" style='width:80px; text-align: right; padding-right: 5px;' maxlength='10'
                                        onkeypress='return validaNum(event,this.id);' class='onlyNumber' autocomplete="off"/>
                    </td>`;
                break;
            //Cantidad ()
            //Costo
            //Impuesto
            case 700:
                cadena += `
                    <td>
                        <input type='text' id='i_cantidad_${idTr}' name='i_cantidad' value="${factura.Cbdetvolumen}" style='width:80px; text-align: right; padding-right: 5px;' maxlength='10'
                                        onkeypress='return validaNum(event,this.id);' class='onlyNumber' autocomplete="off"/>
                    </td>
                    <td>
                        <input type='text' id='i_costo_${idTr}' name='i_costo' value="${factura.Cbdetcosto}" style='width:80px; text-align: right; padding-right: 5px;' maxlength='10'
                                        onkeypress='return validaNum(event,this.id);' class='onlyNumber' autocomplete="off"/>
                    </td>
                    <td>
                        <input type='text' id='i_impuesto_${idTr}' name='i_impuesto' value="${factura.Cbdetimpuesto}" style='width:80px; text-align: right; padding-right: 5px;' maxlength='10'
                                        onkeypress='return validaNum(event,this.id);' class='onlyNumber' autocomplete="off"/>
                    </td>`;
                break;
            //Costo
            case 800:
                cadena += `
                    <td>
                        <input type='text' id='i_costo_${idTr}' name='i_costo' value="${factura.Cbdetcosto}" style='width:80px; text-align: right; padding-right: 5px;' maxlength='10'
                                        onkeypress='return validaNum(event,this.id);' class='onlyNumber' autocomplete="off"/>
                    </td>`;
                break;
        }
    }

    if (!MODELO_WEB.Readonly) {
        cadena += `  
                    <td>
                        <a href='#' onclick="quitarFilaFactura('${idTr}')"><img src='../../Content/Images/btn-cancel.png' /></a>
                    </td>
    `;

    }

    cadena += `  
                </tr>
    `;

    return cadena;
}

function agregarHtmlFilaRecepcion(idTr, factura) {
    var cadena = `
                <tr id='${idTr}'>
                    <td>
                        <input type='text' id='i_fecha_${idTr}' name='i_fecha_vol' class='i_fecha_vol' value="${factura.CbdetfechaemisionDesc}" style='width:90px' />
                    </td>
                    <td>
                        <input type='text' class='onlyNumber' id='i_volumen_${idTr}' name='i_volumen' VALUE="${factura.Cbdetvolumen}" style='width:80px; text-align: center;color: dimgray !important;background-color: silver !important;' maxlength='10' onkeypress='return validaNum(event,this.id);' disabled/>
                    </td>
                    <td>
                        <input type="hidden" value="${factura.MensajeValidacion}" id='i_msj_val_${idTr}' name='i_msj_val' />
                        <span id="html_val_recep_${idTr}" style='white-space: break-spaces;'>${factura.MensajeValidacion}</span>
                    </td>
    `;

    if (!MODELO_WEB.Readonly) {
        cadena += `
                    <td>
                        <a href='#' onclick="quitarFilaFactura('${idTr}')"><img src='../../Content/Images/btn-cancel.png' /></a>
                    </td>
        `;
    }

    cadena += `
                </tr>
    `;

    return cadena;
}

function quitarFilaFactura(idTr) {
    $("#" + idTr).remove();
}

function setEventoFecha() {

    //fechas de cada fila factura
    $('.i_fecha').unbind();
    $('.i_fecha').Zebra_DatePicker({
        onChange: function (view, elements) {
        },
        onSelect: function (date) {
        }
    });


    $('.i_fecha_vol').unbind();
    $('.i_fecha_vol').Zebra_DatePicker({
        direction: [MODELO_WEB.RangoIniValidoRecepcion, MODELO_WEB.RangoFinValidoRecepcion],
        onChange: function (view, elements) {
        },
        onSelect: function (date) {
            var id_fecha = $(this).attr('id');
            var sufijo = id_fecha.substring(8, id_fecha.length);

            getVolumenRecepcion(date, sufijo);
        }
    });
}

/////////////////////////////////////////////
//Guardar y validaciones
function guardarFormSeccion(concepcodiSeccion) {

    var seccion = obtenerSeccionXcnp(MODELO_WEB.ListaSeccion, concepcodiSeccion).Seccion;

    //
    var objCab = generarObjSeccion();
    var listaF = generarListaFacturaFromDiv();
    var listaRecepcion = generarListaRecepcionFromDiv();
    //var listaDmr = generarListaDmr();
    var listaMerma = generarListaMerma();

    //
    var msjVal1 = validarObjSeccion(objCab, seccion.Ccombnumeral);
    var msjVal2 = validarListaFactura(listaF, seccion.ConcepcodiFactura, seccion.Ccombnumeral, seccion.OpcionalFactura, seccion.Moneda, seccion.EsObligatorioTC);
    var msjVal3 = "";
    //var msjVal3 = validarListaDmr(listaDmr, seccion.ConcepcodiDemurrage, seccion.OpcionalDemurrage);
    var msjVal4 = validarListaMerma(listaMerma, seccion.ConcepcodiMerma, seccion.OpcionalMerma);
    var msjVal5 = validarListaRecepcion(listaRecepcion, seccion.ConcepcodiCombAlmacenado, seccion.Ccombnumeral);

    //caso especial de mermas
    //si solo existe una fila en pantalla (por defecto) y no tiene datos entonces es válido
    if (listaF.length == 1 && msjVal2 != '') {
        if (seccion.OpcionalFactura) {
            listaF = [];
            msjVal2 = '';
        }
    }

    if (msjVal1 == '' && msjVal2 == '' && msjVal3 == '' && msjVal4 == '' && msjVal5 == '') {
        //actualizar valores de seccion
        seccion.FechaIngreso = objCab.FechaIngreso;
        seccion.CombustibleAlmacenado = objCab.CombustibleAlmacenado;
        seccion.Procedencia = objCab.Procedencia;
        seccion.Proveedor = objCab.Proveedor;
        seccion.Sitio = objCab.Sitio;
        seccion.Sitio2 = objCab.Sitio2;
        seccion.Puerto = objCab.Puerto;
        seccion.Puerto2 = objCab.Puerto2;
        seccion.PcSup = objCab.PcSup;
        seccion.PcInf = objCab.PcInf;

        //
        if (seccion.ConcepcodiFactura > 0) {
            var itemDato = obtenerItemDato(concepcodiSeccion, seccion.ConcepcodiFactura);
            itemDato.ListaDetalle = listaF;
        }

        if (seccion.ConcepcodiCombAlmacenado > 0) {
            var itemDato = obtenerItemDato(concepcodiSeccion, seccion.ConcepcodiCombAlmacenado);
            itemDato.ListaDetalle = listaRecepcion;
        }
        //
        if (seccion.ConcepcodiDemurrage > 0) {
            var itemDato = obtenerItemDato(concepcodiSeccion, seccion.ConcepcodiDemurrage);
            itemDato.ListaDetalle = listaDmr;
        }
        //
        if (seccion.ConcepcodiMerma > 0) {
            var itemDato = obtenerItemDato(concepcodiSeccion, seccion.ConcepcodiMerma);
            itemDato.ListaDetalle = listaMerma;
        }

        //actualizar handson
        actualizarExcelWeb();

        $('#divformSeccion').bPopup().close();
    } else {
        if (msjVal1 != '') {
            alert(msjVal1);
        }
        if (msjVal2 != '') {
            alert(msjVal2);
        }
        if (msjVal3 != '') {
            alert(msjVal3);
        }
        if (msjVal4 != '') {
            alert(msjVal4);
        }
        if (msjVal5 != '') {
            alert(msjVal5);
        }
    }
}

function generarObjSeccion() {
    //var fechaIngreso = $("#txt_fecha_ingreso").val();
    var volumenComb = parseFloat($("#txt_volumen_combustible").val()) || 0;

    var proc = $("#txt_procedencia").val();
    var prov = $("#txt_proveedor").val();
    var sitio = $("#txt_sitio").val();
    var sitio2 = $("#txt_sitio2").val();
    var puerto = $("#txt_puerto").val();
    var puerto2 = $("#txt_puerto2").val();
    var pcSup = parseFloat($("#txt_pcSup").val()) || 0;
    var pcInf = parseFloat($("#txt_pcInf").val()) || 0;

    var obj = {
        //FechaIngreso: fechaIngreso,
        CombustibleAlmacenado: volumenComb,
        Procedencia: proc,
        Proveedor: prov,
        Sitio: sitio,
        Sitio2: sitio2,
        Puerto: puerto,
        Puerto2: puerto2,
        PcSup: pcSup,
        PcInf: pcInf,
    };

    return obj;
}

function validarObjSeccion(obj, numeral) {
    var msj = '';

    if (MODELO_WEB.Estcomcodi == TIPO_COMB_LIQUIDO) {
        if (numeral == 100 && MODELO_WEB.EsEditableItem106) {
            //si el envio se encuentra observado, el agente puede ingresar manualmente el valor de combustible
            if (obj.CombustibleAlmacenado < 0)
                msj += "Debe ingresar volumen de combustible. ";
        }

        if (numeral == 200) {
            if (!esValidoString(obj.Proveedor))
                msj += "Debe ingresar proveedor del combustible. ";

            if (!esValidoString(obj.Sitio))
                msj += "Debe ingresar sitio de entrega. ";
        }

    } else {
        if (numeral == 100 && MODELO_WEB.EsEditableItem106) {
            //si el envio se encuentra observado, el agente puede ingresar manualmente el valor de combustible
            if (obj.CombustibleAlmacenado < 0)
                msj += "Debe ingresar cantidad de carbón. ";
        }

        if (numeral == 200) {
            if (!esValidoString(obj.Procedencia))
                msj += "Debe ingresar proveedor. ";

            if (!esValidoString(obj.Proveedor))
                msj += "Debe ingresar proveedor. ";

            if (!esValidoString(obj.Sitio))
                msj += "Debe ingresar sitio. ";
        }

    }

    return msj;
}

function generarListaFacturaFromDiv() {
    var listaFact = [];

    $('#tbody_factura').find('tr').each(function () {
        var idtmp = $(this).get()[0].id;
        if (idtmp != '') {
            var idFila = " #" + idtmp;
            var id_fila_i_factura = idFila + " " + 'input[name=i_factura]';
            var id_fila_i_fecha = idFila + " " + 'input[name=i_fecha]';
            var id_fila_i_tipocambio = idFila + " " + 'input[name=i_tipocambio]';
            var id_fila_i_moneda = idFila + " " + 'select[name=i_moneda]';
            var id_fila_i_cantidad = idFila + " " + 'input[name=i_cantidad]';
            var id_fila_i_costo = idFila + " " + 'input[name=i_costo]';
            var id_fila_i_costo2 = idFila + " " + 'input[name=i_costo2]';
            var id_fila_i_impuesto = idFila + " " + 'input[name=i_impuesto]';

            var i_factura = $(id_fila_i_factura).val();
            var i_fecha = $(id_fila_i_fecha).val();
            var i_tipocambio = parseFloat($(id_fila_i_tipocambio).val()) || 0;
            var i_moneda = $(id_fila_i_moneda).val();
            var i_moneda_simbolo = $('option:selected', id_fila_i_moneda).data('moneda');
            var i_cantidad = parseFloat($(id_fila_i_cantidad).val()) || 0;
            var i_costo = parseFloat($(id_fila_i_costo).val()) || 0;
            var i_costo2 = parseFloat($(id_fila_i_costo2).val()) || 0;
            var i_impuesto = parseFloat($(id_fila_i_impuesto).val()) || 0;

            var factura = {
                Cbdetcompago: i_factura,
                Cbdettipo: TIPO_DETALLE_FACTURA,
                CbdetfechaemisionDesc: i_fecha,
                Cbdettipocambio: i_tipocambio,
                Cbdetmoneda: i_moneda,
                Cbsimbmoneda: i_moneda_simbolo,
                Cbdetvolumen: i_cantidad,
                Cbdetcosto: i_costo,
                Cbdetcosto2: i_costo2,
                Cbdetimpuesto: i_impuesto,
            };

            listaFact.push(factura);
        }
    });

    return listaFact;
}

function generarListaRecepcionFromDiv() {

    var listaFact = [];

    $('#tbody_recepcion').find('tr').each(function () {
        var idtmp = $(this).get()[0].id;
        if (idtmp != '') {
            var idFila = " #" + idtmp;

            var id_fila_i_fecha = idFila + " " + 'input[name=i_fecha_vol]';
            var id_fila_i_cantidad = idFila + " " + 'input[name=i_volumen]';
            var id_fila_i_msj = idFila + " " + 'input[name=i_msj_val]';

            var i_fecha = $(id_fila_i_fecha).val();
            var i_cantidad = parseFloat($(id_fila_i_cantidad).val()) || 0;
            var msj = $(id_fila_i_msj).val();
            if (msj == null) msj = "";

            var factura = {
                CbdetfechaemisionDesc: i_fecha,
                Cbdetvolumen: i_cantidad,
                MensajeValidacion: msj
            };

            listaFact.push(factura);
        }
    });

    return listaFact;
}

function validarListaFactura(listaF, concepcodiFactura, numeral, esOpcional, monedaSeccion, esObligatorioTC) {
    var msj = '';

    if (concepcodiFactura > 0) {
        if (listaF.length == 0) {
            if (!esOpcional)
                msj = "Debe ingresar como mínimo una factura.";
        }

        var arrayComprobante = [];

        for (var i = 0; i < listaF.length; i++) {
            var msjXFila = esValidoFactura(listaF[i], numeral, monedaSeccion, esObligatorioTC);
            if (msjXFila != '')
                msj += "Fila " + (i + 1) + ": " + msjXFila + "\n";
            arrayComprobante.push(listaF[i].Cbdetcompago.toUpperCase());
        }

        if (_val_ExisteStringDuplicado(arrayComprobante))
            msj += "Existe Comprobantes de pago repetidos. Debe quitar los elementos duplicados.";
    }

    return msj;
}

function validarListaRecepcion(listaRecepcion, concepcodiFactura, numeral) {
    var msj = '';

    if (concepcodiFactura > 0) {
        if (listaRecepcion.length == 0) {
            msj = "Debe ingresar como mínimo una Fecha de recepción.";
        } else {
            var arrayFecha = [];

            for (var i = 0; i < listaRecepcion.length; i++) {
                var msjXFila = esValidoRecepcion(listaRecepcion[i], numeral);
                if (msjXFila != '')
                    msj += "Fila " + (i + 1) + ": " + msjXFila + "\n";
                else
                    arrayFecha.push(listaRecepcion[i].CbdetfechaemisionDesc);
            }

            if (_val_ExisteStringDuplicado(arrayFecha))
                msj += "Existe Fechas de recepción repetidas. Debe quitar las fechas duplicadas.";
        }
    }

    return msj;
}

function esValidoRecepcion(obj, numeral) {
    var msj = '';
    if (MODELO_WEB.Estcomcodi == TIPO_COMB_LIQUIDO) {

        if (!esValidoString(obj.CbdetfechaemisionDesc))
            msj += "Debe seleccionar fecha de emisión. ";

        if (esValidoString(obj.MensajeValidacion))
            msj += obj.MensajeValidacion;
    }
    return msj;
}

function _val_ExisteStringDuplicado(strArray) {
    var alreadySeen = [];
    var existeDupl = false;

    strArray.forEach(function (str) {
        if (alreadySeen[str])
            existeDupl = true;
        else
            alreadySeen[str] = true;
    });

    return existeDupl;
}

function esValidoFactura(obj, numeral, monedaSeccion, esObligatorioTC) {
    var msj = '';

    if (!esValidoString(obj.Cbdetcompago))
        msj += "Debe ingresar comprobante de pago. ";

    if (!esValidoString(obj.CbdetfechaemisionDesc))
        msj += "Debe seleccionar fecha de emisión. ";

    if (obj.Cbdettipocambio <= 0 && (esObligatorioTC || obj.Cbsimbmoneda != monedaSeccion))
        msj += "Debe ingresar tipo de cambio. ";

    if (MODELO_WEB.Estcomcodi == TIPO_COMB_LIQUIDO) {
        if (!esValidoString(obj.Cbdetmoneda))
            msj += "Debe seleccionar moneda. ";

        if (obj.Cbdetcosto <= 0)
            msj += "Debe ingresar costo. ";

        if (numeral == 200 || numeral == 300) {
            if (obj.Cbdetvolumen <= 0)
                msj += "Debe ingresar cantidad. ";

            //if (obj.Cbdetimpuesto <= 0)
            //    msj += "Debe ingresar impuesto. ";
        }
    } else {
        if (numeral == 200 || numeral == 300) {
            if (obj.Cbdetvolumen <= 0)
                msj += "Debe ingresar cantidad. ";
            if (obj.Cbdetcosto <= 0)
                msj += "Debe ingresar costo. ";
        }
        if (numeral == 400) {
            if (obj.Cbdetcosto <= 0)
                msj += "Debe ingresar costo. ";
        }
        if (numeral == 500) {
            if (obj.Cbdetcosto <= 0)
                msj += "Debe ingresar costo. ";
            //if (obj.Cbdetimpuesto <= 0)
            //    msj += "Debe ingresar impuesto. ";
        }
        if (numeral == 600) {
            if (obj.Cbdetcosto <= 0 && obj.Cbdetcosto2 <= 0)
                msj += "Debe ingresar C. Embarque o C. Desembarque.";
            if (obj.Cbdetcosto > 0 && obj.Cbdetcosto2 > 0)
                msj += "Debe ingresar C. Embarque o C. Desembarque, no ambos.";
        }
        if (numeral == 700) {
            if (obj.Cbdetvolumen <= 0)
                msj += "Debe ingresar cantidad. ";
            if (obj.Cbdetcosto <= 0)
                msj += "Debe ingresar costo. ";
            //if (obj.Cbdetimpuesto <= 0)
            //    msj += "Debe ingresar impuesto. ";
        }
        if (numeral == 800) {
            if (obj.Cbdetcosto <= 0)
                msj += "Debe ingresar costo. ";
        }
    }

    return msj;
}

function generarListaDmr() {
    var lista = [];

    var idFila = "#";
    var id_fila_i_moneda = idFila + "" + 'dmrg_i_moneda';
    var id_fila_i_costo = idFila + "" + 'dmrg_i_costo';

    var i_moneda = $(id_fila_i_moneda).val();
    var i_costo = parseFloat($(id_fila_i_costo).val()) || 0;

    var obj = {
        Cbdettipo: TIPO_DETALLE_DEMURRAGE,
        Cbdetmoneda: i_moneda,
        Cbdetcosto: i_costo,
    };

    lista.push(obj);

    return lista;
}

function validarListaDmr(listaF, concepcodiDmr, esOpcional) {
    var msj = '';

    if (concepcodiDmr > 0) {
        if (listaF.length == 0) {
            msj = "Debe ingresar demurrage.";
        } else {

            var obj = listaF[0];

            if (!esValidoString(obj.Cbdetmoneda))
                msj += "Debe seleccionar moneda de demurrage. ";

            if (!esOpcional && obj.Cbdetcosto <= 0)
                msj += "Debe ingresar costo de demurrage. ";
        }
    }

    return msj;
}


function generarListaMerma() {
    var lista = [];

    var idFila = "#";
    var id_fila_i_moneda = idFila + "" + 'merma_i_moneda';
    var id_fila_i_costo = idFila + "" + 'merma_i_costo';

    var i_moneda = $(id_fila_i_moneda).val();
    var i_costo = parseFloat($(id_fila_i_costo).val()) || 0;

    var obj = {
        Cbdettipo: TIPO_DETALLE_MERMA,
        Cbdetmoneda: i_moneda,
        Cbdetcosto: i_costo,
    };

    lista.push(obj);

    return lista;
}

function validarListaMerma(listaF, concepcodiMerma, esOpcional) {
    var msj = '';

    if (concepcodiMerma > 0) {
        if (listaF.length == 0) {
            msj = "Debe ingresar merma.";
        } else {

            var obj = listaF[0];

            if (!esValidoString(obj.Cbdetmoneda))
                msj += "Debe seleccionar moneda de costo por merma. ";

            if (!esOpcional && obj.Cbdetcosto <= 0)
                msj += "Debe ingresar costo por merma.";
        }
    }

    return msj;
}

function monedaTipoCambio(data, monedaSeccion, esObligatorioTC) {
    var val = data.value;
    if (data.value == monedaSeccion) {
        data.parentElement.previousElementSibling.style.visibility = 'hidden';
    } else {
        data.parentElement.previousElementSibling.style.visibility = 'visible';
    }

    if (esObligatorioTC == 'true') {
        data.parentElement.previousElementSibling.style.visibility = 'visible';
    }
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

function dibujarTablaError() {
    var cadena = `
        <div style='clear:both; height:5px'></div>
            <table id='tablaError' border='1' class='pretty tabla-adicional' cellspacing='0' style='width: 470px'>
                <thead>
                    <tr>
                        <th>Item</th>
                        <th>Valor</th>
                        <th>Tipo Error</th>
                    </tr>
                </thead>
                <tbody>
    `;

    var len = MODELO_WEB.ListaErrores.length;
    for (var i = 0; i < len; i++) {
        var item = MODELO_WEB.ListaErrores[i];
        cadena += `
                    <tr>
                        <td>${item.Celda}</td>
                        <td>${item.Valor}</td>
                        <td>${item.Mensaje}</td>
                    </tr>
        `;
    }

    var seccionAcrchivos2 = MODELO_WEB.ListaSeccionDocumento[0];
    var archivosSec2 = seccionAcrchivos2.ListaArchivo;
    numArchivosS2 = archivosSec2.length;
    if (numArchivosS2 == 0) {
        cadena += `
                    <tr>
                        <td>2</td>
                        <td>Sin archivo</td>
                        <td>No se adjunto comprobantes en la sección 2</td>
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


////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Log
////////////////////////////////////////////////////////////////////////////////////////////////////////////
function cargarHtmlLogXEnvio() {
    if (EVT_GLOBAL.HtmlLogEnvio != null && EVT_GLOBAL.HtmlLogEnvio != '') {

        $("#log_envio").html(EVT_GLOBAL.HtmlLogEnvio);
        $("#log_envio").show();
    }
}

function cargarHtmlCostosVariables() {

    if (EVT_GLOBAL.HtmlListaEnvioRelCv != null && EVT_GLOBAL.HtmlListaEnvioRelCv != '') {

        $("#cv_envio").html(EVT_GLOBAL.HtmlListaEnvioRelCv);
        $("#cv_envio").show();

    }
}