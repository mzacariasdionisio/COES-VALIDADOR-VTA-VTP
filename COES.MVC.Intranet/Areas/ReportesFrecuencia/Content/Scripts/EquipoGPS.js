var equipoGPS = "EquipoGPS/";
var controler = siteRoot + "ReportesFrecuencia/" + equipoGPS;

//Funciones de busqueda
$(document).ready(function () {

    $("#btnManualUsuario").click(function () {
        window.location = controler + 'DescargarManualUsuario';
    });

    $('#tabla').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "aaSorting": [[1, "asc"], [2, "asc"]]
    });

    /*if ($('#Entidad_PeridcCodi').val() == '0') {

        var año = `${$('#Entidad_PeridcAnio option:selected').html() + '.' + $('#Entidad_PeridcMes option:selected').html()}`;
        $('#Entidad_PeridcNombre').val(año)
    }*/

    $('#frmEquipoGPS').submit(function (event) {
        if ($('#frmEquipoGPS').valid()) {
            $.ajax({
                type: 'GET',
                url: controler + 'Save',
                data: $('#frmEquipoGPS').serialize(),
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

    /*$('#Entidad_PeridcAnio').change(() => {
        if ($('#Entidad_PeridcCodi').val() == '0') {

            var año = `${$('#Entidad_PeridcAnio option:selected').html() + '.' + $('#Entidad_PeridcMes option:selected').html()}`;
            $('#Entidad_PeridcNombre').val(año)
        }

    })*/

    /*$('#Entidad_PeridcMes').change(() => {
        if ($('#Entidad_PeridcCodi').val() == '0') {

            var año = `${$('#Entidad_PeridcAnio option:selected').html() + '.' + $('#Entidad_PeridcMes option:selected').html()}`;
            $('#Entidad_PeridcNombre').val(año)
        }
    })*/
});

