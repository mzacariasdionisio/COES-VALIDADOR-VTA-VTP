$(document).ready(() => {
    //$(".editar").click(function (e) {
    //    e.preventDefault();
    //    var id = $(this).attr("id").split("_")[1];
    //    $.ajax({
    //        type: 'POST',
    //        url: controler + "Edit",
    //        data: { id: id },
    //        success: function (evt) {
    //            $('#popupEq #contenidoPopup').html(evt);
    //            setTimeout(function () {
    //                $('#popupEq').bPopup({
    //                    easing: 'easeOutBack',
    //                    speed: 450,
    //                    transition: 'slideDown'
    //                });
    //            }, 50);
    //        },
    //        error: function () {
    //            mostrarError();
    //        }
    //    });
    //});

    $(".eliminar").click(function (e) {
        e.preventDefault();
        var id = $(this).attr("id").split("_")[1];

        if (confirm("¿Esta seguro de eliminar el registro?")) {

            $.ajax({
                type: 'POST',
                url: controler + "Delete",
                data: { id: id },
                success: function (evt) {
                    if (evt == "True") {
                        $('#popupMensajeZ #btnAceptarMsj').hide();
                        $('#popupMensajeZ #cmensaje').html('<div class="exito mensajes">El registro se ha eliminado</div>');
                        setTimeout(function () {
                            $('#popupMensajeZ').bPopup({
                                easing: 'easeOutBack',
                                speed: 450,
                                transition: 'slideDown',
                            });
                        }, 50);
                        location.reload();
                    }
                    else {
                        $('#popupMensajeZ #btnAceptarMsj').hide();
                        $('#popupMensajeZ #cmensaje').html('<div class="error mensajes">No se pudo eliminar el registro</div>');
                        setTimeout(function () {
                            $('#popupMensajeZ').bPopup({
                                easing: 'easeOutBack',
                                speed: 450,
                                transition: 'slideDown',
                            });
                        }, 50);
                    }

                },
                error: function () {
                    mostrarError();
                }
            });

        }


    });

})