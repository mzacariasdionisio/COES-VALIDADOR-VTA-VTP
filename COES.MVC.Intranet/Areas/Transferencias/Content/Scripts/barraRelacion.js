var controler = siteRoot + "transferencias/barra/";

//Funciones de busqueda
$(document).ready(function () {

    viewEvent();
    oTable = $('#tabla').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true"
    });

});

addAntiForgeryToken = function (data) {
    data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
    return data;
};

//Funciones de vista detalle
function viewEvent() {
    $('.view').click(function (e) {
        e.preventDefault();
        var id = $(this).attr("id").split("_")[1];

        if (id == null) {
            return;
        }

        $.ajax({
            type: 'POST',
            url: controler + "ListaSuministro",
            data: { barrtTra: id},
            success: function (evt) {
                $('#listaSuministro').html(evt);
                //add_deleteEvent();
            },
            error: function () {
                alert("Lo sentimos, se ha producido un error");
            }
        });
    });
};


