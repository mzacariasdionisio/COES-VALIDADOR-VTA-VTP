var hot121 = null;
var listaAnios121 = [];
var listaTypeNumeric121 = [];
var configuracionColumnas121 = {};

$(function () {
    configuracionColumnas121 = generarconfiguracionColumnas121(anioPeriodo - 1, horizonteFin + 10);

    crearTablaITCF121((anioPeriodo - 1), (horizonteFin + 10), []);

    var formulariosa = document.getElementById('tabla121');
    formulariosa.addEventListener('change', function () {
        cambiosRealizados = true;
    });
});

function generarconfiguracionColumnas121(anioInicia, anioFin) {

    const configuracion = {};

    // Configuración específica para las columnas 0 y 3
    configuracion[0] = { tipo: "entero", permitirNegativo: false };
    configuracion[3] = { tipo: "decimal", decimales: 4, permitirNegativo: false };

    // Configuración dinámica para las columnas a partir de la 6
    for (let i = 6; i <= 6 + (anioFin - anioInicia); i++) {
        configuracion[i] = { tipo: "decimal", decimales: 4, permitirNegativo: false };
    }

    return configuracion;
    
}

function isNumeric(value) {
    var parsed = parseFloat(value);
    return !isNaN(parsed) && isFinite(parsed);
}
function currencyRenderer121(instance, td, row, col, prop, value, cellProperties) {
    let rows = instance.countRows() - 4; // Últimas 4 filas son de totales

    if (row >= rows) {
        let total = 0;

        // Calcular totales según las reglas específicas
        for (let index = 0; index < rows; index++) {
            let cellValue = parseFloat(instance.getDataAtCell(index, col)) || 0;

            if (row === rows && cellValue >= 138) {
                total += cellValue;
            } else if (row === rows + 1 && cellValue >= 30 && cellValue < 138) {
                total += cellValue;
            } else if (row === rows + 2 && cellValue > 1 && cellValue < 30) {
                total += cellValue;
            } else if (row === rows + 3) {
                total += cellValue;
            }
        }

        td.innerHTML = total.toFixed(4); // Mostrar el total calculado
        td.style.backgroundColor = ""; // Quitar estilos en las filas de totales
    } else {
        // Validar si la celda está vacía
        if (value === null || value === "") {
            td.innerHTML = ""; // Dejar vacío si no hay valor
            td.style.backgroundColor = ""; // No resaltar
            cellProperties.valid = true; // Marcar como válida
        } else if (!isNumeric(value)) {
            // Si no es numérico, marcar la celda como incorrecta
            td.innerHTML = value || "";
            td.style.backgroundColor = "#ffcccc"; // Fondo rojo claro
            cellProperties.valid = false; // Marcar como inválida
        } else {
            // Si es numérico, mostrar el valor y limpiar estilos
            td.innerHTML = parseFloat(value).toFixed(4);
            td.style.backgroundColor = ""; // Quitar estilos
            cellProperties.valid = true; // Marcar como válida
        }
    }
}


function crearTablaITCF121(anioInicia, anioFin, data) {

    var grilla = document.getElementById("tablef121");

    // Configurar años dinámicamente.
    listaAnios121.length = 0; // Reiniciar años previamente configurados.
    listaTypeNumeric121.length = 0;

    for (var i = anioInicia; i <= anioFin; i++) {
        listaAnios121.push(i);
        listaTypeNumeric121.push({ type: "numeric", renderer: currencyRenderer121 });
    }

    // Crear estructura base de la tabla con filas vacías.
    const filasBase = 50; // Número inicial de filas.
    for (var i = 0; i < filasBase; i++) {
        const filaVacia = new Array(6 + listaAnios121.length).fill("");
        data.push(filaVacia);
    }

    // Agregar filas de totales con concatenación de las primeras 6 celdas.
    const filasTotales = ["TOTAL MAT", "TOTAL AT", "TOTAL MT", "TOTAL"];
    filasTotales.forEach(label => {
        const totalRow = new Array(6 + listaAnios121.length).fill("");
        for (let i = 0; i < 6; i++) {
            totalRow[i] = label; // Concatenar el nombre en las primeras 6 celdas.
        }
        data.push(totalRow);
    });

    // Destruir la tabla anterior, si existe.
    if (hot121) {
        hot121.destroy();
    }

    // Crear la tabla.
    hot121 = new Handsontable(grilla, {
        data: data,
        rowHeaders: true,
        colHeaders: true,
        colWidths: 170,
        manualColumnResize: true,
        columnSorting: false,
        filters: false,
        contextMenu: false,
        autoColumnSize: true,
        autoRowSize: true,
        nestedHeaders: [
            [
                { label: "", colspan: 6 },
                { label: "MAXIMA DEMANDA MW", colspan: listaAnios121.length + 1},
            ],
            [
                "AREA DE </br> DEMANDA",
                "SISTEMA",
                "SUBESTACIÓN",
                "TENSIÓN (kV)",
                "BARRA",
                "IDCARGA",
                ...listaAnios121,
            ],
        ],
        columns: [
            { type: "text" }, // Columna 0
            { type: "text" }, // Columna 1
            { type: "text" }, // Columna 2
            { type: "numeric" }, // Columna 3
            { type: "text" }, // Columna 4
            { type: "text" }, // Columna 5
            ...listaTypeNumeric121, // Años dinámicos.
        ],
        cells: function (row, col) {
            const cellProperties = {};

            // Últimas 4 filas son de totales (inhabilitadas).
            if (row >= data.length - 4) {
                cellProperties.readOnly = true;
            } else {
                // Todas las demás filas son editables.
                cellProperties.readOnly = false;
            }

            // Alinear texto a la derecha en la columna de totales.
            if (row >= data.length - 4 && col === 0) {
                cellProperties.className = "htRight";
            }

            return cellProperties;
        },
    });
}

function getDatos121() {
    
    var datosArray = hot121.getData();
    var headers = hot121.getSettings().nestedHeaders[1].slice(6); // Obtener los headers de los años

    var datosFiltrados = datosArray.slice(0, -4).filter(function (row) {
        // Verificar si al menos una columna tiene un valor no vacío
        return row.some(function (value) {
            return value !== "";
        });
    });
    var datosObjetos = datosFiltrados.map(function (row) {
        var detalleAnios = row.slice(6).map((valor, index) => {
            return { Anio: headers[index], Valor: valor };
        });
        return {
            AreaDemanda: row[0], // AREA DE DEMANDA
            Sistema: row[1], // SISTEMA
            Subestacion: row[2], // SUBESTACIÓN
            Tension: row[3], // TENSIÓN (kV)
            Barra: row[4], // TENSIÓN (kV)
            IdCarga: row[5], // IDCARGA
            ListItcdf121Det: detalleAnios, // Datos de los años
        };
    });
    console.log(datosObjetos);
    return datosObjetos;
}

function grabarItcdf121() {
    if (!validarTablaAntesDeGuardar(hot121)) {
        mostrarNotificacion("No se puede guardar porque hay datos inválidos en la tabla.");
        return; // Detener el guardado si hay errores
    }
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.Itcdf121DTOs = getDatos121();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarItcdf121',
            data: param,

            success: function (result) {
                console.log(result);
                stopLoading();
                if (result == 1) {
                    mostrarMensaje('mensajeFicha', 'exito', 'Los datos se grabaron correctamente.');
                    cambiosRealizados = false;
                }
                else {
                    mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                stopLoading();
                mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function cargarDatos121() {
    
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetItcdf121',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),

            success: function (response) {
                
                var hoja121Data = response.responseResult;
                setDatos121(hoja121Data);
                cambiosRealizados = false;
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function setDatos121(lista121) {
    hot121.loadData([]);
    if (modoModel === "consultar") {
        desactivarCamposFormulario("f121");
        $("#btnGrabar121").hide();
        $("#btnImportarExcel121").hide();
    }
    // Inicializar datosArray con filas vacías si aún no existe
    let datosArray = hot121 ? hot121.getData() : [];

    // Asegurarse de que la tabla tenga las filas mínimas (50 filas + 4 filas de totales)
    const totalFilas = Math.max(lista121 ? lista121.length : 0, 50) + 4;
    while (datosArray.length < totalFilas) {
        datosArray.push(new Array(5 + listaAnios121.length).fill(""));
    }
    // Actualizar o agregar datos en base a `lista121`
    lista121.forEach((item, index) => {
        if (!datosArray[index]) {
            datosArray[index] = new Array(6 + listaAnios121.length).fill("");
        }

        datosArray[index][0] = item.AreaDemanda;
        datosArray[index][1] = item.Sistema;
        datosArray[index][2] = item.Subestacion;
        datosArray[index][3] = item.Tension;
        datosArray[index][4] = item.Barra;
        datosArray[index][5] = item.IdCarga;

        item.ListItcdf121Det.forEach((detalle) => {
            const anioIndex = listaAnios121.indexOf(detalle.Anio);
            if (anioIndex !== -1) {
                datosArray[index][6 + anioIndex] = detalle.Valor !== null ? parseFloat(detalle.Valor).toFixed(4) : "";
            }
        });
    });

 

    // Agregar las filas de totales al final
    const filasTotales = ["TOTAL MAT", "TOTAL AT", "TOTAL MT", "TOTAL"];
    filasTotales.forEach((label, i) => {
        const totalIndex = datosArray.length - 4 + i; // Últimas 4 filas
        datosArray[totalIndex] = new Array(6 + listaAnios121.length).fill("");
        datosArray[totalIndex][5] = label; // Etiqueta en la sexta columna
    });

    // Recargar datos en la tabla
    hot121.loadData(datosArray);

    // Actualizar las propiedades de las celdas (inhabilitar últimas 4 filas)
    hot121.updateSettings({
        cells: (row, col) => {
            const cellProperties = {};
            if (row >= datosArray.length - 4) {
                cellProperties.readOnly = true; // Bloquear últimas 4 filas
                cellProperties.className = "htRight";
            } else {
                cellProperties.readOnly = false; // Habilitar resto
            }

            return cellProperties;
        },
    });
    // Recalcular los totales
    recalcularTotales(datosArray);
    hot121.render();
}

function recalcularTotalesYActualizar(data) {
    const rows = data.length - 4; // Últimas 4 filas son los totales
    const cols = data[0].length;

    // Inicializar las filas de totales
    const totalMat = new Array(cols).fill("");
    totalMat[0] = "TOTAL MAT";

    const totalAt = new Array(cols).fill("");
    totalAt[0] = "TOTAL AT";

    const totalMt = new Array(cols).fill("");
    totalMt[0] = "TOTAL MT";

    const totalGeneral = new Array(cols).fill("");
    totalGeneral[0] = "TOTAL";

    // Recalcular columnas de totales
    for (let col = 6; col < cols; col++) {
        let totalMatValue = 0;
        let totalAtValue = 0;
        let totalMtValue = 0;
        let totalGeneralValue = 0;

        for (let row = 0; row < rows; row++) {
            const cellValue = parseFloat(data[row][col]) || 0;

            if (cellValue >= 138) {
                totalMatValue += cellValue;
            } else if (cellValue >= 30 && cellValue < 138) {
                totalAtValue += cellValue;
            } else if (cellValue > 1 && cellValue < 30) {
                totalMtValue += cellValue;
            }
            totalGeneralValue += cellValue;
        }

        totalMat[col] = totalMatValue.toFixed(4);
        totalAt[col] = totalAtValue.toFixed(4);
        totalMt[col] = totalMtValue.toFixed(4);
        totalGeneral[col] = totalGeneralValue.toFixed(4);
    }

    // Asignar los totales a las últimas 4 filas
    data[rows] = totalMat;
    data[rows + 1] = totalAt;
    data[rows + 2] = totalMt;
    data[rows + 3] = totalGeneral;

    // Actualizar la tabla
    hot121.loadData(data);
    hot121.render();
}

function exportarTablaExcel121() {
    // Recalcular totales antes de exportar
    const datosArray = hot121.getData();
    //recalcularTotalesYActualizar(datosArray);

    // Excluir las últimas 4 filas de datosArray
    const datosSinTotales = datosArray.slice(0, datosArray.length - 4);

    // Preparar encabezados
    const headers = [
        [
            "",
            "",
            "",
            "",
            "",
            "",
            "MAXIMA DEMANDA MW",
        ],
        [
            "AREA DE DEMANDA",
            "SISTEMA",
            "SUBESTACION",
            "TENSION (kV)",
            "BARRA",
            "IDCARGA",
            ...listaAnios121,
        ],
    ];

    const data = headers.concat(datosSinTotales);

    // Crear hoja de Excel
    const ws = XLSX.utils.aoa_to_sheet(data);
    aplicarBordes(ws);

    // Definir las celdas combinadas para los encabezados
    ws["!merges"] = [
        { s: { r: 0, c: 6 }, e: { r: 0, c: 6 + listaAnios121.length - 1 } }, // Combina "MAXIMA DEMANDA MW"
    ];

    // Aplicar bordes y alinear texto
    const range = XLSX.utils.decode_range(ws["!ref"]);
    for (let row = range.s.r; row <= range.e.r; row++) {
        for (let col = range.s.c; col <= range.e.c; col++) {
            const cellAddress = XLSX.utils.encode_cell({ r: row, c: col });
            if (!ws[cellAddress]) ws[cellAddress] = { t: "s", v: "" }; // Asegurar celdas vacías

            // Aplicar bordes a todas las celdas
            ws[cellAddress].s = {
                border: {
                    top: { style: "thin", color: { rgb: "000000" } },
                    bottom: { style: "thin", color: { rgb: "000000" } },
                    left: { style: "thin", color: { rgb: "000000" } },
                    right: { style: "thin", color: { rgb: "000000" } },
                },
                alignment: {
                    horizontal: "center", // Alineación centrada
                },
            };
        }
    }

    // Crear libro y agregar hoja
    const wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "F121");

    // Exportar el archivo
    XLSX.writeFile(wb, "ITC_DemEDE_08-F121.xlsx");
}

function importarExcel121() {
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

            updateTableFromExcel121(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}
function updateTableFromExcel121(jsonData) {
    const headers = jsonData.slice(0, 2);
    const dataImportada = jsonData.slice(2);

    // Validar los datos importados
    const datosValidados = validarYFormatearDatos2(dataImportada, configuracionColumnas121, mostrarNotificacion, hot121);

    // Filtrar filas no necesarias
    const datosSinTotales = datosValidados.filter((row) => {
        return !row[0] || !row[0].toString().toUpperCase().includes("TOTAL");
    });

    // Agregar filas vacías para las últimas 4
    while (datosSinTotales.length < 50) {
        datosSinTotales.push(new Array(datosValidados[0].length).fill(""));
    }

    const filasTotales = ["TOTAL MAT", "TOTAL AT", "TOTAL MT", "TOTAL"];
    filasTotales.forEach((label) => {
        const filaTotal = new Array(datosValidados[0].length).fill("");
        filaTotal[5] = label;
        datosSinTotales.push(filaTotal);
    });

    // Recalcular los totales
    recalcularTotales(datosSinTotales);

    // Recargar los datos en la tabla
    hot121.loadData(datosSinTotales);

    // Actualizar propiedades de celdas
    hot121.updateSettings({
        cells: function (row, col) {
            const cellProperties = {};
            const config = configuracionColumnas121[col];
            const value = hot121.getDataAtCell(row, col);

            // Validar si es una fila de solo lectura (últimas 4 filas)
            if (row >= datosSinTotales.length - 4) {
                cellProperties.readOnly = true; // Bloquear edición
            } else if (config) {
                // Validar según la configuración
                if (value === null || value === "") {
                    // Si la celda está vacía
                    cellProperties.isCustomInvalid = false; // No marcar como inválido
                } else if (
                    (config.tipo === "entero" && (!isNumeric(value) || !Number.isInteger(parseFloat(value)))) ||
                    (config.tipo === "decimal" && (!isNumeric(value) || parseFloat(value) < 0))
                ) {
                    cellProperties.isCustomInvalid = true; // Marca personalizada
                } else {
                    cellProperties.isCustomInvalid = false; // Valor válido
                }
            }

            // Renderizador personalizado
            cellProperties.renderer = function (instance, td, row, col, prop, value, cellProperties) {
                Handsontable.renderers.TextRenderer.apply(this, arguments); // Usar renderizador predeterminado

                if (value === null || value === "") {
                    td.innerHTML = ""; // Dejar vacío si no hay valor
                    td.style.backgroundColor = ""; // No resaltar
                    cellProperties.valid = true; // Marcar como válida
                    td.classList.remove("invalid");
                    td.classList.remove("htInvalid");
                } else if (cellProperties.isCustomInvalid) {
                    td.style.backgroundColor = "#ff4c42"; // Fondo rojo claro para inválidos
                    cellProperties.valid = false; // Marcar como inválida
                } else {
                    td.style.backgroundColor = ""; // Limpiar fondo para válidos
                    cellProperties.valid = true; // Marcar como válida
                    td.classList.remove("invalid");
                    td.classList.remove("htInvalid");
                }
            };

            return cellProperties;
        },
    });

    hot121.render();
    mostrarNotificacion("Los datos se importaron correctamente.", '#a3e352');
}

function recalcularTotales(data) {
    const rows = data.length - 4; // Últimas 4 filas son los totales.
    const cols = data[0].length;

    // Inicializar las filas de totales.
    const totalMat = new Array(cols).fill("");
    totalMat[5] = "TOTAL MAT";

    const totalAt = new Array(cols).fill("");
    totalAt[5] = "TOTAL AT";

    const totalMt = new Array(cols).fill("");
    totalMt[5] = "TOTAL MT";

    const totalGeneral = new Array(cols).fill("");
    totalGeneral[5] = "TOTAL";


    // Recalcular columnas de totales.
    for (let col = 6; col < cols; col++) {
        let totalMatValue = 0;
        let totalAtValue = 0;
        let totalMtValue = 0;
        let totalGeneralValue = 0;

        for (let row = 0; row < rows; row++) {
            const cellValue = parseFloat(data[row][col]) || 0;

            if (cellValue >= 138) {
                totalMatValue += cellValue;
            } else if (cellValue >= 30 && cellValue < 138) {
                totalAtValue += cellValue;
            } else if (cellValue > 1 && cellValue < 30) {
                totalMtValue += cellValue;
            }
            totalGeneralValue += cellValue;
        }

        totalMat[col] = totalMatValue.toFixed(4);
        totalAt[col] = totalAtValue.toFixed(4);
        totalMt[col] = totalMtValue.toFixed(4);
        totalGeneral[col] = totalGeneralValue.toFixed(4);
    }

    // Asignar los totales a las últimas 4 filas.
    data[rows] = totalMat;
    data[rows + 1] = totalAt;
    data[rows + 2] = totalMt;
    data[rows + 3] = totalGeneral;
}
