var sControlador = siteRoot + "CompensacionRSF/ProvisionBase/";

$(document).ready(function () {
    buscar();

    $('.txtFecha').Zebra_DatePicker({

    });

    $('#btnNuevo').click(function () {
        nuevo();
    });

});

buscar = function () {
    $.ajax({
        type: 'POST',
        url: sControlador + "lista",
        success: function (evt) {
            $('#listado').html(evt);
            addDeleteEvent();
            viewEvent();
            oTable = $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true",
                "aaSorting": [[0, "desc"], [1, "desc"], [2, "asc"]]
            });
        },
        error: function () {
            mostrarError();
        }
    });
};

//Funciones de eliminado de registro
addDeleteEvent = function () {
    $(".delete").click(function (e) {
        e.preventDefault();
        if (confirm("¿Desea eliminar la información seleccionada?")) {
            vcrpbcodi = $(this).attr("id").split("_")[1];
            $.ajax({
                type: "post",
                dataType: "text",
                url: sControlador + "Delete",
                data: addAntiForgeryToken({ vcrpbcodi: vcrpbcodi}),
                success: function (resultado) {
                    console.log(resultado);
                    if (resultado == "true") {
                        $("#fila_" + vcrpbcodi + "_").remove();
                        mostrarExito("Se ha eliminado correctamente el registro");
                    }
                    else {
                        mostrarError("No se ha logrado eliminar el registro: ");
                    }
                }
            });
        }
    });
};

nuevo = function () {
    window.location.href = sControlador + "new";
}

//Funciones de vista detalle
viewEvent = function () {
    $('.view').click(function () {
        vcrpbcodi = $(this).attr("id").split("_")[1];
        abrirPopup(vcrpbcodi);
    });
};

abrirPopup = function (vcrpbcodi) {
    $.ajax({
        type: 'POST',
        url: sControlador + "View",
        data: { vcrpbcodi: vcrpbcodi},
        success: function (evt) {
            $('#popup').html(evt);
            setTimeout(function () {
                $('#popup').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            mostrarError("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}