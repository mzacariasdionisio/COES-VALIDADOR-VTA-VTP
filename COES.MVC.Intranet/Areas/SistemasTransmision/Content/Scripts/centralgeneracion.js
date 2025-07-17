var sControlador = siteRoot + "sistemastransmision/centralgeneracion/";

$(function () {
    $('#btnNuevo').click(function () {
        nuevo();
    });

    $('#btnRegresar').click(function () {
        regresar();
    });
    
    $('#btnBuscar').click(function () {
        buscar();
    });

    buscar();
});

buscar = function() {
    $.ajax({
        type: 'POST',
        url: sControlador + "lista",
        data: { nombre: $('#txtNombre').val() },
        success: function (evt) {
            $('#listado').html(evt);
            addDeleteEvent();
            oTable = $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true"
            });
        },
        error: function () {
            mostrarError();
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

nuevo = function ()
{
    var Stgenrcodi = document.getElementById('EntidadGenerador_Stgenrcodi').value;
    window.location.href = sControlador + "new?id=" + Stgenrcodi;
}

regresar = function () {
    var Stpercodi = document.getElementById('EntidadRecalculo_Stpercodi').value;
    var Strecacodi = document.getElementById('EntidadRecalculo_Strecacodi').value;
    window.location.href = siteRoot + "sistemastransmision/empresa/index?stpercodi=" + Stpercodi + "&strecacodi=" + Strecacodi;
}