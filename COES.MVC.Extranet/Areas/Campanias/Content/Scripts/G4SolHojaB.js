
$(function () {
    //$('#txtConcesiondefinitiva').val(obtenerFechaActualMesAnio());
    //$('#txtVentaenergia').val(obtenerFechaActualMesAnio());
    //$('#txtEjecucionobra').val(obtenerFechaActualMesAnio());
    //$('#txtContratosfinancieros').val(obtenerFechaActualMesAnio());

    $('#txtConcesiondefinitiva').Zebra_DatePicker({
        format: 'm/Y',
        onSelect: function () {
            cambiosRealizados = true; // Marcar que hubo un cambio
        }
    });
    $('#txtVentaenergia').Zebra_DatePicker({
        format: 'm/Y',
        onSelect: function () {
            cambiosRealizados = true; // Marcar que hubo un cambio
        }
    });
    $('#txtEjecucionobra').Zebra_DatePicker({
        format: 'm/Y',
        onSelect: function () {
            cambiosRealizados = true; // Marcar que hubo un cambio
        }
    });
    $('#txtContratosfinancieros').Zebra_DatePicker({
        format: 'm/Y',
        onSelect: function () {
            cambiosRealizados = true; // Marcar que hubo un cambio
        }
    });
    $('#btnGrabarSolFichaB').on('click', function () {
        grabarFichaBG4();
    });
    var formularioc = document.getElementById('FichaB');
    formularioc.addEventListener('change', function () {
        cambiosRealizados = true;
        console.log(cambiosRealizados);
    });

});



function calcularInversionTotal() {

    var txtEstudiofactibilidad = parseFloat(document.getElementById('txtEstudiofactibilidad').value) || 0;
    var txtInvestigacionescampo = parseFloat(document.getElementById('txtInvestigacionescampo').value) || 0;
    var txtGestionesfinancieras = parseFloat(document.getElementById('txtGestionesfinancieras').value) || 0;
    var txtDisenospermisos = parseFloat(document.getElementById('txtDisenospermisos').value) || 0;

    var txtObrasciviles = parseFloat(document.getElementById('txtObrasciviles').value) || 0;
    var txtEquipamiento = parseFloat(document.getElementById('txtEquipamiento').value) || 0;
    var txtLineatransmisio = parseFloat(document.getElementById('txtLineatransmisio').value) || 0;

    var txtAdministracion = parseFloat(document.getElementById('txtAdministracion').value) || 0;
    var txtAduanas = parseFloat(document.getElementById('txtAduanas').value) || 0;
    var txtSupervision = parseFloat(document.getElementById('txtSupervision').value) || 0;
    var txtGastosgestion = parseFloat(document.getElementById('txtGastosgestion').value) || 0;

    var txtImprevistos = parseFloat(document.getElementById('txtImprevistos').value) || 0;
    var txtIgv = parseFloat(document.getElementById('txtIgv').value) || 0;
    var txtOtrosgastos = parseFloat(document.getElementById('txtOtrosgastos').value) || 0;


    var inversionTotalSinIGV = txtEstudiofactibilidad + txtInvestigacionescampo + txtGestionesfinancieras + txtDisenospermisos +
        txtObrasciviles + txtEquipamiento + txtLineatransmisio  +
        txtAdministracion + txtAduanas + txtSupervision + txtGastosgestion +
        txtImprevistos + txtOtrosgastos;

    var inversionTotalConIGV = txtEstudiofactibilidad + txtInvestigacionescampo + txtGestionesfinancieras + txtDisenospermisos +
        txtObrasciviles + txtEquipamiento + txtLineatransmisio  +
        txtAdministracion + txtAduanas + txtSupervision + txtGastosgestion +
        txtImprevistos + txtIgv + txtOtrosgastos;


    document.getElementById('txtInversiontotalsinigv').value = inversionTotalSinIGV.toFixed(4);
    document.getElementById('txtInversiontotalconigv').value = inversionTotalConIGV.toFixed(4);
}


var camposInversion = document.querySelectorAll('#txtEstudiofactibilidad, #txtInvestigacionescampo, #txtGestionesfinancieras, #txtDisenospermisos, ' +
    '#txtObrasciviles, #txtEquipamiento, #txtLineatransmisio, ' +
    '#txtAdministracion, #txtAduanas, #txtSupervision, #txtGastosgestion, ' +
    '#txtImprevistos, #txtIgv, #txtOtrosgastos');
camposInversion.forEach(function (input) {
    input.addEventListener('input', calcularInversionTotal);
});


calcularInversionTotal();


document.querySelectorAll('.txtnumber').forEach(input => {
    input.addEventListener('input', function () {
        this.value = this.value.replace(/[^0-9.]/g, '');

 
        if (this.value.length > 1 && this.value[0] === '0' && this.value[1] !== '.') {
            this.value = this.value.substring(1);
        }

        if (this.value < 0) {
            this.value = '';
        }
    });
});


getSolHojaB = function () {
    var param = {};


    // Si el campo est� vac�o, devolver null en lugar de 0
    param.Estudiofactibilidad = $("#txtEstudiofactibilidad").val() === "" ? null : parseFloat($("#txtEstudiofactibilidad").val());
    param.Investigacionescampo = $("#txtInvestigacionescampo").val() === "" ? null : parseFloat($("#txtInvestigacionescampo").val());
    param.Gestionesfinancieras = $("#txtGestionesfinancieras").val() === "" ? null : parseFloat($("#txtGestionesfinancieras").val());
    param.Disenospermisos = $("#txtDisenospermisos").val() === "" ? null : parseFloat($("#txtDisenospermisos").val());

    param.Obrasciviles = $("#txtObrasciviles").val() === "" ? null : parseFloat($("#txtObrasciviles").val());
    param.Equipamiento = $("#txtEquipamiento").val() === "" ? null : parseFloat($("#txtEquipamiento").val());
    param.Lineatransmision = $("#txtLineatransmisio").val() === "" ? null : parseFloat($("#txtLineatransmisio").val());

    param.Administracion = $("#txtAdministracion").val() === "" ? null : parseFloat($("#txtAdministracion").val());
    param.Aduanas = $("#txtAduanas").val() === "" ? null : parseFloat($("#txtAduanas").val());
    param.Supervision = $("#txtSupervision").val() === "" ? null : parseFloat($("#txtSupervision").val());
    param.Gastosgestion = $("#txtGastosgestion").val() === "" ? null : parseFloat($("#txtGastosgestion").val());

    param.Imprevistos = $("#txtImprevistos").val() === "" ? null : parseFloat($("#txtImprevistos").val());
    param.Igv = $("#txtIgv").val() === "" ? null : parseFloat($("#txtIgv").val());
    param.Otrosgastos = $("#txtOtrosgastos").val() === "" ? null : parseFloat($("#txtOtrosgastos").val());

    param.Inversiontotalsinigv = $("#txtInversiontotalsinigv").val() === "" ? null : parseFloat($("#txtInversiontotalsinigv").val());
    param.Inversiontotalconigv = $("#txtInversiontotalconigv").val() === "" ? null : parseFloat($("#txtInversiontotalconigv").val());

    param.Financiamientotipo = $("#txtFinanciamientotipo").val() || null;
    param.Financiamientoestado = $("#txtFinanciamientoestado").val() || null;
    param.Porcentajefinanciado = $("#txtPorcentajefinanciado").val() || null;
    param.Concesiondefinitiva = $("#txtConcesiondefinitiva").val() || null;
    param.Ventaenergia = $("#txtVentaenergia").val() || null;
    param.Ejecucionobra = $("#txtEjecucionobra").val() || null;
    param.Contratosfinancieros = $("#txtContratosfinancieros").val() || null;
    param.Observaciones = $("#txtObservaciones").val() || null;

    return param;
};

function grabarFichaBG4() {
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.SolHojaBDTO = getSolHojaB();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarSolHojaB',
            data: param,
            
            success: function (result) {
                console.log(result);
                if (result==1) {
                    mostrarMensaje('mensajeFicha', 'exito', 'Los datos se grabaron correctamente.');
                }
                else {

                    mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
            }
        });
    }
    cambiosRealizados = false;
}

function cargarDatosB() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetSolHojaB',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            
            success: function (response) {
                console.log(response);
                var hojaBData = response.responseResult;
                setSolHojaB(hojaBData);
                cambiosRealizados = false;
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

setSolHojaB = function (param) {
    $("#txtEstudiofactibilidad").val(param.Estudiofactibilidad);
    $("#txtInvestigacionescampo").val(param.Investigacionescampo);
    $("#txtGestionesfinancieras").val(param.Gestionesfinancieras);
    $("#txtDisenospermisos").val(param.Disenospermisos);
    $("#txtObrasciviles").val(param.Obrasciviles);
    $("#txtEquipamiento").val(param.Equipamiento);
    $("#txtLineatransmisio").val(param.Lineatransmision);
    $("#txtAdministracion").val(param.Administracion);
    $("#txtAduanas").val(param.Aduanas);
    $("#txtSupervision").val(param.Supervision);
    $("#txtGastosgestion").val(param.Gastosgestion);
    $("#txtImprevistos").val(param.Imprevistos);
    $("#txtIgv").val(param.Igv);
    $("#txtOtrosgastos").val(param.Otrosgastos);
    $("#txtInversiontotalsinigv").val(param.Inversiontotalsinigv);
    $("#txtInversiontotalconigv").val(param.Inversiontotalconigv);
    $("#txtFinanciamientotipo").val(param.Financiamientotipo);
    $("#txtFinanciamientoestado").val(param.Financiamientoestado);
    $("#txtPorcentajefinanciado").val(param.Porcentajefinanciado);
    $("#txtConcesiondefinitiva").val(param.Concesiondefinitiva);
    $("#txtVentaenergia").val(param.Ventaenergia);
    $("#txtEjecucionobra").val(param.Ejecucionobra);
    $("#txtContratosfinancieros").val(param.Contratosfinancieros);
    $("#txtObservaciones").val(param.Observaciones);
    if (modoModel == "consultar") {
        desactivarCamposFormulario('FichaA');
        $("#btnGrabarSolFichaB").hide();
    }

}