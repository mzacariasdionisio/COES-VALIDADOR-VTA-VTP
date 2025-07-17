var periodo = "periodoDeclaracion/";
var controler = siteRoot + "transferencias/" + periodo;

//Funciones de busqueda
$(document).ready(function () {

    $('#tabla').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "aaSorting": [[1, "desc"], [2, "desc"]]
    });

    if ($('#Entidad_PeridcCodi').val() == '0') {

        var año = `${$('#Entidad_PeridcAnio option:selected').html() + '.' + $('#Entidad_PeridcMes option:selected').html()}`;
        $('#Entidad_PeridcNombre').val(año)
    }

    $('#frmPeriodoDeclaracion').submit(function (event) {
        if ($('#frmPeriodoDeclaracion').valid()) {
            $.ajax({
                type: 'GET',
                url: controler + 'Save',
                data: $('#frmPeriodoDeclaracion').serialize(),
                success: function (data) {
                    if (data.sError == '') {
                        location.href = controler + 'index'
                    }
                    else {
                        $('#sError').html(data.sError )
                    }
                }
            })
        }
        event.preventDefault();
        return false;
    })


    $('#Entidad_PeridcAnio').change(() => {
        if ($('#Entidad_PeridcCodi').val() == '0') {

            var año = `${$('#Entidad_PeridcAnio option:selected').html() + '.' + $('#Entidad_PeridcMes option:selected').html()}`;
            $('#Entidad_PeridcNombre').val(año)
        }

    })

    $('#Entidad_PeridcMes').change(() => {
        if ($('#Entidad_PeridcCodi').val() == '0') {

            var año = `${$('#Entidad_PeridcAnio option:selected').html() + '.' + $('#Entidad_PeridcMes option:selected').html()}`;
            $('#Entidad_PeridcNombre').val(año)
        }
    })
});

