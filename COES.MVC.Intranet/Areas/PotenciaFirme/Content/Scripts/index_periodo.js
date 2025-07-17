var controlador = siteRoot + 'PotenciaFirme/Periodo/';
var ancho = 1000;
var GUARDAR = 0;
var EDITAR = 1;

var OBJ_NEW = null;

$(function () {
    $('#cntMenu').css("display", "none");

    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').bind('easytabs:after', function () {
        refrehDatatable();
    });

    $('#rec_fecha_limite').inputmask({
        mask: "1/2/y h:s",
        placeholder: "dd/mm/yyyy hh:mm",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#rec_fecha_limite').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#rec_fecha_limite').val(date + " 23:59");
        }
    });

    $('#ed_rec_fecha_limite').inputmask({
        mask: "1/2/y h:s",
        placeholder: "dd/mm/yyyy hh:mm",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#ed_rec_fecha_limite').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#ed_rec_fecha_limite').val(date + " 23:59");
        }
    });

    $('#tab-container').easytabs('select', '#vistaPeriodo');

    $("#btnNuevoRecalculo").click(function () {

        var ultimoRecalculo = OBJ_NEW;

        if (!ultimoRecalculo.tienePF && ultimoRecalculo.tipo != '') {

            alert("No existe cálculo de potencia firme para el último Recálculo");

        } else {
            popupRecalculoNuevo();
        }
    });

    $("#btnCrearRecalculo").click(function () {
        guardarRecalculo(GUARDAR);
    });
    $("#btnGenerarCalculo").click(function () {
        generarCalculoPF();
    });
    $("#btnEditarRecalculo").click(function () {
        guardarRecalculo(EDITAR);
    });

    $("#btnCancelarRecalculo").click(function () {
        $('#popupRecalculo').bPopup().close();
    });
    $("#btnCancelarCalculo").click(function () {
        $('#popupCalculoPF').bPopup().close();
    });
    $("#btnCancelarERecalculo").click(function () {
        $('#popupEditarRecalculo').bPopup().close();
    });


    $('input:radio[name=tipo_recalculo]').change(function () {
        setearTextoTipo();
    });


    $(document).on('change', 'input:radio[name=tipo_recalculo]', function () {
        setearTextoTipo();
    });

    mostrarListadoPeriodos();

});

///////////////////////////

/// Formulario Periodo
///////////////////////////

/** Metodo que muestra el listado de los periodos */
function mostrarListadoPeriodos() {
    $('#listado').html('');
    $('#rec_listado').html('');
    $("#div_recalculo").hide();

    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 1100;

    $('#tab-container').easytabs('select', '#vistaPeriodo');

    $.ajax({
        type: 'POST',
        url: controlador + "PeriodoListado",
        data: {},
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#listado').html(evt.Resultado);

                var anchoReporte = $('#tabla_periodo').width();
                $("#listado").css("width", (ancho) + "px");
                var altotabla = parseInt($('#listado').height()) || 0;
                $('#tabla_periodo').dataTable({
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
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
};

function refrehDatatable() {
    var altotabla = parseInt($('#listado').height()) || 0;
    $('#tabla_periodo').dataTable({
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

    $('#tabla_recalculo').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "ordering": false,
        "searching": false,
        "iDisplayLength": 15,
        "info": false,
        "paging": false,
        "scrollX": true,
        "scrollY": $('#rec_listado').height() > 355 ? 355 + "px" : "100%"
    });
}

///////////////////////////
/// Formulario Recalculo
///////////////////////////

function verListadoRecalculo(pericodi) {

    $("#div_recalculo").hide();
    $("#div_reporte").hide();
    $(".rec_periodo").val('');
    $("#rec_anio").val('');
    $("#rec_mes").val('');
    $("#rec_periodo_escogido").val('');


    $("#rec_recacodi").val(0);

    $('#tab-container').easytabs('select', '#vistaRecalculo');

    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 1100;

    $.ajax({
        type: 'POST',
        url: controlador + "RecalculoListado",
        data: {
            pericodi: pericodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $("#div_recalculo").show();
                //$("#div_reporte").show();
                $("#mensaje_reportes").show();

                $('#rec_listado').html(evt.Resultado);
                $(".rec_periodo").text(evt.PfPeriodo.Pfperinombre);
                $("#rec_pericodi").val(pericodi);
                $("#rec_anio").val(evt.PfPeriodo.Pfperianio);
                $("#rec_mes").val(evt.PfPeriodo.Pfperimes);
                $("#rec_periodo_escogido").val(evt.PeriodoActual);

                OBJ_NEW = {
                    anio: evt.PfPeriodo.Pfperianio,
                    mes: evt.PfPeriodo.Pfperimes,
                    tipo: evt.TipoRecalculo,
                    tienePF: evt.TieneReportePF,
                    listaIND: evt.ListaIndRecalculo
                };

                //inicializar popup registro
                $(".rec_nombre_periodo").val(evt.PfPeriodo.Pfperimes + "-" + evt.PfPeriodo.Pfperianio);

                //
                var anchoReporte = $('#tabla_recalculo').width();
                $("#rec_listado").css("width", (ancho) + "px");

                $('#tabla_recalculo').dataTable({
                    "sPaginationType": "full_numbers",
                    "destroy": "true",
                    "ordering": false,
                    "searching": false,
                    "iDisplayLength": 15,
                    "info": false,
                    "paging": false,
                    "scrollX": true,
                    "scrollY": $('#rec_listado').height() > 400 ? 400 + "px" : "100%"
                });
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
};

function guardarRecalculo(tipo) {
    var obj = {};
    var estado = "";
    if (tipo == GUARDAR) {
        obj = getObjetoJsonRecalculo();
        estado = "A";
    }
    else {
        obj = getObjetoJsonEditarRecalculo();
        estado = obj.estado;
    }

    if (confirm('¿Desea guardar el recálculo?')) {
        var msj = validarRecalculo(obj);

        if (msj == "") {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                traditional: true,
                url: controlador + 'RecalculoGuardar',
                data: {
                    recacodi: obj.pfrecacodi,
                    pericodi: obj.pfpericodi,
                    irecacodi: obj.irecacodi,
                    comentario: obj.comentario,
                    nombre: obj.nombreRecalculo,
                    revisionInd: obj.revisionInd,
                    strFechaLimite: obj.fechaLimite,
                    informe: obj.informe,
                    tipo: obj.tipo
                },
                cache: false,
                success: function (result) {
                    if (result.Resultado == '-1') {
                        alert('Ha ocurrido un error:' + result.Mensaje);
                    } else {
                        if (result.Mensaje == "" || result.Mensaje == null) {
                            if (tipo == GUARDAR) {
                                alert(obj.nombreRecalculo + " del periodo " + $(".rec_nombre_periodo").val() + "  ha sido generada exitosamente.");
                                $('#popupRecalculo').bPopup().close();
                            } else {
                                alert(obj.nombreRecalculo + " del periodo " + $(".rec_nombre_periodo").val() + "  ha sido editado exitosamente.");
                                $('#popupEditarRecalculo').bPopup().close();
                            }

                            mostrarListadoPeriodos();
                            verListadoRecalculo(obj.pfpericodi);
                        }
                        else {
                            alert(result.Mensaje);
                        }
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

function validarInsumosCalculo(obj) {
    var msj = "";

    if (obj.version_pg <= 0 || obj.version_pa <= 0 || obj.version_fi <= 0 || obj.version_fp <= 0) {
        msj += "Debe ingresar todos los insumos." + "\n";
    }

    return msj;
}

function validarRecalculo(obj) {
    var msj = "";
    if (obj.nombreRecalculo == null || obj.nombreRecalculo.trim() == "") {
        msj += "Debe ingresar nombre de la revisión." + "\n";
    }

    if (obj.pfpericodi <= 0) {
        msj += "Debe seleccionar un periodo." + "\n";
    }

    if (obj.irecacodi <= 0) {
        msj += "Debe seleccionar un recálculo de Indisponibilidade." + "\n";
    }

    return msj;
}

function editarRecalculo(recacodi) {
    $("#rec_recacodi").val(0);

    $.ajax({
        type: 'POST',
        url: controlador + 'RecalculoObjeto',
        data: {
            recacodi: recacodi
        },
        cache: false,
        success: function (evt) {
            if (evt.Resultado == '-1') {
                alert('Ha ocurrido un error: ' + evt.Mensaje);
            } else {
                var obj_recalculo = evt.PfRecalculo;

                $("#rec_recacodi").val(obj_recalculo.Pfrecacodi);

                $("#ed_rec_nombre_revision").val(obj_recalculo.Pfrecanombre);
                $("#ed_rec_comentario").val(obj_recalculo.Pfrecadescripcion);
                $("#ed_rec_fecha_limite").val(obj_recalculo.PfrecafechalimiteDesc);
                //$('input:radio[name=ed_estado_recalculo]').val([obj_recalculo.Pfrecaestado]);

                $('input:radio[name=ed_tipo_recalculo]').val([obj_recalculo.Pfrecatipo]);
                $("#ed_rec_informe").val(obj_recalculo.Pfrecainforme);
                setHtmlTipoRecalculo(obj_recalculo.Pfrecatipo, 0, evt.ListaIndRecalculo, obj_recalculo.Irecacodi);

                setTimeout(function () {
                    $('#popupEditarRecalculo').bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown'
                    });
                }, 50);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function verRecalculo(recacodi) {
    $.ajax({
        type: 'POST',
        url: controlador + 'RecalculoObjeto',
        data: {
            recacodi: recacodi
        },
        cache: false,
        success: function (evt) {
            if (evt.Resultado == '-1') {
                alert('Ha ocurrido un error: ' + evt.Mensaje);
            } else {

                $("#ver_rec_nombre_periodo").html(evt.PfPeriodo.Pfperimes + "-" + evt.PfPeriodo.Pfperianio);
                $("#ver_rec_nombre_revision").html(evt.PfRecalculo.Pfrecanombre);
                $("#ver_rec_comentario").html(evt.PfRecalculo.Pfrecadescripcion);
                $("#ver_rec_fecha_limite").html(evt.PfRecalculo.PfrecafechalimiteDesc);

                $("#ver_estado_recalculo").html(evt.PfRecalculo.Pfrecaestado == "A" ? "Abierto" : "Cerrado");
                $("#ver_rec_modif_usuario").html(evt.PfRecalculo.UltimaModificacionUsuarioDesc);
                $("#ver_rec_modif_fecha").html(evt.PfRecalculo.UltimaModificacionFechaDesc);

                setTimeout(function () {
                    $('#popupVerRecalculo').bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown'
                    });
                }, 50);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function generarCalculoPF() {
    var obj = {};
    obj = getObjetoJsonCalculo();
    var msj = validarInsumosCalculo(obj);


    if (msj == "") {
        var recacodi = $("#hfPfRecacodi").val();
        if (confirm('¿Desea generar el cálculo de Potencia Firme?')) {

            $.ajax({
                type: 'POST',
                url: controlador + 'GenerarCalculoPF',
                data: {
                    versionPG: obj.version_pg,
                    versionPA: obj.version_pa,
                    versionFI: obj.version_fi,
                    versionFP: obj.version_fp,
                    versionCCV: obj.version_ccv,
                    recacodi: recacodi
                },
                success: function (result) {
                    if (result.Resultado == '-1') {
                        alert('Ha ocurrido un error:' + result.Mensaje);
                    } else {
                        if (result.Resultado == '1') {
                            $('#popupCalculoPF').bPopup().close();
                            alert("El cálculo de la Potencia Firme ha sido generada exitosamente.");
                            //mostrarListadoPeriodos();
                            verListadoRecalculo(obj.pfpericodi);
                        }
                    }
                },
                error: function (err) {
                    alert('Ha ocurrido un error.');
                }
            });

        }

    } else {
        alert(msj);
    }
}

function descargarReporte(numReporte) {

    var reporteCodi = $("#hfMiRptCodi").val();
    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoPFirme",
        data: {
            pfrtcodi: reporteCodi,
            tipoReporte: numReporte
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.Resultado == "0")
                    alert(evt.Mensaje);
                else
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

function ObtenerInsumosParaCalculoPF(pericodi, recacodi) {

    $.ajax({
        type: 'POST',
        url: controlador + '/CargarRevisionesInsumos',
        data: {
            pericodi: pericodi,
            recacodi: recacodi
        },
        success: function (aData) {
            openPopup("popupCalculoPF");
            $('#cuerpoProcesar').html(aData);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });

}

function verListadoReporte(recacodi) {
    $("#div_reporte").hide();
    $(".rec_recalculo").html('');

    $.ajax({
        type: 'POST',
        url: controlador + 'RecalculoObjeto',
        data: {
            recacodi: recacodi
        },
        cache: false,
        success: function (evt) {
            if (evt.Resultado == '-1') {
                alert('Ha ocurrido un error: ' + evt.Mensaje);
            } else {
                $('#tab-container').easytabs('select', '#vistaReporte');
                $("#mensaje_reportes").hide();

                $("#div_reporte").show();
                $(".rec_periodo").text(evt.PfPeriodo.Pfperimes + "-" + evt.PfPeriodo.Pfperianio);
                $("#rec_recacodi").val(evt.PfRecalculo.Pfrecacodi);
                $(".rec_recalculo").html(evt.PfRecalculo.Pfrecanombre);
                $("#hfMiRptCodi").val(evt.IdReporte);

            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function getObjetoJsonCalculo() {
    var obj = {};

    obj.pfpericodi = parseInt($("#rec_pericodi").val()) || 0;
    obj.version_pg = $("#ins_revision_pg").val() || 0;
    obj.version_pa = $("#ins_revision_pa").val() || 0;
    obj.version_fi = $("#ins_revision_fi").val() || 0;
    obj.version_fp = $("#ins_revision_fp").val() || 0;
    obj.version_ccv = $("#ins_revision_ccv").val() || 0;

    return obj;
}

function getObjetoJsonRecalculo() {
    var obj = {};

    obj.pfpericodi = parseInt($("#rec_pericodi").val()) || 0;
    obj.pfrecacodi = parseInt($("#rec_recacodi").val()) || 0;
    obj.nombreRecalculo = $("#rec_nombre_revision").val();
    obj.comentario = $("#rec_comentario").val();
    obj.revisionInd = $("#rec_revision_ind").val();
    obj.fechaLimite = $("#rec_fecha_limite").val();
    //obj.estado = $('input[name=estado_recalculo]:checked').val();
    obj.informe = $("#rec_informe").val();
    obj.tipo = $('input[name=tipo_recalculo]:checked').val();
    obj.irecacodi = $("#rec_revision_ind").val();

    return obj;
}

function getObjetoJsonEditarRecalculo() {
    var obj = {};

    obj.pfpericodi = parseInt($("#rec_pericodi").val()) || 0;
    obj.pfrecacodi = parseInt($("#rec_recacodi").val()) || 0;
    obj.nombreRecalculo = $("#ed_rec_nombre_revision").val();
    obj.comentario = $("#ed_rec_comentario").val();
    obj.revisionInd = "";
    obj.fechaLimite = $("#ed_rec_fecha_limite").val();
    //obj.estado = $('input[name=ed_estado_recalculo]:checked').val();
    obj.informe = $("#ed_rec_informe").val();
    obj.tipo = $('input[name=ed_tipo_recalculo]:checked').val();
    obj.irecacodi = $("#ed_rec_revision_ind").val();

    return obj;
}

function setearFechasXFechaFin() {
    var fec1 = '';

    var periodo = $('#rec_periodo_escogido').val();
    fechaIni = moment(periodo, 'MM YYYY').toDate();
    fechaFin = moment(fechaIni).endOf('month').toDate();
    fechaFinMes = moment(fechaFin).format('YYYY/MM/DD');

    fec1 = obtenerDiaSiguiente(fechaFinMes, 3) + " 23:59";

    $("#rec_fecha_limite").val(fec1);
}

function obtenerDiaSiguiente(sFecha, numDia) { //sFecha, formato YYYY/MM/DD STRING
    checkdateFrom = convertStringToDate(sFecha, "00:00:00");
    horaFin01 = moment(checkdateFrom).set('date', moment(checkdateFrom).get('date') + numDia);

    return moment(horaFin01).format('DD/MM/YYYY');
}

//convierte 2 cadenas de texto fecha(dd/mm/yyyy) y horas(hh:mm:ss) a tipo Date
function convertStringToDate(fecha, horas) {
    var partsFecha = fecha.split('/');
    horas = obtenerHoraValida(horas);
    if (horas == "") {
        return "";
    }
    var partsHoras = horas.split(':');
    //new Date(yyyy, mm-1, dd, hh, mm, ss);
    return new Date(partsFecha[0], partsFecha[1] - 1, partsFecha[2], partsHoras[0], partsHoras[1], partsHoras[2]);
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

function popupRecalculoNuevo() {
    $("#rec_recacodi").val(0);
    $("#rec_nombre_revision").val('');
    $("#rec_comentario").val('');

    $("#popupRecalculo .popup-title span").html('Nuevo recálculo');

    setearFechasXFechaFin();
    var evt = OBJ_NEW;

    $("#rec_informe").val("INFORME COES/D/DO/SME-INF-0XX-" + evt.anio);


    setHtmlTipoRecalculo(evt.tipo, 1, evt.listaIND, 0);
    setearTextoTipo();

    setTimeout(function () {
        $('#popupRecalculo').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 50);
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


function setHtmlTipoRecalculo(tipo, modo, listaIND, irecacodi) {
    var html = '';
    if (modo == 1) { //nuevo
        if (tipo == null || tipo == "")
            html = `
                    <input type='radio' name='tipo_recalculo' value='M' id='mensual'> <label for="mensual">Mensual</label><br>
                    <input type='radio' name='tipo_recalculo' value='R1' id='revision'> <label for="revision">Revisión 01</label>
                        `;

        if (tipo == "PM")
            html = `<input type='radio' name='tipo_recalculo' value='M' id='mensual' checked='checked'> <label for="mensual">Mensual</label><br>
                    <input type='radio' name='tipo_recalculo' value='R1' id='revision'> <label for="revision">Revisión 01</label>
                        `;

        if (tipo == "M")
            html = `<input type='radio' name='tipo_recalculo' value='R1' id='revision' checked='checked'> <label for="revision">Revisión 01</label>`;

        if (tipo && tipo.substring(0, 1) == 'R') {
            var numR = parseInt(tipo.substring(1, 2)) + 1;
            html = `
                    <input type='radio' name='tipo_recalculo' value='R${numR}' id='revision' checked='checked'> <label for="revision">Revisión 0${numR}</label>
                        `;
        }

        $("#td_tipo_recalculo").html(html);

    } else { //editar
        if (tipo == "M")
            html = `<input type='radio' name='ed_tipo_recalculo' value='M' id='mensual' checked='checked'> <label for="mensual">Mensual</label>`;

        if (tipo && tipo.substring(0, 1) == 'R') {
            var numR = parseInt(tipo.substring(1, 2));
            html = `
                    <input type='radio' name='ed_tipo_recalculo' value='R${numR}' id='revision' checked='checked'> <label for="revision">Revisión 0${numR}</label>
                        `;
        }

        $("#td_ed_tipo_recalculo").html(html);
    }

    //combo recalculo Indisponibilidades
    var idSelect = modo == 1 ? "#rec_revision_ind" : "#ed_rec_revision_ind";
    $(idSelect).empty();
    if (listaIND.length > 0) {
        $.each(listaIND, function (i, item) {
            $(idSelect).get(0).options[$(idSelect).get(0).options.length] = new Option(item.Irecanombre, item.Irecacodi);
        });
    } else {
        $(idSelect).get(0).options[0] = new Option("--SELECCIONE--", "0");
    }

    if (modo != 1) //editar
        $(idSelect).val(irecacodi);
}


function setearTextoTipo() {
    var tipo = $('input[name=tipo_recalculo]:checked').val();

    //nombre
    if (tipo == "M")
        $("#rec_nombre_revision").val("Mensual");

    if (tipo !== undefined && tipo.substring(0, 1) == 'R') {
        var numR = tipo.substring(1, 2);
        $("#rec_nombre_revision").val("Revisión 0" + numR);
    }
}

