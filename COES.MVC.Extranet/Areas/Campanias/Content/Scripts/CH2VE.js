var hot4 = null;
$(function () {
    crearTablaDiagCargHorario(30, 1440, 30);

    var formularioa = document.getElementById('tableDiagCargHorario');
    formularioa.addEventListener('change', function () {
        cambiosRealizados = true;

    });
});


function getDataCH2VE() {
    var datosArray = hot4.getData();
    var datosObjetos = datosArray.map(function (row) {
        return {
            Hora: row[0],
            ConsumoEnergetico: row[1],
            ProduccionCentral: row[2],
        };
    });
    console.log(datosObjetos);
    // Filtrar objetos que tienen al menos uno de los campos con datos válidos
    var datosFiltrados = datosObjetos.filter(function (obj) {
        return (
            obj.ConsumoEnergetico !== null && obj.ConsumoEnergetico !== "" ||
            obj.ProduccionCentral !== null && obj.ProduccionCentral !== "" 
        );
    });
    return datosFiltrados;
}

function crearTablaDiagCargHorario(iniciot, fint, increm) {

    var data = [];
    for (var time = iniciot; time <= fint; time += increm) {
        var hora = Math.floor((time / 60) == 24 ? 0 : (time / 60)).toString().padStart(2, '0');
        var min = (time % 60).toString().padStart(2, '0');;
        var row = [`${hora}:${min}`];
        for (var i = 0; i < 2; i++) {
            row.push('');
        }
        data.push(row);
    }

    var grilla = document.getElementById('tableDiagCargHorario');

    if (typeof hot4 !== 'undefined' && hot4 !== null) {
        hot4.destroy();
    }

    hot4 = new Handsontable(grilla, {
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
                { label: 'HORA', colspan: 1 },
                { label: 'Consumo energ&eacute;tico para producci&oacute;n de H2', colspan: 1 },
                { label: 'Producci&oacute;n de central asociada', colspan: 1 }
            ]
        ],
        columns: [
            { data: 0, readOnly: true },
            {
                data: 1,
                type: 'numeric', format: '0.0'
            },
            {
                data: 2,
                type: 'numeric', format: '0.0'
            }
        ],
        beforeChange: function (changes, source) {
            console.log("beforeChange event triggered");
            generalBeforeChange(this, changes, source);
        },
        afterChange: function (changes, source) {
            console.log("afterChange event triggered");
            generalAfterChange(this, changes, source);
        }
    });

    hot4.render();
}

function grabarCH2VE() {
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.Ch2veDTOs = getDataCH2VE();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarCH2VE',
            data: param,
            
            success: function (result) {
                console.log(result);
                if (result) {
                    mostrarMensaje('mensajeFicha', 'exito', 'Los datos se grabaron correctamente.');
                    cambiosRealizados = false;

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
}

function cargarCH2VE() {
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetCuestionarioH2E',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            
            success: function (response) {
                console.log(response);
                var hojaCRes = response.responseResult;
                setDataCH2VE(hojaCRes);
                cambiosRealizados = false;
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function setDataCH2VE(datos) {
    // Obtener la data actual de la tabla Handsontable
    var currentData = hot4.getData();

    // Iterar sobre los nuevos datos para encontrar coincidencias y actualizar la tabla
    datos.forEach(function (nuevoDato) {
        // Buscar el índice de la fila en la tabla actual que coincide con la 'Hora' del nuevo dato
        var rowIndex = currentData.findIndex(function (fila) {
            return fila[0] === nuevoDato.Hora; // Comparar por la columna 'Hora'
        });

        // Si se encuentra una coincidencia, actualizamos las columnas correspondientes
        if (rowIndex !== -1) {
            currentData[rowIndex][1] = nuevoDato.ConsumoEnergetico !== null ? nuevoDato.ConsumoEnergetico : ''; // Asegurarse de mostrar el 0
            currentData[rowIndex][2] = nuevoDato.ProduccionCentral !== null ? nuevoDato.ProduccionCentral : ''; // Asegurarse de mostrar el 0
        }
    });

    // Recargar los datos actualizados en Handsontable
    hot4.loadData(currentData);
    hot4.render(); // Renderizar la tabla para reflejar los cambios

    if (modoModel === "consultar") {
        desactivarCamposFormulario('H2VE');
        $('#btnGrabarH2VE').hide();
    }
}

function exportarTablaExcelCH2VE() {
    // Definir los encabezados de la tabla
    var headers = [
        ["HORA", "Consumo energético para producción de H2", "Producción de central asociada"]
    ];

    // Obtener los datos de la tabla Handsontable (hot4)
    var datosArray = hot4.getData();
    // Concatenar los encabezados con los datos
    var data = headers.concat(datosArray);

    // Crear una hoja de trabajo (worksheet) a partir de los datos
    var ws = XLSX.utils.aoa_to_sheet(data);
    aplicarBordes(ws);
    // Aplicar formato de 4 decimales a partir de la celda (1, 1)
    var startRow = 1; // Fila 2 en términos de Excel
    var startCol = 1; // Columna B en términos de Excel

    for (var R = startRow; R < data.length; R++) {
        for (var C = startCol; C < data[R].length; C++) {
            var cellAddress = XLSX.utils.encode_cell({ r: R, c: C });
            var cell = ws[cellAddress];
            if (cell && typeof cell.v === 'number') {
                cell.z = '0.0'; // Aplicar el formato de 4 decimales
            }
        }
    }

    // Crear un nuevo libro de trabajo (workbook) y agregar la hoja
    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "H2VE");

    // Exportar el libro de trabajo a un archivo Excel
    XLSX.writeFile(wb, "HVE_04-DiagramaPU.xlsx");
}


function importarExcelCH2VE() {
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

            // Llamar a la función para actualizar la tabla con los datos importados
            updateTableFromExcelCH2VE(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}

function updateTableFromExcelCH2VE(jsonData) {
    // El primer elemento contiene los encabezados, por lo tanto, comenzamos desde el segundo elemento
    var data = jsonData.slice(1);

    // Actualizar los datos de la tabla Handsontable (hot4) con los datos importados
    hot4.loadData(data);

    // Renderizar la tabla para reflejar los cambios
    hot4.render();
}

function generarGraficaCH2VE() {
    // Obtener los datos de la tabla Handsontable
    var tableData = hot4.getData();

    // Arrays para las categorías (eje X) y las series de datos (eje Y)
    var categories = [];
    var consumoEnergeticoData = [];
    var produccionCentralData = [];

    // Recorrer los datos de la tabla y llenar las categorías y series
    for (var i = 0; i < tableData.length; i++) {
        var hora = tableData[i][0]; // Columna de Hora
        var consumoEnergetico = parseFloat(tableData[i][1]); // Columna de Consumo energético
        var produccionCentral = parseFloat(tableData[i][2]); // Columna de Producción de Central

        // Asegurarnos de que no haya datos vacíos o inválidos antes de agregarlos
        if (hora !== "" && !isNaN(consumoEnergetico) && !isNaN(produccionCentral)) {
            categories.push(hora); // Añadir hora al eje X
            consumoEnergeticoData.push(consumoEnergetico); // Añadir Consumo energético a la serie 1
            produccionCentralData.push(produccionCentral); // Añadir Producción de Central a la serie 2
        }
    }

    // Crear la gráfica usando Highcharts
    Highcharts.chart("containerGraficaH2E", {
        chart: {
            type: 'line'
        },
        title: {
            text: "Consumo Energético y Producción de Central"
        },
        xAxis: {
            categories: categories,
            title: {
                text: "Tiempo (Hora)"
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: "Valores"
            }
        },
        series: [
            {
                name: "Consumo Energético",
                data: consumoEnergeticoData,
                marker: {
                    enabled: false
                }
            },
            {
                name: "Producción de Central",
                data: produccionCentralData,
                marker: {
                    enabled: false
                }
            }
        ]
    });
}

