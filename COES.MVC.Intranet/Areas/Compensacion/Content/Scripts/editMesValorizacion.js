var controlador = siteRoot + "compensacion/general/";

$( /**
   * Llamadas iniciales
   * @returns {} 
   */
    $(document).ready(function () {

        //$('#btnCancelar').click(function () {
        //    cancelar();
        //});

        $('#btnGrabar').click(function () {
            grabarMesValorizacion();
        });

        //$('#btnCerrarPeriodo').click(function () {
        //    grabarEstadoPeriodo('0');
        //});
        //$('#btnAbrirPeriodo').click(function () {
        //    grabarEstadoPeriodo('1');
        //});

        //$('#btnNuevoCalculo').click(function () {
        //    crearMesValorizacion();
        //});

        //$('#btnInicializar').click(function () {
        //    inicializarMesValorizacion();
        //});


        //$("#pecacodi").change(function () {
        //    ObtenerRegistroPeriodoCalculo();
        //    llenarListadoProcesos();
        //});

        /** Nuevos botones*/
        $('#btnRegresar').click(function () {
            regresar();
        });
        $('#btnRefrescar').click(function () {
            refrescar();
        });

        $('#btnNuevo').click(function () {
            nuevo();
        });

        $('#btnCancelar').click(function () {
            cancelar();
        });

        listarVersionesPeriodo();

        //Inicializamos la pantalla
        //ObtenerRegistroPeriodoCalculo();
        //llenarListadoProcesos();
        //VerificarPeriodosActivos();



    }));

function verProcesos(pecacodi, pecanombre) {
    //alert(pecacodi);
    document.getElementById('lblTituloEntidades').innerHTML = 'Datos de la versión: ' + pecanombre;
    llenarListadoProcesos(pecacodi);
}

function ObtenerRegistroPeriodoCalculo() {

    var valor = $("#pecacodi").val();
    if (valor != null && valor != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerRegistroPeriodoCalculo',
            data: {
                pecacodi: valor
            },
            dataType: 'json',
            success: function (data) {
                //Actualizamos los datos de la pantalla
                if (data != null) {
                    $("#pecanombre").val(data.PecaNombre);
                    $("#tc").val(data.PecaTipoCambio);
                    $("#version").val(data.PecaVersionVtea);
                    $("#estado").val(1);
                    //$('input[id=estado][value="' + data.PecaEstRegistro + '"]').prop('checked', true).trigger('change');


                    if (data.PecaEstRegistro == "0") {
                        $("#btnInicializar").hide();
                        $("#btnGrabar").hide();
                        $("#btnCerrarPeriodo").hide();
                        $("#btnAbrirPeriodo").show();

                        $("#pecanombre").attr('disabled', 'disabled');
                        $("#tc").attr('disabled', 'disabled');
                        $("#version").attr('disabled', 'disabled');
                        $("#estado").val('Cerrado');

                    }
                    else {
                        $("#btnInicializar").show();
                        $("#btnGrabar").show();
                        $("#btnCerrarPeriodo").show();
                        $("#btnAbrirPeriodo").hide();

                        $("#pecanombre").removeAttr('disabled');
                        $("#tc").removeAttr('disabled');
                        $("#version").removeAttr('disabled');
                        $("#estado").val('En proceso');
                    }
                }
                else {
                    $("#btnInicializar").hide();
                    $("#btnGrabar").hide();
                    $("#btnCerrarPeriodo").hide();
                    $("#btnAbrirPeriodo").hide();

                    $("#pecanombre").attr('disabled', 'disabled');
                    $("#tc").attr('disabled', 'disabled');
                    $("#version").attr('disabled', 'disabled');

                }
            },
            error: function (response) {
                mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    } else {
        $("#btnInicializar").hide();
        $("#btnGrabar").hide();
        $("#btnCerrarPeriodo").hide();
        $("#btnAbrirPeriodo").hide();

        $("#pecanombre").attr('disabled', 'disabled');
        $("#tc").attr('disabled', 'disabled');
        $("#version").attr('disabled', 'disabled');
    }
}

function VerificarPeriodosActivos() {

    var valor = $("#pericodi").val();
    if (valor != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'ObtenerNroCalculosActivosPeriodo',
            data: {
                pericodi: valor
            },
            dataType: 'json',
            success: function (data) {
                //Actualizamos los datos de la pantalla
                if (data > 0) {
                    $("#btnNuevoCalculo").hide();
                }
                else {
                    $("#btnNuevoCalculo").show();
                }


            },
            error: function (response) {
                mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    }
}
var llenarListadoProcesos =
    /**
    * Pinta el listado de periodos según el año seleccionado
    * @returns {} 
    */
    function (pecacodi) {


        $.ajax({
            type: "POST",
            url: controlador + "ListarEntidades",
            data: {
                pecacodi: pecacodi
            },
            success: function (evt) {
                //document.getElementById('lblTituloEntidades').innerHTML = 'Datos de la versión: ' + evt.VcePeriodoCalculoDTO.PecaNombre;
                fillGridProcesos(evt);
            },
            error: function () {
                mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    };

function fillGridProcesos(data) {

    document.getElementById('tbListado').style.display = '';

    $("#listado").html(data);
    $("#tabla").dataTable({
        "scrollY": 314,
        "scrollX": false,
        "sDom": "t",
        "ordering": false,
        "bDestroy": true,
        "bPaginate": false,
        "iDisplayLength": 50
    });
}


function obtenerData(id, proceso, pecacodi) {

    var metodo = "";

    if (proceso == "VCE_ENERGIA") {
        metodo = "obtenerDataEnergia";
    }

    if (proceso == "VCE_HORA_OPERACION") {
        metodo = "obtenerDataHorasOperacion";
    }

    if (proceso == "TRN_COSTO_MARGINAL") {
        metodo = "";
    }

    if (proceso == "VCE_DATCALCULO") {
        metodo = "obtenerDataCalculo";
    }

    if (metodo == "") {
        alert("Proceso no registrado");
        return false;
    }

    $.ajax({
        type: 'POST',
        url: controlador + metodo,
        data: {
            id: id,
            pecacodi: pecacodi
        },
        dataType: 'json',
        beforeSend: function () {
            mostrarAlerta("Obteniendo Información ...");
        },
        success: function (result) {
            mostrarExito("Registros obtenidos.");
            llenarListadoProcesos(pecacodi);
        },
        error: function () {
            mostrarError();
        }
    });
}

function crearMesValorizacion() {

    if (confirm("¿Está seguro de crear un nuevo cálculo?")) {

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + "CrearMesValorizacion",
            data: {
                pericodi: $('#pericodi').val(),
                pecanombre: $('#pecanombre').val(),
                tipocambio: $('#tc').val(),
                version: $('#version').val(),
                motivo: $('#motivo').val()
            },
            beforeSend: function () {
                mostrarAlerta("Creando cálculo ..");
            },
            success: function (evt) {
                alert("Registro creado correctamente.");
                //Volvemos a poblar los periodos;
                //dwr.util.removeAllOptions("pecacodi");
                //dwr.util.addOptions("pecacodi", evt.ListVcePeriodoCalculo, 'PecaCodi', 'PecaNombre');
                //dwr.util.setValue("pecacodi", evt.VcePeriodoCalculoDTO.PecaCodi);

                listarVersionesPeriodo();
                $('#popupEdicion').bPopup().close();
                //ObtenerRegistroPeriodoCalculo();
                //llenarListadoProcesos();
                //VerificarPeriodosActivos();
            },
            error: function () {
                mostrarError("Error al crear registro");
            }
        });
    }
}

function inicializarMesValorizacion(pecacodi) {


    if (confirm("¿Está seguro de inicializar toda la data del Cálculo?")) {


        $.ajax({
            type: 'POST',
            url: controlador + "InicializarMesValorizacion",
            data: {
                pecacodi: pecacodi
            },
            beforeSend: function () {
                mostrarAlerta("Inicializando cálculo ..");
            },
            success: function (evt) {
                mostrarExito("Base de datos actualizada correctamente.");
                //Volvemos a poblar la grilla
                //documet.getElementById('lblTituloEntidades').innerHTML = 'Datos de la versión: ' + evt.VcePeriodoCalculoDTO.PecaNombre;
                fillGridProcesos(evt);
            },
            error: function () {
                mostrarError("Error al crear registro");
            }
        });
    }
}


function grabarMesValorizacion() {

    if ($('#pecanombre').val() == "" || $('#pecanombre').val() == null) {
        alert("Debe ingresar un nombre para el periodo de cálculo");
        return false;
    }

    if ($('#tc').val() == "" || $('#tc').val() == null) {
        alert("Debe ingresar el tipo de cambio.");
        return false;
    }

    if ($('#version').val() == "" || $('#version').val() == null) {
        alert("Debe seleccionar una Revisión de VTEA.");
        return false;
    }

    var hdnPecacodi = $('#hdnPecacodi').val();

    if (hdnPecacodi == 0) {
        crearMesValorizacion();
    } else {
        if (confirm("¿Desea guardar la información?")) {

            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: controlador + "GrabarMesValorizacion",
                data: {
                    pecacodi: hdnPecacodi,
                    pecanombre: $('#pecanombre').val(),
                    tc: $('#tc').val(),
                    version: $('#version').val(),
                    estado: $('#estado').val(),
                    motivo: $('#motivo').val()
                },
                beforeSend: function () {
                    mostrarAlerta("Guardando Información ..");
                },
                success: function (evt) {
                    if (evt.msg != null) {
                        mostrarError(evt.msg);
                    }
                    else {
                        alert("Registro actualizado correctamente.");
                        $('#popupEdicion').bPopup().close();
                        $("#hdnPecacodi").val(0);
                        refrescar();
                        //llenarListadoProcesos();
                    }
                },
                error: function () {
                    mostrarError();
                }
            });
        }
    }


}

function grabarEstadoPeriodo(nuevoEstado, pecacodi) {

    //if ($('#pecacodi').val() == "" || $('#pecacodi').val() == null) {
    //    alert("Debe seleccionar el periodo de cálculo");
    //    return false;
    //}

    var msg = '¿Está seguro de cerrar el periodo?';
    if (nuevoEstado == '1')
        msg = '¿Está seguro de abrir el periodo?';

    if (confirm(msg)) {

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + "GrabarEstadoValorizacion",
            data: {
                pecacodi: pecacodi,
                estado: nuevoEstado
            },
            beforeSend: function () {
                mostrarAlerta("Guardando Información ..");
            },
            success: function (evt) {
                if (evt.msg != null) {
                    mostrarError(evt.msg);
                }
                else {
                    alert("Registro actualizado correctamente.");
                    listarVersionesPeriodo();
                    //ObtenerRegistroPeriodoCalculo();
                    //llenarListadoProcesos();
                }
                //VerificarPeriodosActivos();
            },
            error: function () {
                mostrarError();
            }
        });
    }
}
function soloNumeros(e) {
    var key = window.Event ? e.which : e.keyCode
    return (key <= 13 || (key >= 48 && key <= 57) || key == 46);
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

function limpiarMensaje() {
    $('#mensaje').removeClass();
    $('#mensaje').html('');
    //$('#mensaje').addClass('action-exito');
}

var editarRegistro = function (id) {
    window.location.href = controlador + "EditMesValorizacion?periodo=" + id;
}

function cancelar() {
    $('#popupEdicion').bPopup().close();
}

function regresar() {
    window.location.href = controlador + "MesValorizacion";
}
function refrescar() {
    window.location.reload();
}

function listarVersionesPeriodo() {
    $.ajax({
        type: "POST",
        url: controlador + "ListarVersionPeriodo",
        data: {
            pericodi: $("#pericodi").val()
        },
        success: function (data) {
            $("#listadoVersiones").html(data);
            $("#tablaVersiones").dataTable({
                "scrollY": 314,
                "scrollX": false,
                "sDom": "t",
                "ordering": false,
                "bDestroy": true,
                "bPaginate": false,
                "iDisplayLength": 50
            });
        },
        error: function () {
            mostrarMensaje("error al cargar datos", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
        }
    });
}

function nuevo() {
    $("#hdnPecacodi").val(0);

    $("#popupEdicion").bPopup({
        autoClose: false
    });
}

function editarPeriodo(pecacodi) {
    $("#hdnPecacodi").val(pecacodi);

    if (pecacodi != null && pecacodi != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerRegistroPeriodoCalculo',
            data: {
                pecacodi: pecacodi
            },
            dataType: 'json',
            success: function (data) {
                //Actualizamos los datos de la pantalla
                if (data != null) {
                    $("#pecanombre").val(data.PecaNombre);
                    $("#tc").val(data.PecaTipoCambio);
                    $("#version").val(data.PecaVersionVtea);
                    $("#motivo").val(data.PecaMotivo);
                    //$('input[id=estado][value="' + data.PecaEstRegistro + '"]').prop('checked', true).trigger('change');


                    if (data.PecaEstRegistro == "0") {
                        $("#estado").val('Cerrado');

                    }
                    else {

                        $("#estado").val('En proceso');
                    }

                    $("#popupEdicion").bPopup({
                        autoClose: false
                    });
                }

            },
            error: function (response) {
                mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    } else {
        //$("#btnInicializar").hide();
        //$("#btnGrabar").hide();
        //$("#btnCerrarPeriodo").hide();
        //$("#btnAbrirPeriodo").hide();

        $("#pecanombre").attr('disabled', 'disabled');
        $("#tc").attr('disabled', 'disabled');
        $("#version").attr('disabled', 'disabled');
    }
}

function eliminarPeriodo(pecacodi, estado) {

    if (pecacodi != null && pecacodi != "") {
        if (eliminarPeriodo != "Cerrado") {
            var msg = '¿Está seguro de eliminar la versión?';

            if (confirm(msg)) {

                $.ajax({
                    type: 'POST',
                    url: controlador + 'DeletePeriodo',
                    data: {
                        pecacodi: pecacodi
                    },
                    dataType: 'json',
                    success: function (data) {
                        //Actualizamos los datos de la pantalla
                        if (data.success) {

                            listarVersionesPeriodo();

                        } else {
                            mostrarMensaje('mensaje', 'No se logró eliminar registro.', $tipoMensajeError, $modoMensajeCuadro);
                        }

                    },
                    error: function (response) {
                        mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
                    }
                });

            }
        } else {
            mostrarMensaje('mensaje', 'Versión Cerrada, no puede ser eliminada!', $tipoMensajeError, $modoMensajeCuadro);
        }

    } 
}