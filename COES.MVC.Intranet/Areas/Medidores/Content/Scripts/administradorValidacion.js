var controlador = siteRoot + 'Medidores/validacion/';
var listFormatCodi = [];
var listFormatPeriodo = [];
$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '222px',
        filter: true
    });
    $('#cbEmpresa').multipleSelect('checkAll');

    $('#cbFormato').change(function () {
        cargarEmpresas();
    });

    $('#mes').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {

        }
    });

    $('#btnBuscar').click(function () {
        if ($('#cbFormato').val() > 0) {
            mostrarListado();
        }
        else {
            alert("Error: Seleccionar Formato");
        }
    });

    cargarEmpresas();
});

function mostrarListado() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    var formato = $('#cbFormato').val();
    $('#hfEmpresa').val(empresa);
    $.ajax({
        type: 'POST',
        url: controlador + "lista",
        data: {
            sEmpresas: $('#hfEmpresa').val(),
            idformato: formato,
            mes: $('#mes').val()
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(evt);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarEmpresas() {
    var formato = $('#cbFormato').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarEmpresas',

        data: { idFormato: formato },

        success: function (aData) {
            $('#empresas').html(aData);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}