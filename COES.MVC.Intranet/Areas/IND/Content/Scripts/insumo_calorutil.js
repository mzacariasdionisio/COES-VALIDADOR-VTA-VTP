var controlador = siteRoot + 'IND/DispCalorUtil/';
var RECURSO_CU = 4;
var VIENE_DE_CONSULTA = 1;
var listaCentral = [];
var tblError;

$(function () {
    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').easytabs('select', '#vistaDisponibilidad');

    $(".tabL").on("click", function () {
        armarTablaNoDisponibles();
    });

    $('#cbAnio').change(function () {
        listadoPeriodo();
    });

    $('#cbPeriodo').change(function () {
        cargarRevisiones();
    });

    $('#cbRevision').change(function () {
        cargarListado();
    });

    setearFechas();

    $('#cbEmpresa').change(function () {
        cargarListado();
    });

    $("#btnConsultar").click(function () {
        cargarListado(VIENE_DE_CONSULTA);
    });

    $("#btnGuardarData").click(function () {
        GuardarDataTabla();

    });

    $("#btnEditarData").click(function () {
        activarEdicionDeLaTabla(true)
    });

    $("#btnVerHistorial").click(function () {
        openPopup("historialCU");
    });

    $("#btnVerErrores").click(function () {
        var dataDispMin = listarDispoConMinutos();
        var dataInvalido = dataDispMin.filter(x => x.Pfcumin > 15 || x.Pfcumin < 1 || x.Pfcumin == '');
        cargarListaErrores(dataInvalido);
        openPopup("erroresCU");
    });

    tblError = $('#tblErrores').DataTable({
        "columns": [
            { "data": "Emprnomb" },
            { "data": "Central" },
            { "data": "PfcufechaDesc2" },
            { "data": "Pfcumin" },
            { "data": "Error" },
        ]
    });

    cargarListado();
});

/**
 * Muestra el listado
 */
function cargarListado(origen) {
    $("#mensaje").css("display", "none");
    origen = parseInt(origen) || 0;
    var msj = "";
    var obj = {};

    if (origen == VIENE_DE_CONSULTA) {
        obj = getObjetoJsonConsulta();
        msj = validarConsulta(obj);
    }

    if (msj == "") {

        $("#contenidoCalorUtil").hide();
        $("#tblCalorUtil").html('');
        var revision = parseInt($('#cbRevision').val()) || 0;

        $.ajax({
            url: controlador + "CargarLstCalorUtil",
            data: {
                verscodi: -2,
                strFechaIni: $("#fecha_ini").val(),
                hIni: $("#cbHoraIni").val(),
                strFechaFin: $("#fecha_fin").val(),
                hFin: $("#cbHoraFin").val(),
                recacodi: revision,
                empresas: $("#cbEmpresa").val()
            },
            type: 'POST',
            success: function (result) {
                if (result.Resultado === "-1") {
                    mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al cargar el listado de Calor Útil. Error: ' + result.Mensaje);
                    //mostrarMensaje('mensaje', 'error', 'Se ha producido un error: ' + result.Mensaje);
                } else {
                    listaCentral = result.ListaCentral;

                    $('#tab-container').easytabs('select', '#vistaDisponibilidad');
                    activarEdicionDeLaTabla(!result.TieneRegistroPrevio);

                    $("#contenidoCalorUtil").show();
                    $("#tblCalorUtil").html(result.Resultado);
                    $('#tabla_cu').dataTable({
                        "scrollX": true,
                        "scrollY": "400px",
                        "scrollCollapse": true,
                        "sDom": 't',
                        "ordering": false,
                        paging: false,
                    });

                    $('input[type=checkbox][name^="checkd_todo_"]').unbind();
                    $('input[type=checkbox][name^="checkd_todo_"]').change(function () {

                        var idchecktodo = $(this).attr('name') + '';
                        var codigo = idchecktodo.substring(12, idchecktodo.length); //equipadre_emprcodi
                        var valorCheck = $(this).prop('checked');

                        $("input[type=checkbox][id$=_" + codigo + "]").each(function () {
                            $("#" + this.id).prop("checked", valorCheck);
                            $("#" + this.id).trigger("change");
                        });
                    });

                    listadoVersion();
                    $('#hversion').val(result.Version);
                    if (result.NumVersion != null) {
                        $('#versnumero').text("Versión: " + result.NumVersion);
                    } else {
                        $('#versnumero').text("");
                    }
                }
            },
            error: function (xhr, status) {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al cargar el listado de Calor Útil.');
                //mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    } else {
        alert(msj);
    }
}

/**
 * Muestra el listado 
 */
function verPorVersion(verscodi) {
    $("#contenidoCalorUtil").hide();
    var revision = parseInt($('#cbRevision').val()) || 0;

    $.ajax({
        url: controlador + "CargarLstCalorUtil",
        data: {
            recacodi: revision,
            verscodi: verscodi,
            strFechaIni: $("#fecha_ini").val(),
            hIni: $("#cbHoraIni").val(),
            strFechaFin: $("#fecha_fin").val(),
            hFin: $("#cbHoraFin").val(),
            empresas: -1
        },
        type: 'POST',
        success: function (result) {
            if (result.Resultado === "-1") {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al cargar el listado de Calor Útil. Error: ' + result.Mensaje);
                //mostrarMensaje('mensaje', 'error', 'Se ha producido un error: ' + result.Mensaje);
            } else {
                $('#tab-container').easytabs('select', '#vistaDisponibilidad');
                activarEdicionDeLaTabla(!result.TieneRegistroPrevio);
                $('#historialCU').bPopup().close();

                $("#contenidoCalorUtil").show();
                $("#tblCalorUtil").html(result.Resultado);
                $('#tabla_cu').dataTable({
                    "scrollX": true,
                    "scrollY": "400px",
                    "scrollCollapse": true,
                    "sDom": 't',
                    "ordering": false,
                    paging: false,
                });

                $('input[type=checkbox][name^="checkd_todo_"]').unbind();
                $('input[type=checkbox][name^="checkd_todo_"]').change(function () {

                    var idchecktodo = $(this).attr('name') + '';
                    var codigo = idchecktodo.substring(12, idchecktodo.length); //equipadre_emprcodi
                    var valorCheck = $(this).prop('checked');

                    $("input[type=checkbox][id$=_" + codigo + "]").each(function () {
                        $("#" + this.id).prop("checked", valorCheck);
                        $("#" + this.id).trigger("change");
                    });
                });

                listadoVersion();
                $('#hversion').val(result.Version);
                if (result.NumVersion != null) {
                    $('#versnumero').text("Versión: " + result.NumVersion);
                } else {
                    $('#versnumero').text("");
                }
            }
        },
        error: function (xhr, status) {
            //mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error al cargar el listado de Calor Útil por versión.');

        }
    });
}

/**
 * Cambia de estado (A:APROBADO) a cierta versión, las demas pasan a estado (G:GENERADO)
 */
function aprobarVersion(verscodi, recursocodi, recacodi) {
    if (confirm('¿Desea aprobar la versión escogida?')) {
        $.ajax({
            url: controlador + "AprobarVersionInsumo",
            data: {
                verscodi: verscodi,
                recursocodi: recursocodi,
                recacodi: recacodi
            },
            type: 'POST',
            success: function (result) {
                if (result.Resultado === "-1") {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error al aprobar una versión');
                } else {
                    listadoVersion();

                    mostrarMensaje('mensaje', 'message', 'Cambios realizados con éxito.');
                }
            },
            error: function (xhr, status) {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

/**
 * Función que verifica cual de los dos botones (Guardar o Editar) debe mostrarse
 */
function activarEdicionDeLaTabla(esEditable) {
    if (esEditable) {
        $("#versionInsumo").hide();
        $("#btnEditarData").hide();
        $("#btnGuardarData").show();
        //habilitarModoEdicion(tablaFactorIndisponibilidad);

    } else {
        $("#btnEditarData").show();
        $("#btnGuardarData").hide();
        //habilitarModoSoloLectura(tablaFactorIndisponibilidad);
        $("#versionInsumo").show();
    }
}

function habilitarModoSoloLectura(hanson) {
    hanson.updateSettings({
        readOnly: true
    });
}

function habilitarModoEdicion(hanson) {
    hanson.updateSettings({
        readOnly: false
    });
}

/**
 * Abre los popUps existentes en la vista principal
 */
function openPopup(contentPopup) {
    setTimeout(function () {
        $("#" + contentPopup).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}

/**
 * Llena la lista de errores
 */
function registrarErrores(result) {
    if (!result.valid)
        listaErrorFI.push(result.error);
    return result.valid;
}


function armarTablaNoDisponibles() {

    var dataValido = listarNoCheckDeUsuario();

    var dataJson = {
        lstCu: dataValido,
        strFechaIni: $("#fecha_ini").val(),
        hIni: $("#cbHoraIni").val(),
        strFechaFin: $("#fecha_fin").val(),
        hFin: $("#cbHoraFin").val(),
        periodo: $('#txtPeriodo').val(),
        recacodi: $('#cbRevision').val(),
        empresas: $("#cbEmpresa").val(),
    };

    $.ajax({
        url: controlador + "CargarLIstaCalorUtilIndsipo",

        data: JSON.stringify(dataJson),
        type: 'POST',
        async: false,
        contentType: 'application/json; charset=UTF-8',
        success: function (result) {
            if (result.Resultado === "-1") {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error al momento de mostrar Listado de Centrales Sin Calor Útil.');
            } else {
                //textAlertCononfirmacion = result.Resultado;
                $('#listadoSinCU').html(result.Resultado);
            }
        },
        error: function (xhr, status) {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            //mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function cargarListaErrores(dataInvalido) {
    tblError.clear();
    tblError.rows.add(dataInvalido).draw();
}

/**
 * Guarda los datos proporcionados en la tabla handson
 */
function GuardarDataTabla() {

    var dataValido = listarNoCheckDeUsuario();
    var dataDispMin = listarDispoConMinutos();

    var data = $.merge(dataValido, dataDispMin);

    var dataInvalido = dataDispMin.filter(x => x.Pfcumin > 15 || x.Pfcumin < 1 || x.Pfcumin == '');

    if (dataInvalido.length > 0) {
        alert("Existen errores en la tabla");
        cargarListaErrores(dataInvalido);
        openPopup("erroresCU");
        return;
    }

    var dataJson = {
        lstCu: data,
        strFechaIni: $("#fecha_ini").val(),
        hIni: $("#cbHoraIni").val(),
        strFechaFin: $("#fecha_fin").val(),
        hFin: $("#cbHoraFin").val(),
        periodo: $('#txtPeriodo').val(),
        recacodi: $('#cbRevision').val(),
        empresas: $("#cbEmpresa").val(),
    };

    $.ajax({
        url: controlador + "GuardarDisponibilidadCalorUtil",
        data: JSON.stringify(dataJson),
        type: 'POST',
        contentType: 'application/json; charset=UTF-8',
        success: function (result) {
            if (result.Resultado === "-1") {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error al momento de guardar la información');
            } else {
                if (result.Resultado === "1") {
                    mostrarMensaje('mensaje', 'message', 'Los valores fueron guardados exitosamente.');

                    cargarListado();
                    activarEdicionDeLaTabla(false);
                }
            }
        },
        error: function (xhr, status) {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });

    //}


};

function listarNoCheckDeUsuario() {
    var listaData = [];

    $('input[type=checkbox][id^="checkdispcu_"]:not(:checked)').each(function () {

        var idCheck = $(this).attr('id') + '';
        var arr = idCheck.split('_');

        var idFila = arr[1];
        var idEquipadre = arr[2];
        var idEmpresa = arr[3];

        var strFecha = $("#hfTdFecha_" + idFila).val();
        var strH = $("#hfTdh_" + idFila).val();

        var entity = {
            Emprcodi: idEmpresa,
            Equipadre: idEquipadre,
            PfcufechaDesc: strFecha,
            Pfcuh: strH,
            Pfcutienedisp: 0,
            Pfcumin: null

        };

        listaData.push(entity);
    });

    $('input[type=checkbox][id^="checkdispcu_"]:is(:checked)').each(function () {

        var idCheck = $(this).attr('id') + '';
        var arr = idCheck.split('_');

        var idFila = arr[1];
        var idEquipadre = arr[2];
        var idEmpresa = arr[3];

        var strFecha = $("#hfTdFecha_" + idFila).val();
        var strH = $("#hfTdh_" + idFila).val();

        var txtMin = idCheck.replace("checkdispcu", "txtMin");
        var valMin = $("#" + txtMin).val();

        if (valMin !== '15') {

            var entity = {
                Emprcodi: idEmpresa,
                Equipadre: idEquipadre,
                PfcufechaDesc: strFecha,
                Pfcuh: strH,
                Pfcutienedisp: 1,
                Pfcumin: +valMin
            };

            listaData.push(entity);
        }

    });

    return listaData;
}

function listarDispoConMinutos() {
    var listaData = [];

    $('input[type=checkbox][id^="checkdispcu_"]:is(:checked)').each(function () {

        var idCheck = $(this).attr('id') + '';
        var arr = idCheck.split('_');

        var idFila = arr[1];
        var idEquipadre = arr[2];
        var idEmpresa = arr[3];

        var strFecha = $("#hfTdFecha_" + idFila).val();
        var strH = $("#hfTdh_" + idFila).val();

        var txtMin = idCheck.replace("checkdispcu", "txtMin");
        var valMin = $("#" + txtMin).val();

        if (valMin !== '15') {

            var entity = {
                Emprcodi: idEmpresa,
                Equipadre: idEquipadre,
                PfcufechaDesc: strFecha,
                Pfcuh: strH,
                Pfcutienedisp: 1,
                Pfcumin: valMin === '' ? valMin : +valMin
            };

            if (entity.Pfcumin > 15) {
                entity.Error = "El valor es mayor a 15 min."
            }

            if (entity.Pfcumin < 0) {
                entity.Error = "El valor es menor a 1 min."
            }

            if (entity.Pfcumin == '') {
                entity.Error = "El valor es vacío"
            }

            if (entity.Pfcumin > 15 || entity.Pfcumin < 0 || entity.Pfcumin == '') {
                var central = listaCentral.find(x => x.Equipadre == entity.Equipadre);
                entity.Central = central.Central;
                entity.Emprnomb = central.Emprnomb;
                entity.PfcufechaDesc2 = $("#" + idCheck).parent().parent().find('td:eq(0)').text();
            }



            listaData.push(entity);
        }

    });

    return listaData;
}

/**
 * Parametros de filtro por defecto
 */
function SetearFiltradoDefault() {
    $("#hversion").val('0');
    $('#cbEmpresa').val(-1);
}

function setearFechas() {
    $('#fecha_ini').unbind();
    $('#fecha_fin').unbind();

    var fechaIni = $("#hfFechaIni").val();
    var fechaFin = $("#hfFechaFin").val();

    $("#fecha_ini").val(fechaIni);
    $("#fecha_fin").val(fechaFin);

    $('#fecha_ini').Zebra_DatePicker({
        direction: [fechaIni, fechaFin],
        pair: $('#fecha_fin'),
        onSelect: function (date) {
            $('#fecha_fin').val(date);
        }
    });
    $('#fecha_fin').Zebra_DatePicker({
        direction: [fechaIni, fechaFin],
        onSelect: function (date) {
            setearFechasXFechaFin();
        }
    });
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
                        $('#cbPeriodo').get(0).options[$('#cbPeriodo').get(0).options.length] = new Option(item.Iperinombre, item.Ipericodi);
                    });
                } else {
                    $('#cbPeriodo').get(0).options[0] = new Option("--", "0");
                }

                cargarRevisiones();
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
};

function cargarRevisiones() {

    var indpericod = parseInt($("#cbPeriodo").val()) || 0;

    $("#cbRevision").empty();

    $.ajax({
        type: 'POST',
        url: controlador + "CargarRevisiones",
        data: {
            indpericodi: indpericod,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                $("#hfFechaIni").val(evt.FechaIni);
                $("#hfFechaFin").val(evt.FechaFin)
                setearFechas();

                if (evt.ListaRecalculo.length > 0) {
                    $.each(evt.ListaRecalculo, function (i, item) {
                        $('#cbRevision').get(0).options[$('#cbRevision').get(0).options.length] = new Option(item.Irecanombre, item.Irecacodi);
                    });
                } else {
                    $('#cbRevision').get(0).options[0] = new Option("--", "0");
                }

                cargarListado();
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}


/**
 * Lista en un popup las versiones existentes para el insumo segun tipo de insumo y revisión
 * */
function listadoVersion() {

    $('#listadoHCU').html('');

    var recurso = RECURSO_CU;     //CAMBIAR SEGUN INSUMO LA TABLA RECURSO 
    var recacodi = parseInt($("#cbRevision").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "VersionListado",
        data: {
            recurso: recurso,
            recacodi: recacodi,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#listadoHCU').html(evt.Resultado);

                $("#listadoHCU").css("width", (820) + "px");

                $('.tabla_version_x_recalculo').dataTable({
                    "sPaginationType": "full_numbers",
                    "destroy": "true",
                    "ordering": false,
                    "searching": true,
                    "iDisplayLength": -1,
                    "info": false,
                    "paging": false,
                    "scrollX": false,
                    "scrollY": "250px",
                });
            } else {
                //mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error al listar las versiones.');
                mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error al listar las versiones. Error:' + evt.Mensaje);
            }
        },
        error: function (err) {
            //mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
};


/**
 * Validar que existan todos los parametros al momento de hacer la consulta
 */
function validarConsulta(obj) {
    var msj = "";

    if (obj.consulta_empr == 0) {
        msj += "Debe ingresar una empresa correcta para realizar la consulta." + "\n";
    } else {
        if (obj.consulta_feci == 0) {
            msj += "Debe ingresar una fecha inicial correcta para realizar la consulta." + "\n";
        } else {
            if (obj.consulta_hori == 0) {
                msj += "Debe ingresar una hora inicial correcta para realizar la consulta." + "\n";
            } else {
                if (obj.consulta_fecf == 0) {
                    msj += "Debe ingresar una fecha final correcta para realizar la consulta." + "\n";
                } else {
                    if (obj.consulta_horf == 0) {
                        msj += "Debe ingresar una hora final correcta para realizar la consulta." + "\n";
                    } else {

                    }
                }
            }
        }
    }

    return msj;
}

/**
 * Parametros para consulta
 * */
function getObjetoJsonConsulta() {
    var obj = {};
    obj.consulta_revi = $("#cbRevision").val() || 0;
    obj.consulta_empr = $("#cbEmpresa").val() || 0;

    obj.consulta_feci = $("#fecha_ini").val() || 0;
    obj.consulta_hori = $("#cbHoraIni").val() || 0;
    obj.consulta_fecf = $("#fecha_fin").val() || 0;
    obj.consulta_horf = $("#cbHoraFin").val() || 0;

    return obj;
}

/**
 * Muestra mensajes de notificación
 */
function mostrarMensaje(id, tipo, mensaje) {
    //$('#' + id).removeClass();
    //$('#' + id).addClass('action-' + tipo);
    //$('#' + id).html(mensaje);

    alert(mensaje);
};

function mostrarMensaje_(id, tipo, mensaje) {
    $("#mensaje").css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};

function cambioDisponibilidad(ctrl) {

    var dispId = $(ctrl).attr('id');
    var txtMin = dispId.replace("checkdispcu", "txtMin");

    if ($(ctrl).is(':checked')) {
        var id = $("#" + txtMin);
        id.show();
        if (id.val() == '') id.val(15);

    } else {
        $("#" + txtMin).hide();
    }

}


function validarNumeroEntero(item, evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;

    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }

    return true;
}
