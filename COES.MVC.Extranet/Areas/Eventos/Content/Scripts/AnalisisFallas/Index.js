var controlador = siteRoot + 'eventos/AnalisisFallas/';

$(function () {
    $('#dtInicio').Zebra_DatePicker({
    });

    $('#dtFin').Zebra_DatePicker({
    });

    $("#btnConsultar").click(function () {
        Consultar();
    });

    ObtenerListado();
});

function verDetalleEvento(valor) {
    location.href = controlador + "Interrupcionsuministro?id=" + valor;
};

function ObtenerListado() {
    var cboEmpresaPropietaria = $("#cboEmpresaPropietaria").val();
    var dtInicio = $("#dtInicio").val();
    var dtFin = $("#dtFin").val();
    var descripcionEvento = $("#txtDescripcionEvento").val();

    var chkERACMF = $("#chkERACMF").is(':checked');
    var chkEveSinDatosReportados = $("#chkEveSinDatosReportados").is(':checked');

    if (chkERACMF) {
        chkERACMF = "S";
    } else {
        chkERACMF = "N";
    }

    if (chkEveSinDatosReportados) {
        chkEveSinDatosReportados = "N";
    } else {
        chkEveSinDatosReportados = "T";
    }

    var miDataM = {
        EmpresaPropietaria: cboEmpresaPropietaria,
        ERACMF: chkERACMF,
        DI: dtInicio,
        DF: dtFin,
        EveSinDatosReportados: chkEveSinDatosReportados,
        Descripcion: descripcionEvento
    };
    
    $("#listado").html('');

    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoInterrupciones',
        contentType: "application/json; charset=utf-8",

        data: JSON.stringify(miDataM),

        success: function (eData) {
            $('#listado').css("width", $('.form-main').width() + "px");


           
            $('#listado').html(eData);
            //$('#tabla').dataTable({
            //    bJQueryUI: true,
            //    "scrollY": 440,
            //    "scrollX": false,
            //    "sDom": 'ft',
            //    "ordering": false,
            //    "order": [[1, 'desc']],
            //    "iDisplayLength": -1
            //});
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function Consultar() {
    ObtenerListado();
}