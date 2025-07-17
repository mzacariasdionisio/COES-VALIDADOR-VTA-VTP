var hotred3 = null;
const configuracionColumnasred3 = {
    0: { tipo: "texto", largo: 60 },
    1: { tipo: "texto", largo: 60 },
    2: { tipo: "texto", largo: 60 },
    3: { tipo: "texto", largo: 60 },
    4: { tipo: "texto", largo: 60 },
    5: { tipo: "texto", largo: 60 },
6: { tipo: "decimal", decimales: 4, permitirNegativo: false }
};
$(function () {
    crearTablaITCRED3([]);

});

function crearTablaITCRED3(data) {
    var grilla = document.getElementById("detalleFormatoRed3");

    // Si no hay datos, inicializa con 50 filas vac�as
    if (data.length === 0) {
        for (var i = 0; i < 50; i++) {
            var row = new Array(7).fill("");
            data.push(row);
        }
    }

   

    if (typeof hotred3 !== "undefined" && hotred3 !== null) {
        hotred3.destroy();
    }

    hotred3 = new Handsontable(grilla, {
        data: data,
        rowHeaders: true,
        colHeaders: true,
        colWidths: 170,
        manualColumnResize: true,
        autoRowSize: true,
        columnSorting: false,
        filters: false,
        contextMenu: true,
        autoColumnSize: true,
        nestedHeaders: [
            [
                "IDCIRCUITO",
                "BARRA[P]",
                "BARRA[S]",
                "BARRA[T]",
                "CDGTRAFO",
                "OPR(TAP)",
                "POS(TAP)" 
            ],
        ],
        columns: [
           { type: "text" },
           { type: "text" },
           { type: "text" },
           { type: "text" },
           { type: "text" },
           { type: "text" },
           { type: "text" },
        ],
        beforeChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {

                return generalBeforeChange_4(this, changes, configuracionColumnasred3);
            }
        },
        afterChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
                generalAfterChange_global(this, changes, configuracionColumnasred3);
            }
        }
    
    });
    hotred3.render();
}
function getDatosRED3() {
    var datosArray = hotred3.getData();
    var datosObjetos = datosArray.map(function (row) {
        return {
            IdCircuito: row[0],     // IDCIRCUITO
            BarraP: row[1],        // BARRA[P]
            BarraS: row[2],        // BARRA[S]
            BarraT: row[3],        // BARRA[T]
            CdgTrafo: row[4],       // CDGTRAFO
            OprTap: row[5],        // OPR(TAP)
            PosTap: row[6]         // POS(TAP)
        };
    }).filter(function (row) {
        // Verificar si al menos una columna tiene un valor no vac�o
        return Object.values(row).some(function (value) {
            return value !== "";
        });
    });
    return datosObjetos;
}

function cargarDatosRed3() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetItcRed3',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            
            success: function (response) {
                console.log(response);
                var hojared3Data = response.responseResult;
                setDatosRed3(hojared3Data);
                cambiosRealizados = false;
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function setDatosRed3(listared3) {
    if (modoModel == "consultar") {
        desactivarCamposFormulario('Fichared3');
        $('#btnGrabarFichared3').hide();
    }
    var datosArray = [];

    // Mapea los datos de la lista a la estructura de la tabla
    listared3.forEach(function (item) {
        var row = [
            item.IdCircuito !== null && item.IdCircuito !== undefined ? item.IdCircuito : "",   // IDCIRCUITO
            item.BarraP !== null && item.BarraP !== undefined ? item.BarraP : "",              // BARRA[P]
            item.BarraS !== null && item.BarraS !== undefined ? item.BarraS : "",              // BARRA[S]
            item.BarraT !== null && item.BarraT !== undefined ? item.BarraT : "",              // BARRA[T]
            item.CdgTrafo !== null && item.CdgTrafo !== undefined ? item.CdgTrafo : "",        // CDGTRAFO
            item.OprTap !== null && item.OprTap !== undefined ? item.OprTap : "",              // OPR(TAP)
            item.PosTap !== null && item.PosTap !== undefined ? item.PosTap : ""               // POS(TAP)
        ];
        datosArray.push(row);
    });

    // Completa con filas vacías hasta llegar a 50
    while (datosArray.length < 50) {
        datosArray.push(new Array(7).fill(""));
    }
    datosArray = formatearDatosGlobal(datosArray, configuracionColumnasred3, mostrarNotificacion);
    // Reconstruye la tabla con los datos nuevos
    crearTablaITCRED3(datosArray);
}

function grabarRed3() {
    if (!validarTablaAntesDeGuardar(hotred3)) {
        mostrarNotificacion("No se puede guardar porque hay datos inválidos en la tabla.");
        return; // Detener el guardado si hay errores
    }
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.ListaItcRed3DTO = getDatosRED3();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarItcred3',
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

function exportarTablaExcelred3() {
    var headers = [
        [
            "IDCIRCUITO",
            "BARRA[P]",
            "BARRA[S]",
            "BARRA[T]",
            "CDGTRAFO",
            "OPR(TAP)",
            "POS(TAP)" 
        ],
    ];

    var datosArray = hotred3.getData();
    var data = headers.concat(datosArray);

    var ws = XLSX.utils.aoa_to_sheet(data);
    aplicarBordes(ws);
    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "RED3");

    XLSX.writeFile(wb, "ITC_SEyP_05-RED3.xlsx");
}

function importarExcelred3() {
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

            updateTableFromExcelred3(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}

function updateTableFromExcelred3(jsonData) {
    var headers = jsonData.slice(0, 1); // Las dos primeras filas son los headers
    var data = jsonData.slice(1); // El resto son los datos

    // Validar y formatear los datos importados
    const datosValidados = validarYFormatearDatos2(data, configuracionColumnasred3, mostrarNotificacion, hotred3);
    while (datosValidados.length < 50) {
        datosValidados.push(new Array(7).fill("")); // Agregar filas vacías
    }
    // Cargar los datos validados en la tabla, incluyendo errores
    hotred3.loadData(datosValidados);

    // Verificar errores después de la validación
    const celdasConErrores = [];
    datosValidados.forEach((row, rowIndex) => {
        row.forEach((value, colIndex) => {
            const config = configuracionColumnasred3[colIndex];
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
                    hotred3.setCellMeta(rowIndex, colIndex, "valid", false);
                    hotred3.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "#ffcccc", // Rojo claro
                    });
                } else {
                    // Si es válido, limpiar estilo previo
                    hotred3.setCellMeta(rowIndex, colIndex, "valid", true);
                    hotred3.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "",
                    });
                }
            }
        });
    });

    hotred3.render(); // Refrescar tabla

    if (celdasConErrores.length > 0) {
        mostrarNotificacion(
            `Se importaron datos con errores.<br>Errores detectados en:<br>${celdasConErrores.join("<br>")}`
        );
    } else {
        mostrarNotificacion("Los datos se importaron correctamente.", '#a3e352');
    }
}
