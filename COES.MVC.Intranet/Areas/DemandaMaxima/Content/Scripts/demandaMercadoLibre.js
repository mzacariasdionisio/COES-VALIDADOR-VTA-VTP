var controlador = siteRoot + 'demandaMaxima/DemandaMercadoLibre/';
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

    $('#btnConsultar').click(function () {
        //- pr16.HDT - Inicio 01/04/2018: Cambio para atender el requerimiento.
        //pintarPaginado();
        //pintarBusqueda(1);
        var pasaValidacion = true;
             
        if (pasaValidacion) {
            pintarPaginado();
            pintarBusqueda(1);
        }
        //- HDT Fin
    });

    $('#btnExportar').click(function () {
        
        exportarInformacion();
        
    });

    $('#btnGenerarRegistroDemandas').click(function () {
        
        generarRegistroDemandas();
        
    });

    $('#btnActualizarPeriodo').click(function () {

        actualizarPeriodo();

    });

    
    $("#cbPeriodoIni").change(function () {
        changePeriodo();
    });

    if ($("#cbPeriodoIni").val() != null) {
        changePeriodo();
    }

    $("#cbPeriodoIniSicli").change(function () {
        changePeriodoSicli();
    });

    if ($("#cbPeriodoIniSicli").val() != null) {
        changePeriodoSicli();
    }
    //- HDT fin
    var permisoEdicion = $("#hdnEdicion").val();
    if (permisoEdicion == '1') {
        document.getElementById('tbEdicion').style.display = '';
    }

});

//- pr16.HDT - Inicio 01/04/2018: Cambio para atender el requerimiento.
function changePeriodo() {

    if ($("#cbPeriodoIni").val() != null && $("#cbPeriodoIni").val() != '') {

        $.ajax({
            type: "POST",
            url: controlador + "obtenerMaximaDemanda",
            data: {
                periodoInicial: $("#cbPeriodoIni").val()
            },
            dataType: "json",
            success: function (resultado) {
                $("#fechaMD").val(resultado.FechaMD);
                $("#horaMD").val(resultado.HoraMD);
                $("#valorMD").val(resultado.ValorMD);
            },
            error: function (xhr) {
                mostrarError("Ocurrio un problema >> readyState: " + xhr.readyState + " | status: " + xhr.status + " | responseText: " + xhr.responseText);
            }
        });
    }
}

function actualizarPeriodo() {

    var estadoPeriodo = $("#hdnEstadoPeriodo").val();
    var estadoPeriodoDemanda = $("#hdnEstadoPeriodoDemanda").val();
    if (estadoPeriodoDemanda == '0') {
        if (estadoPeriodo == '0') {
            return mostrarAlerta('El periodo no se encuentra abierto. No se puede cerrar el periodo Demanda');
        }
    }

    $.ajax({
        type: "POST",
        url: controlador + "actualizarPeriodo",
        data: {
            periodo: $("#cbPeriodoIniSicli").val(),
            estadoPeriodoDemanda: estadoPeriodoDemanda
        },
        dataType: "json",
        success: function (resultado) {
            
            $("#hdnEstadoPeriodo").val(resultado.PSicliCerrado);
            $("#hdnEstadoPeriodoDemanda").val(resultado.PSicliCerradoDemanda);

            if (resultado.PSicliCerradoDemanda == '1') {
                var cerrarPeriodo = document.getElementById('btnActualizarPeriodo');
                cerrarPeriodo.value = 'Abrir Periodo';
            } else {
                var cerrarPeriodo = document.getElementById('btnActualizarPeriodo');
                cerrarPeriodo.value = 'Cerrar Periodo';
            }
            mostrarExito('Periodo actualizado correctamente.');
        },
        error: function (xhr) {
            mostrarError("Ocurrio un problema >> readyState: " + xhr.readyState + " | status: " + xhr.status + " | responseText: " + xhr.responseText);
        }
    });
}

function changePeriodoSicli() {

    if ($("#cbPeriodoIniSicli").val() != null && $("#cbPeriodoIniSicli").val() != '') {

        $.ajax({
            type: "POST",
            url: controlador + "obtenerMaximaDemandaSicli",
            data: {
                periodoInicial: $("#cbPeriodoIniSicli").val()
            },
            dataType: "json",
            success: function (resultado) {
                $("#fechaMDS").val(resultado.FechaMD);
                $("#horaMDS").val(resultado.HoraMD);
                $("#valorMDS").val(resultado.ValorMD);
                $("#hdnEstadoPeriodo").val(resultado.EstadoPeriodo);
                $("#hdnEstadoPeriodoDemanda").val(resultado.EstadoPeriodoDemanda);
                
                if (resultado.EstadoPeriodoDemanda == '1') {
                    document.getElementById('btnActualizarPeriodo').value = 'Abrir Periodo';
                    //cerrarPeriodo.innerHTML = 'Abrir Periodo';
                } else {
                    document.getElementById('btnActualizarPeriodo').value = 'Cerrar Periodo';
                    //cerrarPeriodo.innerHTML = 'Cerrar Periodo';
                }
            },
            error: function (xhr) {
                mostrarError("Ocurrio un problema >> readyState: " + xhr.readyState + " | status: " + xhr.status + " | responseText: " + xhr.responseText);
            }
        });
    }
}


pintarPaginado = function () {

    //var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    //if (empresa == "[object Object]") empresa = "";
    //$('#hfEmpresa').val(empresa);

    var suministrador = $('#cbSuministrador').val();
    var razonSocial = $('#txtRazonSocial').val();

    if ($('#cbPeriodoIni').val() != "" && $('#cbPeriodoSicli').val() != "") {

        $.ajax({
            type: 'POST',
            url: controlador + "paginado",
            data: {                
                periodo: $('#cbPeriodoIni').val(),
                suministrador: suministrador,
                empresa: razonSocial
            },
            success: function (evt) {
                $('#paginado').html(evt);
                mostrarPaginado();
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });

    }
    else {
        mostrarAlerta("Seleccione rango de fechas del periodo.");
    }
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

function mostrarPaginado() {
    $.ajax({
        type: 'POST',
        url: controlador + 'paginado',
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            alert("Error al cargar el paginado");
        }
    });
}

function pintarBusqueda(id) {
    var inp1 = document.getElementById('cbPeriodoIni').value;

    //var suministrador = $('#cbSuministrador').multipleSelect('getSelects');
    //if (suministrador == "[object Object]") suministrador = "";
    //$('#hfSuministrador').val(suministrador);   

    var suministrador = $('#cbSuministrador').val();

    if ($('#cbPeriodoIni').val() != "") {

        $.ajax({
            type: 'POST',
            url: controlador + 'ListarReporteInformacion',
            data: {
                periodo: $('#cbPeriodoIni').val(),
                //codigoZona: $('#cbZona').val(),
                //codigoPuntoMedicion: $('#hfSubestaciones').val(),
                empresa: $('#txtRazonSocial').val(),
                //suministrador: $('#hfSuministrador').val(),
                suministrador: suministrador,
                nroPagina: id
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
                mostrarMensaje("Consulta generada.")
            },
            error: function () {
                mostrarAlerta("Error al obtener la consulta");
            }
        });
    }
    else {
        mostrarAlerta("Seleccione periodo.");
    }
}

function exportarInformacion() {
    var inp1 = document.getElementById('cbPeriodoIni').value;

    //var suministrador = $('#cbSuministrador').multipleSelect('getSelects');
    //if (suministrador == "[object Object]") suministrador = "";
    //$('#hfSuministrador').val(suministrador);   

    var suministrador = $('#cbSuministrador').val();

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: controlador + "GenerarReporte",
        data: {
            periodo: $('#cbPeriodoIni').val(),
            periodoSICLI: $('#cbPeriodoIniSicli').val(),            
            empresa: $('#txtRazonSocial').val(),
            //suministrador: $('#hfSuministrador').val(),
            suministrador: suministrador
        },
        success: function (result) {

            if (result != "-1") {
                window.location.href = controlador + 'DescargarFormato?file=' + result;                
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

function generarRegistroDemandas() {

    if ($('#cbPeriodoIni').val() != "" ) {
        $.ajax({
            type: 'POST',
            url: controlador + 'generarRegistroDemandas',
            data: {                
                periodo: $('#cbPeriodoIni').val(),
                periodoSicli: $('#cbPeriodoIniSicli').val(),
                fechaDemandaMaxima: $('#fechaMD').val(),
                fechaDemandaMaximaSicli: $('#fechaMDS').val()
            },
            dataType: 'json',
            success: function (result) {
                if (result != -1) {
                    mostrarExito('Proceso Generado satisfactoriamente');
                }
                else {
                    mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    }
    
}