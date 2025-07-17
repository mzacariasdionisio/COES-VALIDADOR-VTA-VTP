var controlador = siteRoot + 'Migraciones/AnexoA/';

$(function () {
    $('#cbTipoAgente').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarAgentes();
        }
    });


    $('#cbCentralInt').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            mostrarListado();
        }
    });


    $('#cbAgente').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            mostrarListado();
        }
    });


    $('#btnBuscar').click(function () {
        $('#listado').html("");
        $('#paginado').html("");
        $('#btnExpotar').show();
        buscarDatos();
    });
    $('#btnGrafico').click(function () {
        //$('#paginado').html("");
        //pintarPaginado(0);

        generarGrafico()
        //mostrarPaginado();
        //$('#btnExpotar').hide();

    });
    $('#btnExportExcel').click(function () {
        exportarExcelReporte();

    });

    cargarPrevio();
    cargarAgentes();

});

function cargarPrevio() {
    $('#cbTipoAgente').multipleSelect('checkAll');
    $('#cbCentralInt').multipleSelect('checkAll');
    $('#cbRecurso').multipleSelect('checkAll');
    $('#cbAgente').multipleSelect('checkAll');
    $('#cbEstado').multipleSelect('checkAll');
}



function getValueCustomizeMultipleSelect(tag) {
    var value = tag.multipleSelect('getSelects');
    if (value == "[object Object]") value = -1;
    if (value == "") value = "0";
    return value;
}

function mostrarListado() {


    var tipoAgente = getValueCustomizeMultipleSelect($('#cbTipoAgente'));
    var centralInt = getValueCustomizeMultipleSelect($('#cbCentralInt'));
    var agente = getValueCustomizeMultipleSelect($('#cbAgente'));

    $.ajax({
        type: 'POST',
        url: controlador + "listaQuemaGas",
        data: {
            idsTipoAgente: tipoAgente.toString(),
            idsCentralInt: centralInt.toString(),
            idsAgente: agente.toString()
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);

            $('#tabla').dataTable({
                // "aoColumns": aoColumns(),                
                "bAutoWidth": false,
                "bSort": false,
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": true,
                "iDisplayLength": 50
            });

        },
        error: function () {
            alert("Ha ocurrido un error en lista");
        }
    });
}


function parseJsonDate(jsonDateString) {
    return new Date(parseInt(jsonDateString.replace('/Date(', '')));
}

mostrarPaginado = function () {

    $.ajax({
        type: 'POST',
        url: controlador + 'Paginado',
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            alert("Error paginado");
        }
    });
}


function cargarAgentes() {
    var tipoAgente = $('#cbTipoAgente').multipleSelect('getSelects');

    if (tipoAgente == "[object Object]") tipoAgente = "-1";
    $('#hfTipoAgente').val(tipoAgente);
    $.ajax({
        type: 'POST',
        async:false,
        url: controlador + 'CargarAgentes',

        data: { idTipoAgente: tipoAgente.toString() },

        success: function (aData) {
            $('#agentes').html(aData);
            
            setTimeout(function () { mostrarListado(); }, 10);
        },
        error: function () {
            alert("Ha ocurrido un error");
            return;
        }
    });

    
}



function exportarExcelReporte() {

    var tipoAgente = getValueCustomizeMultipleSelect($('#cbTipoAgente'));
    var centralInt = getValueCustomizeMultipleSelect($('#cbCentralInt'));
    var agente = getValueCustomizeMultipleSelect($('#cbAgente'));


    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarReporteXLSQuema',
        data: {
            idsTipoAgente: tipoAgente.toString(),
            idsCentralInt: centralInt.toString(),
            idsAgente: agente.toString()
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {//
                window.location = controlador + "ExportarReporte?tipo=8";
            }
            if (result == -1) {
                alert("Error en reporte result")
            }
            if (result == 2) {// Si no existen registros
                alert("No existen registros !");
            }
        },
        error: function () {
            alert("Error en reporte");;
        }
    });
}

