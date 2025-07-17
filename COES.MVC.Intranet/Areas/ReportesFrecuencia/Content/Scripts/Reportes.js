var controler = siteRoot + "ReportesFrecuencia/ReporteFrecuenciaAudit/"

$(document).ready(function () {

    $('#frmEquipoGPS').submit(function (event) {
        var sError = '';

        // Validar Código
        if ($('#IdGPS').val().trim() === '') {
            sError += 'El campo Código es obligatorio.\n';
        }

        // Validar Fecha Inicio
        if ($('#FechaInicial').val().trim() === '') {
            sError += 'El campo Fecha Inicio es obligatorio.\n';
        }

        // Validar Fecha Fin
        if ($('#FechaFinal').val().trim() === '') {
            sError += 'El campo Fecha Fin es obligatorio.\n';
        }
        var fechaInicial = new Date($("#FechaInicial").val());
        var fechaFinal = new Date($("#FechaFinal").val());
        if (fechaInicial >= fechaFinal) {
            sError += 'La Fecha Inicial debe ser menor que la Fecha Final.';
        }

        // Mostrar mensaje de error si hay algún problema
        if (sError !== '') {
            $('#sError').text(sError);
            event.preventDefault(); // Evitar que se envíe el formulario
        } else {
            // Limpiar el mensaje de error si todo está bien
            $('#sError').text('');
        }

        if (sError == '') {
            var confirmacion = confirm("¿Estás seguro de grabar los cambios?");
            if (confirmacion) {
                var data = {
                    Usuario: $('#Usuario').val(),  // Tomar el valor del campo Usuario
                    IdGPS: $('#IdGPS').val(),      // Tomar el valor del campo IdGPS
                    FechaInicial: $('#FechaInicial').val(), // Tomar el valor del campo FechaInicial
                    FechaFinal: $('#FechaFinal').val(),
                    id: $('#ID').val()
                };

                $.ajax({
                    type: 'GET',
                    url: controler + $('#Accion').val(),
                    data: data,
                    success: function (data) {

                        if (data.sError == '') {
                            location.href = controler + 'index/' + $('#IdGPS').val()
                        }
                        else {
                            $('#sError').html(data.sError)
                        }
                    }
                });
            } else { $('#sError').html('');}

            event.preventDefault();
            return false;
        }
    });

});
