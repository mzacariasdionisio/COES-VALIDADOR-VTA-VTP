var controlador = siteRoot + 'hidrologia/';
var listFormatCodi = [];
var listFormatPeriodo = [];
$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '222px',
        filter: true
    });
    $('#cbLectura').multipleSelect({
        width: '222px',
        filter: true,
        onClose: function (view) {
            listarFormato();
        }
    });
    $('#cbEstado').multipleSelect({
        width: '222px',
        filter: true
    });
    $('#cbLectura').click(function () {
        //listarFormato();
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

    cargarPrevio();
    buscarDatos();
});

function cargarPrevio() {
    var strFormatCodi = $('#hfFormatCodi').val();
    var strFormatPeriodo = $('#hfFormatPeriodo').val();
    listFormatCodi = strFormatCodi.split(',');
    listFormatPeriodo = strFormatPeriodo.split(',');

    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbLectura').multipleSelect('checkAll');
    $('#cbEstado').multipleSelect('checkAll');
    listarFormato();
 
}

function buscarDatos() {
    pintarPaginado(1)  
    mostrarListado(1);
}

function pintarPaginado(id) {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var formato = $('#cbFormato').multipleSelect('getSelects');
    var lectura = $('#cbLectura').multipleSelect('getSelects');
    var estado = $('#cbEstado').multipleSelect('getSelects');

    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    if (formato == "[object Object]") formato = "-1";
    if (formato == "") formato = "-1";
    if (lectura == "[object Object]") lectura = "-1";
    if (lectura == "") lectura = "-1";
    if (estado == "[object Object]") estado = "-1";
    if (estado == "") estado = "-1";
    $('#hfEmpresa').val(empresa);
    $('#hfFormato').val(formato);
    $('#hfLectura').val(lectura);
    $('#hfEstado').val(estado);

    $.ajax({
        type: 'POST',
        url: controlador + "Envio/paginado",
        data: {
            idsEmpresa: $('#hfEmpresa').val(),
            fechaIni: $('#FechaDesde').val(),
            fechaFin: $('#FechaHasta').val(),
            idsFormato: $('#hfFormato').val(),
            idsLectura: $('#hfLectura').val(),
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

function listarFormato() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var lectura = $('#cbLectura').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    if (lectura == "[object Object]") lectura = "-1";
    if (lectura == "") lectura = "-1";
    $('#hfEmpresa').val(empresa);
    $('#hfLectura').val(lectura);
    $.ajax({
        type: 'POST',
        url: controlador + "Envio/ListarFormatosXLectura",
        
        data: {
            sLectura: $('#hfLectura').val(), sEmpresa: $('#hfEmpresa').val()
        },
        success: function (evt) {
            $('#listTipoInformacion').html(evt);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });

}

function mostrarListado(nroPagina) {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var formato = $('#cbFormato').multipleSelect('getSelects');
    var lectura = $('#cbLectura').multipleSelect('getSelects');
    var estado = $('#cbEstado').multipleSelect('getSelects');

    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    if (formato == "[object Object]") formato = "-1";
    if (formato == "") formato = "-1";
    if (lectura == "[object Object]") lectura = "-1";
    if (lectura == "") lectura = "-1";
    if (estado == "[object Object]") estado = "-1";
    if (estado == "") estado = "-1";
    $('#hfEmpresa').val(empresa);
    $('#hfFormato').val(formato);
    $('#hfLectura').val(lectura);
    $('#hfEstado').val(estado);
    $.ajax({
        type: 'POST',
        url: controlador + "Envio/lista",
        data: {
            idsEmpresa: $('#hfEmpresa').val(), 
            fechaIni: $('#FechaDesde').val(),
            fechaFin: $('#FechaHasta').val(),
            idsFormato: $('#hfFormato').val(),
            idsLectura: $('#hfLectura').val(),
            idsEstado: $('#hfEstado').val(),
            nPaginas: nroPagina
            
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                bJQueryUI: true,
                "scrollY": 550,
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
    var formato = $('#cbFormato').multipleSelect('getSelects');
    var lectura = $('#cbLectura').multipleSelect('getSelects');
    var estado = $('#cbEstado').multipleSelect('getSelects');

    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    if (formato == "[object Object]") formato = "-1";
    if (formato == "") formato = "-1";
    if (lectura == "[object Object]") lectura = "-1";
    if (lectura == "") lectura = "-1";
    if (estado == "[object Object]") estado = "-1";
    if (estado == "") estado = "-1";
    $('#hfEmpresa').val(empresa);
    $('#hfFormato').val(formato);
    $('#hfLectura').val(lectura);
    $('#hfEstado').val(estado);

    $.ajax({
        type: 'POST',
        url: controlador + 'Envio/GenerarArchivoReporteXLS',
        data: {
            idsEmpresa: $('#hfEmpresa').val(),
            fechaIni: $('#FechaDesde').val(),
            fechaFin: $('#FechaHasta').val(),
            idsFormato: $('#hfFormato').val(),
            idsLectura: $('#hfLectura').val(),
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

function descargarEnvio(idEnvio)
{
    $.ajax({
        type: 'POST',
        url: controlador + 'Envio/GenerarArchivoEnvio',
        data: {
            idEnvio: idEnvio,
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1)
                window.location = controlador + "Envio/ExportarEnvio";
            if (result == -1)
                alert("Error en exportar envío... id:" + result);
        },
        error: function () {
            alert("Error en exoprtar envío...");
        }
    });
}