var hotred4 = null;
const configuracionColumnasred4 = {
    0: { tipo: "texto", largo: 60 },
    1: { tipo: "texto", largo: 60 },
    2: { tipo: "texto", largo: 60 },
    3: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    4: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    5: { tipo: "entero",  permitirNegativo: false },
    6: { tipo: "entero", permitirNegativo: false },
    
     
};
$(function () {
    crearTablaITCRED4([]);

});

function crearTablaITCRED4(data) {
    var grilla = document.getElementById("detalleFormatoRed4");

    // Si no hay datos, inicializa con 50 filas vacías
    if (data.length === 0) {
        for (var i = 0; i < 50; i++) {
            var row = new Array(7).fill("");
            data.push(row);
        }
    }

    if (typeof hotred4 !== "undefined" && hotred4 !== null) {
        hotred4.destroy();
    }

    hotred4 = new Handsontable(grilla, {
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
                "IDCMP",
                "BARRA",
                "Tipo",
                "Vn(kV)",
                "Cap(Mvar)",
                "N° Pasos",
                "PasoAct"   
            ],
        ],
        columns: [
           { type: "text" },
           { type: "text" },
           { type: "text" },
            { type: "text"},
            { type: "text" },
           { type: "text" },
           { type: "text" },
        ],
        beforeChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {

                return generalBeforeChange_4(this, changes, configuracionColumnasred4);
            }
        },
        afterChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
                generalAfterChange_global(this, changes, configuracionColumnasred4);
            }
        }
    
    });
    hotred4.render();
}

function getDatosRED4() {
    var datosArray = hotred4.getData();
    var datosObjetos = datosArray.map(function (row) {
        return {
            IdCmp: row[0],        // IDCMP
            Barra: row[1],        // BARRA
            Tipo: row[2],         // Tipo
            Vnkv: row[3],        // Vn(kV)
            CapmVar: row[4],     // Cap(Mvar)
            Npasos: row[5],      // Nº Pasos
            PasoAct: row[6]      // PasoAct
        };
    }).filter(function (row) {
        // Verificar si al menos una columna tiene un valor no vacío
        return Object.values(row).some(function (value) {
            return value !== "";
        });
    });
    return datosObjetos;
}


function cargarDatosRed4() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetItcRed4',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            
            success: function (response) {
                console.log(response);
                var hojared4Data = response.responseResult;
                setDatosRed4(hojared4Data);
                cambiosRealizados = false;
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function setDatosRed4(listared4) {
    if (modoModel == "consultar") {
        desactivarCamposFormulario('Fichared4');
        $('#btnGrabarFichared4').hide();
    }
    var datosArray = [];

    // Mapea los datos de la lista a la estructura de la tabla
    listared4.forEach(function (item) {
        var row = [
            item.IdCmp !== null && item.IdCmp !== undefined ? item.IdCmp : "",       // IDCMP
            item.Barra !== null && item.Barra !== undefined ? item.Barra : "",      // BARRA
            item.Tipo !== null && item.Tipo !== undefined ? item.Tipo : "",         // Tipo
            item.Vnkv !== null && item.Vnkv !== undefined ? item.Vnkv : "",         // Vn(kV)
            item.CapmVar !== null && item.CapmVar !== undefined ? item.CapmVar : "",// Cap(Mvar)
            item.Npasos !== null && item.Npasos !== undefined ? item.Npasos : "",   // Nº Pasos
            item.PasoAct !== null && item.PasoAct !== undefined ? item.PasoAct : "" // PasoAct
        ];
        datosArray.push(row);
    });

    // Completa con filas vacías hasta llegar a 50
    while (datosArray.length < 50) {
        datosArray.push(new Array(7).fill(""));
    }

    datosArray = formatearDatosGlobal(datosArray, configuracionColumnasred4, mostrarNotificacion);
    // Reconstruye la tabla con los datos nuevos
    crearTablaITCRED4(datosArray);
}

function grabarRed4() {
    if (!validarTablaAntesDeGuardar(hotred4)) {
        mostrarNotificacion("No se puede guardar porque hay datos inválidos en la tabla.");
        return; // Detener el guardado si hay errores
    }
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.ListaItcRed4DTO = getDatosRED4();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarItcred4',
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

function exportarTablaExcelred4() {
    var headers = [
        [
            "IDCMP",
            "BARRA",
            "Tipo",
            "Vn(kV)",
            "Cap(Mvar)",
            "Nº Pasos",
            "PasoAct" 
        ],
    ];

    var datosArray = hotred4.getData();
    var data = headers.concat(datosArray);

    var ws = XLSX.utils.aoa_to_sheet(data);
    aplicarBordes(ws);
    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "RED4");

    XLSX.writeFile(wb, "ITC_SEyP_06-RED4.xlsx");
}

function importarExcelred4() {
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

            updateTableFromExcelred4(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}

function updateTableFromExcelred4(jsonData) {
    var headers = jsonData.slice(0, 1); // Las dos primeras filas son los headers
    var data = jsonData.slice(1); // El resto son los datos

    // Validar y formatear los datos importados
    const datosValidados = validarYFormatearDatos2(data, configuracionColumnasred4, mostrarNotificacion, hotred4);
    while (datosValidados.length < 50) {
        datosValidados.push(new Array(7).fill("")); // Agregar filas vacías
    }
    // Cargar los datos validados en la tabla, incluyendo errores
    hotred4.loadData(datosValidados);

    // Verificar errores después de la validación
    const celdasConErrores = [];
    datosValidados.forEach((row, rowIndex) => {
        row.forEach((value, colIndex) => {
            const config = configuracionColumnasred4[colIndex];
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
                    hotred4.setCellMeta(rowIndex, colIndex, "valid", false);
                    hotred4.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "#ffcccc", // Rojo claro
                    });
                } else {
                    // Si es válido, limpiar estilo previo
                    hotred4.setCellMeta(rowIndex, colIndex, "valid", true);
                    hotred4.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "",
                    });
                }
            }
        });
    });

    hotred4.render(); // Refrescar tabla

    if (celdasConErrores.length > 0) {
        mostrarNotificacion(
            `Se importaron datos con errores.<br>Errores detectados en:<br>${celdasConErrores.join("<br>")}`
        );
    } else {
        mostrarNotificacion("Los datos se importaron correctamente.", '#a3e352');
    }
}
