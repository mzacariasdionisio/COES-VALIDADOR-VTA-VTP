var controler = siteRoot + "transfpotencia/recalculopotencia/";

//Funciones de busqueda
$(document).ready(function () {
    buscar();
    $('#btnBuscar').click(function () {
        buscar();
    });

});

buscar = function () {
    $.ajax({
        type: 'POST',
        url: controler + "lista",
        data: { nombre: $('#txtNombre').val() },
        success: function (evt) {
            $('#listado').html(evt);
            addDeleteEvent();
            viewEvent();
            oTable = $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true",
                "aaSorting": [[0, "desc"], [1, "desc"]]
            });
        },
        error: function () {
            mostrarError("Lo sentimos, se ha producido un error");
        }
    });
};

//Funciones de eliminado de registro
addDeleteEvent = function () {
    $(".delete").click(function (e) {
        e.preventDefault();
        if (confirm("¿Desea eliminar la información seleccionada?")) {
            pericodi = $(this).attr("id").split("_")[1];
            recpotcodi = $(this).attr("id").split("_")[2];
            $.ajax({
                type: "post",
                dataType: "text",
                url: controler + "Delete",
                data: addAntiForgeryToken({ pericodi: pericodi, recpotcodi: recpotcodi }),
                success: function (resultado) {
                    console.log(resultado);
                    if (resultado == "true") {
                        $("#fila_" + pericodi + "_" + recpotcodi).remove();
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

//Funciones de vista detalle
viewEvent = function () {
    $('.view').click(function () {
        pericodi = $(this).attr("id").split("_")[1];
        recpotcodi = $(this).attr("id").split("_")[2];
        abrirPopup(pericodi, recpotcodi);
    });
};

abrirPopup = function (pericodi, recpotcodi) {
    $.ajax({
        type: 'POST',
        url: controler + "View",
        data: { pericodi: pericodi, recpotcodi: recpotcodi },
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

