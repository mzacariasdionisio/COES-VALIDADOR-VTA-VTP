var sControlador = siteRoot + "sistemastransmision/compensacion/";

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

buscar = function () {
    var id = document.getElementById('EntidadSistema_Sistrncodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + "lista",
        data: { id: id },
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
                        alert("No se ha logrado eliminar el registro:" + resultado);
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
        id = $(this).attr("id").split("_")[1];
        abrirPopup(id);
    });
};

abrirPopup = function (id) {
    $.ajax({
        type: 'POST',
        url: sControlador + "View/" + id,
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
    var Sistrncodi = document.getElementById('EntidadSistema_Sistrncodi').value;
    window.location.href = sControlador + "new?id=" + Sistrncodi;
}

regresar = function () {
    var Stpercodi = document.getElementById('EntidadRecalculo_Stpercodi').value;
    var Strecacodi = document.getElementById('EntidadRecalculo_Strecacodi').value;
    window.location.href = siteRoot + "sistemastransmision/elementost/index?stpercodi=" + Stpercodi + "&strecacodi=" + Strecacodi;
}