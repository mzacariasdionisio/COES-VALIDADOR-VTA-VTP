var controlador = siteRoot + 'Migraciones/Reporte/';

$(function () {

    $('#cbEmpresa').multipleSelect({
        width: '250px',
        filter: true
    });

    cargarEmpresas(0);

    $('#cbTipoEmpresa').change(function () {
        $('#rb10MW').hide(); $('#txt10MW').hide();
        if (this.value == "4") { $('#rb10MW').prop('checked', false); $('#rb10MW').show(); $('#rb10MW').show(); $('#txt10MW').show(); }
        cargarEmpresas(0);
        return false;
    });

    $(window).resize(function () {
        updateContainer();
    });

    $('#txtFechaInicio').Zebra_DatePicker({
        pair: $('#txtFechaFin'),
        onSelect: function (date) {
            var date1 = getFecha(date);
            var date2 = getFecha($('#txtFechaFin').val());
            if (date2 > date1) {
                $('#txtFechaFin').val(date);
            }
        }
    });
    $('#txtFechaFin').Zebra_DatePicker({
        direction: [0, 90]
    });

    $("#btnConsultar").click(function () {
        buscarDemandaBarras($(this).val());
    });


    $("#btnExportar").click(function () {
        exportarDemandaBarras();
    });
    $('#rb10MW').hide();
    $('#txt10MW').hide();

    $("#rb10MW").change(function () {
        if (this.checked) {
            cargarEmpresas(1);
        } else { cargarEmpresas(0); }
    });
});

function buscarDemandaBarras() {
    pintarPaginado();
    mostrarListado(1, $("#txtFechaInicio").val());
}


function getSmultipleSelect(tag) {
    var value = tag.multipleSelect('getSelects');
    if (value == "[object Object]") value = -1;
    if (value == "") value = "-1";
    return value;
}

cargarEmpresas = function (x) {
    $('#cbEmpresa').get(0).options.length = 0;
    $.ajax({
        type: 'GET',
        url: controlador + '/CargarEmpresas',
        dataType: 'json',
        data: { idTipoEmpresa: $('#cbTipoEmpresa').val(), tip: x },
        cache: false,
        ///async: false,
        success: function (aData) {
            $('#cbEmpresa').get(0).options.length = 0;
            $.each(aData, function (i, item) {
                $('#cbEmpresa').get(0).options[$('#cbEmpresa').get(0).options.length] = new Option(item.Text, item.Value);
            });

            $('#cbEmpresa').multipleSelect('refresh');
            $('#cbEmpresa').multipleSelect('checkAll');
        },
        error: function () {
            mostrarError();
        }
    });

};

function updateContainer() {
    var $containerWidth = $(window).width();

    $('#listado').css("width", $containerWidth - 250 + "px");
}

exportarDemandaBarras = function () {

    var dataSend = {
        idTipoEmpresa: $('#cbTipoEmpresa').val(),
        emprcodi: getSmultipleSelect($("#cbEmpresa")).join(","),
        lectcodi: $("input[name='demanda']:checked").val(),
        tipoinfocodi: $("#rbPotencia").is(':checked'),
        fechaInicio: $("#txtFechaInicio").val(),
        fechaFin: $("#txtFechaFin").val()
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarDemadaBarras',
        dataType: 'json',
        data: dataSend,
        cache: false,
        success: function (aData) {
            switch (aData.nRegistros) {
                case 1: window.location = controlador + "Exportar?fi=" + aData.Resultado; break;// si hay elementos
                case 2: alert("No existen registros !"); break;// sino hay elementos
                case -1: alert("Error en reporte result"); break;// Error en C#
            }
        },
        error: function (err) {
            mostrarError();
        }
    });
}


mostrarError = function () {
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
    $('#mensaje').css("display", "block");
};



function pintarBusqueda(nroPagina, fecha) {
    mostrarListado(nroPagina, fecha);
}

function mostrarListado(nroPagina, fecha) {
    $('#mensaje').css("display", "none");

    var dataSend = {
        emprcodi: getSmultipleSelect($("#cbEmpresa")).join(","),
        lectcodi: $("input[name='demanda']:checked").val(),
        tipoinfocodi: $("#rbPotencia").is(':checked'),
        fecha: fecha
    }

    $('#paginado').css("display", "block");
    $('#tab-container').css("display", "none");
    $('#hfNroPagina').val(nroPagina);

    $.ajax({
        type: 'POST',
        url: controlador + "ListaDemandaBarras",
        data: dataSend,
        success: function (evt) {
            updateContainer();
            $('#listado').html(evt.Resultado);
            $('#tblDemandaBarras').dataTable({
                "scrollX": true,
                "scrollY": "850px",
                "scrollCollapse": true,
                "sDom": 't',
                "ordering": false,
                paging: false,
                fixedColumns: {
                    leftColumns: 1
                }
            });
            $("#listado").css("overflow-x", "auto");
            $("#listado").css("overflow-y", "hidden");
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}


function pintarPaginado() {
    var dataSend = {
        emprcodi: getSmultipleSelect($("#cbEmpresa")).join(","),
        lectcodi: $("input[name='demanda']:checked").val(),
        tipoinfocodi: $("#rbPotencia").is(':checked'),
        fechaInicio: $("#txtFechaInicio").val(),
        fechaFin: $("#txtFechaFin").val()
    }

    $.ajax({
        type: 'POST',
        url: controlador + "PaginadoDemandaBarras",
        data: dataSend,
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}


