var controlador = siteRoot + 'hidrologia/'
$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '222px',
        filter: true
    });
    $('#cbRecurso').multipleSelect({
        width: '222px',
        filter: true
    });
    $('#cbCuenca').multipleSelect({
        width: '222px',
        filter: true
    });

    $('#btnBuscar').click(function () {
        buscarRecursosCuenca();
    });

    $('#btnExpotar').click(function () {
        exportarExcel();
    });
    cargarPrevio();

    $(window).resize(function () {
        $('#listado').css("width", $('#mainLayout').width() + "px");
    });
});

function cargarPrevio() {
    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbCuenca').multipleSelect('checkAll');
    $('#cbRecurso').multipleSelect('checkAll');
}

function exportarExcel() {
    var empresas = $('#cbEmpresa').multipleSelect('getSelects');
    var cuencas = $('#cbCuenca').multipleSelect('getSelects');
    var recursos = $('#cbRecurso').multipleSelect('getSelects');

    if (empresas == "[object Object]") empresas = "-1";
    if (empresas == "") empresas = "-1";
    if (cuencas == "[object Object]") cuencas = "-1";
    if (cuencas == "") cuencas = "-1";
    if (recursos == "[object Object]") recursos = "-1";
    if (recursos == "") recurso = "-1";
    $('#hfEmpresa').val(empresas);
    $('#hfCuenca').val(cuencas);
    $('#hfRecurso').val(recursos);

    $.ajax({
        type: 'POST',
        url: controlador + 'Topologia/GenerarReporteXLS',
        data: {
            sempresas: $('#hfEmpresa').val(),
            scuencas: $('#hfCuenca').val(),
            srecursos: $('#hfRecurso').val(),
        },
        dataType: 'json',
        success: function (result) {
            window.location = controlador + "Topologia/ExportarReporte";
            if (result == -1) {
                alert("Error en reporte result")
            }
        },
        error: function () {
            alert("Error en reporte");
        }
    });
}

function buscarRecursosCuenca() {
    //pintarPaginado();
    mostrarListado(1);
}

function mostrarListado() {
    var empresas = $('#cbEmpresa').multipleSelect('getSelects');
    var cuencas = $('#cbCuenca').multipleSelect('getSelects');
    var recursos = $('#cbRecurso').multipleSelect('getSelects');

    if (empresas == "[object Object]") empresas = "-1";
    if (empresas == "") empresas = "-1";
    if (cuencas == "[object Object]") cuencas = "-1";
    if (cuencas == "") cuencas = "-1";
    if (recursos == "[object Object]") recursos = "-1";
    if (recursos == "") recurso = "-1";
    $('#hfEmpresa').val(empresas);
    $('#hfCuenca').val(cuencas);
    $('#hfRecurso').val(recursos);
    $.ajax({
        type: 'POST',
        url: controlador + "Topologia/lista",
        data: {
            sempresas: $('#hfEmpresa').val(),
            scuencas: $('#hfCuenca').val(),
            srecursos: $('#hfRecurso').val(),
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 3000
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}
