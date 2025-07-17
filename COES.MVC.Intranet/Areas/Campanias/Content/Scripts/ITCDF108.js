var hot108 = null;
const configuracionColumnas108 = {
   // 1: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    2: { tipo: "decimal", decimales: 4, permitirNegativo: true },
    3: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    4: { tipo: "decimal", decimales: 4, permitirNegativo: false },
};

$(function () {
    crearTablaITCF108((anioPeriodo - 1), (horizonteFin + 5), []);
});

function isNumeric(value) {
    var parsed = parseFloat(value);
    return !isNaN(parsed) && isFinite(parsed);
}

function customRenderer(instance, td, row, col, prop, value, cellProperties) {
    let rows = instance.countRows() - 1; // Última fila reservada para promedios
    const cellMeta = instance.getCellMeta(row, col);

    if (row === rows) {
        // Cálculo de la tasa de crecimiento promedio en la última fila
        let demandaInicial = null;
        let demandaFinal = null;
        let anioInicial = null;
        let anioFinal = null;

        for (let index = 0; index < rows; index++) {
            const cellValue = instance.getDataAtCell(index, col); // Valor de la celda
            const anio = instance.getDataAtCell(index, 1); // Año correspondiente

            if (isNumeric(cellValue) && parseFloat(cellValue) >= 0) { // Ignorar valores negativos
                if (demandaInicial === null) {
                    demandaInicial = parseFloat(cellValue);
                    anioInicial = anio;
                }
                demandaFinal = parseFloat(cellValue);
                anioFinal = anio;
            }
        }

        if (demandaInicial !== null && demandaFinal !== null && anioFinal > anioInicial) {
            const tasaCrecimientoPromedio =
                Math.pow(demandaFinal / demandaInicial, 1 / (anioFinal - anioInicial)) - 1;
            td.innerHTML = (tasaCrecimientoPromedio * 100).toFixed(4) + " %";
            cellMeta.valid = true;
        } else {
            td.innerHTML = "";
            cellMeta.valid = true;
        }
    } else {
        if (isNumeric(value)) {
            const numericValue = parseFloat(value);
            if (numericValue < 0) {
                td.innerHTML = numericValue.toFixed(4);
                td.style.backgroundColor = "red";
                td.style.color = "rgb(36, 92, 134)";
                td.title = "Valor negativo";
                cellMeta.valid = false;
            } else {
                td.innerHTML = numericValue.toFixed(4);
                td.style.backgroundColor = "";
                td.style.color = "";
                td.title = "";
                cellMeta.valid = true;
            }
        } else {
            if (value === null || value === "") {
                td.innerHTML = "";
                td.style.backgroundColor = "";
                td.style.color = "";
                td.title = "";
                cellMeta.valid = true;
            } else {
                td.innerHTML = value;
                td.style.backgroundColor = "red";
                td.style.color = "rgb(36, 92, 134)";
                td.title = "Dato incorrecto";
                cellMeta.valid = false;
            }
        }
    }
}



function sumaRenderer(instance, td, row, col, prop, value, cellProperties) {
    let rows = instance.countRows() - 1; // Última fila reservada para promedios

    if (row < rows) {
        // Columnas de datos AT, MT, BT
        const cols = [2, 3, 4];
        let total = 0;
        let hasData = false;

        cols.forEach((c) => {
            const cellValue = instance.getDataAtCell(row, c);
            if (isNumeric(cellValue)) {
                total += parseFloat(cellValue);
                hasData = true; // Indica que al menos una celda tiene dato numérico
            }
        });

        // Si no hay datos numéricos, dejar la celda vacía
        td.innerHTML = hasData ? total.toFixed(4) : "";
    } else {
        // Cálculo de la tasa de crecimiento promedio en la última fila
        let demandaInicial = null;
        let demandaFinal = null;
        let anioInicial = null;
        let anioFinal = null;

        const firstRow = instance.getDataAtRow(0);
        const lastRow = instance.getDataAtRow(rows - 1);

        const firstRowTotal = (isNumeric(firstRow[2]) ? firstRow[2] : 0) +
            (isNumeric(firstRow[3]) ? firstRow[3] : 0) +
            (isNumeric(firstRow[4]) ? firstRow[4] : 0);

        const lastRowTotal = (isNumeric(lastRow[2]) ? lastRow[2] : 0) +
            (isNumeric(lastRow[3]) ? lastRow[3] : 0) +
            (isNumeric(lastRow[4]) ? lastRow[4] : 0);

        if (isNumeric(firstRowTotal) && isNumeric(lastRowTotal)) {
            demandaInicial = parseFloat(firstRowTotal);
            demandaFinal = parseFloat(lastRowTotal);
            anioInicial = firstRow[1];
            anioFinal = lastRow[1];

            if (demandaInicial !== null && demandaFinal !== null && anioFinal > anioInicial) {
                const tasaCrecimientoPromedio = Math.pow(demandaFinal / demandaInicial, 1 / (anioFinal - anioInicial)) - 1;
                td.innerHTML = (tasaCrecimientoPromedio * 100).toFixed(4) + " %";
            } else {
                td.innerHTML = "";
            }
        } else {
            td.innerHTML = "";
        }
    }
}


function crearTablaITCF108(inicioanParam, finanParam, data) {
    var grilla = document.getElementById("tablef108");
    var total = finanParam - inicioanParam;

    // Asignar globalmente inicioan y finan para su uso en el cálculo
    inicioan = inicioanParam;
    finan = finanParam;

    if (data.length === 0) {
        for (var i = 0; i <= total; i++) {
            var row = [];
            if (i === 0) {
                row.push("Hist.");
            } else {
                row.push(i);
            }
            row.push(inicioan + i);
            row.push("");
            row.push("");
            row.push("");
            row.push("");
            data.push(row);
        }

        // Agregar fila para promedios
        var promedioRow = ["", "TASA PROMEDIO (%)", "", "", "", ""];
        data.push(promedioRow);
    }

    if (typeof hot108 !== "undefined" && hot108 !== null) {
        hot108.destroy();
    }

    hot108 = new Handsontable(grilla, {
        data: data,
        rowHeaders: true,
        colHeaders: true,
        colWidths: 170,
        manualColumnResize: true,
        columnSorting: false,
        maxRows: data.length,
        filters: false,
        dropdownMenu: true,
        autoColumnSize: true,
        nestedHeaders: [
            [
                { label: "", colspan: 2 },
                { label: "NIVEL DE TENSI&Oacute;N", colspan: 3 },
                { label: "TOTAL VENTAS", colspan: 1 },
            ],
            [{ label: "A&Ntilde;O", colspan: 2 }, "AT", "MT", "BT", "AT + MT + BT"],
        ],
        columns: [
            { readOnly: true },
            { readOnly: true },
            { type: "text" },
            { type: "text" },
            { type: "text" },
            {
                type: "text"
                //   readOnly: true,
                // renderer: sumaRenderer, // La última columna calcula la suma
            },
        ],
        cells: function (row, col) {
            var cellProperties = {};

            // Aplicar fondo gris claro (#FAFAFA) a la última columna (columna 5)
            if (col === 5) {
                cellProperties.renderer = function (instance, td, row, col, prop, value, cellProperties) {
                    Handsontable.renderers.TextRenderer.apply(this, arguments);
                    td.style.backgroundColor = "#FAFAFA"; // Fondo gris claro
                };
            }

            return cellProperties;
        }
        ,
        beforeChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
                return generalBeforeChange_4(this, changes, configuracionColumnas108);
            }
        },
        afterChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
                generalAfterChange_global(this, changes, configuracionColumnas108);
            }
        }

    });


    hot108.render();
    // Hook para actualizar la tasa de crecimiento en tiempo real mientras se escribe
    hot108.addHook("afterChange", function (changes, source) {
        if (!changes || source === "loadData") return; // Evitar bucles infinitos

        let huboCambio = false;

        changes.forEach(([row, col, oldValue, newValue]) => {
            if (col >= 2 && col <= 4) { // Solo afecta AT, MT y BT
                if (oldValue !== newValue) {
                    huboCambio = true;
                }
            }
        });

        if (huboCambio) {
            actualizarSumaYPromedio();
        }
    });

}

//let timeoutId;
//let actualizandoTasa = false; // Flag para evitar bucles infinitos

//hot108.addHook("afterChange", function (changes, source) {
//    if (source !== "loadData" && changes && !actualizandoTasa) {
//        clearTimeout(timeoutId); // Cancelar cualquier ejecución previa

//        timeoutId = setTimeout(() => {
//            console.log("Calculando tasa de crecimiento promedio...");

//            let datosActuales = hot108.getData();
//            let filas = datosActuales.length - 1; // Última fila reservada para promedios

//            let demandaInicial = null;
//            let demandaFinal = null;
//            let anioInicial = null;
//            let anioFinal = null;

//            for (let i = 0; i < filas; i++) {
//                let total = parseFloat(datosActuales[i][5]); // Total en la columna 5
//                let anio = parseInt(datosActuales[i][1]); // Año en la columna 1

//                if (!isNaN(anio) && !isNaN(total) && total > 0) {
//                    if (demandaInicial === null) {
//                        demandaInicial = total;
//                        anioInicial = anio;
//                    }
//                    demandaFinal = total;
//                    anioFinal = anio;
//                }
//            }

//            // Calcular la tasa de crecimiento promedio
//            if (demandaInicial !== null && demandaFinal !== null && anioFinal > anioInicial) {
//                const tasaCrecimientoPromedio = Math.pow(demandaFinal / demandaInicial, 1 / (anioFinal - anioInicial)) - 1;

//                actualizandoTasa = true; // Evita que `setDataAtCell` dispare `afterChange` nuevamente
//                hot108.setDataAtCell(filas, 5, (tasaCrecimientoPromedio * 100).toFixed(4) + " %");
//                actualizandoTasa = false; // Restablecer el flag después de actualizar

//                console.log(`Tasa de crecimiento promedio calculada: ${(tasaCrecimientoPromedio * 100).toFixed(4)} %`);
//            } else {
//                actualizandoTasa = true;
//                hot108.setDataAtCell(filas, 5, ""); // Si no hay suficientes datos, dejar vacío
//                actualizandoTasa = false;

//                console.log("No hay suficientes datos para calcular la tasa de crecimiento promedio.");
//            }

//        }, 500); // Espera 500ms para evitar múltiples ejecuciones
//    }
//});
let actualizandoDatos = false; // Flag para evitar recursión infinita
let pendienteActualizacion = false; // Flag para evitar llamadas múltiples

function actualizarSumaYPromedio() {
    if (actualizandoDatos) return;

    actualizandoDatos = true;
    console.log("Actualizando sumas y tasas de crecimiento...");

    let datosActuales = hot108.getData();
    let filas = datosActuales.length - 1; // Última fila reservada para tasa de crecimiento
    let columnas = [2, 3, 4, 5]; // Columnas AT, MT, BT y Total

    // Variables para demanda inicial y final por cada columna
    let demandaInicial = { 2: null, 3: null, 4: null, 5: null };
    let demandaFinal = { 2: null, 3: null, 4: null, 5: null };
    let anioInicial = { 2: null, 3: null, 4: null, 5: null };
    let anioFinal = { 2: null, 3: null, 4: null, 5: null };

    // Recorrer todas las filas de datos
    for (let i = 0; i < filas; i++) {
        let anio = parseInt(datosActuales[i][1]); // Año en la segunda columna
        let at = isNumeric(datosActuales[i][2]) ? parseFloat(datosActuales[i][2]) : null;
        let mt = isNumeric(datosActuales[i][3]) ? parseFloat(datosActuales[i][3]) : null;
        let bt = isNumeric(datosActuales[i][4]) ? parseFloat(datosActuales[i][4]) : null;

        // Calcular la suma de AT, MT, BT en la columna 5
        let suma = (at ?? 0) + (mt ?? 0) + (bt ?? 0);
        let tieneDatos = at !== null || mt !== null || bt !== null;
        hot108.setDataAtCell(i, 5, tieneDatos ? suma.toFixed(4) : "", "silent");

        // Recorrer las columnas para encontrar la demanda inicial y final
        columnas.forEach(col => {
            let valor = parseFloat(datosActuales[i][col]);
            if (isNumeric(valor)) {
                if (demandaInicial[col] === null) {
                    demandaInicial[col] = valor;
                    anioInicial[col] = anio;
                }
                demandaFinal[col] = valor;
                anioFinal[col] = anio;
            }
        });
    }

    // Aplicar la fórmula de tasa de crecimiento promedio por cada columna
    columnas.forEach(col => {
        if (
            demandaInicial[col] !== null &&
            demandaFinal[col] !== null &&
            anioInicial[col] !== null &&
            anioFinal[col] !== null &&
            anioFinal[col] > anioInicial[col]
        ) {
            const tasaCrecimientoPromedio =
                Math.pow(demandaFinal[col] / demandaInicial[col], 1 / (anioFinal[col] - anioInicial[col])) - 1;
            hot108.setDataAtCell(filas, col, (tasaCrecimientoPromedio * 100).toFixed(4) + " %", "silent");
        } else {
            hot108.setDataAtCell(filas, col, "", "silent");
        }
    });

    actualizandoDatos = false;
    hot108.render();
    console.log("Tasas de crecimiento actualizadas correctamente.");
}

// ✅ **Único evento para detectar cambios, eliminaciones y pegados**
hot108.addHook('afterChange', function (changes, source) {
    if (!changes || source === "loadData") return;
    if (!pendienteActualizacion) {
        pendienteActualizacion = true;
        requestAnimationFrame(() => {
            actualizarSumaYPromedio();
            pendienteActualizacion = false;
        });
    }
});

// ✅ **Detectar cambios en selección para eliminaciones masivas**
hot108.addHook("afterSelectionEnd", function () {
    if (!pendienteActualizacion) {
        pendienteActualizacion = true;
        requestAnimationFrame(() => {
            actualizarSumaYPromedio();
            pendienteActualizacion = false;
        });
    }
});

hot108.addHook("afterChange", function (changes, source) {
    if (!changes || source === "loadData") return;

    let seEliminoAlgo = false; // Flag para detectar eliminaciones

    changes.forEach(([row, col, oldValue, newValue]) => {
        if (col >= 2 && col <= 4) { // Solo en columnas AT, MT y BT
            if ((oldValue !== null && oldValue !== "") && (newValue === null || newValue === "")) {
                seEliminoAlgo = true; // Detecta que un dato fue eliminado
            }
        }
    });

    if (seEliminoAlgo) {
        actualizarSumaYPromedio();
    }
});



function getDatos108() {
    var datosArray = hot108.getData();

    // Filtrar filas donde al menos una de las columnas AT, MT o BT tenga datos válidos
    var datosFiltrados = datosArray.filter(row => {
        let anio = row[1]; // Año en la segunda columna
        let atval = row[2];
        let mtval = row[3];
        let btval = row[4];

        // Validar si la fila debe enviarse: al menos un valor no debe estar vacío
        let tieneValores = (
            (atval !== "" && atval !== null) ||
            (mtval !== "" && mtval !== null) ||
            (btval !== "" && btval !== null)
        );

        return anio !== null && anio !== "" && !isNaN(anio) && tieneValores;
    });

    var datosObjetos = datosFiltrados.map(function (row) {
        return {
            Anio: row[1], // Año
            Atval: row[2] === "" || row[2] === null ? null : row[2],   // AT
            Mtval: row[3] === "" || row[3] === null ? null : row[3],   // MT
            Btval: row[4] === "" || row[4] === null ? null : row[4]    // BT
        };
    });

    console.log("Datos enviados al backend:", datosObjetos);
    return datosObjetos;
}

 

function cargarDatos108() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetItcdf108',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            
            success: function (response) {
                console.log(response);
                var hoja108Data = response.responseResult;
                setDatos108(hoja108Data);
                cambiosRealizados = false;
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function setDatos108(lista108) {
    console.log("Cargando datos en la tabla ITCF108");
  //  hot108.loadData([]);
    // Obtener los datos actuales de la tabla para conservar los años
    var datosArray = hot108.getData();

    // Convertir la lista del backend en un diccionario basado en el año (segunda columna)
    var datosPorAnio = {};
    lista108.forEach(item => {
        datosPorAnio[item.Anio] = {
            Atval: isNumeric(item.Atval) ? parseFloat(item.Atval).toFixed(4) : "", // Formato 4 decimales
            Mtval: isNumeric(item.Mtval) ? parseFloat(item.Mtval).toFixed(4) : "", // Formato 4 decimales
            Btval: isNumeric(item.Btval) ? parseFloat(item.Btval).toFixed(4) : ""  // Formato 4 decimales
        };
    });

    // Recorrer la tabla actual y sincronizar con los datos del backend
    for (let i = 0; i < datosArray.length - 1; i++) { // Excluir la última fila de tasa promedio
        let anioTabla = datosArray[i][1]; // Año en la segunda columna

        if (datosPorAnio[anioTabla]) {
            // Si hay datos para este año, asignarlos
            datosArray[i][2] = datosPorAnio[anioTabla].Atval;
            datosArray[i][3] = datosPorAnio[anioTabla].Mtval;
            datosArray[i][4] = datosPorAnio[anioTabla].Btval;
        } else {
            // Si no hay datos para este año, dejar la fila vacía (excepto el año)
            datosArray[i] = [datosArray[i][0], anioTabla, "", "", "", ""];
        }
    }

    // Aplicar los datos sincronizados en la tabla sin modificar la estructura
    hot108.loadData(datosArray);
    actualizarSumaYPromedio();
}

function exportarTablaExcel108() {
    var headers = [
        [
            "",
            "",
            "NIVEL DE TENSION",
            "NIVEL DE TENSION",
            "NIVEL DE TENSION",
            "TOTAL DE VENTA",
        ],
        ["AÑO", "AÑO", "AT", "MT", "BT", "AT + MT + BT"],
    ];

    // Obtener los datos actuales de la tabla
    var datosArray = hot108.getData();

    // Recalcular la suma de la última columna (AT + MT + BT)
    datosArray.forEach((row, rowIndex) => {
        if (rowIndex < datosArray.length - 1) { // Excluye la última fila (promedio)
            let suma = 0;
            for (let col = 2; col <= 4; col++) {
                const valor = parseFloat(row[col]);
                suma += isNumeric(valor) ? valor : 0;
            }
            row[5] = suma.toFixed(4); // Asigna la suma a la última columna
        }
    });

    // Recalcular los valores de la última fila (TASA PROMEDIO (%))
    var promedioRow = datosArray[datosArray.length - 1]; // Última fila
    promedioRow[1] = "TASA PROMEDIO (%)"; // Cambiar el nombre de la fila

    for (let col = 2; col <= 5; col++) {
        let demandaInicial = null;
        let demandaFinal = null;
        let anioInicial = null;
        let anioFinal = null;

        for (let row = 0; row < datosArray.length - 1; row++) {
            const cellValue = datosArray[row][col]; // Valor de la columna
            const anio = datosArray[row][1]; // Año correspondiente

            if (isNumeric(cellValue)) {
                if (demandaInicial === null) {
                    demandaInicial = parseFloat(cellValue);
                    anioInicial = anio;
                }
                demandaFinal = parseFloat(cellValue);
                anioFinal = anio;
            }
        }

        // Calcular TASA PROMEDIO (%)
        if (
            demandaInicial !== null &&
            demandaFinal !== null &&
            anioInicial !== null &&
            anioFinal !== null &&
            anioFinal > anioInicial
        ) {
            const tasaCrecimientoPromedio =
                Math.pow(demandaFinal / demandaInicial, 1 / (anioFinal - anioInicial)) - 1;
            promedioRow[col] = (tasaCrecimientoPromedio * 100).toFixed(4) + " %";
        } else {
            promedioRow[col] = ""; // Si no hay datos suficientes, dejar vacío
        }
    }

    // Agregar los encabezados y los datos
    var data = headers.concat(datosArray);

    // Crear la hoja de Excel
    var ws = XLSX.utils.aoa_to_sheet(data);
    aplicarBordes(ws);

    // Definir las celdas combinadas
    ws["!merges"] = [
        { s: { r: 0, c: 2 }, e: { r: 0, c: 4 } }, // Combinar "NIVEL DE TENSIÓN"
        { s: { r: 0, c: 5 }, e: { r: 0, c: 5 } }, // Combinar "TOTAL DE VENTA"
    ];
    aplicarFondoGris(ws, [5], hot108, headers.length);
    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "F108");

    // Descargar el archivo Excel
    XLSX.writeFile(wb, "ITC_DemEDE_02-F108.xlsx");
}

function importarExcel108() {
    var fileInput = document.createElement("input");
    fileInput.type = "file";
    fileInput.accept = ".xlsx, .xls";
    fileInput.onchange = function (e) {
        var file = e.target.files[0];
        var reader = new FileReader();

        reader.onload = function (e) {
            var data = new Uint8Array(e.target.result);
            var workbook = XLSX.read(data, { type: "array" });

            var firstSheet = workbook.Sheets[workbook.SheetNames[0]];

            var jsonData = XLSX.utils.sheet_to_json(firstSheet, { header: 1 });

            updateTableFromExcel108(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}

function updateTableFromExcel108(jsonData) {
    let data = jsonData.slice(2); // Excluir encabezados del archivo Excel

    // Obtener los datos actuales de la tabla
    const totalFilas = hot108.countRows(); // Mantener el número de filas
    const totalColumnas = hot108.countCols(); // Número total de columnas
    const ultimaFila = totalFilas - 1; // Índice de la última fila (tasa promedio)

    // Guardar estilos y validaciones antes de importar
    const stylesBackup = {};
    for (let row = 0; row < totalFilas; row++) {
        for (let col = 2; col < totalColumnas; col++) { // Solo columnas editables
            const cellMeta = hot108.getCellMeta(row, col);
            stylesBackup[`${row}-${col}`] = {
                backgroundColor: cellMeta.style?.backgroundColor || "",
                color: cellMeta.style?.color || "",
                fontWeight: cellMeta.style?.fontWeight || "",
                valid: cellMeta.valid,
            };
        }
    }

    // Recorrer cada fila sin modificar la cantidad de filas
    for (let rowIndex = 0; rowIndex < totalFilas; rowIndex++) {
        if (data[rowIndex]) { // Solo si hay datos en la fila importada
            for (let col = 2; col < totalColumnas; col++) { // Desde la tercera columna en adelante
                if (data[rowIndex][col] !== undefined) {
                    let newValue = isNumeric(data[rowIndex][col]) ? parseFloat(data[rowIndex][col]).toFixed(4) : "";
                    hot108.setDataAtCell(rowIndex, col, newValue, "silent");
                }
            }
        }
    }

    // Restaurar los estilos y validaciones después de importar datos
    for (const key in stylesBackup) {
        const [row, col] = key.split("-").map(Number);
        const { backgroundColor, color, fontWeight, valid } = stylesBackup[key];

        hot108.setCellMeta(row, col, "style", {
            backgroundColor,
            color,
            fontWeight,
        });
        hot108.setCellMeta(row, col, "valid", valid);
    }

    // Verificar errores en los datos importados y resaltar en rojo (excepto en la última fila)
    const celdasConErrores = [];
    for (let row = 0; row < totalFilas - 1; row++) { // Ignorar la última fila
        for (let col = 2; col < totalColumnas; col++) {
            let value = hot108.getDataAtCell(row, col);
            const config = configuracionColumnas108[col];

            if (config && value !== null && value !== "" && value !== undefined) {
                let parsedValue = parseFloat(value);
                let esValido = true;

                if (isNaN(parsedValue)) {
                    esValido = false;
                } else if (!config.permitirNegativo && parsedValue < 0) {
                    esValido = false;
                } else if (config.tipo === "decimal" && value.split(".")[1]?.length > config.decimales) {
                    esValido = false;
                } else if (config.tipo === "entero" && !Number.isInteger(parsedValue)) {
                    esValido = false;
                }

                if (!esValido) {
                    celdasConErrores.push(`Fila ${row + 1}, Columna ${col + 1}`);
                    hot108.setCellMeta(row, col, "valid", false);
                    hot108.getCell(row, col).style.backgroundColor = "#ff4c42"; // Fondo rojo claro
                    hot108.getCell(row, col).style.color = "white"; // Texto blanco
                } else {
                    // Si es válido, limpiar estilo previo
                    hot108.setCellMeta(row, col, "valid", true);
                    hot108.getCell(row, col).style.backgroundColor = "";
                    hot108.getCell(row, col).style.color = "";
                }
            }
        }
    }

    // Asegurar que la última fila no tenga errores resaltados
    for (let col = 2; col < totalColumnas; col++) {
        hot108.setCellMeta(ultimaFila, col, "valid", true);
        hot108.getCell(ultimaFila, col).style.backgroundColor = "";
        hot108.getCell(ultimaFila, col).style.color = "";
    }

    // Refrescar la tabla con los cambios
    hot108.render();

    // Mostrar notificación de errores si los hay
    if (celdasConErrores.length > 0) {
        mostrarNotificacion(
            `Se importaron datos con errores.<br>Errores detectados en:<br>${celdasConErrores.join("<br>")}`
        );
    } else {
        mostrarNotificacion("Los datos se importaron correctamente.", "#a3e352");
    }

    // Recalcular sumas y tasas después de la importación
    actualizarSumaYPromedio();
}
