var controlador = siteRoot + 'Equipamiento/Envio/';

$(function () {
    $('#btnInicio').click(function () {
        regresarPrincipal();
    });

    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').easytabs('select', '#tabLista');

    $('#tab-container').bind('easytabs:after', function (event, $clicked, $targetPanel, settings) {
        //el tab origen es tabDetalle y el destino es tabLista entonces realizar autoguardado
        if ($targetPanel.selector == '#tabLista') {
            if (getObjetoFiltro().fteeqcodi > 0) //siempre debe estar visible un formulario
                guardarFormularioCargaDatos(ACCION_GUARDADO_SISTEMA);
        }
    });

    HEIGHT_FORMULARIO = $(window).height() - 300;

    //barra de herramientas (izquierda)
    $('#btnAutoguardar').click(function () {
        guardarFormularioCargaDatos(ACCION_GUARDADO_MANUAL);
    });
    $('#btnLimpiar').click(function () {
        limpiarEquipoTemporalExcelWeb();
    });
    $('#btnHistorialAutoguardado').click(function () {
        mostrarHistorialAutoguardados();
    });

    //barra de herramientas (derecha)
    $('#btnDescargar').click(function () {
        exportarFormatoConexIntegModif();
    });
    crearPuploadFormato3();

    $('#btnHabilitarAddEquipo').click(function () {
        mostrarListadoEquiposM();
    });
    $('#btnAceptarM').click(function () {
        actualizarVersionAddEquipo();
    });

    $('#btnEnviarDatos').click(function () {
        enviarExcelWeb();
    });
    $('#btnMostrarErrores').click(function () {
        //validarEnvio();
        mostrarDetalleErrores();
    });
    $('#btnVerEnvio').click(function () {
        mostrarEnviosAnteriores();
    });
    $('#btnExpandirRestaurar').click(function () {
        expandirRestaurar();
    });

    //mostrar opción
    var codigoEtapa = parseInt($("#hfFtetcodi").val()) || 0;
    if (ETAPA_MODIFICACION != codigoEtapa)
        $(".filtro_proyecto").css("display", "table-cell");
    $(".div_herr_filtro").css("display", "table-cell");
    $("#btnExpandirRestaurar").parent().css("display", "table-cell");

    //tabla web
    OPCION_GLOBAL_EDITAR = $("#hfTipoOpcion").val() == "E";
    FLAG_AUTOGUARDADO_HABILITADO = parseInt($('#hdHabilitarAutoguardado').val()) == 1;

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

    //Muestra fec max rpta
    var infoFMR = $("#hdNotaFecMaxRpta").val();
    if (infoFMR != "" && infoFMR != undefined && infoFMR != null) {
        var carpeta = parseInt($("#hfIdEstado").val());
        if (carpeta == ESTADO_OBSERVADO) {
            $("#idFecMaxRpta").html(`<b>Plazo Máximo de Respuesta:</b>  ${infoFMR}`);
            $("#idFecMaxRpta").show();
        }
    }

    if (OPCION_GLOBAL_EDITAR)
        $("#nota_").css("display", "block");
    else
        $("#nota_").css("display", "none");

    //verificar autoguardado
    verificarAutoguardadoYlistarEq();

    //activar autoguardado si está habilitado
    activarIntervaloAutoguardado();
});

async function regresarPrincipal() {
    EXISTE_CAMBIO_SIN_GUARDAR = false;

    //verificar que haya cambios
    await ValidarCambiosSinGuardar();

    if (EXISTE_CAMBIO_SIN_GUARDAR) {
        if (confirm("¿Desea guardar los cambios ingresados antes de salir de la ventana?")) {
            await guardarFormularioCargaDatos(ACCION_GUARDADO_MANUAL);
        }
    }
    var estadoEnvio = parseInt($("#hfIdEstado").val()) || 0;
    document.location.href = controlador + "Index?carpeta=" + estadoEnvio;
}

function getObjetoFiltro() {
    var filtro = {};

    filtro.fteeqcodi = parseInt($("#hfIdEquipoEnvio").val()) || 0;
    filtro.accion = parseInt($("#hdAccion").val()) || 0;

    filtro.codigoEnvio = parseInt($("#hfIdEnvio").val()) || 0;
    filtro.codigoVersion = parseInt($("#hfIdVersion").val());
    filtro.fteeqcodisLimpiar = $("#hfFteeqcodisLimpiar").val();

    filtro.codigoEmpresa = parseInt($("#hfEmprcodi").val()) || 0;
    filtro.codigoEtapa = parseInt($("#hfFtetcodi").val()) || 0;
    filtro.codigoProyecto = parseInt($("#hfFtprycodi").val()) || 0;
    filtro.codigoEquipos = $("#hfCodigoEquipos").val();
    filtro.codigoEquiposSeleccionado = listarCodigoYTipoChecked();
    filtro.estado = parseInt($("#hfIdEstado").val()) || 0;
    filtro.claveCookie = $("#hdClaveCookie").val() || '';

    return filtro;
}

//FORMULARIO
function listarEquipoEnvioConexIntegModif() {
    _limpiarBarraMensaje_('mensajeEvento');
    $('#tab-container').easytabs('select', '#tabLista');

    var versionEnvio = parseInt($("#hfIdVersion").val());
    var filtro = getObjetoFiltro();

    $.ajax({
        type: 'POST',
        url: controlador + "ListarEqConexIntegModifXEnvio",
        data: {
            codigoEnvio: filtro.codigoEnvio,
            versionEnvio: versionEnvio
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                LISTA_VERSIONES = evt.ListaVersion;
                LISTA_AUTOGUARDADO = evt.ListaAutoguardado;
                LISTA_ERRORES = evt.ListaErrores;

                $("#hfFteeqcodis").val(evt.LstFteeqcodis);
                $("#hfFteeqcodinomb").val(evt.LstEnviosEqNombres);
                $("#hfCodigoEquipos").val(evt.CodigoEquipos);

                var htmlTabla = dibujarTablaEquipoEnvioConexIntegModif(evt.ListaEnvioEq, filtro.codigoEnvio);
                $("#listadoDetalleEquipo").html(htmlTabla);

                $('#tablaDetalleEquipo').dataTable({
                    "scrollY": 350,
                    "scrollX": false,
                    "sDom": 'ft',
                    "ordering": false,
                    "iDisplayLength": -1
                });

                setTimeout(function () {
                    $("#chkSeleccionar").on("click", function () {
                        var check = $('#chkSeleccionar').is(":checked");
                        $("input[name=chkSeleccion]").prop("checked", check);
                    });
                }, 50);

                //barra de herramientas
                if (OPCION_GLOBAL_EDITAR) {
                    $(".barra_herramienta_envio").show();
                    $("#leyenda_alerta").show();

                    $("#btnHistorialAutoguardado").parent().css("display", "table-cell");
                    $("#btnAutoguardar").parent().css("display", "table-cell");
                    $("#btnLimpiar").parent().css("display", "table-cell");

                    $("#btnDescargar").parent().css("display", "table-cell");
                    $("#btnImportar").parent().css("display", "table-cell");
                    $("#btnEnviarDatos").parent().css("display", "table-cell");
                    $("#btnMostrarErrores").parent().css("display", "table-cell");
                    $("#btnHabilitarAddEquipo").parent().css("display", "table-cell"); //visible en carpeta observado
                } else {
                    $(".barra_herramienta_envio").show();
                    $("#btnDescargar").parent().css("display", "table-cell");
                    $("#btnVerEnvio").parent().css("display", "table-cell");
                }

                if (filtro.codigoEnvio <= 0 || filtro.estado == ESTADO_SOLICITADO || filtro.estado == ESTADO_OBSERVADO) {
                    $("#btnHistorialAutoguardado").parent().css("display", "table-cell");
                }

                var ftEditada = $("#hdFTEditadaUsuario").val();
                if (ftEditada == "S") {
                    _mostrarMensaje_('mensajeEvento', 'alert', "La Ficha Técnica de un equipo fue modificado (agregó, eliminó o desactivó items) por lo que el envío no puede seguir con el flujo habitual.");
                }

            } else {

                _mostrarMensaje_('mensajeEvento', $tipoMensajeMensaje, evt.Mensaje);
            }
        },
        error: function (err) {
            _mostrarMensaje_('mensajeEvento', 'error', 'Ha ocurrido un error.');
        }
    });
}

function dibujarTablaEquipoEnvioConexIntegModif(lista) {
    var cadena = '';

    var thSelec = '';
    if (OPCION_GLOBAL_EDITAR) thSelec = `<th style="width: 40px">Sel. Todos <br/> <input type="checkbox" id="chkSeleccionar"> </th>`;

    var thAccion = '';
    var flagHabilitado = parseInt($("#hdHabEditEq").val()) || 0;
    if (flagHabilitado == 1) thAccion = `<th style="width: 40px">Acción</th>`;

    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaDetalleEquipo" cellspacing="0"  >
        <thead>            
            <tr style="height: 30px;">
                ${thSelec}
                <th style="width: 40px">Ficha Técnica</th>
                <th style="width: 40px">Código</th>
                <th style="width: 150px">Empresa <br>Titular</th>
                <th style="width: 150px">Empresa <br>Copropietaria</th>
                <th style="width: 100px">Tipo de <br>equipo / categoría</th>
                <th style="width: 75px">Ubicación</th>
                <th style="width: 125px">Nombre</th>
                ${thAccion}
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

        var htmlquitarEq = '';
        if (flagHabilitado) {
            htmlquitarEq = `
                <td style="width: 40px">
                    <a href="#"  onclick="quitarEquipo(${item.Fteeqcodi});">${IMG_CANCELAR}</a>
                </td>
            `;
        }

        cadena += `
           <tr >
                ${tdSelec}
                <td style="width: 40px">
                    <a href="#"  onclick="redireccionarFormulario(${item.Fteeqcodi});">
                       ${IMG_EDITAR_FICHA}
                    </a>
                </td>
                <td style="width: 40px">${item.Idelemento}</td>
                <td style="width: 150px">${item.Nombempresaelemento}</td>
                <td style="width: 150px">${item.Nombempresacopelemento}</td>
                <td style="width: 100px">${item.Tipoelemento}</td>
                <td style="width: 75px">${item.Areaelemento}</td>
                <td style="width: 125px">${item.Nombreelemento}</td>
                ${htmlquitarEq}
           </tr >    
        `;

        fila++;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function redireccionarFormulario(dEquipoEnvio) {
    var existeVersionSeleccionada = parseInt($("#hfNumVersionSeleccionadaPopup").val()) || 0;


    $('#tab-container').easytabs('select', '#tabDetalle');

    $("#hfIdEquipoEnvio").val(dEquipoEnvio);

    //FormularioExcelWeb
    listarFicha(existeVersionSeleccionada > 0 ? OPCION_FORMULARIO_CAMBIO_ENTRE_VERSION : OPCION_FORMULARIO_ACTUAL_BD);
}

function listarCodigoYTipoChecked() {
    if (!OPCION_GLOBAL_EDITAR) return "-1";

    var selected = [];
    $('input[name=chkSeleccion]').each(function () {
        if ($(this).is(":checked")) {
            selected.push($(this).attr('id'));
        }
    });

    return selected.join(",");
}

function listarFicha(opcionFormulario) {
    //pausar automatico
    detenerIntervaloAutoguardado();

    _limpiarBarraMensaje_('mensajeEvento');
    $("#detalle_ficha_tecnica").hide();
    $("#detalle_ficha_tecnica").html("");

    var filtro = getObjetoFiltro();

    var versA = parseInt($("#hfVersionAnteriorPopup").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerDatosFT",
        data: {
            fteeqcodi: filtro.fteeqcodi,
            fteeqcodisLimpiar: filtro.fteeqcodisLimpiar,
            versionAnterior: versA,
            tipoForm: opcionFormulario
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                var lstConfiguraciones = evt.ReporteDatoXEq.ListaItemConfig
                _determinarColumnasAdicionales(lstConfiguraciones);

                MODELO_FICHA = evt.ReporteDatoXEq;

                CODIGO_ENVIO = filtro.codigoEnvio;
                CODIGO_VERSION = MODELO_FICHA.Ftevercodi;

                listaDataRevisionFT = evt.ListaRevisionParametrosAFT;

                //Inicializar vista previa
                $("#detalle_ficha_tecnica").unbind();
                $("#detalle_ficha_tecnica").show();
                $("#detalle_ficha_tecnica").css("width", anchoPortal + "px");
                $("#detalle_ficha_tecnica").css("height", HEIGHT_FORMULARIO + "px");
                $("#detalle_ficha_tecnica").html(_extranet_generarHtmlReporteDetalleFichaTecnica(MODELO_FICHA));

                //Eventos javascript de carga de archivos
                for (var i = 0; i < MODELO_FICHA.ListaAllItems.length; i++) {
                    var reg = MODELO_FICHA.ListaAllItems[i];

                    //valor
                    if (reg.EsArchivo) {
                        pUploadArchivo(reg.Ftitcodi, TIPO_ARCHIVO_VALOR_DATO, getIdPrefijoPuploadFT(reg, TIPO_ARCHIVO_VALOR_DATO));
                    } else {
                        //evento al pegar
                        $("#campo_input_" + reg.Ftitcodi).bind('paste', function (e) {
                            var ctl = $(this);
                            setTimeout(function () {
                                var texto = (ctl.val() ?? "").trim();
                                ctl.val(texto);
                            }, 100);
                        });

                        $("#campo_input_anotacion_" + reg.Ftitcodi).bind('paste', function (e) {
                            var ctl = $(this);
                            setTimeout(function () {
                                var texto = (ctl.val() ?? "").trim();
                                ctl.val(texto);
                            }, 100);
                        });
                    }

                    //sustento
                    if (reg.EsArchivoAdjuntado) {
                        pUploadArchivo(reg.Ftitcodi, TIPO_ARCHIVO_SUSTENTO_DATO, getIdPrefijoPuploadFT(reg, TIPO_ARCHIVO_SUSTENTO_DATO));
                    }
                }

                $('#btnGuardarAnotacion').unbind();
                $('#btnGuardarAnotacion').click(function () {
                    _extranet_guardarTemporalmenteAnotacion();
                });

                //barra de herramientas
                if (OPCION_GLOBAL_EDITAR) {
                    $(".barra_herramienta_envio").show();
                    $("#btnEnviarDatos").parent().css("display", "table-cell");
                    $("#btnMostrarErrores").parent().css("display", "table-cell");
                } else {
                    $(".barra_herramienta_envio").show();
                    $("#btnDescargar").parent().css("display", "table-cell");
                    $("#btnVerEnvio").parent().css("display", "table-cell");
                }

                //bloque de revisión
                if (getObjetoFiltro().codigoEnvio > 0 && getObjetoFiltro().estado != ESTADO_SOLICITADO && getObjetoFiltro().estado != ESTADO_CANCELADO)
                    agregarBloqueRevision(listaDataRevisionFT);

                //volver activar automatico
                activarIntervaloAutoguardado();

                //que la pantalla vaya al fondo para mostrar la mayor cantidad de elementos del formulario
                _irAFooterPantalla();
            } else {

                mostrarMensaje_('mensaje_popupProyecto', $tipoMensajeMensaje, evt.Mensaje);
            }
        },
        error: function (err) {
            _mostrarMensaje_('mensaje_popupProyecto', 'error', 'Ha ocurrido un error.');
        }
    });
}

function agregarBloqueRevision(lista) {
    //solo mostrar bloque amarillo si el equipo fue revisado en Intranet
    if (lista.length > 0) {
        agregarColumnasRevision(lista);
    }
}

function agregarColumnasRevision(listaRevision) {
    var filasTabla = document.getElementById("reporte").rows;

    var miFtetcodi = parseInt($("#hfFtetcodi").val()) || 0;

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
                var cadena1 = 'Proceso de Revisión';
                nuevaCol1.innerHTML = cadena1;
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
                    nuevaCol2.style.width = "200px";
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
                let numeral = myArray[0];
                let miFtitcodi = myArray[1];
                let miPropcodi = myArray[2];
                let miConcepcodi = myArray[3];
                let esArchivo = myArray[4];

                var _ftitcodi = null;
                var _propcodi = null;
                var _concepcodi = null;
                if (!isNaN(miFtitcodi)) {
                    _ftitcodi = parseInt(miFtitcodi);
                }
                if (!isNaN(miPropcodi))
                    _propcodi = parseInt(miPropcodi);
                if (!isNaN(miConcepcodi))
                    _concepcodi = parseInt(miConcepcodi);

                const registro = listaRevision.find(x => x.Ftitcodi === _ftitcodi);

                var tagFila = document.getElementById(idFila);

                for (var i = 1; i <= numNuevasCol; i++) {

                    //Agrego
                    var nuevaCol = document.createElement('td');

                    var cadena = '';

                    //para las filas donde 
                    if ((miPropcodi != "N" || miConcepcodi != "N")) {

                        if (registro != undefined) {
                            //Verifico si bloqueo toda la fila
                            if (registro.FilaBloqueada) {

                                switch (i) {
                                    case 1: cadena = obtenerHtmlCeldaRevisionFilaBloqueada(i, miFtitcodi, nuevaCol, registro.Fteeqcodi, registro.ValObsCoes, registro.ListaArchivosObsCoes); nuevaCol.classList.add("celdaREV"); nuevaCol.setAttribute("id", "celdaCont_" + i + "_" + miFtitcodi); break;
                                    case 2: cadena = obtenerHtmlCeldaRevisionFilaBloqueada(i, miFtitcodi, nuevaCol, registro.Fteeqcodi, registro.ValRptaAgente, registro.ListaArchivosRptaAgente); nuevaCol.classList.add("celdaREV"); nuevaCol.setAttribute("id", "celdaCont_" + i + "_" + miFtitcodi); break;
                                    case 3: cadena = obtenerHtmlCeldaRevisionFilaBloqueada(i, miFtitcodi, nuevaCol, registro.Fteeqcodi, registro.ValRptaCoes, registro.ListaArchivosRptaCoes); nuevaCol.classList.add("celdaREV"); nuevaCol.setAttribute("id", "celdaCont_" + i + "_" + miFtitcodi); break;
                                    case 4: nuevaCol.style.background = COLOR_BLOQUEADO; //coloreo Fondo
                                        cadena = mostrarListadoHtml(CELDA_REV_VER, registro.ValEstado, registro.ListaEstados, miPropcodi, miConcepcodi, miFtitcodi, registro.Fteeqcodi, INTRANET);
                                        nuevaCol.classList.add("celdaREVEstado");
                                        break;
                                }


                            } else { //Verifico por cada Columna                           

                                var regEnLista = listaDataRevisionFT.find(x => x.Fteeqcodi === registro.Fteeqcodi && x.Ftitcodi === registro.Ftitcodi);

                                if (i == 1) {

                                    if (regEnLista != null) {
                                        if (regEnLista.NumcolumnaEditada == i) {
                                            registro.ValObsCoes = regEnLista.MsgColumnaEditada;
                                            registro.ListaArchivosObsCoes = regEnLista.ListaArchivosObsCoes;
                                        }
                                    }
                                    cadena = obtenerHtmlCeldaRevisionFilaNoBloqueada(i, miFtitcodi, nuevaCol, registro.CeldaObsCoesEstaBloqueada, registro.Fteeqcodi, registro.ValObsCoes, regEnLista.IdValorEstado, registro.ListaArchivosObsCoes);
                                    nuevaCol.classList.add("celdaREV");
                                    nuevaCol.setAttribute("id", "celdaCont_" + i + "_" + miFtitcodi);
                                }
                                if (i == 2) {
                                    if (regEnLista != null) {
                                        if (regEnLista.NumcolumnaEditada == i) {
                                            registro.ValRptaAgente = regEnLista.MsgColumnaEditada;
                                            registro.ListaArchivosRptaAgente = regEnLista.ListaArchivosRptaAgente;
                                        }
                                    }
                                    cadena = obtenerHtmlCeldaRevisionFilaNoBloqueada(i, miFtitcodi, nuevaCol, registro.CeldaRptaAgenteEstaBloqueada, registro.Fteeqcodi, registro.ValRptaAgente, regEnLista.IdValorEstado, registro.ListaArchivosRptaAgente);
                                    nuevaCol.classList.add("celdaREV");
                                    nuevaCol.setAttribute("id", "celdaCont_" + i + "_" + miFtitcodi);
                                }
                                if (i == 3) {
                                    if (regEnLista != null) {
                                        if (regEnLista.NumcolumnaEditada == i) {
                                            registro.ValRptaCoes = regEnLista.MsgColumnaEditada;
                                            registro.ListaArchivosRptaCoes = regEnLista.ListaArchivosRptaCoes;
                                        }
                                    }
                                    cadena = obtenerHtmlCeldaRevisionFilaNoBloqueada(i, miFtitcodi, nuevaCol, registro.CeldaRptaCoesEstaBloqueada, registro.Fteeqcodi, registro.ValRptaCoes, regEnLista.IdValorEstado, registro.ListaArchivosRptaCoes);
                                    nuevaCol.classList.add("celdaREV");
                                    nuevaCol.setAttribute("id", "celdaCont_" + i + "_" + miFtitcodi);
                                }
                                if (i == 4) {
                                    if (regEnLista != null) {

                                        registro.ValEstado = regEnLista.IdValorEstado;

                                    }
                                    if (registro.CeldaEstadoEstaBloqueada) {
                                        nuevaCol.style.background = COLOR_BLOQUEADO; //coloreo Fondo
                                        cadena = mostrarListadoHtml(CELDA_REV_VER, registro.ValEstado, registro.ListaEstados, miPropcodi, miConcepcodi, miFtitcodi, registro.Fteeqcodi, INTRANET);
                                    } else {//esta desbloqueada en la extranet
                                        cadena = mostrarListadoHtml(CELDA_REV_EDITAR, registro.ValEstado, registro.ListaEstados, miPropcodi, miConcepcodi, miFtitcodi, registro.Fteeqcodi, INTRANET);
                                    }
                                    nuevaCol.classList.add("celdaREVEstado");
                                }
                            }
                        }

                    } else {
                        var n;
                        if (!isNaN(numeral)) {
                            n = parseFloat(numeral)
                            var s = Number.isInteger(n)

                            if (s) {
                                nuevaCol.style.background = COLOR_NUMERAL;
                            } else {
                                nuevaCol.style.background = COLOR_BLOQUEADO;
                            }
                        } else
                            nuevaCol.style.background = COLOR_BLOQUEADO;


                    }

                    nuevaCol.innerHTML = cadena;
                    tagFila.appendChild(nuevaCol);
                }


            }
        }
        numF++;
    }

    createResizableTable(document.getElementById('reporte'));

}

function obtenerHtmlCeldaRevisionFilaNoBloqueada(indiceColumnaBloque, ftitcodi, columna, esCeldaBloqueada, fteeqcodi, valorCelda, opcionCombo, listaArchivos) {
    var cadena = "";
    if (esCeldaBloqueada) {
        columna.style.background = COLOR_BLOQUEADO; //coloreo Fondo
        if (valorCelda != null && valorCelda != "") {
            cadena = mostrarTextoHtml(indiceColumnaBloque, ftitcodi, CELDA_REV_VER, fteeqcodi, valorCelda, listaArchivos, false);
        } else {
            cadena = mostrarTextoHtml(indiceColumnaBloque, ftitcodi, CELDA_REV_VER, fteeqcodi, "", listaArchivos, false);
        }
    } else {//esta desbloqueada en la intranet
        //if (valorCelda == null || valorCelda == "") {
        cadena = mostrarTextoHtml(indiceColumnaBloque, ftitcodi, CELDA_REV_EDITAR, fteeqcodi, valorCelda, listaArchivos, false);
        //}
    }
    return cadena;
}

function obtenerHtmlCeldaRevisionFilaBloqueada(indiceColumnaBloque, ftitcodi, columna, fteeqcodi, valorCelda, listaArchivos) {
    var cadena = "";
    columna.style.background = COLOR_BLOQUEADO; //coloreo Fondo
    if (valorCelda != null && valorCelda != "") {
        cadena = mostrarTextoHtml(indiceColumnaBloque, ftitcodi, CELDA_REV_VER, fteeqcodi, valorCelda, listaArchivos, false);
    } else {
        cadena = mostrarTextoHtml(indiceColumnaBloque, ftitcodi, CELDA_REV_VER, fteeqcodi, "", listaArchivos, false);
    }
    return cadena;
}

function mostrarTextoHtml(indiceColumnaBloque, ftitcodi, accion, fteeqcodi, msgHtml, listaArchivos, agregoCssParaOcultarCelda) {

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
                       <a title="Editar registro" href="#" style="float: right;" onClick=" mostrarPopupObservacion(${indiceColumnaBloque}, ${fteeqcodi}, ${ftitcodi},${accion},'${msgCodificado}');">${IMG_EDITAR} </a>
                `;
    } else {
        if (accion == CELDA_REV_VER) {
            styleFondo = 'background:' + COLOR_BLOQUEADO;

            //solo se muestra el boton Ver si hay comentario
            var hayBtnVerMasLectura = (listaArchivos != null ? listaArchivos.length : 0) > 0 || tamTexto > 20;
            if (hayBtnVerMasLectura) {
                htmlBtn = `
                       <a title="Ver registro" href="#" style="float: right;color: green;font-weight: bold;" onClick="mostrarPopupObservacion(${indiceColumnaBloque}, ${fteeqcodi}, ${ftitcodi},${accion},'${msgCodificado}');">Ver Más</a>
                `;
            } else {
                htmlBtn = `
                `;
            }
        }
    }

    var numArchivos = listaArchivos != null ? listaArchivos.length : 0;
    var htmlArchivos = ''; //
    if (numArchivos > 0) htmlArchivos = generarTablaListaBodyXObs(listaArchivos, accion, false, indiceColumnaBloque, fteeqcodi, ftitcodi);

    var htmlDiv = '';

    if (agregoCssParaOcultarCelda) {
        htmlDiv += `
        <table style = "width: -webkit-fill-available;">
            <tr id="bloqueTexto_${indiceColumnaBloque}_${ftitcodi}" style="display: none;">
                <td style='border: 0;${styleFondo}'>
                   <div id="datoCelda_${indiceColumnaBloque}_${ftitcodi}" >
                        ${htmlEditado} 
                   </div>
                   <div>
                        ${htmlBtn}
                   </div>
                </td>
            </tr>
            
        </table>
        `;
    } else {
        htmlDiv += `
        <table style = "width: -webkit-fill-available;">
            <tr id="bloqueTexto_${indiceColumnaBloque}_${ftitcodi}">
                <td style='border: 0;${styleFondo}'>
                   <div id="datoCelda_${indiceColumnaBloque}_${ftitcodi}">
                        ${htmlEditado}
                   </div>
                   <div>
                        ${htmlBtn}
                   </div>
                </td>
            </tr>
        </table>
        `;
        htmlDiv += `
        <div id="data_Archivos_${indiceColumnaBloque}_${ftitcodi}">
            <table>
                <tr>                
                        ${htmlArchivos}                
               </tr>
            </table>
        </div>
        `;
    }

    return htmlDiv;
}

function mostrarListadoHtml(accion, valEstado, listado, miPropcodi, miConcepcodi, miFtitcodi, fteeqcodi, interfaz) {
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

    //cambio estilo del combo seleccionado
    var colorEstado = obtenerColorTextoSegunEstado(valEstado);


    //textoEstado = "No Subsanado";    

    //Para solicitud e intranet, se debe mostrar vacio en Estado
    if (valEstado == "" && miEstenvcodi == ESTADO_SOLICITADO && interfaz == INTRANET) {
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
            <select ${htmlHabilitacion} name="" id="ckbEstado_${miFtitcodi}_${miPropcodi}_${miConcepcodi}" onchange="CambiarOpcionCombo('${miPropcodi}','${miConcepcodi}', '${miFtitcodi}','${fteeqcodi}');" 
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

function CambiarOpcionCombo(miPropcodi, miConcepcodi, miFtitcodi, miFteeqcodi) {
    var miEstenvcodi = $("#hfIdEstado").val();
    var opcionSeleccionada = $("#ckbEstado_" + miFtitcodi + "_" + miPropcodi + "_" + miConcepcodi).val();

    //cambio estilo del combo seleccionado
    var colorTexto = obtenerColorTextoSegunEstado(opcionSeleccionada);
    $("#ckbEstado_" + miFtitcodi + "_" + miPropcodi + "_" + miConcepcodi).css("color", colorTexto);

    //reemplazo el IDVALOR en el array el valor del seleccionado
    var ftitcodi = parseInt(miFtitcodi) || 0;
    var fteeqcodi = parseInt(miFteeqcodi) || 0;
    const registro = listaDataRevisionFT.find(x => x.Ftitcodi === ftitcodi && x.Fteeqcodi === fteeqcodi);
    var numCol = 0;
    var pos = -1;
    if (registro != null) {
        pos = listaDataRevisionFT.findIndex(function (x) { return x.Ftitcodi === ftitcodi && x.Fteeqcodi === fteeqcodi; });
        numCol = listaDataRevisionFT[pos].NumcolumnaEditada;
        listaDataRevisionFT[pos].IdValorEstado = opcionSeleccionada;
    }

    if (miEstenvcodi == ESTADO_SOLICITADO) { //Solo para los envios SOLICITADOS de intranet (los SUBSANADOS no bloquean celda de la 3ra columna de revision) 
        if (esString(opcionSeleccionada)) {
            //Si se selecciona CONFORME en INTRANET deshabilito la fila
            if (opcionSeleccionada.trim() == OpcionConforme) {
                $("#celdaCont_" + numCol + "_" + ftitcodi).css("background", COLOR_BLOQUEADO);
                $("#bloqueTexto_" + numCol + "_" + ftitcodi).css("display", "none");

                if (pos != -1) {
                    listaDataRevisionFT[pos].MsgColumnaEditada = "";
                    $("#datoCelda_" + numCol + "_" + ftitcodi).html("");
                }

            } else {
                $("#celdaCont_" + numCol + "_" + ftitcodi).css("background", "#FFFFFF");
                $("#bloqueTexto_" + numCol + "_" + ftitcodi).css("display", "table-row");
            }
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

//Versión temporal
async function verificarAutoguardadoYlistarEq() {
    if (OPCION_GLOBAL_EDITAR && FLAG_AUTOGUARDADO_HABILITADO) {
        var idEnvioTemporal = $("#hfIdEnvioTemporal").val();
        var idVersionTemporal = $("#hfIdVersionTemporal").val();
        var flagEquipoAutoguardado = parseInt($("#hfFlagEquipoAutoguardado").val()) || 0;

        $("#hfIdEnvio").val(idEnvioTemporal);
        $("#hfIdVersion").val(idVersionTemporal);

        if (flagEquipoAutoguardado == 1) {
            if (confirm("Hay información previamente llenada para el envío a realizar con los mismos equipos ¿Desea precargarla?")) {
                //mostrar los datos del autoguardado
                ULTIMO_MENSAJE_AUTOGUARDADO = HAY_AUTOGUARDADO_Y_USO_INFO;
            } else {
                //mostrar los datos por defectos del autoguardo (se aplica un "limpiar")
                ULTIMO_MENSAJE_AUTOGUARDADO = HAY_AUTOGUARDADO_Y_NO_USO_INFO;
            }
        }

        //crear version o actualizar la versión temporal
        VERSION_TEMPORAL_CREADO_OK = false;
        await generarVersionTemporal();

        if (VERSION_TEMPORAL_CREADO_OK) {
            listarEquipoEnvioConexIntegModif();
        }
    } else {
        //solo lectura
        listarEquipoEnvioConexIntegModif();
    }
}

async function generarVersionTemporal() {
    //pausar automatico
    detenerIntervaloAutoguardado();

    return $.ajax({
        type: 'POST',
        url: controlador + "CrearVersionTemporal",
        dataType: 'json',
        data: {
            codigoEnvio: getObjetoFiltro().codigoEnvio,
            codigoVersion: getObjetoFiltro().codigoVersion,
            codigoEmpresa: getObjetoFiltro().codigoEmpresa,
            codigoEtapa: getObjetoFiltro().codigoEtapa,
            codigoProyecto: getObjetoFiltro().codigoProyecto,
            codigoEquipos: getObjetoFiltro().codigoEquipos,
            esLimpiarVersion: ULTIMO_MENSAJE_AUTOGUARDADO == HAY_AUTOGUARDADO_Y_NO_USO_INFO,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                VERSION_TEMPORAL_CREADO_OK = true;

                $("#hfIdEnvio").val(evt.IdEnvio);
                $("#hfIdEnvioTemporal").val(evt.IdEnvio);
                $("#hfIdVersion").val(evt.IdVersionTemporal);
                $("#hfFteeqcodisLimpiar").val(evt.FteeqcodisLimpiar);

                //si la versión es cero pero ya se crea un envio/versión temporal
                if (evt.ClaveCookie != null && evt.ClaveCookie != "")
                    $("#hdClaveCookie").val(evt.ClaveCookie);

                //volver activar automatico
                activarIntervaloAutoguardado();
            } else {
                OPCION_GLOBAL_EDITAR = false;
                FLAG_AUTOGUARDADO_HABILITADO = false;

                alert(evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

//Enviar Excel web
async function enviarExcelWeb() {
    //si el usuario no da clic en guardar manual
    await guardarFormularioCargaDatos(ACCION_GUARDADO_SISTEMA, true);

    _limpiarBarraMensaje_("mensajeEvento");

    if (LISTA_ERRORES.length > 0) {
        //Valido el envio dado que se trata de subsanacion
        if (getObjetoFiltro().codigoEnvio > 0) {
            msg = validarDataASubsanar();
        }

        mostrarDetalleErrores();
    } else {
        if (confirm("¿Desea enviar información a COES?  Una vez enviado, se procederá con la revisión por parte de COES.")) {

            //pausar automatico
            detenerIntervaloAutoguardado();

            $.ajax({
                type: 'POST',
                url: controlador + "EnviarSolicitudConexIntegModif",
                data: {
                    codigoEnvio: getObjetoFiltro().codigoEnvio,
                    versionEnvio: getObjetoFiltro().codigoVersion,
                },
                success: function (evt) {
                    if (evt.Resultado != "-1") {
                        alert("Se envió la información correctamente");

                        //regresar a la pantalla principal
                        $("#hfIdEstado").val(evt.IdEstado);
                        var estadoEnvio = parseInt($("#hfIdEstado").val()) || 0;
                        document.location.href = controlador + "Index?carpeta=" + estadoEnvio;
                    } else {
                        var fechahora = obtenerFechaHoraActual();
                        _mostrarMensaje_('mensaje_GuardadoTemporal', 'error', fechahora + " - Llegó la información del agente al servidor COES, pero ocurrió un problema al guardarlo.");
                        guardarCookie(getObjetoFiltro().claveCookie, TIPO_COOKIE_ENVIAR_CON_CONEXION);

                        //volver activar automatico
                        activarIntervaloAutoguardado();
                    }
                },
                error: function (err) {
                    var fechahora = obtenerFechaHoraActual();
                    _mostrarMensaje_('mensaje_GuardadoTemporal', 'error', fechahora + " - Se perdió la conexión agente/servidor COES, no llegó la información al servidor.");
                    guardarCookie(getObjetoFiltro().claveCookie, TIPO_COOKIE_ENVIAR_SIN_CONEXION);

                    //volver activar automatico
                    activarIntervaloAutoguardado();
                }
            });
        }
    }
}

function validarDataASubsanar() {
    var msj = "";

    var ftEditada = $("#hdFTEditadaUsuario").val();
    if (ftEditada == "S") {
        msj += "<p>La Ficha Técnica de un equipo fue modificado (agregó, eliminó o desactivó items) por lo que el envío no puede seguir con el flujo habitual.</p>";
    }

    var miEstenvcodi = $("#hfIdEstado").val();

    //Solo valida si es un envio observado y presiona ENVIAR
    if (miEstenvcodi == ESTADO_OBSERVADO) {

        //Cuando en la columna Estado se haya seleccionado Observado y no se ingrese comentario
        if (LISTA_ERRORES != null && LISTA_ERRORES.length > 0) {
            msj += "<p>Existen validaciones pendientes de levantar.</p>";
        }
    }
    else {
        msj += "<p>Este envío no tiene permitido la acción de subsanar.</p>";
    }

    return msj;
}

//agregar equipos
function mostrarListadoEquiposM() {
    $("#listadoEquiposM").html("");

    var filtro = {};
    filtro.idempresa = getObjetoFiltro().codigoEmpresa;
    filtro.etapaid = getObjetoFiltro().codigoEtapa;
    filtro.proyectoid = getObjetoFiltro().codigoProyecto;
    filtro.codigoEquipos = getObjetoFiltro().codigoEquipos;

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerListadoEquiposCIMO",
        data: {
            emprcodi: filtro.idempresa,
            ftetcodi: filtro.etapaid,
            ftprycodi: filtro.proyectoid,
            codigoEquipoNoSeleccionable: filtro.codigoEquipos,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                var htmlL = dibujarTablaEquipoEnvioM(evt.ListaEquipoEnvio);
                $("#listadoEquiposM").html(htmlL);

                abrirPopup("popupEquiposM");

                setTimeout(function () {
                    $('#tablaDetalleEquipoM').dataTable({
                        "scrollY": 300,
                        "scrollX": false,
                        "sDom": 'ft',
                        "ordering": false,
                        "iDisplayLength": -1
                    });
                }, 150);

                //Toda la columna cambia (al escoger casilla cabecera)
                $('input[type=checkbox][name^="chkMTodo"]').unbind();
                $('input[type=checkbox][name^="chkMTodo"]').change(function () {
                    var valorCheck = $(this).prop('checked');
                    $("input[type=checkbox][id^=chkM_]").each(function () {
                        $("#" + this.id).prop("checked", valorCheck);
                    });
                });

            } else {
                mostrarMensaje('mensaje_popupNuevo', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje_popupNuevo', 'error', 'Ha ocurrido un error al mostrar equipos para la etapa Modificación.');
        }
    });
}

function dibujarTablaEquipoEnvioM(lista) {
    var cadena = '';

    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaDetalleEquipoM" cellspacing="0"  >
        <thead>            
            <tr style="height: 30px;">
                <th style="width: 40px">Sel. Todos <br/> <input type="checkbox" id="chkMTodo" name="chkMTodo"> </th>
                
                <th style="width: 40px">Código</th>
                <th style="width: 150px">Empresa <br>Titular</th>
                <th style="width: 150px">Empresa <br>Copropietaria</th>
                <th style="width: 180px">Tipo de <br>equipo / categoría</th>
                <th style="width: 160px">Ubicación</th>
                <th style="width: 180px">Nombre</th>
            </tr>
        </thead>
        <tbody>
    `;

    var fila = 1;
    for (key in lista) {
        var item = lista[key];
        var seleccionable = item.CheckSeleccionableEnNuevo;
        var color = "";

        var tdSelec = `<input type="checkbox" value="${item.TipoYCodigo}" data-catecodifila="${item.Catecodi}" name="chkM_${item.TipoYCodigo}" id="chkM_${item.TipoYCodigo}" />`;

        cadena += `
           <tr >
        `;

        if (seleccionable) {
            cadena += `
                <td style="width: 40px; white-space: inherit;">
                    ${tdSelec}
                </td>
            `;
        } else {
            color = "#C0C1C2";
            cadena += `
                <td style="background: ${color}; white-space: inherit; width: 40px">
                    
                </td>
            `;
        }
        cadena += `
                
                <td style="background: ${color}; white-space: inherit; width: 40px">${item.Codigo}</td>
                <td style="background: ${color}; white-space: inherit; width: 150px">${item.EmpresaNomb}</td>
                <td style="background: ${color}; white-space: inherit; width: 150px">${item.EmpresaCoNomb}</td>
                <td style="background: ${color}; white-space: inherit; width: 180px">${item.Tipo}</td>
                <td style="background: ${color}; white-space: inherit; width: 160px">${item.Ubicacion}</td>
                <td style="background: ${color}; white-space: inherit; width: 180px">${item.EquipoNomb}</td>
        `;

        cadena += `
           </tr >    
        `;

        fila++;
    }
    cadena += "</tbody></table>";

    return cadena;
}

async function actualizarVersionAddEquipo() {
    //si el usuario no da clic en guardar manual
    await guardarFormularioCargaDatos(ACCION_GUARDADO_SISTEMA, true);

    //equipos seleccionados del popup
    let arrSeleccionados = [];
    $('input[type=checkbox][name^="chkM_"]:checked').each(function () {
        arrSeleccionados.push(this.value);
    });
    let strSeleccionados = arrSeleccionados.join(",");

    return $.ajax({
        type: 'POST',
        url: controlador + "AgregarEquipoVersion",
        dataType: 'json',
        data: {
            codigoEnvio: getObjetoFiltro().codigoEnvio,
            codigoVersion: getObjetoFiltro().codigoVersion,
            codigoEquipos: strSeleccionados,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                cerrarPopup('popupEquiposM');

                //actualizar listado de equipos
                listarEquipoEnvioConexIntegModif();
            } else {
                alert(evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

//quitar equipos
async function quitarEquipo(dEquipoEnvio) {
    //si el usuario no da clic en guardar manual
    await guardarFormularioCargaDatos(ACCION_GUARDADO_SISTEMA, true);

    if (confirm("¿Está seguro de eliminar el equipo para este envío?")) {

        return $.ajax({
            type: 'POST',
            url: controlador + "QuitarEquipo",
            dataType: 'json',
            data: {
                fteeqcodi: dEquipoEnvio
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {

                    alert("Se eliminió el equipo correctamente");

                    //actualizar listado de equipos
                    listarEquipoEnvioConexIntegModif();
                } else {
                    alert(evt.Mensaje);
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
}

//Descargar formato
function exportarFormatoConexIntegModif() {

    if (_esValidoFormularioExportarImportar()) {
        //pausar automatico
        detenerIntervaloAutoguardado();

        $.ajax({
            type: 'POST',
            url: controlador + "GenerarFormatoConexIntegModif",
            data: {
                codigoEnvio: getObjetoFiltro().codigoEnvio,
                fteeqcodis: getObjetoFiltro().codigoEquiposSeleccionado,
                fteeqcodisLimpiar: getObjetoFiltro().fteeqcodisLimpiar,
                versionEnvio: getObjetoFiltro().codigoVersion
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    window.location = controlador + "Exportar?file_name=" + evt.Resultado;
                } else {
                    alert("Ha ocurrido un error: " + evt.Mensaje);
                }

                //volver activar automatico
                activarIntervaloAutoguardado();
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    } else {
        alert("No es posible descargar. Debe seleccionar uno o más equipos.");
    }
}

function _esValidoFormularioExportarImportar() {
    var esValidoForm = true;

    esValidoForm = listarCodigoYTipoChecked() != '';

    return esValidoForm;
}

//Importar formato
function crearPuploadFormato3() {
    var uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnImportar',
        url: controlador + "UploadFormatoConexIntegModif",
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '5mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx,xls" }
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                if (uploader.files.length == 2) {
                    uploader.removeFile(uploader.files[0]);
                }
                uploader.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                //mostrarEventoAlerta(numHoja, "Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                leerFileUpExcel();
            },
            Error: function (up, err) {
                //mostrarEventoError(numHoja, err.code + "-" + err.message);
            }
        }
    });

    uploader.init();
}

function leerFileUpExcel() {
    //pausar automatico
    detenerIntervaloAutoguardado();

    OPCION_IMPORTACION = true;

    //cuando se importar no debe autoguardarse el formulario y este debe ocultarse
    $('#tab-container').easytabs('select', '#tabLista');
    $("#detalle_ficha_tecnica").html("");
    MODELO_FICHA = null;
    listaDataRevisionFT = [];

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: controlador + 'LeerFileUpExcelFormatoConexIntegModif',
        data: {
            codigoEnvio: getObjetoFiltro().codigoEnvio,
            estado: getObjetoFiltro().estado,
            versionEnvio: getObjetoFiltro().codigoVersion,
            mensajeAutoguardado: ULTIMO_MENSAJE_AUTOGUARDADO
        },
        success: function (evt) {
            var fechahora = obtenerFechaHoraActual();

            if (evt.Resultado != "-1") {
                _mostrarMensaje_('mensajeEvento', 'exito', "Se ha cargado correctamente el archivo.");

                if (evt.Resultado == "1") {
                    _mostrarMensaje_('mensaje_GuardadoTemporal', 'exito', fechahora + " - Autoguardado correctamente.");

                    LISTA_AUTOGUARDADO = evt.ListaAutoguardado;
                    LISTA_ERRORES = evt.ListaErrores;
                }

                ULTIMO_MENSAJE_AUTOGUARDADO = '';
            }
            else {
                _mostrarMensaje_('mensajeEvento', "error", evt.Mensaje);
                //alert("Ha ocurrido un error en la importación del archivo excel.");
            }

            //volver activar automatico
            activarIntervaloAutoguardado();
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

//AUTOGUARDADO
async function guardarFormularioCargaDatos(tipoAutoguardado, flagAutomError) {
    //Solo guardar cuando este activa la opción "editar envio" y el usuario es agente
    if (OPCION_GLOBAL_EDITAR && FLAG_AUTOGUARDADO_HABILITADO && !FLAG_PAUSAR_AUTOGUARDADO) {

        //pausar automatico
        detenerIntervaloAutoguardado();

        var msjErrorSistema = obtenerCookie(getObjetoFiltro().claveCookie);

        var listaItem = generarModeloFicha();

        var modeloWeb = {
            Ftenvcodi: getObjetoFiltro().codigoEnvio,
            Ftevercodi: getObjetoFiltro().codigoVersion,
            Fteeqcodi: getObjetoFiltro().fteeqcodi,
            FteeqcodisLimpiar: getObjetoFiltro().fteeqcodisLimpiar,
            HayPendiente1erAutoguardado: NUM_AUTOGUARDADO == 0,
            ListaTreeData: listaItem,
            TipoAutoguardado: tipoAutoguardado,
            MensajeAutoguardado: ULTIMO_MENSAJE_AUTOGUARDADO,
            MensajeNoConexion: msjErrorSistema,
            ListaRevision: listaDataRevisionFT
        };

        //siempre hay autoguardado así no hay abierto un formulario
        await $.ajax({
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
                var fechahora = obtenerFechaHoraActual();

                if (evt.Resultado != "-1") {
                    NUM_AUTOGUARDADO++;

                    //se asume que el mensaje de error previamente en cookie fue guardado correctamente en el servidor
                    eliminarCookie(getObjetoFiltro().claveCookie);

                    if (evt.Resultado == "1") { //hubo cambios
                        $("#hfFteeqcodisLimpiar").val(""); //se realizó el limpiar de los equipo
                        if (ACCION_GUARDADO_MANUAL == tipoAutoguardado)
                            _mostrarMensaje_('mensaje_GuardadoTemporal', 'exito', fechahora + " - Se generó una copia de autoguadado exitosamente.");

                        LISTA_AUTOGUARDADO = evt.ListaAutoguardado;
                        LISTA_ERRORES = evt.ListaErrores;
                    } else {
                        if (ACCION_GUARDADO_MANUAL == tipoAutoguardado)
                            _mostrarMensaje_('mensaje_GuardadoTemporal', 'exito', fechahora + " - No se generó una copia de autoguadado debido a que no hubo cambios de datos.");
                    }

                    ULTIMO_MENSAJE_AUTOGUARDADO = '';
                } else {
                    if (ACCION_GUARDADO_MANUAL == tipoAutoguardado || (ACCION_GUARDADO_SISTEMA == tipoAutoguardado && flagAutomError)) {
                        var tipoError = TIPO_COOKIE_GUARDAR_CON_CONEXION;
                        if (ACCION_GUARDADO_SISTEMA == tipoAutoguardado && flagAutomError) tipoError = TIPO_COOKIE_ENVIAR_CON_CONEXION;
                        _mostrarMensaje_('mensaje_GuardadoTemporal', 'error', fechahora + " - Llegó la información del agente al servidor COES, pero ocurrió un problema al guardarlo.");
                        guardarCookie(getObjetoFiltro().claveCookie, tipoError);
                    }
                }

                //volver activar automatico
                activarIntervaloAutoguardado();
            },
            error: function (err) {
                var fechahora = obtenerFechaHoraActual();
                if (ACCION_GUARDADO_MANUAL == tipoAutoguardado || (ACCION_GUARDADO_SISTEMA == tipoAutoguardado && flagAutomError)) {
                    var tipoError = TIPO_COOKIE_GUARDAR_SIN_CONEXION;
                    if (ACCION_GUARDADO_SISTEMA == tipoAutoguardado && flagAutomError) tipoError = TIPO_COOKIE_ENVIAR_SIN_CONEXION;
                    _mostrarMensaje_('mensaje_GuardadoTemporal', 'error', fechahora + " - Se perdió la conexión agente/servidor COES, no llegó la información al servidor.");
                    guardarCookie(getObjetoFiltro().claveCookie, tipoError);
                }

                //volver activar automatico
                activarIntervaloAutoguardado();
            }
        });
    }

    return true;
}

async function limpiarEquipoTemporalExcelWeb() {
    listarFicha(OPCION_FORMULARIO_LIMPIAR);
}

async function ValidarCambiosSinGuardar(tipoAutoguardado) {
    //Solo guardar cuando este activa la opción "editar envio" y el usuario es agente
    if (OPCION_GLOBAL_EDITAR && FLAG_AUTOGUARDADO_HABILITADO) {

        //pausar automatico
        detenerIntervaloAutoguardado();

        var listaItem = generarModeloFicha();

        var modeloWeb = {
            Ftenvcodi: getObjetoFiltro().codigoEnvio,
            Ftevercodi: getObjetoFiltro().codigoVersion,
            Fteeqcodi: getObjetoFiltro().fteeqcodi,
            FteeqcodisLimpiar: getObjetoFiltro().fteeqcodisLimpiar,
            HayPendiente1erAutoguardado: NUM_AUTOGUARDADO == 0,
            ListaTreeData: listaItem,
            TipoAutoguardado: tipoAutoguardado,
            MensajeAutoguardado: ULTIMO_MENSAJE_AUTOGUARDADO,
            ListaRevision: listaDataRevisionFT
        };

        //siempre hay autoguardado así no hay abierto un formulario
        return $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + "ValidarCambiosSinGuardar",
            contentType: "application/json",
            data: JSON.stringify({
                modelWeb: modeloWeb,
            }),
            beforeSend: function () {
                //mostrarExito("Enviando Información ..");
            },
            success: function (evt) {
                EXISTE_CAMBIO_SIN_GUARDAR = evt.Resultado == "1";

                //volver activar automatico
                activarIntervaloAutoguardado();
            },
            error: function (err) {
            }
        });
    }
}

function activarIntervaloAutoguardado() {
    detenerIntervaloAutoguardado();

    var numMIn = parseInt($('#hdIMinutosAutoguardado').val());

    FLAG_PAUSAR_AUTOGUARDADO = false;
    var idIntervalo = setInterval(function () {
        guardarFormularioCargaDatos(ACCION_GUARDADO_SISTEMA);
    }, numMIn * 60 * 1000);

    LISTA_ID_INTERVALO_AUTOGUARDADO.push(idIntervalo);

    //console.log("ID: " + idIntervalo);
}

function detenerIntervaloAutoguardado() {
    FLAG_PAUSAR_AUTOGUARDADO = true;
    for (var i = 0; i < LISTA_ID_INTERVALO_AUTOGUARDADO.length; i++) {
        clearInterval(LISTA_ID_INTERVALO_AUTOGUARDADO[i]);

    }
}

function mostrarHistorialAutoguardados() {

    var cadena = '';
    cadena += `
            <div style='clear:both; height:5px'></div>
                <table id='tablaHistAutoguardados' border='1' class='pretty tabla-adicional' cellspacing='0'>
                    <thead>
                        <tr>                                        
                            <th>Versión</th>
                            <th>Usuario</th>
                            <th>Fecha de Autoguardado</th>
                            <th>Operación</th>
                            <th>Tipo</th>
                            <th>Descripción</th>
                        </tr>
                    </thead>
                    <tbody>
                `;
    for (var i = 0; i < LISTA_AUTOGUARDADO.length; i++) {
        var reg = LISTA_AUTOGUARDADO[i];

        cadena += `   <tr> 
                                        
                        <td>${reg.NumeroVersion}</td>
                        <td>${reg.Fteverusucreacion}</td>
                        <td>${reg.FteverfeccreacionDesc}</td>
                        <td>${reg.FteveroperacionDesc}</td>
                        <td>${reg.RealizadoPor}</td>
                        <td>${reg.Fteverdescripcion}</td>
                    </tr>
                    `;
    }
    cadena += `     </tbody>
                            </table>
                        </div>
                `;

    abrirPopup("historialAutoguardados");

    $('#idHistoryAutoguardados').html(cadena);

    setTimeout(function () {
        $('#tablaHistAutoguardados').dataTable({
            "sDom": 'ft',
            "ordering": false,
            "iDisplayLength": -1,
            "scrollY": 300,
        });
    }, 150);

}

//manejo de cookies
function guardarCookie(nombre, valor) {
    var diasAExpirar = 2; //maximo 2 dias

    const d = new Date();
    d.setTime(d.getTime() + (diasAExpirar * 24 * 60 * 60 * 1000));
    let expires = "expires=" + d.toUTCString();
    document.cookie = nombre + "=" + valor + ";" + expires + ";path=/";
}

function obtenerCookie(nombre) {
    let nombreBuscado = nombre + "=";
    let ca = document.cookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(nombreBuscado) == 0) {
            return c.substring(nombreBuscado.length, c.length);
        }
    }
    return "";
}

function eliminarCookie(nombre) {
    document.cookie = nombre + "=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Archivos adjuntos de observaciones
////////////////////////////////////////////////////////////////////////////////////////////////////////////

function cargarHtmlArchivoXObs(nColumna, fteeqcodi, ftitcodi, accion) {

    var prefijo = "_sec_doc_" + ftitcodi;
    var htmlSec = "<div id=div_" + prefijo + ">";
    htmlSec += generarHtmlTablaDocumentoXObs(nColumna, prefijo, fteeqcodi, ftitcodi, accion);
    htmlSec += "</div>";

    $("#html_archivos_x_obs").html(htmlSec);

    //plugin archivo
    pUploadArchivoXObs(nColumna, prefijo, fteeqcodi, ftitcodi, accion);

}

function generarHtmlTablaDocumentoXObs(nColumna, idPrefijo, fteeqcodi, ftitcodi, accion) {

    var listaArchivo = [];
    //Obtengo listado de archivo segun columna y fila
    var elemento = listaDataRevisionFT.find(x => x.Fteeqcodi === fteeqcodi && x.Ftitcodi === ftitcodi);
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
                        <spam style="float: left;"><b>Nota:</b><i> Ingrese solo comentarios al formulario, las imagenes y otros archivos deben ser adjuntados</i></spam>
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

    html += generarTablaListaBodyXObs(listaArchivoRevTemp, accion, true, nColumna, fteeqcodi, ftitcodi);

    html += `
                        </div>

                    </td>
                </tr>
            </table>
                <div style='clear:both; height:10px;width:100px;'></div>
    `;

    return html;
}

function generarTablaListaBodyXObs(listaArchivo, accion, enPopup, nColumna, fteeqcodi, ftitcodi) {

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
                <table border="0" class="pretty " cellspacing="0" style="width:auto" id="">
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

                            <td onclick="verArchivoXObs(${nColumna}, ${fteeqcodi}, ${ftitcodi},${i}, ${enPopup});" title='Visualizar archivo - ${nomb}' style="cursor: pointer;width:30px;${styleFondo}">
                                	&#128065;
                            </td>
                            <td onclick="descargarArchivoXObs(${nColumna}, ${fteeqcodi}, ${ftitcodi},${i}, ${enPopup});" title='Descargar archivo - ${nomb}' style="cursor: pointer;width:30px;${styleFondo}">
                                <img width="15" height="15" src="../../Content/Images/btn-download.png" />
                            </td>
                            <td style="text-align:left;${styleFondo}" >
                                ${textoEnPopup}
                            </td>
        `;
        if (accion == CELDA_REV_EDITAR && enPopup) {
            html += `     
                            <td onclick="eliminarRowXObs(${nColumna}, ${fteeqcodi}, ${ftitcodi},${i})" title='Eliminar archivo ${nomb}' style="width:30px;cursor:pointer;${styleFondo}">
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

//Vista previa
function verArchivoXObs(nColumna, fteeqcodi, ftitcodi, pos, enPopup) {

    var regArchivo;
    var tipoArchivo = getTipoArchivoXColumna(nColumna);
    var listaArchivo = [];

    if (enPopup) {
        //busco en la lista temporal
        regArchivo = listaArchivoRevTemp[pos];
    } else {
        //Obtengo listado de archivo guardado segun columna y fila 
        var elemento = listaDataRevisionFT.find(x => x.Fteeqcodi === fteeqcodi && x.Ftitcodi === ftitcodi);
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

        var metodoVistaPrevia = 'VistaPreviaArchivoEnvio';

        if (esVistaPrevia) {
            $.ajax({
                type: 'POST',
                url: controlador + metodoVistaPrevia,
                data: {
                    idEnvio: getObjetoFiltro().codigoEnvio,
                    idVersion: getObjetoFiltro().codigoVersion,
                    idElemento: ftitcodi,
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

function pUploadArchivoXObs(nColumna, prefijo, fteeqcodi, ftitcodi, accion) {
    var uploaderP23 = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFile' + prefijo,
        container: document.getElementById('container'),
        chunks_size: '50mb',
        multipart_params: {
            idEnvio: getObjetoFiltro().codigoEnvio,
            idVersion: getObjetoFiltro().codigoVersion,
            idElemento: ftitcodi,
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
                $('#progreso' + prefijo).html("Archivo enviado.").css('textAlign', 'left');
                setTimeout(function () {
                    $('#progreso' + prefijo).html("");
                }, 2500);
            },
            FileUploaded: function (up, file, result) {
                agregarRowXObs(nColumna, prefijo, fteeqcodi, ftitcodi, JSON.parse(result.response).nuevonombre, file.name, accion);
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

function agregarRowXObs(nColumna, prefijo, fteeqcodi, ftitcodi, nuevoNombre, nombreArchivo, accion) {

    var elemento = listaDataRevisionFT.find(x => x.Fteeqcodi === fteeqcodi && x.Ftitcodi === ftitcodi);

    if (elemento != null) {

        listaArchivoRevTemp.push({ EsNuevo: true, Ftearcnombrefisico: nuevoNombre, Ftearcnombreoriginal: nombreArchivo });

        $("#listaArchivos" + prefijo).html(generarTablaListaBodyXObs(listaArchivoRevTemp, accion, true, nColumna, fteeqcodi, ftitcodi));
    }

}

function descargarArchivoXObs(nColumna, fteeqcodi, ftitcodi, pos, enPopup) {
    var regArchivo;
    var tipoArchivo = getTipoArchivoXColumna(nColumna);
    var listaArchivo = [];

    if (enPopup) {
        //busco en la lista temporal
        regArchivo = listaArchivoRevTemp[pos];
    } else {
        //Obtengo listado de archivo guardado segun columna y fila 
        var elemento = listaDataRevisionFT.find(x => x.Fteeqcodi === fteeqcodi && x.Ftitcodi === ftitcodi);
        switch (nColumna) {

            case 1: listaArchivo = elemento.ListaArchivosObsCoes; break;
            case 2: listaArchivo = elemento.ListaArchivosRptaAgente; break;
            case 3: listaArchivo = elemento.ListaArchivosRptaCoes; break;
        }

        regArchivo = listaArchivo[pos];
    }

    if (regArchivo != null) {
        window.location = controlador + 'DescargarArchivoEnvio?fileName=' + regArchivo.Ftearcnombrefisico + "&fileNameOriginal=" + regArchivo.Ftearcnombreoriginal
            + '&idElemento=' + ftitcodi + '&tipoArchivo=' + tipoArchivo + '&idEnvio=' + getObjetoFiltro().codigoEnvio + '&idVersion=' + getObjetoFiltro().codigoVersion;
    }
}

function eliminarRowXObs(nColumna, fteeqcodi, ftitcodi, pos) {

    listaArchivoRevTemp.splice(pos, 1);

    var prefijo = "_sec_doc_" + ftitcodi;

    $("#listaArchivos" + prefijo).html(generarTablaListaBodyXObs(listaArchivoRevTemp, CELDA_REV_EDITAR, true, nColumna, fteeqcodi, ftitcodi));
}

function getTipoArchivoXColumna(nColumna) {
    if (nColumna == 1) return TIPO_ARCHIVO_REVISION_OBSCOES;
    if (nColumna == 2) return TIPO_ARCHIVO_REVISION_RPTAAGENTE;
    if (nColumna == 3) return TIPO_ARCHIVO_REVISION_RPTACOES;

    return "NA";
}


//Observaciones html 
function mostrarPopupObservacion(nColumna, fteeqcodi, ftitcodi, accion, objMsg) {

    popupFormularioObservacion(nColumna, fteeqcodi, ftitcodi, accion, objMsg);
}

function popupFormularioObservacion(nColumna, fteeqcodi, ftitcodi, accion, objMsg) {
    _limpiarBarraMensaje_('mensajePopupCelda');

    tinymce.remove();
    $('#btnGuardarObsHtml').unbind();
    $("#htmlArchivos").unbind();
    $("#htmlArchivos").html('');
    $("#idFormularioObservacion").html('');

    var objMsg = JSON.parse(decodeURIComponent(objMsg));
    var msgHtml = "";

    if (accion == CELDA_REV_EDITAR) {
        const registro = listaDataRevisionFT.find(x => x.Ftitcodi === ftitcodi && x.NumcolumnaEditada === nColumna && x.Fteeqcodi === fteeqcodi);
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
                <td colspan="3" style="padding-top: 2px; text-align: center;">
                    <input type="button" id="btnGuardarObsHtml" value="Guardar" />                    
                </td>
            </tr>
        `;
    }

    $('#idFormularioObservacion').html(htmlDiv);

    $('#btnGuardarObsHtml').click(function () {
        _guardarObsHtml(nColumna, fteeqcodi, ftitcodi);
    });

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
        cargarHtmlArchivoXObs(nColumna, fteeqcodi, ftitcodi, accion);
        //cargarHtmlArchivoXObs('F3', objItemObs.Obs, posTab, fila, col, objTab.EsEditableObs);

    }, 50);

}

function _guardarObsHtml(nColumna, fteeqcodi, ftitcodi) {
    _limpiarBarraMensaje_('mensajePopupCelda');

    var htmlObs = $("#contenido_html_obs").val();

    var msg = validarMensajeCelda(htmlObs);

    if (msg == "") {
        //reemplazo en el array
        const registro = listaDataRevisionFT.find(x => x.Ftitcodi === ftitcodi && x.NumcolumnaEditada === nColumna && x.Fteeqcodi === fteeqcodi);
        if (registro != null) {
            var pos = listaDataRevisionFT.findIndex(function (x) { return x.Ftitcodi === ftitcodi && x.NumcolumnaEditada === nColumna && x.Fteeqcodi === fteeqcodi; });
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

        $("#datoCelda_" + nColumna + "_" + ftitcodi).html(htmlEditado)

        //Actualiza el listado de archivos
        $("#data_Archivos_" + nColumna + "_" + ftitcodi).html(generarTablaListaBodyXObs(listaArchivo, CELDA_REV_EDITAR, false, nColumna, fteeqcodi, ftitcodi));


        //cierro popup
        $('#popupFormularioObservacion').bPopup().close();
    } else {
        _mostrarMensaje_('mensajePopupCelda', "alert", msg);
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

////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Errores
////////////////////////////////////////////////////////////////////////////////////////////////////////////
function mostrarDetalleErrores() {

    $("#idTerrores").html('');

    var htmlTerrores = dibujarTablaError(LISTA_ERRORES);
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
                        <th style='width: 50px;'>Código <br>de equipo</th>
                        <th style='width: 200px;'>Ubicación</th>
                        <th style='width: 150px;'>Equipo</th>
                        <th style='width: 75px;'>Celda</th>
                        <th style='width: 200px;'>Tipo Error</th>
                    </tr>
                </thead>
                <tbody>
    `;

    for (var i = 0; i < listaError.length; i++) {
        var item = listaError[i];
        var n = i + 1;

        var s1 = item.CodigoElemento > 0 ? item.CodigoElemento : "";
        var s2 = item.CodigoElemento > 0 ? item.Ubicacion : "";
        var s3 = item.CodigoElemento > 0 ? item.NombreEquipo : "";

        cadena += `
                    <tr>
                        <td style='width:  10px; white-space: inherit;'>${n}</td>
                        <td style='width:  50px; white-space: inherit;'>${s1}</td>
                        <td style='width: 200px; white-space: inherit;'>${s2}</td>
                        <td style='width: 150px; white-space: inherit;'>${s3}</td>
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
            "scrollY": 330,
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });
    }, 50);
}

function dibujarTablaEnvios() {

    var cadena = `
    <div style='clear:both; height:5px'></div>
        <table id='tablalenvio' border='1' class='pretty tabla-adicional' cellspacing='0'>
            <thead>
                <tr>
                    <th># Versión</th>
                    <th>Fecha Hora</th>
                    <th>Usuario</th>
                </tr>
            </thead>
            <tbody>
    `;

    var numElementosVersiones = LISTA_VERSIONES.length;
    var ant = 1;
    for (var key = 0; key < numElementosVersiones; key++) {

        var verCodiAnterior = 0;
        if (numElementosVersiones == 1) {

        } else {
            if (key < (numElementosVersiones - 1)) {
                verCodiAnterior = LISTA_VERSIONES[ant].Ftevercodi;
                ant++;
            }

        }

        cadena += `
            <tr onclick='mostrarEnvioExcelWeb(${LISTA_VERSIONES[key].Ftevercodi},${verCodiAnterior}, ${LISTA_VERSIONES[key].NumeroVersion});' style='cursor:pointer'>
                <td>${LISTA_VERSIONES[key].NumeroVersion}</td>
                <td>${LISTA_VERSIONES[key].FteverfeccreacionDesc} </td>
                <td>${LISTA_VERSIONES[key].Fteverusucreacion}</td>
            </tr>
        `;


    }

    cadena += "</tbody></table>";

    return cadena;
}

function mostrarEnvioExcelWeb(versionAMostrar, versionAnterior, numVersion) {
    $('#enviosanteriores').bPopup().close();

    _limpiarBarraMensaje_("mensajeEnviosAnteriores");
    $("#hfNumVersionSeleccionadaPopup").val(numVersion);
    $("#hfVersionSeleccionadaPopup").val(versionAMostrar);
    $("#hfVersionAnteriorPopup").val(versionAnterior);

    //actualizar titulo pantalla
    $("#version_desc").html(" - Versión " + numVersion);
    $("#hfIdVersion").val(versionAMostrar); //versión global

    OPCION_GLOBAL_EDITAR = false;

    //listar equipos de la versión
    listarEquipoEnvioConexIntegModif();
}

//Expandir - Restaurar
function expandirRestaurar() {
    if ($('#hfExpandirContraer').val() == "E") {
        expandirExcel();
        //calculateSize2(1);
        expandirExcelImagen();

        //parte izquierda
        $("#detalle_ficha_tecnica").css("width", ($(window).width() - 50) + "px");
        $("#detalle_ficha_tecnica").css("height", ($(window).height() - 180) + "px");

        //parte derecha
        $(".campo_cab_add_celdas.celdaREV").css("width", 120 + "px");
        $(".campo_cab_add_celdas.celdaREVEstado").css("width", 55 + "px");
    }
    else {
        restaurarExcel();
        //calculateSize2(2);
        restaurarExcelImagen();

        //parte izquierda
        $("#detalle_ficha_tecnica").css("width", anchoPortal + "px");
        $("#detalle_ficha_tecnica").css("height", HEIGHT_FORMULARIO + "px");

        //parte derecha
        $(".campo_cab_add_celdas.celdaREV").css("width", 200 + "px");
        $(".campo_cab_add_celdas.celdaREVEstado").css("width", 55 + "px");

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