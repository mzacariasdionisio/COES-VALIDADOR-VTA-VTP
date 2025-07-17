var controlador = siteRoot + 'admin/reporteusuario/'

$(function () {
       
    $('#btnConsultar').click(function () {
        consultar();
    });

    $('#btnExportar').click(function () {
        exportar();
    });

    consultar();
});

consultar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'consultausuario',
        data: {
            empresa: $('#cbEmpresa').val(),
            estado: $('#cbEstado').val()
        },
        dataType: 'json',
        success: function (result) {
            $('#listado').html(result);           
        },
        error: function () {
            mostrarError();
        }
    });
}

exportar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "exportar",
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                location.href = controlador + "descargar";
            }
            else {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

mostrarUsuario = function (idEmpresa, estado) {
    
    listarUsuarios(idEmpresa, estado);
}

mostrarUsuarioTotal = function (idEmpresa) {
    listarUsuarios(idEmpresa, '-1');
}

listarUsuarios = function (idEmpresa, estado){
    
    $.ajax({
        type: 'POST',
        url: controlador + 'listado',
        data: {
            idEmpresa: idEmpresa,
            estado:estado
        },
        success: function (evt) {

            $('#contenidoDetalle').html(evt);
            setTimeout(function () {

                $('#tabla').dataTable({
                });

                $('#popupDetalle').bPopup({
                    autoClose: false
                });

            }, 50);

        },
        error: function () {
            mostrarError();
        }
    });
}

mostrarError = function ()
{
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-error');
    $('#mensaje').text('Ha ocurrido un error.');
}