var controlador = siteRoot + 'PMPO/ReporteCumplimiento/';

var ANCHO_REPORTE = 1000;

$(function () {
    //$('#cntMenu').css("display", "none");

    $('#txtMesElaboracion').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
        }
    });

    $('#btn-accept-report').click(function () {
        consultarReporteCumplimiento();
    });
    $('#guardarReporte').click(function () {
        guardarReporte();
    });
    $('#verHistorico').click(function () {
        popupHistorico();
    });

    consultarReporteCumplimiento();
});

function consultarReporteCumplimiento() {
    var mesElaboracion = $("#txtMesElaboracion").val();
    var estadoCumplimiento = $("#ddl-EstaCumpl").val();

    ANCHO_REPORTE = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 1100;

    $.ajax({
        type: 'POST',
        url: controlador + "ListaReporteCumplimiento",
        dataType: 'json',
        data: {
            mesElaboracion: mesElaboracion,
            estadoCumplimiento: estadoCumplimiento,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                $("#reporte").html(evt.HtmlReporte);

                $('#listado').css("width", ANCHO_REPORTE + "px");
                $('#table-resumen').dataTable({
                    "scrollY": 430,
                    "scrollX": true,
                    "sDom": 'ft',
                    "ordering": false,
                    "iDisplayLength": -1
                });
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function guardarReporte() {
    var mesElaboracion = $("#txtMesElaboracion").val();
    var estadoCumplimiento = $("#ddl-EstaCumpl").val();

    $.ajax({
        type: 'POST',
        url: controlador + "GuardarReporte",
        dataType: 'json',
        data: {
            mesElaboracion: mesElaboracion,
            estadoCumplimiento: estadoCumplimiento,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                descargarPdf(evt.Resultado);
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function descargarPdf(id) {
    window.location.href = controlador + 'DowloadFilePdf?id=' + id;
}

//Historial
function popupHistorico() {
    var mesElaboracion = $("#txtMesElaboracion").val();
    $("#idHistorial").html('');

    $.ajax({
        type: 'POST',
        url: controlador + "ListaHistorialCumplimiento",
        dataType: 'json',
        data: {
            mesElaboracion: mesElaboracion,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                $('#idHistorial').html(dibujarTablaHistorial(evt.ListaReporteOsinerg));

                setTimeout(function () {
                    $('#popupHistorial').bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown',
                        modalClose: false
                    });
                    $('#tablalHistorial').dataTable({
                        "scrollY": 330,
                        "scrollX": true,
                        "sDom": 'ft',
                        "ordering": false,
                        "bPaginate": false,
                        "stripeClasses": [],
                        "iDisplayLength": -1
                    });
                }, 50);

            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function dibujarTablaHistorial(lista) {

    var cadena = '';
    cadena += `
    <div style='clear:both; height:5px'></div>
    <table id='tablalHistorial' border='1' class='pretty tabla-adicional' cellspacing='0'>
        <thead>
            <tr>
                <th>Reporte</th>
                <th>Mes Elaboración</th>
                <th>Fecha creación</th>
                <th>Usuario de creación</th>
                <th>Descargar</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        cadena += `
            <tr>
                <td style='text-align: center;height: 26px;'>${lista[key].Repdescripcion}</td>
                <td style='text-align: center;'>${lista[key].RepfechaDesc}</td>
                <td style='text-align: center;'>${lista[key].RepfeccreacionDesc}</td>
                <td style='text-align: center;'>${lista[key].Repusucreacion}</td>
                <td style='text-align: center;' onclick='descargarPdf(${ lista[key].Repcodi});' style='cursor:pointer'>
                    <a>
                        <img src="${siteRoot}Content/Images/Download.png" />
                    </a>
                </td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}