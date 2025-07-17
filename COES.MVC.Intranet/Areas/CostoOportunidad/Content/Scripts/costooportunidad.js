var controlador = siteRoot + 'costooportunidad/costooportunidad/';
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
    $('#tab-container').easytabs({ animate: false });
    $('#inner-container').easytabs({ animate: false });
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

    $('#btnEnviarDatos').click(function () {
        enviarExcelWebParametros();
    });

    $('#btnExpotar').click(function () {
        exportarExcelReporte();
    });

    crearPupload();
});

function enviarExcelWebParametros() {
    if (confirm("¿Desea enviar información a COES?")) {
        envioData();
    }
}

function envioData() {
    var fecha = $("#txtFecha").val();
    var idEnvio = $("#hfIdEnvio").val();
    $.ajax({
        type: 'POST',
        dataType: 'json',
        //async: false,
        contentType: 'application/json',
        traditional: true,
        url: controlador + "GrabarExcelWebParametros",
        data: JSON.stringify({
            data: hot.getData()
            , fecha: fecha
            //,listaPtos: ListaPtos
        }),

        beforeSend: function () {
            mostrarExito("Enviando Información ..");
        },
        success: function (evt) {
            if (evt.Resultado == 1) {
                $("#hfIdEnvio").val(evt.IdEnvio);
                //mostrarFormulario(envioDatos);
                mostrarExito("Los datos se enviaron correctamente");
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

function cargarTabs(val) {
    $('#divCargaDatos').html("<div class='bodyexcel' id='detalleFormato'></div>");
    if (val > 0) {
        $("#tab_2").show();
        $("#tab_3").show();

        mostrarFormularioCostos(importarDatos);
        mostrarListado();

        $('#divParametros').html("<div class='bodyexcel' id='detalleFormatoParametros'></div>");
        $('#divReporte').html("");
    } else {
        $('#tabss a[href="#carga"]').click();
        $("#tab_2").hide();
        $("#tab_3").hide();
    }
}

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
                cargarTabs(evt.IdEnvio);
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
                        $('#mensaje').removeClass();
                        $('#mensaje').html("Informacion Exitosa!!");
                        $('#mensaje').addClass('action-exito');
                        break;
                    case importarDatos:
                        $('#mensaje').removeClass();
                        $('#mensaje').html("<strong>Los datos se cargaron correctamente....</strong>");
                        $('#mensaje').addClass('action-exito');
                        //mostrarExito("<strong>Los datos se cargaron correctamente....</strong>");
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

function mostrarFormularioCostos(accion) {
    //listErrores = [];    
    var fecha = $("#txtFecha").val();
    var idEnvio = 1;
    //$('#hfFecha').val($('#txtFecha').val());       
    //var tipoCentral = $('#cbTipoCentral').val();     

    /*if (typeof hot != 'undefined') {
        hot.destroy();
    }*/
    $.ajax({
        type: 'POST',
        url: controlador + "MostrarCostosOportunidad",
        dataType: 'json',
        data: {
            fecha: fecha,
            idEnvio: idEnvio,
        },
        success: function (evt) {
            if (evt != -1) {
                crearGrillaExcelCostos(evt);
                evtHot = evt;
                /*switch (accion) {
                    case envioDatos:
                        var mensaje = mostrarMensajeEnvio(idEnvio);
                        mostrarExito("Los datos se grabaron correctamente. " + mensaje);
                        hideMensaje();
                        break;
                    case importarDatos:
                        mostrarExito("<strong>Los datos se cargaron correctamente, por favor presione el botón guardar para grabar.</strong>");
                        break;
                }*/
                if (evtHot.Handson.ReadOnly) {
                    $('#btnSelectExcel').css("display", "none");
                }
                else {
                    $('#btnSelectExcel').css("display", "block");
                }

                if (evt.Handson.ListaExcelData.length > 1) $('#barraCostos').css("display", "block");
                else $('#barraCostos').css("display", "none");
            }
            else {
                alert("La empresa no tiene puntos de medición para cargar.");
                document.location.href = controlador + 'index';
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
        url: siteRoot + 'CostoOportunidad/CostoOportunidad/Upload',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '30mb',
            mime_types: [
                { title: "Archivos ZIP .zip", extensions: "zip" },
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
                    
                    if (retorno.split(',')[0] == '10') {
                        $('#mensaje').removeClass();
                        $('#mensaje').html("El Archivo .zip contiene archivo(s) incompleto(s), " + retorno.split(',')[1]);
                        $('#mensaje').addClass('action-alert');
                    } else {
                        if (retorno.split(',')[0] == '1') {
                            $("#hfIdEnvio").val(-1);//-1 indica que el handsonetable mostrara datos del archivo excel 
                            mostrarFormularioReserva(importarDatos);
                        }
                        else {
                            mostrarError("Error: Formato desconocido.");
                        }
                    }
                }
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
    var retorno = "";
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
            retorno = evt.Resultado;
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

//FUNCIONES  - TAB 03 (REPORTE)
function mostrarListado() {
    $.ajax({
        type: 'POST',
        url: controlador + "ListaReporte",
        data: {
            fecha: $('#hfFecha').val()
        },
        success: function (evt) {
            $('#divDespachoConReserva').html(evt);
            if ($('#tabla th').length > 1) {
                $('#tabla').dataTable({
                    "bAutoWidth": false,
                    "bSort": false,
                    "scrollY": 800,
                    "scrollX": 1200,
                    "sDom": 't',
                    "iDisplayLength": 100
                });
            }
            mostrarDespachoSinReserva();
        },
        error: function (evt) {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarDespachoSinReserva() {
    $.ajax({
        type: 'POST',
        url: controlador + "ListaDespachoSinReserva",
        data: {
            fecha: $('#hfFecha').val()
        },
        success: function (evt) {
            $('#divDespachoSinReserva').html(evt);
            if ($('#tablaSin th').length > 1) {
                $('#tablaSin').dataTable({
                    "bAutoWidth": false,
                    "bSort": false,
                    "scrollY": 800,
                    "scrollX": 1200,
                    "sDom": 't',
                    "iDisplayLength": 100
                });
            }
            mostrarReservaEjec();
        },
        error: function (evt) {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarReservaEjec() {
    $.ajax({
        type: 'POST',
        url: controlador + "ReporteReservaEject",
        data: {
            fecha: $('#hfFecha').val()
        },
        success: function (evt) {
            $('#divReservaEjecutada').html(evt);
            if ($('#tabla2 th').length > 1) {
                $('#tabla2').dataTable({
                    "bAutoWidth": false,
                    "bSort": false,
                    "scrollY": 800,
                    "scrollX": 1200,
                    "sDom": 't',
                    "iDisplayLength": 100
                });
            }
            mostrarReservaProg();
        },
        error: function (evt) {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarReservaProg() {
    $.ajax({
        type: 'POST',
        url: controlador + "ReporteReservaProg",
        data: {
            fecha: $('#hfFecha').val()
        },
        success: function (evt) {
            $('#divReservaProgramada').html(evt);
            if ($('#tabla3 th').length > 1) {
                $('#tabla3').dataTable({
                    "bAutoWidth": false,
                    "bSort": false,
                    "scrollY": 800,
                    "scrollX": 1200,
                    "sDom": 't',
                    "iDisplayLength": 100
                });
            }
            mostrarCOConReserva();
        },
        error: function (evt) {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarCOConReserva() {
    $.ajax({
        type: 'POST',
        url: controlador + "ReporteCOConReserva",
        data: {
            fecha: $('#hfFecha').val()
        },
        success: function (evt) {
            $('#divDespachoCOConReserva').html(evt);
            if ($('#tabla4 th').length > 1) {
                $('#tabla4').dataTable({
                    "bAutoWidth": false,
                    "bSort": false,
                    "scrollY": 800,
                    "scrollX": 1200,
                    "sDom": 't',
                    "iDisplayLength": 100
                });
            }
            mostrarCOSinReserva()
        },
        error: function (evt) {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarCOSinReserva() {
    $.ajax({
        type: 'POST',
        url: controlador + "ReporteCOSinReserva",
        data: {
            fecha: $('#hfFecha').val()
        },
        success: function (evt) {
            $('#divDespachoCOSinReserva').html(evt);
            if ($('#tabla5 th').length > 1) {
                $('#tabla5').dataTable({
                    "bAutoWidth": false,
                    "bSort": false,
                    "scrollY": 800,
                    "scrollX": 1200,
                    "sDom": 't',
                    "iDisplayLength": 100
                });
            }
        },
        error: function (evt) {
            alert("Ha ocurrido un error");
        }
    });
}

function exportarExcelReporte() {

    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarReporteXls',
        data: {
            fecha: $('#hfFecha').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {// si hay elementos
                window.location = controlador + "ExportarReporte";
            }
            if (result == 2) { // sino hay elementos
                alert("No existen registros !");
            }
            if (result == -1) {
                alert("Error en reporte result")
            }
        },
        error: function () {
            alert("Error en reporte");;
        }
    });
}