var controlador = siteRoot + 'PrimasRER/ReportesBI/';
var listReportPowerBi;

$(document).ready(function () {
    powerBI();
    //openBI(event, 'Demanda');
});

obtenerReportes = function (id, name) {
    $.ajax({
        url: controlador + 'ObtenerReportes',
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            id: id,
            name: name
        }),
        async: false,
        contentType: "application/json;charset=utf8",
        dataType: 'json',
        success: function (response) {
            //listReportPowerBi = response;
            if (response.iResultado == 1) {
                listReportPowerBi = response.list;
            } else {
                alert(response.sMensaje)
            }
        },
        error: function (response) {
            alert("Error");
        }
    });
}

renderTabs = function () {
}

renderPowerBI = function () {
    // Get models. models contains enums that can be used.
    let models = window['powerbi-client'].models;

    // Embed configuration used to describe the what and how to embed.
    let config = {
        type: 'report',
        tokeyType: models.TokenType.Embed,
        accessToken: "",
        embedUrl: "",
        id: "",
        newRender: true,
        pageView: 'fitToWidth',
        settings: {
            filterPaneEnabled: false,
            navContentPaneEnabled: true,
            layoutType: models.LayoutType.Custom,
            customLayout: {
                displayOption: models.DisplayOption.FitToWidth //.FitToPage
            }
        }
    };

    let embedContainer;
    let reportUnique = [];

    //for (let i = 0; i < listReportPowerBi.length; i++) {
    //    if ($('.' + listReportPowerBi[i].Id)[0]) {
    //        // Read embed application token from textbox
    //        config.accessToken = listReportPowerBi[i].TokenAccess;

    //        // Read embed URL from textbox
    //        config.embedUrl = listReportPowerBi[i].EmbedUrl;

    //        // Read report Id from textbox
    //        config.id = listReportPowerBi[i].Id;

    //        $('.' + listReportPowerBi[i].Id).css("height", 700);

    //        // Get a reference to the embedded report HTML element
    //        embedContainer = $('.' + listReportPowerBi[i].Id)[0];

    //        // Embed the report and display it within the div container.
    //        reportUnique.push(powerbi.embed(embedContainer, config));
    //    }
    //}

    for (const element of listReportPowerBi) {
        if ($('.' + element.Id)[0]) {
            // Read embed application token from textbox
            config.accessToken = element.TokenAccess;

            // Read embed URL from textbox
            config.embedUrl = element.EmbedUrl;

            // Read report Id from textbox
            config.id = element.Id;

            $('.' + element.Id).css("height", 700);

            // Get a reference to the embedded report HTML element
            embedContainer = $('.' + element.Id)[0];

            // Embed the report and display it within the div container.
            reportUnique.push(powerbi.embed(embedContainer, config));
        }
    }

    reportUnique.forEach(function (item) {
        // Report.off removes a given event handler if it exists.
        item.off("loaded");

        // Report.on will add an event handler which prints to Log window.
        item.on("loaded", function () {
            item.getPages().then((reportPages) => {
                reportPages[0].setActive();
            });
        });

        // Report.off removes a given event handler if it exists.
        //item.off("rendered");

        // Report.on will add an event handler which prints to Log window.
        item.on("rendered", function () { });

        item.on("error", function (event) {
            reportUnique.off("error");
        });

        item.off("saved");
        item.on("saved", function (event) {
            Log.log(event.detail);
            if (event.detail.saveAs) {
                Log.logText('In order to interact with the new report, create a new token and load the new report');
            }
        });
    });
}

powerBI = function () {
    let id = $(".tabcontent").prop("id");
    let name = $("#sName").val();
    obtenerReportes(id, name);
    renderPowerBI();
}

