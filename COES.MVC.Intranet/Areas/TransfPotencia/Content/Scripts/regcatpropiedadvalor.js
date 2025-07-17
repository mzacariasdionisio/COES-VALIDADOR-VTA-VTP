var controlador = siteRoot + 'TransfPotencia/Potenciafirme/'

$(function () {
    $("#btnNuevo").click(function () {
        openPopupCrear();
    });

    $('#cbTipTecno').change(function () {
        cargarCategoriadet();
    });
});

function popupNuevo() {
    $.ajax({
        type: 'POST',
        url: controlador + "PopupNuevoRegCatpropiedadValor",
        data: { idCat: $('#cbTipTecno').val(), id: $('#cbCatedet').val(), descrip: $("#cbCatedet option:selected").text() },
        success: function (evt) {
            $('#popupTablaNuevo').html(evt);
            $('#txtFecha').Zebra_DatePicker();

            $("#txtVal").keyup(function () {
                var $this = $(this);
                $this.val($this.val().replace(/[^\d.]/g, ''));
            })

            $("#btnGrabar").click(function () {
                saveRegCatpropiedadValor();
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

function consultaRegCatpropiedadValor() {
    $.ajax({
        type: 'POST',
        url: controlador + "ConsultaRegCatpropiedadValor",
        dataType: 'json',
        data: { ctgdetcodi: $('#cbCatedet').val(), url: siteRoot },
        success: function (evt) {
            $('#listado').html(evt.Resultado);
            $('#btnNuevo').show();
        },
        error: function (err) { alert("Error..!!"); }
    });
}

function saveRegCatpropiedadValor() {
    $.ajax({
        type: 'POST',
        url: controlador + "SaveRegCatpropiedadValor",
        dataType: 'json',
        data: { eqctpvvalor: $('#txtVal').val(), eqcatpcodi: $('#cbPropCate').val(), ctgdetcodi: $('#hdCtgdetcodi').val(), fecha: $('#txtFecha').val() },
        success: function (evt) {
            if (evt.Nregistros != -1) {
                alert("Transaccion exitosa..!!");
                consultaRegCatpropiedadValor();
                $('#popupTablaNuevo').bPopup().close();
            }
            else { alert("Ha ocurrido un error."); }
        },
        error: function (err) { alert("Error..!!"); }
    });
}

function cargarCategoriadet() {
    $("#cbCatedet").empty();
    $('#cbCatedet').get(0).options[0] = new Option("--TODOS--", "-1");

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarCategoriaDet',
        dataType: 'json',
        data: {
            ctgcodi: $('#cbTipTecno').val()
        },
        success: function (data) {
            if (data.ListaEqCategoriaDet.length > 0) {
                $.each(data.ListaEqCategoriaDet, function (i, item) {
                    $('#cbCatedet').get(0).options[$('#cbCatedet').get(0).options.length] = new Option(item.Ctgdetnomb, item.Ctgdetcodi);
                });
            }

            $('#cbCatedet').change(function () {
                consultaRegCatpropiedadValor();
            });
        },
        error: function (err) { alert("Error..!!"); }
    });
}

function cargarPropiedadvalor() {
    $("#cbPropCate").empty();
    $('#cbPropCate').get(0).options[0] = new Option("--TODOS--", "-1");

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarPropiedadValor',
        dataType: 'json',
        data: {
            ctgcodi: $('#cbTipTecno').val()
        },
        success: function (data) {
            if (data.ListaEqCatpropiedad.length > 0) {
                $.each(data.ListaEqCatpropiedad, function (i, item) {
                    $('#cbPropCate').get(0).options[$('#cbPropCate').get(0).options.length] = new Option(item.Eqcatpnomb, item.Eqcatpcodi);
                });
            }

            $('#cbPropCate').change(function () {
                consultaRegCatpropiedadValor();
            });
        },
        error: function (err) { alert("Error..!!"); }
    });
}

function eliminarReg(x, y) {
    $.ajax({
        type: 'POST',
        url: controlador + "DeleteRegCatpropiedadValor",
        dataType: 'json',
        data: { eqctpvcodi: x, eqctpvfechadat: y },
        success: function (evt) {
            if (evt.Nregistros == 1) {
                alert("Transaccion exitosa..!!");
                consultaRegCatpropiedadValor();
                $('#popupTablaNuevo').bPopup().close();
            } else {
                if (evt.Nregistros == -2) { alert("La fecha de regimiento es menor a la fecha actual."); }
                else { alert("Ha ocurrido un error."); }
            }
        },
        error: function (err) { alert("Error..!!"); }
    });
}