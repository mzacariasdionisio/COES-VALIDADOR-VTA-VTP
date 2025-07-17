var controlador = siteRoot + 'TransfSeniales/TransfSeniales/';
$(function () {

    $('#txtFecha').Zebra_DatePicker({        
       
    });

    $('#btnBuscar').click(function () {
        cargarCMRReporte();
    });

   
    cargarCMRReporte();

    //cargarVersion();
});


cargarCMRReporte = function () {
    
    $.ajax({
        type: "POST",
        url: controlador + "Lista",
        data: {
            fecha: $('#txtFecha').val(),            
            empresa: $('#cbEmpresa').val()           
        },
        success: function (evt) {
            $('#vistaCMRReporte').html(evt);
            $('#spanNS').html($('#hfContador').val());
            $('#spanVersion').html($('#hfVersion').val());

        },
        error: function () {
            mostrarError();
        }
    });
}

cargarVersion = function () {

    $.ajax({
        type: "POST",
        url: controlador + "Version",
        data: {
            fecha: $('#txtFecha').val()
        },
        success: function (evt) {            
            

        },
        error: function () {
            mostrarError();
        }
    });
}

function mostrarError() {
    alert("Error");
}