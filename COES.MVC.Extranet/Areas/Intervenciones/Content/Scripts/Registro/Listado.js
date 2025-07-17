var controler = siteRoot + "Intervenciones/Registro/";

viewEvent();

//Setea el datatable
$('#TablaConsultaIntervencion').dataTable({
    "destroy": "true",
    "sPaginationType": "full_numbers",
    searching: true,
    paging: true,
    info: false,
    "aoColumnDefs": [{
        'bSortable': false,
        'aTargets': [0, 12, 15, 16, 17]
    }],
    "iDisplayLength": 15,
    "scrollX": true
});

$(".ChkSeleccion").on("click", function () {
    if ($(this).is(':checked')) {
        document.getElementById("Aprobar").value += $(this).val() + ";";
    }
    else {
        document.getElementById("Aprobar").value += $(this).val() + ";";
    }

    if ($('.ChkSeleccionar').prop('checked')) {
        $(".ChkSeleccionar").prop('checked', false)
    }
    else {
        validarAllCheck();
    }
});

function validarAllCheck() {
    var paginaActual = $('#hfNroPagina').val();

    pagActualGlobal = paginaActual;

    var progrcodi = document.getElementById('Progrcodi').value;
    var tipoProgramacion = document.getElementById('idTipoProgramacion').value;
    var detalle = document.getElementById('Detalle').value;

    var aprobacion = document.getElementById('Aprobar').value;

    var tipoevencodi = JSON.stringify($('#cboTipoIntervencion').val());
    var areacodi = JSON.stringify($('#cboUbicacion').val());
    var famcodi = JSON.stringify($('#cboEquipo').val());

    var interindispo = JSON.stringify($('#InterDispo').val());
    var estadocodi = JSON.stringify($('#estadocodi').val());
    var interfechaini = $('#fechaI').val();
    var interfechafin = $('#fechaF').val();

    var estado;
    $.ajax({
        type: 'POST',
        url: controler + "ListadoRegistro",
        data: {
            progrcodi: progrcodi, tipoProgramacion: tipoProgramacion, tipoevencodi: tipoevencodi,
            areacodi: areacodi, famcodi: famcodi, interindispo: interindispo, estadocodi: estadocodi,
            interfechaini: interfechaini, interfechafin: interfechafin, nroPagina: paginaActual, detalle: detalle,
            aprobacion: aprobacion, tipo: 5
        },
        success: function (evt) {
            if (evt == "true") {
                $(".ChkSeleccionar").prop('checked', true);
            }
        }
    });
}