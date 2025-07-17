var controlador = siteRoot + 'eventos/pruebaunidad/';

$(function () {

    $('#txtFechaDesde').Zebra_DatePicker({
    });

    $('#txtFechaHasta').Zebra_DatePicker({
    });

    $('#btnNuevo').click(function () {
        editar(0,1);
    });
    
    $('#rbPeriodoVigente').prop('checked', true);

    $('#btnBuscar').click(function () {
        buscar();
    });

    //$('#txtFechaDesde').val("01/01/2017");

    $(document).ready(function () {
        buscar();
    });
});

buscar = function () {
    pintarPaginado();
    mostrarListado(1);
}

obtenerEstado = function () {
    var estado = "N";

    if ($('#rbPeriodoVigente').is(':checked')) {
        estado = 'N';
    }
    else {
        if ($('#rbPeriodoEliminado').is(':checked')) {
            estado = 'S';
        }
        else {

            if ($('#rbPeriodoTodos').is(':checked')) {
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
            estado: estado,
            prundFechaIni: $('#txtFechaDesde').val(),
            prundFechaFin: $('#txtFechaHasta').val(),
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
            estado: estado,
            prundFechaIni: $('#txtFechaDesde').val(),
            prundFechaFin: $('#txtFechaHasta').val(),
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
    var crearRegistro = false;
    var unidadSorteada = "";
    
    if (id == 0)
    {
        //validar si hay sorteo
        $.ajax({
            type: 'POST',
            url: controlador + "validarSorteo",
            data: {
            },
            dataType: 'json',
            cache: false,
            success: function (evt) {

                var cad = evt;                                
                var n = cad.indexOf(",");                

                var equicodi = cad.substr(0, n);
                var equinomb = cad.substr(n + 1);

                if (equicodi != "-1" && equicodi != "-2") {

                    //--------------
                    document.location.href = controlador + "editar?id=" + id + "&accion=" + accion;
                    //--------------
                }
                else
                {
                    if (equicodi != "-2") {



                        if (confirm('No se encontró unidad sorteada o no se realizó sorteo.\nDesea crear el registro sin unidad sorteada?')) {
                            //crear Registro
                            $.ajax({
                                type: 'POST',
                                url: controlador + "crearregistrosinsorteo",
                                data: {
                                },
                                dataType: 'json',
                                cache: false,
                                success: function (evt) {
                                    if (evt != "-1") {
                                        //actualizar listado
                                        buscar();
                                    }
                                    else {
                                        mostrarError();
                                    }
                                },
                                error: function (xhr, textStatus, exceptionThrown) {
                                    mostrarError();
                                }
                            });
                        }
                    }
                    else
                    {
                        alert("Registro ya se encuentra creado.\nNo se puede continuar...");
                    }
                }




            },
            error: function (xhr, textStatus, exceptionThrown) {
                mostrarError();
            }
        });

    }
    else
    {
        //--------------
        document.location.href = controlador + "editar?id=" + id + "&accion=" + accion;
        //--------------
    }

    
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

