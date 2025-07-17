var controlador = siteRoot + 'reservafrianodoenergetico/potenciaconsigna/';

$(function () {

    $('#txtFechaDesde').Zebra_DatePicker({
    });

    $('#txtFechaHasta').Zebra_DatePicker({
    });

    $('#btnNuevo').click(function () {
        editar(0, 1);
    });

    $('#rbPConsignaVigente').prop('checked', true);
    
    $('#cbNrsmodNombre').on('change', function () {
        cargarModoOperacion(this.value, 'cbGrupoNomb',true);
    });
 
    $('#btnBuscar').click(function () {
        buscar();
    });

    $(document).ready(function() {
        $('#cbNrsmodNombre').val($('#hfNrsmodNombre').val());
        $('#cbGrupoNomb').val($('#hfGrupoNomb').val());
        configuraEstado($('#hfEstado').val());

        buscar();
    });
});

configuraEstado = function (estado) {
    
    switch (estado) {
        case "N":
            $('#rbPConsignaVigente').prop('checked', true);
            break;
        case "S":
            $('#rbPConsignaEliminado').prop('checked', true);
            break;
        case "T":
            $('#rbPConsignaTodos').prop('checked', true);
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


obtenerEstado = function () {
    var estado = "N";


    if ($('#rbPConsignaVigente').is(':checked')) {
        estado = 'N';
    }
    else {
        if ($('#rbPConsignaEliminado').is(':checked')) {
            estado = 'S';
        }
        else {

            if ($('#rbPConsignaTodos').is(':checked')) {
                estado = 'T';
            }
            else {
                estado = 'S';
            }
        }
    }
    return estado;
}

pintarPaginado = function () {

    var estado = obtenerEstado();

    $.ajax({
        type: 'POST',
        url: controlador + "paginado",
        data: {            
            nrsmodCodi: $('#cbNrsmodNombre').val(),
            grupoCodi: $('#cbGrupoNomb').val(),
            nrpcFechaIni: $('#txtFechaDesde').val(),
            nrpcFechaFin: $('#txtFechaHasta').val(),
            estado: estado
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

cargarModoOperacion = function(nrsmodcodi,control,todos)
{
    
    $.ajax({
        type: 'POST',
        url: controlador + "cargarmodooperacion",
        dataType: 'json',
        cache: false,
        data: {
            nrsmodCodi: nrsmodcodi
        },
        success: function (evt) {

            var _len = evt.length;
            var cad1 = _len + "\r\n";

            $('#' + control).empty();

            if (todos)
            {
                var select = document.getElementById(control);
                select.options[0] = new Option("TODOS", 0);
            }

            for (i = 0; i < _len; i++) {

                post = evt[i];
                var select = document.getElementById(control);
                select.options[select.options.length] = new Option(post.Gruponomb, post.Grupocodi);                
            }            
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
            grupoCodi: $('#cbGrupoNomb').val(),
            nrpcFechaIni: $('#txtFechaDesde').val(),
            nrpcFechaFin: $('#txtFechaHasta').val(),
            estado:estado,
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
