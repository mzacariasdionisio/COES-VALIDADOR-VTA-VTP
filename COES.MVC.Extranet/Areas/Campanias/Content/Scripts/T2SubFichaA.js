var seccionUbicacionSubA = 4;
var seccionDiagramaUni = 5;
var seccionSubA = 17;
var numTrafo = 4;
var numEquipo = 4;
var hotT2Sub1 = null;
var hotT2Sub2 = null;
var hotT2Sub3 = null;
var catTrasnformadoresPotencia = 33;
var catTrasnformadoresPotenciaPruebas = 39;
var catCompensacionReactivaPruebas = 40;
var listaCatalogo = [];
var listaCatalogo2 = [];
var listaCatalogo3 = [];

$(function () {
    cargarCatalogoTablaSubestacion(catTrasnformadoresPotencia);
    cargarCatalogoTablaSubestacion2(catTrasnformadoresPotenciaPruebas);
    cargarCatalogoTablaSubestacion3(catCompensacionReactivaPruebas);
    var ruta_interna = obtenerRutaProyecto();  
    crearUploaderGeneral('btnSubirUbicacionSubA', '#tablaUbicacionSubest', seccionUbicacionSubA, null, ruta_interna);
    crearUploaderGeneral('btnSubirUnifaSubA', '#tablaDiagramaUnifiliarFile', seccionDiagramaUni, null, ruta_interna);
  //  $('#txtFechaPuestaServicio').val(obtenerFechaActualMesAnio());
    $('#txtFechaPuestaServicio').Zebra_DatePicker({
        format: 'm/Y',
        view: 'months',
        select_other_months: false,
        show_select_today: false,
        months: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Setiembre', 'Octubre', 'Noviembre', 'Diciembre'],
        onSelect: function () {
            cambiosRealizados = true; // Marcar que hubo un cambio
        }
    });
    var formulariost2a = document.getElementById('FichaT2A');
    formulariost2a.addEventListener('change', function () {
        cambiosRealizados = true;
    });
});

function grabarFichaT2A() {
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.t2SubestFicha1DTO = getDataHoja1Subest();
        param.listaT2SubestFicha1Det1DTO = getDatosSubEstDet1(); //listaT2SubestFicha1Det1DTO
        param.listaT2SubestFicha1Det2DTO = getDatosSubEstDet2()
        param.listaT2SubestFicha1Det3DTO = getDatosSubEstDet3()
        console.log(param);
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarT2T2SubestFicha1',
            data: param,
            
            success: function (result) {
                console.log(result);
                if (result) {
                    mostrarMensaje('mensajeFicha', 'exito', 'Los datos se grabaron correctamente.')
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
    cambiosRealizados = false
}




function crearUploader(idBoton, idTabla, seccionCodigo, procesarArchivoCallback) {
    var uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: idBoton,
        url: controladorFichas + 'UploadArchivoGeneral',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '5mb',
            mime_types: [
                { title: "Imagenes", extensions: "jpg,jpeg,gif,png" },
                { title: "Archivos comprimidos", extensions: "zip,rar,kmz" },
                { title: "Documentos", extensions: "pdf,doc,docx,xls,xlsx,ppt,pptx,csv,msg" }
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                if (up.files.length > 1) {
                    up.removeFile(up.files[0]);
                }
                up.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarMensaje('mensajeFicha', 'alert', "Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            FileUploaded: function (up, file, response) {
                var json = JSON.parse(response.response);
                if (json.indicador == 1) {
                    up.removeFile(file);
                    procesarArchivoCallback(json.fileNameNotPath, json.nombreReal, json.extension, idTabla, seccionCodigo);
                }
            },
            UploadComplete: function (up, files) {
                mostrarMensaje('mensajeFicha', 'alert', "Subida completada. <strong>Por favor espere...</strong>");
            },
            Error: function (up, err) {
                mostrarMensaje('mensajeFicha', 'error', err.code + "-" + err.message);
            }
        }
    });

    uploader.init();
    return uploader;
}

function procesarArchivoSubA(fileNameNotPath, nombreReal, tipo, tabla, seccionCodigo) {
    var param = {
        SeccCodi: seccionCodigo,
        ProyCodi: $("#txtPoyCodi").val(),
        ArchNombre: nombreReal,
        ArchNombreGenerado: fileNameNotPath,
        ArchTipo: tipo
    };

    $.ajax({
        type: 'POST',
        url: controladorFichas + 'GrabarRegistroArchivo',
        data: param,
        
        success: function (result) {
            if (result.responseResult > 0) {
                mostrarMensaje('mensajeFicha', 'exito', 'Archivo registrado correctamente.');
                console.log("Id Archivo:" + result.id);
                agregarFilaTablaSubA(nombreReal, result.id, tabla);
            } else {
                mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
        }
    });
}

function agregarFilaTablaSubA(nombre, id, tabla) {
    var nuevaFila = '<tr id="fila' + id + '">' +
        '<td>' + nombre + '</td>' +
        '<td><a href="#"><img src="'+ siteRoot + 'Content/Images/eliminar.png" class="btnEliminar" onclick="eliminarFile(' + id + ')" title="Eliminar archivo"/></a></td>' +
        '</tr>';

    $(tabla).append(nuevaFila);
}


getDataHoja1Subest = function () {
    var param = {};
    param.Nombresubestacion = $("#txtNombreSubestacion").val();
    param.Tipoproyecto = $("#txtTipoProyecto").val();
    param.Fechapuestaservicio = $("#txtFechaPuestaServicio").val();
    param.Empresapropietaria = $("#txtEmpresaPropietaria").val();
    param.Sistemabarras = $("#sistemaBarrasSelect").val();
    param.OtroSistemaBarras = $("#txtOtroSistemaBarras").val();
    param.Numtrafo = numTrafo;
    param.Numequipos = numEquipo;
    return param;
}

function crearTablaSub1(numTrafo, existingData = [], setData1 = []) {
    var grilla = document.getElementById("detalle1TablaSubestacion");

    // Ordenar listaCatalogo por DataCatCodi en orden ascendente
    listaCatalogo.sort(function (a, b) {
        return a.DataCatCodi - b.DataCatCodi;
    });
    var data = [];
    for (var i = 0; i < listaCatalogo.length; i++) {
        var row = [];
        row.push(listaCatalogo[i].DataCatCodi);
        row.push(listaCatalogo[i].DesDataCat);
        row.push(listaCatalogo[i].DescortaDatacat);

        // Si hay datos existentes, mant�n esos datos y a�ade columnas vac�as para los nuevos trafos
        if (existingData[i]) {
            for (var j = 3; j < existingData[i].length; j++) {
                row.push(existingData[i][j]);  // Mant�n los valores de los trafos existentes
            }
        }

        // A�ade columnas vac�as para los nuevos trafos adicionales
        for (var j = existingData[i] ? existingData[i].length - 3 : 0; j < numTrafo; j++) {
            row.push("");
        }

        data.push(row);
    }

    if (hotT2Sub1 !== null) {
        hotT2Sub1.destroy();
    }
    // Crear las cabeceras din�micamente
    var nestedHeaders = [
        ["", "Caracteristicas", ""].concat(Array.from({ length: numTrafo }, (v, i) => `Trafo ${i + 1}`))
    ];

    // Crear las columnas din�micamente
    var columns = [
        { readOnly: true },  // Para DataCatCodi
        { readOnly: true },  // Para DesDataCat
        { readOnly: true }   // Para DescortaDatacat
    ].concat(Array.from({ length: numTrafo }, () => ({})));  // Agregar las columnas de trafos

    hotT2Sub1 = new Handsontable(grilla, {
        data: data,
        rowHeaders: true,
        colHeaders: true,
        colWidths: 170,
        manualColumnResize: true,
        columnSorting: false,
        filters: false,
        contextMenu: true,
        autoColumnSize: true,
        nestedHeaders: nestedHeaders,
        columns: columns,
        hiddenColumns: {
            columns: [0],
            indicators: true,
        },
    });
    hotT2Sub1.render();
    if (setData1.length > 0) setDataDFormatoDet1(setData1);
}

function cargarCatalogoTablaSubestacion(id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListarCatalogo',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (response) {
            listaCatalogo = response;
            console.log(listaCatalogo);
            crearTablaSub1(numTrafo);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarCatalogoTablaSubestacion2(id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListarCatalogo',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (response) {
            listaCatalogo2 = response;
            console.log(listaCatalogo);
            crearTablaSub2(numTrafo);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
function cargarCatalogoTablaSubestacion3(id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListarCatalogo',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (response) {
            listaCatalogo3 = response;
            console.log(listaCatalogo);
            crearTablaSub3(numTrafo);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function crearTablaSub2(numTrafo, existingData = [], setData2 = []) {
    var grilla = document.getElementById("detalle2TablaSubestacion");

    // Ordenar listaCatalogo por DataCatCodi en orden ascendente
    listaCatalogo2.sort(function (a, b) {
        return a.DataCatCodi - b.DataCatCodi;
    });

    var data = [];
    for (var i = 0; i < listaCatalogo2.length; i++) {
        var row = [];
        row.push(listaCatalogo2[i].DataCatCodi);
        row.push(listaCatalogo2[i].DesDataCat);
        row.push(listaCatalogo2[i].DescortaDatacat);

        // Si hay datos existentes, mant�n esos datos y a�ade columnas vac�as para los nuevos trafos
        if (existingData[i]) {
            for (var j = 3; j < existingData[i].length; j++) {
                row.push(existingData[i][j]);  // Mant�n los valores de los trafos existentes
            }
        }

        // A�ade columnas vac�as para los nuevos trafos adicionales
        for (var j = existingData[i] ? existingData[i].length - 3 : 0; j < numTrafo; j++) {
            row.push("");
        }

        data.push(row);
    }

    if (hotT2Sub2 !== null) {
        hotT2Sub2.destroy();
    }

    // Crear las cabeceras din�micamente
    var nestedHeaders = [
        ["", "Pruebas", ""].concat(Array.from({ length: numTrafo }, (v, i) => `Trafo ${i + 1}`))
    ];

    // Crear las columnas din�micamente
    var columns = [
        { readOnly: true },  // Para DataCatCodi
        { readOnly: true },  // Para DesDataCat
        { readOnly: true }   // Para DescortaDatacat
    ].concat(Array.from({ length: numTrafo }, () => ({})));  // Agregar las columnas de trafos

    hotT2Sub2 = new Handsontable(grilla, {
        data: data,
        rowHeaders: true,
        colHeaders: true,
        colWidths: 170,
        manualColumnResize: true,
        columnSorting: false,
        filters: false,
        contextMenu: true,
        autoColumnSize: true,
        nestedHeaders: nestedHeaders,
        columns: columns,
        hiddenColumns: {
            columns: [0],
            indicators: true,
        },
    });
    hotT2Sub2.render();
    if (setData2.length > 0) setDataDFormatoDet2(setData2);
}
function crearTablaSub3(numEquipos, existingData = [], setData3 = []) {
    var grilla = document.getElementById("detalle3TablaSubestacion");

    // Ordenar listaCatalogo por DataCatCodi en orden ascendente
    listaCatalogo3.sort(function (a, b) {
        return a.DataCatCodi - b.DataCatCodi;
    });

    var data = [];
    for (var i = 0; i < listaCatalogo3.length; i++) {
        var row = [];
        row.push(listaCatalogo3[i].DataCatCodi);
        row.push(listaCatalogo3[i].DesDataCat);
        row.push(listaCatalogo3[i].DescortaDatacat);

        // Si hay datos existentes, mant�n esos datos y a�ade columnas vac�as para los nuevos trafos
        if (existingData[i]) {
            for (var j = 3; j < existingData[i].length; j++) {
                row.push(existingData[i][j]);  // Mant�n los valores de los trafos existentes
            }
        }

        // A�ade columnas vac�as para los nuevos trafos adicionales
        for (var j = existingData[i] ? existingData[i].length - 3 : 0; j < numEquipos; j++) {
            row.push("");
        }

        data.push(row);
    }

    if (hotT2Sub3 !== null) {
        hotT2Sub3.destroy();
    }

    // Crear las cabeceras din�micamente
    var nestedHeaders = [
        ["", "Pruebas", ""].concat(Array.from({ length: numEquipos }, (v, i) => `Equipo ${i + 1}`))
    ];

    // Crear las columnas din�micamente
    var columns = [
        { readOnly: true },  // Para DataCatCodi
        { readOnly: true },  // Para DesDataCat
        { readOnly: true }   // Para DescortaDatacat
    ].concat(Array.from({ length: numEquipos }, () => ({})));  // Agregar las columnas de trafos

    hotT2Sub3 = new Handsontable(grilla, {
        data: data,
        rowHeaders: true,
        colHeaders: true,
        colWidths: 170,
        manualColumnResize: true,
        columnSorting: false,
        filters: false,
        contextMenu: true,
        autoColumnSize: true,
        nestedHeaders: nestedHeaders,
        columns: columns,
        hiddenColumns: {
            columns: [0],
            indicators: true,
        },
    });
    hotT2Sub3.render();
    if (setData3.length > 0) setDataDFormatoDet3(setData3);

}
function eliminarFile(id) {
    document.getElementById("contenidoPopup").innerHTML = '¿Estás seguro de realizar esta operación?';
    $('#popupProyectoGeneral').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    $('#btnConfirmarPopup').off('click').on('click', function() {
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'EliminarFile',
            data: {
                id: id,
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    $("#fila" + id).remove();
                    mostrarMensaje('mensajeFicha', 'exito', 'El archivo se eliminó correctamente.');
                }
                else {
                    mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
            }
        });
        popupClose('popupProyectoGeneral');
    });  
}

function getDatosSubEstDet1() {
    var datosArray = hotT2Sub1.getData();
    var nestedHeaders = hotT2Sub1.getSettings().nestedHeaders[0]; // Obtener las cabeceras dinámicas de trafos

    var datosObjetos = [];

    // Recorrer cada fila de la tabla
    datosArray.forEach(function (row) {
        var dataCatCodi = row[0];  // DataCatCodi de la fila

        // Recorrer los trafos (a partir de la cuarta columna en adelante)
        for (var i = 3; i < row.length; i++) {
            var trafoHeader = nestedHeaders[i];  // Obtener el valor de la cabecera, e.g., "Trafo 1", "Trafo 2"
            var trafoNum = parseInt(trafoHeader.replace("Trafo ", ""), 10);  // Extraer el número del trafo
            var valorTrafo = row[i];  // Valor del trafo

            // Crear un objeto para cada trafo
            if (valorTrafo !== "") {  // Solo incluir si el valor del trafo no es vacío
                datosObjetos.push({
                    DataCatCodi: dataCatCodi,
                    NumTrafo: trafoNum,    // El número del trafo extraído de la cabecera
                    ValorTrafo: valorTrafo // El valor del trafo en esa celda
                });
            }
        }
    });

    return datosObjetos;
}

function getDatosSubEstDet2() {
    var datosArray = hotT2Sub2.getData();
    var nestedHeaders = hotT2Sub2.getSettings().nestedHeaders[0]; // Obtener las cabeceras dinámicas de trafos

    var datosObjetos = [];

    // Recorrer cada fila de la tabla
    datosArray.forEach(function (row) {
        var dataCatCodi = row[0];  // DataCatCodi de la fila

        // Recorrer los trafos (a partir de la cuarta columna en adelante)
        for (var i = 3; i < row.length; i++) {
            var trafoHeader = nestedHeaders[i];  // Obtener el valor de la cabecera, e.g., "Trafo 1", "Trafo 2"
            var trafoNum = parseInt(trafoHeader.replace("Trafo ", ""), 10);  // Extraer el número del trafo
            var valorTrafo = row[i];  // Valor del trafo

            // Crear un objeto para cada trafo
            if (valorTrafo !== "") {  // Solo incluir si el valor del trafo no es vacío
                datosObjetos.push({
                    DataCatCodi: dataCatCodi,
                    NumTrafo: trafoNum,    // El número del trafo extraído de la cabecera
                    ValorTrafo: valorTrafo // El valor del trafo en esa celda
                });
            }
        }
    });

    return datosObjetos;
}

function getDatosSubEstDet3() {
    var datosArray = hotT2Sub3.getData();
    var nestedHeaders = hotT2Sub3.getSettings().nestedHeaders[0]; // Obtener las cabeceras dinámicas de trafos

    var datosObjetos = [];

    // Recorrer cada fila de la tabla
    datosArray.forEach(function (row) {
        var dataCatCodi = row[0];  // DataCatCodi de la fila

        for (var i = 3; i < row.length; i++) {
            var trafoHeader = nestedHeaders[i]
            var trafoEquipo = parseInt(trafoHeader.replace("Equipo ", ""), 10);
            var valorEquipo = row[i]; 

            if (valorEquipo !== "") { 
                datosObjetos.push({
                    DataCatCodi: dataCatCodi,
                    NumEquipo: trafoEquipo,    
                    ValorEquipo: valorEquipo 
                });
            }
        }
    });

    return datosObjetos;
}
function agregarTrafo() {
    // Obtener los datos actuales de la tabla
    var existingData = hotT2Sub1.getData();
    var existingData2 = hotT2Sub2.getData();

    // Incrementar el número de trafos
    numTrafo += 1;

    // Volver a crear la tabla con el nuevo número de trafos y los datos existentes
    crearTablaSub1(numTrafo, existingData);
    crearTablaSub2(numTrafo, existingData2);
}

function agregarEquipo() {
    // Obtener los datos actuales de la tabla
    var existingData = hotT2Sub3.getData();

    // Incrementar el número de trafos
    numEquipo += 1;
    crearTablaSub3(numEquipo, existingData);
}

// Agregar después de las funciones existentes en T2SubFichaA.js

function exportarTablaExcelTransformadores1() {
    // Definir los encabezados
    var headers = [
        ["", "Características", ""].concat(Array.from({ length: numTrafo }, (v, i) => `Trafo ${i + 1}`))
    ];

    // Obtener los datos de la tabla
    var datosArray = hotT2Sub1.getData();
    datosArray.forEach((row, index) => {
        row[0] = index + 1;
    });
    var data = headers.concat(datosArray);

    // Crear una hoja de trabajo
    var ws = XLSX.utils.aoa_to_sheet(data);
    aplicarBordes(ws);
    // Aplicar formato de números
    for (var R = 1; R < data.length; R++) {
        for (var C = 3; C < data[R].length; C++) {
            var cellAddress = XLSX.utils.encode_cell({ r: R, c: C });
            var cell = ws[cellAddress];
            if (cell && typeof cell.v === 'number') {
                cell.z = '0.0000';
            }
        }
    }

    // Crear libro y añadir la hoja
    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Ficha1");

    // Exportar el archivo
    XLSX.writeFile(wb, "G6A_03-TransformadorCaracteristica.xlsx");
}

function exportarTablaExcelTransformadores2() {
    // Definir los encabezados
    var headers = [
        ["", "Pruebas", ""].concat(Array.from({ length: numTrafo }, (v, i) => `Trafo ${i + 1}`))
    ];

    // Obtener los datos de la tabla
    var datosArray = hotT2Sub2.getData();
    datosArray.forEach((row, index) => {
        row[0] = index + 1;
    });
    var data = headers.concat(datosArray);

    // Crear una hoja de trabajo
    var ws = XLSX.utils.aoa_to_sheet(data);
    aplicarBordes(ws);
    // Aplicar formato de números
    for (var R = 1; R < data.length; R++) {
        for (var C = 3; C < data[R].length; C++) {
            var cellAddress = XLSX.utils.encode_cell({ r: R, c: C });
            var cell = ws[cellAddress];
            if (cell && typeof cell.v === 'number') {
                cell.z = '0.0000';
            }
        }
    }

    // Crear libro y añadir la hoja
    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Ficha1");

    // Exportar el archivo
    XLSX.writeFile(wb, "G6A_04-TransformadorPrueba.xlsx");
}

function exportarTablaExcelEquipos() {
    // Definir los encabezados
    var headers = [
        ["", "Pruebas", ""].concat(Array.from({ length: numEquipo }, (v, i) => `Equipo ${i + 1}`))
    ];

    // Obtener los datos de la tabla
    var datosArray = hotT2Sub3.getData();
    datosArray.forEach((row, index) => {
        row[0] = index + 1;
    });
    var data = headers.concat(datosArray);

    // Crear una hoja de trabajo
    var ws = XLSX.utils.aoa_to_sheet(data);
    aplicarBordes(ws);
    // Aplicar formato de números
    for (var R = 1; R < data.length; R++) {
        for (var C = 3; C < data[R].length; C++) {
            var cellAddress = XLSX.utils.encode_cell({ r: R, c: C });
            var cell = ws[cellAddress];
            if (cell && typeof cell.v === 'number') {
                cell.z = '0.0000';
            }
        }
    }

    // Crear libro y añadir la hoja
    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Ficha1");

    // Exportar el archivo
    XLSX.writeFile(wb, "G6A_05-EquiposCompensacionReactiva.xlsx");
}

function importarExcelTransformadores1() {
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

            updateTableFromExcelTrans1(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}

function importarExcelTransformadores2() {
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

            updateTableFromExcelTrans2(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}

function importarExcelEquipos() {
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

            updateTableFromExcelEquipos(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}

function updateTableFromExcelTrans1(jsonData) {
    var headers = jsonData[0];// Las dos primeras filas son los headers
    var data = jsonData.slice(1); // El resto son los datos
    numTrafo = data[0].length - 3;
    crearTablaSub1(numTrafo, data)
    var existingData = hotT2Sub2.getData();
    crearTablaSub2(numTrafo, existingData);
}

function updateTableFromExcelTrans2(jsonData) {
    var headers = jsonData[0];// Las dos primeras filas son los headers
    var data = jsonData.slice(1); // El resto son los datos
    numTrafo = data[0].length - 3;
    crearTablaSub2(numTrafo, data);
    var existingData = hotT2Sub1.getData();
    crearTablaSub1(numTrafo, existingData);
}

function updateTableFromExcelEquipos(jsonData) {
    var headers = jsonData[0];// Las dos primeras filas son los headers
    var data = jsonData.slice(1); // El resto son los datos
    numEquipo = data[0].length - 3;
    crearTablaSub3(numEquipo, data)
}

function cargarDatosT2SubA() {
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetT2SubEstacionesA',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),

            success: function (response) {
                var hojaAData = response.responseResult;
                setDataHoja1Subest(hojaAData);
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
    cambiosRealizados = false;
}

setDataHoja1Subest = function (param) {
    $("#txtNombreSubestacion").val(param.NombreSubestacion);
    $("#txtTipoProyecto").val(param.TipoProyecto);
    $("#txtFechaPuestaServicio").val(param.FechaPuestaServicio);
    $("#txtEmpresaPropietaria").val(param.EmpresaPropietaria);
    $("#sistemaBarrasSelect").val(param.SistemaBarras);
    $("#txtOtroSistemaBarras").val(param.OtroSistemaBarras);
    setupDropdownToggle('sistemaBarrasSelect', 'txtOtroSistemaBarras');
    cargarArchivosRegistrados(seccionUbicacionSubA, '#tablaUbicacionSubest');
    cargarArchivosRegistrados(seccionDiagramaUni, '#tablaDiagramaUnifiliarFile');


    if (numTrafo == param.NumTrafo || param.NumTrafo == 0) {
        setDataDFormatoDet1(param.Lista1DTOs);
        setDataDFormatoDet2(param.Lista2DTOs);
    } else {
        numTrafo = param.NumTrafo;
        setTablaTrafo(param);
    }
    if (numEquipo == param.NumEquipos || param.NumEquipos == 0) {
        setDataDFormatoDet3(param.Lista3DTOs);
    } else {
        numEquipo = param.NumEquipos;
        setTablaEquipo(param);
    }
    if (modoModel == "consultar") {
        desactivarCamposFormulario('FichaT2A');
        $('#btnGrabarFichaASubestacion').hide();
    }

}

function setTablaEquipo(hojaAData) {
    var existingData = hotT2Sub3.getData();
    crearTablaSub3(numEquipo, existingData, hojaAData.Lista3DTOs);
}
function setTablaTrafo(hojaAData) {
    var existingData = hotT2Sub1.getData();
    var existingData2 = hotT2Sub2.getData();
    crearTablaSub1(numTrafo, existingData, hojaAData.Lista1DTOs);
    crearTablaSub2(numTrafo, existingData2, hojaAData.Lista2DTOs);
}
function setDataDFormatoDet1(datosSeteo) {
    var hotInstance = hotT2Sub1.getInstance(); // Obtener la instancia de Handsontable
    var data = hotInstance.getData(); // Obtener los datos actuales de la tabla
    // Iterar sobre los datos de seteo
    for (var i = 0; i < datosSeteo.length; i++) {
        var dato = datosSeteo[i];
        var dataCatCodi = dato.DataCatCodi; // Convertir a string para comparación
        var trafo = parseInt(dato.NumTrafo);
        var valor = dato.ValorTrafo;
        var rowIndex = data.findIndex(row => row[0] == dataCatCodi);
        if (rowIndex !== -1) {
            if (trafo) {
                var columnIndex = 2 + trafo;
                if (columnIndex < data[rowIndex].length) {
                    hotInstance.setDataAtCell(rowIndex, columnIndex, valor); // Marcar la casilla correspondiente
                }
            }
        }
    }
}
function setDataDFormatoDet2(datosSeteo) {
    var hotInstance2 = hotT2Sub2.getInstance(); // Obtener la instancia de Handsontable
    var data = hotInstance2.getData(); // Obtener los datos actuales de la tabla
    // Iterar sobre los datos de seteo
    for (var i = 0; i < datosSeteo.length; i++) {
        var dato = datosSeteo[i];
        var dataCatCodi = dato.DataCatCodi; // Convertir a string para comparación
        var trafo = parseInt(dato.NumTrafo);
        var valor = dato.ValorTrafo;
        var rowIndex = data.findIndex(row => row[0] == dataCatCodi);
        if (rowIndex !== -1) {
            if (trafo) {
                var columnIndex = 2 + trafo;
                if (columnIndex < data[rowIndex].length) {
                    hotInstance2.setDataAtCell(rowIndex, columnIndex, valor); // Marcar la casilla correspondiente
                }
            }
        }
    }
}
function setDataDFormatoDet3(datosSeteo) {
    var hotInstance3 = hotT2Sub3.getInstance(); // Obtener la instancia de Handsontable
    var data = hotInstance3.getData(); // Obtener los datos actuales de la tabla
    // Iterar sobre los datos de seteo
    for (var i = 0; i < datosSeteo.length; i++) {
        var dato = datosSeteo[i];
        var dataCatCodi = dato.DataCatCodi; // Convertir a string para comparación
        var equipo = parseInt(dato.NumEquipo);
        var valor = dato.ValorEquipo;
        var rowIndex = data.findIndex(row => row[0] == dataCatCodi);
        if (rowIndex !== -1) {
            if (equipo) {
                var columnIndex = 2 + equipo;
                if (columnIndex < data[rowIndex].length) {
                    hotInstance3.setDataAtCell(rowIndex, columnIndex, valor); // Marcar la casilla correspondiente
                }
            }
        }
    }
}