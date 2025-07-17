var controlador = siteRoot + 'Monitoreo/LogErrores/';
var ancho = 900;
var MSJ_NO_DATA = 'No existen registros';

$(function () {

    $('#tab-container').easytabs({
        animate: false
    });

    ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;

    $('#txtMesIMM-1').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            filtroEmpresas(1);
            FiltroComboVersion(1);

        }
    });

    $('#txtMesIMM-2').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            filtroEmpresas(2);
            FiltroComboVersion(2);
        }

    });

    $('#txtMesIMM-3').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            filtroEmpresas(3);
            FiltroComboVersion(3);
        }
    });

    $('#txtMesIMM-4').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            filtroEmpresas(4);
            FiltroComboVersion(4);
        }
    });

    $('#txtMesIMM-5').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            filtroEmpresas(5);
            FiltroComboVersion(5);
        }
    });


    $('#txtMesIMM-6').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            filtroEmpresas(6);
            FiltroComboVersion(6);
        }
    });

    $('#txtMesIMM-7').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            filtroEmpresas(7);
            FiltroComboVersion(7);
        }
    });

    $('#cbEmpresaIMM-1').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function (view) {
        }
    });

    $('#cbEmpresaIMM-2').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function (view) {
        }
    });


    $('#cbEmpresaIMM-3').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function (view) {
        }
    });

    $('#cbEmpresaIMM-4').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function (view) {
        }
    });

    $('#cbEmpresaIMM-5').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function (view) {
        }
    });

    $('#cbEmpresaIMM-6').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function (view) {
        }
    });

    $('#cbEmpresaIMM-7').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function (view) {
        }
    });

    $('#cbEmpresaIMM-1').multipleSelect('checkAll');
    $('#cbEmpresaIMM-2').multipleSelect('checkAll');
    $('#cbEmpresaIMM-3').multipleSelect('checkAll');
    $('#cbEmpresaIMM-4').multipleSelect('checkAll');
    $('#cbEmpresaIMM-5').multipleSelect('checkAll');
    $('#cbEmpresaIMM-6').multipleSelect('checkAll');
    $('#cbEmpresaIMM-7').multipleSelect('checkAll');

    filtroEmpresas(1);
    filtroEmpresas(2);
    filtroEmpresas(3);
    filtroEmpresas(4);
    filtroEmpresas(5);
    filtroEmpresas(6);
    filtroEmpresas(7);

    $('#btnProcesarIndicador1').click(function () {
        pintarPaginado(1);
        pintarBusqueda(1);
    }
    );

    $('#btnProcesarIndicador2').click(function () {
        pintarPaginado(2);
        pintarBusqueda2(1);
    }
);

    $('#btnProcesarIndicador3').click(function () {
        pintarPaginado(3);
        pintarBusqueda3(1);
    }
);

    $('#btnProcesarIndicador4').click(function () {
        pintarPaginado(4);
        pintarBusqueda4(1);
    }
);

    $('#btnProcesarIndicador5').click(function () {
        pintarPaginado(5);
        pintarBusqueda5(1);
    }
);

    $('#btnProcesarIndicador6').click(function () {
        pintarPaginado(6);
        pintarBusqueda6(1);
    }
);

    $('#btnProcesarIndicador7').click(function () {
        pintarPaginado(7);
        pintarBusqueda7(1);
    }
);

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

    var id = $("#cbVersionIndicador" + indicador).val();
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
                if (aData.Resultado == "-1") {
                    alert("No existe versión a consultar");
                    $('#listado' + indicador).html(MSJ_NO_DATA);
                }
                else {
                    mostrarDataByIndicador(aData.ListaResultado[0], indicador, colFijasIzq, colFijasDer);
                }
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

function FiltroComboVersion(indicador) {

    var fechaDia = $('#txtMesIMM-' + indicador).val();
    $.ajax({
        type: 'POST',
        url: controlador + 'ComboVersion',
        data: {
            fecha: fechaDia,
            indicador: indicador
        },
        success: function (aData) {
            $('#divVersionIndicador' + indicador).html(aData);
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}

function filtroEmpresas(indicador) {
    var fechaDia = $('#txtMesIMM-' + indicador).val();
    $.ajax({
        type: 'POST',
        url: controlador + 'ComboEmpresa',
        data: {
            fechaDia: fechaDia,
            indicador: indicador

        },
        success: function (aData) {
            $('#divEmpresaIndicador' + indicador).html(aData);
            $('#cbEmpresaIMM-' + indicador).multipleSelect({
                width: '150px',
                filter: true,
                onClose: function (view) {
                    if (indicador == 5) {
                        filtroBarrasIndicador(5);
                    }
                    if (indicador == 6) {
                        filtroBarrasIndicador(6);
                    }

                    if (indicador == 7) {
                        filtroBarrasIndicador(7);
                    }

                }
            });
            $('#cbEmpresaIMM-' + indicador).multipleSelect('checkAll');

            eval("FiltroComboVersion")(indicador);
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}
