var hotred5 = null;
const configuracionColumnasred5 = {
    0: { tipo: "texto", largo: 60 },
    1: { tipo: "texto", largo: 60 },
    2: { tipo: "texto", largo: 60 },
    3: { tipo: "decimal", decimales: 4, permitirNegativo: false }, // Pd(MW): 4 decimales, sin negativos
    4: { tipo: "decimal", decimales: 4, permitirNegativo: false }, // Pn(MW): 4 decimales, sin negativos
    5: { tipo: "decimal", decimales: 4, permitirNegativo: false }, // Qn(min): 4 decimales, sin negativos
    6: { tipo: "decimal", decimales: 4, permitirNegativo: false }  // Qn(max): 4 decimales, sin negativos
};
$(function () {
    crearTablaITCRED5([]);
});




function crearTablaITCRED5(data) {
    var grilla = document.getElementById("detalleFormatoRed5");

    // Si no hay datos, inicializa con 50 filas vacías
    if (data.length === 0) {
        for (var i = 0; i < 50; i++) {
            var row = new Array(7).fill("");
            data.push(row);
        }
    }

    if (typeof hotred5 !== "undefined" && hotred5 !== null) {
        hotred5.destroy();
    }

    hotred5 = new Handsontable(grilla, {
        data: data,
        rowHeaders: true,
        colHeaders: [
            "CIAGEN",
            "IDGEN",
            "BARRA",
            "Pd(MW)",
            "Pn(MW)",
            "Qn(min)",
            "Qn(max)"
        ],
        colWidths: 170,
        manualColumnResize: true,
        autoRowSize: true,
        columnSorting: false,
        filters: false,
        contextMenu: true,
        autoColumnSize: true,
        columns: [
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

                return generalBeforeChange_4(this, changes, configuracionColumnasred5);
            }
        },
        afterChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
                generalAfterChange_global(this, changes, configuracionColumnasred5);
            }
        }
    });

    hotred5.render();
}

function getDatosRED5() {
    var datosArray = hotred5.getData();
    var datosObjetos = datosArray.map(function (row) {
        return {
            CaiGen: row[0],   // CIAGEN
            IdGen: row[1],    // IDGEN
            Barra: row[2],    // BARRA
            PdMw: row[3],    // Pd(MW)
            PnMw: row[4],    // Pn(MW)
            QnMin: row[5],   // Qn(min)
            QnMa: row[6]    // Qn(max)
        };
    }).filter(function (row) {
        // Verificar si al menos una columna tiene un valor no vac�o
        return Object.values(row).some(function (value) {
            return value !== "";
        });
    });
    return datosObjetos;
}

function cargarDatosRed5() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetItcRed5',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            
            success: function (response) {
                console.log(response);
                var hojared5Data = response.responseResult;
                setDatosRed5(hojared5Data);
                cambiosRealizados = false;
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function setDatosRed5(listared5) {
    if (modoModel == "consultar") {
        desactivarCamposFormulario('Fichared5');
        $('#btnGrabarFichared5').hide();
    }
    var datosArray = [];

    // Mapea los datos de la lista a la estructura de la tabla
    listared5.forEach(function (item) {
        var row = [
            item.CaiGen !== null && item.CaiGen !== undefined ? item.CaiGen : "",   // CIAGEN
            item.IdGen !== null && item.IdGen !== undefined ? item.IdGen : "",     // IDGEN
            item.Barra !== null && item.Barra !== undefined ? item.Barra : "",     // BARRA
            item.PdMw !== null && item.PdMw !== undefined ? item.PdMw : "",       // Pd(MW)
            item.PnMw !== null && item.PnMw !== undefined ? item.PnMw : "",       // Pn(MW)
            item.QnMin !== null && item.QnMin !== undefined ? item.QnMin : "",    // Qn(min)
            item.QnMa !== null && item.QnMa !== undefined ? item.QnMa : ""        // Qn(max)
        ];
        datosArray.push(row);
    });

    // Completa con filas vacías hasta llegar a 50
    while (datosArray.length < 50) {
        datosArray.push(new Array(7).fill(""));
    }

    datosArray = formatearDatosGlobal(datosArray, configuracionColumnasred5, mostrarNotificacion);

    // Llama a la función para crear la tabla con los nuevos datos
    crearTablaITCRED5(datosArray);
}

function grabarRed5() {
    if (!validarTablaAntesDeGuardar(hotred5)) {
        mostrarNotificacion("No se puede guardar porque hay datos inválidos en la tabla.");
        return; // Detener el guardado si hay errores
    }
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.ListaItcRed5DTO = getDatosRED5();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarItcred5',
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

function exportarTablaExcelred5() {
    var headers = [
        [
            "CIAGEN",
            "IDGEN",
            "BARRA",
            "Pd(MW)",
            "Pn(MW)",
            "Qn(min)",
            "Qn(max)"
        ],
    ];

    var datosArray = hotred5.getData().map(row => {
        return row.map((value, index) => {
            if (index >= 3 && index <= 6) {
                // Formatea los campos numéricos con 4 decimales
                return value !== null && value !== "" ? parseFloat(value).toFixed(4) : "";
            }
            return value; // Otros campos permanecen igual
        });
    });

    var data = headers.concat(datosArray);

    var ws = XLSX.utils.aoa_to_sheet(data);
    aplicarBordes(ws);
    // Ajustar ancho de columnas para mejor visualización en Excel
    ws["!cols"] = [
        { wch: 10 }, // CIAGEN
        { wch: 10 }, // IDGEN
        { wch: 10 }, // BARRA
        { wch: 10 }, // Pd(MW)
        { wch: 10 }, // Pn(MW)
        { wch: 10 }, // Qn(min)
        { wch: 10 }, // Qn(max)
    ];

    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "RED5");

    XLSX.writeFile(wb, "ITC_SEyP_07-RED5.xlsx");
}

function importarExcelred5() {
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

            updateTableFromExcelred5(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}

function updateTableFromExcelred5(jsonData) {
    var headers = jsonData.slice(0, 1); // La primera fila contiene los headers
    var data = jsonData.slice(1); // Las filas restantes contienen los datos


    // Validar y formatear los datos importados
    const datosValidados = validarYFormatearDatos2(data, configuracionColumnasred5, mostrarNotificacion, hotred5);
    while (datosValidados.length < 50) {
        datosValidados.push(new Array(7).fill("")); // Agregar filas vacías
    }
    // Cargar los datos validados en la tabla, incluyendo errores
    hotred5.loadData(datosValidados);

    // Verificar errores después de la validación
    const celdasConErrores = [];
    datosValidados.forEach((row, rowIndex) => {
        row.forEach((value, colIndex) => {
            const config = configuracionColumnasred5[colIndex];
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
                    hotred5.setCellMeta(rowIndex, colIndex, "valid", false);
                    hotred5.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "#ffcccc", // Rojo claro
                    });
                } else {
                    // Si es válido, limpiar estilo previo
                    hotred5.setCellMeta(rowIndex, colIndex, "valid", true);
                    hotred5.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "",
                    });
                }
            }
        });
    });

    hotred5.render(); // Refrescar tabla

    if (celdasConErrores.length > 0) {
        mostrarNotificacion(
            `Se importaron datos con errores.<br>Errores detectados en:<br>${celdasConErrores.join("<br>")}`
        );
    } else {
        mostrarNotificacion("Los datos se importaron correctamente.", '#a3e352');
    }
}
