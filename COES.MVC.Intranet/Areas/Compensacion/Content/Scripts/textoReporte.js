var controlador = siteRoot + "compensacion/general/";

$( /**
   * Llamadas iniciales
   * @returns {} 
   */
   $(document).ready(function () {
       mostrarMensaje("Selecionar el periodo y la versión");

       $('#btnConsultar').click(function () {
           if (ValidarVersion('Consultar', 1)) {
               pintarBusqueda();
           }
       });

       $('#btnGuadarEdicion').click(function () {
           guardarEdicion();
       });

       $('#btnCancelarEdicion').click(function () {
           cancelarEdicion();
       });

       /*$('#btnExportar').click(function () {
           if (ValidarVersion('Exportar', 0)) {
               exportar();
           }
       });

       $('#btnCrear').click(function () {
           if (ValidarVersion('Nuevo Punto', 0)) {
               editarRegistro($("#pecacodi").val(), 0);
           }
       });
       */
       $("#pericodi").change(function () { ObtenerPeriodoCalculo(this.value, ''); });

       //Inicializamos la pantalla
       ObtenerPeriodoCalculo($("#pericodi").val(), '');
       //pintarBusqueda();
   }));

function exportar() {
    // controlador no implementado
    $.ajax({
        type: 'POST',
        url: controlador + 'exportarTextoReporte',
        data: {
            pecacodi: $("#pecacodi").val()
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
        error: function (xhr, status, error) {
            alert(xhr.responseText);
        }
        /* function () {
        mostrarError('Opcion Exportar: Ha ocurrido un error');

    }*/
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
            url: controlador + "listarTextoReporte",
            data: {
                pecacodi: $("#pecacodi").val()
            },
            success: function (evt) {
                //$("#listado").html(evt);
                //$("#tabla").dataTable({
                //    "scrollY": 314,
                //    "scrollX": false,
                //    "sDom": "t",
                //    "ordering": false,
                //    "bDestroy": true,
                //    "bPaginate": false,
                //    "iDisplayLength": 50
                //});

                $('#listado').html(evt);
                $('#tabla').dataTable({
                    "ordering": true,
                    "paging": false,
                    "scrollY": 400,
                    "bDestroy": true
                });
                mostrarMensaje("Consulta generada");
            },
            error: function () {
                mostrarError('Opcion Consultar: Ha ocurrido un error');
            }
            
        });
    };

function modificarTextoReporte(pecacodi, codreporte, codtexto) {

    $('#esNuevo').val(0);

    $("#codreporte").prop("disabled", true);
    $("#codtexto").prop("disabled", true);
    

    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerTextoReporte',
        data: {
            pecacodi: pecacodi,
            codreporte: codreporte,
            codtexto: codtexto
        },
        dataType: 'json',
        success: function (data) {
            var jsonData = JSON.parse(data);

            $('#codreporte').val(jsonData.Txtrepcodreporte);
            $('#codtexto').val(jsonData.Txtrepcodtexto);
            $('#texto').val(jsonData.Txtreptexto);
     
            $("#popupEdicion").bPopup({
                autoClose: false
            });
           //  posicionar cursor al final del texto
          //  texto = $("#texto").val();
          //  $("#texto").focus().val("").val(texto);
        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });

}

/*
* Guarda la edición del incremento/reducción
*/
function guardarEdicion() {

   /* var pecacodi = $.get("periodo");
    if (pecacodi == "" || pecacodi == null) {
        alert('No se ha podido identificar el periodo');
        return;
    }
    */
    var pecacodi = $('#pecacodi').val();
    if (pecacodi == "" || pecacodi == null) {
        alert('No se ha podido identificar el periodo');
        return;
    }
    var codreporte = $('#codreporte').val();
    if (codreporte == "" || codreporte == null) {
        alert('Codigo de reporte no identificado');
        return;
    }

    var codtexto = $('#codtexto').val();
    if (codtexto == "" || codtexto == null) {
        alert('Codigo de texto no identificado');
        return;
    }

    var texto = $('#texto').val().trim();
    $('#texto').val(texto);

    if (texto == "" || texto == null) {
        $('#texto').focus();
        alert('Por favor proporcione el texto');
        return;
    }

    var esNuevo = false;

    if ($('#esNuevo').val() == 1) {
        esNuevo = true;
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarTextoReporte',
        data: {
            pecacodi: pecacodi,
            codreporte: codreporte,
            codtexto: codtexto,
            texto: texto,
            EsNuevo: esNuevo
        },
        dataType: 'json',
        success: function (result) {

            if (result.success) {
                $('#popupEdicion').bPopup().close();
                pintarBusqueda();
                if (esNuevo) {
                    mostrarExito("Se ha creado el texto del reporte");
                }
                else {
                    mostrarExito("Se ha modificado el texto del reporte");
                }
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

function cancelarEdicion() {

    $('#popupEdicion').bPopup().close();

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

var editarRegistro = function (pecacodi, id) {
    window.location.href = controlador + "EditPtoGrupo?pecacodi=" + pecacodi + "&id=" + id;
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