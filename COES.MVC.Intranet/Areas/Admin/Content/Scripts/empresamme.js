var controlador = siteRoot + 'admin/empresa/';

$(function () {

    $('#txtFecParitcipacion').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "date"
    });

    $('#txtFecParitcipacion').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtFecParitcipacion').val(date);
        }
    });

    $('#txtFecRetiro').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "date"
    });

    $('#txtFecRetiro').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtFecRetiro').val(date);
        }
    });


    $('#btnConsultarMME').click(function () {
        buscarMME();
    })

    $('#cntBusquedaMME').keypress(function (e) {
        if (e.keyCode == '13') {
            $('#btnConsultarMME').click();
        }
    });

    $('#btnNuevoMME').click(function () {
        editarEmpresaMME(0,0);
    });

    buscarMME();
});

buscarMME = function () {
    pintarBusquedaMME(1);
}

pintarBusquedaMME = function (nroPagina) {
    $('#hfNroPaginaMME').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "listadomme",
        data: {
            nombre: $('#txtNombreFiltroMME').val(),
            ruc: $('#txtRucFiltroMME').val(),
            idTipoEmpresa: $('#cbTipoEmpresaFiltroMME').val(),
            nroPagina: nroPagina,
            indicadorGrabar: $('#hfIndicadorGrabarMME').val()
        },
        success: function (evt) {
            $('#listadoMME').css("width", $('#mainLayoutMME').width() + "px");
            $('#listadoMME').html(evt);
            $('#tablaMME').dataTable({
                "scrollY": 457,
                "scrollX": false,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50
            });
        },
        error: function () {
            mostrarError('mensajeMME');
        }
    });
}

editarEmpresaMME = function (mmeid, emprid) {
    
    $.ajax({
        type: 'POST',
        url: controlador + 'editarMME',
        data: {
            EmprMME: mmeid,
            EmprCodi: emprid,
            indicadorGrabar: $('#hfIndicadorGrabarMME').val(),
            tipo: $('#cbTipoEmpresaFiltroMME').val()
        },
        success: function (evt) {
            $('#contenidoDetalleMME').html(evt);

            $('#hfTipoEmprCodiMME').val($('#cbTipoEmpresaFiltroMME').val());
            if (mmeid > 0) $('#cbEmpresaMME').prop('disabled', true);
            if (mmeid > 0 && emprid > 0) $('#cbEmpresaMME').val(emprid);
               
            $('#btnGrabarMME').click(function () {
                grabarMME();
            })

            $('#btnCancelarMME').click(function () {
                cancelarMME();
            });

            setTimeout(function () {
                $('#popupDetalleMME').bPopup({
                    autoClose: false
                });
            }, 500);
        },
        error: function () {
            mostrarError('mensajeMME');
        }
    });
};
var flag = false;

historialMME = function (idEmpresa) {

    $.ajax({
        type: 'POST',
        url: controlador + 'HistorialMME',
        data: {
            Emprcodi: idEmpresa
        },
        success: function (evt) {
            $('#contenidoAuditoriaMME').html(evt);

            setTimeout(function () {
                $('#popupAuditoriaMME').bPopup({
                    autoClose: false
                });
            }, 500);
        },
        error: function () {
            mostrarError('mensajeMME');
        }
    })

}

darBajaEmpresa = function (id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'darbaja',
            data: {
                idEmpresa: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarExito('La operación se realizó correctamente.', 'mensajeMME');
                    buscar();
                }
                else {
                    mostrarError('mensajeMME');
                }
            },
            error: function () {
                mostrarError('mensajeMME');
            }
        });
    }
}

eliminarEmpresa = function (id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'eliminar',
            data: {
                idEmpresa: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarExito('La operación se realizó correctamente.', 'mensajeMME');
                    buscar();
                }
                else {
                    mostrarError('mensajeMME');
                }
            },
            error: function () {
                mostrarError('mensajeMME');
            }
        });
    }
}

grabarMME = function () {
    var mensaje = validar();

    if (mensaje == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'grabarmme',
            data: $('#frmRegistro').serialize(),
            dataType: 'json',
            success: function (result) {
                if (result.IdResultado == 1) {
                    mostrarExito('La operación se realizó correctamente.', 'mensajeMME');

                    buscarMME();
                    cancelarMME();
                }
                else if (result.IdResultado == 2) {
                    mostrarValidaciones(result.Validaciones);
                }
                else {
                    mostrarError('mensajeEditMME');
                }
            },
            error: function () {
                mostrarError('mensajeEditMME');
            }
        });
    }
    else {
        mostrarAlerta(mensaje, 'mensajeEditMME');
    }
}



validar = function () {
    var mensaje = "<ul>";
    var flag = true;

    if ($('#cbEmpresaMME').val() == "-2") {
        mensaje = mensaje + "<li>Seleccionar Empresa</li>";
        flag = false;
    }

    if ($('#txtFecParitcipacion').val() == "") {
        mensaje = mensaje + "<li>Ingrese Fecha de Participación</li>";
        flag = false;
    }

    mensaje = mensaje + "</ul>";

    if (flag) mensaje = "";

    return mensaje;
}

mostrarValidaciones = function (validaciones) {
    var mensaje = "<ul>";
    for (var j in validaciones) {
        mensaje = mensaje + "<li>" + validaciones[j] + "</li>";
    }
    mensaje = mensaje + "</ul>";

    mostrarAlerta(mensaje, 'mensajeEditMME');
}

cancelarMME = function () {
    $('#popupDetalleMME').bPopup().close();
}

mostrarError = function (id) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-error');
    $('#' + id).text('Ha ocurrido un error.');
}

mostrarExito = function (mensaje, id) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-exito');
    $('#' + id).html(mensaje);
}

mostrarAlerta = function (mensaje, id) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-alert');
    $('#' + id).html(mensaje);
}