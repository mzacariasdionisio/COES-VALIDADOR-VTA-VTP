var controlador = siteRoot + 'coordinacion/asociacionempresa/';

$(function () {

    $('#btnExportar').on('click', function () {
        exportar();
    });

    $('#btnNuevo').on('click', function () {
        agregar();
    });

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $("#btnGrabarNuevo").click(function () {
        grabarNuevo();   
    });

    $('#btnCancelarNuevo').click(function () {
        $('#popupAsociacion').bPopup().close();
    });

    $('#btnGrabarScada').click(function () {
        actuaizarNombreScada();
    });

    $('#btnCancelarScada').click(function () {
        $('#popupEditar').bPopup().close();
    });

    consultar();
});

grabarNuevo = function () {

    $.ajax({
        type: 'POST',
        url: controlador + "grabar",
        data: {
            emprcodi: $("#cbEmpresa").val(),
            emprcodisp7: $("#cbEmpresasp7").val()
        },
        success: function (result) {
            if (result == 1) {

                $('#mensajeAsociacion').removeClass();
                $('#mensajeAsociacion').html("Los datos han sido guardados correctamente");
                $('#mensajeAsociacion').addClass('action-exito');

                setTimeout(function () {
                    $('#popupAsociacion').bPopup().close();

                }, 50);
                consultar();
            } else {
                mostrarMensaje('mensaje', 'error', 'No se pudo actualizar el registro.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'No se pudo actualizar el registro.');
        }
    });

};


consultar = function () {

    $.ajax({
        type: 'POST',
        url: controlador + 'listado',
        data: {
            nombre: $('#txtNombre').val()
        },
        success: function (evt) {
            $('#listado').html(evt);
            $('#tabla').dataTable({
                "columnDefs": [
                    {
                        "targets": 0,
                        "width": "50px"
                    },
                    {
                        "targets": 1,
                        "width": "150px"
                    },
                    {
                        "targets": 2,
                        "width": "350px"
                    },
                    {
                        "targets": 3,
                        "width": "150px"
                    },
                    {
                        "targets": 4,
                        "width": "350px"
                    },
                    {
                        "targets": 5,
                        "width": "150px"
                    },
                    {
                        "targets": 6,
                        "width": "150px"
                    }
                ],
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
};


agregar = function () {

    $("#cbEmpresa").val("-1");
    $("#cbEmpresasp7").val("-1");

    setTimeout(function () {
        $('#popupAsociacion').bPopup({
            autoClose: false,
        });
    }, 50);
};


eliminar = function (emprcodi, emprcodisic) {
    if (confirm('¿Está seguro de realizar esta operación?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'eliminar',
            data: {
                emprcodi: emprcodi,
                emprcodisic: emprcodisic
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'La operación se realizó correctamente.');
                    consultar();
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'No se pudo eliminar el registro.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'No se pudo eliminar el registro.');
            }
        });
    }
};

editar = function (emprcodi, emprnomb) {
    $('#hfCodigoEmpresaScada').val(emprcodi);
    $('#txtNombreScada').val(emprnomb);
    $('#popupEditar').bPopup({
        autoClose: false
    });
};

actuaizarNombreScada = function () {

    if ($('#txtNombreScada').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'actualizarsp7',
            data: {
                emprcodi: $('#hfCodigoEmpresaScada').val(),
                emprnomb: $('#txtNombreScada').val()
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'La operación se realizó correctamente.');
                    $('#popupEditar').bPopup().close();
                    consultar();
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
};


exportar = function () {

    $.ajax({
        type: 'POST',
        url: controlador + 'exportar',
        data: {
            nombre: $('#txtNombre').val()
        },
        success: function (resultado) {
            if (resultado == 1) {
                location.href = controlador + "descargar";
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });

};

/*===Funciones Generales===*/

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};

getFecha = function (date) {
    var parts = date.split("/");
    var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date.getTime();
};
