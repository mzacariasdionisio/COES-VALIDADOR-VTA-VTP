var controlador = siteRoot + 'Equipamiento/FTAdministrador/';

$(function () {
    FECHA_SISTEMA = $("#hdFechaSistema").val();
    if ($("#hfIdEnvio").val() == 2)
    {
        FECHA_SISTEMA = "19/06/2025";
    }

    //obtener el alto disponible para el formulario (alto total - header, filtros, footer)
    HEIGHT_FORMULARIO = $(window).height() - 300;

    //ocultar barra lateral izquierda
    $("#btnOcultarMenu").click();

    anchoPortal = $(window).width() - 85;

    $('#btnInicio').click(function () {
        regresarPrincipal();
    });

    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').easytabs('select', '#tabLista');

    $('#btnDescargar').click(function () {
        exportarFormatoConexIntegModif();
    });

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

    } else {
        limpiarBarraMensaje('mensajeFecMR')
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

    //mostrar opción
    var codigoEtapa = parseInt($("#hfFtetcodi").val()) || 0;
    if (ETAPA_MODIFICACION != codigoEtapa)
        $(".filtro_proyecto").css("display", "table-cell");
    $("#div_herr_filtro").css("display", "table-cell");
    $("#btnExpandirRestaurar").parent().css("display", "table-cell");
    $('#btnExpandirRestaurar').click(function () {
        expandirRestaurar();
    });

    $('#btnDescargarRevisionFichas').click(function () {
        exportarFormatoConexIntegModif();
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
    $('#desaprob_fecVigencia').Zebra_DatePicker({
        format: "d/m/Y",
        direction: [FECHA_SISTEMA, "31/12/2070"],
    });
    $('#btnPopupDenegar').click(function () {
        validarYMostrarPopupDenegar(this);
    });
    $('#btnEnviarDenegacion').click(function () {
        realizarDenegacion();
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
        limpiarBarraMensaje("mensajeEvento");
        limpiarBarraMensaje("mensaje_popupAprobar");

        var filtro = datosFiltroAprobacion();
        var msg = validarDatosFiltroAprobacion(filtro);

        if (msg == "") {
            verificarParametrosVacios();
        } else {
            mostrarMensaje('mensaje_popupAprobar', 'error', msg);
        }

    });
    $('#btnAceptarRellenoP').click(function () {
        realizarAprobacionConRelleno();

    });
    $(".cierreRelleno").click(function () {

        if (confirm('¿Desea cancelar la aprobación del envío?')) {
            cerrarPopup('popupAprobar');
        }
    });

    /** APROBAR PARCIALMENTE ENVIO **/
    $('#aprobP_fecVigencia').Zebra_DatePicker({
        format: "d/m/Y",
        direction: [FECHA_SISTEMA, "31/12/2070"],
    });
    $('#btnPopupAprobarParcialmente').click(function () {
        validarYMostrarPopupAprobarP(this);
    });
    $('#btnEnviarAprobacionP').click(function () {
        limpiarBarraMensaje("mensajeEvento");
        limpiarBarraMensaje("mensaje_popupAprobarParcial");

        var filtro = datosFiltroAprobacionP();
        var msg = validarDatosFiltroAprobacionP(filtro);

        if (msg == "") {
            verificarParametrosVaciosP();
        } else {
            mostrarMensaje('mensaje_popupAprobarParcial', 'error', msg);
        }

    });

    $('#btnAceptarRellenoAP').click(function () {
        realizarAprobacionPConRelleno();

    });
    $(".cierreRellenoAP").click(function () {

        if (confirm('¿Desea cancelar la aprobación parcial del envío?')) {
            cerrarPopup('popupAprobarParcial');
        }
    });

    /** DERIVAR ENVIO **/
    $('#deriv_fecmaxrpta').Zebra_DatePicker({
        format: "d/m/Y",
        direction: [FECHA_SISTEMA, "31/12/2070"],
    });
    $('#btnPopupDerivar').click(function () {
        validarYMostrarPopupDerivar(this);
    });
    $('#btnEnviarDerivacion').unbind();
    $('#btnEnviarDerivacion').click(function () {
        realizarDerivacion();
    });

    //guardar manual
    $('#btnAutoguardar').click(function () {
        guardarEquipoTemporalExcelWeb();
    });

    //tabla web
    OPCION_GLOBAL_EDITAR = $("#hfTipoOpcion").val() == "E";
    listarEquipoEnvioConexIntegModif();
    visibilidadBotones();

    crearPuploadFormato3();
});


function regresarPrincipal(estado) {
    var estadoEnvio = parseInt($("#hfIdEstado").val()) || 0;

    if (estado != null) {
        estadoEnvio = estado;
    }

    document.location.href = controlador + "Index?carpeta=" + estadoEnvio;
}

/**
 * Listado de equipos y modos de operación del envio
 * */
function listarEquipoEnvioConexIntegModif() {

    limpiarBarraMensaje('mensajeEvento');

    var filtro = getObjetoFiltro();

    $.ajax({
        type: 'POST',
        url: controlador + "ListarEqConexIntegModifXEnvio",
        data: {
            codigoEnvio: filtro.codigoEnvio,
            versionEnvio: filtro.codigoVersion,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                OBJ_VALIDACION_ENVIO = evt.ValidacionEnvio;

                $("#hfFteeqcodis").val(evt.LstFteeqcodis);
                $("#hfFteeqcodinomb").val(evt.LstEnviosEqNombres);

                var htmlTabla = dibujarTablaEquipoEnvioConexIntegModif(evt.ListaEnvioEq, filtro.codigoEnvio);
                $("#listadoDetalleEquipo").html(htmlTabla);

                $('#tablaDetalleEquipo').dataTable({
                    "scrollY": 350,
                    "scrollX": false,
                    "sDom": 't',
                    "ordering": false,
                    "iDisplayLength": -1
                });

                setTimeout(function () {
                    $("#chkSeleccionarTodo").on("click", function () {
                        var check = $('#chkSeleccionarTodo').is(":checked");
                        $("input[name=chkSeleccion]").prop("checked", check);
                    });
                }, 50);

                //Verifico si el check cabecera debe pintarse o no al editar cualquier check hijo
                $('input[type=checkbox][name="chkSeleccion"]').change(function () {
                    verificarCheckGrupal();
                });

                var ftEditada = $("#hdFTEditadaUsuario").val();
                if (ftEditada == "S") {
                    mostrarMensaje('mensajeEvento', 'alert', "La Ficha Técnica de un equipo fue modificado (agregó, eliminó o desactivó items) por lo que el envío no puede seguir con el flujo habitual.");
                }

            } else {

                mostrarMensaje('mensajeEvento', "alert", evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensajeEvento', 'error', 'Ha ocurrido un error.');
        }
    });
}

function verificarCheckGrupal() {
    //Empresas Interrupcion Suministro con check
    var val1 = 0;
    $("input[type=checkbox][id^=chkSeleccion_]").each(function () {
        var IsCheckedIS = $("#" + this.id)[0].checked;
        if (IsCheckedIS) {

        } else {
            val1 = val1 + 1;
        }
    });

    var v = true;
    if (val1 > 0)
        v = false;

    $("#chkSeleccionarTodo").prop("checked", v);
}

function getObjetoFiltro() {
    var filtro = {};

    filtro.fteeqcodi = parseInt($("#hfIdEquipoEnvio").val()) || 0;

    filtro.codigoEnvio = parseInt($("#hfIdEnvio").val()) || 0;
    filtro.codigoEnvioTemporal = parseInt($("#hfIdEnvioTemporal").val());
    filtro.codigoVersion = parseInt($("#hfIdVersion").val());

    filtro.codigoEmpresa = parseInt($("#hfEmprcodi").val()) || 0;
    filtro.codigoEtapa = parseInt($("#hfFtetcodi").val()) || 0;
    filtro.codigoProyecto = parseInt($("#hfFtprycodi").val()) || 0;
    filtro.codigoEquipos = listarCodigoYTipoChecked();
    filtro.estado = parseInt($("#hfIdEstado").val()) || 0;

    return filtro;
}

function listarCodigoYTipoChecked() {
    //if (!OPCION_GLOBAL_EDITAR) return "-1";

    var selected = [];
    $('input[name=chkSeleccion]').each(function () {
        if ($(this).is(":checked")) {
            var codigoCheck = $(this).attr('id');
            const myArray = codigoCheck.split("_");
            let idEq = myArray[1];
            selected.push(idEq);
            //selected.push($(this).attr('id'));
        }
    });

    return selected.join(",");
}

function dibujarTablaEquipoEnvioConexIntegModif(lista, idEnvio) {
    var cadena = '';

    var thSelec = '';

    var miEstenvcodi = parseInt($("#hfIdEstado").val()) || 0;

    if (miEstenvcodi != ESTADO_OBSERVADO) thSelec = `<th style="width: 40px">Sel. Todos <br/> <input type="checkbox" id="chkSeleccionarTodo"> </th>`;
    //thSelec = `<th style="width: 40px">Sel. Todos <br/> <input type="checkbox" id="chkSeleccionarTodo"> </th>`;

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
            </tr>
        </thead>
        <tbody>
    `;

    var fila = 1;
    for (var key in lista) {
        var item = lista[key];

        var tdSelec = '';
        if (miEstenvcodi != ESTADO_OBSERVADO) {
            var checked = OPCION_IMPORTACION ? ' checked ' : '';
            tdSelec = `
                <td style="width: 40px">
                    <input type="checkbox" value="${item.Fteeqcodi}" name="chkSeleccion" id="chkSeleccion_${item.Fteeqcodi}" ${checked} />
                </td>
            `;
        }
        //var checked = OPCION_IMPORTACION ? ' checked ' : '';
        //tdSelec = `
        //        <td style="width: 40px">
        //            <input type="checkbox" value="${item.Fteeqcodi}" name="chkSeleccion" id="chkSeleccion_${item.Fteeqcodi}" ${checked} />
        //        </td>
        //    `;

        cadena += `
           <tr >
                ${tdSelec}
                <td style="width: 40px">
                    <a href="#"  onclick="redireccionarFormulario(${idEnvio},${item.Fteeqcodi});">
                       ${IMG_EDITAR_FICHA}
                    </a>
                </td>
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

function redireccionarFormulario(idEnvio, dEquipoEnvio) {
    $('#tab-container').easytabs('select', '#tabDetalle');

    $("#hfIdEquipoEnvio").val(dEquipoEnvio);

    //FormularioExcelWeb
    listarFicha();

}

//Descargar formato
function exportarFormatoConexIntegModif() {
    var filtro = getObjetoFiltro();
    //actualizar
    var idsAreasUsuario = $('#hdIdsAreaTotales').val();

    if (_esValidoFormularioExportarImportar()) {
        $.ajax({
            type: 'POST',
            url: controlador + "GenerarFormatoConexIntegModif",
            data: {
                codigoEnvio: filtro.codigoEnvio,
                versionEnvio: filtro.codigoVersion,
                estado: filtro.estado,
                areasUsuario: idsAreasUsuario,
                codigoEquipos: filtro.codigoEquipos
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
    } else {
        alert("No es posible descargar. Debe seleccionar uno o más equipos.");
    }
}

function _esValidoFormularioExportarImportar() {
    var esValidoForm = true;

    esValidoForm = listarCodigoYTipoChecked() != '';

    return esValidoForm;
}

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
    var filtro = getObjetoFiltro();
    OPCION_IMPORTACION = true;

    //cuando se importar no debe autoguardarse el formulario y este debe ocultarse
    $('#tab-container').easytabs('select', '#tabLista');
    $("#detalle_ficha_tecnica").html("");
    MODELO_FICHA = null;
    listaDataRevisionFT = [];
    listaDataRevisionArea = [];

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: controlador + 'LeerFileUpExcelFormatoConexIntegModif',
        data: {
            codigoEnvio: filtro.codigoEnvio,
            versionEnvio: filtro.codigoVersion,
            estado: filtro.estado
        },
        success: function (evt) {
            if (evt.Resultado == "1") {

                if (OPCION_IMPORTACION) {
                    mostrarMensaje('mensajeEvento', "exito", "Se ha cargado correctamente el archivo.");
                    //$("#mensajeEvento").show();
                    OBJ_VALIDACION_ENVIO = evt.ValidacionEnvio;
                }

                ULTIMO_MENSAJE_AUTOGUARDADO = '';
                //listarEquipoEnvioConexIntegModif();
            }
            else {
                if (evt.Resultado == "-1") {
                    mostrarMensaje('mensajeEvento', "error", evt.Mensaje);
                } else {
                    alert("Ha ocurrido un error en la importación del archivo excel.");
                }
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function visibilidadBotones() {

    var estadoEnvio = parseInt($("#hfIdEstado").val()) || 0;
    var etapaEnvio = parseInt($("#hfFtetcodi").val()) || 0;
    var tipoFormato = parseInt($("#hdIdEnvioTipoFormato").val()) || 0;

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
            if (estadoEnvio == ESTADO_SOLICITADO || estadoEnvio == ESTADO_SUBSANADO)
                $("#bq_importarFichas").css("display", "inline-table");
        }
    } else {
        if (etapaEnvio == ETAPA_OPERACION_COMERCIAL) {
            if (estadoEnvio == ESTADO_SOLICITADO || estadoEnvio == ESTADO_SUBSANADO || estadoEnvio == ESTADO_APROBADO || estadoEnvio == ESTADO_APROBADO_PARCIAL || estadoEnvio == ESTADO_DESAPROBADO || estadoEnvio == ESTADO_CANCELADO) {
                $("#bq_descargaContenido").css("display", "inline-table");
            }
        } else {
            if (etapaEnvio == ETAPA_MODIFICACION) {
                if (tipoFormato == TIPO_FORMATO_DARBAJA) //Dar Baja
                {
                    if (estadoEnvio == ESTADO_SOLICITADO || estadoEnvio == ESTADO_APROBADO || estadoEnvio == ESTADO_APROBADO_PARCIAL || estadoEnvio == ESTADO_DESAPROBADO || estadoEnvio == ESTADO_CANCELADO) {
                        $("#bq_descargaContenido").css("display", "inline-table");
                    }
                } else {
                    if (estadoEnvio == ESTADO_SOLICITADO || estadoEnvio == ESTADO_SUBSANADO || estadoEnvio == ESTADO_APROBADO || estadoEnvio == ESTADO_APROBADO_PARCIAL || estadoEnvio == ESTADO_DESAPROBADO || estadoEnvio == ESTADO_CANCELADO) {
                        $("#bq_descargaFichas").css("display", "inline-table");
                        if (estadoEnvio == ESTADO_SOLICITADO || estadoEnvio == ESTADO_SUBSANADO)
                            $("#bq_importarFichas").css("display", "inline-table");
                    }
                }
            }
        }
    }
}

function validarPermisoAccionBotones(evt) {
    var estadoEnvio = parseInt($("#hfIdEstado").val()) || 0;
    var etapaEnvio = parseInt($("#hfFtetcodi").val()) || 0;
    var tipoFormato = parseInt($("#hdIdEnvioTipoFormato").val()) || 0;

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
                if (tipoFormato == TIPO_FORMATO_DARBAJA) //Dar Baja
                {
                    if (estadoEnvio == ESTADO_SOLICITADO || estadoEnvio == ESTADO_APROBADO || estadoEnvio == ESTADO_APROBADO_PARCIAL || estadoEnvio == ESTADO_DESAPROBADO || estadoEnvio == ESTADO_CANCELADO) {
                        if (idBtn == "btnDescargarRevisionContenido") {
                            permiso = true;
                        }
                    }
                } else {
                    if (estadoEnvio == ESTADO_SOLICITADO || estadoEnvio == ESTADO_SUBSANADO || estadoEnvio == ESTADO_APROBADO || estadoEnvio == ESTADO_APROBADO_PARCIAL || estadoEnvio == ESTADO_DESAPROBADO || estadoEnvio == ESTADO_CANCELADO) {
                        if (idBtn == "btnDescargarRevisionFichas") {
                            permiso = true;
                        }
                    }
                }

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

//FORMULARIO
var MODELO_OBTENER_DATOS_FT = null;
function listarFicha() {
    var filtro = getObjetoFiltroFT();
    restaurarExcelImagen();
    $("#detalle_ficha_tecnica").hide();
    $("#detalle_ficha_tecnica").html("");

    var idsAreasUsuario = $('#hdIdsAreaTotales').val();

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerDatosFT",
        data: {
            fteeqcodi: filtro.fteeqcodi,
            areasUsuario: idsAreasUsuario,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                var lstConfiguraciones = evt.ReporteDatoXEq.ListaItemConfig
                _determinarColumnasAdicionales(lstConfiguraciones);

                listaDataRevisionFT = evt.ListaRevisionParametrosAFT;
                listaDataRevisionArea = evt.ListaRevisionAreasFT;

                MODELO_FICHA = evt.ReporteDatoXEq;

                CODIGO_ENVIO = filtro.codigoEnvio;
                //CODIGO_VERSION = filtro.codigoVersion;
                CODIGO_VERSION = parseInt($("#hfIdVersion").val()) || 0;

                //Inicializar vista previa
                $("#detalle_ficha_tecnica").show();
                $("#detalle_ficha_tecnica").css("width", anchoPortal + "px"); //max width para que sea el maximo de la pantalla del usuario
                $("#detalle_ficha_tecnica").css("max-width", anchoPortal + "px"); //max width para que sea el maximo de la pantalla del usuario
                $("#detalle_ficha_tecnica").css("height", HEIGHT_FORMULARIO + "px");
                $("#detalle_ficha_tecnica").html(_extranet_generarHtmlReporteDetalleFichaTecnica(MODELO_FICHA));

                //barra de herramientas
                if (OPCION_GLOBAL_EDITAR) {
                    var estadoEnvio = parseInt($("#hfIdEstado").val()) || 0;
                    var etapaEnvio = parseInt($("#hfFtetcodi").val()) || 0;

                    if (estadoEnvio == ESTADO_APROBADO && etapaEnvio == ETAPA_CONEXION) {
                        $("#btnAutoguardar").parent().css("display", "none");
                    } else {
                        $("#btnAutoguardar").parent().css("display", "table-cell");
                    }
                }

                agregarBloqueRevision(evt);

                //variable global para mostrar ventana de solo lectura de la revision de las areas
                MODELO_OBTENER_DATOS_FT = evt;
                agregarBloqueAreas(evt);
                //que la pantalla vaya al fondo para mostrar la mayor cantidad de elementos del formulario
                _irAFooterPantalla();

            } else {

                mostrarMensaje('mensajeEvento', "error", evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensajeEvento', 'error', 'Ha ocurrido un error.');
        }
    });
}

function agregarBloqueRevision(evt) {
    limpiarBarraMensaje('mensajeEvento');

    agregarColumnasRevision(evt.ListaRevisionParametrosAFT);
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
                var cadena1 = 'Administrador Ficha Técnica';
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
                    nuevaCol2.style.width = "120px";
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
                                        nuevaCol.classList.add("celdaREVEstado"); break;
                                }


                            } else { //Verifico por cada Columna                           

                                var regEnLista = listaDataRevisionFT.find(x => x.Fteeqcodi === registro.Fteeqcodi && x.Ftitcodi === registro.Ftitcodi);

                                if (i == 1) {

                                    if (regEnLista != null) {
                                        if (regEnLista.NumcolumnaEditada == i) {
                                            registro.ValObsCoes = regEnLista.ValObsCoes;
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
                                            registro.ValRptaAgente = regEnLista.ValRptaAgente;
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
                                            registro.ValRptaCoes = regEnLista.ValRptaCoes;
                                            registro.ListaArchivosRptaCoes = regEnLista.ListaArchivosRptaCoes;
                                        }
                                    }
                                    cadena = obtenerHtmlCeldaRevisionFilaNoBloqueada(i, miFtitcodi, nuevaCol, registro.CeldaRptaCoesEstaBloqueada, registro.Fteeqcodi, registro.ValRptaCoes, regEnLista.IdValorEstado, registro.ListaArchivosRptaCoes);
                                    nuevaCol.classList.add("celdaREV");
                                    nuevaCol.setAttribute("id", "celdaCont_" + i + "_" + miFtitcodi);
                                }
                                if (i == 4) {
                                    registro.ValEstado = ""; //por defecto
                                    if (regEnLista != null && regEnLista.IdValorEstado !== undefined) {
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
    var miEstenvcodi = parseInt($("#hfIdEstado").val()) || 0;
    if (esCeldaBloqueada) {
        columna.style.background = COLOR_BLOQUEADO; //coloreo Fondo
        if (valorCelda != null && valorCelda != "") {
            cadena = mostrarTextoHtml(indiceColumnaBloque, ftitcodi, CELDA_REV_VER, fteeqcodi, valorCelda, listaArchivos, false);
        } else {
            cadena = mostrarTextoHtml(indiceColumnaBloque, ftitcodi, CELDA_REV_VER, fteeqcodi, "", listaArchivos, false);
        }
    } else {//esta desbloqueada en la intranet
        if (esString(opcionCombo)) {
            if (opcionCombo.trim() === OpcionConforme && miEstenvcodi == ESTADO_SOLICITADO) {
                cadena = mostrarTextoHtml(indiceColumnaBloque, ftitcodi, CELDA_REV_EDITAR, fteeqcodi, "", listaArchivos, true);
                columna.style.background = COLOR_BLOQUEADO; //coloreo Fondo

            } else {
                cadena = mostrarTextoHtml(indiceColumnaBloque, ftitcodi, CELDA_REV_EDITAR, fteeqcodi, valorCelda, listaArchivos, false);
            }
        } else {
            cadena = mostrarTextoHtml(indiceColumnaBloque, ftitcodi, CELDA_REV_EDITAR, fteeqcodi, valorCelda, listaArchivos, false);
        }

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
                       <a title="Editar registro" href="#" style="float: right;" onClick="mostrarPopupObservacion(${indiceColumnaBloque}, ${fteeqcodi}, ${ftitcodi},${accion},'${msgCodificado}');">${IMG_EDITAR} </a>
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

    var estiloVisible = agregoCssParaOcultarCelda ? ` style="display: none;"` : '';
    htmlDiv += `
        <table>
            <tr id="bloqueTexto_${indiceColumnaBloque}_${ftitcodi}" ${estiloVisible}>
                <td style='border: 0;${styleFondo}'>
                   <div id="datoCelda_${indiceColumnaBloque}_${ftitcodi}" style=" word-break: break-all;">                     
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

    //Para solicitud en intranet, se debe mostrar vacio en Estado
    if ((valEstado == null || valEstado == "") && miEstenvcodi == ESTADO_SOLICITADO && interfaz == INTRANET) {
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
            <select ${htmlHabilitacion} name="" id="ckbEstado_${miFtitcodi}_${miPropcodi}_${miConcepcodi}" 
                        onchange="CambiarOpcionCombo('${miPropcodi}','${miConcepcodi}', '${miFtitcodi}','${fteeqcodi}');" 
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

                    //Limpio los archivos cargados
                    listaDataRevisionFT[pos].ListaArchivosObsCoes = [];
                    $("#data_Archivos_" + numCol + "_" + ftitcodi).html("");
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

                            <td onclick="verArchivoXObs(${nColumna}, ${fteeqcodi}, ${ftitcodi},${i}, ${enPopup});" title='Visualizar archivo - ${nomb}' style="cursor: pointer;width:30px;${styleFondo}">
                                	&#128065;
                            </td>
                            <td onclick="descargarArchivoXObs(${nColumna}, ${fteeqcodi}, ${ftitcodi},${i}, ${enPopup});" title='Descargar archivo - ${nomb}' style="cursor: pointer;width:30px;${styleFondo}">
                                <img width="15" height="15" src="../../Content/Images/btn-download.png" />
                            </td>
                            <td style="text-align:left;${styleFondo}">
                                ${textoEnPopup}
                            </td>
        `;
        if (accion == CELDA_REV_EDITAR && enPopup) {
            html += `     
                            <td onclick="eliminarRowXObs(${nColumna}, ${fteeqcodi}, ${ftitcodi},${i})" title='Eliminar archivo' style="width:30px;cursor:pointer;${styleFondo}">
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

//AUTOGUARDADO
async function guardarEquipoTemporalExcelWeb() {
    var filtro = getObjetoFiltro();

    var modeloWeb = {
        Ftenvcodi: filtro.codigoEnvio,
        Ftevercodi: filtro.codigoVersion,
        Fteeqcodi: filtro.fteeqcodi,
        MensajeAutoguardado: ULTIMO_MENSAJE_AUTOGUARDADO,
        ListaRevision: listaDataRevisionFT
    };

    if (modeloWeb.Fteeqcodi > 0) {
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
                        mostrarMensaje('mensajeEvento', 'exito', "Se guardó correctamente.");

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

function pUploadArchivoXObs(nColumna, prefijo, fteeqcodi, idElemento, accion) {
    var uploaderP23 = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFile' + prefijo,
        container: document.getElementById('container'),
        chunks_size: '50mb',
        multipart_params: {
            idEnvio: getObjetoFiltro().codigoEnvio,
            idVersion: getObjetoFiltro().codigoVersion,
            idElemento: idElemento,
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
                agregarRowXObs(nColumna, prefijo, fteeqcodi, idElemento, JSON.parse(result.response).nuevonombre, file.name, accion);
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

function descargarArchivoXObs(nColumna, fteeqcodi, idElemento, pos, enPopup) {
    var regArchivo;
    var tipoArchivo = getTipoArchivoXColumna(nColumna);
    var listaArchivo = [];

    if (enPopup) {
        //busco en la lista temporal
        regArchivo = listaArchivoRevTemp[pos];
    } else {
        //Obtengo listado de archivo guardado segun columna y fila 
        var elemento = listaDataRevisionFT.find(x => x.Fteeqcodi === fteeqcodi && x.Ftitcodi === idElemento);
        switch (nColumna) {

            case 1: listaArchivo = elemento.ListaArchivosObsCoes; break;
            case 2: listaArchivo = elemento.ListaArchivosRptaAgente; break;
            case 3: listaArchivo = elemento.ListaArchivosRptaCoes; break;
        }

        regArchivo = listaArchivo[pos];
    }

    if (regArchivo != null) {
        window.location = controlador + 'DescargarArchivoEnvio?fileName=' + regArchivo.Ftearcnombrefisico + "&fileNameOriginal=" + regArchivo.Ftearcnombreoriginal
            + '&idElemento=' + idElemento + '&tipoArchivo=' + tipoArchivo + '&idEnvio=' + getObjetoFiltro().codigoEnvio + '&idVersion=' + getObjetoFiltro().codigoVersion;
    }
}

//Observaciones html 
function mostrarPopupObservacion(nColumna, fteeqcodi, ftitcodi, accion, objMsg) {

    popupFormularioObservacion(nColumna, fteeqcodi, ftitcodi, accion, objMsg);
}

function popupFormularioObservacion(nColumna, fteeqcodi, ftitcodi, accion, objMsg) {
    limpiarBarraMensaje('mensajePopupCelda');

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
                <td colspan="3" style="padding-top: 2px; ">
                    <input type="button" id="btnGuardarObsHtml" value="Guardar" style ="margin: 0px 10px 0px 350px;"/>
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


    }, 50);

}

function _guardarObsHtml(nColumna, fteeqcodi, ftitcodi) {
    limpiarBarraMensaje('mensajePopupCelda');
    var listaArchivo = [];

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


////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//   BLOQUE AREAS
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function agregarBloqueAreas(evt) {
    //limpiarBarraMensaje('mensajeEvento');

    //mostrar bloque azul luego de que se haya derivado el envio
    if (evt.ListaRevisionAreasFT != null && evt.ListaRevisionAreasFT.length > 0)
        agregarColumnasAreasRevision(evt.ListaRevisionAreasFT);
}

function agregarColumnasAreasRevision(listaRevision) {
    var filasTabla = document.getElementById("reporte").rows;

    var miFtetcodi = parseInt($("#hfFtetcodi").val()) || 0;

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
                            nuevaCol2.classList.add("celdaREVEstado_areas");
                            nuevaCol2.style.width = "55px";
                            break;
                        case 4:
                            cadena2 = 'Revisión Subsanación';
                            nuevaCol2.classList.add("celdaREV_areas");
                            break;
                        case 5:
                            cadena2 = 'Estado Subsanación';
                            nuevaCol2.classList.add("celdaREVEstado_areas");
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
                            if (i == 1) {

                                cadena = registro.AreasNombAsignadas;
                                nuevaCol.classList.add("celdaREV_areas");
                                nuevaCol.style.background = COLOR_BLOQUEADO;
                                nuevaCol.setAttribute("id", "celdaContArea_" + i + "_" + miFtitcodi);
                            }
                            if (i == 2) {
                                cadena = obtenerHtmlAreasCeldaRevisionFilaBloqueada(i, miFtitcodi, nuevaCol, registro.Fteeqcodi, registro.LstFilaRevision);
                                nuevaCol.classList.add("celdaREV_areas");
                                nuevaCol.setAttribute("id", "celdaContArea_" + i + "_" + miFtitcodi);
                            }

                            if (i == 3) {
                                cadena = obtenerHtmlAreasCeldaRevisionFilaBloqueada(i, miFtitcodi, nuevaCol, registro.Fteeqcodi, registro.LstFilaRevision);
                                nuevaCol.classList.add("celdaREV_areas");
                                nuevaCol.setAttribute("id", "celdaContArea_" + i + "_" + miFtitcodi);

                            }
                            if (i == 4) {
                                cadena = obtenerHtmlAreasCeldaRevisionFilaBloqueada(i, miFtitcodi, nuevaCol, registro.Fteeqcodi, registro.LstFilaRevision);
                                nuevaCol.classList.add("celdaREV_areas");
                                nuevaCol.setAttribute("id", "celdaContArea_" + i + "_" + miFtitcodi);
                            }

                            if (i == 5) {
                                cadena = obtenerHtmlAreasCeldaRevisionFilaBloqueada(i, miFtitcodi, nuevaCol, registro.Fteeqcodi, registro.LstFilaRevision);
                                nuevaCol.classList.add("celdaREV_areas");
                                nuevaCol.setAttribute("id", "celdaContArea_" + i + "_" + miFtitcodi);
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

function obtenerHtmlAreasCeldaRevisionFilaBloqueada(indiceColumnaBloque, ftitcodi, columna, fteeqcodi, lstFilaRevision) {
    var cadena = "";

    columna.style.background = COLOR_BLOQUEADO; //coloreo Fondo
    if (indiceColumnaBloque != 3 && indiceColumnaBloque != 5) {
        cadena = mostrarTextoAreasHtml(indiceColumnaBloque, ftitcodi, fteeqcodi, lstFilaRevision);
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
        <table>
            <tr id="bloqueAreaTexto_${indiceColumnaBloque}_${ftitcodi}">
                <td style='border: 0;${styleFondo}'>
                   <div id="datoAreaCelda_${indiceColumnaBloque}_${ftitcodi}">
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
        <table>
            <tr id="bloqueAreaTexto_${indiceColumnaBloque}_${ftitcodi}">
                <td style='border: 0;${styleFondo}'>
                   <div id="datoAreaCelda_${indiceColumnaBloque}_${ftitcodi}">
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

function mostrarTextoAreasHtml(indiceColumnaBloque, ftitcodi, fteeqcodi, lstFilaRevision) {

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
                    htmlArchivos = generarTablaAreaListaBodyXObs(listaArchivos, false, indiceColumnaBloque, fteeqcodi, ftitcodi);
                    htmlArchivos2 = generarTablaAreaListaBodyXObs(listaArchivos, true, indiceColumnaBloque, fteeqcodi, ftitcodi);
                }
                var hayBtnVerMasLectura = (reg.ListaArchivosSolicitados != null ? reg.ListaArchivosSolicitados.length : 0) > 0 || textoSinEtiquetas.length > 20;
                var htmlVerMas = '';
                if (hayBtnVerMasLectura) {

                    var id_popup_area = `Bloque2_${Date.now()}_${ftitcodi}_${fteeqcodi}_${i}`;

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
        <table>
            <tr id="bloqueAreaTexto_${indiceColumnaBloque}_${ftitcodi}">
                <td style='border: 0;${styleFondo}'>
                   <div id="datoAreaCelda_${indiceColumnaBloque}_${ftitcodi}" style="word-break: break-all;">
                     ${htmlAreaResp}
                   </div>
                   <div>
                        
                   </div>
                </td>
            </tr>
        </table>
        `;

                htmlDiv += `
        <div id="data_AreaArchivos_${indiceColumnaBloque}_${ftitcodi}"> 
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
                    htmlArchivos = generarTablaAreaListaBodyXObs(listaArchivos, false, indiceColumnaBloque, fteeqcodi, ftitcodi);
                    htmlArchivos2 = generarTablaAreaListaBodyXObs(listaArchivos, true, indiceColumnaBloque, fteeqcodi, ftitcodi);
                }
                var hayBtnVerMasLectura = (listaArchivos != null ? listaArchivos.length : 0) > 0 || textoSinEtiquetas.length > 20;
                var htmlVerMas = '';
                if (hayBtnVerMasLectura) {

                    var id_popup_area = `Bloque4_${Date.now()}_${ftitcodi}_${fteeqcodi}_${i}`;

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
        <table>
            <tr id="bloqueAreaTexto_${indiceColumnaBloque}_${ftitcodi}">
                <td style='border: 0;${styleFondo}'>
                   <div id="datoAreaCelda_${indiceColumnaBloque}_${ftitcodi}" style="word-break: break-all;">
                     ${htmlAreaResp}
                   </div>
                </td>
            </tr>
        </table>
        `;

                htmlDiv += `
        <div id="data_AreaArchivos_${indiceColumnaBloque}_${ftitcodi}"> 
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

function generarTablaAreaListaBodyXObs(listaArchivo, enPopup, nColumna, fteeqcodi, ftitcodi) {

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

                            <td onclick="verAreaArchivoXObs(${nColumna}, ${fteeqcodi}, ${ftitcodi},${i}, ${enPopup});" title='Visualizar archivo - ${nomb}' style="cursor: pointer;width:30px;${styleFondo}">
                                	&#128065;
                            </td>
                            <td onclick="descargarArchivoAreaXObs(${nColumna}, ${fteeqcodi}, ${ftitcodi},${i}, ${enPopup});" title='Descargar archivo - ${nomb}' style="cursor: pointer;width:30px;${styleFondo}">
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

//Vista previa áreas
function verAreaArchivoXObs(nColumna, fteeqcodi, ftitcodi, pos, enPopup) {

    var regArchivo;
    var tipoArchivo = "REV_AREA";
    var listaArchivo = [];

    if (enPopup) {
        //busco en la lista temporal
        regArchivo = listaArchivoRevAreaTemp[pos];
    } else {
        //Obtengo listado de archivo guardado segun columna y fila 
        var elemento = listaDataRevisionArea.find(x => x.Ftitcodi === ftitcodi);
        switch (nColumna) {

            case COLUMNA_REV_SOLICITADO: listaArchivo = elemento.ListaArchivosSolicitados; break;
            case COLUMNA_REV_SUBSANADO: listaArchivo = elemento.ListaArchivosSubsanados; break;
            //case 3: listaArchivo = elemento.ListaArchivosRptaCoes; break;
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
                    idEnvio: CODIGO_ENVIO,
                    idVersion: CODIGO_VERSION,
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

function descargarArchivoAreaXObs(nColumna, fteeqcodi, ftitcodi, pos, enPopup) {
    var regArchivo;
    var tipoArchivo = "REV_AREA";
    var listaArchivo = [];


    //Obtengo listado de archivo guardado segun columna y fila 
    var elemento = listaDataRevisionArea.find(x => x.Ftitcodi === ftitcodi);
    switch (nColumna) {

        case COLUMNA_REV_SOLICITADO: listaArchivo = elemento.ListaArchivosSolicitados; break;
        case COLUMNA_REV_SUBSANADO: listaArchivo = elemento.ListaArchivosSubsanados; break;
        //case 3: listaArchivo = elemento.ListaArchivosRptaCoes; break;
    }

    regArchivo = listaArchivo[pos];

    if (regArchivo != null) {
        window.location = controlador + 'DescargarArchivoEnvio?fileName=' + regArchivo.Ftearcnombrefisico + "&fileNameOriginal=" + regArchivo.Ftearcnombreoriginal
            + '&idElemento=' + ftitcodi + '&tipoArchivo=' + tipoArchivo + '&idEnvio=' + getObjetoFiltro().codigoEnvio + '&idVersion=' + getObjetoFiltro().codigoVersion;
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
        mostrarMensaje('mensajeEvento', "alert", msg);

    }
}

async function validarDataAObservar() {
    //Si existen cambios sin guardar entonces realizar el autoguardado
    await guardarEquipoTemporalExcelWeb();

    var msj = "";

    var miEstenvcodi = $("#hfIdEstado").val();
    var ftEditada = $("#hdFTEditadaUsuario").val();
    if (ftEditada == "S") {
        msj += "<p>La Ficha Técnica de un equipo fue modificado (agregó, eliminó o desactivó items) por lo que el envío no puede seguir con el flujo habitual.</p>";
    }

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
    $("#desaprob_fecVigencia").val('');
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

    var ftEditada = $("#hdFTEditadaUsuario").val();
    if (ftEditada == "S") {
        msj += "<p>La Ficha Técnica de un equipo fue modificado (agregó o eliminó items) por lo que el envío no puede seguir con el flujo habitual.</p>";
    }

    var miEstenvcodi = $("#hfIdEstado").val();
    var miFteeqcodis = $("#hfFteeqcodis").val();
    var miFtetcodi = $("#hfFtetcodi").val();

    //Solo valida si es un envio subsanado y se presiona DENEGAR
    if (miEstenvcodi == ESTADO_SUBSANADO) {

        //Cuando en la columna Estado se haya seleccionado Observado y no se ingrese comentario
        if (OBJ_VALIDACION_ENVIO.LstSalidaDenegarEnvio != null && OBJ_VALIDACION_ENVIO.LstSalidaDenegarEnvio.length > 0) {
            mostrarDetalleErrores(OBJ_VALIDACION_ENVIO.LstSalidaDenegarEnvio);

            msj += "<p>Existen validaciones pendientes de levantar.</p>";
        }

    } else {
        if (miEstenvcodi == ESTADO_APROBADO && miFtetcodi == ETAPA_CONEXION) { //No debe validar, debe pasar

        } else {
            msj += "<p>Este envío no tiene permitido la acción de denegar.</p>";
        }
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
    var miEstenvcodi = $("#hfIdEstado").val();

    filtro.fecVigencia = "";
    filtro.Mensaje = $('#desaprob_mensajeCoes').val();
    filtro.CCAgentes = $("#desaprob_ccAgente").val();
    filtro.idEnvio = parseInt($('#hfIdEnvio').val()) || 0;

    if (miEstenvcodi == ESTADO_APROBADO) {
        filtro.fecVigencia = $('#desaprob_fecVigencia').val();

        //cambio los estados a Denegado
        for (key in listaDataRevisionFT) {
            var fila = listaDataRevisionFT[key];

            fila.IdValorEstado = OpcionDenegado;
        }

    }

    return filtro;
}

function validarDatosFiltroDenegacion(datos) {
    var msj = "";
    var miEstenvcodi = $("#hfIdEstado").val();
    var paraAgente = $("#desaprob_hfparaAgente").val();

    var strFechaHoy = diaActualSistema(); // en dd/mm/yyyy

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

    if (miEstenvcodi == ESTADO_APROBADO) {
        if (esFechaValida(datos.fecVigencia)) {
            if (convertirFecha(strFechaHoy) > convertirFecha(datos.fecVigencia)) {
                msj += "<p>Debe escoger una fecha de vigencia correcta, la fecha debe ser posterior o igual al día actual (" + strFechaHoy + ").</p>";
            }
        } else {
            msj += "<p>Debe ingresar fecha de vigencia correcta (en formato dd//mm/yyyy).</p>";
        }
    }

    return msj;
}


/////////////////////////////////////
///////         APROBAR       ///////
/////////////////////////////////////
async function validarYMostrarPopupAprobar(ev) {
    limpiarBarraMensaje('mensajeEvento');
    limpiarBarraMensaje('mensaje_popupAprobar');

    //Reseteo la fecha
    $("#aprob_fecVigencia").val('');
    $("#aprob_enlaceSistema").val($("#aprob_hfenlaceSistema").val());
    $("#aprob_enlaceCarta").val('');
    $("#aprob_enlaceOtro").val('');
    $("#aprob_ccAgente").val($("#aprob_hfccAgente").val());
    $('input[name=chkCvariable_A]').prop('checked', false);

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
    var miFteeqcodis = $("#hfFteeqcodis").val();
    var ftEditada = $("#hdFTEditadaUsuario").val();
    if (ftEditada == "S") {
        msj += "<p>La Ficha Técnica de un equipo fue modificado (agregó o eliminó items) por lo que el envío no puede seguir con el flujo habitual.</p>";
    }

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

function verificarParametrosVacios() {
    limpiarBarraMensaje("mensajeEvento");
    limpiarBarraMensaje("mensaje_popupAprobar");
    limpiarBarraMensaje("mensaje_Relleno");

    $('input[name=PV]').prop('checked', false);
    $('#hdHayPVacios').val(0);
    $('#hdOpcionRelleno').val('');

    if (OBJ_VALIDACION_ENVIO.TieneParametrosVacios) {
        abrirPopup("popupParamVacios");

    } else {
        realizarAprobacion();
    }
}

function realizarAprobacionConRelleno() {
    limpiarBarraMensaje("mensaje_Relleno");
    $('#hdHayPVacios').val(1);


    var filtro = datosFiltroRelleno();
    var msg = validarDatosFiltroRelleno(filtro);

    if (msg == "") {
        $('#hdOpcionRelleno').val(filtro.opcionRelleno);
        realizarAprobacion();
    } else {
        mostrarMensaje('mensaje_Relleno', 'error', msg);
    }

}


function datosFiltroRelleno() {
    var filtro = {};

    var inputR = document.querySelector('input[name="PV"]:checked');
    var valRelleno = (inputR != null && inputR != undefined) ? document.querySelector('input[name="PV"]:checked').value : "";

    filtro.opcionRelleno = valRelleno;

    return filtro;
}

function validarDatosFiltroRelleno(datos) {
    var msj = "";

    if (datos.opcionRelleno == "") {
        msj += "<p>Debe escoger una de las opciones para la carga a Base de Datos.</p>";
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
                idCV: filtro.codigoCV,
                ccAgentes: filtro.CCAgentes,
                hayParamVacios: filtro.conParamVacios,
                opcionReemplazo: filtro.opcionReemplazo
            }),
            cache: false,
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    alert('Se realizó la aprobación correctamente.');
                    regresarPrincipal(ESTADO_APROBADO);
                } else {
                    cerrarPopup('popupParamVacios');
                    mostrarMensaje('mensaje_popupAprobar', 'error', 'Ha ocurrido un error: ' + evt.Mensaje);

                }
            },
            error: function (err) {
                cerrarPopup('popupParamVacios');
                mostrarMensaje('mensaje_popupAprobar', 'error', 'Ha ocurrido un error.');
            }
        });
    } else {
        cerrarPopup('popupParamVacios');
        mostrarMensaje('mensaje_popupAprobar', 'error', msg);
    }
}


function datosFiltroAprobacion() {
    var filtro = {};

    var numElegidos = 0;
    $("input[type=checkbox][id^=chkbxCV_A_").each(function () {
        if ($(this).prop('checked') == true) {
            numElegidos++;
        }
    });

    var inputR = document.querySelector('input[name="chkCvariable_A"]:checked');
    var cvSel = (inputR != null && inputR != undefined) ? document.querySelector('input[name="chkCvariable_A"]:checked').value : "";

    filtro.fecVigencia = $('#aprob_fecVigencia').val();
    filtro.enlaceSI = $('#aprob_enlaceSistema').val();
    filtro.enlaceCarta = $('#aprob_enlaceCarta').val();
    filtro.enlaceOtro = $('#aprob_enlaceOtro').val();
    filtro.codigoCV = cvSel;
    filtro.CCAgentes = $("#aprob_ccAgente").val();
    filtro.idEnvio = parseInt($('#hfIdEnvio').val()) || 0;
    filtro.conParamVacios = parseInt($('#hdHayPVacios').val()) || 0;
    filtro.opcionReemplazo = $('#hdOpcionRelleno').val();
    filtro.NumElegidosCV = numElegidos;

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

    if (datos.NumElegidosCV >= 2) {
        msj += "<p>Solo se permite seleccionar un evento de Costo Variable.</p>";
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



/////////////////////////////////////////////////
///////         APROBAR  PARCIALMENTE     ///////
/////////////////////////////////////////////////
async function validarYMostrarPopupAprobarP(ev) {
    limpiarBarraMensaje('mensajeEvento');
    limpiarBarraMensaje('mensaje_popupAprobarParcial');


    //Reseteo la fecha
    $("#aprobP_fecVigencia").val('');
    $("#aprobP_enlaceSistema").val($("#aprobP_hfenlaceSistema").val());
    $("#aprobP_enlaceCarta").val('');
    $("#aprobP_enlaceOtro").val('');
    $("#aprobP_ccAgente").val($("#aprobP_hfccAgente").val());
    $('input[name=chkCvariable_AP]').prop('checked', false);
    $("hdEnvioParamAprobados").val("");
    $("hdEnvioParamDenegados").val("");

    //valido si tiene permiso para accion
    var msg = validarPermisoAccionBotones(ev);
    if (msg == "") {
        //valido la informacion proporcionada
        var msg2 = await validarDataAAprobarP();
        //var msg2 = "";
        if (msg2 == "") {
            ObtenerListadoParametrosModificados();

        } else {
            mostrarMensaje('mensajeEvento', "alert", msg2);
        }
    } else {
        mostrarMensaje('mensajeEvento', "alert", msg);
    }
}

async function validarDataAAprobarP() {
    //Si existen cambios sin guardar entonces realizar el autoguardado
    await guardarEquipoTemporalExcelWeb();

    var msj = "";

    var miFtetcodi = $("#hfFtetcodi").val();
    var miTipoFormato = $("#hdIdEnvioTipoFormato").val();
    var miEstenvcodi = $("#hfIdEstado").val();

    var ftEditada = $("#hdFTEditadaUsuario").val();
    if (ftEditada == "S") {
        msj += "<p>La Ficha Técnica de un equipo fue modificado (agregó o eliminó items) por lo que el envío no puede seguir con el flujo habitual.</p>";
    }

    //Solo valida si es un envio subsanado y se presiona APROBAR PARCIALMENTE (ademas solo permitido en modif sin dar de baja)
    if (miEstenvcodi == ESTADO_SUBSANADO && miFtetcodi == ETAPA_MODIFICACION && miTipoFormato == TIPO_FORMATO_CONEXINTMODIF) {

        $("#hdEnvioParamAprobados").val(OBJ_VALIDACION_ENVIO.EnvioParamApr);
        $("#hdEnvioParamDenegados").val(OBJ_VALIDACION_ENVIO.EnvioParamDng);

        //Cuando en la columna Estado se haya seleccionado Observado y no se ingrese comentario
        if (OBJ_VALIDACION_ENVIO.LstSalidaParcialAprobarEnvio != null && OBJ_VALIDACION_ENVIO.LstSalidaParcialAprobarEnvio.length > 0) {
            mostrarDetalleErrores(OBJ_VALIDACION_ENVIO.LstSalidaParcialAprobarEnvio);

            msj += "<p>Existen validaciones pendientes de levantar.</p>";
        }

    } else {
        msj += "<p>Este envío no tiene permitido la acción de aprobación parcial. Solo se permite aprobar parcialmente envios subsanados en etapa de Modificación de Ficha Técnica (sin da de baja M.O.)</p>";
    }

    return msj;
}

function verificarParametrosVaciosP() {
    limpiarBarraMensaje("mensajeEvento");
    limpiarBarraMensaje("mensaje_popupAprobarParcial");
    limpiarBarraMensaje("mensaje_RellenoAP");

    $('input[name=PV_AP]').prop('checked', false);
    $('#hdHayPVacios').val(0);
    $('#hdOpcionRelleno').val('');

    if (OBJ_VALIDACION_ENVIO.TieneParametrosVacios) {
        abrirPopup("popupParamVaciosP");

    } else {
        realizarAprobacionParcial();
    }
}

function realizarAprobacionPConRelleno() {
    limpiarBarraMensaje("mensaje_RellenoAP");
    $('#hdHayPVacios').val(1);//Seteo 


    var filtro = datosFiltroRellenoP();
    var msg = validarDatosFiltroRellenoP(filtro);

    if (msg == "") {
        $('#hdOpcionRelleno').val(filtro.opcionRelleno);
        realizarAprobacionParcial();
    } else {
        mostrarMensaje('mensaje_RellenoAP', 'error', msg);
    }

}


function datosFiltroRellenoP() {
    var filtro = {};

    var inputR = document.querySelector('input[name="PV_AP"]:checked');
    var valRelleno = (inputR != null && inputR != undefined) ? document.querySelector('input[name="PV"]:checked').value : "";

    filtro.opcionRelleno = valRelleno;

    return filtro;
}

function validarDatosFiltroRellenoP(datos) {
    var msj = "";

    if (datos.opcionRelleno == "") {
        msj += "<p>Debe escoger una de las opciones para la carga a Base de Datos.</p>";
    }

    return msj;
}


function realizarAprobacionParcial() {
    limpiarBarraMensaje("mensajeEvento");
    limpiarBarraMensaje("mensaje_popupAprobarParcial");

    var filtro = datosFiltroAprobacionP();
    var msg = validarDatosFiltroAprobacionP(filtro);

    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarAprobacionParcialFT',
            dataType: 'json',
            contentType: "application/json",
            data: JSON.stringify({
                ftenvcodi: filtro.idEnvio,
                data: listaDataRevisionFT,
                fecVigencia: filtro.fecVigencia,
                enlaceSI: filtro.enlaceSI,
                enlaceCarta: filtro.enlaceCarta,
                enlaceOtro: filtro.enlaceOtro,
                idCV: filtro.codigoCV,
                ccAgentes: filtro.CCAgentes,
                hayParamVacios: filtro.conParamVacios,
                opcionReemplazo: filtro.opcionReemplazo,
                fitcfgcodiAprobados: filtro.fitcfgcodiAprobados,
                fitcfgcodiDenegados: filtro.fitcfgcodiDenegados
            }),
            cache: false,
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    alert('Se realizó la aprobación parcial correctamente.');
                    regresarPrincipal(ESTADO_APROBADO_PARCIAL);
                } else {
                    cerrarPopup('popupParamVaciosP');
                    mostrarMensaje('mensaje_popupAprobarParcial', 'error', 'Ha ocurrido un error: ' + evt.Mensaje);

                }
            },
            error: function (err) {
                cerrarPopup('popupParamVaciosP');
                mostrarMensaje('mensaje_popupAprobarParcial', 'error', 'Ha ocurrido un error.');
            }
        });
    } else {
        cerrarPopup('popupParamVaciosP');
        mostrarMensaje('mensaje_popupAprobarParcial', 'error', msg);
    }
}


function datosFiltroAprobacionP() {
    var filtro = {};

    var numElegidos = 0;
    $("input[type=checkbox][id^=chkbxCV_AP_").each(function () {
        if ($(this).prop('checked') == true) {
            numElegidos++;
        }
    });

    var inputR = document.querySelector('input[name="chkCvariable_AP"]:checked');
    var cvSel = (inputR != null && inputR != undefined) ? document.querySelector('input[name="chkCvariable_AP"]:checked').value : "";

    filtro.fecVigencia = $('#aprobP_fecVigencia').val();
    filtro.enlaceSI = $('#aprobP_enlaceSistema').val();
    filtro.enlaceCarta = $('#aprobP_enlaceCarta').val();
    filtro.enlaceOtro = $('#aprobP_enlaceOtro').val();
    filtro.codigoCV = cvSel;
    filtro.CCAgentes = $("#aprobP_ccAgente").val();
    filtro.idEnvio = parseInt($('#hfIdEnvio').val()) || 0;
    filtro.conParamVacios = parseInt($('#hdHayPVacios').val()) || 0;
    filtro.opcionReemplazo = $('#hdOpcionRelleno').val();
    filtro.fitcfgcodiAprobados = $("#hdEnvioParamAprobados").val();
    filtro.fitcfgcodiDenegados = $("#hdEnvioParamDenegados").val();
    filtro.NumElegidosCV = numElegidos;

    return filtro;
}

function validarDatosFiltroAprobacionP(datos) {
    var msj = "";
    var paraAgente = $("#aprobP_hfparaAgente").val();

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

    if (datos.NumElegidosCV >= 2) {
        msj += "<p>Solo se permite seleccionar un evento de Costo Variable.</p>";
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

    if (datos.fteeqcodisAprobados == "") {
        msj += "<p>No existe equipos aprobados.</p>";
    }
    return msj;
}

function ObtenerListadoParametrosModificados() {
    var fitcfgcodiAprobados = $("#hdEnvioParamAprobados").val();
    var fitcfgcodiDenegados = $("#hdEnvioParamDenegados").val();
    var idEnvio = parseInt($('#hfIdEnvio').val()) || 0;

    $("#listaParamAprobados").html("");
    $("#listaParamDesaprobados").html("");

    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerListadoParametrosModificadosAP',
        dataType: 'json',
        contentType: "application/json",
        data: JSON.stringify({
            ftenvcodi: idEnvio,
            fitcfgcodiAprobados: fitcfgcodiAprobados,
            fitcfgcodiDenegados: fitcfgcodiDenegados
        }),
        cache: false,
        success: function (evt) {
            if (evt.Resultado != "-1") {
                var htmlAPParamModificadosAprobados = evt.HtmlParametrosModifAprobados;
                var htmlAPParamModificadosDenegados = evt.HtmlParametrosModifDenegados;

                $("#listaParamAprobados").html(htmlAPParamModificadosAprobados);
                $("#listaParamDesaprobados").html(htmlAPParamModificadosDenegados);

                abrirPopup("popupAprobarParcial");
            } else {
                mostrarMensaje('mensajeEvento', 'error', 'Ha ocurrido un error: ' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensajeEvento', 'error', 'Ha ocurrido un error.');
        }
    });
}


////////////////////////////////////////////////
// Util
////////////////////////////////////////////////

/**
 * obtiene todos los equipos que tengan ciertos estados, si si se requiere que todos tengan dicho estado o basta con 1
 */
function obtenerEquiposPorEstado(arrayFteeqcodisDataDistinct, estadoRequerido, requiereTodasFilas) {
    var arrayBuscado = [];
    for (key in arrayFteeqcodisDataDistinct) {
        var fteeqcodiX = arrayFteeqcodisDataDistinct[key];

        var arrayTotalXEquipo = listaDataRevisionFT.filter(x => x.Fteeqcodi === fteeqcodiX);
        var numTotalFilasXEquipo = arrayTotalXEquipo.length;

        var arrayBuscados = listaDataRevisionFT.filter(x => x.Fteeqcodi === fteeqcodiX && x.IdValorEstado === estadoRequerido);
        var numFilasBuscadoXEquipo = arrayBuscados.length;

        if (requiereTodasFilas == TODOS) {
            if (numTotalFilasXEquipo == numFilasBuscadoXEquipo) {
                arrayBuscado.push(fteeqcodiX);
            }
        } else {
            if (requiereTodasFilas == BASTA1) {
                if (numTotalFilasXEquipo > 0 && numFilasBuscadoXEquipo >= 1) {
                    arrayBuscado.push(fteeqcodiX);
                }
            }
        }

    }

    return arrayBuscado;
}

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

//Expandir - Restaurar
function expandirRestaurar() {
    if ($('#hfExpandirContraer').val() == "E") {
        expandirExcel();
        //calculateSize2(1);
        expandirExcelImagen();

        //parte izquierda
        $("#detalle_ficha_tecnica").css("width", ($(window).width() - 20) + "px");
        $("#detalle_ficha_tecnica").css("max-width", ($(window).width() - 20) + "px");
        $("#detalle_ficha_tecnica").css("height", ($(window).height() - 180) + "px");

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
        $("#detalle_ficha_tecnica").css("width", anchoPortal + "px");
        $("#detalle_ficha_tecnica").css("max-width", anchoPortal + "px");
        $("#detalle_ficha_tecnica").css("height", HEIGHT_FORMULARIO + "px");

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

    anchoPortal = $(window).width() - 85;
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