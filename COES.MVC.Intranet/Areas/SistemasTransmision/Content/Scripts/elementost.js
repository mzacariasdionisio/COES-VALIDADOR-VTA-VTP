var sControlador = siteRoot + "sistemastransmision/elementost/";

$(document).ready(function () {
    $('#btnNuevo').click(function () {
        nuevo();
    });

    $('#btnExportarSistema').click(function () {
        exportarSistema(1);
    });

    $('#btnRefrescar').click(function () {
        refrescar();
    });

    $('#btnConsultar').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        buscar();
    });

    if ($('#pericodi').val() > 0 && $('#recacodi').val() > 0) {
        buscar();
    }

});

buscar = function () {
    $.ajax({
        type: 'POST',
        url: sControlador + "lista",
        data: { recacodi: $('#recacodi').val() },
        success: function (evt) {
            $('#listado').html(evt);
            addDeleteEvent();
            viewEvent();
            oTable = $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true"
            });
            $('#btnNuevo').show();
            mostrarExito('Puede consultar y modificar la información');
        },
        error: function () {
            mostrarError("Lo sentimos, se ha producido un error");
        }
    });
};

//Descripcion
viewEvent = function () {
    $('.view').click(function () {
        id = $(this).attr("id").split("_")[1];
        abrirPopup(id);
    });
};

abrirPopup = function (id) {
    $.ajax({
        type: 'POST',
        url: sControlador + "View",
        data: { id: id },
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
            id = $(this).attr("id").split("_")[1];
            $.ajax({
                type: "post",
                dataType: "text",
                url: sControlador + "Delete/" + id,
                data: addAntiForgeryToken({ id: id }),
                success: function (resultado) {
                    if (resultado == "true") {
                        $("#fila_" + id).remove();
                        mostrarExito("El registro se ha eliminado correctamente");
                    }
                    else
                        mostrarError("No se ha logrado eliminar el registro");
                }
            });
        }
    });
};

Recargar = function () {
    var cmbPericodi = document.getElementById('pericodi');
    var cmbRecacodi = document.getElementById('recacodi');
    window.location.href = sControlador + "index?pericodi=" + cmbPericodi.value + "&recacodi=" + cmbRecacodi.value;
    //index?pericodi=1&recacodi=2
}

RecargarTitular = function () {
    var cboTitular = document.getElementById('emprcodi');
    var cmbPericodi = document.getElementById('pericodi');
    var cmbRecacodi = document.getElementById('recacodi');
    window.location.href = sControlador + "New?pericodi=" + cmbPericodi.value + "&recacodi=" + cmbRecacodi.value + "&emprcodi=" + cboTitular.value;
    //New?pericodi=1&recacodi=1&emprcodi=1
}

RecargarActualizar = function () {
    var idsistema = document.getElementById('IDSistema');
    var idempresa = document.getElementById('emprcodi');
    window.location.href = sControlador + "Edit?id=" + idsistema.value + "&emprcodi=" + idempresa.value;
    //Edit?id=5&emprcodi=3
}

nuevo = function () {
    var Stpercodi = document.getElementById('EntidadRecalculo_Stpercodi').value;
    var Strecacodi = document.getElementById('EntidadRecalculo_Strecacodi').value;
    window.location.href = sControlador + "new?pericodi=" + Stpercodi + "&recacodi=" + Strecacodi;
}

exportarSistema = function (formato) {
    var Stpercodi = document.getElementById('EntidadRecalculo_Stpercodi').value;
    var Strecacodi = document.getElementById('EntidadRecalculo_Strecacodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'ExportarSistema',
        data: { stpercodi: Stpercodi, strecacodi: Strecacodi, formato: formato },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                window.location = sControlador + 'abrirarchivo?formato=' + formato + '&file=' + result;
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

refrescar = function () {
    window.location.href = sControlador + "index";
}