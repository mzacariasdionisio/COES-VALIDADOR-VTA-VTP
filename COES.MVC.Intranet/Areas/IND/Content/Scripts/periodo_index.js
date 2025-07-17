var controlador = siteRoot + 'IND/periodo/';
var ancho = 1000;
var OBJ_RECALCULO = null;
var OBJ_NEW = null;

var TIPO_ACCION_NUEVO = 1;
var TIPO_ACCION_VER = 2;
var TIPO_ACCION_EDITAR = 3;

$(function () {
    $('#cntMenu').css("display", "none");

    $('#tab-container').easytabs({
        animate: false
    });

    $('#tab-container').bind('easytabs:after', function () {
        refrehDatatable();
    });

    $('#tab-container').easytabs('select', '#vistaPeriodo');

    $("#btnNuevo").click(function () {
        popupPeriodoNuevo();
    });

    $("#btnBuscar").click(function () {
        mostrarListado();
    });

    $("#btnNuevoRecalculo").click(function () {
        OBJ_RECALCULO = null;
        $("#popupRecalculo div.popup-title span").html('Registro de recálculo');
        popupRecalculo(TIPO_ACCION_NUEVO);
    });

    $("#btnGuardarPeriodo").click(function () {
        guardarPeriodo();
    });
    $("#btnGuardarRecalculo").click(function () {
        guardarRecalculo();
    });

    if ($("#tipo_menu25").val() == "1")
        $("#btnNuevoRecalculo").show();

    mostrarListado();

});

///////////////////////////
/// Formulario Periodo
///////////////////////////

function mostrarListado() {
    var omitirQuincenal = $("#tipo_menu25").val() == "2" ? 1 : 0;

    $('#listado').html('');
    $('#rec_listado').html('');
    $("#div_recalculo").hide();

    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 1100;

    $('#tab-container').easytabs('select', '#vistaPeriodo');

    $.ajax({
        type: 'POST',
        url: controlador + "PeriodoListado",
        data: {
            omitirQuincenal: omitirQuincenal
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#listado').html(evt.Resultado);

                $("#listado").css("width", (ancho) + "px");
                var altotabla = parseInt($('#listado').height()) || 0;
                var a = $('#tabla_periodo').dataTable({
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
}

///////////////////////////
/// Formulario Recalculo
///////////////////////////

function verListadoRecalculo(ipericodi) {

    var omitirQuincenal = $("#tipo_menu25").val() == "2" ? 1 : 0;

    $("#div_recalculo").hide();
    $("#div_reporte").hide();
    $(".rec_periodo").val('');
    $("#rec_anio").val('');
    $("#rec_mes").val('');
    $("#rec_informe").val('');
    $("#rec_desc").val('');
    $("#rec_recacodi").val(0);

    $('#tab-container').easytabs('select', '#vistaRecalculo');

    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 1100;

    $.ajax({
        type: 'POST',
        url: controlador + "RecalculoListado",
        data: {
            ipericodi: ipericodi,
            omitirQuincenal: omitirQuincenal
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $("#div_recalculo").show();
                //$("#div_reporte").show();

                $('#rec_listado').html(evt.Resultado);
                $(".rec_periodo").text(evt.IndPeriodo.Iperinombre);
                $("#rec_pericodi").val(ipericodi);

                $("#rec_anio").val(evt.IndPeriodo.Iperianio);
                $("#rec_mes").val(evt.IndPeriodo.Iperimes);

                //
                OBJ_NEW = {
                    fecha_ini: evt.FechaIni,
                    fecha_fin: evt.FechaFin,
                    anio: evt.IndPeriodo.Iperianio,
                    mes: evt.IndPeriodo.Iperimes,
                    tipo: evt.TipoRecalculo
                };

                //
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
}

function guardarRecalculo() {
    var obj = getObjetoJsonRecalculo();
    if (confirm('¿Desea guardar el recálculo?')) {
        var msj = validarRecalculo(obj);

        if (msj == "") {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                traditional: true,
                url: controlador + 'RecalculoGuardar',
                data: {
                    irecacodi: obj.irecacodi,
                    ipericodi: obj.ipericodi,
                    descripcion: obj.descripcion,
                    nombre: obj.nombre,
                    strFechaIni: obj.strFechaIni,
                    strFechaFin: obj.strFechaFin,
                    strFechaLimite: obj.strFechaLimite,
                    strFechaObs: obj.strFechaObs,
                    informe: obj.informe,
                    estado: obj.estado,
                    tipo: obj.tipo
                },
                cache: false,
                success: function (result) {
                    if (result.Resultado == '-1') {
                        alert('Ha ocurrido un error:' + result.Mensaje);
                    } else {
                        alert("Se guardó correctamente el registro");
                        $('#popupRecalculo').bPopup().close();

                        mostrarListado();
                        verListadoRecalculo(obj.ipericodi);
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

function editarRecalculo(irecacodi) {
    $("#popupVerRecalculo div.popup-title span").html("Edición de Recálculo");

    $.ajax({
        type: 'POST',
        url: controlador + 'RecalculoObjeto',
        data: {
            irecacodi: irecacodi,
            tipoMenu: $("#tipo_menu25").val()
        },
        cache: false,
        success: function (evt) {
            if (evt.Resultado == '-1') {
                alert('Ha ocurrido un error: ' + evt.Mensaje);
            } else {
                OBJ_RECALCULO = evt.IndRecalculo;
                $("#popupRecalculo div.popup-title span").html('Edición de recálculo');

                popupRecalculo(TIPO_ACCION_EDITAR);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function verRecalculo(irecacodi) {
    $("#popupVerRecalculo div.popup-title span").html("Información de Recálculo");

    $.ajax({
        type: 'POST',
        url: controlador + 'RecalculoObjeto',
        data: {
            irecacodi: irecacodi,
            tipoMenu: $("#tipo_menu25").val()
        },
        cache: false,
        success: function (evt) {
            if (evt.Resultado == '-1') {
                alert('Ha ocurrido un error: ' + evt.Mensaje);
            } else {

                $("#ver_rec_nombre").html(evt.IndRecalculo.Irecanombre);
                $("#ver_rec_fecha_ini").html(evt.IndRecalculo.IrecafechainiDesc);
                $("#ver_rec_fecha_fin").html(evt.IndRecalculo.IrecafechafinDesc);
                $("#ver_rec_desc").html(evt.IndRecalculo.Irecadescripcion);
                $("#ver_rec_informe").html(evt.IndRecalculo.Irecainforme);
                $("#ver_rec_fecha_limite").html(evt.IndRecalculo.IrecafechalimiteDesc);
                $("#ver_rec_fecha_obs").html(evt.IndRecalculo.IrecafechaobsDesc);
                $("#ver_estado_recalculo").html(evt.IndRecalculo.IrecaestadoDesc);
                $("#ver_rec_modif_usuario").html(evt.IndRecalculo.UltimaModificacionUsuarioDesc);
                $("#ver_rec_modif_fecha").html(evt.IndRecalculo.UltimaModificacionFechaDesc);

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

function popupRecalculo(tipoAccion) {
    inicializarPopup(tipoAccion);

    setTimeout(function () {
        $('#popupRecalculo').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 50);
}

function inicializarPopup(tipoAccion) {
    var fechaIni = '', fechaFin = '';
    if (tipoAccion == TIPO_ACCION_EDITAR) {
        var evt = OBJ_RECALCULO;
        fechaIni = evt.FechaIniPeriodoDesc;
        fechaFin = evt.FechaFinPeriodoDesc;

        setHtmlTipoRecalculo(evt.Irecatipo, 0, false);

        $("#rec_recacodi").val(evt.Irecacodi);
        $('input:radio[name=tipo_recalculo]').val([evt.Irecatipo]);
        $("#rec_nombre").val(evt.Irecanombre);
        $("#rec_fecha_ini").val(evt.IrecafechainiDesc);
        $("#rec_fecha_fin").val(evt.IrecafechafinDesc);
        $("#rec_desc").val(evt.Irecadescripcion);
        $("#rec_informe").val(evt.Irecainforme);
        $("#rec_fecha_limite").val(evt.IrecafechalimiteDesc);
        $("#rec_fecha_obs").val(evt.IrecafechaobsDesc);

        $('input:radio[name=estado_recalculo]').val([evt.Irecaestado]);

    } else {
        //inicializar popup registro
        var evt = OBJ_NEW;
        fechaIni = evt.fecha_ini;
        fechaFin = evt.fecha_fin;

        var omitirQuincenal = $("#tipo_menu25").val() == "2";
        setHtmlTipoRecalculo(evt.tipo, 1, omitirQuincenal);

        $("#rec_recacodi").val(0);
        $("#rec_nombre").val('');
        $("#rec_fecha_ini").val(fechaIni);
        $("#rec_fecha_fin").val(fechaFin);
        $("#rec_desc").val('');
        $("#rec_informe").val("INFORME COES/D/DO/SME-INF-0XX-" + evt.anio);

        setearFechasXFechaFin();
    }

    $('#rec_fecha_ini').unbind();
    $('#rec_fecha_fin').unbind();
    $('#rec_fecha_limite').unbind();
    $('#rec_fecha_obs').unbind();
    $('input:radio[name=tipo_recalculo]').unbind();


    if (tipoAccion == TIPO_ACCION_NUEVO) {
        $('input:radio[name=tipo_recalculo]').change(function () {
            setearObs();
            setearTextoTipo();
        });
        setearTextoTipo();
    }
    setearObs();

    $('#rec_fecha_ini').Zebra_DatePicker({
        direction: [fechaIni, fechaFin],
        pair: $('#rec_fecha_fin'),
        onSelect: function (date) {
            $('#rec_fecha_fin').val(date);
        }
    });
    $('#rec_fecha_fin').Zebra_DatePicker({
        direction: [fechaIni, fechaFin],
        onSelect: function (date) {
            setearFechasXFechaFin();
        }
    });

    $('#rec_fecha_limite').inputmask({
        mask: "1/2/y h:s",
        placeholder: "dd/mm/yyyy hh:mm",
        alias: "datetime",
        hourFormat: "24"
    });
    $('#rec_fecha_obs').inputmask({
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
    $('#rec_fecha_obs').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#rec_fecha_obs').val(date + " 23:59");
        }
    });
}

function setearFechasXFechaFin() {
    var fec1 = '';
    var fec2 = '';

    var tipo = $('input[name=tipo_recalculo]:checked').val();
    if (tipo == "PQ" || tipo == "PM") {
        fec1 = obtenerDiaSiguiente($('#rec_fecha_fin').val(), 3) + " 23:59";
        fec2 = obtenerDiaSiguiente($('#rec_fecha_fin').val(), 4) + " 12:00";
    } else {
        fec1 = obtenerDiaSiguiente($('#rec_fecha_fin').val(), 5) + " 23:59";
        fec2 = obtenerDiaSiguiente($('#rec_fecha_fin').val(), 5) + " 12:00";
    }

    $("#rec_fecha_limite").val(fec1);
    $("#rec_fecha_obs").val(fec2);
}

function setearObs() {
    $(".td_obs").hide();
    var tipo = $('input[name=tipo_recalculo]:checked').val();

    if (tipo == "PQ" || tipo == "PM")
        $(".td_obs").show();
}

function setearTextoTipo() {
    var tipo = $('input[name=tipo_recalculo]:checked').val();

    //nombre
    if (tipo == "PQ")
        $("#rec_nombre").val("Primera Quincena");
    if (tipo == "PM")
        $("#rec_nombre").val("Preliminar Mensual");
    if (tipo == "M")
        $("#rec_nombre").val("Mensual");

    if (tipo.substring(0, 1) == 'R') {
        var numR = tipo.substring(1, 2);
        $("#rec_nombre").val("Revisión 0" + numR);
    }

    //fechas

    if (tipo == "PQ") {
        var fechaNewFin = "15/" + ("" + OBJ_NEW.mes).padStart(2, "0") + "/" + OBJ_NEW.anio;
        $("#rec_fecha_ini").val(OBJ_NEW.fecha_ini);
        $("#rec_fecha_fin").val(fechaNewFin);
    } else {
        $("#rec_fecha_ini").val(OBJ_NEW.fecha_ini);
        $("#rec_fecha_fin").val(OBJ_NEW.fecha_fin);
    }
    setearFechasXFechaFin();
}

function setHtmlTipoRecalculo(tipo, modo, flagOmitirQuincenal) {
    var html = '';
    if (modo == 1) { //nuevo
        if ((tipo == null || tipo == "") && !flagOmitirQuincenal)
            html = `
                    <input type='radio' name='tipo_recalculo' value='PQ' checked='checked'>Primera Quincena<br />
                    <input type='radio' name='tipo_recalculo' value='PM'>Preliminar Mensual <br />
                    <input type='radio' name='tipo_recalculo' value='M'>Mensual <br />
                    <input type='radio' name='tipo_recalculo' value='R1'>Revisión 01
                        `;

        if (tipo == "PQ")
            html = `
                    <input type='radio' name='tipo_recalculo' value='PM' checked='checked'>Preliminar Mensual <br />
                    <input type='radio' name='tipo_recalculo' value='M'>Mensual <br />
                    <input type='radio' name='tipo_recalculo' value='R1'>Revisión 01
                        `;

        if (tipo == "PM")
            html = `
                    <input type='radio' name='tipo_recalculo' value='M' checked='checked'>Mensual <br />
                    <input type='radio' name='tipo_recalculo' value='R1'>Revisión 01
                        `;

        if (tipo == "M")
            html = `
                    <input type='radio' name='tipo_recalculo' value='R1' checked='checked'>Revisión 01
                        `;

        if (tipo.substring(0, 1) == 'R') {
            var numR = parseInt(tipo.substring(1, 2)) + 1;
            html = `
                    <input type='radio' name='tipo_recalculo' value='R${numR}' checked='checked'>Revisión 0${numR}
                        `;
        }
    } else { //editar
        if (tipo == "PQ")
            html = `
                    <input type='radio' name='tipo_recalculo' value='PQ' checked='checked'>Primera Quincena<br />
                        `;

        if (tipo == "PM")
            html = `
                    <input type='radio' name='tipo_recalculo' value='PM'>Preliminar Mensual <br />
                        `;

        if (tipo == "M")
            html = `
                    <input type='radio' name='tipo_recalculo' value='M'>Mensual <br />
                        `;

        if (tipo.substring(0, 1) == 'R') {
            var numR2 = parseInt(tipo.substring(1, 2));
            html = `
                    <input type='radio' name='tipo_recalculo' value='R${numR2}'>Revisión 0${numR2}
                        `;
        }
    }

    $("#td_tipo_recalculo").html(html);
}

function getObjetoJsonRecalculo() {
    var obj = {};

    obj.ipericodi = parseInt($("#rec_pericodi").val()) || 0;
    obj.irecacodi = parseInt($("#rec_recacodi").val()) || 0;
    obj.tipo = $('input[name=tipo_recalculo]:checked').val();
    obj.nombre = $("#rec_nombre").val();
    obj.descripcion = $("#rec_desc").val();
    obj.strFechaIni = $("#rec_fecha_ini").val();
    obj.strFechaFin = $("#rec_fecha_fin").val();
    obj.strFechaLimite = $("#rec_fecha_limite").val();
    obj.strFechaObs = $("#rec_fecha_obs").val();
    obj.informe = $("#rec_informe").val();
    obj.estado = $('input[name=estado_recalculo]:checked').val();

    return obj;
}

function validarRecalculo(obj) {
    var msj = "";
    if (obj.nombre == null || obj.nombre.trim() == "") {
        msj += "Debe ingresar nombre." + "\n";
    }

    if (obj.ipericodi <= 0) {
        msj += "Debe seleccionar un periodo." + "\n";
    }

    if (obj.informe == null || obj.informe.trim() == "") {
        msj += "Debe ingresar el nombre del informe.";
    }

    if (obj.informe == null || obj.informe.trim() == "") {
        msj += "Debe ingresar el nombre del informe.";
    }

    return msj;
}

function verListadoReporte(irecacodi) {
    $("#div_reporte").hide();
    $(".rec_recalculo").html('');
    $("#rec_reporte").html('');

    $.ajax({
        type: 'POST',
        url: controlador + 'RecalculoObjeto',
        data: {
            irecacodi: irecacodi,
            tipoMenu: $("#tipo_menu25").val()
        },
        cache: false,
        success: function (evt) {
            if (evt.Resultado == '-1') {
                alert('Ha ocurrido un error: ' + evt.Mensaje);
            } else {
                $('#tab-container').easytabs('select', '#vistaReporte');

                $("#rec_reporte").html(evt.Resultado2);

                $("#div_reporte").show();

                $(".rec_periodo").text(evt.IndPeriodo.Iperinombre);
                $("#rec_recacodi").val(evt.IndRecalculo.Irecacodi);
                $(".rec_recalculo").html(evt.IndRecalculo.Irecanombre);

            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

//
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

function obtenerDiaSiguiente(sFecha, numDia) { //sFecha, formato YYYY-MM-DD STRING
    var checkdateFrom = convertStringToDate(sFecha, "00:00:00");
    var horaFin01 = moment(checkdateFrom).set('date', moment(checkdateFrom).get('date') + numDia);

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
    return new Date(partsFecha[2], partsFecha[1] - 1, partsFecha[0], partsHoras[0], partsHoras[1], partsHoras[2]);
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

///////////////////////////
/// Descargar archivos excel
///////////////////////////

function descargarReporte(id) {

    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoExcelCuadro",
        data: {
            cuadro: id,
            irecacodi: $('#rec_recacodi').val(),
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
