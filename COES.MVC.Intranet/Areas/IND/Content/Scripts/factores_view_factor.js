var controlador = siteRoot + 'IND/ReporteFactores/';
var ancho = 1000;
var ALTURA_HANDSON = 600;

var FACTOR_FORT_TERMICO = 8;
var FACTOR_PROG_TERMICO = 9;
var FACTOR_PROG_HIDRO = 10;
var FACTOR_PRESENCIA = 11;

var LISTA_HoT = [null];

$(function () {
    $('#cntMenu').css("display", "none");

    $('#cbEmpresa').multipleSelect({
        width: '250px',
        filter: true,
        single: false,
        onClose: function () {
            cargarListaCentral();
        }
    });

    $('#cbEmpresa').multipleSelect('checkAll');

    $('#cbCentral').multipleSelect({
        width: '220px',
        filter: true,
        single: false,
        onClose: function () {
            cargarHansonWeb();
        }
    });

    $('#cbCentral').multipleSelect('checkAll');

    //
    $("#btnExportarExcel").click(function () {
        exportarExcel();
    });

    $("#btnRegresar").click(function () {
        var cuadro = parseInt($("#hfCuadro").val()) || 0;
        var recalculo = parseInt($("#hfRecalculo").val()) || 0;
        var periodo = parseInt($("#hfPeriodo").val()) || 0;

        window.location.href = siteRoot + "IND/ReporteFactores/Factor?idCuadro=" + cuadro + "&pericodi=" + periodo + "&recacodi=" + recalculo;
    });

    $("#btnAprobar").click(function () {
        var irptcodi = $("#hfReporteVersion").val();
        aprobarVersion(irptcodi);
    });

    cargarHansonWeb();
});

///////////////////////////
/// Listar filtros 
///////////////////////////

function cargarListaEmpresa() {
    $("#div_empresa_filtro").html('');
    $("#div_central_filtro").html('');

    $.ajax({
        type: 'POST',
        url: controlador + 'ViewCargarFiltros',
        dataType: 'json',
        data: {
            irptcodi: $("#hfReporteVersion").val(),
            centralIntegrante: $('#cbCentralIntegrante').val(),
            empresa: getEmpresa(),
            famcodi: $("#cbFamilia").val()
        },
        cache: false,
        success: function (data) {
            $("#div_empresa_filtro").html('<select id="cbEmpresa" name="cbEmpresa"></select>');
            $("#div_central_filtro").html('<select id="cbCentral" name="cbCentral"></select>');

            if (data.Resultado != "-1") {
                if (data.ListaEmpresa.length > 0) {
                    $.each(data.ListaEmpresa, function (i, item) {
                        $('#cbEmpresa').get(0).options[$('#cbEmpresa').get(0).options.length] = new Option(item.Emprnomb, item.Emprcodi);
                    });
                }
                if (data.ListaCentral.length > 0) {
                    $.each(data.ListaCentral, function (i, item) {
                        $('#cbCentral').get(0).options[$('#cbCentral').get(0).options.length] = new Option(item.Central, item.Equipadre);
                    });
                }

                $('#cbEmpresa').multipleSelect({
                    width: '250px',
                    filter: true,
                    single: false,
                    onClose: function () {
                        cargarListaCentral();
                    }
                });
                $('#cbEmpresa').multipleSelect('checkAll');

                $('#cbCentral').multipleSelect({
                    width: '220px',
                    filter: true,
                    single: false,
                    onClose: function () {
                        cargarHansonWeb();
                    }
                });
                $('#cbCentral').multipleSelect('checkAll');

                cargarHansonWeb();
            } else {
                alert("Ha ocurrido un error: " + data.Mensaje);
            }
        },
        error: function (err) {
            mostrarError();
        }
    });
}

function cargarListaCentral() {
    $("#div_central_filtro").html('');

    $.ajax({
        type: 'POST',
        url: controlador + 'ViewCargarFiltros',
        dataType: 'json',
        data: {
            irptcodi: $("#hfReporteVersion").val(),
            centralIntegrante: $('#cbCentralIntegrante').val(),
            empresa: getEmpresa(),
            famcodi: $("#cbFamilia").val()
        },
        cache: false,
        success: function (data) {
            $("#div_central_filtro").html('<select id="cbCentral" name="cbCentral"></select>');

            if (data.ListaCentral.length > 0) {
                $.each(data.ListaCentral, function (i, item) {
                    $('#cbCentral').get(0).options[$('#cbCentral').get(0).options.length] = new Option(item.Central, item.Equipadre);
                });
            }

            $('#cbCentral').multipleSelect({
                width: '220px',
                filter: true,
                single: false,
                onClose: function () {
                    cargarHansonWeb();
                }
            });
            $('#cbCentral').multipleSelect('checkAll');

            cargarHansonWeb();
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function getEmpresa() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]" || empresa.length == 0) empresa = "-1";
    $('#hfEmpresa').val(empresa);
    var idEmpresa = $('#hfEmpresa').val();

    return idEmpresa;
}

function getCentral() {
    var central = $('#cbCentral').multipleSelect('getSelects');
    if (central == "[object Object]" || central.length == 0) central = "-1";
    $('#hfCentral').val(central);
    var idEmpresa = $('#hfCentral').val();

    return idEmpresa;
}

///////////////////////////
/// Handson 
///////////////////////////
function cargarHansonWeb() {
    var cuadro = parseInt($("#hfCuadro").val()) || 0;

    switch (cuadro) {
        case FACTOR_PRESENCIA:
            cargarHansonWebFactorPresencia();
            break;

        case FACTOR_PROG_HIDRO:
            cargarHansonWebFactorProgHidro();
            break;

        case FACTOR_PROG_TERMICO:
            cargarHansonWebFactorProgTermico();
            break;

        case FACTOR_FORT_TERMICO:
            cargarHansonWebFactorFortTermico();
            break;
    }
}

function cargarHansonWebFactorPresencia() {

    ALTURA_HANDSON = parseInt($(".panel-container").height());
    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 800;

    $("#listado1").html('');

    var container1 = document.getElementById('listado1');

    if (typeof LISTA_HoT[0] != 'undefined' && LISTA_HoT[0] !== null) {
        LISTA_HoT[0].destroy();
    }

    $.ajax({
        type: 'POST',
        url: controlador + "ViewCargarExcelWebFactorPresencia",
        data: {
            irptcodi: $("#hfReporteVersion").val(),
            empresa: getEmpresa(),
            central: getCentral(),
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                crearGrillaExcelFactorPresencia(0, container1, evt.HandsonFP, ALTURA_HANDSON);
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function cargarHansonWebFactorFortTermico() {

    ALTURA_HANDSON = parseInt($(".panel-container").height());
    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 800;

    $("#listado1").html('');

    var container1 = document.getElementById('listado1');

    if (typeof LISTA_HoT[0] != 'undefined' && LISTA_HoT[0] !== null) {
        LISTA_HoT[0].destroy();
    }

    $.ajax({
        type: 'POST',
        url: controlador + "ViewCargarExcelWebFactorFortTermico",
        data: {
            irptcodi: $("#hfReporteVersion").val(),
            empresa: getEmpresa(),
            central: getCentral(),
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                crearGrillaExcelFactorFortTermico(0, container1, evt.HandsonFF, ALTURA_HANDSON);
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function cargarHansonWebFactorProgTermico() {

    ALTURA_HANDSON = parseInt($(".panel-container").height());
    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 800;

    $("#listado1").html('');

    var container1 = document.getElementById('listado1');

    if (typeof LISTA_HoT[0] != 'undefined' && LISTA_HoT[0] !== null) {
        LISTA_HoT[0].destroy();
    }

    $.ajax({
        type: 'POST',
        url: controlador + "ViewCargarExcelWebFactorProgTermico",
        data: {
            irptcodi: $("#hfReporteVersion").val(),
            empresa: getEmpresa(),
            central: getCentral(),
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                crearGrillaExcelFactorProgTermico(0, container1, evt.HandsonFF, ALTURA_HANDSON);
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function cargarHansonWebFactorProgHidro() {

    ALTURA_HANDSON = parseInt($(".panel-container").height());
    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 800;

    $("#listado1").html('');

    var container1 = document.getElementById('listado1');

    if (typeof LISTA_HoT[0] != 'undefined' && LISTA_HoT[0] !== null) {
        LISTA_HoT[0].destroy();
    }

    $.ajax({
        type: 'POST',
        url: controlador + "ViewCargarExcelWebFactorProgramadoHidro",
        data: {
            irptcodi: $("#hfReporteVersion").val(),
            empresa: getEmpresa(),
            central: getCentral(),
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                crearGrillaExcelFactorProgHidro(0, container1, evt.HandsonFP, ALTURA_HANDSON);
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}
function exportarExcel() {

    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoExcelCuadro",
        data: {
            irptcodi: $("#hfReporteVersion").val(),
            empresa: getEmpresa(),
            central: getCentral(),
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "Exportar?file_name=" + evt.Resultado;
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });

}

function refrehDatatable() {
    $('#tabla_reporte_consolidado').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "ordering": false,
        "searching": true,
        "iDisplayLength": 15,
        "info": false,
        "paging": false,
        "scrollX": true,
        "scrollY": $('#listado1').height() > 400 ? 400 + "px" : "100%"
    });
}

