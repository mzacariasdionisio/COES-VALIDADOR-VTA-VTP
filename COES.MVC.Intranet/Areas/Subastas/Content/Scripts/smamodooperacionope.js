var controlador = siteRoot + 'Subastas/ModoOperacion/'

$(document).ready(function () {

    function validar() {
        var mensaje = "";

        if ($('#urs').val() == "" || $('#urs').val() == 0) {
            mensaje = mensaje + " Falta Seleccionar URS" + "<br />";
        }
        if ($('#atributo').val() == "" || $('#atributo').val() == -1 || $('#atributo').val() == null) {
            mensaje = mensaje + "Falta seleccionar en Atributo" + "<br / >";
        }
        return mensaje;
    };

    $('#btnRegistrarModoOperacion').unbind();
    $('#btnRegistrarModoOperacion').click(function () {
        var rmj = validar();
        if (rmj == "" || rmj == -1 || rmj == null) {
        $.ajax({
            type: "post",
            url: controlador + "MantenimientoModoOperacion",
            //datatype: 'json',
            data: {
                grupocodi: $('#atributo').val(),
                accion: "nuevo"
            },
            success: function (result) {
                if (result.Resultado == '-1') {
                    alert('Ha ocurrido un error:' + result.Mensaje);
                } else {
                    //todos
                    $('#btnCancelar').click();
                    mensajeOperacion(result.Resultado, 1);
                }
            },
            error: function (req, status, error) {
                mensajeOperacion(error);
                validaErrorOperation(req.status);
            }
        });   
        } else {
            mensajesError(rmj);
        }
    });

    $('#btnModificarModoOperacion').click(function () {
        $.ajax({
            type: "post",
            url: controlador + "MantenimientoModoOperacion",
            //datatype: 'json',
            data: {
                grupocodi: $('#atributo').val(),
                accion: "Editar"
            },
            cache: false,
            global: false,
            success: function (resultado) {
                //todos
                $('#btncancelar').click();
                mensajeOperacion(resultado, 1);
            },
            error: function (req, status, error) {
                mensajeOperacion(error);
                validaErrorOperation(req.status);
            }
        });
    });

    // Filtro para listar el MULTISELECT al escoger USUARIO
    $('#urs').change(function () {
        $('#atributo').html($('#atributo').multipleSelect({
            width: '200px',
            onClick: function (view) {
            },
            onClose: function (view) {
            }
        })
        );
        $.ajax({
            type: "POST",
            global: false,
            url: controlador + 'ListarMO',
            data: { tipo: $(this).val() },
            cache: false,
            success: function (resultado) {

                for (i = 0, l = resultado.length; i < l; i++) {

                    $('#atributo').append($('<option>', { value: resultado[i].Grupocodi, text: resultado[i].Gruponomb }));
                }
                $('#atributo').append($('#atributo').multipleSelect({
                    width: '200px',
                    onClick: function (view) {
                    },
                    onClose: function (view) {
                    }
                })
            );
            },
            error: function (req, status, error) {
                mensajeOperacion(error);
            }
        });
    });

});