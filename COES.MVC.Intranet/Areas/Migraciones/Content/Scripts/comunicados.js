var controlador = siteRoot + 'Migraciones/Configuraciones/'
var oTable;
$(function () {
    //$('#txtFecha').Zebra_DatePicker();
    $('#txtFechaini').Zebra_DatePicker();
    $('#txtFechafin').Zebra_DatePicker();

    $('#txtFecha').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtFecha').val(date + " 00:00:00");
        }
    });

    $("#btnCrear").click(function () {
        f_limpiar();
        openPopupCrear();
        $('#event').val(1);
        $('#comcodi').val(0);
    });

    $("#btnGrabar").click(function () {
        openCrear($('#event').val());
    });

    cargarComunicados(1);
    cargarComunicados(2);

    $(window).resize(function () {
        updateContainer();
    });
});

function updateContainer() {
    var $containerWidth = $(window).width();

    $('#listado1').css("width", $containerWidth - 240 + "px");
    $('#listado2').css("width", $containerWidth - 240 + "px");
}

function openPopupCrear() {
    setTimeout(function () {
        $('#popupTablaNuevo').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}

function f_limpiar() {
    $("#txtTitulo").val('');
    $("#txtDescripcion").val('');
    $("#txtLink").val('');
    $("#txtFecha").val($("#ff").val());
    $("#txtFechaini").val($("#ff2").val());
    $("#txtFechafin").val($("#ff2").val());
    document.getElementById("rd1").checked = true;
    document.getElementById("rd2").checked = false;
}

function openCrear(x) {
    if ($('#txtTitulo').val() == "") { alert("Ingresar Titulo. Obligatorio..!!"); return; }
    if ($('#txtDescripcion').val() == "") { alert("Ingresar Descripcion. Obligatorio..!!"); return; }
    var estad = "N";
    var tipo = "N"; //No es de tipo sala de prensa
    if ($("#rd1").is("checked")) { estad = "A"; }

    $.ajax({
        type: 'POST',
        url: controlador + "SaveComunicado",
        dataType: 'json',
        data: {
            fecha: $('#txtFecha').val(),
            titu: $('#txtTitulo').val(),
            descrip: $('#txtDescripcion').val(),
            lin: $('#txtLink').val(),
            fecha1: $('#txtFechaini').val(),
            fecha2: $('#txtFechafin').val(),
            est: estad, evnto: x, comcodi: $('#comcodi').val(),
            tipocomu: tipo
        },
        success: function (evt) {
            if (evt.nRegistros != -1) {
                cargarComunicados(1);
                cargarComunicados(2);
                $('#popupTablaNuevo').bPopup().close();
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
            updateContainer();
            if (x == 1) {
                $('#listado1').html(evt.Resultado);
                if (evt.nRegistros > 0) {
                    oTable = $('#tb_info1').dataTable({
                        "bJQueryUI": true,
                        "scrollY": "auto",
                        "scrollX": true,
                        "sDom": 't',
                        "ordering": true,
                        "iDisplayLength": 150,
                        "sPaginationType": "full_numbers"
                    });
                    oTable.rowReordering({ sURL: controlador + "UpdateOrdenComunicados" });
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
    $.ajax({
        type: 'POST',
        url: controlador + "DeleteComunicado",
        dataType: 'json',
        data: { comcodi: x },
        success: function (evt) {
            if (evt.nRegistros != -1) {
                cargarComunicados(1);
                cargarComunicados(2);
            }
            else { alert("Ha ocurrido un error."); }
        },
        error: function (err) { alert("Error al cargar Excel Web"); }
    });
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
                    cargarComunicados(2);
                } else {
                    $('#comcodi').val(evt.Wbcomunicados.Comcodi);
                    $('#txtFecha').val(evt.Wbcomunicados.ComfechaDesc);
                    $('#txtTitulo').val(evt.Wbcomunicados.Comtitulo);
                    $('#txtDescripcion').val(evt.Wbcomunicados.Comdesc);
                    $('#txtLink').val(evt.Wbcomunicados.Comlink);
                    $('#txtFechaini').val(evt.Wbcomunicados.ComfechainiDesc);
                    $('#txtFechafin').val(evt.Wbcomunicados.ComfechafinDesc);

                    if (evt.Wbcomunicados.Comestado == 'A') {
                        $('#rd1').prop("checked", true);
                        $('#rd2').prop("checked", false);
                    } else {
                        $('#rd1').prop("checked", false);
                        $('#rd2').prop("checked", true);
                    }
                    openPopupCrear();
                }
                $('#event').val(y);
            }
            else { alert("Ha ocurrido un error."); }
        },
        error: function (err) { alert("Error al cargar Excel Web"); }
    });
}