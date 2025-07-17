var controlador = siteRoot + 'PotenciaFirmeRemunerable/Configuracion/';

var BARRA = 1;
var LINEA = 2;
var TRAFO2 = 3;
var TRAFO3 = 4;
var COMPDINAMICA = 5;
var GAMSVTP = 6;
var GAMSSSAA = 7;
var GAMSEQUIPOS = 8;
var CONGESTION = 9;
var PENALIDAD = 10;

var OPCION_VER = 1;
var OPCION_EDITAR = 2;
var OPCION_NUEVO = 3;
var OPCION_ACTUAL = 0;
var OPCION_LISTA = 0;

function cargarInterfazPropiedadXTipo(codigo) {

    $('#tab-container_detalle').unbind();
    $('#tab-container_detalle').easytabs({
        animate: false
    });

    $('#tab-container_detalle').easytabs('select', '#vistaListado');
    $("#form_historico").hide();

    var pericodi = getPericodi();
    buscarParametro(codigo, pericodi);
}

function VerEquipo(pfreqpcodi, tipoEquipo) {
    //guardar el tipo de equipo en el popup
    $("#hfTipoEntidad").val(tipoEquipo);

    //cargar popup
    cargarInterfazPropiedadXTipo(pfreqpcodi);
}

function getPericodi() {

    var tipoEntidad = parseInt($("#hfTipoEntidad").val()) || 0;
    var idPericodi = 'cbPeriodo';

    switch (tipoEntidad) {
        case BARRA: idPericodi = 'cbPeriodoBarra'; break;
        case LINEA: idPericodi = 'cbPeriodoLinea'; break;
        case TRAFO2: idPericodi = 'cbPeriodoTrafo2'; break;
        case TRAFO3: idPericodi = 'cbPeriodoTrafo3'; break;
        case COMPDINAMICA: idPericodi = 'cbPeriodoCompDinamica'; break;
        //case GAMSVTP: idPericodi = ''; break;
        //case GAMSSSAA: idPericodi = ''; break;
        //case GAMSEQUIPOS: idPericodi = ''; break;
        case CONGESTION: idPericodi = 'cbPeriodoCongestion'; break;
        case PENALIDAD: idPericodi = 'cbPeriodoPenalidad'; break;
    }

    var pericodi = parseInt($("#" + idPericodi).val()) || 0;



    return pericodi;
}

function actualizarPantallaPrincipal(tipoEntidad) {
    switch (tipoEntidad) {
        case BARRA:
        case LINEA:
        case TRAFO2:
        case TRAFO3:
        case COMPDINAMICA:
            cargarListadoEquipos(tipoEntidad);
            break;
        case GAMSVTP: 
            cargarListaGamsVtp();
            break;
        case GAMSSSAA:
            cargarListaGamsSsaa();
            break;
        case GAMSEQUIPOS:
            cargarListaGamsEquipos();
            break;
        case CONGESTION:
            cargarListadoCongestion();
            break;
        case PENALIDAD:
            cargarListadoPenalidad();
            break;
    }

}

//consulta
function buscarParametro(pfrentcodi, pericodi) {
    $("#listado_parametro_barra").html('');

    $.ajax({
        type: 'POST',
        url: controlador + "ListadoPropiedadXTipo",
        data: {
            pfrentcodi: pfrentcodi,
            pericodi: pericodi
        },
        success: function (evt) {

            $('#listado_parametro_barra').html(evt);
            $('#tabla_parametro').dataTable({
                "sDom": 'ft',
                "ordering": true,
                "iDisplayLength": -1,
                "order": [[1, "asc"]]
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
};

function verHistoricoPropiedad(grupocodi, concepcodi, concepdesc, valor, tipoEntidad) {
    OPCION_ACTUAL = OPCION_VER;
    OPCION_LISTA = OPCION_VER;
    inicializarGrupoDat(OPCION_ACTUAL, grupocodi, concepcodi, concepdesc, valor, tipoEntidad);
}

function editarHistoricoPropiedad(grupocodi, concepcodi, concepdesc, valor, tipoEntidad) {
    OPCION_ACTUAL = OPCION_EDITAR;
    OPCION_LISTA = OPCION_EDITAR;
    inicializarGrupoDat(OPCION_ACTUAL, grupocodi, concepcodi, concepdesc, valor, tipoEntidad);
}

function inicializarGrupoDat(opcion, grupocodi, concepcodi, concepdesc, valor, tipoEntidad) {
    $("#hfGrupocodiDat").val(grupocodi);
    $("#hfConcepcodiDat").val(concepcodi);
    $("#hfTipoEntidad").val(tipoEntidad);
    $("#parametroConfig").html(concepdesc);
    $('#listadoGrupoDat').html('');

    configurarFormularioGrupodatNuevo();

    $("#form_historico").show();
    $('#tab-container_detalle').easytabs('select', '#vistaDetalle');

    switch (opcion) {
        case OPCION_VER:
            $("#form_historico .content-botonera").hide();
            $("#form_historico .titulo_listado").hide();
            break;
        case OPCION_EDITAR:
            $("#form_historico .content-botonera").show();
            break;
        case OPCION_NUEVO:
            $("#form_historico .content-botonera").hide()
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
    $("#fechaAct").val($("#txt_fecha_default_nuevo").val());
    $("#fechaData").val($("#txt_fecha_default_nuevo").val());
    $("#valorData").val('');
    $("#hfDeleted").val(0);
    $("#btnGrupodatGuardar").val("Registrar");
    $("#formularioGrupodat .popup-title span").html("Nuevo Registro");
    $("#fechaData").removeAttr('disabled');
    $("#fechaData").parent().find("button").prop("class", "Zebra_DatePicker_Icon Zebra_DatePicker_Icon_Inside");
}

function editarGrupodat(FechadatDesc, Formuladat, FechaactDesc, deleted, tipoEntidad) {
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
        url: controlador + 'ListarHistoricoPropiedadXTipo',
        data: {
            pfrentcodi: grupocodi,
            pfrcnpcodi: concepcodi,
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
        },
        error: function (err) {
            alert('Ha ocurrido un error: ' + err);
        }
    });
}

function guardarGrupodat() {

    var pericodi = getPericodi();
    var entity = getObjetoGrupodatJson();

    if (confirm('¿Desea guardar el registro?')) {
        var msj = validarData(entity);
        var jsonResult = "PfrEntidadDatGuardar";

        if (msj == "") {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: "application/json",
                url: controlador + jsonResult,
                data: JSON.stringify({
                    obj: entity,
                    tipoAccion: OPCION_ACTUAL,
                }),
                cache: false,
                success: function (result) {
                    if (result.Resultado == '-1') {
                        alert('Ha ocurrido un error:' + result.Mensaje);
                    } else {
                        alert("Se guardó correctamente el registro");

                        actualizarPantallaPrincipal(entity.Pfrcatcodi);
                        buscarParametro(entity.Pfrentcodi, pericodi);
                        listarHistoricos(entity.Pfrentcodi, entity.Pfrcnpcodi);
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

function eliminarGrupodat(grupocodi, concepcodi, FechadatDesc, deleted, tipoEntidad) {

    var pericodi = getPericodi();
    var entity = {};
    entity.Prfdatfechavigdesc = FechadatDesc;
    entity.Pfrentcodi = grupocodi || 0;
    entity.Pfrcnpcodi = concepcodi || 0;
    entity.Pfrdatdeleted = deleted || 0;
    entity.Pfrcatcodi = tipoEntidad;

    if (confirm("¿Desea eliminar el registro?")) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            contentType: "application/json",
            url: controlador + 'PfrEntidadDatEliminar',
            data: JSON.stringify({
                obj: entity,
            }),
            cache: false,
            success: function (result) {
                if (result.Resultado == '-1') {
                    alert('Ha ocurrido un error:' + result.Mensaje);
                } else {
                    alert("Se eliminó correctamente el registro");

                    buscarParametro(entity.Pfrentcodi, pericodi);
                    listarHistoricos(entity.Pfrentcodi, entity.Pfrcnpcodi);
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
    obj.Prfdatfechavigdesc = $("#fechaData").val();
    obj.Pfrdatvalor = $("#valorData").val();
    obj.Pfrentcodi = parseInt($("#hfGrupocodiDat").val()) || 0;
    obj.Pfrcnpcodi = parseInt($("#hfConcepcodiDat").val()) || 0;
    obj.Pfrdatdeleted = parseInt($("#hfDeleted").val()) || 0;
    obj.Pfrcatcodi = parseInt($("#hfTipoEntidad").val()) || 0;

    return obj;
}

function validarData(obj) {
    var msj = "";

    if (obj.Prfdatfechavigdesc == null || obj.Prfdatfechavigdesc == '') {
        msj += "Debe seleccionar una fecha";
    }
    if (obj.Pfrdatvalor == null || obj.Pfrdatvalor == '') {
        msj += "Debe ingresar un valor";
    }

    return msj;
}
