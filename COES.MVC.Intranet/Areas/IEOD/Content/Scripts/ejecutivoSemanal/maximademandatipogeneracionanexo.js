$(function () {
    cargarMaximaDemandaTipoGeneracionAnexo();
});

function cargarMaximaDemandaTipoGeneracionAnexo() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarMaximaDemandaTipoGeneracionAnexo',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (aData) {
            $('#listado').html(aData.Resultado);
            $('#MaximaDemandaTipoGeneracionAnexo').dataTable();
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}