var controlador = siteRoot + "resarcimientos/puntoentrega/";

$(document).ready(function () {
  
    $('.btnOpenFileManagerEmpResponsable').click(function () {
        $.ajax({
            type: "POST",
            global: false,
            url: controlador+"EmpResponsable",
            data: { registro: $(this).attr('data-registro') },
            cache: false,
            success: function (resultado) {
                $('#ele-popup-content').html('<div class="title_tabla_pop_up">Empresas Generadoras</div>' + resultado);

                $('#ele-popup-content > .tabla-formulario-empresas').dataTable();

                var t = setTimeout(function () {
                    $('#ele-popup').bPopup({ modalClose: false, escClose: false });
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
