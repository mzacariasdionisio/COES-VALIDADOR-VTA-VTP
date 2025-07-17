var controlador = siteRoot + "rechazocarga/programa/";

$( /**
   * Llamadas iniciales
   * @returns {} 
   */
   $(document).ready(function () {
       //mostrarMensaje("Selecionar el periodo y la versión");      

       $('#btnConsultar').click(function () {
           pintarBusqueda();
       });

       $('#btnGuadarEdicion').click(function () {
           guardarEdicion();
       });

       $('#btnCancelarEdicion').click(function () {
           cancelarEdicion();
       });

       $('#btnAgregar').click(function () {
           nuevoRegistro();
       });

       $("#horizonteEdit").change(function () {
           muestraCodigoReprogramaRef();
       });

       $('#fechaMensual').Zebra_DatePicker({
           direction: 30,
           format: 'Y-m',
           onSelect: function (date) {
               $('#fechaMensual').val(date);

           }
       });
       $('#fechaDiaria').Zebra_DatePicker({
           direction: true,
           format: 'd/m/Y',
           onSelect: function (date) {
               $('#fechaDiaria').val(date);

           }
       });

       $("#horizonteDuplicar").change(function () {
           habilitarHorizonte();
       });

       $('#btnCerrarCopia').click(function () {
           $("#popupDuplicarPrograma").bPopup().close();
       });

       $('#btnGenerarCopia').click(function () {
           generarCopia();
       });

       $('#btnExportar').click(function () {
           exportarExcel();
       });

   }));

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

var pintarBusqueda =
    /**
    * Pinta el listado de periodos según el año seleccionado
    * @returns {} 
    */
    function () {

        $.ajax({
            type: "POST",
            url: controlador + "listarPrograma",
            data: {
                horizonte: $("#horizonte").val(),
                codigoPrograma: $("#codigoPrograma").val(),
                estadoPrograma: $("#estado").val(),
                verReprograma: $("#hdnVerReprograma").val()
            },
            success: function (evt) {               

                $('#listado').html("");
                $('#listado').css("width", $('#mainLayout').width() + "px");
                $('#listado').html(evt);
                $('#tabla').dataTable({                                    
                    "destroy": "true",
                    "sPaginationType": "full_numbers",
                    "ordering": false,
                    "searching": true,
                    "paging": true,
                    "lengthMenu": [
                        [25, 50, -1],
                        [25, 50, "Todos"]
                    ],
                    "info": true,
                    "iDisplayLength": 25
                });
                
                mostrarMensaje("Consulta generada.");
            },
            error: function () {
                mostrarError('Opción Consultar: Ha ocurrido un error');
            }
        });
    };

var nuevoRegistro = function () {
    window.location.href = controlador + "EditPrograma";
}

function cancelarEdicion() {

    $('#popupEdicion').bPopup().close();

}
function guardarEdicion() {

    var horizonte = $('#horizonteEdit').val();
    if (horizonte == "" || horizonte == null) {
        alert('No se ha seleccionado el horizonte.');
        return;
    }

    var programaAbrev = $('#codigoProgramaEdit').val();
    if (programaAbrev == "" || programaAbrev == null) {
        alert('No se ha ingresado el codigo programa.');
        return;
    }

    var nombrePrograma = $('#nombreProgramaEdit').val();
    if (nombrePrograma == "" || nombrePrograma == null) {
        alert('No se ha ingresado el nombre del programa.');
        return;
    }

    var estadoPrograma = $('#estadoEdit').val();
    if (estadoPrograma == "" || estadoPrograma == null) {
        alert('No se ha seleccionado el estado.');
        return;
    }   

    var horizonteReprograma = $('#hdnHorizonteReprograma').val();
    var codigoProgramaRef = $('#codigoProgramaRef').val();
    if (horizonte == horizonteReprograma) {
        if (codigoProgramaRef == "" || codigoProgramaRef == null) {
            //alert('No se ha ingresado el código programa de Ref.');
            //return;
        }
    }

    var esNuevo = false;
    var codPrograma = $('#hdnCodigoPrograma').val();
    
    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarPrograma',
        data: {
            codPrograma: codPrograma,
            horizonte: horizonte,
            programaAbrev: programaAbrev,
            nombrePrograma: nombrePrograma,
            estadoPrograma: estadoPrograma,
            codigoProgramaRef: codigoProgramaRef,
            esNuevo: false
            
        },
        dataType: 'json',
        success: function (result) {

            if (result.success) {
                $('#popupEdicion').bPopup().close();
                pintarBusqueda();
                alert("Se ha modificado el programa.");
            }
            else {
                alert(result.message);
            }

        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });
}
function modificarPrograma(codiPrograma) {
    $('#esNuevo').val(0);

    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerPrograma',
        data: {
            codigoPrograma: codiPrograma
        },
        dataType: 'json',
        success: function (data) {
            var jsonData = JSON.parse(data);
           
            $('#horizonteEdit').val(jsonData.Rchorpcodi);
            $('#codigoProgramaEdit').val(jsonData.Rcprogabrev);
            $('#nombreProgramaEdit').val(jsonData.Rcprognombre);
            $('#estadoEdit').val(jsonData.Rcprogestado);
            
            $('#hdnCodigoPrograma').val(jsonData.Rcprogcodi);

            $("#popupEdicion").bPopup({
                autoClose: false
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });

}

function eliminarPrograma(codiPrograma) {
    if (confirm("¿Desea eliminar el programa seleccionado?")) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + "EliminarPrograma",
            data: {
                codigoprograma: codiPrograma
            },
            success: function (result) {

                if (result.success) {
                    mostrarExito("Se ha eliminado el registro correctamente.");
                    pintarBusqueda();
                }
                else {
                    alert(result.message);
                }

            },
            error: function () {
                mostrarError('Opción Eliminar: Ha ocurrido un error');
            }
        });
    }
}
function generarReporte(codiPrograma) {
    if (confirm("¿Desea generar el reporte del programa seleccionado?")) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + "GenerarReporte",
            data: {
                codigoprograma: codiPrograma
            },
            success: function (result) {

                if (result != "-1") {
                    window.location.href = controlador + 'DescargarFormato?file=' + result;
                    //mostrarExito("Se ha eliminado el registro correctamente.");
                    //pintarBusqueda();
                }
                else {
                    alert("Error al generar el archivo.");
                }

            },
            error: function () {
                mostrarError('Opción Reporte: Ha ocurrido un error');
            }
        });
    }
}
function muestraCodigoReprogramaRef() {

    var horizonte = $('#horizonteEdit').val();
    var horizonteReprograma = $('#hdnHorizonteReprograma').val();
    if (horizonte == horizonteReprograma) {
        document.getElementById('trCodigoProgramaRef').style.visibility = 'visible';
    } else {
        document.getElementById('trCodigoProgramaRef').style.visibility = 'hidden';
    }

    $('#codigoProgramaRef').val('');
}

function habilitarHorizonte() {

    var horizonte = $('#horizonteDuplicar').val();
    var horizonteSemanal = $('#hdnHorizonteSemanal').val();
    var horizonteMensual = $('#hdnHorizonteMensual').val();
    //alert(horizonte);
    document.getElementById('tdDiaria').style.display = 'none';
    document.getElementById('tdMensual').style.display = 'none';
    document.getElementById('tdSemanal').style.display = 'none';

    if (horizonte > 0) {
        switch (horizonte) {
            case horizonteSemanal: document.getElementById('tdSemanal').style.display = ''; break;
            case horizonteMensual: document.getElementById('tdMensual').style.display = ''; break;
            default: document.getElementById('tdDiaria').style.display = ''; break;
        }
    }

}

function duplicarPrograma(codigoPrograma) {
    $('#hdnCodigoProgramaDuplicar').val(codigoPrograma);
    $("#horizonteDuplicar").val('0');
    $("#fechaMensual").val('');
    $("#cbSemanaAnio").val('0');
    $("#fechaDiaria").val('');
    
    $("#popupDuplicarPrograma").bPopup({
        autoClose: false
    });
}

function generarCopia() {        
    
    var horizonteDuplicar = $("#horizonteDuplicar").val();
    var fechaMensual = $("#fechaMensual").val();
    var semanaAnio = $("#cbSemanaAnio").val();
    var fechaDiaria = $("#fechaDiaria").val();   

    
    if (horizonteDuplicar == 0) {
        alert('No se ha seleccionado un horizonte.');
        return;
    }

    if (horizonteDuplicar == $("#hdnHorizonteDiario").val() && fechaDiaria == '') {
        alert('Ingrese una fecha diaria.');
        return;
    }

    if (horizonteDuplicar == $("#hdnHorizonteMensual").val() && fechaMensual == '') {
        alert('Ingrese un mes.');
        return;
    }

    if (horizonteDuplicar == $("#hdnHorizonteSemanal").val() && semanaAnio == '0') {
        alert('Seleccione una semana.');
        return;
    }   

   
    var programaDuplicar = $("#hdnCodigoProgramaDuplicar").val();
    
    $.ajax({
        type: "POST",
        url: controlador + "GenerarCopiaPrograma",
        data: {            
            codigoProgramaDuplicar: programaDuplicar,
            horizonte: horizonteDuplicar,
            fechaMensual: fechaMensual,
            fechaDiaria: fechaDiaria,
            semana: semanaAnio
        },
        success: function (result) {

            if (result.success) {
                $('#popupDuplicarPrograma').bPopup().close();
                pintarBusqueda();
                alert("Se ha duplicado el programa.");
            }
            else {
                alert(result.message);
            }
        },
        error: function () {
            mostrarError('Opción Duplicar: Ha ocurrido un error');
        }
    });
}

function exportarExcel() {

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: controlador + 'GenerarReporteLista',
        data: {
            horizonte: $("#horizonte").val(),
            codigoPrograma: $("#codigoPrograma").val(),
            estadoPrograma: $("#estado").val(),
            verReprograma: $("#hdnVerReprograma").val()
        },
        success: function (result) {
            if (result != "-1") {
                window.location.href = controlador + 'DescargarFormato?file=' + result;
                //mostrarExito("Se ha eliminado el registro correctamente.");
                //pintarBusqueda();
            }
            else {
                alert("Error al generar el archivo.");
            }
        },
        error: function () {
            mostrarAlerta("Error al generar el archivo");
        }
    });
}

function generarReporteEjecutados(codiPrograma) {
    if (confirm("¿Desea generar el reporte seleccionado?")) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + "GenerarReporteEjecutados",
            data: {
                codigoprograma: codiPrograma
            },
            success: function (result) {

                if (result != "-1") {
                    window.location.href = controlador + 'DescargarFormato?file=' + result;
                    //mostrarExito("Se ha eliminado el registro correctamente.");
                    //pintarBusqueda();
                }
                else {
                    alert("Error al generar el archivo.");
                }

            },
            error: function () {
                mostrarError('Opción Reporte: Ha ocurrido un error');
            }
        });
    }
}