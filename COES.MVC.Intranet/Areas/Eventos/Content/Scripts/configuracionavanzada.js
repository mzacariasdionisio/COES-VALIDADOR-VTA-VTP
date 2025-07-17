var controlador = siteRoot + 'eventos/rsf/';
var URSUrsnomb = null;
var URSEmprnomb = null;
var URSGruponomb = null;
var URSGrupotipo = null;
var URSEquicodi = null;

$(function () {

    $('#btnGrabar').on('click', function () {
        grabar();
    });

    
});


grabar = function () {
    
    var validacion = "";
    URSUrsnomb = document.getElementByName("Ursnomb")[0].value;
    URSEmprnomb = document.getElementByName("URSEmprnomb")[0].value;
    URSGruponomb = document.getElementByName("URSGruponomb")[0].value;
    URSGrupotipo = document.getElementByName("URSGrupotipo")[0].value;
    URSEquicodi = document.getElementByName("URSEquicodi")[0].value;

    if (validacion == "") {
        $.ajax({
            type: "POST",
            url: controlador + 'ConfAvanzada',
            dataType: "json",
            contentType: 'application/json',
            traditional: true,
            data: JSON.stringify({
                datos: datos
            }),
            success: function (result) {
                URSUrsnomb,
               
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', validacion);
    }
};
