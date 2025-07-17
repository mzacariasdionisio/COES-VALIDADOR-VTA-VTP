var controlador = siteRoot + 'Equipamiento/equipo/';
$(function () {
    $('#cbTipoEmpresa').val(-2);
    $('#cbEmpresa').val(-2);
    $('#cbTipoEquipo').val(-2);
    cargarEmpresas();
    buscarEquipos();
    $('#btnBuscar').click(function () {
        buscarEquipos();
    });
    $('#cbTipoEmpresa').change(function () {
        cargarEmpresas();
        return false;
    });
    $(window).resize(function () {
        $('#listado').css("width", $('#mainLayout').width() + "px");
    });
});
cargarEmpresas = function () {
    $.ajax({
        type: 'GET',
        url: controlador + '/CargarEmpresas',
        dataType: 'json',
        data: { idTipoEmpresa: $('#cbTipoEmpresa').val() },
        cache: false,
        success: function (aData) {
            $('#cbEmpresa').get(0).options.length = 0;
            $('#cbEmpresa').get(0).options[0] = new Option("TODOS", "-2");
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
    pintarPaginado();
    mostrarListado(1);
};
pintarPaginado = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "PaginadoEquipos",
        data: $('#frmBusquedaEquipo').serialize(),
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            mostrarError();
        }
    });
};
mostrarListado = function (nroPagina) {
    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "ListadoEquipos",
        data: $('#frmBusquedaEquipo').serialize(),
        success: function (evt) {

            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": false,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50
            });

            //$.get(evt, function (data) {
            //    $("#listado").html("");
            //    for (var i = 0; i < data.datos.length; i++) {
            //        var tr = `<tr>
            //              <td>`+ data.datos[i].nombre + `</td>
            //              <td>`+ data.datos[i].apellido + `</td>
            //              <td>`+ data.datos[i].cargo + `</td>
            //              <td>`+ data.datos[i].empresa + `</td>
            //            </tr>`;
            //        $("#listado").append(tr)
            //    }
            //});
        },
        error: function () {
            mostrarError();
        }
    });
};
pintarBusqueda = function (nroPagina) {
    mostrarListado(nroPagina);
};