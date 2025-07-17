$(document).ready(function () {

    cargarListado();

    $('#btnAceptar').click(function () {
        enviarSolicitud();
    });

    $('#btnCancelar').click(function () {
        document.location.href = siteRoot;
    });
});

cargarListado = function () {
    $.ajax({
        type: 'POST',      
        url: siteRoot + 'account/solicitar/listado',       
        success: function (evt) {
            $('#listado').html(evt);
           
            $('#cbSelectAll').click(function (e) {
                var table = $(e.target).closest('table');
                $('td input:checkbox', table).prop('checked', this.checked);
            });
        },
        error: function () {
            mostrarAlerta();
        }
    });
}

enviarSolicitud = function () {

    var modulos = "";
    var countModulo = 0;
    $('#tbPendientes tbody input:checked').each(function () {
        modulos = modulos + $(this).val() + ",";
        countModulo++;
    });

    if (countModulo > 0) {
        if (confirm('¿Está seguro de enviar la solicitud?')) {
            $.ajax({
                type: 'POST',
                data: {
                    modulos: modulos
                },
                url: siteRoot + 'account/solicitar/enviar',
                dataType: 'json',
                success: function (result) {
                    if (result == 1) {
                        cargarListado();
                        mostrarExito();
                    }
                    else {
                        mostrarError();
                    }
                },
                error: function () {
                    mostrarAlerta();
                }
            });
        }
    }
    else {
        mostrarAlerta('Por favor seleccione modulos a solicitar')
    }
}

enviarRecordatorio = function (id) {
    if (confirm('¿Está seguro de enviar el recordatorio?')) {
        $.ajax({
            type: 'POST',
            data: {
                id: id
            },
            url: siteRoot + 'account/solicitar/recordatorio',
            dataType: 'json',
            success: function (result) {
                if (result == 1) {                    
                    mostrarExito();
                }
                else {
                    mostrarError();
                }
            },
            error: function () {
                mostrarAlerta();
            }
        });
    }
}

mostrarAlerta = function (mensaje){
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}

mostrarError = function () {
    $('#mensaje').removeClass();
    $('#mensaje').html('Ha ocurrido un error');
    $('#mensaje').addClass('action-error');
}

mostrarExito = function () {
    $('#mensaje').removeClass();
    $('#mensaje').html('La operación se realizó exitosamente');
    $('#mensaje').addClass('action-exito');
}
