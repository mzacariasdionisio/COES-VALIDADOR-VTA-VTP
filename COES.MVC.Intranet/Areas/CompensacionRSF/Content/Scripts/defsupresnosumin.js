var sControlador = siteRoot + "CompensacionRSF/DefSupResNoSumin/";


$(document).ready(function () {
    buscar();
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
                "aaSorting": [[0, "desc"]]
            });
        },
        error: function () {
            mostrarError();
        }
    });
};

nuevo = function () {
    window.location.href = sControlador + "new";
}

//Funciones de eliminado de registro
addDeleteEvent = function () {
    $(".delete").click(function (e) {
        e.preventDefault();
        if (confirm("¿Desea eliminar la información seleccionada?")) {
            pericodi = $(this).attr("id").split("_")[1];
            vcrdsrcodi = $(this).attr("id").split("_")[2];
            $.ajax({
                type: "post",
                dataType: "text",
                url: sControlador + "Delete",
                data: addAntiForgeryToken({ pericodi: pericodi, vcrdsrcodi: vcrdsrcodi }),
                success: function (resultado) {
                    console.log(resultado);
                    if (resultado == "true") {
                        $("#fila_" + pericodi + "_" + vcrdsrcodi).remove();
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
        vcrdsrcodi = $(this).attr("id").split("_")[2];
        abrirPopup(pericodi, vcrdsrcodi);
    });
};

abrirPopup = function (pericodi, vcrdsrcodi) {
    $.ajax({
        type: 'POST',
        url: sControlador + "View",
        data: { pericodi: pericodi, vcrdsrcodi: vcrdsrcodi },
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

