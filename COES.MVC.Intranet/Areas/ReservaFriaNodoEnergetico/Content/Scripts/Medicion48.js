var controlador = siteRoot + 'reservafrianodoenergetico/medicion48/';

$(function () {

    $('#txtFechaDesde').Zebra_DatePicker({
    });

    $('#txtFechaHasta').Zebra_DatePicker({
    });

    $('#btnNuevo').click(function () {
        var fecha = new Date();
        var dd = fecha.getDate();
        var mm = fecha.getMonth() + 1;
        var yyyy = fecha.getFullYear();
        dd = (dd < 10 ? "0" + dd : dd);
        mm = (mm < 10 ? "0" + mm : mm);
        //var fechaymd = yyyy + "-" + mm + "-" + dd;
        var fechaymd = yyyy + "" + mm + "" + dd + "";

        editar(86, fechaymd, 3, -1, 1);
    });


    $('#btnBuscar').click(function () {
        buscar();
    });

    $(document).ready(function () {
        $('#cbTipoinfoAbrev').val("1");
        $('#cbTipoinfoAbrev').prop("disabled", true);

        $('#cbLectAbrev').val($('#hfLectAbrev').val());
        $('#cbPtomediDesc').val($('#hfPtomediDesc').val());

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

pintarPaginado = function () {

    $.ajax({
        type: 'POST',
        url: controlador + "paginado",
        data: {
            lectcodi: $('#cbLectAbrev').val(),
            tipoinfocodi: $('#cbTipoinfoAbrev').val(),
            ptomedicodi: $('#cbPtomediDesc').val(),
            fechaini: $('#txtFechaDesde').val(),
            fechafin: $('#txtFechaHasta').val(),
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
            lectcodi: $('#cbLectAbrev').val(),
            tipoinfocodi: $('#cbTipoinfoAbrev').val(),
            ptomedicodi: $('#cbPtomediDesc').val(),
            fechaini: $('#txtFechaDesde').val(),
            fechafin: $('#txtFechaHasta').val(),
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

editar = function (lectcodi, medifecha, tipoinfocodi, ptomedicodi, accion) {

    document.location.href = controlador +
        "editar?idlect=" +
        lectcodi +
        "&fecha=" +
        medifecha +
        "&idtipoinfo=" +
        tipoinfocodi +
        "&id=" +
        ptomedicodi +
        "&accion=" +
        accion;
}

eliminar = function (idlect, medifecha, tipoinfocodi, ptomedicodi) {

    if (confirm('¿Está seguro de eliminar este registro?')) {

        $.ajax({
            type: 'POST',
            url: controlador + "eliminar",
            dataType: 'json',
            cache: false,
            data: {
                idlect: idlect,
                fecha: medifecha,
                idtipoinfo: tipoinfocodi,
                id: ptomedicodi
            },
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

