var controler = siteRoot + "PrimasRER/FactorPerdida/";
var controlerDetalle = siteRoot + "PrimasRER/FactorPerdidaDetalle/";
$(document).ready(function () {
    buscar();
    $('.txtFecha').Zebra_DatePicker({
        disabled: true
    });
    $('#btnNuevo').click(function () {
        nuevo();
    });

});

buscar = function () {
    $.ajax({
        type: 'POST',
        url: controler + "lista",
        success: function (evt) {
            $('#listado').html(evt);
            addDeleteEvent();
            viewEvent();
            oTable = $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true",
                "aaSorting": [[0, "desc"], [1, "desc"], [2, "asc"]]
            });
        },
        error: function () {
            mostrarError();
        }
    });
};

nuevo = function () {
    var id = 0
    window.location.href = controlerDetalle + "index?id=" + id;
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
                    if (resultado == "true") {
                        $("#fila_" + id).remove();
                        mostrarExito("La operación se realizó con exito...!");
                    }
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


editarFactor = function (Rerpprcodi) {
    console.log("Editar Factor de perdidas")
    $.ajax({
        type: 'POST',
        url: controler + "EditFactor",
        data: {
            Rerfpdcodi: Rerfpdcodi
        },
        success: function (evt) {
            $('#edit').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupEditParametroRER').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function () {
            mostrarError();
        }
    });
};

mostrarExito = function (msg) {
    $('#mensaje').removeClass();
    $('#mensaje').html(msg);
    $('#mensaje').addClass("action-exito");
};
