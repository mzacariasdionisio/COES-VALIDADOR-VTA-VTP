var controlador = siteRoot + 'campanias/plantransmision/';
var controladorFichas = siteRoot + 'campanias/fichasproyecto/';
var hotFichaB = null;
var aniosCabecera = [];
var headerCabe = [];
var valoresListaB = [];
var catRequisitofichab = 7;


$(function () {
    inicializar();
});

function inicializar() {
    cargarCatalogoTablaFichaB(catRequisitofichab);
   // $('#txtFechaPuestaOperacion').val(obtenerFechaActualMesAnio());
    $('#txtFechaPuestaOperacion').Zebra_DatePicker({
        format: 'm/Y',
        onSelect: function () {
            cambiosRealizados = true; // Marcar que hubo un cambio
        }
    });
    $('#btnGrabarHojaB').on('click', function () {
        grabarFichaB();
    });
    var formulariobl = document.getElementById('GLFichaB');
    formulariobl.addEventListener('change', function () {
        cambiosRealizados = true;
    });
}

getDataLineasFichaB = function () {
    var param = {};
    param.Fecpuestaope = $("#txtFechaPuestaOperacion").val();
    return param;
}

function cargarCatalogoTablaFichaB(id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListarCatalogo',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (eData) {
            valoresListaB = eData;
            tablaFichaBHan(anioPeriodo, horizonteFin);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function tablaFichaBHan(inicioan, finan) {
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

    for (var i = 0; i < valoresListaB.length; i++) {
        var row = [];
        row.push(valoresListaB[i].DataCatCodi);
        row.push(valoresListaB[i].DesDataCat);
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

    var grilla = document.getElementById("detalleFormatoHojaB");
    console.log("grilla:", grilla);

    if (typeof hotFichaB !== "undefined" && hotFichaB !== null) {
        hotFichaB.destroy();
    }

    hotFichaB = new Handsontable(grilla, {
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

    hotFichaB.render();
}
function grabarFichaB() {
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.LineasFichaBDTO = getDataLineasFichaB();
        param.ListLineasFichaBDetDTO = getDataFichaBDet();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarLineasFichaB',
            data: param,
            
            success: function (result) {
                console.log("result");
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

function getDataFichaBDet() {
    var hotInstance = hotFichaB.getInstance(); // Obtener la instancia de Handsontable
    var data = hotInstance.getData(); // Obtener los datos actuales de la tabla
    var valoresMarcados = [];

    // Iterar sobre los datos para obtener los valores marcados
    for (var i = 0; i < data.length; i++) {
        var fila = data[i];
        var desDataCat = fila[0]; // Obtener el texto de la columna "DesDataCat"
        var dataCat = valoresListaB.find(item => item.DataCatCodi === desDataCat); // Buscar el objeto DATACATCODI correspondiente a desDataCat

        if (!dataCat) {
            console.warn("No se encontr� DATACATCODI para:", desDataCat);
            continue;
        }

        // Manejar la segunda columna "A�o 0 Trimestre 0"
        var segundoColumnaMarcada = fila[2]; // Obtener el valor de la segunda columna
        if (segundoColumnaMarcada) {
            var param = {};
            param.Datacatcodi = dataCat.DataCatCodi;
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
                param.Datacatcodi = dataCat.DataCatCodi;
                param.Anio = anio;
                param.Trimestre = trimestre;
                param.Valor = 1;
                valoresMarcados.push(param)
            }
        }
    }
    return valoresMarcados;
}

function cargarFichaB() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetLFichaB',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            
            success: function (response) {
                console.log(response);
                var hojabRes = response.responseResult;
                setDataFichaB(hojabRes);
                setDataFichaBDet(hojabRes.LineasFichaBDetDTO);
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
    cambiosRealizados = false;
}

setDataFichaB = function (dataResponse) {
    var fechaConvertida = convertirFechaMesAnio(dataResponse.FecPuestaOpe)
   // $("#txtFechaPuestaOperacion").val(dataResponse.FecPuestaOpe);
    setFechaPickerGlobal("#txtFechaPuestaOperacion", fechaConvertida, "mm/aaaa");
    if (modoModel == "consultar") {
        desactivarCamposFormulario('GLFichaB');
        $("#btnGrabarHojaB").hide();
        hotFichaB.updateSettings({
            cells: function (row, col) {
                var cellProperties = {};
                cellProperties.readOnly = true;
                return cellProperties;
            }
        });
    }
}


function setDataFichaBDet(datosSeteo) {
    var hotInstance = hotFichaB.getInstance(); // Obtener la instancia de Handsontable
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
}

//function convertirFecha(fechaJSON) {

//    // Extraer el timestamp de la cadena
//    let timestamp = parseInt(fechaJSON.match(/\d+/)[0]);

//    // Crear un objeto de fecha con el timestamp (en milisegundos)
//    let fecha = new Date(timestamp);

//    // Obtener d�a, mes y a�o
//    let dia = fecha.getDate().toString().padStart(2, '0');
//    let mes = (fecha.getMonth() + 1).toString().padStart(2, '0'); // Enero es 0
//    let anio = fecha.getFullYear();

//    // Formatear la fecha como 'DD-MM-YYYY'
//    let fechaFormateada = `${dia}-${mes}-${anio}`;
//    return fechaFormateada;
//}

function exportarTablaExcelHojaB() {


    // Definir las cabeceras manualmente
    aniosCabecera.push({ label: "", colspan: 1 });
    aniosCabecera.push({ label: "", colspan: 1 });
    aniosCabecera.push({ label: "", colspan: 1 });

    var inicioan = horizonteInicio; // Ejemplo: puedes ajustar esto seg�n tus necesidades
    var finan = horizonteFin; // Ejemplo: puedes ajustar esto seg�n tus necesidades
    var anioTotal = finan - inicioan + 1;

    for (var i = 0; i < anioTotal; i++) {
        aniosCabecera.push({
            label: inicioan + i + "</br> TRIMESTRE",
            colspan: 4,
        });
    }

    headerCabe.push({ label: "", colspan: 1 });
    headerCabe.push({ label: "A&ntilde;o", colspan: 1 });
    headerCabe.push({ label: "A&ntilde;o " + (inicioan - 1) + "</br>o antes", colspan: 1 });
    for (var i = 0; i < anioTotal; i++) {
        for (var j = 0; j < 4; j++) {
            headerCabe.push(j + 1);
        }
    }

    // Obtener los datos de la tabla Handsontable
    var data = hotFichaB.getData();

    // Combinar cabeceras con los datos de la tabla
    var dataWithHeaders = [
        aniosCabecera.map(header => header.label),
        headerCabe.map(header => typeof header === 'object' ? header.label : header)
    ].concat(data);

    // Crear una hoja de c�lculo con los datos
    var ws = XLSX.utils.aoa_to_sheet(dataWithHeaders);

    // Definir las celdas combinadas en la hoja de c�lculo
    var merges = [];
    var colIndex = 0;
    for (var i = 0; i < aniosCabecera.length; i++) {
        if (aniosCabecera[i].colspan > 1) {
            merges.push({
                s: { r: 0, c: colIndex },
                e: { r: 0, c: colIndex + aniosCabecera[i].colspan - 1 }
            });
            colIndex += aniosCabecera[i].colspan;
        } else {
            colIndex++;
        }
    }
    ws["!merges"] = merges;

    // Crear un libro de Excel
    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "FichaB");

    // Guardar el archivo Excel
    XLSX.writeFile(wb, "G7B_08-Cronograma Líneas.xlsx");
}

function importarExcelHojaB() {
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
            updateTableFromExcelHojaC(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}

function updateTableFromExcelHojaC(jsonData) {
    var headers = jsonData.slice(0, 2); // Las dos primeras filas son las cabeceras
    var data = jsonData.slice(2); // El resto son los datos

    // Cargar los datos en la tabla Handsontable
    hotFichaB.loadData(data);
}
