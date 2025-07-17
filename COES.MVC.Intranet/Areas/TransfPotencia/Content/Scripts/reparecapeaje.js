var controler = siteRoot + "transfpotencia/reparecapeaje/";

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
                "destroy": "true"
            });
        },
        error: function () {
            mostrarError("Lo sentimos, se ha producido un error");
        }
    });
};

//Funciones de vista detalle
viewEvent = function () {
    $('.view').click(function () {
        rrpecodi = $(this).attr("id").split("_")[1];
        abrirPopup(rrpecodi);
    });
};

abrirPopup = function (rrpecodi) {
    var pericodi = $('#EntidadRecalculoPotencia_Pericodi').val();
    var recpotcodi = $('#EntidadRecalculoPotencia_Recpotcodi').val();
    $.ajax({
        type: 'POST',
        url: controler + "View",
        data: { pericodi: pericodi, recpotcodi: recpotcodi, rrpecodi: rrpecodi },
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

//Funciones de eliminado de registro
addDeleteEvent = function () {
    $(".delete").click(function (e) {
        e.preventDefault();
        if (confirm("¿Desea eliminar la información seleccionada?")) {
            rrpecodi = $(this).attr("id").split("_")[1];
            var pericodi = $('#EntidadRecalculoPotencia_Pericodi').val();
            var recpotcodi = $('#EntidadRecalculoPotencia_Recpotcodi').val();
            $.ajax({
                type: "post",
                dataType: "text",
                url: controler + "Delete",
                data: addAntiForgeryToken({ pericodi: pericodi, recpotcodi :recpotcodi, rrpecodi: rrpecodi }),
                success: function (resultado) {
                    if (resultado == "true") {
                        $("#fila_" + rrpecodi).remove();
                        mostrarExito("Se ha eliminado correctamente el registro");
                    }
                    else
                        mostrarError("No se ha logrado eliminar el registro");
                }
            });
        }
    });
};
