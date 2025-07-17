//Observaciones html - Tab Formato 3
function mostrarPopupObservacion(posTab, fila, col) {

    popupFormularioObservacion(posTab, fila, col);
}

function popupFormularioObservacion(posTab, fila, col) {
    tinymce.remove();
    $('#btnGuardarObsHtml').unbind();
    $("#htmlArchivos").unbind();
    $("#htmlArchivos").html('');
    $("#idFormularioObservacion").html('');

    //
    var objTab = MODELO_LISTA_CENTRAL[posTab];
    var objItemObs = _getItemObsByPos(objTab.ArrayItemObs, fila, col);

    var htmlObs = objItemObs.Obs.Cbobshtml ?? "";
    var habilitacion = objTab.EsEditableObs ? "" : 'disabled';

    var htmlDiv = '';
    htmlDiv += `
        <table style="width:100%">
            <tr>
	            <td class="registro-control" style="width:790px;">
		            <textarea name="Contenido" id="contenido_html_obs" maxlength="2000" cols="180" rows="22"  ${habilitacion}>
			            ${htmlObs}
		            </textarea>
	            </td>
            </tr>
        </table>

        <input type='hidden' id='hfXObs_Tipo' value='F3' />

        <div id='html_archivos_x_obs'></div>
    `;

    if (objTab.EsEditableObs) {
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
        _guardarObsHtml(posTab, fila, col);
    });

    setTimeout(function () {
        $("#popupFormularioObservacion .popup-title").html(objItemObs.Ccombnombre);
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
                'insertfile undo redo | styleselect fontsizeselect | forecolor backcolor | bullist numlist outdent indent | link | table | image | mybutton | preview',

            menubar: false,
            readonly: 0,
            language: 'es',
            statusbar: false,
            convert_urls: false,
            plugin_preview_width: 790,
            setup: function (editor) {
                editor.on('change',
                    function () {
                        editor.save();
                    });
            }
        });

        //archivos
        cargarHtmlArchivoXObs('F3', objItemObs.Obs, posTab, fila, col, objTab.EsEditableObs);

    }, 50);

}

function _guardarObsHtml(posTab, fila, col) {
    //
    var htmlObs = $("#contenido_html_obs").val();

    //
    var objTabCentral = MODELO_LISTA_CENTRAL[posTab];
    var objItemObs = _getItemObsByPos(objTabCentral.ArrayItemObs, fila, col);
    objItemObs.Obs.Cbobshtml = htmlObs;

    //actualizar handson
    var arrayCambios = [];

    var arrayCeldaCambio = [];
    arrayCeldaCambio.push(fila); //row
    arrayCeldaCambio.push(col); //col
    arrayCeldaCambio.push(_getHtmlObservacion(posTab, fila, col, !objTabCentral.EsEditableObs)); //value

    arrayCambios.push(arrayCeldaCambio);

    LISTA_OBJETO_HOJA[objTabCentral.Equicodi].hot.setDataAtCell(arrayCambios);

    $('#popupFormularioObservacion').bPopup().close();
}

function _getItemObsEstadoByFila(listaItemObs, posRow) {
    if (listaItemObs != null) {
        for (var i = 0; i < listaItemObs.length; i++) {
            if (listaItemObs[i].EsColEstado && listaItemObs[i].PosRow == posRow)
                return listaItemObs[i];
        }
    }

    return null;
}

function _getItemObsByPos(listaItemObs, posRow, posCol) {
    if (listaItemObs != null) {
        for (var i = 0; i < listaItemObs.length; i++) {
            if (listaItemObs[i].PosCol == posCol && listaItemObs[i].PosRow == posRow)
                return listaItemObs[i];
        }
    }

    return null;
}

function _getHtmlObservacion(posTab, row, col, esEditable) {

    var objTabCentral = MODELO_LISTA_CENTRAL[posTab];
    var objItemObs = _getItemObsByPos(objTabCentral.ArrayItemObs, row, col);

    var styleFondo = !esEditable ? 'background-color: rgb(255, 228, 196)' : "";
    var htmlEditado = objItemObs != null ? (objItemObs.Obs.Cbobshtml ?? "") : "";
    var htmlBtn = "";
    if (esEditable) htmlBtn = `
                <td style='width: 25px;border: 0;'>
                    <a title="Editar registro" href="JavaScript:mostrarPopupObservacion(${posTab}, ${row}, ${col});">${IMG_EDITAR} </a>
                </td>
    `;
    var htmlArchivos = '';
    if (objItemObs.Obs.ListaArchivoXObs) htmlArchivos = generarTablaListaBodyXObs('F3', objItemObs.Obs.ListaArchivoXObs, objItemObs.Obs.Ccombcodi, "html_temporal", posTab, row, col, esEditable, false);

    var htmlDiv = '';
    htmlDiv += `
        <table>
            <tr>
                <td style='width: 275px;border: 0;${styleFondo}'>${htmlEditado}</td>
                ${htmlBtn}
            </tr>
            <tr>${htmlArchivos}</td>
        </table>
    `;

    return htmlDiv;
}

function _getCeldaEsEditableObservacionF3(posTab, row, col) {
    var objTabCentral = MODELO_LISTA_CENTRAL[posTab];
    var objItemObs = _getItemObsByPos(objTabCentral.ArrayItemObs, row, col);

    var matrizTipoEstado = objTabCentral.Handson.MatrizTipoEstado;

    if (objTabCentral.EsEditableObs) {
        return matrizTipoEstado[row][col] > 0;
    }

    return false;
}

//Observaciones html - Tab Informe Sustentatorio
function mostrarPopupObservacionTabSustento(fila, col) {

    popupFormularioObservacionTabSustento(fila, col);
}

function popupFormularioObservacionTabSustento(fila, col) {
    tinymce.remove();
    $('#btnGuardarObsHtml').unbind();
    $("#idFormularioObservacion").html('');

    //
    var objItemObs = MODELO_DOCUMENTO.SeccionCombustible.ListaObs[col - 3];

    var htmlObs = objItemObs.Cbobshtml ?? "";
    var habilitacion = MODELO_DOCUMENTO.EsEditableObs ? "" : 'disabled';

    var htmlDiv = '';
    htmlDiv += `
        <table style="width:100%">
            <tr>
	            <td class="registro-control" style="width:790px;">
		            <textarea name="Contenido" id="contenido_html_obs" maxlength="2000" cols="180" rows="22"  ${habilitacion}>
			            ${htmlObs}
		            </textarea>
	            </td>
            </tr>
        </table>

        <input type='hidden' id='hfXObs_Tipo' value='S' />

        <div id='html_archivos_x_obs'></div>
    `;

    if (MODELO_DOCUMENTO.EsEditableObs) {
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
        _guardarObsHtmlTabSustento(fila, col);
    });

    setTimeout(function () {
        $("#popupFormularioObservacion .popup-title").html(objItemObs.Ccombnombre);
        $('#popupFormularioObservacion').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });

        var idHtml = "#contenido_html_obs";
        tinymce.init({
            selector: idHtml,
            plugins: [
                //'paste textcolor colorpicker textpattern link table image imagetools preview'
                'wordcount advlist anchor autolink codesample colorpicker contextmenu fullscreen image imagetools lists link media noneditable preview  searchreplace table template textcolor visualblocks wordcount'
            ],
            toolbar1:
                //'insertfile undo redo | fontsizeselect bold italic underline strikethrough | forecolor backcolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link | table | image | mybutton | preview',
                'insertfile undo redo | styleselect fontsizeselect | forecolor backcolor | bullist numlist outdent indent | link | table | image | mybutton | preview',

            menubar: false,
            readonly: 0,
            language: 'es',
            statusbar: false,
            convert_urls: false,
            plugin_preview_width: 790,
            setup: function (editor) {
                editor.on('change',
                    function () {
                        editor.save();
                    });
            }
        });

        //archivos
        cargarHtmlArchivoXObs('S', objItemObs, 0, fila, col, MODELO_DOCUMENTO.EsEditableObs);

    }, 50);

}

function _guardarObsHtmlTabSustento(fila, col) {
    //
    var htmlObs = $("#contenido_html_obs").val();

    //
    var objItemObs = MODELO_DOCUMENTO.SeccionCombustible.ListaObs[col - 3];
    objItemObs.Cbobshtml = htmlObs;

    //actualizar handson
    var arrayCambios = [];

    var arrayCeldaCambio = [];
    arrayCeldaCambio.push(fila); //row
    arrayCeldaCambio.push(col); //col
    arrayCeldaCambio.push(_getHtmlObservacionTabSustento(fila, col, false)); //value

    arrayCambios.push(arrayCeldaCambio);

    HOT_SUSTENTO.setDataAtCell(arrayCambios);

    $('#popupFormularioObservacion').bPopup().close();
}

function _getHtmlObservacionTabSustento(row, col, soloLectura) {

    var objItemObs = MODELO_DOCUMENTO.SeccionCombustible.ListaObs[col - 3];

    var styleFondo = soloLectura ? 'background-color: rgb(255, 228, 196)' : "";
    var htmlEditado = objItemObs != null ? (objItemObs.Cbobshtml ?? "") : "";
    var htmlBtn = "";
    if (!soloLectura) htmlBtn = `
                <td style='width: 25px;border: 0;'>
                    <a title="Editar registro" href="JavaScript:mostrarPopupObservacionTabSustento(${row}, ${col});">${IMG_EDITAR} </a>
                </td>
    `;
    var htmlArchivos = '';
    if (objItemObs.ListaArchivoXObs) htmlArchivos = generarTablaListaBodyXObs('S', objItemObs.ListaArchivoXObs, objItemObs.Ccombcodi, "html_temporal", 0, row, col, !soloLectura, false);

    var htmlDiv = '';
    htmlDiv += `
        <table>
            <tr>
                <td style='width: 275px;border: 0;${styleFondo}'>${htmlEditado}</td>
                ${htmlBtn}
            </tr>
            <tr>${htmlArchivos}</td>
        </table>
    `;

    return htmlDiv;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Archivos adjuntos de observaciones
////////////////////////////////////////////////////////////////////////////////////////////////////////////

function cargarHtmlArchivoXObs(tipo, objObs, posTab, fila, col, tienePermisoEditar) {
    var prefijo = "_sec_doc_" + objObs.Ccombcodi;
    var htmlSec = "<div id=div_" + prefijo + ">";
    htmlSec += generarHtmlTablaDocumentoXObs(tipo, objObs.ListaArchivoXObs, objObs.Ccombcodi, prefijo, posTab, fila, col, tienePermisoEditar);
    htmlSec += "</div>";

    $("#html_archivos_x_obs").html(htmlSec);

    //plugin archivo
    pUploadArchivoXObs(tipo, objObs.Ccombcodi, "_sec_doc_" + objObs.Ccombcodi, posTab, fila, col);

}

function generarHtmlTablaDocumentoXObs(tipo, listaArchivo, concepcodi, idPrefijo, posTab, fila, col, tienePermisoEditar) {

    var html = `
            <table class="content-tabla-search" style="width:100%">
                <tr>
                    <td>
                        <div style="clear:both; height:10px"></div>
    `;

    if (tienePermisoEditar) {
        html += `        
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

    html += generarTablaListaBodyXObs(tipo, listaArchivo, concepcodi, idPrefijo, posTab, fila, col, tienePermisoEditar, tienePermisoEditar);

    html += `
                        </div>

                    </td>
                </tr>
            </table>
                <div style='clear:both; height:10px;width:100px;'></div>
    `;

    return html;
}

function generarTablaListaBodyXObs(tipo, listaArchivo, concepcodi, idPrefijo, posTab, fila, col, tienePermisoEditar, incluirEliminar) {

    var styleFondo = "";
    if (!incluirEliminar) {
        styleFondo = ';border: 0; height: 0px; line-height: 0px;white-space: nowrap;';
        styleFondo += (!tienePermisoEditar ? ";background-color: rgb(255, 228, 196)" : "");
    }
    var html = `
                <table border="0" class="pretty tabla-icono" cellspacing="0" style="width:auto" id="tabla${idPrefijo}">
                    <tbody>`;

    for (var i = 0; i < listaArchivo.length; i++) {
        var item = listaArchivo[i];
        var idrow = "row" + item.Cbobsaorden;
        var nomb = item.Cbobsanombreenvio;

        html += `
                        <tr id="${idrow}">

                            <td onclick="verArchivoXObs('${tipo}', ${concepcodi}, ${i}, ${posTab}, ${fila}, ${col});" title='Visualizar archivo' style="cursor: pointer;width:30px;${styleFondo}">
                                	&#128065;
                            </td>
                            <td onclick="descargarArchivoXObs('${tipo}', ${concepcodi}, ${i}, ${posTab}, ${fila}, ${col});" title='Descargar archivo' style="cursor: pointer;width:30px;${styleFondo}">
                                <img width="15" height="15" src="../../Content/Images/btn-download.png" />
                            </td>
                            <td style="text-align:left;${styleFondo}" title='${nomb}'>
                                ${nomb}
                            </td>
        `;
        if (incluirEliminar) {
            html += `     
                            <td onclick="eliminarRowXObs('${tipo}', ${concepcodi}, ${i}, ${posTab}, ${fila}, ${col})" title='Eliminar archivo' style="width:30px;cursor:pointer;${styleFondo}">
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

function pUploadArchivoXObs(tipo, concepcodi, prefijo, posTab, fila, col) {
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
                agregarRowXObs(tipo, concepcodi, posTab, fila, col, JSON.parse(result.response).nuevonombre, file.name);
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

function agregarRowXObs(tipo, concepcodi, posTab, fila, col, nuevoNombre, nombreArchivo) {
    var regObs = null;
    var esEditable = false;
    //
    if (tipo == "F3") {
        var objTab = MODELO_LISTA_CENTRAL[posTab];
        regObs = _getItemObsByPos(objTab.ArrayItemObs, fila, col).Obs;
        esEditable = objTab.EsEditableObs;
    } else {
        regObs = MODELO_DOCUMENTO.SeccionCombustible.ListaObs[col - 3];
        esEditable = MODELO_DOCUMENTO.EsEditableObs;
    }

    if (regObs != null) {
        regObs.ListaArchivoXObs.push({ EsNuevo: true, Ccombcodi: concepcodi, Cbobsanombrefisico: nuevoNombre, Cbobsanombreenvio: nombreArchivo });

        var idPrefijo = "_sec_doc_" + regObs.Ccombcodi;
        $("#listaArchivos" + idPrefijo).html(generarTablaListaBodyXObs(tipo, regObs.ListaArchivoXObs, regObs.Ccombcodi, idPrefijo, posTab, fila, col, esEditable, true));
    }
}

function eliminarRowXObs(tipo, concepcodi, pos, posTab, fila, col) {

    var regObs = null;
    //
    if (tipo == "F3") {
        var objTab = MODELO_LISTA_CENTRAL[posTab];
        regObs = _getItemObsByPos(objTab.ArrayItemObs, fila, col);
    }

    if (regObs != null) {
        var listaArchTmp = [];
        for (var i = 0; i < regObs.Obs.ListaArchivoXObs.length; i++) {
            if (i != pos)
                listaArchTmp.push(regObs.Obs.ListaArchivoXObs[i]);
        }

        regObs.Obs.ListaArchivoXObs = listaArchTmp;

        var idPrefijo = "_sec_doc_" + regObs.Obs.Ccombcodi;
        $("#listaArchivos" + idPrefijo).html(generarTablaListaBodyXObs(tipo, regObs.Obs.ListaArchivoXObs, regObs.Obs.Ccombcodi, idPrefijo, pos, posTab, fila, objTab.EsEditableObs, true));
    }
}

function descargarArchivoXObs(tipo, concepcodi, pos, posTab, fila, col) {
    var regArchivo = null;

    //
    if (tipo == "F3") {
        var objTab = MODELO_LISTA_CENTRAL[posTab];
        var objItemObs = _getItemObsByPos(objTab.ArrayItemObs, fila, col);
        regArchivo = objItemObs.Obs.ListaArchivoXObs[pos];
    } else {
        regObs = MODELO_DOCUMENTO.SeccionCombustible.ListaObs[col - 3];
        regArchivo = regObs.ListaArchivoXObs[pos];
    }

    if (regArchivo != null) {
        window.location = controlador + `DescargarArchivoTemporal?idConcepto=${regArchivo.Ccombcodi}&fileName=${regArchivo.Cbobsanombrefisico}`;
    }
}

//Vista previa
function verArchivoXObs(tipo, concepcodi, pos, posTab, fila, col) {
    var regArchivo = null;

    //
    if (tipo == "F3") {
        var objTab = MODELO_LISTA_CENTRAL[posTab];
        var objItemObs = _getItemObsByPos(objTab.ArrayItemObs, fila, col);
        regArchivo = objItemObs.Obs.ListaArchivoXObs[pos];
    } else {
        regObs = MODELO_DOCUMENTO.SeccionCombustible.ListaObs[col - 3];
        regArchivo = regObs.ListaArchivoXObs[pos];
    }

    if (regArchivo != null) {
        var nombreArchivo = regArchivo.Cbobsanombrefisico.toLowerCase();
        var esPdf = nombreArchivo.endsWith(".pdf");
        var esVistaPrevia = nombreArchivo.endsWith(".pdf") || nombreArchivo.endsWith(".xlsx") || nombreArchivo.endsWith(".docx") || nombreArchivo.endsWith(".xls") || nombreArchivo.endsWith(".doc");

        if (esVistaPrevia) {
            $.ajax({
                type: 'POST',
                url: controlador + 'VistaPreviaArchivo',
                data: {
                    idConcepto: regArchivo.Ccombcodi,
                    fileName: nombreArchivo
                },
                success: function (model) {
                    if (model.Resultado != "") {

                        var rutaCompleta = window.location.href;
                        var ruraInicial = rutaCompleta.split("Combustibles");
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
