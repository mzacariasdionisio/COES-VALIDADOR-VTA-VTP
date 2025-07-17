var compensacion = "compensacion/";
var controler = siteRoot + "transferencias/" + compensacion;

//Funciones de busqueda
$(document).ready(function () {
    buscar();
    $('#btnBuscar').click(function () {
        buscar();
    });

    $('#btnGenerarExcel').click(function () {
        generarExcel();
    });
});

function buscar() {
    $.ajax({
        type: 'POST',
        url: controler + "lista",
        data: { nombre: $('#txtNombre').val()  },
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
};

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

//Funciones de eliminado de registro
function addDeleteEvent() {
    $(".delete").click(function (e) {
        e.preventDefault();
        if (confirm("¿Desea eliminar la información seleccionada?")) {
            id = $(this).attr("id").split("_")[1];
            $.ajax({
                type: "post",
                dataType: "text",
                url: controler + "Delete/" + id,
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

function viewEvent() {
    $('.view').click(function () {
        id = $(this).attr("id").split("_")[1];
        abrirPopup(id);
    });
};

abrirPopup = function (id) {
    $('#popup').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown',
        loadUrl: controler + "View/" + id
    }
    );
}
