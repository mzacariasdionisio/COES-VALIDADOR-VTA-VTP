var controlador = siteRoot + 'reservafrianodoenergetico/proceso/';

$(function () {

    $('#txtFechaDesde').Zebra_DatePicker({
    });

    $('#txtFechaHasta').Zebra_DatePicker({
    });

    $('#btnNuevo').click(function () {
        editar(0, 1);
    });

    $('#rbProcesoVigente').prop('checked', true);

   

    $('#btnBuscar').click(function () {
        buscar();
    });
    
    $(document).ready(function () {
        $('#cbNrcptAbrev').val($('#hfNrcptAbrev').val());
        $('#cbGrupoNomb').val($('#hfGrupoNomb').val());
        configuraEstado($('#hfEstado').val());

        buscar();
    });
});

configuraEstado = function (estado) {

    switch (estado) {
        case "N":
            $('#rbProcesoVigente').prop('checked', true);
            break;
        case "S":
            $('#rbProcesoEliminado').prop('checked', true);
            break;
        case "T":
            $('#rbProcesoTodos').prop('checked', true);
            break;
    default:
    }    
}

convertirFecha = function (dateStr) {
    var parts = dateStr.split("/");
    return new Date(parts[2], parts[1] - 1, parts[0]);
}

buscar = function () {
    var fechaini = convertirFecha($('#txtFechaDesde').val());
    var fechafin = convertirFecha($('#txtFechaHasta').val());

    if (fechaini <= fechafin) {
        pintarPaginado();
        mostrarListado(1);
    } else {
        alert("Fecha inicial supera a la final");
    }    
}

pintarPaginado = function () {
    var estado = obtenerEstado();

    $.ajax({
        type: 'POST',
        url: controlador + "paginado",
        data: {
            estado: estado,
            nrperCodi: 0,//$('#cbNrperMes').val(),
            grupoCodi: $('#cbGrupoNomb').val(),
            nrcptCodi: $('#cbNrcptAbrev').val(),
            nrprcFechaInicio: $('#txtFechaDesde').val(),
            nrprcFechaFin: $('#txtFechaHasta').val()
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

obtenerEstado = function () {
    var estado = "N";


    if ($('#rbProcesoVigente').is(':checked')) {
        estado = 'N';
    }
    else {
        if ($('#rbProcesoEliminado').is(':checked')) {
            estado = 'S';
        }
        else {

            if ($('#rbProcesoTodos').is(':checked')) {
                estado = 'T';
            }
            else {
                estado = 'S';
            }
        }
    }

    return estado;
}

mostrarListado = function (nroPagina) {
    var estado = obtenerEstado();
    
    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "lista",
        data: {
            estado: estado,
            nrperCodi: 0,//$('#cbNrperMes').val(),
            grupoCodi: $('#cbGrupoNomb').val(),
            nrcptCodi: $('#cbNrcptAbrev').val(),
            nrprcFechaInicio: $('#txtFechaDesde').val(),
            nrprcFechaFin: $('#txtFechaHasta').val(),
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

editar = function (id, accion) {
    document.location.href = controlador + "editar?id=" + id + "&accion=" + accion;
}

eliminar = function (id) {

    if (confirm('¿Está seguro de eliminar este registro?')) {

        $.ajax({
            type: 'POST',
            url: controlador + "eliminar",
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

