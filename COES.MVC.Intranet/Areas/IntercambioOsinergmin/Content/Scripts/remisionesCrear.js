var controlador = siteRoot + "intercambioOsinergmin/remisiones/";

$( /**
   * Llamadas inciales
   * @returns {} 
   */
    function () {
        //Le damos el formato de datepicker a la caja de periodo
        $("#PeriodoRemisionModel_Periodo").Zebra_DatePicker({
            format: "m Y",    //Solo debe escoger el año y mes
            onSelect: function(periodo) {
                validarPeriodo(periodo);
            }
        });

        $('#btnRemitir').click(function () {
            remitirTodo();
        });

        $('#btnCancelar').click(function () {
            cancelar();
        });

        //Cuando el periodo cambie, hay que validar el periodo
        //$("#PeriodoRemisionModel_Periodo").change(validarPeriodo); //.on("change", validarPeriodo());
    });

function cancelar() {
    window.location.href = controlador + "Index";
}

function checkAll(ele) {
    var checkboxes = document.getElementsByTagName('input');
    if (ele.checked) {
        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].type == 'checkbox') {
                checkboxes[i].checked = true;
            }
        }
    } else {
        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].type == 'checkbox') {
                checkboxes[i].checked = false;
            }
        }
    }
}

var validarPeriodo =
    /**
    * Valida que el periodo actual no exista, si no existe, pide confirmación para crearlo
    * @returns {} void
    */
    function (periodo) {
        if ($("#PeriodoRemisionModel_Periodo").val().length === 7) {
            $.ajax({
                type: "POST",
                url: controlador + "ValidarPeriodo",
                data: {
                    periodo: periodo
                },
                dataType: "json",
                success: function (result) {
                    if (result === 1) {
                        //exito
                        if (!confirm('Deseas crear el periodo ingresado?'))
                            $("#PeriodoRemisionModel_Periodo").val("");
                        else {
                            crearPeriodo(periodo);
                        }
                    }else if (result === 0) {
                        //error
                        mostrarMensaje("mensaje", "El periodo ingresado, ya existe.", $tipoMensajeMensaje, $modoMensajeCuadro);
                        $("#PeriodoRemisionModel_Periodo").val("");
                    }
                    else {
                        mostrarMensaje("mensaje", "El periodo ingresado no puede ser mayor a la fecha actual.", $tipoMensajeMensaje, $modoMensajeCuadro);
                        $("#PeriodoRemisionModel_Periodo").val("");
                    }
                },
                error: function () {
                    mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
                }
            });
        }
    };

var crearPeriodo =
    /**
    * Crea el periodo
    * @returns {} 
    */
    function (periodo) {
        $.ajax({
            type: "POST",
            url: controlador + "CrearPeriodo",
            data: {
                periodo: periodo
            },
            dataType: "json",
            success: function (result) {
                if (result === 1) {
                    mostrarExito("Periodo creado con exito.");
                    $("#PeriodoImportacionModel_Periodo").prop("disabled", true);
                    pintarBusqueda();
                    document.getElementById('tdBotonRemitir').style.display = '';
                } else {
                    $("#PeriodoRemisionModel_Periodo").val("");
                }               
            },
            error: function () {
                mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    };

var pintarBusqueda =
    function () {
        if ($("#PeriodoRemisionModel_Periodo").val() === null) return;
        $.ajax({
            type: "POST",
            url: controlador + "listarEntidades",
            data: {
                periodo: $("#PeriodoRemisionModel_Periodo").val()
            },
            success: function (evt) {
                $("#listado").html(evt);
                table = $("#tabla").DataTable({
                    "scrollY": 314,
                    "scrollX": false,
                    "sDom": "t",
                    "ordering": false,
                    "bDestroy": true,
                    "bPaginate": false,
                    "iDisplayLength": 50
                });
                if ($("#remitir").length) {
                    $("#remitir").click(remitir);
                }
            },
            error: function (xhr) {
                mostrarError("Ocurrio un problema >> readyState: " + xhr.readyState + " | status: " + xhr.status + " | responseText: " + xhr.responseText);
            }
        });
    };

function mostrarError(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}

function mostrarExito(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}

function remitirRegistro(id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'remitirRegistro',
        data: {
            periodo: $("#PeriodoRemisionModel_Periodo").val(),
            tabla: id
        },
        dataType: 'json',
        success: function (result) {
            if (result.resultado == 0) {
                mostrarExito(result.mensaje);
            } else {
                mostrarError(result.mensaje);
            }
            pintarBusqueda();
        },
        error: function () {
            mostrarError('Ha ocurrido un error: Envio a Osinergmin - 1');
        }
    });
}

function remitirTodo() {
    var cadena = "";
    var checkboxes = document.getElementById('tbSeleccionados').getElementsByTagName('input');
    for (var i = 0; i < checkboxes.length; i++) {
        if (checkboxes[i].type == 'checkbox' && checkboxes[i].checked == true) {
            var valor = checkboxes[i].id;
            if (cadena == "") {
                cadena = valor;
            }
            else {
                cadena = cadena + "," + valor;
            }
        }
    }

    if (cadena != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'remitirTodo',
            data: {
                periodo: $("#PeriodoRemisionModel_Periodo").val(),
                cadena: cadena
            },
            dataType: 'json',
            success: function (result) {
                if (result.resultado == 0) {
                    mostrarExito(result.mensaje);
                } else {
                    mostrarError(result.mensaje);
                }
                pintarBusqueda();
            },
            error: function () {
                mostrarError('Ha ocurrido un error: Envío a Osinergmin - 2');
            }
        });
    } else {
        mostrarError('Por favor seleccione al menos un registro');
    }


}

function descargarLog(id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'Exportar',
        data: {
            periodo: $("#PeriodoRemisionModel_Periodo").val(),
            tabla: id
        },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                document.location.href = controlador + 'descargar?formato=1&file=' + result
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