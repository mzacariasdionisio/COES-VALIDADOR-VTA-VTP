var controlador = siteRoot + 'PrimasRER/periodo/';
var ancho = 1000;

var tipo_accion_nuevo = 1;
var tipo_accion_ver = 2;
var tipo_accion_editar = 3;

$(function () {
    $('#cntMenu').css("display", "none");

    $('#tab-container').easytabs({
        animate: false
    });

    $('#tab-container').bind('easytabs:after', function () {
        refrehDatatable();
    });

    $('#tab-container').easytabs('select', '#vistaPeriodo');

    $("#btnBuscar").click(function () {
        listarPeriodos();
    });

    $("#btnNuevoRevision").click(function () {
        popupRevision(tipo_accion_nuevo);
    });

    $("#btnGuardarRevision").click(function () {
        guardarRevision();
    });

    $(".iconoInformativo").click(function () {
        mostrarMensaje();
    });
    
    listarPeriodos();

});

function listarPeriodos() {
    $('#listado').html('');
    $('#rec_listado').html('');
    $("#div_revision").hide();
    $('#tab-container').easytabs('select', '#vistaPeriodo');
    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 1100;

    $.ajax({
        type: 'POST',
        url: controlador + "ListarPeriodos",
        data: {
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

function mostrarMensaje() {
    alert("Esta fecha será el límite de envió de Solicitudes de EDI en el ‘Periodo de Reporte EDI’ por el sistema Extranet. Luego de esa fecha no será posible ingresar, editar ni eliminar Solicitudes de EDI");
}

function listarRevisiones(ipericodi) {
    $("#div_revision").hide();
    $("#rev_ipericodi").val(0);
    $(".rev_iperinombre").val('');

    $('#tab-container').easytabs('select', '#vistaRevision');
    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 1100;

    $.ajax({
        type: 'POST',
        url: controlador + "ListarRevisiones",
        data: {
            ipericodi: ipericodi,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $("#div_revision").show();
                $('#rev_listado').html(evt.Resultado);
                $("#rev_ipericodi").val(ipericodi);
                $(".rev_iperinombre").text(evt.IndPeriodo.Iperinombre);
                $("#rev_listado").css("width", (ancho) + "px");

                $('#tabla_revision').dataTable({
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

function guardarRevision() {
    var rerRevision = getRerrevision();
    var msj = validarRevision(rerRevision);
    if (msj != "") {
        alert(msj);
        return;
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarRevision',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            rerRevision: rerRevision,
        }),
        dataType: 'json',
        traditional: true,
        cache: false,
        success: function (result) {
            if (result.Resultado == '-1') {
                alert('Ha ocurrido un error: ' + result.Mensaje);
            } else {
                alert("Se guardó la revisión satisfactoriamente");
                $('#popupRevision').bPopup().close();
                listarPeriodos();
                listarRevisiones(rerRevision.Ipericodi);
                parent.listarPeriodos(); //Invoca a listarPeriodos ubicada fuera del iFrame, para actualizar los filtros de periodos y revisiones
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function editarRevision(rerrevcodi) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerRevision',
        data: {
            rerrevcodi: rerrevcodi,
        },
        cache: false,
        success: function (evt) {
            if (evt.Resultado == '-1') {
                alert('Ha ocurrido un error: ' + evt.Mensaje);
            } else {
                popupRevision(tipo_accion_editar, evt.RerRevision);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function verRevision(rerrevcodi) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerRevision',
        data: {
            rerrevcodi: rerrevcodi,
        },
        cache: false,
        success: function (evt) {
            if (evt.Resultado == '-1') {
                alert('Ha ocurrido un error: ' + evt.Mensaje);
            } else {
                popupRevision(tipo_accion_ver, evt.RerRevision);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function popupRevision(tipoAccion, revision=null) {
    if (tipoAccion == tipo_accion_nuevo) {
        $("#popupRevision div.popup-title span").html('Registro de revisión');
        $("#rev_rerrevcodi").val(0);
        $('input:radio[name=rev_tipo]').unbind();
        $("#rev_nombre").val('');
        $("#rev_fecha").val('');
        $('select[name=rev_estado]').val(['A']);
    }
    else if (tipoAccion == tipo_accion_editar) {
        $("#popupRevision div.popup-title span").html('Edición de revisión');
        $("#rev_rerrevcodi").val(revision.Rerrevcodi);
        $('input:radio[name=rev_tipo]').val([revision.Rerrevtipo]);
        $("#rev_nombre").val(revision.Rerrevnombre);
        $("#rev_fecha").val(revision.RerrevfechaDesc);
        $('select[name=rev_estado]').val([revision.Rerrevestado]);
    }
    else if (tipoAccion == tipo_accion_ver) {
        $("#popupVerRevision div.popup-title span").html("Información de revisión");
        $("#ver_rev_tipo").html(revision.RerrevtipoDesc);
        $("#ver_rev_nombre").html(revision.Rerrevnombre);
        $("#ver_rev_fecha").html(revision.RerrevfechaDesc);
        $("#ver_rev_estado").html(revision.RerrevestadoDesc);
        $("#ver_rev_modif_usuario").html(revision.Rerrevusumodificacion);
        $("#ver_rev_modif_fecha").html(revision.RerrevfecmodificacionDesc);
    }

    if (tipoAccion == tipo_accion_nuevo || tipoAccion == tipo_accion_editar) {
        $('#rev_fecha').inputmask({
            mask: "1/2/y h:s",
            placeholder: "dd/mm/yyyy hh:mm",
            alias: "datetime",
            hourFormat: "24"
        });
        $('#rev_fecha').Zebra_DatePicker({
            readonly_element: false,
            onSelect: function (date) {
                $('#rev_fecha').val(date + " 23:59");
            }
        });

        setTimeout(function () {
            $('#popupRevision').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown'
            });
        }, 50);
    }
    else if (tipoAccion == tipo_accion_ver) {
        setTimeout(function () {
            $('#popupVerRevision').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown'
            });
        }, 50);
    }
}

function getRerrevision() {
    var obj = {};
    obj.Rerrevcodi = parseInt($("#rev_rerrevcodi").val()) || 0;
    obj.Ipericodi = parseInt($("#rev_ipericodi").val()) || 0;
    obj.Rerrevtipo = $('input[name=rev_tipo]:checked').val();
    obj.Rerrevnombre = $("#rev_nombre").val();
    obj.RerrevfechaDesc = "" + $("#rev_fecha").val() + ":00";
    obj.Rerrevestado = $('select[name=rev_estado]').val();
    return obj;
}

function validarRevision(obj) {
    var msj = "";

    if (obj.Ipericodi <= 0) {
        msj += "Debe seleccionar un periodo." + "\n";
    }

    if (obj.Rerrevcodi < 0) {
        msj += "Código de revisión no válido." + "\n";
    }

    if (obj.Rerrevtipo == null || obj.Rerrevtipo.trim() == "") {
        msj += "Debe seleccionar un tipo." + "\n";
    }

    if (obj.Rerrevnombre == null || obj.Rerrevnombre.trim() == "") {
        msj += "Debe ingresar un nombre." + "\n";
    }

    if (obj.RerrevfechaDesc == null || obj.RerrevfechaDesc.trim() == "") {
        msj += "Debe ingresar una fecha." + "\n";
    }

    if (obj.Rerrevestado == null || obj.Rerrevestado.trim() == "") {
        msj += "Debe seleccionar una estado." + "\n";
    }

    return msj;
}

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

    $('#tabla_revision').dataTable({
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
