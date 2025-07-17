function cargarHtmlDocumentoXEnvio(listaSeccionDocumento, tienePermisoEditar) {
    var html = '';

    for (var i = 0; i < listaSeccionDocumento.length; i++) {
        var seccion = listaSeccionDocumento[i];
        var prefijo = "_sec_doc_" + seccion.Seccion.Ccombcodi;
        var htmlSec = "<div id=div_" + prefijo + ">";
        htmlSec += generarHtmlTablaDocumento(seccion.ListaArchivo, seccion.Seccion.Ccombcodi, prefijo, seccion.Seccion.Titulo, seccion.Seccion.Descripcion, tienePermisoEditar);
        htmlSec += "</div>";

        html += htmlSec;
    }

    $("#html_archivos").html(html);

    for (var i = 0; i < listaSeccionDocumento.length; i++) {
        var seccion = listaSeccionDocumento[i];
        var idEnvio = 0;
        pUploadArchivo(idEnvio, seccion.Seccion.Ccombcodi, "_sec_doc_" + seccion.Seccion.Ccombcodi);
    }
}

function generarHtmlTablaDocumento(listaArchivo, concepcodi, idPrefijo, titulo, desc) {

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

    if (!MODELO_WEB.Readonly) {
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

                        <div id="listaArchivos${idPrefijo}" class="content-tabla">
    `;

    html += generarTablaListaBody(listaArchivo, concepcodi, idPrefijo);

    html += `
                        </div>

                    </td>
                </tr>
            </table>
                <div style='clear:both; height:10px;width:100px;'></div>
    `;

    return html;
}

function generarTablaListaBody(listaArchivo, concepcodi, idPrefijo) {

    var html = `
                <table border="0" class="pretty tabla-icono" cellspacing="0" style="width:400px" id="tabla${idPrefijo}">

                    <tbody>`;

    for (var i = 0; i < listaArchivo.length; i++) {
        var item = listaArchivo[i];
        var idrow = "row" + item.Archienvioorden;
        var nomb = item.Cbarchnombreenvio;

        html += `
                        <tr id="${idrow}">

                            <td style="width:30px;">
                                <img width="15" height="15" src="../../Content/Images/file.png" />
                            </td>
                            <td onclick="descargarArchivo(${concepcodi},${i});" style="cursor:pointer;text-align:left;">
                                <a href="#">${nomb}</a>
                            </td>
        `;
        if (!MODELO_WEB.Readonly) {
            html += `     
                            <td onclick="eliminarRow(${concepcodi},${i})" style="width:30px;cursor:pointer;">
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

//
var ARRAY_PUPLOAD = [];

function pUploadArchivo(idEnvio, concepcodi, prefijo) {
    var uploaderP23 = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFile' + prefijo,
        container: document.getElementById('container'),
        chunks_size: '20mb',
        multipart_params: {
            "concepcodi": concepcodi,
            "idEnvio": idEnvio
        },
        url: siteRoot + 'Combustibles/envio/UploadTemporal',
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
                agregarRow(concepcodi, JSON.parse(result.response).nuevonombre, file.name);
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

    ARRAY_PUPLOAD.push(uploaderP23);
}

function agregarRow(concepcodi, nuevoNombre, nombreArchivo) {
    var seccion = obtenerSeccionXcnp(MODELO_WEB.ListaSeccionDocumento, concepcodi);
    seccion.ListaArchivo.push({ EsNuevo: true, Ccombcodi: concepcodi, Cbarchnombrefisico: nuevoNombre, Cbarchnombreenvio: nombreArchivo });

    var idPrefijo = "_sec_doc_" + seccion.Seccion.Ccombcodi;
    $("#listaArchivos" + idPrefijo).html(generarTablaListaBody(seccion.ListaArchivo, seccion.Seccion.Ccombcodi, idPrefijo, EVT_GLOBAL.AccionEditar));
}

function eliminarRow(concepcodi, pos) {
    var seccion = obtenerSeccionXcnp(MODELO_WEB.ListaSeccionDocumento, concepcodi);
    var listaArchTmp = [];

    for (var i = 0; i < seccion.ListaArchivo.length; i++) {
        if (i != pos)
            listaArchTmp.push(seccion.ListaArchivo[i]);
    }

    seccion.ListaArchivo = listaArchTmp;

    var idPrefijo = "_sec_doc_" + seccion.Seccion.Ccombcodi;
    $("#listaArchivos" + idPrefijo).html(generarTablaListaBody(seccion.ListaArchivo, seccion.Seccion.Ccombcodi, idPrefijo, EVT_GLOBAL.AccionEditar));
}

function descargarArchivo(concepcodi, pos) {
    var regArchivo = obtenerArchivo(MODELO_WEB.ListaSeccionDocumento, concepcodi, pos);
    if (regArchivo != null) {
        if (regArchivo.EsNuevo)
            window.location = controlador + 'DescargarArchivoTemporal?fileName=' + regArchivo.Cbarchnombrefisico + '&concepcodi=' + regArchivo.Ccombcodi;
        else
            window.location = controlador + 'DescargarArchivoEnvio?fileName=' + regArchivo.Cbarchnombrefisico + '&concepcodi=' + regArchivo.Ccombcodi + '&idEnvio=' + MODELO_WEB.Cbenvcodi;
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
