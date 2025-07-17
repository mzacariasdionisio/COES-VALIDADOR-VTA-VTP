var controlador = siteRoot + 'web/ayudamovil/';
$(function () {

    $('#tab-container').easytabs({
        animate: false
    });

    tinymce.init({
        selector: '#txtAyuda',
        plugins: [
            'paste textcolor colorpicker textpattern'
        ],
        toolbar1: 'insertfile undo redo | bold italic | alignleft aligncenter alignright alignjustify |  bullist numlist outdent indent| forecolor backcolor',
        menubar: false,
        language: 'es',
        statusbar: false,
        setup: function (editor) {
            editor.on('change', function () {
                editor.save();
            });
        }
    });

    tinymce.init({
        selector: '#txtAyudaEng',
        plugins: [
            'paste textcolor colorpicker textpattern'
        ],
        toolbar1: 'insertfile undo redo | bold italic | alignleft aligncenter alignright alignjustify |  bullist numlist outdent indent| forecolor backcolor',
        menubar: false,
        language: 'es',
        statusbar: false,
        setup: function (editor) {
            editor.on('change', function () {
                editor.save();
            });
        }
    });

    $('#btnGrabar').on("click", function () {
        grabar();
    });           

    consultar();
})

consultar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'listar',        
        success: function (evt) {
            $('#listado').html(evt);
            $('#tabla').dataTable({
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

editar = function (id) {

    $.ajax({
        type: 'POST',
        url: controlador + 'editar',
        data: {
            id: id
        },
        global: false,
        dataType:'json',
        success: function (result) {
            $('#spanIdVentana').text(result.IdVentana);
            $('#spanDescripcion').text(result.NombreVentana);
            $('#cbIndicador').val(result.Indicador);
            $('#hfCodigo').val(result.Codigo);
            tinyMCE.get('txtAyuda').setContent(result.TextoAyuda);
            tinymce.get('txtAyudaEng').setContent(result.TextoAyudaEng);
            $('#divGrabar').css('display', 'block');
            $('#tab-container').easytabs('select', '#paso2');
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

grabar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'grabar',
        data: $('#frmRegistro').serialize(),
        dataType: 'json',
        global: false,
        success: function (result) {
            if (result > 0) {
                mostrarMensaje('mensaje', 'exito', 'Los datos se grabaron correctamente.');
                $('#tab-container').easytabs('select', '#paso1');
                consultar();
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

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}