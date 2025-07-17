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
    var htmlSeccion = generarHtmlTablaDocumento(idPrefijo, listaArchivo, seccion, tipoArchivo, esEditable);

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
        var idrow = "row" + item.Ftearcnombrefisico.replace(".", "");
        var nomb = item.Ftearcnombreoriginal;
        var checked = item.Ftearcflagsustentoconf == "S" ? "checked" : "";
        var disabled = esEditable ? '' : 'disabled';

        var tdEliminar = '';
        if (esEditable) tdEliminar = `
                                <a href="#" onclick="eliminarRow(${codigoCarpeta}, '${tipoArchivo}', '${item.Ftearcnombrefisico}')" style="width:30px;cursor:pointer;" title='Quitar archivo "${nomb}" '>
                                    ${IMG_CANCELAR}
                                </a>
        `;

        html += `
                        <tr id="${idrow}">

                            <td style="padding-left: 0px; padding-right: 0px;">
                                <a href="#" onclick="descargarArchivo(${codigoCarpeta}, '${tipoArchivo}', '${item.Ftearcnombrefisico}');" style="cursor:pointer;" title='Descargar archivo "${nomb}" '>   
                                    ${IMG_DESCARGAR}
                                </a>
                                ${tdEliminar}
                            </td>

                            <td style="text-align:center; ${tdConfVisible} " title='Confidencialidad o no del archivo "${nomb}" '>
                                <input type="checkbox" ${checked} ${disabled}>
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
    actualizarListaConCheckConfidencial(tipoArchivo);

    var seccion = obtenerSeccionXcnp(MODELO_FICHA.ListaAllItems, concepcodi);
    var listaArchivo = getListaArchivoXTipo(seccion, tipoArchivo);
    var codigoTipoArchivo = tipoArchivo == TIPO_ARCHIVO_VALOR_DATO ? 1 : 2;
    listaArchivo.push({
        EsNuevo: true, Ftitcodi: concepcodi, Ftearcnombrefisico: nuevoNombre,
        Ftearcnombreoriginal: nombreArchivo, Ftearcorden: listaArchivo.length + 1,
        Ftearcflagsustentoconf: 'N', Ftearctipo: codigoTipoArchivo
    });

    var idPrefijo = getIdPrefijoPuploadFT(seccion, tipoArchivo);
    $("#listaArchivos" + idPrefijo).html(generarTablaListaBody(listaArchivo, seccion, idPrefijo, tipoArchivo, OPCION_GLOBAL_EDITAR));

    //mostramos los mismos archivos para su registro dependiente (mismo concepto o propiedad)
    var ftitcodiCopia = seccion.FtitcodiDependiente;
    if (ftitcodiCopia > 0) {
        var seccionCopia = obtenerSeccionXcnp(MODELO_FICHA.ListaAllItems, ftitcodiCopia);
        var idPrefijoCopia = getIdPrefijoPuploadFT(seccionCopia, tipoArchivo);
        $("#listaArchivos" + idPrefijoCopia).html(generarTablaListaBody([], seccionCopia, idPrefijoCopia, tipoArchivo, false));
    }

}

function eliminarRow(concepcodi, tipoArchivo, nombreArchivo) {
    var seccion = obtenerSeccionXcnp(MODELO_FICHA.ListaAllItems, concepcodi);
    var listaArchivo = getListaArchivoXTipo(seccion, tipoArchivo);
    var listaArchTmp = [];

    for (var i = 0; i < listaArchivo.length; i++) {
        if (listaArchivo[i].Ftearcnombrefisico != nombreArchivo)
            listaArchTmp.push(listaArchivo[i]);
    }

    if (tipoArchivo == TIPO_ARCHIVO_VALOR_DATO)
        seccion.ListaArchivoValor = listaArchTmp;
    else
        seccion.ListaArchivoAdjunto = listaArchTmp;

    var idPrefijo = getIdPrefijoPuploadFT(seccion, tipoArchivo);
    $("#listaArchivos" + idPrefijo).html(generarTablaListaBody(listaArchTmp, seccion, idPrefijo, tipoArchivo, OPCION_GLOBAL_EDITAR));

    //mostramos los mismos archivos para su registro dependiente (mismo concepto o propiedad)
    var ftitcodiCopia = seccion.FtitcodiDependiente;
    if (ftitcodiCopia > 0) {
        var seccionCopia = obtenerSeccionXcnp(MODELO_FICHA.ListaAllItems, ftitcodiCopia);
        var idPrefijoCopia = getIdPrefijoPuploadFT(seccionCopia, tipoArchivo);
        $("#listaArchivos" + idPrefijoCopia).html(generarTablaListaBody([], seccionCopia, idPrefijoCopia, tipoArchivo, false));
    }
}

function descargarArchivo(idElemento, tipoArchivo, nombreArchivo) {
    var regArchivo = obtenerArchivo(MODELO_FICHA.ListaAllItems, idElemento, tipoArchivo, nombreArchivo);

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
        if (seccion.Ftitcodi == concepto) {
            return seccion;
        }
    }

    return null;
}

function obtenerArchivo(listaSeccionDocumento, concepto, tipoArchivo, nombreArchivo) {

    var seccion = obtenerSeccionXcnp(listaSeccionDocumento, concepto);
    var listaArchivo = getListaArchivoXTipo(seccion, tipoArchivo);

    for (var i = 0; i < listaArchivo.length; i++) {
        if (listaArchivo[i].Ftearcnombrefisico == nombreArchivo)
            return listaArchivo[i];
    }

    return null;
}

function actualizarListaConCheckConfidencial(tipoArchivo) {
    for (var i = 0; i < MODELO_FICHA.ListaAllItems.length; i++) {
        var seccion = MODELO_FICHA.ListaAllItems[i];
        var idPrefijo = getIdPrefijoPuploadFT(seccion, tipoArchivo);
        var idTabla = `listaArchivos${idPrefijo}`;

        var listaArchivo = getListaArchivoXTipo(seccion, tipoArchivo);

        for (var j = 0; j < listaArchivo.length; j++) {
            var arch = listaArchivo[j];
            var idTr = "row" + (arch.Ftearcnombrefisico.replace(".", ""));
            var tieneCheck = $(`#${idTabla} tr[id=${idTr}] input[type=checkbox]`).is(':checked');
            arch.Ftearcflagsustentoconf = tieneCheck ? "S" : "N";
        }
    }
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