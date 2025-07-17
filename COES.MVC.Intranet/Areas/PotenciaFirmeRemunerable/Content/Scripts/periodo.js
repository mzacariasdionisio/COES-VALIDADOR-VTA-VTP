var controlador = siteRoot + 'PotenciaFirmeRemunerable/Periodo/';
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

        //if (!ultimoRecalculo.tienePF && ultimoRecalculo.tipo != '') {
            
            //mostrarMensaje('mensaje', 'error', 'No existe cálculo de potencia firme para el último Recálculo');

        //} else {
            popupRecalculoNuevo();
        //}
    });

    $("#btnCrearRecalculo").click(function () {
        guardarRecalculo(GUARDAR);
    });
    $("#btnGenerarCalculo").click(function () {
        generarCalculoPFR();
    });
    $("#btnEditarRecalculo").click(function () {
        guardarRecalculo(EDITAR);
    });

    $("#btnCancelarRecalculo").click(function () {
        $('#popupRecalculo').bPopup().close();
    });
    $("#btnCancelarCalculo").click(function () {
        $('#popupCalculoPFR').bPopup().close();
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

/////////////////////////////
//    Formulario Periodo
/////////////////////////////

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
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error: ' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
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
                $(".rec_periodo").text(evt.PfrPeriodo.Pfrpernombre);
                $("#rec_pericodi").val(pericodi);
                $("#rec_anio").val(evt.PfrPeriodo.Pfrperanio);
                $("#rec_mes").val(evt.PfrPeriodo.Pfrpermes);
                $("#rec_periodo_escogido").val(evt.PeriodoActual);

                OBJ_NEW = {
                    anio: evt.PfrPeriodo.Pfrperanio,
                    mes: evt.PfrPeriodo.Pfrpermes,
                    tipo: evt.TipoRecalculo,
                    tienePF: evt.TieneReportePF
                };

                //inicializar popup registro
                $(".rec_nombre_periodo").val(evt.PfrPeriodo.Pfrpermes + "-" + evt.PfrPeriodo.Pfrperanio);

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
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error: ' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
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
    
    var msj = validarRecalculo(obj);
    if (msj == "") {
        if (confirm('¿Desea guardar el recálculo?')) {


            
            $.ajax({
                type: 'POST',
                dataType: 'json',
                traditional: true,
                url: controlador + 'RecalculoGuardar',
                data: {
                    recacodi: obj.pfrreccodi,
                    pericodi: obj.pfrpercodi,
                    comentario: obj.comentario,
                    nombre: obj.nombreRecalculo,
                    //revisionPf: obj.revisionPf,
                    strFechaLimite: obj.fechaLimite,
                    informe: obj.informe,
                    tipo: obj.tipo
                },
                cache: false,
                success: function (result) {
                    if (result.Resultado == '-1') {
                        mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error: ' + result.Mensaje);
                    } else {
                        if (result.Mensaje == "" || result.Mensaje == null) {

                            mostrarMensaje('mensaje', 'error', 'La ' + obj.nombreRecalculo + ' del periodo ' + $(".rec_nombre_periodo").val() + '  ha sido generada exitosamente.');
                            mostrarListadoPeriodos();
                            verListadoRecalculo(obj.pfrpercodi);
                        }
                        else {
                            mostrarMensaje('mensaje', 'error', result.Mensaje);
                        }
                        $('#popupRecalculo').bPopup().close();
                        $('#popupEditarRecalculo').bPopup().close();


                    }
                },
                error: function (err) {
                    mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                }
            });
            
        }
    }
    else {
        mostrarMensaje('mensaje', 'error', msj);
    }
}

function validarRecalculo(obj) {     
    var msj = "";
    if (obj.nombreRecalculo == null || obj.nombreRecalculo.trim() == "") {
        msj += "Debe ingresar nombre de la revisión." + "\n";
    }    

    if (obj.pfrpercodi <= 0) {
        msj += "Debe seleccionar un periodo." + "\n";
    }

    //campos obligatorios
    if (obj.informe == null || obj.informe.trim() == "") {
        msj += "Debe ingresar el informe." + "\n";
    }
    if (obj.fechaLimite == null || obj.fechaLimite.trim() == "") {
        msj += "Debe ingresar una fecha límite para elcalculo de Potencia Firme Remunerable." + "\n";
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
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error: ' + evt.Mensaje);
            } else {
                var obj_recalculo = evt.PfrRecalculo;

                $("#rec_recacodi").val(obj_recalculo.Pfrreccodi);

                $("#ed_rec_nombre_revision").val(obj_recalculo.Pfrrecnombre);
                $("#ed_rec_comentario").val(obj_recalculo.Pfrrecdescripcion);
                $("#ed_rec_fecha_limite").val(obj_recalculo.PfrrecfechalimiteDesc);

                $('input:radio[name=ed_tipo_recalculo]').val([obj_recalculo.Pfrrectipo]);
                $("#ed_rec_informe").val(obj_recalculo.Pfrrecinforme);
                setHtmlTipoRecalculo(obj_recalculo.Pfrrectipo, 0);

                openPopup("popupEditarRecalculo");
                //setTimeout(function () {
                //    $('#popupEditarRecalculo').bPopup({
                //        easing: 'easeOutBack',
                //        speed: 450,
                //        transition: 'slideDown'
                //    });
                //}, 50);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
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
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error: ' + evt.Mensaje);
            } else {
                var obj_periodo = evt.PfrPeriodo;
                var obj_recalculo = evt.PfrRecalculo;

                $("#ver_rec_nombre_periodo").html(obj_periodo.Pfrpermes + "-" + obj_periodo.Pfrperanio);
                $("#ver_rec_nombre_revision").html(obj_recalculo.Pfrrecnombre);
                $("#ver_rec_comentario").html(obj_recalculo.Pfrrecdescripcion);
                $("#ver_rec_fecha_limite").html(obj_recalculo.PfrrecfechalimiteDesc);

                $("#ver_estado_recalculo").html(obj_recalculo.Estado == "A" ? "Abierto" : "Cerrado");
                $("#ver_rec_modif_usuario").html(obj_recalculo.UltimaModificacionUsuarioDesc);
                $("#ver_rec_modif_fecha").html(obj_recalculo.UltimaModificacionFechaDesc);

                
                openPopup("popupVerRecalculo");
                //setTimeout(function () {
                //    $('#popupVerRecalculo').bPopup({
                //        easing: 'easeOutBack',
                //        speed: 450,
                //        transition: 'slideDown'
                //    });
                //}, 50);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function popupRecalculoNuevo() {
    $("#rec_recacodi").val(0);
    $("#rec_nombre_revision").val('');
    $("#rec_comentario").val('');

    $("#popupRecalculo .popup-title span").html('Nuevo recálculo');

    setearFechasXFechaFin();
    var evt = OBJ_NEW;

    $("#rec_informe").val("INFORME COES/D/DO/SME-INF-0XX-" + evt.anio);


    setHtmlTipoRecalculo(evt.tipo, 1);
    setearTextoTipo();


    openPopup("popupRecalculo");
    
}

function setearFechasXFechaFin() {
    var fec1 = '';

    var periodo = $('#rec_periodo_escogido').val();
    fechaIni = moment(periodo, 'MM YYYY').toDate();
    fechaFin = moment(fechaIni).endOf('month').toDate();
    fechaFinMes = moment(fechaFin).format('YYYY/MM/DD');

    fec1 = obtenerDiaSiguiente(fechaFinMes, 10) + " 23:59";

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

function setHtmlTipoRecalculo(tipo, modo) {
    var html = '';
    if (modo == 1) { //nuevo
        if (tipo == null )
            html = `
                    <input type='radio' name='tipo_recalculo' value='M' id='mensual'> <label for="mensual">Mensual</label><br>
                    <input type='radio' name='tipo_recalculo' value='R1' id='revision'> <label for="revision">Revisión 01</label>
                        `;

        if (tipo == "")
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


function getObjetoJsonRecalculo() {
    var obj = {};

    obj.pfrpercodi = parseInt($("#rec_pericodi").val()) || 0;
    obj.pfrreccodi = parseInt($("#rec_recacodi").val()) || 0;
    obj.nombreRecalculo = $("#rec_nombre_revision").val();
    obj.comentario = $("#rec_comentario").val();
    obj.fechaLimite = $("#rec_fecha_limite").val();
    obj.informe = $("#rec_informe").val();
    obj.tipo = $('input[name=tipo_recalculo]:checked').val();

    return obj;
}

function getObjetoJsonEditarRecalculo() {
    var obj = {};

    obj.pfrpercodi = parseInt($("#rec_pericodi").val()) || 0;
    obj.pfrreccodi = parseInt($("#rec_recacodi").val()) || 0;
    obj.nombreRecalculo = $("#ed_rec_nombre_revision").val();
    obj.comentario = $("#ed_rec_comentario").val();
    obj.fechaLimite = $("#ed_rec_fecha_limite").val();
    obj.informe = $("#ed_rec_informe").val();
    obj.tipo = $('input[name=ed_tipo_recalculo]:checked').val();

    return obj;
}

function mostrarMensaje(id, tipo, mensaje) {
    alert(mensaje);
};

function mostrarMensaje_(id, tipo, mensaje) {
    $("#mensaje").css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};

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
        async: false,
        success: function (evt) {
            if (evt.Resultado == '-1') {
                alert('Ha ocurrido un error: ' + evt.Mensaje);
            } else {


                $('#tab-container').easytabs('select', '#vistaReporte');
                $("#mensaje_reportes").hide();

                $("#div_reporte").show();
                $(".rec_periodo").text(evt.PfrPeriodo.Pfrpermes + "-" + evt.PfrPeriodo.Pfrperanio);
                $("#rec_recacodi").val(evt.PfrRecalculo.Pfrreccodi);
                $(".rec_recalculo").html(evt.PfrRecalculo.Pfrrecnombre);
                $("#hfMiRptCodi").val(evt.IdReporte);
                $("#hfMiRptCodiDatos").val(evt.IdReporteDatos);

            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });

    mostrarReporte();

}

function descargarReporte(numReporte) {

    var pfrreccodi = $("#rec_recacodi").val();
    var reporteCodi = $("#hfMiRptCodi").val();
    var reporteCodiDatos = $("#hfMiRptCodiDatos").val();
    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoPFirmeRemunerable",
        data: {
            Pfrrptcodi: reporteCodi,
            PfrrptcodiDatos: reporteCodiDatos,
            tipoReporte: numReporte,
            pfrreccodi: pfrreccodi
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

function descargarReporteSalida() {

    var pfrreccodi = $("#rec_recacodi").val();
    $.ajax({
        type: 'POST',
        url: controlador + "DescargarArchivosGams",
        data: {
            pfrreccodi: pfrreccodi
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

function descargarReporteLVTPOPF(numReporte, numEscenario) {

    var pfrreccodi = $("#rec_recacodi").val();

    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoExcelLVTPOPF",
        data: {
            tipoReporte: numReporte,
            pfrreccodi: pfrreccodi,
            numEscenario: numEscenario
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

function mostrarReporte() {

    var pfrreccodi = $("#rec_recacodi").val();
    $.ajax({
        type: 'POST',
        url: controlador + "ReportesSalida",
        data: {
            pfrreccodi: pfrreccodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                $("#tblLvtpPpf").html(evt.Resultado);

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

function ObtenerInsumosParaCalculoPFR(pfrpercodi_, pfrreccodi_) {
     
    $.ajax({
        type: 'POST',
        url: controlador + '/CargarVentanaProcesar',
        data: {
            pfrpercodi: pfrpercodi_,
            pfrreccodi: pfrreccodi_
        },
        success: function (aData) {
            openPopup("popupCalculoPFR");
            $('#cuerpoProcesar').html(aData);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });

}

function generarCalculoPFR() {
    var obj = {};
    obj = getObjetoJsonCalculoPFR();

    var msj = validarDataCalculoPFR(obj);


    if (msj == "") {

        if (confirm('¿Desea generar el cálculo de Potencia Firme Remunerable?')) {

            $.ajax({
                type: 'POST',
                url: controlador + 'GenerarCalculoPFR',
                data: {
                    pfrreccodi: obj.pfrreccodi,
                    pfrecacodi: obj.recalculoPf,
                    indrecacodiant: obj.recalculoIndMesAnterior,
                    recpotcodi: obj.recalculoVTP
                },
                success: function (result) {
                    if (result.Resultado == '-1') {
                        alert('Ha ocurrido un error:' + result.Mensaje);
                    } else {
                        if (result.Resultado == '1') {
                            $('#popupCalculoPFR').bPopup().close();
                            alert("El cálculo de la Potencia Firme Remunerable ha sido generada exitosamente.");

                            verListadoRecalculo(obj.pfrpercodi);
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



function getObjetoJsonCalculoPFR() {
    var obj = {};

    obj.pfrreccodi = parseInt($("#hfInsPfrReccodi").val()) || 0;
    obj.pfrpercodi = parseInt($("#rec_pericodi").val()) || 0;

    obj.recalculoPf = $("#ins_recalculo_pf").val() || 0;
    obj.recalculoIndMesAnterior = $("#ins_recalculo_ind_mes_anterrior").val() || 0;
    obj.recalculoVTP = $("#ins_recalculo_vtp").val() || 0;

    obj.valorCR = $("#hfValorCR").val() || 0;
    obj.valorCA = $("#hfValorCA").val() || 0;
    obj.valorMR = $("#hfValorMR").val() || 0;

    return obj;
}


function validarDataCalculoPFR(obj) {
    var msj = "";
    if (obj.recalculoPf == null || obj.recalculoPf == 0) {
        msj += "Debe ingresar una revision de Potencia Firme." + "\n";
    }

    if (obj.recalculoIndMesAnterior == null || obj.recalculoIndMesAnterior == 0) {
        msj += "Debe ingresar una revision de Indisponibilidades." + "\n";
    }

    if (obj.recalculoVTP == null || obj.recalculoVTP == 0) {
        msj += "Debe ingresar una revision VTP." + "\n";
    }

    if (obj.valorCR == null || obj.valorCR == "SIN DATO" || obj.valorCR == 0) {
        msj += "No existe valor de Costo de Racionamiento vigente para este periodo." + "\n";
    }

    if (obj.valorCA == null || obj.valorCA == "SIN DATO" || obj.valorCA == 0) {
        msj += "No existe valor de Canon de Agua vigente para este periodo." + "\n";
    }

    if (obj.valorMR == null || obj.valorMR == "SIN DATO" || obj.valorMR == 0) {
        msj += "No existe valor de Margen de Reserva vigente para este periodo." + "\n";
    }

    return msj;
}