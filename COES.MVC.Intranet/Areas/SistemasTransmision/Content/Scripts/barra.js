var sControlador = siteRoot + "SistemasTransmision/Barra/";

$(document).ready(function () {
    $('#btnNuevo').hide();
    buscarBarra();

    $('#btnConsultar').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
    });

    if ($('#stpercodi').val() > 0 && $('#strecacodi').val() > 0) {

    }

    $('#tab-container').easytabs({
        animate: false
    });
});

buscarBarra = function () {
    $.ajax({
        type: 'POST',
        url: sControlador + "List",
        data: { strecacodi: $('#strecacodi').val() },
        success: function (evt) {
            $('#listadoBarra').html(evt);
            addDeleteEvent();
            oTable = $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true"
            });
            $('#btnNuevo').show();
            mostrarExito('Puede consultar y modificar la información');
        },
        error: function () {
            mostrarError("Lo sentimos, se ha producido un error");
        }
    });
};

addDeleteEvent = function() {
    $(".delete").click(function (e) {
        e.preventDefault();
        if (confirm("¿Desea eliminar la información seleccionada?")) {
            id = $(this).attr("id").split("_")[1];
            $.ajax({
                type: "post",
                dataType: "text",
                url: sControlador + "Delete/" + id,
                data: addAntiForgeryToken({ id: id }),
                success: function (resultado) {
                    if (resultado == "true")
                        $("#fila_" + id).remove();
                    else
                        alert("No se ha logrado eliminar el registro");
                }
            });
        }
    });
};