var sControlador = siteRoot + "AporteIntegrantes/ajuste/";

//Funciones de busqueda
$(document).ready(function () {
    var caiprscodi = document.getElementById('EntidadPresupuesto_Caiprscodi').value;
    if (caiprscodi) {
        buscar();
    }

    $('#btnNuevo').click(function () {
        nuevo();
    });

    $('#btnRefrescar').click(function () {
        refrescar();
    });
    
    $('#btnBuscar').click(function () {
        buscar();
    });
});

buscar = function () {
    var caiprscodi = document.getElementById("EntidadPresupuesto_Caiprscodi").value;
    $.ajax({
        type: 'POST',
        url: sControlador + "Lista",
        data: { caiprscodi: caiprscodi },
        success: function (evt) {
            $('#listado').html(evt);
            addDeleteEvent();
            viewEvent();
            oTable = $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true",
                "aaSorting": [[1, "desc"]]
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
    var caiprscodi = document.getElementById("EntidadPresupuesto_Caiprscodi").value;
    window.location.href = sControlador + "new?caiprscodi=" + caiprscodi;
}

refrescar = function () {
    window.location.href = sControlador + "index";
}