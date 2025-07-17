var controler = siteRoot + 'Medidores/stockcombustible/'

$(function () {

    $('#txtFechaDesde').Zebra_DatePicker({
        pair: $('#txtFechaHasta'),
        onSelect: function (date) {
            var date1 = getFecha(date);
            var date2 = getFecha($('#txtFechaHasta').val());
            if (date1 > date2) {
                $('#txtFechaHasta').val(date);
            }
        }
    });

    $('#txtFechaHasta').Zebra_DatePicker({
        direction: true
    });        

    $('#btnExportar').click(function () {
        exportar();
    });

    $('#btnCancelar').click(function () {
        document.location.href = siteRoot + "home/default";
    });

    $('#btnConsultar').click(function () {
        consultar();
    });

    $('#cbEmpresa').change(function () {
        cargarGrupos();
    });

    consultar();
});

consultar = function () {
    $.ajax({
        type: "POST",
        url: controler + "listar",
        data: {
            fechaDesde: $('#txtFechaDesde').val(),
            fechaHasta: $('#txtFechaHasta').val(),
            idEmpresa: $('#cbEmpresa').val(),
            idGrupo: $('#cbGrupo').val(),
            idTipoCombustible: $('#cbCombustible').val()
        },
        success: function (evt) {
            $('#listado').html(evt);
            oTable = $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true"
            });
        },
        error: function (req, status, error) {
            mostrarError();
        }
    });
}

cargarGrupos = function () {

    $('#cbGrupo').get(0).options.length = 0;

    if ($('#cbEmpresa').val() != "") {
        $.ajax({
            type: 'POST',
            url: controler + 'obtenergrupos',
            dataType: 'json',
            data: { idEmpresa: $('#cbEmpresa').val() },
            cache: false,
            success: function (aData) {
                $('#cbGrupo').get(0).options.length = 0;
                $('#cbGrupo').get(0).options[0] = new Option("-SELECCIONE-", "");
                $.each(aData, function (i, item) {
                    $('#cbGrupo').get(0).options[$('#cbGrupo').get(0).options.length] = new Option(item.Gruponomb, item.PtoMediCodi);
                });
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else
    {
        $('#cbGrupo').get(0).options[0] = new Option("-SELECCIONE-", "");
    }
}

exportar = function () {
    $.ajax({
        type: 'POST',
        url: controler + "exportar",
        dataType: 'json',
        data: {
            fechaDesde: $('#txtFechaDesde').val(),
            fechaHasta: $('#txtFechaHasta').val(),
            idEmpresa: $('#cbEmpresa').val(),
            idGrupo: $('#cbGrupo').val(),
            idTipoCombustible: $('#cbCombustible').val()
        },
        success: function (result) {
            if (result == 1) {
                window.location = controler + 'descargar'
            }            
            else {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

getFecha = function (date) {
    var parts = date.split("/");
    var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date.getTime();
}

mostrarError = function () {
    alert("Ha ocurrido un error.");
}