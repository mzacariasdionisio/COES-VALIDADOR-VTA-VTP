var controlador = siteRoot + 'StockCombustibles/validacion/';
var listFormatCodi = [];
var listFormatPeriodo = [];
$(function () {

    $('#cbFormato').change(function () {
        cargarEmpresas();
    });
    $('#FechaDesde').Zebra_DatePicker({

    });

    //$('#cbSelectAll1').click(function (e) {
    //    alert("Exit");
    //    var table = $(e.target).closest('table');
    //    $('td input:checkbox', table).prop('checked', this.checked);
    //});

    //$('#cbSelectAll2').click(function (e) {

    //    var table = $(e.target).closest('table');
    //    $('td input:checkbox', table).prop('checked', this.checked);
    //});

    $('#btnBuscar').click(function () {
        if ($('#cbFormato').val() > 0) {
            mostrarListado();
        }
        else {
            alert("Error: Seleccionar Formato");
        }
    });

    cargarEmpresas();
    mostrarListado();
});

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
            fecha: $('#FechaDesde').val()
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