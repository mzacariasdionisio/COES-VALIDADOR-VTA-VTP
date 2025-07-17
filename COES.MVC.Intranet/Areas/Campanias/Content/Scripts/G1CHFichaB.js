$(function () {
    //$('#txtConcesiondefinitiva').val(obtenerFechaActualMesAnio());
    //$('#txtVentaenergia').val(obtenerFechaActualMesAnio());
    //$('#txtEjecucionobra').val(obtenerFechaActualMesAnio());
    //$('#txtContratosfinancieros').val(obtenerFechaActualMesAnio());

    $('#txtConcesiondefinitiva').Zebra_DatePicker({
        format: 'm/Y',
        onSelect: function () {
            cambiosRealizados = true;
        }
    });
    $('#txtVentaenergia').Zebra_DatePicker({
        format: 'm/Y',
        onSelect: function () {
            cambiosRealizados = true;
        }
    });
    $('#txtEjecucionobra').Zebra_DatePicker({
        format: 'm/Y',
        onSelect: function () {
            cambiosRealizados = true;
        }
    }); 
    $('#txtContratosfinancieros').Zebra_DatePicker({
        format: 'm/Y',
        onSelect: function () {
            cambiosRealizados = true;
        }
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
    var txtUsoagua = parseFloat(document.getElementById('txtUsoagua').value) || 0;
    var txtOtrosgastos = parseFloat(document.getElementById('txtOtrosgastos').value) || 0;

    // Calcular las sumas
    var inversionTotalSinIGV = txtEstudiofactibilidad + txtInvestigacionescampo + txtGestionesfinancieras + txtDisenospermisos +
        txtObrasciviles + txtEquipamiento + txtLineatransmisio + txtObrasregulacion +
        txtAdministracion + txtAduanas + txtSupervision + txtGastosgestion +
        txtImprevistos + txtUsoagua + txtOtrosgastos;

    var inversionTotalConIGV = txtEstudiofactibilidad + txtInvestigacionescampo + txtGestionesfinancieras + txtDisenospermisos +
        txtObrasciviles + txtEquipamiento + txtLineatransmisio + txtObrasregulacion +
        txtAdministracion + txtAduanas + txtSupervision + txtGastosgestion +
        txtImprevistos + txtIgv + txtUsoagua + txtOtrosgastos;

    // Actualizar los campos de resultado
    document.getElementById('txtInversiontotalsinigv').value = inversionTotalSinIGV.toFixed(4);
    document.getElementById('txtInversiontotalconigv').value = inversionTotalConIGV.toFixed(4);
}

// Asociar la funci�n calcularInversionTotal al evento input de los campos relevantes
var camposInversion = document.querySelectorAll('#txtEstudiofactibilidad, #txtInvestigacionescampo, #txtGestionesfinancieras, #txtDisenospermisos, ' +
    '#txtObrasciviles, #txtEquipamiento, #txtLineatransmisio, #txtObrasregulacion, ' +
    '#txtAdministracion, #txtAduanas, #txtSupervision, #txtGastosgestion, ' +
    '#txtImprevistos, #txtIgv, #txtUsoagua, #txtOtrosgastos');
camposInversion.forEach(function (input) {
    input.addEventListener('input', calcularInversionTotal);
});

// Llamar a la funci�n al cargar la p�gina para inicializar los valores si es necesario
calcularInversionTotal();

// Validar los campos para aceptar solo números positivos
document.querySelectorAll('.txtnumber').forEach(input => {
    input.addEventListener('input', function () {
        // Remover cualquier carácter que no sea un número o un punto decimal
        this.value = this.value.replace(/[^0-9.]/g, '');

        // Permitir solo un punto decimal
        const parts = this.value.split('.');
        if (parts.length > 2) {
            this.value = parts[0] + '.' + parts.slice(1).join('');
        }

        // Remover los ceros iniciales
        if (this.value.length > 1 && this.value[0] === '0' && this.value[1] !== '.') {
            this.value = this.value.substring(1);
        }

        // Si el valor es negativo, establecerlo en vacío
        if (parseFloat(this.value) < 0) {
            this.value = '';
        }
    });
});

document.querySelectorAll('.txtinteger').forEach(input => {
    input.addEventListener('input', function () {
        // Permitir solo dígitos numéricos
        this.value = this.value.replace(/[^0-9]/g, '');

        // Remover los ceros iniciales
        if (this.value.length > 1 && this.value[0] === '0') {
            this.value = this.value.substring(1);
        }

        // Si el valor es negativo, establecerlo en vacío (si se desea evitar negativos)
        if (this.value < 0) {
            this.value = '';
        }
    });
});


getHojaB = function () {
    var param = {};

    // Obtener los valores de los campos de inversi�n
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
    param.Usoagua = $("#txtUsoagua").val();
    param.Otrosgastos = $("#txtOtrosgastos").val();

    // Obtener los valores calculados de inversi�n total
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

function grabarFichaB() {
    const divInicio = document.getElementById('panel-container');
    divInicio.scrollTop = 0;
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.RegHojaBDTO = getHojaB();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarFichaB',
            data: param,
            success: function (result) {
                console.log(result);
                if (result) {
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


setHojaB = function (param) {
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
    $("#txtUsoagua").val(param.Usoagua);
    $("#txtOtrosgastos").val(param.Otrosgastos);

    // Asignar los valores calculados de inversi�n total
    $("#txtInversiontotalsinigv").val(param.Inversiontotalsinigv);
    $("#txtInversiontotalconigv").val(param.Inversiontotalconigv);

    // Asignar otros valores directos
    $("#txtFinanciamientotipo").val(param.Financiamientotipo);
    $("#txtFinanciamientoestado").val(param.Financiamientoestado);
    $("#txtPorcentajefinanciado").val(param.Porcentajefinanciado);
    $("#txtConcesiondefinitiva").val(param.Concesiondefinitiva);
    $("#txtVentaenergia").val(param.Ventaenergia);
    $("#txtEjecucionobra").val(param.Ejecucionobra);
    $("#txtContratosfinancieros").val(param.Contratosfinancieros);
    $("#txtObservaciones").val(param.Observaciones);
    if (modoModel == "consultar") {
        desactivarCamposFormulario('HFichaB');
        $("#btnGrabarFichaB").hide();
    }
    cambiosRealizados = false; // Reiniciar la variable

}

function cargarDatosB() {
    console.log("Editar");
    //popupGuardadoAutomatico();
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetChHojaB',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            
            success: function (response) {
                console.log(response);
                var hojaBData = response.responseResult;
                setHojaB(hojaBData);
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

