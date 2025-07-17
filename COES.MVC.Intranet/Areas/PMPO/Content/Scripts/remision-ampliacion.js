var controlador = siteRoot + 'PMPO/ConfiguracionPlazos/';

var ANCHO_REPORTE = 1000;

$(function () {
    //$('#cntMenu').css("display", "none");

    $('#txtMesElaboracion').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
        }
    });

    $('#btn-accept-report').click(function () {
        consultarAmpliacion();
    });

    //popup
    $('#btn-nuevo-ampliacion').click(function () {
        $("#ampl-div-message").hide();

        $("#ampl-ddl-companies").removeAttr("disabled");
        $("#ampl-txtMesElaboracion").removeAttr("disabled");
        $("#ampl-ddl-information-types").removeAttr("disabled");

        setTimeout(function () {
            $('#popupAmpliacionPlazo').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown',
                modalClose: false
            });
        }, 50);
    });
    $('#ampl-btn-save').click(function () {
        guardarAmpliacion();
    });

    $('#ampl-txtMesElaboracion').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
        }
    });
    $('#ampl-fechaElaboracion').Zebra_DatePicker({});
    $("#ampl-ddl-companies").change(function () {
        listarFormatoXEmpresa();
    });

    //
    consultarAmpliacion();
});

function consultarAmpliacion() {
    var emprcodi = parseInt($("#ddl-companies").val()) || 0;
    var formatcodi = parseInt($("#ddl-information-types").val()) || 0;
    var mesElaboracion = $("#txtMesElaboracion").val();

    ANCHO_REPORTE = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 1100;

    $.ajax({
        type: 'POST',
        url: controlador + "ListaAmpliacion",
        dataType: 'json',
        data: {
            emprcodi: emprcodi,
            formatcodi: formatcodi,
            mesElaboracion: mesElaboracion,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                $("#reporte").html(dibujarTablaReporte(evt.ListaAmpliacion, mesElaboracion));

                $('#listado').css("width", ANCHO_REPORTE + "px");
                $('#table-resumen').dataTable({
                    "scrollY": 430,
                    "scrollX": true,
                    "sDom": 'ft',
                    "ordering": false,
                    "iDisplayLength": -1
                });

                mostrarPlazoMes(evt.PlazoFormato);
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function dibujarTablaReporte(lista, mesElaboracion) {

    var cadena = '';
    cadena += `
    <div style='clear:both; height:5px'></div>
    <table id='table-resumen' border='1' class='pretty tabla-icono' cellspacing='0'>
        <thead>
            <tr>
                <th>Editar</th>
                <th>Empresa</th>
                <th>Formato</th>
                <th>Mes</th>
                <th style='background-color: #FFA500;'>Ampliación hasta</th>
                <th>Último Usuario</th>
                <th>Fecha de Modificación</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        cadena += `
            <tr>
                <td style='text-align: center;' onclick='editarAmpliacion("${mesElaboracion}", ${lista[key].Formatcodi}, ${lista[key].Emprcodi});' style='cursor:pointer'>
                    <a>
                        <img src="${siteRoot}Content/Images/btn-edit.png" />
                    </a>
                </td>
                <td style='text-align: center;'>${lista[key].Emprnomb}</td>
                <td style='text-align: center;'>${lista[key].Formatnombre}</td>
                <td style='text-align: center;height: 26px;'>${lista[key].AmplifechaDesc}</td>
                <td style='text-align: center;'>${lista[key].AmplifechaplazoDesc}</td>

                <td style='text-align: center;'>${lista[key].Lastuser}</td>
                <td style='text-align: center;'>${lista[key].LastdateDesc}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function mostrarPlazoMes(obj) {
    $("#btn-notificar-apertura").hide();
    $("#btn-notificar-vencimiento").hide();
    $("#mensajePlazoPmpo").html('');
    $("#mensajePlazoPmpo").hide();

    var html = '';
    html += `
        <div class="pull">
            <b class="color-blue">Periodo:</b>
            <span id="lbl-period">${obj.Periodo}</span>
        </div>
        <div class="pull" style='display: none'>
            <b class="color-blue">Unidad:</b>
            <span id="lbl-unitmeasure" data-id="1" data-symbol="MW">Potencia (MW)</span>
            <a id="btn-unitmeasure-change" href="#">Cambiar</a>
        </div>
        `;

    if (obj.EsCerrado) {
        html += `
        <div class="pull">
            La Extranet se encuentra <b class="color-blue">CERRADO</b>. Solo se permite ampliación por empresa.
        </div>
        `;
    }

    if (!obj.EsCerrado) {
        html += `
        <div id="div-tiempo-restante" class="pull">
            <b class="color-blue">Apertura de envío de información:</b>
            <span id="lbl-clock">${obj.FechaPlazoIniDesc}</span>
        </div>
        <div id="div-fecha-max-remision" class="pull">
            <b class="color-blue">Fecha máxima de remisión:</b>
            <span id="lbl-deadline-for-submission">${obj.FechaPlazoFueraDesc}</span>
        </div>
        `;

        $("#btn-notificar-apertura").show();
        $("#btn-notificar-vencimiento").show();
        $("#mensajePlazoPmpo").show();
    }

    $("#mensajePlazoPmpo").show();
    $("#mensajePlazoPmpo").html(html);
}

//Nueva ampliacion
function listarFormatoXEmpresa() {
    var emprcodi = parseInt($("#ampl-ddl-companies").val()) || 0;

    $("#ampl-ddl-information-types").empty()

    $.ajax({
        type: 'POST',
        url: controlador + "ListaFormatoXEmpresa",
        dataType: 'json',
        data: {
            emprcodi: emprcodi,
        },
        success: function (model) {
            if (model.Resultado != "-1") {

                for (var i = 0; i < model.ListaFormato.length; i++) {
                    var obj = model.ListaFormato[i];
                    $('#ampl-ddl-information-types').append('<option value="' + obj.Formatcodi + '">' + obj.Formatnombre + '</option>');
                }

            } else {
                alert("Ha ocurrido un error: " + model.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function guardarAmpliacion() {
    var emprcodi = parseInt($("#ampl-ddl-companies").val()) || 0;
    var formatcodi = parseInt($("#ampl-ddl-information-types").val()) || 0;
    var mesElaboracion = $("#ampl-txtMesElaboracion").val();
    var fecha = $("#ampl-fechaElaboracion").val();
    var horaAmpl = parseInt($("#ampl-cbHora").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "GuardarAmpliacion",
        dataType: 'json',
        data: {
            emprcodi: emprcodi,
            formatcodi: formatcodi,
            mesElaboracion: mesElaboracion,
            sfechaAmpl: fecha,
            horaAmpl: horaAmpl
        },
        success: function (model) {
            $("#ampl-div-message").show();

            if (model.Resultado != "-1") {
                mostrarMensaje("ampl-div-message", "Se guardó correctamente", $tipoMensajeExito, $modoMensajeCuadro);
                $('#popupAmpliacionPlazo').bPopup().close();

                consultarAmpliacion();
            } else {
                mostrarMensaje("ampl-div-message", model.Mensaje, $tipoMensajeAlerta, $modoMensajeCuadro);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function editarAmpliacion(mesElaboracion, formatcodi, emprcodi) {
    $("#ampl-ddl-companies").val(emprcodi);
    $("#ampl-txtMesElaboracion").val(mesElaboracion);
    $("#ampl-ddl-information-types").empty()

    $("#ampl-ddl-companies").prop('disabled', 'disabled');
    $("#ampl-txtMesElaboracion").prop('disabled', 'disabled');
    $("#ampl-ddl-information-types").prop('disabled', 'disabled');

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerAmpliacion",
        dataType: 'json',
        data: {
            emprcodi: emprcodi,
            formatcodi: formatcodi,
            mesElaboracion: mesElaboracion,
        },
        success: function (model) {
            if (model.Resultado != "-1") {

                $("#ampl-div-message").hide();
                setTimeout(function () {
                    $('#popupAmpliacionPlazo').bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown',
                        modalClose: false
                    });
                }, 50);

                //setear datos
                $("#ampl-fechaElaboracion").val(model.Fecha);
                $("#ampl-cbHora").val(model.Ampliacion.MediaHora);

                for (var i = 0; i < model.ListaFormato.length; i++) {
                    var obj = model.ListaFormato[i];
                    $('#ampl-ddl-information-types').append('<option value="' + obj.Formatcodi + '">' + obj.Formatnombre + '</option>');
                }
                $("#ampl-ddl-information-types").val(formatcodi);
            } else {
                alert("Ha ocurrido un error: " + model.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}