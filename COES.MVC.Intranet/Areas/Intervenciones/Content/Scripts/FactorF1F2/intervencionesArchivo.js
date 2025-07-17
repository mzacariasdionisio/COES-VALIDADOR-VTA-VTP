var LISTA_ARCHIVOS_MMAYOR = [];

var nombrePopup = "#popupAdjuntos";
var mombreModulo = "FactoresF1F2";

function mostrarAdjuntos(infmmcodi, posItem) {

    POS_ITEM = posItem;

    $(nombrePopup).html("");

    $.ajax({
        type: 'POST',
        url: controlador + "GetMmayor",
        dataType: 'json',
        data: { infmmcodi: infmmcodi },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                LISTA_ARCHIVOS_MMAYOR = evt.Entidad.ListaArchivos;

                //Inicializar Formulario
                $(nombrePopup).html(generarHtmlSustento(evt.Entidad));
                dibujarListaArchivo(LISTA_ARCHIVOS_MMAYOR, infmmcodi);

                $(nombrePopup).bPopup({
                    modalClose: false,
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    onClose: function () {
                        $('#popup').empty();
                    }
                });

                //mostrar archivos
                //mostrarListaArchivosNuevo(infmmcodi);
                asignarEventosPopup(infmmcodi);
                adjuntarArchivo(infmmcodi);

            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Error al cargar sustento");
        }
    });
}

function generarHtmlSustento(entidad) {
    return `<div id="divmensaje" style="margin-top: 8px;" hidden>
    <i><strong>(*) la Justificación es obligatoria al hacer una edición</strong> </i>
    <br />
    <i><strong>(*) Al cambiar el tipo de intervención y/o Descripción se guardará como una nueva intervención</strong> </i>
</div>

<div class="popup-title">
    <span>Sustento</span>
    <div class="content-botonera">
        <input type="button" id="bActualizar" value="Actualizar" class='b-update' style='float: right; margin-right: 35px;'/>
        <input type="button" id="bSalir" value="Salir" class='b-close' style='float: right; margin-right: 35px;'/>
    </div>
</div>

            <div id="mainLayoutEdit" style="padding-top:8px; padding-left: 0px; padding-right: 0px;">

                    <table cellpadding="5" style="width: 100%;"">
                        <tr class="fila_form_reg" id="trJustificacion">
                            <td style="width:15%" valign="top">Justificación:</td>
                            <td style="width:85%">
                                <textarea id="justificacion" name="justificacion" value="" style="background-color: white; width:100%;" rows="4">${entidad.Infmmjustif}</textarea>
                            </td>
                        </tr>
                    </table>

            </div>        

            <br><br>

<div id="divarchivo" class="title-seccion" style="margin-top:20px">Carga de archivos <input type="button" id="btnSelectFile" value="Seleccionar archivo" /></div>
<table>
    <tr>
        <td valign="top" style="width:100%">
            <div id="container" class="file-carga" style="min-height: 0px !important">
                <div class="file-carga-titulo">Archivos seleccionados</div>
                <div id="loadingcarga" class="estado-carga">
                    <div class="estado-image"></div>
                    <div class="estado-text">Cargando...</div>
                </div>
                <div id="filelist">No soportado por el navegador.</div>
            </div>
        </td>
        <td style="width:4%"></td>
        <td valign="top" style="width:48%">
            <div id="listaArchivos">

            </div>
        </td>
    </tr>
    <tr>
        <td>
            <div id="listaArchivos2">
                <iframe id="vistaprevia" style="width: 100%; height:500px;" frameborder="0" hidden></iframe>
            </div>
        </td>
    </tr></table>`;
}

function adjuntarArchivo(infmmcodi) {

    uploaderN = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFile',
        container: document.getElementById('container'),
        url: controlador + 'UploadFileMmayor?sModulo=' + mombreModulo + '&anioMes=' + $("#VersionDesc").val() + '&infmmcodi=' + infmmcodi,
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '50mb',
            mime_types: []
        },
        init: {
            PostInit: function () {
                //document.getElementById('filelist').innerHTML = '';
                $('#loadingcarga').css('display', 'none');
            },
            FilesAdded: function (up, files) {
                document.getElementById('filelist').innerHTML = '';
                $('#container').css('display', 'block');

                for (i = 0; i < uploaderN.files.length; i++) {
                    var file = uploaderN.files[i];
                }
                uploaderN.start();
            },
            UploadProgress: function (up, file) {
            },
            UploadComplete: function (up, file, result) {
            },
            FileUploaded: function (up, file, result) {
                var model = JSON.parse(result.response);

                if (model.success) {
                    $('#container').css('display', 'block');
                    $('#loadingcarga').css('display', 'none');

                    agregarRow(infmmcodi, file.name, model.nuevonombre);
                } else {
                    document.getElementById('filelist').innerHTML = `<div class="action-alert">${model.StrMensaje}</div>`;
                }
            },
            Error: function (up, err) {

                if (err.code === -600) {
                    alert("El archivo no debe exceder los 50 MB."); return;
                    //document.getElementById('filelist').innerHTML = `<div class="action-alert">El archivo no debe exceder los 50 MB.</div>`;
                }
                else {
                    document.getElementById('filelist').innerHTML = `<div class="action-alert">Ha ocurrido un error.</div>`;
                }
            }
        }
    });
    uploaderN.init();
}

function agregarRow(infmmcodi, nombreArchivo, nuevoNombre) {

    LISTA_ARCHIVOS_MMAYOR[LISTA_ARCHIVOS_MMAYOR.length] = {
        EsNuevo: true, Infmmcodi: parseInt(infmmcodi), Inarchnombrefisico: nuevoNombre,
        Inarchnombreoriginal: nombreArchivo, Inarchcodi: 0, Infvercodi: 0, Infmmhoja: 0, Inarchorden: 0,
        Inarchestado: 1
    };

    dibujarListaArchivo(LISTA_ARCHIVOS_MMAYOR, infmmcodi);
}

function eliminarRow(infmmcodi, pos) {

    var listaArchivo = LISTA_ARCHIVOS_MMAYOR;
    var listaArchTmp = [];

    for (var i = 0; i < listaArchivo.length; i++) {
        if (i != pos)
            listaArchTmp.push(listaArchivo[i]);
    }

    dibujarListaArchivo(listaArchTmp, infmmcodi);

    LISTA_ARCHIVOS_MMAYOR = listaArchTmp;
}

function descargarArchivo(infmmcodi, posItem, posArchivo) {
    var regArchivo = LISTA_ITEMS_MMAYOR[posItem].ListaArchivos[posArchivo];

    if (regArchivo != null) {
        var anioMes = $("#VersionDesc").val();
        window.location = controlador + `DescargarArchivoMmayor?infmmcodi=${infmmcodi}&anioMes=${anioMes}&fileName=${regArchivo.Inarchnombrefisico}&fileNameOriginal=${regArchivo.Inarchnombreoriginal}`;
    }
}

function dibujarListaArchivo(listaArchivos, infmmcodi) {
    $("#filelist").html('');

    var htmlDiv = '';

    var cont = 0;
    for (key in listaArchivos) {
        var value = listaArchivos[key];

        var htmlFila = `
            <div class="file-name" id="${cont}">
                <a onclick="descargarArchivo(${infmmcodi}, ${POS_ITEM}, ${cont});" style="cursor:pointer;text-align:left;">${value.Inarchnombreoriginal}</a>                
            `;

        var existeVariable1 = !(typeof OPCION_ACTUAL === 'undefined');
        var existeVariable2 = !(typeof OPCION_VER === 'undefined');
        if (existeVariable1 && existeVariable2) {
            if (OPCION_ACTUAL != OPCION_VER) {
                htmlFila += `<a class="remove-item" href="JavaScript:eliminarRow(${cont}, ${infmmcodi});">X</a>`;
            }
        } else {
            htmlFila += `<a class="remove-item" href="JavaScript:eliminarRow(${cont}, ${infmmcodi});">X</a> `;
        }
        htmlFila += "</div>";

        cont++;
        htmlDiv += htmlFila;
    }

    $("#filelist").html(htmlDiv);
}
