var controlador = siteRoot + 'medidores/ieod/'

$(function () {

    $('#txtFechaInicial').Zebra_DatePicker({
        pair: $('#txtFechaFinal'),
        onSelect: function (date) {
            $('#txtExportarDesde').val(date);
            var date1 = getFecha(date);
            var date2 = getFecha($('#txtFechaFinal').val());

            if (date1 > date2) {
                $('#txtFechaFinal').val(date);
            }
        }
    });

    $('#txtFechaFinal').Zebra_DatePicker({
        direction: true
    });


    $('#txtExportarDesde').Zebra_DatePicker({
        pair: $('#txtExportarHasta'),
        onSelect: function (date) {
            $('#txtExportarDesde').val(date);
            var date1 = getFecha(date);
            var date2 = getFecha($('#txtExportarHasta').val());

            if (date1 > date2) {
                $('#txtExportarHasta').val(date);
            }
        }
    });

    $('#txtExportarHasta').Zebra_DatePicker({
        direction: true
    });
      
    $('#btnBuscar').click(function () {
        consultar();
    }); 

    $('#btnOpenExportar').click(function () {
        openExportar();
    });

    $('#btnExportar').click(function () {
        exportar();
    });

    consultar();
});

consultar = function () {
    if ($('#txtFechaInicial').val() != "" && $('#txtFechaFinal').val()) {
        if (rangoFechas()) {

            $.ajax({
                type: 'POST',
                url: controlador + "listado",
                data: {
                    fechaInicial: $('#txtFechaInicial').val(),
                    fechaFinal: $('#txtFechaFinal').val()
                },
                success: function (evt) {
                    $('#listado').css("width", $('#mainLayout').width() + "px");
                    $('#listado').html(evt);
                    $('#tabla').dataTable({
                        "scrollY": 430,
                        "scrollX": true,
                        "sDom": 't',
                        "ordering": false,
                        "iDisplayLength": 5000
                    });
                },
                error: function () {
                    mostrarError();
                }
            });
        }
        else {
            mostrarMensaje('El rango entre fechas debe ser a lo más de 31 días.')
        }
    }
    else {
        mostrarMensaje("Seleccione fecha de inicio y final");
    }
}

openExportar = function () {
    $('#divExportar').css('display', 'block');
}

closeExportar = function () {
    $('#divExportar').css('display', 'none');
}


exportar = function () {
    if ($('#txtExportarDesde').val() != "" && $('#txtExportarHasta').val()) {
        var opcion = $("input:radio[name='rbFormato']:checked").val();
        $.ajax({
            type: 'POST',
            url: controlador + "exportar",
            dataType: 'json',
            data: {
                fechaInicial: $('#txtExportarDesde').val(),
                fechaFinal: $('#txtExportarHasta').val(),
                opcion: opcion
            },
            success: function (result) {
                if (result == 1) {
                    window.location = controlador + 'descargar?opcion=' + opcion;
                }
                else if (result == -1) {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError()
            }
        });
    }
    else {
        mostrarMensaje("Seleccione fecha de inicio y final");
    }
}

mostrarMensaje = function (mensaje) {
    alert(mensaje);
}

getFecha = function (date) {
    var parts = date.split("/");
    var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date.getTime();
}

rangoFechas = function () {
    date1 = getFecha($('#txtFechaInicial').val());
    date2 = getFecha($('#txtFechaFinal').val());
    var dias = (date2 - date1) / (1000 * 60 * 60 * 24);
    if (dias < 31) {
        return true;
    }
    return false;
}