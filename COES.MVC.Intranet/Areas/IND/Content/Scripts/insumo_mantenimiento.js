var controlador = siteRoot + 'IND/mantenimiento/';
var APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO = 0;

var TIPO_ACCION_NUEVO = 1;
var TIPO_ACCION_VER = 2;
var TIPO_ACCION_EDITAR = 3;

var TIPO_FUENTE_INDISP = 1;
var TIPO_FUENTE_MANTTO = 2;
var TIPO_FUENTE_INDISP_DESC = "Aplicativo";
var TIPO_FUENTE_MANTTO_DESC = "Mantenimiento";

var LISTA_CAMBIOS = [];
var LISTA_DELETE = [];

$(function () {
    $('#cntMenu').css("display", "none");

    $('#cbAnio').change(function () {
        listadoPeriodo();
    });
    $('#cbPeriodo').change(function () {
        listadoRecalculo();
    });

    $('#cbRecalculo').change(function () {
        getRecalculo();
    });

    $('#btnBuscar').click(function () {
        buscarEvento();
    });

    $('#btnNuevo').click(function () {
        visualizarPopupMantenimiento(0, 0, false, TIPO_FUENTE_INDISP, TIPO_ACCION_NUEVO);
    });

    mostrarLeyenda();
    $('#cbFuenteDato').change(function () {
        mostrarLeyenda();

        $("#listado").html('');
        buscarEvento();
    });

    $('#cbTipoEmpresa').multipleSelect({
        width: '100%',
        filter: true,
        onClose: function (view) {
            cargarEmpresas();
        }
    });

    $('#cbTipoMantenimiento').multipleSelect({
        width: '100%',
        filter: true
    });

    $('#cbTipoMantto').multipleSelect({
        width: '100%',
        filter: true
    });

    $('#cbFamilia').multipleSelect({
        width: '100%',
        filter: true
    });

    $('#cbEmpresa').multipleSelect({
        width: '200px',
        filter: true
    });

    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbTipoMantenimiento').multipleSelect('checkAll');
    $('#cbTipoEmpresa').multipleSelect('checkAll');
    $('#cbFamilia').multipleSelect('checkAll');
    $('#cbTipoMantto').multipleSelect('checkAll');


    //multiple
    $('#btnAbrirPopup').click(function () {
        LISTA_CAMBIOS = [];
        LISTA_DELETE = [];

        abrirPopupMultiple();
    });
    $('#btnGuardar').click(function () {
        guardarClasificacion();
    });

    //volver a procesar

    $('#desc_fecha_ini').Zebra_DatePicker({});

    $('#desc_fecha_fin').Zebra_DatePicker({});

    $('#btnProcesarC1').click(function () {
        actualizarReporte(1);
    });
    $('#btnProcesarC2').click(function () {
        actualizarReporte(2);
    });
    $('#btnProcesarC4').click(function () {
        actualizarReporte(4);
    });

    $('#tab-container').easytabs();

    getRecalculo();
    //buscarEvento();
});

function buscarEvento() {
    $("#mensaje_consultar").hide();
    mostrarListado();
}

function cargarEmpresas() {
    $("#empresas").html('');

    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');
    $('#hfTipoEmpresa').val(tipoEmpresa);
    $.ajax({
        type: 'POST',
        url: controlador + 'EmpresaListado',

        data: { tiposEmpresa: $('#hfTipoEmpresa').val() },

        success: function (data) {
            if (data.Resultado != '-1') {
                $("#empresas").html("<select id='cbEmpresa' name='IdEmpresa' multiple='multiple'></select>");

                if (data.ListaEmpresa.length > 0) {
                    $.each(data.ListaEmpresa, function (i, item) {
                        $('#cbEmpresa').get(0).options[$('#cbEmpresa').get(0).options.length] = new Option(item.Emprnomb, item.Emprcodi);
                    });
                }

                $('#cbEmpresa').multipleSelect({
                    width: '250px',
                    filter: true
                });
                $('#cbEmpresa').multipleSelect('checkAll');
            } else {
                alert("Ha ocurrido un error: " + data.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error ");
        }
    });
}

function mostrarListado() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');

    var tipoMantenimiento = $('#cbTipoMantenimiento').multipleSelect('getSelects');
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');
    var tipoEquipo = $('#cbFamilia').multipleSelect('getSelects');
    var tipoMantto = $('#cbTipoMantto').multipleSelect('getSelects');
    var pericodi = parseInt($("#cbPeriodo").val()) || 0;

    if (empresa == "[object Object]") empresa = "-1";

    $('#hfEmpresa').val(empresa);
    $('#hfTipoMantenimiento').val(tipoMantenimiento);
    $('#hfTipoEmpresa').val(tipoEmpresa);
    $('#hfTipoEquipo').val(tipoEquipo);
    $('#hfTipoMantto').val(tipoMantto);

    var flagFiltro = $('input[name=rbFiltro]:checked').val();

    $('#reporte').css("display", "block");
    $('#listado').html('');

    $.ajax({
        type: 'POST',
        url: controlador + "Lista",
        data: {
            tiposMantenimiento: $('#hfTipoMantenimiento').val(),
            pericodi: pericodi,
            indispo: $('#cbIndispo').val(),
            tiposEmpresa: $('#hfTipoEmpresa').val(), empresas: $('#hfEmpresa').val(),
            tiposEquipo: $('#hfTipoEquipo').val(), interrupcion: $('#cbInterrupcion').val(),
            tiposMantto: $('#hfTipoMantto').val(),
            flagFiltro: flagFiltro
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#listado').css("width", $('#mainLayout').width() + "px");

                $('#listado').html(evt.Resultado);
                $('#listado2').html(evt.Resultado2);
                $('#listado3').html(evt.Resultado3);

                $(document).ready(function () {
                    var restarPos = $("#tipoRptHtml").val() == "1" ? 0 : 5;

                    $('#foot_mantto_1').html('<input type="text" placeholder="Buscar Ubicación" data-index="' + (17 - restarPos) + '" style="width: 105px;" />');
                    $('#foot_mantto_2').html('<input type="text" placeholder="Equipo" data-index="' + (18 - restarPos) + '" style="width: 50px;" />');

                    // DataTable
                    var tableMantto = $('#tabla_mantto_todo').DataTable({
                        scrollY: "300px",
                        scrollX: true,
                        scrollCollapse: true,
                        paging: false,
                        "sDom": 'ft',
                        ordering: false,
                        "stripeClasses": [],
                        columnDefs: [{
                            targets: 16 - restarPos, //empresa
                            render: $.fn.dataTable.render.ellipsis(25, true)
                        },
                        {
                            targets: 17 - restarPos, //ubicacion
                            render: $.fn.dataTable.render.ellipsis(20, true)
                        },
                            //{
                            //    targets: 17 - restarPos, //descripcion
                            //    render: $.fn.dataTable.render.ellipsis(90, true)
                            //}
                        ]
                    });

                    // Filter event handler
                    $(tableMantto.table().container()).on('keyup', 'tfoot input', function () {
                        tableMantto
                            .column($(this).data('index'))
                            .search(this.value)
                            .draw();
                    });

                    //https://stackoverflow.com/questions/56838899/datatable-first-radio-button-of-list-never-showing-checked
                    var htmlRow = $("#hdFirstRowRadio").val();
                    if (htmlRow != null) {
                        var params = htmlRow.split(',');
                        $("input[name=rbtnIndisponibilidad_" + params[0] + "]").prop('checked', false).removeAttr('checked');

                        if (params[1] != null && params[1].length > 0)
                            $("input[name=rbtnIndisponibilidad_" + params[0] + "][value=" + params[1] + "]").prop('checked', true).attr('checked', 'checked');
                        else
                            $("input[name=rbtnIndisponibilidad_" + params[0] + "][value='-1']").prop('checked', true).attr('checked', 'checked');
                    }

                    $('input[type=radio][name^="rbtnIndisponibilidad"]').unbind();
                    $('input[type=radio][name^="rbtnIndisponibilidad"]').change(function () {

                        var idFila = $(this).parent().parent().attr('id') + '';
                        var codigo = idFila.substring(3, idFila.length);

                        var tipoindisp = $('input[name="rbtnIndisponibilidad_' + codigo + '"]:checked').val();

                        if (tipoindisp == "PP" || tipoindisp == "FP") {
                            $("#txt_prmw_" + codigo).show();
                        } else {
                            $("#txt_prmw_" + codigo).val("");
                            $("#txt_prmw_" + codigo).hide();
                        }
                    });

                });
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarError(msj) {
    if (msj !== undefined && msj != null)
        alert("Ha ocurrido un error. " + msj);
    else
        alert("Ha ocurrido un error. ");
    console.log(msj);
}

function mostrarLeyenda() {
    $("#leyenda_app").hide();
    $("#leyenda_mantto").hide();
    var fuenteDato = parseInt($('#cbFuenteDato').val()) || 0;
    if (TIPO_FUENTE_INDISP == fuenteDato)
        $("#leyenda_app").show();
    else
        $("#leyenda_mantto").show();
}

////////////////////////////////////////////////////////////////////////////////////
/// Equipo
////////////////////////////////////////////////////////////////////////////////////

function seleccionarEquipo(idEquipo, equipo, substacion, empresa, famabrev, idEmpresa) {
    $('#txtNuevoEmpresa').text(empresa);
    $('#txtNuevoUbicacion').text(substacion);
    $('#txtNuevoTipoEquipo').text(famabrev);
    $('#txtNuevoEquipo').text(equipo);
    $("#hfFormEquicodi").val(idEquipo);

    $('#busquedaEquipo').bPopup().close();
    $('#busquedaEquipo').bPopup().close();
}

function visualizarSeleccionarEquipo() {
    var hayCambio = $("#hfHayCambios").val();
    if (APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO == 0 && hayCambio == 0) {
        cargarBusquedaEquipo(APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO);
    } else {
        openBusquedaEquipo();
    }

    APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO++;
}

function mostrarPopupMantenimiento() {
    setTimeout(function () {
        $('#popupMantenimiento').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: true
        });
    }, 50);
}

function cargarBusquedaEquipo(flag) {
    $.ajax({
        type: "POST",
        url: controlador + "BusquedaEquipoIndex",
        data: {
            filtroFamilia: 0
        },
        global: false,
        success: function (evt) {
            $('#busquedaEquipo').html(evt);
            if ($('#hfCodigoIndMantto').val() == "0" || flag == 0) {
                openBusquedaEquipo();
            }
        },
        error: function (req, status, error) {
            mostrarError();
        }
    });
}

function openBusquedaEquipo() {
    $('#busquedaEquipo').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    $('#txtFiltro').focus();
}

////////////////////////////////////////////////////////////////////////////////////
/// Crud de mantenimiento
////////////////////////////////////////////////////////////////////////////////////

function visualizarPopupMantenimiento(indmancodi, manttocodi, hayCambios, idFuente, tipoAccion) {
    APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO = 0
    if (hayCambios > 0) {
        mostrarPopupMantenimiento();
    } else {
        var pericodi = parseInt($("#cbPeriodo").val()) || 0;

        $('#busquedaEquipo').html('');
        $.ajax({
            type: 'POST',
            url: controlador + "MantenimientoFormulario",
            data: {
                indmancodi: indmancodi,
                manttocodi: manttocodi,
                pericodi: pericodi,
                idFuente: idFuente,
                tipoAccion: tipoAccion
            },
            success: function (dataHtml) {
                $('#idPopupMantenimiento').html(dataHtml);

                inicializarFormulario();

                mostrarPopupMantenimiento();
            },
            error: function (err) {
                mostrarError(err);
            }
        });
    }
}

function inicializarFormulario() {
    var fuenteDato = parseInt($("#hfFuenteDato").val());
    var accion = parseInt($("#hfTipoAccion").val());
    var fuenteDatoDesc = '';
    switch (fuenteDato) {
        case TIPO_FUENTE_INDISP:
            fuenteDatoDesc = TIPO_FUENTE_INDISP_DESC;
            break;
        case TIPO_FUENTE_MANTTO:
            fuenteDatoDesc = TIPO_FUENTE_MANTTO_DESC;
            break;
    }

    switch (accion) {
        case TIPO_ACCION_NUEVO:

            $("#popupMantenimiento .popup-title span").text("Nuevo Registro (" + TIPO_FUENTE_INDISP_DESC + ")");

            $('#txtNuevoIniF').Zebra_DatePicker({
                onSelect: function () {
                    setFechaSiguiente($('#txtNuevoIniF').val());
                    $("#hfFechaActual").val($("#txtNuevoIniF").val());
                    $("#txtNuevoFinF").val($("#hfFechaSiguiente").val());
                    $("#txtNuevoFinH").val('00:00:00');
                }
            });
            $(".replicar").show();

            $(".btnAcciones").show();
            $("#btnVisualizarEquipo").show();
            $("#mensaje").show();

            break;
        case TIPO_ACCION_VER:

            $("#popupMantenimiento .popup-title span").text("Detalle de Registro (" + fuenteDatoDesc + ")");

            if (fuenteDato == TIPO_FUENTE_INDISP) {
                var mantocodi = parseInt($("#hfCodigoEveMantto").val());
                if (mantocodi > 0) {
                    var msj = "<ul>" + "<li>Este registro es la modificación de un mantenimiento del SGOCOES.</li>" + "</ul>";
                    $('#mensaje').show();
                    $('#mensaje').removeClass();
                    $('#mensaje').addClass('action-message');
                    $('#mensaje').html(msj);
                }
            } else {
                var flagconsiderado = parseInt($("#hfFlagConsiderado").val())
                if (flagconsiderado == 0) {
                    var msj1 = "<ul>" + "<li>Este registro no es considerado en el cálculo del aplicativo.</li>" + "</ul>";
                    $('#mensaje').show();
                    $('#mensaje').removeClass();
                    $('#mensaje').addClass('action-message');
                    $('#mensaje').html(msj1);
                }
            }

            $("#txtNuevoIniF").prop('disabled', 'disabled');
            $("#txtNuevoIniH").prop('disabled', 'disabled');
            $("#txtNuevoFinH").prop('disabled', 'disabled');
            $("#cbNuevoTipoMantenimiento").prop('disabled', 'disabled');
            $("#cbNuevoTipoEvento").prop('disabled', 'disabled');
            $("#cbNuevoIndispo").prop('disabled', 'disabled');
            $("#cbNuevoInterrupcion").prop('disabled', 'disabled');
            $("#descripcion").prop('disabled', 'disabled');
            $(".replicar").hide();

            $(".btnAcciones2").show();

            break;
        case TIPO_ACCION_EDITAR:

            $("#popupMantenimiento .popup-title span").text("Edición de Registro (" + fuenteDatoDesc + ")");

            if (fuenteDato == TIPO_FUENTE_MANTTO) {
                var msj2 = "<ul>" + "<li>Las modificaciones se guardarán en el aplicativo Indisponibilidades.</li>" + "</ul>";
                $('#mensaje').show();
                $('#mensaje').removeClass();
                $('#mensaje').addClass('action-message');
                $('#mensaje').html(msj2);
            } else {
                var mantocodi1 = parseInt($("#hfCodigoEveMantto").val());
                if (mantocodi1 > 0) {
                    var msj3 = "<ul>" + "<li>Este registro es la modificación de un mantenimiento del SGOCOES.</li>" + "</ul>";
                    $('#mensaje').show();
                    $('#mensaje').removeClass();
                    $('#mensaje').addClass('action-message');
                    $('#mensaje').html(msj3);
                }
            }
            $("#txtNuevoIniF").prop('disabled', 'disabled');
            $("#cbNuevoTipoMantenimiento").prop('disabled', 'disabled');
            $("#cbNuevoTipoEvento").prop('disabled', 'disabled');
            $("#cbNuevoIndispo").prop('disabled', 'disabled');
            $("#cbNuevoInterrupcion").prop('disabled', 'disabled');
            $(".replicar").hide();

            $(".btnAcciones").show();

            break;
    }

    $('#btnAceptar').unbind();
    $('#btnAceptar').click(function () {
        guardarMantenimiento();
    });

    $('#btnCancelar').unbind();
    $('#btnCancelar').click(function () {
        $('#popupMantenimiento').bPopup().close();
    });

    $('#btnAceptar2').unbind();
    $("#btnAceptar2").click(function () {
        $('#popupMantenimiento').bPopup().close();
    });

    $('#btnVisualizarEquipo').unbind();
    $('#btnVisualizarEquipo').click(function () {
        visualizarSeleccionarEquipo();
    });

    //Inicializacion de campos
    var hfFormEmpresa = $("#hfFormEmpresa").val();
    var hfFormTipoEquipo = $("#hfFormTipoEquipo").val();
    var hfFormUbicacion = $("#hfFormUbicacion").val();
    var hfFormEquipo = $("#hfFormEquipo").val();
    var evenclasecodi = $("#hfFormTipoMantenimiento").val();
    var descripcion = $("#hfFormDescripcion").val();
    var hfTipoIndisp = $("#hfRbtnNuevoTipoIndisp").val();
    var hfAsocproc = $("#hfCheckAsocprocNuevo").val();

    $("#txtNuevoEmpresa").text(hfFormEmpresa);
    $("#txtNuevoUbicacion").text(hfFormUbicacion);
    $("#txtNuevoTipoEquipo").text(hfFormTipoEquipo);
    $("#txtNuevoEquipo").text(hfFormEquipo);
    $("#cbNuevoTipoMantenimiento").val(evenclasecodi);
    $("#descripcion").val(descripcion);

    $("#cboTipoIndisp25").val(hfTipoIndisp);
    $('#cboTipoIndisp25').unbind();
    $('#cboTipoIndisp25').change(function () {
        var tipoindisp1 = $('#cboTipoIndisp25').val();
        if (tipoindisp1 == "PP" || tipoindisp1 == "FP") {
            $("#td_pr").show();
        } else {
            $("#prmwNuevo").val("");
            $("#td_pr").hide();
        }
    });
    var tipoindisp = $('#cboTipoIndisp25').val();
    if (tipoindisp == "PP" || tipoindisp == "FP") {
        $("#td_pr").show();
    } else {
        $("#prmwNuevo").val("");
        $("#td_pr").hide();
    }

    var esCogen = $("#hfGrupocogeneracion").val();
    if (esCogen == "S")
        $("#tr_asoc").show();

    if (hfAsocproc != null && hfAsocproc != '' && hfAsocproc != '0')
        $("#checkAsocprocNuevo").attr('checked', 'checked');

    $('#txtNuevoIniH').inputmask({
        mask: "h:s:s",
        placeholder: "hh:mm:ss",
        alias: "datetime",
        hourformat: 24
    });
    $('#txtNuevoFinH').inputmask({
        mask: "h:s:s",
        placeholder: "hh:mm:ss",
        alias: "datetime",
        hourformat: 24
    });
    $("#txtNuevoFinH").on('blur', actualizartxtHoraFin);
    $("#txtNuevoFinH").keypress(actualizartxtHoraFin);
    $("#txtNuevoFinH").keyup(actualizartxtHoraFin);
}

function verMantto(idFuente, indmancodi, manttocodi) {
    visualizarPopupMantenimiento(indmancodi, manttocodi, false, idFuente, TIPO_ACCION_VER);
}

function editarMantto(idFuente, indmancodi, manttocodi) {
    visualizarPopupMantenimiento(indmancodi, manttocodi, false, idFuente, TIPO_ACCION_EDITAR);
}

function guardarMantenimiento() {
    var msj = validarRegistro();
    var fuenteDato = parseInt($("#hfFuenteDato").val());
    var accion = parseInt($("#hfTipoAccion").val());

    if (msj == "") {
        $('#mensaje').html('');
        $('#mensaje').removeClass();
        var entity = generarObjMantenimiento();
        var objMantto = JSON.stringify(entity);

        $.ajax({
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            traditional: true,
            async: false,
            url: controlador + 'GuardarMantenimiento',
            data: JSON.stringify({
                data: objMantto,
                idFuente: fuenteDato,
                tipoAccion: accion
            }),
            success: function (result) {
                if (result.Resultado == "1") {
                    $('#popupMantenimiento').bPopup().close();
                    switch (accion) {
                        case TIPO_ACCION_NUEVO:
                            alert("Se registró correctamente el nuevo mantenimiento");
                            break;
                        case TIPO_ACCION_EDITAR:
                            alert("Se modificó correctamente el mantenimiento");
                            break;
                    }
                    buscarEvento();
                } else {
                    alert(result.Mensaje);
                }
            },
            error: function (result) {
                mostrarError(result);
            }
        });
    } else {
        $('#mensaje').show();
        $('#mensaje').removeClass();
        $('#mensaje').addClass('action-alert');
        $('#mensaje').html(msj);
    }
}

function generarObjMantenimiento() {
    var id = $("#hfCodigoIndMantto").val();
    var id2 = $("#hfCodigoEveMantto").val();
    var equicodi = $("#hfFormEquicodi").val();
    var HophoriniFecha = $("#txtNuevoIniF").val();
    var HophoriniHora = $("#txtNuevoIniH").val();
    var HophorfinFecha = $("#txtNuevoFinF").val();
    var HophorfinHora = $("#txtNuevoFinH").val();
    var evenclasecodi = $("#cbNuevoTipoMantenimiento").val();
    var descripcion = $("#descripcion").val();
    var nroDiaReplicar = $("#numDiaReplicar").val();
    var Tipoindisp = $('#cboTipoIndisp25').val();
    var pr = parseFloat($("#prmwNuevo").val()) || 0;
    var Asocproc = $('#checkAsocprocNuevo').prop('checked') ? "S" : "N";
    var observacion = $("#comentario").val();
    observacion = observacion != null && observacion != undefined ? observacion.trim() : "";

    if (Tipoindisp != "PP" && Tipoindisp != "FP") {
        pr = null;
    }
    pr = pr > 0 ? pr : null

    var model = {};
    model.Equicodi = equicodi;
    model.FechaInicio = HophoriniFecha;
    model.HoraInicial = HophoriniHora;
    model.FechaFin = HophorfinFecha;
    model.HoraFinal = HophorfinHora;
    model.IdTipoMantenimiento = parseInt(evenclasecodi);
    model.Descripcion = descripcion;
    model.Indmancodi = id;
    model.Manttocodi = id2;
    model.NroDiaReplicar = nroDiaReplicar;

    model.Tipoindisp = Tipoindisp;
    model.Pr = pr;
    model.Asocproc = Asocproc;
    model.Comentario = observacion;

    return model;
}

function validarRegistro() {
    $("#numDiaReplicar").val(parseInt($("#numDiaReplicar").val()) || 0);
    $("#txtNuevoIniH").val(obtenerHoraValida($("#txtNuevoIniH").val()));
    $("#txtNuevoFinH").val(obtenerHoraValida($("#txtNuevoFinH").val()));

    var model = generarObjMantenimiento();

    var mensaje = "";

    if (model.Equicodi == '0') {
        mensaje = mensaje + "<li>Seleccione el equipo.</li>";
    }
    if (model.FechaInicio == '') {
        mensaje = mensaje + "<li>Ingrese la Fecha inicio.</li>";
    }
    else {
        if (model.HoraInicial == '') {
            mensaje = mensaje + "<li>Ingrese hora inicio.</li>";
        }
    }
    if (model.FechaFin == '') {
        mensaje = mensaje + "<li>Ingrese la Fecha fin.</li>";
    }
    else {
        if (model.HoraFinal == '') {
            mensaje = mensaje + "<li>Ingrese hora fin.</li>";
        }
    }
    if (model.IdTipoMantenimiento == '-1') {
        mensaje = mensaje + "<li>Seleccione Mantenimiento.";
    }
    if (model.NroDiaReplicar < 0) {
        mensaje = mensaje + "<li>No es un número de días adicionales válido.";
    }
    if (model.NroDiaReplicar > 30) {
        mensaje = mensaje + "<li>Solo es permitido un máximo de 30 días adicionales.";
    }

    //if (model.Comentario == '') {
    //    mensaje = mensaje + "<li>Ingrese Justificación.";
    //}

    if (mensaje != "") {
        mensaje = "<ul>" + mensaje + "</ul>";
    }

    return mensaje;
}

function obtenerHoraValida(hora) {
    if (hora !== undefined && hora != null) {
        hora = hora.replace('h', '0');
        hora = hora.replace('h', '0');

        hora = hora.replace('m', '0');
        hora = hora.replace('m', '0');

        hora = hora.replace('s', '0');
        hora = hora.replace('s', '0');
        return hora;
    }

    return '';
}

function actualizartxtHoraFin() {
    var fechaSiguiente = $("#hfFechaSiguiente").val();
    var fecha = $("#hfFechaActual").val();
    var hora = $("#txtNuevoFinH").val();
    if (hora == '00:00:00') {
        $("#txtNuevoFinF").val(fechaSiguiente);
    } else {
        $("#txtNuevoFinF").val(fecha);
    }
}

function setFechaSiguiente(fecha) {
    var parts = fecha.split("/");
    var date = new Date(parts[2], parts[1] - 1, parts[0]);
    var result = new Date(date);
    result.setDate(result.getDate() + 1);
    var fechaSiguiente = moment(result).format('DD/MM/YYYY');
    $("#hfFechaSiguiente").val(fechaSiguiente);
}

//Cambios múltiples
function abrirPopupMultiple() {
    $("#txt_motivo_cambio").val('');
    $('#txt_motivo_cambio').focus();

    listarCheckDeUsuario();

    //if (LISTA_CAMBIOS.length > 0 || LISTA_DELETE.length > 0) {
    //    $("#num_modif").html(LISTA_CAMBIOS.length);
    //    $("#num_eliminar").html(LISTA_DELETE.length);

    setTimeout(function () {
        $('#popupMultiple').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: true
        });
    }, 50);
    //} else {
    //    alert("No existen actualizaciones.");
    //}
}

function guardarClasificacion() {
    var observacion = $("#txt_motivo_cambio").val();
    observacion = observacion != null && observacion != undefined ? observacion.trim() : "";

    if (observacion != '') {
        if (confirm("¿Desea guardar los cambios efectuados?")) {

            var pericodi = parseInt($("#cbPeriodo").val()) || 0;

            var model = {
                listaUpdate: LISTA_CAMBIOS,
                listaDelete: LISTA_DELETE,
                observacion: observacion,

                tiposMantenimiento: $('#hfTipoMantenimiento').val(),
                pericodi: pericodi,
                indispo: $('#cbIndispo').val(),
                tiposEmpresa: $('#hfTipoEmpresa').val(), empresas: $('#hfEmpresa').val(),
                tiposEquipo: $('#hfTipoEquipo').val(), interrupcion: $('#cbInterrupcion').val(),
                tiposMantto: $('#hfTipoMantto').val(),
            };

            $.ajax({
                type: 'POST',
                contentType: 'application/json; charset=UTF-8',
                dataType: 'json',
                url: controlador + "ListadoManttoGuardar",
                data: JSON.stringify(model),
                success: function (evt) {
                    if (evt.Resultado != "-1") {
                        $('#popupMultiple').bPopup().close();
                        $('#mensaje').hide();

                        mostrarListado();
                    } else {
                        mostrarError("Error al Grabar: " + evt.Mensaje);
                    }
                },
                error: function (err) {
                    mostrarError('Ha ocurrido un error');
                }
            });
        }
    } else {
        alert("Debe ingresar Motivo de los cambios.");
    }
}

function listarCheckDeUsuario() {
    LISTA_DELETE = [];
    LISTA_CAMBIOS = [];

    $("tr.fila_dato").each(function () {

        var idFila = $(this).attr('id') + '';
        var codigo = idFila.substring(3, idFila.length);

        if (codigo != '_') {
            var mancodigo = parseInt($('#id_eve_' + codigo).val()) || 0;
            var indcodigo = parseInt($('#id_ind_' + codigo).val()) || 0;
            var tipoindisp = $('input[name="rbtnIndisponibilidad_' + codigo + '"]:checked').val();
            var pr = parseFloat($('#txt_prmw_' + codigo).val()) || 0;
            var asoc = $('#check_asocproc_' + codigo).is(':checked') ? "S" : "N";
            var omitir7d = $('#check_omitir7d_' + codigo).is(':checked') ? "S" : "N";
            var omitirexcesopr = $('#check_omitirexcesopr_' + codigo).is(':checked') ? "S" : "N";

            var bIncluir = $('#check_incluir_' + codigo).is(':checked') ? "S" : "N";
            var bExcluir = $('#check_excluir_' + codigo).is(':checked') ? "S" : "N";

            if (tipoindisp != "PP" && tipoindisp != "FP")
                pr = null;

            var entity = {
                Manttocodi: mancodigo != 0 ? mancodigo : null,
                Indmancodi: indcodigo > 0 ? indcodigo : null,
                Indmantipoindisp: tipoindisp,
                Indmanpr: pr > 0 ? pr : null,
                Indmanasocproc: asoc,
                Indmanomitir7d: omitir7d,
                Indmanomitirexcesopr: omitirexcesopr,
                TipoDelete: ''
            };

            if (bIncluir == "N" && bExcluir == "N") {
                LISTA_CAMBIOS.push(entity);
            } else {
                if (bIncluir == "S") {
                    entity.TipoDelete = 'I';
                    LISTA_DELETE.push(entity);
                }

                if (bExcluir == "S") {
                    entity.TipoDelete = 'E';
                    LISTA_DELETE.push(entity);
                }
            }
        }
    });

}

function verHistorialMantto(indmancodi, manttocodi) {
    $("#popupMantenimiento div.popup-title span").html('Historial de Cambios');
    $("#idPopupMantenimiento").html('');

    $.ajax({
        type: 'POST',
        url: controlador + "VerHistorialCambio",
        data: {
            indmancodi: indmancodi,
            manttocodi: manttocodi
        },
        success: function (dataHtml) {
            $('#idPopupMantenimiento').html(dataHtml);

            setTimeout(function () {
                $('#popupMantenimiento').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: true
                });

                $('#tabla_hist').dataTable({
                    scrollX: false,
                    scrollCollapse: false,
                    paging: false,
                    "sDom": 't',
                    ordering: false,
                });

            }, 50);

        },
        error: function (err) {
            alert('Ha ocurrido un error');
        }
    });

}

///
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
                        $('#cbPeriodo').get(0).options[$('#cbPeriodo').get(0).options.length] = new Option(item.Iperinombre, item.Ipericodi);
                    });
                } else {
                    $('#cbPeriodo').get(0).options[0] = new Option("--", "0");
                }

                //mostrarListado();
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}
function listadoRecalculo() {

    var ipericodi = parseInt($("#cbPeriodo").val()) || 0;

    $("#cbRecalculo").empty();

    $.ajax({
        type: 'POST',
        url: controlador + "RecalculoListado",
        data: {
            ipericodi: ipericodi,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.ListaRecalculo.length > 0) {
                    $.each(evt.ListaRecalculo, function (i, item) {
                        $('#cbRecalculo').get(0).options[$('#cbRecalculo').get(0).options.length] = new Option(item.Irecanombre, item.Irecacodi);
                    });
                } else {
                    $('#cbRecalculo').get(0).options[0] = new Option("--", "0");
                }

                getRecalculo();
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function getRecalculo() {
    var irecacodi = parseInt($("#cbRecalculo").val()) || 0;

    $(".td_filtro_fecha").hide();
    $("#td_estado_recalculo").html('');
    $(".td_procesar").hide();

    if (irecacodi > 0) {
        $.ajax({
            type: 'POST',
            url: controlador + "RecalculoDatosInsumo",
            data: {
                irecacodi: irecacodi,
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    $(".td_filtro_fecha").show();

                    $("#desc_fecha_ini").val(evt.FechaIni);
                    $("#desc_fecha_fin").val(evt.FechaFin);

                    var color = evt.IndRecalculo.Estado != "A" ? "red" : "blue";
                    $("#td_estado_recalculo").html('<span style="font-weight: bold;;color: ' + color + '">' + evt.IndRecalculo.IrecaestadoDesc + '</span>');

                    if (evt.IndRecalculo.Estado == "A")
                        $(".td_procesar").show();
                } else {
                    alert("Ha ocurrido un error: " + evt.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}

function actualizarReporte(cuacodi) {
    var irecacodi = parseInt($("#cbRecalculo").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + 'ActualizarReporte',
        data: {
            cuacodi: cuacodi,
            irecacodi: irecacodi
        },
        cache: false,
        success: function (result) {
            if (result.Resultado == '-1') {
                alert('Ha ocurrido un error:' + result.Mensaje);
            } else {
                alert("Se actualizó correctamente el reporte.");

            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}