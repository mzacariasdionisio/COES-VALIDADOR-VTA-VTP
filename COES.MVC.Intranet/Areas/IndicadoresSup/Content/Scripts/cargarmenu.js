var controlador = siteRoot + 'IndicadoresSup/reporteejecutivo/';

$(function () {
    cargarMenu();

    $('#openMenuSiosein').click(function () {
        $('#contenedorMenuSiosein').slideToggle("slow");
    });

    $('#closeMenuSiosein').click(function () {
        $('#contenedorMenuSiosein').css("display", "none");
    });
});

function fnClick(x) {
    document.location.href = controlador + x;
}

function cargarMenu() {

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarMenu',
        dataType: 'json',
        success: function (e) {
            $('#MenuID').html(e.Resultado);
            $('#myTable').DataTable({
                "paging": false,
                "lengthChange": false,
                "pagingType": false,
                "ordering": false,
                "info": false
            });
            $('#CodiMenu').val(1);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

