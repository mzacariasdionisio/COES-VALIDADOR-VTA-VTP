var controlador = siteRoot + 'PMPO/GeneracionArchivosDAT/';
/**
* Llamadas iniciales
* @returns {} 
*/
$(document).ready(function () {
    $('#tabla').dataTable({
        //"paging": false
        "pageLength": 25
    });

});

function GenerarRepGrupoRelaso() {

    var idDef = '2,3';

    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarRepGrupoRelaso',
        dataType: 'json',
        data: {
            strGrrdefcodi: idDef
        },
        success: function (result) {

            if (result != -1) {
                document.location.href = controlador + 'descargar?formato=' + 1 + '&file=' + result
                mostrarExitoOperacion("Exportación realizada.");
            }
            else {
                mostrarError('Opcion Exportar: Ha ocurrido un error');
            }

        },
        error: function () {
            mostrarMensaje('Ha ocurrido un error');
        }
    });
}