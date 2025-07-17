var controlador = siteRoot + 'Equipamiento/equipo/';
$(function () {
    //$('#cbTipoEmpresa').val(-2);
    //$('#cbEmpresa').val(-2);
    $('#cbTipoEquipo').val(-2);
    $('#cbEstado').val(' ');
    cargarEmpresas();
    buscarEquipos();

    $('#btnBuscar').click(function () {
        buscarEquipos();
    });

    $('#btnCopiar').click(function () {
        var resultado = confirm("¿Está seguro de realizar la operación?");
        if (resultado) {
            copiarPropiedades();
        } else {
            alert("Se canceló la operación");
        }
    });
    
    $('#cbTipoEmpresa').change(function () {
        cargarEmpresas();
        return false;
    });

    $(window).resize(function () {
        $('#listado').css("width", $('#mainLayout').width() + "px");
    });
});
mostrarExitoOperacion = function () {
    $('#mensaje').removeClass("action-error");
    $('#mensaje').addClass("action-exito");
    $('#mensaje').text("La operación se realizó con exito...!");
    $('#mensaje').css("display", "block");
};

mostrarError = function () {
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
    $('#mensaje').css("display", "block");
};
cargarEmpresas = function () {
    $.ajax({
        type: 'GET',
        url: controlador + '/CargarEmpresas',
        dataType: 'json',
        data: { idTipoEmpresa: $('#cbTipoEmpresa').val() },
        cache: false,
        success: function (aData) {
            $('#cbEmpresa').get(0).options.length = 0;
            $.each(aData, function (i, item) {
                $('#cbEmpresa').get(0).options[$('#cbEmpresa').get(0).options.length] = new Option(item.Text, item.Value);
            });
        },
        error: function () {
            mostrarError();
        }
    });
};
buscarEquipos = function () {
    $('#mensaje').css("display", "none");
    mostrarListado(1);
};
mostrarListado = function (nroPagina) {
    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "ListadoEquiposCopia",
        data: $('#frmBusquedaEquipo').serialize(),
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": false,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50,
                "autoWidth": true,
                "bScrollCollapse": true
            });
            $('#tabla2').dataTable({
                "scrollY": 430,
                "scrollX": false,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50,
                "autoWidth": true,
                "bScrollCollapse": true
            });
        },
        error: function () {
            mostrarError();
        }
    });
};
copiarPropiedades = function() {
    var cantEquipoOrigen = $("input[name=equipoOrigen]:checked").length;
    var cantEquipoDestino = $("input[name=equipoDestino]:checked").length;
    if (cantEquipoOrigen > 0 && cantEquipoDestino > 0) {

        var elegidoOrigen = $("input[name=equipoOrigen]:checked").first();
        var elegidoDestino = $("input[name=equipoDestino]:checked").first();
        var codOrigen = elegidoOrigen.data('codigo');
        var codDestino = elegidoDestino.data('codigo');

        if (codOrigen != codDestino) {
            $.ajax({
                type: 'POST',
                url: controlador + "CopiarPropiedades",
                data: {
                    equipoOrigen: codOrigen,
                    equipoDestino: codDestino
                },
                success: function(resultado) {
                    if (resultado == 1) {
                        mostrarExitoOperacion();
                    } else
                        mostrarError();
                },
                error: function() {
                    mostrarError();
                }
            });
        } else {
            alert('Los equipos origen y destino no pueden ser iguales.');
        }


    } else {
        alert('Debe seleccionar un equipo origen y un equipo destino.');
    }
};