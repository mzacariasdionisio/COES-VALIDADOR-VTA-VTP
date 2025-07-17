var controlador = siteRoot + 'IEOD/';
$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '222px',
        filter: true
    });

    $('#cbFormato').change(function () {
        cargarEmpresas();
    });

    $('#cbEstado').multipleSelect({
        width: '222px',
        filter: true
    });

    $('#fechaInicio').Zebra_DatePicker({
    });

    $('#fechaFin').Zebra_DatePicker({
    });

    $('#btnBuscar').click(function () {
        buscarDatos();
    });
    $('#btnExportar').click(function () {
        exportarExcel();
    });

    $('#cbEmpresa').multipleSelect('checkAll');

    $('#cbEstado').multipleSelect('checkAll');

    $('#cbEstado').change(function () {
        buscarDatos();
    });

    cargarEmpresas();
    buscarDatos();
});


function buscarDatos() {
    pintarPaginado(1)
    mostrarListado(1);
}

function pintarPaginado(id) {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var formato = $('#cbFormato').val();
    var estado = $('#cbEstado').multipleSelect('getSelects');

    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    if (estado == "[object Object]") estado = "-1";
    if (estado == "") estado = "-1";
    $('#hfEmpresa').val(empresa);
    $('#hfFormato').val(formato);
    $('#hfEstado').val(estado);

    $.ajax({
        type: 'POST',
        url: controlador + "Envio/paginado",
        data: {
            idsEmpresa: $('#hfEmpresa').val(),
            fInicio: $('#fechaInicio').val(),
            fFin: $('#fechaFin').val(),
            idsFormato: formato,
            idsEstado: $('#hfEstado').val()
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            alert("Ha ocurrido un error paginado");
        }
    });
}

function mostrarListado(nroPagina) {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var formato = $('#cbFormato').val();
    var estado = $('#cbEstado').multipleSelect('getSelects');

    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    if (estado == "[object Object]") estado = "-1";
    if (estado == "") estado = "-1";
    $('#hfEmpresa').val(empresa);

    $.ajax({
        type: 'POST',
        url: controlador + "Envio/lista",
        data: {
            idsEmpresa: $('#hfEmpresa').val(),
            fInicio: $('#fechaInicio').val(),
            fFin: $('#fechaFin').val(),
            idsFormato: formato,
            idsEstado: $('#hfEstado').val(),
            nPaginas: nroPagina

        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                bJQueryUI: true,
                "scrollY": 320,
                "scrollX": true,
                "sDom": 't',
                "ordering": true,
                "iDisplayLength": 50
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function pintarBusqueda(nroPagina) {
    mostrarListado(nroPagina);
}

function exportarExcel() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var formato = $('#cbFormato').val();
    var estado = $('#cbEstado').multipleSelect('getSelects');

    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    if (estado == "[object Object]") estado = "-1";
    if (estado == "") estado = "-1";
    $('#hfEmpresa').val(empresa);
    $('#hfFormato').val(formato);
    $('#hfEstado').val(estado);

    $.ajax({
        type: 'POST',
        url: controlador + 'Envio/GenerarArchivoReporteXLS',
        data: {
            idsEmpresa: $('#hfEmpresa').val(),
            fInicio: $('#fechaInicio').val(),
            fFin: $('#fechaFin').val(),
            idsFormato: formato,
            idsEstado: $('#hfEstado').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1)
                window.location = controlador + "Envio/ExportarReporte";
            if (result == -1)
                alert("Error en exportar reporte...");
        },
        error: function () {
            alert("Error en reporte...");
        }
    });
}

function cargarEmpresas() {
    var formato = $('#cbFormato').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'Envio/CargarEmpresas',

        data: { idFormato: formato },

        success: function (aData) {
            $('#empresas').html(aData);
            buscarDatos();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function descargarEnvio(idEnvio, idFormato) {
    var numHoja = '';

    $.ajax({
        type: 'POST',
        async: true,
        contentType: 'application/json',
        url: controlador + 'Envio/generarformato',
        data: JSON.stringify({
            idEnvio: idEnvio,
            idFormato: idFormato
        }),
        beforeSend: function () {
            mostrarExito(numHoja, "Descargando información ...");
        },
        success: function (result) {
            if (result.length > 0 && result != "-1") {
                mostrarExito(numHoja, "<strong>Los datos se descargaron correctamente</strong>");
                window.location.href = controlador + 'Envio/descargarformato?archivo=' + result;
            }
            else {
                alert("Error en descargar el archivo");
            }
        },
        error: function (result) {
            alert('ha ocurrido un error al descargar el archivo excel. ' + result.status + ' - ' + result.statusText + '.');
        }
    });
}