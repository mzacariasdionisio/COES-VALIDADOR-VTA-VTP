var controlador = siteRoot + 'TransfPotencia/Potenciafirme/'

$(function () {
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            consultaIndispo();
        }
    });

    $("#btnValidar").click(function () {
        saveIndispo();
    });

    consultaIndispo();
});

function saveIndispo() {
    $.ajax({
        type: 'POST',
        url: controlador + "SaveIndispo",
        dataType: 'json',
        data: { fecha: $('#txtFecha').val() },
        success: function (evt) {
            if (evt.Nregistros != -1) {
                alert("Transaccion exitosa..!!");
                consultaIndispo();
            }
            else { alert("Ha ocurrido un error."); }
        },
        error: function (err) { alert("Error al cargar Excel Web"); }
    });
}

function consultaIndispo() {
    $.ajax({
        type: 'POST',
        url: controlador + "ConsultaIndispo",
        dataType: 'json',
        data: {
            fecha: $('#txtFecha').val()
        },
        success: function (evt) {
            if (evt.Nregistros > 0) {
                $('#listado').html(evt.Resultado);
                $("#tabla").dataTable();
            } else { $("#btnValidar").hide(); $("#listado").html(""); }
        },
        error: function (err) { alert("Error..!!"); }
    });
}