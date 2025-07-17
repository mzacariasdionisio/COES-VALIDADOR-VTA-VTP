var sControlador = siteRoot + "AporteIntegrantes/maximademanda/";

//Funciones de busqueda
$(document).ready(function () {
    var caiprscodi = document.getElementById('caiprscodi').value;
    var caiajcodi = document.getElementById('caiajcodi').value;
    if (caiprscodi && caiajcodi)
    {
        buscar();
    }

    $('.txtFecha').Zebra_DatePicker({
    });

    $('#btnNuevo').click(function () {
        nuevo();
    });

    $('#btnRefrescar').click(function () {
        refrescar();
    });

    $('#btnBuscar').click(function () {
        buscar();
    });

    $('#btnCopiarMD').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        copiarMD();
    });
});

buscar = function () {
    var caiprscodi = document.getElementById('caiprscodi').value;
    var caiajcodi = document.getElementById('caiajcodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'Lista',
        data: { caiajcodi: caiajcodi },
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
            alert("Lo sentimos, se ha producido un error");
        }
    });
};

copiarMD = function () {
    var caiprscodi = document.getElementById('caiprscodi').value;
    var caiajcodi = document.getElementById('caiajcodi').value;
    if (caiajcodi == "") {
        mostrarError("Por favor, seleccionar una versión de recalculo");
    }
    else {
        $.ajax({
            type: 'POST',
            url: sControlador + 'copiarMD',
            data: { caiprscodi: caiprscodi, caiajcodi: caiajcodi },
            dataType: 'json',
            success: function (model) {
                if (model.sError == "") {
                    mostrarExito("Felicidades, se copio correctamente la información de " + model.iNumReg + " meses.");
                    buscar();
                }
                else {
                    mostrarError("Lo sentimos, ha ocurrido el siguiente error: " + model.sError);
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    }
}

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
    var caiajcodi = document.getElementById('caiajcodi').value;
    window.location.href = sControlador + "new?caiajcodi=" + caiajcodi;
}

refrescar = function () {
    var caiprscodi = document.getElementById('caiprscodi').value;
    window.location.href = sControlador + "index?caiprscodi=" + caiprscodi;
}