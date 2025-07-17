var controlador = siteRoot + "compensacion/general/";

$( /**
   * Llamadas iniciales
   * @returns {} 
   */
   $( document ).ready(function() {
       //pintarBusqueda();
       mostrarMensaje("Selecionar el periodo y la versión");

       $('#fechaini').Zebra_DatePicker({
           format: 'd/m/Y'
       });

       $('#fechafin').Zebra_DatePicker({
           format: 'd/m/Y'
       });

       $('#btnConsultar').click(function () {
           if (ValidarVersion('Consultar', 1)) {
               pintarBusqueda();
           }
       });

       $('#btnExportar').click(function () {
           if (ValidarVersion('Exportar', 0)) {
               exportar();
           }
       });

       $('#btnProcesar').click(function () {
           if (ValidarVersion('Procesar', 0)) {
               procesarCompensacion();
           }
       });

       //- compensaciones.HDT - 21/03/2017: Cambio para atender el requerimiento. 
       $('#btnIncrementosReducciones').click(function () {
           if (ValidarVersion('Incrementos/Reducciones', 0)) {
               procesarIncrementosReducciones();
           }
       });
       //- HDT Fin

       //- compensaciones.HDT - 21/03/2017: Cambio para atender el requerimiento. 
       $('#btnCompensacionEspecialIlo2').click(function () {
           if (ValidarVersion('Compensacion Especial (Ilo2)', 0)) {
               procesarCompensacionEspecialIlo2();
           }
       });
       //- HDT Fin

       //- compensaciones.HDT - 29/03/2017: Cambio para atender el requerimiento. 
       $('#btnRegistrarCalculoManual').click(function () {
           if (ValidarVersion('Registrar Calculo Manual', 0)) {
               procesarRegistrarCalculoManual();
           }
       });
       //- HDT Fin
       //- compensaciones.HDT - 12/04/2017: Cambio para atender el requerimiento. 
       $("#pericodi").change(function () { ObtenerPeriodoCalculo(this.value, ''); });
       $("#pecacodi").change(function () { VerificarEstadoPeriodoCalculo(this.value); });
       $("#empresa").change(function () { ObtenerListaCentral(this.value, ''); });
       $("#central").change(function () { ObtenerListaGrupo(this.value, ''); });
       $("#grupo").change(function () { ObtenerListaModo(this.value, ''); });

       //Inicializamos la pantalla
       ObtenerPeriodoCalculo($("#pericodi").val(), '');
       VerificarEstadoPeriodoCalculo($("#pecacodi").val());
       //- HDT Fin

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

//- compensaciones.HDT - 12/04/2017: Cambio para atender el requerimiento. 
function ObtenerPeriodoCalculo(valor, selected) {

    if (valor != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerPeriodoCalculo',
            data: {
                pericodi: valor
            },
            dataType: 'json',
            success: function (data) {
                var jsonData = JSON.parse(data);
                dwr.util.removeAllOptions("pecacodi");
                dwr.util.addOptions("pecacodi", jsonData, 'id', 'name');
                dwr.util.setValue("pecacodi", selected);
            },
            error: function () {
                mostrarError('Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    }
    else {
        dwr.util.removeAllOptions("pecacodi");
    }
}

function procesarCompensacion() {
    $.ajax({
        type: 'POST',
        url: controlador + 'grabarArranquesParadas',
        data: {
            pecacodi: $("#pecacodi").val()
        },
        dataType: 'json',
        success: function (result) {
            mostrarExito("Proceso culminado con Éxito.");
        },
        error: function () {
            mostrarError('Opcion Procesar: Ha ocurrido un error');
        }
    });
}

//- compensaciones.HDT - 21/03/2017: Cambio para atender el requerimiento. 
function procesarIncrementosReducciones() {
    var pecacodi = $("#pecacodi").val();
    window.location.href = controlador + "IncrementosReducciones?periodo=" + pecacodi;
    //window.location.href = controlador + "IncrementosReducciones?periodo=1";
    
}

//- compensaciones.HDT - 29/03/2017: Cambio para atender el requerimiento. 
function procesarCompensacionEspecialIlo2() {
    var pecacodi = $("#pecacodi").val();
    window.location.href = controlador + "CompensacionEspecialIlo2?periodo=" + pecacodi;
    //window.location.href = controlador + "CompensacionEspecialIlo2?periodo=22";
}

//- compensaciones.HDT - 29/03/2017: Cambio para atender el requerimiento. 
function procesarRegistrarCalculoManual() {
    var pecacodi = $("#pecacodi").val();
    window.location.href = controlador + "RegistroCalculoManual?periodo=" + pecacodi;
    //window.location.href = controlador + "RegistroCalculoManual?periodo=22";
}

function exportar() {

    $.ajax({
        type: 'POST',
        url: controlador + 'exportarArranquesParadas',
        data: {
            pecacodi: $("#pecacodi").val(),
            empresa: $("#empresa").val(),
            central: $("#central").val(),
            grupo: $("#grupo").val(),
            modo: $("#modo").val()

        },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                document.location.href = controlador + 'descargar?formato=' + 1 + '&file=' + result
                mostrarMensaje("Exportación realizada");
            }
            else {
                mostrarError('Opcion Exportar: Ha ocurrido un error');
            }
        },
        error: function () {
            mostrarError('Opcion Exportar: Ha ocurrido un error');

        }
    });
}

var pintarBusqueda =
    /**
    * Pinta el listado de periodos según el año seleccionado
    * @returns {} 
    */
    function () {
        $.ajax({
            type: "POST",
            url: controlador + "ListarArranquesParadas",
            data: {
                pecacodi: $("#pecacodi").val(),
                empresa: $("#empresa").val(),
                central: $("#central").val(),
                grupo: $("#grupo").val(),
                modo: $("#modo").val()
            },
            success: function (evt) {

                $('#listado').css("width", $('#mainLayout').width() + "px");
                $('#listado').html(evt);
                $('#tabla').dataTable({
                    "ordering": false,
                    "paging": false,
                    "scrollY": 350,
                    "scrollX": true,
                    "bDestroy": true
                });
               // alert(evt.mensaje);
                mostrarMensaje("Consulta generada");
                
                //$('#listado').html(evt);
                //$('#tabla').dataTable({
                //    "ordering": true,
                //    "paging": false,
                //    "scrollY": 475,
                //    "bDestroy": true
                //});
            },
            error: function() {
                mostrarError('Opcion Consultar: Ha ocurrido un error');
            }
        });
    };



function ObtenerListaCentral(valor, selected) {

    if (valor != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerListaCentral',
            data: {
                emprcodi: valor
            },
            dataType: 'json',
            success: function (data) {
                var jsonData = JSON.parse(data);
                dwr.util.removeAllOptions("modo");
                dwr.util.removeAllOptions("grupo");
                dwr.util.removeAllOptions("central");
                dwr.util.addOptions("central", jsonData, 'id', 'name');
                dwr.util.setValue("central", selected);
            },
            error: function () {
                //mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
                mostrarError('Ha ocurrido un error');
            }
        });
    }
    else {
        dwr.util.removeAllOptions("modo");
        dwr.util.removeAllOptions("grupo");
        dwr.util.removeAllOptions("central");

    }
}

function ObtenerListaGrupo(valor, selected) {
    var empresa;
    empresa = $("#empresa").val();

    if (valor != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerListaGrupo',
            data: {
                emprcodi:empresa,
                grupopadre: valor
            },
            dataType: 'json',
            success: function (data) {
                var jsonData = JSON.parse(data);
                dwr.util.removeAllOptions("modo");
                dwr.util.removeAllOptions("grupo");
                dwr.util.addOptions("grupo", jsonData, 'id', 'name');
                dwr.util.setValue("grupo", selected);
            },
            error: function () {
                //mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
                mostrarError('Ha ocurrido un error');
            }
        });
    }
    else {
        dwr.util.removeAllOptions("modo");
        dwr.util.removeAllOptions("grupo");
    }
}

function ObtenerListaModo(valor, selected) {
    var empresa;
    empresa = $("#empresa").val();

    if (valor != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerListaModo',
            data: {
                emprcodi: empresa,
                grupopadre: valor
            },
            dataType: 'json',
            success: function (data) {
                var jsonData = JSON.parse(data);
                dwr.util.removeAllOptions("modo");
                dwr.util.addOptions("modo", jsonData, 'id', 'name');
                dwr.util.setValue("modo", selected);
            },
            error: function () {
                //mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
                mostrarError('Ha ocurrido un error');
            }
        });
    }
    else {
        dwr.util.removeAllOptions("modo");
    }
}

function VerificarEstadoPeriodoCalculo(valor) {

    if (valor != null && valor != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'ObtenerRegistroPeriodoCalculo',
            data: {
                pecacodi: valor
            },
            dataType: 'json',
            success: function (data) {
                if (data != null) {
                    if (data.PecaEstRegistro == "0") {
                        $("#btnProcesar").hide();
                        $("#btnIncrementosReducciones").hide();
                        $("#btnCompensacionEspecialIlo2").hide();
                        $("#btnRegistrarCalculoManual").hide();
                    }
                    else {
                        $("#btnProcesar").show();
                        $("#btnIncrementosReducciones").show();
                        $("#btnCompensacionEspecialIlo2").show();
                        $("#btnRegistrarCalculoManual").show();
                    }
                }
            },
            error: function () {
                mostrarError('Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    }
    else {
        $("#btnProcesar").hide();
        $("#btnIncrementosReducciones").hide();
        $("#btnCompensacionEspecialIlo2").hide();
        $("#btnRegistrarCalculoManual").hide();
    }
}

function ValidarVersion(titulo_opcion, limpiar_listado) {
    if ($("#pecacodi").val() == "" || $("#pecacodi").val() == null) {

        if (limpiar_listado == 1) {
            $("#listado").empty();
        }

        mostrarAlerta("Opcion " + titulo_opcion + ": Verificar la selección del periodo y la versión");

        return false;
    }
    else {
        return true;
    }
}
