var controlador = siteRoot + "intercambioOsinergmin/importaciones/";

$( /**
   * Llamadas inciales
   * @returns {} 
   */
    function () {

        $('#btnCancelar').click(function () {
            cancelar();
        });

        //Le damos el formato de datepicker a la caja de periodo
        $("#PeriodoImportacionModel_Periodo").Zebra_DatePicker({
            format: "m Y",    //Solo debe escoger el año y mes
            onSelect: function (periodo) {
                validarPeriodo(periodo);
            }
        });
    });

function cancelar() {
    window.location.href = controlador + "Index";
}

var validarPeriodo =
    /**
    * Valida que el periodo actual no exista, si no existe, pide confirmación para crearlo
    * @returns {} void
    */
    function (periodo) {
        if ($("#PeriodoImportacionModel_Periodo").val().length === 7) {
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
                            $("#PeriodoImportacionModel_Periodo").val("");
                        else {
                            crearPeriodo(periodo);
                        }
                    }
                    else if (result === 0) {
                        //error
                        mostrarMensaje("mensaje", "El periodo ingresado, ya existe.", $tipoMensajeMensaje, $modoMensajeCuadro);
                        $("#PeriodoImportacionModel_Periodo").val("");
                    }
                    else {
                        mostrarMensaje("mensaje", "El periodo ingresado no puede ser mayor a la fecha actual.", $tipoMensajeMensaje, $modoMensajeCuadro);
                        $("#PeriodoImportacionModel_Periodo").val("");
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

                    window.location.href = controlador + "Edit?periodo=" + periodo;

                    //pintarBusqueda();
                    //document.getElementById('tdBotonRemitir').style.display = '';
                } else {
                    $("#PeriodoRemisionModel_Periodo").val("");
                }
            },
            error: function () {
                mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
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
