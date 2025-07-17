const controlador = siteRoot + 'portalinformacion/';
const renderedReports = {}; // cache local por tabId

function renderPowerBIReportById(report) {
    const models = window['powerbi-client'].models;

    const embedConfiguration = {
        type: 'report',
        id: report.Id,
        embedUrl: report.EmbedUrl,
        accessToken: report.TokenAccess,
        tokenType: models.TokenType.Embed,
        settings: {
            panes: {
                filters: { visible: false },
                pageNavigation: { visible: true }
            },
            navContentPaneEnabled: true,
            layoutType: models.LayoutType.Custom,
            customLayout: {
                displayOption: models.DisplayOption.FitToWidth
            }
        }
    };

    const reportContainer = document.getElementById(report.TabId);
    if (!reportContainer) {
        console.warn('Contenedor no encontrado para:', report.TabId);
        return;
    }

    if (
        report.Id === "2766d7c8-a6fc-4759-8574-4c7fe90ce821" || // Generación
        report.Id === "93bacba8-215b-456d-af98-20f45049e8cc"    // Energía
    ) {
        $(reportContainer).css({
            height: "100vh",
            width: "85%",
            marginLeft: "7.5%"
        });
    } else {
        $(reportContainer).css("height", "700px");
    }

    powerbi.reset(reportContainer);
    powerbi.embed(reportContainer, embedConfiguration);
}

function renderSingleReport(tabId) {
    if (renderedReports[tabId]) {
        return; // ya renderizado previamente
    }

    $.ajax({
        url: controlador + 'ObtenerReportePorTabId',
        type: "POST",
        async: true,
        data: JSON.stringify(tabId),
        contentType: "application/json;charset=utf-8",
        dataType: 'json',
        success: function (report) {
            if (!report || !report.Id) {
                console.warn('No se encontró reporte para:', tabId);
                return;
            }

            renderPowerBIReportById(report);
            renderedReports[tabId] = true; // marcar como ya cargado
        },
        error: function () {
            alert("Error al obtener el reporte Power BI.");
        }
    });
}

function refrescarMonitoreoSEIN() {
    const now = new Date();
    const minutos = now.getUTCMinutes().toString().padStart(2, '0');
    const segundos = now.getUTCSeconds().toString().padStart(2, '0');

    if ((minutos === '05' || minutos === '35') && segundos === '00') {
        const visibleTab = $('.tabcontent:visible > div').attr('id');
        if (visibleTab) {
            renderedReports[visibleTab] = false; // forzar recarga
            renderSingleReport(visibleTab);
        }
    }
}

function switchTab(tabId, contentId, buttonId) {
    $('.tabcontent').hide();
    $('.coes-button').removeClass('powerbi-focus');

    $(`#${contentId}`).show();
    $(`#${tabId}`).css({
        visibility: "visible",
        height: "100vh",
        width: "85%",
        marginLeft: "7.5%"
    });
    $(`#${buttonId}`).addClass('powerbi-focus');

    renderSingleReport(tabId); // ahora usa el nuevo endpoint y cache
}
