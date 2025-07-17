var controlador = siteRoot + "transferencias/calculorentacongestion/";

//Funciones de busqueda
$(document).ready(function () {   
    
    //consultar();

    $('#btnConsultar').click(function () {
        consultar();
    });

    $('#btnProcesar').click(function () {
        validar();
    });

    $('#btnGenerarReporte').click(function () {
        exportarDatos();
    });

    $("#version").prop("disabled", true);
    $("#pericodi").change(function () {
        if ($("#pericodi").val() != "") {
            var options = {};
            options.url = controlador + "GetVersiones";
            options.type = "POST";
            options.data = JSON.stringify({ pericodi: $("#pericodi").val() });
            options.dataType = "json";
            options.contentType = "application/json";
            options.success = function (modelo) {
                //console.log(modelo);
                $("#version").empty();
                $("#version").removeAttr("disabled");
                $.each(modelo.ListaRecalculo, function (k, v) {
                    var option = '<option value=' + v.RecaCodi + '>' + v.RecaNombre + '</option>';
                    $('#version').append(option);
                    //console.log(option);
                });
                $('#mensaje').removeClass();
                $('#mensaje').html('');
            };
            options.error = function () { mostrarError("Lo sentimos, no se encuentran registrada ninguna revisión"); };
            $.ajax(options);
        }
        else {
            $("#version").empty();
            $("#version").prop("disabled", true);
            document.getElementById('btnProcesar').style.display = "none";
            //document.getElementById('divOpcionCarga').style.display = "none";
        }
    });

    $("#version").change(function () {
        if ($("#pericodi").val() != "" && $("#version").val() != "0") {
            var options = {};
            options.url = controlador + "GetVersion";
            options.type = "POST";
            options.data = JSON.stringify({ pericodi: $("#pericodi").val(), recacodi: $("#version").val() });
            options.dataType = "json";
            options.contentType = "application/json";
            options.success = function (modelo) {
                //console.log(modelo);               
                if (modelo.bEjecutar == true)
                {
                    $('#hdnEstado').val(modelo.Entidad.RecaEstado);
                    document.getElementById('btnProcesar').style.display = "";
                    document.getElementById('btnCopiar').style.display = "";
                }
                else
                {
                    $('#hdnEstado').val('');
                    document.getElementById('btnProcesar').style.display = "none";
                    document.getElementById('btnCopiar').style.display = "none";
                }

                $("#numeroRegistros").val(modelo.NumeroRegistros);
                $("#fechaActualizacion").val(modelo.UltimaFechaActualizacion);

                
            };
            options.error = function () { mostrarError("Lo sentimos, no se encuentran registrada ninguna versión"); };
            $.ajax(options);
        }
        else {
            document.getElementById('btnProcesar').style.display = "none";
            document.getElementById('btnCopiar').style.display = "none";
            //document.getElementById('divOpcionCarga').style.display = "none";
            $("#numeroRegistros").val('');
            $("#fechaActualizacion").val('');
        }
    });

    $('#btnDetalleCostosMarginales').click(function () {
        generarCostosMarginales();
    });

    $('#btnDetalleEntregasRetiros').click(function () {
        generarEntregasRetiros();
    });

    $('#btnCopiar').click(function () {
        validarCopiaCostosMarginales();
    });
});

function procesar() {    
    $.ajax({
        type: 'POST',
        url: controlador + "procesarcalculo",
        data:{
            pericodi:$('#pericodi').val(),
            recacodi: $('#version').val(),
            estado: $('#hdnEstado').val()
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 480,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "bDestroy": true,
                "bPaginate": false,
                "iDisplayLength": 50
            });
            obtenerMontosTotales();
            //document.getElementById("btnMostrarErrores").style.display = 'none';
            //document.getElementById("btnGenerarReporte").style.display = '';
            if ($('#hdnEstado').val() == "") {
                mostrarAlerta("La versión no tiene el estado 'Abierto'");
            } else {
            mostrarMensaje("Proceso generado.");
            }
            
        },
        error: function () {
            mostrarAlerta("Lo sentimos, se ha producido un error");
        }
    });
};
function consultar() {    

    var pericodi = $('#pericodi').val();
    if (pericodi == '' || pericodi == '0') {
        mostrarAlerta("Debe seleccionar el periodo.");
        return;
    }
    var recacodi = $('#version').val();
    if (recacodi == '' || recacodi == '0') {
        mostrarAlerta("Debe seleccionar la versión.");
        return;
    }

    $.ajax({
        type: 'POST',
        url: controlador + "consultarperiodoempresas",
        data: {
            pericodi: pericodi,
            recacodi: recacodi
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 480,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "bDestroy": true,
                "bPaginate": false,
                "iDisplayLength": 50
            });
            obtenerMontosTotales();
            var estado = $('#hdnEstado').val();
            if (estado == 'Abierto') {
                document.getElementById('btnProcesar').style.display = "";
            } else {
                document.getElementById('btnProcesar').style.display = "none";
            }
            mostrarMensaje("Consulta generada.");
        },
        error: function () {
            mostrarAlerta("Lo sentimos, se ha producido un error");
        }
    });
};

function validar() {
    var pericodi = $('#pericodi').val();
    if (pericodi == '' || pericodi == '0') {
        mostrarAlerta("Debe seleccionar el periodo.");
        return;
    }
    var recacodi = $('#version').val();
    if (recacodi == '' || recacodi == '0') {
        mostrarAlerta("Debe seleccionar la versión.");
        return;
    }
    $.ajax({
        type: 'POST',
        url: controlador + "validarcalculo",
        data: {
            pericodi: pericodi,
            recacodi: recacodi
        },
        success: function (result) {
            if (result == "-1") {
                mostrarError("No se han ingresado los porcentajes para el reparto RND, por favor completar los datos y vuelva a procesar.");
                res = -1;
            } else if (result == "-2") {
                res = -2;
                //mostrarError("No todas las barras tienen definido el costo marginal.");
                mostrarError("El periodo y versión seleccionado no tiene Costo Marginal definido.");
                //procesar();
                //exportarDatosError();
            } else {
                //Se realiza el proceso normal
                procesar();
            }
            
        },
        error: function () {
            mostrarAlerta("Lo sentimos, se ha producido un error");
        }
    });

    
};

function copiarDatosCostosMarginales() {
    var pericodi = $('#pericodi').val();
    if (pericodi == '' || pericodi == '0') {
        mostrarAlerta("Debe seleccionar el periodo.");
        return;
    }
    var recacodi = $('#version').val();
    if (recacodi == '' || recacodi == '0') {
        mostrarAlerta("Debe seleccionar la versión.");
        return;
    }

    var tipoCopiaId = $('input:radio[name=tipoCopia]:checked').val();    
        
    $.ajax({
        type: 'POST',
        url: controlador + "copiarCostosMarginales",
        data: {
            pericodi: pericodi,
            recacodi: recacodi,
            tipoCopia: tipoCopiaId
        },
        success: function (result) {
            if (result == "-1") {
                mostrarError("Hubo un error al copiar los datos, por favor vuelva a procesar.");
                //res = -1;
            }

            if (result == "0") {
                mostrarExito("Se realizo la copia de datos correctamente.");
                //res = -1;
            }

        },
        error: function () {
            mostrarAlerta("Lo sentimos, se ha producido un error");
        }
    });


};

function obtenerMontosTotales() {
    $.ajax({
        type: 'POST',
        url: controlador + "obtenermontostotales",
        data: {
            pericodi: $('#pericodi').val(),
            recacodi: $('#version').val()
        },
        success: function (result) {

            if (result.length > 0) {
                var montos = result.split('/');
                $('#rentaCongestion').val(montos[0]);
                $('#rentaNoAsiganda').val(montos[1]);
            }
            //document.getElementById("btnGenerarReporte").style.display = '';
        },
        error: function () {
            mostrarAlerta("Lo sentimos, se ha producido un error");
        }
    });
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

function exportarDatos() {
    var pericodi = $('#pericodi').val();
    if (pericodi == '' || pericodi == '0') {
        mostrarAlerta("Debe seleccionar el periodo.");
        return;
    }
    var recacodi = $('#version').val();
    if (recacodi == '' || recacodi == '0') {
        mostrarAlerta("Debe seleccionar la versión.");
        return;
    }

    if (confirm("¿Desea generar reporte?")) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + "GenerarReporte",
            data: {
                pericodi: pericodi,
                recacodi: recacodi
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
function exportarDatosError() {

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: controlador + "GenerarReporteErrores",
        data: {
            pericodi: $('#pericodi').val(),
            recacodi: $('#version').val()
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

function generarCostosMarginales() {

    var pericodi = $('#pericodi').val();
    if (pericodi == '' || pericodi == '0') {
        mostrarAlerta("Debe seleccionar el periodo.");
        return;
    }

    var recacodi = $('#version').val();
    if (recacodi == '' || recacodi == '0') {
        mostrarAlerta("Debe seleccionar la versión.");
        return;
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: controlador + "GenerarReporteCostosMarginales",
        data: {
            pericodi: $('#pericodi').val(),
            recacodi: $('#version').val()
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
};

function generarEntregasRetiros() {

    var pericodi = $('#pericodi').val();
    if (pericodi == '' || pericodi == '0') {
        mostrarAlerta("Debe seleccionar el periodo.");
        return;
    }

    var recacodi = $('#version').val();
    if (recacodi == '' || recacodi == '0') {
        mostrarAlerta("Debe seleccionar la versión.");
        return;
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: controlador + "GenerarReporteEntregasRetiros",
        data: {
            pericodi: $('#pericodi').val(),
            recacodi: $('#version').val()
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
};

function validarCopiaCostosMarginales() {
    var pericodi = $('#pericodi').val();
    if (pericodi == '' || pericodi == '0') {
        mostrarAlerta("Debe seleccionar el periodo.");
        return;
    }
    var recacodi = $('#version').val();
    if (recacodi == '' || recacodi == '0') {
        mostrarAlerta("Debe seleccionar la versión.");
        return;
    }
    var tipoCopiaId = $('input:radio[name=tipoCopia]:checked').val();

    if (recacodi == 1 && tipoCopiaId == 2) {
        mostrarAlerta("La versión seleccionada no puede usarse para el tipo de copia seleccionado.");
        return;
    }

    $.ajax({
        type: 'POST',
        url: controlador + "ValidarCopiaCostosMarginales",
        data: {
            pericodi: pericodi,
            recacodi: recacodi
        },
        success: function (result) {
             if (result == "-1") {
                //res = -1;
                mostrarError("No todas las barras tienen definido el costo marginal.");                
                //procesar();
                exportarDatosError();
            } else {
                //Se realiza la copia normal
                 copiarDatosCostosMarginales();
            }

        },
        error: function () {
            mostrarAlerta("Lo sentimos, se ha producido un error");
        }
    });


};
