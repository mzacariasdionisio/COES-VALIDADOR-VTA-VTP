var hotred1 = null;
const configuracionColumnasred1 = {
    0: { tipo: "texto", largo: 60 },
    1: { tipo: "entero", permitirNegativo: false },
    2: { tipo: "entero", permitirNegativo: false },
    3: { tipo: "texto", largo: 60 },
    
};
$(function () {
    crearTablaITCRED1([]);

});

function crearTablaITCRED1(data) {
    var grilla = document.getElementById("detalleFormatoRed1");
    // Si no hay datos, inicializa con 50 filas vacías
    if (data.length === 0) {
        for (var i = 0; i < 50; i++) {
            var row = new Array(4).fill("");
            data.push(row);
        }
    }

    if (typeof hotred1 !== "undefined" && hotred1 !== null) {
        hotred1.destroy();
    }

    hotred1 = new Handsontable(grilla, {
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
                "BARRA",
                "Vn(kV)",
                "Vo(pu)",
                "Tipo"
            ],
        ],
        columns: [
           { type: "text" },
            { type: "text" },
            { type: "text" }, 
           { type: "text"}, 
        ],
        beforeChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
              
                return generalBeforeChange_4(this, changes, configuracionColumnasred1);
            }
        },
        afterChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
                generalAfterChange_global(this, changes, configuracionColumnasred1);
            }
        }

    });
    hotred1.render();
}


function getDatosRED1() {
    var datosArray = hotred1.getData();
    var datosObjetos = datosArray.map(function (row) {
        return {
            Barra: row[0],     // BARRA
            Vnpu: row[1],     // Vn(kV)
            Vopu: row[2],     // Vo(pu)
            Tipo: row[3],      // Tipo
        };
    }).filter(function (row) {
        // Verificar si al menos una columna tiene un valor no vacío
        return Object.values(row).some(function (value) {
            return value !== "";
        });
    });
    return datosObjetos;
}

function cargarDatosRed1() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetItcRed1',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            
            success: function (response) {
                console.log(response);
                var hojared1Data = response.responseResult;
                setDatosRed1(hojared1Data);
                cambiosRealizados = false;
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function setDatosRed1(listared1) {
 
    if (modoModel == "consultar") {
        desactivarCamposFormulario('Fichared1');
        $('#btnGrabarFichared1').hide();
    }
    var datosArray = [];

    // Mapea los datos de la lista a la estructura de la tabla
    listared1.forEach(function (item) {
        var row = [
            item.Barra !== null && item.Barra !== undefined ? item.Barra : "",  // BARRA
            item.Vnpu !== null && item.Vnpu !== undefined ? item.Vnpu : "",   // Vn(kV)
            item.Vopu !== null && item.Vopu !== undefined ? item.Vopu : "",   // Vo(pu)
            item.Tipo !== null && item.Tipo !== undefined ? item.Tipo : ""    // Tipo
        ];
        datosArray.push(row);
    });

    // Completa con filas vacías hasta llegar a 50
    while (datosArray.length < 50) {
        var emptyRow = new Array(4).fill("");
        datosArray.push(emptyRow);
    }
    datosArray = formatearDatosGlobal(datosArray, configuracionColumnasred1);
    // Crea la tabla con los nuevos datos
    crearTablaITCRED1(datosArray);
}

function grabarRed1() {
    if (!validarTablaAntesDeGuardar(hotred1)) {
        mostrarNotificacion("No se puede guardar porque hay datos inválidos en la tabla.");
        return; // Detener el guardado si hay errores
    }
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.ListaItcRed1DTO = getDatosRED1();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarItcred1',
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

function exportarTablaExcelred1() {
    var headers = [
        [
            "BARRA",
            "Vn(kV)",
            "Vo(pu)",
            "Tipo"
        ],
    ];

    var datosArray = hotred1.getData();
    var data = headers.concat(datosArray);

    var ws = XLSX.utils.aoa_to_sheet(data);
    aplicarBordes(ws);
    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "RED1");

    XLSX.writeFile(wb, "ITC_SEyP_03-RED1.xlsx");
}

function importarExcelred1() {
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

            updateTableFromExcelred1(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}

function updateTableFromExcelred1(jsonData) {
    var headers = jsonData.slice(0, 1); // Las dos primeras filas son los headers
    var data = jsonData.slice(1); // El resto son los datos

    // Validar y formatear los datos importados
    const datosValidados = validarYFormatearDatos2(data, configuracionColumnasred1, mostrarNotificacion, hotred1);

    while (datosValidados.length < 50) {
        datosValidados.push(new Array(4).fill("")); // Agregar filas vacías
    }
    // Cargar los datos validados en la tabla, incluyendo errores
    hotred1.loadData(datosValidados);

    // Verificar errores después de la validación
    const celdasConErrores = [];
    datosValidados.forEach((row, rowIndex) => {
        row.forEach((value, colIndex) => {
            const config = configuracionColumnasred1[colIndex];
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
                    hotred1.setCellMeta(rowIndex, colIndex, "valid", false);
                    hotred1.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "#ffcccc", // Rojo claro
                    });
                } else {
                    // Si es válido, limpiar estilo previo
                    hotred1.setCellMeta(rowIndex, colIndex, "valid", true);
                    hotred1.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "",
                    });
                }
            }
        });
    });

    hotred1.render(); // Refrescar tabla

    if (celdasConErrores.length > 0) {
        mostrarNotificacion(
            `Se importaron datos con errores.<br>Errores detectados en:<br>${celdasConErrores.join("<br>")}`
        );
    } else {
        mostrarNotificacion("Los datos se importaron correctamente.", '#a3e352');
    }
}
