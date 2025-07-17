var controlador = siteRoot + 'StockCombustibles/';
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

    $('#FechaDesde').Zebra_DatePicker({

    });

    $('#FechaHasta').Zebra_DatePicker({

    });

    $('#btnBuscar').click(function () {
        buscarDatos();
    });
    $('#btnExportar').click(function () {
        exportarExcel();
    });

    $('#cbEmpresa').multipleSelect('checkAll');

    $('#cbEstado').multipleSelect('checkAll');

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
            fechaIni: $('#FechaDesde').val(),
            fechaFin: $('#FechaHasta').val(),
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
            fechaIni: $('#FechaDesde').val(),
            fechaFin: $('#FechaHasta').val(),
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
            fechaIni: $('#FechaDesde').val(),
            fechaFin: $('#FechaHasta').val(),
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

function descargarEnvio(idEnvio, idFormato) {
    var action = "";
    switch (idFormato) {
        case 57:
            action = 'generarformatoconsumo';
            break;
        case 58:
            action = 'generarformatopresion';
            break;
        case 59:
            action = 'generarformatodisponibilidad';
            break;
        case 60:
            action = 'generarformatoquema';
            break;

    }

    $.ajax({
        type: 'POST',
        url: controlador + 'Envio/' + action,
        data: {
            idEnvio: idEnvio
        },
        dataType: 'json',
        success: function (result) {

            if (result == 1)
                window.location = controlador + 'Envio/descargarformato';
            else
                alert("Error en exportar envío... id:" + result);
        },
        error: function (result) {
            alert('ha ocurrido un error al descargar el archivo excel. ' + result.status + ' - ' + result.statusText + '.');
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
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}