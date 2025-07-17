var controlador = siteRoot + "compensacion/general/";

$( /**
   * Llamadas iniciales
   * @returns {} 
   */
   $(document).ready(function () {
       mostrarMensaje("Selecionar el periodo y la versión");

       $('#fechaini').Zebra_DatePicker({
           format: 'd/m/Y'
       });

       $('#fechafin').Zebra_DatePicker({
           format: 'd/m/Y'
       });

       $('#btnConsultar').click(function () {
           if (ValidarVersion('Consultar',1)) {
               pintarBusqueda();
           }
       });

       $('#btnProcesar').click(function () {
           var x = ValidarVersion('Procesar', 0);

           if (x==true) {
               procesarCompensacion();
           }
       });

       $('#btnExportar').click(function () {
           if (ValidarVersion('Exportar',0)) {
               exportar();
           }
       });

       $("#pericodi").change(function () { ObtenerPeriodoCalculo(this.value, ''); });
       $("#pecacodi").change(function () { VerificarEstadoPeriodoCalculo(this.value); });
       $("#empresa").change(function () { ObtenerListaCentral(this.value, ''); });
       $("#central").change(function () { ObtenerListaGrupo(this.value, ''); });
       $("#grupo").change(function () { ObtenerListaModo(this.value, ''); });
       
       //Inicializamos la pantalla
       ObtenerPeriodoCalculo($("#pericodi").val(), '');
       VerificarEstadoPeriodoCalculo($("#pecacodi").val());

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

function procesarCompensacion() {
    $.ajax({
        type: 'POST',
        url: controlador + 'procesarCompensacionEspecial',
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

function exportar() {

    $.ajax({
        type: 'POST',
        url: controlador + 'exportarCompensacionesEspeciales',
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
            url: controlador + "listarCompensacionesEspeciales",
            data: {
                pecacodi: $("#pecacodi").val(),
                empresa: $("#empresa").val(),
                central: $("#central").val(),
                grupo: $("#grupo").val(),
                modo: $("#modo").val()
            },
            success: function (evt) {
                $('#listado').html(evt);
                $('#tabla').dataTable({
                    "ordering": true,
                    "paging": false,
                    "scrollY": 352,
                    "bDestroy": true
                });
                mostrarMensaje("Consulta generada");
            },
            error: function () {
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
                mostrarError('Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
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
                emprcodi: empresa,
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
                mostrarError('Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
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
                mostrarError('Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    }
    else {
        dwr.util.removeAllOptions("modo");
    }
}

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
                VerificarEstadoPeriodoCalculo($("#pecacodi").val());
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
                    }
                    else {
                        $("#btnProcesar").show();
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