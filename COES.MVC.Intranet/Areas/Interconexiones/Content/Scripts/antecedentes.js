var controlador = siteRoot + 'interconexiones/informe/';

$(function () {

    $('#tab-container').easytabs({
        animate: false
    });

    consultar();

    $('#btnNuevo').on('click', function () {
        editar(0);
    });

    $('#btnGrabar').on('click', function () {
        grabar();
    });      

    $('#btnRegresar').on('click', function () {
        document.location.href = controlador + "index";
    });
    /*
    tinymce.init({
        selector: '#txtContenido',
        plugins: [
            //'paste textcolor colorpicker textpattern link table image imagetools preview'
            'wordcount advlist anchor autolink codesample colorpicker contextmenu fullscreen image imagetools lists link media noneditable preview  searchreplace table template textcolor visualblocks wordcount'
        ],
        toolbar1:
            //'insertfile undo redo | fontsizeselect bold italic underline strikethrough | forecolor backcolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link | table | image | mybutton | preview',
            'insertfile undo redo | styleselect fontsizeselect | forecolor backcolor | bullist numlist outdent indent | link | table | image | mybutton | preview',

        menubar: false,        
        language: 'es',
        statusbar: false,
        convert_urls: false,
        plugin_preview_width: 1000,
        setup: function (editor) {
            editor.on('change',
                function () {
                    editor.save();
                });
            editor.addButton('mybutton', {
                type: 'menubutton',
                text: 'Agregar Variables',
                tooltip: "Ingrese una variable",
                icon: false
            });

        }
    });*/

    $('#btnGrabar').hide();
});

function consultar() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListaAntecedentes',       
        success: function (evt) {
            $('#listado').html(evt);           
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function editar(id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'EditarAntecedentes',
        data: {
            id: id
        },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {               
                $('#hfCodigoAntecedente').val(id);
                var txt = "";
                if (result.Entidad.Intantcontenido != null) txt = result.Entidad.Intantcontenido;
                //tinyMCE.get('txtContenido').setContent(txt);
                $('#txtContenido').val(txt);
                $('#tab-container').easytabs('select', '#tabEdicion');
                $('#btnGrabar').show();
               
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    })
}

function eliminar(id) {
    if (confirm('¿Está seguro de eliminar el registro?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'EliminarAntecedente',
            data: {
                id: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    consultar();
                    mostrarMensaje('mensaje', 'exito', 'El registro se eliminó correctamente.');
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function grabar() {
    $.ajax({
        type: 'POST',
        url: controlador + 'GrabarAntecedente',
        data: $('#frmRegistro').serialize(),
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                consultar();
                $('#tab-container').easytabs('select', '#tabListado');
                //tinyMCE.get('txtContenido').setContent("");
                $('#txtContenido').val("");
                $('#btnGrabar').hide();
                mostrarMensaje('mensaje', 'exito', 'El registro se grabó correctamente.');
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    })
}


function mostrarMensaje(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}