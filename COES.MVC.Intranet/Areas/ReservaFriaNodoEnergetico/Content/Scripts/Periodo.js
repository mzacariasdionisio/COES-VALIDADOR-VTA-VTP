var controlador = siteRoot + 'reservafrianodoenergetico/periodo/';

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
        editar(0, 1);
    });

    
    $('#btnBuscar').click(function () {
        buscar();
    });

    $(document).ready(function () {
        buscar();
    });

});

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
            estado: estado,
            fechaIni: $('#txtFechaDesde').val() + "-01",
            fechaFin: $('#txtFechaHasta').val() + "-01"            
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

obtenerEstado= function()
{
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

mostrarListado = function (nroPagina) {

    var estado = obtenerEstado();    

    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "lista",
        data: {
            estado:estado,
            fechaIni: $('#txtFechaDesde').val() + "-01",
            fechaFin: $('#txtFechaHasta').val() + "-01",
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

editar = function (idNrperCodi, accion) {

    $.ajax({
        type: 'POST',
        url: controlador + "editar",
        cache: false,
        data: {
            id:idNrperCodi,
            accion: accion
        },
        success: function (evt) {

            $('#contenidoEdicion').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
            
            configurarPeriodo();            
        }
    });   
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


//funciones editar
validarRegistro = function () {

    var mensaje = "<ul>";
    var flag = true;

    if ($('#rbNrperEliminadoS').is(':checked')) {
        $('#hfNrperEliminado').val('S');
    }

    if ($('#rbNrperEliminadoN').is(':checked')) {
        $('#hfNrperEliminado').val('N');
    }

    if (flag) mensaje = "";
    return mensaje;
}

grabar = function () {
    var mensaje = validarRegistro();

    if (mensaje == "") {

        $.ajax({
            type: 'POST',
            url: controlador + "grabar",
            dataType: 'json',
            data: $('#form').serialize(),
            success: function (result) {
                if (result == "-2") {
                    alert("Periodo ya existe registrado");                    
                }
                else {

                    if (result != "-1") {

                        mostrarExito();
                        $('#hfNrperCodi').val(result);
                        
                        //cerrar popup
                        $('#popupEdicion').bPopup().close();

                        //actualizar grid
                        mostrarListado(1);
                        
                    }
                    else {
                        mostrarError();
                    }

                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        mostrarAlerta(mensaje);
    }
}

mostrarAlerta = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-alert');
    $('#mensaje').html(mensaje);
}

mostrarExito = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-exito');
    $('#mensaje').html('La operación se realizó con éxito.');
}

configurarPeriodo = function () {
    $('#txtNrperMes').inputmask({
        mask: "y-m",
        placeholder: "mm/yyyy",
        alias: "datetime"
    });

    $('#txtNrperMes').Zebra_DatePicker({
        format: 'Y/m',
        readonly_element: false,
        onSelect: function (date) {
            $('#txtNrperMes').val(date);
        }
    });

    $(document).ready(function () {

        if ($('#hfNrperEliminado').val() == 'S') { $('#rbNrperEliminadoS').prop('checked', true); }
        if ($('#hfNrperEliminado').val() == 'N') { $('#rbNrperEliminadoN').prop('checked', true); }

        if ($('#hfAccion').val() == 0) {
            $('#btnGrabar').hide();
        }
    });

    $('#rbNrperEliminadoN').prop('checked', true);    
    $('#btnGrabar').click(function () {
        grabar();
    });
}