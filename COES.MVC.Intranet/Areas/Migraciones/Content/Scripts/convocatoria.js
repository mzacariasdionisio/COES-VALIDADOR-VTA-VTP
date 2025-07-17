var controlador = siteRoot + 'Migraciones/Convocatoria/'
var oTable;
$(function () {
    $('#tab-container').easytabs({
        animate: false
    });

    $('#txtFechaini').Zebra_DatePicker({
        pair: $('#txtFechafin'),
        onSelect: function (date) {
            var date1 = getFecha(date);
            var date2 = getFecha($('#txtFechafin').val());

            if (date1 > date2) {
                $('#txtFechafin').val(date);
            }
        }
    });

    $('#txtFechafin').Zebra_DatePicker({
        direction: true
    });

    $("#btnCrear").click(function () {
        f_limpiar();
        $('#tab-container').easytabs('select', '#paso2');
    });

    $("#btnGrabar").click(function () {
        openCrear2();
    });

    $('#btnCancelarEdit').on('click', function () {
        f_limpiar();
        $('#tab-container').easytabs('select', '#paso1');
    });
    cargarConvocatorias(1);

    $(window).resize(function () {
        updateContainer();
    });

    /*tinymce.init({
        selector: '#txtDescripcion',
        plugins: [
            'paste textcolor colorpicker textpattern link preview'
        ],
        convert_urls: false,
        toolbar1:
            'insertfile undo redo | bold italic | alignleft aligncenter alignright alignjustify |  bullist numlist outdent indent| forecolor backcolor link preview',
        menubar: false,
        language: 'es',
        statusbar: false,
        setup: function (editor) {
            editor.on('change', function () {
                editor.save();
            });
        }
    });*/

});

function updateContainer() {
    var $containerWidth = $(window).width();

    $('#listado1').css("width", $containerWidth - 240 + "px");
}

function f_limpiar() {
    $("#txtAbreviatura").val('');
    $("#txtNombre").val('');
    $('#txtDescripcion').val('');
    $("#txtFechaini").val($("#ff").val());
    $("#txtFechafin").val($("#ff").val());
    $("#progreso").html('');

}

function openCrear2() {
    if ($('#txtAbreviatura').val() == "") { alert("Ingresar Codigo de Convocatoria. Obligatorio..!!"); return; }
    if ($('#txtNombre').val() == "") { alert("Ingresar Nombre. Obligatorio..!!"); return; }
    if ($('#txtFechaini').val() == "") { alert("Ingresar Fecha de publicacion. Obligatorio..!!"); return; }
    if ($('#txtFechaini').val() == "") { alert("Ingresar Fecha de publicacion. Obligatorio..!!"); return; }

    var estad = "N";

    $('#hfEstado').val(estad);

    $.ajax({
        type: 'POST',
        url: controlador + "SaveConvocatoria",
        dataType: 'json',
        data: $('#formRegistro').serialize(),
        success: function (evt) {
            if (evt.nRegistros != -1) {
                f_limpiar();
                $('#tab-container').easytabs('select', '#paso1');
                cargarConvocatorias(1);
            }
            else { alert("Ha ocurrido un error."); }
        },
        error: function (err) { alert("Error al registrar convocatoria."); }
    });
}




function cargarConvocatorias(x) {
    $.ajax({
        type: 'POST',
        url: controlador + "CargarConvocatorias",
        dataType: 'json',
        data: { typ: x },
        success: function (evt) {
            updateContainer();
            if (x == 1) {
                $('#listado1').html(evt.Resultado);
                $('#listado1').css("width", $('#mainLayout').width() - 40 + "px");
            } else {
                $('#listado2').html(evt.Resultado);
                if (evt.nRegistros > 0) {
                    $("#tb_info2").dataTable({
                        "ordering": false,
                        "bLengthChange": false,
                        "bInfo": false,
                    });
                }
            }
        },
        error: function (err) { alert("Error..!!"); }
    });
}

function delete_(x) {

    if (confirm('¿Esta seguro de realizar esta operacion?')) {

        $.ajax({
            type: 'POST',
            url: controlador + "DeleteConvocatoria",
            dataType: 'json',
            data: { convcodi: x },
            success: function (evt) {
                if (evt.nRegistros != -1) {
                    cargarConvocatorias(1);
                }
                else { alert("Ha ocurrido un error."); }
            },
            error: function (err) { alert("Error al eliminar convocatoria."); }
        });
    }
}

function edit_(x) {
    $.ajax({
        type: 'POST',
        url: controlador + "ProcesoEditConvocatoria",
        dataType: 'json',
        data: { convcodi: x },
        success: function (evt) {
            if (evt.nRegistros != -1) {
                $('#convcodi').val(evt.Wbconvocatorias.Convcodi);
                $('#txtAbreviatura').val(evt.Wbconvocatorias.Convabrev);
                $('#txtNombre').val(evt.Wbconvocatorias.Convnomb);
                $('#txtDescripcion').val(evt.Wbconvocatorias.Convdesc);
                $('#txtFechaini').val(evt.Wbconvocatorias.ConvfechainiDesc);
                $('#txtFechafin').val(evt.Wbconvocatorias.ConvfechafinDesc);

                if (evt.Wbconvocatorias.Convestado == 'A') {
                        $('#rd1').prop("checked", true);
                        $('#rd2').prop("checked", false);
                    } else {
                        $('#rd1').prop("checked", false);
                        $('#rd2').prop("checked", true);
                    }
                    $('#tab-container').easytabs('select', '#paso2');
            }
            else { alert("Ha ocurrido un error."); }
        },
        error: function (err) { alert("Error al obtener datos de convocatoria."); }
    });
}

getFecha = function (date) {
    var parts = date.split("/");
    var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date.getTime();
}