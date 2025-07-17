var TIPO_ARCHIVO_REQUISITO = "A_R";
var TIPO_ARCHIVO_MODO = "A_M";

var IMG_CANCELAR = '<img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="" width="19" height="19" style="">';
var IMG_DESCARGAR = '<img src="' + siteRoot + 'Content/Images/btn-download.png" alt="" width="19" height="19" style="">';


////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Archivos de Sustento
////////////////////////////////////////////////////////////////////////////////////////////////////////////
function cargarHtmlDocumentoXEnvio(listaSeccionDocumento, tipoArchivo, esEditable, esDarBaja) {
    var html = '';

    html += '<table id="tabla_OP" class="miTablaR" style="width: max-content;" >';
    html += '<tr id="1raFila"> <td> </td> </tr>';
    html += '<tr id="2daFila"> <td> </td> </tr>';
    for (var i = 0; i < listaSeccionDocumento.length; i++) {
        var seccion = listaSeccionDocumento[i];
        var revisable = "N";

        if (seccion.EsObligatorioArchivo) {
            revisable = "S";
        }

        var idPrefijo = "";
        var idPrefijoFila = "";
        if (esDarBaja) {
            idPrefijo = "_sec_doc_" + seccion.Grupocodi;
            idPrefijoFila = "_sec_doc_" + seccion.Grupocodi + "_" + revisable;
        }
        else {
            idPrefijo = "_sec_doc_" + seccion.Fevrqcodi;
            idPrefijoFila = "_sec_doc_" + seccion.Fevrqcodi + "_" + revisable;
        }

        var htmlSeccion1 = generarHtmlTablaDocumentoCab(idPrefijo, seccion.ListaArchivo, seccion, esEditable);
        var htmlSeccion2 = generarHtmlTablaDocumentoCuerpo(idPrefijo, seccion.ListaArchivo, seccion, tipoArchivo, esEditable, esDarBaja);

        var htmlSec = `
            <tr id='divCab${idPrefijoFila}' >
                ${htmlSeccion1}
            </tr>
            <tr id='divCuerpo${idPrefijoFila}' >
                ${htmlSeccion2}
            </tr>
        `;


        html += htmlSec;
    }
    html += '</table >';
    $("#html_archivos").html(html);
    $("#html_archivos").css("width", WIDTH_FORMULARIO + "px");
    $("#html_archivos").css("height", HEIGHT_FORMULARIO + "px");

}


function generarHtmlTablaDocumentoCab(idPrefijo, listaArchivo, seccion, esEditable) {

    var titulo = seccion.Fevrqliteral != null ? (seccion.Fevrqliteral.trim() != "" ? (seccion.Fevrqliteral + " - " + seccion.Fevrqdesc) : seccion.Fevrqdesc) : seccion.Fevrqdesc;
    var html = `
       <td colspan="1" class="tdPadd" style="width: 550px;background-color:SteelBlue;color:white;padding-top: 4px; padding-bottom: 4px;">
           <b>${titulo}</b>
       </td>
    `;

    return html;
}

function generarHtmlTablaDocumentoCuerpo(idPrefijo, listaArchivo, seccion, tipoArchivo, esEditable, esDarBaja) {

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

        htmlTablaArchivo = generarTablaListaBody(listaArchivo, seccion, idPrefijo, tipoArchivo, esEditable, esDarBaja);
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

function generarTablaListaBody(listaArchivo, seccion, idPrefijo, tipoArchivo, esEditable, esDarBaja) {

    var htmlVisible = listaArchivo.length > 0 ? "" : `;display:none;`;

    var html = `
                <table border="0" class="pretty tabla-icono tabla_archivo" cellspacing="0" style="width:500px; margin-left: 5px; margin-right: 5px; margin-bottom: 10px; ${htmlVisible}" id="tabla${idPrefijo}">
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
        var idrow = "row" + item.Ftearcorden;
        var nomb = item.Ftearcnombreoriginal;
        var checked = item.Ftearcflagsustentoconf == "S" ? "checked" : "";
        var disabled = esEditable ? '' : 'disabled';

        var tdEliminar = '';
        if (esEditable) tdEliminar = `
                                <a href="#" onclick="eliminarRow(${seccion.Fevrqcodi},'${tipoArchivo}',${i}, ${esDarBaja})" style="width:30px;cursor:pointer;">
                                    ${IMG_CANCELAR}
                                </a>
        `;

        //Si es Confidencial
        if (checked == "checked") {

            html += `
                        <tr id="${idrow}">

                            <td style="width:30px;">
                                <a href="#" onclick="descargarArchivo(${seccion.Fevrqcodi},'${tipoArchivo}',${i}, ${true});" style="cursor:pointer;">
                                    ${IMG_DESCARGAR}
                                </a>
                                ${tdEliminar}
                            </td>

                            <td style="text-align:left;">
                              ${nomb}
                            </td>

                            <td style="text-align:center;">
                                <input type="checkbox" ${checked} ${disabled}>
                            </td>
            `;

        } else {
            html += `
                        <tr id="${idrow}">

                            <td style="width:30px;">
                                <a href="#" onclick="descargarArchivo(${seccion.Fevrqcodi},'${tipoArchivo}',${i}, ${false});" style="cursor:pointer;">
                                    ${IMG_DESCARGAR}
                                </a>
                                ${tdEliminar}
                            </td>

                            <td style="text-align:left;">
                              ${nomb}
                            </td>

                            <td style="text-align:center;">
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

function descargarArchivo(concepcodi, tipoArchivo, pos, esArchivoConfidencial) {
    var regArchivo = obtenerArchivo(LISTA_REQUISITO_ARCHIVO, concepcodi, pos);

    if (regArchivo != null) {
        window.location = controlador + 'DescargarArchivoEnvio?fileName=' + regArchivo.Ftearcnombrefisico + "&fileNameOriginal=" + regArchivo.Ftearcnombreoriginal
            + '&idElemento=' + concepcodi + '&tipoArchivo=' + tipoArchivo + '&idEnvio=' + getObjetoFiltro().codigoEnvio + '&idVersion=' + getObjetoFiltro().codigoVersion
            + '&esArchivoConf=' + esArchivoConfidencial;
    }
}

function obtenerArchivo(listaSeccionDocumento, concepto, pos) {

    var seccion = obtenerSeccionXcnp(listaSeccionDocumento, concepto);

    for (var i = 0; i < seccion.ListaArchivo.length; i++) {
        if (i == pos)
            return seccion.ListaArchivo[i];
    }

    return null;
}

function obtenerSeccionXcnp(listaSeccion, concepto) {
    for (var i = 0; i < listaSeccion.length; i++) {
        var seccion = listaSeccion[i];
        if (seccion.Fevrqcodi == concepto)
            return seccion;
    }

    return null;
}