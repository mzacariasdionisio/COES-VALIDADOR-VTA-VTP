var idPosFormato = 0;
var idFuenteDatos = 0;
var tipoFormato = 0; // 0 => Generacion, 1 => Transmision; 2 => Distribucion; 3 => Usuarios Libres
/// Formato
var idFormatoConsumoCombustible = 57;
var idFormatoPresionGasTemp = 58;
var idFormatoDisponibilidadGas = 59;
var idFormatoQuemaGas = 60;
var idFormatoDespacho = 62;
var idFormatoTensGener = 64;
var idFormatoCalorUtil = 65;
var idFFormatoFlujPot = 63;
var idFormatoTensionBarrra = 66;
var idFormatoDemandaDiaria = 85;
var idFormatoAutoProd = 67;
var idFormatoFteEnerPrimaria = 68;

/// Fuente Datos
var idFuenteDatosHorasOperacion = 1;
var idFuenteDatosDesconexionEquipo = 2;
var idFuenteDatosRestriccionesOperativas = 3;

/// Panel Ieod

var panelIeod = [
    {
        id: 0,
        idcc: "IDCC-G",
        item: [
            {
                id: 1,
                formato: idFormatoConsumoCombustible,
                fdatcodi: 0,
                label: "Stock y Consumo de Combustibles",
                url: "StockCombustibles/Envio/consumo/",
                estado: 1,
                horaDeclaracion: "16:05:03",
                cumplimiento: 0,
                horaPlazo: "08:00:00"
            },
            {
                id: 2,
                formato: idFormatoPresionGasTemp,
                fdatcodi: 0,
                label: "Presión de Gas y Temparatura",
                url: "StockCombustibles/Envio/presiongas/",
                estado: 1,
                horaDeclaracion: "16:05:03",
                cumplimiento: 1,
                horaPlazo: "08:00:00"
            },
            {
                id: 3,
                formato: idFormatoDisponibilidadGas,
                fdatcodi: 0,
                label: "Disponibilidad de Gas",
                url: "StockCombustibles/Envio/DisponibilidadGas/",
                estado: 0,
                horaDeclaracion: "16:05:03",
                cumplimiento: 0,
                horaPlazo: "08:00:00"
            },
            {
                id: 4,
                formato: idFormatoQuemaGas,
                fdatcodi: 0,
                label: "Quema y Venteo de Gas",
                url: "StockCombustibles/envio/quemagas/",
                estado: 0,
                horaDeclaracion: "16:05:03",
                cumplimiento: 0,
                horaPlazo: "08:00:00"
            },
            {
                id: 5,
                formato: 0,
                fdatcodi: idFuenteDatosHorasOperacion,
                label: "Horas de Operación",
                url: "ieod/HorasOperacion/index/",
                estado: 1,
                horaDeclaracion: "16:05:03",
                cumplimiento: 0,
                horaPlazo: "08:00:00"
            },
            {
                id: 6,
                formato: idFormatoDespacho,
                fdatcodi: 0,
                label: "Despacho Unidades de Generación",
                url: "ieod/Despacho/index/",
                estado: 1,
                horaDeclaracion: "16:05:03",
                cumplimiento: 0,
                horaPlazo: "08:00:00"
            },
            {
                id: 7,
                formato: idFormatoTensGener,
                fdatcodi: 0,
                label: "Tensión de Generación",
                url: "ieod/TensionGener/index/",
                estado: 1,
                horaDeclaracion: "16:05:03",
                cumplimiento: 0,
                horaPlazo: "08:00:00"
            },
            {
                id: 8,
                formato: idFormatoCalorUtil,
                fdatcodi: 0,
                label: "Calor Útil",
                url: "ieod/CalorUtil/index/",
                estado: 1,
                horaDeclaracion: "16:05:03",
                cumplimiento: 0,
                horaPlazo: "08:00:00"
            },
            {
                id: 9,
                formato: 0,
                fdatcodi: idFuenteDatosRestriccionesOperativas,
                label: "Restricciones Operativas",
                url: "ieod/RestriccionesOperativas/index/",
                estado: 1,
                horaDeclaracion: "16:05:03",
                cumplimiento: 0,
                horaPlazo: "08:00:00"
            },
            {
                id: 10,
                formato: idFormatoFteEnerPrimaria,
                fdatcodi: 0,
                label: "Fuente de Energía Primaria",
                url: "ieod/EnergiaPrimaria/index/",
                estado: 1,
                horaDeclaracion: "16:05:03",
                cumplimiento: 0,
                horaPlazo: "08:00:00"
            }
        ]
    },
        {
            id: 1,
            idcc: "IDCC-T",
            item:
                [
                {
                    id: 1,
                    formato: idFFormatoFlujPot,
                    fdatcodi: 0,
                    label: "Flujo de Potencia Activa y Reactiva",
                    url: "ieod/FlujoTrans/index/",
                    estado: 1,
                    horaDeclaracion: "16:05:03",
                    cumplimiento: 0,
                    horaPlazo: "08:00:00"
                },
                {
                    id: 2,
                    formato: idFormatoTensionBarrra,
                    fdatcodi: 0,
                    label: "Tensión de Barras",
                    url: "ieod/TensionBarra/index/",
                    estado: 1,
                    horaDeclaracion: "16:05:03",
                    cumplimiento: 0,
                    horaPlazo: "08:00:00"
                },
                {
                    id: 3,
                    formato: 0,
                    fdatcodi: idFuenteDatosDesconexionEquipo,
                    label: "Desconexión de Equipos",
                    url: "ieod/DesconexionEquipos/index/",
                    estado: 1,
                    horaDeclaracion: "16:05:03",
                    cumplimiento: 0,
                    horaPlazo: "08:00:00"
                }
                ]
        },
        {
            id: 2,
            idcc: "IDCC-D",
            item:
                [
                    {
                        id: 1,
                        formato: idFormatoDemandaDiaria,
                        fdatcodi: 0,
                        label: "Demanda con Potencia Activa y Reactiva",
                        url: "ieod/DemandaDiaria/index/",
                        estado: 1,
                        horaDeclaracion: "16:05:03",
                        cumplimiento: 0,
                        horaPlazo: "08:00:00"
                    }
                ]
        },
        {
            id: 3,
            idcc: "IDCC-UL",
            item:
                [
                    {
                        id: 1,
                        formato: idFormatoDemandaDiaria,
                        fdatcodi: 0,
                        label: "Demanda con Potencia Activa y Reactiva",
                        url: "ieod/DemandaDiariaUL/index/",
                        estado: 1,
                        horaDeclaracion: "16:05:03",
                        cumplimiento: 0,
                        horaPlazo: "08:00:00"
                    },
                    {
                        id: 2,
                        formato: idFormatoAutoProd,
                        fdatcodi: 0,
                        label: "Potencia de Autoproductores",
                        url: "ieod/PotAutoProd/index/",
                        estado: 1,
                        horaDeclaracion: "16:05:03",
                        cumplimiento: 0,
                        horaPlazo: "08:00:00"
                    }
                ]
        }

];

$(function () {
    updateContainer();
    $(window).resize(function () {
        updateContainer();
    });
});

function updateContainer() {
    var $containerWidth = $(window).width();
    if ($containerWidth <= 1650) {
        ocultarPanel();
    }
    if ($containerWidth > 1650) {
        mostrarPanel();
    }
}

function dibujarPanelIeod(item, pag, itemSelec) {
    $.ajax({
        type: 'POST',
        url: controlador + "GetInformacionPanelIEOD",
        data: {
            idEmpresa: $('#cbEmpresa').val(),
            fecha: $('#txtFecha').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result.Error == -1) {
                alert(result.Descripcion);
            } else {
                callDatosPanelIeod(item, pag, itemSelec, result.ListaPanelIEOD);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error en generar el Panel IEOD");
        }
    });
}

function callDatosPanelIeod(item, pag, itemSelec, listaPanel) {
    var itemFinal = 0;
    var html = "<label>Envío Información IEOD</label>";
    html += "<div id='opcion_panel' style='float: right;'></div>";
    html += "<div class='cuerpo_panel_ieod' style='clear:both; height:10px'></div>"
    html += "<div class='cuerpo_panel_ieod' style= 'display:table'>";
    html += "   <div style=display:table-row'>";
    for (var i = 0; i < panelIeod.length; i++) {
        if (itemSelec > -1) {
            itemFinal = itemSelec;
        }
        else {
            itemFinal = item;
        }
        if (panelIeod[i].id == item) {
            html += "        <div class='celda-enter-div' style='display: table-cell; '>" + panelIeod[i].idcc + "</div>";
        }
        else {
            html += "        <div class='celda-div' style='display: table-cell; '><a onclick='dibujarPanelIeod(" + panelIeod[i].id + ", " + pag + "," + itemFinal + ");'>" + panelIeod[i].idcc + "</a></div>";
        }
    }
    html += "   </div>";
    html += "</div>";
    html += "<div class='cuerpo_panel_ieod' style='clear:both; height:10px'></div>"
    html += "<div class='cuerpo_panel_ieod' style= 'display:table'>";
    for (var i = 0; i < panelIeod[item].item.length; i++) {
        estado = "";
        backcolor = "white";
        idestado = verificarEnvioFortmato(listaPanel, panelIeod[item].item[i]);
        var clase = "celda-estado-div";

        if (idestado == 1) {
            backcolor = "#daf5bc";
            switch (panelIeod[item].item[i].cumplimiento) {
                case 1:
                    estado = "<font style='color:green;'>Enviado en plazo</font>";
                    break;
                case 2:
                    estado = "<font style='color:blue;'>Enviado</font>";
                    break;
                case 3:
                    estado = "<font style='color:blue;'>Incompleto</font>";
                    break;
                default:
                    estado = "<font style='color:#2292e4;'>Enviado en fuera de plazo</font>";
                    break;
            }
        }
        else {
            estado = "<font style='color:red;'>No enviado</font>"
        }
        if ((panelIeod[item].item[i].id == pag) && (itemFinal == item)) {
            clase = "celda-estado-enter-div";
        }

        html += "   <div class='cuerpo_panel_ieod' style=display:table-row'>";
        html += "        <div class='" + clase + "' style='display: table-cell; '>  </div>";
        html += "        <div class='celda-item-div' style='display: table-cell;background:" + backcolor + ";'><a href='" + siteRoot + panelIeod[item].item[i].url + "'>" + panelIeod[item].item[i].label + "</a><br>";
        html += "        Estado: " + estado + "    </div>";
        html += "   </div>";
    }
    html += "</div>";

    //agregar 

    $('.panel-ieod').html(html).fadeIn();
    updateContainer();
}

function verificarEnvioFortmato(listaPanel, obj) {
    var formato = obj.formato;
    var fdatcodi = obj.fdatcodi;

    for (var i = 0 ; i < listaPanel.length; i++) {
        if (listaPanel[i].Formatcodi == formato && listaPanel[i].Fdatcodi == fdatcodi) {
            obj.cumplimiento = listaPanel[i].Cumplimiento;
            return 1;
        }
    }
    return 0;
}

function ocultarPanel() {
    var html = '<a href="JavaScript:mostrarPanel();"><img src="' + initialURL + '/Content/Images/panel_max.png" style="width: 15px;" title="Maximizar Panel IEOD" alt="Max IEOD"></a>';
    $("#opcion_panel").html(html);
    $(".panel-ieod").css('height', '15px');
    $(".panel-ieod").css('width', '202px');
    $(".cuerpo_panel_ieod").hide();
}

function mostrarPanel() {
    var html = '<a href="JavaScript:ocultarPanel();"><img src="' + initialURL + '/Content/Images/panel_min.png" style="width: 15px;" title="Minimizar Panel IEOD" alt="Min IEOD"></a>';
    $("#opcion_panel").html(html);
    $(".panel-ieod").css('height', '600px');
    $(".panel-ieod").css('width', '260px');
    $(".cuerpo_panel_ieod").show();
}