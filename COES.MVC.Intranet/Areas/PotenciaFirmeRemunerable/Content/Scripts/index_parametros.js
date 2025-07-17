var controlador = siteRoot + "PotenciaFirmeRemunerable/Configuracion/";

var OPCION_VER = 1;
var OPCION_EDITAR = 2;
var OPCION_NUEVO = 3;
var OPCION_ACTUAL = 0;
var OPCION_LISTA = 0;

$(document).ready(function () {


    $('#cbAnio').change(function () {
        listadoPeriodo();
    });

    $('#cbPeriodo').change(function () {
        buscarParametro();
    });

    buscarParametro();
});

function editarAgrupacion() {
    setTimeout(function () {
        $('#popupHistoricoConcepto').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 250);
}

//configuración parámetros

//consulta
function buscarParametro() {
    var pericodi = parseInt($("#cbPeriodo").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "ListadoParametros",
        data: {
            pfrpercodi: pericodi
        },
        success: function (evt) {

            $('#listado_parametro').html(evt);
            $('#tabla_parametro').dataTable({
                "sDom": 'ft',
                "ordering": true,
                "iDisplayLength": -1,
                "order": [[2, "asc"]]
            });
        },
        error: function (err) {
            alert('Ha ocurrido un error: ' + err);
        }
    });
};

function verHistoricoParametro(grupocodi, concepcodi, concepdesc, valor) {
    OPCION_ACTUAL = OPCION_VER;
    OPCION_LISTA = OPCION_VER;
    inicializarGrupoDat(OPCION_ACTUAL, grupocodi, concepcodi, concepdesc, valor);
}

function editarParametroConfig(grupocodi, concepcodi, concepdesc, valor) {
    OPCION_ACTUAL = OPCION_EDITAR;
    OPCION_LISTA = OPCION_EDITAR;
    inicializarGrupoDat(OPCION_ACTUAL, grupocodi, concepcodi, concepdesc, valor);
}

function inicializarGrupoDat(opcion, grupocodi, concepcodi, concepdesc, valor) {
    $("#hfGrupocodiDat").val(grupocodi);
    $("#hfConcepcodiDat").val(concepcodi);
    $("#parametroConfig").html(concepdesc);
    $('#listadoGrupoDat').html('');

    configurarFormularioGrupodatNuevo();

    switch (opcion) {
        case OPCION_VER:
            $("#popupHistoricoConcepto .content-botonera").hide();
            $("#popupHistoricoConcepto .titulo_listado").hide();
            break;
        case OPCION_EDITAR:
            $("#popupHistoricoConcepto .content-botonera").show();
            break;
        case OPCION_NUEVO:
            $("#popupHistoricoConcepto .content-botonera").hide()
            break;
    }

    $("#btnGrupodatNuevo").unbind();
    $('#btnGrupodatNuevo').click(function () {
        $("#btnGrupodatNuevo").hide();
        configurarFormularioGrupodatNuevo();
    });

    $("#btnGrupodatGuardar").unbind();
    $('#btnGrupodatGuardar').click(function () {
        guardarGrupodat();
    });

    $("#btnGrupodatConsultar").unbind();
    $('#btnGrupodatConsultar').click(function () {
        listarHistoricos(grupocodi, concepcodi);
    });

    listarHistoricos(grupocodi, concepcodi);

    $('#fechaData').Zebra_DatePicker({
    });
}

function configurarFormularioGrupodatNuevo() {
    OPCION_ACTUAL = OPCION_NUEVO;
    $("#formularioGrupodat").show();
    $("#fechaAct").val($("#hfFechaAct").val());
    $("#fechaData").val($("#hfFechaAct").val());
    $("#valorData").val('');
    $("#hfDeleted").val(0);
    $("#btnGrupodatGuardar").val("Registrar");
    $("#formularioGrupodat .popup-title span").html("Nuevo Registro");
    $("#fechaData").removeAttr('disabled');
    $("#fechaData").parent().find("button").prop("class", "Zebra_DatePicker_Icon Zebra_DatePicker_Icon_Inside");
}

function editarGrupodat(FechadatDesc, Formuladat, FechaactDesc, deleted) {
    OPCION_ACTUAL = OPCION_EDITAR;
    $("#formularioGrupodat").show();
    $("#fechaAct").val(FechaactDesc);
    $("#fechaData").val(FechadatDesc);
    $("#valorData").val(Formuladat);
    $("#hfDeleted").val(deleted);
    $("#btnGrupodatGuardar").val("Actualizar");
    $("#formularioGrupodat .popup-title span").html("Modificar Registro");
    $("#fechaData").prop('disabled', 'disabled');
    $("#fechaData").parent().find("button").prop("class", "Zebra_DatePicker_Icon Zebra_DatePicker_Icon_Disabled Zebra_DatePicker_Icon_Inside");
}

function listarHistoricos(grupocodi, concepcodi) {
    $("#btnGrupodatNuevo").show();
    $("#formularioGrupodat").hide();

    $.ajax({
        type: 'POST',
        url: controlador + 'ListarHistoricoParametros',
        data: {
            grupocodi: grupocodi,
            concepcodi: concepcodi,
            opedicion: OPCION_LISTA
        },
        success: function (result) {
            $('#listadoGrupoDat').html(result);

            $('#tablaListadoGrupodat').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true",
                "bInfo": false,
                "ordering": false,
                "order": [[2, "asc"]],
                "iDisplayLength": 15
            });

            setTimeout(function () {
                $('#popupHistoricoConcepto').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 250);
        },
        error: function (err) {
            alert('Ha ocurrido un error: ' + err);
        }
    });
}

function guardarGrupodat() {
    var entity = getObjetoGrupodatJson();

    if (confirm('\u00BFDesea guardar el registro?')) {
        var msj = validarData(entity);
        var jsonResult = "GrupodatGuardar";

        if (msj == "") {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                traditional: true,
                url: controlador + jsonResult,
                data: {
                    tipoAccion: OPCION_ACTUAL,
                    grupocodi: entity.grupocodi,
                    concepcodi: entity.concepcodi,
                    strfechaDat: entity.fechaData,
                    formuladat: entity.valorData,
                    deleted: entity.deleted
                },
                cache: false,
                success: function (result) {
                    if (result.Resultado == '-1') {
                        alert('Ha ocurrido un error:' + result.Mensaje);
                    } else {
                        alert("Se guard\u00F3 correctamente el registro");
                        buscarParametro();
                        listarHistoricos(entity.grupocodi, entity.concepcodi);
                    }
                },
                error: function (err) {
                    alert('Ha ocurrido un error.');
                }
            });
        } else {
            alert(msj);
        }
    }
}

function eliminarGrupodat(grupocodi, concepcodi, FechadatDesc, deleted) {
    if (confirm("\u00BFDesea eliminar el registro?")) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            traditional: true,
            url: controlador + 'GrupodatEliminar',
            data: {
                grupocodi: grupocodi,
                concepcodi: concepcodi,
                strfechaDat: FechadatDesc,
                deleted: deleted
            },
            cache: false,
            success: function (result) {
                if (result.Resultado == '-1') {
                    alert('Ha ocurrido un error:' + result.Mensaje);
                } else {
                    alert("Se elimin\u00F3 correctamente el registro");
                    buscarParametro();
                    listarHistoricos(grupocodi, concepcodi);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}

function getObjetoGrupodatJson() {
    var obj = {};
    obj.fechaData = $("#fechaData").val();
    obj.valorData = $("#valorData").val();
    obj.grupocodi = parseInt($("#hfGrupocodiDat").val()) || 0;
    obj.concepcodi = parseInt($("#hfConcepcodiDat").val()) || 0;
    obj.deleted = parseInt($("#hfDeleted").val()) || 0;
    return obj;
}

function validarData(obj) {
    var msj = "";

    if (obj.fechaData == null || obj.fechaData == '') {
        msj += "Debe seleccionar una fecha";
    }
    if (obj.valorData == null || obj.valorData == '') {
        msj += "Debe ingresar un valor";
    }

    return msj;
}

function listadoPeriodo() {

    var anio = parseInt($("#cbAnio").val()) || 0;

    $("#cbPeriodo").empty();

    $.ajax({
        type: 'POST',
        url: controlador + "PeriodoListado",
        data: {
            anio: anio,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.ListaPeriodo.length > 0) {
                    $.each(evt.ListaPeriodo, function (i, item) {
                        $('#cbPeriodo').get(0).options[$('#cbPeriodo').get(0).options.length] = new Option(item.Pfrpernombre, item.Pfrpercodi);
                    });
                } else {
                    $('#cbPeriodo').get(0).options[0] = new Option("--", "0");
                }

                buscarParametro();
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}
