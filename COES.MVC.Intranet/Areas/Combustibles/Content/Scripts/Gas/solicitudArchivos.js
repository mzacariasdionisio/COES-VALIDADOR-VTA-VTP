var HOT_SUSTENTO = null;

var IMG_CANCELAR = '<img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="" width="19" height="19" style="">';
var IMG_DESCARGAR = '<img src="' + siteRoot + 'Content/Images/btn-download.png" alt="" width="19" height="19" style="">';

////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Archivos de Sustento
////////////////////////////////////////////////////////////////////////////////////////////////////////////
function cargarHtmlDocumentoXEnvioTabSustento(objTab) {
    var seccion = objTab.SeccionCombustible;

    var html = '';

    var prefijo = "_sec_doc_" + seccion.Seccion.Ccombcodi;
    var htmlSec = "<div id='div_" + prefijo + "'>";
    htmlSec += generarHtmlTablaDocumentoTabSustento(seccion.Seccion.Ccombcodi, prefijo, seccion.Seccion.Titulo, seccion.Seccion.Descripcion, objTab.EsEditable);
    htmlSec += "</div>";

    html += htmlSec;

    $("#html_archivos").html(html);

    cargarHandsontableArchivoXTabSustento(objTab, "listaArchivos" + prefijo);

    var idEnvio = 0;
    pUploadArchivoTabSustento(idEnvio, seccion.Seccion.Ccombcodi, "_sec_doc_" + seccion.Seccion.Ccombcodi);
}

function generarHtmlTablaDocumentoTabSustento(concepcodi, idPrefijo, titulo, desc, editable) {

    var html = `
            <table class="content-tabla-search" style="width:100%">
                <tr>
                    <td colspan="2" class="tdPadd" style="background-color:SteelBlue;color:white;"><b>${titulo}</b></td>
                </tr>
                <tr>

                    <td style="width: auto;">${desc}</td>
                </tr>
                <tr>
                    <td>
                        <div style="clear:both; height:10px"></div>
    `;

    if (editable) {
        html += `        
                        <div style="width:180px">
                            <input type="button" id="btnSelectFile${idPrefijo}" value="Agregar Archivo" />
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

                        <div id="listaArchivos${idPrefijo}" class="bodyexcel">

                        </div>

                    </td>
                </tr>
            </table>
            <div style='clear:both; height:10px;width:100px;'></div>
    `;

    return html;
}

function cargarHandsontableArchivoXTabSustento(objTabCentral, idHandsontable) {

    var matriz = objTabCentral.Handson.ListaExcelData;
    var matrizTipoEstado = objTabCentral.Handson.MatrizTipoEstado;
    var filaIni = objTabCentral.FilaIni;
    var soloLectura = objTabCentral.Readonly;
    var esEditable = objTabCentral.EsEditable;
    var esEditableObs = objTabCentral.EsEditableObs;

    var colBtnEliminar = 0;
    var colNombreArchivo = colBtnEliminar + 1;
    var colConf = colNombreArchivo + 1;
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

    function rendererFilaPropiedadColNombre(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.color = '#45587D';
        td.style.textAlign = 'left';
        td.style.background = '#F2F2F2';
    }

    function rendererConfidencialCheck(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.CheckboxRenderer.apply(this, arguments);
        td.style.textAlign = 'center';
        td.style.background = '#F2F2F2';

        if (cellProperties.readOnly) {
            $(`input[class="htCheckboxRendererInput"][data-row="${row}"][data-col="${col}"]`).prop("disabled", true);
        }
    }
    function rendererConfidencialSinCheck(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.textAlign = 'center';
        td.style.background = '#F2F2F2';
    }

    function rendererFilaPropiedadColValorNoEditable(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.color = '#000000';
        td.style.textAlign = 'center';
        td.style.background = '#9BC2E6'; //'#EDEDED';
    }

    function rendererFilaPropiedadColValorHtml(instance, td, row, col, prop, value, cellProperties) {
        var valorRO = matrizTipoEstado[row][col] == -2;
        td.style.background = !valorRO ? '#FFFFFF' : '#FFE4C4';
        td.innerHTML = _getHtmlObservacionTabSustento(row, col, valorRO);
    }

    function rendererFilaCeldaReadonly(instance, td, row, col, prop, value, cellProperties) {
        td.style.background = !cellProperties.readOnly ? '#FFFFFF' : '#FFE4C4';
    }

    function rendererBtnEliminarYDescargar(instance, td, row, col, prop, value, cellProperties) {
        td.style.background = !cellProperties.readOnly ? '#FFFFFF' : '#FFE4C4';
        td.innerHTML = _getHtmlAccionArchivoTabSustento(row, cellProperties.readOnly);
    }

    function calculateSizeSustentatorio() {
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
        if (HOT_SUSTENTO != undefined) {
            HOT_SUSTENTO.render();
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
    if (ESTADO_ENVIO_FORMULARIO == ESTADO_SUBSANADO) { //para aprobación parcial
        listaOpcionEstado.push("No Subsanado");
        if (MODELO_LISTA_CENTRAL.length > 1) listaOpcionEstado.push("Conforme parcialmente");
        listaOpcionEstado.push("Conforme");
    }

    //definicion
    var container = document.getElementById(idHandsontable);
    //Handsontable.Dom.addEvent(window, 'resize', calculateSize());
    hotOptions = {
        data: matriz,
        mergeCells: objTabCentral.Handson.ListaMerge,
        maxCols: matriz[0].length,
        maxRows: 100,//maximo 100 archivos
        height: HEIGHT_MINIMO,
        colHeaders: false,
        rowHeaders: false,
        fillHandle: true,
        columnSorting: false,
        colWidths: objTabCentral.Handson.ListaColWidth,
        readOnly: soloLectura,
        cells: function (row, col, prop) {
            if (row >= 0 && col >= 0) {

                //celda no editables
                if (col < colObsCOES) {
                    if (esEditable)
                        this.readOnly = matrizTipoEstado[row][col] <= 0; //-1: no editable, 0: solo lectura, -2: readonly
                    else
                        this.readOnly = true;
                } else {
                    if (col >= colObsCOES && col <= colRptaCOES)
                        this.readOnly = true;
                    else
                        this.readOnly = matrizTipoEstado[row][col] <= 0; //-1: no editable, 0: solo lectura, -2: readonly
                }

                if (row <= filaIni) {
                    this.renderer = rendererFilaSeccion;
                }

                //renderizar filas de datos
                if (row > filaIni) {

                    if (col == colBtnEliminar)
                        this.renderer = rendererBtnEliminarYDescargar;

                    if (col == colNombreArchivo)
                        this.renderer = rendererFilaPropiedadColNombre;

                    if (col == colConf) {
                        if (columnaCheckConf[row]) {
                            this.renderer = rendererConfidencialCheck;
                            this.type = 'checkbox';
                        }
                        else
                            this.renderer = rendererConfidencialSinCheck;
                    }

                    if (col >= colObsCOES && col <= colEstado) {
                        if (matrizTipoEstado[row][col] == -1) //nunca editable
                            this.renderer = rendererFilaPropiedadColValorNoEditable;
                        else {
                            if (row == filaIni + 1) {
                                if (col == colObsCOES)
                                    this.renderer = rendererFilaPropiedadColValorHtml;

                                if (col == colSubsGen)
                                    this.renderer = rendererFilaPropiedadColValorHtml;

                                if (col == colRptaCOES)
                                    this.renderer = rendererFilaPropiedadColValorHtml;

                                if (col == colEstado) {
                                    this.type = 'dropdown';
                                    this.source = listaOpcionEstado;
                                    //if (matrizTipoEstado[row][col] == -2)
                                    //    this.renderer = rendererFilaCeldaReadonly;
                                }
                            }
                        }
                    }
                }
            }
        },
    };
    HOT_SUSTENTO = new Handsontable(container, hotOptions);
    calculateSizeSustentatorio(1);
}

function actualizarModeloSustentoMemoria() {
    //guardar valores de handson
    MODELO_DOCUMENTO.Handson.ListaExcelData = HOT_SUSTENTO.getData();

    //actualizar lista de archivo
    var colBtnEliminar = 0;
    var colNombreArchivo = colBtnEliminar + 1;
    var colConf = colNombreArchivo + 1;
    var colObsCOES = colConf + 1;
    var colSubsGen = colObsCOES + 1;
    var colRptaCOES = colSubsGen + 1;
    var colEstado = colRptaCOES + 1;

    var tieneObs = MODELO_DOCUMENTO.IncluirObservacion;
    var filaIni = tieneObs ? 2 : 1;

    for (var i = 0; i < MODELO_DOCUMENTO.SeccionCombustible.ListaArchivo.length; i++) {
        if (MODELO_DOCUMENTO.Handson.ListaExcelData.length > i + filaIni) {
            var check = MODELO_DOCUMENTO.Handson.ListaExcelData[i + filaIni][colConf];
            MODELO_DOCUMENTO.SeccionCombustible.ListaArchivo[i].Cbarchconfidencial = check ? "1" : "2";

        }
    }

    if (tieneObs) {
        if (MODELO_DOCUMENTO.Handson.ListaExcelData.length > filaIni) {
            var estado = MODELO_DOCUMENTO.Handson.ListaExcelData[0 + filaIni][colEstado];
            MODELO_DOCUMENTO.SeccionCombustible.ListaObs[0].Cbevdavalor = estado;
            MODELO_DOCUMENTO.SeccionCombustible.ListaObs[1].Cbevdavalor = estado;
            MODELO_DOCUMENTO.SeccionCombustible.ListaObs[2].Cbevdavalor = estado;
        }
    }

    MODELO_DOCUMENTO_JSON = _clonarModeloDocumento(MODELO_DOCUMENTO);
}

function _clonarModeloDocumento(modelo) {
    var colBtnEliminar = 0;
    var colNombreArchivo = colBtnEliminar + 1;
    var colConf = colNombreArchivo + 1;
    var colObsCOES = colConf + 1;
    var colSubsGen = colObsCOES + 1;
    var colRptaCOES = colSubsGen + 1;
    var colEstado = colRptaCOES + 1;

    //clonar objeto y quitar html
    var modeloClone = JSON.parse(JSON.stringify(modelo));

    var tieneObs = MODELO_DOCUMENTO.IncluirObservacion;
    var filaIni = tieneObs ? 2 : 1;

    if (tieneObs) {
        var matriz = modeloClone.Handson.ListaExcelData;

        if (matriz.length > 0) {
            for (var n = filaIni; n < matriz.length; n++) {
                for (var col = 0; col < matriz[n].length; col++)
                    if (col >= colObsCOES && col <= colRptaCOES) {
                        matriz[n][col] = "";
                    }
            }
        }
    }

    return modeloClone;
}

//Carga de archivos
function pUploadArchivoTabSustento(idEnvio, concepcodi, prefijo) {
    var uploaderP23 = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFile' + prefijo,
        container: document.getElementById('container'),
        chunks_size: '20mb',
        multipart_params: {
            "idConcepto": concepcodi,
        },
        url: siteRoot + 'Combustibles/GestionGas/UploadTemporal',
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
            },
            FileUploaded: function (up, file, result) {
                //console.log(result);
                agregarRowTabSustento(concepcodi, JSON.parse(result.response).nuevonombre, file.name);
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

function actualizarExcelWebTabSustento() {
    actualizarModeloSustentoMemoria();

    var formulario = {
        IdEnvio: $("#hfIdEnvio").val(),
        FormularioSustento: MODELO_DOCUMENTO_JSON,
        TipoAccionForm: $("#hdTipoAccion").val()
    };

    var dataJson = {
        data: JSON.stringify(formulario)
    };

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: controlador + "ActualizarGrillaArchivo",
        contentType: 'application/json; charset=UTF-8',
        data: JSON.stringify(dataJson),
        beforeSend: function () {
            //mostrarExito("Enviando Información ..");
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                MODELO_DOCUMENTO = evt.FormularioSustento;
                MODELO_DOCUMENTO_JSON = MODELO_DOCUMENTO;

                //tab archivos
                cargarHtmlDocumentoXEnvioTabSustento(MODELO_DOCUMENTO);
            } else {
                //alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error.");
        }
    });
}

function _getHtmlAccionArchivoTabSustento(row, soloLectura) {

    var regArchivo = getArchivoXFilaTabSustento(row);

    var styleFondo = soloLectura ? 'background-color: rgb(255, 228, 196)' : "";

    var htmlTd1 = '';
    var htmlTd2 = '';
    if (regArchivo != null) {

        if (MODELO_DOCUMENTO.EsEditable) htmlTd2 += `
                    <a title="Eliminar registro" href="JavaScript:eliminarRowTabSustento(${row});">${IMG_CANCELAR} </a>
        `;

        htmlTd1 += `
                    <a title="Descargar registro" href="JavaScript:descargarArchivoTabSustento(${row});">${IMG_DESCARGAR} </a>
        `;
    }

    var htmlDiv = `
        <table>
            <tr>
                <td style='padding-right: 0px !important; line-height: 0px;border: 0;${styleFondo}'>${htmlTd1}</td>
                <td style='padding-right: 0px !important; line-height: 0px;border: 0;${styleFondo}'>${htmlTd2}</td>
            </tr>
        </table>
    `;

    return htmlDiv;
}

function agregarRowTabSustento(concepcodi, nuevoNombre, nombreArchivo) {
    actualizarModeloSustentoMemoria();

    var seccion = MODELO_DOCUMENTO.SeccionCombustible;

    seccion.ListaArchivo.push({
        EsNuevo: true,
        Ccombcodi: concepcodi,
        Cbarchnombrefisico: nuevoNombre,
        Cbarchnombreenvio: nombreArchivo
    });

    actualizarExcelWebTabSustento();
}

function eliminarRowTabSustento(fila) {
    var tieneObs = MODELO_DOCUMENTO.IncluirObservacion;
    var filaIni = tieneObs ? 2 : 1;

    var posicion = fila - filaIni;
    var seccion = MODELO_DOCUMENTO.SeccionCombustible;
    var listaArchTmp = [];

    for (var i = 0; i < seccion.ListaArchivo.length; i++) {
        if (i != (posicion))
            listaArchTmp.push(seccion.ListaArchivo[i]);
    }

    seccion.ListaArchivo = listaArchTmp;

    actualizarExcelWebTabSustento();
}

function getArchivoXFilaTabSustento(row) {
    var tieneObs = MODELO_DOCUMENTO.IncluirObservacion;
    var filaIni = tieneObs ? 2 : 1;

    for (var i = 0; i < MODELO_DOCUMENTO.SeccionCombustible.ListaArchivo.length; i++) {
        if (row == i + filaIni) {
            return MODELO_DOCUMENTO.SeccionCombustible.ListaArchivo[i];
        }
    }

    return nul;
}

function descargarArchivoTabSustento(row) {

    var regArchivo = getArchivoXFilaTabSustento(row);
    if (regArchivo != null) {
        var esConf = (regArchivo.Cbarchconfidencial == 1) ? "S" : "N";
        window.location = controlador + `DescargarArchivoTemporal?idConcepto=${regArchivo.Ccombcodi}&fileName=${regArchivo.Cbarchnombrefisico}&esConf=${esConf}`;
    }
}

function loadValidacionFile(mensaje, prefijo) {
    //$('#fileInfo').innerHTML += mensaje;
    $('#fileInfo' + prefijo).removeClass("file-ok");
    $('#fileInfo' + prefijo).removeClass("file-alert");

    $('#fileInfo' + prefijo).html(mensaje);
    $('#fileInfo' + prefijo).addClass("file-alert");
}

function limpiarMensaje() {
    $('#fileInfo').removeClass("file-alert");

    $('#fileInfo').html("");
}
