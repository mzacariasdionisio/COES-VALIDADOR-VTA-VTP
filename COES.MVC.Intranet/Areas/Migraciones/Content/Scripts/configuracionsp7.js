var controlador = siteRoot + 'Migraciones/LectorConfigsp7/'
var CANTIDAD_CLICK_IMPORTAR = 0;

$(function () {
    $("#btnProcesar1").click(function () {
        sincronizarsp7coes();
    });

    btnSelectExcel("2");
    btnSelectExcel("3");
    btnSelectExcel("4");
});

//////////////////////////////////////////////////////////
//// btnSelectExcel
//////////////////////////////////////////////////////////
function btnSelectExcel(numHoja) {
    var url = controlador + 'Upload';
    crearPupload(numHoja, url);
}

function sincronizarsp7coes() {
    if (confirm("Desea sincronizar la Base de Datos SP7 - COES?", "Sincronizar...")) {
        $.ajax({
            type: 'POST',
            url: controlador + "SincronizarSp7Coes",
            dataType: 'json',
            data: {},
            success: function (evt) {
                if (evt.NroRegistros > 0) {
                    $("#alertError").hide();
                    $("#txtUrl4").val(evt.Resultado);
                    $("#btnProcesar2").show();
                    $("#btnProcesar3").show();
                    $("#btnProcesar4").show();
                } else if (evt.NroRegistros == 0) {
                    $("#alertErrorMensaje").html(evt.Mensaje);
                    $("#alertErrorDetalle").html(evt.Detalle);
                    $("#alertError").show();
                    $("#btnProcesar2").show();
                    $("#btnProcesar3").show();
                    $("#btnProcesar4").show();
                    $("#txtUrl4").val("");              
                } else {
                    $("#alertErrorMensaje").html(evt.Mensaje);
                    $("#alertErrorDetalle").html(evt.Detalle);
                    $("#alertError").show();
                    $("#btnProcesar2").hide();
                    $("#btnProcesar3").hide();
                    $("#btnProcesar4").hide();
                    $("#txtUrl4").val("");       
                }
            },
            error: function (err) { alert("Error..!!"); }
        });
    }
}

function crearPupload(x, y) {
    var msjOpc = "", msjCarga = "";
    var uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: "btnProcesar" + x,
        url: y,
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: 0,
            mime_types: [
                { title: "Archivos XML .xml", extensions: "xml" },
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
                var valor = parseInt(x) - 1;
                switch (valor) {
                    case 1: msjOpc = "Área de Responsabilidad"; msjCarga = "AoR"; break;
                    case 2: msjOpc = "ICCP"; msjCarga = "ICCP"; break;
                    case 3: msjOpc = "CIM"; msjCarga = "AoR"; break;
                }
                if (confirm("Desea sincronizar " + msjOpc + " - COES?")) {
                    mostrarAlerta("Subida completada <strong>Por favor espere</strong>");
                    var rt = leerFileUp(valor);
                    var ret = rt.Resultado.split(',');
                    var msj_ = "";
                    if (parseInt(ret[0]) > 0) {
                        msj_ = "Última actualización al " + ret[1] + "(" + msjCarga + ": " + ret[0] + " registros)";
                        mostrarExito("Última actualización al " + ret[1] + "<strong> " + msjCarga + ": " + ret[0] + " registros</strong>");
                    } else {
                        msj_ = "Proceso culminado correctamente.";
                        mostrarExito(msj_);
                    }
                    mostrarExito(msj_);
                    $("#txtUrl" + valor).val(msj_);
                    if (valor == 3) {
                        var mm = "";
                        for (var i = 0; i < rt.ListaString.length; i++) {

                        }
                        $("#txtUrl" + valor + 2).val(mm);
                        $("#txtUrl" + valor + 2).show();
                    } else { $("#txtUrl5").hide(); }
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

function leerFileUp(x) {
    var retorno;

    $.ajax({
        type: 'POST',
        url: controlador + 'LeerFileUpArchivo',
        dataType: 'json',
        async: false,
        data: { btn: x },
        success: function (evt) {
            retorno = evt;
        },
        error: function () {
            mostrarError("Ha ocurrido un error");
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
