var controlador = siteRoot + "transferencias/factorperdidamedia/";

$(document).ready(function () {
    //buscar();
    document.getElementById('divOpcionCarga').style.display = "none";
    $("#Version").prop("disabled", true);
    $("#Pericodi").change(function () {
        if ($("#Pericodi").val() != "") {
            var options = {};
            options.url = controlador + "GetVersion";
            options.type = "POST";
            options.data = JSON.stringify({ pericodi: $("#Pericodi").val() });
            options.dataType = "json";
            options.contentType = "application/json";
            options.success = function (modelo) {
                //console.log(modelo);
                $("#Version").empty();
                $("#Version").removeAttr("disabled");
                $.each(modelo.ListaRecalculo, function (k, v) {
                    var option = '<option value=' + v.RecaCodi + '>' + v.RecaNombre + '</option>';
                    $('#Version').append(option);
                    //console.log(option);
                });
                if (modelo.bEjecutar == true)
                { document.getElementById('divOpcionCarga').style.display = "block"; }
                else
                { document.getElementById('divOpcionCarga').style.display = "none"; }
            };
            options.error = function () { alert("Lo sentimos, no se encuentran registrada ninguna revisión"); };
            $.ajax(options);
        }
        else {
            $("#Version").empty();
            $("#Version").prop("disabled", true);
            document.getElementById('divOpcionCarga').style.display = "none";
        }
    });

    $("#VersionB").prop("disabled", true);
    $("#Pericodi2").change(function () {
        if ($("#Pericodi2").val() != "") {
            var options = {};
            options.url = controlador + "GetVersion";
            options.type = "POST";
            options.data = JSON.stringify({ pericodi: $("#Pericodi2").val() });
            options.dataType = "json";
            options.contentType = "application/json";
            options.success = function (modelo) {
                //console.log(modelo);
                $("#VersionB").empty();
                $("#VersionB").removeAttr("disabled");
                $.each(modelo.ListaRecalculo, function (k, v) {
                    var option = '<option value=' + v.RecaCodi + '>' + v.RecaNombre + '</option>';
                    $('#VersionB').append(option);
                    //console.log(option);
                });
            };
            options.error = function () { alert("Lo sentimos, no se encuentran registrada ninguna revisión"); };
            $.ajax(options);
        }
        else {
            $("#VersionB").empty();
            $("#VersionB").prop("disabled", true);
        }
    });

    $('#tab-container').easytabs({
        animate: false
    });

    $('#btnBuscar').click(function () {
        buscar();
    });

    $('#btnGenerarExcel').click(function () {
        generarExcel();
    });
    //ASSETEC 20190104
    $('#btnCopiarMB').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        copiarMB();
    });

    $('#btnExportarMB').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        exportarMB(1);
    });

    uploadExcelDU();
});

buscar = function () {
    var p;
    var cbo = $("#Pericodi2 option:selected").val();

    if (cbo == "") {
        alert('Por favor, seleccione el Periodo');
    }
    else {
        p = $("#Pericodi2 option:selected").val();
        vers = $("#VersionB option:selected").val();
        $.ajax({
            type: 'POST',
            url: controlador + "lista",
            data: { pericodi: p ,version:vers},
            success: function (evt) {
                $('#listado').html(evt);
                oTable = $('#tabla').dataTable({
                    "sPaginationType": "full_numbers",
                    "destroy": "true",
                    "aaSorting": [[2, "asc"]]
                });
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

uploadExcelDU = function () {
    var fullDate = new Date();
    var twoDigitMonth = ((fullDate.getMonth().length) == 1) ? '0' + (fullDate.getMonth() + 1) : (fullDate.getMonth() + 1);
    var sFecha = fullDate.getFullYear().toString() + twoDigitMonth.toString() + fullDate.getDate().toString() + fullDate.getHours().toString() + fullDate.getMinutes().toString() + fullDate.getSeconds().toString();
    var uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFile',
        container: document.getElementById('container'),
        url: controlador + 'upload?sFecha=' + sFecha,
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '100mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx" }
            ]
        },
        init: {
            PostInit: function () {
                document.getElementById('btnProcesarFile').onclick = function () {
                    if ($("#Pericodi").val() == "") {
                        mostrarMensaje("Por favor, seleccione un periodo");
                    }
                    else {
                        if (uploader.files.length > 0) {
                            uploader.start();
                        }
                        else
                            loadValidacionFile("Seleccione archivo.");
                    }
                    return false;
                };
            },
            FilesAdded: function (up, files) {
                if (uploader.files.length == 2) {
                    uploader.removeFile(uploader.files[0]);
                }
                plupload.each(files, function (file) {
                    loadInfoFile(file.name, plupload.formatSize(file.size));
                });
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarProgreso(file.percent);
            },
            UploadComplete: function (up, file) {
                procesarArchivo(sFecha + "_" + file[0].name, $("#Pericodi").val(), $("#Version").val());
            },
            Error: function (up, err) {
                loadValidacionFile(err.code + "-" + err.message);
            }
        }
    });
    uploader.init();
}

procesarArchivo = function (sFile, Pericodi, Version) {
    $.ajax({
        type: 'POST',
        url: controlador + 'procesararchivo',
        data: { sNombreArchivo: sFile, Pericodi: Pericodi, Version: Version },
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado == "1") {
                mostrarMensaje("Archivo procesado");
            }
            else {
                mostrarMensaje(resultado);
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

generarExcel = function () {
    if ($("#Pericodi2").val() == "") {
        alert("Por favor, seleccione un periodo");
    }
    else {
        var iPericodi = $("#Pericodi2").val();
        var iVersion = $("#VersionB").val();
        $.ajax({
            type: 'POST',
            url: controlador + 'generarexcel',
            data: { iPericodi: iPericodi, iVersion: iVersion },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    window.location = controlador + 'abrirexcel';
                }
                if (result == -1) {
                    alert("Lo sentimos, se ha producido un error");
                }
            },
            error: function () {
                alert("Lo sentimos, se ha producido un error");
            }
        });
    }
}

//ASSETEC 20190104
mostrarExito = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}

mostrarAlerta = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}

exportarMB = function (formato) {
    var pericodi = $("#Pericodi").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'exportarMB',
        data: { pericodi: pericodi },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                window.location = controlador + 'abrirarchivo?formato=' + formato + '&file=' + result;
                mostrarExito("Felicidades, el archivo se descargo correctamente...!");
            }
            else {
                mostrarError("Lo sentimos, ha ocurrido el siguiente error: " + model.sError);
            }
        },
        error: function () {
            mostrarError("Lo sentimos, ha ocurrido el siguiente error");
        }
    });
}

copiarMB = function () {
    var pericodi = $("#Pericodi").val();
    var version = $("#Version").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'copiarMB',
        data: { pericodi: pericodi, version: version },
        dataType: 'json',
        success: function (model) {
            if (model.sError == "") {
                mostrarExito("Felicidades, se copio correctamente " + model.iNumReg + " registros.");
            }
            else {
                mostrarError("Lo sentimos, ha ocurrido el siguiente error: " + model.sError);
            }
        },
        error: function () {
            mostrarError("Lo sentimos, ha ocurrido el siguiente error");
        }
    });
}