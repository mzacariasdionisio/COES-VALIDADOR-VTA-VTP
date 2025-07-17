var controlador = siteRoot + 'demandaMaxima/Cumplimiento/';
var uploader;
var totNoNumero = 0;
var totLimSup = 0;
var totLimInf = 0;
var listErrores = [];
var listDescripErrores = ["BLANCO", "NÚMERO", "No es número", "Valor negativo", "Supera el valor máximo"];
var listValInf = [];
var listValSup = [];
var validaInicial = true;
var hot;
var hotOptions;
var evtHot;

$(function () {

    $('#txtMes').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#txtMesFin').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#cbEmpresa').multipleSelect({
        width: '200px',
        filter: true,
        onClick: function (view) {
        },
        onClose: function (view) {
        }
    });

    $('#cbCumplimiento').multipleSelect({
        width: '200px',
        filter: true,
        onClick: function (view) {
        },
        onClose: function (view) {
        }
    });

    $('#btnConsultar').click(function () {
        buscarCumplimiento();
    });

    $('#btnExportar').click(function () {
        exportarCumplimiento();
    });

    $("#cbTipoEmpresa").change(function () {
        changeTipoEmpresa();
    });


});

function changeTipoEmpresa() {
    var x = document.getElementById("cbTipoEmpresa").value;
    if (x != "") {
        $.ajax({
            type: 'POST',
            url: controlador + "ObtenerListaEmpresas",
            dataType: 'json',
            data: {
                tipoemprcodi: x
            },
            success: function (result) {
                $('#cbEmpresa').empty().multipleSelect('refresh');
                $(result).each(function (i, v) { // indice, valor
                    var $option = $('<option></option>')
                        .attr('value', v.Emprcodi)
                        .text(v.Etiqueta)
                        .prop('selected', false);
                    $('#cbEmpresa').append($option).change();
                })
                $('#cbEmpresa').multipleSelect('refresh')
            },
            error: function () {
                alert("Error al cargar la Lista de Empresas.");
            }
        });
    }
    else {
        $('#cbEmpresa').empty().multipleSelect('refresh');
    }
}

function exportarCumplimiento() {
    var inp1 = document.getElementById('cbPeriodoIni').value;
    var inp2 = document.getElementById('cbPeriodoFin').value;
    // convierte las fechas a yyyymmdd
    tmp = inp1.split('/');
    fini = tmp[2] + tmp[1] + tmp[0];
    tmp = inp2.split('/');
    ffin = tmp[2] + tmp[1] + tmp[0];
    // la comparación
    if (fini > ffin) {
        alert("El periodo inicial no puede ser mayor al periodo final");
    } else {
        //formato = 1 [Excel]
        var formato = "1";
        var empresa = $('#cbEmpresa').multipleSelect('getSelects');
        if (empresa == "[object Object]") empresa = "";
        $('#hfEmpresa').val(empresa);
        var cumplimie = $('#cbCumplimiento').multipleSelect('getSelects');
        if (cumplimie == "[object Object]") cumplimie = "";
        $('#hfCumplimiento').val(cumplimie);
        if ($('#cbPeriodoIni').val() != "" && $('#cbPeriodoFin').val() != "") {
            $.ajax({
                type: 'POST',
                url: controlador + 'exportar',
                data: {
                    nroPagina: 1,
                    empresas: $('#hfEmpresa').val(),
                    tipos: $('#cbTipoEmpresa').val(),
                    ini: $('#cbPeriodoIni').val(),
                    fin: $('#cbPeriodoFin').val(),
                    cumpli: $('#hfCumplimiento').val(),
                    ulcoes: $('#hfUlCoes').val()
                },
                dataType: 'json',
                success: function (result) {
                    if (result != -1) {
                        document.location.href = controlador + 'descargar?formato=' + formato + '&file=' + result
                    }
                    else {
                        mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
                    }
                },
                error: function () {
                    mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
                }
            });
        }
        else {
            mostrarAlerta("Seleccione rango de fechas del periodo.");
        }
    }
}

function mostrarExito(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}

function mostrarError(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}

function mostrarAlerta(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}

function mostrarMensaje(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
}

function buscarCumplimiento() {
    var inp1 = document.getElementById('cbPeriodoIni').value;
    var inp2 = document.getElementById('cbPeriodoFin').value;
    // convierte las fechas a yyyymmdd
    tmp = inp1.split('/');
    fini = tmp[2] + tmp[1] + tmp[0];
    tmp = inp2.split('/');
    ffin = tmp[2] + tmp[1] + tmp[0];
    // la comparación
    if (fini > ffin) {
        alert("El periodo inicial no puede ser mayor al periodo final");
    } else {
        var empresa = $('#cbEmpresa').multipleSelect('getSelects');
        if (empresa == "[object Object]") empresa = "";
        $('#hfEmpresa').val(empresa);
        var abreviatura = $('#txtAbreviatura').val();
        var cumplimie = $('#cbCumplimiento').multipleSelect('getSelects');
        if (cumplimie == "[object Object]") cumplimie = "";
        $('#hfCumplimiento').val(cumplimie);
        var ulcoes = document.getElementById('chkUlcoes').checked;
        $('#hfUlCoes').val((ulcoes === true ? 1 : 0));
        if ($('#cbPeriodoIni').val() != "" && $('#cbPeriodoFin').val() != "") {
            $.ajax({
                type: 'POST',
                url: controlador + 'ListarReporteCumplimiento',
                data: {
                    nroPagina: 1,
                    empresas: $('#hfEmpresa').val(),
                    tipos: $('#cbTipoEmpresa').val(),
                    ini: $('#cbPeriodoIni').val(),
                    fin: $('#cbPeriodoFin').val(),
                    cumpli: $('#hfCumplimiento').val(),
                    ulcoes: $('#hfUlCoes').val(),
                    abreviatura: abreviatura
                },
                success: function (evt) {
                    //$('#listado').css("width", $('#mainLayout').width() + "px");
                    $('#listado').html(evt);
                    $('#tabla').dataTable({
                        "scrollY": 314,
                        "scrollX": false,
                        "sDom": 't',
                        "ordering": false,
                        "bDestroy": true,
                        "bPaginate": false,
                        "iDisplayLength": 50
                    });
                },
                error: function () {
                    alert("Error al obtener la consulta");
                }
            });
        }
        else {
            mostrarAlerta("Seleccione rango de fechas del periodo.");
        }
    }
}