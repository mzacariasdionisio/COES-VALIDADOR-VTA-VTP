var controlador = siteRoot + 'InformeEjecutivoMen/ReporteEjecutivo/'

var IMG_VER_NUMERAL = '<img src="' + siteRoot + 'Content/Images/btn-open.png" alt="" width="19" height="19" style="">';
var IMG_DESCARGA_EXCEL = '<img src="' + siteRoot + 'Content/Images/ExportExcel.png" alt="" width="19" height="19" style="">';
var IMG_DESCARGA_PDF = '<img src="' + siteRoot + 'Content/Images/pdf.png" alt="" width="19" height="19" style="">';
var IMG_SUBIR_PDF = '<img src="' + siteRoot + 'Content/Images/subir.png" alt="" width="19" height="19" style="">';
var IMG_DESCARGA_LOG = '<img src="' + siteRoot + 'Content/Images/file.png" alt="" width="19" height="19" style="">';

$(function () {

    $('#DPMesConsulta').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            //setearFechaSesion();
            versionListado();
        }
    });

    $('#MesAnhoIniNuevo').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#btnGuardarVersion').click(function () {
        versionGuardar();
    });

    versionListado();
    $('#btnPopupNuevaVersion').click(function () {
        $("#MesAnhoIniNuevo").val($("#DPMesConsulta").val());
        $('#txtMotivo').val('');
        setTimeout(function () {
            $('#idPopupVersion').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown',
                modalClose: false
            });
        }, 50);
    });

    cargarUploaderArchivoPDF();
});

function versionGuardar() {
    $('#txtMotivo').val($('#txtMotivo').val().trim());

    //datos
    var fechaPeriodo = $('#MesAnhoIniNuevo').val();
    var motivo = $('#txtMotivo').val();

    if (motivo != "") {
        $.ajax({
            type: 'POST',
            async: true,
            url: controlador + 'GuardarNuevaVersion',
            data: {
                motivo: motivo,
                fechaPeriodo: fechaPeriodo
            },
            success: function (result) {
                if (result.Resultado == "-1") {
                    alert("Ocurrio un error en la generación de la versión");
                } else {                   
                    versionListado();
                    $('#idPopupVersion').bPopup().close();
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    } else {
        alert("Debe escribir un motivo para poder hacer la generación");
    }
}

function versionListado() {
    $("#div_reporte").html('');
    $("#mensaje").hide();

    var fechaPeriodo = $('#DPMesConsulta').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoVersion',
        dataType: 'json',
        data: {
            fechaPeriodo: fechaPeriodo
        },
        cache: false,
        success: function (data) {
            if (data.Resultado != "-1") {
                console.log("Informe Ejecutivo mensual " + fechaPeriodo);
                console.log(data.ObjFecha);

                var html = dibujarTablaListadoVersion(data.ListaVersion);
                $("#div_reporte").html(html);

                $('#tblListado').dataTable({
                    "destroy": "true",
                    "scrollY": 550,
                    "scrollX": true,
                    "sDom": 't',
                    "ordering": false,
                    "bPaginate": false,
                    "iDisplayLength": -1
                });

            } else {
                alert("Ha ocurrido un error: " + data.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function dibujarTablaListadoVersion(lista) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" width="100%" id="tblListado">
        <thead>
            <tr>
                <th style='width: 70px'>Numerales</th>
                <th style='width: 70px'>Excel</th>
                <th style='width: 70px'>Pdf</th>                
                <th style='width: 40px'>Versión</th>
                <th style='width: 200px'>Periodo</th>
                <th style='width: 400px'>Motivo</th>
                <th style='width: 100px'>Usuario creación</th>
                <th style='width: 100px'>Fecha creación</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];

        var htmlPdf = '';
        if (item.TienePdf) htmlPdf = `
                    <a title="Descargar PDF" href="JavaScript:descargarPdf(${item.Verscodi});">${IMG_DESCARGA_PDF} </a>
                    <a title="Vista previa PDF" href="JavaScript:vistaPreviaPdf(${item.Verscodi});" style='font-size: 28px;' > 👁 </a>
        `;

        cadena += `
            <tr>
                <td style="height: 24px">
                    <a title="Ver numeral" href="JavaScript:verNumeral(${item.Verscodi});">${IMG_VER_NUMERAL} </a>
                </td>
                <td style="height: 24px">
                    <a title="Descargar Excel" href="JavaScript:descargarExcel(${item.Verscodi});">${IMG_DESCARGA_EXCEL} </a>
                    <a title="Vista previa Excel" href="JavaScript:vistaPreviaExcel(${item.Verscodi});" style='font-size: 28px;' > 👁 </a>
                </td>
                <td style="height: 24px">
                    <a title="Cargar PDF" href="JavaScript:subirPDF(${item.Verscodi});">${IMG_SUBIR_PDF} </a>
                    ${htmlPdf}
                </td>               
                <td style="height: 24px;text-align: center; ">${item.VerscorrelativoDesc}</td>
                <td style="height: 24px;text-align: center; ">${item.VersfechaperiodoDesc}</td>
                <td style="height: 24px;text-align: center; ">${item.Versmotivo}</td>
                <td style="height: 24px">${item.Versusucreacion}</td>
                <td style="height: 24px">${item.VersfeccreacionDesc}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

//por defecto redireccionar a Resumen Relevante
function verNumeral(verscodi) {    
    window.location = controlador + 'ProduccionEmpresaGeneradora?codigoVersion=' + verscodi;
}

function subirPDF(version) {
    $('#idArchivoPDF').val(version);
    $('#btnSelectFileArchivoPDF').click();
}

function descargarPdf(idVersion) {
    location.href = controlador + 'DescargarArchivoPDF?idVersion=' + idVersion;
}

function descargarExcel(idVersion) {
    location.href = controlador + 'DescargarArchivoExcel?idVersion=' + idVersion;
}

function vistaPreviaPdf(idVersion) {
    $('#idPopupVistaPrevia').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown',
        modalClose: false,
        follow: [false, false], //x, y        
    });

    $("#vistaprevia").show();
    $('#vistaprevia').html('');
    $('#vistaprevia').attr("src", controlador + 'DownloadArchivoPDF?idVersion=' + idVersion);
}



function vistaPreviaExcel(idVersion) {

    $("#vistaprevia").hide();

    $.ajax({
        type: 'POST',
        url: controlador + 'VisualizarArchivo',
        data: {
            idVersion: idVersion
        },
        success: function (model) {
            if (model.Resultado != "") {
                var rutaCompleta = window.location.href;
                var rutaInicial = rutaCompleta.split("InformeEjecutivoMen"); //Obtener url de intranet
                var urlPrincipal = rutaInicial[0];
                var urlFrame = urlPrincipal + model.Resultado;
                console.log(urlFrame);
                var urlFinal = "https://view.officeapps.live.com/op/embed.aspx?src=" + urlFrame;

                $('#idPopupVistaPrevia').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false
                });

                $("#vistaprevia").show();
                $('#vistaprevia').html('');
                $('#vistaprevia').attr("src", urlFinal);

            } else {
                alert("la vista previa solo permite archivos word, excel y pdf.");
            }
        },
        error: function (err) {
            document.getElementById('filelist').innerHTML = `<div class="action-alert">Ha ocurrido un error.</div>`;
        }
    });

    return true;

}


function cargarUploaderArchivoPDF() {
    uploaderArchivoPDF = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFileArchivoPDF',
        container: document.getElementById('containerArchivoPDF'),
        url: controlador + 'UploadArchivoPDF',
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            mime_types: [
                { title: "Documentos", extensions: "pdf" }
            ]
        },
        init: {
            PostInit: function () {
                document.getElementById('btnSubirArchivoPDF').onclick = function () {
                    if (uploaderArchivoPDF.files.length > 0) {
                        uploaderArchivoPDF.settings.multipart_params = {
                            "idVersion": $('#idArchivoPDF').val()
                        }
                        uploaderArchivoPDF.start();
                    }
                    else
                        loadValidacionFile('fileInfoArchivoPDF', 'Seleccione archivo');
                    return false;
                };
            },
            FilesAdded: function (up, files) {
                if (uploaderArchivoPDF.files.length == 2) {
                    uploaderArchivoPDF.removeFile(uploaderArchivoPDF.files[0]);
                }
                plupload.each(files, function (file) {
                    loadInfoFile('fileInfoArchivoPDF', file.name, plupload.formatSize(file.size));
                });
                uploaderArchivoPDF.refresh();
                $('#popupArchivoPDF').bPopup();
            },
            UploadProgress: function (up, file) {
                mostrarProgreso('progresoArchivoPDF', file.percent);
            },
            FileUploaded: function (up, file, response) {
                var json = JSON.parse(response.response);
                if (json.indicador == 1) {
                    $('#popupArchivoPDF').bPopup().close();
                    up.removeFile(file);
                    versionListado();
                }
            },
            Error: function (up, err) {
                loadValidacionFile('fileInfoArchivoPDF', err.code + "-" + err.message);
            }
        }
    });

    uploaderArchivoPDF.init();
}

function loadInfoFile(id, fileName, fileSize) {
    $('#' + id).html(fileName + " (" + fileSize + ")");
    $('#' + id).removeClass('action-alert');
    $('#' + id).addClass('action-exito');
    $('#' + id).css('margin-bottom', '10px');
}

function loadValidacionFile(id, mensaje) {
    $('#' + id).html(mensaje);
    $('#' + id).removeClass('action-exito');
    $('#' + id).addClass('action-alert');
    $('#' + id).css('margin-bottom', '10px');
}

function mostrarProgreso(id, porcentaje) {
    $('#' + id).text(porcentaje + "%");
}

function getFechaInicio() {
    var fechaInicio = $('#DPMesConsulta').val();
    fechaInicio = fechaInicio + "-01";
    return fechaInicio;
}
