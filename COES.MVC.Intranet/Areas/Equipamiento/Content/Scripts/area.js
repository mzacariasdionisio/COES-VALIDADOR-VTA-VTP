var controlador = siteRoot + 'Equipamiento/';
$(function () {

    $('#cbTipoArea').val($("#idTipoArea").val());
    buscarArea();

    $('#btnBuscar').click(function () {
        buscarArea();
    });
    $("#popupArea").addClass("general-popup");
    $("#popUpDetalleArea").addClass("general-popup");
    $("#popUpEditarArea").addClass("general-popup");

    $('#btnNuevo').click(function () {
        nuevaArea();
    });
    $('#NombreArea').keyup(function (e) {
        if (e.keyCode == 13) {
            buscarArea();
        }
    });

});
function buscarArea() {
    ocultarMensaje();
    mostrarListado(1);
}

function mostrarDetalle(e) {
    //window.location.href = controlador + "Area/Detalle?id=" + e;
    $.ajax({
        type: 'POST',
        url: controlador + "Area/Detalle?id=" + e,
        success: function (evt) {
            $('#detalleArea').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popUpDetalleArea').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function () {
            mostrarError();
        }
    });
}
function editarArea(e) {
    $.ajax({
        type: 'POST',
        url: controlador + "Area/Editar?id=" + e,
        success: function (evt) {
            $('#editarArea').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popUpEditarArea').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function () {
            mostrarError();
        }
    });
};
function Eliminar(e) {
    window.location.href = controlador + "Area/Detalle?idx=" + e;
};
nuevaArea = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "Area/NuevaArea",
        success: function (evt) {
            $('#nuevaarea').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupArea').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function () {
            mostrarError();
        }
    });
};
function pintarPaginado() {
    $.ajax({
        type: 'POST',
        url: controlador + "Area/paginado",
        data: $('#frmBusqueda').serialize(),
        success: function (evt) {
            $('#paginado').html(evt);

        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}
function mostrarListado(nroPagina) {
    $('#hfNroPagina').val(nroPagina);

    $.ajax({
        type: 'POST',
        url: controlador + "Area/ListarAreas",
        data: $('#frmBusqueda').serialize(),
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
};
guardarArea = function () {
    if (confirm('¿Está seguro de agregar la nueva ubicación?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'Area/GuardarArea',
            dataType: 'json',
            data: {
                iTipoArea: $('#cbTipoAreaNuevo').val(),
                sNombreArea: $('#txtNombreArea').val(),
                sAreaAbrev: $('#txtAbreviaturaArea').val(),
                sEstado: $('#cbEstadoNuevo').val(),
            },
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    mostrarExitoOperacion();
                    $('#popupArea').bPopup().close();
                    mostrarListado(1);
                } else
                    mostrarError();
            },
            error: function () {
                mostrarError();
            }
        });
    }
};
 actualizar = function () {
        if (confirm('¿Está seguro de actualizar la ubicación?')) {
            $.ajax({
                type: 'POST',
                url: controlador + "Area/EditarPost",
                data: {
                    iAreaCodi: $("#Areacodi").val(),
                    sAreanomb: $("#Areanomb").val(),
                    sAreaabrev: $("#Areaabrev").val(),
                    idTipoArea: $("#idTipoAreaDet").val(),
                    sEstado: $("#cbEstado").val()
                },
                success: function (resultado) {
                    if (resultado == 1) {
                        mostrarExitoOperacion();
                        $('#popUpEditarArea').bPopup().close();
                        mostrarListado(1);
                    } else
                        mostrarError();
                },
                error: function () {
                    alert("Ha ocurrido un error");
                }
            });
        }
    };
mostrarError = function () {
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
    $('#mensaje').css("display", "block");
};
ocultarMensaje = function() {
    $('#mensaje').css("display", "none");
};
mostrarExitoOperacion = function () {
    $('#mensaje').removeClass("action-error");
    $('#mensaje').addClass("action-exito");
    $('#mensaje').text("Se realizó la operación..!");
    $('#mensaje').css("display", "block");
};