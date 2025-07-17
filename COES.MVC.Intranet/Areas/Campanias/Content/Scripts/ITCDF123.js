var hot123 = null;
var listaAniosF123 = [];
var listaTypeNumeric = [];
var configuracionColumnas123 = {};
$(function () {
    configuracionColumnas123 = generarconfiguracionColumnas1232(anioPeriodo + 1, horizonteFin);
    crearTablaITCF123((anioPeriodo + 4));

});

function generarconfiguracionColumnas1232(anioInicia, anioFin) {
    
    const configuracion = {};

    // Rango dinámico de columnas basado en los años
    for (let i = 2; i <= 2 + (anioFin - anioInicia + 1); i++) {
        if (i === 2) {
            // Configuración especial para la columna 2
            configuracion[i] = { tipo: "especial", permitirNegativo: false };
        } else {
            // Configuración para otras columnas como entero
            configuracion[i] = { tipo: "decimal", decimales: 4, permitirNegativo: false };
        }
    }

    return configuracion;
}


function crearTablaITCF123(anioInicia) {
    var data = [];
    listaAniosF123.push(anioInicia);
    for (let i = 1; i <= 3; i++) {
        listaAniosF123.push(listaAniosF123[i - 1] + 5);
        listaTypeNumeric.push({type: "numeric"});
    }
    // Configurar columnas como tipo "text" para cada año, incluyendo "Año 1"
    listaanio = listaAniosF123.map(() => ({ type: "text" }));

    for (var i = 0; i < 50; i++) {
        var row = [];
        row.push("");
        row.push("");
        row.push("");
        for (var j = 0; j < listaAniosF123.length; j++) {
            row.push("");
        }
        data.push(row);
    }

    var grilla = document.getElementById("tablef123");

    if (typeof hot123 !== "undefined" && hot123 !== null) {
        hot123.destroy();
    }

    hot123 = new Handsontable(grilla, {
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
                { label: "", colspan: 1 },
                { label: "", colspan: 1 },
                { label: "", colspan: 1 },
                { label: "MW/Km^2", colspan: listaAniosF123.length },
            ],
            [
                "UTM-Este",
                "UTM-Norte",
                "UTM-Zona",
                ...listaAniosF123,
            ],
        ],
        columns: [
           { type: "text" , readOnly: true},
           { type: "text", readOnly: true}, 
            {type: "text", readOnly: true},
            ...listaanio 
        ],
        beforeChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
                return generalBeforeChange_4(this, changes, configuracionColumnas123);
            }
        },
        afterChange: function (changes, source) {
            afterChangeValidateEmpty(this, changes, source);
        }
    });
    hot123.render();
    if (modoModel == "consultar") {
        $("#btnGrabar123").hide();
        hot123.updateSettings({
            cells: function (row, col) {
                var cellProperties = {};
                cellProperties.readOnly = true;
                return cellProperties;
            }
        });
    }
}


function getDatos123() {
    var datosArray = hot123.getData();
    var datosObjetos = datosArray.map(function (row) {
        return {
            UtmEste: row[0] || null,
            UtmNorte: row[1] || null,
            UtmZona: row[2] || null,
            Anio1: row[3] || null,
            Anio2: row[4] || null,
            Anio3: row[5] || null,
            Anio4: row[6] || null
        };
    }).filter(function (obj) {
        // Filtrar filas donde todos los campos sean nulos o vacíos
        return Object.values(obj).some(value => value !== null && value !== "");
    });
    return datosObjetos;
}


function grabarItcdf123() {
    if (!validarTablaAntesDeGuardar(hot123)) {
        mostrarNotificacion("No se puede guardar porque hay datos inválidos en la tabla.");
        return; // Detener el guardado si hay errores
    }
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.Itcdf123DTOs = getDatos123();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarItcdf123',
            data: param,
            
            success: function (result) {
                console.log(result);
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

function cargarDatos123() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetItcdf123',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            
            success: function (response) {
                console.log(response);
                var hoja123Data = response.responseResult;
                setDatos123(hoja123Data);
                cambiosRealizados = false;
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function setDatos123(lista123) {
    if (modoModel == "consultar") {
        desactivarCamposFormulario('f123');
        $('#btnGrabar123').hide();
    }
    hot123.loadData([]);
    var datosArray = [];

    // Mapea los datos de la lista a la estructura de la tabla
    lista123.forEach(function (item) {
        var row = [
            item.UtmEste,
            item.UtmNorte,
            item.UtmZona,
            item.Anio1 !== null ? parseFloat(item.Anio1).toFixed(4) : "",
            item.Anio2 !== null ? parseFloat(item.Anio2).toFixed(4) : "",
            item.Anio3 !== null ? parseFloat(item.Anio3).toFixed(4) : "",
            item.Anio4 !== null ? parseFloat(item.Anio4).toFixed(4) : ""
        ];
        datosArray.push(row);
    });

    // Completa con filas vacías hasta llegar a 50
    while (datosArray.length < 50) {
        var emptyRow = new Array(7).fill("");
        datosArray.push(emptyRow);
    }

    hot123.loadData(datosArray);
}

function exportarTablaExcel123() {
    console.log('lsiata años',listaAniosF123)
    var headers = [
        [
            "",
            "",
            "",
            "MW/Km^2",
        ],
        [
            "UTM-Este",
            "UTM-Norte",
            "UTM-Zona",
            ...listaAniosF123,
        ],
    ];

    var datosArray = hot123.getData();
    var data = headers.concat(datosArray);

    var ws = XLSX.utils.aoa_to_sheet(data);
    aplicarBordes(ws);
    // Definir las celdas combinadas
    ws["!merges"] = [
        { s: { r: 0, c: 3 }, e: { r: 0, c: 3 + listaAniosF123.length - 1 } }, // Combina "MW/Km^2"
    ];

    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "F123");

    XLSX.writeFile(wb, "ITC_DemEDE_09-F123.xlsx");
}

function importarExcel123() {

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

            updateTableFromExcel123(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}

function updateTableFromExcel123(jsonData) {
    
    var headers = jsonData.slice(0, 2); // Las dos primeras filas son los headers
    var data = jsonData.slice(2); // El resto son los datos
    // Validar y formatear los datos importados
    const datosValidados = validarYFormatearDatos2(data, configuracionColumnas123, mostrarNotificacion, hot123);

    // Cargar los datos validados en la tabla, incluyendo errores
    hot123.loadData(datosValidados);

    // Verificar errores después de la validación
    const celdasConErrores = [];
    datosValidados.forEach((row, rowIndex) => {
        row.forEach((value, colIndex) => {
            const config = configuracionColumnas123[colIndex];
            if (config && value !== null && value !== "" && value !== undefined) {
                let parsedValue = parseFloat(value);
                let esValido = true;

                if (isNaN(parsedValue)) {
                    esValido = false;
                } else if (!config.permitirNegativo && parsedValue < 0) {
                    esValido = false;
                } else if (config.tipo === "especial") {
                    const valoresPermitidos = [17, 18, 19];
                    if (!valoresPermitidos.includes(parsedValue)) {
                        esValido = false; // Marcar como inv�lido
                       
                    }
                }
                else if (config.tipo === "decimal" && value.split(".")[1]?.length > config.decimales) {
                    esValido = false;
                } else if (config.tipo === "entero" && !Number.isInteger(parsedValue)) {
                    esValido = false;
                }

                if (!esValido) {
                    celdasConErrores.push(`Fila ${rowIndex + 1}, Columna ${colIndex + 1}`);
                    hot123.setCellMeta(rowIndex, colIndex, "valid", false);
                    hot123.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "#ffcccc", // Rojo claro
                    });
                } else {
                    // Si es válido, limpiar estilo previo
                    hot123.setCellMeta(rowIndex, colIndex, "valid", true);
                    hot123.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "",
                    });
                }
            }
        });
    });

    hot123.render(); // Refrescar tabla

    if (celdasConErrores.length > 0) {
        mostrarNotificacion(
            `Se importaron datos con errores.<br>Errores detectados en:<br>${celdasConErrores.join("<br>")}`
        );
    } else {
        mostrarNotificacion("Los datos se importaron correctamente.", '#a3e352');
    }
}