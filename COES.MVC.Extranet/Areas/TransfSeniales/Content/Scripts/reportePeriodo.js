var controlador = siteRoot + 'TransfSeniales/ReporteMensual/';
$(function () {


    $('#txtFechaIn').Zebra_DatePicker({
        format: 'M Y',
        onSelect: function (fechaFormat, fecha) {
            $('#txtfecha').val(fecha.substring(5, 7) + " " + fecha.substring(0, 4));
        }
    });



    $('#btnBuscar').click(function () {
        cargarCMRPeriodo();
    });

   
    cargarCMRPeriodo();
});


cargarCMRPeriodo = function () {
    $.ajax({
        type: "POST",
        url: controlador + "ListaMensual",
        data: {
            emprcodi: $('#cbEmpresa').val(),
            fechaPeriodo: $('#txtfecha').val()
        },


        success: function (evt) {
            $('#vistaCMRPeriodo').html(evt);

        },
        error: function () {
            mostrarError();
        }
    });
}


function mostrarError() {
    alert("Error");
}

