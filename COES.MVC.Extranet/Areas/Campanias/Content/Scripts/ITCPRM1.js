var hotprm1 = null;
const configuracionColumnasprm1 = {
    0: { tipo: "texto", largo: 100 },
    1: { tipo: "texto", largo: 100 },
    2: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    3: { tipo: "texto", largo: 100 },
    4: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    5: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    6: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    7: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    8: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    9: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    10: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    11: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    12: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    13: { tipo: "decimal", decimales: 4, permitirNegativo: false }
     
};
$(function () {
    crearTablaITCPRM1([]);
    //cargarDatosPrm1();

    var formulariob = document.getElementById('Fichaprm1');
    formulariob.addEventListener('change', function () {
        cambiosRealizados = true;
        console.log(cambiosRealizados);
    });

});
$('#btnGrabarFichaprm1').on('click', function () {
    grabarPrm1();
});

function generalAfterChange(hotInstance, changes, configuracionColumnasprm1) {
    changes.forEach(([row, col, oldValue, newValue]) => {
        const config = configuracionColumnasprm1[col]; // Configuración basada en la columna
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

function crearTablaITCPRM1(data) {
    var grilla = document.getElementById("detalleFormatoPrm1");

    // Si no hay datos, inicializa con 50 filas vacías
    if (data.length === 0) {
        for (var i = 0; i < 50; i++) {
            var row = new Array(14).fill("");
            data.push(row);
        }
    }

    if (hotprm1 !== null) {
        hotprm1.destroy();
    }

    hotprm1 = new Handsontable(grilla, {
        data: data,
        rowHeaders: true,
        colHeaders: true,
        colWidths: 170,
        manualColumnResize: true,
        columnSorting: false,
        filters: false,
        contextMenu: true,
        autoColumnSize: true,
        autoRowSize: true,
        //viewportRowRenderingOffset: data.length, 
        nestedHeaders: [
            [
                "ELECTRODUCTO",
                "DESCRIPCION",
                "Vn(KV)",
                "TIPO",
                "SECCION(mm2)",
                "CTR(1/°C)",
                "R(Ohm/km)",
                "X(Ohm/km)",
                "B(uS/km)",
                "Ro(Ohm/km)",
                "Xo(Ohm/km)",
                "Bo(uS/km)",
                "Capacidad(A)",
                "TmxOp(°C)"
            ],
        ],
        columns: [
           { type: "text" },
           { type: "text"}, 
           { type: "text" },
           { type: "text"}, 
           { type: "text" },
            { type: "text" },
            { type: "text" },
            { type: "text" },
            { type: "text" },
            { type: "text" },
            { type: "text" },
            { type: "text" },
            { type: "text" },
            { type: "text" }
        ],
        beforeChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
              
                return generalBeforeChange_4(this, changes, configuracionColumnasprm1);
            }
        },
        afterChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
                generalAfterChange_global(this, changes, configuracionColumnasprm1);
            }
        }
    });
    hotprm1.render();
 
}

function getDatosPRM1() {
    var datosArray = hotprm1.getData();
    var datosObjetos = datosArray.map(function (row) {
        return {
            Electroducto: row[0], // ELECTRODUCTO
            Descripcion: row[1],  // DESCRIPCIÓN
            Vn: row[2],           // Vn(KV)
            Tipo: row[3],         // TIPO
            Seccion: row[4],      // SECCION(mm2)
            Ctr: row[5],          // CTR(1/C)
            R: row[6],            // R(Ohm/km)
            X: row[7],            // X(Ohm/km)
            B: row[8],            // B(uS/km)
            Ro: row[9],           // Ro(Ohm/km)
            Xo: row[10],          // Xo(Ohm/km)
            Bo: row[11],          // Bo(uS/km)
            Capacidad: row[12],   // Capacidad(A)
            Tmxop: row[13],       // TmxOp(C)
        };
    }).filter(function (row) {
        // Verificar si al menos una columna tiene un valor no vacío
        return Object.values(row).some(function
            (value) {
            return value !== "";
        });
    });
    return datosObjetos;
}

function grabarPrm1() {
    var botonGrabar = $('#btnGrabarFichaprm1'); // Referencia al botón

    if (!validarTablaAntesDeGuardar(hotprm1)) {
        mostrarNotificacion("No se puede guardar porque hay datos inválidos en la tabla.");
        return; // Detener el guardado si hay errores
    }

    // Deshabilitar el botón para evitar múltiples clics
    botonGrabar.prop('disabled', true);

    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.ListaItcPrm1DTO = getDatosPRM1();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarItcprm1',
            data: param,
            success: function (result) {
                console.log(result);
                if (result == 1) {
                    mostrarMensaje('mensajeFicha', 'exito', 'Los datos se grabaron correctamente.');
                    cambiosRealizados = false;
                } else {
                    mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
                }
                // Volver a habilitar el botón
                botonGrabar.prop('disabled', false);
            },
            error: function () {
                mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
                // Volver a habilitar el botón incluso si hay error
                botonGrabar.prop('disabled', false);
            }
        });
    } else {
        mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
        // Volver a habilitar el botón en caso de error de validación
        botonGrabar.prop('disabled', false);
    }
}

function cargarDatosPrm1() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetItcPrm1',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            
            success: function (response) {
                console.log(response);
                var hojaprm1Data = response.responseResult;
                setDatosprm1(hojaprm1Data);
                cambiosRealizados = false;
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function setDatosprm1(listaprm1) {
    if (modoModel == "consultar") {
        desactivarCamposFormulario('Fichaprm1');
        $('#btnGrabarFichaprm1').hide();
        $('#btnImportarExcelPRM1').hide();
    }
    var datosArray = [];

    // Mapea los datos de la lista a la estructura de la tabla
    listaprm1.forEach(function (item) {
        var row = [
            item.Electroducto, // ELECTRODUCTO
            item.Descripcion,  // DESCRIPCIÓN
            item.Vn,           // Vn(KV)
            item.Tipo,         // TIPO
            item.Seccion,      // SECCION(mm2)
            item.Ctr,          // CTR(1/ºC)
            item.R,            // R(Ohm/km)
            item.X,            // X(Ohm/km)
            item.B,            // B(uS/km)
            item.Ro,           // Ro(Ohm/km)
            item.Xo,           // Xo(Ohm/km)
            item.Bo,           // Bo(uS/km)
            item.Capacidad,    // Capacidad(A)
            item.Tmxop         // TmxOp(ºC)
        ];
        datosArray.push(row);
    });

    // Completa con filas vacías hasta llegar a 50
    while (datosArray.length < 50) {
        var emptyRow = new Array(14).fill("");
        datosArray.push(emptyRow);
    }
    datosArray = formatearDatosGlobal(datosArray, configuracionColumnasprm1);

    crearTablaITCPRM1(datosArray);
}

function exportarTablaExcelprm1() {
    var headers = [
        [
            "ELECTRODUCTO",
            "DESCRIPCION",
            "Vn(KV)",
            "TIPO",
            "SECCION(mm2)",
            "CTR(1/ºC)",
            "R(Ohm/km)",
            "X(Ohm/km)",
            "B(uS/km)",
            "Ro(Ohm/km)",
            "Xo(Ohm/km)",
            "Bo(uS/km)",
            "Capacidad(A)",
            "TmxOp(ºC)"
        ],
    ];

    var datosArray = hotprm1.getData();
    var data = headers.concat(datosArray);

    var ws = XLSX.utils.aoa_to_sheet(data);
    aplicarBordes(ws);
    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "PRM1");

    XLSX.writeFile(wb, "ITC_SEyP_01-PRM1.xlsx");
}

function importarExcelprm1() {
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

            updateTableFromExcelprm1(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}

function updateTableFromExcelprm1(jsonData) {
    var headers = jsonData.slice(0, 1); // Las dos primeras filas son los headers
    var data = jsonData.slice(1); // El resto son los datos

    // Validar y formatear los datos importados
    const datosValidados = validarYFormatearDatos2(data, configuracionColumnasprm1, mostrarNotificacion, hotprm1);
    // Asegurar que haya al menos 50 filas después de la importación
    while (datosValidados.length < 50) {
        datosValidados.push(new Array(14).fill("")); // Agregar filas vacías
    }
    // Cargar los datos validados en la tabla, incluyendo errores
    hotprm1.loadData(datosValidados);

    // Verificar errores después de la validación
    const celdasConErrores = [];
    datosValidados.forEach((row, rowIndex) => {
        row.forEach((value, colIndex) => {
            const config = configuracionColumnasprm1[colIndex];
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
                    hotprm1.setCellMeta(rowIndex, colIndex, "valid", false);
                    hotprm1.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "#ffcccc", // Rojo claro
                    });
                } else {
                    // Si es válido, limpiar estilo previo
                    hotprm1.setCellMeta(rowIndex, colIndex, "valid", true);
                    hotprm1.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "",
                    });
                }
            }
        });
    });

    hotprm1.render(); // Refrescar tabla

    if (celdasConErrores.length > 0) {
        mostrarNotificacion(
            `Se importaron datos con errores.<br>Errores detectados en:<br>${celdasConErrores.join("<br>")}`
        );
    } else {
        mostrarNotificacion("Los datos se importaron correctamente.", '#a3e352');
    }
}
