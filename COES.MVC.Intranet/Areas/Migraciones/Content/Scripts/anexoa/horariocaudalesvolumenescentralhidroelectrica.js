//variables globales
var controlador = siteRoot + 'Migraciones/AnexoA/'
var idEmpresa = ""
var fechaInicio = "";
var fechaFin = "";

$(function () {
    $('#tab-container').easytabs({
        animate: false
    });

    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarListaHorariosCaudalVolumenCentralHidroelectrica();
        }
    });

    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    validacionesxFiltro();
    cargarListaHorariosCaudalVolumenCentralHidroelectrica();
}

function cargarListaHorariosCaudalVolumenCentralHidroelectrica() {
    $("#listado1").html('');
    $("#listado2").html('');

    validacionesxFiltro();
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarReporteVertimiento',
            data: { idEmpresa: idEmpresa, fechaInicio: fechaInicio, fechaFin: fechaFin, tipoRpte:'1'},
            success: function (aData) {
                $('#listado1').html(aData[0].Resultado);
                $('#listado2').html(aData[1].Resultado);
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function validacionesxFiltro() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    idEmpresa = $('#hfEmpresa').val();

    fechaInicio = $('#txtFechaInicio').val();
    fechaFin = $('#txtFechaFin').val();

    var arrayFiltro = arrayFiltro || [];
    arrayFiltro.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" });

    validarFiltros(arrayFiltro);
}