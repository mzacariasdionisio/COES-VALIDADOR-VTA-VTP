var controlador = siteRoot + 'Monitoreo/Indicadores/';
var ancho = 900;
var MSJ_NO_DATA = 'No existen registros';

$(function () {
    $(".btn_excel_by_imm").hide();

    $('#tab-container').easytabs({
        animate: false
    });

    ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;

    $('#txtMesIMM-1').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            pintarPaginado(1);
            pintarBusqueda(1);
        }
    });

    $('#txtMesIMM-2').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            pintarPaginado(2);
            pintarBusqueda2(1);
        }

    });

    $('#txtMesIMM-3').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            pintarPaginado(3);
            pintarBusqueda3(1);
        }
    });

    $('#txtMesIMM-4').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            pintarPaginado(4);
            pintarBusqueda4(1);
        }
    });

    $('#txtMesIMM-5').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            pintarPaginado(5);
            pintarBusqueda5(1);
        }
    });


    $('#txtMesIMM-6').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            pintarPaginado(6);
            pintarBusqueda6(1);
        }
    });

    $('#txtMesIMM-7').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            pintarPaginado(7);
            pintarBusqueda7(1);
        }
    });

    $('#cbEmpresaIMM-1').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function (view) {
            pintarPaginado(1);
            pintarBusqueda(1);
        }
    });

    $('#cbEmpresaIMM-2').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function (view) {
            pintarPaginado(2);
            pintarBusqueda2(1);
        }
    });


    $('#cbEmpresaIMM-3').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function (view) {
            pintarPaginado(3);
            pintarBusqueda3(1);
        }
    });

    $('#cbEmpresaIMM-4').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function (view) {
            pintarPaginado(4);
            pintarBusqueda4(1);
        }
    });

    $('#cbEmpresaIMM-5').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function (view) {
            pintarPaginado(5);
            pintarBusqueda5(1);
        }
    });

    $('#cbEmpresaIMM-6').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function (view) {
            pintarPaginado(6);
            pintarBusqueda6(1);
        }
    });

    $('#cbEmpresaIMM-7').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function (view) {
            pintarPaginado(7);
            pintarBusqueda7(1);
        }
    });

    $('#cbEmpresaIMM-1').multipleSelect('checkAll');
    $('#cbEmpresaIMM-2').multipleSelect('checkAll');
    $('#cbEmpresaIMM-3').multipleSelect('checkAll');
    $('#cbEmpresaIMM-4').multipleSelect('checkAll');
    $('#cbEmpresaIMM-5').multipleSelect('checkAll');
    $('#cbEmpresaIMM-6').multipleSelect('checkAll');
    $('#cbEmpresaIMM-7').multipleSelect('checkAll');

    cargarVersionPrimeraPagina();

    $('#btnExportExcel1').click(function () {
        exportarExcelReporte(1);
    });

    $('#btnExportExcel2').click(function () {
        exportarExcelReporte(2);
    });
    $('#btnExportExcel3').click(function () {
        exportarExcelReporte(3);
    });
    $('#btnExportExcel4').click(function () {
        exportarExcelReporte(4);
    });
    $('#btnExportExcel5').click(function () {
        exportarExcelReporte(5);
    });
    $('#btnExportExcel6').click(function () {
        exportarExcelReporte(6);
    });
    $('#btnExportExcel7').click(function () {
        exportarExcelReporte(7);
    });
});

function cargarVersionPrimeraPagina() {
    //Paginado de los reportes
    pintarPaginado(1);
    pintarPaginado(2);
    pintarPaginado(3);
    pintarPaginado(4);
    pintarPaginado(5);
    pintarPaginado(6);
    pintarPaginado(7);

    //Reporte
    var id = $("#idVersion").val();
    var empresa = "-1";

    $.ajax({
        type: 'POST',
        async: true,
        url: controlador + 'CargarIndicadorByTipo',
        data: {
            tipoIndicador: -1,
            idEmpresa: -1,
            nroPagina: 1,
            id: id
        },
        success: function (aData) {
            if (aData != null && aData.Resultado == "1") {
                mostrarDataByIndicador(aData.ListaResultado[0], 1, 2, 0);
                mostrarDataByIndicador(aData.ListaResultado[1], 2, 1, 1);
                mostrarDataByIndicador(aData.ListaResultado[2], 3, 3, 0);
                mostrarDataByIndicador(aData.ListaResultado[3], 4, 3, 0);
                mostrarDataByIndicador(aData.ListaResultado[4], 5, 1, 0);
                mostrarDataByIndicador(aData.ListaResultado[5], 6, 1, 0);
                mostrarDataByIndicador(aData.ListaResultado[6], 7, 1, 0);
            } else {
                alert("Ha ocurrido un error: " + aData.Resultado);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarDataByIndicador(resultado, numero, colFijasIzq, colFijasDer) {
    if (resultado == null || resultado == '') {
        $('#listado' + numero).html(MSJ_NO_DATA);
    } else {
        $('#listado' + numero).html(resultado);
        var anchoReporte = $('#reporte' + numero).width();
        $("#resultado" + numero).css("width", (anchoReporte > ancho ? ancho : anchoReporte) + "px");
        $('#reporte' + numero).dataTable({
            "scrollX": true,
            "scrollY": "780px",
            "scrollCollapse": true,
            "sDom": 't',
            "ordering": false,
            paging: false,
            fixedColumns: {
                leftColumns: colFijasIzq,
                rightColumns: colFijasDer
            }
        });
    }
}

function cargarListaByIndicador(pagina, indicador, colFijasIzq, colFijasDer) {
    var id = $("#idVersion").val();
    var empresa = $('#cbEmpresaIMM-' + indicador).multipleSelect('getSelects');
    if (empresa == "[object Object]" || empresa.length == 0) empresa = "-1";

    if (empresa != "") {
        $.ajax({
            type: 'POST',
            async: true,
            url: controlador + 'CargarIndicadorByTipo',
            data: {
                tipoIndicador: indicador,
                idEmpresa: empresa.join(','),
                nroPagina: pagina,
                id: id
            },
            success: function (aData) {
                mostrarDataByIndicador(aData.ListaResultado[0], indicador, colFijasIzq, colFijasDer);
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    } else {
        alert("Debe seleccionar al menos una empresa");
        $('#cbEmpresaIMM-' + indicador).multipleSelect('checkAll');
    }
}

function pintarBusqueda(nPagina1) {
    cargarListaByIndicador(nPagina1, 1, 2, 0);
}

function pintarBusqueda2(nPagina2) {
    cargarListaByIndicador(nPagina2, 2, 1, 1);
}

function pintarBusqueda3(nPagina3) {
    cargarListaByIndicador(nPagina3, 3, 3, 0);
}

function pintarBusqueda4(nPagina3) {
    cargarListaByIndicador(nPagina3, 4, 3, 0);
}

function pintarBusqueda5(nPagina5) {
    cargarListaByIndicador(nPagina5, 5, 1, 0);
}

function pintarBusqueda6(nPagina6) {
    cargarListaByIndicador(nPagina6, 6, 1, 0);
}

function pintarBusqueda7(nPagina7) {
    cargarListaByIndicador(nPagina7, 7, 1, 0);
}

function pintarPaginado(indicador) {
    var numIndicador = indicador == 1 ? "" : +indicador;
    $("#paginado" + numIndicador).html('');
    var empresa = $('#cbEmpresaIMM-' + indicador).multipleSelect('getSelects');
    var fecha = $('#txtMesIMM-' + indicador).val();
    $.ajax({
        type: 'POST',
        url: controlador + "/PaginadoIndicador" + indicador,
        data: {
            idEmpresa: empresa.join(','),
            fechaInicial: fecha
        },
        success: function (evt) {
            $('#paginado' + indicador).html(evt);
            eval("mostrarPaginado" + numIndicador)();
        },
        error: function (err) {
        }
    });
}

function exportarExcelReporte(tipo) {
    var empresa = $('#cbEmpresaIMM-' + tipo).multipleSelect('getSelects');
    var id = $("#idVersion").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarReporteExcelByTipo',
        data: {
            tipoIndicador: tipo,
            idEmpresa: empresa.join(','),
            id: id
        },
        dataType: 'json',
        success: function (result) {
            switch (result.Total) {
                case 1: window.location = controlador + "ExportarReporteXls?nameFile=" + result.Resultado; break;// si hay elementos
                case 2: alert("No existen registros !"); break;// sino hay elementos
                case -1: alert(result.Mensaje); break;// Error en C#
            }
        },
        error: function (err) {
            alert("Error en reporte");;
        }
    });
}

function descargarExcelVersion(nombreArchivo) {
    window.location = controlador + "ExportarReporteIndicadores?nombreArchivo=" + nombreArchivo;
}

function regresarMesPeriodo(periodo) {
    document.location.href = controlador + 'Index?periodo=' + periodo;
}