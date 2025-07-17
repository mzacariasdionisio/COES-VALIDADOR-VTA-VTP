var controlador = siteRoot + 'costooportunidad/despachodiario/';
var uploader;
var totNoNumero = 0;
var totLimSup = 0;
var totLimInf = 0;
var listErrores = [];
var listDescripErrores = ["BLANCO", "NÚMERO", "No es número", "Valor negativo", "Supera el valor máximo"];
var listValInf = [];
var listValSup = [];
var validaInicial = true;
var hot;
var hotOptions;
var evtHot;

$(function () {
    $('#txtFecha').Zebra_DatePicker({
    });

    $('#btnConsultar').click(function () {
        cargarExcelWeb();
    });

    $('#btnDescargarFormato').click(function () {
        if (validarSeleccionDatos()) descargarFormato();
        else alert("Por favor no cambie las opciones.");
    });

    $('#btnReprogramados').click(function () {
        popUpListaReprogramados();
    });

    $('#btnPorcentajeRpf').click(function () {
        popUpListaPorcentajeRpf();
    });

    crearPupload();
});

function cargarExcelWeb() {
    if ($("#hfFecha").val() != "") {
        $('#mensajeEvento').css("display", "none");
        $('#barraDespacho').css("display", "block");
        $("#hfIdEnvio").val(0);
        mostrarFormularioReserva(consulta);
    }
    else {
        alert("Error!.Ingresar fecha correcta");
    }
}

function mostrarFormularioReserva(accion) {
    listErrores = [];
    var idEnvio = $("#hfIdEnvio").val();
    $('#hfFormato').val($('#cbFormato').val());
    $('#hfFecha').val($('#txtFecha').val());
    if (typeof hot != 'undefined') {
        hot.destroy();
    }

    $.ajax({
        type: 'POST',
        url: controlador + "MostrarHojaExcelWeb",
        dataType: 'json',
        data: {
            idFormato: $('#hfFormato').val(),
            idEnvio: idEnvio,
            fecha: $('#hfFecha').val()
        },
        success: function (evt) {
            if (evt != -1) {
                crearHandsonTable(evt);
                evtHot = evt;
                switch (accion) {
                    case envioDatos:
                        var mensaje = mostrarMensajeEnvio(idEnvio);
                        mostrarExito("Los datos se enviaron correctamente.");
                        break;
                    case envioAnterior:
                        break;
                    case consulta:
                        mostrarExito("Informacion Exitosa!!");
                        break;
                    case importarDatos:
                        //ShowMensaje();
                        mostrarExito("<strong>Los datos se cargaron correctamente....</strong>");
                        break;
                }
            }
            else {
                alert("Ha ocurrido un error.");
            }
        },
        error: function () {
            alert("Error al cargar Excel Web");
        }
    });
}

function findIndiceMerge(col, lista) {
    for (key in lista) {
        if ((col >= lista[key].col) && (col < (lista[key].col + lista[key].colspan))) {
            return key;
        }
    }
    return -1;
}

function mostrarMensajeEnvio() {
    var envio = $("#hfIdEnvio").val();
    if (envio > 0) {
        var plazo = (evtHot.EnPlazo) ? "<strong style='color:green'>en plazo</strong>" : "<strong style='color:red'>fuera de plazo</strong>";
        var mensaje = "<strong>Código de envío</strong> : " + evtHot.IdEnvio + "   , <strong>Fecha de envío: </strong>" + evtHot.FechaEnvio + "   , <strong>Enviado en </strong>" + plazo;
        return mensaje;
    }
    else {
        if (!evtHot.EnPlazo) {
            return "<strong style='color:red'>Fuera de plazo</strong>";
        }
        else return "<strong style='color:green'>En plazo</strong>";
    }
}

function enviarExcelWeb() {
    if (confirm("¿Desea enviar información a COES?")) {
        var fecha = $('#hfFecha').val();
        $.ajax({
            type: 'POST',
            dataType: 'json',
            //async: false,
            contentType: 'application/json',
            traditional: true,
            url: controlador + "GrabarExcelWeb",
            data: JSON.stringify({
                data: hot.getData(),
                idFormato: $("#hfFormato").val(),
                fecha: fecha
            }),
            beforeSend: function () {
                mostrarExito("Enviando Información ..");
            },
            success: function (evt) {
                if (evt.Resultado == 1) {
                    $("#hfIdEnvio").val(evt.IdEnvio);
                    mostrarFormularioReserva(2);

                    mostrarMensaje("Los datos se enviaron correctamente");
                }
                else {
                    mostrarError("Error al Grabar:" + evt.Mensaje);
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

function crearPupload() {
    var msjOpc = "";
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectExcel',
        url: siteRoot + 'CostoOportunidad/DespachoDiario/Upload',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '20mb',
            mime_types: [
                { title: "Archivos ZIP .zip", extensions: "zip" },
                { title: "Archivos RAR .rar", extensions: "rar" }
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
                if (confirm("Al cargar archivo ZIP. Automaticamente se envia la informacion a COES¿Desea Continuar?")) {
                    mostrarAlerta("Subida completada <strong>Por favor espere</strong>");
                    var retorno = leerFileUp();
                    if (retorno == 1 || retorno == 2 || retorno == 3 || retorno == 4) {
                        if (retorno == 2) {
                            mostrarError("Error: El Archivo .zip contiene archivos con Reserva");
                        } else {
                            if (retorno == 3) {
                                mostrarError("Error: El Archivo .zip no contiene archivos con Reserva");
                            } else {
                                if (retorno == 4) {
                                    msjOpc = "El Archivo .zip no contiene la carpeta con reprogramas";
                                }
                                $("#hfIdEnvio").val(-1);//-1 indica que el handsonetable mostrara datos del archivo excel 
                                mostrarFormularioReserva(importarDatos);
                                //mostrarExito(msjOpc + "<strong>Los datos se cargaron correctamente...</strong>");
                            }
                        }
                    }
                    else {
                        mostrarError("Error: Formato descnocido.");
                    }
                }
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploader.init();
}

function leerFileUp() {
    var retorno = 0;
    var fecha = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'LeerFileUpArchivo',
        dataType: 'json',
        async: false,
        data: {
            idFormato: $('#cbFormato').val(),
            fecha: fecha
        },
        success: function (evt) {
            retorno = evt.Retorno;
            ListaPtos = evt.ListaHojaPto;
        },
        error: function () {
            mostrarError("Ha ocurrido un error");
        }
    });
    return retorno;
}

function validarSeleccionDatos() {
    if (!($('#hfFormato').val() == $('#cbFormato').val() && $('#txtFecha').val() == $('#hfFecha').val())) {
        return false;
    }
    return true;
}

function mostrarExito(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}

function mostrarError(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}

function mostrarAlerta(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}

function mostrarMensaje(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
}

function mostrarMensajeEnvio() {
    var envio = $("#hfIdEnvio").val();
    if (envio > 0) {
        var plazo = (evtHot.EnPlazo) ? "<strong style='color:green'>en plazo</strong>" : "<strong style='color:red'>fuera de plazo</strong>";
        var mensaje = "<strong>Código de envío</strong> : " + evtHot.IdEnvio + "   , <strong>Fecha de envío: </strong>" + evtHot.FechaEnvio; "" + "   , <strong>Enviado en </strong>" + plazo;
        return mensaje;
    }
}