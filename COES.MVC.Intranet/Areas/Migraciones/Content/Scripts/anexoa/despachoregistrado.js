//variables globales
var controlador = siteRoot + 'Migraciones/AnexoA/'
var idEmpresa = "";
var idUbicacion = "";
var idEquipo = "";
var fechaInicio = "";
var fechaFin = "";
var ancho = 1200;

$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function (view) {
            CargarCentral();
        }
    });
    
    $('#txtFechaInicio').Zebra_DatePicker({
        //direction: -1
    });

    $('#txtFechaFin').Zebra_DatePicker({
        //direction: -1
    });

    ancho = $("#mainLayout").width() > 1200 ? $("#mainLayout").width() : 1200;

    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    CargarCentral();
}

function CargarCentral() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    var idEmpresa = $('#hfEmpresa').val();
    if (idEmpresa != "") {
        $.ajax({
            type: 'POST',
            async: false,
            url: controlador + 'CargarCentralxEmpresa',
            data: {
                idEmpresa: getEmpresa()
            },
            success: function (aData) {
                $('#central').html(aData);
                $('#cbCentral').multipleSelect({
                    width: '150px',
                    filter: true,
                    onClose: function (view) {
                        cargarLista();
                    }
                });
                $('#cbCentral').multipleSelect('checkAll');
                cargarLista();
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    } else {
        alert("Debe seleccionar al menos una empresa");
        $('#cbEmpresa').multipleSelect('checkAll');
    }
}

function cargarLista() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    var idEmpresa = $('#hfEmpresa').val();

    var central = $('#cbCentral').multipleSelect('getSelects');
    if (empresa == "[object Object]") central = "-1";
    $('#hfCentral').val(central);
    var idCentral = $('#hfCentral').val();

    var potencia = $('#cbTPotencia').val();

    if (empresa != "" && central != "") {
        $.ajax({
            type: 'POST',
            async: false,
            url: controlador + 'CargarListaDespachoRegistrado',
            data: {
                idEmpresa: getEmpresa(),
                idCentral: getCentral(),
                idPotencia: potencia,
                fechaIni: getFechaInicio(),
                fechaFin: getFechaFin()
            },
            success: function (aData) {
                $('#listado').html(aData.Resultado);
                $("#resultado").css("width", ancho + "px");

                

                if (aData.Total == 0) {
                    $("#listado").css("width", "400px");
                } else {
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
                }

                $('#idGraficoContainer').html('');
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    } else {
        alert("Debe seleccionar al menos una central");
        $('#cbCentral').multipleSelect('checkAll');
    }
}

function cargarTipoCombustible() { cargarLista(); };
