var controlador = siteRoot + 'formulas/'

$(function () {
    
    $('#FechaDesde').Zebra_DatePicker({
    });

    $('#FechaHasta').Zebra_DatePicker({
    });      

    lista();

    $('#btnBuscar').click(function () {
        buscar();
    });

    $('#btnNuevo').click(function () {
        location.href = controlador + "formula/index";
    });

    $('#btnExportar').click(function () {
        exportar();
    });

    $('#btnRegresar').click(function () {
        document.location.href = controlador + "formula/lista";
    });
});

function buscar()
{
    lista();
}

function lista() {
    $.ajax({
        type: "POST",
        url: controlador + "formula/grid",
        data: { fechaInicio: $('#FechaDesde').val(), fechaFin: $('#FechaHasta').val() },
        success: function (evt) {
            $('#listado').html(evt);
            oTable = $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "aaSorting": [[ 0, "desc" ]],
                "destroy": "true"
            });
        },
        error: function () {
            mostrarError();
        }
    });
}

function mostrarDetalle(id)
{
    location.href = controlador + "formula/detalle?id=" + id;
}

function exportar() {
    $.ajax({
        type: 'POST',
        url: controlador + 'formula/generararchivo',
        data: { fechaInicio: $('#FechaDesde').val(), fechaFin: $('#FechaHasta').val() },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "formula/exportar";
            }
            if (result == -1) {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

function mostrarError() {
    alert("Error");
}