var hot3 = null;
let isUpdating = false;
$(function () {
    crearTablaBalanceOD(horizonteInicio, horizonteFin);

});

function crearTablaBalanceOD(inicioan, finan) {
    var data = [];
    var meses = ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Setiembre', 'Octubre', 'Noviembre', 'Diciembre'];

    // Generar datos de la tabla
    for (var year = inicioan; year <= finan; year++) {
        meses.forEach(function (str) {
            var row = [year];
            row.push(str);
            for (var i = 0; i < 6; i++) {
                row.push('');
            }
            data.push(row);
        });
        // TOTAL A�O
        var row = ['TOTAL ' + year];
        for (var i = 0; i < 7; i++) {
            row.push('');
        }
        data.push(row);
    }

    var grilla = document.getElementById('tableBalanceOD');

    if (typeof hot3 !== 'undefined' && hot3 !== null) {
        hot3.destroy();
    }

    // Inicializar Handsontable
    hot3 = new Handsontable(grilla, {
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
                { label: 'DEMANDA ESTIM. PROY. DE PROD. HIDROG.', colspan: 3 },
                { label: 'GENERA. ESTIM. DE PROY. DE GENE. ASOC.', colspan: 3 }
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
                { label: 'HP', colspan: 1 },
                { label: 'HFP', colspan: 1 },
                { label: 'ENERGIA(GWh)', colspan: 1 },
                { label: 'HP', colspan: 1 },
                { label: 'HFP', colspan: 1 }
            ]
        ],
        columns: [
            { data: 0, readOnly: true },
            { data: 1, readOnly: true },
            { data: 2, type: 'numeric', format: '0.0000' },
            { data: 3, type: 'numeric', format: '0.0000' },
            { data: 4, type: 'numeric', format: '0.0000' },
            { data: 5, type: 'numeric', format: '0.0000' },
            { data: 6, type: 'numeric', format: '0.0000' },
            { data: 7, type: 'numeric', format: '0.0000' }
        ],
        cells: function (row, col) {
            var cellProperties = {};
            var rowData = this.instance.getDataAtRow(row);
            // Hacer la fila TOTAL de solo lectura
            if (rowData[0] && rowData[0].toString().includes("TOTAL")) {
                cellProperties.readOnly = true;
            }
            return cellProperties;
        },
        beforeChange: function (changes, source) {
            console.log("beforeChange event triggered");
            generalBeforeChange(this, changes, source);
        },
        afterChange: function (changes, source) {
            console.log("afterChange event triggered", source);
            if (source === "loadData") {
                calcularTotalesAnuales();
            }
            if (changes) {
                calcularTotalesAnuales();
            }
        }
    });

    // Calcular los totales iniciales
    //calcularTotalesAnuales();
    hot3.render();
}



function calcularTotalesAnuales() {
    // Verifica que la tabla Handsontable (hot3) esté inicializada
    if (typeof hot3 === 'undefined' || hot3 === null) {
        console.error("La tabla Handsontable no está inicializada.");
        return;
    }

    if (isUpdating) {
        console.log("Evitando recursión en calcularTotalesAnuales");
        return;
    }

    isUpdating = true;
    try {
        // Obtener todos los datos actuales de la tabla
        var currentData = hot3.getData();

        // Objeto para almacenar las sumas y máximos por año
        var totalesAnuales = {};

        // Primera pasada: Calcular totales por año
        currentData.forEach(function (row) {
            var anio = row[0];
            var mes = row[1];

            // Si la fila es un TOTAL, saltarla
            if (anio && anio.toString().includes("TOTAL")) return;

            // Inicializar el total para el año si no existe
            if (!totalesAnuales[anio]) {
                totalesAnuales[anio] = {
                    sum: [0, 0], // Para columnas 4 y 7
                    max: [-Infinity, -Infinity, -Infinity, -Infinity], // Para columnas 2, 3, 5, 6
                };
            }

            // Actualizar los valores
            for (var i = 2; i <= 7; i++) {
                var valor = parseFloat(row[i]) || 0;
                if (i === 3 || i === 4 || i === 6 || i === 7) {
                    // Actualizar máximo
                    let maxIndex = i === 3 ? 0 : i === 4 ? 1 : i === 6 ? 2 : 3;
                    totalesAnuales[anio].max[maxIndex] = Math.max(totalesAnuales[anio].max[maxIndex], valor);
                } else {
                    // Actualizar suma
                    let sumIndex = i === 2 ? 0 : 1;
                    totalesAnuales[anio].sum[sumIndex] += valor;
                }
            }
        });

        // Segunda pasada: Actualizar o insertar las filas TOTAL
        currentData.forEach(function (row) {
            var anio = row[0];

            // Identificar filas TOTAL
            if (anio && anio.toString().includes("TOTAL")) {
                var anioBase = anio.replace("TOTAL ", "");
                if (totalesAnuales[anioBase]) {
                    // Asignar valores a la fila TOTAL
                    row[2] = parseFloat(totalesAnuales[anioBase].sum[0].toFixed(4)); 
                    row[3] = parseFloat(totalesAnuales[anioBase].max[0].toFixed(4)); 
                    row[4] = parseFloat(totalesAnuales[anioBase].max[1].toFixed(4)); 
                    row[5] = parseFloat(totalesAnuales[anioBase].sum[1].toFixed(4));
                    row[6] = parseFloat(totalesAnuales[anioBase].max[2].toFixed(4)); 
                    row[7] = parseFloat(totalesAnuales[anioBase].max[3].toFixed(4)); 
                }
            }
        });

        // Actualizar la tabla sin recargarla completamente
        hot3.loadData(currentData);
    } catch (error) {
        console.error("Error en calcularTotalesAnuales:", error);
    } finally {
        // Desactivar bandera
        isUpdating = false;
    }

    // Renderizar la tabla
    hot3.render();
}

function getDataCH2VC() {
    var datosArray = hot3.getData();
    var datosObjetos = datosArray.map(function (row) {
        var isTotalRow = row[0] && row[0].toString().includes("TOTAL");

        return {
            Anio: row[0],
            Mes: row[1],
            DemandaEnergia: isTotalRow ? "" : row[2],
            DemandaHP: isTotalRow ? "" : row[3],
            DemandaHFP: isTotalRow ? "" : row[4],
            GeneracionEnergia: isTotalRow ? "" : row[5],
            GeneracionHP: isTotalRow ? "" : row[6],
            GeneracionHFP: isTotalRow ? "" : row[7]
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


function grabarCH2VC() {
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.CH2VCDTOs = getDataCH2VC(); // Obtiene los datos filtrados
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarCH2VC',
            data: param,
            success: function (result) {
                console.log(result);
                if (result) {
                    mostrarMensaje('mensajeFicha', 'exito', 'Los datos se grabaron correctamente.');
                    cambiosRealizados = false;
                } else {
                    mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function cargarDatosC() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetCuestionarioH2C',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),

            success: function (response) {
                console.log(response);
                var hojaCRes = response.responseResult;
                setDataCH2VC(hojaCRes);
                cambiosRealizados = false;
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function setDataCH2VC(dataObjects) {
    if (typeof hot3 === 'undefined' || hot3 === null) {
        console.error("La tabla Handsontable no está inicializada.");
        return;
    }

    var currentData = hot3.getData();
    dataObjects.forEach(function (obj) {
        var rowIndex = currentData.findIndex(function (row) {
            return row[0] == obj.Anio && row[1] === obj.Mes;
        });

        if (rowIndex !== -1) {
            currentData[rowIndex][2] = obj.DemandaEnergia ?? '';   // Energía
            currentData[rowIndex][3] = obj.DemandaHP ?? '';        // HP
            currentData[rowIndex][4] = obj.DemandaHFP ?? '';       // HFP
            currentData[rowIndex][5] = obj.GeneracionEnergia ?? '';// Energía Generación
            currentData[rowIndex][6] = obj.GeneracionHP ?? '';     // HP Generación
            currentData[rowIndex][7] = obj.GeneracionHFP ?? '';    // HFP Generación
        }
    });

    hot3.loadData(currentData);
    calcularTotalesAnuales(); // Calcular totales después de cargar los datos
    hot3.render();

    if (modoModel === "consultar") {
        desactivarCamposFormulario('H2VC');
        $('#btnGrabarH2VC').hide();
    }
}

function exportarTablaExcelCH2VC() {
    // Definir los encabezados como en el ejemplo proporcionado
    var headers = [
        [
            "",
            "",
            "DEMANDA ESTIM. PROY. DE PROD. HIDROG.",
            "DEMANDA ESTIM. PROY. DE PROD. HIDROG.",
            "DEMANDA ESTIM. PROY. DE PROD. HIDROG.",
            "GENERA. ESTIM. DE PROY. DE GENE. ASOC.",
            "GENERA. ESTIM. DE PROY. DE GENE. ASOC.",
            "GENERA. ESTIM. DE PROY. DE GENE. ASOC."
        ],
        [
            "AÑO",
            "MES",
            "ENERGIA(GWh)",
            "HP",
            "HFP",
            "ENERGIA(GWh)",
            "HP",
            "HFP"
        ]
    ];

    // Obtener los datos de la tabla Handsontable (hot3)
    var datosArray = hot3.getData();
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
    XLSX.utils.book_append_sheet(wb, ws, "CH2VC");

    // Exportar el libro de trabajo a un archivo Excel
    XLSX.writeFile(wb, "HVC_03-EscenarioBase.xlsx");
}


function importarExcelCH2VC() {
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
            updateTableFromExcelBalanceOD(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}

function updateTableFromExcelBalanceOD(jsonData) {
    // Los encabezados ocupan las dos primeras filas, los datos comienzan en la tercera fila
    var headers = jsonData.slice(0, 2); // Las dos primeras filas son los headers (no se usan aqu�)
    var data = jsonData.slice(2); // El resto son los datos

    // Actualizar los datos de la tabla Handsontable (hot3) con los datos importados
    hot3.loadData(data);

    // Renderizar la tabla para reflejar los cambios
    hot3.render();
}
