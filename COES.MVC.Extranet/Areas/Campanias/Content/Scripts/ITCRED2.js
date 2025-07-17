var hotred2 = null;
const configuracionColumnasred2 = {
    0: { tipo: "texto", largo: 120 },
    1: { tipo: "texto", largo: 60 },
    2: { tipo: "texto", largo: 60 },
    3: { tipo: "entero", permitirNegativo: false },
    4: { tipo: "entero", permitirNegativo: false },
    5: { tipo: "texto", largo: 60 },
    6: { tipo: "decimal", decimales: 4, permitirNegativo: false },

     
};


$(function () {
    crearTablaITCRED2([]);
    var formulariob = document.getElementById('tablered2');
    formulariob.addEventListener('change', function () {
        cambiosRealizados = true;
        console.log(cambiosRealizados);
    });
});
// AfterChange para validar celdas vacías y quitar estilos


function generalAfterChange(hotInstance, changes, configuracionColumnasred2) {
    changes.forEach(([row, col, oldValue, newValue]) => {
        const config = configuracionColumnasred2[col]; // Configuración basada en la columna
        const cellMeta = hotInstance.getCellMeta(row, col);

        // Si la celda está vacía, se considera válida
        if (newValue === null || newValue === "") {
            cellMeta.valid = true;
            cellMeta.style = { backgroundColor: "" }; // Quitar color de fondo
            cambiosRealizados = true;
        }
    });

    hotInstance.render(); // Refrescar la tabla para aplicar estilos
}


function crearTablaITCRED2(data) {
    var grilla = document.getElementById("detalleFormatoRed2");

    // Si no hay datos, inicializa con 50 filas vacías
    if (data.length === 0) {
        for (var i = 0; i < 50; i++) {
            var row = new Array(7).fill("");
            data.push(row);
        }
    }

    

    if (typeof hotred2 !== "undefined" && hotred2 !== null) {
        hotred2.destroy();
    }

    hotred2 = new Handsontable(grilla, {
        data: data,
        rowHeaders: true,
        autoRowSize: true,
        colHeaders: true,
        colWidths: 170,
        manualColumnResize: true,
        columnSorting: false,
        filters: false,
        contextMenu: true,
        autoColumnSize: true,
        nestedHeaders: [
            [
                "LINEA",
                "BARRA[E]",
                "BARRA[R]",
                "N° TERNAS",
                "TRAMO",
                "ELECTRODUCTO",
                "LONG(km)"
            ],
        ],
        columns: [
           { type: "text"},
           { type: "text"},
           { type: "text"},
           { type: "text"},
           { type: "text"}, 
           { type: "text"},
           { type: "text"}, 
        ],
        beforeChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
              
                return generalBeforeChange_4(this, changes, configuracionColumnasred2);
            }
        }
        ,
        afterChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
                generalAfterChange(this, changes, configuracionColumnasred2);
            }
        }
    
    });
    hotred2.render();
}


function getDatosRED2() {
    var datosArray = hotred2.getData();
    var datosObjetos = datosArray.map(function (row) {
        return {
            Linea: row[0],          // LINEA
            BarraE: row[1],        // BARRA[E]
            BarraR: row[2],        // BARRA[R]
            Nternas: row[3],      // Nº TERNAS
            Tramo: row[4],          // TRAMO
            Electroducto: row[5],   // ELECTRODUCTO
            Longitud: row[6]         // LONG(km)
        };
    }).filter(function (row) {
        // Verificar si al menos una columna tiene un valor no vacío
        return Object.values(row).some(function (value) {
            return value !== "";
        });
    });
    return datosObjetos;
}


function cargarDatosRed2() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetItcRed2',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            
            success: function (response) {
                console.log(response);
                var hojared2Data = response.responseResult;
                setDatosRed2(hojared2Data);
                cambiosRealizados = false;
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function setDatosRed2(listared2) {
    if (modoModel == "consultar") {
        desactivarCamposFormulario('Fichared2');
        $('#btnGrabarFichared2').hide();
        $('#btnImportarExcelRED2').hide();
    }
    var datosArray = [];

    // Mapea los datos de la lista a la estructura de la tabla
    listared2.forEach(function (item) {
        var row = [
            item.Linea !== null && item.Linea !== undefined ? item.Linea : "",            // LINEA
            item.BarraE !== null && item.BarraE !== undefined ? item.BarraE : "",        // BARRA[E]
            item.BarraR !== null && item.BarraR !== undefined ? item.BarraR : "",        // BARRA[R]
            item.Nternas !== null && item.Nternas !== undefined ? item.Nternas : "",    // Nº TERNAS
            item.Tramo !== null && item.Tramo !== undefined ? item.Tramo : "",          // TRAMO
            item.Electroducto !== null && item.Electroducto !== undefined ? item.Electroducto : "", // ELECTRODUCTO
            item.Longitud !== null && item.Longitud !== undefined ? item.Longitud : ""  // LONG(km)
        ];
        datosArray.push(row);
    });

    // Completa con filas vacías hasta llegar a 50
    while (datosArray.length < 50) {
        var emptyRow = new Array(7).fill("");
        datosArray.push(emptyRow);
    }
    datosArray = formatearDatosGlobal(datosArray, configuracionColumnasred2, mostrarNotificacion);
    // Crea la tabla con los nuevos datos
    crearTablaITCRED2(datosArray);
}

// Función para validar y formatear los datos


// Función para validar la tabla antes de guardar

// Función para actualizar la tabla desde Excel
function updateTableFromExcelred2(jsonData) {
    // El encabezado se ignora en esta tabla, tomamos solo los datos
    var data = jsonData.slice(1); // Datos ignorando encabezados

    // Validar y formatear los datos importados
    const datosValidados = validarYFormatearDatos2(data, configuracionColumnasred2, mostrarNotificacion, hotred2);
    while (datosValidados.length < 50) {
        datosValidados.push(new Array(7).fill("")); // Agregar filas vacías
    }
    // Cargar los datos validados en la tabla, incluyendo errores
    hotred2.loadData(datosValidados);

    // Verificar errores después de la validación
    const celdasConErrores = [];
    datosValidados.forEach((row, rowIndex) => {
        row.forEach((value, colIndex) => {
            const config = configuracionColumnasred2[colIndex];
            if (config && value !== null && value !== "" && value !== undefined) {
                let parsedValue = parseFloat(value);
                let esValido = true;

                if (config.tipo === "texto") {
                    // Validar longitud del texto
                    if (value.length > config.largo) {
                        esValido = false;
                        celdasConErrores.push(`Fila ${rowIndex + 1}, Columna ${colIndex + 1} (Excede longitud máxima)`);
                    }
                } else if (isNaN(parsedValue)) {
                    esValido = false;
                    celdasConErrores.push(`Fila ${rowIndex + 1}, Columna ${colIndex + 1}`);
                } else if (!config.permitirNegativo && parsedValue < 0) {
                    esValido = false;
                    celdasConErrores.push(`Fila ${rowIndex + 1}, Columna ${colIndex + 1}`);
                } else if (config.tipo === "decimal" && value.split(".")[1]?.length > config.decimales) {
                    esValido = false;
                    celdasConErrores.push(`Fila ${rowIndex + 1}, Columna ${colIndex + 1}`);
                } else if (config.tipo === "entero" && !Number.isInteger(parsedValue)) {
                    esValido = false;
                    celdasConErrores.push(`Fila ${rowIndex + 1}, Columna ${colIndex + 1}`);
                }

                if (!esValido) {
                    hotred2.setCellMeta(rowIndex, colIndex, "valid", false);
                    hotred2.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "#ffcccc", // Rojo claro
                    });
                } else {
                    // Si es válido, limpiar estilo previo
                    hotred2.setCellMeta(rowIndex, colIndex, "valid", true);
                    hotred2.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "",
                    });
                }
            }
        });
    });

    hotred2.render(); // Refrescar tabla

    if (celdasConErrores.length > 0) {
        mostrarNotificacion(
            `Se importaron datos con errores.<br>Errores detectados en:<br>${celdasConErrores.join("<br>")}`
        );
    } else {
        mostrarNotificacion("Los datos se importaron correctamente.", '#a3e352');
    }
}

 
// Función para importar datos desde Excel
function importarExcelred2() {
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

            // Validar y cargar datos desde Excel
            updateTableFromExcelred2(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}

// Función para guardar datos
function grabarRed2() {
    if (!validarTablaAntesDeGuardar(hotred2)) {
        mostrarNotificacion("No se puede guardar porque hay datos inválidos en la tabla.");
        return; // Detener el guardado si hay errores
    }

    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();

    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.ListaItcRed2DTO = getDatosRED2();

        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarItcred2',
            data: param,

            success: function (result) {
                console.log(result);
                if (result == 1) {
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
    } else {
        mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
    }
}



function exportarTablaExcelred2() {
    var headers = [
        [
            "LINEA",
            "BARRA[E]",
            "BARRA[R]",
            "Nº TERNAS",
            "TRAMO",
            "ELECTRODUCTO",
            "LONG(km)"
        ],
    ];

    var datosArray = hotred2.getData();
    var data = headers.concat(datosArray);

    var ws = XLSX.utils.aoa_to_sheet(data);
    aplicarBordes(ws);
    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "RED2");

    XLSX.writeFile(wb, "ITC_SEyP_04-RED2.xlsx");
}
