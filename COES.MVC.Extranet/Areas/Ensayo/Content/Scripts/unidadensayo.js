var controlador = siteRoot + 'Ensayo/'

$(function () {
    
    $('#btnAceptar').click(function () {
        agregarUnidad();
    });
    
});

function agregarUnidad() {
    
    var codequipo = $('#cbUnidad').val();
    alert(codequipo);
    $.ajax({
        type: 'POST',
        url: controlador + 'envio/GrabarObservacion',

        data: { txtObserv: observ, icodestado: codestado, icodenvio: codenvio },

        success: function (aData) {

            //$('#centrales').html(aData);

            //alert(aData);
            //$('#cbCentral').get(0).options.length = 0;
            //$('#cbCentral').get(0).options[0] = new Option("--SELECCIONE--", "");
            //$.each(aData, function (i, item) {
            //    $('#cbCentral').get(0).options[$('#cbCentral').get(0).options.length] = new Option(item.Text, item.Value);
            //});
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}