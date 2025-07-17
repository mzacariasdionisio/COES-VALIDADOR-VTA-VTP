var etapaERA = "EtapaERA/";
var controler = siteRoot + "ReportesFrecuencia/" + etapaERA;

//Funciones de busqueda
$(document).ready(function () {

    $('#tabla').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "aaSorting": [[1, "asc"], [2, "asc"]]
    });


    $('#frmEtapa').submit(function (event) {
        if ($('#frmEtapa').valid()) {
            $.ajax({
                type: 'GET',
                url: controler + 'Save',
                data: $('#frmEtapa').serialize(),
                success: function (data) {
                    if (data.sError == '') {
                        location.href = controler + 'index'
                    }
                    else {
                        $('#sError').html(data.sError)
                    }
                }
            })
        }
        event.preventDefault();
        return false;
    })


    


});

