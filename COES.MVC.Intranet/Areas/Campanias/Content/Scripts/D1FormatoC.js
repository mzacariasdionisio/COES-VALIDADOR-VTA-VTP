var controlador = siteRoot + 'campanias/plantransmision/';
var controladorFormatos = siteRoot + 'campanias/fichasproyecto/';
var hotDFormatoC = null;
var valoresListaC = [];
var aniosCabecera = [];
var headerCabe = [];

$(function () {
    crearTablaDiagCargHorario(30, 1440, 30);
    $('#btnGrabarEolicaHojaAGrafico').on('click', function () {
        $('#dataFichaAEo').hide();
        $('#curvaTurbinaCon').show();
    });

});

getDataFormatoC = function () {
    var param = {};
    param.PlanReducir = $('input[name="txtPlanReducir"]:checked').val();
    param.Alternativa = $('input[name="txtAlternativa"]:checked').val();
    param.Otro = $("#txtOtro").val();
    return param;
}

function grabarDFormatoC() {
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.FormatoD1CDTO = getDataFormatoC();
        param.ListaFormatoDCDet1 = getDataDFormatoCDet();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarDFormatoC',
            data: param,
            
            success: function (result) {
                console.log(result);
                if (result) {
                    mostrarMensaje('mensajeFormato', 'exito', 'Los datos se grabaron correctamente.');

                }
                else {

                    mostrarMensaje('mensajeFormato', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeFormato', 'error', 'Se ha producido un error.');
            }
        });
    }
    cambiosRealizados = false;
}

setDataFormatoC = function (dataResponse) {
    $("input[name='txtPlanReducir'][value='" + dataResponse.PlanReducir + "']").prop("checked", true);
    $("input[name='txtAlternativa'][value='" + dataResponse.Alternativa + "']").prop("checked", true);
    $("#txtOtro").val(dataResponse.Otro);
    if (modoModel == "consultar") {
        //desactivarCamposC();
        $("#btnGrabarFormatoC").hide();
        hotDFormatoC.updateSettings({
            cells: function (row, col) {
                var cellProperties = {};
                cellProperties.readOnly = true;
                return cellProperties;
            }
        });
    }

    if (modoModel == "consultar") {
        desactivarCamposFormulario('DGPFormatoD1C');
        $('#D1GrabarC').hide();
    }
}

function getDataDFormatoCDet() {
    var datosArray = hotDFormatoC.getData();
    var datosObjetos = datosArray.map(function (row) {
        return {
            Hora: row[0],
            Demanda: row[1],
            Generacion: row[2],
        };
    });
    console.log(datosObjetos);
    // Filtrar objetos que tienen al menos uno de los campos con datos v�lidos
    var datosFiltrados = datosObjetos.filter(function (obj) {
        return (
            obj.Demanda !== null && obj.Demanda !== "" ||
            obj.Generacion !== null && obj.Generacion !== ""
        );
    });
    return datosFiltrados;
}

function crearTablaDiagCargHorario(iniciot, fint, increm) {

    var data = [];
    for (var time = iniciot; time <= fint; time += increm) {
        var hora = Math.floor((time / 60) === 24 ? 0 : (time / 60)).toString().padStart(2, '0');
        var min = (time % 60).toString().padStart(2, '0');;
        var row = [`${hora}:${min}`];
        for (var i = 0; i < 2; i++) {
            row.push('');
        }
        data.push(row);
    }

    var grilla = document.getElementById('tableCargaDiarioDG');

    if (typeof hotDFormatoC !== 'undefined' && hotDFormatoC !== null) {
        hotDFormatoC.destroy();
    }

    hotDFormatoC = new Handsontable(grilla, {
        data: data,
        rowHeaders: true,
        colHeaders: true,
        colWidths: 170,
        manualColumnResize: true,
        columnSorting: false,
        filters: false,
        contextMenu: true,
        autoColumnSize: true,
        nestedHeaders: [
            [
                { label: 'HORA', colspan: 1 },
                { label: 'Demanda (P.U)', colspan: 1 },
                { label: 'Generaci&oacute;n (P.U)', colspan: 1 }
            ]
        ],
        columns: [
            { data: 0, readOnly: true },
            {
                data: 1,
                type: 'numeric', format: '0.0'
            },
            {
                data: 2,
                type: 'numeric', format: '0.0'
            }
        ],
        beforeChange: function (changes, source) {
            console.log("beforeChange event triggered");
            generalBeforeChange(this, changes, source);
        },
        afterChange: function (changes, source) {
            console.log("afterChange event triggered");
            generalAfterChange(this, changes, source);
        }
    });

    hotDFormatoC.render();
}


function cargarDFormatoC() {
    limpiarMensaje('mensajeFicha');
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetDFormatoC',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            
            success: function (response) {
                console.log(response);
                var hojaCRes = response.responseResult;
                setDataFormatoC(hojaCRes);
                setDataDetFormatoC(hojaCRes.ListaFormatoDe1CDet);
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
    cambiosRealizados = false;
}

function setDataDetFormatoC(datos) {
    // Obtener la data actual de la tabla Handsontable
    var currentData = hotDFormatoC.getData();

    // Iterar sobre los nuevos datos para encontrar coincidencias y actualizar la tabla
    datos.forEach(function (nuevoDato) {
        // Buscar el índice de la fila en la tabla actual que coincide con la 'Hora' del nuevo dato
        var rowIndex = currentData.findIndex(function (fila) {
            return fila[0] === nuevoDato.Hora; // Comparar por la columna 'Hora'
        });

        // Si se encuentra una coincidencia, actualizamos las columnas correspondientes
        if (rowIndex !== -1) {
            currentData[rowIndex][1] = nuevoDato.Demanda !== null && nuevoDato.Demanda !== undefined
                ? nuevoDato.Demanda
                : ''; // Actualizar Consumo Energético
            currentData[rowIndex][2] = nuevoDato.Generacion !== null && nuevoDato.Generacion !== undefined
                ? nuevoDato.Generacion
                : ''; // Actualizar Producción Central
        }
    });

    // Recargar los datos actualizados en Handsontable
    hotDFormatoC.loadData(currentData);
    hotDFormatoC.render(); // Renderizar la tabla para reflejar los cambios
}

function exportarTablaExcelDFormatoC() {
    // Definir los encabezados de la tabla
    var headers = [
        ["HORA", "Demanda total de unidad productiva asociada al proyecto de GD", "Generación de proyecto de GD"]
    ];

    // Obtener los datos de la tabla Handsontable (hotDFormatoC)
    var datosArray = hotDFormatoC.getData();
    // Concatenar los encabezados con los datos
    var data = headers.concat(datosArray);

    // Crear una hoja de trabajo (worksheet) a partir de los datos
    var ws = XLSX.utils.aoa_to_sheet(data);
    aplicarBordes(ws);
    // Crear un nuevo libro de trabajo (workbook) y agregar la hoja
    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Datos");

    // Exportar el libro de trabajo a un archivo Excel
    XLSX.writeFile(wb, "D1C_01-DiagramaPU.xlsx");
}

function importarExcelDFormatoC() {
    var fileInput = document.createElement("input");
    fileInput.type = "file";
    fileInput.accept = ".xlsx, .xls";
    fileInput.onchange = function (e) {
        var file = e.target.files[0];
        var reader = new FileReader();

        reader.onload = function (e) {
            var data = new Uint8Array(e.target.result);
            var workbook = XLSX.read(data, { type: "array" });

            // Obtener la primera hoja de trabajo (sheet)
            var firstSheet = workbook.Sheets[workbook.SheetNames[0]];

            // Convertir la hoja a formato JSON
            var jsonData = XLSX.utils.sheet_to_json(firstSheet, { header: 1 });

            // Llamar a la funci�n para actualizar la tabla con los datos importados
            updateTableFromExcelDFormatoC(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}

function updateTableFromExcelDFormatoC(jsonData) {
    // El primer elemento contiene los encabezados, por lo tanto, comenzamos desde el segundo elemento
    var data = jsonData.slice(1);

    // Actualizar los datos de la tabla Handsontable (hotDFormatoC) con los datos importados
    hotDFormatoC.loadData(data);

    // Renderizar la tabla para reflejar los cambios
    hotDFormatoC.render();
}

function generarGraficaDFormatoC() {
    var tableData = hotDFormatoC.getData();

    var categories = [];
    var demandaData = [];
    var generacionData = [];

    for (var i = 0; i < tableData.length; i++) {
        var hora = tableData[i][0]; 
        var demandad = parseFloat(tableData[i][1]);
        var generaciond = parseFloat(tableData[i][2]);

        if (hora !== "" && !isNaN(demandad) && !isNaN(generaciond)) {
            categories.push(hora); 
            demandaData.push(demandad);
            generacionData.push(generaciond);
        }
    }

    Highcharts.chart("containerGraficaDFormatoC", {
        chart: {
            type: 'line'
        },
        title: {
            text: "Carga Diario Tipico en por Unidad (PU)"
        },
        xAxis: {
            categories: categories,
            title: {
                text: "Tiempo (Hora)"
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: "p.u"
            }
        },
        series: [
            {
                name: "Demanda (PU)",
                data: demandaData,
                marker: {
                    enabled: false
                }
            },
            {
                name: "Generacion (PU)",
                data: generacionData,
                marker: {
                    enabled: false
                }
            }
        ]
    });
}