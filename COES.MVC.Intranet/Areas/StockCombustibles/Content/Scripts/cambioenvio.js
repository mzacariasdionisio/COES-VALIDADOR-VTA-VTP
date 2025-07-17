var controlador = siteRoot + 'hidrologia/';
var listFormatCodi = [];
var listFormatPeriodo = [];
$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '222px',
        filter: true
    });
    $('#cbEmpresa').multipleSelect('checkAll');

    $('#cbFormato').change(function () {
        horizonte();
    });

    $('#txtFecha').Zebra_DatePicker({

    });   
    $('#txtMes').Zebra_DatePicker({
        format: 'm Y'
    });
    $('#Anho').Zebra_DatePicker({
        format: 'Y',
        onSelect: function () {
            cargarSemanaAnho()
        }
    });
    $('#cbLectura').change(function () {
        listarFormato();
    });

    $('#btnBuscar').click(function () {
        if ($('#cbFormato').val() > 0) {
            buscarDatos();
        }
        else {
            alert("Error: Seleccionar Formato");
        }
    });
    cargarPrevio();
});

function cargarPrevio() {
    var strFormatCodi = $('#hfFormatCodi').val();
    var strFormatPeriodo = $('#hfFormatPeriodo').val();
    listFormatCodi = strFormatCodi.split(',');
    listFormatPeriodo = strFormatPeriodo.split(',');
    listarFormato(-1);
    $('#Anho').val($('#hfAnho').val());
    $('#cbSemana').val($('#hfSemana').val());
    cargarSemanaAnho();
}

function buscarDatos() {
    //pintarPaginado(1)  
    mostrarListado(1);
}

function pintarPaginado(id) {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var cuenca = $('#cbCuenca').multipleSelect('getSelects');

    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    if (cuenca == "[object Object]") cuenca = "-1";
    if (cuenca == "") cuenca = "-1";

    $('#hfEmpresa').val(empresa);
    $('#hfCuenca').val(cuenca);

    var tipoInformacion = $('#cbTipoInformacion').val();

    $.ajax({
        type: 'POST',
        url: controlador + "reporte/paginado",
        data: {
            idsEmpresa: $('#hfEmpresa').val(), idsCuenca: $('#hfCuenca').val(), idTipoInformacion: tipoInformacion,
            fechaInicial: $('#FechaDesde').val(), fechaFinal: $('#FechaHasta').val()
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

function mostrarListado(nroPagina) {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    semana = $("#cbSemana").val();
    anho = $("#Anho").val();
    semana = anho.toString() + semana;

    $.ajax({
        type: 'POST',
        url: controlador + "CambioEnvio/lista",
        data: {
            idsEmpresa: $('#hfEmpresa').val(),
            idFormato: $('#cbFormato').val(),
            fecha: $('#txtFecha').val(),
            mes: $('#txtMes').val(),
            semana: semana
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            if ($('#tabla th').length > 1) {
                $('#tabla').dataTable({

                    scrollY: 600,
                    scrollX: true,
                    scrollCollapse: true,
                    paging: false,
                    fixedColumns: {
                        leftColumns: 5,
                        rightColumns: 0
                    }
                });
            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function listarFormato() {
    $.ajax({
        type: 'POST',
        url: controlador + "Cambioenvio/ListarFormatosXLectura",
        dataType: 'json',
        cache: false,
        data: {
            sLectura: $('#cbLectura').val()
        },
        success: function (aData) {
            $('#cbFormato').get(0).options.length = 0;
            $('#cbFormato').get(0).options[0] = new Option("--SELECCIONE--", "");
            $.each(aData, function (i, item) {
                $('#cbFormato').get(0).options[$('#cbFormato').get(0).options.length] = new Option(item.Text, item.Value);
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });

}

function horizonte() {
    
    var opcion = buscarPeriodo($('#cbFormato').val());

    switch (parseInt(opcion)) {
        case 1: //dia
            $('#cntFecha').css("display", "block");
            $('#cntSemana').css("display", "none");
            $('#fechasSemana').css("display", "none");
            $('#cntMes').css("display", "none");
            break;
        case 2: //semanal
            $('#cntFecha').css("display", "none");
            $('#cntSemana').css("display", "block");
            $('#fechasSemana').css("display", "block");
            $('#cntMes').css("display", "none");
            break;
            //mensual

            //break;
        case 3: case 5:
            $('#cntFecha').css("display", "none");
            $('#cntSemana').css("display", "none");
            $('#fechasSemana').css("display", "none");
            $('#cntMes').css("display", "block");
            break;
    }
}

function buscarPeriodo(valor) {// valor: formatCodi

    for (var i = 0 ; i < listFormatCodi.length; i++)
        if (listFormatCodi[i] == valor) return listFormatPeriodo[i];

}

function cargarSemanaAnho() {
    var anho = $('#Anho').val();
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: controlador + 'Cambioenvio/CargarSemanas',
        cache: false,
        data: {
            idAnho: anho
        },
        success: function (aData) {
            $('#cbSemana').get(0).options.length = 0;
            $.each(aData, function (i, item) {
                $('#cbSemana').get(0).options[$('#cbSemana').get(0).options.length] = new Option(item.Text, item.Value);
            });
            $('#cbSemana').val($('#hfSemana').val());
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });

}