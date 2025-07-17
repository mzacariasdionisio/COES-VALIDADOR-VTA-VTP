var controlador = siteRoot + 'IEOD/HorasOperacion/';
var evtHot;
var ancho = 900;

$(function () {
    moment.locale('es');

    $('#idForHorasOperacion').on('click', '#botonCancelar', function () {
        $('#newHorasOperacion').bPopup().close();
    })
    $('#idForHorasOperacion').on('click', '#botonAceptar', function () {
        actualizaHorasOperacion();
    });

    $('#cbTipoCentral').change(function () {
        cargarListaEmpresa(0);
    });
    $('#cbEmpresa').change(function () {
        $('#listado').html('');
        mostrarOcultarCentral(0);
    });
    $('#txtFechaInicio').Zebra_DatePicker({
        onSelect: function (date) {
            $('#txtFechaFin').val(date);
            $('#listado').html('');
        }
    });
    $('#txtFechaFin').Zebra_DatePicker({
        onSelect: function () {
            $('#listado').html('');
        }
    });
    $('#cbTipoOperacion').multipleSelect({
        width: '222px',
        filter: true
    });
    $('#cbTipoOperacion').multipleSelect('checkAll');
    cargarListaEmpresa();

    $('#btnBuscar').click(function () {
        mostrarListado();
    });
    $('#btnExportar').click(function () {
        exportarReporteHOP();
    });
    $('#btnExportarOsinergmin').click(function () {
        exportarReporteHOP_Osinergmin();
    });

    ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;
});

/////////////////////////////////////////////////////////////////////////////////////////////
/// Reporte
/////////////////////////////////////////////////////////////////////////////////////////////
function mostrarListado() {
    $('#listado').html('');
    $('#listado').hide();
    var idTipoOperacion = $('#cbTipoOperacion').multipleSelect('getSelects');
    if (idTipoOperacion == "[object Object]") idTipoOperacion = "-1";
    if (idTipoOperacion == "") idTipoOperacion = "-1";
    $('#hfTipoOperacion').val(idTipoOperacion);
    var sFechaIni = $('#txtFechaInicio').val()
    var sFechaFin = $('#txtFechaFin').val()
    var idEmpresa = $('#cbEmpresa').val();
    var idTipoCentral = $('#cbTipoCentral').val();
    var idCentral = -2;
    var idFiltroEnsayoPe = $('#cbEnsayoPe').val();
    var idFiltroEnsayoPMin = $('#cbEnsayoPmin').val();
    var checkCompensar = getCheckCompensar();

    $.ajax({
        type: 'POST',
        url: controlador + "ListaReporte",
        dataType: 'json',
        data: {
            idEmpresa: idEmpresa,
            sIdTipoOperacion: $('#hfTipoOperacion').val(),
            sFechaIni: sFechaIni,
            sFechaFin: sFechaFin,
            idTipoCentral: idTipoCentral,
            idCentral: idCentral,
            idFiltroEnsayoPe: idFiltroEnsayoPe,
            idFiltroEnsayoPMin: idFiltroEnsayoPMin,
            checkCompensar: checkCompensar
        },
        success: function (evt) {
            if (evt.Error == -1) {
                alert(evt.Descripcion);
            } else {
                $('#listado').css("width", $('#mainLayout').width() + "px");

                $('#listado').html(evt.Reporte);
                if (idTipoCentral == 37 || idTipoCentral == 39) {
                    $(".modo_operacion").hide();
                }
                if (idTipoCentral == 4) {
                    $(".titulo.modo_operacion").text("Grupo");
                }
                if (idTipoCentral == 5) {
                    $(".titulo.modo_operacion").text("Modo de Operación");
                }

                $('#listado').show();

                var anchoReporte = $('#tabla').width();
                $("#listado").css("width", (anchoReporte > ancho ? ancho : anchoReporte) + "px");

                $('#tabla').dataTable({
                    "language": {
                        "emptyTable": "¡No existen Horas de Operación registradas!"
                    },
                    "order": [[1, 'asc']],
                    "destroy": "true",
                    "info": false,
                    "searching": true,
                    "paging": false,
                    "scrollX": true,
                    "scrollY": $('#listado').height() > 200 ? 500 + "px" : "100%"
                });
            }
        },
        error: function () {
            alert("Ha ocurrido un error en listado");
        }
    });
}

function cargarListaEmpresa() {
    $('#listado').html('');
    $("#cbEmpresa").empty();
    $('#cbEmpresa').get(0).options[0] = new Option("--TODOS--", "-2");

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaEmpresaXTipoCentral',
        dataType: 'json',
        data: {
            tipoCentral: $('#cbTipoCentral').val()
        },
        cache: false,
        success: function (data) {
            if (data.ListaEmpresas.length > 0) {
                $.each(data.ListaEmpresas, function (i, item) {
                    $('#cbEmpresa').get(0).options[$('#cbEmpresa').get(0).options.length] = new Option(item.Emprnomb, item.Emprcodi);
                });
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

function exportarReporteHOP() {
    var idTipoOperacion = $('#cbTipoOperacion').multipleSelect('getSelects');
    if (idTipoOperacion == "[object Object]") idTipoOperacion = "-1";
    if (idTipoOperacion == "") idTipoOperacion = "-1";
    $('#hfTipoOperacion').val(idTipoOperacion);
    var sFechaIni = $('#txtFechaInicio').val()
    var sFechaFin = $('#txtFechaFin').val()
    var idEmpresa = $('#cbEmpresa').val();
    var idTipoCentral = $('#cbTipoCentral').val();
    var idCentral = -2;
    var idFiltroEnsayoPe = $('#cbEnsayoPe').val();
    var idFiltroEnsayoPMin = $('#cbEnsayoPmin').val();
    var checkCompensar = getCheckCompensar();

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarExcelReporteHOP',
        data: {
            idEmpresa: idEmpresa,
            sIdTipoOperacion: $('#hfTipoOperacion').val(),
            sFechaIni: sFechaIni,
            sFechaFin: sFechaFin,
            idTipoCentral: idTipoCentral,
            idCentral: idCentral,
            idFiltroEnsayoPe: idFiltroEnsayoPe,
            idFiltroEnsayoPMin: idFiltroEnsayoPMin,
            checkCompensar: checkCompensar
        },
        success: function (evt) {
            if (evt.Error == undefined) {
                window.location.href = controlador + 'DescargarExcelReporte?archivo=' + evt[0] + '&nombre=' + evt[1];
            }
            else {
                alert("Error:" + evt.Descripcion);
            }
        },
        error: function (result) {
            alert('ha ocurrido un error al descargar el archivo excel. ' + result.status + ' - ' + result.statusText + '.');
        }
    });
}

function exportarReporteHOP_Osinergmin() {
    var sFechaIni = $('#txtFechaInicio').val();
    var sFechaFin = $('#txtFechaFin').val();
    var checkCompensar = getCheckCompensar();

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarExcelReporteHOPOsinergmin',
        data: {
            sFechaIni: sFechaIni,
            sFechaFin: sFechaFin,
            checkCompensar: checkCompensar
        },
        success: function (evt) {
            if (evt.Error == undefined) {
                window.location.href = controlador + 'DescargarExcelReporte?archivo=' + evt[0] + '&nombre=' + evt[1];
            }
            else {
                alert("Error:" + evt.Descripcion);
            }
        },
        error: function (result) {
            alert('ha ocurrido un error al descargar el archivo excel. ' + result.status + ' - ' + result.statusText + '.');
        }
    });
}

function getCheckCompensar() {
    var estado = false;
    if ($('#check_comp').is(':checked')) {
        estado = true;
    }
    return estado;
}

/////////////////////////////////////////////////////////////////////////////////////////////
/// Edición
/////////////////////////////////////////////////////////////////////////////////////////////

function verHoraOperacion(hopcodi, tipoCentral, idEmpresa, idCentral) {

    $.ajax({
        type: 'POST',
        url: controlador + 'PopUpEditarHorasOperacion',
        async: false,
        data: {
            hopcodi: hopcodi,
            idEmpresa: idEmpresa,
            idTipoCentral: tipoCentral,
            central: idCentral
        },
        success: function (evt) {
            $('#idForHorasOperacion').html(evt);

            setTimeout(function () {
                $('#newHorasOperacion').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false
                });
            }, 50);
        },
        error: function (result) {
            alert('ha ocurrido un error al generar vista');
        }
    });
}

function generarModelHoraOperacionEdicion() {
    var idEmpresa = $('#hfIdEmpresa').val();
    var idTipoCentral = $('#hfTipoCentral').val();
    var idCentral = $('#hfIdCentral').val();

    var fecha = $('#hfFecha').val();
    var fechaFin = $('#txtFueraParaleloF').val();
    var hopCodi = $('#hfHopcodi').val();
    var ordenArranque = $('#txtOrdenArranqueH').val();
    var enParalelo = $('#txtEnParaleloH').val();
    var ordenParada = $('#txtOrdenParadaH').val();
    var fueraParalelo = $('#txtFueraParaleloH').val();
    var tipoOperacion = $('#cbTipoOp').val();
    var descripcion = $('#TxtDescripcion').val();
    var equicodi = $('#hfEquicodi').val();
    var grupocodi = $('#hfGrupocodi').val();

    if (equicodi == "") {
        equicodi = -1;
    }
    if (grupocodi == "") {
        grupocodi = -1;
    }

    var horArranque = ordenArranque != null && ordenArranque != "" ? moment(convertStringToDate(fecha, ordenArranque)).toDate() : null;
    var horIni = enParalelo != null && enParalelo != "" ? moment(convertStringToDate(fecha, enParalelo)).toDate() : null;
    var horParada = ordenParada != null && ordenParada != "" ? moment(convertStringToDate(fecha, ordenParada)).toDate() : "";
    var horFin = fueraParalelo != null && fueraParalelo != "" ? moment(convertStringToDate(fechaFin, fueraParalelo)).toDate() : null;
    var tipoOp = tipoOperacion;
    var fueraServicio = null;
    var valCkeckfueraServ = document.getElementById("chkFueraServicio").checked;
    if (valCkeckfueraServ == 1) {
        fueraServicio = 'F';
    }

    var model = {};
    var horaOperacion = {};
    horaOperacion.Hopcodi = hopCodi;
    horaOperacion.Hophorini = horIni;
    horaOperacion.Hophorfin = horFin;
    horaOperacion.Hophorordarranq = horArranque;
    horaOperacion.Hophorparada = horParada;
    horaOperacion.Subcausacodi = tipoOp;
    horaOperacion.Hopfalla = fueraServicio;
    horaOperacion.Hopdesc = descripcion;
    horaOperacion.Equicodi = equicodi;
    horaOperacion.Grupocodi = grupocodi;

    model.HoraOperacion = horaOperacion;
    model.IdCentralSelect = idCentral;
    model.IdEmpresa = idEmpresa;
    model.IdTipoCentral = idTipoCentral;

    return model;
}

function grabarActualizacionesHOP() {
    var data = JSON.stringify(generarModelHoraOperacionEdicion());
    $.ajax({
        type: 'POST',
        url: controlador + "RegistrarEdicionHoReporte",
        data: {
            dataHo: data
        },
        success: function (evt) {
            if (evt.Resultado == 1) {
                alert("La Hora de Operación se guardó correctamente");
                $('#newHorasOperacion').bPopup().close();
                mostrarListado();
            } else {
                alert(evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error en grabar hora de operación");
        }
    });
}

function diferenciaFechas(enParalelo, fueraParalelo, sfecha) {
    var HoraIni = convertStringToDate(sfecha, enParalelo);
    var HoraFin = convertStringToDate(sfecha, fueraParalelo);
    // comprobar si una fecha es anterior a otra    
    return moment(HoraIni).isBefore(HoraFin);
}