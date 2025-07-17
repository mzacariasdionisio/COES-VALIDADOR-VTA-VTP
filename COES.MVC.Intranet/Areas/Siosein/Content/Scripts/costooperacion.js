var controlador = siteRoot + 'Siosein/CostoOperacion/';
var ancho = 900;

$(function () {

    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true
    });

    $('#btnBuscar').click(function () {
        cargarlistaXdia();
    });

    $('#btnBuscarXmes').click(function () {
        cargarlistaXmes();
    });

    $('#txtFechaInicio').Zebra_DatePicker({
        onSelect: function () {
        }
    });

    $('#txtMes').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            cargarlistaXmes();
        }
    });

    ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;

    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
}

function cargarlistaXdia() {
    $("#tituloCosto").hide();
    $('#listado').html('');

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    var idEmpresa = $('#hfEmpresa').val();
    var fechaInicio = $("#txtFechaInicio").val();
    var tipoDatoMostrar = $("#tipoDatoMostrar").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaCostosOperacion',
        data: { idEmpresa: idEmpresa, fechaInicio: fechaInicio, tipoDatoMostrar: tipoDatoMostrar },
        success: function (aData) {
            if (aData.Resultado !== undefined && aData.Resultado != null) {
                $("#tituloCosto").show();
                $("#costoTotal").html(aData.CostoTotalOperacion);
                $('#listado').html(aData.Resultado);

                var anchoReporte = $('#reporte').width();
                $("#resultado").css("width", (anchoReporte > ancho ? ancho : anchoReporte) + "px");

                $('#reporte').dataTable({
                    "scrollX": true,
                    "scrollCollapse": true,
                    "sDom": 't',
                    "ordering": false,
                    paging: false,
                    fixedColumns: {
                        leftColumns: 1
                    }
                });

            } else {
                alert("Ha ocurrido un error");
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarlistaXmes() {

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    var idEmpresa = $('#hfEmpresa').val();
    var fechaInicio = $("#txtMes").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaCostosOperacionXMes',
        data: { idEmpresa: idEmpresa, fechaInicio: fechaInicio },
        success: function (aData) {
            $('#listado').html(aData);
            $("#idGraficoContainer").html('');
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}