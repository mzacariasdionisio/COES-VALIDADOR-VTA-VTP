var controlador = siteRoot + "rolturnos/configuracion/";
$(function () {

    $('#tab-container').easytabs({
        animate: false
    });

    $('#tab-container').bind('easytabs:before', function (id, val, t) {
        limpiarMensaje('mensaje');
    });

    $('#txtAnio').Zebra_DatePicker({
        format: 'Y'
    });

    $('#cbMes').val($('#hfMes').val());

    $('#btnConsultar').on('click', function () {
        cargarConfiguracion();
    });

    $('#btnNuevoActividad').on('click', function () {
        editarActividad(0);
    });

    $('#btnAgregarGrupo').on('click', function () {
        agregarGrupo();
    });

    $('#btnGrabarConfiguracion').on('click', function () {
        grabarConfiguracion();
    });

    $('#btnRegresar').on('click', function () {
        document.location.href = siteRoot + 'rolturnos/rolturno/index';
    });


    cargarConfiguracion();
    cargarActividades();
});

function cargarConfiguracion() {
    limpiarMensaje('mensaje');
    $.ajax({
        type: 'POST',
        url: controlador + "ConfiguracionPersonal",
        datatype: "json",
        data: {
            anio: $('#txtAnio').val(),
            mes: $('#cbMes').val()
        },
        success: function (model) {
            $('#personalConfiguracion').html(model);

            $(".personal-grupo").sortable({
                connectWith: ".personal-grupo",
                cursor: "move",
                helper: "clone",
                items: "> div",
                stop: function (event, ui) {

                }
            }).disableSelection();

            $(".person-drag-drop").sortable({
                connectWith: ".person-drag-drop",
                cursor: "move",
                helper: "clone",
                items: "> div",
                stop: function (event, ui) {

                }
            }).disableSelection();

            $('.eliminar-grupo').on('click', function () {
                eliminarGrupo();
            });
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Lo sentimos, se ha producido un error al obtener las fechas de operacion');
        }
    });
}

function agregarGrupo() {

    var countGroup = $(".personal-grupo-item").length + 1;
    var htmlGrupo =
        "<div class='personal-grupo-item' id='grupo-n-" + countGroup + "'> " +
        "    <div class='grupo-options'>                                    " +
        "       <div>Tipo de grupo:</div>                                   " +
        "       <div>                                                       " +
        "           <select id='cbTipoGrupo'>                               " +
        "              <option value='P' '>Programadores</option>           " +
        "              <option value='E')'>Especialistas</option>           " +
        "              <option value='S')'>Subdirector</option>             " +
        "              <option value='O')'>Otros</option>                   " +
        "           </select>                                               " +
        "       </div>                                                      " +
        "       <div><a href=\"JavaScript:eliminarGrupo('grupo-n-" + countGroup + "');\">Eliminar grupo</a></div> " +
        "    </div>                                                         " +
        "    <div class='grupo-person-list person-drag-drop'>               " +
        "    </div>                                                         " +
        "</div>                                                             ";

    $('.personal-grupo').append(htmlGrupo);
    $(".person-drag-drop").sortable({
        connectWith: ".person-drag-drop",
        cursor: "move",
        helper: "clone",
        items: "> div",
        stop: function (event, ui) {

        }
    }).disableSelection();
}

function eliminarGrupo(id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {

        $('#' + id + " .grupo-person-list >div").detach().appendTo('.personal-list');
        $('#' + id).remove();
    }
}

function eliminarPersonal(id) {
    console.log("eliminar");
    $('#persona-' + id).detach().appendTo('.personal-list');
}

function grabarConfiguracion() {
    var mensaje = validarConfiguracion();
    if (mensaje == "") {
        var data = [];
        $('.personal-grupo-item').each(function () {

            var listaPerson = [];
            $('#' + $(this).attr('id') + ' .grupo-person-list .personal-seleccion').each(function () {
                listaPerson.push($(this).attr('data-id'));
            });
            var item = {
                TipoGrupo: $(this).find('select').val(),
                ListaPersonas: listaPerson
            };
            data.push(item);
        });

        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarConfiguracion',
            contentType: 'application/json',
            data: JSON.stringify({
                data: data,
                anio: $('#txtAnio').val(),
                mes: $('#cbMes').val(),
            }),
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    cargarConfiguracion();
                    mostrarMensaje('mensaje', 'exito', 'La configuración de personal de realizó correctamente.');
                }
                else
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', mensaje);
    }
}

function validarConfiguracion() {
    var countGroup = $(".personal-grupo-item").length;
    var mensaje = "";
    if (countGroup == 0) {
        mensaje = "Debe agregar al menos un grupo de personal en la configuración";
    }
    else {
        $('.personal-grupo-item').each(function () {
            var countPerson = 0;
            $('#' + $(this).attr('id') + ' .grupo-person-list .personal-seleccion').each(function () {
                var idPerson = $(this).attr('data-id');
                countPerson++;
            });

            if (countPerson == 0) {
                mensaje = "Debe agregar al menos una persona a cada grupo creado";
            }
        });
    }

    return mensaje;
}

function cargarActividades() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListarActividad',
        success: function (evt) {
            $('#listadoActividad').html(evt);
            $('#tablaActividad').dataTable({
                "iDisplayLength": 25
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function editarActividad(id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'EditarActividad',
        data: {
            id: id
        },
        global: false,
        success: function (evt) {
            $('#contenidoActividad').html(evt);
            setTimeout(function () {
                $('#popupActividad').bPopup({
                    autoClose: false,
                    modalClose: false,
                    escClose: false,
                    follow: [false, false]
                });
            }, 50);

            $('#btnGrabarActividad').on("click", function () {
                grabarActividad();
            });

            $('#btnCancelarActividad').on("click", function () {
                $('#popupActividad').bPopup().close();
            });

            $('#cbReporte').val($('#hfReporte').val());
            $('#cbEstadoActividad').val($('#hfEstadoActividad').val());
            $('#cbTipoResponsable').val($('#hfTipoResponsable').val());
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function eliminarActividad(id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'EliminarActividad',
            data: {
                id: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'El registro se eliminó correctamente.');
                    cargarActividades();
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function grabarActividad() {
    var validacion = validarActividad();

    if (validacion == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarActividad',
            data: $('#frmRegistroActividad').serialize(),
            dataType: 'json',
            global: false,
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'Los datos de la actividad se grabaron correctamente.');
                    $('#popupActividad').bPopup().close();
                    cargarActividades();
                }
                else {
                    mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeEdicion', 'alert', validacion);
    }
}

function validarActividad() {
    var html = "<ul>";
    var flag = true;

    if ($('#txtAbreviaturaActividad').val() == "") {
        html = html + "<li>Debe ingresar la abreviatura.</li>";
        flag = false;
    }

    if ($('#txtDescripcionActividad').val() == "") {
        html = html + "<li>Debe ingresar la descripción.</li>";
        flag = false;
    }

    html = html + "</ul>";

    if (flag) {
        html = "";
    }
    return html;
}

function mostrarMensaje(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

function limpiarMensaje(id) {
    $('#' + id).removeClass('action-alert');
    $('#' + id).removeClass('action-exito');
    $('#' + id).removeClass('action-message');
    $('#' + id).removeClass('action-error');
    $('#' + id).html('');
}