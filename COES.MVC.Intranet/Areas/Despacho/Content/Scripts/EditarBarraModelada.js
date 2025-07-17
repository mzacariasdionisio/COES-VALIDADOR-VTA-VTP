var controlador = siteRoot + 'despacho/BarraModelada/';

$(function () {

    $('#txtGrupotension').keypress(function (event) {
        return (event.which >= 48 && event.which <= 57) || event.which == 8 || event.which == 46;
    });

    $('#btnCancelarEdit').on('click', function () {
        $('#popupEditar').bPopup().close();
    });

    $('#btnGrabarEdit').on('click', function () {
        editarBarra();
    });

});
function editarBarra() {

    var idsBarraEquipos = tablaedit.$('input[type="checkbox"]').serialize();
    idsBarraEquipos = idsBarraEquipos.replace(/id=/g, "");
    //var arrayIds = idsBarraEquipos.split('&');
    var grupocodi = $("#hdnGrupoCodi").val();
    var nombreBarra = $("#txtGruponomb").val();
    var abreviaturaBarra = $("#txtGrupoabrev").val();
    var tensionBarra = $("#txtGrupotension").val();

    $.ajax({
        type: 'POST',
        url: controlador + "ActualizarBarra",
        dataType: "json",
        data: {
            iGrupocodi:grupocodi,
            sNombre: nombreBarra,
            sAbrevitura: abreviaturaBarra,
            sTension: tensionBarra,
            equipos: idsBarraEquipos
        },
        success: function (resultado) {
            
            if (resultado == 1) {
                mostrarMensaje('mensaje', 'exito', 'El proceso se ejecutó correctamente.');
                $('#popupEditar').bPopup().close();
                consultar();
            } else
                mostrarMensaje('mensaje', 'error', 'Se produjo un error.');
            
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se produjo un error.');
        }
    });

};