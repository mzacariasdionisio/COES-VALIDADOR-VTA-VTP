var controlador = siteRoot + 'Monitoreo/CmgProgramados/';
var ancho = 900;
$(function () {
    ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;

    $('#txtMes').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            listaMes();
        }
    });

    $('#btnCargar').click(function () {
        cargarCmgProg();
    }
    );

  });

function cargarCmgProg() {
    if (confirm("¿Va a cargar Costos Marginales Programados de Yupana?") == true) {

    $.ajax({
        type: 'POST',
        url: controlador + "CargarCostosMarginalesProg",
        data: {
            fechaMes: $("#txtMes").val()
        },
        success: function (evt) {
            alert('Cargo Correctamente');
            listaMes();
        },
        error: function (err) {
            alert('Ha ocurrido un error');
        }
    });
    }
}

function listaMes() {

    $.ajax({
        type: 'POST',
        url: controlador + "ListaMensual",
        data: {
            fechaMes: $("#txtMes").val()
        },
        success: function (evt) {
            $('#listado').css("width", ancho + "px");
            $('#listado').html(evt);
        },
        error: function (err) {
            alert('Ha ocurrido un error');
        }
    });

}

   

