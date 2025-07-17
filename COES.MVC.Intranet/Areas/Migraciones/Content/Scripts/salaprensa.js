var controlador = siteRoot + 'Migraciones/SalaPrensa/'
var oTable;
$(function () {
    $('#tab-container').easytabs({
        animate: false
    });

    $('#txtFecha').Zebra_DatePicker();
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
        openCrear2($('#event').val());
        f_limpiar();
        $('#tab-container').easytabs('select', '#paso1');
    });

    $('#btnCancelarEdit').on('click', function () {
        f_limpiar();
        $('#tab-container').easytabs('select', '#paso1');
    });
    cargarComunicados(1);

    $(window).resize(function () {
        updateContainer();
    });
    
    tinymce.init({
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
    });

});

function updateContainer() {
    var $containerWidth = $(window).width();

    $('#listado1').css("width", $containerWidth - 240 + "px");
}

function f_limpiar() {
    $("#txtTitulo").val('');
    $("#txtResumen").val('');
    tinyMCE.get('txtDescripcion').setContent("");
    $("#txtFecha").val($("#ff").val());
    $("#txtFechaini").val($("#ff").val());
    $("#txtFechafin").val($("#ff").val());
    $("#fileInfo").html('');
    $("#progreso").html('');
    $("#event").val('1');
    $("#imgComunicado").prop("src", null);

}

function openCrear2(x) {
    if ($('#txtTitulo').val() == "") { alert("Ingresar Titulo. Obligatorio..!!"); return; }
    if ($('#txtResumen').val() == "") { alert("Ingresar Descripción Breve. Obligatorio..!!"); return; }
    if ($('#txtDescripcion').val() == "") { alert("Ingresar Descripcion. Obligatorio..!!"); return; }
    
    var estad = "N";
    var tipo = "S"; //es de tipo sala de prensa

    $('#hfEvento').val(x);
    $('#hfEstado').val(estad);

    $.ajax({
        type: 'POST',
        url: controlador + "SaveComunicado",
        dataType: 'json',
        data: $('#formRegistro').serialize(),
        success: function (evt) {
            if (evt.nRegistros != -1) {
                cargarComunicados(1);
            }
            else { alert("Ha ocurrido un error."); }
        },
        error: function (err) { alert("Error al cargar Excel Web"); }
    });
}

function cargarComunicados(x) {
    $.ajax({
        type: 'POST',
        url: controlador + "CargarComunicados",
        dataType: 'json',
        data: { typ: x },
        success: function (evt) {
            //f_limpiar();
            updateContainer();
            if (x == 1) {
                $('#listado1').html(evt.Resultado);
                if (evt.nRegistros > 0) {
                    //oTable = $('#tb_info1').dataTable({
                    //    "bJQueryUI": true,
                    //    "scrollY": "auto",
                    //    "scrollX": false,
                    //    "sDom": 't',
                    //    "ordering": true,
                    //    "iDisplayLength": 150,
                    //    "sPaginationType": "full_numbers"
                    //});
                    //oTable.rowReordering({ sURL: controlador + "UpdateOrdenComunicados" });
                }
                $('#listado1').css("width", $('#mainLayout').width() - 10 + "px");
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
    
    if (confirm('¿Está seguro de realizar esta operación?')) {

        $.ajax({
            type: 'POST',
            url: controlador + "DeleteComunicado",
            dataType: 'json',
            data: { comcodi: x },
            success: function (evt) {
                if (evt.nRegistros != -1) {
                    cargarComunicados(1);
                }
                else { alert("Ha ocurrido un error."); }
            },
            error: function (err) { alert("Error"); }
        });
    }
}

function edit_(x, y, z) {
    $.ajax({
        type: 'POST',
        url: controlador + "ProcesoEditComunicado",
        dataType: 'json',
        data: { comcodi: x, evnto: y, pos: z },
        success: function (evt) {
            if (evt.nRegistros != -1) {
                if (y == 1) {
                    cargarComunicados(1);

                } else {
                    $('#comcodi').val(evt.Wbcomunicados.Comcodi);
                    $('#txtFecha').val(evt.Wbcomunicados.ComfechaDesc);
                    $('#txtTitulo').val(evt.Wbcomunicados.Comtitulo);
                    $('#txtResumen').val(evt.Wbcomunicados.Comresumen);
                    tinyMCE.get('txtDescripcion').setContent(evt.Wbcomunicados.Comdesc);
                    $('#txtFechaini').val(evt.Wbcomunicados.ComfechainiDesc);
                    $('#txtFechafin').val(evt.Wbcomunicados.ComfechafinDesc);
                    $("#fileInfo").val('');
                    $("#progreso").val('');
                    $('#imgComunicado').prop("src",evt.Imagen);

                    if (evt.Wbcomunicados.Comestado == 'A') {
                        $('#rd1').prop("checked", true);
                        $('#rd2').prop("checked", false);
                    } else {
                        $('#rd1').prop("checked", false);
                        $('#rd2').prop("checked", true);
                    }
                    $('#tab-container').easytabs('select', '#paso2');
                }
                $('#event').val(y);
            }
            else { alert("Ha ocurrido un error."); }
        },
        error: function (err) { alert("Error al cargar Excel Web"); }
    });
}

getFecha = function (date) {
    var parts = date.split("/");
    var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date.getTime();
}