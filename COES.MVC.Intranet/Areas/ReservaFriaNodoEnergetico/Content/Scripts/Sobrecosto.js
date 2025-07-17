var controlador = siteRoot + 'reservafrianodoenergetico/sobrecosto/';

$(function () {

    $('#txtFechaDesde').Zebra_DatePicker({
    });

    $('#txtFechaHasta').Zebra_DatePicker({
    });

    $('#btnNuevo').click(function() {
        editar(0, 1);
    });

    $('#rbSobrecostoVigente').prop('checked', true);
    
    $('#btnBuscar').click(function () {
        buscar();
    });

    $(document).ready(function () {
        buscar();
    });
});

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


    if ($('#rbSobrecostoVigente').is(':checked')) {
        estado = 'N';
    }
    else {
        if ($('#rbSobrecostoEliminado').is(':checked')) {
            estado = 'S';
        }
        else {

            if ($('#rbSobrecostoTodos').is(':checked')) {
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
            nrscFechaIni: $('#txtFechaDesde').val(),
            nrscFechaFin: $('#txtFechaHasta').val(),
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
            nrscFechaIni: $('#txtFechaDesde').val(),
            nrscFechaFin: $('#txtFechaHasta').val(),
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

    $.ajax({
        type: 'POST',
        url: controlador + "editar",
        cache: false,
        data: {
            id: id,
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

            configurarSobrecosto();            
        }
    });    
}

configurarSobrecosto = function()
{
    $('#txtNrscFecha').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#txtNrscFecha').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtNrscFecha').val(date);
        }
    });

    $('#txtNrscFecCreacion').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#txtNrscFecCreacion').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtNrscFecCreacion').val(date);
        }
    });

    $('#txtNrscFecModificacion').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#txtNrscFecModificacion').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtNrscFecModificacion').val(date);
        }
    });

    $('#btnCancelar').click(function () {
        document.location.href = controlador;
    });

    $('#btnCancelar2').click(function () {
        document.location.href = controlador;
    });

    $(document).ready(function () {

        $('#rbNrscEliminadoN').prop('checked', true);


        if ($('#hfNrscEliminado').val() == 'S') { $('#rbNrscEliminadoS').prop('checked', true); }
        if ($('#hfNrscEliminado').val() == 'N') { $('#rbNrscEliminadoN').prop('checked', true); }

        if ($('#hfAccion').val() == 0) {
            $('#btnGrabar').hide();
        }

    });

    $('#btnGrabar').click(function () {
        grabar();
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
mostrarAlerta = function (mensaje) {
    $('#mensajeEditar').removeClass();
    $('#mensajeEditar').addClass('action-alert');
    $('#mensajeEditar').html(mensaje);
}

mostrarExito = function () {
    $('#mensajeEditar').removeClass();
    $('#mensajeEditar').addClass('action-exito');
    $('#mensajeEditar').html('La operación se realizó con éxito.');
}

validarRegistro = function () {

    var mensaje = "<ul>";
    var flag = true;

    $('#hfGrupoCodi').val($('#cbxGrupoCodi').val());

    if ($('#rbNrscEliminadoS').is(':checked')) {
        $('#hfNrscEliminado').val('S');
    }

    if ($('#rbNrscEliminadoN').is(':checked')) {
        $('#hfNrscEliminado').val('N');
    }

    if (!validarDecimal($('#txtNrscCodespacho0').val())) {
        mensaje += "<li>CO Despacho0 debe ser numérico</li>";
        flag = false;
    }

    if (!validarDecimal($('#txtNrscCodespacho1').val())) {
        mensaje += "<li>CO Despacho1 debe ser numérico</li>";
        flag = false;
    }

    if (flag) mensaje = "";
    return mensaje;
}

validarDecimal = function (valor) {
    var RE = /^\d*\.?\d*$/;
    if (RE.test(valor)) {
        return true;
    } else {
        return false;
    }
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
                if (result != "-1") {
                    mostrarExito();
                    $('#hfNrscCodi').val(result);
                    document.location.href = controlador;
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
    else {
        mostrarAlerta(mensaje);
    }
}

