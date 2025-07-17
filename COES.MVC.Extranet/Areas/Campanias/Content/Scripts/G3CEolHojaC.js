var controlador = siteRoot + 'campanias/plantransmision/';
var controladorFichas = siteRoot + 'campanias/fichasproyecto/';
var hotTableHojaEolC = null;
var valoresListaC = [];
// Las cabeceras deben estar definidas aqu� si no son globales
var aniosCabecera = [];
var headerCabe = [];

$(function () {
    inicializar();
});

function inicializar() {
    cargarCatalogoTablaFichaC(catRequisitoCentralHidro);
 
    $('#txtFechaPuertaOperacionEo').Zebra_DatePicker({
        format: 'm/Y', 
        onSelect: function () {
            cambiosRealizados = true; // Marcar que hubo un cambio
        }
    });
    $('#btngrabarEolHojaC').on('click', function () {
        grabarEolHojaC();
    });
}

getDataHojaCEo = function () {

    var param = {};
    param.Fecpuestaope = $("#txtFechaPuertaOperacionEo").val();
    return param;
}

function cargarCatalogoTablaFichaC(id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListarCatalogo',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (eData) {
            valoresListaC = eData;
            tablaHojaCEol(anioPeriodo, horizonteFin);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
function cargarDatosC() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetEolHojaC',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            
            success: function (response) {
                console.log(response);
                var hojaCRes = response.responseResult;


               
                if (hojaCRes.Proycodi == 0) {
                    //hojaCRes.Fecpuestaope = obtenerFechaActualMesAnio();
                }

                setDataHojaC(hojaCRes);
                setDataDetHojaC(hojaCRes.DetRegHojaEolCDTO);
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}
function tablaHojaCEol(inicioan, finan) {
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

    var grilla = document.getElementById("detalleFormatoHojaEolC");
    console.log("grilla:", grilla);

    if (typeof hotTableHojaEolC !== "undefined" && hotTableHojaEolC !== null) {
        hotTableHojaEolC.destroy();
    }

    hotTableHojaEolC = new Handsontable(grilla, {
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
    hotTableHojaEolC.render();
}
function setDataDetHojaC(datosSeteo) {
    var hotInstance = hotTableHojaEolC.getInstance(); // Obtener la instancia de Handsontable
    var data = hotInstance.getData(); // Obtener los datos actuales de la tabla

    // Iterar sobre los datos de seteo
    if(datosSeteo){
        for (var i = 0; i < datosSeteo.length; i++) {
            var dato = datosSeteo[i];
            var dataCatCodi = dato.Datacatcodi; // Convertir a string para comparaci�n
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
function getDataDetHojaEolC() {
    var hotInstance = hotTableHojaEolC.getInstance(); // Obtener la instancia de Handsontable
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
function grabarEolHojaC() {
    console.log("result");
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.RegHojaEolCDTO = getDataHojaCEo();
        param.DetRegHojaEolCDTO = getDataDetHojaEolC();
        console.log(param);
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarEolHojaC',
            data: param,
            
            success: function (result) {
                console.log("resulte "+result);
                if (result) {
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
setDataHojaC = function (dataResponse) {
    formated = convertirFechaMesAnio(dataResponse.Fecpuestaope);
    setFechaPickerGlobal("#txtFechaPuertaOperacionEo", formated, "mm/aaaa");
    if (modoModel == "consultar") {
        desactivarCamposFormulario('FichaC');
        $("#btngrabarEolHojaC").hide();
        hotTableHojaEolC.updateSettings({
            cells: function (row, col) {
                var cellProperties = {};
                cellProperties.readOnly = true;
                return cellProperties;
            }
        });
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
//function convertirFecha(fechaJSON) {

//    let timestamp = parseInt(fechaJSON.match(/\d+/)[0]);

//    let fecha = new Date(timestamp);

//    let dia = fecha.getDate().toString().padStart(2, '0');
//    let mes = (fecha.getMonth() + 1).toString().padStart(2, '0');
//    let anio = fecha.getFullYear();

//    let fechaFormateada = `${dia}-${mes}-${anio}`;
//    return fechaFormateada;
//}