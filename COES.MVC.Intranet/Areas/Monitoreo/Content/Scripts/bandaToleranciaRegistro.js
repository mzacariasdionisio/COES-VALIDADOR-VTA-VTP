var controlador = siteRoot + 'Monitoreo/BandaTolerancia/';
var ancho = 1200;
$(function () {
    $('#btnNuevo').click(function () {
        editarBanda(0);
    });

    consultarBanda();
});

/////////////////////////////////////////////////////////////////////////////////
/// Actualizacion de Banda de Tolerancia
/////////////////////////////////////////////////////////////////////////////////

function consultarBanda() {
    $.ajax({
        type: 'POST',
        async: true,
        url: controlador + 'ListarBandaTolerancia',
        success: function (data) {
            $('#listadoBanda').html(data);

            $('#reporteBanda').dataTable({
                "scrollY": 400,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "bPaginate": false,
                "iDisplayLength": -1
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function editarBanda(id) {
    $('#nuevoBandaTolerancia').html('');
    $('#editarBandaTolerancia').html('');
    $('#verBandaTolerancia').html('');

    $.ajax({
        type: 'POST',
        url: controlador + "EditarBandaTolerancia",
        data: {
            id: id
        },
        success: function (evt) {
            if (id == 0) {
                $('#nuevoBandaTolerancia').html(evt);

                setTimeout(function () {
                    $('#popupNuevoBandaTolerancia').bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown'
                    });
                }, 50);

                $("#cbEstado").prop('disabled', 'disabled');

            } else {
                $('#editarBandaTolerancia').html(evt);

                setTimeout(function () {
                    $('#popupEditarBandaTolerancia').bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown'
                    });
                }, 50);

                $("#periodo").prop('disabled', 'disabled');
                $("#cbImme").prop('disabled', 'disabled');
            }

            $("#cbImme").val($("#hfImmecodi").val());
            $("#cbEstado").val($("#hfEstado").val());
            $('#periodo').unbind();
            $('#periodo').Zebra_DatePicker({
                format: 'm Y',
                onSelect: function () {
                }
            });
            $('#btnGuardarBanda').unbind();
            $('#btnGuardarBanda').click(function () {
                guardarBanda();
            });

        },
        error: function (err) {
            alert('Ha ocurrido un error');
        }
    });
}

function guardarBanda() {
    var obj = getObjetoBanda();
    if (validarBanda(obj) && confirm('¿Está seguro que desea guardar la configuración de la Banda de Tolerancia?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'GuardarBandaTolerancia',
            data: {
                id: obj.id,
                immme: obj.immme,
                periodo: obj.periodo,
                valorreferencia: obj.valorreferencia,
                valortolerancia: obj.valortolerancia,
                criterio: obj.criterio,
                normativa: obj.normativa,
                estado: obj.estado
            },
            success: function (result) {
                if (result.Resultado != "-1") {
                    alert("La configuración se registró correctamente ");
                    if (obj.id == 0) {
                        $('#popupNuevoBandaTolerancia').bPopup().close();
                    } else {
                        $('#popupEditarBandaTolerancia').bPopup().close();
                    }
                    consultarBanda();
                } else {
                    alert("Ha ocurrido un error: " + result.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error');
            }
        });
    }
}

function validarBanda(obj) {
    var msj = '';

    if (obj.immme == 0)
        msj += "Debe seleccionar un indicador\n";

    patron = /[0-9-.]/;

    if (obj.valorreferencia == null || obj.valorreferencia.trim() == "" || !patron.test(obj.valorreferencia))
        msj += "El valor de referencia no es válido\n";
    if (obj.valortolerancia == null || obj.valortolerancia.trim() == "" || !patron.test(obj.valortolerancia))
        msj += "El valor de tolerancia no es válido\n";

    if (msj != '') {
        alert(msj);
        return false;
    }
    return true;
}

function getObjetoBanda() {

    var id = parseInt($("#hfCodi").val()) || 0;
    var immme = parseInt($("#cbImme").val()) || 0;
    var periodo = $("#periodo").val();
    var valorreferencia = $("#valorreferencia").val();
    var valortolerancia = $("#valortolerancia").val();
    var criterio = $("#criterio").val();
    var normativa = $("#normativa").val();
    var estado = $("#cbEstado").val();

    var obj = {};
    obj.immme = immme;
    obj.id = id;
    obj.periodo = periodo;
    obj.valorreferencia = valorreferencia;
    obj.valortolerancia = valortolerancia;
    obj.criterio = criterio;
    obj.normativa = normativa;
    obj.estado = estado;

    return obj;
}

function verBanda(id) {
    $('#nuevoBandaTolerancia').html('');
    $('#editarBandaTolerancia').html('');
    $('#verBandaTolerancia').html('');

    $.ajax({
        type: 'POST',
        url: controlador + "VerBandaTolerancia",
        data: {
            id: id
        },
        success: function (evt) {
            $('#verBandaTolerancia').html(evt);

            setTimeout(function () {
                $('#popupVerBandaTolerancia').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function (err) {
            alert('Ha ocurrido un error');
        }
    });
}

function valida(e) {
    tecla = (document.all) ? e.keyCode : e.which;

    //Tecla de retroceso para borrar, siempre la permite
    if (tecla == 8) {
        return true;
    }

    // Patron de entrada, en este caso solo acepta numeros y punto
    patron = /[0-9-.]/;
    tecla_final = String.fromCharCode(tecla);
    return patron.test(tecla_final);
}