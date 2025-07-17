var hotOpt = null;
var hotPes = null;
var anioIni = 2018;
const configuracionColumnascgdb = {

    2: { tipo: "decimal", decimales: 4, permitirNegativo: true },
    3: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    4: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    5: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    6: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    7: { tipo: "decimal", decimales: 4, permitirNegativo: false }
};

$(function () {
    crearTablaBalanceOptimista(anioIni, horizonteFin);
    crearTablaBalancePesimista(anioIni, horizonteFin);
    var formularioccgdc = document.getElementById('CCGDC');
    $(formularioccgdc).off('change').on('change', function () {
        cambiosRealizados = true;
    });
});

function grabarCCGDC() {
    if (!validarTablaAntesDeGuardar(hotOpt)) {
        // mostrarNotificacion("No se puede guardar porque hay datos inválidos en la tabla.");
        return; // Detener el guardado si hay errores
    }
    if (!validarTablaAntesDeGuardar(hotPes)) {
        // mostrarNotificacion("No se puede guardar porque hay datos inválidos en la tabla.");
        return; // Detener el guardado si hay errores
    }
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.CCGDCOptDTOs = getDataCCGDCOptimista();
        param.CCGDCPesDTOs = getDataCCGDCPesimista();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarCCGDC',
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

function cargarCCGDC() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetCCGDC',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            
            success: function (response) {
                console.log(response);
                var hojaOpt = response.responseResult1;
                var hojaPes = response.responseResult2;
                setDataCCGDCOpt(hojaOpt);
                setDataCCGDCPes(hojaPes);
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function setDataCCGDCOpt(dataObjects) {
    if (typeof hotOpt === 'undefined' || hotOpt === null) {
        console.error("La tabla Handsontable no está inicializada.");
        return;
    }
    // Obtener la cantidad de filas y columnas actuales de la tabla
    var rowCount = hotOpt.countRows();
    var colCount = hotOpt.countCols();

    // Limpiar los valores de todas las celdas (sin modificar encabezados ni configuración)
    var emptyData = [];
    for (var r = 0; r < rowCount; r++) {
        var row = [];
        for (var c = 0; c < colCount; c++) {
            row.push((c < 2) ? hotOpt.getDataAtCell(r, c) : ""); // Mantener Año y Mes, limpiar demás celdas
        }
        emptyData.push(row);
    }
    hotOpt.loadData(emptyData);

    // Obtener todos los datos actuales de la tabla
    var currentData = hotOpt.getData();

    // Iterar sobre cada objeto de datos proporcionado
    dataObjects.forEach(function (obj) {
        // Buscar el índice de fila correspondiente al año y mes del objeto actual
        var rowIndex = currentData.findIndex(function (row) {
            return row[0] === obj.Anio && row[1] === obj.Mes;
        });

        // Si se encuentra la fila correspondiente, actualizar las celdas específicas
        if (rowIndex !== -1) {
            currentData[rowIndex][2] = formatDecimal(obj.DemandaEnergia) ?? ""; // Energía
            currentData[rowIndex][3] = formatDecimal(obj.DemandaHP) ?? ""; // HP
            currentData[rowIndex][4] = formatDecimal(obj.DemandaHFP) ?? ""; // HFP
            currentData[rowIndex][5] = formatDecimal(obj.GeneracionEnergia) ?? ""; // Energía Generación
            currentData[rowIndex][6] = formatDecimal(obj.GeneracionHP) ?? ""; // HP Generación
            currentData[rowIndex][7] = formatDecimal(obj.GeneracionHFP) ?? ""; // HFP Generación
        }
    });

    // Cargar los datos actualizados en la tabla
    hotOpt.loadData(currentData);

    // Renderizar la tabla para reflejar los cambios
    hotOpt.render();

    if (modoModel === "consultar") {
        desactivarCamposFormulario('CCGDC');
        $('#btnGrabarCCGDC').hide();
    }
}



function crearTablaBalanceOptimista(inicioan, finan) {

    var data = [];
    var meses = ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Setiembre', 'Octubre', 'Noviembre', 'Diciembre'];
    for (var year = inicioan; year <= finan; year++) {
        meses.forEach(function (str) {
            var row = [year];
            row.push(str);
            for (var i = 0; i < 6; i++) {
                row.push('');
            }
            data.push(row);
        });
    }
    var grilla = document.getElementById('tableBalanceODOpt');

    if (typeof hotOpt !== 'undefined' && hotOpt !== null) {
        hotOpt.destroy();
    }

    hotOpt = new Handsontable(grilla, {
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
                { label: '', colspan: 1 },
                { label: '', colspan: 1 },
                { label: 'DEMANDA ESTIMADA DE UNIDAD PRODUCTIVA ASOCIADA<br>AL PROYECTO DE GENERACIÓN DISTRIBUIDA', colspan: 3 },
                { label: 'GENERACIÓN ESTIMADA DE PROYECTO DE GENERACIÓN DISTRIBUIDA', colspan: 3 }
            ],
            [
                { label: '', colspan: 1 },
                { label: '', colspan: 1 },
                { label: '', colspan: 1 },
                { label: 'POTENCIA(MW)', colspan: 2 },
                { label: '', colspan: 1 },
                { label: 'POTENCIA(MW)', colspan: 2 },
            ],
            [
                { label: 'A&ntilde;o', colspan: 1 },
                { label: 'Mes', colspan: 1 },
                { label: 'ENERGIA(GWh)', colspan: 1 },
                { label: 'HP(3)', colspan: 1 },
                { label: 'HFP(4)', colspan: 1 },
                { label: 'ENERGIA(GWh)', colspan: 1 },
                { label: 'HP(3)', colspan: 1 },
                { label: 'HFP(4)', colspan: 1 }
            ]
        ],
        columns: [
            { data: 0, readOnly: true},
            { data: 1, readOnly: true},
            {
                data: 2,
                type: 'text'
            },
            {
                data: 3,
                type: 'text'
            },
            {
                data: 4,
                type: 'text'
            },
            {
                data: 5,
                type: 'text'
            },
            {
                data: 6,
                type: 'text'
            },
            {
                data: 7,
                type: 'text'
            }
        ],
        beforeChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
                return generalBeforeChange_4(this, changes, configuracionColumnascgdb);
            }
        },
        afterChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
                generalAfterChange_global(this, changes, configuracionColumnascgdb);
            }
        }
    });

    hotOpt.render();
}

function getDataCCGDCOptimista() {
    var datosArray = hotOpt.getData();
    var datosObjetos = datosArray.map(function (row) {
        return {
            Anio: row[0],
            Mes: row[1],
            DemandaEnergia: row[2],
            DemandaHP: row[3],
            DemandaHFP: row[4],
            GeneracionEnergia: row[5],
            GeneracionHP: row[6],
            GeneracionHFP: row[7]
        };
    });
    console.log(datosObjetos);
    // Filtrar objetos que tienen al menos uno de los campos con datos v�lidos
    var datosFiltrados = datosObjetos.filter(function (obj) {
        return (
            obj.DemandaEnergia !== null && obj.DemandaEnergia !== "" ||
            obj.DemandaHP !== null && obj.DemandaHP !== "" ||
            obj.DemandaHFP !== null && obj.DemandaHFP !== "" ||
            obj.GeneracionEnergia !== null && obj.GeneracionEnergia !== "" ||
            obj.GeneracionHP !== null && obj.GeneracionHP !== "" ||
            obj.GeneracionHFP !== null && obj.GeneracionHFP !== ""
        );
    });
    return datosFiltrados;
}

function exportarTablaExcelCCGDCOptimista() {
    // Definir los encabezados como en el ejemplo proporcionado
    var headers = [
        [
            "",
            "",
            "DEMANDA ESTIMADA DE UNIDAD PRODUCTIVA ASOCIADA AL PROYECTO DE GENERACIÓN DISTRIBUIDA",
            "DEMANDA ESTIMADA DE UNIDAD PRODUCTIVA ASOCIADA AL PROYECTO DE GENERACIÓN DISTRIBUIDA",
            "DEMANDA ESTIMADA DE UNIDAD PRODUCTIVA ASOCIADA AL PROYECTO DE GENERACIÓN DISTRIBUIDA",
            "GENERACIÓN ESTIMADA DE PROYECTO DE GENERACIÓN DISTRIBUIDA",
            "GENERACIÓN ESTIMADA DE PROYECTO DE GENERACIÓN DISTRIBUIDA",
            "GENERACIÓN ESTIMADA DE PROYECTO DE GENERACIÓN DISTRIBUIDA"
        ],
        [
            "AÑO",
            "MES",
            "ENERGIA(GWh)",
            "POTENCIA (MW) HP",
            "POTENCIA (MW) HFP",
            "ENERGIA(GWh)",
            "POTENCIA (MW) HP",
            "POTENCIA (MW) HFP"
        ]
    ];

    // Obtener los datos de la tabla Handsontable (hotOpt)
    var datosArray = hotOpt.getData();
    // Concatenar los encabezados con los datos
    var data = headers.concat(datosArray);

    // Crear una hoja de trabajo (worksheet) a partir de los datos
    var ws = XLSX.utils.aoa_to_sheet(data);
    aplicarBordes(ws);
    // Definir las celdas combinadas para los encabezados
    ws["!merges"] = [
        { s: { r: 0, c: 2 }, e: { r: 0, c: 4 } }, // Combina DEMANDA ESTIM. PROY. DE PROD. HIDROG.
        { s: { r: 0, c: 5 }, e: { r: 0, c: 7 } }  // Combina GENERA. ESTIM. DE PROY. DE GENE. ASOC.
    ];
    // Aplicar formato de 4 decimales a partir de la celda (2, 2)
    var startRow = 2; // Fila 3 en t�rminos de Excel
    var startCol = 2; // Columna C en t�rminos de Excel

    for (var R = startRow; R < data.length; R++) {
        for (var C = startCol; C < data[R].length; C++) {
            var cellAddress = XLSX.utils.encode_cell({ r: R, c: C });
            var cell = ws[cellAddress];
            if (cell && typeof cell.v === 'number') {
                cell.z = '0.0000'; // Aplicar el formato de 4 decimales
            }
        }
    }

    // Crear un nuevo libro de trabajo (workbook) y agregar la hoja
    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "CCGDC");

    // Exportar el libro de trabajo a un archivo Excel
    XLSX.writeFile(wb, "GDC_02-EscenarioOptimista.xlsx");
}

function importarExcelCCGDCOptimista() {
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
            updateTableFromExcelBalanceOptimista(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}
function updateTableFromExcelBalanceOptimista(jsonData) {
    // Los encabezados ocupan las dos primeras filas, los datos comienzan en la tercera fila
    var headers = jsonData.slice(0, 2); // Las dos primeras filas son los headers (no se usan aqu�)
    var data = jsonData.slice(2); // El resto son los datos

    // Obtener el número de filas actuales en la tabla
    const currentRowCount = hotOpt.countRows();
    const currentColCount = hotOpt.countCols();

    // Limitar el número de filas importadas al número de filas existentes
    if (data.length > currentRowCount) {
        data = data.slice(0, currentRowCount); // Truncar datos al número permitido
    }
    // **1. Limpiar todas las celdas antes de importar nueva data**
    for (let row = 0; row < currentRowCount; row++) {
        for (let col = 0; col < currentColCount; col++) {
            hotOpt.setCellMeta(row, col, "valid", true);
            hotOpt.setCellMeta(row, col, "style", {
                backgroundColor: "",
            });
        }
    }
    // Guardar los valores originales de la primera columna
    const primeraColumna = hotOpt.getData().map(row => row[0]);
    const segColumna = hotOpt.getData().map(row => row[1]);


    // Validar y formatear los datos
    const datosValidados = validarYFormatearDatos2(data, configuracionColumnascgdb, mostrarNotificacion, hotOpt);

    // Restaurar la primera columna antes de cargar los datos validados
    datosValidados.forEach((row, rowIndex) => {
        row[0] = primeraColumna[rowIndex];
        row[1] = segColumna[rowIndex];
    });

    // Almacenar estilos existentes antes de cargar los nuevos datos
    const stylesBackup = {};
    for (let row = 0; row < hotOpt.countRows(); row++) {
        for (let col = 0; col < hotOpt.countCols(); col++) {
            const cellMeta = hotOpt.getCellMeta(row, col);
            stylesBackup[`${row}-${col}`] = {
                backgroundColor: cellMeta.style?.backgroundColor || "",
                color: cellMeta.style?.color || "",
                fontWeight: cellMeta.style?.fontWeight || "",
                valid: cellMeta.valid,
            };
        }
    }


    // Actualizar los datos de la tabla Handsontable (hotOpt) con los datos importados
    hotOpt.loadData(data);

    // Restaurar los estilos y validaciones
    for (const key in stylesBackup) {
        const [row, col] = key.split("-").map(Number);
        const { backgroundColor, color, fontWeight, valid } = stylesBackup[key];

        hotOpt.setCellMeta(row, col, "style", {
            backgroundColor,
            color,
            fontWeight,
        });
        hotOpt.setCellMeta(row, col, "valid", valid);
    }
    // Verificar errores en los datos validados
    const celdasConErrores = [];
    datosValidados.forEach((row, rowIndex) => {
        row.forEach((value, colIndex) => {
            const config = configuracionColumnascgdb[colIndex];
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
                    celdasConErrores.push(`Fila ${rowIndex + 1}, Columna ${colIndex + 1}`);
                    hotOpt.setCellMeta(rowIndex, colIndex, "valid", false);
                    hotOpt.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "#ffcccc", // Rojo claro
                    });
                } else {
                    // Si es válido, limpiar estilo previo
                    hotOpt.setCellMeta(rowIndex, colIndex, "valid", true);
                    hotOpt.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "",
                    });
                }
            }
        });
    });

    // Renderizar la tabla para reflejar los cambios
    hotOpt.render();
}



//PESIMISTA

function setDataCCGDCPes(dataObjects) {
    if (typeof hotPes === 'undefined' || hotPes === null) {
        console.error("La tabla Handsontable no está inicializada.");
        return;
    }

    // Obtener la cantidad de filas y columnas actuales de la tabla
    var rowCount = hotOpt.countRows();
    var colCount = hotOpt.countCols();

    // Limpiar los valores de todas las celdas (sin modificar encabezados ni configuración)
    var emptyData = [];
    for (var r = 0; r < rowCount; r++) {
        var row = [];
        for (var c = 0; c < colCount; c++) {
            row.push((c < 2) ? hotPes.getDataAtCell(r, c) : ""); // Mantener Año y Mes, limpiar demás celdas
        }
        emptyData.push(row);
    }
    hotPes.loadData(emptyData);
    // Obtener todos los datos actuales de la tabla
    var currentData = hotPes.getData();

    // Iterar sobre cada objeto de datos proporcionado
    dataObjects.forEach(function (obj) {
        // Buscar el índice de fila correspondiente al año y mes del objeto actual
        var rowIndex = currentData.findIndex(function (row) {
            return row[0] === obj.Anio && row[1] === obj.Mes;
        });

        // Si se encuentra la fila correspondiente, actualizar las celdas específicas
        if (rowIndex !== -1) {
            currentData[rowIndex][2] = formatDecimal(obj.DemandaEnergia) ?? ""; // Energía
            currentData[rowIndex][3] = formatDecimal(obj.DemandaHP) ?? ""; // HP
            currentData[rowIndex][4] = formatDecimal(obj.DemandaHFP) ?? ""; // HFP
            currentData[rowIndex][5] = formatDecimal(obj.GeneracionEnergia) ?? ""; // Energía Generación
            currentData[rowIndex][6] = formatDecimal(obj.GeneracionHP) ?? ""; // HP Generación
            currentData[rowIndex][7] = formatDecimal(obj.GeneracionHFP) ?? ""; // HFP Generación
        }
    });

    // Cargar los datos actualizados en la tabla
    hotPes.loadData(currentData);

    // Renderizar la tabla para reflejar los cambios
    hotPes.render();
}

function crearTablaBalancePesimista(inicioan, finan) {

    var data = [];
    var meses = ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Setiembre', 'Octubre', 'Noviembre', 'Diciembre'];
    for (var year = inicioan; year <= finan; year++) {
        meses.forEach(function (str) {
            var row = [year];
            row.push(str);
            for (var i = 0; i < 6; i++) {
                row.push('');
            }
            data.push(row);
        });
    }
    var grilla = document.getElementById('tableBalanceODPe');

    if (typeof hotPes !== 'undefined' && hotPes !== null) {
        hotPes.destroy();
    }

    hotPes = new Handsontable(grilla, {
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
                { label: '', colspan: 1 },
                { label: '', colspan: 1 },
                {
                    label: 'DEMANDA ESTIMADA DE UNIDAD PRODUCTIVA ASOCIADA <br> AL PROYECTO DE GENERACIÓN DISTRIBUIDA', colspan: 3 },
                {
                    label: 'GENERACIÓN ESTIMADA DE PROYECTO <br> DE GENERACIÓN DISTRIBUIDA', colspan: 3 }
            ],
            [
                { label: '', colspan: 1 },
                { label: '', colspan: 1 },
                { label: '', colspan: 1 },
                { label: 'POTENCIA(MW)', colspan: 2 },
                { label: '', colspan: 1 },
                { label: 'POTENCIA(MW)', colspan: 2 },
            ],
            [
                { label: 'A&ntilde;o', colspan: 1 },
                { label: 'Mes', colspan: 1 },
                { label: 'ENERGIA(GWh)', colspan: 1 },
                { label: 'HP(3)', colspan: 1 },
                { label: 'HFP(4)', colspan: 1 },
                { label: 'ENERGIA(GWh)', colspan: 1 },
                { label: 'HP(3)', colspan: 1 },
                { label: 'HFP(4)', colspan: 1 }
            ]
        ],
        columns: [
            { data: 0, readOnly: true },
            { data: 1, readOnly: true },
            {
                data: 2,
                type: 'text'
            },
            {
                data: 3,
                type: 'text'
            },
            {
                data: 4,
                type: 'text'
            },
            {
                data: 5,
                type: 'text'
            },
            {
                data: 6,
                type: 'text'
            },
            {
                data: 7,
                type: 'text'
            }
        ],
        beforeChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
                return generalBeforeChange_4(this, changes, configuracionColumnascgdb);
            }
        },
        afterChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
                generalAfterChange_global(this, changes, configuracionColumnascgdb);
            }
        }
        
    });

    hotPes.render();
}

function getDataCCGDCPesimista() {
    var datosArray = hotPes.getData();
    var datosObjetos = datosArray.map(function (row) {
        return {
            Anio: row[0],
            Mes: row[1],
            DemandaEnergia: row[2],
            DemandaHP: row[3],
            DemandaHFP: row[4],
            GeneracionEnergia: row[5],
            GeneracionHP: row[6],
            GeneracionHFP: row[7]
        };
    });
    console.log(datosObjetos);
    // Filtrar objetos que tienen al menos uno de los campos con datos v�lidos
    var datosFiltrados = datosObjetos.filter(function (obj) {
        return (
            obj.DemandaEnergia !== null && obj.DemandaEnergia !== "" ||
            obj.DemandaHP !== null && obj.DemandaHP !== "" ||
            obj.DemandaHFP !== null && obj.DemandaHFP !== "" ||
            obj.GeneracionEnergia !== null && obj.GeneracionEnergia !== "" ||
            obj.GeneracionHP !== null && obj.GeneracionHP !== "" ||
            obj.GeneracionHFP !== null && obj.GeneracionHFP !== ""
        );
    });
    return datosFiltrados;
}

function exportarTablaExcelCCGDCPesimista() {
    // Definir los encabezados como en el ejemplo proporcionado
    var headers = [
        [
            "",
            "",
            "DEMANDA ESTIMADA DE UNIDAD PRODUCTIVA ASOCIADA AL PROYECTO DE GENERACIÓN DISTRIBUIDA",
            "DEMANDA ESTIMADA DE UNIDAD PRODUCTIVA ASOCIADA AL PROYECTO DE GENERACIÓN DISTRIBUIDA",
            "DEMANDA ESTIMADA DE UNIDAD PRODUCTIVA ASOCIADA AL PROYECTO DE GENERACIÓN DISTRIBUIDA",
            "GENERACIÓN ESTIMADA DE PROYECTO DE GENERACIÓN DISTRIBUIDA",
            "GENERACIÓN ESTIMADA DE PROYECTO DE GENERACIÓN DISTRIBUIDA",
            "GENERACIÓN ESTIMADA DE PROYECTO DE GENERACIÓN DISTRIBUIDA"
        ],
        [
            "AÑO",
            "MES",
            "ENERGIA(GWh)",
            "POTENCIA (MW) HP",
            "POTENCIA (MW) HFP",
            "ENERGIA(GWh)",
            "POTENCIA (MW) HP",
            "POTENCIA (MW) HFP"
        ]
    ];

    // Obtener los datos de la tabla Handsontable (hotOpt)
    var datosArray = hotPes.getData();
    // Concatenar los encabezados con los datos
    var data = headers.concat(datosArray);

    // Crear una hoja de trabajo (worksheet) a partir de los datos
    var ws = XLSX.utils.aoa_to_sheet(data);
    aplicarBordes(ws);
    // Definir las celdas combinadas para los encabezados
    ws["!merges"] = [
        { s: { r: 0, c: 2 }, e: { r: 0, c: 4 } }, // Combina DEMANDA ESTIM. PROY. DE PROD. HIDROG.
        { s: { r: 0, c: 5 }, e: { r: 0, c: 7 } }  // Combina GENERA. ESTIM. DE PROY. DE GENE. ASOC.
    ];

    // Aplicar formato de 4 decimales a partir de la celda (2, 2)
    var startRow = 2; // Fila 3 en t�rminos de Excel
    var startCol = 2; // Columna C en t�rminos de Excel

    for (var R = startRow; R < data.length; R++) {
        for (var C = startCol; C < data[R].length; C++) {
            var cellAddress = XLSX.utils.encode_cell({ r: R, c: C });
            var cell = ws[cellAddress];
            if (cell && typeof cell.v === 'number') {
                cell.z = '0.0000'; // Aplicar el formato de 4 decimales
            }
        }
    }
    // Crear un nuevo libro de trabajo (workbook) y agregar la hoja
    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "CCGDC");

    // Exportar el libro de trabajo a un archivo Excel
    XLSX.writeFile(wb, "GDB_03-EscenarioPesimista.xlsx");
}

function importarExcelCCGDCPesimista() {
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
            updateTableFromExcelBalancePesimista(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}

function updateTableFromExcelBalancePesimista(jsonData) {
    // Los encabezados ocupan las dos primeras filas, los datos comienzan en la tercera fila
    var headers = jsonData.slice(0, 2); // Las dos primeras filas son los headers (no se usan aqu�)
    var data = jsonData.slice(2); // El resto son los datos

    // Obtener el número de filas actuales en la tabla
    const currentRowCount = hotPes.countRows();
    const currentColCount = hotPes.countCols();

    // Limitar el número de filas importadas al número de filas existentes
    if (data.length > currentRowCount) {

        data = data.slice(0, currentRowCount); // Truncar datos al número permitido
    }
    // **1. Limpiar todas las celdas antes de importar nueva data**
    for (let row = 0; row < currentRowCount; row++) {
        for (let col = 0; col < currentColCount; col++) {
            hotPes.setCellMeta(row, col, "valid", true);
            hotPes.setCellMeta(row, col, "style", {
                backgroundColor: "",
            });
        }
    }
    // Guardar los valores originales de la primera columna
    const primeraColumna = hotPes.getData().map(row => row[0]);
    const segColumna = hotPes.getData().map(row => row[1]);


    // Validar y formatear los datos
    const datosValidados = validarYFormatearDatos2(data, configuracionColumnascgdb, mostrarNotificacion, hotPes);

    // Restaurar la primera columna antes de cargar los datos validados
    datosValidados.forEach((row, rowIndex) => {
        row[0] = primeraColumna[rowIndex];
        row[1] = segColumna[rowIndex];
    });

    // Almacenar estilos existentes antes de cargar los nuevos datos
    const stylesBackup = {};
    for (let row = 0; row < hotPes.countRows(); row++) {
        for (let col = 0; col < hotPes.countCols(); col++) {
            const cellMeta = hotPes.getCellMeta(row, col);
            stylesBackup[`${row}-${col}`] = {
                backgroundColor: cellMeta.style?.backgroundColor || "",
                color: cellMeta.style?.color || "",
                fontWeight: cellMeta.style?.fontWeight || "",
                valid: cellMeta.valid,
            };
        }
    }


    // Actualizar los datos de la tabla Handsontable (hotPes) con los datos importados
    hotPes.loadData(data);

    // Restaurar los estilos y validaciones
    for (const key in stylesBackup) {
        const [row, col] = key.split("-").map(Number);
        const { backgroundColor, color, fontWeight, valid } = stylesBackup[key];

        hotPes.setCellMeta(row, col, "style", {
            backgroundColor,
            color,
            fontWeight,
        });
        hotPes.setCellMeta(row, col, "valid", valid);
    }
    // Verificar errores en los datos validados
    const celdasConErrores = [];
    datosValidados.forEach((row, rowIndex) => {
        row.forEach((value, colIndex) => {
            const config = configuracionColumnascgdb[colIndex];
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
                    celdasConErrores.push(`Fila ${rowIndex + 1}, Columna ${colIndex + 1}`);
                    hotPes.setCellMeta(rowIndex, colIndex, "valid", false);
                    hotPes.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "#ffcccc", // Rojo claro
                    });
                } else {
                    // Si es válido, limpiar estilo previo
                    hotPes.setCellMeta(rowIndex, colIndex, "valid", true);
                    hotPes.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "",
                    });
                }
            }
        });
    });

    // Renderizar la tabla para reflejar los cambios
    hotPes.render();
}
