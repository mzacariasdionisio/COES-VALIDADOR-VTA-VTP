var hot4CCGDD = null;

const configuracionColumnasCCGDD = {

    1: { tipo: "decimal", decimales: 1, permitirNegativo: true },
    2: { tipo: "decimal", decimales: 1, permitirNegativo: true },
 
 };
 
$(function () {
    crearTablaDiagCargHorario(30, 1440, 30);
 
});

function validateRow(hotInstance, rowIndex) {
    const totalCols = hotInstance.countCols();
    let isValidRow = true;

    for (let col = 1; col < totalCols; col++) { // Ignorar la columna 0 (Hora)
        const cellValue = hotInstance.getDataAtCell(rowIndex, col);
        const cellMeta = hotInstance.getCellMeta(rowIndex, col);

        if (cellValue === "" || cellValue === null) {
            cellMeta.valid = true;
            cellMeta.style = { backgroundColor: "" }; // Sin fondo
        } else {
            const parsedValue = parseFloat(cellValue);
            if (isNaN(parsedValue) || parsedValue < 0 || parsedValue > 1) {
                isValidRow = false;
                cellMeta.valid = false;
                cellMeta.style = { backgroundColor: "#ffcccc" }; // Fondo rojo
            } else {
                cellMeta.valid = true;
                cellMeta.style = { backgroundColor: "" }; // Sin fondo
            }
        }
    }

    hotInstance.render();
    return isValidRow;
}

function getDataCCGDD() {
    var datosArray = hot4CCGDD.getData();
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

    var grilla = document.getElementById('tableDiagCargHorario');

    if (typeof hot4CCGDD !== 'undefined' && hot4CCGDD !== null) {
        hot4CCGDD.destroy();
    }

    hot4CCGDD = new Handsontable(grilla, {
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
                { label: 'Demanda total de unidad productiva asociada al proyecto de GD', colspan: 1 },
                { label: 'Generaci&oacute;n de proyecto de GD', colspan: 1 }
            ]
        ],
        columns: [
            { data: 0, readOnly: true },
            {
                data: 1,
                type: 'text'
            },
            {
                data: 2,
                type: 'text'
            }
        ],
        beforeChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {

                return generalBeforeChange_4(this, changes, configuracionColumnasCCGDD);
            }
        }
  
    });
    hot4CCGDD.addHook("afterChange", function (changes, source) {
        if (source !== "loadData") {
            changes.forEach(([row, col, oldValue, newValue]) => {
                if (col > 0) { // Ignorar la columna Hora
                    validateRow(hot4CCGDD, row);
                }
            });
        }
    });


    hot4CCGDD.render();
}

function grabarCCGDD() {

    if (!validarTablaAntesDeGuardar(hot4CCGDD)) {
        mostrarNotificacion("No se puede guardar porque hay datos inválidos en la tabla.");
        return; // Detener el guardado si hay errores
    }
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.Ccgdddtos = getDataCCGDD();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarCCGDD',
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
                //$('#popupProyecto').bPopup().close()
                mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
            }
        });
    }
    cambiosRealizados = false;
}

function cargarCCGDD() {
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetCCGDD',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            
            success: function (response) {
                console.log(response);
                var hojaCRes = response.responseResult;
                setDataCCGDD(hojaCRes);
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function setDataCCGDD(datos) {
    // Obtener la data actual de la tabla Handsontable
    var currentData = hot4CCGDD.getData();

    // Iterar sobre los nuevos datos para encontrar coincidencias y actualizar la tabla
    datos.forEach(function (nuevoDato) {
        // Buscar el índice de la fila en la tabla actual que coincide con la 'Hora' del nuevo dato
        var rowIndex = currentData.findIndex(function (fila) {
            return fila[0] === nuevoDato.Hora; // Comparar por la columna 'Hora'
        });

        // Si se encuentra una coincidencia, actualizamos las columnas correspondientes
        if (rowIndex !== -1) {
            currentData[rowIndex][1] = nuevoDato.Demanda !== null ? nuevoDato.Demanda.toFixed(1) : '0.0';
            currentData[rowIndex][2] = nuevoDato.Generacion !== null ? nuevoDato.Generacion.toFixed(1) : '0.0';
        }
    });

    // Recargar los datos actualizados en Handsontable
    hot4CCGDD.loadData(currentData);
    hot4CCGDD.render(); // Renderizar la tabla para reflejar los cambios

    if (modoModel === "consultar") {
        desactivarCamposFormulario('CCGDD');
        $('#btnGrabarCCGDD').hide();
    }
}

function exportarTablaExcelCCGDD() {
    // Definir los encabezados de la tabla
    var headers = [
        ["HORA", "Demanda total de unidad productiva asociada al proyecto de GD", "Generación de proyecto de GD"]
    ];

    // Obtener los datos de la tabla Handsontable
    var datosArray = hot4CCGDD.getData();

    // Reemplazar celdas vacías por '0.0'
    datosArray = datosArray.map(row => row.map(cell => (cell === null || cell === "") ? "0.0" : cell));

    // Concatenar los encabezados con los datos
    var data = headers.concat(datosArray);

    // Crear una hoja de trabajo (worksheet) a partir de los datos
    var ws = XLSX.utils.aoa_to_sheet(data);
    aplicarBordes(ws);
    // Crear un nuevo libro de trabajo (workbook) y agregar la hoja
    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "DatosCCGDD");

    // Exportar el libro de trabajo a un archivo Excel
    XLSX.writeFile(wb, "GDD_04-PerfilPU.xlsx");
}

function importarExcelCCGDD() {
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

            // Llamar a la función para actualizar la tabla con los datos importados
            updateTableFromExcelCCGDD(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}


function updateTableFromExcelCCGDD(jsonData) {
    // Excluir la primera fila (cabecera)
    const data = jsonData.slice(1);

    // Obtener los datos actuales de la tabla
    const currentData = hot4CCGDD.getData();

    // Iterar sobre las filas importadas
    data.forEach((row, rowIndex) => {
        if (rowIndex < currentData.length) {
            row.forEach((cell, colIndex) => {
                if (colIndex > 0) { // Saltar la primera columna de los datos importados
                    const tableColIndex = colIndex; // Mapear la columna del Excel a partir de la segunda columna de la tabla

                    // Validar la celda
                    const parsedValue = parseFloat(cell);
                    if (cell === null || cell === "" || (typeof cell === "string" && cell.trim() === "")) {
                        // Celdas vacías o con espacios: llenar con "0.0"
                        currentData[rowIndex][tableColIndex] = "0.0";
                    } else if (!isNaN(parsedValue) && parsedValue >= 0) {
                        // Celdas válidas: formatear con un decimal
                        currentData[rowIndex][tableColIndex] = parsedValue.toFixed(1);
                    } else {
                        // Celdas inválidas: marcar como error en la tabla
                        currentData[rowIndex][tableColIndex] = cell;
                        validateRow(hot4CCGDD, rowIndex); // Validar la fila completa
                    }
                }
            });
        }
    });

    // Recargar los datos actualizados en la tabla
    hot4CCGDD.loadData(currentData);

    // Validar todas las filas nuevamente después de cargar los datos
    for (let i = 0; i < currentData.length; i++) {
        validateRow(hot4CCGDD, i);
    }

    // Renderizar la tabla para reflejar los cambios
    hot4CCGDD.render();
}

function generarGraficaCCGDD() {
    // Obtener los datos de la tabla Handsontable
    var tableData = hot4CCGDD.getData();

    // Arrays para las categor�as (eje X) y las series de datos (eje Y)
    var categories = [];
    var consumoEnergeticoData = [];
    var produccionCentralData = [];

    // Recorrer los datos de la tabla y llenar las categor�as y series
    for (var i = 0; i < tableData.length; i++) {
        var hora = tableData[i][0]; // Columna de Hora
        var consumoEnergetico = parseFloat(tableData[i][1]); // Columna de Consumo Energ�tico
        var produccionCentral = parseFloat(tableData[i][2]); // Columna de Producci�n de Central

        // Asegurarnos de que no haya datos vac�os o inv�lidos antes de agregarlos
        if (hora !== "" && !isNaN(consumoEnergetico) && !isNaN(produccionCentral)) {
            categories.push(hora); // A�adir hora al eje X
            consumoEnergeticoData.push(consumoEnergetico); // A�adir Consumo Energ�tico a la serie 1
            produccionCentralData.push(produccionCentral); // A�adir Producci�n de Central a la serie 2
        }
    }

    // Crear la gr�fica usando Highcharts
    Highcharts.chart("containerGraficaCCGDD", {
        chart: {
            type: 'line'
        },
        title: {
            text: "Consumo Energético y Producción de Central"
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
                text: "Valores"
            }
        },
        series: [
            {
                name: "Consumo Energético",
                data: consumoEnergeticoData,
                marker: {
                    enabled: false
                }
            },
            {
                name: "Producción de Central",
                data: produccionCentralData,
                marker: {
                    enabled: false
                }
            }
        ]
    });
}

