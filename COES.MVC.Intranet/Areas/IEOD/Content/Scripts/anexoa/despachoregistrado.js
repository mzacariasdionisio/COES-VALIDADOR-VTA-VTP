var ancho = 900;

$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function (view) {
            CargarCentral();
        }
    });

    parametro1 = $("#cbTPotencia").val();
    $('#cbTPotencia').change(function () {
        parametro1 = $("#cbTPotencia").val();
        cargarLista();
    });

    parametro2 = getTipoLectura48();
    $('input[name=cbLectura48]').change(function () {
        parametro2 = getTipoLectura48();
        cargarLista();
    });

    parametro3 = '-1';
    parametro4 = '0';
    $('#cbTipoGeneracion').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function (view) {
            parametro3 = getTipoGeneracion();
            cargarLista();
        }
    });

    ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;

    cargarValoresIniciales();
});
function mostrarReporteByFiltros() {
    cargarLista();
}

function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbTipoGeneracion').multipleSelect('checkAll');
    $('#cbTipoGeneracion').parent().hide()

    CargarCentral();
}

function CargarCentral() {
    var idEmpresa = getEmpresa();
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
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    } else {
        alert("Debe seleccionar al menos una empresa");
        $('#cbEmpresa').multipleSelect('checkAll');
    }
}

function cargarLista() {
    var idEmpresa = getEmpresa();
    var idCentral = getCentral();
    var fechaInicio = getFechaInicio();
    var fechaFin = getFechaFin();

    var potencia = $('#cbTPotencia').val();
    var idtipoGeneracion = getTipoGeneracion();
    var soloRecursosRER = $("#ckRecursos").prop('checked') ? '1' : '0';
    parametro4 = soloRecursosRER;

    if (idEmpresa != "" && idCentral != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarListaDespachoRegistrado',
            data: {
                idEmpresa: idEmpresa,
                idCentral: idCentral,
                idPotencia: potencia,
                tipoDato48: getTipoLectura48(),
                fechaIni: fechaInicio,
                fechaFin: fechaFin,
                idtipoGeneracion: idtipoGeneracion,
                soloRecursosRER: soloRecursosRER
            },
            success: function (aData) {
                if (aData.Resultado != "-1") {
                    $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);
                    $('#listado').html(aData.Resultado);

                    var anchoReporte = $('#reporte').width();
                    $("#resultado").css("width", (anchoReporte > ancho ? ancho : anchoReporte) + "px");

                    $('#reporte').dataTable({
                        "scrollX": true,
                        "scrollY": "780px",
                        "scrollCollapse": true,
                        "sDom": 't',
                        "ordering": false,
                        paging: false,
                        fixedColumns: {
                            leftColumns: 1
                        }
                    });
                } else {
                    $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);
                    $('#listado').html('');
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    } else {
        alert("Debe seleccionar al menos una central");
        $('#cbCentral').multipleSelect('checkAll');
    }
}