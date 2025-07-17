var hotp012 = null;

$(function () {
    crearTablaITCFP012();

    
});

function crearTablaITCFP012() {
    var data = [];
    for (var i = 0; i <= 50; i++) {
        var row = [];
        row.push("");
        row.push("");
        row.push("");
        row.push("");
        row.push("");
        data.push(row);
    }

    var grilla = document.getElementById("tablefp102");

    if (typeof hotp012 !== "undefined" && hotp012 !== null) {
        hotp012.destroy();
    }

    hotp012 = new Handsontable(grilla, {
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
                { label: "C&Oacute;DIGO </br> SICLI", colspan: 1 },
                { label: "NOMBRE DEL </br> CLIENTE", colspan: 1 },
                { label: "SUBESTACI&Oacute;N(0)", colspan: 1 },
                { label: "BARRA(1)", colspan: 1 },
                { label: "C&Oacute;DIGO NIVEL </br> DE TENSI&Oacute;N(2)", colspan: 1 },
            ],
        ],
        columns: [
            { type: "text", readOnly: true},
            { type: "text", readOnly: true},
            { type: "text", readOnly: true },
            { type: "text", readOnly: true },
            { type: 'dropdown', readOnly: true,
              source: ['BT', 'MT', 'AT', 'MAT'] 
            },
        ],
        afterChange: function (changes, source) {
            if (source === 'loadData') return; // Evitar validación durante la carga de datos

            changes.forEach(([row, col, oldValue, newValue]) => {
                if (newValue == null) return;

                // Verificar si el nuevo valor supera los 120 caracteres
                if (newValue.length > 120) {
                    hotp012.setCellMeta(row, col, "valid", false);
                    hotp012.setCellMeta(row, col, "style", {
                        backgroundColor: "#ffcccc", // Rojo claro para errores
                    });
                } else {
                    hotp012.setCellMeta(row, col, "valid", true);
                    hotp012.setCellMeta(row, col, "style", {
                        backgroundColor: "", // Limpiar estilo previo
                    });
                }

                // Validar solo la columna 4 (CÓDIGO NIVEL DE TENSIÓN)
                if (col === 4) {
                    const opcionesPermitidas = ['BT', 'MT', 'AT', 'MAT'];

                    if (newValue.trim() === "") {
                        hotp012.setCellMeta(row, col, "valid", true);
                        hotp012.setCellMeta(row, col, "style", { backgroundColor: "" });
                    } else if (!opcionesPermitidas.includes(newValue)) {
                        hotp012.setCellMeta(row, col, "valid", false);
                        hotp012.setCellMeta(row, col, "style", { backgroundColor: "#ffcccc" });
                    } else {
                        hotp012.setCellMeta(row, col, "valid", true);
                        hotp012.setCellMeta(row, col, "style", { backgroundColor: "" });
                    }
                }
            });

            hotp012.render(); // Refrescar la tabla para aplicar cambios
        }

    });
    hotp012.render();
}


function getDatosFP012() {
    
    var datosArray = hotp012.getData();
    var datosFiltrados = datosArray.filter(function (row) {
        // Verificar si al menos una columna tiene un valor no vacío
        return row.some(function (value) {
            return value !== "";
        });
    });
    var datosObjetos = datosFiltrados.map(function (row) {
        return {
            CodigoSicli: row[0],          // CÓDIGO SICLI
            NombreCliente: row[1],        // NOMBRE DEL CLIENTE
            Subestacion: row[2],          // SUBESTACIÓN(0)
            Barra: row[3],                // BARRA(1)
            CodigoNivelTension: row[4]    // CÓDIGO NIVEL DE TENSIÓN(2)
        };
    });
    return datosObjetos;
}


function grabarItcdfp012() {
    console.log(hotp012);
    
    if (!validarTablaAntesDeGuardar(hotp012)) {
        mostrarNotificacion("No se puede guardar porque hay datos inválidos en la tabla.");
        return; // Detener el guardado si hay errores
    }
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.Itcdfp012DTOs = getDatosFP012();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarItcdfp012',
            data: param,
            
            success: function (result) {
                if (result==1) {
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

function cargarDatos012() {
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetItcdf012',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            
            success: function (response) {
                var hoja012Data = response.responseResult;
                setDatos012(hoja012Data);
                cambiosRealizados = false;
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function setDatos012(lista012) {
    hotp012.loadData([]);
    // Obtiene los datos actuales de la tabla
    var datosArray = hotp012.getData();
    lista012.forEach(function (item, index) {
        if (!datosArray[index]) {
            datosArray[index] = [];
        }
        datosArray[index][0] = item.CodigoSicli;
        datosArray[index][1] = item.NombreCliente;
        datosArray[index][2] = item.Subestacion;
        datosArray[index][3] = item.Barra;
        datosArray[index][4] = item.CodigoNivelTension;
    });

    // Completa con filas vacías hasta llegar a 50
    while (datosArray.length < 50) {
        var emptyRow = new Array(10).fill("");
        datosArray.push(emptyRow);
    }
    if (modoModel == "consultar") {
        desactivarCamposFormulario('fp102');
        $('#btnGrabarFP102').hide();
    }
    hotp012.loadData(datosArray);


}

function exportarTablaExcel012() {
    var headers = [
        [
            "CÓDIGO SICLI",
            "NOMBRE DEL CLIENTE",
            "SUBESTACION(0)",
            "BARRA(1)",
            "CÓDIGO NIVEL DE TENSIÓN(2)",
        ],
    ];

    var datosArray = hotp012.getData();
    var data = headers.concat(datosArray);

    var ws = XLSX.utils.aoa_to_sheet(data);
    aplicarBordes(ws);
    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Datos");

    XLSX.writeFile(wb, "ITC_DemEDE_04-FP01_2.xlsx");
}

function importarExcel012() {
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

            updateTableFromExcel012(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}

function updateTableFromExcel012(jsonData) {
    var headers = jsonData.slice(0, 1); // Primera fila es el header
    var data = jsonData.slice(1); // Resto de filas con datos

    var opcionesPermitidas = ['BT', 'MT', 'AT', 'MAT'];
    debugger;
    // Procesar datos asegurando que solo se importan 5 columnas y se validan
    var datosValidados = data.map((row, rowIndex) => {
        var newRow = row.slice(0, 5).map((value, colIndex) => {
            let val = String(value || "").trim(); // Convertir a string y eliminar espacios

            // Validar longitud de caracteres
            if (val.length > 120) {
                console.warn(`Valor demasiado largo en fila ${rowIndex + 1}, col ${colIndex + 1}: ${val}`);
                return { value: val, valid: false };
            }

            // Validar columna 4 (CÓDIGO NIVEL DE TENSIÓN)
            if (colIndex === 4) {
                if (val === "" || opcionesPermitidas.includes(val)) {
                    return { value: val, valid: true };
                } else {
                    console.warn(`Valor inválido en fila ${rowIndex + 1}: ${val}`);
                    return { value: val, valid: false };
                }
            }
            return { value: val, valid: true };
        });

        return newRow;
    });

    // Eliminar filas vacías (todas las celdas en blanco)
    datosValidados = datosValidados.filter(row => row.some(cell => cell.value !== ""));

    // Rellenar con filas vacías hasta llegar a 50 filas
    while (datosValidados.length < 50) {
        datosValidados.push(new Array(5).fill({ value: "", valid: true }));
    }

    // Cargar datos en la tabla
    hotp012.loadData(datosValidados.map(row => row.map(cell => cell.value)));

    // Aplicar estilos de validación
    datosValidados.forEach((row, rowIndex) => {
        row.forEach((cell, colIndex) => {
             
                if (!cell.valid) {
                    hotp012.setCellMeta(rowIndex, colIndex, "valid", false);
                    hotp012.setCellMeta(rowIndex, colIndex, "className", "cell-error");
                } else {
                    hotp012.setCellMeta(rowIndex, colIndex, "valid", true);
                    hotp012.setCellMeta(rowIndex, colIndex, "className", "");
                }
            
        });
    });

    hotp012.render(); // Refrescar la tabla para aplicar los cambios

    // Mostrar notificación si hay errores
    const celdasConErrores = [];
    datosValidados.forEach((row, rowIndex) => {
        row.forEach((cell, colIndex) => {
            if (colIndex === 4 && !cell.valid) {
                celdasConErrores.push(`Fila ${rowIndex + 1}`);
            }
        });
    });

    if (celdasConErrores.length > 0) {
        const erroresLimitados = celdasConErrores.slice(0, 10).join('\n');
        mostrarNotificacion(`Se importaron datos con errores en:\n${erroresLimitados}`);
    } else {
        mostrarNotificacion("Los datos se importaron correctamente.", "#a3e352");
    }
}
