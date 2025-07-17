var controlador = siteRoot + 'CostoOportunidad/FactorUtilizacion/';

$(function () {
    $('#txtFecIni').Zebra_DatePicker({
        format: 'd/m/Y',        
        pair: $('#txtFecFin'),
        direction: -1
    });

    $('#txtFecFin').Zebra_DatePicker({
        format: 'd/m/Y',        
        pair: $('#txtFecIni'),
        direction: [true, '31/12/2050']
    });

    $('#txtFecProceso').Zebra_DatePicker({
        format: 'd/m/Y', 
        direction: [false, diaActual()]        
    });
    
    cargarListadoFactoresUtilizacion();
    
    $("#btnProcesoDiario").click(function () {
        limpiarBarraMensaje("mensaje_popup");
        $('#txtFecProceso').val($('#hfFechaDefecto').val());
        abrirPopup("popupProcesoManual");
    });

    ////Bloqueado temporalmente
    //$("#btnReprocesarTodo").click(function () {
    //    reprocesarTodos();
    //});

    $("#btnConsultar").click(function () {
        cargarListadoFactoresUtilizacion();
    });

    $("#btnProcesarManual").click(function () {
        procesarManualmente();
    });

    $("#btnReemplazarValores").click(function () {
        limpiarBarraMensaje("mensaje");
        limpiarBarraMensaje("mensaje_popupRV");
        $("#txtNombreArchivo").hide();
        $("#txtNombreArchivo").html('');
        $('#listadoErroresExcel').html('');
        $('#btnProcesarReemplazo').css('display', 'none');
        abrirPopup("popupReemplazarValores");
    });

    $('#btnProcesarReemplazo').click(function () {
        uploader.start();
    });

    AdjuntarArchivo();
});

/** Carga lista de factores **/
function cargarListadoFactoresUtilizacion() {

    limpiarBarraMensaje("mensaje");
    var obj = obtenerObjetoRango();

    $.ajax({
        type: 'POST',
        url: controlador + "ListarFactoresUtilizacion",
        data: {
            fecIni: obj.fecIni,
            fecFin: obj.fecFin
        },
        success: function (evt) {
            
            if (evt.Resultado != "-1") {

                ////Bloqueado temporalmente 
                //if (evt.MostrarBtnRT)
                //    $("#btnReprocesarTodo").css("display", "block");
                //else
                //    $("#btnReprocesarTodo").css("display", "none");

                $('#listadoDeFactoresU').html(evt.Resultado);
                refrehDatatable();

            } else {
                alert('Ha ocurrido un error: ' + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error: ' + err);
        }
    });
}

function refrehDatatable() {
    $('#tabla_lstFactores').dataTable({
        "scrollY": 430,
        "scrollX": true,
        "sDom": 't',
        "ordering": false,
        "iDisplayLength": -1
    });
}


function reprocesarCalculo(prodiacodi) {

    limpiarBarraMensaje("mensaje");
    if (confirm("¿Esta seguro que desea reprocesar la ejecución para el día seleccionado?")) {

        $.ajax({
            type: 'POST',
            url: controlador + "ReprocesarCalculo",
            data: {
                prodiacodi: prodiacodi
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    cargarListadoFactoresUtilizacion();
                } else {
                    mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error: ' + evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error: ' + evt.Mensaje);
            }
        });
    } 
}

function procesarManualmente() { 
    var fechaProceso = $("#txtFecProceso").val();
    limpiarBarraMensaje("mensaje");
    limpiarBarraMensaje("mensaje_popup");
    if (fechaProceso != "") {
        if (confirm("¿Esta seguro que desea procesar la ejecución para la fecha seleccionada?” ")) {

            $.ajax({
                type: 'POST',
                url: controlador + "ReprocesarManualmente",
                data: {
                    fecha: fechaProceso
                },
                success: function (evt) {
                    if (evt.Resultado == "1") {
                        $("#txtFecIni").val(fechaProceso);
                        $("#txtFecFin").val(fechaProceso);
                        cerrarPopup('#popupProcesoManual');
                        cargarListadoFactoresUtilizacion();
                    } else {
                        if (evt.Resultado == "2") {
                            cerrarPopup('#popupProcesoManual');
                            mostrarMensaje_('mensaje', 'alert', 'Hay una ejecución en curso, por favor espere unos minutos para volver intentarlo.');
                        } else {
                            if (evt.Resultado == "-2") {
                                mostrarMensaje_('mensaje_popup', 'error', evt.Mensaje);
                            } else {
                                cerrarPopup('#popupProcesoManual');
                                mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error: ' + evt.Mensaje);
                            }
                        }                        
                    }
                },
                error: function (err) {
                    cerrarPopup('#popupProcesoManual');
                    mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error: ' + evt.Mensaje);
                }
            });
        }
    }
    else {
        mostrarMensaje_('mensaje', 'error', 'Debe seleccionar una fecha correcta.');
    }
}


function reprocesarTodos() {
    limpiarBarraMensaje("mensaje");    
    var obj = obtenerObjetoRango();

    if (confirm("¿Esta seguro que desea reprocesar el cálculo para todos los dias no ejecutados?")) {

        $.ajax({
            type: 'POST',
            url: controlador + "ReprocesarTodos",
            data: {   
                fecIni: obj.fecIni,
                fecFin: obj.fecFin
            },
            success: function (evt) {                
                if (evt.Resultado != "-1") {
                    cargarListadoFactoresUtilizacion();
                } else {
                    mostrarMensaje_('mensaje', 'error','Ha ocurrido un error: ' + evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error: ' + evt.Mensaje);
            }
        });
    } else {
        mostrarMensaje_('mensaje', 'error',"mensaje error");
    }
}

async function mostrarReporteError(prodiacodi) {
    
    limpiarBarraMensaje("mensaje");
    $.ajax({
        type: 'POST',
        url: controlador + "MostrarReporteError",
        data: {
            prodiacodi: prodiacodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                abrirPopup("erroresDatos");

                $("#listadoErrores").html(evt.HtmlErrores);
                refrehDatatable2();
            } else {
                mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error.' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function refrehDatatable2() {    
    $('#tblListaerrores').dataTable({        
        "filter": false,
        "scrollCollapse": true,
        "paging": false,
        "scrollY": 330,
    });
}

function descargarResultados(prodiacodi) {
    limpiarBarraMensaje("mensaje");
    $.ajax({
        type: 'POST',
        url: controlador + "GenerarReporteResultados",
        data: {
            prodiacodi: prodiacodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "Exportar?file_name=" + evt.Resultado;
            } else {
                mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error.' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function obtenerObjetoRango() {
    var filtro = {};

    filtro.fecIni = $("#txtFecIni").val();
    filtro.fecFin = $("#txtFecFin").val();

    return filtro;
}

function cerrarPopup(id) {
    $(id).bPopup().close()
}

function abrirPopup(contentPopup) {
    setTimeout(function () {
        $("#" + contentPopup).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}

//Funciones usadas 
function AddZero(num) {
    return (num >= 0 && num < 10) ? "0" + num : num + "";
}

function diaActual() { //devuelve strFecha en formato dd/mm/yyyy
    var now = new Date();
    var strDateTime = [[AddZero(now.getDate()), AddZero(now.getMonth() + 1), now.getFullYear()].join("/")].join(" ");

    return strDateTime;
}


function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

function AdjuntarArchivo() {

    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: "btnSeleccionarArchivo",

        url: controlador + "ProcesarReemplazo",
        multi_selection: false,
        filters: {
            max_file_size: '2mb',
            mime_types: [
                { title: "Archivos xls", extensions: "xls,xlsx" },
            ]
        },
        multipart_params: { },
        init: {
            FilesAdded: function (up, files) {
                if (uploader.files.length === 2) {
                    uploader.removeFile(uploader.files[0]);
                }

                $("#txtNombreArchivo").css('display', 'inline-block');
                for (i = 0; i < files.length; i++) {
                    var file = files[i];
                    $("#txtNombreArchivo").html(file.name);
                }
                
                $('#btnProcesarReemplazo').css('display', 'block');
                $('#listadoErroresExcel').html('');
                limpiarBarraMensaje("mensaje");
                limpiarBarraMensaje("mensaje_popupRV");
            },
            UploadProgress: function (up, file) {
                mostrarMensaje_('mensaje_popupRV', 'alert', "Procesando Archivo, por favor espere ...(<strong>" + (file.percent - 1) + "%</strong>)");                
                $('#listadoErroresExcel').html('');
                
                $('#btnProcesarReemplazo').css('display', 'none');
            },

            FileUploaded: function (up, file, info) {                
                var aData = JSON.parse(info.response);
                if (aData.Resultado != '-1') {                
                    mostrarListaErrores(aData);
                    
                    if (aData.Resultado != 1) {
                        mostrarMensaje_('mensaje_popupRV', 'alert', "El archivo seleccionado tuvo los siguientes errores.");
                    } else {
                        mostrarMensaje_('mensaje', 'exito', "El reemplazo de valores fue realizado exitosamente.");
                        cerrarPopup('#popupReemplazarValores');
                    }
                    
                } else {                
                    mostrarMensaje_('mensaje_popupRV', 'alert', aData.Mensaje);                
                }                
                
                $('#btnProcesarReemplazo').css('display', 'none');
            },
            UploadComplete: function (up, file) {
                var sdfs = 0;
            },
            Error: function (up, err) {
                if (err.message == "File extension error.") {
                    mostrarMensaje_('mensaje_popupRV', 'alert', "El archivo seleccionado no tiene extension .xls o .xlsx.");
                } else {
                    mostrarMensaje_('mensaje_popupRV', 'alert', err.message);
                }
                $('#btnProcesarReemplazo').css('display', 'none');

            }
        }
    });

    uploader.init();
}

function mostrarListaErrores(aData) {
    $('#listadoErroresExcel').html('');

    //Tabla de errores
    if (aData.Resultado != '1') {
        $('#listadoErroresExcel').html(aData.Resultado);        
    }
}

function mostrarMensaje_(id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

}

function limpiarBarraMensaje(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
}