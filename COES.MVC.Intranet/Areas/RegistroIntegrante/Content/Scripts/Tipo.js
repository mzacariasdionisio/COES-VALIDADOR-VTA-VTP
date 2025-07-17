var controlador = siteRoot + 'RegistroIntegrante/Tipo/';
var RowRL = null;
$(function () {

    $("input[type=text]").keyup(function () {
        $("#" + this.id).val($("#" + this.id).val().toUpperCase());
    });

    cargarTipos();
});

verDigitalizado = function () {
    window.open(controlador + 'ver?url=' + $('#hdfNombreArchivoDigitalizado').val(), "_blank", 'fullscreen=yes');
}

descargarDigitalizado = function () {
    document.location.href = controlador + 'Download?url=' + $('#hdfNombreArchivoDigitalizado').val() + '&nombre=' + $('#hdfAdjuntoDigitalizado').val();
}

cargarTipos = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'listado?emprcodi=' + $("#hdfemprcodi").val(),
        success: function (evt) {
            $('#listadoTipos').html(evt);
        },
        error: function () {
            mostrarError();
        }
    });
}

EditarTipo = function (idTipo) {

    $.ajax({
        type: 'POST',
        data: {
            idTipo: idTipo
        },
        url: controlador + 'edicion',
        success: function (evt) {
            $('#contenidoEdicion').html(evt);
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    autoClose: false,
                });
            }, 50);
            
            MostrarCamposPorTipoAgente();

            $('#btnAceptar').click(function () {
                $('#popupEdicion').bPopup().close();
            });

        }
    });



    MostrarCamposPorTipoAgente = function () {

        var codigo = $('#hdfTipoEmpresa').val();
        
        OcultarTipoAgente();

        var GENERADOR = $("#hfTipoComportamientoGenerador_codigo").val();
        var TRANSMISOR = $("#hfTipoComportamientoTrasmisor_codigo").val();
        var DISTRIBUIDOR = $("#hfTipoComportamientoDistribuidor_codigo").val();
        var USUARIOLIBRE = $("#hfTipoComportamientoUsuarioLibre_codigo").val();


        switch (codigo) {
            case GENERADOR:
                $(".Generador").show(1000);
                break;
            case TRANSMISOR:
                $(".Transmisor").show(1000);
                break;
            case DISTRIBUIDOR:
                $(".Distribuidor").show(1000);
                break;
            case USUARIOLIBRE:
                $(".UsuarioLibre").show(1000);
                break;
        }
    }

    OcultarTipoAgente = function () {
        $(".Generador").hide();
        $(".Transmisor").hide();
        $(".Distribuidor").hide();
        $(".UsuarioLibre").hide();
    };
}

function mostrarError() {
    $('#mensaje').removeClass();
    $('#mensaje').html("Ha ocurrido un error, por favor intente nuevamente.");
    $('#mensaje').addClass('action-error');
}




