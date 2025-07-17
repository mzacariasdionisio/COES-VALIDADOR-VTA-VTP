var controlador = siteRoot + 'Migraciones/AnexoA/';
var ancho = 1200;

$(function () {
    cargarLista();
});

function cargarLista() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaRequerimientosPropios',
        data: {
            fechaInicio: $("#txtFechaInicio").val(),
            fechaFin: $("#txtFechaFin").val()
        },
        success: function (aData) {
            for (var i = 1; i <= aData.length; i++) {
                $('#listado' + i).html(aData[i - 1].Resultado);
            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function viewResumen(eq) {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargaResumenPruebasAleatorias',
        data: {
            equicodi: eq,
            fechaInicio: $("#txtFechaInicio").val(),
            fechaFin: $("#txtFechaFin").val()
        },
        success: function (aData) {
            $('#listado').html(aData.Resultado);
            setviewpopup();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function setviewpopup() {
    setTimeout(function () {
        $('#winAnexoA').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
};
