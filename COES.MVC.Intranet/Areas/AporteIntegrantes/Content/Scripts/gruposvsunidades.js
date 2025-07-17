var sControlador = siteRoot + "AporteIntegrantes/gruposvsunidades/";

//Funciones de busqueda
$(document).ready(function () {

    var index = document.getElementById("index").value;
    if (index) {
        buscar();
    }

    $('#btnNuevo').click(function () {
        nuevo();
    });

    $('.txtFecha').Zebra_DatePicker({

    });

    $('#btnRefrescar').click(function () {
        refrescar();
    });

    $('#btnBuscar').click(function () {
        buscar();
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
                "aaSorting": [[0, "asc"], [1, "asc"]]
            });
        },
        error: function () {
            alert("Lo sentimos, se ha producido un error");
        }
    });
};

//Funciones de eliminado de registro
addDeleteEvent = function () {
    $(".delete").click(function (e) {
        e.preventDefault();
        if (confirm("¿Desea eliminar la información seleccionada?")) {
            casdducodi = $(this).attr("id").split("_")[1];
            $.ajax({
                type: "POST",
                dataType: "text",
                url: sControlador + "Delete/" + casdducodi,
                data: addAntiForgeryToken({ casdducodi: casdducodi }),
                success: function (resultado) {
                    if (resultado == "true")
                        $("#fila_" + casdducodi).remove();
                    else
                        alert("No se ha logrado eliminar el registro");
                }
            });
        }
    });
};

addAntiForgeryToken = function (data) {
    data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
    return data;
};

//Funciones de vista detalle
viewEvent = function () {
    $('.view').click(function () {
        casdducodi = $(this).attr("id").split("_")[1];
        abrirPopup(casdducodi);
    });
};

abrirPopup = function (casdducodi) {
    $.ajax({
        type: 'POST',
        url: sControlador + "View/" + casdducodi,
        data: {casdducodi:casdducodi},
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
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

nuevo = function () {
    window.location.href = sControlador + "new";
}

refrescar = function () {
    window.location.href = sControlador + "index";
}