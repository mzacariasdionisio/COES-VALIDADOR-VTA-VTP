var controlador = siteRoot + 'hidrologia/';

$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '222px',
        filter: true
    });
    $('#Fecha').Zebra_DatePicker({

    });
    $('#FechaHasta').Zebra_DatePicker({

    });
    $('#Anho').Zebra_DatePicker({
        format: 'Y',
        onSelect: function () {
            cargarSemanaAnho()
        }
    });

    $('#btnBuscar').click(function () {
        $('#listado').html("");
        $('#btnExpotar').show();
        buscarDatos();
    });

    $('#btnExpotar').click(function () {
        exportarExcelTR();

    });
    cargarPrevio();
    //buscarDatos();

});

function cargarPrevio() {
    $('#cbEmpresa').multipleSelect('checkAll');
    $('input[name=rbidTipo][value=1]').attr('checked', true);

}

function buscarDatos() {
    pintarPaginado(1);
    mostrarListado(1);
}

function pintarPaginado(id) {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";

    var valor = $("input[name='rbidTipo']:checked").val();
    $('#hfidTipo').val(valor.toString());
    $('#hfEmpresa').val(empresa);
    $.ajax({
        type: 'POST',
        url: controlador + "reporte/PaginadoDescargaVert",
        data: {
            idsEmpresa: $('#hfEmpresa').val(),
            tipoReporte: $('#hfidTipo').val(),
            fecha: $('#Fecha').val(),
            fechaFinal: $('#FechaHasta').val()
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado(id);
        },
        error: function () {
            alert("Ha ocurrido un error paginado");
        }
    });
}

function pintarBusqueda(nroPagina, itipo) {
    //itipo : tipo de reporte a mostrar 0: reporte grafico, 1: reporte listado

    if (itipo == 0) {
        generarGraficoTiempoReal(nroPagina);
    }
    else {
        mostrarListado(nroPagina);
    }
}

function mostrarListado(nroPagina) {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";

    var valor = $("input[name='rbidTipo']:checked").val();
    $('#hfidTipo').val(valor.toString());
    $('#hfEmpresa').val(empresa);
    $.ajax({
        type: 'POST',
        url: controlador + "Reporte/listaDescargaVertimiento",
        data: {
            idsEmpresa: $('#hfEmpresa').val(),
            tipoReporte: $('#hfidTipo').val(),
            fecha: $('#Fecha').val(),
            fechaFinal: $('#FechaHasta').val(),
            idsTipoPtoMed: $('#hfUnidad').val(),
            nroPagina: nroPagina
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);

            var numPag = $("#hfNroPaginasActual").val();

            if (numPag > 0) {
                $('#tabla').dataTable({
                    // "aoColumns": aoColumns(),
                    "bAutoWidth": false,
                    "bSort": false,
                    "scrollY": 430,
                    "scrollX": true,
                    "sDom": 't',
                    "iDisplayLength": 50
                });
            }
        },
        error: function () {
            alert("Ha ocurrido un error en lista");
        }
    });
}

function exportarExcelTR() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    var valor = $("input[name='rbidTipo']:checked").val();
    $('#hfidTipo').val(valor.toString());
    $('#hfEmpresa').val(empresa);

    $.ajax({
        type: 'POST',
        url: controlador + 'reporte/GenerarArchivoReporteDescargaVert',//'reporte/GenerarArchivoReporteTR',
        data: {
            idsEmpresa: $('#hfEmpresa').val(),
            tipoReporte: $('#hfidTipo').val(),
            fecha: $('#Fecha').val(),
            fechaFinal: $('#FechaHasta').val(),
            idsTipoPtoMed: $('#hfUnidad').val()
        },
        dataType: 'json',
        success: function (result) {

            if (result == 1) {
                window.location = controlador + "reporte/ExportarReporte?tipoReporte=9";
            }

        },
        error: function () {
            mostrarError();
        }
    });
}

function activaBtn() {
    $('#btnGrafico').show();
    $('#btnExpotar').show();
}

function desactivaBtn() {
    $('#btnGrafico').hide();
    $('#btnExpotar').hide();
}

function handleClick(myRadio) {
    currentValue = myRadio.value;
    if (currentValue == 2)
        $('#btnGrafico').show();
    else
        $('#btnGrafico').show();

    //desactivaBtn();
}
