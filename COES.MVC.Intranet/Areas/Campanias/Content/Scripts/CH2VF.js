var controlador = siteRoot + 'campanias/plantransmision/';
var controladorFichas = siteRoot + 'campanias/fichasproyecto/';

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


    var txtAdministracion = parseFloat(document.getElementById('txtAdministracion').value) || 0;
    var txtAduanas = parseFloat(document.getElementById('txtAduanas').value) || 0;
    var txtSupervision = parseFloat(document.getElementById('txtSupervision').value) || 0;
    var txtGastosgestion = parseFloat(document.getElementById('txtGastosgestion').value) || 0;

    var txtImprevistos = parseFloat(document.getElementById('txtImprevistos').value) || 0;
    var txtIgv = parseFloat(document.getElementById('txtIgv').value) || 0;

    var txtOtrosgastos = parseFloat(document.getElementById('txtOtrosgastos').value) || 0;

    // Calcular las sumas
    var inversionTotalSinIGV = txtEstudiofactibilidad + txtInvestigacionescampo + txtGestionesfinancieras + txtDisenospermisos +
        txtObrasciviles + txtEquipamiento + txtLineatransmisio +
        txtAdministracion + txtAduanas + txtSupervision + txtGastosgestion +
        txtImprevistos + txtOtrosgastos;

    var inversionTotalConIGV = txtEstudiofactibilidad + txtInvestigacionescampo + txtGestionesfinancieras + txtDisenospermisos +
        txtObrasciviles + txtEquipamiento + txtLineatransmisio +
        txtAdministracion + txtAduanas + txtSupervision + txtGastosgestion +
        txtImprevistos + txtIgv + txtOtrosgastos;

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

// Validar los campos para aceptar solo n�meros positivos
document.querySelectorAll('.txtnumber').forEach(input => {
    input.addEventListener('input', function () {
        // Remover cualquier caracter que no sea un n�mero o un punto decimal
        this.value = this.value.replace(/[^0-9.]/g, '');

        // Remover los ceros iniciales
        if (this.value.length > 1 && this.value[0] === '0' && this.value[1] !== '.') {
            this.value = this.value.substring(1);
        }

        // Si el valor es negativo, establecerlo en vac�o
        if (this.value < 0) {
            this.value = '';
        }
    });
});


getDataCH2VF = function () {

    var param = {};

    param.EstudioFactibilidad = $("#txtEstudiofactibilidad").val();
    param.InvestigacionesCampo = $("#txtInvestigacionescampo").val();
    param.GestionesFinancieras = $("#txtGestionesfinancieras").val();
    param.DisenosPermisos = $("#txtDisenospermisos").val();
    param.ObrasCiviles = $("#txtObrasciviles").val();
    param.Equipamiento = $("#txtEquipamiento").val();
    param.LineaTransmision = $("#txtLineatransmisio").val();
    param.Administracion = $("#txtAdministracion").val();
    param.Aduanas = $("#txtAduanas").val();
    param.Supervision = $("#txtSupervision").val();
    param.GastosGestion = $("#txtGastosgestion").val();
    param.Imprevistos = $("#txtImprevistos").val();
    param.Igv = $("#txtIgv").val();
    param.OtrosGastos = $("#txtOtrosgastos").val();
    param.InversionTotalSinIgv = $("#txtInversiontotalsinigv").val();
    param.InversionTotalConIgv = $("#txtInversiontotalconigv").val();
    param.FinanciamientoTipo = $("#txtFinanciamientotipo").val();
    param.FinanciamientoEstado = $("#txtFinanciamientoestado").val();
    param.PorcentajeFinanciado = $("#txtPorcentajefinanciado").val();
    param.ConcesionDefinitiva = $("#txtConcesiondefinitiva").val();
    param.VentaEnergia = $("#txtVentaenergia").val();
    param.EjecucionObra = $("#txtEjecucionobra").val();
    param.ContratosFinancieros = $("#txtContratosfinancieros").val();
    param.Observaciones = $("#txtObservaciones").val();
    console.log(param);

    return param;


}

function btnGrabarCH2VF() {
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.Ch2vfDTO = getDataCH2VF();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarCH2VF',
            data: param,

            success: function (result) {
                console.log(result);
                if (result == 1) {
                    mostrarMensaje('mensajeFicha', 'exito', 'Los datos se grabaron correctamente.');
                    cambiosRealizados = false;
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
}

function cargarCH2VF() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetCH2VF',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),

            success: function (response) {
                console.log(response);
                var hojaBData = response.responseResult;
                setCH2VF(hojaBData);
                cambiosRealizados = false;
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

setCH2VF = function (param) {
    $("#txtEstudiofactibilidad").val(param.EstudioFactibilidad);
    $("#txtInvestigacionescampo").val(param.InvestigacionesCampo);
    $("#txtGestionesfinancieras").val(param.GestionesFinancieras);
    $("#txtDisenospermisos").val(param.DisenosPermisos);
    $("#txtObrasciviles").val(param.ObrasCiviles);
    $("#txtEquipamiento").val(param.Equipamiento);
    $("#txtLineatransmisio").val(param.LineaTransmision);
    $("#txtAdministracion").val(param.Administracion);
    $("#txtAduanas").val(param.Aduanas);
    $("#txtSupervision").val(param.Supervision);
    $("#txtGastosgestion").val(param.GastosGestion);
    $("#txtImprevistos").val(param.Imprevistos);
    $("#txtIgv").val(param.Igv);
    $("#txtOtrosgastos").val(param.OtrosGastos);
    $("#txtInversiontotalsinigv").val(param.InversionTotalSinIgv);
    $("#txtInversiontotalconigv").val(param.InversionTotalConIgv);
    $("#txtFinanciamientotipo").val(param.FinanciamientoTipo);
    $("#txtFinanciamientoestado").val(param.FinanciamientoEstado);
    $("#txtPorcentajefinanciado").val(param.PorcentajeFinanciado);
    $("#txtConcesiondefinitiva").val(param.ConcesionDefinitiva);
    $("#txtVentaenergia").val(param.VentaEnergia);
    $("#txtEjecucionobra").val(param.EjecucionObra);
    $("#txtContratosfinancieros").val(param.ContratosFinancieros);
    $("#txtObservaciones").val(param.Observaciones);
    if (modoModel == "consultar") {
        desactivarCamposFormulario('H2VF');
        $('#btnGrabarH2VF').hide();
    }
}


function exportarTablaExcelHojaC() {


    // Definir las cabeceras manualmente
    aniosCabecera.push({ label: "", colspan: 1 });
    aniosCabecera.push({ label: "", colspan: 1 });
    aniosCabecera.push({ label: "", colspan: 1 });

    var inicioan = horizonteInicio; // Ejemplo: puedes ajustar esto seg�n tus necesidades
    var finan = horizonteFin; // Ejemplo: puedes ajustar esto seg�n tus necesidades
    var anioTotal = finan - inicioan + 1;

    for (var i = 0; i < anioTotal; i++) {
        aniosCabecera.push({
            label: inicioan + i + "</br> TRIMESTRE",
            colspan: 4,
        });
    }

    headerCabe.push({ label: "", colspan: 1 });
    headerCabe.push({ label: "A&ntilde;o", colspan: 1 });
    headerCabe.push({ label: "A&ntilde;o " + (inicioan - 1) + "</br>o antes", colspan: 1 });
    for (var i = 0; i < anioTotal; i++) {
        for (var j = 0; j < 4; j++) {
            headerCabe.push(j + 1);
        }
    }

    // Obtener los datos de la tabla Handsontable
    var data = hotTableHojaC.getData();

    // Combinar cabeceras con los datos de la tabla
    var dataWithHeaders = [
        aniosCabecera.map(header => header.label),
        headerCabe.map(header => typeof header === 'object' ? header.label : header)
    ].concat(data);

    // Crear una hoja de c�lculo con los datos
    var ws = XLSX.utils.aoa_to_sheet(dataWithHeaders);
    aplicarBordes(ws);
    // Definir las celdas combinadas en la hoja de c�lculo
    var merges = [];
    var colIndex = 0;
    for (var i = 0; i < aniosCabecera.length; i++) {
        if (aniosCabecera[i].colspan > 1) {
            merges.push({
                s: { r: 0, c: colIndex },
                e: { r: 0, c: colIndex + aniosCabecera[i].colspan - 1 }
            });
            colIndex += aniosCabecera[i].colspan;
        } else {
            colIndex++;
        }
    }
    ws["!merges"] = merges;

    // Crear un libro de Excel
    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "HojaF");

    // Guardar el archivo Excel
    XLSX.writeFile(wb, "HojaF_Datos.xlsx");
}

function importarExcelHojaC() {
    var fileInput = document.createElement("input");
    fileInput.type = "file";
    fileInput.accept = ".xlsx, .xls";
    fileInput.onchange = function (e) {
        var file = e.target.files[0];
        var reader = new FileReader();

        reader.onload = function (e) {
            var data = new Uint8Array(e.target.result);
            var workbook = XLSX.read(data, { type: "array" });

            // Leer la primera hoja del archivo Excel
            var firstSheet = workbook.Sheets[workbook.SheetNames[0]];

            // Convertir la hoja de Excel a un formato JSON
            var jsonData = XLSX.utils.sheet_to_json(firstSheet, { header: 1 });

            // Actualizar la tabla Handsontable con los datos importados
            updateTableFromExcelHojaC(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}

function updateTableFromExcelHojaC(jsonData) {
    var headers = jsonData.slice(0, 2); // Las dos primeras filas son las cabeceras
    var data = jsonData.slice(2); // El resto son los datos

    // Cargar los datos en la tabla Handsontable
    hotTableHojaC.loadData(data);
}
