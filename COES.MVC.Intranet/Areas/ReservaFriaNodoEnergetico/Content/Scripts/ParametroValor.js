var controlador = siteRoot + 'reservafrianodoenergetico/parametrovalor/';

$(function () {

    $('#txtFechaDesde').Zebra_DatePicker({
    });

    $('#txtFechaHasta').Zebra_DatePicker({
    });

    $('#btnNuevo').click(function() {
        editar(0, 1);
    });

    $('#rbValorVigente').prop('checked', true);
    
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

    if ($('#rbValorVigente').is(':checked')) {
        estado = 'N';
    }
    else {
        if ($('#rbValorEliminado').is(':checked')) {
            estado = 'S';
        }
        else {

            if ($('#rbValorTodos').is(':checked')) {
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
            siparCodi: $('#cbSiparAbrev').val(),
            siparvFechaInicial: $('#txtFechaDesde').val(),
            siparvFechaFinal: $('#txtFechaHasta').val(),
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
            siparCodi: $('#cbSiparAbrev').val(),
            siparvFechaInicial: $('#txtFechaDesde').val(),
            siparvFechaFinal: $('#txtFechaHasta').val(),
            nroPage: nroPagina,
            estado: estado
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
            
            configurarValor();
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


configurarValor= function()
{
    $('#txtSiparvFechaInicial').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#txtSiparvFechaInicial').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtSiparvFechaInicial').val(date);
        }
    });

    $('#txtSiparvFechaFinal').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#txtSiparvFechaFinal').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtSiparvFechaFinal').val(date);
        }
    });

    $(document).ready(function () {
        
        if ($('#hfSiparvEliminado').val() == 'S') { $('#rbSiparvEliminadoS').prop('checked', true); $('#rbSiparvEliminadoN').prop('checked', false); }
        if ($('#hfSiparvEliminado').val() == 'N') { $('#rbSiparvEliminadoS').prop('checked', false); $('#rbSiparvEliminadoN').prop('checked', true); }
        
        $('#cbSiparCodi').val($('#hfSiparCodi').val());

        if ($('#hfAccion').val() == 0) {
            $('#btnGrabar').hide();
        }
    });

    $('#btnGrabar').click(function () {
        grabar();
    });
}

validarNumero = function (valor) {
    return !isNaN(parseFloat(valor)) && isFinite(valor);
}

mostrarAlerta = function (mensaje) {
    $('#mensajeEdit').removeClass();
    $('#mensajeEdit').addClass('action-alert');
    $('#mensajeEdit').html(mensaje);
}

mostrarExito = function () {
    $('#mensajeEdit').removeClass();
    $('#mensajeEdit').addClass('action-exito');
    $('#mensajeEdit').html('La operación se realizó con éxito.');
}

validarRegistro = function () {

    var mensaje = "<ul>";
    var flag = true;

    $('#hfSiparCodi').val($('#cbxSiparCodi').val());

    if ($('#rbSiparvEliminadoS').is(':checked')) {
        $('#hfSiparvEliminado').val('S');
    }

    if ($('#rbSiparvEliminadoN').is(':checked')) {
        $('#hfSiparvEliminado').val('N');
    }

    if (!validarNumero($('#txtSiparvValor').val())) {
        mensaje = mensaje + "<li>Valor ingresado no es numérico</li>";
        flag = false;
    }

    var fhIni = $('#txtSiparvFechaInicial').val();
    var fhFin = $('#txtSiparvFechaFinal').val();


    var ldIni = convertDate(fhIni);
    var ldFin = convertDate(fhFin);



    if (ldFin < ldIni) {
        mensaje = mensaje + "<li>Corregir rango de fechas: Fecha inicial supera a la final</li>";
        flag = false;
    }

    var nota = String($('#txtSiparvNnota').val())

    if (nota + "X" == "X") {
        mensaje = mensaje + "<li>Debe ingresar nota</li>";
        flag = false;
    }
    
    if (flag) mensaje = "";
    return mensaje;
}

convertDate = function (fechahora) {

    var fh = fechahora.toString().split(" ");
    var fecha = fh[0].split("/");
    var dd = fecha[0];
    var mm = fecha[1] - 1;
    var yyyy = fecha[2];

    return new Date(yyyy, mm, dd, 0, 0, 0);
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
                    $('#hfSiparvCodi').val(result);
                    document.location.href = controlador;
                                        
                    //cerrar popup
                    $('#popupEdicion').bPopup().close();

                    //actualizar grid
                    mostrarListado(1);
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

