var controler = siteRoot + 'eventos/registro/';

$(function () {

    $('#tab-container').easytabs({
        animate: false
    });

    $('#txtHoraInicial').inputmask({
        mask: "1/2/y h:s:s",
        placeholder: "dd/mm/yyyy hh:mm:ss",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#txtHoraInicial').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtHoraInicial').val(date + " 00:00:00");
        }
    });

    $('#txtHoraFinal').inputmask({
        mask: "1/2/y h:s:s",
        placeholder: "dd/mm/yyyy hh:mm:ss",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#txtHoraFinal').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtHoraFinal').val(date + " 00:00:00");
        }
    });

    $('#cbTipoEvento').change(function () {
        cargarSubCausaEvento();
    });

    $('#btnBuscarEquipo').click(function () {
        openBusquedaEquipo();
    });

    $('#btnGrabar').click(function () {
        grabar();
    });

    $('#btnCancelar').click(function () {
        document.location.href = siteRoot + 'eventos/evento/index';
    });

    $('#btnAddInterrupcion').click(function () {
        openInterrupcion(0, 0, 0, 0);
    });

    $('#btnImportarInforme').click(function () {
        importarInterrupciones();
    });

    $('#cbInformeFalla').click(function () {
        $('#ampliacionIfHorario').val("");
        $('#ampliacionIpiHorario').val("");
        $('#amplicacionIpiDias').val("0");
        $('#amplicacionIfDias').val("0");
        if ($('#cbInformeFalla').is(':checked')) {
            $("#cbInformeFalla2").prop('disabled', true);
            $('#contedor-ampliacion').show();
            $('#hfInformeFalla2').val("N")
            $('#cbInformeFalla2').prop('checked', false);
        }
        if (!$('#cbInformeFalla').is(':checked')) {
            $("#cbInformeFalla2").prop('disabled', false);
            $('#contedor-ampliacion').hide();
            $('#hfInformeFalla2').val("N");
        }
        if ($('#cbInformeFalla2').is(':checked')) {
            $("#cbInformeFalla").prop('disabled', false);
            $('#contedor-ampliacion').show();
            $('#hfInformeFalla').val("N");
        }

    })

    $('#cbInformeFalla2').click(function () {
        $('#ampliacionIfHorario').val("");
        $('#ampliacionIpiHorario').val("");
        $('#amplicacionIpiDias').val("0");
        $('#amplicacionIfDias').val("0");
        if ($('#cbInformeFalla2').is(':checked')) {
            $("#cbInformeFalla").prop('disabled', true);
            $('#contedor-ampliacion').show();
            $('#hfInformeFalla').val("N");
            $('#cbInformeFalla').prop('checked', false);
        }
        if (!$('#cbInformeFalla2').is(':checked')) {
            $("#cbInformeFalla").prop('disabled', false);
            $('#contedor-ampliacion').hide();
            $('#hfInformeFalla').val("N");
        }
        if ($('#cbInformeFalla').is(':checked')) {
            $("#cbInformeFalla2").prop('disabled', false);
            $('#contedor-ampliacion').show();
            $('#hfInformeFalla2').val("N");
        }
    })

    $('#contedor-ampliacion').hide();

    cargarPrevio();

    $('#btnGrabarInterrupcion').click(function () {
        grabarInterrupcion();
    });

    $('#btnCancelarInterrupcion').click(function () {
        $('#popupInterrupcion').bPopup().close();
    });

    $('#cbTipoRegistro').change(function () {
        cargarTipoRegistro();
    });

    $('#btnInforme').click(function () {
        cargarEstadoInforme($('#hfCodigoEvento').val());
    });

    $('#btnConvertir').click(function () {
        convertir();
    });

    $('#btnExportarInterrupcion').click(function () {
        exportarInterrupcion();
    });



    //cargarEquipos();
    listarInterrupciones();
    cargarBusquedaEquipo();


});

cargarPrevio = function () {

    $('#cbTipoEvento').val($('#hfTipoEvento').val());
    $('#cbSubCausaEvento').val($('#hfSubCausaEvento').val());
    $('#cbTipoFalla').val($('#hfTipoFalla').val());
    $('#cbFases').val($('#hfFases').val());
    $('#cbTipoRegistro').val($('#hfTipoRegistro').val());
    $('#cbAreaOperativa').val($('#hfAreaOperativa').val());

    if ($('#hfMensajeSMS').val() == "S") {
        $('#cbMensajeSMS').prop('checked', true);
    }

    if ($('#hfRelevante').val() == "S") {
        $('#cbRelevante').prop('checked', true);
    }

    //if ($('#hfAnalisisFalla').val() == "S") {
    //    $('#cbAnalisisFalla').prop('checked', true);
    //}
    var a = $('#hfInformeFalla2').val()
    if ($('#hfInformeFalla').val() == "S" && $('#hfInformeFalla2').val() == "N") {
        $('#cbInformeFalla').prop('checked', true);
        $('#cbInformeFalla2').prop('disabled', true);
        $('#hfInformeFalla2').val("N")
        $('#contedor-ampliacion').show();
    }

    if ($('#hfInformeFalla2').val() == "S" && $('#hfInformeFalla').val() == "N") {
        $('#cbInformeFalla2').prop('checked', true);
        $('#cbInformeFalla').prop('disabled', true);
        $('#hfInformeFalla').val("N")
        $('#contedor-ampliacion').show();
    }

    if ($('#hfInformeFalla2').val() == "S" && $('#hfInformeFalla').val() == "S") {
        $('#cbInformeFalla').prop('checked', true);
        $('#cbInformeFalla2').prop('checked', true);
    }


    if ($('#hfAsegOperacion').val() == "S") {
        $('#cbAsegOperacion').prop('checked', true);
    }

    if ($('#hfInterrupcionTotal').val() == "S") {
        $('#cbInterrupcionTotal').prop('checked', true);
    }

    if ($('#hfDisminucionCarga').val() == "S") {
        $('#cbDisminucionCarga').prop('checked', true);
    }

    if ($('#hfRechazoManual').val() == "S") {
        $('#cbRechazoManual').prop('checked', true);
    }

    if ($('#hfRechazoAutomatico').val() == "S") {
        $('#cbRechazoAutomatico').prop('checked', true);
    }

    if ($('#hfProvocaInterrupcion').val() == "S") {
        $('#cbProvovaInterrupcion').prop('checked', true);
    }

    if ($('#hfDesconectaGeneracion').val() == "S") {
        $('#cbDesconectaGeneracion').prop('checked', true);
    }


    $('#tablaDesconexion').css('display', 'none');
    $('#tablaMalaCalidad').css('display', 'none');

    if ($('#hfTipoRegistro').val() == "D") {
        $('#tablaDesconexion').css('display', 'block');
    }

    if ($('#hfTipoRegistro').val() == "C") {
        $('#tablaMalaCalidad').css('display', 'block');
    }

    $('input[name="MalaCalidad"][value="' + $('#hfMalaCalidad').val() + '"]').attr('checked', true);

    var idEvento = $('#hfCodigoEvento').val();

    if (idEvento == "0") {
        $('#btnAddInterrupcion').css('display', 'none');
        $('#btnImportarInforme').css('display', 'none');
        $('#btnExportarInterrupcion').css('display', 'none');
    }

    if ($('#hfCodigoEvento').val() != '0') {
        $('#btnConvertir').css('display', 'block');
    }
    else {
        $('#btnConvertir').css('display', 'none');
    }
}

cargarSubCausaEvento = function () {

    if ($('#cbTipoEvento').val() != "") {
        $.ajax({
            type: 'POST',
            url: controler + 'cargarsubcausaevento',
            dataType: 'json',
            data: { idTipoEvento: $('#cbTipoEvento').val() },
            cache: false,
            success: function (aData) {
                $('#cbSubCausaEvento').get(0).options.length = 0;
                $('#cbSubCausaEvento').get(0).options[0] = new Option("-SELECCIONE-", "");
                $.each(aData, function (i, item) {
                    $('#cbSubCausaEvento').get(0).options[$('#cbSubCausaEvento').get(0).options.length] =
                        new Option(item.Subcausadesc, item.Subcausacodi);
                });
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        $('#cbSubCausaEvento').get(0).options.length = 0;
    }
}

cargarBusquedaEquipo = function () {
    $.ajax({
        type: "POST",
        url: siteRoot + "eventos/busqueda/index",
        data: {},
        success: function (evt) {
            $('#busquedaEquipo').html(evt);

            if ($('#hfCodigoEvento').val() == "0") {
                openBusquedaEquipo();
            }
        },
        error: function (req, status, error) {
            mostrarError();
        }
    });
}

openBusquedaEquipo = function () {

    $('#busquedaEquipo').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    $('#txtFiltro').focus();
}

seleccionarEquipo = function (idEquipo, substacion, equipo, empresa, idEmpresa) {

    $.ajax({
        type: 'POST',
        url: controler + 'agregarequipo',
        dataType: 'json',
        data: { idEquipo: idEquipo },
        cache: false,
        success: function (result) {
            if (result != -1) {
                cargarEquipos(result);
            }
            else {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });

    $('#busquedaEquipo').bPopup().close();
}

cargarEquipos = function (result) {

    //agregamos registros al final de la tabla
    var count = 0;
    var flag = true;
    $('#tablaEquipo>tbody tr').each(function (i) {
        $punto = $(this).find('#hfEquipoItem');
        if ($punto.val() == result.Equicodi) {
            flag = false;
        }
    });

    if (flag) {
        $('#tablaEquipo> tbody').append(
            '<tr>' +
            '   <td>' + result.EMPRNOMB + '</td>' +
            '   <td>' + result.TAREAABREV + ' ' + result.AREANOMB + '</td>' +
            '   <td>' + result.FAMABREV + '</td>' +
            '   <td>' + result.DESCENTRAL + '</td>' +
            '   <td>' + result.Equiabrev + '</td>' +
            '   <td>' + result.Equitension + '</td>' +
            '   <td style="text-align:center">' +
            '       <input type="hidden" id="hfEquipoItem" value="' + result.Equicodi + '" /> ' +
            '       <img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="" onclick="$(this).parent().parent().remove();" style="cursor:pointer" />' +
            '   </td>' +
            '</tr>'
        );
    }
    else {
        mostrarAlerta('Ya se ha agregado el equipo.');
    }
}

quitarEquipo = function (id) {
    //$.ajax({
    //    type: 'POST',
    //    url: controler + 'eliminarequipo',
    //    dataType: 'json',
    //    data: { idEquipo: id },
    //    cache: false,
    //    success: function (result) {
    //        if (result == 1) {
    //            cargarEquipos();
    //        }
    //        else {
    //            mostrarError();
    //        }
    //    },
    //    error: function () {
    //        mostrarError();
    //    }
    //});
}

grabar = function () {
    var mensaje = validarRegistro();
    if (mensaje == "") {

        var count = 0;
        var items = "";
        $('#tablaEquipo>tbody tr').each(function (i) {
            $punto = $(this).find('#hfEquipoItem');
            var constante = (count > 0) ? "," : "";
            items = items + constante + $punto.val();
            count++;
        });

        $('#hfListaEquipos').val(items);

        $.ajax({
            type: 'POST',
            url: controler + "grabar",
            dataType: 'json',
            data: $('#formEvento').serialize(),
            success: function (result) {
                if (result > 1) {
                    mostrarExito();

                    var indicadorNew = $('#hfCodigoEvento').val();
                    $('#hfCodigoEvento').val(result);
                    $('#btnAddInterrupcion').css('display', 'block');
                    $('#btnImportarInforme').css('display', 'block');
                    $('#btnExportarInterrupcion').css('display', 'none');
                    $('#btnConvertir').css('display', 'block');

                    if (indicadorNew == '0') {
                        //enviarNotificacion();
                    }

                }
                if (result == -1) {
                    mostrarError();
                }
                if (result == -2) {
                    mostrarAlerta("La fecha inicial debe ser menor a la final.");
                }
                if (result == -3) {
                    mostrarAlerta("Solo tiene plazo para grabar hasta las 7:00 horas.");
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        mostrarAlerta(mensaje);
    }
}


enviarNotificacion = function () {

    var tipoEvento = $('#cbSubCausaEvento').val();
    var titulo = $('#txtDescripcion').val();
    var cuerpo = $('#txtDetalleAdicional').val() + "\n" + "Para más detalles ingrese a la sección de Eventos del App COES";

    var json = {
        "to": "/topics/" + tipoEvento,
        "notification": {
            "body": cuerpo,
            "title": titulo,
            "content_available": true,
            "priority": "high"
        }
    };

    $.ajax({
        url: "https://fcm.googleapis.com/fcm/send",
        method: 'POST',
        dataType: 'json',
        crossDomain: true,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(json),
        cache: false,
        headers: {
            Authorization: "key=AAAAdqfpa8E:APA91bFsGQpPKYlWTpdhhkEQ22gW4PRz9xHn5yo7HkBoW0P4BDHXK6-T0YIgvM-0TRZ-KwrwqDFg969i19rNGChuuGjRDXb-Deihz5i88935aHXm7X_njK_fGvLD8Tjdx5_tflHxZt23"
        },
        success: function (data) {
            console.log(data);
        },
        error: function (error) {
            console.log(error);
        }
    });
}


validarRegistro = function () {

    var mensaje = "<ul>";
    var flag = true;
    if ($('#cbTipoEvento').val() == '') {
        mensaje = mensaje + "<li>Seleccione el tipo de evento.</li>";
        flag = false;
    }

    if ($('#txtHoraInicial').val() == '') {
        mensaje = mensaje + "<li>Ingrese la hora inicial.</li>";
        flag = false;
    }
    else {
        if (!$('#txtHoraInicial').inputmask("isComplete")) {
            mensaje = mensaje + "<li>Ingrese hora inicial.</li>";
            flag = false;
        }
    }

    if ($('#txtHoraFinal').val() == '') {
        mensaje = mensaje + "<li>Ingrese la hora final.</li>";
        flag = false;
    }
    else {
        if (!$('#txtHoraFinal').inputmask("isComplete")) {
            mensaje = mensaje + "<li>Ingrese hora final.</li>";
            flag = false;
        }
    }

    if ($('#txtDescripcion').val() == '') {
        mensaje = mensaje + "<li>Ingrese la descripción del evento.</li>";
        flag = false;
    }

    if ($('#cbSubCausaEvento').val() == '') {
        mensaje = mensaje + "<li>Seleccione la causa del evento.";
        flag = false;
    }

    if ($('#hfIndRemitenteCorreo').val() == 'S') {
        if ($('#txtRemitenteCorreo').val() == '') {
            mensaje = mensaje + "<li>Ingrese el nombre del remitente del correo a enviar a los Agentes.</li>";
            flag = false;
        }
    }


    if ($('#cbTipoRegistro').val() == "D") {
        if ($('#cbInterrupcionTotal').is(':checked')) {
            if ($('#txtInterrupcionTotal').val() == '') {
                mensaje = mensaje + "<li>Ingrese la magnitud para la Interrupción Total</li>";
                flag = false;
            }
        }
        if ($('#cbDisminucionCarga').is(':checked')) {
            if ($('#txtDisminucionCarga').val() == '') {
                mensaje = mensaje + "<li>Ingrese la magnitud para la Disminución de Carga</li>";
                flag = false;
            }
        }
        if ($('#cbRechazoManual').is(':checked')) {
            if ($('#txtRechazoManual').val() == '') {
                mensaje = mensaje + "<li>Ingrese la magnitud para el Rechazo Manual</li>";
                flag = false;
            }
        }
        if ($('#cbRechazoAutomatico').is(':checked')) {
            if ($('#txtRechazoAutomatico').val() == '') {
                mensaje = mensaje + "<li>Ingrese la magnitud para el Rechazo Automático</li>";
                flag = false;
            }
        }
    }
    if ($('#hfInformeFalla').val()=="S" && $('#hfInformeFalla2').val()=="S") {
        mensaje = mensaje + "<li>Solo debe seleccionar Informe de Fallas o Informe de Fallas N2 .";
        flag = false;
    }

    var count = $('#tablaEquipo >tbody >tr').length;

    if (count == 0) {
        mensaje = mensaje + "<li>Seleccione equipos.</li>";
        flag = false;
    }

    if (expresionRegular($('#txtDescripcion').val()) == 1) {
        mensaje = mensaje + "<li>La Descripción no puede contener caracteres especiales.</li>";
        flag = false;
    }

    mensaje = mensaje + "</ul>";

    if (flag) mensaje = "";

    $('#hfMensajeSMS').val('N')
    if ($('#cbMensajeSMS').is(':checked')) $('#hfMensajeSMS').val('S');

    $('#hfRelevante').val('N')
    if ($('#cbRelevante').is(':checked')) $('#hfRelevante').val('S');

    $('#hfAnalisisFalla').val('N');
    /*if ($('#cbAnalisisFalla').is(':checked')) $('#hfAnalisisFalla').val('S');*/

    $('#hfInformeFalla').val('N')
    if ($('#cbInformeFalla').is(':checked')) $('#hfInformeFalla').val('S');

    $('#hfInformeFalla2').val('N')
    if ($('#cbInformeFalla2').is(':checked')) $('#hfInformeFalla2').val('S');

    $('#hfAsegOperacion').val('N')
    if ($('#cbAsegOperacion').is(':checked')) $('#hfAsegOperacion').val('S');

    $('#hfInterrupcionTotal').val('N');
    if ($('#cbInterrupcionTotal').is(':checked')) $('#hfInterrupcionTotal').val('S');

    $('#hfDisminucionCarga').val('N');
    if ($('#cbDisminucionCarga').is(':checked')) $('#hfDisminucionCarga').val('S');

    $('#hfRechazoManual').val('N');
    if ($('#cbRechazoManual').is(':checked')) $('#hfRechazoManual').val('S');

    $('#hfRechazoAutomatico').val('N');
    if ($('#cbRechazoAutomatico').is(':checked')) $('#hfRechazoAutomatico').val('S');

    $('#hfProvocaInterrupcion').val('N');
    if ($('#cbProvovaInterrupcion').is(':checked')) $('#hfProvocaInterrupcion').val('S');

    $('#hfDesconectaGeneracion').val('N');
    if ($('#cbDesconectaGeneracion').is(':checked')) $('#hfDesconectaGeneracion').val('S');

    $('#hfMalaCalidad').val($("input:radio[name='MalaCalidad']:checked").val());

    return mensaje;
}

cargarTipoRegistro = function () {
    $('#tablaDesconexion').css('display', 'none');
    $('#tablaMalaCalidad').css('display', 'none');
    var tipo = $('#cbTipoRegistro').val();

    if (tipo == "D") {
        $('#tablaDesconexion').css('display', 'block');
    }
    if (tipo == "C") {
        $('#tablaMalaCalidad').css('display', 'block');
    }
}

listarInterrupciones = function () {
    $.ajax({
        type: "POST",
        url: controler + 'listainterrupcion',
        data: {
            idEvento: $('#hfCodigoEvento').val(),
        },
        success: function (evt) {
            $('#contenedorInterrupcion').html(evt);
        },
        error: function (req, status, error) {
            mostrarError();
        }
    });
}

openInterrupcion = function (id, tiempo, total, idItemInforme) {
    $.ajax({
        type: "POST",
        url: controler + 'interrupcion',
        data: {
            idEvento: $('#hfCodigoEvento').val(),
            id: id,
            tiempo: tiempo,
            total: total,
            idItemInforme: idItemInforme
        },
        success: function (evt) {
            $('#cntInterrupcion').html(evt);

            $('#listaPuntoInterrupcion').dataTable({
                "sPaginationType": "full_numbers",
                "bInfo": false,
                "bLengthChange": false
            });

            setTimeout(function () {
                $('#popupInterrupcion').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false
                });
            }, 50);
        },
        error: function (req, status, error) {
            mostrarError();
        }
    });
}

deleteInterrupcion = function (id) {
    if (confirm('¿Está seguro de eliminar este registro?')) {
        $.ajax({
            type: 'POST',
            url: controler + 'deleteinterrupcion',
            dataType: 'json',
            data: {
                id: id,
                idEvento: $('#hfCodigoEvento').val()
            },
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    listarInterrupciones();
                }
                else {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

seleccionarPuntoInterrupcion = function (id, emprnomb, ptoInterrupcion, ptoEntrega) {
    $('#hfIdPtoInterrupcion').val(id);
    $('#spanPtoInterrupcion').text(ptoInterrupcion);
    $('#spanPtoEntrega').text(ptoEntrega);
    $('#spanEmpresa').text(emprnomb);
}

grabarInterrupcion = function () {

    var mensaje = validarInterrupcion();

    if (mensaje == "") {

        $('#hfIndBajoCarga').val("N");
        if ($('#cbBajoCarga').is(':checked')) {
            $('#hfIndBajoCarga').val("S");
        }
        if ($('#cbRacmf').is(':checked')) {
            $('#hfIndRacmf').val("S");
        }
        if ($('#cbRechazoManual').is(':checked')) {
            $('#hfIndManualr').val("S");
        }

        $.ajax({
            type: 'POST',
            url: controler + 'grabarinterrupcion',
            dataType: 'json',
            data: $('#frmInterrupcion').serialize(),
            cache: false,
            success: function (resultado) {
                if (resultado > 0) {
                    listarInterrupciones();
                    $('#popupInterrupcion').bPopup().close();

                    if ($('#hfIdItemInforme').val() != "") {
                        if ($('#hfIdItemInforme').val() != "0") {
                            importarInterrupciones();
                        }
                    }
                }
                else {
                    $('#mensajeInterrupcion').removeClass();
                    $('#mensajeInterrupcion').addClass('action-error');
                    $('#mensajeInterrupcion').html('Ha ocurrido un error.');
                }
            },
            error: function () {
                $('#mensajeInterrupcion').removeClass();
                $('#mensajeInterrupcion').addClass('action-error');
                $('#mensajeInterrupcion').html('Ha ocurrido un error.');
            }
        });
    }
    else {
        $('#mensajeInterrupcion').removeClass();
        $('#mensajeInterrupcion').addClass('action-alert');
        $('#mensajeInterrupcion').html(mensaje);
    }
}

validarInterrupcion = function () {
    var mensaje = "<ul>";
    var flag = true;

    if ($('#hfIdPtoInterrupcion').val() == "") {
        mensaje = mensaje + "<li>Seleccione punto de interrupción.</li>";
        flag = false;
    }
    if ($('#txtInterTiempo').val() == "") {
        mensaje = mensaje + "<li>Ingrese el tiempo de interrupción.</li>";
        flag = false;
    }

    if ($('#cbBajoCarga').is(':checked')) {
        //if ($('#txtInterDe').val() == "") {
        //    mensaje = mensaje + "<li>Ingrese los MW iniciales.</li>";
        //    flag = false;
        //}
        //if ($('#txtInterA').val() == "") {
        //    mensaje = mensaje + "<li>Ingrese los MW finales.</li>";
        //    flag = false;
        //}

        if ($('#txtInterTotal').val() == "" && $('#txtInterDe').val() == "" && $('#txtInterA').val() == "") {
            mensaje = mensaje + "<li>Ingrese los MW interrumpidos o los MW iniciales y finales.</li>";
            flag = false;
        }
        else {
            if ($('#txtInterTotal').val() == "") {
                if (!($('#txtInterDe').val() != "" && $('#txtInterA').val() != "")) {
                    mensaje = mensaje + "<li>Ingrese los MW iniciales y finales.</li>";
                    flag = false;
                }
            }
        }
    }
    else {
        if ($('#txtInterTotal').val() == "") {
            mensaje = mensaje + "<li>Ingrese los MW interrumpidos.</li>";
            flag = false;
        }
    }

    mensaje = mensaje + "</ul>";

    if (flag) {
        mensaje = "";
    }

    return mensaje;
}

importarInterrupciones = function () {
    $.ajax({
        type: "POST",
        url: controler + 'importacion',
        data: {
            idEvento: $('#hfCodigoEvento').val()
        },
        success: function (evt) {
            $('#contenedorImportacion').html(evt);
        },
        error: function (req, status, error) {
            mostrarError();
        }
    });
}

asignarPuntoInterrupcion = function (tiempo, total, idItemInforme) {
    openInterrupcion(0, tiempo, total, idItemInforme);
}

cargarEstadoInforme = function (idEvento) {
    $.ajax({
        type: 'POST',
        url: siteRoot + "eventos/evento/informe",
        data: {
            idEvento: idEvento
        },
        success: function (evt) {
            $('#contenedorInforme').html(evt);

            setTimeout(function () {
                $('#popupInforme').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });

                $('#tablaReporte').dataTable({
                    "sPaginationType": "full_numbers",
                    "aaSorting": [[0, "desc"]],
                    "destroy": "true",
                    "sDom": 'ftp',
                });

            }, 50);
        },
        error: function () {
            mostrarError();
        }
    });
}

convertir = function () {
    if (confirm('¿Está seguro de realizar esta operación?')) {
        $.ajax({
            type: 'POST',
            url: controler + "cambiarbitacora",
            dataType: 'json',
            data: {
                idEvento: $('#hfCodigoEvento').val()
            },
            success: function (result) {
                if (result == 1) {
                    document.location.href = controler + 'bitacora?id=' + $('#hfCodigoEvento').val();
                }
                if (result == -1) {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

showInforme = function (idEmpresa, idEvento) {
    document.location.href = siteRoot + 'eventos/informe?evento=' + idEvento + '&empresa=' + idEmpresa;
}

informeConsolidado = function (idEvento, indicador) {
    document.location.href = siteRoot + 'eventos/informe?evento=' + idEvento + '&empresa=0&indicador=' + indicador;
}

verHistorioAuditoria = function () {
    var id = $('#hfCodigoEvento').val();

    $.ajax({
        type: "POST",
        url: controler + "auditoria",
        data: {
            idEvento: id
        },
        success: function (evt) {
            $('#detalleAuditoria').html(evt);

            setTimeout(function () {
                $('#popupAuditoria').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false
                });
            }, 1);

        },
        error: function () {
            mostrarError();
        }
    });
}

exportarInterrupcion = function () {
    $.ajax({
        type: 'POST',
        url: controler + 'exportarinterrupcion',
        dataType: 'json',
        cache: false,
        data: {
            idEvento: $('#hfCodigoEvento').val()
        },
        success: function (resultado) {
            if (resultado == 1) {
                location.href = controler + "descargarinterrupcion";
            }
            else {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

validarNumero = function (item, evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;

    if (charCode == 46) {
        var regex = new RegExp(/\./g)
        //var regex = new RegExp(/^ [0 - 9]{ 1, 3}$ |^ [0 - 9]{ 1, 3}\.[0 - 9]{ 1, 3}$/);
        var count = $(item).val().match(regex);
        if (count.length > 1) {
            return false;
        }
    }

    if (charCode == 45) {
        var regex = new RegExp(/\-/g)
        var count = $(item).val().match(regex).length;
        if (count > 0) {
            return false;
        }
    }

    if (charCode > 31 && charCode != 45 && (charCode < 48 || charCode > 57)) {
        return false;
    }

    return true;
}

mostrarError = function () {
    alert("Ha ocurrido un error");
}

mostrarAlerta = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-alert');
    $('#mensaje').html(mensaje);
}

mostrarExito = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-exito');
    $('#mensaje').html('La operación se realizó con éxito.');
}

const formato = /[`!@#$%^&*+={}'"\\|<>\?~\n]/;
const expresionRegular = (texto) => {
    if (formato.test(texto)) {
        return 1;
    }
    else
        return 0;
}