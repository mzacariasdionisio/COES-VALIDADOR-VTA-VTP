var controler = siteRoot + "transfpotencia/peajeingreso/";

//Funciones de busqueda
$(document).ready(function () {
    buscar();
    $('#btnBuscar').click(function () {
        buscar();
    });

    $('#btnGenerarExcel').click(function () {
        descargarArchivo(1);
    });

});

function buscar() {
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
                "aaSorting": [[3, "desc"], [4, "desc"], [1, "desc"], [0, "asc"], [2, "asc"]]
            });
        },
        error: function () {
            mostrarError("Lo sentimos, se ha producido un error");
        }
    });
};

//Funciones de exportación de datos
descargarArchivo = function (formato) {
    $.ajax({
        type: 'POST',
        url: controler + 'exportardata',
        data: { pericodi: $('#EntidadRecalculoPotencia_Pericodi').val(), recpotcodi: $('#EntidadRecalculoPotencia_Recpotcodi').val(), formato: formato },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                window.location = controler + 'abrirarchivo?formato=' + formato + '&file=' + result;
                mostrarExito("Felicidades, el archivo se descargo correctamente...!");
            }
            else {
                mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });
}

//Funciones de eliminado de registro
addDeleteEvent = function () {
    $(".delete").click(function (e) {
        e.preventDefault();
        if (confirm("¿Desea eliminar la información seleccionada?")) {
            pingcodi = $(this).attr("id").split("_")[1];
            var pericodi = $('#EntidadRecalculoPotencia_Pericodi').val();
            var recpotcodi = $('#EntidadRecalculoPotencia_Recpotcodi').val();
            $.ajax({
                type: "post",
                dataType: "text",
                url: controler + "Delete",
                data: addAntiForgeryToken({ pericodi: pericodi, recpotcodi: recpotcodi, pingcodi: pingcodi }),
                success: function (resultado) {
                    if (resultado == "true") {
                        $("#fila_" + pingcodi).remove();
                        mostrarExito("Se ha eliminado correctamente el registro");
                    }
                    else
                        mostrarError("No se ha logrado eliminar el registro");
                }
            });
        }
    });
};

//Funciones de vista detalle
viewEvent = function () {
    $('.view').click(function () {
        pingcodi = $(this).attr("id").split("_")[1];
        abrirPopup(pingcodi);
    });
};

abrirPopup = function (pingcodi) {
    var pericodi = $('#EntidadRecalculoPotencia_Pericodi').val();
    var recpotcodi = $('#EntidadRecalculoPotencia_Recpotcodi').val();
    $.ajax({
        type: 'POST',
        url: controler + "View",
        data: { pericodi: pericodi, recpotcodi: recpotcodi, pingcodi: pingcodi },
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