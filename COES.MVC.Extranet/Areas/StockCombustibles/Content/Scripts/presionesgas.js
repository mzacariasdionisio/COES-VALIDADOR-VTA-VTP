var controlador = siteRoot + 'StockCombustibles/envio/'
var fechaTmp = '';
$(function () {
    idFuenteDatos = 0;
    idFormato = idFormatoPresionGasTemp;
    tipoFormato = 0;
    dibujarPanelIeod(tipoFormato, 2, -1);

    $('#txtFecha').Zebra_DatePicker({
        onSelect: function () {
            $("#hfIdEnvio").val('0');
            limpiarBarra();
            fechaTmp = '';
        }
    });

    $('#btnConsultar').click(function () {
        mostrarExcelWeb();
    });
    $('#cbEmpresa').change(function () {
        $("#hfIdEnvio").val('0');
        fechaTmp = '';
        limpiarBarra();
        dibujarPanelIeod(tipoFormato, 2, -1);
    });
    $('#txtFecha').click(function () {
        limpiarBarra();
    });

    $('#btnEnviarDatos').click(function () {
        if (evtHot.Handson.ReadOnly) {
            alert("No se puede enviar información, solo diponible para  visualización");
            return
        }
        else {
            enviarExcelWeb();
        }
    });

    $('#btnDescargarFormato').click(function () {
        if (validarSeleccionDatos()) {
            descargarFormato();
        }
        else {
            alert("Por favor seleccione la empresa correcta.");
        }
    });

    $('#btnMostrarErrores').click(function () {
        mostrarDetalleErrores();
    });

    $('#btnVerEnvios').click(function () {
        popUpListaEnvios();
    });
    $('#btnEditarEnvio').click(function () {
        $("#hfIdEnvio").val('0');
        fechaTmp = 'cambio';
        deshabilitarEdicionEnvio();
        mostrarFormularioPresion();
    });
    limpiarBarra();
    crearPupload();
});

function limpiarBarra() {
    $("#btnEnviarDatos").parent().hide();
    $("#btnMostrarErrores").parent().hide();
    $("#btnAgregarFila").parent().parent().hide();
    $('#barraPresion').css("display", "none");
    $("#mensajeEvento").hide();
    $("#mensaje").html("Por favor seleccione la empresa y la fecha.");
    $("#mensaje").show();
    $('#detalleFormato').html("");
    deshabilitarEdicionEnvio();
}
//Muestra la barra de herramemntas para administrar los datos de stock de combustible ingresados
function mostrarExcelWeb() {
    $("#btnEnviarDatos").parent().hide();
    $("#btnMostrarErrores").parent().hide();
    $("#btnAgregarFila").parent().parent().hide();
    deshabilitarEdicionEnvio();
    if ($("#txtFecha").val() != "") {
        $('#mensajeEvento').css("display", "none");
        $('#barraPresion').css("display", "block");
        $("#hfIdEnvio").val(0);
        mostrarFormularioPresion(consulta);
    }
    else {
        alert("Error!.Ingresar fecha correcta");
    }
}

function mostrarEnvioExcelWeb(envio) {
    $('#enviosanteriores').bPopup().close();
    $("#hfIdEnvio").val(envio);
    mostrarFormularioPresion(envioAnterior);
}

function mostrarFormularioPresion(accion) {
    listErrores = [];
    var idEmpresa = $("#cbEmpresa").val();
    var fecha = $("#txtFecha").val();
    if (fechaTmp == '') {
        accion = envioAnterior;
        $("#hfIdEnvio").val('-1')
    }
    var idEnvio = $("#hfIdEnvio").val();
    $('#hfEmpresa').val($('#cbEmpresa').val());
    $('#hfFecha').val($('#txtFecha').val());
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    $.ajax({
        type: 'POST',
        url: controlador + "MostrarPresionGas",
        dataType: 'json',
        data: {
            idEmpresa: idEmpresa,
            fecha: fecha,
            idEnvio: idEnvio
        },
        success: function (evt) {
            if (evt != -1) {
                evtHot = evt;
                if (!evtHot.Handson.ReadOnly) {
                    $("#btnEnviarDatos").parent().show();
                    $("#btnMostrarErrores").parent().show();
                    $("#btnAgregarFila").parent().parent().show();
                }

                crearGrillaExcelPresion(evt);
                switch (accion) {
                    case envioDatos:
                        var mensaje = mostrarMensajeEnvio(idEnvio);
                        mostrarExito("Los datos se enviaron correctamente. " + mensaje);
                        hideMensaje();
                        habilitarEdicionEnvio();
                        break;
                    case envioAnterior:
                        if (fechaTmp == '') {
                            fechaTmp = 'cambio';
                            idEnvio = evt.IdEnvioLast;
                            $("#hfIdEnvio").val(idEnvio);
                        }
                        if (idEnvio <= 0) {
                            var mensaje = mostrarMensajeEnvio();
                            mostrarMensaje(mensaje);
                            $('#divAcciones').css('display', 'block');
                        } else {
                            var mensaje = mostrarMensajeEnvio(idEnvio);
                            mostrarMensaje(mensaje);
                        }
                        if (idEnvio > 0) {
                            habilitarEdicionEnvio();
                        }
                        break;
                    case consulta:
                        var mensaje = mostrarMensajeEnvio();
                        mostrarMensaje(mensaje);
                        deshabilitarEdicionEnvio();
                        break;
                    case importarDatos:
                        mostrarExito("<strong>Los datos se cargaron correctamente, por favor presione el botón enviar para grabar.</strong>");
                        break;
                    default:
                        var mensaje = mostrarMensajeEnvio();
                        mostrarMensaje(mensaje);
                        $('#divAcciones').css('display', 'block');
                        deshabilitarEdicionEnvio();
                        break;
                }
                if (evtHot.Handson.ReadOnly) {
                    $('#btnSelectExcel').css("display", "none");
                }
                else {
                    $('#btnSelectExcel').css("display", "block");
                }

            }
            else {
                alert("La empresa no tiene puntos de medición para cargar.");
                document.location.href = controlador + 'presiongas';
            }

            dibujarPanelIeod(tipoFormato, 2, -1);
        },
        error: function () {
            alert("Error al cargar Excel Web");
        }
    });
}

function enviarExcelWeb() {
    if (confirm("¿Desea enviar información a COES?")) {
        //var $container = $('#detalleFormato');
        if (validarEnvio()) {
            //$('#hfDataExcel').val((hot.getData()));
            //$('#hfIdFormato').val(50);
            var empresa = $('#cbEmpresa').val();
            var fecha = $('#txtFecha').val();
            $('#hfEmpresa').val(empresa);
            $.ajax({
                type: 'POST',
                dataType: 'json',
                //async: false,
                contentType: 'application/json',
                traditional: true,
                url: controlador + "GrabarExcelWebPresion",
                data: JSON.stringify({
                    data: hot.getData(),
                    idEmpresa: empresa,
                    fecha: fecha
                }),


                beforeSend: function () {
                    mostrarExito("Enviando Información ..");
                },
                success: function (evt) {
                    if (evt.Resultado == 1) {
                        $("#hfIdEnvio").val(evt.IdEnvio);

                        $("#btnEnviarDatos").parent().hide();
                        $("#btnMostrarErrores").parent().hide();
                        $("#btnAgregarFila").parent().parent().hide();
                        mostrarFormularioPresion(envioDatos);
                        mostrarExito("Los datos se enviaron correctamente");
                        fechaTmp = '';
                    }
                    else {
                        mostrarError("Error al Grabar");
                    }
                },
                error: function () {
                    mostrarError();
                }

            });

        }

    }

}

function validarEnvio() {
    retorno = true;
    totalErrores = listErrores.length;
    getTotalErrores();
    //valida si existen errores
    if ((totalErrores) > 0) {
        mostrarError("Existen errores en las celdas, favor corregir y vuelva a envíar");
        mostrarDetalleErrores();
        return false;
    }
    return retorno;

}

function descargarFormato() {
    $.ajax({
        type: 'POST',
        url: controlador + 'generarformatopresion',
        async: false,
        contentType: 'application/json',
        data: JSON.stringify({
            data: hot.getData(),
            idEmpresa: $('#hfEmpresa').val(),
            dia: $('#hfFecha').val()
        }),
        success: function (result) {
            if (result.length > 0) {
                window.location.href = controlador + 'descargarformato?archivo=' + result;
            }
            else {
                alert("Error en descargar el archivo");
            }
        },
        error: function (result) {
            alert('ha ocurrido un error al descargar el archivo excel. ' + result.status + ' - ' + result.statusText + '.');
        }
    });
}

function crearPupload() {
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectExcel',
        url: siteRoot + 'StockCombustibles/envio/Upload',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '5mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx,xls" }
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
            FileUploaded: function (up, file, info) {
                console.log('a');
            },
            UploadProgress: function (up, file) {
                showMensaje();
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
                hideMensajeEvento();
            },
            UploadComplete: function (up, file) {
                hideMensaje();
                var retorno = leerFileUpExcel();
                if (retorno == 1) {
                    limpiarError();
                    $("#hfIdEnvio").val(-2);//-1 indica que el handsontable mostrara datos del archivo excel                    
                    mostrarFormularioPresion(importarDatos);

                }
                else {
                    mostrarError("Error: Formato desconocido.");
                }

            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploader.init();
}


