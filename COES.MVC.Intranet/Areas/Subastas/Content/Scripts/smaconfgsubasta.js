var controlador = siteRoot + 'Subastas/ConfgSubasta/';

var LISTADO_AMPLIACION = [];
var OBJETO_VALOR_AMPLIACION = null;
var IMG_EDITAR = '<img src="' + siteRoot + 'Content/Images/btn-edit.png" alt="" width="19" height="19" style="">';

$(function () {
    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').bind('easytabs:after', function () {
        refrehDatatable();
    });

    //tab 1
    pestanioSubasta();

    //tab 2
    $('#btnNuevoAmpliacion').click(function () {
        abriPopupNuevoAmpliacion();
    });
    $('#btnGuardarAmpliacion').click(function () {
        ampliacionGuardar();
    });

    ampliacionListado();
});

function pestanioSubasta() {

    var arrHoras = ['00:00', '00:30', '01:00', '01:30', '02:00', '02:30', '03:00', '03:30', '04:00', '04:30', '05:00', '05:30'
        , '06:00', '06:30', '07:00', '07:30', '08:00', '08:30', '09:00', '09:30', '10:00', '10:30', '11:00', '11:30'
        , '12:00', '12:30', '13:00', '13:30', '14:00', '14:30', '15:00', '15:30', '16:00', '16:30', '17:00', '17:30'
        , '18:00', '18:30', '19:00', '19:30', '20:00', '20:30', '21:00', '21:30', '22:00', '22:30', '23:00', '23:30', '23:59'];

    for (var i = 0, j = arrHoras.length - 1; i < j; i++) {
        $('#hor-subasta-hora-inicio').append(new Option(arrHoras[i], arrHoras[i], false, false));
    }

    for (var i = 1, j = arrHoras.length; i < j; i++) {
        $('#hor-subasta-hora-fin').append(new Option(arrHoras[i], arrHoras[i], false, false));
    }

    for (var i = 0, j = arrHoras.length; i < j; i++) {
        $('#hor-subasta-hora-envio').append(new Option(arrHoras[i], arrHoras[i], false, false));
    }

    for (var i = 1; i < 31; i++) {
        $('#maximo-dias-oferta').append(new Option(i, i, false, false));
    }

    $('#Enviar').click(function () {

        var inicio = $('#hor-subasta-hora-inicio').val();
        var fin = $('#hor-subasta-hora-fin').val();
        var ncp = $('#hor-subasta-hora-envio').val();
        var maxdiasoferta = $('#maximo-dias-oferta').val();

        if (maxdiasoferta == "") {
            mensajeOperacion("\u00BF" + "Debe ingresar el numero maximo de dias de oferta", 1);
        }
        else {
            mensajeOperacion("\u00BF" + "Esta seguro de agregar este horario: Hora Inicio: " + inicio + " - " + "Hora Fin: " + fin +
                " - " + "Hora desencriptación: " + ncp +
                " - Max. Dias de Oferta: " + maxdiasoferta + "?", null
                , {
                    showCancel: true,
                    onOk: function () {
                        $.ajax({
                            type: "POST",
                            url: controlador + "MantenimientoParametro",
                            //dataType: 'json',
                            data: {
                                inicio: $('#hor-subasta-hora-inicio').val(),
                                fin: $('#hor-subasta-hora-fin').val(),
                                envioncp: $('#hor-subasta-hora-envio').val(),
                                maxdias: $('#maximo-dias-oferta').val(),//AGREGADO POR STS - 2018
                                accion: "Nuevo"
                            },
                            success: function (result) {

                                if (result.Resultado == '-1') {
                                    alert('Ha ocurrido un error:' + result.Mensaje);
                                } else {
                                    //todos
                                    $('#btnCancelar').click();
                                    mensajeOperacion('Registro exitoso', 1);
                                }
                            },
                            error: function (req, status, error) {
                                mensajeOperacion(error);
                                validaErrorOperation(req.status);
                            }
                        });
                    },
                    onCancel: function () {

                    }
                });
        }
    });

    $('#maximo-dias-oferta').keypress(function (e) {
        if (isNaN(this.value + String.fromCharCode(e.charCode)))
            return false;
    })
        .on("cut copy paste", function (e) {
            e.preventDefault();
        });
}

//Tab 2: Crud de Motivos
async function abriPopupNuevoAmpliacion() {

    //valores por defecto
    $("#hfSmaapcodi").val(0);

    $('#txtMesProceso').unbind();
    $("#txtMesProceso").val($("#hfMesProcesoSig").val());
    $('#txtMesProceso').removeAttr('disabled');
    $('#txtMesProceso').Zebra_DatePicker({
        format: 'm Y',
        direction: [$("#hfMesProcesoActual").val(), '12 2090'],
        onSelect: function (date) {
            _actualizarValoresXDefectoPopup();
        },
    });
    _actualizarValoresXDefectoPopup();

    $("#cbEstadoAmpliacion").val("A");

    setTimeout(function () {
        $('#popupAmpliacion').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 250);
}

async function _actualizarValoresXDefectoPopup() {
    await _obtenerValorDefectoAmpliacion();

    $("#txtDiaDefecto").html(OBJETO_VALOR_AMPLIACION.PlazoDefectoDia);
    $("#txtHoraDefecto").html(OBJETO_VALOR_AMPLIACION.PlazoDefectoHora);
    $("#txtHoraDefecto2").html(OBJETO_VALOR_AMPLIACION.PlazoDefectoHora);

    $('#txtNuevoPlazo').unbind();
    $('#txtNuevoPlazo').val(OBJETO_VALOR_AMPLIACION.NuevoPlazoDefectoDia);
    $('#txtNuevoPlazo').Zebra_DatePicker({
    });

}

async function editarPopupAmpliacion(id) {
    var obj = LISTADO_AMPLIACION.find((element) => element.Smaapcodi == id);

    $("#hfSmaapcodi").val(id);

    $("#txtMesProceso").val(obj.SmaapaniomesDesc);
    $('#txtMesProceso').attr('disabled', 'disabled');  
    $("#td_mes_proceso button.Zebra_DatePicker_Icon").hide();

    await _obtenerValorDefectoAmpliacion();

    //var txtDia = obj.SmaapplazodefectoDesc .substring(0, 9);
    //var txtHora = obj.SmaapplazodefectoDesc.substring(11, 15);
    $("#txtDiaDefecto").html(OBJETO_VALOR_AMPLIACION.PlazoDefectoDia);
    $("#txtHoraDefecto").html(OBJETO_VALOR_AMPLIACION.PlazoDefectoHora);
    $("#txtHoraDefecto2").html(OBJETO_VALOR_AMPLIACION.PlazoDefectoHora);

    var txtDia = obj.SmaapnuevoplazoDesc.substring(0, 10);
    $('#txtNuevoPlazo').unbind();
    $('#txtNuevoPlazo').val(txtDia);
    $('#txtNuevoPlazo').Zebra_DatePicker({
    });

    $("#cbEstadoAmpliacion").val(obj.Smaapestado);

    setTimeout(function () {
        $('#popupAmpliacion').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 250);
}

async function _obtenerValorDefectoAmpliacion() {
    OBJETO_VALOR_AMPLIACION = null;

    return $.ajax({
        type: 'POST',
        url: controlador + "ObtenerValorDefectoAmpliacion",
        dataType: 'json',
        data: {
            mesProceso: $("#txtMesProceso").val(),
        },
        success: function (evt) {
            OBJETO_VALOR_AMPLIACION = evt;
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function _generarObjetoAmpliacionForm() {

    var obj = {};
    obj.Smaapcodi = parseInt($("#hfSmaapcodi").val()) || 0;
    obj.SmaapaniomesDesc = $("#txtMesProceso").val();
    obj.SmaapnuevoplazoDesc = $("#txtNuevoPlazo").val();
    obj.Smaapestado = $("#cbEstadoAmpliacion").val();

    return obj;
}

function ampliacionGuardar() {
    var objJson = _generarObjetoAmpliacionForm();
    var msjVal = "";

    if (msjVal == "") {
        $.ajax({
            type: 'POST',
            async: true,
            url: controlador + 'GuardarAmpliacion',
            data: {
                smaapcodi: objJson.Smaapcodi,
                mesProceso: objJson.SmaapaniomesDesc,
                fechaFinPlazo: objJson.SmaapnuevoplazoDesc,
                estado: objJson.Smaapestado,
            },
            success: function (result) {
                if (result.Resultado == "-1") {
                    alert(result.Mensaje);
                } else {
                    alert("La información ha sido guardada exitosamente.");

                    ampliacionListado();
                    $('#popupAmpliacion').bPopup().close();
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    } else {
        alert(msjVal);
    }
}

function ampliacionListado() {
    LISTADO_AMPLIACION = [];
    $("#div_listado_ampliacion").html('');

    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoAmpliacion',
        dataType: 'json',
        data: {
        },
        cache: false,
        success: function (data) {
            if (data.Resultado != "-1") {
                LISTADO_AMPLIACION = data.ListaAmpliacion;

                var html = dibujarTablaListadoAmpliacion(LISTADO_AMPLIACION);
                $("#div_listado_ampliacion").html(html);

                refrehDatatable();

            } else {
                alert("Ha ocurrido un error: " + data.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function dibujarTablaListadoAmpliacion(lista) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" width="100%" id="tblListadoAmpliacion">
        <thead>
            <tr>
                <th style='width: 70px'>Acciones</th>
                <th style='width: 40px'>Mes</th>
                <th style='width: 200px'>Plazo por defecto</th>
                <th style='width: 200px'>Nuevo plazo</th>
                <th style='width: 200px'>Estado</th>
                <th style='width: 100px'>Usuario creación</th>
                <th style='width: 100px'>Fecha creación</th>
                <th style='width: 100px'>Usuario modificación</th>
                <th style='width: 100px'>Fecha modificación</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (var key in lista) {
        var item = lista[key];

        var thAccion = '';
        if (item.EsEditable) thAccion = `<a title="Editar" href="JavaScript:editarPopupAmpliacion(${item.Smaapcodi});">${IMG_EDITAR} </a>`;

        cadena += `
            <tr>
                <td style="height: 24px">
                    ${thAccion}
                </td>
                <td style="height: 24px">${item.SmaapaniomesTexto}</td>
                <td style="height: 24px">${item.SmaapplazodefectoDesc}</td>
                <td style="height: 24px">${item.SmaapnuevoplazoDesc}</td>
                <td style="height: 24px">${item.SmaapestadoDesc}</td>
                <td style="height: 24px">${item.Smaapusucreacion}</td>
                <td style="height: 24px">${item.SmaapfeccreacionDesc}</td>
                <td style="height: 24px">${item.Smaapusumodificacion}</td>
                <td style="height: 24px">${item.SmaapfecmodificacionDesc}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function refrehDatatable() {

    $('#tblListadoAmpliacion').dataTable({
        "destroy": "true",
        "scrollY": 550,
        "scrollX": true,
        "sDom": 'ft',
        "ordering": false,
        "bPaginate": false,
        "iDisplayLength": -1
    });
}