var controlador = siteRoot + 'campanias/plantransmision/';
var controladorFichas = siteRoot + 'campanias/fichasproyecto/';
var hotHojaclineas = null;
var valoresListaC = [];

$(function () {
    $('#btnGrabarFichaC').on('click', function () {
        grabarFichaC();
    });
    cargarCatalogoTablaFichaC(catRequisitoCentralHidro);
    //$('#txtFechaPuertaOperacion').val(obtenerFechaActualMesAnio());
    $('#txtFechaPuertaOperacion').Zebra_DatePicker({
        format: 'm/Y',
        onSelect: function () {
            cambiosRealizados = true; // Marcar que hubo un cambio
        }
    });
});

getDataHojaC = function () {
    var param = {};
    param.Fecpuestaope = $("#txtFechaPuertaOperacion").val();
    return param;
}


function cargarCatalogoTablaFichaC(id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListarCatalogo',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (eData) {
            //cargarListaParametrosTablaFichaC(eData, selectHtml, horizonteInicio, horizonteFin);
            valoresListaC = eData;
            tablaHojaCHan(anioPeriodo, horizonteFin)
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function grabarFichaC() {
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.RegHojaCDTO = getDataHojaC();
        param.DetRegHojaCDTO = getDataDetHojaC();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarFichaC',
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
}

function tablaHojaCHan(inicioan, finan) {
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

    var grilla = document.getElementById("detalleFormatoHojaCHidro");
    console.log("grilla:", grilla);

    if (typeof hotHojaclineas !== "undefined" && hotHojaclineas !== null) {
        hotHojaclineas.destroy();
    }

    hotHojaclineas = new Handsontable(grilla, {
        data: data,
        // rowHeaders: true,
        colHeaders: true,
        colWidths: [0,200, 100, 50],
        manualColumnResize: true,
        columnSorting: false,
        filters: false,
        dropdownMenu: true,
        autoColumnSize: true,
        autoRowSize: true,
        nestedHeaders: [[...aniosCabecera], [...headerCabe]],
        columns: [
            { readOnly: true }, // Columna oculta
            { readOnly: true }, // Columna "Año"
            { type: "checkbox" }, // Columna "Año 2023 o antes"
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

    hotHojaclineas.render();
}


function getDataDetHojaC() {
    var hotInstance = hotHojaclineas.getInstance(); // Obtener la instancia de Handsontable
    var data = hotInstance.getData(); // Obtener los datos actuales de la tabla
    var valoresMarcados = [];

    // Iterar sobre los datos para obtener los valores marcados
    for (var i = 0; i < data.length; i++) {
        var fila = data[i];
        var desDataCat = fila[0]; // Obtener el texto de la columna "DesDataCat"
        var dataCat = valoresListaC.find(item => item.DataCatCodi === desDataCat); // Buscar el objeto DATACATCODI correspondiente a desDataCat

        if (!dataCat) {
            console.warn("No se encontró DATACATCODI para:", desDataCat);
            continue;
        }

        // Manejar la segunda columna "Año 0 Trimestre 0"
        var segundoColumnaMarcada = fila[2]; // Obtener el valor de la segunda columna
        if (segundoColumnaMarcada) {
            var param = {};
            param.Datacatcodi = dataCat.DataCatCodi;
            param.Anio = 0;
            param.Trimestre =0;
            param.Valor = 1;
            valoresMarcados.push(param)
        }

        // Iterar sobre las columnas con checkboxes
        for (var j = 3; j < fila.length; j++) {
            var anio = Math.floor((j - 3) / 4) + 1; // Calcular el año basado en la posición de la columna
            var trimestre = (j - 3) % 4 + 1; // Calcular el trimestre basado en la posición de la columna
            var estaMarcado = fila[j]; // Verificar si el checkbox está marcado

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

function cargarDatosC() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetChHojaC',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            
            success: function (response) {
                console.log(response);
                var hojaCRes = response.responseResult;
                setDataHojaC(hojaCRes);
                setDataDetHojaC(hojaCRes.DetRegHojaCs);
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

setDataHojaC = function (dataResponse) {
   // var fechaConvertida = convertirFechaMesAnio(dataResponse.Fecpuestaope)
    $("#txtFechaPuertaOperacion").val(dataResponse.Fecpuestaope);
    if (modoModel == "consultar") {
        desactivarCamposFormulario('FichaC');
        $("#btnGrabarFichaC").hide();
        hotHojaclineas.updateSettings({
            cells: function (row, col) {
                var cellProperties = {};
                cellProperties.readOnly = true;
                return cellProperties;
            }
        });
    }
}

function setDataDetHojaC(datosSeteo) {
    var hotInstance = hotHojaclineas.getInstance(); // Obtener la instancia de Handsontable
    var data = hotInstance.getData(); // Obtener los datos actuales de la tabla

    // Iterar sobre los datos de seteo
    if(datosSeteo){
        for (var i = 0; i < datosSeteo.length; i++) {
            var dato = datosSeteo[i];
            var dataCatCodi = dato.Datacatcodi; // Convertir a string para comparación
            var anio = parseInt(dato.Anio);
            var trimestre = parseInt(dato.Trimestre);
            var valor = parseInt(dato.Valor); // Asumiendo que Valor es un número (1 o 0)
            var rowIndex = data.findIndex(row => row[0] == dataCatCodi);
    
            if (rowIndex !== -1) {
                // Actualizar la columna "Año 0 Trimestre 0"
                if (anio === 0 && trimestre === 0) {
                    hotInstance.setDataAtCell(rowIndex, 2, true); // Marcar la casilla correspondiente
                }
    
                // Actualizar la columna correspondiente al año y trimestre
                if (anio > 0 && trimestre > 0) {
                    // Calcular el índice de la columna en función de anio y trimestre
                    var columnIndex = 3 + (anio - 1) * 4 + (trimestre - 1);
    
                    if (columnIndex < data[rowIndex].length) {
                        hotInstance.setDataAtCell(rowIndex, columnIndex, true); // Marcar la casilla correspondiente
                    }
                }
            }
        }
    }
}