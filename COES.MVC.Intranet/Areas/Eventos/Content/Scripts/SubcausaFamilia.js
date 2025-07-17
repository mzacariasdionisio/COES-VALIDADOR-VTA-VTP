var controlador = siteRoot + 'eventos/subcausafamilia/'

$(function () {

    $('#btnNuevo').click(function () {
        editar(0,1);
    });

    

    $('#btnBuscar').click(function () {
        buscar();
    });


    $(document).ready(function () {
        $('#rbVigente').prop('checked', true);
        buscar();
    });


    

});

buscar = function () {
    pintarPaginado();
    mostrarListado(1);
}

pintarPaginado = function () {
    
    var estado = obtenerEstado();

    $.ajax({
        type: 'POST',
        url: controlador + "paginado",
        data: {
            estado:estado,
            subcausaCodi: $('#cbSubcausaDesc').val(),
            famCodi: $('#cbFamAbrev').val(),
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
            estado:estado,
            subcausaCodi: $('#cbSubcausaDesc').val(),
            famCodi: $('#cbFamAbrev').val(),
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

    //document.location.href = controlador + "editar?id=" + id + "&accion=" + accion;

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

            configurarRelacion();

        }
    });

}

configurarRelacion = function () {

    $(document).ready(function () {

        $('#rbScaufaEliminadoN').prop('checked', true);
        if ($('#hfScaufaEliminado').val() == 'S') { $('#rbScaufaEliminadoS').prop('checked', true); }
        if ($('#hfScaufaEliminado').val() == 'N') { $('#rbScaufaEliminadoN').prop('checked', true); }

        $('#cbSubcausaCodi').val($('#hfSubcausaCodi').val());
        $('#cbFamCodi').val($('#hfFamCodi').val());


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


obtenerEstado = function () {
    var estado = "N";


    if ($('#rbVigente').is(':checked')) {
        estado = 'N';
    }
    else {
        if ($('#rbEliminado').is(':checked')) {
            estado = 'S';
        }
        else {

            if ($('#rbTodos').is(':checked')) {
                estado = 'T';
            }
            else {
                estado = 'S';
            }

        }


    }

    return estado;
}



validarRegistro = function () {

    var mensaje = "<ul>";
    var flag = true;

    $('#hfSubcausaCodi').val($('#cbxSubcausaCodi').val());
    $('#hfFamCodi').val($('#cbxFamCodi').val());

    if ($('#rbScaufaEliminadoS').is(':checked')) {
        $('#hfScaufaEliminado').val('S');
    }

    if ($('#rbScaufaEliminadoN').is(':checked')) {
        $('#hfScaufaEliminado').val('N');
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
                    alert("Relación ya se encuentra registrada");
                }
                else {

                    if (result != "-1") {

                        mostrarExito();
                        $('#hfScaufaCodi').val(result);

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