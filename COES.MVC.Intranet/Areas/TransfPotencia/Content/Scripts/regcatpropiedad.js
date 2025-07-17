var controlador = siteRoot + 'TransfPotencia/Potenciafirme/'

$(function () {

    $("#btnNuevo").click(function () {
        openPopupCrear();
    });

    $('#cbTipTecno').change(function () {
        consultaRegCatpropiedad();
    });

    consultaRegCatpropiedad();
});

function popupNuevo() {
    $.ajax({
        type: 'POST',
        url: controlador + "PopupNuevoRegCatpropiedad",
        data: { id: $('#cbTipTecno').val(), descrip: $("#cbTipTecno option:selected").text() },
        success: function (evt) {
            $('#popupTablaNuevo').html(evt);

            $("#btnGrabar").click(function () {
                saveRegCatpropiedad();
            });
        },
        error: function (err) { alert("Error..!!"); }
    });
}

function openPopupCrear() {
    popupNuevo();
    setTimeout(function () {
        $('#popupTablaNuevo').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}

function consultaRegCatpropiedad() {
    $.ajax({
        type: 'POST',
        url: controlador + "ConsultaRegCatpropiedad",
        dataType: 'json',
        data: { ctgcodi: $('#cbTipTecno').val(), url: siteRoot },
        success: function (evt) {
            $('#listado').html(evt.Resultado);
        },
        error: function (err) { alert("Error..!!"); }
    });
}

function saveRegCatpropiedad() {
    $.ajax({
        type: 'POST',
        url: controlador + "SaveRegCatpropiedad",
        dataType: 'json',
        data: { nam: $('#txtCatepNomb').val(), ctgcodi: $('#hdCtgcodi').val() },
        success: function (evt) {
            if (evt.Nregistros != -1) {
                alert("Transaccion exitosa..!!");
                consultaRegCatpropiedad();
                $('#popupTablaNuevo').bPopup().close();
            }
            else { alert("Ha ocurrido un error."); }
        },
        error: function (err) { alert("Error al cargar Excel Web"); }
    });
}