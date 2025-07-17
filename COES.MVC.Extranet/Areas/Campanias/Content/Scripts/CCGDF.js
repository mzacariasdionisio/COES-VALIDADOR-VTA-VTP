var controlador = siteRoot + 'campanias/plantransmision/';
var controladorFichas = siteRoot + 'campanias/fichasproyecto/';
var hotCCGDF = null;
var valoresListaC = [];
var catRequisitoccgdf = 7;
var aniosCabecera = [];
var headerCabe = [];

$(function () {
    cargarCatalogoTablaCCGDF(catRequisitoccgdf);
    var formularioccgdf = document.getElementById('CCGDF');
    $(formularioccgdf).off('change').on('change', function () {
        cambiosRealizados = true;
    });
});

function cargarCatalogoTablaCCGDF(id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListarCatalogo',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (eData) {
            //cargarListaParametrosTablaCCGDF(eData, selectHtml, horizonteInicio, horizonteFin);
            valoresListaC = eData;
            tablaCCGDF(anioPeriodo, horizonteFin)
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function grabarCCGDF() {
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.CcgdfDTOs = getDataCCGDF();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarCCGDF',
            data: param,
            
            success: function (result) {
                console.log(result);
                if (result) {
                    mostrarMensaje('mensajeFicha', 'exito', 'Los datos se grabaron correctamente.');

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
    cambiosRealizados = false;
}

function tablaCCGDF(inicioan, finan) {
    console.log("Entro CargarHoja excel");
    var anioTotal = finan - inicioan + 1;
    var data = [];
    var aniosCabecera = [];
    aniosCabecera.push({ label: "", colspan: 1 });
    aniosCabecera.push({ label: "", colspan: 1 });
    aniosCabecera.push({ label: "", colspan: 1 });
    for (var i = 0; i < anioTotal; i++) {
        aniosCabecera.push({
            label: inicioan + i + "</br> TRIMESTRE",
            colspan: 4,
        });
    }
    console.log("aniosCabecera:", aniosCabecera);

    var headerCabe = [];
    headerCabe.push({ label: "", colspan: 1 });
    headerCabe.push({ label: "A&ntilde;o", colspan: 1 });
    headerCabe.push({ label: "A&ntilde;o " + (inicioan - 1) + "</br>o antes", colspan: 1 });
    for (var i = 0; i < anioTotal; i++) {
        for (var j = 0; j < 4; j++) {
            headerCabe.push(j + 1);
        }
    }
    console.log("headerCabe:", headerCabe);

    for (var i = 0; i < valoresListaC.length; i++) {
        var row = [];
        row.push(valoresListaC[i].DataCatCodi);
        row.push(valoresListaC[i].DesDataCat);
        row.push(false);
        for (var j = 0; j < anioTotal * 4; j++) {
            row.push(false);
        }
        data.push(row);
    }
    console.log("data:", data);
    var checkConfig = [];
    for (var j = 0; j < anioTotal * 4; j++) {
        checkConfig.push({ type: "checkbox" });
    }

    var grilla = document.getElementById("detalleTableCCGDF");
    console.log("grilla:", grilla);

    if (typeof hotCCGDF !== "undefined" && hotCCGDF !== null) {
        hotCCGDF.destroy();
    }

    hotCCGDF = new Handsontable(grilla, {
        data: data,
        // rowHeaders: true,
        colHeaders: true,
        colWidths: [0, 200, 100, 50],
        manualColumnResize: true,
        columnSorting: false,
        filters: false,
        dropdownMenu: true,
        autoColumnSize: true,
        autoRowSize: true,
        nestedHeaders: [[...aniosCabecera], [...headerCabe]],
        columns: [
            { readOnly: true }, // Columna oculta
            { readOnly: true }, // Columna "A�o"
            { type: "checkbox" }, // Columna "A�o 2023 o antes"
            ...checkConfig, // Ajustar para incluir todos los trimestres
        ],
        hiddenColumns: {
            columns: [0],
            indicators: true,
        },
        autoWrapRow: true,
        fixedColumnsLeft: 2,
        beforeChange: function(changes, source) {
            if (source === 'edit' || source === 'paste') {
                changes.forEach(change => {
                    const [row, prop, oldValue, newValue] = change;
                    if (newValue === "1" || newValue === 1 || newValue === true) {
                        change[3] = true;
                    } else {
                        change[3] = false;
                    }
                });
            }
        },
    });

    hotCCGDF.render();
}


function getDataCCGDF() {
    var hotInstance = hotCCGDF.getInstance(); // Obtener la instancia de Handsontable
    var data = hotInstance.getData(); // Obtener los datos actuales de la tabla
    var valoresMarcados = [];

    // Iterar sobre los datos para obtener los valores marcados
    for (var i = 0; i < data.length; i++) {
        var fila = data[i];
        var desDataCat = fila[0]; // Obtener el texto de la columna "DesDataCat"
        var dataCat = valoresListaC.find(item => item.DataCatCodi === desDataCat); // Buscar el objeto DATACATCODI correspondiente a desDataCat

        if (!dataCat) {
            console.warn("No se encontr� DATACATCODI para:", desDataCat);
            continue;
        }

        // Manejar la segunda columna "A�o 0 Trimestre 0"
        var segundoColumnaMarcada = fila[2]; // Obtener el valor de la segunda columna
        if (segundoColumnaMarcada) {
            var param = {};
            param.DataCatCodi = dataCat.DataCatCodi;
            param.Anio = 0;
            param.Trimestre = 0;
            param.Valor = 1;
            valoresMarcados.push(param)
        }

        // Iterar sobre las columnas con checkboxes
        for (var j = 3; j < fila.length; j++) {
            var anio = Math.floor((j - 3) / 4) + 1; // Calcular el a�o basado en la posici�n de la columna
            var trimestre = (j - 3) % 4 + 1; // Calcular el trimestre basado en la posici�n de la columna
            var estaMarcado = fila[j]; // Verificar si el checkbox est� marcado

            if (estaMarcado) {
                var param = {};
                param.DataCatCodi = dataCat.DataCatCodi;
                param.Anio = anio;
                param.Trimestre = trimestre;
                param.Valor = 1;
                valoresMarcados.push(param)
            }
        }
    }
    return valoresMarcados;
}

function cargarCCGDF() {
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetCCGDF',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            
            success: function (response) {
                console.log(response);
                var hojaCRes = response.responseResult;
                setDataCCGDF(hojaCRes);
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function setDataCCGDF(datosSeteo) {
    var hotInstance = hotCCGDF.getInstance(); // Obtener la instancia de Handsontable
    var data = hotInstance.getData(); // Obtener los datos actuales de la tabla

    // Iterar sobre los datos de seteo
    if(datosSeteo){
        for (var i = 0; i < datosSeteo.length; i++) {
            var dato = datosSeteo[i];
            var dataCatCodi = dato.DataCatCodi; // Convertir a string para comparaci�n
            var anio = parseInt(dato.Anio);
            var trimestre = parseInt(dato.Trimestre);
            var valor = parseInt(dato.Valor); // Asumiendo que Valor es un n�mero (1 o 0)
            var rowIndex = data.findIndex(row => row[0] == dataCatCodi);
    
            if (rowIndex !== -1) {
                // Actualizar la columna "A�o 0 Trimestre 0"
                if (anio === 0 && trimestre === 0) {
                    hotInstance.setDataAtCell(rowIndex, 2, true); // Marcar la casilla correspondiente
                }
    
                // Actualizar la columna correspondiente al a�o y trimestre
                if (anio > 0 && trimestre > 0) {
                    // Calcular el �ndice de la columna en funci�n de anio y trimestre
                    var columnIndex = 3 + (anio - 1) * 4 + (trimestre - 1);
    
                    if (columnIndex < data[rowIndex].length) {
                        hotInstance.setDataAtCell(rowIndex, columnIndex, true); // Marcar la casilla correspondiente
                    }
                }
            }
        }
    }

    if (modoModel == "consultar") {
        desactivarCamposFormulario('CCGDF');
        $('#btnGrabarCCGDF').hide();
    }
}
function exportarTablaExcelCCGDFHojaC() {
    
    var inicioan = horizonteInicio;
    var finan = horizonteFin;
    var anioTotal = finan - inicioan + 1;

    // Cabecera principal (Años con salto de línea)
    var aniosCabecera = [
        { label: "AÑO", colspan: 1 },
        { label: "AÑO " + (inicioan - 1) + " o antes", colspan: 1 }
    ];

    for (var i = 0; i < anioTotal; i++) {
        aniosCabecera.push({
            label: (inicioan + i) + "\nTRIMESTRE", // Salto de línea
            colspan: 4,
        });
    }

    // Segunda cabecera (Trimestres)
    var headerCabe = [
        { label: "AÑO", colspan: 1 },
        { label: "AÑO " + (inicioan - 1) + " o antes", colspan: 1 }
    ];

    for (var i = 0; i < anioTotal; i++) {
        for (var j = 0; j < 4; j++) {
            headerCabe.push("TRIMESTRE " + (j + 1));
        }
    }

    // Obtener datos de Handsontable
    var data = hotCCGDF.getData();

    // Construir estructura para exportar
    var dataWithHeaders = [
        aniosCabecera.map(header => header.label),
        headerCabe.map(header => (typeof header === 'object' ? header.label : header))
    ].concat(data);

    // Crear hoja de Excel
    var ws = XLSX.utils.aoa_to_sheet(dataWithHeaders);

    // Aplicar combinaciones de celdas para la cabecera
    var merges = [];

    // Combinar las dos primeras celdas de la primera fila
    merges.push({ s: { r: 0, c: 0 }, e: { r: 1, c: 0 } });
    merges.push({ s: { r: 0, c: 1 }, e: { r: 1, c: 1 } });

    var colIndex = 2;
    for (var i = 0; i < aniosCabecera.length - 2; i++) {
        merges.push({
            s: { r: 0, c: colIndex },
            e: { r: 0, c: colIndex + 3 }
        });
        colIndex += 4;
    }

    ws["!merges"] = merges;

    // Crear libro de Excel y guardar archivo
    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "CCGDF");

    XLSX.writeFile(wb, "CCGDF_Export.xlsx");
}

function importarExcelCCGDFHojaC() {
    var fileInput = document.createElement("input");
    fileInput.type = "file";
    fileInput.accept = ".xlsx, .xls";
    fileInput.onchange = function (e) {
        var file = e.target.files[0];
        var reader = new FileReader();

        reader.onload = function (e) {
            var data = new Uint8Array(e.target.result);
            var workbook = XLSX.read(data, { type: "array" });

            // Leer la primera hoja del archivo Excel
            var firstSheet = workbook.Sheets[workbook.SheetNames[0]];

            // Convertir la hoja de Excel a un formato JSON
            var jsonData = XLSX.utils.sheet_to_json(firstSheet, { header: 1 });

            // Actualizar la tabla Handsontable con los datos importados
            updateTableFromExcelCCGDFHojaC(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}

function updateTableFromExcelCCGDFHojaC(jsonData) {
    var headers = jsonData.slice(0, 2); // Las dos primeras filas son las cabeceras
    var data = jsonData.slice(2); // El resto son los datos

    // Cargar los datos en la tabla Handsontable
    hotCCGDF.loadData(data);
}
