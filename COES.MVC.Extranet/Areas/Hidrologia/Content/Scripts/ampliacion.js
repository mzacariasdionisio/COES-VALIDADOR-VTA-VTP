var controlador = siteRoot + 'Hidrologia/ampliacion/';

var listFormatCodi = [];
var listFormatPeriodo = [];

$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '200px',
        filter: true
    });
    $('#cbLectura').multipleSelect({
        width: '222px',
        filter: true,
        onClose: function (view) {
            listarFormato();
        }
    });

    $('#idFecha').Zebra_DatePicker({
    });
    $('#btnBuscar').click(function () {
        mostrarListado();
    });
    $('#btnAmpliar').click(function () {
        agregarAmpliacion();
    });
    //$("#cbEmpresa").val($("#hfEmpresa").val());

    //$('#cbEmpresa').change(function () {
    //    listarFormato($('#cbLectura').val());
    //});


 
    valoresIniciales();
    mostrarListado();



});

function valoresIniciales() {
    var strFormatCodi = $('#hfFormatCodi').val();
    var strFormatPeriodo = $('#hfFormatPeriodo').val();
    listFormatCodi = strFormatCodi.split(',');
    listFormatPeriodo = strFormatPeriodo.split(',');
    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbLectura').multipleSelect('checkAll');

    listarFormato();
    //$('#cbTipoInformacion').val(0);
}

function mostrarListado() {

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var formato = $('#cbFormato').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    if (formato == "[object Object]") formato = "-1";
    if (formato == "") formato = "-1";
    $('#hfEmpresa').val(empresa);
    $('#hfFormato').val(formato);

    $.ajax({
        type: 'POST',
        url: controlador + "lista",
        data: {
            sEmpresa: $('#hfEmpresa').val(), fecha: $('#idFecha').val(), sFormato: $('#hfFormato').val()
        },

        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
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

function agregarAmpliacion() {
    $.ajax({
        type: 'POST',
        url: controlador + "AgregarAmpliacion",
        data: {
             idModulo: $('#hfIdModulo').val()
        },
        success: function (evt) {

            $('#detalleAmpliacion').html(evt);

            setTimeout(function () {
                $('#validaciones').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false
                });
            }, 50);
        },
        error: function () {
            alert("Error en mostrar periodo");
        }
    });
}

function grabarAmpliacion() {
    var error = false;
    var hora = $('#cbHora').val();
    var formato = $('#listTipoInformacion2 select').val();
    if (formato == 0) {
        alert("Ingresar Formato");
    }
    else {
        var sem = $('#cbSemana2').val();

        $('#validaciones').bPopup().close();

        $.ajax({
            type: 'POST',
            url: controlador + "grabarValidacion",
            dataType: 'json',
            data: {
                fecha: $('#idFechaEnvio').val(), hora: hora, empresa: $('#cbEmpresa2').val(),
                idFormato: formato, semana: sem, mes: $('#txtMes').val()
            },
            success: function (evt) {
                if (evt == 1) {
                    mostrarListado();
                }
                else {
                    alert("Error en Grabar Ampliación");
                }
            },
            error: function () {
                alert("Error en Grabar Ampliación");
            }
        });
    }
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
        url: controlador + "ListarFormatosXLectura",
        data: {
            sLectura: $('#hfLectura').val(), sEmpresa: $('#hfEmpresa').val(), idModulo: $('#hfIdModulo').val()
        },
        success: function (evt) {
            $('#listTipoInformacion').html(evt);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });

}

function listarFormato2(idLectura) {
    $.ajax({
        type: 'POST',
        url: controlador + "ListarFormatosXLectura2",
        data: {
            idLectura: idLectura, idEmpresa: $('#cbEmpresa2').val(), idModulo: $('#hfIdModulo').val()
        },
        success: function (evt) {
            $('#listTipoInformacion2').html(evt);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });

}

function buscarFormatPeriodo(valor) {
    for (var i = 0 ; i < listFormatCodi.length; i++)
        if (listFormatCodi[i] == valor) return listFormatPeriodo[i];
}

horizonte = function (valor) {
    var opcion = buscarFormatPeriodo(valor);
    switch (parseInt(opcion)) {
        case 1: //dia
            $('#trFecha td').show();
            $('#trSemana td').hide();
            $('#trMes td').hide();
            break;
        case 2: //semanal            
            $('#trMes td').hide();
            $('#trFecha td').hide();
            $('#trSemana td').show();
            break;
        case 3: case 5:
            $('#trMes td').show();
            $('#trFecha td').hide();
            $('#trSemana td').hide();
            break;
    }
}