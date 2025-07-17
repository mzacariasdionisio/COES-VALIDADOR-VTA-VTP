var hot110 = null;
var listaAnios110 = [];
var listaTypeNumeric110 = [];
var configuracionColumnas110 = {};

$(function () {
    configuracionColumnas110 = generarconfiguracionColumnas110(anioPeriodo - 1, horizonteFin + 10);

    crearTablaITCF110((anioPeriodo - 1), (horizonteFin + 10), []);

});

function isNumeric(value) {
    var parsed = parseFloat(value);
    return !isNaN(parsed) && isFinite(parsed);
}

function generarconfiguracionColumnas110(anioInicia, anioFin) {

    const configuracion = {

    };

    // Configuración específica para las columnas 0 y 3
    configuracion[0] = { tipo: "entero", permitirNegativo: false };
    configuracion[1] = { tipo: "texto", largo:255 };
    configuracion[2] = { tipo: "texto", largo:255 };
    configuracion[3] = { tipo: "decimal", decimales: 4, permitirNegativo: false };
    configuracion[4] = { tipo: "texto", largo: 255 };
    configuracion[5] = { tipo: "texto", largo: 255 };
    // Configuración dinámica para las columnas a partir de la 6
    for (let i = 6; i <= 6+ (anioFin - anioInicia); i++) {
        configuracion[i] = { tipo: "decimal", decimales: 4, permitirNegativo: false };
    }

    return configuracion;
    
}

function currencyRenderer110(instance, td, row, col, prop, value, cellProperties) {
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

        td.innerHTML = total.toFixed(4);  // Mostrar el total calculado
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

function crearTablaITCF110(anioInicia, anioFin, data) {
    var grilla = document.getElementById("tablef110");

    if (data.length == 0) {
        for (var i = anioInicia; i <= anioFin; i++) {
            listaAnios110.push(i);
            listaTypeNumeric110.push({ type: "text", renderer: currencyRenderer110 , readOnly: true})
        }
        for (var i = 0; i <= 50; i++) {
            var row = [];
            row.push("");
            row.push("");
            row.push("");
            row.push("");
            row.push("");
            row.push("");
            for (var j = 0; j < listaAnios110.length; j++) {
                row.push("");
            }
            data.push(row);
        }

        var totalMat = [];
        totalMat.push("");
        totalMat.push("");
        totalMat.push("");
        totalMat.push("");
        totalMat.push("");
        totalMat.push("TOTAL MAT");
        for (var i = 0; i < listaAnios110.length; i++) {
            totalMat.push("");
        }
        data.push(totalMat); // Añadimos la fila de máximo

        var totalAt = [];
        totalAt.push("");
        totalAt.push("");
        totalAt.push("");
        totalAt.push("");
        totalAt.push("");
        totalAt.push("TOTAL AT");
        for (var i = 0; i < listaAnios110.length; i++) {
            totalAt.push("");
        }
        data.push(totalAt); // Añadimos la fila de máximo


        var totalMt = [];
        totalMt.push("");
        totalMt.push("");
        totalMt.push("");
        totalMt.push("");
        totalMt.push("");
        totalMt.push("TOTAL MT");
        for (var i = 0; i < listaAnios110.length; i++) {
            totalMt.push("");
        }
        data.push(totalMt); // Añadimos la fila de máximo

        var total = [];
        total.push("");
        total.push("");
        total.push("");
        total.push("");
        total.push("");
        total.push("TOTAL");
        for (var i = 0; i < listaAnios110.length; i++) {
            total.push("");
        }
        data.push(total); // Añadimos la fila de máximo

    }
    if (typeof hot110 !== "undefined" && hot110 !== null) {
        hot110.destroy();
    }




    hot110 = new Handsontable(grilla, {
        data: data,
        rowHeaders: true,
        colHeaders: true,
        colWidths: 170,
        manualColumnResize: true,
        columnSorting: false,
        autoRowSize: true,
        filters: false,
        contextMenu: false,
        autoColumnSize: true,
        nestedHeaders: [
            [
                { label: "", colspan: 1 },
                { label: "", colspan: 1 },
                { label: "", colspan: 1 },
                { label: "", colspan: 1 },
                { label: "", colspan: 1 },
                { label: "", colspan: 1 },
                { label: "MAXIMA DEMANDA MW", colspan: listaAnios110.length },
            ],
            [
                "AREA DE </br> DEMANDA",
                "SISTEMA",
                "SUBESTACI&Oacute;N",
                "TENSI&Oacute;N (kV)",
                "BARRA",
                "IDCARGA",
                ...listaAnios110,
            ],
        ],
         columns: [
            { type: "text", readOnly: true },
            { type: "text", readOnly: true}, 
            { type: "text", readOnly: true },
            { type: "text", readOnly: true},
            { type: "text", readOnly: true },
            { type: "text", readOnly: true},
            ...listaTypeNumeric110 
        ],
     
        beforeChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
                
                return generalBeforeChange_4(this, changes, configuracionColumnas110);
            }
        },
        afterChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
                generalAfterChange(this, changes, configuracionColumnas110);
            }
        },
    });
    hot110.render();
    if (modoModel == "consultar") {
        $("#btnGrabarFicha104").hide();
        hot110.updateSettings({
            cells: function (row, col) {
                var cellProperties = {};
                cellProperties.readOnly = true;
                return cellProperties;
            }
        });
    }
}

function getDatos110() {
    var datosArray = hot110.getData();
    var headers = hot110.getSettings().nestedHeaders[1].slice(6); // Obtener los headers de los años
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
            ListItcdf110Det: detalleAnios, // Datos de los años
        };
    });
    console.log(datosObjetos);
    return datosObjetos;
}

function grabarItcdf110() {
    if (!validarTablaAntesDeGuardar(hot110)) {
        mostrarNotificacion("No se puede guardar porque hay datos inválidos en la tabla.");
        return; // Detener el guardado si hay errores
    }

    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.Itcdf110DTOs = getDatos110();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarItcdf110',
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

function cargarDatos110() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetItcdf110',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            
            success: function (response) {
                console.log(response);
                var hoja110Data = response.responseResult;
                setDatos110(hoja110Data);
                cambiosRealizados = false;
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function setDatos110(lista110) {
    if (modoModel === "consultar") {
        desactivarCamposFormulario('f110');
        $('#btnGrabar110').hide();
    }
    hot110.loadData([]);
    // Inicializar datosArray con filas vacías si aún no existe
    let datosArray = hot110 ? hot110.getData() : [];

    // Asegurarse de que la tabla tenga las filas mínimas (50 filas + 4 filas de totales)
    const totalFilas = Math.max(lista110 ? lista110.length : 0, 50) + 4;
    while (datosArray.length < totalFilas) {
        datosArray.push(new Array(5 + listaAnios110.length).fill(""));
    }

    // Actualizar los datos según la lista recibida
    if (lista110) {
        lista110.forEach((item, index) => {
            datosArray[index][0] = item.AreaDemanda !== null && item.AreaDemanda !== undefined ? item.AreaDemanda : "";
            datosArray[index][1] = item.Sistema !== null && item.Sistema !== undefined ? item.Sistema : "";
            datosArray[index][2] = item.Subestacion !== null && item.Subestacion !== undefined ? item.Subestacion : "";
            datosArray[index][3] = item.Tension ? parseFloat(item.Tension).toFixed(4) : "";
            datosArray[index][4] = item.Barra !== null && item.Barra !== undefined ? item.Barra : "";
            datosArray[index][5] = item.IdCarga !== null && item.IdCarga !== undefined ? item.IdCarga : "";

            // Actualizar las columnas de años
            item.ListItcdf110Det.forEach((detalle) => {
                const anioIndex = listaAnios110.indexOf(detalle.Anio);
                if (anioIndex !== -1) {
                    datosArray[index][6 + anioIndex] = detalle.Valor !== null && detalle.Valor !== undefined
                        ? parseFloat(detalle.Valor).toFixed(4)
                        : "";
                }
            });
        });
    }

    // Asegurarse de que las últimas 4 filas sean las de totales
    const filasTotales = ["TOTAL MAT", "TOTAL AT", "TOTAL MT", "TOTAL"];
    filasTotales.forEach((label, i) => {
        const totalIndex = datosArray.length - 4 + i; // Últimas 4 filas
        datosArray[totalIndex] = new Array(6 + listaAnios110.length).fill("");
        datosArray[totalIndex][5] = label; // Etiqueta en la sexta columna
    });

    // Actualizar los datos en la tabla
    hot110.loadData(datosArray);

    // Configurar las propiedades de las celdas
    hot110.updateSettings({
        cells: (row, col) => {
            const cellProperties = {};
            if (row >= datosArray.length - 4) {
                cellProperties.readOnly = true; // Bloquear las últimas 4 filas
                cellProperties.className = "htRight";
            } else {
                cellProperties.readOnly = false; // Permitir edición en las demás filas
            }
            return cellProperties;
        },
    });

    // Recalcular los totales
    recalcularTotales(datosArray);

    hot110.render();
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
    hot110.loadData(data);
    hot110.render();
}


function exportarTablaExcel110() {
    var headers = [
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
            ...listaAnios110,
        ],
    ];

    var datosArray = hot110.getData();

    // Excluir las últimas 4 filas de datosArray
    const datosSinTotales = datosArray.slice(0, datosArray.length - 4);

    var data = headers.concat(datosSinTotales);

    var ws = XLSX.utils.aoa_to_sheet(data);
    aplicarBordes(ws);
    // Definir las celdas combinadas
    ws["!merges"] = [
        { s: { r: 0, c: 6 }, e: { r: 0, c: 6 + listaAnios110.length - 1 } }, // Combina "MAXIMA DEMANDA MW"
    ];

    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "F110-01");

    XLSX.writeFile(wb, "ITC_DemEDE_06-F110.xlsx");
}

function importarExcel110() {
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

            updateTableFromExcel110(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}

function updateTableFromExcel110(jsonData) {
    const headers = jsonData.slice(0, 2);
    const dataImportada = jsonData.slice(2);

    // Validar los datos importados
    const datosValidados = validarYFormatearDatos2(dataImportada, configuracionColumnas110, mostrarNotificacion, hot110);

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
    hot110.loadData(datosSinTotales);

    // Actualizar propiedades de celdas
    hot110.updateSettings({
        cells: function (row, col) {
            const cellProperties = {};
            const config = configuracionColumnas110[col];
            const value = hot110.getDataAtCell(row, col);

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

    hot110.render();
    mostrarNotificacion("Los datos se importaron correctamente.", '#a3e352');
}


function recalcularTotales(data) {
    const rows = data.length - 4; // Últimas 4 filas son los totales
    const cols = data[0].length;

    // Inicializar las filas de totales
    const totalMat = new Array(cols).fill("");
    totalMat[6] = "TOTAL MAT";

    const totalAt = new Array(cols).fill("");
    totalAt[6] = "TOTAL AT";

    const totalMt = new Array(cols).fill("");
    totalMt[6] = "TOTAL MT";

    const totalGeneral = new Array(cols).fill("");
    totalGeneral[6] = "TOTAL";

    // Recalcular columnas de totales
    for (let col = 7; col < cols; col++) {
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
}
