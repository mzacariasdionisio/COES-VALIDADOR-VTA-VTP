var controlador = siteRoot + 'admin/integrante/';

$(function () {

    $('#btnGenerarMasivo').on('click', function () {
        generarMasivo();
    });

    $('#btnConsultar').on('click', function () {
        buscar();
    });

    $('#cbTipo').on('change', function () {
        buscar();
    });

    $('#btnExportar').on('click', function () {
        exportar();
    });
             

    buscar();
});



buscar = function () {
    pintarPaginado();
    pintarBusqueda(1);
};

pintarPaginado = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "paginado",
        data: {
            tipoAgente: $('#cbTipoAgente').val(),
            tipoEmpresa: $('#cbTipoEmpresa').val(),
            indicador: $('#cbUsuarioTramite').val(),
            ruc: $('#txtRUC').val(),
            razonSocial: $('#txtRazonSocial').val()
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            mostrarError('mensaje');
        }
    });
}

pintarBusqueda = function (nroPagina) {
    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "listado",
        data: {
            tipoAgente: $('#cbTipoAgente').val(),
            tipoEmpresa: $('#cbTipoEmpresa').val(),
            indicador: $('#cbUsuarioTramite').val(),
            ruc: $('#txtRUC').val(),
            razonSocial: $('#txtRazonSocial').val(),
            nroPagina: nroPagina
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 457,
                "scrollX": false,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50
            });
        },
        error: function () {
            mostrarError('mensaje');
        }
    });
}

verCorreos = function (idEmpresa, int) {
    var enlace = controlador + 'detalle?id=' + idEmpresa + '&ind='+int;
    window.open(enlace, '_blank');
};

generarMasivo = function () {
    if (confirm('¿Está seguro de realizar esta acción?')) {

        var puntos = "";
        $('#tabla tbody tr').each(function (i, row) {
            var $actualrow = $(row);
            $checkbox = $actualrow.find('input:checked');
            if ($checkbox.is(':checked')) {
                puntos = puntos + $checkbox.val() + ",";
            }
        });

        $.ajax({
            type: 'POST',
            url: controlador + 'generarmasivo',
            data: {
                empresas: puntos,
                tipo: $('#cbTipo').val()
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'Operación realizada correctamente.');
                }
                if (result == -1) {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
};

enviarCredenciales = function (id) {
    if (confirm('Se va a generar una nueva clave para el acceso al Portal de Trámite Virtual y se enviará esta a los correos electrónicos de los usuarios registrados de la empresa.¿Está seguro ?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'crearcredenciales',
            data: {
                idEmpresa: id
            },
            dataType: 'json',
            global: false,
            success: function (result) {               
                if (result == 1) {                   
                    mostrarMensaje('mensaje', 'exito', 'Operación realizada correctamente.');
                    buscar();
                }
                else if (result == -2) {
                    mostrarMensaje('mensaje', 'alert', 'No se puede crear la cuenta ya que no existe cuenta de correo.');
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
        url: controlador + "exportar",
        data: {
            tipoAgente: $('#cbTipoAgente').val(),
            tipoEmpresa: $('#cbTipoEmpresa').val()
        },
        dataType: 'json',
        cache: false,
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
}

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}