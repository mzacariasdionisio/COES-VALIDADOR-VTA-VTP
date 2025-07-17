var controler = siteRoot + "transfpotencia/recalculopotencia/";

//Funciones de busqueda
$(document).ready(function () {


    $("#Entidad_Pericodi").change(function () {
        var val = $("#Entidad_Pericodi option:selected").val();
        //alert(val);
        
        $.ajax({
            type: 'POST',
            async: true,
            url: controler + "ObtenerMaximaDemanda",
            data: { periodo: val },
            success: function (evt) {
                console.log(evt);
                
                $("#Recpotinterpuntames").val(evt);
            },
            error: function () {
                mostrarError("Lo sentimos, se ha producido un error");
            }
        });

    });

});