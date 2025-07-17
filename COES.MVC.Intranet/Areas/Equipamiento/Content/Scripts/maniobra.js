var controlador = siteRoot + 'equipamiento/maniobra/'

$(function () {

    $('#cbFamilia').change(function () {
        listarEquipos();
    });

    $('#btnGrabar').click(function () {
        grabarEnlace();
    });

    $('#btnCancelar').click(function () {
        $('#popupEnlace').bPopup().close();
    });

    listarEquipos();
});

listarEquipos = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "listar",
        data: {
            famCodi: $('#cbFamilia').val()
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(evt);
            $('#tabla').dataTable({                
            });
        },
        error: function () {
            mostrarError();
        }
    });
}

actualizarProcedimiento = function (idEquipo, idFamilia, enlace) {
    $('#hfIdEquipo').val(idEquipo);
    $('#hfIdFamilia').val(idFamilia);
    $('#txtEnlace').val(enlace);

    $('#popupEnlace').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

grabarEnlace = function ()
{
    alert("hola");
    $.ajax({
        type: 'POST',
        url: controlador + 'actualizarenlace',
        dataType: 'json',
        data: {
            idEquipo: $('#hfIdEquipo').val(),
            idFamilia: $('#hfIdFamilia').val(),
            enlace: $('#txtEnlace').val()
        },
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                listarEquipos();
                $('#popupEnlace').bPopup().close();
            }
            else {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

mostrarError = function () {
    alert("Ha ocurrido un error.");
}