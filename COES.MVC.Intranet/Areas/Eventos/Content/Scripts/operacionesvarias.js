var controlador = siteRoot + 'eventos/operacionesvarias/';

$(function () {

    $('#FechaDesde').Zebra_DatePicker({
    });

    $('#FechaHasta').Zebra_DatePicker({
    });

    $('#btnExportar').click(function () {
        crearReporte();

    });


    $('#btnNuevo').click(function () {
        editar(0, 1, $('#cbEvenclase').val(), $('#cbSubcausa').val());
    });


    $('#btnBuscar').click(function () {
        buscar();
    });


    $('#btnCopiarBloque').click(function () {
        copiarRegistro($('#FechaHasta').val());
    });

    $('#btnCopiarHorizonte').click(function () {
        copiarHorizonte(0);
    });

    $('#btnEliminarMasivo').click(function () {
        eliminarmasivo();
    }); 

    $('#btnGuardarLineasMasivo').click(function () {
        obtenerRegistrosLineas();
    }); 
    $(document).ready(function () {
        $('#cbEvenclase').val($('#hfHorizonte').val());
        $('#cbSubcausa').val($('#hfTipoOperacion').val());
        buscar();
    });

});

function convertirFecha(dateStr) {
    var parts = dateStr.split("/");
    return new Date(parts[2], parts[1] - 1, parts[0]);
}

buscar = function () {

    var fechaini = convertirFecha($('#FechaDesde').val());
    var fechafin = convertirFecha($('#FechaHasta').val());
    $('#hfHorizonte').val($('#cbEvenclase').val());
    $('#hfTipoOperacion').val($('#cbSubcausa').val());

    if (fechaini <= fechafin) {
        pintarPaginado();
        mostrarListado(1);
    } else {
        alert("Fecha inicial supera a la final");
    }

}

pintarBusqueda = function (nroPagina) {
    mostrarListado(nroPagina);
}


validarHora = function (hora, mensajeHora) {

    var result = false;

    if (hora == "24:00")
        return true;

    var re = /^\s*([01]?\d|2[0-3]):?([0-5]\d)\s*$/;
    if ((m = hora.match(re))) {
        return true;
    }
    return false;

}


validarHoras = function (fechaBase, horaInicial, horaFinal) {

    var result = "";

    if (!validarHora(horaInicial)) {
        result += fechaBase + ": Hora Inicial no es correcta";
    }

    if (!validarHora(horaFinal)) {
        if (result == "")
            result += fechaBase + ": Hora Final no es correcta";
        else
            result += ". Hora Final no es correcta";
    }

    //validacion de horas
    if (horaFinal != "24:00") {
        if (Date.parse("01/01/2016 " + horaInicial + ":00") >= Date.parse("01/01/2016 " + horaFinal + ":00")) {
            if (result == "")
                result += fechaBase + ": Hora Inicial supera o iguala a Final";
            else
                result += ". Hora Inicial supera o iguala a Final";
        }
    }

    return result;
}


copiarBloqueSemanal = function () {

    //validación de horas
    var mensaje = "";
    if ($('#cbxCopiar1').is(':checked')) mensaje += validarHoras($("#hfFecha1").val(), $('#horaini1').val(), $('#horafin1').val());
    if ($('#cbxCopiar2').is(':checked')) mensaje += validarHoras($("#hfFecha2").val(), $('#horaini2').val(), $('#horafin2').val());
    if ($('#cbxCopiar3').is(':checked')) mensaje += validarHoras($("#hfFecha3").val(), $('#horaini3').val(), $('#horafin3').val());
    if ($('#cbxCopiar4').is(':checked')) mensaje += validarHoras($("#hfFecha4").val(), $('#horaini4').val(), $('#horafin4').val());
    if ($('#cbxCopiar5').is(':checked')) mensaje += validarHoras($("#hfFecha5").val(), $('#horaini5').val(), $('#horafin5').val());
    if ($('#cbxCopiar6').is(':checked')) mensaje += validarHoras($("#hfFecha6").val(), $('#horaini6').val(), $('#horafin6').val());
    if ($('#cbxCopiar7').is(':checked')) mensaje += validarHoras($("#hfFecha7").val(), $('#horaini7').val(), $('#horafin7').val());

    if (mensaje == "") {

        if ($('#cbxCopiar1').is(':checked')) { $('#hfIccheck1').val("S"); } else $('#hfIccheck1').val("N");
        if ($('#cbxCopiar2').is(':checked')) { $('#hfIccheck2').val("S"); } else $('#hfIccheck2').val("N");
        if ($('#cbxCopiar3').is(':checked')) { $('#hfIccheck3').val("S"); } else $('#hfIccheck3').val("N");
        if ($('#cbxCopiar4').is(':checked')) { $('#hfIccheck4').val("S"); } else $('#hfIccheck4').val("N");
        if ($('#cbxCopiar5').is(':checked')) { $('#hfIccheck5').val("S"); } else $('#hfIccheck5').val("N");
        if ($('#cbxCopiar6').is(':checked')) { $('#hfIccheck6').val("S"); } else $('#hfIccheck6').val("N");
        if ($('#cbxCopiar7').is(':checked')) { $('#hfIccheck7').val("S"); } else $('#hfIccheck7').val("N");

        if ($('#cbxCopiarTexto').is(':checked')) { $('#hfCopiarTexto').val("S"); } else $('#hfCopiarTexto').val("N");


        $.ajax({
            type: 'POST',
            url: controlador + "/copiarBloqueSemana",
            dataType: 'json',
            data: $('#formCopiarBloque').serialize(),
            success: function (result) {
                if (result >= 1) {
                    $('#popupEdicion').bPopup().close();
                    mostrarExito();
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
    else {
        mostrarAlerta(mensaje);

    }

}

copiarBloqueDiario = function () {

    if ($('#cbxCopiarTexto').is(':checked')) { $('#hfCopiarTexto').val("S"); } else $('#hfCopiarTexto').val("N");

    $.ajax({
        type: 'POST',
        url: controlador + "/copiarBloqueDia",
        dataType: 'json',
        data: $('#formCopiarBloque').serialize(),
        success: function (result) {
            if (result >= 1) {
                $('#popupEdicion').bPopup().close();
                mostrarExito();
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


crearReporte = function () {

    if ($('#cbSubcausa').val() == 0) {
        alert('Debe elegir tipo de operación');
        return;
    }

    $.ajax({
        type: 'POST',
        url: controlador + "listasinpaginado",
        data: {
            evenClase: $('#cbEvenclase').val(),
            subCausacodi: $('#cbSubcausa').val(),
            fechaIni: $('#FechaDesde').val(),
            fechaFin: $('#FechaHasta').val(),
            nombreOperacion: $('#cbSubcausa').find('option:selected').text(),
            evenClaseDescripcion: $('#cbEvenclase').find('option:selected').text()
        },
        success: function (resultado) {
            if (resultado != 1) {
                mostrarError();
            } else {
                window.location = controlador + "descargar";
            }

        },
        error: function () {
            mostrarError();
        }
    });
}


mostrarExito = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-exito');
    $('#mensaje').html('La operación se realizó con éxito.');
}


mostrarAlerta = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-alert');
    $('#mensaje').html(mensaje);
}


pintarPaginado = function () {

    $.ajax({
        type: 'POST',
        url: controlador + "/paginado",
        data: {
            evenClase: $('#cbEvenclase').val(),
            subCausacodi: $('#cbSubcausa').val(),
            fechaIni: $('#FechaDesde').val(),
            fechaFin: $('#FechaHasta').val()
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            mostrarError();
        }
    });
}


mostrarListado = function (nroPagina) {

    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "lista",
        data: {
            evenClase: $('#cbEvenclase').val(),
            subCausacodi: $('#cbSubcausa').val(),
            fechaIni: $('#FechaDesde').val(),
            fechaFin: $('#FechaHasta').val(),
            nroPage: nroPagina
        },
        success: function (evt) {

            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50
            });

            $('#cbSelectAll').click(function (e) {

                //var table = $('#tabla');
                var table = $('#listado');
                $('td input:checkbox.checkbox-grupo', table).prop('checked', this.checked);
            });
        },
        error: function () {
            mostrarError();
        }
    });
}

mostrarError = function () {
    alert('Ha ocurrido un error.');
}


getCheckedBoxes = function (chkboxName) {
    var checkboxes = document.getElementsByName(chkboxName);
    var checkboxesChecked = [];

    for (var i = 0; i < checkboxes.length; i++) {

        if (checkboxes[i].checked) {
            checkboxesChecked.push(checkboxes[i]);
        }
    }

    return checkboxesChecked.length > 0 ? checkboxesChecked : null;
}


eliminarEvento = function (id) {

    if (confirm('¿Está seguro de eliminar este registro?')) {

        $.ajax({
            type: 'POST',
            url: controlador + "eliminar",
            dataType: 'json',
            cache: false,
            data: { idIccodi: id },
            success: function (resultado) {
                if (resultado == 1) {
                    buscar();
                } else {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

eliminarmasivo = function () {
    var iccodi = "";

    var table = $('#listado');
    $('td input:checkbox', table).each(function () {   
        iccodi = iccodi + (iccodi != "" ? "," : "");
        iccodi = iccodi + $(this).val();
    });

    if (iccodi.length > 0) {
        $.ajax({
            type: 'POST',
            url: controlador + "eliminarMasivo",
            dataType: 'json',
            cache: false,
            data: { idIccodis: iccodi },
            success: function (resultado) {
                if (resultado == 1) {
                    buscar();
                } else {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    } else {
        alert("Debe seleccionar registros previamente...");
        return;
    }

}

editarRegistro = function (id) {
    $.ajax({
        type: 'POST',
        data: { iccodi: id },
        url: controlador + 'editar',
        success: function (evt) {

            $('#contenidoEdicion').html(evt);
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    autoClose: false
                });

            },
                50);
            $('#btnGrabar').click(function () {
                grabarRegistro();
            });

            $('#btnCancelar').click(function () {
                $('#popupEdicion').bPopup().close();
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function formatearFecha(fecha) {
    var dia = String(fecha.getDate()).padStart(2, '0');
    var mes = String(fecha.getMonth() + 1).padStart(2, '0'); // Los meses son indexados desde 0
    var año = fecha.getFullYear();
    var horas = String(fecha.getHours()).padStart(2, '0');
    var minutos = String(fecha.getMinutes()).padStart(2, '0');
    var segundos = String(fecha.getSeconds()).padStart(2, '0');

    return `${dia}/${mes}/${año} ${horas}:${minutos}:${segundos}`;
}

grabarAutomatico = function (object) {
    var jsonObject = JSON.parse(object);
    var fechaini = new Date(jsonObject.Ichorini);
    var fechafin = new Date(jsonObject.Ichorfin);

    jsonObject.Ichorini = formatearFecha(fechaini);
    jsonObject.Ichorfin = formatearFecha(fechafin);

    $.ajax({
        type: 'POST',
        url: controlador + "grabar",
        dataType: 'json',
        data: jsonObject,
        success: function (result) {
            if (result > 1) {
                mostrarExito();
                $('#txthp').val("");
                $('#txtfp').val("");

                $('#hficcodi').val(result);
                console.log(result);
                console.log(object);

                buscar();

                //grabarEvento1(result);

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

grabarRegistro = function () {
    return;
}

editar = function (id, accion, horz, top) {
    document.location.href = controlador + "editar?id=" + id + "&accion=" + accion + "&horz=" + horz + "&top=" + top;
}


copiarEvento = function (id) {
    document.location.href = controlador + "copiaranuevo?id=" + id;
}


obtenerRegistros = function () {
    //obtener check
    var iccodi = "";

    $('#tablaArea input:checked').each(function () {
        iccodi = iccodi + $(this).val() + ",";
    });

    return iccodi;
}

obtenerRegistrosLineas = function () {
    var iccodis = "";
    $('#tablaArea .checkbox-grupo-lineas:checked').each(function () {
        iccodis = iccodis + $(this).val() + ",";
        grabarAutomatico($(this).val());
    });
    if (iccodis == "") {
        alert("Debe seleccionar los equipos a registrar...");
        return;
    }

    console.log("Codigos Lineas: ", iccodis);
    return iccodis;
}


copiarRegistro = function (fechaFin) {

    var iccodis = obtenerRegistros();

    if (iccodis == "") {
        alert("Debe seleccionar registros previamente...");
        return;
    }

    $.ajax({
        type: 'POST',
        data: { fechaFin: fechaFin },
        url: controlador + 'copiar',
        success: function (evt) {

            $('#contenidoEdicion').html(evt);
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    autoClose: false
                });

            },
                50);

            $('#btnCopiarBloqueSemana').click(function () {
                copiarBloqueSemanal();
            });

            $('#btnCopiarBloqueDia').click(function () {
                copiarBloqueDiario();
            });

            $('#titulo').text("Copiar bloque");
            $('#hfIccodis').val(iccodis);

        },
        error: function (xhr, ajaxOptions, thrownError) {
            //alert(xhr.responseText);
            alert("Ha ocurrido un error");
        }
    });
}


copiarHorizonte = function (id2) {

    var subcausaDescripcion = $('#cbSubcausa').find('option:selected').text();
    var subcausaCodigo = $('#cbSubcausa').find('option:selected').val();
    var fecha = $('#FechaHasta').val();

    $.ajax({
        type: 'POST',
        data: {
            id: subcausaCodigo,
            txt: subcausaDescripcion
        },
        url: controlador + 'CopiarHorizonte',
        success: function (evt) {

            $('#contenidoEdicion').html(evt);
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    autoClose: false
                });

            }, 50);

            $('#btncancelcopy').click(function () {
                $('#popupEdicion').bPopup().close();
            });

            $('#titulo').text("Copiar datos de horizonte");

            //Copiado horizonte
            $('#FechaDesdeHorizonteHorizonte').Zebra_DatePicker({
            });

            $('#FechaHastaHorizonteHorizonte').Zebra_DatePicker({
            });

            $('#FechaDesdeEjecutadoDiario').Zebra_DatePicker({
            });

            $('#FechaHastaEjecutadoDiario').Zebra_DatePicker({
            });

            $('#FechaDesdeHorizonteHorizonte').val(fecha);
            $('#FechaHastaHorizonteHorizonte').val(fecha);

            var parteFecha = fecha.split("/");
            var fechaInicial = new Date(Number(parteFecha[2]), Number(parteFecha[1] - 1), Number(parteFecha[0]));

            fechaInicial.setDate(fechaInicial.getDate() + 1);

            var dia = ((fechaInicial.getDate() < 10) ? "0" : "") + (fechaInicial.getDate());
            var mes = ((fechaInicial.getMonth() + 1 < 10) ? "0" : "") + (fechaInicial.getMonth() + 1);
            var anho = fechaInicial.getFullYear();

            var fechafin = dia + '/' + mes + '/' + anho;

            $('#FechaDesdeEjecutadoDiario').val(fechafin);
            $('#FechaHastaEjecutadoDiario').val(fechafin);

            if (subcausaCodigo == 0) {
                $('#OperacionUnica').hide();
                $('#rbOperacionTodas').prop('checked', true);
            }
            else {
                $('#tipoOperacion').text(subcausaDescripcion);
                $('#rbOperacionTodas').prop('checked', false);
                $('#rbOperacionUnica').prop('checked', true);
            }

            //activacion de botones            
            $('#btnMensualSemanal').click(function () {
                copiarHorizonteHorizonte(4, 3, $('#FechaDesdeHorizonteHorizonte').val(), $('#FechaHastaHorizonteHorizonte').val(), "mensual a semanal");
            });

            $('#btnSemanalDiario').click(function () {
                copiarHorizonteHorizonte(3, 2, $('#FechaDesdeHorizonteHorizonte').val(), $('#FechaHastaHorizonteHorizonte').val(), "semanal a diario");
            });

            $('#btnAnualMensual').click(function () {
                copiarHorizonteHorizonte(5, 4, $('#FechaDesdeHorizonteHorizonte').val(), $('#FechaHastaHorizonteHorizonte').val(), "anual a mensual");
            });

            $('#btnDiarioEjecutado').click(function () {
                copiarHorizonteHorizonte(2, 1, $('#FechaDesdeHorizonteHorizonte').val(), $('#FechaHastaHorizonteHorizonte').val(), "diario a ejecutado");
            });

            $('#btnEjecutadoDiario').click(function () {

                if (($('#FechaDesdeHorizonteHorizonte').val() == $('#FechaHastaHorizonteHorizonte').val())
                    &&
                    ($('#FechaDesdeEjecutadoDiario').val() == $('#FechaHastaEjecutadoDiario').val())
                )
                    copiarEjecutadoDiario(1, 2, $('#FechaDesdeHorizonteHorizonte').val(), $('#FechaHastaEjecutadoDiario').val(), "ejecutado al diario");
                else {
                    alert("Copiado de datos solo puede ser de un día desde origen a destino");
                }
            });

        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.responseText);
        }
    });
}

function convertirFecha(dateStr) {
    var parts = dateStr.split("/");
    return new Date(parts[2], parts[1] - 1, parts[0]);
}

copiarHorizonteHorizonte = function (evenClasecodiOrigen, evenClasecodiDestino, fechaInicial, fechaFinal, textoAdicional) {

    var fechaini = convertirFecha(fechaInicial);
    var fechafin = convertirFecha(fechaFinal);

    if (fechaini > fechafin) {
        alert("Fecha inicial supera a la final");
        return;
    }


    var subCausaDescripcion = $('#cbSubcausa').find('option:selected').text();
    var subCausaCodigo;

    if ($('#rbOperacionTodas').is(':checked')) {
        subCausaCodigo = 0;
    }
    else {
        subCausaCodigo = $('#cbSubcausa').find('option:selected').val()
    }

    var respuesta = confirm("Desea actualizar la información del " + textoAdicional + " del día " + fechaInicial + " al día " + fechaFinal + ((subCausaCodigo != 0) ? " (" + subCausaDescripcion + ")" : "") + " ?");

    if (!respuesta)
        return;

    $.ajax({
        type: 'POST',
        data: {
            evenClasecodiOrigen: evenClasecodiOrigen,
            evenClasecodiDestino: evenClasecodiDestino,
            fechaIni: fechaInicial,
            fechaFin: fechaFinal,
            subCausacodi: subCausaCodigo
        },
        url: controlador + 'copiarRegistrosHorizonteHorizonte',
        success: function (result) {
            if (result >= 0) {
                $('#popupEdicion').bPopup().close();
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


copiarEjecutadoDiario = function (evenClasecodiOrigen, evenClasecodiDestino, fechaOrigen, fechaDestino, textoAdicional) {

    var respuesta = confirm("Desea actualizar la información del " + textoAdicional + " del día " + fechaOrigen + " al día " + fechaDestino + " ?");

    var subCausaCodigo;

    if ($('#rbOperacionTodas').is(':checked')) {
        subCausaCodigo = 0;
    }
    else {
        subCausaCodigo = $('#cbSubcausa').find('option:selected').val()
    }

    if (!respuesta)
        return;

    $.ajax({
        type: 'POST',
        data: {
            evenClasecodiOrigen: evenClasecodiOrigen,
            evenClasecodiDestino: evenClasecodiDestino,
            fechaOrigen: fechaOrigen,
            fechaDestino: fechaDestino,
            subCausacodi: subCausaCodigo
        },
        url: controlador + 'copiarRegistrosHorizonteyFecha',
        success: function (result) {
            if (result >= 0) {
                $('#popupEdicion').bPopup().close();
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


activaRadio = function (nombreRadio) {

    $("#rbOperacionTodas").attr("checked", (nombreRadio == "rbOperacionTodas"));
    $("#rbOperacionUnica").attr("checked", (nombreRadio == "rbOperacionUnica"));

}
