var controlador = siteRoot + "Siosein/RemisionesOsinergmin/";
var controlador2 = siteRoot + "Siosein/TablasPrieDeclaracionMen/"; //SIOSEIN-PRIE-2021
var table;
var radio;

var dvRemision = $("#popupTabla .popupRemision"); //SIOSEIN-PRIE-2021
var dvLeyenda = $("#popupTabla .popupLeyenda"); //SIOSEIN-PRIE-2021
var dvSeleccionarOpcion = $("#popupTabla .popupSeleccionarOpcion"); //SIOSEIN-PRIE-2021

$(function () {
    $('.rbTipo').click(function () {
        mostrarTipo();
    });

    $("#btnCancelar").click(function () {
        window.location.href = siteRoot + "Siosein/RemisionesOsinergmin";
    });

    $('#btnRemitir').click(function () {
        remitirTodo();
    });

    //SIOSEIN-PRIE-2021
    $(".popupSeleccionarOpcion #btnAceptar").click(function () {
        console.log("boton aceptar");
        aceptarVentanaProcesoRemision();
    });
    //

    seteoPeriodo();

    $("#btnLeyenda").click(function () {
        dvRemision.hide(); //SIOSEIN-PRIE-2021
        dvSeleccionarOpcion.hide(); //SIOSEIN-PRIE-2021
        dvLeyenda.show(); //SIOSEIN-PRIE-2021
        $('#popupTabla').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    });
});

function checkAll(ele) {
    var checkboxes = document.getElementsByTagName('input');
    if (ele.checked) {
        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].type == 'checkbox') {
                checkboxes[i].checked = true;
            }
        }
    } else {
        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].type == 'checkbox') {
                checkboxes[i].checked = false;
            }
        }
    }
}

function difusionTabla(tab) {
    var periodo = $("#txtPeriodo").val();
    var contr = siteRoot + 'Siosein/TablasPrieDeclaracionMen/';
    switch (tab) {
        case '1': document.location.href = contr + 'DifusionPotenciaFirme?periodo=' + periodo; break;
        case '2': document.location.href = contr + 'DifusionPotenciaEfectiva?periodo=' + periodo; break;
        case '3': document.location.href = contr + 'DifusionCostosMarginales?periodo=' + periodo; break;
        case '4': document.location.href = contr + 'DifusionCostosVariables?periodo=' + periodo; break;
        case '5': document.location.href = contr + 'DifusionProduccionEnergia?periodo=' + periodo; break;
        case '6': document.location.href = contr + 'DifusionDesviaciones?periodo=' + periodo; break;
        case '7': document.location.href = contr + 'DifusionTransferenciaEnergia?periodo=' + periodo; break;
        case '8': document.location.href = contr + 'DifusionPagoTransferenciaPotencia?periodo=' + periodo; break;
        case '9': document.location.href = contr + 'DifusionBalanceEmpresas?periodo=' + periodo; break;
        case '10': document.location.href = contr + 'DifusionPagoTransferenciaEnergia?periodo=' + periodo; break;
        case '11': document.location.href = contr + 'DifusionCompensacionIngresoTarifario?periodo=' + periodo; break;
        case '12': document.location.href = contr + 'DifusionTransmisionPCSPTyPCSGT?periodo=' + periodo; break;
        case '13': document.location.href = contr + 'DifusionRecaudacionPeajesConexionTransmision?periodo=' + periodo; break;
        case '14': document.location.href = contr + 'DifusionCostosOperacionEjecutados?periodo=' + periodo; break;
        case '15': document.location.href = contr + 'DifusionHorasOperacion?periodo=' + periodo; break;
        case '16': document.location.href = contr + 'DifusionEnergSumiEjecMensual?periodo=' + periodo; break;
        case '17': document.location.href = contr + 'DifusionFlujoInterconexionEjec?periodo=' + periodo; break;
        case '18': document.location.href = contr + 'DifusionCaudalesEjecDia?periodo=' + periodo; break;
        case '19': document.location.href = contr + 'DifusionVolumenReserEjecDia?periodo=' + periodo; break;
        case '20': document.location.href = contr + 'DifusionVolumenLagos?periodo=' + periodo; break;
        case '21': document.location.href = contr + 'DifusionVolumenEmbalses?periodo=' + periodo; break;
        case '22': document.location.href = contr + 'DifusionHidrologiaCuencas?periodo=' + periodo; break;
        case '23': document.location.href = contr + 'DifusionVolumenCombustible?periodo=' + periodo; break;
        case '24': document.location.href = contr + 'DifusionHechosRelevantes?periodo=' + periodo; break;
        case '25': document.location.href = contr + 'DifusionInstallRepotenciaRetiros?periodo=' + periodo; break;
        case '26': document.location.href = contr + 'DifusionProgOperacionMensual?periodo=' + periodo; break;
        case '27': document.location.href = contr + 'DifusionProgOperacionCostosMarginalesMensual?periodo=' + periodo; break;
        case '28': document.location.href = contr + 'DifusionCostosOperacionProgMensual?periodo=' + periodo; break;
        case '29': document.location.href = contr + 'DifusionEmbalsesEstacionalesProgMensual?periodo=' + periodo; break;
        case '30': document.location.href = contr + 'DifusionProgOperacionSemanal?periodo=' + periodo; break;
        case '31': document.location.href = contr + 'DifusionCostosOperacionSemanal?periodo=' + periodo; break;
        case '32': document.location.href = contr + 'DifusionCostosMarginalesSemanal?periodo=' + periodo; break;
        case '33': document.location.href = contr + 'DifusionProgOperacionDiario?periodo=' + periodo; break;
        case '34': document.location.href = contr + 'DifusionCostosOperacionDiario?periodo=' + periodo; break;
        case '35': document.location.href = contr + 'DifusionCostosMarginalesDiario?periodo=' + periodo; break;
    }
}

function verificarTabla(tab) {
    var contr = siteRoot + 'Siosein/TablasPrieDeclaracionMen/';
    var periodo = $("#txtPeriodo").val();
    switch (tab) {
        case '1': document.location.href = contr + 'PotenciaFirme?periodo=' + periodo; break;
        case '2': document.location.href = contr + 'PotenciaEfectiva?periodo=' + periodo; break;
        case '3': document.location.href = contr + 'VerificarCostosMarginales?periodo=' + periodo; break;
        case '4': document.location.href = contr + 'CostosVariables?periodo=' + periodo; break;
        case '5': document.location.href = contr + 'ProduccionEnergia?periodo=' + periodo; break;
        case '6': document.location.href = contr + 'Desviaciones?periodo=' + periodo; break;
        case '7': document.location.href = contr + 'TransferenciaEnergia?periodo=' + periodo; break;
        case '8': document.location.href = contr + 'PagoTransferenciaPotencia?periodo=' + periodo; break;
        case '9': document.location.href = contr + 'BalanceEmpresas?periodo=' + periodo; break;
        case '10': document.location.href = contr + 'PagoTransferenciaEnergia?periodo=' + periodo; break;
        case '11': document.location.href = contr + 'CompensacionIngresoTarifario?periodo=' + periodo; break;
        case '12': document.location.href = contr + 'TransmisionPCSPTyPCSGT?periodo=' + periodo; break;
        case '13': document.location.href = contr + 'RecaudacionPeajesConexionTransmision?periodo=' + periodo; break;
        case '14': document.location.href = contr + 'CostosOperacionEjecutados?periodo=' + periodo; break;
        case '15': document.location.href = contr + 'HorasOperacion?periodo=' + periodo; break;
        case '16': document.location.href = contr + 'EnergSumiEjecMensual?periodo=' + periodo; break;
        case '17': document.location.href = contr + 'FlujoInterconexionEjec?periodo=' + periodo; break;
        case '18': document.location.href = contr + 'CaudalesEjecDia?periodo=' + periodo; break;
        case '19': document.location.href = contr + 'VolumenReserEjecDia?periodo=' + periodo; break;
        case '20': document.location.href = contr + 'VolumenLagos?periodo=' + periodo; break;
        case '21': document.location.href = contr + 'VolumenEmbalses?periodo=' + periodo; break;
        case '22': document.location.href = contr + 'HidrologiaCuencas?periodo=' + periodo; break;
        case '23': document.location.href = contr + 'VolumenCombustible?periodo=' + periodo; break;
        case '24': document.location.href = contr + 'HechosRelevantes?periodo=' + periodo; break;
        case '25': document.location.href = contr + 'InstallRepotenciaRetiros?periodo=' + periodo; break;
        case '26': document.location.href = contr + 'VerificarProgOperacionMensual?periodo=' + periodo; break;
        case '27': document.location.href = contr + 'VerificarProgOperacionCostosMarginalesMensual?periodo=' + periodo; break;
        case '28': document.location.href = contr + 'VerificarCostosOperacionProgMensual?periodo=' + periodo; break;
        case '29': document.location.href = contr + 'VerificarEmbalsesEstacionalesProgMensual?periodo=' + periodo; break;
        case '30': document.location.href = contr + 'ProgOperacionSemanal?periodo=' + periodo; break;
        case '31': document.location.href = contr + 'CostosOperacionSemanal?periodo=' + periodo; break;
        case '32': document.location.href = contr + 'CostosMarginalesSemanal?periodo=' + periodo; break;
        case '33': document.location.href = contr + 'ProgOperacionDiario?periodo=' + periodo; break;
        case '34': document.location.href = contr + 'CostosOperacionDiario?periodo=' + periodo; break;
        case '35': document.location.href = contr + 'CostosMarginalesDiario?periodo=' + periodo; break;
    }
}

function cargarTabla(tab) {
    var contr = siteRoot + 'Siosein/TablasPrieDeclaracionMen/';
    var periodo = $("#txtPeriodo").val();
    switch (tab) {
        case '9': document.location.href = contr + 'IndexExcelWebCOMP?periodo=' + periodo; break;
    }
}

function seteoPeriodo(Cabpriperiodo) {
    var periodo = $("#txtPeriodo").val();
    var contr = siteRoot + 'Siosein/TablasPrieDeclaracionMen/';
    $.ajax({
        type: "POST",
        url: contr + "LoadPeriodo",
        data: {
            Cabpriperiodo: periodo
        },
        success: function (data) {
            //if (data == 1) { window.location.href = controlador + "Edit"; }
        },
        error: function () {
            alert("Error");
        }
    });
}

mostrarTipo = function () {

    var tipo = $('input[name=rbTipo]:checked').val();

    if (tipo == "X") {
        radio = "x";
    }

    //if (tipo == "C") {
    //    radio = "c";
    //}
    //if (tipo == "Z") {
    //    radio = "z";
    //}

}

function remitirRegistro(id) {
    var periodo = $("#txtPeriodo").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'remitirRegistroIndividual',
        data: {
            periodo: periodo,
            tabla: id
        },
        dataType: 'json',
        success: function (result) {
            alert(result.Mensaje);
            location.reload();
        },
        error: function () {
            //mostrarError('Ha ocurrido un error: verificar el query');
            alert('Ha ocurrido un error: verificar el query');
        }
    });
}

function remitirTodo() {
    var cadena = "";
    var checkboxes = document.getElementById('tbSeleccionados').getElementsByTagName('input');
    for (var i = 0; i < checkboxes.length; i++) {
        if (checkboxes[i].type == 'checkbox' && checkboxes[i].checked == true) {
            var valor = checkboxes[i].id;
            if (cadena == "") {
                cadena = valor;
            }
            else {
                cadena = cadena + "," + valor;
            }
        }
    }

    if (cadena != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'remitirTodo',
            data: {
                periodo: $("#txtPeriodo").val(),
                cadena: cadena
            },
            dataType: 'json',
            success: function (result) {
                alert(result.Mensaje);
                location.reload();
            },
            error: function () {
                alert('Ha ocurrido un error: verificar el query');
            }
        });
    } else {
        alert('Por favor seleccione al menos un registro');
    }
}

// #region SIOSEIN-PRIE-2021
function abrirVentanaProcesoRemision(tprieabrev, tpriecodi, tpriedescription) {
    dvLeyenda.hide();
    dvRemision.hide();
    $("#tprieabrev").val(tprieabrev);
    $("#tpriecodi").val(tpriecodi);
    $("#resultadoValidacion").html("");
    $("#resultadoValidacion").attr('class', '');

    var nombreTabla = "TABLA ";
    if (tpriecodi.length < 2) {
        nombreTabla += "0";
    }
    nombreTabla += tpriecodi + ": " + tpriedescription + " (" + tprieabrev + ")";
    $("#nombreTabla").html(nombreTabla);

    dvSeleccionarOpcion.show();
    $("#popupTabla").bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown',
        modalClose: false
    });
}

function aceptarVentanaProcesoRemision() {
    var opcion = document.getElementsByName('opcionEjecutar');
    for (i = 0; i < opcion.length; i++) {
        if (opcion[i].checked) {
            var proceso = opcion[i].value;
            break;
        }
    }
    console.log('proceso: ', proceso);
    if (proceso == "rbValidacion") {
        var tprieabrev = $("#tprieabrev").val();
        var tpriecodi = $("#tpriecodi").val();
        validarFormatoOsinergmin(tpriecodi, tprieabrev)
    }
    else if (proceso == "rbRemision") {
        var tprieabrev = $("#tprieabrev").val();
        remitirRegistro(tprieabrev)
    }
    else {
        alert("Se seleccionó una opción inválida");
    }
}

function validarFormatoOsinergmin(tpriecodi, tprieabrev) {
    $("#resultadoValidacion").html("");
    $("#resultadoValidacion").attr('class', '');
    var periodo = $("#txtPeriodo").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'validarFormatoOsinergmin',
        data: {
            tpriecodi: tpriecodi,
            tprieabrev: tprieabrev,
            periodo: periodo
        },
        dataType: 'json',
        success: function (model) {
            if (model.Resultado == "0") {
                $("#resultadoValidacion").attr('class', model.Resultado2);
                $("#resultadoValidacion").html(model.Mensaje);
                $("#resultadoValidacion").append('<a href=' + controlador2 + 'abrirarchivo?tipo=' + 1 + '&nombreArchivo=' + model.Mensaje2 + '>Log de Errores</a>');
            } else if (model.Resultado == "1") {
                $("#resultadoValidacion").attr('class', model.Resultado2);
                $("#resultadoValidacion").html(model.Mensaje);
            } else {
                $("#resultadoValidacion").html('Ha ocurrido un error');
            }
        },
        error: function () {
            alert('Ha ocurrido un error: verificar');
        }
    });
}

function cancelar() {
    $('#popupTabla').bPopup().close();
}

function remisionExitosa(usuario, fecha) {
    dvRemision.show();
    dvLeyenda.hide();
    dvSeleccionarOpcion.hide();

    dvRemision.find("#RemisionUsuario").html(usuario);
    dvRemision.find("#RemisionFecha").html(fecha);

    $('#popupTabla').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown',
        modalClose: false
    });
}

function remisionFallida(tpriecodi, rccaCodi) {

    ExportarLogErrores(tpriecodi, rccaCodi);
}

function ExportarLogErrores(tpriecodi, rccaCodi) {

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarLogErrores',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            periodo: $("#txtPeriodo").val(),
            tpriecodi: tpriecodi,
            rccaCodi: rccaCodi
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result == "-1") {
                alert("Ha ocurrido un error inesperado.");
            }
            else if (result == "-2") {
                alert("No se encontró el periodo");
            }
            else {
                console.log('result:', result)
                window.location = controlador2 + 'abrirarchivo?tipo=' + 1 + '&nombreArchivo=' + result;
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

//#endregion

// #region Historial
function abrirVentanaHistorialVerificacion(titulo, tpriecodi) {

    $("#popupHistorial .popup-title span").html(titulo + " - Historial de Verificación");
    $('#div_historial').html('');

    $.ajax({
        type: 'POST',
        url: controlador + "ListarHistorialVerificacion",
        data: {
            periodo: $("#txtPeriodo").val(),
            tpriecodi: tpriecodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                var html = dibujarTablaHistorialVerificacion(evt.ListaHistorialVerificacion);
                $('#div_historial').html(html);

                abrirPopup("popupHistorial");
            } else {

                alert('Ha ocurrido un error al listar las versiones :' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function dibujarTablaHistorialVerificacion(lista) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" width="100%" id="tabla_historial">
        <thead>
            <tr>
                <th>Versión</th>
                <th>Estado</th>
                <th>Fecha</th>
                <th>Usuario</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];

        cadena += `
            <tr class='id_${item.Cabpricodi}'>
                <td style="height: 24px">${item.Cabpriversion}</td>
                <td style="height: 24px">${item.EstadoDesc}</td>
                <td style="height: 24px">${item.CabprifeccreacionDesc}</td>
                <td style="height: 24px">${item.Cabpriusucreacion}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function abrirVentanaHistorialRemision(titulo, pseinCodi, rtabcodi, tpriecodi) {

    $("#popupHistorial .popup-title span").html(titulo + " - Historial de Remisión");
    $('#div_historial').html('');

    $.ajax({
        type: 'POST',
        url: controlador + "ListarHistorialRemision",
        data: {
            pseinCodi: pseinCodi,
            rtabcodi: rtabcodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                var html = dibujarTablaHistorialRemision(evt.ListaHistorialRemision, tpriecodi);
                $('#div_historial').html(html);

                abrirPopup("popupHistorial");
            } else {

                alert('Ha ocurrido un error al listar las versiones :' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function dibujarTablaHistorialRemision(lista, tpriecodi) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" width="100%" id="tabla_historial">
        <thead>
            <tr>
                <th># Registros <br/> remitidos</th>
                <th>Fecha inicio</th>
                <th>Estado</th>
                <th>Fecha fin de remisión</th>
                <th>Usuario</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];

        var sOnclick = "";
        if (item.RccaEstadoEnvio == "0") sOnclick = ` onclick="remisionFallida(${tpriecodi},${item.RccaCodi})" style='cursor:pointer' `;

        cadena += `
            <tr class='id_${item.RccaCodi}' ${sOnclick}>
                <td style="height: 24px">${item.RccaNroRegistros}</td>
                <td style="height: 24px">${item.RccaFecCreacionDesc}</td>
                <td>${item.RccaEstadoEnvioDesc}</td>
                <td style="height: 24px">${item.RccaFecModificacionDesc}</td>
                <td style="height: 24px">${item.RccaUsuModificacion}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function abrirPopup(contentPopup) {
    setTimeout(function () {
        $("#" + contentPopup).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}

//#endregion
