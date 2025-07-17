var HOT_SUSTENTO = null;
var TIPO_ARCHIVO_VALOR_DATO = "A_VD";
var TIPO_ARCHIVO_SUSTENTO_DATO = "A_SD";

var IMG_CANCELAR = '<img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="" width="19" height="19" style="">';
var IMG_DESCARGAR = '<img src="' + siteRoot + 'Content/Images/file.png" alt="" width="19" height="19" style="">';
var IMG_AGREGAR_ARCHIVO = '<img src="' + siteRoot + 'Content/Images/btn-add.png" alt="Agregar archivo" title="Agregar archivo" width="19" height="19" style="">';
var IMG_ARCHIVO_CONFIDENCIAL = '<img src="' + siteRoot + 'Content/Images/DownFileConfidencial.png" alt="Confidencial" title="Confidencial" width="19" height="19" style="">';

////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Archivos de Sustento
////////////////////////////////////////////////////////////////////////////////////////////////////////////
function cargarHtmlDocumentoXEnvio(seccion, tipoArchivo, esEditable) {
    var html = '';

    var idPrefijo = getIdPrefijoPuploadFT(seccion, tipoArchivo);
    var listaArchivo = getListaArchivoXTipo(seccion, tipoArchivo);
    var htmlSeccion = generarHtmlTablaDocumento(idPrefijo, listaArchivo, seccion, tipoArchivo, esEditable);;

    var htmlSec = `
            <div id='div_${idPrefijo}'' >
                ${htmlSeccion}
            </div>
        `;

    html += htmlSec;

    return html;
}

function generarHtmlTablaDocumento(idPrefijo, listaArchivo, seccion, tipoArchivo, esEditable) {

    var htmlBtnAgregar = '';
    var htmlTablaArchivo = '';

    if (esEditable) {
        htmlBtnAgregar = `
            <div style="">
                <a type="button" id="btnSelectFile${idPrefijo}">
                    ${IMG_AGREGAR_ARCHIVO}
                </a>
            </div>
        `;
    }

    htmlTablaArchivo = generarTablaListaBody(listaArchivo, seccion, idPrefijo, tipoArchivo, esEditable);

    var html = `
                ${htmlBtnAgregar}

                <div id="fileInfo${idPrefijo}"></div>
                <div id="progreso${idPrefijo}"></div>
                <div id="fileInfo${idPrefijo}" class="file-upload plupload_container ui-widget-content " style="display:none"></div>

                <input type="hidden" id="hfile${idPrefijo}" name="file${idPrefijo}" value=" " />
                        
                <div id="listaArchivos${idPrefijo}" class="content-tabla">

                ${htmlTablaArchivo}

                </div>
    `;

    return html;
}

function generarTablaListaBody(listaArchivo, seccion, idPrefijo, tipoArchivo, esEditable) {
    esEditable = false;
    var codigoCarpeta = seccion.Ftitcodi;
    if (seccion.EsReplica) {
        var seccionFuente = obtenerSeccionXcnp(MODELO_FICHA.ListaAllItems, seccion.FtitcodiFuente);
        listaArchivo = getListaArchivoXTipo(seccionFuente, tipoArchivo);

        codigoCarpeta = seccion.FtitcodiFuente;
    }
    var htmlVisible = listaArchivo.length > 0 ? "" : `;display:none;`;
    var flagConf = tipoArchivo == TIPO_ARCHIVO_VALOR_DATO ? seccion.HabilitarCheckValorConfidencial : seccion.HabilitarCheckSustentoConfidencial;
    var tdConfVisible = flagConf ? "" : `;display:none;`;


    var html = `
                <table border="0" class="pretty tabla-icono tabla_archivo" cellspacing="0" style="width:70px; ; ${htmlVisible}" id="tabla${idPrefijo}">
                    <thead>
                        <tr>
                            <th style='width: 40px'>Acción</th>
                            <th style='width: 30px; ${tdConfVisible} '>${IMG_ARCHIVO_CONFIDENCIAL}</th>
                        </tr>
                    </thead>

                    <tbody>`;

    for (var i = 0; i < listaArchivo.length; i++) {
        var item = listaArchivo[i];
        var idrow = "row" + i;
        var nomb = item.Ftearcnombreoriginal;
        var checked = item.Ftearcflagsustentoconf == "S" ? "checked" : "";
        var disabled = 'disabled';

        var permisoConfidencial = false;

        var tdEliminar = '';
        if (esEditable) tdEliminar = `
                                <a href="#" onclick="eliminarRow(${codigoCarpeta}, '${tipoArchivo}', ${i})" style="width:30px;cursor:pointer;" title='Quitar archivo "${nomb}" '>
                                    ${IMG_CANCELAR}
                                </a>
        `;



        //Si es Confidencial
        if (checked == "checked") {

            html += `
                        <tr id="${idrow}">

                            <td style="padding-left: 0px; padding-right: 0px; background:${COLOR_BLOQUEADO} ">
                                <a href="#" onclick="descargarArchivo(${codigoCarpeta}, '${tipoArchivo}', ${i}, ${true});" style="cursor:pointer;" title='Descargar archivo "${nomb}" '>
                                    ${IMG_DESCARGAR}
                                </a>
                                ${tdEliminar}
                            </td>

                            <td style="background:${COLOR_BLOQUEADO}; text-align:center; ${tdConfVisible} " title='Confidencialidad o no del archivo "${nomb}" '>
                                <input type="checkbox" ${checked} ${disabled}>
                            </td>
            `;

        } else {
            html += `
                        <tr id="${idrow}">

                            <td style="padding-left: 0px; padding-right: 0px; background:${COLOR_BLOQUEADO} ">
                                <a href="#" onclick="descargarArchivo(${codigoCarpeta}, '${tipoArchivo}', ${i}, ${false});" style="cursor:pointer;" title='Descargar archivo "${nomb}" '>
                                    ${IMG_DESCARGAR}
                                </a>
                                ${tdEliminar}
                            </td>

                            <td style="background:${COLOR_BLOQUEADO}; text-align:center; ${tdConfVisible} " title='Confidencialidad o no del archivo "${nomb}" '>
                                <input type="checkbox" ${checked} ${disabled}>
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

function descargarArchivo(idElemento, tipoArchivo, pos, esArchivoConfidencial) {
    var regArchivo = obtenerArchivo(MODELO_FICHA.ListaAllItems, idElemento, tipoArchivo, pos);

    if (regArchivo != null) {
        window.location = controlador + 'DescargarArchivoEnvio?fileName=' + regArchivo.Ftearcnombrefisico + "&fileNameOriginal=" + regArchivo.Ftearcnombreoriginal
            + '&idElemento=' + idElemento + '&tipoArchivo=' + tipoArchivo + '&idEnvio=' + getObjetoFiltro().codigoEnvio + '&idVersion=' + getObjetoFiltro().codigoVersion
            + '&esArchivoConf=' + esArchivoConfidencial;
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
        if (seccion.Ftitcodi == concepto) {
            return seccion;
        }
    }

    return null;
}

function obtenerArchivo(listaSeccionDocumento, concepto, tipoArchivo, pos) {

    var seccion = obtenerSeccionXcnp(listaSeccionDocumento, concepto);
    var listaArchivo = getListaArchivoXTipo(seccion, tipoArchivo);

    for (var i = 0; i < listaArchivo.length; i++) {
        if (i == pos)
            return listaArchivo[i];
    }

    return null;
}

function getIdPrefijoPuploadFT(reg, tipoArchivo) {
    var idPrefijo = "_sec_doc_" + tipoArchivo + "_" + reg.Ftitcodi;

    return idPrefijo;
}

function getListaArchivoXTipo(reg, tipoArchivo) {
    var lista = TIPO_ARCHIVO_VALOR_DATO == tipoArchivo ? reg.ListaArchivoValor : reg.ListaArchivoAdjunto;
    lista = lista != null ? lista : [];
    return lista;
}