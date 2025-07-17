var controlador = siteRoot + 'Migraciones/AnexoA/';

$(function () {
    $('#cbTipoAgente').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarAgentes();
        }
    });


    /*   $('#cbCentralInt').multipleSelect({
           width: '150px',
           filter: true
       });
       */
    $('#cbYacimiento').multipleSelect({
        width: '150px',
        filter: true,
        onClose:function() {
            mostrarListado();
        }
    });




    //$('#cbAgente').multipleSelect({
    //    width: '150px',
    //    filter: true,
    //    onClose: function () {
    //        mostrarListado();
    //    }
    //});

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
    $('#btnExpotar').click(function () {
        exportarExcelReporte();

    });
    cargarPrevio();
    cargarAgentes();
    
});

function cargarPrevio() {
    $('#cbTipoAgente').multipleSelect('checkAll');
    /*$('#cbCentralInt').multipleSelect('checkAll');*/
    $('#cbRecurso').multipleSelect('checkAll');
    $('#cbAgente').multipleSelect('checkAll');
    $('#cbEstado').multipleSelect('checkAll');

    $('#cbYacimiento').multipleSelect('checkAll');
}

function buscarDatos() {
    //pintarPaginado(1);
    mostrarListado();
}

function getValueCustomizeMultipleSelect(tag) {
    var value = tag.multipleSelect('getSelects');
    if (value == "[object Object]") value = -1;
    if (value == "") value = "0";
    return value;
}



function mostrarListado() {

    var tipoAgente = getValueCustomizeMultipleSelect($('#cbTipoAgente'));
    var agente = getValueCustomizeMultipleSelect($('#cbAgente'));
    var yacimiento = getValueCustomizeMultipleSelect($('#cbYacimiento'));


    $.ajax({
        type: 'POST',
        url: controlador + "listaDisponibilidadGas",
        data: {
            idsTipoAgente: tipoAgente.toString(),
            idsCentralInt: $('#hfCentralInt').val(),
            idsYacimientos: yacimiento.toString(),
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
        url: controlador + 'CargarAgentes',

        data: { idTipoAgente: $('#hfTipoAgente').val() },

        success: function (aData) {
            $('#agentes').html(aData);
            mostrarListado();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}
