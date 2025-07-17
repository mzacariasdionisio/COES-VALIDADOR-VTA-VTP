var controler = siteRoot + 'transfpotencia/peajeegreso/';

$(function () {
    $('#btnConsultar').on('click', function () {
        consultar();
    });
});

Recargar = function () {
    var cmbPericodi = document.getElementById('pericodi');
    var cmbRecpotcodi = document.getElementById('recpotcodi');
    window.location.href = controler + "index?pericodi=" + cmbPericodi.value + "&recpotcodi=" + cmbRecpotcodi.value;
};

consultar = function () {

    $.ajax({
        type: 'POST',
        url: controler + 'listado',
        data: {
            pericodi: $('#pericodi').val(),
            recpotcodi: $('#recpotcodi').val()
        },
        success: function (evt) {
            $('#listado').html(evt);
            $('#tabla').dataTable({
                "iDisplayLength": 25
            });
        },
        error: function () {
            alert("Error");
        }
    })
}