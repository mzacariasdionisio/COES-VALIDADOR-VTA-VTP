var TIPO_ARCHIVO_REQUISITO = "A_R";

var IMG_CANCELAR = '<img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="" width="19" height="19" style="">';
var IMG_DESCARGAR = '<img src="' + siteRoot + 'Content/Images/btn-download.png" alt="" width="19" height="19" style="">';

////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Archivos de Sustento
////////////////////////////////////////////////////////////////////////////////////////////////////////////
function cargarHtmlDocumentoXEnvio(listaSeccionDocumento, tipoArchivo) {
    var html = '';
    html += '<table id="tabla_OP" class=" miTablaR"  >'; //width: max-content va en el js
    html += '<tr id="1raFila" > <td> </td></tr>';
    html += '<tr id="2daFila" > <td> </td></tr>';
    for (var i = 0; i < listaSeccionDocumento.length; i++) {
        var seccion = listaSeccionDocumento[i];

        var idPrefijo = getIdPrefijo(seccion);
        var htmlSeccion1 = generarHtmlTablaDocumentoCab(seccion);
        var htmlSeccion2 = generarHtmlTablaDocumentoCuerpo(idPrefijo, seccion.ListaArchivo, seccion, tipoArchivo);
        var estiloCambio = seccion.CambioValor == 1 ? "background: #FCB700;" : "";

        var htmlSec = `
            <tr id='divCab${idPrefijo}' >
                ${htmlSeccion1}
            </tr>
            <tr id='divCuerpo${idPrefijo}' style='${estiloCambio}'>
                ${htmlSeccion2}
            </tr>
        `;

        html += htmlSec;
    }
    html += '</table >';
    $("#html_archivos").html(html);
    $("#html_archivos").css("width", WIDTH_FORMULARIO + "px");
    $("#html_archivos").css("height", HEIGHT_FORMULARIO + "px");

    for (var i = 0; i < listaSeccionDocumento.length; i++) {
        var seccion = listaSeccionDocumento[i];

        var idEnvio = 0;

        var idPrefijo = getIdPrefijo(seccion);
        if (seccion.EsObligatorioArchivo) {
            pUploadArchivo(seccion.Fevrqcodi, tipoArchivo, idPrefijo);
        }
    }
}


function generarHtmlTablaDocumentoCab(seccion) {

    var html = `
       <td colspan="1" class="tdPadd" style="width: 550px;background-color:SteelBlue;color:white;padding-top: 4px; padding-bottom: 4px;">
           <b>${seccion.Fevrqliteral} ${seccion.Fevrqdesc}</b>
       </td>
    `;

    return html;
}

function generarHtmlTablaDocumentoCuerpo(idPrefijo, listaArchivo, seccion, tipoArchivo) {
    var esEditable = seccion.EsFilaEditableExtranet;

    var htmlBtnAgregar = '';
    var htmlTablaArchivo = '';

    if (seccion.EsObligatorioArchivo) {
        if (esEditable) htmlBtnAgregar = `
            <div style="clear:both; height:10px"></div>

            <div style="width:180px">
                <input type="button" id="btnSelectFile${idPrefijo}" value="Agregar Archivo" />
                <div style="font-size: 10px;padding-left: 15px;color: #5F0202;">Tamaño máximo por archivo: 50MB</div>
            </div>
                        
            <div style="clear:both; height:5px"></div>
        `;

        htmlTablaArchivo = generarTablaListaBody(listaArchivo, seccion, idPrefijo, tipoArchivo, esEditable);
    } else {
        htmlTablaArchivo = `
            <div style="padding-bottom: 10px;padding-left: 5px;">
                <input type="checkbox" class="checkbox-no-aplica">
                <span>No aplica.</span>
            </div>
        `;
    }

    var html = `
            <td style="border: 1px solid #dddddd;">

                        ${htmlBtnAgregar}

                        <div id="fileInfo${idPrefijo}"></div>
                        <div id="progreso${idPrefijo}"></div>
                        <div id="fileInfo${idPrefijo}" class="file-upload plupload_container ui-widget-content " style="display:none"></div>

                        <input type="hidden" id="hfile${idPrefijo}" name="file${idPrefijo}" value=" " />
                        
                        <div id="listaArchivos${idPrefijo}" class="content-tabla" style="width: 75px;">

                        ${htmlTablaArchivo}

                        </div>

                    </td>
    `;

    return html;
}

function generarTablaListaBody(listaArchivo, seccion, idPrefijo, tipoArchivo, esEditable) {

    var htmlVisible = listaArchivo.length > 0 ? "" : `;display:none;`;

    var html = `
                <table border="0" class="pretty tabla-icono tabla_archivo" cellspacing="0" style="width:500px; margin-left: 5px;margin-right: 5px; margin-bottom: 10px; ${htmlVisible}" id="tabla${idPrefijo}">
                    <thead>
                        <tr>
                            <th style='width: 50px'>Acción</th>
                            <th style='width: 400px'>Nombre</th>
                            <th style='width: 50px'>Confidencial</th>
                        </tr>
                    </thead>

                    <tbody>`;

    for (var i = 0; i < listaArchivo.length; i++) {
        var item = listaArchivo[i];
        var idrow = "row" + item.Ftearcnombrefisico.replace(".", "");
        var nomb = item.Ftearcnombreoriginal;
        var checked = item.Ftearcflagsustentoconf == "S" ? "checked" : "";
        var disabled = esEditable ? '' : 'disabled';

        var tdEliminar = '';
        if (esEditable) tdEliminar = `
                                <a class="btnERFila_${seccion.Fevrqcodi}" href="#" onclick="eliminarRow(${seccion.Fevrqcodi}, '${tipoArchivo}', '${item.Ftearcnombrefisico}')" style="width:30px;cursor:pointer;">
                                    ${IMG_CANCELAR}
                                </a>
        `;

        html += `
                        <tr id="${idrow}">

                            <td style="width:30px;">
                                <a href="#" onclick="descargarArchivo(${seccion.Fevrqcodi},'${tipoArchivo}', '${item.Ftearcnombrefisico}');" style="cursor:pointer;">
                                    ${IMG_DESCARGAR}
                                </a>
                                ${tdEliminar}
                            </td>

                            <td style="text-align:left;">
                              ${nomb}
                            </td>

                            <td style="text-align:center;">
                                <input class="ckbFila_${seccion.Fevrqcodi}" type="checkbox" ${checked} ${disabled}>
                            </td>
        `;

        html += "       </tr>";
    }

    html += `
                    </tbody>
                </table>`;

    return html;
}

//
var ARRAY_PUPLOAD = [];

function pUploadArchivo(idElemento, tipoArchivo, prefijo) {

    var uploaderP23 = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFile' + prefijo,
        container: document.getElementById('container'),
        chunks_size: '20mb',
        multipart_params: {
            idEnvio: getObjetoFiltro().codigoEnvio,
            idVersion: getObjetoFiltro().codigoVersion,
            idElemento: idElemento,
            tipoArchivo: tipoArchivo
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

                //deshabilitar botones
                $("input[id^=btnSelectFile_]").prop('disabled', true);
                $("input[id^=btnSelectFile_]").css('pointer-events', 'none');
            },
            UploadProgress: function (up, file) {
                $('#progreso' + prefijo).html(file.percent + "%");
            },

            UploadComplete: function (up, file) {
                $('#progreso' + prefijo).html("");
            },
            FileUploaded: function (up, file, result) {
                //habilitar botones
                $("input[id^=btnSelectFile_]").prop('disabled', false);
                $("input[id^=btnSelectFile_]").css('pointer-events', 'auto');

                $('#progreso' + prefijo).html("");
                agregarRow(idElemento, tipoArchivo, JSON.parse(result.response).nuevonombre, file.name);
            },
            Error: function (up, err) {
                //habilitar botones
                $("input[id^=btnSelectFile_]").prop('disabled', false);
                $("input[id^=btnSelectFile_]").css('pointer-events', 'auto');

                loadValidacionFile(err.code + "-" + err.message);
                if (err.code = -600) //error de tamaño
                    err.message = "Error: El archivo adjuntado supera el tamaño límite (50MB) por archivo. ";
                alert(err.message);
            }
        }
    });
    uploaderP23.init();

    ARRAY_PUPLOAD.push(uploaderP23);
}

function agregarRow(concepcodi, tipoArchivo, nuevoNombre, nombreArchivo) {
    actualizarListaConCheckConfidencial();

    var seccion = obtenerSeccionXcnp(LISTA_REQUISITO_ARCHIVO, concepcodi);
    var codigoTipoArchivo = tipoArchivo == TIPO_ARCHIVO_REQUISITO ? 3 : 4;
    seccion.ListaArchivo.push({
        EsNuevo: true, Fevrqcodi: concepcodi, Ftearcnombrefisico: nuevoNombre,
        Ftearcnombreoriginal: nombreArchivo, Ftearcorden: seccion.ListaArchivo.length + 1,
        Ftearcflagsustentoconf: 'N', Ftearctipo: codigoTipoArchivo
    });

    var idPrefijo = getIdPrefijo(seccion);
    $("#listaArchivos" + idPrefijo).html(generarTablaListaBody(seccion.ListaArchivo, seccion, idPrefijo, tipoArchivo, OPCION_GLOBAL_EDITAR));
}

function eliminarRow(concepcodi, tipoArchivo, nombreArchivo) {
    var seccion = obtenerSeccionXcnp(LISTA_REQUISITO_ARCHIVO, concepcodi);
    var listaArchTmp = [];

    for (var i = 0; i < seccion.ListaArchivo.length; i++) {
        if (seccion.ListaArchivo[i].Ftearcnombrefisico != nombreArchivo)
            listaArchTmp.push(seccion.ListaArchivo[i]);
    }

    seccion.ListaArchivo = listaArchTmp;

    var idPrefijo = getIdPrefijo(seccion);
    $("#listaArchivos" + idPrefijo).html(generarTablaListaBody(seccion.ListaArchivo, seccion, idPrefijo, tipoArchivo, OPCION_GLOBAL_EDITAR));
}

function descargarArchivo(idElemento, tipoArchivo, nombreArchivo) {
    var regArchivo = obtenerArchivo(LISTA_REQUISITO_ARCHIVO, idElemento, nombreArchivo);

    if (regArchivo != null) {
        window.location = controlador + 'DescargarArchivoEnvio?fileName=' + regArchivo.Ftearcnombrefisico + "&fileNameOriginal=" + regArchivo.Ftearcnombreoriginal
            + '&idElemento=' + idElemento + '&tipoArchivo=' + tipoArchivo + '&idEnvio=' + getObjetoFiltro().codigoEnvio + '&idVersion=' + getObjetoFiltro().codigoVersion;
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

function obtenerSeccionXcnp(listaSeccion, concepto) {
    for (var i = 0; i < listaSeccion.length; i++) {
        var seccion = listaSeccion[i];
        if (seccion.Fevrqcodi == concepto)
            return seccion;
    }

    return null;
}

function obtenerArchivo(listaSeccionDocumento, concepto, nombreArchivo) {

    var seccion = obtenerSeccionXcnp(listaSeccionDocumento, concepto);

    for (var i = 0; i < seccion.ListaArchivo.length; i++) {
        if (seccion.ListaArchivo[i].Ftearcnombrefisico == nombreArchivo)
            return seccion.ListaArchivo[i];
    }

    return null;
}

function actualizarListaConCheckConfidencial() {
    for (var i = 0; i < LISTA_REQUISITO_ARCHIVO.length; i++) {
        var seccion = LISTA_REQUISITO_ARCHIVO[i];

        var idPrefijo = getIdPrefijo(seccion);
        var idTabla = `tabla${idPrefijo}`;

        for (var j = 0; j < seccion.ListaArchivo.length; j++) {
            var arch = seccion.ListaArchivo[j];
            var idTr = "row" + (arch.Ftearcnombrefisico.replace(".", ""));
            var tieneCheck = $(`#${idTabla} tr[id=${idTr}] input[type=checkbox]`).is(':checked');
            arch.Ftearcflagsustentoconf = tieneCheck ? "S" : "N";
        }
    }
}

function getIdPrefijo(seccion) {
    var revisable = "N";

    if (seccion.EsObligatorioArchivo) {
        revisable = "S";
    }

    var idPrefijoFila = "_sec_doc_" + seccion.Fevrqcodi + "_" + revisable;

    return idPrefijoFila;
}
