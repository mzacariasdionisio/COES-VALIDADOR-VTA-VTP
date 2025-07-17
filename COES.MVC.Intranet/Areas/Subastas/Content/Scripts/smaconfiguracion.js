var controlador = siteRoot + 'Subastas/Configuracion/'
var OPCION_VER = 1;
var OPCION_EDITAR = 2;
var OPCION_NUEVO = 3;
var OPCION_ACTUAL = 0;
var OPCION_LISTA = 0;

var LISTADO_MOTIVO = [];
var IMG_EDITAR = '<img src="' + siteRoot + 'Content/Images/btn-edit.png" alt="" width="19" height="19" style="">';

$(document).ready(function () {
    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').bind('easytabs:after', function () {
        refrehDatatable();
    });

    //Tab1

    //Método Eliminar Configuración
    $('.btnEliminarConfiguracion').click(function () {
        var valor = $(this).attr('data-valor');
        var reg = $(this).attr('data-registro');
        mensajeOperacion("\u00BF" + "Esta seguro de eliminar la configuracion: " + valor + "?", null
            , {
                showCancel: true,
                onOk: function () {
                    $.ajax({
                        type: "POST",
                        global: false,
                        url: controlador + 'ConfiguracionEliminada',
                        //dataType: 'json',
                        data: { registro: reg },
                        cache: false,
                        success: function (resultado) {
                            mensajeOperacion(resultado, 1);
                        },
                        error: function (req, status, error) {
                            mensajeOperacion(error);
                            validaErrorOperation(req.status);
                        }
                    });
                },
                onCancel: function () {

                }
            });
    });

    //Muestra ventana de Editar Configuración
    $('.btnEditarConfiguracion').click(function () {
        var reg = $(this).attr('data-registro');
        $.ajax({
            type: "POST",
            global: false,
            url: controlador + 'EditarConfiguracion',
            //dataType: 'json',
            data: {
                registro: reg,
            },
            cache: false,
            success: function (resultado) {
                $('#ele-popup-content').html('<div class="title_tabla_pop_up">' + $('.btnEditarConfiguracion').attr('title') + '</div>' + resultado);
                var t = setTimeout(function () {
                    $('#ele-popup').bPopup({ modalClose: false, escClose: false });
                    clearTimeout(t);
                }, 60)
            },
            error: function (req, status, error) {
                mensajeOperacion(error);
                validaErrorOperation(req.status);
            }
        });
    });

    //Muestra ventana Nueva Configuración
    $('.btnNuevaConfiguracion').click(function () {
        $.ajax({
            type: "POST",
            global: false,
            url: controlador + 'NuevaConfiguracion',
            //dataType: 'json',
            data: {},
            cache: false,
            success: function (resultado) {

                $('#ele-popup-content').html('<div class="title_tabla_pop_up">' + $('.btnNuevaConfiguracion').attr('title') + '</div>' + resultado);
                var t = setTimeout(function () {
                    $('#ele-popup').bPopup({ modalClose: false, escClose: false });
                    clearTimeout(t);
                }, 60)
            },
            error: function (req, status, error) {
                mensajeOperacion(error);
                validaErrorOperation(req.status);
            }
        });
    });

    //configuración parámetros
    $('#txtFechaData').Zebra_DatePicker({
        onSelect: function () {
            buscarParametro();
        }
    });
    buscarParametro();


    //tab2
    $('#btnNuevoMotivo').click(function () {
        abriPopupNuevoMotivo();
    });
    $('#btnGuardarMotivo').click(function () {
        motivoGuardar();
    });

    motivoListado();
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
    var fechaConsulta = $('#txtFechaData').val();

    $.ajax({
        type: 'POST',
        url: controlador + "ListadoParametros",
        data: {
            fechaConsulta: fechaConsulta
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

//Tab 2: Crud de Motivos
function abriPopupNuevoMotivo() {
    //valores por defecto
    $("#hfSmammcodi").val(0);
    $("#txtMotivo").val("");
    $("#cbEstadoMotivo").val("A");

    setTimeout(function () {
        $('#popupMotivo').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 250);
}

function editarPopupMotivo(id) {
    var obj = LISTADO_MOTIVO.find((element) => element.Smammcodi == id);

    $("#hfSmammcodi").val(id);
    $("#txtMotivo").val(obj.Smammdescripcion);
    $("#cbEstadoMotivo").val(obj.Smammestado);

    setTimeout(function () {
        $('#popupMotivo').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 250);
}

function _generarObjetoMotivoForm() {

    var obj = {};
    obj.Smammcodi = parseInt($("#hfSmammcodi").val()) || 0;
    obj.Smammdescripcion = $("#txtMotivo").val().trim();
    obj.Smammestado = $("#cbEstadoMotivo").val();

    return obj;
}

function _validarObjMotivo(obj) {
    var listaMsj = [];

    var tamanioMotivo = obj.Smammdescripcion.length;
    if (tamanioMotivo > 300) listaMsj.push("Se ha superado el número máximo de caracteres del campo 'Motivo'. ");
    if (tamanioMotivo == 0) listaMsj.push("No se ha registrado el Motivo de activación de Oferta por defecto. ");

    return listaMsj.join('\n');
}

function motivoGuardar() {
    var objJson = _generarObjetoMotivoForm();
    var msjVal = _validarObjMotivo(objJson);

    if (msjVal == "") {
        $.ajax({
            type: 'POST',
            async: true,
            url: controlador + 'GuardarMotivo',
            data: {
                smammcodi: objJson.Smammcodi,
                motivo: objJson.Smammdescripcion,
                estado: objJson.Smammestado
            },
            success: function (result) {
                if (result.Resultado == "-1") {
                    alert(result.Mensaje);
                } else {
                    alert("El registro se ha guardado correctamente.");

                    motivoListado();
                    $('#popupMotivo').bPopup().close();
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    } else {
        alert(msjVal);
    }
}

function motivoListado() {
    LISTADO_MOTIVO = [];
    $("#div_listado_motivo").html('');

    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoMotivo',
        dataType: 'json',
        data: {
        },
        cache: false,
        success: function (data) {
            if (data.Resultado != "-1") {
                LISTADO_MOTIVO = data.ListaMotivo;

                var html = dibujarTablaListadoMotivo(LISTADO_MOTIVO);
                $("#div_listado_motivo").html(html);

                $('#tblListadoMotivo').dataTable({
                    "destroy": "true",
                    "scrollY": 550,
                    "scrollX": true,
                    "sDom": 'ft',
                    "ordering": false,
                    "bPaginate": false,
                    "iDisplayLength": -1
                });

            } else {
                alert("Ha ocurrido un error: " + data.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function dibujarTablaListadoMotivo(lista) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" width="100%" id="tblListadoMotivo">
        <thead>
            <tr>
                <th style='width: 70px'>Acciones</th>
                <th style='width: 40px'>Motivo de Activación de Oferta por defecto</th>
                <th style='width: 200px'>Estado</th>
                <th style='width: 100px'>Usuario creación</th>
                <th style='width: 100px'>Fecha creación</th>
                <th style='width: 100px'>Usuario modificación</th>
                <th style='width: 100px'>Fecha modificación</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (var key in lista) {
        var item = lista[key];

        cadena += `
            <tr>
                <td style="height: 24px">
                    <a title="Editar" href="JavaScript:editarPopupMotivo(${item.Smammcodi});">${IMG_EDITAR} </a>
                </td>
                <td style="height: 24px; text-align: left; padding-left: 5px;">${item.Smammdescripcion}</td>
                <td style="height: 24px">${item.SmammestadoDesc}</td>
                <td style="height: 24px">${item.Smammusucreacion}</td>
                <td style="height: 24px">${item.SmammfeccreacionDesc}</td>
                <td style="height: 24px">${item.Smammusumodificacion}</td>
                <td style="height: 24px">${item.SmammfecmodificacionDesc}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function refrehDatatable() {

    $('#tblListadoMotivo').dataTable({
        "destroy": "true",
        "scrollY": 550,
        "scrollX": true,
        "sDom": 'ft',
        "ordering": false,
        "bPaginate": false,
        "iDisplayLength": -1
    });
}