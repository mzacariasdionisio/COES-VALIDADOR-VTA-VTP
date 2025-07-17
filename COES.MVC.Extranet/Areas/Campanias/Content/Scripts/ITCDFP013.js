var hot013 = null;
var listaAnios013 = [];
var listaTypeNumeric013 = [];
var configuracionColumnas013 = {};

$(function () {
    configuracionColumnas013 = generarconfiguracionColumnas013(anioPeriodo + 1, horizonteFin);
    crearTablaITCFP013(anioPeriodo + 1, horizonteFin);

    var formulariosa = document.getElementById('tabla103');
    formulariosa.addEventListener('change', function () {
        cambiosRealizados = true;
    });
});
function generalAfterChange(hotInstance, changes, configuracionColumnas013) {
    changes.forEach(([row, col, oldValue, newValue]) => {
        const config = configuracionColumnas013[col]; // Configuración basada en la columna
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


function generarconfiguracionColumnas013(anioInicia, anioFin) {
    const configuracion = {
        0: { tipo: "texto", largo: 255 },
        1: { tipo: "texto", largo: 255 }
};
    for (let i = 2; i < 2 + (anioFin - anioInicia + 1); i++) {
        configuracion[i] = { tipo: "decimaltruncal", decimales: 4, permitirNegativo: false };
    }
    return configuracion;
}

function crearTablaITCFP013(anioInicia, anioFin) {
    listaAnios013 = [];
    listaTypeNumeric013 = [];
    for (var i = anioInicia; i <= anioFin; i++) {
        listaAnios013.push(i);
        listaTypeNumeric013.push({ type: "text" });
    }

    var data = [];
    for (var i = 0; i <= 50; i++) {
        var row = [];
        row.push("");
        row.push("");
        for (var j = 0; j < listaAnios013.length; j++) {
            row.push("");
        }
        data.push(row);
    }

    var grilla = document.getElementById("tablefp103");

    if (hot013 !== null) {
        hot013.destroy();
    }

    hot013 = new Handsontable(grilla, {
        data: data,
        rowHeaders: true,
        colHeaders: true,
        autoRowSize: true,
        colWidths: 170,
        manualColumnResize: true,
        columnSorting: false,
        filters: false,
        contextMenu: true,
        autoColumnSize: true,
        nestedHeaders: [
            [
                { label: "", colspan: 1 },
                { label: "", colspan: 1 },
                { label: "POTENCIA SOLICITADA (MW)[0]", colspan: listaAnios013.length + 1 }
            ], [
                "NOMBRE DEL CLIENTE",
                "TIPO DE CARGA [1]",
                ...listaAnios013
            ],
        ],
        columns: [
            {},
            {},
            ...listaTypeNumeric013
        ],
        beforeChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {

                return generalBeforeChange_4(this, changes, configuracionColumnas013);
            }
        },
        afterChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
                generalAfterChange(this, changes, configuracionColumnas013);
            }
        }
    });

    hot013.render();

    if (modoModel === "consultar") {
        hot013.updateSettings({
            cells: function (row, col) {
                var cellProperties = {};
                cellProperties.readOnly = true;
                return cellProperties;
            }
        });
    }
}

function getDatos013() {
    
    var datosArray = hot013.getData();
    var headers = hot013.getSettings().nestedHeaders[1].slice(2); // Obtener los headers de los a�os
    var datosFiltrados = datosArray.filter(function (row) {
        // Verificar si al menos una columna tiene un valor no vacío
        return row.some(function (value) {
            return value !== "";
        });
    });

    var datosObjetos = datosFiltrados.map(function (row) {
        var detalleAnios = row.slice(2).map((valor, index) => {
            return { Anio: headers[index], Valor: valor };
        });
        return {
            NombreCliente: row[0], // NOMBRE DEL CLIENTE
            TipoCarga: row[1], // TIPO DE CARGA
            ListItcdf013Det: detalleAnios, // Datos de los a�os
        };
    });
    return datosObjetos;
    
}

function grabarItcdf013() {
   
    if (!validarTablaAntesDeGuardar(hot013)) {
        mostrarNotificacion("No se puede guardar porque hay datos inválidos en la tabla.");
        return; // Detener el guardado si hay errores
    }
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.Itcdfp013DTOs = getDatos013();

        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarItcdfp013',
            data: param,
            
            success: function (result) {
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

function cargarDatos013() {
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetItcdf013',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            
            success: function (response) {
                var hoja013Data = response.responseResult;
                setDatos013(hoja013Data);
                cambiosRealizados = false;
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function setDatos013(lista013) {
    
    const totalFilas = 50; // Total de filas que debe tener la tabla
    const datosArray = [];
    hot013.loadData([]);
    // Rellenar datos provenientes del backend
    lista013.forEach(function (item, index) {
        // Ordenar los detalles por año antes de asignar
        const detalleOrdenado = item.ListItcdf013Det.sort((a, b) => a.Anio - b.Anio);

        // Crear una fila con los datos ordenados
        datosArray[index] = [
            item.NombreCliente || "",  // Columna: Nombre del Cliente
            item.TipoCarga || "",      // Columna: Tipo de Carga
            ...detalleOrdenado.map(detalle => {
                // Formatear el valor a 4 decimales si es numérico
                if (typeof detalle.Valor === "number") {
                    return detalle.Valor.toFixed(4);
                }
                return detalle.Valor || ""; // Si no es número, devolver tal cual
            })
        ];
    });

    // Completar con filas vacías si es necesario
    for (let i = datosArray.length; i < totalFilas; i++) {
        const filaVacia = ["", ""];
        for (let j = 0; j < listaAnios013.length; j++) {
            filaVacia.push(""); // Agrega celdas vacías para cada año
        }
        datosArray.push(filaVacia);
    }

    // Cargar los datos ajustados en la tabla
    hot013.loadData(datosArray);

    // Asegurarse de que se rendericen los cambios
    hot013.render();

    // Si está en modo de consulta, desactivar campos y ocultar botones
    if (modoModel === "consultar") {
        desactivarCamposFormulario('fp103');
        $('#btnGrabarFichaA').hide();
        $('#btnImportarExcel013').hide();
    }
}

function exportarTablaExcel013() {
    var headers = [
        [
            "",
            "",
            "POTENCIA SOLICITADA (MW)[0]",
        ],
        [
            "NOMBRE DEL CLIENTE",
            "TIPO DE CARGA [1]",
            ...listaAnios013,
        ],
    ];

    var datosArray = hot013.getData();

    // Procesar los datos desde la tercera columna
    var datosConcatenados = datosArray.map(row => {
        return row.slice(0, 2).concat(
            row.slice(2).map(celda => celda || "") // Asegúrate de que no haya valores `null` o `undefined`
        );
    });

    var data = headers.concat(datosConcatenados);

    var ws = XLSX.utils.aoa_to_sheet(data);
    aplicarBordes(ws);

    // Definir las celdas combinadas
    ws["!merges"] = [
        { s: { r: 0, c: 2 }, e: { r: 0, c: 2 + listaAnios013.length - 1 } }, // Combina "POTENCIA"
    ];

    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "FP01-3");

    XLSX.writeFile(wb, "ITC_DemEDE_05-FP01_3.xlsx");
}

function importarExcel013() {
    var fileInput = document.createElement("input");
    fileInput.type = "file";
    fileInput.accept = ".xlsx, .xls";

    fileInput.onchange = function (e) {
        var file = e.target.files[0];
        var reader = new FileReader();

        reader.onload = function (e) {
            console.log("Archivo leído con éxito.");

            var data = new Uint8Array(e.target.result);
            var workbook = XLSX.read(data, { type: "array" });

            console.log("Workbook cargado:");

            var firstSheet = workbook.Sheets[workbook.SheetNames[0]];
            console.log("Primera hoja obtenida:");

            var jsonData = XLSX.utils.sheet_to_json(firstSheet, { header: 1 });
            console.log("Datos del archivo (JSON):");

            updateTableFromExcel013(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}

function updateTableFromExcel013(jsonData) {
    console.log("Procesando los datos del Excel...");

    var headers = jsonData.slice(0, 2); // Las dos primeras filas son los headers
    console.log("Encabezados detectados:", headers);

    var data = jsonData.slice(2); // El resto son los datos

    // Obtener el tamaño actual de la tabla
    const currentRows = hot013.countRows(); // Número actual de filas
    const currentCols = hot013.countCols(); // Número actual de columnas (fijas)

    // Validar y formatear los datos importados
    const datosValidados = validarYFormatearDatos2(data, configuracionColumnas013, mostrarNotificacion, hot013);

    // Ajustar tamaño de las filas si los datos importados son mayores
    const numRows = Math.max(currentRows, datosValidados.length);

    hot013.updateSettings({
        minRows: numRows, // Ajustar el número mínimo de filas
        maxRows: numRows, // Fijar el número máximo de filas según los datos importados
    });

    // Rellenar la tabla con los datos importados
    for (let i = 0; i < numRows; i++) {
        if (i < datosValidados.length) {
            // Rellenar las filas con los datos importados
            for (let j = 0; j < currentCols; j++) {
                const value = datosValidados[i][j] !== undefined ? datosValidados[i][j] : null;
                hot013.setDataAtCell(i, j, value);
            }
        } else {
            // Si hay filas adicionales, dejarlas vacías
            for (let j = 0; j < currentCols; j++) {
                hot013.setDataAtCell(i, j, null);
            }
        }
    }

    // Validar los datos y resaltar errores
    const celdasConErrores = [];
    datosValidados.forEach((row, rowIndex) => {
        row.forEach((value, colIndex) => {
            const config = configuracionColumnas013[colIndex];
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
                } else if (config.tipo === "decimaltruncal") {
                    const decimales = 4; // Siempre se validan 4 decimales
                    const partes = value.toString().split(".");

                    if (partes.length === 2) {
                        const decimalesActuales = partes[1].length;

                        if (decimalesActuales < decimales) {
                            // Completar con ceros hasta 4 decimales
                            const nuevoValor = parsedValue.toFixed(decimales);
                            row[colIndex] = nuevoValor;
                            hot013.setDataAtCell(rowIndex, colIndex, nuevoValor);
                        } else if (decimalesActuales > decimales) {
                            esValido = false; // Más de 4 decimales no es válido
                        }
                    } else {
                        // Si no tiene parte decimal
                        const nuevoValor = parsedValue.toFixed(decimales);
                        row[colIndex] = nuevoValor;
                        hot013.setDataAtCell(rowIndex, colIndex, nuevoValor);
                    }
                } else if (config.tipo === "decimal" && value.split(".")[1]?.length > config.decimales) {
                    esValido = false;
                    celdasConErrores.push(`Fila ${rowIndex + 1}, Columna ${colIndex + 1}`);
                } else if (config.tipo === "entero" && !Number.isInteger(parsedValue)) {
                    esValido = false;
                    celdasConErrores.push(`Fila ${rowIndex + 1}, Columna ${colIndex + 1}`);
                }

                if (!esValido) {
                    hot013.setCellMeta(rowIndex, colIndex, "valid", false);
                    hot013.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "#ffcccc", // Rojo claro
                    });
                } else {
                    // Si es válido, limpiar estilo previo
                    hot013.setCellMeta(rowIndex, colIndex, "valid", true);
                    hot013.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "",
                    });
                }
            }
        });
    });


    hot013.render(); // Refrescar la tabla

    if (celdasConErrores.length > 0) {
        mostrarNotificacion(
            `Se importaron datos con errores.<br>Errores detectados en:<br>${celdasConErrores.join("<br>")}`
        );
    } else {
        mostrarNotificacion("Los datos se importaron correctamente.", '#a3e352');
    }
}
