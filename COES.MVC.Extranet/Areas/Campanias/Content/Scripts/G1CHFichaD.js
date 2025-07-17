var controlador = siteRoot + 'campanias/plantransmision/';
var controladorFichas = siteRoot + 'campanias/fichasproyecto/';
var listaCuenca = [];
var listaAnios = [];
var anioInicio = 0;
var anioFin = 0;
var contador = 0;
var contadorMedia = 0;
var hotFichaD = null;
var totalAniosD = 0;
var entre = false;

const configuracionColumnasG1D = {
    1: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    2: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    3: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    4: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    5: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    6: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    7: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    8: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    9: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    10: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    11: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    12: { tipo: "decimal", decimales: 4, permitirNegativo: false },
};


$(function () {
    $("#formFinTablaD").hide();
    $('#mensajeCuencaCaudal').hide();
    $('#btnGrabarFichaD').on('click', function () {
        grabarFichaD();
    });
    var formulariod = document.getElementById('HFichaD');
    formulariod.addEventListener('change', function () {
        cambiosRealizados = true;
    });
    var ruta_interna = obtenerRutaProyecto();  
   crearUploaderGeneral('btnSubirFormatoCuenca', '#tablaCuencaFile', 1, null, ruta_interna);
    //crearPuploadFichaD();
});

getDataHojaD = function () {

    var param = {};
    dataCheck = [];

    param.Estudiofactibilidad = $("#txtCuenca").val();
    param.Investigacionescampo = $("#txtCaudal").val();
    console.log(param);

    return param;
}

function agregarFilaCuenca() {
    var validacion = validarHojaD();
    if (validacion == "") {
        $('#mensajeCuencaCaudal').hide();
        var cuenca = $("#txtCuencaCabecera").val();
        var caudal = $("#txtCaudalCabecera").val();

        var existe = listaCuenca.some(function (item) {
            return item.Cuenca === cuenca && item.Caudal === caudal;
        });

        if (existe) {
            mostrarMensaje('mensajeFicha', 'error', 'No se pueden agregar Caudales repetidos');
            console.log("error repetido");
            return;
        }

        var cuencaObj = {};

        var uuid = generateUUID();
        console.log(cuenca);
        console.log(caudal);
        var estado = "Pendiente"
        var col1 = '<td>' + cuenca + '</td>';
        var col2 = '<td>' + caudal + '</td>'
        var col3 = '<td style="width:70px">' + estado + '</td>'
        var col4 = `
                    <td>
                        <a href="#" onclick="mostraTablaDetalle('${uuid}')">
                            <img src="${siteRoot}Areas/Campanias/Content/Images/datosHidro.png" title="Datos Hidrol&oacute;gicos" />
                        </a>
                        <a href="#" onclick="eliminarFilaCuenca('${uuid}')">
                            <img src="${siteRoot}Content/Images/eliminar.png" title="Eliminar" />
                        </a>
                    </td>`;

        var nuevaFila = '<tr id="fila' + uuid + '">' + col1 + col2 + col3 + col4 + '</tr>';
        var tabla = $('#tablaCuenca');
        cuencaObj.Caudal = caudal;
        cuencaObj.Cuenca = cuenca;
        cuencaObj.id = uuid;
        cuencaObj.Estado = estado;
        listaCuenca.push(cuencaObj);
        tabla.append(nuevaFila);
        $("#txtCuencaCabecera").val("");
        $("#txtCaudalCabecera").val("");
    } else {
        $('#mensajeCuencaCaudal').show();
        mostrarMensaje('mensajeCuencaCaudal', 'alert', validacion);
    }

}

function generateUUID() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

function mostraTablaDetalle(idobj) {
    $("#formInicioTablaD").hide();
    $("#formFinTablaD").show();
    $("#txtidcabeceraD").val(idobj);
    cargarCatalogoAnio(idobj);
}


function mostraTablaDetalleBack(idobj) {
    console.log("ID del objeto:", idobj);

    // Ocultar el formulario inicial y mostrar el formulario de detalle
    $("#formInicioTablaD").hide();
    $("#formFinTablaD").show();
    $("#txtidcabeceraD").val(idobj);

    // Obtener el rango de años desde los parámetros o lista predefinida
    $.ajax({
        type: 'POST',
        url: controlador + 'ListarCatalogo',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: 8 }), // Ajusta el parámetro si es necesario
        success: function (listaValores) {
            if (listaValores && listaValores.length > 0) {
                const anioInicio = listaValores[0].Valor; // Primer valor es el año inicial
                const anioFin = parseInt($('#txtPoyAnio').val()); // Obtiene el año final del input

                // Buscar el objeto cuenca por su ID
                var obCuenca = listaCuenca.find(cuenca => cuenca.id === idobj);

                if (obCuenca) {
                    // Combina el rango de años con los datos del backend
                    const datosDetalle = obCuenca.ListDetRegHojaD || [];
                    tablaHojaDHan(anioInicio, anioFin, { ListDetRegHojaD: datosDetalle });
                } else {
                    console.log("No se encontró una cuenca con el ID especificado.");
                }
            } else {
                console.log("No se recibieron datos válidos del catálogo.");
            }
        },
        error: function () {
            console.log("Error al obtener los valores del catálogo.");
        }
    });
}



function eliminarFilaCuenca(id,valor) {
    document.getElementById("contenidoPopup").innerHTML = '¿Estás seguro de que deseas eliminar esta fila?';
    $('#popupProyectoGeneral').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    $('#btnConfirmarPopup').off('click').on('click', function() {
        var index = listaCuenca.findIndex(cuenca => cuenca.id === id);

        if (index !== -1) {
            var cuenca = listaCuenca[index];

            if (cuenca.Estado === "Pendiente") {
                // Eliminar visualmente para estado "Pendiente"
                listaCuenca.splice(index, 1);
                $('#fila' + id).remove();
                mostrarMensaje('mensajeFicha', 'exito', 'Fila eliminada correctamente.');
            } else if (cuenca.Estado === "Cargado") {
                // Consumir el servicio para eliminar para estado "Cargado"
                $.ajax({
                    type: "POST",
                    url: controlador + "EliminarCuenca",
                    data: { id: valor },
                    dataType: "json",
                    success: function (result) {
                        if (result == 1) {
                            // Eliminar visualmente tras éxito del servicio
                            listaCuenca.splice(index, 1);
                            $('#fila' + id).remove();
                            mostrarMensaje('mensajeFicha', 'exito', 'La cuenca se eliminó correctamente.');
                        } else {
                            mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error al eliminar la cuenca.');
                        }
                    },
                    error: function () {
                        mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error al intentar eliminar la cuenca.');
                    }
                });
            } else {
                mostrarMensaje('mensajeFicha', 'alert', 'No se puede eliminar la fila en su estado actual.');
            }
        } else {
            mostrarMensaje('mensajeFicha', 'error', 'No se encontró la fila para eliminar.');
        }
        popupClose('popupProyectoGeneral');
    });
}


function cargarCatalogoAnio(idobj) {
    var id = 8;
    console.log("Entro excelsssss");
    $.ajax({
        type: 'POST',
        url: controlador + 'ListarCatalogo',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (eData) {
            if (eData.length > 0) {
                cargarListaParametrosCuenca(eData, idobj);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaParametrosCuenca(listaValores, idobj) {
    anioFin = $('#txtPoyAnio').val(); 
    console.log(anioFin);
    console.log("Entro excel1");
    $.each(listaValores, function (index, catalogo) {
        // Crear la opción
        console.log(listaValores);
        if (index == 0) {
            anioInicio = catalogo.Valor;
            console.log("anioInicio = catalogo.Valor;", catalogo.Valor);
            var obCuenca = listaCuenca.find(cuenca => cuenca.id === idobj);
            tablaHojaDHan(anioInicio, anioFin, obCuenca)
          
        }
    });
}


function grabarFichaD() {
    if (entre) {
        const resultadg = validarTablaAntesDeGuardar(hotFichaD);
   
        if (!resultadg) {
        return;
        }
    }
    if (cambiosRealizados == true && entre == true ) {
        guardarDetalleTabla();
        cambiosRealizados = false;
    }

    const divInicio = document.getElementById('panel-container');
    divInicio.scrollTop = 0;
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    param.ListRegHojaD = listaCuenca;
    var idProyecto = $("#txtPoyCodi").val();

    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;

        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarFichaD',
            data: param,
            success: function (result) {
                if (result) {
                    listaCuenca.forEach((cuenca) => {
                        cuenca.Estado = "Cargado"; // Actualiza el estado
                    });

                    // Reflejar el estado actualizado en la tabla
                    listaCuenca.forEach((cuenca) => {
                        $(`#fila${cuenca.id} td:nth-child(3)`).text(cuenca.Estado);
                    });

                    mostrarMensaje('mensajeFicha', 'exito', 'Los datos se grabaron correctamente.');
                } else {
                    mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
            }
        });
    }
    cambiosRealizados = false;
}
// Validar toda la fila cuando se edita o elimina contenido
function validateRow(hotInstance, rowIndex) {
    const totalCols = hotInstance.countCols();
    let isValidRow = true;

    for (let col = 0; col < totalCols; col++) {
        const cellValue = hotInstance.getDataAtCell(rowIndex, col);
        const config = configuracionColumnasG1D[col];
        const cellMeta = hotInstance.getCellMeta(rowIndex, col);

        if (config) {
            if (cellValue === "" || cellValue === null) {
              
                    cellMeta.valid = true;
                    cellMeta.style = { backgroundColor: "" }; // Sin fondo
                
            } else {
                const parsedValue = parseFloat(cellValue);
                if (isNaN(parsedValue) || (!config.permitirNegativo && parsedValue < 0)) {
                    isValidRow = false;
                    cellMeta.valid = false;
                    cellMeta.style = { backgroundColor: "#ffcccc" }; // Fondo rojo
                } else {
                    cellMeta.valid = true;
                    cellMeta.style = { backgroundColor: "" }; // Sin fondo
                }
            }
        }
    }

    // Aplicar estilos de validación a toda la fila
    hotInstance.render();
    return isValidRow;
}
function tablaHojaDHan(inicioan, finan, obSeleccionado) {
    var data = [];
    totalAniosD = finan - inicioan;

    // Crear las filas para cada año
    for (var i = 0; i <= totalAniosD; i++) {
        var row = [];
        row.push(Number(inicioan) + i); // Columna de años
        for (let j = 0; j < 12; j++) {
            row.push(""); // Inicializar meses vacíos
        }
        data.push(row);
    }

    // Cargar datos de detalle
    var maximoAnualRow = ["MAXIMO"];
    var minimoAnualRow = ["MINIMO"];
    var mediaAnualRow = ["MEDIA"];
    for (let i = 0; i < 12; i++) {
        maximoAnualRow.push("");
        minimoAnualRow.push("");
        mediaAnualRow.push("");
    }
    data.push(maximoAnualRow);
    data.push(minimoAnualRow);
    data.push(mediaAnualRow);

    var structuredData = obSeleccionado.ListDetRegHojaD;
    if (structuredData && structuredData.length > 0) {
        const months = [
            "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
            "Julio", "Agosto", "Setiembre", "Octubre", "Noviembre", "Diciembre"
        ];


        // Procesar datos del backend (structuredData)
        var structuredData = obSeleccionado.ListDetRegHojaD;
        if (structuredData && structuredData.length > 0) {
            const months = [
                "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                "Julio", "Agosto", "Setiembre", "Octubre", "Noviembre", "Diciembre"
            ];

            structuredData.forEach(item => {
                let yearIndex = item.Anio - inicioan; // Determinar fila por año
                let monthIndex = months.indexOf(item.Mes); // Determinar columna por mes
                if (yearIndex >= 0 && yearIndex <= totalAniosD && monthIndex >= 0 && monthIndex < 12) {
                    data[yearIndex][monthIndex + 1] = item.Valor; // Asignar valor a la celda correspondiente
                }
            });
        }
    }
    
    var grilla = document.getElementById("detalleFormatoHojaD");

    if (typeof hotFichaD !== "undefined" && hotFichaD !== null) {
        hotFichaD.destroy();
    }

    hotFichaD = new Handsontable(grilla, {
        data: data,
        rowHeaders: true,
        colHeaders: [
            "Anual", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
            "Julio", "Agosto", "Setiembre", "Octubre", "Noviembre", "Diciembre", "Media Anual",
        ],
        colWidths: 170,
        manualColumnResize: true,
        columnSorting: false,
        filters: false,
        dropdownMenu: true,
        autoColumnSize: true,
        columns: [
            { readOnly: true }, // Columna de años
            ...Array(12).fill({ type: "text" }), // Columnas de meses
            { readOnly: true }, // Media Anual (última columna)
        ],
        cells: function (row, col) {
            var cellProperties = {};

            // Aplicar fondo gris en la columna "Media Anual"
            if (col === 13) {
                cellProperties.renderer = function (instance, td, row, col, prop, value, cellProperties) {
                    Handsontable.renderers.TextRenderer.apply(this, arguments);
                    td.style.backgroundColor = "#FAFAFA"; // Fondo gris claro
                };
            }
 

            return cellProperties;
        },
        beforeChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
                return generalBeforeChange_4(this, changes, configuracionColumnasG1D);
            }
        }
    });

    hotFichaD.addHook("afterChange", function (changes, source) {
        if (source !== "loadData") { // Evitar validación al cargar datos
            changes.forEach(([row, col, oldValue, newValue]) => {
                validateRow(hotFichaD, row);
            });
        }
    });
    // Configura los eventos solo si aún no están configurados
    if (!hotFichaD.hasHook('afterChange')) {
        hotFichaD.addHook("afterChange", function (changes, source) {
            if (source !== "runningMyCalc") {

                updateTotals();
            }
        });
    }
    //if (!hotFichaD.hasHook('beforeChange')) {
    //    hotFichaD.addHook("beforeChange", function (changes, source) {
    //        console.log("beforeChange event triggered");
    //        generalBeforeChange(this, changes, source);
    //    });
    //}

    hotFichaD.addHook("afterChange", function (changes, source) {
        if (source !== "runningMyCalc") {
            updateTotals();
        }
    });
    entre = true;
    updateTotals();
    hotFichaD.render();
}

function getStructuredData() {
    var structuredData = [];
    var tableData = hotFichaD.getData();
    var months = [
        "Enero",
        "Febrero",
        "Marzo",
        "Abril",
        "Mayo",
        "Junio",
        "Julio",
        "Agosto",
        "Setiembre",
        "Octubre",
        "Noviembre",
        "Diciembre",
    ];

    for (let i = 0; i <= totalAniosD; i++) {
        for (let j = 1; j <= 12; j++) {
            if (
                tableData[i][j] !== "" &&
                !isNaN(parseFloat(tableData[i][j])) &&
                tableData[i][j] !== null
            ) {
                structuredData.push({
                    Anio: tableData[i][0],
                    Mes: months[j - 1],
                    Valor: parseFloat(tableData[i][j]),
                });
            }
        }
    }
    console.log(structuredData);

    return structuredData;
}


function guardarDetalleTabla() {
    
    if (entre) { 
    if (!validarTablaAntesDeGuardar(hotFichaD)) {
        mostrarNotificacion("No se puede guardar porque hay datos inválidos en la tabla.");
        return ; // Detener el guardado si hay errores
        }
    }
    entre = false;
 
    var listaDetalle = getStructuredData();
    var idObtenido = $('#txtidcabeceraD').val();
    var obCuenca = listaCuenca.find(cuenca => cuenca.id === idObtenido);

    if (!obCuenca) {
        console.log(`No se encontró la cuenca con el ID: ${idObtenido}`);
        return; // Salir de la función si no se encuentra la cuenca
    }

    if (listaDetalle.length > 0) {
        // Formatear todos los valores desde la segunda columna a 4 decimales
        listaDetalle = listaDetalle.map(detalle => {
            if (!isNaN(detalle.Valor)) {
                detalle.Valor = parseFloat(detalle.Valor).toFixed(4); // Formatear a 4 decimales
            }
            return detalle;
        });

        cambiosRealizados = true;
        obCuenca.ListDetRegHojaD = listaDetalle; // Actualiza los detalles
        obCuenca.Estado = "Cargado"; // Cambia el estado
        console.log('Datos guardados:', obCuenca);
    } else {
        console.log(`No hay datos para guardar en la cuenca con el ID: ${idObtenido}`);
    }

    $('#txtidcabeceraD').val("0");
    $("#formInicioTablaD").show();
    $("#formFinTablaD").hide();
    return true;
}



function validarHojaD() {
    var mensaje = "<ul>";
    var flag = true;

    if ($("#txtCuencaCabecera").val().length < 1) {
        mensaje = mensaje + "<li>Ingrese una Cuenca</li>";
        flag = false;
    }

    if ($("#txtCaudalCabecera").val().length < 1) {
        mensaje = mensaje + "<li>Ingrese nombre de caudalo</li>";
        flag = false;
    }

    mensaje = mensaje + "</ul>";

    if (flag) mensaje = "";

    return mensaje;
}

function handleFile() {
    var fileInput = document.createElement('input');
    fileInput.type = 'file';
    fileInput.accept = '.xlsx, .xls';
    fileInput.onchange = function (e) {
        var file = e.target.files[0];
        var reader = new FileReader();

        reader.onload = function (e) {
            var data = new Uint8Array(e.target.result);
            var workbook = XLSX.read(data, { type: 'array' });

            var firstSheet = workbook.Sheets[workbook.SheetNames[0]];

            var jsonData = XLSX.utils.sheet_to_json(firstSheet, { header: 1 });

            // Llama a la función para procesar los datos
            updateTableFromExcel(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}


function updateTableFromExcel(jsonData) {
    const headers = jsonData[0]; // La primera fila son los encabezados
    const data = jsonData.slice(1); // El resto son los datos (contenido del Excel)

    // Obtener los datos actuales de la tabla
    const existingData = hotFichaD.getData();

    // Limpiar todas las columnas (excepto la primera columna)
    const clearedData = existingData.map(row => {
        for (let colIndex = 1; colIndex < row.length; colIndex++) {
            row[colIndex] = ""; // Limpiar todas las columnas desde la segunda
        }
        return row;
    });

    // Actualizar solo las columnas a partir de la segunda, sin modificar la primera columna
    const updatedData = clearedData.map((row, rowIndex) => {
        if (rowIndex < data.length) {
            const importedRow = data[rowIndex]; // Datos correspondientes en la fila importada
            for (let colIndex = 1; colIndex < row.length; colIndex++) { // Empezar desde la segunda columna
                const value = parseFloat(importedRow[colIndex]);
                if (!isNaN(value) && value >= 0) {
                    row[colIndex] = value; // Actualizar las celdas con valores válidos
                }
            }
        }
        return row; // Mantener la fila actualizada
    });

    // Recargar la tabla con los datos actualizados
    hotFichaD.loadData(updatedData);
}


function updateTotals() {
    for (let j = 1; j <= 12; j++) {
        hotFichaD.setDataAtCell(
            totalAniosD + 1,
            j,
            calculateMax(j),
            "runningMyCalc"
        );
        hotFichaD.setDataAtCell(
            totalAniosD + 2,
            j,
            calculateMin(j),
            "runningMyCalc"
        );
        hotFichaD.setDataAtCell(
            totalAniosD + 3,
            j,
            calculateMean(j),
            "runningMyCalc"
        );
    }

    for (let i = 0; i <= totalAniosD; i++) {
        hotFichaD.setDataAtCell(
            i,
            13,
            calculateAnnualMean(i),
            "runningMyCalc"
        );
    }
}



function exportarTablaExcel() {
    var headers = [
        [
            "Anual",
            "Enero",
            "Febrero",
            "Marzo",
            "Abril",
            "Mayo",
            "Junio",
            "Julio",
            "Agosto",
            "Setiembre",
            "Octubre",
            "Noviembre",
            "Diciembre",
            "Media Anual",
        ]
    ];

    var datosArray = hotFichaD.getData();

    // Excluir filas de totales y columna no deseada
    var dataSinTotales = datosArray.slice(0, totalAniosD + 1);

    // Calcular la media anual por fila y agregarla
    var dataFinal = dataSinTotales.map(row => {
        let valoresMensuales = row.slice(1, 13).map(val => parseFloat(val)).filter(val => !isNaN(val));
        let mediaAnual = valoresMensuales.length > 0
            ? (valoresMensuales.reduce((sum, val) => sum + val, 0) / valoresMensuales.length).toFixed(2)
            : "";
        return row.slice(0, -1).concat(mediaAnual); // Agrega la media anual al final de la fila
    });

    // Combinar encabezados y datos
    var data = headers.concat(dataFinal);

    // Crear hoja y aplicar bordes
    var ws = XLSX.utils.aoa_to_sheet(data);
    aplicarBordes(ws);

    // Aplicar fondo gris a la columna "Media Anual"
    aplicarFondoGris(ws, [13], hotFichaD, headers.length);

    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Caudales");

    // Descargar archivo
    XLSX.writeFile(wb, "G1D_02-Caudales.xlsx");
}

function calculateMax(columnIndex) {
    let values = getDataForColumn(columnIndex);
    return values.length ? Math.max(...values) : "";
}

function calculateMin(columnIndex) {
    let values = getDataForColumn(columnIndex);
    return values.length ? Math.min(...values) : "";
}

function calculateMean(columnIndex) {
    let values = getDataForColumn(columnIndex);
    return values.length
        ? (
            values.reduce((acc, val) => acc + val, 0) / values.length
        ).toFixed(2)
        : "";
}

function calculateAnnualMean(rowIndex) {
    let values = [];
    for (let j = 1; j <= 12; j++) {
        let cellValue = parseFloat(hotFichaD.getDataAtCell(rowIndex, j));
        if (!isNaN(cellValue)) {
            values.push(cellValue);
        }
    }
    return values.length
        ? (
            values.reduce((acc, val) => acc + val, 0) / values.length
        ).toFixed(2)
        : "";
}

function getDataForColumn(columnIndex) {
    var values = [];
    for (let i = 0; i <= totalAniosD; i++) {
        let cellValue = parseFloat(hotFichaD.getDataAtCell(i, columnIndex));
        if (!isNaN(cellValue)) {
            values.push(cellValue);
        }
    }
    return values;
}

function crearPuploadFichaD() {
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSubirFormatoCuenca',
        url: controladorFichas + 'UploadArchivoGeneral',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '5mb',
            mime_types: [
                { title: "Imagenes", extensions: "jpg,jpeg,gif,png" },
                { title: "Archivos comprimidos", extensions: "zip,rar" },
                { title: "Documentos", extensions: "pdf,doc,docx,xls,xlsx,ppt,pptx,csv,msg" }
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                if (uploader.files.length == 2) {
                    uploader.removeFile(uploader.files[0]);
                }
                uploader.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarMensaje('mensajeFicha', 'alert', "Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            FileUploaded: function (up, file, response) {
                var json = JSON.parse(response.response);
                if (json.indicador == 1) {
                    up.removeFile(file);
                    procesarArchivoFichaD(json.fileNameNotPath, json.nombreReal, json.extension)
                }
            },
            UploadComplete: function (up, file) {
                mostrarMensaje('mensajeFicha', 'alert', "Subida completada. <strong>Por favor espere...</strong>");

            },
            Error: function (up, err) {
                mostrarMensaje('mensajeFicha', 'error', err.code + "-" + err.message);
            }
        }
    });
    uploader.init();
}

function procesarArchivoFichaD(fileNameNotPath, nombreReal, tipo) {
    param = {};
    param.SeccCodi = 1;
    param.ProyCodi = $("#txtPoyCodi").val();;
    param.ArchNombre = nombreReal;
    param.ArchNombreGenerado = fileNameNotPath;
    param.ArchTipo = tipo;
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'GrabarRegistroArchivo',
        data: param,
        
        success: function (result) {
            if (result.responseResult > 0) {
                mostrarMensaje('mensajeFicha', 'exito', 'Archivo registrado correctamente.');
                console.log("Id Archivo:" + result.id)
                agregarFilaFichaD(nombreReal, result.id);
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

function agregarFilaFichaD(nombre, id) {
    // Obtener el cuerpo de la tabla
    var nuevaFila = '<tr id="fila' + id + '">' +
        '<td>' + nombre + '</td>' +
        '<td> <a href="#"><img src="' + siteRoot + 'Content/Images/eliminar.png" class="btnEliminar" onclick="eliminarFile(' + id + ')" title="Eliminar archivo"/></a></td>' +
        '</tr>';
    var tabla = $('#tablaCuencaFile');
    tabla.append(nuevaFila);
}

$(document).on('click', '.detalle-link', function (e) {
    entre = true;
    e.preventDefault(); // Evita el comportamiento predeterminado del enlace
    const uuid = $(this).data('uuid'); // Obtiene el UUID del enlace
    const datosDetalle = $(`#fila${uuid}`).data('detalle'); // Obtiene los datos almacenados en la fila

    if (datosDetalle) {
        mostraTablaDetalleBack(uuid, datosDetalle);
    } else {
        console.log(`No se encontraron detalles para la fila con ID: ${uuid}`);
    }
});

setHojaD = function (param) {
    // Limpia la tabla para evitar duplicados
    $("#tablaCuenca tbody").html("");

    if (param && param.length > 0) {
        param.forEach((detalle) => {
            var uuid = generateUUID();
            var estado = detalle.ListDetRegHojaD && detalle.ListDetRegHojaD.length > 0 ? "Cargado" : detalle.Estado;

            // Añade una fila para cada entrada
            $("#tablaCuenca tbody").append(`
                <tr id="fila${uuid}">
                    <td>${detalle.Cuenca}</td>
                    <td>${detalle.Caudal}</td>
                    <td>${estado}</td>
                    <td>
                        <a href="#" class="detalle-link btnActive" data-uuid="${uuid}">
                            <img src="${siteRoot}Areas/Campanias/Content/Images/datosHidro.png" title="Datos Hidrológicos" />
                        </a>
                        <a href="#" onclick="eliminarFilaCuenca('${uuid}','${detalle.Hojadcodi}')">
                            <img src="${siteRoot}Content/Images/eliminar.png" title="Eliminar" />
                        </a>
                    </td>
                </tr>
            `);

            $(`#fila${uuid}`).data('detalle', detalle.ListDetRegHojaD || []);

            listaCuenca.push({
                Caudal: detalle.Caudal,
                Cuenca: detalle.Cuenca,
                id: uuid,
                Estado: estado,
                ListDetRegHojaD: detalle.ListDetRegHojaD || [],
            });
        });
    } else {
        console.log("No hay datos para mostrar en la tabla.");
    }

    if (modoModel === "consultar") {
        desactivarCamposFormulario('HFichaD');
        $('#btnGrabarFichaD').hide();
    }
};

function cargarDatosD() {
    console.log("Cargando datos de la Ficha D...");
    listaCuenca = []; // Vacía la lista de cuencas
    $("#tablaCuenca tbody").html(""); // Elimina todas las filas de la tabla

    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();

        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetChHojaD',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            success: function (response) {
                console.log("Respuesta del servidor:", response);

                if (response.success && response.responseResult) {
                    // Formatear los valores a 4 decimales
                    const formattedData = response.responseResult.map(detalle => {
                        if (detalle.ListDetRegHojaD) {
                            detalle.ListDetRegHojaD = detalle.ListDetRegHojaD.map(item => ({
                                ...item,
                                Valor: parseFloat(item.Valor).toFixed(4), // Formatear a 4 decimales
                            }));
                        }
                        return detalle;
                    });

                    setHojaD(formattedData); // Pasar los datos formateados

                    // Agregar evento de clic para cargar el detalle
                    formattedData.forEach(detalle => {
                        if (detalle.ListDetRegHojaD && detalle.ListDetRegHojaD.length > 0) {
                            $(`#fila${detalle.Hojadcodi} a:first`).on('click', function (e) {
                                e.preventDefault();
                                mostraTablaDetalleBack(detalle.Hojadcodi, detalle.ListDetRegHojaD);
                            });
                        }
                    });
                } else {
                    console.log("No se recibieron datos válidos del servidor.");
                }
                cambiosRealizados = false;
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error al cargar los datos.');
            }
        });
    }
}
