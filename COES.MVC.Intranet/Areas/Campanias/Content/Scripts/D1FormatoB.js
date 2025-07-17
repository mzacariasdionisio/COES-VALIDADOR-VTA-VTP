var controlador = siteRoot + 'campanias/plantransmision/';
var controladorFormatos = siteRoot + 'campanias/fichasproyecto/';
var hotDFormatoC = null;
var valoresListaC = [];
var aniosCabecera = [];
var headerCabe = [];
var hotD1B = null;
const configuracionColumnasd1b = {
    2: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    3: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    4: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    5: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    6: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    7: { tipo: "decimal", decimales: 4, permitirNegativo: true },
    8: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    9: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    10:{ tipo: "decimal", decimales: 4, permitirNegativo: false },
     
};
$(function () {
//    $('#txtFechaIngreso').val(obtenerFechaActualMesAnio());

    $('#txtFechaIngreso').Zebra_DatePicker({
        format: 'm/Y',
        onSelect: function () {
            cambiosRealizados = true; // Marcar que hubo un cambio
        }
    });
    crearDFormatoB(horizonteInicio, horizonteFin);


});

getDataDFormatoB = function () {
    var param = {};
    param.NombreCarga = $("#txtNombreCarga").val();
    param.Propietario = $("#txtPropietarioB").val();
    param.FechaIngreso = $("#txtFechaIngreso").val();
    param.BarraConexion = $("#txtBarraConexion").val();
    param.NivelTension = $("#txtNivelTensionS").val();
    return param;
}

function EnergiaSubtotalRenderer(instance, td, row, col, prop, value, cellProperties) {
    const rows = instance.countRows() - (horizonteInicio - horizonteFin + 1);
    const isTotalRow = instance.getDataAtCell(row, 0)?.toString().includes("TOTAL");

    if (isTotalRow) {
        const year = instance.getDataAtCell(row, 0).split(' ')[1];
        let total = 0;

        for (let r = 0; r < instance.countRows(); r++) {
            const cellYear = instance.getDataAtCell(r, 0);
            if (cellYear && cellYear.toString() === year) {
                const cellValue = parseFloat(instance.getDataAtCell(r, col)) || 0;
                total += cellValue;
            }
        }

        td.innerHTML = total.toFixed(4);
        td.style.backgroundColor = ''; // Resetea el fondo si es correcto
    } else {
        if (value === '' || value === null) {
            td.innerHTML = ''; // Deja la celda vacía
            td.style.backgroundColor = ''; // Celda correcta (sin color)
        } else if (isNaN(value)) {
            td.innerHTML = value; // Muestra el valor no numérico
            td.style.backgroundColor = 'red'; // Marca como incorrecta
        } else {
            td.innerHTML = parseFloat(value).toFixed(4); // Formatea el valor numérico
            td.style.backgroundColor = ''; // Celda correcta (sin color)
        }
    }
}

function PotenciaSubtotalRenderer(instance, td, row, col, prop, value, cellProperties) {
    const rows = instance.countRows() - (horizonteInicio - horizonteFin + 1);
    const isTotalRow = instance.getDataAtCell(row, 0)?.toString().includes("TOTAL");

    if (isTotalRow) {
        const year = instance.getDataAtCell(row, 0).split(' ')[1];
        let maxValor = 0;

        for (let r = 0; r < instance.countRows(); r++) {
            const cellYear = instance.getDataAtCell(r, 0);
            if (cellYear && cellYear.toString() === year) {
                const cellValue = parseFloat(instance.getDataAtCell(r, col)) || 0;
                maxValor = Math.max(maxValor || 0, cellValue || 0);
            }
        }

        td.innerHTML = maxValor.toFixed(4);
        td.style.backgroundColor = ''; // Resetea el fondo si es correcto
    } else {
        if (value === '' || value === null) {
            td.innerHTML = ''; // Deja la celda vacía
            td.style.backgroundColor = ''; // Celda correcta (sin color)
        } else if (isNaN(value)) {
            td.innerHTML = value; // Muestra el valor no numérico
            td.style.backgroundColor = 'red'; // Marca como incorrecta
        } else {
            td.innerHTML = parseFloat(value).toFixed(4); // Formatea el valor numérico
            td.style.backgroundColor = ''; // Celda correcta (sin color)
        }
    }
}

function EnergiaColumnRenderer(instance, td, row, col, prop, value, cellProperties) {
    const isTotalRow = instance.getDataAtCell(row, 0)?.toString().includes("TOTAL");

    if (isTotalRow) {
        const year = instance.getDataAtCell(row, 0).split(' ')[1]; // Obtiene el año del texto "TOTAL año"

        let totalDemanda = 0;
        let totalGeneracion = 0;

        // Sumar las demandas y las generaciones para el año actual
        for (let r = 0; r < instance.countRows(); r++) {
            const cellYear = instance.getDataAtCell(r, 0);
            if (cellYear && cellYear.toString() === year) {
                const demanda = parseFloat(instance.getDataAtCell(r, 2)) || 0; // Columna de demanda
                const generacion = parseFloat(instance.getDataAtCell(r, 5)) || 0; // Columna de generación
                totalDemanda += demanda;
                totalGeneracion += generacion;
            }
        }

        // Calcular la demanda neta solo si hay datos válidos
        if (totalDemanda > 0 || totalGeneracion > 0) {
            const demandaNeta = totalDemanda - totalGeneracion;
            td.innerHTML = demandaNeta.toFixed(4); // Muestra la demanda neta formateada a 4 decimales
        } else {
            td.innerHTML = ''; // Deja la celda vacía si no hay datos válidos
        }
    } else {
        const energiaDemanda = parseFloat(instance.getDataAtCell(row, 2));
        const energiaGeneracion = parseFloat(instance.getDataAtCell(row, 5));

        // Solo realizar el cálculo si ambos valores están definidos y son números
        if (!isNaN(energiaDemanda) && !isNaN(energiaGeneracion)) {
            const total = energiaDemanda - energiaGeneracion;
            td.innerHTML = total.toFixed(4);
        } else {
            td.innerHTML = ''; // Deja la celda vacía si no hay datos válidos
        }
    }

    // Aplicar fondo gris si es la columna 8
    if (col === 8) {
        td.style.backgroundColor = "#FAFAFA"; // Fondo gris claro
    }
}

function crearDFormatoB(inicioan, finan) {

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
    for (var year = inicioan; year <= finan; year++) {
        var row = ['TOTAL ' + year];
        for (var i = 0; i < 7; i++) {
            row.push('');
        }
        data.push(row);
    }
    var grilla = document.getElementById('tableDemanGener');

    if (typeof hotD1B !== 'undefined' && hotD1B !== null) {
        hotD1B.destroy();
    }

    hotD1B = new Handsontable(grilla, {
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
                { label: 'DEMANDA', colspan: 3 },
                { label: 'GENERACI&Oacute;N (5)', colspan: 3 },
                { label: 'DEMANDA NETA = DEMANDA - GENERACI&Oacute;N', colspan: 3 }
            ],
            [
                { label: '', colspan: 1 },
                { label: '', colspan: 1 },
                { label: '', colspan: 1 },
                { label: 'POTENCIA(MW)', colspan: 2 },
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
                { label: 'HFP(4)', colspan: 1 },
                { label: 'ENERGIA(GWh)', colspan: 1 },
                { label: 'HP(3)', colspan: 1 },
                { label: 'HFP(4)', colspan: 1 }
            ]
        ],
        columns: [
            { data: 0, readOnly: true },
            { data: 1, readOnly: true },
            { data: 2, type: "text", renderer: EnergiaSubtotalRenderer },
            { data: 3,type: "text", renderer: PotenciaSubtotalRenderer },
            { data: 4, type: "text", renderer: PotenciaSubtotalRenderer },
            { data: 5, type: "text", renderer: EnergiaSubtotalRenderer },
            { data: 6, type: "text", renderer: PotenciaSubtotalRenderer },
            { data: 7, type: "text", renderer: PotenciaSubtotalRenderer },
            { data: 8, type: "text", renderer: EnergiaColumnRenderer },
            { data: 9, type: "text", renderer: PotenciaSubtotalRenderer },
            { data: 10, type: "text", renderer: PotenciaSubtotalRenderer }
        ],
        beforeChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
                
                return generalBeforeChange_4(this, changes, configuracionColumnasd1b);
            }
        }
    });
    hotD1B.render();
}

function getDataDFormatoBDet() {
    var datosArray = hotD1B.getData();
    
    datosArray = validarYFormatearDatos(datosArray, configuracionColumnasd1b, mostrarNotificacion);
    var datosObjetos = datosArray.map(function (row) {
        return {
            Anio: row[0],
            Mes: row[1],
            DemandaEnergia: row[2],
            DemandaHP: row[3],
            DemandaHFP: row[4],
            GeneracionEnergia: row[5],
            GeneracionHP: row[6],
            GeneracionHFP: row[7],
            DemandaNetaHP: row[9],
            DemandaNetaHFP: row[10],
        };
    });

    // Filtrar objetos que tienen al menos uno de los campos con datos v�lidos
    var datosFiltrados = datosObjetos.filter(function (obj) {
        return (
            obj.DemandaEnergia !== null && obj.DemandaEnergia !== "" ||
            obj.DemandaHP !== null && obj.DemandaHP !== "" ||
            obj.DemandaHFP !== null && obj.DemandaHFP !== "" ||
            obj.GeneracionEnergia !== null && obj.GeneracionEnergia !== "" ||
            obj.GeneracionHP !== null && obj.GeneracionHP !== "" ||
            obj.GeneracionHFP !== null && obj.GeneracionHFP !== "" ||
            obj.DemandaNetaHP !== null && obj.DemandaNetaHP !== ""
        );
    });
    return datosFiltrados;
}

function grabarDFormatoB() {

    if (!validarTablaAntesDeGuardar(hotD1B)) {
        mostrarNotificacion("No se puede guardar porque hay datos inválidos en la tabla.");
        return; // Detener el guardado si hay errores
    }
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.FormatoD1BDTO = getDataDFormatoB();
        param.ListaFormatoDet1B = getDataDFormatoBDet();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarDFormatoB',
            data: param,
            
            success: function (result) {
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

function cargarDFormatoB() {
    console.log("Editar");
    limpiarMensaje('mensajeFicha');
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetDFormatoB',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            
            success: function (response) {
                console.log(response);
                var hojaCRes = response.responseResult;

                if (hojaCRes.Proycodi == 0) {
                  //  hojaCRes.FechaIngreso = obtenerFechaActualMesAnio();
                   
                }
                //else {
                  
                //}


                setDataDFormato(hojaCRes);
                setDataDFormatoBDet(hojaCRes.ListaFormatoDet1B)
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
    cambiosRealizados = false;
}

function setDataDFormatoBDet(dataObjects) {
    // Verifica que la tabla Handsontable (hotD1B) est� inicializada
    if (typeof hotD1B === 'undefined' || hotD1B === null) {
        console.error("La tabla Handsontable no está inicializada.");
        return;
    }
    
    // Obtener todos los datos actuales de la tabla
    var currentData = hotD1B.getData();
    currentData = validarYFormatearDatos(currentData, configuracionColumnasd1b, mostrarNotificacion);
    
    // Iterar sobre cada objeto de datos proporcionado
    dataObjects.forEach(function (obj) {
        // Buscar el �ndice de fila correspondiente al a�o y mes del objeto actual
        var rowIndex = currentData.findIndex(function (row) {
            return row[0] == obj.Anio && row[1] === obj.Mes;
        });

        // Si se encuentra la fila correspondiente, actualizar las celdas espec�ficas
        if (rowIndex !== -1) {
            // Actualizar solo las celdas que corresponden a los valores proporcionados
            currentData[rowIndex][2] = obj.DemandaEnergia != null ? parseFloat(obj.DemandaEnergia).toFixed(4) : ''; // Energía
            currentData[rowIndex][3] = obj.DemandaHP != null ? parseFloat(obj.DemandaHP).toFixed(4) : ''; // HP
            currentData[rowIndex][4] = obj.DemandaHFP != null ? parseFloat(obj.DemandaHFP).toFixed(4) : ''; // HFP
            currentData[rowIndex][5] = obj.GeneracionEnergia != null ? parseFloat(obj.GeneracionEnergia).toFixed(4) : ''; // Energía Generación
            currentData[rowIndex][6] = obj.GeneracionHP != null ? parseFloat(obj.GeneracionHP).toFixed(4) : ''; // HP Generación
            currentData[rowIndex][7] = obj.GeneracionHFP != null ? parseFloat(obj.GeneracionHFP).toFixed(4) : ''; // HFP Generación
            currentData[rowIndex][9] = obj.DemandaNetaHP != null ? parseFloat(obj.DemandaNetaHP).toFixed(4) : ''; // HFP Generación
            currentData[rowIndex][10] = obj.DemandaNetaHFP != null ? parseFloat(obj.DemandaNetaHFP).toFixed(4) : ''; // HFP Generación
        }

    });

    // Cargar los datos actualizados en la tabla
    hotD1B.loadData(currentData);

    // Renderizar la tabla para reflejar los cambios
    hotD1B.render();
}

function setDataDFormato(param) {
    $("#txtNombreCarga").val(param.NombreCarga);
    $("#txtPropietarioB").val(param.Propietario);
    $("#txtFechaIngreso").val(param.FechaIngreso);
    $("#txtBarraConexion").val(param.BarraConexion);
    $("#txtNivelTensionS").val(param.NivelTension);
    if (modoModel == "consultar") {
        desactivarCamposFormulario('DGPFormatoD1B');
        $('#D1GrabarB').hide();
    }
}
function exportarTablaExcelDFormatoB() {
    // Definir los encabezados como en el ejemplo proporcionado
    var headers = [
        [
            "",
            "",
            "DEMANDA",
            "DEMANDA",
            "DEMANDA",
            "GENERACION(5)",
            "GENERACION(5)",
            "GENERACION(5)",
            "DEMANDA NETA",
            "DEMANDA NETA",
            "DEMANDA NETA"
        ],
        [
            "AñO",
            "MES",
            "ENERGIA(GWh)",
            "HP(3)",
            "HFP(4)",
            "ENERGIA(GWh)",
            "HP(3)",
            "HFP(4)",
            "ENERGIA(GWh)",
            "HP(3)",
            "HFP(4)"
        ]
    ];
    var datosArray = [];

    // Iterar sobre las filas de la tabla
    for (let r = 0; r < hotD1B.countRows(); r++) {
        let rowData = hotD1B.getDataAtRow(r); // Obtener la fila completa

        // Calcular demanda neta si no está calculada
        let energiaDemanda = parseFloat(rowData[2]) || 0; // Columna de energía de demanda
        let energiaGeneracion = parseFloat(rowData[5]) || 0; // Columna de energía de generación
        let demandaNeta = energiaDemanda - energiaGeneracion;

        // Si es una fila TOTAL, incluir el valor 0 si no hay datos
        if (rowData[0]?.toString().includes("TOTAL")) {
            rowData[8] = parseFloat(demandaNeta.toFixed(4)); // Asignar el total con ceros
        } else {
            // Si el valor de demanda neta es 0, dejar la celda vacía
            rowData[8] = demandaNeta !== 0 ? parseFloat(demandaNeta.toFixed(4)) : '';
        }

        // Agregar la fila procesada al arreglo de datos
        datosArray.push(rowData);
    }

    // Concatenar los encabezados con los datos
    var data = headers.concat(datosArray);

    // Crear una hoja de trabajo (worksheet) a partir de los datos
    var ws = XLSX.utils.aoa_to_sheet(data);
    aplicarBordes(ws);
    // Definir las celdas combinadas para los encabezados
    ws["!merges"] = [
        { s: { r: 0, c: 2 }, e: { r: 0, c: 4 } }, // Combina DEMANDA columnas
        { s: { r: 0, c: 5 }, e: { r: 0, c: 7 } }, // Combina GENERACIÓN columnas
        { s: { r: 0, c: 8 }, e: { r: 0, c: 10 } }  // Combina DEMANDA NETA columnas
    ];

    // Aplicar formato numérico a las celdas
    for (var R = 2; R < data.length; R++) {
        for (var C = 2; C < data[R].length; C++) {
            var cellAddress = XLSX.utils.encode_cell({ r: R, c: C });
            var cell = ws[cellAddress];
            if (cell && typeof cell.v === 'number') {
                cell.z = '0.0000';
            }
        }
    }
    aplicarFondoGris(ws, [8], hotD1B, headers.length);

    // Crear un nuevo libro de trabajo (workbook) y agregar la hoja
    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Datos");

    // Exportar el libro de trabajo a un archivo Excel
    XLSX.writeFile(wb, "D1B_01-DemandaBaseMensual.xlsx");
}

function importarExcelDFormatoB() {
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
 
    // Ignorar las dos primeras filas (cabeceras) pero mantenerlas en la tabla
    const headers = jsonData.slice(0, 2); // Guardar headers si es necesario para otros contextos
    const data = jsonData.slice(2); // Ignorar cabeceras, pero no modificar la estructura de las columnas

    // Validar y formatear los datos
    const datosValidados = validarYFormatearDatos2(data, configuracionColumnasd1b, mostrarNotificacion, hotD1B);

    // Almacenar estilos existentes antes de cargar los nuevos datos
    const stylesBackup = {};
    for (let row = 0; row < hotD1B.countRows(); row++) {
        for (let col = 0; col < hotD1B.countCols(); col++) {
            const cellMeta = hotD1B.getCellMeta(row, col);
            stylesBackup[`${row}-${col}`] = {
                backgroundColor: cellMeta.style?.backgroundColor || "",
                color: cellMeta.style?.color || "",
                fontWeight: cellMeta.style?.fontWeight || "",
                valid: cellMeta.valid,
            };
        }
    }

    // Cargar los datos validados
    hotD1B.loadData(datosValidados);

    // Restaurar los estilos y validaciones
    for (const key in stylesBackup) {
        const [row, col] = key.split("-").map(Number);
        const { backgroundColor, color, fontWeight, valid } = stylesBackup[key];

        hotD1B.setCellMeta(row, col, "style", {
            backgroundColor,
            color,
            fontWeight,
        });
        hotD1B.setCellMeta(row, col, "valid", valid);
    }

    // Refrescar la tabla para aplicar los estilos restaurados
    hotD1B.render();

    // Verificar errores en los datos validados
    const celdasConErrores = [];
    datosValidados.forEach((row, rowIndex) => {
        row.forEach((value, colIndex) => {
            const config = configuracionColumnasd1b[colIndex];
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
                    hotD1B.setCellMeta(rowIndex, colIndex, "valid", false);
                    hotD1B.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "#ffcccc", // Rojo claro
                    });
                } else {
                    // Si es válido, limpiar estilo previo
                    hotD1B.setCellMeta(rowIndex, colIndex, "valid", true);
                    hotD1B.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "",
                    });
                }
            }
        });
    });

    hotD1B.render(); // Refrescar tabla

    // Mostrar notificaciones según los errores detectados
    if (celdasConErrores.length > 0) {
        mostrarNotificacion(
            `Se importaron datos con errores.<br>Errores detectados en:<br>${celdasConErrores.join("<br>")}`
        );
    } else {
        mostrarNotificacion("Los datos se importaron correctamente.", '#a3e352');
    }
}


function recalcularTotales(instance) {
    const rowCount = instance.countRows();
    const colCount = instance.countCols();

    // Crear un mapa de años para almacenar subtotales
    const subtotales = {};

    for (let r = 0; r < rowCount; r++) {
        const year = instance.getDataAtCell(r, 0)?.toString();
        if (year && !year.includes("TOTAL")) {
            if (!subtotales[year]) {
                subtotales[year] = {
                    energiaDemanda: 0,
                    energiaGeneracion: 0,
                    potenciaMax: Array(colCount).fill(0),
                };
            }

            subtotales[year].energiaDemanda += parseFloat(instance.getDataAtCell(r, 2)) || 0;
            subtotales[year].energiaGeneracion += parseFloat(instance.getDataAtCell(r, 5)) || 0;

            for (let c = 3; c < colCount; c++) {
                subtotales[year].potenciaMax[c] = Math.max(
                    subtotales[year].potenciaMax[c],
                    parseFloat(instance.getDataAtCell(r, c)) || 0
                );
            }
        }
    }

    // Insertar los totales en las filas correspondientes
    for (let r = 0; r < rowCount; r++) {
        const year = instance.getDataAtCell(r, 0)?.toString();
        if (year?.includes("TOTAL")) {
            const actualYear = year.split(' ')[1];
            const totalData = subtotales[actualYear];

            if (totalData) {
                instance.setDataAtCell(r, 2, totalData.energiaDemanda.toFixed(4));
                instance.setDataAtCell(r, 5, totalData.energiaGeneracion.toFixed(4));

                for (let c = 3; c < colCount; c++) {
                    instance.setDataAtCell(r, c, totalData.potenciaMax[c].toFixed(4));
                }
            }
        }
    }
}



let isChangeVal = true;

function generalAfterChangeFormatoB(hotInstance, changes, source) {
    if (source === 'edit' && isChangeVal) {

        changes.forEach(function ([row, prop, oldValue, newValue]) {
            var numericValue = parseFloat(newValue);

            if (!isNaN(numericValue)) {

                var formattedValue = numericValue.toFixed(4);

                hotInstance.setDataAtCell(row, prop, parseFloat(formattedValue), 'set');
            } else {
                console.log("New value is not a number:", newValue);
            }
            if (prop === 2 || prop === 5) { // Supón que columna 2 es demanda y 5 es generación
                calcularDemandaNeta(hotInstance,row);
            }
        });
    } else {
        console.log("Source is not 'edit' or isChangeValid is false");
    }
}

function calcularDemandaNeta(instance,row) {
    const cellValue = instance.getDataAtCell(row, 0); // Obtiene el valor de la celda

    const isTotalRow = instance.getDataAtCell(row, 0)?.toString().includes("TOTAL");
    
    if (isTotalRow) {
        const year = instance.getDataAtCell(row, 0).split(' ')[1]; // Obtiene el año del texto "TOTAL año"
        
        let totalDemanda = 0;
        let totalGeneracion = 0;

        for (let r = 0; r < instance.countRows(); r++) {
            const cellYear = instance.getDataAtCell(r, 0);
            if (cellYear && cellYear.toString() === year) {
                totalDemanda += parseFloat(instance.getDataAtCell(r, 2)) || 0; // Columna de demanda
                totalGeneracion += parseFloat(instance.getDataAtCell(r, 5)) || 0; // Columna de generación
            }
        }

        // Calcular la demanda neta
        const demandaNeta = totalDemanda - totalGeneracion;
        instance.setDataAtCell(row, 8, parseFloat(demandaNeta.toFixed(4)), 'set'); 
    } else {
        let total = 0;
        let energiaDemanda = parseFloat(instance.getDataAtCell(row, 2)) || 0;
        let energiaGeneracion = parseFloat(instance.getDataAtCell(row, 5)) || 0;
        total = energiaDemanda - energiaGeneracion;
        //td.innerHTML = total.toFixed(4);
        instance.setDataAtCell(row, 8, parseFloat(total.toFixed(4)), 'set');
    } 
}

function CalcularEnergiaSubtotal(instance, td, row, col, prop, value, cellProperties) {
    let rows = instance.countRows() - (horizonteInicio - horizonteFin + 1);
    const isTotalRow = instance.getDataAtCell(row, 0)?.toString().includes("TOTAL");
    
    if (isTotalRow) {
        const year = instance.getDataAtCell(row, 0).split(' ')[1]; 
        let total = 0;

        for (let r = 0; r < instance.countRows(); r++) {
            const cellYear = instance.getDataAtCell(r, 0);
            if (cellYear && cellYear.toString() === year) {
                const cellValue = parseFloat(instance.getDataAtCell(r, col)) || 0;
                total += cellValue;
            }
        }

        // Actualiza la celda usando setDataAtCell
        instance.setDataAtCell(row, col, parseFloat(total.toFixed(4)), 'set');
    } else {
        const formattedValue = value ? parseFloat(value).toFixed(4) : '';
        instance.setDataAtCell(row, col, formattedValue ? parseFloat(formattedValue) : null, 'set');
    }
}

function CalcularPotenciaSubtotal(instance, td, row, col, prop, value, cellProperties) {
    let rows = instance.countRows() - (horizonteInicio - horizonteFin + 1);
    const isTotalRow = instance.getDataAtCell(row, 0)?.toString().includes("TOTAL");
    
    if (isTotalRow) {
        const year = instance.getDataAtCell(row, 0).split(' ')[1]; 
        let maxValor = 0;

        for (let r = 0; r < instance.countRows(); r++) {
            const cellYear = instance.getDataAtCell(r, 0);
            if (cellYear && cellYear.toString() === year) {
                const cellValue = parseFloat(instance.getDataAtCell(r, col)) || 0;
                maxValor = Math.max(maxValor, cellValue); // Simplificación de la lógica
            }
        }

        // Actualiza la celda usando setDataAtCell
        instance.setDataAtCell(row, col, parseFloat(maxValor.toFixed(4)), 'set');
    } else {
        const formattedValue = value ? parseFloat(value).toFixed(4) : '';
        instance.setDataAtCell(row, col, formattedValue ? parseFloat(formattedValue) : null, 'set');
    }
}