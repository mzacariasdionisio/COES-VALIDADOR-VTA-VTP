var controlador = siteRoot + 'Monitoreo/ControlCambios/';
var MSJ_NO_DATA = 'No existen registros';

var ancho = 900;
$(function () {

    $('#tab-container').easytabs({
        animate: false
    });


    ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;

    $('#txtMesIMM-1').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            filtroEmpresas(1);
            filtroComboVersion(1);
        }
    });

    $('#txtMesIMM-2').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            filtroEmpresas(2);
            filtroComboVersion(2);
        }

    });

    $('#txtMesIMM-3').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            filtroEmpresas(3);
            filtroComboVersion(3);
        }
    });

    $('#txtMesIMM-4').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            filtroEmpresas(4);
            filtroComboVersion(4);
        }
    });

    $('#txtMesIMM-5').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            filtroEmpresas(5);
            filtroComboVersion(5);
        }
    });


    $('#txtMesIMM-6').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            filtroEmpresas(6);
            filtroComboVersion(6);
        }
    });

    $('#txtMesIMM-7').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            filtroEmpresas(7);
            filtroComboVersion(7);
        }
    });



    filtroEmpresas(1);
    filtroEmpresas(2);
    filtroEmpresas(3);
    filtroEmpresas(4);
    filtroEmpresas(5);
    filtroEmpresas(6);
    filtroEmpresas(7);




    filtroComboVersion(1);
    filtroComboVersion(2);
    filtroComboVersion(3);
    filtroComboVersion(4);
    filtroComboVersion(5);
    filtroComboVersion(6);
    filtroComboVersion(7);



    $('#btnProcesarIndicador1').click(function () {
        cargarListaByIndicador(1, 2, 0);
    }
    );

    $('#btnProcesarIndicador2').click(function () {
        cargarListaByIndicador(2, 1, 1);
    }
);

    $('#btnProcesarIndicador3').click(function () {
        cargarListaByIndicador(3, 3, 0);
    }
);

    $('#btnProcesarIndicador4').click(function () {
        cargarListaByIndicador(4, 3, 0);
    }
);

    $('#btnProcesarIndicador5').click(function () {
        cargarListaByIndicador(5, 1, 0);
    }
);

    $('#btnProcesarIndicador6').click(function () {
        cargarListaByIndicador(6, 1, 0);
    }
);

    $('#btnProcesarIndicador7').click(function () {
        cargarListaByIndicador(7, 1, 0);
    }
);


    filtroBarrasIndicadorInicio(5);
    filtroBarrasIndicadorInicio(6);
    filtroBarrasIndicadorInicio(7);





});

function mostrarDataByIndicador(resultado, numero, colFijasIzq, colFijasDer) {
    $('#listado' + numero).html(resultado);
    var anchoReporte = $('#reporte' + numero).width();
    $("#resultado" + numero).css("width", (anchoReporte > ancho ? ancho : anchoReporte) + "px");
    $('#reporte' + numero).dataTable({
        "scrollX": true,
        "scrollY": "780px",
        "scrollCollapse": true,
        "sDom": 't',
        "ordering": false,
        paging: false
    });
}

function cargarListaByIndicador(indicador, colFijasIzq, colFijasDer) {
    var id = $("#cbVersionIndicador" + indicador).val();
    var empresa = $('#cbEmpresaIMM-' + indicador).multipleSelect('getSelects');
    if (empresa == "[object Object]" || empresa.length == 0) empresa = "-1";
    var barra = $('#cbBarraIndicador' + indicador).multipleSelect('getSelects');
    if (barra == "[object Object]" || barra.length == 0) barra = [-1];
    
    if (empresa != "") {
        $.ajax({
            type: 'POST',
            async: true,
            url: controlador + 'CargarIndicadorByTipo',
            data: {
                tipoIndicador: indicador,
                idEmpresa: empresa.join(','),
                id: id,
                barra: barra.join(',')
            },
            success: function (aData) {
                if (aData.Resultado == 1) {
                    mostrarDataByIndicador(aData.ListaResultado[0], indicador, colFijasIzq, colFijasDer);
                }
                else {
                    alert("No existe existe versión a consultar");
                    $('#listado' + indicador).html(MSJ_NO_DATA);
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

function filtroComboVersion(indicador) {

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

function filtroBarrasIndicadorInicio(indicador) {
    var empresa = $('#cbEmpresaIMM-' + indicador).multipleSelect('getSelects');
    var fechaDia = $('#txtMesIMM-' + indicador).val();
    if (empresa == "[object Object]" || empresa.length == 0) empresa = "-1";

    $.ajax({
        type: 'POST',
        url: controlador + 'ComboBarra',
        data: {
            empresa: -1,
            fechaDia: fechaDia,
            indicador: indicador
        },
        success: function (aData) {
            $('#divBarraIndicador' + indicador).html(aData);
            $('#cbBarraIndicador' + indicador).multipleSelect({
                width: '150px',
                filter: true,
                onClose: function (view) {
                }
            });
            $('#cbBarraIndicador' + indicador).multipleSelect('checkAll');
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}

function filtroBarrasIndicador(indicador) {
    var empresa = $('#cbEmpresaIMM-' + indicador).multipleSelect('getSelects');
    var fechaDia = $('#txtMesIMM-' + indicador).val();
    if (empresa == "[object Object]" || empresa.length == 0) empresa = "-1";

    $.ajax({
        type: 'POST',
        url: controlador + 'ComboBarra',
        data: {
            empresa: empresa.join(','),
            fechaDia: fechaDia,
            indicador: indicador
        },
        success: function (aData) {
            $('#divBarraIndicador' + indicador).html(aData);
            $('#cbBarraIndicador' + indicador).multipleSelect({
                width: '150px',
                filter: true,
                onClose: function (view) {
                }
            });
            $('#cbBarraIndicador' + indicador).multipleSelect('checkAll');
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
      

        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}