var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarListaDifusionInstallRepotenciaRetiros_Ing();
            cargarListaDifusionInstallRepotenciaRetiros_Ret();
        }
    });
    $('#cbEmpresa').multipleSelect('checkAll');

    cargarListaDifusionInstallRepotenciaRetiros_Ing();
    cargarListaDifusionInstallRepotenciaRetiros_Ret();
});

function cargarListaDifusionInstallRepotenciaRetiros_Ing() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    var periodo = $("#txtFecha1").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionInstallRepotenciaRetiros_Ing',
        data: { idEmpresa: $('#hfEmpresa').val(), periodo: periodo },
        success: function (aData) {
            $('#listado1').html(aData.Resultado);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaDifusionInstallRepotenciaRetiros_Ret() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    var periodo = $("#txtFecha1").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionInstallRepotenciaRetiros_Ret',
        data: { idEmpresa: $('#hfEmpresa').val(), periodo: periodo },
        success: function (aData) {
            $('#listado2').html(aData.Resultado);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}