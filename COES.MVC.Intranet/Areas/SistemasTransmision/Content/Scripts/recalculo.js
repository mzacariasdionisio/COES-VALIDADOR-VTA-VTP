var sControlador = siteRoot + "SistemasTransmision/recalculo/";

//Funciones de busqueda
$(document).ready(function () {
    buscar();
    $('#btnNuevo').click(function () {
        nuevo();
    });

    $('#btnRegresar').click(function () {
        regresar();
    });

    $('#btnBuscar').click(function () {
        buscar();
    });
});

buscar = function () {
    $.ajax({
        type: 'POST',
        url: sControlador + "lista",
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
            mostrarError();
        }
    });
};

mostrarError = function () {
    alert("Ha ocurrido un error.");
}

//Funciones de eliminado de registro
addDeleteEvent = function () {
    $(".delete").click(function (e) {
        e.preventDefault();
        if (confirm("¿Desea eliminar la información seleccionada?")) {
            id = $(this).attr("id").split("_")[1];
            //console.log(id);
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

addAntiForgeryToken = function (data) {
    data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
    return data;
};

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
    var Stpercodi = document.getElementById('Entidad_Stpercodi').value;
    window.location.href = sControlador + "new?id=" + Stpercodi;
}

regresar = function () {
    window.location.href = siteRoot + "SistemasTransmision/periodo/index";
}