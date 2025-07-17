
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
    $('#btnGrabarBioFichaB').on('click', function () {
        grabarBioFichaB();
    });
    var formularioalB = document.getElementById('BFichaB');
    formularioalB.addEventListener('change', function () {
        cambiosRealizados = true;
    });
});



function calcularInversionTotal() {
    // Obtener los valores de los campos
    var txtEstudiofactibilidad = parseFloat(document.getElementById('txtEstudiofactibilidad').value) || 0;
    var txtInvestigacionescampo = parseFloat(document.getElementById('txtInvestigacionescampo').value) || 0;
    var txtGestionesfinancieras = parseFloat(document.getElementById('txtGestionesfinancieras').value) || 0;
    var txtDisenospermisos = parseFloat(document.getElementById('txtDisenospermisos').value) || 0;

    var txtObrasciviles = parseFloat(document.getElementById('txtObrasciviles').value) || 0;
    var txtEquipamiento = parseFloat(document.getElementById('txtEquipamiento').value) || 0;
    var txtLineatransmisio = parseFloat(document.getElementById('txtLineatransmisio').value) || 0;
    var txtObrasregulacion = parseFloat(document.getElementById('txtObrasregulacion').value) || 0;

    var txtAdministracion = parseFloat(document.getElementById('txtAdministracion').value) || 0;
    var txtAduanas = parseFloat(document.getElementById('txtAduanas').value) || 0;
    var txtSupervision = parseFloat(document.getElementById('txtSupervision').value) || 0;
    var txtGastosgestion = parseFloat(document.getElementById('txtGastosgestion').value) || 0;

    var txtImprevistos = parseFloat(document.getElementById('txtImprevistos').value) || 0;
    var txtIgv = parseFloat(document.getElementById('txtIgv').value) || 0;
    var txtOtrosgastos = parseFloat(document.getElementById('txtOtrosgastos').value) || 0;

    // Calcular las sumas
    var inversionTotalSinIGV = txtEstudiofactibilidad + txtInvestigacionescampo + txtGestionesfinancieras + txtDisenospermisos +
        txtObrasciviles + txtEquipamiento + txtLineatransmisio + txtObrasregulacion +
        txtAdministracion + txtAduanas + txtSupervision + txtGastosgestion +
        txtImprevistos + txtOtrosgastos;

    var inversionTotalConIGV = txtEstudiofactibilidad + txtInvestigacionescampo + txtGestionesfinancieras + txtDisenospermisos +
        txtObrasciviles + txtEquipamiento + txtLineatransmisio + txtObrasregulacion +
        txtAdministracion + txtAduanas + txtSupervision + txtGastosgestion +
        txtImprevistos + txtIgv + txtOtrosgastos;

    // Actualizar los campos de resultado
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


getBioHojaB = function () {
    var param = {};

    param.Estudiofactibilidad = $("#txtEstudiofactibilidad").val();
    param.Investigacionescampo = $("#txtInvestigacionescampo").val();
    param.Gestionesfinancieras = $("#txtGestionesfinancieras").val();
    param.Disenospermisos = $("#txtDisenospermisos").val();

    param.Obrasciviles = $("#txtObrasciviles").val();
    param.Equipamiento = $("#txtEquipamiento").val();
    param.Lineatransmision = $("#txtLineatransmisio").val();
    param.Obrasregulacion = $("#txtObrasregulacion").val();

    param.Administracion = $("#txtAdministracion").val();
    param.Aduanas = $("#txtAduanas").val();
    param.Supervision = $("#txtSupervision").val();
    param.Gastosgestion = $("#txtGastosgestion").val();

    param.Imprevistos = $("#txtImprevistos").val();
    param.Igv = $("#txtIgv").val();
    param.Otrosgastos = $("#txtOtrosgastos").val();

    param.Inversiontotalsinigv = $("#txtInversiontotalsinigv").val();
    param.Inversiontotalconigv = $("#txtInversiontotalconigv").val();

    param.Financiamientotipo = $("#txtFinanciamientotipo").val();
    param.Financiamientoestado = $("#txtFinanciamientoestado").val();
    param.Porcentajefinanciado = $("#txtPorcentajefinanciado").val();
    param.Concesiondefinitiva = $("#txtConcesiondefinitiva").val();
    param.Ventaenergia = $("#txtVentaenergia").val();
    param.Ejecucionobra = $("#txtEjecucionobra").val();
    param.Contratosfinancieros = $("#txtContratosfinancieros").val();
    param.Observaciones = $("#txtObservaciones").val();

    console.log($("#txtInversiontotalsinigv").val());
    console.log(param.Inversiontotalsinigv);

    console.log(param);

    return param;
}


function grabarBioFichaB() {
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.BioHojaBDTO = getBioHojaB();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarBioHojaB',
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
                stopLoading();
                mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
            }
        });
    }
    cambiosRealizados = false;
}

setBioHojaB = function (param) {
    $("#txtEstudiofactibilidad").val(param.Estudiofactibilidad);
    $("#txtInvestigacionescampo").val(param.Investigacionescampo);
    $("#txtGestionesfinancieras").val(param.Gestionesfinancieras);
    $("#txtDisenospermisos").val(param.Disenospermisos);

    $("#txtObrasciviles").val(param.Obrasciviles);
    $("#txtEquipamiento").val(param.Equipamiento);
    $("#txtLineatransmisio").val(param.Lineatransmision);
    $("#txtObrasregulacion").val(param.Obrasregulacion);

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
        desactivarCamposFormulario('BFichaB');
        $("#btnGrabarBioFichaB").hide();
    }
}

function cargarDatosB() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetBioHojaB',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            
            success: function (response) {
                console.log(response);
                var hojaBData = response.responseResult;
                setBioHojaB(hojaBData);
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
    cambiosRealizados = false;
}
