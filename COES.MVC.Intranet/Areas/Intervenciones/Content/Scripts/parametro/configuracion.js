var controladorParametro = siteRoot + 'Intervenciones/parametro/';
var controladorIntervenciones = siteRoot + 'Intervenciones/Registro/';

$(function () {
    $('#tab-container').easytabs();

    $('#txtHoraEjecucion').mask("99:99:99");

    $('#btnGuardarHoraE').on("click", function () {
        if (ValidarHoraEjecucion() === "") {
            GuardarHoraEjecucion();
        }
        else {
            $('#PProceso').show();
            $('#PProceso').removeClass();
            $('#PProceso').html(ValidarHoraEjecucion());
            $('#PProceso').addClass('action-alert');
        }
    });

    $('#btnEjecutarEstructuraFS').on("click", function () {
        ejecutarNuevaEstructuraFS();
    });

    $('#txtFechaConsulta').inputmask({
        mask: "1/2/y h:s",
        placeholder: "dd/mm/yyyy hh:mm",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#txtFechaConsulta').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtFechaConsulta').val(date + " 00:00");
            actualizarVistaPrevia();
        }
    });


    $("#txtFechaConsulta").on('change', function () {
        actualizarVistaPrevia();
    });

    $("#btnGuardar").click(function () {
        grabarPlazoReversion();
    });

    $('#DiaPlazo').change(function () {
        actualizarVistaPrevia();
    });
    $('#txtMinPlazo').change(function () {
        actualizarVistaPrevia();
    });

    $('#DiaPlazo').keyup(function () {
        actualizarVistaPrevia();
    });
    $('#txtMinPlazo').keyup(function () {
        actualizarVistaPrevia();
    });

    $('#DiaPlazo').blur(function () {
        actualizarVistaPrevia();
    });
    $('#txtMinPlazo').blur(function () {
        actualizarVistaPrevia();
    });

    $('#cboTipoProgramacion').on("change", function () {
        var tipoPrograma = parseInt($("#cboTipoProgramacion").val()) || 0;
        if (tipoPrograma > 0) {
            consultarParametros(tipoPrograma);
        }
        else {
            $("#DiaPlazo").val("");
            $('#txtMinPlazo').val("");
        }
    });

    $("#btnEliminarRevertidos").click(function () {
        eliminarProcesoReversion();
    });
    //consultarParametros(0);

    $('#txtHoraEjecucionAprobacion').mask("99:99");
    $('#cboTipoProgramAprobacion').on("change", function () {
        var tipoPrograma = parseInt($("#cboTipoProgramAprobacion").val()) || 0;
        if (tipoPrograma > 0) {
            obtenerAprobacionAuto(tipoPrograma);
        }
    });

    $("#btnGuardarAprobacion").click(function () {
        guardarEjecucionAprobacion();
    });

    //cargar archivos
    $('#btnCargarArchivo').on("click", function () {
        cargarArchivoAutomatico();
    });

    //porcentaje similitud
    $('#txtValorPorcentaje').mask("999");
    $('#btnGuardarPorcentaje').on("click", function () {
        guardarPorcentajeSimilitud();
    });

    //pestaña 
    cargarHtmlSubcarpeta();

    //pestaña porcentaje similitud
    obtenerPorcentajeSimilitud();
});

//cargar archivos
function cargarArchivoAutomatico() {
    if (confirm("¿Desea realizar la carga automática de archivos?")) {
        $.ajax({
            url: controladorParametro + "CargarArchivosAutomatico",
            contentType: "application/json; charset=utf-8",
            type: 'POST',
            dataType: 'json',
            success: function (model) {
                if (model.Resultado !== '-1') {
                    $('#PProceso').removeClass();
                    $('#PProceso').addClass('action-exito');
                    $('#PProceso').text('Se procesó correctamente!');
                } else {
                    $('#PProceso').removeClass();
                    $('#PProceso').addClass('action-error');
                    $('#PProceso').text('Error al cargar archivos');
                }
            },
            error: function (result) {
                $('#PProceso').removeClass();
                $('#PProceso').addClass('action-alert');
                $('#PProceso').html('no se obtuvo conexión con el servidor');
            }
        });
    }
}

//plazo
function consultarParametros(tipoProgramacion) {
    //cargarHtmlSubcarpeta(); 
    $.ajax({
        type: 'POST',
        url: controladorParametro + "ObtenerParametroPlazo",
        dataType: 'json',
        data: {
            tipoProgramacion: tipoProgramacion
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                if (tipoProgramacion == 2) {
                    $("#tituloPlazo").html("Hasta");
                }
                else {
                    $("#tituloPlazo").html("Plazo en Reversión");
                }

                $("#DiaPlazo").val(evt.ParametroPlazo.DiaPlazo);
                var confIni = obtenerDatoHoraMin(evt.ParametroPlazo.MinutoPlazo);
                $('#txtMinPlazo').val(confIni[0] + ":" + confIni[1]);
                $('#txtMinPlazo').mask('00:00');

                $("#txtFechaConsulta").val(evt.ParametroPlazo.FechaConsulta);
                $("#txtUsuarioModifReversion").text(evt.ParametroPlazo.UsuarioModificacion);
                $("#txtFechaModifReversion").text(evt.ParametroPlazo.FechaModificacion);
                actualizarVistaPrevia();
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function obtenerDatoHoraMin(hfMinutos) {
    var arrayHM = [];

    var txtHoras = "00" + Math.floor(hfMinutos / 60).toString();
    var horas = txtHoras.substr(txtHoras.length - 2, 2);
    var txtMinutos = "00" + (hfMinutos % 60).toString();
    var minutos = txtMinutos.substr(txtMinutos.length - 2, 2);

    arrayHM.push(horas);
    arrayHM.push(minutos);

    return arrayHM;
}

function convertirHoraAMinutos(minPlazo) {
    var mPlazo = parseInt(minPlazo.substr(0, 2)) * 60 + parseInt(minPlazo.substr(3, 2));
    mPlazo = parseInt(mPlazo) || 0;

    return mPlazo;
}

//convierte 2 cadenas de texto fecha(dd/mm/yyyy) y horas(hh:mm:ss) a tipo Date
function convertStringToDate(fecha, horas) {
    var partsFecha = fecha.split('/');
    if (horas == "") {
        return "";
    }
    var partsHoras = horas.split(':');
    return new Date(partsFecha[2], partsFecha[1] - 1, partsFecha[0], partsHoras[0], partsHoras[1], partsHoras[2]);
}


function actualizarVistaPrevia() {
    var tipoPrograma = parseInt($("#cboTipoProgramacion").val()) || 0;

    var strFecha = $("#txtFechaConsulta").val();
    var strHoraConsulta = $("#txtFechaConsulta").val().substr(11, 5) + ":00";
    strFecha = strFecha.substr(0, 2) + "/" + strFecha.substr(3, 2) + "/" + strFecha.substr(6, 4);

    if (tipoPrograma == 2) {
        //var Fecha = strFecha;
        var horaHasta = $("#txtMinPlazo").val() + ":00";

        var plazinidia = parseInt($("#DiaPlazo").val()) || 0;
        var plazinimin = convertirHoraAMinutos($("#txtMinPlazo").val());
        //var horaenminutos = convertirHoraAMinutos($("#txtMinPlazo").val());

        var dIniPlazo = moment(convertStringToDate(strFecha, strHoraConsulta));
        var dFinPlazo = moment(convertStringToDate(strFecha, horaHasta));

        dFinPlazo = dFinPlazo.add(plazinidia, 'days');

        $("#txtFechaPlazo1").text(dIniPlazo.format('DD/MM/YYYY HH:mm'));
        $("#txtFechaPlazo2").text(dFinPlazo.format('DD/MM/YYYY HH:mm'));
    }
    else {
        //var Fecha = strFecha;
        var plazinidia = parseInt($("#DiaPlazo").val()) || 0;
        var plazinimin = convertirHoraAMinutos($("#txtMinPlazo").val());

        var dIniPlazo = moment(convertStringToDate(strFecha, strHoraConsulta));
        var dFinPlazo = moment(convertStringToDate(strFecha, strHoraConsulta));

        dFinPlazo = dFinPlazo.add(plazinidia, 'days').add(plazinimin, 'minutes');

        $("#txtFechaPlazo1").text(dIniPlazo.format('DD/MM/YYYY HH:mm'));
        $("#txtFechaPlazo2").text(dFinPlazo.format('DD/MM/YYYY HH:mm'));
    }
}

//Guardar plaxo reversión
function grabarPlazoReversion() {

    var msj = validarReversion();

    if (msj == "") {
        actualizarVistaPrevia();
        var tipoProgramacion = parseInt($("#cboTipoProgramacion").val()) || 0;
        var diaPlazo = parseInt($("#DiaPlazo").val()) || 0;

        var minPlazo = $('#txtMinPlazo').val();
        var minutoPlazo = parseInt(minPlazo.substr(0, 2)) * 60 + parseInt(minPlazo.substr(3, 2));

        $.ajax({
            type: 'POST',
            url: controladorParametro + "GuardarParametroPlazoReversion",
            dataType: 'json',
            data: {
                diaPlazo: diaPlazo,
                minutoPlazo: minutoPlazo,
                tipoProgramacion: tipoProgramacion
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    alert("Se actualizó correctamente");

                    consultarParametros(tipoProgramacion);
                } else {
                    alert("Ocurrió un error: " + evt.Mensaje);
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error en guardar el formato");
            }
        });
    }
    else {
        alert("Error: " + msj);
    }
}

function validarReversion() {
    var msj = "";

    if ($("#cboTipoProgramacion").val() == null || $("#cboTipoProgramacion").val() == "0") {
        msj += "Debe seleccionar un tipo de programación." + "\n";
    }
    else {
        //validar dias negativos
        if ($("#DiaPlazo").val() < 0) {
            msj += "La cantidad de días no puede ser negativo." + "\n";
        }
        else {
            // validar fecha mayor a 1 minuto
            var minPlazo = $('#txtMinPlazo').val();
            var minutoPlazo = parseInt(minPlazo.substr(0, 2)) * 60 + parseInt(minPlazo.substr(3, 2));
            if ($("#DiaPlazo").val() == 0 && minutoPlazo < 60) {
                msj += "El plazo debe tener mayor almenos en 1 minuto." + "\n";
            }
        }
    }

    return msj;
}

//Hora de Ejecucion
function GuardarHoraEjecucion() {
    horaejecucion = JSON.stringify({ 'horaejecucion': $('#txtHoraEjecucion').val() });
    console.log(horaejecucion);
    $.ajax({
        url: controladorParametro + "GuardarHoraEjecucion",
        contentType: "application/json; charset=utf-8",
        type: 'POST',
        data: horaejecucion,
        dataType: 'json',
        success: function (model) {
            if (model.Resultado !== '-1') {
                $('#PProceso').removeClass();
                $('#PProceso').addClass('action-exito');
                $('#PProceso').text('Se Guardó Correctamente!');
            } else {
                $('#PProceso').removeClass();
                $('#PProceso').addClass('action-error');
                $('#PProceso').text('Error al Guardar Hora de Ejecución');
            }
        },
        error: function (result) {
            $('#PProceso').removeClass();
            $('#PProceso').addClass('action-alert');
            $('#PProceso').html('no se obtuvo conexión con el servidor');
        }
    });
}

//Validar Hora Ejecucion
function ValidarHoraEjecucion() {

    var horaEjecucion = $('#txtHoraEjecucion').val();
    //var fechaInicios = "";
    var resultado = horaEjecucion.split(":");
    var hora = resultado[0];
    var minuto = resultado[1];

    var HoraEje = '';
    HoraEje = "";
    var flagHoraEje = true;
    if ($('#txtHoraEjecucion').val().replace(/\s/g, '') === "") {
        HoraEje = "Ingrese un Valor";
        $('#txtHoraEjecucion').focus();
        flagHoraEje = false;
    }
    else {
        if (hora > 24) {
            HoraEje = "Ingrese Hora Correcta";
            $('#txtHoraEjecucion').focus();
            flagHoraEje = false;
        }

        if (minuto > 60) {
            HoraEje = "Ingrese Minuto Correcto";
            $('#txtHoraEjecucion').focus();
            flagHoraEje = false;
        }
    }
    if ($('#txtHoraEjecucion').val().length !== 5) {
        HoraEje = "Ingrese Una Hora Hora de Ejecución Válida";
        $('#txtHoraEjecucion').focus();
        flagHoraEje = false;
    }

    if (flagHoraEje) HoraEje = "";

    return HoraEje;
}

//Manejo de archivos
function verArchivo(subcarpeta) {
    $.ajax({
        type: 'POST',
        url: controladorParametro + 'ObtenerFolder',
        data: {
            subcarpeta: subcarpeta
        },
        dataType: 'json',
        success: function (model) {

            if (model.Resultado !== '-1') {
                $('#hfBaseDirectory').val("");
                $('#hfRelativeDirectory').val(model.PathSubcarpeta);

                browser();
                $('.bread-crumb').css("display", 'none');
                $('#resultado').hide();
                $('#folder').show();
            } else {
                alert(model.StrMensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function cargarHtmlSubcarpeta() {
    var listaSubcarpeta = $("#subcarpetasIntervenciones").val().split('|');
    $('#listado').html(dibujarTablaSubcarpeta(listaSubcarpeta));

    $('#tablaListado').dataTable({
        "scrollY": 530,
        "scrollX": true,
        "destroy": "true",
        "sDom": 't',
        "ordering": false,
        "iDisplayLength": -1,
        "stripeClasses": [],
    });
}

function dibujarTablaSubcarpeta(lista) {

    var cadena = '';
    cadena += `
    <div style='clear:both; height:5px'></div>
    <table id='tablaListado' border='1' class='pretty tabla-icono' cellspacing='0' style='width: 100%'>
        <thead>
            <tr>
                <th style='width: 50px'>Ver archivos</th>
                <th style='width: 150px'>Subcarpeta</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];

        cadena += `
            <tr>
                <td style='text-align: center;'>    
                    <a href="JavaScript:verArchivo('${item}');"><img src="${siteRoot}Content/Images/folder_open.png" title="Permite ver los archivos" /></a>
                </td>
                <td style='text-align: left;'>${item}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

//Guardar plaxo reversión
function eliminarProcesoReversion() {
    var tipoProgramacion = parseInt($("#cboTipoProgramacion").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controladorIntervenciones + "EliminarReversion",
        dataType: 'json',
        success: function (evt) {
            if (evt.Resultado != "-1") {
                alert(evt.StrMensaje);
            } else {
                alert("Ocurrió un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ocurrió un error: " + evt.Mensaje);
        }
    });
}

//obtener aprobación automática
function obtenerAprobacionAuto(tipoProgramacion) {
    //cargarHtmlSubcarpeta(); 
    $.ajax({
        type: 'POST',
        url: controladorParametro + "ObtenerAprobacionAuto",
        dataType: 'json',
        data: {
            tipoProgramacion: tipoProgramacion
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                $('#txtHoraEjecucionAprobacion').val(evt.HoraEjecucion)
                $("#cbDia").val(evt.Dia)

                $("#txtUsuarioAprobModif").text(evt.ParametroPlazo.UsuarioModificacion);
                $("#txtFechaAprobModif").text(evt.ParametroPlazo.FechaModificacion);

                if (tipoProgramacion == 3) {
                    $("#filaDia").show();
                }
                else {
                    $("#filaDia").hide();
                }

            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

//Guardar horario de ejecución de la aprobación
function guardarEjecucionAprobacion() {

    var msj = ValidarEjecucionAprobacion();

    if (msj == "") {
        var tipoProgramacion = parseInt($("#cboTipoProgramAprobacion").val()) || 0;
        var dia = parseInt($("#cbDia").val());
        var horaejecucion = $('#txtHoraEjecucionAprobacion').val();

        $.ajax({
            type: 'POST',
            url: controladorParametro + "GuardarProcesoAprobacionAutomatica",
            dataType: 'json',
            data: {
                tipoProgramacion: tipoProgramacion,
                dia: dia,
                horaejecucion: horaejecucion
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    alert("Se actualizó correctamente la configuración");

                    obtenerAprobacionAuto(tipoProgramacion);
                } else {
                    alert("Ocurrió un error: " + evt.Mensaje);
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error en guardar el formato");
            }
        });
    }
    else {
        alert(msj);
    }
}

//Validar Ejecucion Aprobación
function ValidarEjecucionAprobacion() {
    var msj = "";

    var horaEjecucion = $('#txtHoraEjecucionAprobacion').val();
    var resultado = horaEjecucion.split(":");
    var hora = resultado[0];
    var minuto = resultado[1];

    if ($("#cboTipoProgramAprobacion").val() == null || $("#cboTipoProgramAprobacion").val() == "0") {
        msj += "Debe seleccionar un tipo de programación." + "\n";
    }

    // solo para programa semanal validar el día
    if ($("#cboTipoProgramAprobacion").val() == 3) {
        if ($("#cbDia").val() == null) {
            msj += "Debe seleccionar el día de ejecución." + "\n";
        }
    }

    //validar ejecución
    if ($('#txtHoraEjecucionAprobacion').val().replace(/\s/g, '') === "") {
        $('#txtHoraEjecucionAprobacion').focus();
        msj += "Debe ingresar la hora de ejecución" + "\n";
    }
    else {
        if (hora > 24) {
            $('#txtHoraEjecucionAprobacion').focus();
            msj += "Debe ingresar la hora Correcta" + "\n";
        }

        if (minuto > 60) {
            $('#txtHoraEjecucionAprobacion').focus();
            msj += "Debe ingresar minuto Correcto" + "\n";
        }

        if ($('#txtHoraEjecucionAprobacion').val().length !== 5) {
            $('#txtHoraEjecucionAprobacion').focus();
            msj += "Debe ingresar una Hora de Ejecución Válida" + "\n";
        }
    }

    return msj;
}

//porcentaje de similitud

//obtener aprobación automática
function obtenerPorcentajeSimilitud() {
    $.ajax({
        type: 'POST',
        url: controladorParametro + "ObtenerPorcentajeSimilitud",
        dataType: 'json',
        success: function (evt) {
            if (evt.Resultado != "-1") {

                $('#txtValorPorcentaje').val(evt.ParametroPlazo.ValorPorcentaje)
                $("#txtPorcentajeUsuarioModif").text(evt.ParametroPlazo.UsuarioModificacion);
                $("#txtPorcentajeFechaModif").text(evt.ParametroPlazo.FechaModificacion);

            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function guardarPorcentajeSimilitud() {

    var msj = validarPorcentajeSimilitud();

    if (msj == "") {
        var valorPorcentaje = $('#txtValorPorcentaje').val();

        $.ajax({
            type: 'POST',
            url: controladorParametro + "GuardarPorcentajeSimilitud",
            dataType: 'json',
            data: {
                valor: valorPorcentaje
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    alert("Se actualizó correctamente la configuración");

                    obtenerPorcentajeSimilitud();
                } else {
                    alert("Ocurrió un error: " + evt.Mensaje);
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error en guardar la configuración");
            }
        });
    }
    else {
        alert(msj);
    }
}

function validarPorcentajeSimilitud() {
    var msj = "";

    var valorPorcentaje = $('#txtValorPorcentaje').val();

    //validar valor
    if ($('#txtValorPorcentaje').val().replace(/\s/g, '') === "") {
        $('#txtValorPorcentaje').focus();
        msj += "Debe ingresar el valor de porcentaje" + "\n";
    }
    else {
        if (valorPorcentaje > 100) {
            $('#txtValorPorcentaje').focus();
            msj += "Debe ingresar valor menor al 100%" + "\n";
        }

        if (valorPorcentaje < 0) {
            $('#txtValorPorcentaje').focus();
            msj += "Debe ingresar valor mayor a 0%" + "\n";
        }
    }

    return msj;
}