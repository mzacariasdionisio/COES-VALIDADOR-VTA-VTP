var recalculo = "recalculo/";
var controler = siteRoot + "transferencias/" + recalculo;

//Funciones de busqueda
$(document).ready(function () {
    buscar();

    $('.txtFecha').Zebra_DatePicker({

    });

    $('#btnBuscar').click(function () {
        buscar();
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
function addDeleteEvent() {
    $(".delete").click(function (e) {
        e.preventDefault();
        id = $(this).attr("id").split("_")[2];
        est = $(this).attr("id").split("_")[1];
        if (est == "a") {
            if (confirm("¿Desea eliminar la revisión seleccionada?, considerar que si cuenta con valorización esta tambien será eliminada!")) {
                console.log(id);
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
        } else {
            alert("Revisión no puede ser eliminada ya que se encuentra Cerrada!");
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
    $.ajax({
        type: 'POST',
        url: controler + "View/" + id,
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

