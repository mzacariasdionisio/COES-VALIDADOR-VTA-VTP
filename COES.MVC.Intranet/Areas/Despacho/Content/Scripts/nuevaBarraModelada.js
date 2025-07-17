
var controlador = siteRoot + 'despacho/BarraModelada/';

$(function () {

    $('#txtGrupotension').keypress(function (event) {
        return (event.which >= 48 && event.which <= 57) || event.which == 8 || event.which == 46;
    });

    $('#btnCancelar').on('click', function () {
        $('#popupNuevo').bPopup().close();
    });

    $('#btnGrabar').on('click', function () {
        guardarBarra();
    });
    
});
function isNumber(evt, element) {

    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (
        (charCode != 45 || $(element).val().indexOf('-') != -1) &&      // “-” CHECK MINUS, AND ONLY ONE.
            (charCode != 46 || $(element).val().indexOf('.') != -1) &&      // “.” CHECK DOT, AND ONLY ONE.
            (charCode < 48 || charCode > 57))
        return false;

    return true;
};

function guardarBarra() {

    var idsBarraEquipos = tabla.$('input[type="checkbox"]').serialize();
    idsBarraEquipos = idsBarraEquipos.replace(/id=/g, "");
    //var arrayIds = idsBarraEquipos.split('&');

    var nombreBarra = $("#txtGruponomb").val();
    var abreviaturaBarra = $("#txtGrupoabrev").val();
    var tensionBarra = $("#txtGrupotension").val();

    $.ajax({
        type: 'POST',
        url: controlador + "GrabarBarra",
        dataType: "json",
        data: {
            sNombre: nombreBarra,
            sAbrevitura: abreviaturaBarra,
            sTension: tensionBarra,
            equipos: idsBarraEquipos
        },
        success: function (resultado) {
            
            if (resultado == 1) {
                mostrarMensaje('mensaje', 'exito', 'El proceso se ejecutó correctamente.');
                $('#popupNuevo').bPopup().close();
                consultar();
            } else
                mostrarMensaje('mensaje', 'error', 'Se produjo un error.');
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se produjo un error.');
        }
    });

};