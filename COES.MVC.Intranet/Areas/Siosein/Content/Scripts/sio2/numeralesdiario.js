var controlador = siteRoot + 'Siosein/Numerales/'
var width_rpt = 900;
var height_rpt = 700;

$(function () {
    $('#txtFecha').Zebra_DatePicker({
        onSelect: function () {
            consultaDiaCM();
        }
    });

    $('#btnExcel').click(function () {
        exportarExcel();
    });

    $("#btnCargar").click(function () {
        saveCargaCM();
    });

    $("#btnVerLog").click(function () {
        viewLogAuditoriaMen();
    });

    crearPupload();
    consultaDiaCM();
});

function consultaDiaCM() {
    $.ajax({
        type: 'POST',
        url: controlador + "ConsultaDiaCM",
        //dataType: 'json',
        data: {
            fecha: $('#txtFecha').val()
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() - 30 + "px");
            $("#listado").html(evt);

            mostrarExito("Informacion Exitosa!!");
            
        },
        error: function (err) { alert("Error..!!"); }
    });
}

function mostrarAlerta(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}

function mostrarError(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}

function mostrarExito(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}

function mostrarMessage(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
}

function exportarExcel() {
    $.ajax({
        type: 'POST',
        url: controlador + "ExportarExcelCM",
        dataType: 'json',
        data: {
            fecha: $('#txtFecha').val()
        },
        success: function (result) {
            window.location = controlador + "ExportarReporteXls?nameFile=" + result.Resultado;
        },
        error: function (err) { alert("Error..!!"); }
    });
}

function crearPupload() {
    var msjOpc = "";
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectExcel',
        url: controlador + 'uploadexcel',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '30mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx,xls" },
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                if (uploader.files.length == 2) {
                    uploader.removeFile(uploader.files[0]);
                }
                uploader.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                mostrarAlerta("Subida completada <strong>Por favor espere</strong>");

                width_rpt = parseInt($(".content-tabla").width());
                height_rpt = parseInt($(".content-tabla").height());
                $('#listado').css("width", width_rpt - 30 + "px");
                $('#listado').css("height", height_rpt + "px");
                $("#listado").html(leerFileUp());
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
                if (err.code == -600) {
                    alert("La capacidad máxima de Zip es de 30Mb..... \nEliminar carpetas o archivos que no son parte del contenido del archivo ZIP."); return;
                }
            }
        }
    });
    uploader.init();
}

function leerFileUp() {
    var retorno;
    var fecha = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'LeerFileUpArchivoDiario',
        async: false,
        data: { fecha: fecha },
        //dataType: 'json',
        success: function (evt) {
            retorno = evt;
        },
        error: function (up, err) {
            mostrarError("Ha ocurrido un error");
        }
    });
    return retorno;
}

function saveCargaCM() {
    $.ajax({
        type: 'POST',
        url: controlador + "SaveCMdia",
        dataType: 'json',
        data: { fecha: $('#txtFecha').val() },
        success: function (evt) {
            if (evt != -1) {
                //consultaCM();
                alert("Carga Completa de Informacion de Costos Marginales.");
                crearPupload();
            }
            else { alert("Ha ocurrido un error."); }
        },
        error: function () { alert("Error al cargar Excel Web"); }
    });
}

function viewLogAuditoriaMen() {
    $.ajax({
        type: 'POST',
        url: controlador + "ViewLogAuditoriaDia",
        dataType: 'json',
        data: { fecha: $('#txtFecha').val() },
        success: function (evt) {
            $('#listadolog').html(evt.Resultado);
            openPopup();
        },
        error: function (err) { alert("Error..!!"); }
    });
}

function openPopup() {
    setTimeout(function () {
        $('#popupTablaNuevo').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}
