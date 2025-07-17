var controlador = siteRoot + 'Equipamiento/FTAdministrador/';
const FORMATO_OPERACION_COMERCIAL = 2;
const FORMATO_DAR_BAJA = 3;

$(function () {
    FECHA_SISTEMA = $("#hdFechaSistema").val();

    HEIGHT_FORMULARIO = $(window).height() - 270;

    //ocultar barra lateral izquierda
    $("#btnOcultarMenu").click();

    WIDTH_FORMULARIO = $(window).width() - 85;

    var tipoFormato = parseInt($("#hfTipoFormato").val()) || 0;
    if (tipoFormato == TIPO_FORMATO_DARBAJA) HEIGHT_FORMULARIO = 360;

    $('#btnInicio').click(function () {
        regresarPrincipal();
    });

    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').easytabs('select', '#tabLista');

    //guardar manual
    $('#btnAutoguardar').click(function () {
        guardarEquipoTemporalExcelWeb();
    });

    //tabla web
    OPCION_GLOBAL_EDITAR = $("#hfTipoOpcion").val() == "E";
    CODIGO_ENVIO = getObjetoFiltro().codigoEnvio;
    CODIGO_VERSION = getObjetoFiltro().codigoVersion;

    //Muestra fec max rpta
    var infoFMR = $("#hdNotaFecMaxRpta").val();
    if (infoFMR != "" && infoFMR != undefined && infoFMR != null) {
        var carpeta = parseInt($("#hfIdEstado").val());
        if (carpeta == ESTADO_SUBSANADO) {
            $("#idFecMaxRpta").html(`<b>Plazo Máximo de Revisión de la Subsanación de Observaciones:</b>  ${infoFMR} (Fecha ingresada al momento de la observación por parte de COES)`);
            $("#idFecMaxRpta").show();
        }
        if (carpeta == ESTADO_OBSERVADO) {
            $("#idFecMaxRpta").html(`<b>Plazo Máximo de Respuesta:</b>  ${infoFMR}`);
            $("#idFecMaxRpta").show();
        }
    }

    //Muestra info adicional : Mensaje Cancelacion
    var infoAdicional = $("#hdNotaCancelacion").val();
    if (infoAdicional != "" && infoAdicional != undefined && infoAdicional != null) {
        var prefijo = "";
        var estadoEnvio = parseInt($("#hfIdEstado").val()) || 0;
        if (estadoEnvio == ESTADO_CANCELADO) {
            prefijo = "<b>Motivo de Cancelación : </b>";
        }
        $("#idMsgCancelacion").html(prefijo + `  "${infoAdicional}"`);
        $("#idMsgCancelacion").show();
    }
    $("#btnExpandirRestaurar").parent().css("display", "table-cell");
    $('#btnExpandirRestaurar').click(function () {
        expandirRestaurar();
    });

    /** OBSERVAR **/
    $('#obs_fecMaxRpta').Zebra_DatePicker({
        format: "d/m/Y",
    });
    $('#btnPopupObservar').click(function () {
        validarYMostrarPopupObservar(this);
    });
    $('#btnEnviarObs').click(function () {
        realizarObservacion();
    });

    /** DENEGAR ENVIO **/
    $('#btnPopupDenegar').click(function () {
        validarYMostrarPopupDenegar(this);
    });
    $('#btnEnviarDenegacion').click(function () {
        realizarDenegacion();
    });

    $('#btnDescargarRevisionContenido').click(function () {
        exportarFormatoRevisionContenido();
    });


    /** APROBAR ENVIO **/
    $('#aprob_fecVigencia').Zebra_DatePicker({
        format: "d/m/Y",
        direction: [FECHA_SISTEMA, "31/12/2070"],
    });
    $('#btnPopupAprobar').click(function () {
        validarYMostrarPopupAprobar(this);
    });
    $('#btnEnviarAprobacion').click(function () {
        realizarAprobacion();
    });

    /** APROBAR PARCIALMENTE ENVIO **/
    $('#btnPopupAprobarParcialmente').click(function () {
        mostrarMensaje('mensajeEvento', "alert", "Ingreso válido solo para envíos en la etapa de Modificación de Ficha Técnica que no sean Dar de Baja.");
    });

    /** DERIVAR ENVIO **/
    $('#deriv_fecmaxrpta').Zebra_DatePicker({
        format: "d/m/Y",
        direction: [FECHA_SISTEMA, "31/12/2070"],
    });
    $('#btnPopupDerivar').click(function () {
        validarYMostrarPopupDerivar(this);
    });
    $('#btnEnviarDerivacion').click(function () {
        realizarDerivacion();
    });

    listarArchivosEnvio();

    visibilidadBotones();   


    if (tipoFormato == FORMATO_OPERACION_COMERCIAL) {
        $('#tab-container').bind('easytabs:before', function (id, val, t) {
            if (t.selector == "#tabDetalle") {
                setTimeout(function () {
                    createResizableTable(document.getElementById('tabla_OP'));
                }, 2000)
            }

        });
    } else {
        if (tipoFormato == FORMATO_DAR_BAJA) {

            setTimeout(function () {
                createResizableTable(document.getElementById('tabla_OP'));
            }, 4000);

        }
    }
});

function regresarPrincipal(estado) {
    var estadoEnvio = parseInt($("#hfIdEstado").val()) || 0;

    if (estado != null) {
        estadoEnvio = estado;
    }

    document.location.href = controlador + "Index?carpeta=" + estadoEnvio;
}

function getObjetoFiltro() {
    var filtro = {};

    filtro.codigoEnvio = parseInt($("#hfIdEnvio").val()) || 0;
    filtro.codigoVersion = parseInt($("#hfIdVersion").val()) || 0;

    filtro.codigoEmpresa = parseInt($("#hfEmprcodi").val()) || 0;
    filtro.codigoEtapa = parseInt($("#hfFtetcodi").val()) || 0;
    filtro.codigoProyecto = parseInt($("#hfFtprycodi").val()) || 0;
    filtro.codigoEquipos = $("#hfCodigoEquipos").val();

    return filtro;
}

//FORMULARIO
function listarArchivosEnvio() {
    limpiarBarraMensaje('mensajeEvento');

    var idsAreasUsuario = $('#hdIdsAreaTotales').val();
    var filtro = getObjetoFiltro();

    $.ajax({
        type: 'POST',
        url: controlador + "ListarRequisitoXEnvio",
        data: {
            codigoEnvio: filtro.codigoEnvio,
            versionEnvio: filtro.codigoVersion,
            areasUsuario: idsAreasUsuario
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                listaDataRevisionFT = evt.ListaRevisionParametrosAFT;
                listaDataRevisionArea = evt.ListaRevisionAreasContenido;

                OBJ_VALIDACION_ENVIO = evt.ValidacionEnvio;

                LISTA_REQUISITO_ARCHIVO = evt.ListaReqEvento;
                cargarHtmlDocumentoXEnvio(LISTA_REQUISITO_ARCHIVO, TIPO_ARCHIVO_REQUISITO, false, false);

                LISTA_VERSIONES = evt.ListaVersion;
                LISTA_AUTOGUARDADO = evt.ListaAutoguardado;
                LISTA_ERRORES = evt.ListaErrores;

                var htmlTabla = dibujarTablaEquipoEnvioConexIntegModif(evt.ListaEnvioEq, filtro.codigoEnvio);
                $("#listadoDetalleEquipo").html(htmlTabla);

                /*$('#tablaDetalleEquipo').dataTable({
                    "scrollY": false,
                    "scrollX": false,
                    "sDom": 't',
                    "ordering": false,
                    "iDisplayLength": -1
                });*/

                //barra de herramientas
                if (OPCION_GLOBAL_EDITAR) {
                    $("#barra_herramienta_envio").show();
                    $(".barra_herramienta_envio_2").show();
                    $("#leyenda_alerta").show();

                    $("#btnHistorialAutoguardado").parent().css("display", "table-cell");
                    $("#btnAutoguardar").parent().css("display", "table-cell");
                    $("#btnLimpiar").parent().css("display", "table-cell");

                    $("#btnEnviarDatos").parent().css("display", "table-cell");
                    $("#btnMostrarErrores").parent().css("display", "table-cell");

                    DEBE_AUTOGUARDAR = true;
                } else {
                    $("#barra_herramienta_envio").show();
                    $("#btnVerEnvio").parent().css("display", "table-cell");
                }

                setTimeout(function () {
                    $("#chkSeleccionar").on("click", function () {
                        var check = $('#chkSeleccionar').is(":checked");
                        $("input[name=chkSeleccion]").prop("checked", check);
                    });
                }, 50);

                agregarBloqueRevision(evt);
                agregarBloqueAreas(evt);

            } else {

                mostrarMensaje('mensajeEvento', "alert", evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensajeEvento', 'error', 'Ha ocurrido un error.');
        }
    });
}

function dibujarTablaEquipoEnvioConexIntegModif(lista, idEnvio) {
    var cadena = '';

    var thSelec = '';
    if (OPCION_GLOBAL_EDITAR) thSelec = `<th style="width: 40px">Sel. Todos <br/> <input type="checkbox" id="chkSeleccionar"> </th>`;

    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaDetalleEquipo" cellspacing="0"  >
        <thead>            
            <tr style="height: 30px;">
                <th style="width: 40px">Código</th>
                <th style="width: 150px">Empresa <br>Titular</th>
                <th style="width: 150px">Empresa <br>Copropietaria</th>
                <th style="width: 100px">Tipo de <br>equipo / categoría</th>
                <th style="width: 75px">Ubicación</th>
                <th style="width: 125px">Nombre</th>
            </tr>
        </thead>
        <tbody>
    `;

    var fila = 1;
    for (key in lista) {
        var item = lista[key];

        var tdSelec = '';
        if (OPCION_GLOBAL_EDITAR) {
            var checked = OPCION_IMPORTACION ? ' checked ' : '';
            tdSelec = `
                <td style="width: 40px">
                    <input type="checkbox" value="${item.Fteeqcodi}" name="chkSeleccion" id="${item.Fteeqcodi}" ${checked} />
                </td>
            `            ;
        }

        cadena += `
           <tr >
                <td style="width: 40px">${item.Idelemento}</td>
                <td style="width: 150px">${item.Nombempresaelemento}</td>
                <td style="width: 150px">${item.Nombempresacopelemento}</td>
                <td style="width: 100px">${item.Tipoelemento}</td>
                <td style="width: 75px">${item.Areaelemento}</td>
                <td style="width: 125px">${item.Nombreelemento}</td>
           </tr >    
        `;

        fila++;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function agregarBloqueRevision() {
    limpiarBarraMensaje('mensajeEvento');

    agregarColumnasRevision(listaDataRevisionFT);
}

function agregarColumnasRevision(listaRevision) {
    var filasTabla = document.getElementById("tabla_OP").rows;

    var numF = 1;
    for (key in filasTabla) {

        var fila = filasTabla[key];
        var idFila = fila.id;

        var numNuevasCol = 4;

        if (numF < 3) { //NUAVAS COLUMNAS PARA LAS 2 PRIMERAS FILAS

            if (numF == 1) {
                var tagFila1 = document.getElementById("1raFila");

                //agrego primera fila
                var nuevaCol1 = document.createElement('td');
                var cadena1 = 'Administrador Ficha Técnica';
                nuevaCol1.innerHTML = cadena1;
                nuevaCol1.style.textAlign = "center";
                nuevaCol1.style.border = "1px solid " + COLOR_BORDE;
                nuevaCol1.colSpan = 4;
                nuevaCol1.classList.add("campo_titulo_tab");
                tagFila1.appendChild(nuevaCol1);
            }

            if (numF == 2) {
                //agrego segunda fila
                var tagFila2 = document.getElementById("2daFila");

                for (var i = 1; i <= numNuevasCol; i++) {
                    var nuevaCol2 = document.createElement('th');

                    nuevaCol2.classList.add("campo_cab_add_celdas");
                    nuevaCol2.style.textAlign = "center";
                    nuevaCol2.style.width = "120px";
                    nuevaCol2.style.border = "1px solid " + COLOR_BORDE;
                    var cadena2 = '';
                    switch (i) {
                        case 1:
                            cadena2 = '<b style="text-aling: center;">Observación (COES)</b>';
                            nuevaCol2.classList.add("celdaREV");
                            break;
                        case 2:
                            cadena2 = 'Respuesta Subsanación de Observación (Agente)';
                            nuevaCol2.classList.add("celdaREV");
                            break;
                        case 3:
                            cadena2 = 'Respuesta Subsanación de Observación (COES)';
                            nuevaCol2.classList.add("celdaREV");
                            break;
                        case 4:
                            cadena2 = 'Estado';
                            nuevaCol2.classList.add("celdaREVEstado");
                            nuevaCol2.style.width = "55px";
                            break;
                    }

                    nuevaCol2.innerHTML = cadena2;
                    tagFila2.appendChild(nuevaCol2);
                }
            }

        } else { //filas con DATA DE LA TABLA
            if (idFila != null && idFila != "") {
                const myArray = idFila.split("_");
                let nomDiv = myArray[0];
                let miFevrqcodi = myArray[3];
                let esRevisable = myArray[4];

                var _fevrqcodi = null;
                if (!isNaN(miFevrqcodi))
                    _fevrqcodi = parseInt(miFevrqcodi);

                const registro = listaRevision.find(x => x.Fevrqcodi === _fevrqcodi);

                var tagFila = document.getElementById(idFila);

                for (var i = 1; i <= numNuevasCol; i++) {

                    //Agrego
                    var nuevaCol = document.createElement('td');
                    nuevaCol.style.border = "1px solid " + COLOR_BORDE;

                    var cadena = '';


                    ////////////////
                    if (nomDiv == "divCab") {
                        nuevaCol.style.background = COLOR_CAB_REQUISITO;

                    } else {
                        if (esRevisable == "S") {
                            //Verifico si bloqueo toda la fila
                            if (registro.FilaBloqueada) {

                                switch (i) {
                                    case 1: cadena = obtenerHtmlCeldaRevisionFilaBloqueada(i, miFevrqcodi, nuevaCol, registro.ValObsCoes, registro.ListaArchivosObsCoes); nuevaCol.classList.add("celdaREV"); nuevaCol.setAttribute("id", "celdaCont_" + i + "_" + miFevrqcodi); break;
                                    case 2: cadena = obtenerHtmlCeldaRevisionFilaBloqueada(i, miFevrqcodi, nuevaCol, registro.ValRptaAgente, registro.ListaArchivosRptaAgente); nuevaCol.classList.add("celdaREV"); nuevaCol.setAttribute("id", "celdaCont_" + i + "_" + miFevrqcodi); break;
                                    case 3: cadena = obtenerHtmlCeldaRevisionFilaBloqueada(i, miFevrqcodi, nuevaCol, registro.ValRptaCoes, registro.ListaArchivosRptaCoes); nuevaCol.classList.add("celdaREV"); nuevaCol.setAttribute("id", "celdaCont_" + i + "_" + miFevrqcodi); break;
                                    case 4: nuevaCol.style.background = COLOR_BLOQUEADO; //coloreo Fondo
                                        cadena = mostrarListadoHtml(CELDA_REV_VER, registro.ValEstado, registro.ListaEstados, miFevrqcodi, INTRANET);
                                        nuevaCol.classList.add("celdaREVEstado"); break;
                                }


                            } else { //Verifico por cada Columna

                                switch (i) {
                                    case 1: cadena = obtenerHtmlCeldaRevisionFilaNoBloqueada(i, miFevrqcodi, nuevaCol, registro.CeldaObsCoesEstaBloqueada, registro.ValObsCoes, registro.ListaArchivosObsCoes); nuevaCol.classList.add("celdaREV"); nuevaCol.setAttribute("id", "celdaCont_" + i + "_" + miFevrqcodi); break; //columna OBS COES
                                    case 2: cadena = obtenerHtmlCeldaRevisionFilaNoBloqueada(i, miFevrqcodi, nuevaCol, registro.CeldaRptaAgenteEstaBloqueada, registro.ValRptaAgente, registro.ListaArchivosRptaAgente); nuevaCol.classList.add("celdaREV"); nuevaCol.setAttribute("id", "celdaCont_" + i + "_" + miFevrqcodi); break;//columna RPTA AGENTE
                                    case 3: cadena = obtenerHtmlCeldaRevisionFilaNoBloqueada(i, miFevrqcodi, nuevaCol, registro.CeldaRptaCoesEstaBloqueada, registro.ValRptaCoes, registro.ListaArchivosRptaCoes); nuevaCol.classList.add("celdaREV"); nuevaCol.setAttribute("id", "celdaCont_" + i + "_" + miFevrqcodi); break;//columna RPTA COES
                                    case 4: //columna ESTADO
                                        if (registro.CeldaEstadoEstaBloqueada) {
                                            nuevaCol.style.background = COLOR_BLOQUEADO; //coloreo Fondo
                                            cadena = mostrarListadoHtml(CELDA_REV_VER, registro.ValEstado, registro.ListaEstados, miFevrqcodi, INTRANET);
                                        } else {//esta desbloqueada en la extranet
                                            cadena = mostrarListadoHtml(CELDA_REV_EDITAR, registro.ValEstado, registro.ListaEstados, miFevrqcodi, INTRANET);
                                        }
                                        nuevaCol.classList.add("celdaREVEstado"); break;
                                }
                            }
                        } else {
                            nuevaCol.style.background = COLOR_BLOQUEADO;
                        }
                    }
                    ////////////////////


                    nuevaCol.innerHTML = cadena;
                    tagFila.appendChild(nuevaCol);
                }


            }
        }
        numF++;
    }

    //createResizableTable(document.getElementById('tabla_OP'));

}

function visibilidadBotones() {

    var estadoEnvio = parseInt($("#hfIdEstado").val()) || 0;
    var etapaEnvio = parseInt($("#hfFtetcodi").val()) || 0;
    var tipoFormato = parseInt($("#hdIdEnvioTipoFormato").val()) || 0;

    //var esEditable = $("#hdEditable").val();

    //Botones Principales
    if (OPCION_GLOBAL_EDITAR) {
        if (estadoEnvio == ESTADO_SOLICITADO) {
            $("#bq_derivar").css("display", "inline-table");
            $("#bq_aprobar").css("display", "inline-table");
            $("#bq_observar").css("display", "inline-table");

            $("#bq_derivar").css("float", "right");
            $("#bq_aprobar").css("float", "right");
            $("#bq_observar").css("float", "right");

        } else {

            if (estadoEnvio == ESTADO_SUBSANADO) {


                $("#bq_derivar").css("display", "inline-table");
                $("#bq_denegar").css("display", "inline-table");
                $("#bq_aprobar").css("display", "inline-table");
                $("#bq_aprobarparcialmente").css("display", "inline-table");

                $("#bq_derivar").css("float", "right");
                $("#bq_denegar").css("float", "right");
                $("#bq_aprobar").css("float", "right");
                $("#bq_aprobarparcialmente").css("float", "right");
            } else {

                if (estadoEnvio == ESTADO_APROBADO) {
                    //Visible, solo para aquellos envios en etapa de conexión
                    if (etapaEnvio == ETAPA_CONEXION) {
                        $("#bq_denegar").css("display", "inline-table");
                        $("#bq_denegar").css("float", "right");
                    }
                }
                else {
                    ////para los VER DETALLES TODOS OCULTOS

                }
            }
        }
    } else {
        ////para los VER DETALLES TODOS OCULTOS

    }

    //Botones Descarga
    if (etapaEnvio == ETAPA_CONEXION || etapaEnvio == ETAPA_INTEGRACION) {
        if (estadoEnvio == ESTADO_SOLICITADO || estadoEnvio == ESTADO_SUBSANADO || estadoEnvio == ESTADO_APROBADO || estadoEnvio == ESTADO_APROBADO_PARCIAL || estadoEnvio == ESTADO_DESAPROBADO || estadoEnvio == ESTADO_CANCELADO) {
            $("#bq_descargaFichas").css("display", "inline-table");
        }
    } else {
        if (etapaEnvio == ETAPA_OPERACION_COMERCIAL) {
            if (estadoEnvio == ESTADO_SOLICITADO || estadoEnvio == ESTADO_SUBSANADO || estadoEnvio == ESTADO_APROBADO || estadoEnvio == ESTADO_APROBADO_PARCIAL || estadoEnvio == ESTADO_DESAPROBADO || estadoEnvio == ESTADO_CANCELADO) {
                $("#bq_descargaContenido").css("display", "inline-block");
            }
        } else {
            if (etapaEnvio == ETAPA_MODIFICACION) {
                if (tipoFormato == TIPO_FORMATO_DARBAJA) //Dar Baja
                {
                    if (estadoEnvio == ESTADO_SOLICITADO || estadoEnvio == ESTADO_APROBADO || estadoEnvio == ESTADO_APROBADO_PARCIAL || estadoEnvio == ESTADO_DESAPROBADO || estadoEnvio == ESTADO_CANCELADO) {
                        $("#bq_descargaContenido").css("display", "inline-block");
                    }
                } else {
                    if (estadoEnvio == ESTADO_SOLICITADO || estadoEnvio == ESTADO_SUBSANADO || estadoEnvio == ESTADO_APROBADO || estadoEnvio == ESTADO_APROBADO_PARCIAL || estadoEnvio == ESTADO_DESAPROBADO || estadoEnvio == ESTADO_CANCELADO) {
                        $("#bq_descargaFichas").css("display", "inline-table");
                    }
                }
            }
        }
    }
}

function obtenerHtmlCeldaRevisionFilaNoBloqueada(indiceColumnaBloque, fevrqcodi, columna, esCeldaBloqueada, valorCelda, listaArchivos) {
    var cadena = "";
    if (esCeldaBloqueada) {
        columna.style.background = COLOR_BLOQUEADO; //coloreo Fondo
        if (valorCelda != null && valorCelda != "") {
            cadena = mostrarTextoHtml(indiceColumnaBloque, fevrqcodi, CELDA_REV_VER, valorCelda, listaArchivos);
        } else {
            cadena = mostrarTextoHtml(indiceColumnaBloque, fevrqcodi, CELDA_REV_VER, "", listaArchivos);
        }
    } else {//esta desbloqueada en la intranet
            cadena = mostrarTextoHtml(indiceColumnaBloque, fevrqcodi, CELDA_REV_EDITAR, valorCelda, listaArchivos);
    }
    return cadena;
}

function obtenerHtmlCeldaRevisionFilaBloqueada(indiceColumnaBloque, fevrqcodi, columna, valorCelda, listaArchivos) {
    var cadena = "";
    columna.style.background = COLOR_BLOQUEADO; //coloreo Fondo
    if (valorCelda != null && valorCelda != "") {
        cadena = mostrarTextoHtml(indiceColumnaBloque, fevrqcodi, CELDA_REV_VER, valorCelda, listaArchivos);
    } else {
        cadena = mostrarTextoHtml(indiceColumnaBloque, fevrqcodi, CELDA_REV_VER, "", listaArchivos);
    }
    return cadena;
}

function mostrarTextoHtml(indiceColumnaBloque, fevrqcodi, accion, msgHtml, listaArchivos) {

    var htmlEditado = "";

    let reg = {
        "Mensaje": `${msgHtml}`
    }
    var msgCodificado = `${encodeURIComponent(JSON.stringify(reg))}`;

    var styleFondo = "";

    var textoSinEtiquetas = removerTagsHtml(msgHtml);
    var tamTexto = textoSinEtiquetas.length;

    if (msgHtml != null) {
        htmlEditado = textoSinEtiquetas;
        if (tamTexto > 20) {
            htmlEditado = textoSinEtiquetas.substring(0, 20) + " (...)";
        }
    }

    var htmlBtn = "";
    if (accion == CELDA_REV_EDITAR) {

        htmlBtn = `
                    <a title="Editar registro" href="#" style="float: right;" onClick=" mostrarPopupObservacion(${indiceColumnaBloque}, ${fevrqcodi},${accion},'${msgCodificado}');">${IMG_EDITAR} </a>
        `;
    } else {
        if (accion == CELDA_REV_VER) {
            styleFondo = 'background:' + COLOR_BLOQUEADO;

            //solo se muestra el boton Ver si hay comentario
            var hayBtnVerMasLectura = (listaArchivos != null ? listaArchivos.length : 0) > 0 || tamTexto > 20;
            if (hayBtnVerMasLectura) {
                htmlBtn = `
                         <a title="Ver registro" href="#" style="float: right;color: green;font-weight: bold;" onClick=" mostrarPopupObservacion(${indiceColumnaBloque}, ${fevrqcodi},${accion},'${msgCodificado}');">Ver Más </a>
                `;
            } else {
                htmlBtn = `
                `;
            }
        }
    }

    var numArchivos = listaArchivos != null ? listaArchivos.length : 0;
    var htmlArchivos = ''; //
    if (numArchivos > 0) htmlArchivos = generarTablaListaBodyXObs(listaArchivos, accion, false, indiceColumnaBloque, fevrqcodi);



    var htmlDiv = '';
    //-webkit-fill-available para que tome todo el ancho de la columna
    htmlDiv += `
        <table style = "width: -webkit-fill-available;">
            <tr id="bloqueTexto_${indiceColumnaBloque}_${fevrqcodi}">
                   <div id="datoCelda_${indiceColumnaBloque}_${fevrqcodi}" style=" word-break: break-all;">
                     ${htmlEditado}
                   </div>
                   <div>
                        ${htmlBtn}
                   </div>
            </tr>
            
        </table>
    `;
    htmlDiv += `
        <div id="data_Archivos_${indiceColumnaBloque}_${fevrqcodi}">
            <table>
                <tr>                
                        ${htmlArchivos}                
               </tr>
            </table>
        </div>
        `;

    return htmlDiv;
}

function generarTablaListaBodyXObs(listaArchivo, accion, enPopup, nColumna, fevrqcodi) {

    listaArchivo.sort((x, y) => x.NombreArchivo - y.NombreArchivo); // ordenamieto

    var styleFondo = "";

    if (accion == CELDA_REV_VER) {
        if (enPopup) {
            styleFondo = 'background: white; word-break: break-all;';
        } else {
            styleFondo = 'background:' + COLOR_BLOQUEADO + '; border: 0px; word-break: break-all;';
        }
    } else {
        if (accion == CELDA_REV_EDITAR) {
            if (enPopup) {
                styleFondo = 'background: white; word-break: break-all;';
            } else {
                styleFondo = 'background: white; border: 0px; word-break: break-all;';
            }

        }
    }
    var html = `
                <table border="0" class="pretty tabla-icono" cellspacing="0" style="width:auto" id="">
                    <tbody>`;

    for (var i = 0; i < listaArchivo.length; i++) {
        var item = listaArchivo[i];
        var idrow = "row" + i;
        var nomb = item.Ftearcnombreoriginal;

        var textoEnPopup = '';
        if (enPopup) {
            textoEnPopup = nomb;
        }

        html += `
                        <tr id="${idrow}">

                            <td onclick="verArchivoXObs(${nColumna}, ${fevrqcodi},${i}, ${enPopup});" title='Visualizar archivo - ${nomb}' style="cursor: pointer;width:30px;${styleFondo}">
                                	&#128065;
                            </td>
                            <td onclick="descargarArchivoXObs(${nColumna}, ${fevrqcodi},${i}, ${enPopup});" title='Descargar archivo - ${nomb}' style="cursor: pointer;width:30px;${styleFondo}">
                                <img width="15" height="15" src="../../Content/Images/btn-download.png" />
                            </td>
                            <td style="text-align:left;${styleFondo}" title='${nomb}'>
                                ${textoEnPopup}
                            </td>
        `;
        if (accion == CELDA_REV_EDITAR && enPopup) {
            html += `     
                            <td onclick="eliminarRowXObs(${nColumna}, ${fevrqcodi},${i})" title='Eliminar archivo' style="width:30px;cursor:pointer;${styleFondo}">
                                <a href="#"><img src="../../Content/Images/btn-cancel.png" /></a>
                            </td>                                           
                 `;
        }

        html += "       </tr>";
    }

    html += `
                    </tbody>
                </table>`;

    return html;
}

function mostrarListadoHtml(accion, valEstado, listado, miFevrqcodi, interfaz) {
    var htmlDiv = '';
    var tieneEstadoPreseleccionado = false;
    var htmlColoSelect = '';
    var htmlHabilitacion = "";

    var miEstenvcodi = parseInt($("#hfIdEstado").val()) || 0;

    var textoEstado = "";

    if (valEstado != null) {
        switch (valEstado.trim()) {
            case OpcionConforme: textoEstado = "Conforme"; break;
            case OpcionNoSubsanado: textoEstado = "No Subsanado"; break;
            case OpcionSubsanado: textoEstado = "Subsanado"; break;
            case OpcionObservado: textoEstado = "Observado"; break;
            case OpcionDenegado: textoEstado = "Denegado"; break;
        }
    }

    if (accion == CELDA_REV_VER)
        htmlHabilitacion = "disabled";

    ////textoEstado = "No Subsanado";    

    //cambio estilo del combo seleccionado
    var colorEstado = obtenerColorTextoSegunEstado(valEstado);

    //Para solicitud e intranet, se debe mostrar vacio en Estado
    if ((valEstado == "" || valEstado == null) && miEstenvcodi == ESTADO_SOLICITADO && interfaz == INTRANET) {
        valEstado = "-1";
        textoEstado = "";
        tieneEstadoPreseleccionado = true;
    } else {
        if (textoEstado != null) {
            if (textoEstado.trim() != "") {
                tieneEstadoPreseleccionado = true;
            }
        }
    }

    if (tieneEstadoPreseleccionado)
        htmlColoSelect += ` color:  ${colorEstado}`;

    if (listado.length > 0) {



        htmlDiv += `
            <select ${htmlHabilitacion} name="" id="ckbEstado_${miFevrqcodi}" onchange="CambiarOpcionCombo('${miFevrqcodi}');" 
                style="border: none; background: transparent; width: 86px; text-align: center;font-size: 11px;font-weight: bold; ${htmlColoSelect} " >
        `;

        if (tieneEstadoPreseleccionado) {
            htmlDiv += `
                    <option value="${valEstado}" disabled selected hidden>${textoEstado.trim()}</option>
                `;
        }
        for (key in listado) {
            var opcion = listado[key];
            htmlDiv += `
                <option style="color: ${opcion.ValColor};" value="${opcion.Codigo}">${opcion.Texto}</option>                
        `;
        }
        htmlDiv += `
            </select>
        `;
    } else {
        if (tieneEstadoPreseleccionado)
            htmlDiv += `
                    <div style="${htmlColoSelect}"> ${textoEstado}  </div>
                `;


    }

    return htmlDiv;
}

function CambiarOpcionCombo(miFevrqcodi) {
    var miEstenvcodi = $("#hfIdEstado").val();
    var opcionSeleccionada = $("#ckbEstado_" + miFevrqcodi).val();

    //cambio estilo del combo seleccionado
    var colorTexto = obtenerColorTextoSegunEstado(opcionSeleccionada);
    $("#ckbEstado_" + miFevrqcodi).css("color", colorTexto);

    //reemplazo en el array el valor del seleccionado
    var fevrqcodi = parseInt(miFevrqcodi) || 0;
    const registro = listaDataRevisionFT.find(x => x.Fevrqcodi === fevrqcodi);
    var numCol = 0;
    var pos = -1;
    if (registro != null) {
        pos = listaDataRevisionFT.findIndex(function (x) { return x.Fevrqcodi === fevrqcodi; });
        numCol = listaDataRevisionFT[pos].NumcolumnaEditada;
        listaDataRevisionFT[pos].IdValorEstado = opcionSeleccionada;
    }

    if (miEstenvcodi == ESTADO_SOLICITADO) { //Solo para los envios SOLICITADOS de intranet (los SUBSANADOS no bloquean celda de la 3ra columna de revision) 
        //Si se selecciona CONFORME en INTRANET deshabilito la fila
        if (opcionSeleccionada.trim() == OpcionConforme) {
            $("#celdaCont_" + numCol + "_" + fevrqcodi).css("background", COLOR_BLOQUEADO);
            $("#bloqueTexto_" + numCol + "_" + fevrqcodi).css("display", "none");

            if (pos != -1) {
                listaDataRevisionFT[pos].MsgColumnaEditada = "";
                $("#datoCelda_" + numCol + "_" + fevrqcodi).html("");

                //Limpio los archivos cargados
                listaDataRevisionFT[pos].ListaArchivosObsCoes = null;
                $("#data_Archivos_" + numCol + "_" + fevrqcodi).html("");
            }

        } else { //Si es diferente a SOLICITUD no se limpian la celda al escoger CONFORME
            $("#celdaCont_" + numCol + "_" + fevrqcodi).css("background", "#FFFFFF");
            $("#bloqueTexto_" + numCol + "_" + fevrqcodi).css("display", "table-row");
        }
    }
}


function obtenerColorTextoSegunEstado(opcionSeleccionada) {
    var colorTexto = "#000000";

    if (opcionSeleccionada != null) {
        switch (opcionSeleccionada.trim()) {

            case OpcionConforme: colorTexto = ColorAzul; break;
            case OpcionNoSubsanado: colorTexto = ColorNaranja; break;
            case OpcionSubsanado: colorTexto = ColorVerde; break;
            case OpcionObservado: colorTexto = ColorRojo; break;
            case OpcionDenegado: colorTexto = ColorVioleta; break;
        }
    }

    return colorTexto;
}

//AUTOGUARDADO
async function guardarEquipoTemporalExcelWeb() {
    var filtro = getObjetoFiltro();

    var modeloWeb = {
        Ftenvcodi: filtro.codigoEnvio,
        Ftevercodi: filtro.codigoVersion,
        Fteeqcodi: 0,
        MensajeAutoguardado: ULTIMO_MENSAJE_AUTOGUARDADO,
        ListaRevision: listaDataRevisionFT
    };

    return $.ajax({
        type: 'POST',
        dataType: 'json',
        url: controlador + "GuardarDatosFT",
        contentType: "application/json",
        data: JSON.stringify({
            modelWeb: modeloWeb,
        }),
        beforeSend: function () {
            //mostrarExito("Enviando Información ..");
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                if (evt.Resultado == "1") {
                    mostrarMensaje('mensaje_GuardadoTemporal', 'exito', "Se guardó correctamente.");

                    OBJ_VALIDACION_ENVIO = evt.ValidacionEnvio;
                }

                ULTIMO_MENSAJE_AUTOGUARDADO = '';

                //que la pantalla vaya al fondo para mostrar la mayor cantidad de elementos del formulario
                _irAFooterPantalla();
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
/// Archivos adjuntos de observaciones
////////////////////////////////////////////////////////////////////////////////////////////////////////////
function cargarHtmlArchivoXObs(nColumna, fevrqcodi, accion) {

    var prefijo = "_sec_arch_" + fevrqcodi;
    var htmlSec = "<div id=div_" + prefijo + ">";
    htmlSec += generarHtmlTablaDocumentoXObs(nColumna, prefijo, fevrqcodi, accion);
    htmlSec += "</div>";

    $("#html_archivos_x_obs").html(htmlSec);

    //plugin archivo    
    pUploadArchivoXObs(nColumna, prefijo, fevrqcodi, accion);

}

function generarHtmlTablaDocumentoXObs(nColumna, idPrefijo, fevrqcodi, accion) {

    var listaArchivo = [];
    //Obtengo listado de archivo segun columna y fila
    var elemento = listaDataRevisionFT.find(x => x.Fevrqcodi === fevrqcodi);
    switch (nColumna) {

        case 1: listaArchivo = elemento.ListaArchivosObsCoes; break;
        case 2: listaArchivo = elemento.ListaArchivosRptaAgente; break;
        case 3: listaArchivo = elemento.ListaArchivosRptaCoes; break;
    }

    //clono la lista
    listaArchivoRevTemp = [];
    for (key in listaArchivo) {
        var reg = listaArchivo[key];

        listaArchivoRevTemp.push(reg);
    }

    var html = `
            <table class="content-tabla-search" style="width:100%">
                <tr>
                    <td>
                        <div style="clear:both; height:10px"></div>
    `;

    if (accion == CELDA_REV_EDITAR) {
        html += `
                        <spam><b>Nota:</b><i> Ingrese solo comentarios al formulario, las imagenes y otros archivos deben ser adjuntados</i></spam>
                        <div style="width:180px">
                            <input type="button" id="btnSelectFile${idPrefijo}" value="Adjuntar Archivo" />
                            <div style="font-size: 10px;padding-left: 15px;color: #5F0202;">Tamaño máximo por archivo: 50MB</div>
                        </div>
        `;
    }

    html += `
                        <div style="clear:both; height:5px"></div>
                        <div id="fileInfo${idPrefijo}"></div>
                        <div id="progreso${idPrefijo}"></div>
                        <div id="fileInfo${idPrefijo}" class="file-upload plupload_container ui-widget-content " style="display:none"></div>

                        <input type="hidden" id="hfile${idPrefijo}" name="file${idPrefijo}" value=" " />
                        <input type="hidden" id="hcodenvio${idPrefijo}" value="@Model.IdEnvio" />

                        <div id="listaArchivos${idPrefijo}" class="content-tabla">
    `;

    html += generarTablaListaBodyXObs(listaArchivoRevTemp, accion, true, nColumna, fevrqcodi);

    html += `
                        </div>

                    </td>
                </tr>
            </table>
                <div style='clear:both; height:10px;width:100px;'></div>
    `;

    return html;
}

function pUploadArchivoXObs(nColumna, prefijo, fevrqcodi, accion) {
    var uploaderP23 = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFile' + prefijo,
        container: document.getElementById('container'),
        chunks_size: '50mb',
        multipart_params: {
            idEnvio: getObjetoFiltro().codigoEnvio,
            idVersion: getObjetoFiltro().codigoVersion,
            idElemento: fevrqcodi,
            tipoArchivo: getTipoArchivoXColumna(nColumna)
        },
        url: controlador + 'UploadTemporal',
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '50mb',
            mime_types: [
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                up.refresh();
                uploaderP23.start();
            },
            UploadProgress: function (up, file) {
                $('#progreso' + prefijo).html(file.percent + "%");
            },

            UploadComplete: function (up, file) {
                $('#progreso' + prefijo).html("Archivo enviado.");
                setTimeout(function () {
                    $('#progreso' + prefijo).html("");
                }, 2500);
            },
            FileUploaded: function (up, file, result) {
                agregarRowXObs(nColumna, prefijo, fevrqcodi, JSON.parse(result.response).nuevonombre, file.name, accion);
            },
            Error: function (up, err) {
                loadValidacionFile(err.code + "-" + err.message);
                if (err.code = -600) //error de tamaño
                    err.message = "Error: El archivo adjuntado supera el tamaño límite (50MB) por archivo. ";
                alert(err.message);
            }
        }
    });
    uploaderP23.init();
}

function agregarRowXObs(nColumna, prefijo, fevrqcodi, nuevoNombre, nombreArchivo, accion) {

    var elemento = listaDataRevisionFT.find(x => x.Fevrqcodi === fevrqcodi);

    if (elemento != null) {

        listaArchivoRevTemp.push({ EsNuevo: true, Ftearcnombrefisico: nuevoNombre, Ftearcnombreoriginal: nombreArchivo });

        $("#listaArchivos" + prefijo).html(generarTablaListaBodyXObs(listaArchivoRevTemp, accion, true, nColumna, fevrqcodi));
    }

}

function eliminarRowXObs(nColumna, fevrqcodi, pos) {

    listaArchivoRevTemp.splice(pos, 1);


    var prefijo = "_sec_arch_" + fevrqcodi;

    $("#listaArchivos" + prefijo).html(generarTablaListaBodyXObs(listaArchivoRevTemp, CELDA_REV_EDITAR, true, nColumna, fevrqcodi));
}

function getTipoArchivoXColumna(nColumna) {
    if (nColumna == 1) return TIPO_ARCHIVO_REVISION_OBSCOES;
    if (nColumna == 2) return TIPO_ARCHIVO_REVISION_RPTAAGENTE;
    if (nColumna == 3) return TIPO_ARCHIVO_REVISION_RPTACOES;

    return "NA";
}

//Vista previa
function verArchivoXObs(nColumna, fevrqcodi, pos, enPopup) {

    var regArchivo;
    var tipoArchivo = getTipoArchivoXColumna(nColumna);
    var listaArchivo = [];

    if (enPopup) {
        //busco en la lista temporal
        regArchivo = listaArchivoRevTemp[pos];
    } else {
        //Obtengo listado de archivo guardado segun columna y fila 
        var elemento = listaDataRevisionFT.find(x => x.Fevrqcodi === fevrqcodi);
        switch (nColumna) {

            case 1: listaArchivo = elemento.ListaArchivosObsCoes; break;
            case 2: listaArchivo = elemento.ListaArchivosRptaAgente; break;
            case 3: listaArchivo = elemento.ListaArchivosRptaCoes; break;
        }

        regArchivo = listaArchivo[pos];
    }

    if (regArchivo != null) {
        var nombreArchivo = regArchivo.Ftearcnombrefisico.toLowerCase();
        var esPdf = nombreArchivo.endsWith(".pdf");
        var esVistaPrevia = nombreArchivo.endsWith(".pdf") || nombreArchivo.endsWith(".xlsx") || nombreArchivo.endsWith(".docx") || nombreArchivo.endsWith(".xls") || nombreArchivo.endsWith(".doc");

        var metodoVistaPrevia =  'VistaPreviaArchivoEnvio';

        if (esVistaPrevia) {
            $.ajax({
                type: 'POST',
                url: controlador + metodoVistaPrevia,
                data: {
                    idEnvio: getObjetoFiltro().codigoEnvio,
                    idVersion: getObjetoFiltro().codigoVersion,
                    idElemento: fevrqcodi,
                    fileName: nombreArchivo,
                    tipoArchivo: tipoArchivo
                },
                success: function (model) {
                    if (model.Resultado != "") {

                        var rutaCompleta = window.location.href;
                        var ruraInicial = rutaCompleta.split("Equipamiento");
                        var urlPrincipal = ruraInicial[0];

                        var urlFrame = urlPrincipal + model.Resultado;
                        var urlFinal = "";
                        if (!esPdf) {
                            urlFinal = "https://view.officeapps.live.com/op/embed.aspx?src=" + urlFrame;
                        } else {
                            urlFinal = urlFrame;
                        }

                        setTimeout(function () {
                            $('#popupArchivoVistaPrevia').bPopup({
                                easing: 'easeOutBack',
                                speed: 450,
                                transition: 'slideDown',
                                modalClose: false
                            });

                            $('#vistaprevia').html('');
                            $("#vistaprevia").show();
                            $('#vistaprevia').attr("src", urlFinal);
                        }, 50);

                    } else {
                        alert("La vista previa solo permite archivos word, excel y pdf.");
                    }
                },
                error: function (err) {
                    alert("Ha ocurrido un error.");
                }
            });
        }
        else {
            alert("La vista previa solo permite archivos word, excel y pdf.");
        }
    }

    return true;
}

function descargarArchivoXObs(nColumna, fevrqcodi, pos, enPopup) {
    //var regArchivo = obtenerArchivo(LISTA_REQUISITO_ARCHIVO, concepcodi, pos);
    var regArchivo;
    var tipoArchivo = getTipoArchivoXColumna(nColumna);
    var listaArchivo = [];

    if (enPopup) {
        //busco en la lista temporal
        regArchivo = listaArchivoRevTemp[pos];
    } else {
        //Obtengo listado de archivo guardado segun columna y fila 
        var elemento = listaDataRevisionFT.find(x => x.Fevrqcodi === fevrqcodi);
        switch (nColumna) {

            case 1: listaArchivo = elemento.ListaArchivosObsCoes; break;
            case 2: listaArchivo = elemento.ListaArchivosRptaAgente; break;
            case 3: listaArchivo = elemento.ListaArchivosRptaCoes; break;
        }

        regArchivo = listaArchivo[pos];
    }

    if (regArchivo != null) {
        window.location = controlador + 'DescargarArchivoEnvio?fileName=' + regArchivo.Ftearcnombrefisico + "&fileNameOriginal=" + regArchivo.Ftearcnombreoriginal
            + '&idElemento=' + fevrqcodi + '&tipoArchivo=' + tipoArchivo + '&idEnvio=' + getObjetoFiltro().codigoEnvio + '&idVersion=' + getObjetoFiltro().codigoVersion;
    }
}

//Observaciones html 
function mostrarPopupObservacion(nColumna, fevrqcodi, accion, objMsg) {

    popupFormularioObservacion(nColumna, fevrqcodi, accion, objMsg);
}

function popupFormularioObservacion(nColumna, fevrqcodi, accion, objMsg) {
    limpiarBarraMensaje('mensajePopupCelda');

    tinymce.remove();
    $('#btnGuardarObsHtml').unbind();
    $("#htmlArchivos").unbind();
    $("#htmlArchivos").html('');
    $("#idFormularioObservacion").html('');

    var objMsg = JSON.parse(decodeURIComponent(objMsg));
    var msgHtml = "";

    if (accion == CELDA_REV_EDITAR) {
        const registro = listaDataRevisionFT.find(x => x.Fevrqcodi === fevrqcodi && x.NumcolumnaEditada === nColumna);
        msgHtml = registro != null ? registro.MsgColumnaEditada : "";
    } else {
        msgHtml = objMsg.Mensaje;
    }

    var esEditable = accion == CELDA_REV_EDITAR ? true : false;
    var miReadonly = esEditable ? 0 : 1;
    var habilitacion = esEditable ? "" : 'disabled';

    var htmlDiv = '';
    htmlDiv += `
        <table style="width:100%">
            <tr>
	            <td class="registro-control" style="width:790px;">
		            <textarea name="Contenido" id="contenido_html_obs" maxlength="2000" cols="180" rows="22"  ${habilitacion}>
			            ${msgHtml}
		            </textarea>
	            </td>
            </tr>
        </table>

        <input type='hidden' id='hfXObs_Tipo' value='F3' />

        <div id='html_archivos_x_obs'></div>
    `;

    if (esEditable) {
        htmlDiv += `
            <tr>                
                <td colspan="3" style="padding-top: 2px;">
                    <input type="button" id="btnGuardarObsHtml" value="Guardar" style ="margin: 0px 10px 0px 350px;"/>
                </td>
            </tr>
        `;
    }

    $('#idFormularioObservacion').html(htmlDiv);

    $('#btnGuardarObsHtml').click(function () {
        _guardarObsHtml(nColumna, fevrqcodi);
    });

    setTimeout(function () {
        //$("#popupFormularioObservacion .popup-title").html(objItemObs.Ccombnombre);
        $('#popupFormularioObservacionC').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });

        //plugin html
        var idHtml = "#contenido_html_obs";
        tinymce.init({
            selector: idHtml,
            plugins: [
                //'paste textcolor colorpicker textpattern link table image imagetools preview'
                'wordcount advlist anchor autolink codesample colorpicker contextmenu fullscreen image imagetools lists link media noneditable preview  searchreplace table template textcolor visualblocks wordcount'
            ],
            toolbar1:
                //'insertfile undo redo | fontsizeselect bold italic underline strikethrough | forecolor backcolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link | table | image | mybutton | preview',
                'insertfile undo redo | styleselect fontsizeselect | forecolor backcolor | bullist numlist outdent indent | link | table | mybutton | preview',

            menubar: false,
            readonly: miReadonly,
            language: 'es',
            statusbar: false,
            convert_urls: false,
            plugin_preview_width: 790,
            setup: function (editor) {
                editor.on('change',
                    function () {
                        editor.save();
                    });
                editor.on("paste", function (e) {
                    var pastedData = e.clipboardData.getData('text/plain');
                    if (pastedData == "") {
                        e.preventDefault();
                        setTimeout(function () {
                            $("#inputText").html('');
                        }, 100)
                    }
                });
            }
        });

        //archivos
        cargarHtmlArchivoXObs(nColumna, fevrqcodi, accion);

    }, 50);

}

function _guardarObsHtml(nColumna, fevrqcodi) {
    limpiarBarraMensaje('mensajePopupCelda');
    var listaArchivo = [];
    var htmlObs = $("#contenido_html_obs").val();
    var msg = validarMensajeCelda(htmlObs);

    if (msg == "") {
        //reemplazo en el array
        const registro = listaDataRevisionFT.find(x => x.Fevrqcodi === fevrqcodi && x.NumcolumnaEditada === nColumna);
        if (registro != null) {
            var pos = listaDataRevisionFT.findIndex(function (x) { return x.Fevrqcodi === fevrqcodi && x.NumcolumnaEditada === nColumna; });
            //const filaEliminada = listaDataRevisionFT.splice(pos, 1);
            listaDataRevisionFT[pos].MsgColumnaEditada = htmlObs;

            //Obtengo listado de archivo segun columna y fila
            switch (nColumna) {

                case 1: listaDataRevisionFT[pos].ListaArchivosObsCoes = listaArchivoRevTemp;
                    listaArchivo = listaDataRevisionFT[pos].ListaArchivosObsCoes; break;
                case 2: listaDataRevisionFT[pos].ListaArchivosRptaAgente = listaArchivoRevTemp;
                    listaArchivo = listaDataRevisionFT[pos].ListaArchivosRptaAgente; break;
                case 3: listaDataRevisionFT[pos].ListaArchivosRptaCoes = listaArchivoRevTemp;
                    listaArchivo = listaDataRevisionFT[pos].ListaArchivosRptaCoes; break;
            }
        }


        //Muestra el texto en celda
        var htmlEditado = "";
        var textoSinEtiquetas = removerTagsHtml(htmlObs);
        var tamTexto = textoSinEtiquetas.length;

        if (htmlObs != null) {
            htmlEditado = textoSinEtiquetas;
            if (tamTexto > 20) {
                htmlEditado = textoSinEtiquetas.substring(0, 20) + " (...)";
            }
        }
        $("#datoCelda_" + nColumna + "_" + fevrqcodi).html(htmlEditado)

        //Actualiza el listado de archivos
        $("#data_Archivos_" + nColumna + "_" + fevrqcodi).html(generarTablaListaBodyXObs(listaArchivo, CELDA_REV_EDITAR, false, nColumna, fevrqcodi));


        //cierro popup
        $('#popupFormularioObservacionC').bPopup().close();
    } else {
        mostrarMensaje('mensajePopupCelda', "alert", msg);
    }
}


function validarMensajeCelda(mensajeHtml) {
    var salida = "";

    var textoSinEtiquetas = removerTagsHtml(mensajeHtml);
    var tamTexto = textoSinEtiquetas.length;

    if (tamTexto > 2000) {
        salida = "El mensaje ingresado sobrepasa la cantidad de caracteres permitidos (2000 caracteres).";
    }

    return salida;
}

function removerTagsHtml(str) {
    if ((str === null) || (str === ''))
        return "";
    else
        str = str.toString();

    // Expresión regular para identificar etiquetas HTML en la cadena de entrada. Reemplazar la etiqueta HTML identificada con una cadena nula.
    var sinEtiquetas = str.replace(/(<([^>]+)>)/ig, '');

    return sinEtiquetas.replaceAll('&nbsp;', '').trim();
}


function createResizableTable(table) {
    const cols = table.querySelectorAll('th');
    var i = 0;
    [].forEach.call(cols, function (col) {
        if (i < numColumnasResizables) {
            // Add a resizer element to the column
            const resizer = document.createElement('div');
            resizer.classList.add('resizer');

            // Set the height
            resizer.style.height = `${table.offsetHeight}px`;

            col.appendChild(resizer);

            createResizableColumn(col, resizer, table);
        }
        i++;
    });
}

function createResizableColumn(col, resizer, mitabla) {
    let x = 0;
    let w = 0;
    let wT = 0;

    const mouseDownHandler = function (e) {
        x = e.clientX;

        const styles = window.getComputedStyle(col);
        w = parseInt(styles.width, 10);

        const stylesT = window.getComputedStyle(mitabla);
        wT = parseInt(stylesT.width, 10);

        document.addEventListener('mousemove', mouseMoveHandler);
        document.addEventListener('mouseup', mouseUpHandler);

        resizer.classList.add('resizing');
    };

    const mouseMoveHandler = function (e) {
        const dx = e.clientX - x;
        col.style.width = `${w + dx}px`;
        wT = 1405;
    };

    const mouseUpHandler = function () {
        resizer.classList.remove('resizing');
        document.removeEventListener('mousemove', mouseMoveHandler);
        document.removeEventListener('mouseup', mouseUpHandler);
    };

    resizer.addEventListener('mousedown', mouseDownHandler);
}


function validarPermisoAccionBotones(evt) {
    var estadoEnvio = parseInt($("#hfIdEstado").val()) || 0;
    var etapaEnvio = parseInt($("#hfFtetcodi").val()) || 0;

    var permiso = false;
    let idBtn = evt.id;

    //BOTONES PRINCIPALES
    if (OPCION_GLOBAL_EDITAR) {
        if (estadoEnvio == ESTADO_SOLICITADO) {
            switch (idBtn) {
                case "btnPopupDerivar":
                case "btnPopupObservar":
                case "btnPopupAprobar":
                    permiso = true; break;
            }
        } else {
            if (estadoEnvio == ESTADO_SUBSANADO) {
                switch (idBtn) {
                    case "btnPopupDerivar":
                    case "btnPopupDenegar":
                    case "btnPopupAprobarParcialmente":
                    case "btnPopupAprobar":
                        permiso = true; break;
                }
            } else {
                if (estadoEnvio == ESTADO_APROBADO) {
                    if (etapaEnvio == ETAPA_CONEXION) {
                        switch (idBtn) {
                            case "btnPopupDenegar":
                                permiso = true; break;
                        }
                    }
                }
            }
        }
    }

    //BOTONES DESCARGAS
    if (etapaEnvio == ETAPA_CONEXION || etapaEnvio == ETAPA_INTEGRACION) {
        if (estadoEnvio == ESTADO_SOLICITADO || estadoEnvio == ESTADO_SUBSANADO || estadoEnvio == ESTADO_APROBADO || estadoEnvio == ESTADO_APROBADO_PARCIAL || estadoEnvio == ESTADO_DESAPROBADO || estadoEnvio == ESTADO_CANCELADO) {
            if (idBtn == "btnDescargarRevisionFichas") {
                permiso = true;
            }
        }
    } else {
        if (etapaEnvio == ETAPA_OPERACION_COMERCIAL) {
            if (estadoEnvio == ESTADO_SOLICITADO || estadoEnvio == ESTADO_SUBSANADO || estadoEnvio == ESTADO_APROBADO || estadoEnvio == ESTADO_APROBADO_PARCIAL || estadoEnvio == ESTADO_DESAPROBADO || estadoEnvio == ESTADO_CANCELADO) {
                if (idBtn == "btnDescargarRevisionContenido") {
                    permiso = true;
                }
            }
        } else {
            if (etapaEnvio == ETAPA_MODIFICACION) {
                //nunca entra aqui

            } else {

            }
        }
    }

    var msg = "";
    if (!permiso) {
        msg = "Usted no tiene permiso para realizar esta acción.";
    }

    return msg;
}

//Descargar excel
function exportarFormatoRevisionContenido() {
    limpiarBarraMensaje('mensajeEvento');
    var idsAreasUsuario = $('#hdIdsAreaTotales').val();

    var filtro = getObjetoFiltro();

    $.ajax({
        type: 'POST',
        url: controlador + "GenerarFormatoRevisionContenidoOC",
        data: {
            codigoEnvio: filtro.codigoEnvio,
            areasUsuario: idsAreasUsuario,
            codigoEmpresa: filtro.codigoEmpresa,
            codigoEtapa: filtro.codigoEtapa,
            codigoProyecto: filtro.codigoProyecto,
            codigoEquipos: filtro.codigoEquipos,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "Exportar?file_name=" + evt.Resultado;
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//   BLOQUE AREAS
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function agregarBloqueAreas(evt) {
    //limpiarBarraMensaje('mensajeEvento');

    agregarColumnasAreasRevision(evt.ListaRevisionAreasContenido);
}

function agregarColumnasAreasRevision(listaRevision) {
    var filasTabla = document.getElementById("tabla_OP").rows;

    var numF = 1;
    for (key in filasTabla) {

        var fila = filasTabla[key];
        var idFila = fila.id;

        var numNuevasCol = 5;

        if (numF < 3) { //NUAVAS COLUMNAS PARA LAS 2 PRIMERAS FILAS

            if (numF == 1) {
                var tagFila1 = document.getElementById("1raFila");

                //agrego primera fila
                var nuevaCol1 = document.createElement('td');
                var cadena1 = 'Revisión Áreas COES';
                nuevaCol1.innerHTML = cadena1;
                nuevaCol1.style.textAlign = "center";
                nuevaCol1.style.border = "1px solid " + COLOR_BORDE;
                nuevaCol1.colSpan = 5;
                nuevaCol1.classList.add("campo_titulo_tab_areas");
                tagFila1.appendChild(nuevaCol1);
            }

            if (numF == 2) {
                //agrego segunda fila
                var tagFila2 = document.getElementById("2daFila");

                for (var i = 1; i <= numNuevasCol; i++) {
                    var nuevaCol2 = document.createElement('th');

                    nuevaCol2.classList.add("campo_cab_add_celdas_areas");
                    nuevaCol2.style.textAlign = "center";
                    nuevaCol2.style.width = "120px";
                    nuevaCol2.style.border = "1px solid " + COLOR_BORDE;
                    var cadena2 = '';
                    switch (i) {
                        case 1:
                            cadena2 = '<b style="text-aling: center;">Áreas</b>';
                            nuevaCol2.classList.add("celdaREVEstado_areas");
                            nuevaCol2.style.width = "55px";
                            break;
                        case 2:
                            cadena2 = 'Revisión Solicitud';
                            nuevaCol2.classList.add("celdaREV_areas");
                            break;
                        case 3:
                            cadena2 = 'Estado Solicitud';
                            nuevaCol2.classList.add("celdaREV_areas");
                            nuevaCol2.style.width = "55px";
                            break;
                        case 4:
                            cadena2 = 'Revisión Subsanación';
                            nuevaCol2.classList.add("celdaREV_areas");
                            break;
                        case 5:
                            cadena2 = 'Estado Subsanación';
                            nuevaCol2.classList.add("celdaREV_areas");
                            nuevaCol2.style.width = "55px";
                            break;
                    }

                    nuevaCol2.innerHTML = cadena2;
                    tagFila2.appendChild(nuevaCol2);
                }
            }

        } else { //filas con DATA DE LA TABLA
            if (idFila != null && idFila != "") {
                const myArray = idFila.split("_");
                let nomDiv = myArray[0];
                let miFevrqcodi = myArray[3];
                let esRevisable = myArray[4];

                var _fevrqcodi = null;
                if (!isNaN(miFevrqcodi))
                    _fevrqcodi = parseInt(miFevrqcodi);

                const registro = listaRevision.find(x => x.Fevrqcodi === _fevrqcodi);

                var tagFila = document.getElementById(idFila);

                for (var i = 1; i <= numNuevasCol; i++) {

                    //Agrego
                    var nuevaCol = document.createElement('td');
                    nuevaCol.style.border = "1px solid " + COLOR_BORDE;

                    var cadena = '';


                    ////////////////
                    if (nomDiv == "divCab") {
                        nuevaCol.style.background = COLOR_CAB_REQUISITO;

                    } else {
                        //para las filas donde 
                        if ((miFevrqcodi != "N")) {

                            if (registro != undefined) {
                                if (i == 1) {

                                    cadena = registro.AreasNombAsignadas;
                                    nuevaCol.classList.add("celdaREV_areas");
                                    nuevaCol.style.background = COLOR_BLOQUEADO;
                                    nuevaCol.setAttribute("id", "celdaContArea_" + i + "_" + miFevrqcodi);
                                }

                                if (i == 2) {
                                    cadena = obtenerHtmlAreasCeldaRevisionFilaBloqueada(i, miFevrqcodi, nuevaCol, registro.LstFilaRevision);
                                    nuevaCol.classList.add("celdaREV_areas");
                                    nuevaCol.setAttribute("id", "celdaContArea_" + i + "_" + miFevrqcodi);
                                }

                                if (i == 3) {
                                    cadena = obtenerHtmlAreasCeldaRevisionFilaBloqueada(i, miFevrqcodi, nuevaCol, registro.LstFilaRevision);
                                    nuevaCol.classList.add("celdaREV_areas");
                                    nuevaCol.setAttribute("id", "celdaContArea_" + i + "_" + miFevrqcodi);
                                }

                                if (i == 4) {
                                    cadena = obtenerHtmlAreasCeldaRevisionFilaBloqueada(i, miFevrqcodi, nuevaCol, registro.LstFilaRevision);
                                    nuevaCol.classList.add("celdaREV_areas");
                                    nuevaCol.setAttribute("id", "celdaContArea_" + i + "_" + miFevrqcodi);
                                }

                                if (i == 5) {

                                    cadena = obtenerHtmlAreasCeldaRevisionFilaBloqueada(i, miFevrqcodi, nuevaCol, registro.LstFilaRevision);
                                    nuevaCol.classList.add("celdaREV_areas");
                                    nuevaCol.setAttribute("id", "celdaContArea_" + i + "_" + miFevrqcodi);
                                }
                            }
                        }
                    }

                    nuevaCol.innerHTML = cadena;
                    tagFila.appendChild(nuevaCol);
                }

            }
        }
        numF++;
    }
    //createResizableTable(document.getElementById('tabla_OP'));  // Esto va cuando se visualiza la pantalla, como esta en otra pestaña, debe llamarse cuando se abra esa pestaña

}

function obtenerHtmlAreasCeldaRevisionFilaBloqueada(indiceColumnaBloque, fevrqcodi, columna, lstFilaRevision) {
    var cadena = "";

    columna.style.background = COLOR_BLOQUEADO; //coloreo Fondo
    if (indiceColumnaBloque != 3 && indiceColumnaBloque != 5) {
        cadena = mostrarTextoAreasHtml(indiceColumnaBloque, fevrqcodi, lstFilaRevision);
    }

    var styleFondo = 'background:' + COLOR_BLOQUEADO;

    if (indiceColumnaBloque == 3) {
        for (var i = 0; i < lstFilaRevision.length; i++) {
            var reg = lstFilaRevision[i];

            if (reg.EstadoSolicitado.length > 0) {
                var htmlAreaEstado = "";
                htmlAreaEstado = "<b>" + reg.Nombre + "</b>: ";
                htmlAreaEstado += reg.EstadoSolicitado;

                cadena += `
        <table style = "width: -webkit-fill-available;>
            <tr id="bloqueAreaTexto_${indiceColumnaBloque}_${fevrqcodi}">
                <td style='border: 0;${styleFondo}'>
                   <div id="datoAreaCelda_${indiceColumnaBloque}_${fevrqcodi}">
                     ${htmlAreaEstado}
                   </div>
                </td>
            </tr>
        </table>
        `;
            }
        }
    }

    if (indiceColumnaBloque == 5) {
        for (var i = 0; i < lstFilaRevision.length; i++) {
            var reg = lstFilaRevision[i];

            if (reg.EstadoSubsanado.length > 0) {
                var htmlAreaEstado = "";
                htmlAreaEstado = "<b>" + reg.Nombre + "</b>: ";
                htmlAreaEstado += reg.EstadoSubsanado;

                cadena += `
        <table style = "width: -webkit-fill-available;>
            <tr id="bloqueAreaTexto_${indiceColumnaBloque}_${fevrqcodi}">
                <td style='border: 0;${styleFondo}'>
                   <div id="datoAreaCelda_${indiceColumnaBloque}_${fevrqcodi}">
                     ${htmlAreaEstado}
                   </div>
                </td>
            </tr>
        </table>
        `;
            }
        }
    }

    return cadena;
}

var LISTA_HTML_POPUP_AREAS = [];

function mostrarTextoAreasHtml(indiceColumnaBloque, fevrqcodi, lstFilaRevision) {

    var styleFondo = 'background:' + COLOR_BLOQUEADO;
    var htmlDiv = '';

    if (indiceColumnaBloque == 2) {
        for (var i = 0; i < lstFilaRevision.length; i++) {
            var reg = lstFilaRevision[i];

            if (reg.MsgSolicitado.length > 0 || reg.ListaArchivosSolicitados.length > 0) {
                var textoHtmlObs = reg.MsgHtmlSolicitado ?? '';
                var textoSinEtiquetas = reg.MsgSolicitado;
                if (textoSinEtiquetas.length > 20) {
                    textoSinEtiquetas = textoSinEtiquetas.substring(0, 20) + " (...)";
                }

                var listaArchivos = reg.ListaArchivosSolicitados;
                var numArchivos = listaArchivos != null ? listaArchivos.length : 0;
                var htmlArchivos = ''; //
                var htmlArchivos2 = ''; //
                if (numArchivos > 0) {
                    htmlArchivos = generarTablaAreaListaBodyXObs(listaArchivos, false, indiceColumnaBloque, fevrqcodi);
                    htmlArchivos2 = generarTablaAreaListaBodyXObs(listaArchivos, true, indiceColumnaBloque, fevrqcodi);
                }
                var hayBtnVerMasLectura = (reg.ListaArchivosSolicitados != null ? reg.ListaArchivosSolicitados.length : 0) > 0 || textoSinEtiquetas.length > 20;
                var htmlVerMas = '';
                if (hayBtnVerMasLectura) {

                    var id_popup_area = `Bloque2_${Date.now()}_${fevrqcodi}_${i}`;

                    var objRevArea = {
                        Id: id_popup_area,
                        reg1: textoHtmlObs,
                        reg2: htmlArchivos2,
                    };
                    LISTA_HTML_POPUP_AREAS.push(objRevArea);

                    htmlVerMas = `                    
                       <a title="Ver registro" href="#" style="float: right;color: green;font-weight: bold;" onClick="popupFormularioRevAreas('${id_popup_area}');">Ver Más</a>
                    `;
                }

                var htmlAreaResp = "";
                htmlAreaResp = "<b>" + reg.Nombre + "</b>: ";
                htmlAreaResp += textoSinEtiquetas + htmlVerMas;

                htmlDiv += `
        <table style = "width: -webkit-fill-available;">
            <tr id="bloqueAreaTexto_${indiceColumnaBloque}_${fevrqcodi}">
                <td style='border: 0;${styleFondo}'>
                   <div id="datoAreaCelda_${indiceColumnaBloque}_${fevrqcodi}">
                     ${htmlAreaResp}
                   </div>
                   <div>
                        
                   </div>
                </td>
            </tr>
        </table>
        `;

                htmlDiv += `
        <div id="data_AreaArchivos_${indiceColumnaBloque}_${fevrqcodi}"> 
            <table>
                <tr>                
                        ${htmlArchivos}                
               </tr>
            </table>
        </div>
        `;
            }
        }
    }
    if (indiceColumnaBloque == 4) {
        for (var i = 0; i < lstFilaRevision.length; i++) {
            var reg = lstFilaRevision[i];

            if (reg.MsgSubsanado.length > 0 || reg.ListaArchivosSubsanados.length > 0) {
                var textoHtmlObs = reg.MsgHtmlSubsanado ?? '';
                var textoSinEtiquetas = reg.MsgSubsanado;
                if (textoSinEtiquetas.length > 20) {
                    textoSinEtiquetas = textoSinEtiquetas.substring(0, 20) + " (...)";
                }

                var listaArchivos = reg.ListaArchivosSubsanados;
                var numArchivos = listaArchivos != null ? listaArchivos.length : 0;
                var htmlArchivos = ''; //
                var htmlArchivos2 = ''; //
                if (numArchivos > 0) {
                    htmlArchivos = generarTablaAreaListaBodyXObs(listaArchivos, false, indiceColumnaBloque, fevrqcodi);
                    htmlArchivos2 = generarTablaAreaListaBodyXObs(listaArchivos, true, indiceColumnaBloque, fevrqcodi);
                }
                var hayBtnVerMasLectura = (listaArchivos != null ? listaArchivos.length : 0) > 0 || textoSinEtiquetas.length > 20;
                var htmlVerMas = '';
                if (hayBtnVerMasLectura) {

                    var id_popup_area = `Bloque4_${Date.now()}_${fevrqcodi}_${i}`;

                    var objRevArea = {
                        Id: id_popup_area,
                        reg1: textoHtmlObs,
                        reg2: htmlArchivos2,
                    };
                    LISTA_HTML_POPUP_AREAS.push(objRevArea);

                    htmlVerMas = `                    
                       <a title="Ver registro" href="#" style="float: right;color: green;font-weight: bold;" onClick="popupFormularioRevAreas('${id_popup_area}');">Ver Más</a>
                    `;
                }

                var htmlAreaResp = "";
                htmlAreaResp = "<b>" + reg.Nombre + "</b>: ";
                htmlAreaResp += textoSinEtiquetas + htmlVerMas;

                htmlDiv += `
        <table style = "width: -webkit-fill-available;">
            <tr id="bloqueAreaTexto_${indiceColumnaBloque}_${fevrqcodi}">
                <td style='border: 0;${styleFondo}'>
                   <div id="datoAreaCelda_${indiceColumnaBloque}_${fevrqcodi}" style=" word-break: break-all;">
                     ${htmlAreaResp}
                   </div>
                </td>
            </tr>
        </table>
        `;

                htmlDiv += `
        <div id="data_AreaArchivos_${indiceColumnaBloque}_${fevrqcodi}" > 
            <table>
                <tr>                
                        ${htmlArchivos}                
               </tr>
            </table>
        </div>
        `;
            }
        }
    }

    return htmlDiv;
}

function generarTablaAreaListaBodyXObs(listaArchivo, enPopup, nColumna, fevrqcodi) {

    listaArchivo.sort((x, y) => x.Ftearcnombreoriginal - y.Ftearcnombreoriginal); // ordenamieto

    var styleFondo = "";
    styleFondo = 'background:' + COLOR_BLOQUEADO + '; border: 0px; word-break: break-all;';

    var html = `
                <table border="0" class="pretty tabla-icono" cellspacing="0" style="width:auto" id="">
                    <tbody>`;


    for (var i = 0; i < listaArchivo.length; i++) {
        var item = listaArchivo[i];
        var idrow = "row" + i;
        var nomb = item.Ftearcnombreoriginal;

        var textoEnPopup = '';
        if (enPopup) {
            textoEnPopup = nomb;
        }

        html += `
                        <tr id="${idrow}">

                            <td onclick="verAreaArchivoXObs(${nColumna}, ${fevrqcodi},${i}, ${enPopup});" title='Visualizar archivo - ${nomb}' style="cursor: pointer;width:30px;${styleFondo}">
                                	&#128065;
                            </td>
                            <td onclick="descargarArchivoAreaXObs(${nColumna}, ${fevrqcodi},${i}, ${enPopup});" title='Descargar archivo - ${nomb}' style="cursor: pointer;width:30px;${styleFondo}">
                                <img width="15" height="15" src="../../Content/Images/btn-download.png" />
                            </td>
                            <td style="text-align:left;${styleFondo}" title='${nomb}'>
                                ${textoEnPopup}
                            </td>
        `;

        html += "       </tr>";
    }

    html += `
                    </tbody>
                </table>`;

    return html;
}

function popupFormularioRevAreas(idPopup) {
    var objPopup = LISTA_HTML_POPUP_AREAS.find(x => x.Id === idPopup);
    if (objPopup == null) {
        objPopup = {
            Id: '',
            reg1: '',
            reg2: '',
        };

    }
    limpiarBarraMensaje('mensajePopupCelda');

    tinymce.remove();
    $("#htmlArchivos").unbind();
    $("#htmlArchivos").html('');
    $("#idFormularioObservacion").html('');

    var msgHtml = objPopup.reg1;
    var archivosHtml = objPopup.reg2;

    var esEditable = false;
    var miReadonly = esEditable ? 0 : 1;
    var habilitacion = esEditable ? "" : 'disabled';

    var htmlDiv = '';
    htmlDiv += `
        <table style="width:100%">
            <tr>
	            <td class="registro-control" style="width:790px;">
		            <textarea name="Contenido" id="contenido_html_obs" maxlength="2000" cols="180" rows="22"  ${habilitacion}>
			            ${msgHtml}
		            </textarea>
	            </td>
            </tr>
        </table>

        <input type='hidden' id='hfXObs_Tipo' value='F3' />
        <div id='html_archivos_x_obs'></div>
    `;

    $('#idFormularioObservacion').html(htmlDiv);

    setTimeout(function () {
        //$("#popupFormularioObservacion .popup-title").html(objItemObs.Ccombnombre);
        $('#popupFormularioObservacion').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });

        //plugin html
        var idHtml = "#contenido_html_obs";
        tinymce.init({
            selector: idHtml,
            plugins: [
                //'paste textcolor colorpicker textpattern link table image imagetools preview'
                'wordcount advlist anchor autolink codesample colorpicker contextmenu fullscreen image imagetools lists link media noneditable preview  searchreplace table template textcolor visualblocks wordcount'
            ],
            toolbar1:
                //'insertfile undo redo | fontsizeselect bold italic underline strikethrough | forecolor backcolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link | table | image | mybutton | preview',
                'insertfile undo redo | styleselect fontsizeselect | forecolor backcolor | bullist numlist outdent indent | link | table | mybutton | preview',

            menubar: false,
            readonly: miReadonly,
            language: 'es',
            statusbar: false,
            convert_urls: false,
            plugin_preview_width: 790,
            setup: function (editor) {
                editor.on('change',
                    function () {
                        editor.save();
                    });
                editor.on("paste", function (e) {
                    var pastedData = e.clipboardData.getData('text/plain');
                    if (pastedData == "") {
                        e.preventDefault();
                        setTimeout(function () {
                            $("#inputText").html('');
                        }, 100)
                    }
                });
            }
        });

        //archivos      
        var prefijo = "_sec_doc_" + 5555;
        var htmlSec = "<div id=div_" + prefijo + ">";
        htmlSec += archivosHtml;
        htmlSec += "</div>";

        $("#html_archivos_x_obs").html(htmlSec);


    }, 50);

}

function verAreaArchivoXObs(nColumna, fevrqcodi, pos, enPopup) {

    var regArchivo;
    var tipoArchivo = TIPO_ARCHIVO_AREA_REVISION;
    var listaArchivo = [];

    if (enPopup) {
        //busco en la lista temporal
        regArchivo = listaArchivoRevAreaTemp[pos];
    } else {
        //Obtengo listado de archivo guardado segun columna y fila 
        var elemento = listaDataRevisionArea.find(x => x.Fevrqcodi === fevrqcodi);
        switch (nColumna) {
            case COLUMNA_REV_SOLICITADO: listaArchivo = elemento.ListaArchivosSolicitados; break;
            case COLUMNA_REV_SUBSANADO: listaArchivo = elemento.ListaArchivosSubsanados; break;
        }

        regArchivo = listaArchivo[pos];
    }

    if (regArchivo != null) {
        var nombreArchivo = regArchivo.Ftearcnombrefisico.toLowerCase();
        var esPdf = nombreArchivo.endsWith(".pdf");
        var esVistaPrevia = nombreArchivo.endsWith(".pdf") || nombreArchivo.endsWith(".xlsx") || nombreArchivo.endsWith(".docx") || nombreArchivo.endsWith(".xls") || nombreArchivo.endsWith(".doc");

        var metodoVistaPrevia = 'VistaPreviaArchivoEnvio';

        if (esVistaPrevia) {
            $.ajax({
                type: 'POST',
                url: controlador + metodoVistaPrevia,
                data: {
                    idEnvio: getObjetoFiltro().codigoEnvio,
                    idVersion: getObjetoFiltro().codigoVersion,
                    idElemento: fevrqcodi,
                    fileName: nombreArchivo,
                    tipoArchivo: tipoArchivo
                },
                success: function (model) {
                    if (model.Resultado != "") {

                        var rutaCompleta = window.location.href;
                        var ruraInicial = rutaCompleta.split("Equipamiento");
                        var urlPrincipal = ruraInicial[0];

                        var urlFrame = urlPrincipal + model.Resultado;
                        var urlFinal = "";
                        if (!esPdf) {
                            urlFinal = "https://view.officeapps.live.com/op/embed.aspx?src=" + urlFrame;
                        } else {
                            urlFinal = urlFrame;
                        }

                        setTimeout(function () {
                            $('#popupArchivoVistaPrevia').bPopup({
                                easing: 'easeOutBack',
                                speed: 450,
                                transition: 'slideDown',
                                modalClose: false
                            });

                            $('#vistaprevia').html('');
                            $("#vistaprevia").show();
                            $('#vistaprevia').attr("src", urlFinal);
                        }, 50);

                    } else {
                        alert("La vista previa solo permite archivos word, excel y pdf.");
                    }
                },
                error: function (err) {
                    alert("Ha ocurrido un error.");
                }
            });
        }
        else {
            alert("La vista previa solo permite archivos word, excel y pdf.");
        }
    }

    return true;
}

function descargarArchivoAreaXObs(nColumna, fevrqcodi, pos, enPopup) {
    var regArchivo;
    var tipoArchivo = TIPO_ARCHIVO_AREA_REVISION;
    var listaArchivo = [];

    if (enPopup) {
        //busco en la lista temporal
        regArchivo = listaArchivoRevAreaTemp[pos];
    } else {
        //Obtengo listado de archivo guardado segun columna y fila 
        var elemento = listaDataRevisionArea.find(x => x.Fevrqcodi === fevrqcodi);
        switch (nColumna) {
            case COLUMNA_REV_SOLICITADO: listaArchivo = elemento.ListaArchivosSolicitados; break;
            case COLUMNA_REV_SUBSANADO: listaArchivo = elemento.ListaArchivosSubsanados; break;
        }

        regArchivo = listaArchivo[pos];
    }

    if (regArchivo != null) {
        window.location = controlador + 'DescargarArchivoEnvio?fileName=' + regArchivo.Ftearcnombrefisico + "&fileNameOriginal=" + regArchivo.Ftearcnombreoriginal
            + '&idElemento=' + fevrqcodi + '&tipoArchivo=' + tipoArchivo + '&idEnvio=' + getObjetoFiltro().codigoEnvio + '&idVersion=' + getObjetoFiltro().codigoVersion;
    }
}

/**
 * Acciones de los botones
 */

/// Observar ///
async function validarYMostrarPopupObservar(ev) {
    limpiarBarraMensaje('mensajeEvento');
    limpiarBarraMensaje('mensaje_popupObservar');

    //Reseteo la fecha
    $("#obs_fecMaxRpta").val($("#obs_hffecMaxRpta").val());


    //valido si tiene permiso para accion
    var msg = validarPermisoAccionBotones(ev);
    if (msg == "") {
        //valido la informacion proporcionada
        var msg2 = await validarDataAObservar();
        if (msg2 == "") {
            abrirPopup("popupObservar");
        } else {
            mostrarMensaje('mensajeEvento', "alert", msg2);
        }
    } else {
        //alert(msg);
        mostrarMensaje('mensajeEvento', "alert", msg);
    }
}

async function validarDataAObservar() {
    //Si existen cambios sin guardar entonces realizar el autoguardado
    await guardarEquipoTemporalExcelWeb();

    var msj = "";
    var miEstenvcodi = $("#hfIdEstado").val();

    //Solo valida si es un envio solicitado y presiona Observar
    if (miEstenvcodi == ESTADO_SOLICITADO) {

        //Cuando en la columna Estado se haya seleccionado Observado y no se ingrese comentario
        if (OBJ_VALIDACION_ENVIO.LstSalidaObservarEnvio != null && OBJ_VALIDACION_ENVIO.LstSalidaObservarEnvio.length > 0) {
            mostrarDetalleErrores(OBJ_VALIDACION_ENVIO.LstSalidaObservarEnvio);

            msj += "<p>Existen validaciones pendientes de levantar.</p>";
        }
    }
    else {
        msj += "<p>Este envío no tiene permitido la acción de observar.</p>";
    }

    return msj;
}

function realizarObservacion() {
    limpiarBarraMensaje("mensajeEvento");
    limpiarBarraMensaje("mensaje_popupObservar");

    var filtro = datosFiltroObservacion();
    var msg = validarDatosFiltroObservacion(filtro);

    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarObservacionFT',
            dataType: 'json',
            contentType: "application/json",
            data: JSON.stringify({
                ftenvcodi: filtro.idEnvio,
                data: listaDataRevisionFT,
                fecmaxrpta: filtro.fecMaxRpta
            }),
            cache: false,
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    alert('Se realizó la observación correctamente.');
                    regresarPrincipal(ESTADO_OBSERVADO);
                } else {
                    mostrarMensaje('mensaje_popupObservar', 'error', 'Ha ocurrido un error: ' + evt.Mensaje);

                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_popupObservar', 'error', 'Ha ocurrido un error.');
            }
        });
    } else {
        mostrarMensaje('mensaje_popupObservar', 'error', msg);
    }
}

function datosFiltroObservacion() {
    var filtro = {};
    filtro.fecMaxRpta = $('#obs_fecMaxRpta').val();
    filtro.idEnvio = parseInt($('#hfIdEnvio').val()) || 0;

    return filtro;
}

function validarDatosFiltroObservacion(datos) {
    var msj = "";

    var strFechaHoy = diaActualSistema(); // en dd/mm/yyyy

    if (datos.idEnvio == 0) {
        msj += "<p>Código de envío no reconocido.</p>";
    }

    if (datos.fecMaxRpta == "") {
        msj += "<p>Debe escoger una fecha máxima de respuesta correcta.</p>";
    } else {
        if (convertirFecha(strFechaHoy) > convertirFecha(datos.fecMaxRpta)) {
            msj += "<p>Debe escoger una fecha correcta, la fecha debe ser posterior o igual al día actual (" + strFechaHoy + ").</p>";
        }

    }

    return msj;
}


/// DENEGAR ///
async function validarYMostrarPopupDenegar(ev) {
    limpiarBarraMensaje('mensajeEvento');
    limpiarBarraMensaje('mensaje_popupDenegar');

    //Reseteo la fecha
    $("#desaprob_mensajeCoes").val('Denegado');
    $("#desaprob_ccAgente").val($("#desaprob_hfccAgente").val());

    //valido si tiene permiso para accion
    var msg = validarPermisoAccionBotones(ev);
    if (msg == "") {
        //valido la informacion proporcionada
        var msg2 = await validarDataADenegar();
        if (msg2 == "") {
            abrirPopup("popupDenegar");
        } else {
            mostrarMensaje('mensajeEvento', "alert", msg2);
        }
    } else {
        mostrarMensaje('mensajeEvento', "alert", msg);
    }
}

async function validarDataADenegar() {
    //Si existen cambios sin guardar entonces realizar el autoguardado
    await guardarEquipoTemporalExcelWeb();

    var msj = "";
    var miEstenvcodi = $("#hfIdEstado").val();

    //Solo valida si es un envio subsanado y se presiona DENEGAR
    if (miEstenvcodi == ESTADO_SUBSANADO) {

        //Cuando en la columna Estado se haya seleccionado Observado y no se ingrese comentario
        if (OBJ_VALIDACION_ENVIO.LstSalidaDenegarEnvio != null && OBJ_VALIDACION_ENVIO.LstSalidaDenegarEnvio.length > 0) {
            mostrarDetalleErrores(OBJ_VALIDACION_ENVIO.LstSalidaDenegarEnvio);

            msj += "<p>Existen validaciones pendientes de levantar.</p>";
        }

    } else {
        msj += "<p>Este envío no tiene permitido la acción de denegar.</p>";
    }

    return msj;
}

function realizarDenegacion() {
    limpiarBarraMensaje("mensajeEvento");
    limpiarBarraMensaje("mensaje_popupDenegar");

    var filtro = datosFiltroDenegacion();
    var msg = validarDatosFiltroDenegacion(filtro);

    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarDenegacionFT',
            dataType: 'json',
            contentType: "application/json",
            data: JSON.stringify({
                ftenvcodi: filtro.idEnvio,
                data: listaDataRevisionFT,
                fecVigencia: filtro.fecVigencia,
                mensaje: filtro.Mensaje,
                ccAgentes: filtro.CCAgentes
            }),
            cache: false,
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    alert('Se realizó la denegación correctamente.');
                    regresarPrincipal(ESTADO_DESAPROBADO);
                } else {
                    mostrarMensaje('mensaje_popupDenegar', 'error', 'Ha ocurrido un error: ' + evt.Mensaje);

                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_popupDenegar', 'error', 'Ha ocurrido un error.');
            }
        });
    } else {
        mostrarMensaje('mensaje_popupDenegar', 'error', msg);
    }
}

function datosFiltroDenegacion() {
    var filtro = {};

    filtro.fecVigencia = "";
    filtro.Mensaje = $('#desaprob_mensajeCoes').val();
    filtro.CCAgentes = $("#desaprob_ccAgente").val();
    filtro.idEnvio = parseInt($('#hfIdEnvio').val()) || 0;

    return filtro;
}

function validarDatosFiltroDenegacion(datos) {
    var msj = "";
    var paraAgente = $("#desaprob_hfparaAgente").val();


    var numCaracteres = datos.Mensaje.length;
    if (numCaracteres > 500) {
        msj += "<p>El mensaje debe tener como máximo 500 caracteres.</p>";
    }

    if (paraAgente == "") {
        msj += "<p>El sistema no encontró agente del último evento.</p>";
    }

    /*validacion del campo CC*/
    var validaCc = validarCorreo(datos.CCAgentes, 0, -1);
    if (validaCc < 0) {
        msj += "<p>Error encontrado en el campo CC Agentes. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
    }

    if (datos.idEnvio <= 0) {
        msj += "<p>Código de envío no reconocido.</p>";
    }


    return msj;
}

/////////////////////////////////////
///////         APROBAR       ///////
/////////////////////////////////////
async  function validarYMostrarPopupAprobar(ev) {
    limpiarBarraMensaje('mensajeEvento');
    limpiarBarraMensaje('mensaje_popupAprobar');

    //Reseteo la fecha
    $("#aprob_fecVigencia").val('');
    $("#aprob_enlaceSistema").val($("#aprob_hfenlaceSistema").val());
    $("#aprob_enlaceCarta").val('');
    $("#aprob_enlaceOtro").val('');
    $("#aprob_ccAgente").val($("#aprob_hfccAgente").val());

    //valido si tiene permiso para accion
    var msg = validarPermisoAccionBotones(ev);
    if (msg == "") {
        //valido la informacion proporcionada
        var msg2 = await validarDataAAprobar();
        if (msg2 == "") {
            abrirPopup("popupAprobar");
        } else {
            mostrarMensaje('mensajeEvento', "alert", msg2);
        }
    } else {
        mostrarMensaje('mensajeEvento', "alert", msg);
    }
}

async function validarDataAAprobar() {
    //Si existen cambios sin guardar entonces realizar el autoguardado
    await guardarEquipoTemporalExcelWeb();

    var msj = "";

    var miEstenvcodi = $("#hfIdEstado").val();

    //Solo valida si es un envio solicitado o subsanado y se presiona APROBAR
    if (miEstenvcodi == ESTADO_SOLICITADO || miEstenvcodi == ESTADO_SUBSANADO) {

        //Cuando en la columna Estado se haya seleccionado Observado y no se ingrese comentario
        if (OBJ_VALIDACION_ENVIO.LstSalidaAprobarEnvio != null && OBJ_VALIDACION_ENVIO.LstSalidaAprobarEnvio.length > 0) {
            mostrarDetalleErrores(OBJ_VALIDACION_ENVIO.LstSalidaAprobarEnvio);

            msj += "<p>Existen validaciones pendientes de levantar.</p>";
        }

    } else {
        msj += "<p>Este envío no tiene permitido la acción de aprobación.</p>";
    }

    return msj;
}

function realizarAprobacion() {
    limpiarBarraMensaje("mensajeEvento");
    limpiarBarraMensaje("mensaje_popupAprobar");

    var filtro = datosFiltroAprobacion();
    var msg = validarDatosFiltroAprobacion(filtro);

    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarAprobacionFT',
            dataType: 'json',
            contentType: "application/json",
            data: JSON.stringify({
                ftenvcodi: filtro.idEnvio,
                data: listaDataRevisionFT,
                fecVigencia: filtro.fecVigencia,
                enlaceSI: filtro.enlaceSI,
                enlaceCarta: filtro.enlaceCarta,
                enlaceOtro: filtro.enlaceOtro,
                idCV: "",
                ccAgentes: filtro.CCAgentes,
                hayParamVacios: 0,
                opcionReemplazo: ""
            }),
            cache: false,
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    alert('Se realizó la aprobación correctamente.');
                    regresarPrincipal(ESTADO_APROBADO);
                } else {
                    mostrarMensaje('mensaje_popupAprobar', 'error', 'Ha ocurrido un error: ' + evt.Mensaje);

                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_popupAprobar', 'error', 'Ha ocurrido un error.');
            }
        });
    } else {
        mostrarMensaje('mensaje_popupAprobar', 'error', msg);
    }
}

function datosFiltroAprobacion() {
    var filtro = {};

    filtro.fecVigencia = $('#aprob_fecVigencia').val();
    filtro.enlaceSI = $('#aprob_enlaceSistema').val();
    filtro.enlaceCarta = $('#aprob_enlaceCarta').val();
    filtro.enlaceOtro = $('#aprob_enlaceOtro').val();
    filtro.CCAgentes = $("#aprob_ccAgente").val();
    filtro.idEnvio = parseInt($('#hfIdEnvio').val()) || 0;

    return filtro;
}

function validarDatosFiltroAprobacion(datos) {
    var msj = "";
    var paraAgente = $("#aprob_hfparaAgente").val();

    var strFechaHoy = diaActualSistema(); // en dd/mm/yyyy

    if (esFechaValida(datos.fecVigencia)) {
        if (convertirFecha(strFechaHoy) > convertirFecha(datos.fecVigencia)) {
            msj += "<p>Debe escoger una fecha de vigencia correcta.	La Fecha Vigencia esta fuera del rango permitido (" + strFechaHoy + " para adelante).</p>";
        }
    } else {
        msj += "<p>Debe ingresar fecha de vigencia correcta (en formato dd//mm/yyyy).</p>";
    }

    var enlaceSI = datos.enlaceSI.trim();
    var enlaceCarta = datos.enlaceCarta.trim();
    var enlaceOtro = datos.enlaceOtro.trim();

    //si hay enlaces, verifico que tengan menos de 500 caracteres
    var numCaracteresSI = enlaceSI.length;
    if (numCaracteresSI > 500 && numCaracteresSI > 0) {
        msj += "<p>El primer enlace debe tener como máximo 500 caracteres.</p>";
    }
    var numCaracteresCarta = enlaceCarta.length;
    if (numCaracteresCarta > 500 && numCaracteresCarta > 0) {
        msj += "<p>El segundo enlace debe tener como máximo 500 caracteres.</p>";
    }
    var numCaracteresOtro = enlaceOtro.length;
    if (numCaracteresOtro > 500 && numCaracteresOtro > 0) {
        msj += "<p>El tercer enlace debe tener como máximo 500 caracteres.</p>";
    }

    if (paraAgente == "") {
        msj += "<p>El sistema no encontró agente del último evento.</p>";
    }

    /*validacion del campo CC*/
    var validaCc = validarCorreo(datos.CCAgentes, 0, -1);
    if (validaCc < 0) {
        msj += "<p>Error encontrado en el campo CC Agentes. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
    }

    if (datos.idEnvio <= 0) {
        msj += "<p>Código de envío no reconocido.</p>";
    }

    //valida la existencia de espacios en los enlaces
    var arregloSI = enlaceSI.split(' ');
    var arregloCarta = enlaceCarta.split(' ');
    var arregloOtros = enlaceOtro.split(' ');

    if (arregloSI.length >= 2) {
        msj += "<p>El primer enlace no debe contener espacios.</p>";
    }
    if (arregloCarta.length >= 2) {
        msj += "<p>El segundo enlace no debe contener espacios.</p>";
    }
    if (arregloOtros.length >= 2) {
        msj += "<p>El tercer enlace no debe contener espacios.</p>";
    }
    return msj;
}

////////////////////////////////////////////////
// Util
////////////////////////////////////////////////

function validarCorreo(cadena, minimo, maximo) {
    var arreglo = cadena.split(';');
    var nroCorreo = 0;
    var longitud = arreglo.length;

    if (longitud == 0) {
        arreglo = cadena;
        longitud = 1;
    }

    for (var i = 0; i < longitud; i++) {

        var email = arreglo[i].trim();
        var validacion = validarDirecccionCorreo(email);

        if (validacion) {
            nroCorreo++;
        } else {
            if (email != "")
                return -1;
        }
    }

    if (minimo > nroCorreo)
        return -1;

    if (maximo > 0 && nroCorreo > maximo)
        return -1;

    return 1;
}


/// DERIVAR ///
function validarYMostrarPopupDerivar(ev) {
    limpiarBarraMensaje('mensajeEvento');
    limpiarBarraMensaje('mensaje_popupDerivar');

    ////Reseteo la fecha
    var fecDer = $("#hdFechaDerivacion").val();
    $("#deriv_fecmaxrpta").val(fecDer);

    //valido si tiene permiso para accion
    var msg = validarPermisoAccionBotones(ev);
    if (msg == "") {
        //valido la informacion proporcionada
        var msg2 = validarDataADerivar();
        if (msg2 == "") {
            abrirPopup("popupDerivar");
        } else {
            mostrarMensaje('mensajeEvento', "alert", msg2);
        }
    } else {
        mostrarMensaje('mensajeEvento', "alert", msg);
    }
}


function validarDataADerivar() {
    var msj = "";
    var miEstenvcodi = parseInt($("#hfIdEstado").val()) || 0;

    //Solo valida si es un envio solicitado y presiona Observar
    if (miEstenvcodi == ESTADO_SOLICITADO || miEstenvcodi == ESTADO_SUBSANADO) {

        //Reviso si la version ya fue derivada
        var flagVersionDerivada = parseInt($("#hdVersionDerivada").val()) || 0;

        if (flagVersionDerivada == 1) {
            msj += "<p>Esta versión del envío ya fue derivada a las áreas correspondientes para su revisión.</p>";
        }

    }
    else {
        msj += "<p>Este envío no tiene permitido la acción de derivar.</p>";
    }

    return msj;
}


function realizarDerivacion() {
    limpiarBarraMensaje("mensajeEvento");
    limpiarBarraMensaje("mensaje_popupDerivar");

    var msgDer = "";
    var estadoRegreso = 1; //usado para regresar y asi provocar refrescar pantalla
    var miEstenvcodi = parseInt($("#hfIdEstado").val()) || 0;
    if (miEstenvcodi == ESTADO_SOLICITADO) {
        msgDer = "¿Desea derivar el envío solicitado a las áreas COES?";
        estadoRegreso = ESTADO_SOLICITADO;
    }
    if (miEstenvcodi == ESTADO_SUBSANADO) {
        msgDer = "¿Desea derivar el envío subsanado a las áreas COES?";
        estadoRegreso = ESTADO_SUBSANADO;
    }


    var filtro = datosFiltroDerivar();
    var msg = validarDatosFiltroDerivacion(filtro);

    if (msg == "") {
        if (confirm(msgDer)) {
            $.ajax({
                type: 'POST',
                url: controlador + 'GrabarDerivacionFT',
                dataType: 'json',
                contentType: "application/json",
                data: JSON.stringify({
                    ftenvcodi: filtro.idEnvio,
                    fecmaxrpta: filtro.fecMaxRpta,
                    data: listaDataRevisionFT
                }),
                cache: false,
                success: function (evt) {
                    if (evt.Resultado != "-1") {
                        cerrarPopup("popupDerivar");
                        alert('Se efectuó la derivación a las áreas COES correctamente.');
                        regresarPrincipal(estadoRegreso);

                    } else {
                        mostrarMensaje('mensaje_popupDerivar', 'error', 'Ha ocurrido un error: ' + evt.Mensaje);

                    }
                },
                error: function (err) {
                    mostrarMensaje('mensaje_popupDerivar', 'error', 'Ha ocurrido un error.');
                }
            });
        }
    } else {
        mostrarMensaje('mensaje_popupDerivar', 'error', msg);
    }

}


function datosFiltroDerivar() {
    var filtro = {};

    filtro.idEnvio = parseInt($('#hfIdEnvio').val()) || 0;
    filtro.fecMaxRpta = $('#deriv_fecmaxrpta').val();

    return filtro;
}

function validarDatosFiltroDerivacion(datos) {
    var msj = "";

    var strFechaHoy = diaActualSistema(); // en dd/mm/yyyy

    if (esFechaValida(datos.fecMaxRpta)) {
        if (convertirFecha(strFechaHoy) > convertirFecha(datos.fecMaxRpta)) {
            msj += "<p>Debe escoger una fecha máxima de respuesta correcta.	La Fecha Máxima de Respuesta esta fuera del rango permitido (" + strFechaHoy + " para adelante).</p>";
        }
    } else {
        msj += "<p>Debe ingresar fecha máxima de respuesta correcta (en formato dd//mm/yyyy).</p>";
    }

    return msj;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Errores
////////////////////////////////////////////////////////////////////////////////////////////////////////////

function mostrarDetalleErrores(listaError) {

    $("#idTerrores").html('');

    var htmlTerrores = dibujarTablaError(listaError);
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
            //, "oLanguage": { "sZeroRecords": "", "sEmptyTable": "" }
        });

    }, 200);
}

function dibujarTablaError(listaError) {

    var cadena = `
        <div style='clear:both; height:5px'></div>
            <table id='tablaError' border='1' class='pretty tabla-adicional' cellspacing='0' style=''>
                <thead>
                    <tr>
                        <th style='width: 10px;'>#</th>
                        <th style='width: 75px;'>Requisito</th>
                        <th style='width: 200px;'>Tipo Error</th>
                    </tr>
                </thead>
                <tbody>
    `;

    for (var i = 0; i < listaError.length; i++) {
        var item = listaError[i];
        var n = i + 1;

        cadena += `
                    <tr>
                        <td style='width:  10px; white-space: inherit;'>${n}</td>
                        <td style='width:  75px; white-space: inherit;'>${item.Celda}</td>
                        <td style='width: 200px; white-space: inherit;'>${item.TipoError}</td>
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

//Expandir - Restaurar
function expandirRestaurar() {
    if ($('#hfExpandirContraer').val() == "E") {
        expandirExcel();
        //calculateSize2(1);
        expandirExcelImagen();

        //parte izquierda
        $("#html_archivos").css("width", ($(window).width() - 20) + "px");
        $("#html_archivos").css("height", ($(window).height() - 180) + "px");

        //parte derecha
        $(".campo_cab_add_celdas.celdaREV").css("width", 85 + "px");
        $(".campo_cab_add_celdas.celdaREVEstado").css("width", 55 + "px");
        $(".campo_cab_add_celdas_areas.celdaREV_areas").css("width", 85 + "px");
        $(".campo_cab_add_celdas_areas.celdaREVEstado_areas").css("width", 55 + "px");
    }
    else {
        restaurarExcel();
        //calculateSize2(2);
        restaurarExcelImagen();

        //parte izquierda
        $("#html_archivos").css("width", WIDTH_FORMULARIO + "px");
        $("#html_archivos").css("height", HEIGHT_FORMULARIO + "px");

        //parte derecha
        $(".campo_cab_add_celdas.celdaREV").css("width", 120 + "px");
        $(".campo_cab_add_celdas.celdaREVEstado").css("width", 55 + "px");
        $(".campo_cab_add_celdas_areas.celdaREV_areas").css("width", 120 + "px");
        $(".campo_cab_add_celdas_areas.celdaREVEstado_areas").css("width", 55 + "px");

        _irAFooterPantalla();
    }
}

function expandirExcelImagen() {
    $('#hfExpandirContraer').val("C");
    $('#spanExpandirContraer').text('Restaurar');

    var img = $('#imgExpandirContraer').attr('src');
    var newImg = img.replace('expandir.png', 'contraer.png');
    $('#imgExpandirContraer').attr('src', newImg);
}

function restaurarExcelImagen() {
    $('#hfExpandirContraer').val("E");
    $('#spanExpandirContraer').text('Expandir');

    var img = $('#imgExpandirContraer').attr('src');
    var newImg = img.replace('contraer.png', 'expandir.png');
    $('#imgExpandirContraer').attr('src', newImg);

    WIDTH_FORMULARIO = $(window).width() - 50;
}

function expandirExcel() {
    $('#idpanel').addClass("divexcel");
    //hot.render();
}

function restaurarExcel() {

    $('#tophead').css("display", "none");
    $('#detExcel').css("display", "block");
    $('#idpanel').removeClass("divexcel");
    $('#itemExpandir').css("display", "block");
    $('#itemRestaurar').css("display", "none");

    //hot.render();
}