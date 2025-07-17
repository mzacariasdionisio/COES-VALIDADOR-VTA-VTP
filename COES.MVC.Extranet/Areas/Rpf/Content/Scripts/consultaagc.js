var controlador = siteRoot + "rpf/envioagc/";
var hot = null;

$(function () {

    $('#txtFecha').Zebra_DatePicker({
        direction: false,
        onSelect: function () {
            cargarUnidad();
        }
    });

    $('#cbEmpresa').on('change', function () {
        cargarUrs();
    });

    $('#cbURS').on('change', function () {
        cargarUnidad();
    });

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    cargarUrs();
});

function cargarUrs() {
    $('#cbURS').get(0).options.length = 0;
    $('#cbURS').get(0).options[0] = new Option("--SELECCIONE--", "-1");
    if ($('#cbEmpresa').val() != "-1") {

        $.ajax({
            type: 'POST',
            url: controlador + 'ObtenerUrsPorEmpresa',
            dataType: 'json',
            data: {
                idEmpresa: $('#cbEmpresa').val()
            },
            cache: false,
            success: function (result) {
                $.each(result, function (i, item) {
                    $('#cbURS').get(0).options[$('#cbURS').get(0).options.length] = new Option(item.Ursnomb, item.Grupocodi);
                });

                cargarUnidad();
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }    
}

function cargarUnidad() {
    $('#cbUnidad').get(0).options.length = 0;
    $('#cbUnidad').get(0).options[0] = new Option("--SELECCIONE--", "-1");

    if ($('#cbURS').val() != "-1" && $('#txtFecha').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'ObtenerUnidadesURS',
            dataType: 'json',
            data: {
                idUrs: $('#cbURS').val(),
                fecha: $('#txtFecha').val()
            },
            cache: false,
            success: function (result) {
                $.each(result, function (i, item) {
                    $('#cbUnidad').get(0).options[$('#cbUnidad').get(0).options.length] = new Option(item.Equinomb, item.Equicodi);
                });
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
}

function consultar() {
    mostrarMensaje('mensaje', 'message', 'Por favor complete los datos exigidos.');
    if ($('#txtFecha').val() != "" && $('#cbURS').val() != "-1" && $('#cbUnidad').val() != "-1" && $('#cbTipoDato').val() != "-1"  &&
        $('#cbHoraDesde').val() != "" && $('#cbHoraHasta').val() != "") {

        $.ajax({
            type: 'POST',
            url: controlador + 'ObtenerConsultaDatos',
            dataType: 'json',
            data: {
                idUrs: $('#cbURS').val(),
                idEquipo: $('#cbUnidad').val(),
                fecha: $('#txtFecha').val(),
                tipoDato: $('#cbTipoDato').val(),
                minutoInicio: $('#cbHoraDesde').val(),
                minutoFin: $('#cbHoraHasta').val()
            },
            cache: false,
            success: function (result) {
                cargarTabla(result)

                if (result.length == 1) {
                    mostrarMensaje('mensaje', 'alert', 'No existen resultados para los filtros seleccionados.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Por favor seleccione todos los filtros.');
    }
}

function cargarTabla(result) {
    if (hot != null) {
        hot.destroy();
    }

    function headerRender(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '11px';
        td.style.color = '#ffffff';
        td.style.background = '#2980B9';
    }

    function contentRender(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '11px';
        td.style.textAlign = 'right';
    }


    var container = document.getElementById('contenido');
    var hotOptions = {
        data: result,
        height: 600,
        maxRows: result.length,
        maxCols: result[0].length,
      
       
        columnSorting: false,
        className: "htCenter",
        readOnly: true,
        fixedRowsTop: 1,
        fixedColumnsLeft: 1,
        cells: function (row, col, prop) {
            var cellProperties = {};

            if (row == 0) {
                cellProperties.renderer = headerRender;
            }
           
            else {
                cellProperties.renderer = contentRender;
            }
            return cellProperties;
        }
    };
    hot = new Handsontable(container, hotOptions);
}

function mostrarMensaje(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};
