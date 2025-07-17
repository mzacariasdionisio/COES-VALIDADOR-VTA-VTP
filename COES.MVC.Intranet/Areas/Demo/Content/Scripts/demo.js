var controler = siteRoot + 'demo/demo/';

$(function () {
    
    $('#btnBuscar').click(function () {
        buscar();
    });

    $('#btnNuevo').click(function () {

        var input = { frecuencias: [{ Fecha: '07/12/2017', Valor:10}, { Fecha: '07/12/2017', Valor:20} ] };
        $.ajax({
            type: "POST",
            url: "http://www.coes.org.pe/servicioscoes/movilservicio.svc/guardarfrecuencia",
            contentType: "application/json",
            headers: {
                // Este header no es necesario si ya lo definiste usando headers.commons
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf8'
            },
            data: JSON.stringify(input),
            success: function (result) {
                alert("POST result: " + JSON.stringify(result));
            },
            error: function(){
                alert("Error");
            }

        });
    });


    buscar();
});

buscar = function () {
   
   
}