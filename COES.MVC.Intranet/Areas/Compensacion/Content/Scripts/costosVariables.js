var controlador = siteRoot + "compensacion/general/";

$( /**
   * Llamadas iniciales
   * @returns {} 
   */
   $(document).ready(function () {
       //pintarBusqueda();
       mostrarMensaje("Selecionar el periodo y la versión");

       $('#btnExportar').click(function () {
           if (ValidarVersion('Exportar', 0)) {
               exportar();
           }
       });

       $('#btnCancelar').click(function () {
           cancelar();
       });

       $('#btnGrabar').click(function () {
           grabarCalculo();
       });

       $('#btnConsultar').click(function () {
           if (ValidarVersion('Consultar', 1)) {
               pintarBusqueda();
           }
       });

       $("#empresa").change(function () { ObtenerListaCentral(this.value, ''); });
       $("#central").change(function () { ObtenerListaGrupo(this.value, ''); });
       $("#grupo").change(function () { ObtenerListaModo(this.value, ''); });

       $("#pericodi").change(function () { ObtenerPeriodoCalculo(this.value, ''); });

       //Inicializamos la pantalla
       ObtenerPeriodoCalculo($("#pericodi").val(), '');
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

function grabarCalculo() {

    var consid = "NO";
    $.each($("input[name='considera']:checked"), function () {
        consid = "SI";
    });

    if (confirm("¿Desea guardar la información?")) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + "GrabarCalculo",
            data: {
                pecacodi: $('#pecacodi').val(),
                grupocodi: $('#grupocodi').val(),
                barrcodi: $('#barrcodi').val(),
                considera: consid,
                energia: $('#energia').val(),
                tiempo: $('#tiempo').val()
            },
            beforeSend: function () {
                mostrarAlerta("Guardando Información ..");
            },
            success: function (evt) {
                mostrarExito("Registro actualizado correctamente.");
            },
            error: function () {
                mostrarError('Opcion Grabar calculo: Ha ocurrido un error');
            }
        });        
    }
}

var pintarBusqueda =
    /**
    * Pinta el listado de periodos según el año seleccionado
    * @returns {} 
    */
    function () {


        $.ajax({
            type: "POST",
            url: controlador + "listarCostosVariables",
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
                mostrarMensaje("Consulta generada");
            },
            error: function() {
                mostrarError('Opcion Consultar: Ha ocurrido un error');
            }
        });
    };

function exportar() {
    $.ajax({
        type: 'POST',
        url: controlador + 'exportarCostosVariables',
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

function cancelar() {
    window.location.href = controlador + "CostosVariables";
}

var editarRegistro = function (pecacodi, grupocodi) {
    window.location.href = controlador + "EditCalculo?per=" + pecacodi + "&grup=" + grupocodi;
}

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
            /*error: function () {
                mostrarError('Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }*/
            error: function (xhr, status, error) {
                alert(xhr.responseText);
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
            error: function (xhr, status, error) {
                alert(xhr.responseText);
            }
                // function () {
                //mostrarError('Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            //}
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