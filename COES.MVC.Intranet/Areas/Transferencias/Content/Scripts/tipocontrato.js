var tipocon = "tipocontrato/";
var controler = siteRoot + "transferencias/tipocontrato/";

$(document).ready(function () {
    buscar();
    $('#btnBuscar').click(function () {
        buscar();
    });

    $('#btnGenerarExcel').click(function () {
        generarExcel();
    });
});

//Funciones de eliminado de registro
function addDeleteEvent() {
    $(".delete").click(function (e) {
        e.preventDefault();
        if (confirm("¿Desea eliminar la información seleccionada?")) {
            id = $(this).attr("id").split("_")[1];
            $.ajax({
                type: "post",
                dataType: "text",
                url: tipocon + "Delete/" + id,
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
                "destroy": "true"
            });
        },
        error: function () {
            alert("Lo sentimos, se ha producido un error");
        }
    });
}

generarExcel = function () {
    $.ajax({
        type: 'POST',
        url: controler + 'generarexcel',
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controler + 'abrirexcel';
            }
            if (result == -1) {
                alert("Lo sentimos, se ha producido un error");
            }
        },
        error: function () {
            alert("Lo sentimos, se ha producido un error");
        }
    });
}

mostrarError = function () {
    alert("Ha ocurrido un error.");
}

var controler2 = siteRoot + "transferencias/" + tipocon;

function viewEvent() {

    $('.view').click(function () {
        id = $(this).attr("id").split("_")[1];
        abrirPopup(id);
    });
};

abrirPopup = function (id) {

    $.ajax({
        type: 'POST',
        url: controler2 + "View/" + id,
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