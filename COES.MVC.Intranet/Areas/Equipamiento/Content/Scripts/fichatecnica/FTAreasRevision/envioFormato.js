var controladorR = siteRoot + 'Equipamiento/FTAreasRevision/';

var IMG_EDITAR_FICHA = `<img src="${siteRoot}Content/Images/btn-properties.png" title="Ver Ficha Técnica o Completar" width="19" height="19"/>`;
var IMG_EDITAR = '<img src="' + siteRoot + 'Content/Images/btn-edit.png" alt="Editar Envío" title="Editar Comentario" width="19" height="19" style="">';
var IMG_VER = '<img src="' + siteRoot + 'Content/Images/btn-open.png" alt="Ver Envío"  title="Ver Comentario"width="19" height="19" style="">';

var OPCION_GLOBAL_EDITAR = false;
var OPCION_IMPORTACION = false;

const ESTADO_SOLICITADO = 1;
const ESTADO_SUBSANADO = 7;

const ESTADO_PENDIENTE = 1;
const ESTADO_ATENDIDO = 2;

var CELDA_REV_EDITAR = 1;
var CELDA_REV_VER = 2;

const INTRANET = 1;
const EXTRANET = 2;

const FORMATO_CONEXINTMODIF = 1

const COLOR_BLOQUEADO = "#f9ede4";
const COLOR_NUMERAL = "#92CDDC";

const COLUMNA_REV_SOLICITADO = 2;
const COLUMNA_REV_SUBSANADO = 4;

var TIPO_ARCHIVO_REVISION = "AR";
var listaArchivoRevTemp = [];

const OpcionConforme = "OK";
const OpcionNoSubsanado = "NS";
const OpcionSubsanado = "S";
const OpcionObservado = "O";
const OpcionDenegado = "D";

const ColorRojo = "#F90505";
const ColorAzul = "#0544F9";
const ColorVerde = "#2BA205";
const ColorNaranja = "#FC9D02";
const ColorVioleta = "#1502FC";

const ETAPA_CONEXION = 1;
const ETAPA_INTEGRACION = 2;
const ETAPA_OPERACION_COMERCIAL = 3;
const ETAPA_MODIFICACION = 4;

var IDS_AREAS_DEL_USUARIO;
var IDS_AREAS_TOTALES;
var NOMB_AREAS_DEL_USUARIO;
var ID_AREA_REVISION;
var NOMB_AREA_REVISION;

var CODIGO_ENVIO = 0;
var CODIGO_VERSION = 0;

var listaDataRevisionFT = [];
var TIPO_ARCHIVO_REVISION_OBSCOES = "REV_OC";
var TIPO_ARCHIVO_REVISION_RPTAAGENTE = "REV_RA";
var TIPO_ARCHIVO_REVISION_RPTACOES = "REV_RC";

var TIPO_ARCHIVO_AREA_REVISION = "REV_AREA";
var listaDataRevisionArea = [];
var listaArchivoRevAreaTemp = [];
var listaRevisionImportacion = [];

var HabilitadoDescargarConfidenciales= false;

$(function () {
    HabilitadoDescargarConfidenciales = $("#hdHabilitadoDescargarArchConfidenciales").val();

    IDS_AREAS_DEL_USUARIO = $('#hdIdsAreaDelUsuario').val();
    IDS_AREAS_TOTALES = $('#hdIdsAreaTotales').val();
    NOMB_AREAS_DEL_USUARIO = $('#hdNombreAreasDelUsuario').val();
    ID_AREA_REVISION = $('#hdIdAreaRevision').val();
    NOMB_AREA_REVISION = $('#hdNombreAreaRevision').val();

    FECHA_SISTEMA = $("#hdFechaSistema").val();

    //obtener el alto disponible para el formulario (alto total - header, filtros, footer)
    HEIGHT_FORMULARIO = $(window).height() - 420;
    anchoPortal = $(window).width() - 85;

    //ocultar barra lateral izquierda
    $("#btnOcultarMenu").click();

    $('#btnInicio').click(function () {
        regresarPrincipal();
    });

    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').easytabs('select', '#tabLista');
    
    $('#btnDescargarRevA').click(function () {
        exportarFormatoConexIntegModif();
    });

    //Muestra fec max rpta
    var infoFMR = $("#hdNotaFecMaxRpta").val();
    if (infoFMR != "" && infoFMR != undefined && infoFMR != null) {
        var carpeta = parseInt($("#hfIdEstado").val());
        if (carpeta == ESTADO_SUBSANADO) {
            $("#idFecMaxRpta").html(`<b>Plazo Máximo de Revisión:</b>  ${infoFMR}`);
            $("#idFecMaxRpta").show();
        }
        if (carpeta == ESTADO_SOLICITADO) {
            $("#idFecMaxRpta").html(`<b>Plazo Máximo de Revisión:</b>  ${infoFMR}`);
            $("#idFecMaxRpta").show();
        }

    } else {
        limpiarBarraMensaje('mensajeFecMR')
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

    //guardar avance
    $('#btnGuardarRevA').click(function () {
        guardarAvanceRevAreas();
    });

    //errores
    $('#btnErroresRevA').click(function () {
        mostrarErroresRevisionAreas();
    });

    //Enviar datos
    $('#btnEnviarDatos').click(function () {
        enviarRevisionFinal();
    });

    //tabla web
    OPCION_GLOBAL_EDITAR = $("#hfTipoOpcion").val() == "E";
    listarEquipoEnvioConexIntegModif();
    visibilidadBotones();

    crearPuploadFormato3();
});

function regresarPrincipal(estado) {
    var carpeta = parseInt($("#hfIdCarpeta").val()) || 0;

    if (estado != null) {
        carpeta = estado;
    }

    document.location.href = controladorR + "Index?carpeta=" + carpeta;
}

/**
 * Listado de equipos y modos de operación del envio
 * */
function listarEquipoEnvioConexIntegModif() {

    limpiarBarraMensaje('mensajeEvento');

    var filtro = getObjetoFiltro();

    $.ajax({
        type: 'POST',
        url: controladorR + "ListarEqConexIntegModifXEnvio",
        data: {
            codigoEnvio: filtro.codigoEnvio,
            versionEnvio: filtro.codigoVersion,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

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
    filtro.codigoVersion = parseInt($("#hfIdVersion").val());

    filtro.codigoEmpresa = parseInt($("#hfEmprcodi").val()) || 0;
    filtro.codigoEtapa = parseInt($("#hfFtetcodi").val()) || 0;
    filtro.codigoProyecto = parseInt($("#hfFtprycodi").val()) || 0;
    filtro.codigoEquipos = listarCodigoYTipoChecked();
    filtro.estado = parseInt($("#hfIdEstado").val()) || 0;

    return filtro;
}

function listarCodigoYTipoChecked() { 

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
    
    thSelec = `<th style="width: 40px">Sel. Todos <br/> <input type="checkbox" id="chkSeleccionarTodo"> </th>`;

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
        
        var checked = OPCION_IMPORTACION ? ' checked ' : '';
        tdSelec = `
                <td style="width: 40px">
                    <input type="checkbox" value="${item.Fteeqcodi}" name="chkSeleccion" id="chkSeleccion_${item.Fteeqcodi}" ${checked} />
                </td>
            `;

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

    if (_esValidoFormularioExportarImportar()) {
        $.ajax({
            type: 'POST',
            url: controladorR + "GenerarFormatoConexIntegModif",
            data: {
                areasTotal: IDS_AREAS_TOTALES,
                idAreaRevision: ID_AREA_REVISION,
                codigoEnvio: filtro.codigoEnvio,
                versionEnvio: filtro.codigoVersion,
                estado: filtro.estado,
                codigoEquipos: filtro.codigoEquipos
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    window.location = controladorR + "Exportar?file_name=" + evt.Resultado;
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
        browse_button: 'btnImportarRevA',
        url: controladorR + "UploadFormatoConexIntegModif",
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

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: controladorR + 'LeerFileUpExcelFormatoConexIntegModif',
        data: {
            codigoEnvio: filtro.codigoEnvio,
            versionEnvio: filtro.codigoVersion,
            estado: filtro.estado,
            idAreaRevision: ID_AREA_REVISION,
            areasTotal: IDS_AREAS_TOTALES
        },
        success: function (evt) {
            if (evt.Resultado == "1") {

                if (OPCION_IMPORTACION) {
                    listaRevisionImportacion = evt.ListaRevisionImportacion
                    mostrarMensaje('mensajeEvento', "exito", "Se ha cargado correctamente el archivo.");
                    $("#mensajeEvento").show();
                }

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

function setearvaloresImportacionEditados() {
    var miEstenvcodi = parseInt($("#hfIdEstado").val()) || 0;
    //para cada parametros que tenga informacion importada
    for (reg in listaRevisionImportacion) {
        var equip = listaRevisionImportacion[reg];
        var lstDerivacion = equip.ListaDerivacion;
        for (item in lstDerivacion) {
            var filaRevision = lstDerivacion[item];

            var miFtitcodi = filaRevision.Ftitcodi;
            var miFteeqcodi = filaRevision.Fteeqcodi;

            var elemento = listaDataRevisionArea.find(x => x.Fteeqcodi === miFteeqcodi && x.Ftitcodi === miFtitcodi);

            if (elemento != null) {
                pos = listaDataRevisionArea.findIndex(function (x) { return x.Ftitcodi === miFtitcodi; });

                if (miEstenvcodi == ESTADO_SOLICITADO) {
                    listaDataRevisionArea[pos].MsgSolicitud = filaRevision.MsgSolicitud;
                    listaDataRevisionArea[pos].IdValorEstadoSolicitado = filaRevision.IdValorEstadoSolicitado;
                }
                if (miEstenvcodi == ESTADO_SUBSANADO) {
                    listaDataRevisionArea[pos].MsgSubsanacion = filaRevision.MsgSubsanacion;
                    listaDataRevisionArea[pos].IdValorEstadoSubsanado = filaRevision.IdValorEstadoSubsanado;
                }
            }
        }
    }
}

//FORMULARIO
function listarFicha() {
    var filtro = getObjetoFiltroFT();
    restaurarExcelImagen();
    $("#detalle_ficha_tecnica").hide();
    $("#detalle_ficha_tecnica").html("");

    $.ajax({
        type: 'POST',
        url: controladorR + "ObtenerDatosFT",
        data: {
            fteeqcodi: filtro.fteeqcodi,
            /*areasUsuario: IDS_AREAS_DEL_USUARIO,*/
            areasTotal: IDS_AREAS_TOTALES,
            nombAreasUsuario: NOMB_AREAS_DEL_USUARIO,
            idAreaRevision: ID_AREA_REVISION,
            nombAreaRevision: NOMB_AREA_REVISION
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                var lstConfiguraciones = evt.ReporteDatoXEq.ListaItemConfig
                _determinarColumnasAdicionales(lstConfiguraciones);

                listaDataRevisionFT = evt.ListaRevisionParametrosAFT;
                listaDataRevisionArea = evt.ListaRevisionAreasFT;
                if (OPCION_IMPORTACION)
                    setearvaloresImportacionEditados();

                MODELO_FICHA = evt.ReporteDatoXEq;

                CODIGO_ENVIO = filtro.codigoEnvio;
                CODIGO_VERSION = parseInt($("#hfIdVersion").val()) || 0;

                //Inicializar vista previa
                $("#detalle_ficha_tecnica").show();
                $("#detalle_ficha_tecnica").css("width", anchoPortal + "px"); //max width para que sea el maximo de la pantalla del usuario
                $("#detalle_ficha_tecnica").css("max-width", anchoPortal + "px"); //max width para que sea el maximo de la pantalla del usuario
                $("#detalle_ficha_tecnica").css("height", HEIGHT_FORMULARIO + "px");
                $("#detalle_ficha_tecnica").html(_extranet_generarHtmlReporteDetalleFichaTecnica(MODELO_FICHA));

                agregarBloqueRevisionAdminFT(evt);

                agregarBloqueAreas(evt);

            } else {

                mostrarMensaje('mensajeEvento', "error", evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensajeEvento', 'error', 'Ha ocurrido un error.');
        }
    });
}


function visibilidadBotones() {

    var estadoEnvio = parseInt($("#hfIdEstado").val()) || 0;

    //Botones Principales
    if (OPCION_GLOBAL_EDITAR) {
        if (estadoEnvio == ESTADO_SOLICITADO || estadoEnvio == ESTADO_SUBSANADO) {
            $("#bq_guardar").css("display", "inline-table");
            $("#bq_importar").css("display", "inline-table");
            $("#bq_descargar").css("display", "inline-table");
            $("#bq_errores").css("display", "inline-table");
            $("#bq_enviar").css("display", "inline-table");

            $("#bq_importar").css("float", "right");
            $("#bq_descargar").css("float", "right");
            $("#bq_errores").css("float", "right");
            $("#bq_enviar").css("float", "right");
        }
    } else {
        if (estadoEnvio == ESTADO_SOLICITADO || estadoEnvio == ESTADO_SUBSANADO) {
            $("#bq_descargar").css("display", "inline-table");
            $("#bq_descargar").css("float", "right");
        }
    }


}


function agregarBloqueRevisionAdminFT(evt) {
    limpiarBarraMensaje('mensajeEvento');

    agregarColumnasRevisionAdminFT(evt.ListaRevisionParametrosAFT);
}

function agregarColumnasRevisionAdminFT(listaRevision) {
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

                            switch (i) {
                                case 1: cadena = obtenerHtmlCeldaRevisionFilaBloqueada(i, miFtitcodi, nuevaCol, registro.Fteeqcodi, registro.ValObsCoes, registro.ListaArchivosObsCoes); nuevaCol.classList.add("celdaREV"); nuevaCol.setAttribute("id", "celdaCont_" + i + "_" + miFtitcodi); break;
                                case 2: cadena = obtenerHtmlCeldaRevisionFilaBloqueada(i, miFtitcodi, nuevaCol, registro.Fteeqcodi, registro.ValRptaAgente, registro.ListaArchivosRptaAgente); nuevaCol.classList.add("celdaREV"); nuevaCol.setAttribute("id", "celdaCont_" + i + "_" + miFtitcodi); break;
                                case 3: cadena = obtenerHtmlCeldaRevisionFilaBloqueada(i, miFtitcodi, nuevaCol, registro.Fteeqcodi, registro.ValRptaCoes, registro.ListaArchivosRptaCoes); nuevaCol.classList.add("celdaREV"); nuevaCol.setAttribute("id", "celdaCont_" + i + "_" + miFtitcodi); break;
                                case 4: nuevaCol.style.background = COLOR_BLOQUEADO; //coloreo Fondo
                                    cadena = mostrarListadoHtml(CELDA_REV_VER, registro.ValEstado, registro.ListaEstados, miPropcodi, miConcepcodi, miFtitcodi, registro.Fteeqcodi, INTRANET);
                                    nuevaCol.classList.add("celdaREVEstado"); break;
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
                       <a title="Ver registro" href="#" style="float: right;color: green;font-weight: bold;" onClick="mostrarPopupObservacion(${indiceColumnaBloque}, ${fteeqcodi}, ${ftitcodi},${accion},'${msgCodificado}');">Ver Más </a>
                `;
            } else {
                htmlBtn = `
                    <td style='width: 25px;border: 0;${styleFondo}'>
                        
                    </td>
                `;
            }
        }
    }

    var numArchivos = listaArchivos != null ? listaArchivos.length : 0;
    var htmlArchivos = ''; //
    if (numArchivos > 0) htmlArchivos = generarTablaListaBodyXObs(listaArchivos, accion, false, indiceColumnaBloque, fteeqcodi, ftitcodi);

    var htmlDiv = '';

    var estiloVisible = agregoCssParaOcultarCelda ? ` style="display: none;"`: '';
        htmlDiv += `
        <table>
            <tr id="bloqueTexto_${indiceColumnaBloque}_${ftitcodi}" ${estiloVisible}>
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

    return htmlDiv;
}


//Observaciones html 
function mostrarPopupObservacion(nColumna, fteeqcodi, ftitcodi, accion, objMsg) {

    popupFormularioObservacion(nColumna, fteeqcodi, ftitcodi, accion, objMsg);
}

function popupFormularioObservacion(nColumna, fteeqcodi, ftitcodi, accion, objMsg) {

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

    });

    setTimeout(function () {
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


function cargarHtmlArchivoXObs(nColumna, fteeqcodi, ftitcodi, accion) {

    var prefijo = "_sec_doc_" + ftitcodi;
    var htmlSec = "<div id=div_" + prefijo + ">";
    htmlSec += generarHtmlTablaDocumentoXObs(nColumna, prefijo, fteeqcodi, ftitcodi, accion);
    htmlSec += "</div>";

    $("#html_archivos_x_obs").html(htmlSec);

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

function agregarRowXObs(nColumna, prefijo, fteeqcodi, ftitcodi, nuevoNombre, nombreArchivo, accion) {

    var elemento = listaDataRevisionFT.find(x => x.Fteeqcodi === fteeqcodi && x.Ftitcodi === ftitcodi);

    if (elemento != null) {

        listaArchivoRevTemp.push({ EsNuevo: true, Ftearcnombrefisico: nuevoNombre, Ftearcnombreoriginal: nombreArchivo });

        $("#listaArchivos" + prefijo).html(generarTablaListaBodyXObs(listaArchivoRevTemp, accion, true, nColumna, fteeqcodi, ftitcodi));
    }

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

function eliminarRowXObs(nColumna, fteeqcodi, ftitcodi, pos) {

}

function getTipoArchivoXColumna(nColumna) {
    if (nColumna == 1) return TIPO_ARCHIVO_REVISION_OBSCOES;
    if (nColumna == 2) return TIPO_ARCHIVO_REVISION_RPTAAGENTE;
    if (nColumna == 3) return TIPO_ARCHIVO_REVISION_RPTACOES;

    return "NA";
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
                            <td style="text-align:left;${styleFondo}" title='${nomb}'>
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
        } else { //para que muestre limpio la caja de combo
            htmlDiv += `
                    <option value="0" disabled selected hidden></option>
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



////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//   BLOQUE AREAS
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function agregarBloqueAreas(evt) {
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
                var cadena1 = 'Revisión ' + NOMB_AREA_REVISION;
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
                            if (!OPCION_GLOBAL_EDITAR) { //VER
                                

                                if (i == 1) {

                                    cadena = registro.AreasNombAsignadas;
                                    nuevaCol.classList.add("celdaREV_areas");
                                    nuevaCol.style.background = COLOR_BLOQUEADO;
                                    nuevaCol.setAttribute("id", "celdaContArea_" + i + "_" + miFtitcodi);
                                }

                                if (i == 2) {
                                    cadena = obtenerHtmlAreasCeldaRevisionFilaBloqueada(i, miFtitcodi, nuevaCol, registro.CeldaRevSolicitudEstaBloqueada, registro.Fteeqcodi, registro.MsgSolicitud, registro.IdValorEstadoSolicitado, registro.ListaArchivosSolicitados);
                                    nuevaCol.classList.add("celdaREV_areas");
                                    nuevaCol.setAttribute("id", "celdaContArea_" + i + "_" + miFtitcodi);
                                }

                                if (i == 3) {                                    
                                    nuevaCol.style.background = COLOR_BLOQUEADO; //coloreo Fondo
                                    cadena = mostrarListadoAreasHtml(CELDA_REV_VER, registro.IdValorEstadoSolicitado, registro.ListaEstadosSolicitado, miPropcodi, miConcepcodi, miFtitcodi, registro.Fteeqcodi, COLUMNA_REV_SOLICITADO);

                                    nuevaCol.classList.add("celdaREVEstado_areas");
                                }

                                if (i == 4) {
                                    cadena = obtenerHtmlAreasCeldaRevisionFilaBloqueada(i, miFtitcodi, nuevaCol, registro.CeldaRevSubsanadoEstaBloqueada, registro.Fteeqcodi, registro.MsgSubsanacion, registro.IdValorEstadoSubsanado, registro.ListaArchivosSubsanados);
                                    nuevaCol.classList.add("celdaREV_areas");
                                    nuevaCol.setAttribute("id", "celdaContArea_" + i + "_" + miFtitcodi);
                                }

                                if (i == 5) {                                    
                                    nuevaCol.style.background = COLOR_BLOQUEADO; //coloreo Fondo
                                    cadena = mostrarListadoAreasHtml(CELDA_REV_VER, registro.IdValorEstadoSubsanado, registro.ListaEstadosSubsanado, miPropcodi, miConcepcodi, miFtitcodi, registro.Fteeqcodi, COLUMNA_REV_SUBSANADO);

                                    nuevaCol.classList.add("celdaREVEstado_areas");
                                }

                            }
                            else { //Verifico por cada Columna


                                if (i == 1) {
                                    
                                    cadena = registro.AreasNombAsignadas;
                                    nuevaCol.classList.add("celdaREV_areas");
                                    nuevaCol.style.background = COLOR_BLOQUEADO;
                                    nuevaCol.setAttribute("id", "celdaContArea_" + i + "_" + miFtitcodi);
                                }
                                if (i == 2) {
                                    cadena = obtenerHtmlAreasCeldaRevisionFilaNoBloqueada(i, miFtitcodi, nuevaCol, registro.CeldaRevSolicitudEstaBloqueada, registro.Fteeqcodi, registro.MsgSolicitud, registro.IdValorEstadoSolicitado, registro.ListaArchivosSolicitados);
                                    nuevaCol.classList.add("celdaREV_areas");
                                    nuevaCol.setAttribute("id", "celdaContArea_" + i + "_" + miFtitcodi);
                                }
                                
                                if (i == 3) {
                                    if (registro.CeldaEstadoSolicitudEstaBloqueada) {
                                        nuevaCol.style.background = COLOR_BLOQUEADO; //coloreo Fondo
                                        cadena = mostrarListadoAreasHtml(CELDA_REV_VER, registro.IdValorEstadoSolicitado, registro.ListaEstadosSolicitado, miPropcodi, miConcepcodi, miFtitcodi, registro.Fteeqcodi, COLUMNA_REV_SOLICITADO);
                                    }
                                    else {
                                        cadena = mostrarListadoAreasHtml(CELDA_REV_EDITAR, registro.IdValorEstadoSolicitado, registro.ListaEstadosSolicitado, miPropcodi, miConcepcodi, miFtitcodi, registro.Fteeqcodi, COLUMNA_REV_SOLICITADO);
                                    }
                                    
                                    nuevaCol.classList.add("celdaREVEstado_areas");
                                }
                                if (i == 4) {
                                    cadena = obtenerHtmlAreasCeldaRevisionFilaNoBloqueada(i, miFtitcodi, nuevaCol, registro.CeldaRevSubsanadoEstaBloqueada, registro.Fteeqcodi, registro.MsgSubsanacion, registro.IdValorEstadoSubsanado, registro.ListaArchivosSubsanados);
                                    nuevaCol.classList.add("celdaREV_areas");
                                    nuevaCol.setAttribute("id", "celdaContArea_" + i + "_" + miFtitcodi);
                                }

                                if (i == 5) {
                                    
                                    if (registro.CeldaEstadoSubsanadoEstaBloqueada) {
                                        nuevaCol.style.background = COLOR_BLOQUEADO; //coloreo Fondo
                                        cadena = mostrarListadoAreasHtml(CELDA_REV_VER, registro.IdValorEstadoSubsanado, registro.ListaEstadosSubsanado, miPropcodi, miConcepcodi, miFtitcodi, registro.Fteeqcodi, COLUMNA_REV_SUBSANADO);
                                    }
                                    else {
                                        cadena = mostrarListadoAreasHtml(CELDA_REV_EDITAR, registro.IdValorEstadoSubsanado, registro.ListaEstadosSubsanado, miPropcodi, miConcepcodi, miFtitcodi, registro.Fteeqcodi, COLUMNA_REV_SUBSANADO);
                                    }
                                    
                                    nuevaCol.classList.add("celdaREVEstado_areas");
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

function obtenerHtmlAreasCeldaRevisionFilaBloqueada(indiceColumnaBloque, ftitcodi, columna, esCeldaBloqueada, fteeqcodi, valorCelda, opcionCombo, listaArchivos) {
    var cadena = "";
        
    columna.style.background = COLOR_BLOQUEADO; //coloreo Fondo
    if (valorCelda != null && valorCelda != "") {
        cadena = mostrarTextoAreasHtml(indiceColumnaBloque, ftitcodi, CELDA_REV_VER, fteeqcodi, valorCelda, listaArchivos, false);
    } else {
        cadena = mostrarTextoAreasHtml(indiceColumnaBloque, ftitcodi, CELDA_REV_VER, fteeqcodi, "", listaArchivos, false);
    }
    
    return cadena;
}

function obtenerHtmlAreasCeldaRevisionFilaNoBloqueada(indiceColumnaBloque, ftitcodi, columna, esCeldaBloqueada, fteeqcodi, valorCelda, opcionCombo, listaArchivos) {
    var cadena = "";
    
    if (esCeldaBloqueada) {
        columna.style.background = COLOR_BLOQUEADO; //coloreo Fondo
        if (valorCelda != null && valorCelda != "") {
            cadena = mostrarTextoAreasHtml(indiceColumnaBloque, ftitcodi, CELDA_REV_VER, fteeqcodi, valorCelda, listaArchivos, false);
        } else {
            cadena = mostrarTextoAreasHtml(indiceColumnaBloque, ftitcodi, CELDA_REV_VER, fteeqcodi, "", listaArchivos, false);
        }
    } else {//esta desbloqueada en la intranet
        
        cadena = mostrarTextoAreasHtml(indiceColumnaBloque, ftitcodi, CELDA_REV_EDITAR, fteeqcodi, valorCelda, listaArchivos, false);
    }
    return cadena;
}

function mostrarListadoAreasHtml(accion, valEstado, listado, miPropcodi, miConcepcodi, miFtitcodi, fteeqcodi, columnaRevisada) {
    var htmlDiv = '';
    var tieneEstadoPreseleccionado = false;
    var htmlColoSelect = '';
    var htmlHabilitacion = "";

    var miEstenvcodi = parseInt($("#hfIdEstado").val()) || 0;

    var textoEstado = "";

    if (valEstado != null) {
        switch (valEstado.trim()) {
            case OpcionConforme: textoEstado = "Conforme"; break;
            case OpcionObservado: textoEstado = "Observado"; break;
        }
    } else {
        valEstado = "";
    }

    if (accion == CELDA_REV_VER)
        htmlHabilitacion = "disabled";

    //cambio estilo del combo seleccionado
    var colorEstado = obtenerColorTextoSegunEstado(valEstado);

    //Para solicitud e intranet, se debe mostrar vacio en Estado
    if (valEstado == "") {
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

        if (columnaRevisada == COLUMNA_REV_SOLICITADO) {
            htmlDiv += `
                <select ${htmlHabilitacion} name="" id="ckbAreaEstado_Solicitado_${miFtitcodi}_${miPropcodi}_${miConcepcodi}" onchange="CambiarAreaOpcionCombo('${miPropcodi}','${miConcepcodi}', '${miFtitcodi}','${fteeqcodi}','${COLUMNA_REV_SOLICITADO}');"
                style="border: none; background: transparent; width: 86px; text-align: center;font-size: 11px;font-weight: bold; ${htmlColoSelect} " >
            `;
        } else {
            if (columnaRevisada == COLUMNA_REV_SUBSANADO) {
                htmlDiv += `
                    <select ${htmlHabilitacion} name="" id="ckbAreaEstado_Subsanado_${miFtitcodi}_${miPropcodi}_${miConcepcodi}" onchange="CambiarAreaOpcionCombo('${miPropcodi}','${miConcepcodi}', '${miFtitcodi}','${fteeqcodi}','${COLUMNA_REV_SUBSANADO}');"
                    style="border: none; background: transparent; width: 86px; text-align: center;font-size: 11px;font-weight: bold; ${htmlColoSelect} " >
                `;
                }
        }
        

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

function CambiarAreaOpcionCombo(miPropcodi, miConcepcodi, miFtitcodi, miFteeqcodi, columnaRevisada) {
    
    var miEstenvcodi = parseInt($("#hfIdEstado").val()) || 0;

    var columna = "";
    if (columnaRevisada == COLUMNA_REV_SOLICITADO) columna = "Solicitado";
    if (columnaRevisada == COLUMNA_REV_SUBSANADO) columna = "Subsanado";

    var opcionSeleccionada = $("#ckbAreaEstado_" + columna + "_" + miFtitcodi + "_" + miPropcodi + "_" + miConcepcodi).val();

    //cambio estilo del combo seleccionado
    var colorTexto = obtenerColorTextoSegunEstado(opcionSeleccionada);
    $("#ckbAreaEstado_" + columna + "_" + miFtitcodi + "_" + miPropcodi + "_" + miConcepcodi).css("color", colorTexto);

    //reemplazo el IDVALOR en el array el valor del seleccionado
    var ftitcodi = parseInt(miFtitcodi) || 0;
    const registro = listaDataRevisionArea.find(x => x.Ftitcodi === ftitcodi );
    var pos = -1;
    if (registro != null) {
        pos = listaDataRevisionArea.findIndex(function (x) { return x.Ftitcodi === ftitcodi; });
        

        if (miEstenvcodi == ESTADO_SOLICITADO)
            listaDataRevisionArea[pos].IdValorEstadoSolicitado = opcionSeleccionada;
        if (miEstenvcodi == ESTADO_SUBSANADO)
            listaDataRevisionArea[pos].IdValorEstadoSubsanado = opcionSeleccionada;
    }

    
}


function mostrarTextoAreasHtml(indiceColumnaBloque, ftitcodi, accion, fteeqcodi, msgHtml, listaArchivos, agregoCssParaOcultarCelda) {

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
                    <td style='width: 25px;border: 0;'>
                       <a title="Editar registro" href="#" onClick=" mostrarPopupAreasObservacion(${indiceColumnaBloque}, ${fteeqcodi}, ${ftitcodi},${accion},'${msgCodificado}');">${IMG_EDITAR} </a>
                    </td>
                `;
    } else {
        if (accion == CELDA_REV_VER) {
            styleFondo = 'background:' + COLOR_BLOQUEADO;

            //solo se muestra el boton Ver si hay comentario
            var hayBtnVerMasLectura = (listaArchivos != null ? listaArchivos.length : 0) > 0 || tamTexto > 20;
            if (hayBtnVerMasLectura) {
                htmlBtn = `
                       <a title="Ver registro" href="#" style="float: right;color: green;font-weight: bold;" onClick="mostrarPopupAreasObservacion(${indiceColumnaBloque}, ${fteeqcodi}, ${ftitcodi},${accion},'${msgCodificado}');">Ver Más </a>
                `;
            } else {
                htmlBtn = `
                    <td style='width: 25px;border: 0;${styleFondo}'>
                        
                    </td>
                `;
            }
        }
    }

    var numArchivos = listaArchivos != null ? listaArchivos.length : 0;
    var htmlArchivos = ''; //
    if (numArchivos > 0) htmlArchivos = generarTablaAreaListaBodyXObs(listaArchivos, accion, false, indiceColumnaBloque, fteeqcodi, ftitcodi);

    var htmlDiv = '';

    var estiloVisible = agregoCssParaOcultarCelda ? ` style="display: none;"` : '';
    htmlDiv += `
        <table>
            <tr id="bloqueAreaTexto_${indiceColumnaBloque}_${ftitcodi}" ${estiloVisible}>
                   <div id="datoAreaCelda_${indiceColumnaBloque}_${ftitcodi}" style="word-break: break-all;">
                     ${htmlEditado}
                   </div>
                   <div>
                        ${htmlBtn}
                    </div>
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

    return htmlDiv;
}


////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Archivos adjuntos de areas
////////////////////////////////////////////////////////////////////////////////////////////////////////////

function cargarAreasHtmlArchivoXObs(nColumna, fteeqcodi, ftitcodi, accion) {

    var prefijo = "_sec_doc_" + ftitcodi;
    var htmlSec = "<div id=div_" + prefijo + ">";
    htmlSec += generarHtmlAreaTablaDocumentoXObs(nColumna, prefijo, fteeqcodi, ftitcodi, accion);
    htmlSec += "</div>";

    $("#html_archivosAreas_x_obs").html(htmlSec);

    //plugin archivo
    pUploadAreaArchivoXObs(nColumna, prefijo, fteeqcodi, ftitcodi, accion, TIPO_ARCHIVO_AREA_REVISION);

}

function generarHtmlAreaTablaDocumentoXObs(nColumna, idPrefijo, fteeqcodi, ftitcodi, accion) {

    var listaArchivo = [];
    //Obtengo listado de archivo segun fila
    var elemento = listaDataRevisionArea.find(x => x.Ftitcodi === ftitcodi);
    switch (nColumna) {

        case 2: listaArchivo = elemento.ListaArchivosSolicitados; break;
        case 4: listaArchivo = elemento.ListaArchivosSubsanados; break;
    }

    //clono la lista
    listaArchivoRevAreaTemp = [];
    for (key in listaArchivo) {
        var reg = listaArchivo[key];

        listaArchivoRevAreaTemp.push(reg);
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

    html += generarTablaAreaListaBodyXObs(listaArchivoRevAreaTemp, accion, true, nColumna, fteeqcodi, ftitcodi);

    html += `
                        </div>

                    </td>
                </tr>
            </table>
                <div style='clear:both; height:10px;width:100px;'></div>
    `;

    return html;
}


function generarTablaAreaListaBodyXObs(listaArchivo, accion, enPopup, nColumna, fteeqcodi, ftitcodi) {
    
    listaArchivo.sort((x, y) => x.Ftearcnombreoriginal - y.Ftearcnombreoriginal); // ordenamieto

    var styleFondo = "";

    if (accion == CELDA_REV_VER) {
        if (enPopup) {
            styleFondo = 'background: white;  word-break: break-all;';
        } else {
            styleFondo = 'background:' + COLOR_BLOQUEADO + '; border: 0px;  word-break: break-all;';
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
        if (accion == CELDA_REV_EDITAR && enPopup) {
            html += `     
                            <td onclick="eliminarRowAreaXObs(${nColumna}, ${fteeqcodi}, ${ftitcodi},${i})" title='Eliminar archivo' style="width:30px;cursor:pointer;${styleFondo}">
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
function verAreaArchivoXObs(nColumna, fteeqcodi, ftitcodi, pos, enPopup) {

    var regArchivo;
    var tipoArchivo = TIPO_ARCHIVO_AREA_REVISION;
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

function pUploadAreaArchivoXObs(nColumna, prefijo, fteeqcodi, ftitcodi, accion, tipoArchivo) {
    var uploaderP23 = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFile' + prefijo,
        container: document.getElementById('container'),
        chunks_size: '50mb',
        
        multipart_params: {
            "idEnvio": getObjetoFiltroGuardarRevArea().codigoEnvio,
            "idVersion": getObjetoFiltroGuardarRevArea().codigoVersion,
            "idElemento": ftitcodi,
            "tipoArchivo": tipoArchivo
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
                agregarRowAreaXObs(nColumna, prefijo, fteeqcodi, ftitcodi, JSON.parse(result.response).nuevonombre, file.name, accion);
            },
            Error: function (up, err) {
                loadValidacionFile(err.code + "-" + err.message);
                if (err.code == -600) //error de tamaño
                    err.message = "Error: El archivo que se adjuntó supera el tamaño límite (50MB) por archivo. ";
                alert(err.message);
            }
        }
    });
    uploaderP23.init();
}

function agregarRowAreaXObs(nColumna, prefijo, fteeqcodi, ftitcodi, nuevoNombre, nombreArchivo, accion) {

    var elemento = listaDataRevisionArea.find(x => x.Ftitcodi === ftitcodi);

    if (elemento != null) {
        
        listaArchivoRevAreaTemp.push({ EsNuevo: true, Ftearcnombrefisico: nuevoNombre, Ftearcnombreoriginal: nombreArchivo });

        $("#listaArchivos" + prefijo).html(generarTablaAreaListaBodyXObs(listaArchivoRevAreaTemp, accion, true, nColumna, fteeqcodi, ftitcodi));
    }

}

function descargarArchivoAreaXObs(nColumna, fteeqcodi, ftitcodi, pos, enPopup) {
    var regArchivo;
    var tipoArchivo = TIPO_ARCHIVO_AREA_REVISION;
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
        }

        regArchivo = listaArchivo[pos];
    }
    
    if (regArchivo != null) {
        window.location = controlador + 'DescargarArchivoEnvio?fileName=' + regArchivo.Ftearcnombrefisico + "&fileNameOriginal=" + regArchivo.Ftearcnombreoriginal
            + '&idElemento=' + ftitcodi + '&tipoArchivo=' + tipoArchivo + '&idEnvio=' + getObjetoFiltro().codigoEnvio + '&idVersion=' + getObjetoFiltro().codigoVersion;
    }
}

function eliminarRowAreaXObs(nColumna, fteeqcodi, ftitcodi, pos) {

    listaArchivoRevAreaTemp.splice(pos, 1);

    var prefijo = "_sec_doc_" + ftitcodi;

    $("#listaArchivos" + prefijo).html(generarTablaAreaListaBodyXObs(listaArchivoRevAreaTemp, CELDA_REV_EDITAR, true, nColumna, fteeqcodi, ftitcodi));
}

//Observaciones html 
function mostrarPopupAreasObservacion(nColumna, fteeqcodi, ftitcodi, accion, objMsg) {

    popupFormularioAreasObservacion(nColumna, fteeqcodi, ftitcodi, accion, objMsg);
}

function popupFormularioAreasObservacion(nColumna, fteeqcodi, ftitcodi, accion, objMsg) {
    limpiarBarraMensaje('mensajePopupCelda');

    tinymce.remove();
    $('#btnGuardarAreasObsHtml').unbind();
    $("#htmlArchivos").unbind();
    $("#htmlArchivos").html('');
    $("#idFormularioObservacion").html('');

    var objMsg = JSON.parse(decodeURIComponent(objMsg));
    var msgHtml = "";


    if (accion == CELDA_REV_EDITAR) {
        const registro = listaDataRevisionArea.find(x => x.Ftitcodi === ftitcodi);

        if (nColumna == COLUMNA_REV_SOLICITADO) {
            msgHtml = registro != null ? registro.MsgSolicitud : "";
        }

        if (nColumna == COLUMNA_REV_SUBSANADO) {
            msgHtml = registro != null ? registro.MsgSubsanacion : "";
        }
        
    } else {
        msgHtml = objMsg.Mensaje != null && objMsg.Mensaje != undefined ? objMsg.Mensaje : "";
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
        <div id='html_archivosAreas_x_obs'></div>
    `;

    if (esEditable) {
        htmlDiv += `
            <tr>                
                <td colspan="3" style="padding-top: 2px; ">
                    <input type="button" id="btnGuardarAreasObsHtml" value="Guardar" style ="margin: 0px 10px 0px 350px;"/>
                </td>
            </tr>
        `;
    }

    $('#idFormularioObservacion').html(htmlDiv);

    $('#btnGuardarAreasObsHtml').click(function () {
        _guardarAreasObsHtml(nColumna, fteeqcodi, ftitcodi);

        //quitar flag importación para evitar precargar los valores importados
        OPCION_IMPORTACION = false;
    });

    setTimeout(function () {
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
        cargarAreasHtmlArchivoXObs(nColumna, fteeqcodi, ftitcodi, accion);


    }, 50);

}

function _guardarAreasObsHtml(nColumna, fteeqcodi, ftitcodi) {
    limpiarBarraMensaje('mensajePopupCelda');
    var listaArchivo = [];

    var htmlObs = $("#contenido_html_obs").val();

    var msg = validarMensajeCelda(htmlObs);

    if (msg == "") {
        //reemplazo en el array
        const registro = listaDataRevisionArea.find(x => x.Ftitcodi === ftitcodi);
        if (registro != null) {
            var pos = listaDataRevisionArea.findIndex(function (x) { return x.Ftitcodi === ftitcodi; });

            if (nColumna == 2) { //Solicitado
                listaDataRevisionArea[pos].MsgSolicitud = htmlObs;
                listaDataRevisionArea[pos].ListaArchivosSolicitados = listaArchivoRevAreaTemp;
            }
            if (nColumna == 4) { //Subsanado
                listaDataRevisionArea[pos].MsgSubsanacion = htmlObs;
                listaDataRevisionArea[pos].ListaArchivosSubsanados = listaArchivoRevAreaTemp;
            }
            listaArchivo = listaArchivoRevAreaTemp;

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

        $("#datoAreaCelda_" + nColumna + "_" + ftitcodi).html(htmlEditado)

        //Actualiza el listado de archivos
        $("#data_AreaArchivos_" + nColumna + "_" + ftitcodi).html(generarTablaAreaListaBodyXObs(listaArchivo, CELDA_REV_EDITAR, false, nColumna, fteeqcodi, ftitcodi));


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

//GUARDADO AVANCE
function guardarAvanceRevAreas() {
    limpiarBarraMensaje("mensajeEvento");
    if (OPCION_GLOBAL_EDITAR) { 

        var filtro = getObjetoFiltroGuardarRevArea();

        var modeloWeb = {
            Ftenvcodi: filtro.codigoEnvio,
            Ftevercodi: filtro.codigoVersion,
            Fteeqcodi: filtro.fteeqcodi,
            Faremcodi: filtro.faremcodi,
            ListaRevisionFT: listaDataRevisionArea
            //ListaRevisionContenido : []
        };

        if (modeloWeb.Fteeqcodi > 0) {
            return $.ajax({
                type: 'POST',
                dataType: 'json',
                url: controladorR + "GuardarDatosRevArea",
                contentType: "application/json",
                data: JSON.stringify({
                    modelWeb: modeloWeb
                }),
                //beforeSend: function () {
                //    //mostrarExito("Enviando Información ..");
                //},
                success: function (evt) {
                    if (evt.Resultado != "-1") {

                        if (evt.Resultado == "1") {
                            mostrarMensaje('mensajeEvento', 'exito', "Se guardó el avance correctamente.");
                            $("#bloquePorcentajeAvance").html(evt.HtmlPorcentajeAvance);
                        }

                    } else {
                        mostrarMensaje('mensajeEvento', 'error', "Ha ocurrido un error: " + evt.Mensaje);
                    }
                },
                error: function (err) {
                    mostrarMensaje('mensajeEvento', 'error', "Ha ocurrido un error.");
                }
            });
        }
    } else {
        mostrarMensaje('mensajeEvento', 'error', "No tiene permiso para guardar la información.");
    }
}

function getObjetoFiltroGuardarRevArea() {
    var filtro = {};

    filtro.fteeqcodi = parseInt($("#hfIdEquipoEnvio").val()) || 0;
    filtro.faremcodi = parseInt($("#hdIdAreaRevision").val()) || 0;
    filtro.codigoEnvio = parseInt($("#hfIdEnvio").val()) || 0;
    filtro.codigoVersion = parseInt($("#hfIdVersion").val());


    return filtro;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Errores
////////////////////////////////////////////////////////////////////////////////////////////////////////////
function mostrarErroresRevisionAreas() {
    limpiarBarraMensaje("mensajeEvento");
    $("#idTerrores").html('');

    var filtro = getObjetoFiltroGuardarRevArea();

    $.ajax({
        type: 'POST',
        url: controladorR + "ListarErroresRevAreas",
        data: {
            idEnvio: filtro.codigoEnvio,
            idVersion: filtro.codigoVersion,
            idArea: filtro.faremcodi,
            tipoFormato: FORMATO_CONEXINTMODIF
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                var htmlTerrores = dibujarTablaError(evt.ListadoErroresRevArea);
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
            } else {
                mostrarMensaje('mensajeEvento', 'error', "Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensajeEvento', 'error', "Ha ocurrido un error.");
        }
    });
    
}


function dibujarTablaError(listaError) {

    var cadena = `
        <div style='clear:both; height:5px'></div>
            <table id='tablaError' border='1' class='pretty tabla-adicional' cellspacing='0' style=''>
                <thead>
                    <tr>
                        <th style='width: 10px;'>#</th>
                        <th style='width: 300px;'>Nombre <br>de equipo</th>
                        <th style='width: 100px;'>Numeral</th>
                        <th style='width: 400px;'>Tipo Error</th>
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
                        <td style='width:  300px; white-space: inherit;'>${item.NombEquipo}</td>
                        <td style='width:  100px; white-space: inherit;'>${item.Numeral}</td>
                        <td style='width: 400px; white-space: inherit;'>${item.Mensaje}</td>
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
/// Enviar Revision Final
////////////////////////////////////////////////////////////////////////////////////////////////////////////
function enviarRevisionFinal() {
    limpiarBarraMensaje("mensajeEvento");
    if (OPCION_GLOBAL_EDITAR) {

        var filtro = getObjetoFiltroGuardarRevArea();

        $.ajax({
            type: 'POST',
            url: controladorR + "EnviarRevisionFinal",
            data: {
                idEnvio: filtro.codigoEnvio,
                idVersion: filtro.codigoVersion,
                idArea: filtro.faremcodi,
                tipoFormato: FORMATO_CONEXINTMODIF
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {

                    //Si hay errores, muestra el listado de estos
                    if (evt.Resultado == "0") {
                        $("#idTerrores").html('');
                        var htmlTerrores = dibujarTablaError(evt.ListadoErroresRevArea);
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
                    } else {
                        if (evt.Resultado == "1") {
                            alert("Se envió la revisión del envío correctamente.");
                            regresarPrincipal(ESTADO_ATENDIDO);
                        }
                    }
                } else {
                    mostrarMensaje('mensajeEvento', 'error', "Ha ocurrido un error: " + evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensajeEvento', 'error', "Ha ocurrido un error.");
            }
        });
    } else {
        mostrarMensaje('mensajeEvento', 'error', "No tiene permiso para enviar la información.");
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// General
////////////////////////////////////////////////////////////////////////////////////////////////////////////

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

function _irAFooterPantalla() {
    setTimeout(function () {
        $('html, body').scrollTop($("#container").offset().top);
    }, 50);
}