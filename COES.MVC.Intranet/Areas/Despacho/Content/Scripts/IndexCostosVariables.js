var controlador = siteRoot + 'Despacho/';
var OPCION_VER = 1;
var OPCION_EDITAR = 2;
var OPCION_NUEVO = 3;
var OPCION_ACTUAL = 0;

$(function () {
    $('#tab-container').easytabs({
        animate: false
    });

    $('#FechaConsultaIni').Zebra_DatePicker({
    });
    $('#FechaConsultaFin').Zebra_DatePicker({
    });

    $('#btnConsultar').click(function () {
        mostrar();
    });

    $('#btnNuevo').click(function () {
        nuevaDetAct();
    });

    $(".tp_diario").show();
    $(".tp_semanal").hide();
    $('#cbTipoPrograma').change(function () {
        if ($('#cbTipoPrograma').val() == 'S') {
            $(".tp_diario").hide();
            $(".tp_semanal").hide();
            cargarSemanaAnho();
        } else {
            $(".tp_semanal").hide();
            $(".tp_diario").show();
        }
    });

    $('#anho').Zebra_DatePicker({
        format: 'Y',
        onSelect: function () {
            cargarSemanaAnho();
        }
    });

    mostrar();
});
function mostrar() {
    mostrarActualizaciones();
    mostrarDocumentosCV();
};

function mostrarActualizaciones() {
    var anio = $("#anho").val();
    var semana = $("#cbSemana").val();
    $.ajax({
        type: "POST",
        url: controlador + "CostosVariables/ListadoCostosVariables",
        data: {
            tipoPrograma: $("#cbTipoPrograma").val(),
            fechaIni: $('#FechaConsultaIni').val(),
            fechaFin: $('#FechaConsultaFin').val(),
            anio: anio,
            semanaOp: semana
        },
        success: function (evt) {
            $('#listado').html(evt);
            $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "aaSorting": [[0, "desc"]],
                "destroy": "true",
                "aoColumnDefs": [
                     { 'bSortable': false },
                     { "sClass": "view", "aTargets": [6] }
                ]
            });
        },
        error: function () {
            mostrarError();
        }
    });
}

function mostrarDocumentosCV() {
    var anio = $("#anho").val();
    var semana = $("#hfSemana").val();
    $.ajax({
        type: "POST",
        url: controlador + "CostosVariables/ListadoCostosVariablesDocumentos",
        data: {
            tipoPrograma: $("#cbTipoPrograma").val(),
            fechaIni: $('#FechaConsultaIni').val(),
            fechaFin: $('#FechaConsultaFin').val(),
            anio: anio,
            semanaOp: semana
        },
        success: function (evt) {
            $('#documentosCV').html(evt);
            $('#tablaDocCV').dataTable({
                "sPaginationType": "full_numbers",
                "aaSorting": [[0, "desc"]],
                "destroy": "true",
                "aoColumnDefs": [
                     { 'bSortable': false },
                     { "sClass": "view", "aTargets": [2] }
                ]
            });
        },
        error: function () {
            mostrarError();
        }
    });
}

actualizarCostoVariable = function (repcodi) {
    location.href = controlador + "DatosModoOperacion/Index?repcodi=" + repcodi;
};
function mostrarDetalle(repCodi) {
    location.href = controlador + "CostosVariables/ViewCostoVariable?repCodi=" + repCodi;
};
function reporteCostoVariable(repCodi) {
    location.href = controlador + "CostosVariables/ReporteCostosVariables?repCodi=" + repCodi;
};

function editarDetAct(id) {
    formularioDetAct(id, OPCION_EDITAR);
}

function nuevaDetAct() {
    formularioDetAct(0, OPCION_NUEVO);
}

function formularioDetAct(id, opcion) {
    $.ajax({
        type: 'POST',
        url: controlador + "CostosVariables/FormularioDetalle",
        data: {
            repcodi: id
        },
        success: function (evt) {
            $('#editarDetAct').html(evt);
            $('#mensaje').css("display", "none");

            inicializarDetAct(opcion);

            setTimeout(function () {
                $('#popupDetAct').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function inicializarDetAct(opcion) {
    $('#FechaNew').Zebra_DatePicker({
        onSelect: function () {
            actualizarNombre();
        }
    });

    $('#FechaEmiNew').inputmask({
        mask: "1/2/y h:s:s",
        placeholder: "dd/mm/yyyy hh:mm:ss",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#FechaEmiNew').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#FechaEmiNew').val(date + " 00:00:00");
        }
    });

    var tipoProg = $("#hfTipoProgramaNew").val();
    $("#cbTipoProgramaNew").unbind();
    $("#cbTipoProgramaNew").val(tipoProg);
    if (tipoProg == 'S') {
        $(".tp_diario_new").hide();
        $(".tp_semanal_new").hide();
    } else {
        $(".tp_diario_new").show();
        $(".tp_semanal_new").hide();
    }

    $('#cbTipoProgramaNew').change(function () {
        if ($('#cbTipoProgramaNew').val() == 'S') {
            $(".tp_diario_new").hide();
            $(".tp_semanal_new").show();
            cargarSemanaAnho("new");
        } else {
            $(".tp_semanal_new").hide();
            $(".tp_diario_new").show();
        }
    });

    $('#anhonew').Zebra_DatePicker({
        format: 'Y',
        onSelect: function () {
            cargarSemanaAnho("new");
        }
    });

    switch (opcion) {
        case OPCION_VER:
            $("#tdCancelar2").show();
            $(".tbfiltroFT").hide();
            $("#lbTeamB").parent().show();
            $(".tr_estado").show();
            break;
        case OPCION_EDITAR:
            $("#cbTipoProgramaNew").prop('disabled', 'disabled');
            $("#FechaNew").prop('disabled', 'disabled');
            $(".tp_diario_new").show();
            $("#tdGuardar").show();
            $("#tdCancelar").show();
            $("#txtNombre").removeAttr('disabled');
            $(".tbfiltroFT").show();
            $(".tr_estado").show();
            break;
        case OPCION_NUEVO:
            $("#tdGuardar").show();
            $("#tdCancelar").show();
            $("#txtNombre").removeAttr('disabled');
            $(".tbfiltroFT").show();
            break;
    }
    $("#btnGuardar").unbind();
    $('#btnGuardar').click(function () {
        guardarDetAct();
    });

    //
    $("#btnGuardar").unbind();
    $('#btnGuardar').click(function () {
        guardarDetAct();
    });
    $("#btnCancelar").unbind();
    $('#btnCancelar').click(function () {
        cerrarPopup();
    });
    $("#btnCancelar2").unbind();
    $('#btnCancelar2').click(function () {
        cerrarPopup();
    });
}

function guardarDetAct() {
    var entity = getObjetoJson();
    if (confirm('¿Desea guardar el Detalle?')) {
        var msj = validarJson(entity);

        if (msj == "") {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                traditional: true,
                url: controlador + 'CostosVariables/GuardarDetalle',
                data: {
                    repcodi: entity.repcodi,
                    tipoPrograma: entity.tipoPrograma,
                    strfecha: entity.strfecha,
                    anio: entity.anio,
                    semanaOp: entity.semanaOp,
                    nombre: entity.nombre,
                    detalle: entity.detalle,
                    observacion: entity.observacion,
                    strFechaEmision: entity.strFechaEmision
                },
                cache: false,
                success: function (result) {
                    if (result.Error != undefined) {
                        alert(result.Descripcion);
                    } else {
                        alert("Se guardó correctamente el Detalle");
                        cerrarPopup();
                        mostrar();
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

function getObjetoJson() {
    var obj = {};
    obj.repcodi = parseInt($("#hfCodigoNew").val()) || 0;
    obj.tipoPrograma = $("#cbTipoProgramaNew").val();
    obj.strfecha = $("#FechaNew").val();
    obj.anio = $("#anhonew").val();
    obj.semanaOp = $("#cbSemananew").val();
    obj.nombre = $("#nombreNew").val();
    obj.detalle = $("#txtDetalleNew").val();
    obj.observacion = $("#txtObsNew").val();
    obj.strFechaEmision = $("#FechaEmiNew").val();

    return obj;
}

function validarJson(obj) {
    var msj = "";
    if (obj.nombre == null || obj.nombre == "") {
        msj += "Debe ingresar nombre.";
    }

    if (obj.strfecha == null || obj.strfecha == "") {
        msj += "Debe seleccionar una fecha";
    }

    if (obj.detalle == null || obj.detalle == "") {
        msj += "Debe ingresar la descripción del detalle.";
    }

    return msj;
}

function cerrarPopup() {
    $('#popupDetAct').bPopup().close();
}

function actualizarNombre() {
    $("#nombreNew").val("");
    var entity = getObjetoJson();

    $.ajax({
        type: 'POST',
        dataType: 'json',
        traditional: true,
        url: controlador + 'CostosVariables/GetNombreRepCV',
        data: {
            tipoPrograma: entity.tipoPrograma,
            strfecha: entity.strfecha,
            anio: entity.anio,
            semanaOp: entity.semanaOp
        },
        cache: false,
        success: function (result) {
            $("#nombreNew").val(result);
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function cargarSemanaAnho(form) {
    var sufijo = form != undefined ? form : "";
    var anho = parseInt($("#anho" + sufijo).val()) || 0;
    var semana = anho + "1";

    $('#cbSemana' + sufijo).empty();

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: controlador + 'CostosVariables/CargarSemanas',
        data: {
            idAnho: anho
        },
        success: function (aData) {
            $('#cbSemana' + sufijo).unbind('change');

            for (var i = 0; i < aData.ListaSemana.length; i++) {
                var item = aData.ListaSemana[i];
                $('#cbSemana' + sufijo).append('<option value=' + item.Valor + '>' + item.Descripcion + '</option>');
            }
            $('#cbSemana' + sufijo).val(aData.SemanaActual);

            if (sufijo != "") {
                actualizarNombre();
                $('#cbSemana' + sufijo).change(function () {
                    actualizarNombre();
                });
            }

            $(".tp_semanal" + sufijo).show();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}