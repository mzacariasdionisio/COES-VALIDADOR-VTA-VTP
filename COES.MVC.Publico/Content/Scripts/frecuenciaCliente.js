$(function () {

    var ticker = $.connection.frecuenciaHub; // the generated client-side hub proxy     


    function init() {
    //    ticker.server.getAllStocks().done(function (stocks) {
    //        $stockTableBody.empty();
    //        $.each(stocks, function () {
    //            var stock = formatStock(this);
    //            $stockTableBody.append(rowTemplate.supplant(stock));
    //        });
    //    });
    }

    // Add a client-side hub method that the server will call
    ticker.client.updateFrecuencia = function (result) {
        actualizarGraficoFrecuencia(result);       
    }

    // Start the connection
    $.connection.hub.start().done(init);

});

