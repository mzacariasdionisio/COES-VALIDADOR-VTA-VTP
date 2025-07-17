var controlador = siteRoot + 'cortoplazo/equipo/';

$(function () {

    $('#txtFechaDesde').Zebra_DatePicker({
    });

    $('#txtFechaHasta').Zebra_DatePicker({
    });

    $('#btnNuevo').click(function () {
        editar(0, 1);
    });



    $('#btnBuscar').click(function () {
        buscar();
    });

    $(document).ready(function () {
        $('#cbFamAbrev').val(4);
        buscar();
    });

});

buscar = function () {
    pintarPaginado();
    mostrarListado(1);
}

pintarPaginado = function () {

    $.ajax({
        type: 'POST',
        url: controlador + "paginado",
        data: {
            famCodi: $('#cbFamAbrev').val(),
            emprCodi: $('#cbEmprNomb').val(),
            areaCodi: $('#cbAreaabrev').val()
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

    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "lista",
        data: {
            emprCodi: $('#cbEmprNomb').val(),
            areacodi: $('#cbAreaabrev').val(),
            famCodi: $('#cbFamAbrev').val(),
            lastdate: $('#txtFechaDesde').val(),
            equiFechiniopcom: $('#txtFechaHasta').val(),
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

editar = function (equipo, propiedad, fecha, accion) {
    //document.location.href = controlador + "editar?equipo=" + equipo + "&propcodi=" + propiedad + "&fecha=" + fecha + "&accion=" + accion;

    $.ajax({
        type: 'POST',
        url: controlador + "editar",
        cache: false,
        data: {
            equipo: equipo,
            propcodi: propiedad,
            fecha: fecha,
            accion: accion,
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

            configurarEdicion();
        }
    });
}

configurarEdicion = function () {
    $('#txtfecha').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#txtfecha').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtfecha').val(date);
        }
    });

    $('#btnCancelar').click(function () {
        document.location.href = controlador;
    });

    $(document).ready(function () {

        $('#rbEquiEstadoN').prop('checked', true);
        if ($('#hfEquiEstado').val() == "NO TIENE") { $('#rbEquiEstadoN').prop('checked', true); }
        if ($('#hfEquiEstado').val() == "ESTACIONAL") { $('#rbEquiEstadoE').prop('checked', true); }
        if ($('#hfEquiEstado').val() == "HORARIA") { $('#rbEquiEstadoH').prop('checked', true); }
        if ($('#hfEquiEstado').val() == "DIARIA") { $('#rbEquiEstadoD').prop('checked', true); }
        if ($('#hfEquiEstado').val() == "SEMANAL") { $('#rbEquiEstadoS').prop('checked', true); }
        if ($('#hfEquiEstado').val() == "MENSUAL") { $('#rbEquiEstadoM').prop('checked', true); }
        if ($('#hfEquiEstado').val() == "ANUAL") { $('#rbEquiEstadoA').prop('checked', true); }
        if ($('#hfEquiEstado').val() == "PLURIANUAL") { $('#rbEquiEstadoP').prop('checked', true); }

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



validarRegistro = function () {

    var mensaje = "<ul>";
    var flag = true;

    $('#hfEmprCodi').val($('#cbxEmprCodi').val());
    $('#hfAreacodi').val($('#cbxAreacodi').val());
    $('#hfFamCodi').val($('#cbxFamCodi').val());

    if ($('#rbEquiEstadoN').is(':checked')) {
        $('#hfEquiEstado').val('NO TIENE');
    }

    if ($('#rbEquiEstadoE').is(':checked')) {
        $('#hfEquiEstado').val('ESTACIONAL');
    }

    if ($('#rbEquiEstadoH').is(':checked')) {
        $('#hfEquiEstado').val('HORARIA');
    }

    if ($('#rbEquiEstadoD').is(':checked')) {
        $('#hfEquiEstado').val('DIARIA');
    }

    if ($('#rbEquiEstadoS').is(':checked')) {
        $('#hfEquiEstado').val('SEMANAL');
    }

    if ($('#rbEquiEstadoM').is(':checked')) {
        $('#hfEquiEstado').val('MENSUAL');
    }

    if ($('#rbEquiEstadoA').is(':checked')) {
        $('#hfEquiEstado').val('ANUAL');
    }

    if ($('#rbEquiEstadoP').is(':checked')) {
        $('#hfEquiEstado').val('PLURIANUAL');
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
            data:
                {
                    propcodi: $('#hfPropCodi').val(),
                    equicodi: $('#hfEquiCodi').val(),
                    fecha: $('#txtfecha').val(),
                    valor: $('#hfEquiEstado').val()
                },
            success: function (result) {
                if (result != "-1") {

                    mostrarExito();

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