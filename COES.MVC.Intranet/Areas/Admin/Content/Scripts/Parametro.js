var controlador = siteRoot + 'admin/parametro/'

$(function () {

    $('#btnNuevo').click(function () {
        editar(0,1);
    })

    buscar();

    $('#btnBuscar').click(function () {
        buscar();
    });

});

buscar = function () {
    pintarPaginado();
    mostrarListado(1);
}

pintarPaginado = function () {
    var abreviatura = "";
    var descripcion = "";

    if (String($('#txtAbreviatura').val()).trim() + "X" != "X")
        abreviatura = String($('#txtAbreviatura').val()).trim();

    if (String($('#txtDescripcion').val()).trim() + "X" != "X")
        descripcion = String($('#txtDescripcion').val()).trim();

    $.ajax({
        type: 'POST',
        url: controlador + "paginado",
        data: {
            abreviatura: abreviatura,
            descripcion: descripcion
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

    var abreviatura = "";
    var descripcion = "";
    
    if ( String($('#txtAbreviatura').val()).trim() + "X" != "X")
        abreviatura = String($('#txtAbreviatura').val()).trim();

    if ( String($('#txtDescripcion').val()).trim() + "X" != "X")
        descripcion = String($('#txtDescripcion').val()).trim();
    
    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "lista",
        data: {
            abreviatura: abreviatura,
            descripcion:descripcion,
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

            configurarParametro();
        }
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

configurarParametro= function()
{
    $('#txtSiparFecCreacion').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#txtSiparFecCreacion').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtSiparFecCreacion').val(date);
        }
    });

    $('#txtSiparFecModificacion').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#txtSiparFecModificacion').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtSiparFecModificacion').val(date);
        }
    });

    $(document).ready(function () {

        if ($('#hfAccion').val() == 0) {
            $('#btnGrabar').hide();
        }
    });

    $('#btnGrabar').click(function () {
        grabar();
    });
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
                if (result != "-1") {
                                       
                    mostrarExito();
                    $('#hfSiparCodi').val(result);
                    
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