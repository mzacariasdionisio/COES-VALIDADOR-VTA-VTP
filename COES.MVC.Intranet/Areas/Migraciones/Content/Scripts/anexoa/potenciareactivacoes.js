//variables globales
var controlador = siteRoot + 'Migraciones/AnexoA/';
var USAR_COMBO_TIPO_RECURSO = false;
$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            CargarCentral();
        }
    });

    $('#btnBuscar').click(function () {
        cargarLista();
    });

    $('#txtFechaInicio').Zebra_DatePicker({
        //direction: -1
    });

    $('#txtFechaFin').Zebra_DatePicker({
        //direction: -1
    });

    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    CargarCentral();
}

function CargarCentral() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarCentralxEmpresa',
        data: {
            idEmpresa: getEmpresa()
        },
        success: function (aData) {
            $('#central').html(aData);
            cargarLista();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarLista() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaPotenciaReactivaCOES',
        data: {
            idEmpresa: getEmpresa(),
            idCentral: getCentral(),
            fechaIni: getFechaInicio(),
            fechaFin: getFechaFin()
        },
        success: function (aData) {
            $('#listado').html(aData.Resultado);
            $('#idGraficoContainer').html('');
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}