var controlador = siteRoot + 'Eventos/AnalisisFallas/';

$(function () {
    //$("#btnOcultarMenu").click();

    $('#dtInicio').Zebra_DatePicker({
    });

    $('#dtFin').Zebra_DatePicker({
    });


    $("#btnConsultar").click(function () {
        Consultar();
    });

    $("#btnConfiguracionEmpresa").click(function () {
        redireccionarConfiguracionEmpresa();
    });

    ObtenerListado();
});

function redireccionarConfiguracionEmpresa() {
    location.href = controlador + "ListaConfiguracionEmpresa";
};

function ObtenerListado() {
    var cboEmpresaPropietaria = $("#cboEmpresaPropietaria").val();
    var cboEmpresaInvolucrada = $("#cboEmpresaInvolucrada").val();
    var cboTipoEquipo = $("#cboTipoEquipo").val();
    var cboEstado = $("#cboEstado").val();
    var dtInicio = $("#dtInicio").val();
    var dtFin = $("#dtFin").val();

    // checks
    var chkRNC = $("#chkRNC").is(':checked');
    var chkERACMF = $("#chkERACMF").is(':checked');
    var chkERACMT = $("#chkERACMT").is(':checked');
    var chkEDAGSF = $("#chkEDAGSF").is(':checked');
    var chkEveSinDatosReportados = $("#chkEveSinDatosReportados").is(':checked');

    /****************/
    //var cboImpugnacion = $("#cboImpugnacion").val();
    //var cboTipoReunion = $("#cboTipoReunion").val();
    /*************************** */

    var Estado = "N";
    //var FuerzaMayor = "N";
    var Anulado = "N";
    //var Impugnacion = "N";
    //var TipoReunion = "T";

    if (cboEstado != "TODOS") {
        if (cboEstado == "FUERZA MAYOR") {
            //FuerzaMayor = "S";
        } else if (cboEstado == "ANULADO") {
            //Anulado = "A";
        } else if (cboEstado == "ARCHIVADO") {
            //Anulado = "X";
        } else {
            Estado = cboEstado;
        }
    }



    if (chkRNC) {
        chkRNC = "S";
    } else {
        chkRNC = "N";
    }

    if (chkERACMF) {
        chkERACMF = "S";
    } else {
        chkERACMF = "N";
    }

    if (chkERACMT) {
        chkERACMT = "S";
    } else {
        chkERACMT = "N";
    }

    if (chkEDAGSF) {
        chkEDAGSF = "S";
    } else {
        chkEDAGSF = "N";
    }
    if (chkEveSinDatosReportados) {
        chkEveSinDatosReportados = "N";
    } else {
        chkEveSinDatosReportados = "T";
    }


    var miDataM = {
        EmpresaPropietaria: cboEmpresaPropietaria,
        EmpresaInvolucrada: cboEmpresaInvolucrada,
        TipoEquipo: cboTipoEquipo,
        Estado: Estado,
        RNC: chkRNC,
        ERACMF: chkERACMF,
        ERACMT: chkERACMT,
        EDAGSF: chkEDAGSF,
        DI: dtInicio,
        DF: dtFin,
        EveSinDatosReportados: chkEveSinDatosReportados

    };

    $("#listado").html('');

    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoInterrupciones',
        contentType: "application/json; charset=utf-8",

        data: JSON.stringify(miDataM),

        success: function (eData) {
            if (eData.Resultado == '-1' && eData.StrMensaje != "") {
                alert(eData.StrMensaje);
            }
            else {
                if (eData.Resultado != "") {
                    $('#listado').css("width", $('#mainLayout').width() - 10 + "px");
                    $("#listado").html(eData.Resultado);
                    $("#reporte").dataTable({
                        bJQueryUI: true,
                        "scrollY": 440,
                        "scrollX": false,
                        "sDom": 'ft',
                        "ordering": true,
                        "order": [[1, 'desc']],
                        "iDisplayLength": -1
                    });           

                }
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function Consultar() {
    ObtenerListado();
}