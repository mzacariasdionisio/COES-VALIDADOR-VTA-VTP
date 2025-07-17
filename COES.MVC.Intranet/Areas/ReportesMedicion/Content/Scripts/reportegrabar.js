var controlador = siteRoot + 'ReportesMedicion/formatoreporte/'

$(function () {
    $("#frmFormato").validate({
        submitHandler: function () {
            grabarFormato();
        },
        rules: {
            Nombre: { required: true, minlength: 9 },
            Descripcion: { required: true, minlength: 9 },
            IdCabecera: { min: 1 },
            IdModulo: { min: 1 },
            IdArea: { min: 1 },
            AllEmpresa: { min: 0 }
        },
        messages: {
            Nombre: {
                required: "Ingrsese nombre de reporte",
                minlength: "Mínino 9 caracteres"
            },
            Descripcion: {
                required: "Ingrsese descripción de reporte",
                minlength: "Mínino 9 caracteres"
            },
            IdCabecera: {
                min: " Seleccionar Opción"
            },
            IdModulo: {
                min: " Seleccionar Opción"
            },
            IdArea: {
                min: " Seleccionar Opción"
            },
            AllEmpresa: {
                min: " Seleccionar Opción"
            }
        }
    });
    
    $('#btnCancelar').click(function () {
        $('#popupFormato').bPopup().close();
        recargar();
    });

    $("#IdLectura").val($("#hfLectura").val());
    $("#IdModulo").val($("#hfModulo").val());
    $("#IdCabecera").val($("#hfCabecera").val());
    $("#IdArea").val($("#hfArea").val());

    if ($("#hfCheckEmpresa").val() == 1) {
        $("#chbEmpresa").prop("checked", true);
    }
    else {
        $("#chbEmpresa").prop("checked", false);
    }
    if ($("#hfCheckEquipo").val() == 1) {
        $("#chbEquipo").prop("checked", true);
    }
    else {
        $("#chbEquipo").prop("checked", false);
    }
    if ($("#hfCheckTipoEquipo").val() == 1) {
        $("#chbTipoEquipo").prop("checked", true);
    }
    else {
        $("#chbTipoEquipo").prop("checked", false);
    }
    if ($("#hfCheckTipoMedida").val() == 1) {
        $("#chbTipoMedida").prop("checked", true);
    }
    else {
        $("#chbTipoMedida").prop("checked", false);
    }
    
    $('#cbTipoReporte').change(function () {
        cargarReporteItem(1);
    });

    $("#cbTipoReporte").val($("#hfTipoReporteItem").val());
    $("#Mrepcodi").val($("#hfReporteItem").val());

    if ($("#hfCheckEsGrafico").val() == 1) {
        $("#chbEsGrafico").prop("checked", true);
    }
    else {
        $("#chbEsGrafico").prop("checked", false);
    }
});

cargarReporteItem = function (x, y) {
    var cb_ = "Mrepcodi", cb_2 = "cbTipoReporte";
    $('option', '#' + cb_).remove();
    if ($('#' + cb_2).val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'cargarReporteItems',
            dataType: 'json',
            data: { id: parseInt($('#' + cb_2).val()) || 0 },
            cache: false,
            success: function (aData) {
                $('#' + cb_).get(0).options.length = 0;
                $('#' + cb_).get(0).options[0] = new Option("--SELECCIONE--", "");
                $.each(aData, function (i, item) {
                    $('#' + cb_).get(0).options[$('#' + cb_).get(0).options.length] = new Option(item.Repdescripcion, item.Repcodi);
                });
                if (y == 1) { $('#Mrepcodi').val($('#hfReporteItem').val()); }
            },
            error: function () { mostrarError(); }
        });
    }
}

function grabarFormato() {

    if ($('#chbEmpresa:checked').val()) {
        $('#hfCheckEmpresa').val(1);
    }
    else {
        $('#hfCheckEmpresa').val(0);
    }

    if ($('#chbEquipo:checked').val()) {
        $('#hfCheckEquipo').val(1);
    }
    else {
        $('#hfCheckEquipo').val(0);
    }

    if ($('#chbTipoEquipo:checked').val()) {
        $('#hfCheckTipoEquipo').val(1);
    }
    else {
        $('#hfCheckTipoEquipo').val(0);
    }

    if ($('#chbTipoMedida:checked').val()) {
        $('#hfCheckTipoMedida').val(1);
    }
    else {
        $('#hfCheckTipoMedida').val(0);
    }

    if ($('#chbEsGrafico:checked').val()) {
        $('#hfCheckEsGrafico').val(1);
    }
    else {
        $('#hfCheckEsGrafico').val(0);
    }

    $.ajax({
        type: 'POST',
        url: controlador + "GrabarReporte",
        dataType: 'json',
        data: $('#frmFormato').serialize(),
        success: function (evt) {
            if (evt == 1) {
                alert("Grabó correctamente");
                $('#popupFormato').bPopup().close();
                recargar();
            }
            if (evt == -1) {
                alert("Ocurrio un error");
            }
        },
        error: function () {
            alert("Ha ocurrido un error en guardar el reporte");
        }
    });
}

function recargar() {
    document.location.href = controlador + "index";
}