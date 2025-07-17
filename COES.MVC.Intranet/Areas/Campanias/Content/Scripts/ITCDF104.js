var hot104 = null;

const configuracionColumnas104 = {
    1: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    2: { tipo: "decimal", decimales: 4, permitirNegativo: true },
    3: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    4: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    5: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    6: { tipo: "decimal", decimales: 4, permitirNegativo: true },
    7: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    8: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    9: { tipo: "decimal", decimales: 2, permitirNegativo: false },
    10: {tipo: "decimal", decimales: 4, permitirNegativo: true },


};
$(function () {
    crearTablaITCF104(1994, (anioPeriodo - 1), []);


});
 
function crearTablaITCF104(inicioan, finan, data) {
    console.log("Entro f104");

    var grilla = document.getElementById("tablef104");

    if (data.length == 0) {
        for (var year = inicioan; year <= finan; year++) {
            var row = [year, "", "", "", "", "", "", "", "", "", ""];
            data.push(row);
        }
    }

    if (typeof hot104 !== "undefined" && hot104 !== null) {
        hot104.destroy();
    }

    hot104 = new Handsontable(grilla, {
        data: data,
        rowHeaders: true,
        colHeaders: true,
        colWidths: 170,
        manualColumnResize: true,
        columnSorting: false,
        filters: false,
        contextMenu: true,
        fixedColumnsLeft: 1,
        autoColumnSize: true,
        nestedHeaders: [
            [
                { label: "", colspan: 1 },
                { label: "PBI", colspan: 2 },
                { label: "N&Uacute;MERO DE CLIENTES", colspan: 2 },
                { label: "POBLACI&Oacute;N", colspan: 2 },
                { label: "VENTAS ELECTRICIDAD", colspan: 2 },
                { label: "PRECIO ENERG&Iacute;A", colspan: 2 },
            ],
            [
                "A&ntilde;o",
                "Millones de soles </br> del 2007",
                "Tasa de </br> crecimiento (%)",
                "Nro de clientes </br> libres",
                "Nro de clientes </br> regulados",
                "Nro de </br> habitantes",
                "Tasa de </br> crecimiento (%)",
                "(Millones de soles) </br> Clientes regulados",
                "(MWh) Clientes </br> regulados",
                "(US$/MWh)",
                "Tasa de crecimiento (%)",
            ],
        ],
        columns: [
            { readOnly: true }, // Año
            { type: "text" }, // Millones de soles
            { type: "text" }, // Tasa de crecimiento %
            { type: "text" }, // Nº clientes libres
            { type: "text" }, // Nº clientes regulados
            { type: "text" }, // Nº habitantes
            { type: "text"}, // Tasa de crecimiento %
            { type: "text" }, // Millones clientes
            { type: "text" }, // Clientes regulados
            { type: "text"}, // US$/MWh
            { type: "text"}, // Tasa de crecimiento %
        ],
        beforeChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
                return generalBeforeChange_4(this, changes, configuracionColumnas104);
            }
        },
        afterChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
                generalAfterChange(this, changes, configuracionColumnas104);
            }
        },
    });
     

    hot104.render();

    //if (modoModel == "consultar") {
    //    $("#btnGrabarFicha104").hide();
    //    hot104.updateSettings({
    //        cells: function (row, col) {
    //            var cellProperties = {};
    //            cellProperties.readOnly = true;
    //            return cellProperties;
    //        }
    //    });
    //}
    if (modoModel == "consultar") {
        desactivarCamposFormulario('f104');
        $('#btnGrabarFicha104').hide();
    }
}

function grabarItcdf104() {
    if (!validarTablaAntesDeGuardar(hot104)) {
        mostrarNotificacion("No se puede guardar porque hay datos inválidos en la tabla.");
        return; // Detener el guardado si hay errores
    }
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.Itcdf104DTOs = getDatos104();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarItcdf104',
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

function getDatos104() {
    var datosArray = hot104.getData();

    // Filtrar filas donde todos los valores (excepto el año) sean vacíos
    var datosFiltrados = datosArray.filter(row =>
        row.slice(1).some(value => value !== null && value !== "")
    );

    // Convertir valores vacíos a `null` antes de enviarlos al backend
    var datosObjetos = datosFiltrados.map(row => ({
        Anio: row[0], // Mantener el año
        MillonesolesPbi: row[1] === "" ? null : row[1],
        TasaCrecimientoPbi: row[2] === "" ? null : row[2],
        NroClientesLibres: row[3] === "" ? null : row[3],
        NroClientesRegulados: row[4] === "" ? null : row[4],
        NroHabitantes: row[5] === "" ? null : row[5],
        TasaCrecimientoPoblacion: row[6] === "" ? null : row[6],
        MillonesClientesRegulados: row[7] === "" ? null : row[7],
        ClientesReguladoSelectr: row[8] === "" ? null : row[8],
        Usmwh: row[9] === "" ? null : row[9],
        TasaCrecimientoEnergia: row[10] === "" ? null : row[10],
    }));

    console.log(datosObjetos); // Verificar en consola antes de enviar

    return datosObjetos;
}

function cargarDatos104() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetItcdf104',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            
            success: function (response) {
                console.log(response);
                var hoja104Data = response.responseResult;
                setDatos104(hoja104Data);
                cambiosRealizados = false;
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function formatearValor(valor, config) {
    if (config && valor !== null && valor !== "" && !isNaN(valor)) {
        let parsedValue = parseFloat(valor);

        if (config.tipo === "decimal") {
            return parsedValue.toFixed(config.decimales);
        } else if (config.tipo === "entero") {
            return Math.round(parsedValue);
        }
    }
    return valor; // Retorna el valor original si no es numérico o no requiere formateo
}

function setDatos104(lista104) {
    console.log("Cargando datos en la tabla ITCF104");

    // Obtener los datos actuales de la tabla para conservar los años
    var datosArray = hot104.getData();

    // Convertir la lista del backend en un diccionario basado en el año
    var datosPorAnio = {};
    lista104.forEach(item => {
        datosPorAnio[item.Anio] = {
            1: item.MillonesolesPbi ?? "",
            2: item.TasaCrecimientoPbi ?? "",
            3: item.NroClientesLibres ?? "",
            4: item.NroClientesRegulados ?? "",
            5: item.NroHabitantes ?? "",
            6: item.TasaCrecimientoPoblacion ?? "",
            7: item.MillonesClientesRegulados ?? "",
            8: item.ClientesReguladoSelectr ?? "",
            9: item.Usmwh ?? "",
            10: item.TasaCrecimientoEnergia ?? "",
        };
    });

    // Recorrer la tabla actual y sincronizar con los datos del backend
    for (let i = 0; i < datosArray.length; i++) {
        let anioTabla = datosArray[i][0]; // Año en la primera columna

        if (datosPorAnio[anioTabla]) {
            // Si hay datos para este año, asignarlos y formatearlos
            for (let colIndex = 1; colIndex <= 10; colIndex++) {
                datosArray[i][colIndex] = formatearValor(datosPorAnio[anioTabla][colIndex], configuracionColumnas104[colIndex]);
            }
        } else {
            // Si no hay datos para este año, dejar la fila vacía (excepto el año)
            datosArray[i] = [anioTabla, "", "", "", "", "", "", "", "", "", ""];
        }
    }

    // Aplicar los datos sincronizados en la tabla sin modificar la estructura
    hot104.loadData(datosArray);
}

function exportarTablaExcel104() {
    var headers = [
        [
            "",
            "PBI",
            "PBI",
            "NUMERO DE CLIENTES",
            "NUMERO DE CLIENTES",
            "POBLACION",
            "POBLACION",
            "VENTAS ELECTRICIDAD",
            "VENTAS ELECTRICIDAD",
            "PRECIO ENERGIA",
            "PRECIO ENERGIA",
        ],
        [
            "AÑO",
            "Millones de soles del 2007",
            "Tasa de crecimiento (%)",
            "Nro de clientes libres",
            "Nro de clientes regulados",
            "Nro de habitantes",
            "Tasa de crecimiento (%)",
            "(Millones de soles) Clientes regulados",
            "(MWh) Clientes regulados",
            "(US$/MWh)",
            "Tasa de crecimiento (%)",
        ],
    ];

    var datosArray = hot104.getData();

    // Formatear los datos basados en la configuración de columnas
    var formattedData = datosArray.map(function (row) {
        return row.map(function (cell, colIndex) {
            const decimalColumns = [1,2,3,4,5, 6,7,8, 10];// Columnas con 4 decimales
            const twoDecimalColumns = [9]; // Columnas con 2 decimales

            if (cell === null || cell === "") return ""; // Mantener celdas vacías
            if (decimalColumns.includes(colIndex)) {
                return parseFloat(cell).toFixed(4); // 4 decimales
            } else if (twoDecimalColumns.includes(colIndex)) {
                return parseFloat(cell).toFixed(2); // 2 decimales
            } else if (!isNaN(cell)) {
                return parseFloat(cell).toString(); // Convertir números a texto
            }
            return cell; // Mantener valores no numéricos
        });
    });

    // Combinar encabezados y datos formateados
    var data = headers.concat(formattedData);

    var ws = XLSX.utils.aoa_to_sheet(data);
    aplicarBordes(ws);
    // Definir las celdas combinadas
    ws["!merges"] = [
        { s: { r: 0, c: 1 }, e: { r: 0, c: 2 } }, // Combina PBI
        { s: { r: 0, c: 3 }, e: { r: 0, c: 4 } }, // Combina NÚMERO DE CLIENTES
        { s: { r: 0, c: 5 }, e: { r: 0, c: 6 } }, // Combina POBLACIÓN
        { s: { r: 0, c: 7 }, e: { r: 0, c: 8 } }, // Combina VENTAS ELECTRICIDAD
        { s: { r: 0, c: 9 }, e: { r: 0, c: 10 } }, // Combina PRECIO ENERGÍA
    ];

    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "104");

    // Exportar el archivo Excel
    XLSX.writeFile(wb, "ITC_DemEDE_01-F104.xlsx");
}

function importarExcel104() {
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

            updateTableFromExcel104(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}
 
function updateTableFromExcel104(jsonData) {
    const headers = jsonData.slice(0, 2); // Encabezados opcionales para validación
    let data = jsonData.slice(2); // Datos, excluyendo las primeras dos filas de encabezados

    // Obtener el número de filas actuales en la tabla
    const currentRowCount = hot104.countRows();

    // Limitar el número de filas importadas al número de filas existentes
    if (data.length > currentRowCount) {
        //mostrarNotificacion(
        //    `Se han detectado más filas de las permitidas en los datos importados. Solo se usarán las primeras ${currentRowCount} filas.`,
        //    '#FFCCCC'
        //);
        data = data.slice(0, currentRowCount); // Truncar datos al número permitido
    }

    // Guardar los valores originales de la primera columna
    const primeraColumna = hot104.getData().map(row => row[0]);

    // Validar y formatear los datos
    const datosValidados = validarYFormatearDatos2(data, configuracionColumnas104, mostrarNotificacion, hot104);

    // Restaurar la primera columna antes de cargar los datos validados
    datosValidados.forEach((row, rowIndex) => {
        row[0] = primeraColumna[rowIndex];
    });

    // Almacenar estilos existentes antes de cargar los nuevos datos
    const stylesBackup = {};
    for (let row = 0; row < hot104.countRows(); row++) {
        for (let col = 0; col < hot104.countCols(); col++) {
            const cellMeta = hot104.getCellMeta(row, col);
            stylesBackup[`${row}-${col}`] = {
                backgroundColor: cellMeta.style?.backgroundColor || "",
                color: cellMeta.style?.color || "",
                fontWeight: cellMeta.style?.fontWeight || "",
                valid: cellMeta.valid,
            };
        }
    }

    // Cargar los datos validados
    hot104.loadData(datosValidados);

    // Restaurar los estilos y validaciones
    for (const key in stylesBackup) {
        const [row, col] = key.split("-").map(Number);
        const { backgroundColor, color, fontWeight, valid } = stylesBackup[key];

        hot104.setCellMeta(row, col, "style", {
            backgroundColor,
            color,
            fontWeight,
        });
        hot104.setCellMeta(row, col, "valid", valid);
    }

    // Refrescar la tabla para aplicar los estilos restaurados
    hot104.render();

    // Verificar errores en los datos validados
    const celdasConErrores = [];
    datosValidados.forEach((row, rowIndex) => {
        row.forEach((value, colIndex) => {
            const config = configuracionColumnas104[colIndex];
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
                    hot104.setCellMeta(rowIndex, colIndex, "valid", false);
                    hot104.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "#ffcccc", // Rojo claro
                    });
                } else {
                    // Si es válido, limpiar estilo previo
                    hot104.setCellMeta(rowIndex, colIndex, "valid", true);
                    hot104.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "",
                    });
                }
            }
        });
    });

    hot104.render(); // Refrescar tabla

    // Mostrar notificaciones según los errores detectados
    if (celdasConErrores.length > 0) {
        mostrarNotificacion(
            `Se importaron datos con errores.<br>Errores detectados en:<br>${celdasConErrores.join("<br>")}`
        );
    } else {
        mostrarNotificacion("Los datos se importaron correctamente.", '#a3e352');
    }
}

function reloadDataWithoutAffectingStyles(hotInstance, newData) {
    // Almacenar los estilos actuales de las celdas
    const stylesBackup = {};

    // Iterar sobre todas las celdas y guardar sus estilos y validaciones
    for (let row = 0; row < hotInstance.countRows(); row++) {
        for (let col = 0; col < hotInstance.countCols(); col++) {
            const cellMeta = hotInstance.getCellMeta(row, col);
            stylesBackup[`${row}-${col}`] = {
                backgroundColor: cellMeta.style?.backgroundColor || "",
                color: cellMeta.style?.color || "",
                fontWeight: cellMeta.style?.fontWeight || "",
                valid: cellMeta.valid,
            };
        }
    }

    // Cargar los nuevos datos
    hotInstance.loadData(newData);

    // Restaurar los estilos y validaciones
    for (const key in stylesBackup) {
        const [row, col] = key.split("-").map(Number);
        const { backgroundColor, color, fontWeight, valid } = stylesBackup[key];

        hotInstance.setCellMeta(row, col, "style", {
            backgroundColor,
            color,
            fontWeight,
        });
        hotInstance.setCellMeta(row, col, "valid", valid);
    }

    // Refrescar la tabla para aplicar los estilos restaurados
    hotInstance.render();
}
