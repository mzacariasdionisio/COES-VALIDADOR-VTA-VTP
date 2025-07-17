var TIPO_MODULO_INTERVENCION = "Informe_Agente";
var TIPO_MODULO_MENSAJE = "Mensajes";

var IMG_ARCHIVO_CANCELAR = '<img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="" width="19" height="19" style="" >';
var IMG_ARCHIVO_DESCARGAR = '<img src="' + siteRoot + 'Content/Images/btn-download.png" alt="" width="19" height="19" style="" >';
var IMG_ARCHIVO_VISTA_PREVIA = '<img src="' + siteRoot + 'Content/Images/vista_previa.png" alt="" width="19" height="19" style="" >';

//global para guardar archivos del mensaje seleccionado
var LISTA_SECCION_ARCHIVO_X_INTERVENCION = [];
var LISTA_SECCION_ARCHIVO_X_MENSAJE = [];
var LISTA_SECCION_ARCHIVO_X_REQUISITO = [];

var TIPO_ARCHIVO_INTERVENCION = 1;
var TIPO_ARCHIVO_MENSAJE = 2;
var TIPO_ARCHIVO_SUSTENTO = 3;

////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Archivos de Intervención (1 sección cuando es el formulario de la intervención y 1 o más secciones por cada requisito de sustento)
////////////////////////////////////////////////////////////////////////////////////////////////////////////

//html
function arch_cargarHtmlArchivoEnPrograma(divHtml, seccion) {

    if (!seccion.EsEditable && seccion.ListaArchivo.length == 0) {
        $("#" + divHtml).html('');
    }
    else {
        //tiene algun archivo o es editable
        var html = '';
        html += '<table class="miTablaR" style="width: auto;" >'; //width: max-content va en el js

        //1. crear html

        var htmlSeccion1 = arch_generarHtmlTablaDocumentoCab(seccion);
        var htmlSeccion2 = arch_generarHtmlTablaDocumentoCuerpo(seccion);

        var htmlSec = `
            <tr id='divCab${seccion.IdPrefijo}'>
                ${htmlSeccion1}
            </tr>
            <tr id='divCuerpo${seccion.IdPrefijo}'>
                ${htmlSeccion2}
            </tr>
        `;

        html += htmlSec;
        html += '</table>';

        $("#" + divHtml).html(html);

        //2. inicializar upload
        if (seccion.EsEditable) {
            arch_pUploadArchivo(seccion);
        }
    }
}

function arch_generarHtmlTablaDocumentoCab(seccion) {
    var html = '';
    if (seccion.Inpstidesc != null && seccion.Inpstidesc != "")
        html = `
       <td colspan="1" class="tdPadd" style="padding-top: 4px; padding-bottom: 4px;padding-left: 10px;">
           <b>${seccion.Inpstidesc}</b>
       </td>
    `;

    return html;
}

function arch_generarHtmlTablaDocumentoCuerpo(seccion) {
    var esEditable = seccion.EsEditable;
    var idPrefijo = seccion.IdPrefijo;

    var html = "";
    var htmlBtnAgregar = '';
    var htmlTablaArchivo = arch_generarTablaListaBody(seccion);

    if (esEditable) {
        htmlBtnAgregar = `
            <div style="clear:both; height:10px"></div>

            <div style="width:180px">
                <input type="button" id="btnSelectFile${idPrefijo}" value="Agregar Archivo" />
                <div style="font-size: 10px;padding-left: 15px;color: #5F0202;">Tamaño máximo por archivo: 50MB</div>
            </div>
                        
            <div style="clear:both; height:5px"></div>
        `;

        html = `
            <td style="width: 440px; border: 1px solid #dddddd;">

                        ${htmlBtnAgregar}

                        <div id="fileInfo${idPrefijo}"></div>
                        <div id="progreso${idPrefijo}"></div>
                        <div id="fileInfo${idPrefijo}" class="file-upload plupload_container ui-widget-content " style="display:none"></div>

                        <input type="hidden" id="hfile${idPrefijo}" name="file${idPrefijo}" value=" " />
                        
                        <div id="listaArchivos${idPrefijo}" class="content-tabla">
                            ${htmlTablaArchivo}
                        </div>

                    </td>
        `;
    } else {
        html = `
                        <div id="listaArchivos${idPrefijo}" class="content-tabla">
                            ${htmlTablaArchivo}
                        </div>
        `;
    }

    return html;
}

function arch_generarTablaListaBody(seccion) {
    var htmlVisible = seccion.ListaArchivo.length > 0 ? "" : `;display:none;`;

    var html = `
                <table border="0" class="pretty tabla-icono tabla_archivo" cellspacing="0" style="width:470px; margin-left: 5px;margin-right: 5px; margin-bottom: 10px; ${htmlVisible}" id="tabla${seccion.IdPrefijo}">
                    <thead>
                        <tr>
                            <th style='width: 70px'>Acción</th>
                            <th style='width: 400px'>Nombre</th>
                        </tr>
                    </thead>

                    <tbody>`;

    for (var i = 0; i < seccion.ListaArchivo.length; i++) {
        var item = seccion.ListaArchivo[i];
        var idrow = "row" + item.Inarchnombrefisico.replace(".", "");
        var nomb = item.Inarchnombreoriginal;

        var tdVistaPreviaOffice = '';
        if (item.TieneVistaPreviaOffice) {
            tdVistaPreviaOffice = `
                                <a href="#" onclick="arch_vistaPreviaOffice('${seccion.Modulo}',${seccion.Subcarpetafiles},'${item.Inarchnombrefisico}');" style="cursor:pointer;"  title="Ver vista previa de archivo">
                                    ${IMG_ARCHIVO_VISTA_PREVIA}
                                </a>
            `;
        }

        var tdVistaPreviaNoOffice = '';
        if (item.TieneVistaPreviaNoOffice) {
            tdVistaPreviaNoOffice = `
                                <a href="#" onclick="arch_vistaPreviaNoOffice('${seccion.Modulo}',${seccion.Subcarpetafiles},'${item.Inarchnombrefisico}');" style="cursor:pointer;"  title="Ver vista previa de archivo">
                                    ${IMG_ARCHIVO_VISTA_PREVIA}
                                </a>
            `;
        }

        var tdEliminar = '';
        if (seccion.EsEditable) {
            tdEliminar = `
                                <a href="#" onclick="arch_eliminarRow('${seccion.Modulo}',${seccion.Subcarpetafiles},'${item.Inarchnombrefisico}')" style="cursor:pointer;" title="Quitar archivo">
                                    ${IMG_ARCHIVO_CANCELAR}
                                </a>
                        `;
        }

        html += `
                        <tr id="${idrow}">

                            <td style="">
                                <a href="#" onclick="arch_descargarArchivo('${seccion.Modulo}',${seccion.Subcarpetafiles},'${item.Inarchnombrefisico}');" style="cursor:pointer;" title="Descargar archivo">
                                    ${IMG_ARCHIVO_DESCARGAR}
                                </a>
                                ${tdVistaPreviaOffice}
                                ${tdVistaPreviaNoOffice}
                                ${tdEliminar}
                            </td>

                            <td style="text-align:left;">
                              ${nomb}
                            </td>
        `;

        html += "       </tr>";
    }

    html += `
                    </tbody>
                </table>`;

    return html;
}

//plugin cargar archivo
var ARRAY_PUPLOAD = [];

function arch_pUploadArchivo(seccion) {
    var modulo = seccion.Modulo;
    var progrcodi = seccion.Progrcodi;
    var carpetafiles = seccion.Carpetafiles;
    var subcarpetafiles = seccion.Subcarpetafiles;
    var prefijo = seccion.IdPrefijo;
    var arrayTiposArchivos = [];
    if (seccion.PuedeCargarArchivoSoloFoto) {
        arrayTiposArchivos.push({ title: "png", extensions: "PNG" });
        arrayTiposArchivos.push({ title: "JPEG", extensions: "JPEG" });
        arrayTiposArchivos.push({ title: "JPG", extensions: "JPG" });
        arrayTiposArchivos.push({ title: "BMP", extensions: "BMP" });
    }

    var uploaderP23 = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFile' + prefijo,
        container: document.getElementById('container'),
        chunks_size: '20mb',
        multipart_params: {
            modulo: modulo,
            progrcodi: progrcodi,
            carpetafiles: carpetafiles,
            subcarpetafiles: subcarpetafiles,
        },
        url: controlador + 'UploadFileEnPrograma',
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '50mb',
            mime_types: arrayTiposArchivos
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
                var model = JSON.parse(result.response);
                arch_agregarRow(modulo, subcarpetafiles, model.nuevonombre, file.name, model.tieneVistaPreviaOffice, model.tieneVistaPreviaNoOffice);
            },
            Error: function (up, err) {
                //habilitar botones
                $("input[id^=btnSelectFile_]").prop('disabled', false);
                $("input[id^=btnSelectFile_]").css('pointer-events', 'auto');

                arch_loadValidacionFile(err.code + "-" + err.message);
                if (err.code = -600) //error de tamaño
                    err.message = "Error: El archivo adjuntado supera el tamaño límite (100MB) por archivo. ";
                alert(err.message);
            }
        }
    });
    uploaderP23.init();

    ARRAY_PUPLOAD.push(uploaderP23);
}

//acciones sobre los archivos
function arch_agregarRow(modulo, subcarpetafiles, nuevoNombre, nombreArchivo, tieneVistaPreviaOffice, tieneVistaPreviaNoOffice) {
    var seccion = arch_obtenerSeccionXSubcarpeta(modulo, subcarpetafiles);

    seccion.ListaArchivo.push({
        EsNuevo: true,
        Inarchnombrefisico: nuevoNombre,
        Inarchnombreoriginal: nombreArchivo,
        Inarchorden: seccion.ListaArchivo.length + 1,
        Inarchtipo: seccion.TipoArchivo,
        TieneVistaPreviaOffice: tieneVistaPreviaOffice,
        TieneVistaPreviaNoOffice: tieneVistaPreviaNoOffice,
        Inarchestado: 1
    });

    $("#listaArchivos" + seccion.IdPrefijo).html(arch_generarTablaListaBody(seccion));
}

function arch_eliminarRow(modulo, subcarpetafiles, nombreArchivo) {
    var seccion = arch_obtenerSeccionXSubcarpeta(modulo, subcarpetafiles);
    var listaArchTmp = [];

    for (var i = 0; i < seccion.ListaArchivo.length; i++) {
        if (seccion.ListaArchivo[i].Inarchnombrefisico != nombreArchivo)
            listaArchTmp.push(seccion.ListaArchivo[i]);
    }

    seccion.ListaArchivo = listaArchTmp;

    $("#listaArchivos" + seccion.IdPrefijo).html(arch_generarTablaListaBody(seccion));
}

function arch_descargarArchivo(modulo, subcarpetafiles, nombreArchivo) {
    var seccion = arch_obtenerSeccionXSubcarpeta(modulo, subcarpetafiles);
    var regArchivo = arch_obtenerArchivo(modulo, subcarpetafiles, nombreArchivo);

    if (regArchivo != null) {
        window.location = `${controlador}DescargarArchivoDePrograma?modulo=${seccion.Modulo}&fileName=${regArchivo.Inarchnombrefisico}&fileNameOriginal=${regArchivo.Inarchnombreoriginal}&progrcodi=${seccion.Progrcodi}&carpetafiles=${seccion.Carpetafiles}&subcarpetafiles=${seccion.Subcarpetafiles}`;
    }
}

function arch_descargarArchivo_consulta(modulo, progrcodi, carpetafiles, subcarpetafiles, nombrefisico, nombreoriginal) {
    window.location = `${controlador}DescargarArchivoDePrograma?modulo=${modulo}&fileName=${nombrefisico}&fileNameOriginal=${nombreoriginal}&progrcodi=${progrcodi}&carpetafiles=${carpetafiles}&subcarpetafiles=${subcarpetafiles}`;
}

//util
function arch_obtenerSeccionXSubcarpeta(modulo, subcarpetafiles) {
    var listaSeccion = [];
    if (modulo == TIPO_MODULO_MENSAJE) listaSeccion = LISTA_SECCION_ARCHIVO_X_MENSAJE;
    if (modulo == TIPO_MODULO_INTERVENCION) listaSeccion = LISTA_SECCION_ARCHIVO_X_INTERVENCION;
    if (modulo == TIPO_MODULO_INTERVENCION && subcarpetafiles > 0) listaSeccion = LISTA_SECCION_ARCHIVO_X_REQUISITO;

    for (var i = 0; i < listaSeccion.length; i++) {
        var seccion = listaSeccion[i];
        if (seccion.Subcarpetafiles == subcarpetafiles)
            return seccion;
    }

    return null;
}

function arch_obtenerArchivo(modulo, subcarpetafiles, nombreArchivo) {
    var seccion = arch_obtenerSeccionXSubcarpeta(modulo, subcarpetafiles);

    for (var i = 0; i < seccion.ListaArchivo.length; i++) {
        if (seccion.ListaArchivo[i].Inarchnombrefisico == nombreArchivo)
            return seccion.ListaArchivo[i];
    }

    return null;
}

function arch_getIdPrefijo(subcarpetafiles) {
    var idPrefijoFila = "_sec_doc_" + subcarpetafiles;

    return idPrefijoFila;
}

function arch_loadValidacionFile(mensaje, prefijo) {
    //$('#fileInfo').innerHTML += mensaje;
    $('#fileInfo' + prefijo).removeClass("file-ok");
    $('#fileInfo' + prefijo).removeClass("file-alert");

    $('#fileInfo' + prefijo).html(mensaje);
    $('#fileInfo' + prefijo).addClass("file-alert");
}

function arch_limpiarMensaje() {
    $('#fileInfo').removeClass("file-alert");

    $('#fileInfo').html("");
}

//Vista previa

function arch_vistaPreviaNoOffice(modulo, subcarpetafiles, nombreArchivo) {
    arch_vistaPreviaGenerico(modulo, subcarpetafiles, nombreArchivo, false);
}

function arch_vistaPreviaOffice(modulo, subcarpetafiles, nombreArchivo) {
    arch_vistaPreviaGenerico(modulo, subcarpetafiles, nombreArchivo, true);
}

function arch_vistaPreviaGenerico(modulo, subcarpetafiles, nombreArchivo, esOffice) {
    var seccion = arch_obtenerSeccionXSubcarpeta(modulo, subcarpetafiles);
    var regArchivo = arch_obtenerArchivo(modulo, subcarpetafiles, nombreArchivo);
    var idVistaPrevia = "#" + seccion.IdDivVistaPrevia;

    $(idVistaPrevia).show();
    $(idVistaPrevia).html('');

    $.ajax({
        type: 'POST',
        url: controler + 'VistaPreviaArchivoDePrograma',
        data: {
            modulo: seccion.Modulo,
            progrcodi: seccion.Progrcodi,
            carpetafiles: seccion.Carpetafiles,
            subcarpetafiles: seccion.Subcarpetafiles,
            fileName: regArchivo.Inarchnombrefisico,
        },
        success: function (model) {
            if (model.Resultado != "") {

                var rutaCompleta = window.location.href;
                var ruraInicial = rutaCompleta.split("/Intervenciones");
                var urlPrincipal = ruraInicial[0];
                if (!urlPrincipal.endsWith('/')) urlPrincipal += '/';

                var urlFrame = urlPrincipal + model.Resultado;
                var urlFinal = "";
                if (esOffice) {
                    urlFinal = "https://view.officeapps.live.com/op/embed.aspx?src=" + urlFrame;
                } else {
                    urlFinal = urlFrame;
                }

                $(idVistaPrevia).attr("src", urlFinal);

                $('html, body').scrollTop($(idVistaPrevia).offset().top);

            } else {
                alert("No está disponible la vista previa para el archivo seleccionado.");
            }
        },
        error: function (err) {
        }
    });
}
