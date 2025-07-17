var controlador = siteRoot + 'PotenciaFirmeRemunerable/Configuracion/';

var NUEVO = 1;
var EDITAR = 2;

var ANCHO_TABLA = 1200;

var listadoPeriodos = "";

$(function () {
    listadoPeriodos = $("#hfListadoPeriodos").val();
    $('#cntMenu').css("display", "none");

    $("#new-penalidad-periodo").Zebra_DatePicker({
        format: 'd-m-Y',
    });

    /** GUARDAR PENALIDAD */
    $("#frmNuevaPenalidad").submit(function (event) {
        event.preventDefault();
        var data = getFormData($(this));
        guardarPenalidad(data, NUEVO);
    });

    /** BOTONES NUEVA PENALIDAD */
    $("#btnNuevo").click(function () {
        abrirPopupNuevaPenalidad();
    });


    // COMBOS AÑOS
    $('#cbAnioPenalidad').change(function () {
        listadoPeriodo();
    });


    // COMBOS PERIODOS   
    $('#cbPeriodoPenalidad').change(function () {
        fechaSegunComboEquipo("cbPeriodoPenalidad", "hf_new-vigencia-penalidad");
        cargarListadoPenalidad();
    });

    ANCHO_TABLA = ($('#mainLayout').width() - 30) + "px";

    // COMBOS ESTADOS
    $('#cbEstadoPenalidad').change(function () {
        cargarListadoPenalidad();
    });

    /** CARGAR LISTADO DE PENALIDADES */
    cargarListadoPenalidad();

});

/** Carga lista de Penalidad **/
function cargarListadoPenalidad() {

    obj = ObtenerFiltroElegido();

    $.ajax({
        type: 'POST',
        url: controlador + "ListarPenalidad",
        data: {
            pericodi: obj.pericodiEscogido,
            estado: obj.estadoEscogido
        },
        success: function (evt) {
            var anchoDiv = ANCHO_TABLA;

            if (evt.Resultado != "-1") {
                var altotabla;
                var idTabla;

                $('#listadoDePenalidades').html(evt.Resultado);


                $("#listadoDePenalidades").css("width", (anchoDiv) + "px");
                altotabla = parseInt($('#listadoDePenalidades').height()) || 0;
                idTabla = "#tabla_lstPenalidades";

                $(idTabla).dataTable({
                    "sPaginationType": "full_numbers",
                    "destroy": "true",
                    "ordering": false,
                    "searching": true,
                    "iDisplayLength": 15,
                    "info": false,
                    "paging": false,
                    "scrollX": true,
                    "scrollY": altotabla > 355 || altotabla == 0 ? 355 + "px" : "100%"
                });


            } else {
                alert('Ha ocurrido un error: ' + evt.Mensaje);
            }

        },
        error: function (err) {
            alert('Ha ocurrido un error: ' + err);
        }
    });
};

/** Carga lista de periodos por año */
function listadoPeriodo() {

    var annio = -1;

    annio = parseInt($("#cbAnioPenalidad").val()) || 0;
    $("#cbPeriodoPenalidad").empty();

    $.ajax({
        type: 'POST',
        url: controlador + "PeriodoListado",
        data: {
            anio: annio,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.ListaPeriodo.length > 0) {
                    $.each(evt.ListaPeriodo, function (i, item) {
                        $('#cbPeriodoPenalidad').get(0).options[$('#cbPeriodoPenalidad').get(0).options.length] = new Option(item.Pfrpernombre, item.Pfrpercodi);
                    });
                    fechaSegunComboEquipo("cbPeriodoPenalidad", "hf_new-vigencia-penalidad");
                    cargarListadoPenalidad();
                } else {
                    $('#cbPeriodoPenalidad').get(0).options[0] = new Option("--", "0");
                }

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

    var altotablaL = parseInt($('#listadoDePenalidades').height()) || 0;
    $("#listadoDePenalidades").css("width", (1450) + "px");
    $('#tabla_lstPenalidades').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "ordering": false,
        "searching": true,
        "iDisplayLength": 15,
        "info": false,
        "paging": false,
        "scrollX": true,
        "scrollY": altotablaL > 355 || altotablaL == 0 ? 355 + "px" : "100%"
    });

}

function ObtenerFiltroElegido() {
    var filtro = {};

    filtro.estadoEscogido = $("#cbEstadoPenalidad").val();
    filtro.pericodiEscogido = parseInt($("#cbPeriodoPenalidad").val()) || 0;

    return filtro;
}

function abrirPopupNuevaPenalidad() {

    resetearPopupAgregarPenalidad();

    var codigoDisponibleEnPenalidad = $("#hfCodDisponiblePenalidad").val();

    $("#new-penalidad-id").val(codigoDisponibleEnPenalidad);
    $("#bloque_add_penalidad_periodo").css("display", "block");
    $("#bloque_add_penalidad_id").css("display", "block");
    $("#bloque_add_penalidad_valor").css("display", "block");
    $("#bloque_add_penalidad_descripcion").css("display", "block");

    setTimeout(function () {
        $('#popupNuevaPenalidad').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    });
}

function cerrarPopup(id) {
    $(id).bPopup().close()
}

function validarFormularioPenalidad(objPenalidad, periodoElegido, accion) {
    var msj = "";

    var validarId = true;
    var validarValor = true;

    var validarVigenciaIni = true;

    var esNuevo = false;

    if (accion == NUEVO) {
        esNuevo = true;
    }


    if (validarId && esNuevo) {
        if (objPenalidad.Pfrpenid == null || objPenalidad.Pfrpenid == '') {
            msj += "Debe ingresar ID" + "\n";
        }
    }

    if (validarValor && esNuevo) {
        if (objPenalidad.Pfrpenvalor == null || objPenalidad.Pfrpenvalor == '') {
            msj += "Debe ingresar Valor" + "\n";
        }
    }


    if (validarVigenciaIni) {
        if (periodoElegido == null || periodoElegido == '') {
            msj += "Debe seleccionar un Periodo" + "\n";
        }
    }

    return msj;
}

function getFormData($form) {
    var disabled = $form.find(':input:disabled').removeAttr('disabled');
    var unindexed_array = $form.serializeArray();
    disabled.attr('disabled', 'disabled');

    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });

    return indexed_array;
}

function guardarPenalidad(dataForm) {

    dataForm.Pfrcatcodi = TIPO_PENALIDAD;
    AgregarEntidadDat(dataForm);

    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarPenalidad',
        contentType: "application/json",
        dataType: 'json',
        data: JSON.stringify({
            pfrPenalidad: dataForm,
        }
        ),
        cache: false,
        success: function (data) {

            if (data.Resultado == "1") {
                $("#popupNuevaPenalidad").bPopup().close()
                $("#frmNuevaPenalidad").trigger("reset");

                alert("Se registró correctamente la Penalidad");

                //Actualizar proximo codigo disponible
                $("#hfCodDisponiblePenalidad").val(data.CodigoDisponiblePenalidad);

                //Listar Penalidades
                cargarListadoPenalidad();

            } else {
                alert('Ha ocurrido un error : ' + data.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function resetearPopupAgregarPenalidad() {

    $("#bloque_add_penalidad_periodo").css("display", "none");
    $("#bloque_add_penalidad_id").css("display", "none");
    $("#bloque_add_penalidad_valor").css("display", "none");
    $("#bloque_add_penalidad_descripcion").css("display", "none");

}

function fechaSegunComboEquipo(comboPenalidad, hfPenalidad) {
    var periodoPenalidad = parseInt($("#" + comboPenalidad).val()) || 0;
    var pCongestion = obtenerFechaIniNuevo(periodoPenalidad);
    $("#" + hfPenalidad).val(pCongestion);
}

function obtenerFechaIniNuevo(pericodi) {
    var arrayDeGrupos = listadoPeriodos.split(",");
    for (var i = 0; i < arrayDeGrupos.length; i++) {
        var grupo = arrayDeGrupos[i];
        var arraySubGrupo = grupo.split("/");
        var strPericodi = arraySubGrupo[0];
        if (strPericodi == pericodi) {
            return arraySubGrupo[1];
        }

    }
    return "";
}