var controler = siteRoot + "Intervenciones/Parametro/";

$(function () {   

    $('#btnActualizar').click(function () {
        actualizarReporte();        
    });
});

function handleClick(myRadio) {
    currentValue = myRadio.value;
   // listarConfigInforme(currentValue);    
    $('#hfidrepor').val(currentValue);
    
}

function actualizarReporte() {
    var idreporte = $('#hfidrepor').val();

    if (idreporte == "") {
        alert("Debe seleccionar un informe!");
    }
    else {
        window.location = controler + 'DetalleInformeListado?id=' + idreporte;
    }
}