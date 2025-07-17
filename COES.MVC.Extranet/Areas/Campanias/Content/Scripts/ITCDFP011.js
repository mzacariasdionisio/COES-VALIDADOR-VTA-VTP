var hot011 = null;
var listaBarra = [];
var numBarra = 1;
let offset = 0;
const pagePreview = 200;
const pageSize = 5000;
let itcfp011Codi = 0;
let loadingCarga = false;
let totalData = false;
var blockSize = 1200;

const configuracionColumnas011 = {
    1: { tipo: "decimal", decimales: 4, permitirNegativo: true } // Se aplicará a todas las columnas de barras
};

$(function () {
    $('#lgdTitleFP011Ext').html("REGISTRO HISTÓRICO DE LAS CARGAS - AÑO " + (anioPeriodo - 1));
    crearTablaITCFP011(numBarra);
    var formulariosa = document.getElementById('tabla101');
    formulariosa.addEventListener('change', function () {
        cambiosRealizados = true;
    });
});

function crearTablaITCFP011(numBarras, existingData = [], setData = []) {
    const intervaloMinutos = 15;
    const listaFechas = generarListaFechasIntervaloAnual(anioPeriodo - 1, intervaloMinutos);
    var data = [];
    for (var i = 0; i <= listaFechas.length - 1; i++) {
        var row = [];
        row.push(listaFechas[i]);
        if (existingData[i]) {
            for (var j = 1; j < existingData[i].length; j++) {
                row.push(existingData[i][j]);
            }
        }
        for (var j = existingData[i] ? existingData[i].length - 1 : 0; j < numBarras; j++) {
            row.push("");
        }
        data.push(row);
    }

    var grilla = document.getElementById("tablefp101");

    if (typeof hot011 !== "undefined" && hot011 !== null) {
        hot011.destroy();
    }
    for (let i = 1; i <= numBarras * 2; i++) {
        configuracionColumnas011[i] = { tipo: "decimal", decimales: 4, permitirNegativo: true };
    }
    hot011 = new Handsontable(grilla, {
        data: data,
        //rowHeaders: true,
        colHeaders: true,
        colWidths: 170,
        manualColumnResize: true,
        columnSorting: false,
        filters: false,
        dropdownMenu: true,
        autoColumnSize: true,
        autoRowSize: true,
        fixedColumnsLeft: 1,
        nestedHeaders: [
            [
                { label: "", colspan: 1 },
            ].concat(Array.from({ length: numBarras }, (v, i) => [
                { label: `BARRA[${i + 1}]`, colspan: 2 },
            ]).flat()),
            [
                { label: "", colspan: 1 },
            ].concat(Array.from({ length: numBarras }, () => [
                { label: "kV", colspan: 2 },
            ]).flat()),
            [
                { label: "FECHA-HORA", colspan: 1 },
            ].concat(Array.from({ length: numBarras }, () => [
                { label: "kW", colspan: 1 },
                { label: "kVAR", colspan: 1 }
            ]).flat()),
        ],
        columns: [
            { readOnly: true }, // Columna de FECHA-HORA
        ].concat(Array.from({ length: (numBarras * 2) }, () => ({ type: "text" }))),
        viewportRowRenderingOffset: 0,
        beforeChange: (changes) => generalBeforeChange_4(hot011, changes, configuracionColumnas011),
        afterChange: (changes, source) => {
            if (["edit", "paste", "autofill"].includes(source)) {
                generalAfterChange_global(hot011, changes, configuracionColumnas011);
            }
        },
        /*afterScrollVertically: function () {
            const lastVisibleRow = hot011.view.wt.wtViewport.rowsRenderCalculator.endRow;
            if (lastVisibleRow > (offset / numBarra) - pagePreview) {
                if (!loadingCarga && !totalData) {
                    cargarDatosFP011Det();
                }
            }
        }*/
    });

    hot011.render();
    if (setData.length > 0) setDataFP011(setData);
    if (modoModel == "consultar") {
        $("#btnGrabarFicha011").hide();
        $("#btnImportarFile").hide();
        $("#btnAgregarBarra").hide();
        hot011.updateSettings({
            cells: function (row, col) {
                var cellProperties = {};
                cellProperties.readOnly = true;
                return cellProperties;
            }
        });
    }
}

function cargarDatosFP011() {
    offset = 0;
    totalData = false
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetItcdf011',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto, offset, pageSize }),

            success: function (response) {
                var hojaAData = response.responseResult;
                itcfp011Codi = hojaAData.Itcdfp011Codi
                offset += hojaAData.ListItcdf011Det.length;
                if (hojaAData.ListItcdf011Det.length < pageSize) {
                    totalData = true;
                }
                setDataHojaFP011(hojaAData);
                cambiosRealizados = false;
                cargarDatosFP011Det();
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
    cambiosRealizados = false;
}

function cargarDatosFP011Det() {
    loadingCarga = true;
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'GetItcdf011Det',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: itcfp011Codi, offset, pageSize }),

        success: function (response) {
            var hojaAData = response.responseResult;
            offset += hojaAData.length;
            if (hojaAData.length < pageSize) {
                totalData = true;
            }
            setDataFP011(hojaAData);
            loadingCarga = false;
            if (!totalData) {
                //const lastVisibleRow = hot011.view.wt.wtViewport.rowsRenderCalculator.endRow;
                const lastVisibleRow = hot011.countRows();
                if (lastVisibleRow > (offset / numBarra) - pagePreview) {
                    cargarDatosFP011Det();
                }
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            loadingCarga = false;
        }
    });
}

function setDataHojaFP011(param) {
    if (!hot011) return; // Evitar errores si la tabla no está creada

    // Obtener los datos actuales
    let data = hot011.getData();

    // Mantener la primera columna (FECHA-HORA) y vaciar el resto
    let nuevaData = data.map(row => [row[0], ...Array(row.length - 1).fill("")]);

    hot011.loadData(nuevaData); // Aplicar la limpieza sin borrar la estructura
    hot011.render(); // Refrescar la tabla

    if (param.NroBarras > 0) {
        // Reemplazar null en Kwval y Kvarval con ""
        param.ListItcdf011Det = param.ListItcdf011Det.map(item => ({
            ...item,
            Kwval: item.Kwval ?? "",  // Si es null, reemplaza con ""
            Kvarval: item.Kvarval ?? "" // Si es null, reemplaza con ""
        }));

        if (numBarra !== param.NroBarras) {
            console.log(`Recreando tabla con ${param.NroBarras} barras`);
            numBarra = param.NroBarras;
            crearTablaITCFP011(numBarra, [], param.ListItcdf011Det);
        } else {
            console.log('Actualizando tabla existente');
            setDataFP011(param.ListItcdf011Det);
        }
    } else {
        console.log("NroBarras es 0 o no definido, no se actualiza la tabla.");
    }
}



function setDataFP011(datosSeteo) {
    if (modoModel == "consultar") {
        desactivarCamposFormulario('fp101');
        $('#btnGrabarFicha011').hide();
        $("#btnImportarFile").hide();
        $("#btnAgregarBarra").hide();
    }

    var hotInstance = hot011.getInstance();
    // hot011.loadData([]);
    var data = hotInstance.getData(); // Obtener los datos actuales de la tabla
    var fechaIndexMap = new Map();

    // Mostrar cargador para evitar bloqueos
    $('#cargador').show();

    // Crear un índice de fechas basado en la tabla actual
    for (var i = 0; i < data.length; i++) {
        fechaIndexMap.set(data[i][0], i);
    }

    // Ordenar y limpiar datos antes de insertarlos
    datosSeteo = datosSeteo
        .filter(d => d.Kwval !== null && d.Kvarval !== null) // Eliminar nulos
        .sort((a, b) => {
            let dateA = new Date(a.FechaHora.split(' ')[0].split('/').reverse().join('-') + 'T' + a.FechaHora.split(' ')[1]);
            let dateB = new Date(b.FechaHora.split(' ')[0].split('/').reverse().join('-') + 'T' + b.FechaHora.split(' ')[1]);
            return dateA - dateB;
        });

    if (datosSeteo.length > 0) {
        let newData = hotInstance.getData(); // Obtener los datos actuales

        // Aplicar los cambios directamente en la matriz
        datosSeteo.forEach(dato => {
            var dataFechaHora = dato.FechaHora;
            if (fechaIndexMap.has(dataFechaHora)) {
                var rowIndex = fechaIndexMap.get(dataFechaHora);
                var columnIndex = (parseInt(dato.BarraNro) * 2) - 1;
                if (columnIndex < newData[rowIndex].length) {
                    newData[rowIndex][columnIndex] = dato.Kwval ? parseFloat(dato.Kwval).toFixed(4) : "";
                    newData[rowIndex][columnIndex + 1] = dato.Kvarval ? parseFloat(dato.Kvarval).toFixed(4) : "";

                }
            }
        });

        // Cargar los datos de una sola vez (MUCHO MÁS RÁPIDO)
        hotInstance.loadData(newData);
    }

    hotInstance.render(); // Refrescar la tabla
    $('#cargador').hide(); // Ocultar cargador
}
// Deshabilitar pegar, cortar y eliminar
hot011.updateSettings({
    beforePaste: () => false,
    beforeCut: () => false,
    beforeRemoveRow: () => false,
    beforeRemoveCol: () => false
});

function setTablaBarra(Data) {
    if (!hot011) {
        // Si la tabla aún no está creada, entonces créala
        var existingData = [];
        crearTablaITCFP011(numBarra, existingData, Data.ListItcdf011Det);
    } else {
        // Si la tabla ya existe, solo actualizar los datos
        setDataFP011(Data.ListItcdf011Det);
    }
}


function getData011() {
    var param = {};
    param.NroBarras = numBarra;
    return param;
}
function getDatos011Det() {
    var datosArray = hot011.getData();
    var headers = hot011.getSettings().nestedHeaders[1].slice(2);
    var datosFiltrados = datosArray.filter(function (row) {
        return (
            row.slice(1).some(function (value, index) {
                return (value != null && value !== "");
            })
        );
    });

    var datosObjetos = datosFiltrados.flatMap(function (row) {
        var detalleBarra = [];
        var j = 1;
        for (let i = 1; i < row.length; i += 2) {
            detalleBarra.push({
                FechaHora: row[0],
                BarraNro: j,
                Kwval: row[i],
                Kvarval: row[i + 1] || null
            });
            j++;
        }
        return detalleBarra;
    });
    return datosObjetos;
}

function grabarItcdf011() {
    if (!validarTablaAntesDeGuardar(hot011)) {
        mostrarNotificacion("No se puede guardar porque hay datos inválidos en la tabla.");
        return; // Detener el guardado si hay errores
    }
    if (!totalData) {
        mostrarNotificacion("Se cargaron parcialmente los datos. Para ver todos los registros, desplácese hacia abajo.");
        return;
    }
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.Itcdfp011DTO = getData011();
        var ListaItcdfp011DetDTO = getDatos011Det();
        param.ListaItcdfp011DetDTO = ListaItcdfp011DetDTO.slice(0, blockSize);
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarItcdfp011',
            data: param,

            success: function (result) {
                if (result.result == 1) {
                    cambiosRealizados = false;
                    var restantes = ListaItcdfp011DetDTO.slice(blockSize); // El resto de los registros
                    if (restantes.length > 0) {
                        grabarItcdf011Det(restantes, result.itcdfp011);
                    } else {
                        mostrarMensaje('mensajeFicha', 'exito', 'Los datos se grabaron correctamente.');
                    }
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
function grabarItcdf011Det(restantes, itcdfp011) {
    var block = restantes.slice(0, blockSize);
    var restante = restantes.slice(blockSize)
    var param = {
        ListaItcdfp011DetDTO: block,
        Itcdfp011DTO: itcdfp011
    };

    $.ajax({
        type: 'POST',
        url: controlador + 'GrabarItcdfp011Det',
        data: param,
        success: function (result) {
            if (result == 1) {
                if (restante.length > 0) {
                    grabarItcdf011Det(restante, itcdfp011);
                }
                else {
                    mostrarMensaje('mensajeFicha', 'exito', 'Los datos se grabaron correctamente.');
                }
                //mostrarMensaje('mensajeFicha', 'exito', 'Los detalles se grabaron correctamente (Bloque ' + (index + 1) + ').');
            } else {
                mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error al guardar los detalles (Bloque ' + (index + 1) + ').');
            }
        },
        error: function () {
            mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error al guardar los detalles (Bloque ' + (index + 1) + ').');
        }
    });
}

function generarListaFechasIntervaloAnual(year, intervaloMinutos) {
    const inicioAnio = new Date(year, 0, 1, 0, 15); // 1 de enero a las 00:15
    const finAnio = new Date(year, 11, 31, 23, 45); // 31 de diciembre a las 23:45
    const intervaloMilisegundos = intervaloMinutos * 60 * 1000; // convertir minutos a milisegundos

    const listaFechas = [];
    for (
        let fecha = inicioAnio;
        fecha <= finAnio;
        fecha = new Date(fecha.getTime() + intervaloMilisegundos)
    ) {
        listaFechas.push(formatearFecha(fecha)); // agregar la fecha formateada
    }

    return listaFechas;
}

function formatearFecha(fecha) {
    const dia = String(fecha.getDate()).padStart(2, "0");
    const mes = String(fecha.getMonth() + 1).padStart(2, "0"); // Los meses van de 0 a 11
    const anio = fecha.getFullYear();
    const horas = String(fecha.getHours()).padStart(2, "0");
    const minutos = String(fecha.getMinutes()).padStart(2, "0");

    return `${dia}/${mes}/${anio} ${horas}:${minutos}`;
}

function exportarTablaExcel011() {
    if (!totalData) {
        mostrarNotificacion("Se cargaron parcialmente los datos. Para ver todos los registros, desplácese hacia abajo.");
        return;
    }
    var nestedHeaders = [
        [
            { label: "", colspan: 1 },
        ].concat(
            Array.from({ length: numBarra }, (_, i) => [{ label: `BARRA[${i + 1}]`, colspan: 1 }, { label: `BARRA[${i + 1}]`, colspan: 1 }]).flat()
        ),
        [
            { label: "", colspan: 1 },
        ].concat(
            Array.from({ length: numBarra }, () => [
                { label: "kV", colspan: 1 },
                { label: "kV", colspan: 1 }
            ]).flat()
        ),
        [
            { label: "FECHA-HORA", colspan: 1 },
        ].concat(
            Array.from({ length: numBarra }, () => [
                { label: "kW", colspan: 1 },
                { label: "kVAR", colspan: 1 }
            ]).flat()
        ),
    ];

    var headers = nestedHeaders.map(row => row.map(col => col.label || ""));

    var datosArray = hot011.getData()
    var data = headers.concat(datosArray);
    console.log(data);
    var ws = XLSX.utils.aoa_to_sheet(data);

    ws["!merges"] = [];

    for (let i = 0; i < numBarra; i++) {
        let startCol = 1 + i * 2; // Calcula la posición inicial para cada barra

        // Combinar celdas en la primera fila ("BARRA[n]")
        ws["!merges"].push({
            s: { r: 0, c: startCol }, // Celda inicial
            e: { r: 0, c: startCol + 1 } // Celda final
        });

        // Combinar celdas en la segunda fila ("kV")
        ws["!merges"].push({
            s: { r: 1, c: startCol },
            e: { r: 1, c: startCol + 1 }
        });
    }
    console.log(ws);
    aplicarBordes(ws);
    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "FP01");
    XLSX.writeFile(wb, "ITC_DemEDE_03-FP01_1.xlsx");
}

function importarExcel011() {
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

            updateTableFromExcel011(jsonData);
            totalData = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}

function updateTableFromExcel011(jsonData) {
    headerB = jsonData[0];
    console.log('json data', jsonData[0]);
    var data = jsonData.slice(3);
    numBarra = (headerB.length - 1) / 2;
    const errores = [];
    var datosArray = hot011.getData();
    var error = false;

    // Validar y preparar los datos
    const datosValidados = data.map((row, rowIndex) => {
        if (datosArray[rowIndex][0] === row[0]) {
            const rowData = row.slice(1).map((value, colIndex) => { // Ignorar la primera columna

                if (value === null || value === "") {
                    return { value: "", valid: true }; // Vacío es válido
                }
                const parsedValue = Number(value);
                if (isNaN(parsedValue)) {
                    errores.push(`Fila ${rowIndex + 1}. Columna ${colIndex + 2}`); // Ajustar índice por la columna excluida
                    return { value: value, valid: false }; // Marcar como inválido, conservar el valor
                }
                const fixedValue = parsedValue.toFixed(4);
                return { value: fixedValue, valid: true }; // Dejar el valor original como válido
            });

            // Agregar un objeto vacío al inicio de la fila
            rowData.unshift({ value: "", valid: true });
            return rowData;

        } else {
            error = true;
            errores.push(`Fila ${rowIndex + 1}. Error formato de fecha, no coincide!`);
            return { valid: false };
        }

    });

    if (error === false) {
        cambiosRealizados = true;
        // Crear la tabla con los datos validados
        crearTablaITCFP011(numBarra, datosValidados.map(row => row.map(cell => cell.value)));

        // Resaltar celdas inválidas sin modificar la data
        datosValidados.forEach((row, rowIndex) => {
            row.forEach((cell, colIndex) => {
                if (!cell.valid) { // Solo celdas inválidas
                    hot011.setCellMeta(rowIndex, colIndex, "valid", false);
                    hot011.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "#ffcccc", // Resaltar con color rojo claro
                    });
                } else {
                    hot011.setCellMeta(rowIndex, colIndex, "valid", true);
                    hot011.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "", // Limpiar estilo si es válido
                    });
                }
            });
        });

        hot011.render(); // Refrescar la tabla
    }

    // Mostrar notificación de errores
    if (errores.length > 0) {
        const erroresLimitados = errores.slice(0, 10); // Mostrar máximo 10 errores
        const mensajeErrores = erroresLimitados.join('\n');
        mostrarNotificacion(
            `Se importaron datos con errores.\nErrores detectados en:\n${mensajeErrores}`
        );
    } else {
        mostrarNotificacion("Los datos se importaron correctamente.", '#a3e352');
    }
}

function agregarBarra() {
    var existingData = hot011.getData(); // Obtener los datos actuales
    const existingValidation = [];

    // Obtener metadatos de validación actuales
    existingData.forEach((row, rowIndex) => {
        existingValidation[rowIndex] = [];
        row.forEach((_, colIndex) => {
            const isValid = hot011.getCellMeta(rowIndex, colIndex).valid;
            existingValidation[rowIndex][colIndex] = isValid === undefined ? true : isValid;
        });
    });

    numBarra += 1;
    crearTablaITCFP011(numBarra, existingData);

    // Restaurar metadatos de validación
    existingValidation.forEach((rowValidation, rowIndex) => {
        rowValidation.forEach((isValid, colIndex) => {
            if (colIndex > 0 && !isValid) { // Validar solo columnas relevantes
                hot011.setCellMeta(rowIndex, colIndex, "valid", false);
                hot011.setCellMeta(rowIndex, colIndex, "style", {
                    backgroundColor: "#ffcccc", // Resaltar en rojo claro
                });
            } else {
                hot011.setCellMeta(rowIndex, colIndex, "valid", true);
                hot011.setCellMeta(rowIndex, colIndex, "style", {
                    backgroundColor: "", // Limpiar el estilo si es válido
                });
            }
        });
    });

    hot011.render(); // Refrescar la tabla
}
