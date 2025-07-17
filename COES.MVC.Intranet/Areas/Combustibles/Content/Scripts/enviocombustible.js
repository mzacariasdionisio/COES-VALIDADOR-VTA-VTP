var controlador = siteRoot + 'Combustibles/gestion/';

$(function () {
    $('#tab-container').easytabs();

    $('#btnInicio').click(function () {
        regresarPrincipal();
    });
    $('#btnGrupomop').click(function () {
        var idGrupo = $("#hdIdGrupo").val();
        var idAgrup = $("#hdIdAgrup").val();
        var fechaConsulta = $("#hdFechaConsulta").val();
        var url = siteRoot + 'Migraciones/Parametro/' + "Index?grupocodi=" + idGrupo + "&agrupcodi=" + idAgrup + "&fechaConsulta=" + fechaConsulta;
        window.open(url, '_blank').focus();
    });

    //
    $('#btnMostrarErrores').click(function () {
        mostrarDetalleErrores();
    });
    $('#btnDescargarFormato').click(function () {
        exportarFormularioEnvio($('#hfIdEnvio').val());
    });

    //abrir popup vigencia
    $('#btnAprobar').click(function () {
        if (validarEnvio()) {
            popupFechaVigencia();
        }
    });
    $("#btnCancelarEnvio").click(function () {
        $('#popupFechaVigencia').bPopup().close();
    });

    $('#btnAprobarEnvio').click(function () {
        aprobarEnvio();
    });

    //observacion
    $("#fechaObsRespuesta").Zebra_DatePicker({
        format: 'd/m/Y',
        direction: 0
    });
    $('#btnObservaciones').click(function () {
        popupObservacion();
    });

    $('#btnAceptarObs').click(function () {
        guardarObservacion();
    });

    //desaprobacion
    $("#btnPopupDesaprobar").click(function () {
        popupDesaprobar();
    });
    $("#btnDesaprobarOk").click(function () {
        guardarDesaprobacion();
    });
    $("#btnDesaprobarCancel").click(function () {
        $('#popupDesaprobar').bPopup().close();
    });

    //cheks de costos variables
    $('input:radio[name="chkCvariable"]').change(
        function () {
            if ($(this).is(':checked')) {
                var idCV = $(this).val();
                var fecha = $("#fechaVigRepcodi_" + idCV).val();

                $("#fechaHoraPantalla").html(fecha + " 00:00");
                $("#FechaVigenciaDesde").val(fecha);
            }
        }
    );
    mostrarGrilla();
});

function regresarPrincipal() {
    var estadoEnvio = $("#hdIdEstado").val();
    document.location.href = controlador + "Index?carpeta=" + estadoEnvio;
}

function mostrarDetalleCV(repCodi) {
    var url = siteRoot + "Despacho/CostosVariables/ViewCostoVariable?repCodi=" + repCodi;
    window.open(url, '_blank').focus();
};

////////////////////////////////////////////////////////////////
function popupObservacion() {
    $('#txtObserv').val('');
    setTimeout(function () {
        $('#popupUnidad').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 50);
}

function guardarObservacion() {
    var observ = $('#txtObserv').val();
    if (observ == null) observ = "";
    observ = observ.trim();
    var codenvio = $("#hfIdEnvio").val();
    var fecha = $("#fechaObsRespuesta").val();

    if (observ.length > 2400) {
        alert(`Error en el envío de la observación. La cantidad de caracteres del texto es mayor a lo permitido (actual: ${observ.length}, máximo: 2400).`);
    }
    else {
        if (confirm('¿Desea enviar las observaciones al generador?')) {
            $.ajax({
                type: 'POST',
                url: controlador + 'GrabarObservacion',
                dataType: 'json',
                data: {
                    txtObserv: observ,
                    icodenvio: codenvio,
                    fechaObs: fecha
                },
                cache: false,
                success: function (evt) {
                    if (evt.Resultado != "-1") {
                        alert('Se envío la observacion al Agente.');

                        $("#hdIdEstado").val(ESTADO_OBSERVADO);
                        regresarPrincipal();
                    } else {
                        alert('Error en el envío de la observación: ' + evt.Mensaje);
                    }
                },
                error: function (err) {
                    alert("Ha ocurrido un error");
                }
            });
        }
    }
}

////////////////////////////////////////////////////////////////
function popupFechaVigencia() {
    //$('#txtObserv').val('');
    setTimeout(function () {
        $('#popupFechaVigencia').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 50);
}

function aprobarEnvio() {
    if (validarEnvio()) {
        var lstCodigocv = ObtenerListaCodigocv();

        if (lstCodigocv.length == 0) {
            alert("Debe seleccionar al menos un registro de Costos Variables");
            return;
        }

        if ($("#FechaVigenciaDesde").val() == null || $("#FechaVigenciaDesde").val() == '') {
            alert("Debe seleccionar Fecha de vigencia");
            return;
        }


        if (confirm('¿Desea aprobar el envío de costo de combustible?')) {
            var vigenciaEscogida = $("#FechaVigenciaDesde").val();

            var data = {
                idEnvio: $('#hfIdEnvio').val(),
                fechaVigencia: vigenciaEscogida,
                listaCodicocv: lstCodigocv,
                correosCCagente: $("#ccAgenteAprobar").val()
            };

            $.ajax({
                type: 'POST',
                url: controlador + 'AprobarEnvio',
                dataType: 'json',
                data: JSON.stringify(data),
                contentType: "application/json",
                cache: false,
                success: function (evt) {
                    if (evt.Resultado == "1") {
                        alert("Se efectuó la aprobación");
                        $('#popupFechaVigencia').bPopup().close();

                        $("#hdIdEstado").val(ESTADO_APROBADO);
                        regresarPrincipal();
                    }
                    else {
                        alert('Error al aprobar el envío: ' + evt.Mensaje);
                    }
                },
                error: function (err) {
                    alert('Ha ocurrido un error.');
                }
            });
        }
    }
}

function ObtenerListaCodigocv() {
    var lstCodigocv = [];
    $("input:radio[name=chkCvariable]:checked").each(function () {
        lstCodigocv.push($(this).val());
    });
    return lstCodigocv;
}

//
function exportarFormularioEnvio(idEnvio) {
    $.ajax({
        type: 'POST',
        url: controlador + "ExportarFormularioEnvio",
        dataType: 'json',
        data: {
            cbenvcodi: idEnvio
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "ExportarReporteFormulario";
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }

        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

////////////////////////////////////////////////////////////////
function popupDesaprobar() {
    $('#txtMsjDesaprobar').val('');
    setTimeout(function () {
        $('#popupDesaprobar').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 50);
}

function guardarDesaprobacion() {
    var observ = $('#txtMsjDesaprobar').val();
    var codenvio = $("#hfIdEnvio").val();

    if (confirm('¿Desea desaprobar la solicitud?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'DesaprobarEnvio',
            dataType: 'json',
            data: {
                mensaje: observ,
                idEnvio: codenvio,
                correosCCagente: $("#ccAgenteDesaprobar").val()
            },
            cache: false,
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    alert('Se desaprobó el envío.');

                    $("#hdIdEstado").val(ESTADO_DESAPROBADO);
                    regresarPrincipal();
                } else {
                    alert('Error: ' + evt.Mensaje);
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
}
