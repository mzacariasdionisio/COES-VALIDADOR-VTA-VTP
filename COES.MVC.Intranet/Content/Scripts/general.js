var controler = siteRoot + "general/";

$(document).ready(function () {
    buscar();
    $('#btnBuscar').click(function () {
        buscar();
    });
});

function buscar() {
  
    $.ajax({
        type: 'POST',
        url: controler + "grilla",
        data: { nombre: $('#txtNombre').val() },
        success: function (evt) {
            $('#listado').html(evt);
            oTable = $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true"                
            });
        },
        error: function () {
            mostrarError();
        }
    });
}



$(document).ready(function () {
    var name = 'ctl00$ctl32$g_221130f0_ece5_4c19_8cbc_eae787ff1412$ctl02';
    var d = new Date();
    var n = d.getMonth();
    $('select[name=' + name + ']').attr('selectedIndex', n);
});
