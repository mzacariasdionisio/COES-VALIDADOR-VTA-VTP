$.message = "";
var controler = siteRoot + "resarcimientos/reporteentrega/";

$(document).ready(function () {
    $('.btnOpenFileManagerEmpResponsable').click(function () {
        $.ajax({
            type: "POST",
            global: false,
            url: controler + "EmpResponsable",
            //dataType: 'json',
            data: { registro: $(this).attr('data-registro') },
            cache: false,
            success: function (resultado) {
                $('#ele-popup-content').html('<div class="title_tabla_pop_up">' + $('.btnOpenFileManagerEmpResponsable') .attr('title')+ '</div>' + resultado);

                $('#ele-popup-content > .tabla-formulario-empresas').dataTable();

                var t = setTimeout(function () {
                    $('#ele-popup').bPopup();
                    clearTimeout(t);
                }, 60)
            },
            error: function (req, status, error) {
                mensajeOperacion(error);
                validaErrorOperation(req.status);
            }
        });
    });
});








