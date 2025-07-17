var hot116 = null;
var listaAnios116 = [];
var listaTypeNumeric116 = [];
var configuracionColumnas116 = {};

$(function () {
    configuracionColumnas116 = generarconfiguracionColumnas116(anioPeriodo-1, horizonteFin+10);
    crearTablaITCF116((anioPeriodo - 1), (horizonteFin + 10), []);
});
function generarconfiguracionColumnas116(anioInicia, anioFin) {
    
    const configuracion = {};

    // Configuración específica para las columnas 0 y 3
    configuracion[0] = { tipo: "entero", permitirNegativo: false };
    configuracion[3] = { tipo: "decimal", decimales: 4, permitirNegativo: false };

    // Configuración dinámica para las columnas a partir de la 6
    for (let i = 7; i <= 7 + (anioFin - anioInicia); i++) {
        configuracion[i] = { tipo: "decimal", decimales: 4, permitirNegativo: false };
    }

    return configuracion;
    
}

function isNumeric(value) {
    var parsed = parseFloat(value);
    return !isNaN(parsed) && isFinite(parsed);
}

function currencyRenderer(instance, td, row, col, prop, value, cellProperties) {
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

        td.innerHTML = parseFloat(total).toFixed(4);// Mostrar el total calculado
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
            td.style.backgroundColor = "#ff4c42"; // Fondo rojo claro
            cellProperties.valid = false; // Marcar como inválida
        } else {
            // Si es numérico, mostrar el valor y limpiar estilos
            td.innerHTML = parseFloat(value).toFixed(4);
            td.style.backgroundColor = ""; // Quitar estilos
            cellProperties.valid = true; // Marcar como válida
        }
    }
}

function generalAfterChange116(hotInstance, changes, configuracionColumnas) {
    changes.forEach(([row, col, oldValue, newValue]) => {
        const config = configuracionColumnas[col]; // Configuración basada en la columna
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


function crearTablaITCF116(anioInicia, anioFin, data) {
    var grilla = document.getElementById("tablef116");

    if (data.length == 0) {
        for (var i = anioInicia; i <= anioFin; i++) {
            listaAnios116.push(i);
            listaTypeNumeric116.push({ type: "text", renderer: currencyRenderer , readOnly: true})
        }
        for (var i = 0; i <= 50; i++) {
            var row = [];
            row.push("");
            row.push("");
            row.push("");
            row.push("");
            row.push("");
            row.push("");
            for (var j = 0; j <= listaAnios116.length; j++) {
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
        totalMat.push("");
        totalMat.push("TOTAL MAT");
        for (var i = 0; i < listaAnios116.length; i++) {
            totalMat.push("");
        }
        data.push(totalMat); // Añadimos la fila de máximo

        var totalAt = [];
        totalAt.push("");
        totalAt.push("");
        totalAt.push("");
        totalAt.push("");
        totalAt.push("");
        totalAt.push("");
        totalAt.push("TOTAL AT");
        for (var i = 0; i < listaAnios116.length; i++) {
            totalAt.push("");
        }
        data.push(totalAt); // Añadimos la fila de máximo


        var totalMt = [];
        totalMt.push("");
        totalMt.push("");
        totalMt.push("");
        totalMt.push("");
        totalMt.push("");
        totalMt.push("");
        totalMt.push("TOTAL MT");
        for (var i = 0; i < listaAnios116.length; i++) {
            totalMt.push("");
        }
        data.push(totalMt); // Añadimos la fila de máximo

        var total = [];
        total.push("");
        total.push("");
        total.push("");
        total.push("");
        total.push("");
        total.push("");
        total.push("TOTAL");
        for (var i = 0; i < listaAnios116.length; i++) {
            total.push("");
        }
        data.push(total); // Añadimos la fila de máximo
    }
   
    if (typeof hot116 !== "undefined" && hot116 !== null) {
        hot116.destroy();
    }

    hot116 = new Handsontable(grilla, {
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
                { label: "", colspan: 1 },
                { label: "", colspan: 1 },
                { label: "", colspan: 1 },
                { label: "", colspan: 1 },
                { label: "", colspan: 1 },
                { label: "", colspan: 1 },
                { label: "", colspan: 1 },
                { label: "MAXIMA DEMANDA MW", colspan: listaAnios116.length + 1 },
            ],
            [
                "AREA DE </br> DEMANDA",
                "SISTEMA",
                "SUBESTACIÓN",
                "TENSI&Oacute;N (kV)",
                "BARRA",
                "NOMBRE CLIENTE",
                "IDCARGA",
                ...listaAnios116,
            ],
        ],
        columns: [
           { type: "text", readOnly: true },
           { type: "text", readOnly: true}, 
           { type: "text", readOnly: true },
           { type: "text", readOnly: true},
           { type: "text" , readOnly: true},
           { type: "text", readOnly: true},
           { type: "text", readOnly: true},
           ...listaTypeNumeric116 
        ],
        beforeChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
                
                return generalBeforeChange_4(this, changes, configuracionColumnas116);
            }
        },
        afterChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
                generalAfterChange_global(this, changes, configuracionColumnas116);
            }
        },
    });
    hot116.render();
    if (modoModel == "consultar") {
        hot116.updateSettings({
            cells: function (row, col) {
                var cellProperties = {};
                cellProperties.readOnly = true;
                return cellProperties;
            }
        });
    }
}

function getDatos116() {
    
    var datosArray = hot116.getData();
    var headers = hot116.getSettings().nestedHeaders[1].slice(7); // Obtener solo los headers de los años desde la columna 8

    var datosObjetos = datosArray.slice(0, -4).map(function (row) {
        var detalleAnios = row.slice(7).map((valor, index) => {
            let anio = headers[index]; // Obtener el año desde la cabecera

            // Si el año es válido y el valor no está vacío, lo agregamos
            if (anio !== "" && !isNaN(anio) && valor !== null && valor !== "") {
                return { Anio: anio, Valor: parseFloat(valor).toFixed(4) };
            }
        }).filter(detalle => detalle !== undefined); // Eliminar valores no válidos

        // Verificar si la fila tiene al menos un dato válido en las primeras 7 columnas
        let tieneDatosPrincipales = row.slice(0, 7).some(value => value !== "" && value !== null);

        // Crear el objeto de la fila
        let filaObjeto = {
            AreaDemanda: row[0], // AREA DE DEMANDA
            Sistema: row[1], // SISTEMA
            Subestacion: row[2], // SUBESTACIÓN
            Tension: row[3], // TENSIÓN (kV)
            Barra: row[4], // BARRA
            NombreCliente: row[5], // NOMBRE CLIENTE
            IdCarga: row[6], // IDCARGA
        };

        // Agregar ListItcdf116Det solo si tiene datos
        if (detalleAnios.length > 0) {
            filaObjeto.ListItcdf116Det = detalleAnios;
        }

        return tieneDatosPrincipales || detalleAnios.length > 0 ? filaObjeto : null;
    }).filter(obj => obj !== null); // Filtrar objetos vacíos

    return datosObjetos;
}

function grabarItcdf116() {
    if (!validarTablaAntesDeGuardar(hot116)) {
        mostrarNotificacion("No se puede guardar porque hay datos inválidos en la tabla.");
        return; // Detener el guardado si hay errores
    }
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.Itcdf116DTOs = getDatos116();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarItcdf116',
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

function cargarDatos116() {
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetItcdf116',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            
            success: function (response) {
                var hoja116Data = response.responseResult;
                setDatos116(hoja116Data);
                cambiosRealizados = false;
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function setDatos116(lista116) {
    
    if (modoModel === "consultar") {
        desactivarCamposFormulario("f116");
        $("#btnGrabar116").hide();
    }
    hot116.loadData([]);

    // Inicializar datosArray con filas vacías si aún no existe
    let datosArray = hot116 ? hot116.getData() : [];

    // Asegurarse de que la tabla tenga las filas mínimas (50 filas + 4 filas de totales)
    const totalFilas = Math.max(lista116 ? lista116.length : 0, 50) + 4;
    while (datosArray.length < totalFilas) {
        datosArray.push(new Array(6 + listaAnios116.length).fill(""));
    }

    // Actualizar o agregar datos en base a `lista116`
    lista116.forEach((item, index) => {
        if (!datosArray[index]) {
            datosArray[index] = new Array(6 + listaAnios116.length).fill("");
        }

        datosArray[index][0] = item.AreaDemanda;
        datosArray[index][1] = item.Sistema;
        datosArray[index][2] = item.Subestacion;
        // Formatear Tension a 4 decimales antes de asignarla
        datosArray[index][3] = item.Tension ? parseFloat(item.Tension).toFixed(4) : "";
        datosArray[index][4] = item.Barra;
        datosArray[index][5] = item.NombreCliente;
        datosArray[index][6] = item.IdCarga;

        item.ListItcdf116Det.forEach((detalle) => {
            const anioIndex = listaAnios116.indexOf(detalle.Anio);
            if (anioIndex !== -1) {
                datosArray[index][7 + anioIndex] = detalle.Valor ? Number(detalle.Valor).toFixed(4) : "";


            }
        });
    });

 

    // Agregar las filas de totales al final
    const filasTotales = ["TOTAL MAT", "TOTAL AT", "TOTAL MT", "TOTAL"];
    filasTotales.forEach((label, i) => {
        const totalIndex = datosArray.length - 4 + i; // Últimas 4 filas
        datosArray[totalIndex] = new Array(6 + listaAnios116.length).fill("");
        datosArray[totalIndex][6] = label; // Etiqueta en la primera columna
    });

    // Recargar datos en la tabla
    hot116.loadData(datosArray);

    // Actualizar las propiedades de las celdas (inhabilitar últimas 4 filas)
    hot116.updateSettings({
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
    recalcularTotales116(datosArray);
    hot116.render();
}

function exportarTablaExcel116() {
    var headers = [
        [
            "",
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
            "NOMBRE CLIENTE",
            "IDCARGA",
            ...listaAnios116, // Agregar listaAnios116 correctamente después de IDCARGA
        ],
    ];

    // Obtener los datos de la tabla

    var datosArray = hot116.getData();
    // Excluir las últimas 4 filas de datosArray
    const datosSinTotales = datosArray.slice(0, datosArray.length - 4);
    // Concatenar las cabeceras con los datos
    var data = headers.concat(datosSinTotales);

    // Crear la hoja de cálculo
    var ws = XLSX.utils.aoa_to_sheet(data);

    // Aplicar bordes a la hoja
    aplicarBordes(ws);

    // Definir las celdas combinadas (ajustar el rango según listaAnios116)
    ws["!merges"] = [
        {
            s: { r: 0, c: 7 }, // Inicio de la combinación (primera fila, octava columna)
            e: { r: 0, c: 7 + listaAnios116.length - 1 }, // Fin de la combinación
        },
    ];

    // Crear el libro de Excel y agregar la hoja
    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "F116");

    // Guardar el archivo de Excel
    XLSX.writeFile(wb, "ITC_DemEDE_07-F116.xlsx");
}

function importarExcel116() {
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

            updateTableFromExcel116(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}
function updateTableFromExcel116(jsonData) {
    const headers = jsonData.slice(0, 2);
    const dataImportada = jsonData.slice(2);

    // Validar los datos importados
    const datosValidados = validarYFormatearDatos2(dataImportada, configuracionColumnas116, mostrarNotificacion, hot116);

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
        filaTotal[6] = label;
        datosSinTotales.push(filaTotal);
    });

    // Recalcular los totales
    recalcularTotales116(datosSinTotales);

    // Formatear números a 4 decimales en las últimas 4 filas
    const numFilas = datosSinTotales.length;
    for (let row = numFilas - 4; row < numFilas; row++) {
        datosSinTotales[row] = datosSinTotales[row].map((value, col) => {
            // Si el valor es un número, formatearlo a 4 decimales
            if (!isNaN(value) && value !== "" && typeof value === "number") {
                return parseFloat(value).toFixed(4);
            }
            return value; // Dejar el resto igual
        });
    }
    // Recargar los datos en la tabla
    hot116.loadData(datosSinTotales);

    // Actualizar propiedades de celdas
    hot116.updateSettings({
        cells: function (row, col) {
            const cellProperties = {};
            const config = configuracionColumnas116[col];
            const value = hot116.getDataAtCell(row, col);

       
            if (config) {
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
            // Validar si es una fila de solo lectura (últimas 4 filas)
            if (row >= datosSinTotales.length - 4) {
                cellProperties.readOnly = true; // Bloquear edición
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

    hot116.render();
    mostrarNotificacion("Los datos se importaron correctamente.", '#a3e352');
}

function recalcularTotales116(data) {
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

        // Asignar valores con 4 decimales
        totalMat[col] = parseFloat(totalMatValue.toFixed(4));
        totalAt[col] = parseFloat(totalAtValue.toFixed(4));
        totalMt[col] = parseFloat(totalMtValue.toFixed(4));
        totalGeneral[col] = parseFloat(totalGeneralValue.toFixed(4));
    }

    // Asignar los totales a las últimas 4 filas
    data[rows] = totalMat;
    data[rows + 1] = totalAt;
    data[rows + 2] = totalMt;
    data[rows + 3] = totalGeneral;
}
