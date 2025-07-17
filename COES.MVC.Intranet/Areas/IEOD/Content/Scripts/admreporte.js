var controlador = siteRoot + 'IEOD/Gestor/';

$(function () {

    //$('#cbTipReporte').multipleSelect({
    //    width: '150px',
    //    filter: true,
    //    onClose: function (view) {
    //        cargarListaAdmReporte();
    //    }
    //});
    //$('#cbTipReporte').multipleSelect('checkAll');
    $('#cbTipReporte').change(function () {
        cargarListaAdmReporte();
    });

    cargarListaAdmReporte();
});

function updItem(id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'updItemAdmReporte',
        data: { repcodi: id },
        dataType: 'json',
        success: function (e) {
            if (e > 0) {
                cargarListaAdmReporte();
            }
            else {
                alert('Item no actualizado');
            }
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}

function cargarListaAdmReporte() {
    if ($('#cbTipReporte').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarListaAdmReporte',
            data: { reptiprepcodi: $('#cbTipReporte').val() },
            success: function (e) {
                $('#listado').html(e);
                $('#rep_pr5').dataTable({
                    "ordering": true,
                    "paging": false,
                    "bDestroy": true,
                    "info": false
                });
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}