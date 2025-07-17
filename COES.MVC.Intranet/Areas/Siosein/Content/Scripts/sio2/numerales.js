var controlador = siteRoot + 'Siosein/Numerales/'
var width_rpt = 900;
var height_rpt = 700;

$(function () {
    $('#txtMesAnio').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            //consultaCM();
        }
    });
    $('#txtFecha').Zebra_DatePicker();
    $('#cbTipCarga').change(function () {
        if ($('#cbTipCarga').val() == "1") {
            $("#tdXls1").hide(); $("#tdXls2").hide(); $("#tdZip1").show(); $("#tdZip2").show();
        } else { $("#tdXls1").show(); $("#tdXls2").show(); $("#tdZip1").hide(); $("#tdZip2").hide(); }
        $("#listado").html("");
        $("#btnCargar").hide();
    });

    $("#btnCargar").click(function () {
        if ($('#cbTipCarga').val() == "1") {
            saveCargaCM();
        } else {
            saveCargaCMdia();
        }
    });

    $("#btnVerLog").click(function () {
        if ($('#cbTipCarga').val() == "1") {
            viewLogAuditoriaMen();
        } else {
            viewLogAuditoriaDia();
        }
    });

    $("#tr_fechaDia").hide();
    crearPupload();
    $("#btnCargar").hide();

    $(window).resize(function () {
        updateContainer();
    });
});

function updateContainer() {
    var $containerWidth = $(window).width();

    $('#listado').css("width", $containerWidth - 240 + "px");
}

function saveCargaCM() {
    $.ajax({
        type: 'POST',
        url: controlador + "SaveCM",
        dataType: 'json',
        data: { mesAnio: $('#txtMesAnio').val(), nrodias: $('#txtNroDias').val() },
        success: function (evt) {
            if (evt != -1) {
                //consultaCM();
                alert("Carga Completa de Informacion de Costos Marginales.");
            }
            else { alert("Ha ocurrido un error."); }
        },
        error: function () { alert("Error al cargar Excel Web"); }
    });
}

function saveCargaCMdia() {
    $.ajax({
        type: 'POST',
        url: controlador + "SaveCMdia",
        dataType: 'json',
        data: { fecha: $('#txtFecha').val() },
        success: function (evt) {
            if (evt != -1) {
                //consultaCM();
                alert("Carga Completa de Informacion de Costos Marginales.");
                //crearPupload();
            }
            else { alert("Ha ocurrido un error."); }
        },
        error: function () { alert("Error al cargar Excel Web"); }
    });
}

function consultaCM() {
    $.ajax({
        type: 'POST',
        url: controlador + "ConsultaCM",
        dataType: 'json',
        data: {
            mesAnio: $('#txtFecha').val()
        },
        success: function (evt) {
            if (evt.NRegistros > 0) {
                $('#tr_fechaDia').show();
                $('#txtFechaDia').Zebra_DatePicker({
                    direction: [evt.FechaInicio, evt.FechaFin],
                    onSelect: function () {
                        consultaDiaCM();
                    }
                });
                $('#txtFechaDia').val(evt.FechaInicio);
                consultaDiaCM();
            } else { $("#btnCargar").hide(); $("#listado").html(""); $("#tr_fechaDia").hide(); mostrarMessage("Cargar informacion de ZIP..!!"); }
        },
        error: function (err) { alert("Error..!!"); }
    });
}

function consultaDiaCM() {
    $.ajax({
        type: 'POST',
        url: controlador + "ConsultaDiaCM",
        //dataType: 'json',
        data: {
            fecha: $('#txtFechaDia').val()
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() - 30 + "px");
            $("#listado").html(evt);

            mostrarExito("Informacion Exitosa!!");

        },
        error: function (err) { alert("Error..!!"); }
    });
}

function crearPupload() {
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnLoad',
        url: controlador + 'Upload',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '30mb',
            mime_types: [
                { title: "Archivos ZIP y Excel", extensions: "zip,xlsx,xls" },
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
                $("#wait").css("display", "block");
            },
            UploadComplete: function (up, file) {
                mostrarAlerta("Subida completada <strong>Por favor espere</strong>");

                updateContainer();
                if ($('#cbTipCarga').val() == "1") {
                    $("#listado").html(leerFileUp());
                } else {
                    $("#listado").html(leerFileUpXls());
                }
                $("#wait").css("display", "none");
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
    var fecha = $('#txtMesAnio').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'LeerFileUpArchivo',
        async: false,
        data: { fecha: fecha },
        //dataType: 'json',
        success: function (evt) {
            retorno = evt;
        },
        error: function (jqXHR, exception) {
            $('#listado').html("");
            var msg = '';
            if (jqXHR.status === 0) {
                msg = 'No hay conexión de red.\n Verifique Network.';
            } else if (jqXHR.status == 404) {
                msg = 'Requested page not found. [404]';
            } else if (jqXHR.status == 500) {
                msg = 'Internal Server Error [500].';
                if (jqXHR.responseText.indexOf("FileAccess") !== -1) {
                    msg = 'Error: Carga simultanea de archivo, favor intentarlo otra vez';
                }
                else if(jqXHR.responseText.indexOf("ORA-") !== -1){
                        msg = 'Error: Carga simultanea de archivo, favor intentarlo otra vez';
                }
                else if (jqXHR.responseText.indexOf("already exists") !== -1) {
                    msg = 'Error: Carga simultanea de archivo, favor intentarlo otra vez';
                }
            } else if (exception === 'parsererror') {
                msg = 'Requested JSON parse failed.';
            } else if (exception === 'timeout') {
                msg = 'Time out error.';
            } else if (exception === 'abort') {
                msg = 'Ajax request aborted.';
            } else {
                msg = 'Uncaught Error.\n' + jqXHR.responseText;
            }

            mostrarError(msg);
        }
    });
    return retorno;
}

function leerFileUpXls() {
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
        error: function (jqXHR, exception) {
            $('#listado').html("");
            var msg = '';
            if (jqXHR.status === 0) {
                msg = 'No hay conexión de red.\n Verifique Network.';
            } else if (jqXHR.status == 404) {
                msg = 'Requested page not found. [404]';
            } else if (jqXHR.status == 500) {
                msg = 'Internal Server Error [500].';
                if (jqXHR.responseText.indexOf("FileAccess") !== -1) {
                    msg = 'Error: Carga simultanea de archivo, favor intentarlo otra vez';
                }
                else if (jqXHR.responseText.indexOf("ORA-") !== -1) {
                    msg = 'Error: Carga simultanea de archivo, favor intentarlo otra vez';
                }
            } else if (exception === 'parsererror') {
                msg = 'Requested JSON parse failed.';
            } else if (exception === 'timeout') {
                msg = 'Time out error.';
            } else if (exception === 'abort') {
                msg = 'Ajax request aborted.';
            } else {
                msg = 'Uncaught Error.\n' + jqXHR.responseText;
            }

            mostrarError(msg);
        }
    });
    return retorno;
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

function viewLogAuditoriaMen() {
    $.ajax({
        type: 'POST',
        url: controlador + "ViewLogAuditoriaMen",
        dataType: 'json',
        data: { mesAnio: $('#txtMesAnio').val() },
        success: function (evt) {
            $('#listadolog').html(evt.Resultado);
            openPopup();
        },
        error: function (err) { alert("Error..!!"); }
    });
}

function viewLogAuditoriaDia() {
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
