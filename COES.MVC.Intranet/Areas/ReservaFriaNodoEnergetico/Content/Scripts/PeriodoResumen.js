var controlador = siteRoot + 'reservafrianodoenergetico/periodoresumen/';

$(function () {

    $('#txtFechaDesde').inputmask({
        mask: "y-m",
        placeholder: "mm/yyyy",
        alias: "datetime"
    });

    $('#txtFechaDesde').Zebra_DatePicker({
        format: 'Y/m',
        readonly_element: false,
        onSelect: function (date) {
            $('#txtFechaDesde').val(date);
        }
    });

    $('#txtFechaHasta').inputmask({
        mask: "y-m",
        placeholder: "mm/yyyy",
        alias: "datetime"
    });

    $('#txtFechaHasta').Zebra_DatePicker({
        format: 'Y/m',
        readonly_element: false,
        onSelect: function (date) {
            $('#txtFechaHasta').val(date);
        }
    });

    $('#rbPeriodoVigente').prop('checked', true);


    $('#btnNuevo').click(function() {
        procesar(0, 0);
    });
    
    $('#btnBuscar').click(function () {
        buscar();
    });
    
    $(document).ready(function () {
        $('#cbNrsmodNombre').val($('#hfNrsmodNombre').val());
        buscar();
    });
});

obtenerEstado = function () {
    var estado = "N";
    return estado;
}

convertirFecha = function (dateStr) {
    var parts = dateStr.split("-");
    return new Date(parts[0], parts[1] - 1, "01");
}

buscar = function () {

    var fechaini = convertirFecha($('#txtFechaDesde').val());
    var fechafin = convertirFecha($('#txtFechaHasta').val());

    if (fechaini <= fechafin) {
        pintarPaginado();
        mostrarListado(1);
    } else {
        alert("Periodo inicial supera al final");
    }
}

pintarPaginado = function () {

    var estado = obtenerEstado();
    
    $.ajax({
        type: 'POST',
        url: controlador + "paginado",
        data: {            
            nrsmodCodi: $('#cbNrsmodNombre').val(),
            estado: estado,
            nrperFecIni: $('#txtFechaDesde').val(),
            nrperFecFin: $('#txtFechaHasta').val(),
        },
        success: function (evt) {            
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            mostrarError();
        }
    });
}

pintarBusqueda = function (nroPagina) {
    mostrarListado(nroPagina);
}

mostrarListado = function (nroPagina) {

    var estado = obtenerEstado();

    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "lista",
        data: {            
            nrsmodCodi: $('#cbNrsmodNombre').val(),
            estado: estado,
            nrperFecIni: $('#txtFechaDesde').val(),
            nrperFecFin: $('#txtFechaHasta').val(),
            nroPage: nroPagina
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50
            });

        },
        error: function () {
            mostrarError();
        }
    });
}

procesar = function (idNrsmodCodi, idNrperCodi) {
    document.location.href = controlador + "editar?idNrsmodCodi=" + idNrsmodCodi + "&idNrperCodi=" + idNrperCodi;
}

eliminar = function (id) {

    if (confirm('¿Está seguro de eliminar este registro?')) {

        $.ajax({
            type: 'POST',
            url: controlador + "desactivar",
            dataType: 'json',
            cache: false,
            data: { id: id },
            success: function (resultado) {
                if (resultado == 1) {
                    buscar();
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
}

mostrarError = function () {
    alert('Ha ocurrido un error.');
}

