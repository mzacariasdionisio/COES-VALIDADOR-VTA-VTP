var controlador = siteRoot + "mediciones/configuracion/";

$(function () {

    $('#btnConsultar').click(function () {
        cargar($('#cbTipoGrupo').val());
        $('#mensaje').css("display", "none");
    });

    $('#btnCancelar').click(function () {
        document.location.href = siteRoot + "home/index";
    });

    cargar($('#cbTipoGrupo').val());
});


cargar = function (idTipoGrupo)
{
    $.ajax({
        type: 'POST',
        url: controlador + "lista",
        data: {
            idTipoGrupo:  idTipoGrupo
        },
        success: function (evt) {
            $('#listado').html(evt);
            $('#tabla').dataTable({
                "iDisplayLength": 50
            });
        },
        error: function () {
            mostrarError();
        }
    });
}


actualizar = function ()
{
    if (confirm('¿Está seguro de actualizar el tipo de grupo?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'cambiartipogrupo',
            dataType: 'json',
            data: {
                idGrupo: $('#hfIdGrupo').val(), idTipoGrupo: $('#cbEditTipoGrupo').val(),
                idTipoGrupo2: $('#cbEditTipoGrupo2').val(), tipoGenerRer: $('#cbAdjudicado').val()
            },
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    mostrarExitoOperacion();
                    $('#popupUnidad').bPopup().close();
                    cargar($('#cbTipoGrupo').val());
                }
                else
                    mostrarError();
            },
            error: function () {
                mostrarError();
            }
        });
    }    
}

editarRegistro = function (idGrupo, emprnomb, gruponomb, grupoabrev, idtipogrupo, idtipogrupo2, tipogenerrer)
{
    $.ajax({
        type: 'POST',
        url: controlador + "edicion",
        data: {
            idGrupo: idGrupo, emprNomb: emprnomb, grupoNomb: gruponomb,
            grupoAbrev: grupoabrev, idTipoGrupo: idtipogrupo, idTipoGrupo2 : idtipogrupo2, tipoGenerRer : tipogenerrer
        },
        success: function (evt) {
            $('#edicionGrupo').html(evt);         
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupUnidad').bPopup({
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

mostrarExitoOperacion = function ()
{
    $('#mensaje').removeClass("action-error");
    $('#mensaje').addClass("action-exito");
    $('#mensaje').text("El tipo de grupo se actualizó...!");
    $('#mensaje').css("display", "block");
}

mostrarError = function ()
{
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
    $('#mensaje').css("display", "block");
}