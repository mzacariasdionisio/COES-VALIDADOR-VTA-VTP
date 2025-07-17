$(function () {
    cargarEventoFallaSuministroEnergAnexo();
});

function cargarEventoFallaSuministroEnergAnexo() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarEventoFallaSuministroEnergAnexo',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (aData) {
            $('#listado').html(aData.Resultado);
            $('#EventoFallaSuministroEnergAnexo').dataTable();
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}