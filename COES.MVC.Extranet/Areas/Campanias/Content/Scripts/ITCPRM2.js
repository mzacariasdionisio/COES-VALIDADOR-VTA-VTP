var hotprm2 = null;
var blockSize = 150;

const configuracionColumnasprm2 = {
    0: { tipo: "texto", largo: 60 },
    1: { tipo: "texto", largo: 60 },
    2: { tipo: "entero", permitirNegativo: false },
    3: { tipo: "entero", permitirNegativo: false },
    4: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    5: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    6: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    7: { tipo: "texto", largo: 120 },
    8: { tipo: "texto", largo: 100 },
    9: { tipo: "texto", largo: 100 },
    10: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    11: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    12: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    13: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    14: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    15: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    16: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    17: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    18: { tipo: "texto", largo: 60 },
    19: { tipo: "texto", largo: 60 },
    20: { tipo: "texto", largo: 60 },
    //13: { tipo: "entero", permitirNegativo: false },
    21: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    22: { tipo: "entero", permitirNegativo: false },
    23: { tipo: "entero", permitirNegativo: false },
    24: { tipo: "entero", permitirNegativo: false }, 
};
$(function () {
    crearTablaITCPRM2([]);
    var formulariob = document.getElementById('tablaprm2');
    formulariob.addEventListener('change', function () {
        
        cambiosRealizados = true;
        console.log(cambiosRealizados);
    });
});
function generalAfterChange(hotInstance, changes, configuracionColumnasprm2) {
    changes.forEach(([row, col, oldValue, newValue]) => {
        const config = configuracionColumnasprm2[col]; // Configuración basada en la columna
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

function crearTablaITCPRM2(data) {
    var grilla = document.getElementById("detalleFormatoPrm2");

    // Si no hay datos, inicializa con 50 filas vacías
    if (data.length === 0) {
        for (var i = 0; i < 50; i++) {
            var row = new Array(25).fill("");
            data.push(row);
        }
    }
    

    if (typeof hotprm2 !== "undefined" && hotprm2 !== null) {
        hotprm2.destroy();
    }

    hotprm2 = new Handsontable(grilla, {
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
        nestedHeaders: [
            [
                "TRANSFORMADOR",
                "TIPO",
                "FASES",
                "N°DVN",
                "Vn(p)",
                "Vn(s)",
                "Vn(t)",
                "Pn(p)",
                "Pn(s)",
                "Pn(t)",
                "Tcc(p-s)",
                "Tcc(s-t)",
                "Tcc(t-p)",
                "Pcu(p-s)",
                "Pcu(s-t)",
                "Pcu(t-p)",
                "Pfe",
                "I(vacio)",
                "GrpCnx",
                "Tap(tipo)",
                "Tap(lado)",
                "Tap(dV)",
                "Tap(min)",
                "Tap(cnt)",
                "Tap(max)",
            ],
        ],
        columns: [
            { type:"text"},
            { type:"text"}, 
            { type:"text"},
            { type:"text"}, 
            { type: "text"},
            { type: "text"},
            { type: "text"},
            { type: "text"},
            { type: "text"},
            { type: "text"},
            { type: "text"},
            { type: "text"},
            { type: "text"},
            { type: "text"},
            { type: "text"},
            { type: "text"},
            { type: "text"},
            { type: "text"},
           { type: "text"}, 
           { type: "text"}, 
           { type: "text"}, 
            { type: "text"},
           { type:"text"}, 
           { type:"text"}, 
           { type:"text"}, 
        ],
        beforeChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
                 
                return generalBeforeChange_4(this, changes, configuracionColumnasprm2);
            }
        },
        afterChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
                generalAfterChange(this, changes, configuracionColumnasprm2);
            }
        },
        stretchH: "all",
        autoWrapRow: true, // Habilita filas adicionales dinámicamente
        maxRows: Infinity, // Permite crecer la tabla
        autoWrapCol: true,

    });
    hotprm2.render();
}

function validateInteger(value, cellProperties) {
    // Ensure value is an integer
    if (!Number.isInteger(value)) {
        cellProperties.className = 'htInvalid';
        return false;
    }
    cellProperties.className = '';
    return true;
}

function getDatosPRM2() {
    var datosArray = hotprm2.getData();
    var datosObjetos = datosArray.map(function (row) {
        return {
            Transformador: row[0],  // TRANSFORMADOR
            Tipo: row[1],           // TIPO
            Fases: row[2],          // FASES
            Ndvn: row[3],         // NDVN
            Vnp: row[4],           // Vn(p)
            Vns: row[5],           // Vn(s)
            Vnt: row[6],           // Vn(t)
            Pnp: row[7],           // Pn(p)
            Pns: row[8],           // Pn(s)
            Pnt: row[9],           // Pn(t)
            Tccps: row[10],        // Tcc(p-s)
            Tccst: row[11],        // Tcc(s-t)
            Tcctp: row[12],        // Tcc(t-p)
            Pcups: row[13],        // Pcu(p-s)
            Pcust: row[14],        // Pcu(s-t)
            Pcutp: row[15],        // Pcu(t-p)
            Pfe: row[16],           // Pfo
            Ivacio: row[17],       // I(vacio)
            Grpcnx: row[18],        // GrpCnx
            Taptipo: row[19],      // Tap(tipo)
            Taplado: row[20],      // Tap(lado)
            Tapdv: row[21],      // Tap(dV) - 1
            Tapmin: row[22],       // Tap(min)
            Tapcnt: row[23],       // Tap(cnt)
            Tapmax: row[24],       // Tap(max)
        };
    }).filter(function (row) {
        // Verificar si al menos una columna tiene un valor no vacío
        return Object.values(row).some(function (value) {
            return value !== "";
        });
    });
    return datosObjetos;
}

function cargarDatosPrm2() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetItcPrm2',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            
            success: function (response) {
                console.log(response);
                var hojaprm2Data = response.responseResult;
                setDatosprm2(hojaprm2Data);
                cambiosRealizados = false;
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function setDatosprm2(listaPRM2) {
    if (modoModel == "consultar") {
        desactivarCamposFormulario('Fichaprm2');
        $('#btnGrabarFichaprm2').hide();
        $('#btnImportarExcelPRM2').hide();
    }
    var datosArray = [];

    // Mapea los datos de la lista a la estructura de la tabla
    listaPRM2.forEach(function (item) {
        var row = [
            item.Transformador, // TRANSFORMADOR
            item.Tipo,          // TIPO
            item.Fases,         // FASES
            item.Ndvn,          // NDVN
            item.Vnp,           // Vn(p)
            item.Vns,           // Vn(s)
            item.Vnt,           // Vn(t)
            item.Pnp,           // Pn(p)
            item.Pns,           // Pn(s)
            item.Pnt,           // Pn(t)
            item.Tccps,         // Tcc(p-s)
            item.Tccst,         // Tcc(s-t)
            item.Tcctp,         // Tcc(t-p)
            item.Pcups,         // Pcu(p-s)
            item.Pcust,         // Pcu(s-t)
            item.Pcutp,         // Pcu(t-p)
            item.Pfe,           // Pfo
            item.Ivacio,        // I(vacio)
            item.Grpcnx,        // GrpCnx
            item.Taptipo,       // Tap(tipo)
            item.Taplado,       // Tap(lado)
            item.Tapdv,         // Tap(dV)
            item.Tapmin,        // Tap(min)
            item.Tapcnt,        // Tap(cnt)
            item.Tapmax         // Tap(max)
        ];
        datosArray.push(row);
    });

    // Completa con filas vacías hasta llegar a 50
    while (datosArray.length < 50) {
        var emptyRow = new Array(25).fill(""); // Usa cadenas vacías
        datosArray.push(emptyRow);
    }
   
    datosArray = formatearDatosGlobal(datosArray, configuracionColumnasprm2);

    crearTablaITCPRM2(datosArray);
}

function grabarPrm2() {
    if (!validarTablaAntesDeGuardar(hotprm2)) {
        mostrarNotificacion("No se puede guardar porque hay datos inválidos en la tabla.");
        return; // Detener el guardado si hay errores
    }
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        var ListaItcPrm2DTO = getDatosPRM2();
        param.ListaItcPrm2DTO = ListaItcPrm2DTO.slice(0, blockSize);
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarItcprm2',
            data: param,
            
            success: function (result) {
                console.log(result);
                if (result == 1) {
                    cambiosRealizados = false;
                    var restantes = ListaItcPrm2DTO.slice(blockSize); // El resto de los registros
                    if (restantes.length > 0) {
                        grabarPrm2_(restantes, idProyecto);
                    } else {
                        mostrarMensaje('mensajeFicha', 'exito', 'Los datos se grabaron correctamente.');
                    }                   
                    
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

function grabarPrm2_(restantes, idProyecto) {
    var block = restantes.slice(0, blockSize);
    var restante = restantes.slice(blockSize)
    var param = {
        ListaItcPrm2DTO: block,
        ProyCodi: idProyecto
    };

    $.ajax({
        type: 'POST',
        url: controlador + 'GrabarItcprm2_',
        data: param,
        success: function (result) {
            if (result == 1) {
                if (restante.length > 0) {
                    grabarPrm2_(restante, idProyecto);
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

function exportarTablaExcelprm2() {
    var headers = [
        [
            "TRANSFORMADOR",
            "TIPO",
            "FASES",
            "NºDVN",
            "Vn(p)",
            "Vn(s)",
            "Vn(t)",
            "Pn(p)",
            "Pn(s)",
            "Pn(t)",
            "Tcc(p-s)",
            "Tcc(s-t)",
            "Tcc(t-p)",
            "Pcu(p-s)",
            "Pcu(s-t)",
            "Pcu(t-p)",
            "Pfe",
            "I(vacio)",
            "GrpCnx",
            "Tap(tipo)",
            "Tap(lado)",
            "Tap(dV)",
            "Tap(min)",
            "Tap(cnt)",
            "Tap(max)",
        ],
    ];

    var datosArray = hotprm2.getData();
    var data = headers.concat(datosArray);

    var ws = XLSX.utils.aoa_to_sheet(data);
    aplicarBordes(ws);
    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "PRM2");

    XLSX.writeFile(wb, "ITC_SEyP_02-PRM2.xlsx");
}

function importarExcelprm2() {
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

            updateTableFromExcelprm2(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}

function updateTableFromExcelprm2(jsonData) {
    var headers = jsonData.slice(0, 1); // Las dos primeras filas son los headers
    var data = jsonData.slice(1); // El resto son los datos
    // Validar y formatear los datos importados
    const datosValidados = validarYFormatearDatos2(data, configuracionColumnasprm2, mostrarNotificacion, hotprm2);

    while (datosValidados.length < 50) {
        datosValidados.push(new Array(25).fill("")); // Agregar filas vacías
    }

    // Cargar los datos validados en la tabla, incluyendo errores
    hotprm2.loadData(datosValidados);

    // Verificar errores después de la validación
    const celdasConErrores = [];
    datosValidados.forEach((row, rowIndex) => {
        row.forEach((value, colIndex) => {
            const config = configuracionColumnasprm2[colIndex];
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
                    hotprm2.setCellMeta(rowIndex, colIndex, "valid", false);
                    hotprm2.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "#ffcccc", // Rojo claro
                    });
                } else {
                    // Si es válido, limpiar estilo previo
                    hotprm2.setCellMeta(rowIndex, colIndex, "valid", true);
                    hotprm2.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "",
                    });
                }
            }
        });
    });

    hotprm2.render(); // Refrescar tabla

    if (celdasConErrores.length > 0) {
        mostrarNotificacion(
            `Se importaron datos con errores.<br>Errores detectados en:<br>${celdasConErrores.join("<br>")}`
        );
    } else {
        mostrarNotificacion("Los datos se importaron correctamente.", '#a3e352');
    }

}
