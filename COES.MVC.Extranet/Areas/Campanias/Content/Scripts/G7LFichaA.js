
var seccion1LinA = 22;
var seccion2LinA = 23;
var seccion3LinA = 24;
var seccion4LinA = 25;
var catInicio = 48;
var catFin = 49;
var hotLineaA1 = null;
var hotLineaA2 = null;

const configuracionColumnasG7A1= {
    0: { tipo: "entero", permitirNegativo: false },
    2: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    4: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    5: { tipo: "entero", permitirNegativo: false },
    6: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    7: { tipo: "entero", permitirNegativo: false },
    8: { tipo: "decimal", decimales: 4, permitirNegativo: false },
};
const configuracionColumnasG7A2 = {
    0: { tipo: "entero", permitirNegativo: false },
    1: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    2: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    3: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    4: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    5: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    6: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    7: { tipo: "decimal", decimales: 4, permitirNegativo: false },
    8: { tipo: "decimal", decimales: 4, permitirNegativo: false },
   
};


$(function () {
    var ruta_interna = obtenerRutaProyecto();  
    crearUploaderGeneral('btnSubirUbicacionLin', '#tablaUbicacionLin', seccion1LinA, null, ruta_interna);
    crearUploaderGeneral('btnSubirFormatoConfgeo', '#tablaConfiguracionGeometrica', seccion2LinA, null, ruta_interna);
    crearUploaderGeneral('btnSubirFormatorutageo', '#tablaRutaGeografica', seccion3LinA, null, ruta_interna);
    crearUploaderGeneral('btnSubirFormatopl', '#tablaPerfilLongi', seccion4LinA, null, ruta_interna);
    cargarCatalogoSubestacion("#txtSubInicio", true);
    cargarCatalogoSubestacion("#txtSubFin", true);
    crearTablaLineasHojaA1([]);
    crearTablaLineasHojaA2([]);
   // $('#txtFecPuestaServ').val(obtenerFechaActualMesAnio());
    $('#txtFecPuestaServ').Zebra_DatePicker({
        format: 'm/Y',
        onSelect: function () {
            cambiosRealizados = true; // Marcar que hubo un cambio
        }
    });
    var formularioald = document.getElementById('GLFichaA');
    formularioald.addEventListener('change', function () {
        cambiosRealizados = true;
    });
});
function grabarFichaLineaA() {
    if (!validarTablaAntesDeGuardar(hotLineaA1,'TABLA 1')) {
        mostrarNotificacion("No se puede guardar porque hay datos inválidos en la tabla.");
        return; // Detener el guardado si hay errores
    }
    if (!validarTablaAntesDeGuardar(hotLineaA2, 'TABLA 2')) {
        mostrarNotificacion("No se puede guardar porque hay datos inválidos en la tabla.");
        return; // Detener el guardado si hay errores
    }
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.lineasFichaADTO = getDataHojaALinea();
        param.listaLineasFichaADet1DTO = getDataHojaALineaDet1();
        param.listaLineasFichaADet2DTO = getDataHojaALineaDet2()
        console.log(param);
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarG7LineasFichaA',
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
    cambiosRealizados = false;

}

getDataHojaALinea = function () {
    var param = {};
    param.NombreLinea = $("#txtNombreLinea").val();
    param.FecPuestaServ = $("#txtFecPuestaServ").val();
    param.SubInicio = $("#txtSubInicio").val();
    param.OtroSubInicio = $("#txtOtroSubInicio").val();
    param.SubFin = $("#txtSubFin").val();
    param.OtroSubFin = $("#txtOtroSubFin").val();
    param.EmpPropietaria = $("#txtEmpresaPropietaria").val();
    param.NivTension = $("#txtnivelTension").val();
    param.CapCorriente = $("#txtCapacidadCorriente").val();
    param.CapCorrienteA = $("#txtCapacidadCorrienteA").val();
    param.TpoSobreCarga = $("#txtTiempoSobrecarga").val();
    param.NumTemas = $("#txtNumTemas").val();
    param.LongTotal = $("#txtLongitud").val();
    param.LongVanoPromedio = $("#txtLongitudVan").val();
    param.TipMatSop = $("#txtTipoMateria").val();
    param.DesProtecPrincipal = $("#txtSistemProteccion").val();
    param.DesProtecRespaldo = $("#txtDescripcionSistema").val();
    param.DesGenProyecto = $("#txtDescripcionGeneral").val();
    console.log(param);
    return param;
}

getDataHojaALineaDet1 = function () {

    var datosArray = hotLineaA1.getData();
    var datosObjetos = datosArray.map(function (row) {
        return {
            Tramo: row[0],
            Tipo: row[1], 
            Longitud: row[2],
            MatConductor: row[3],
            SecConductor: row[4],
            ConductorFase: row[5],
            CapacidadTot: row[6],
            CabGuarda: row[7],
            ResistCabGuarda: row[8]
        };
    }).filter(function (row) {
        // Verificar si al menos una columna tiene un valor no vac�o
        return Object.values(row).some(function
            (value) {
            return value !== "";
        });
    });
    return datosObjetos;
}

getDataHojaALineaDet2 = function () {

    var datosArray = hotLineaA2.getData();
    var datosObjetos = datosArray.map(function (row) {
        return {
            Tramo: row[0],
            R: row[1],
            X: row[2],
            B: row[3],
            G: row[4],
            R0: row[5],
            X0: row[6],
            B0: row[7],
            G0: row[8]
        };
    }).filter(function (row) {
        // Verificar si al menos una columna tiene un valor no vac�o
        return Object.values(row).some(function
            (value) {
            return value !== "";
        });
    });
    return datosObjetos;
}


function crearTablaLineasHojaA1(data) {
    var grilla = document.getElementById("tablaLineasHojaA1");

    if (data.length == 0) {
        for (var i = 0; i < 50; i++) {
            var row = new Array(9).fill("");
            data.push(row);
        }
    }

    if (typeof hotLineaA1 !== "undefined" && hotLineaA1 !== null) {
        hotLineaA1.destroy();
    }

    // Definir las columnas con validaciones
    hotLineaA1 = new Handsontable(grilla, {
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
                { label: "Tramo", colspan: 1 },
                { label: "Tipo", colspan: 1 },
                { label: "Longitud", colspan: 1 },
                { label: "Material de Conductor", colspan: 1 },
                { label: "Seccion del Conductor", colspan: 1 },
                { label: "Conductor por Fase", colspan: 1 },
                { label: "Capacidad Total", colspan: 1 },
                { label: "Cables de Guarda", colspan: 1 },
                { label: "Resistencia Cable de Guarde", colspan: 1 }
            ],
            [
                "#",
                "Aereo o Subterraneo",
                "(km)",
                "Tipo",
                "(mm2)",
                "N",
                "(A)",
                "N",
                "ohm/km"
            ]
        ],
        columns: [
            {
                data: 0,
                type: 'text'
            },
            {
                data: 1
            },
            {
                data: 2,
                type: 'text' 
            },
            {
                data: 3
            },
            {
                data: 4,
                type: 'text' 
            },
            {
                data: 5,
                type: 'text'
            },
            {
                data: 6,
                type: 'text' 
            },
            {
                data: 7,
                type: 'text'
            },
            {
                data: 8,
                type: 'text' 
            }
        ],
        beforeChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {

                return generalBeforeChange_4(this, changes, configuracionColumnasG7A1);
            }
        },
        afterChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
                generalAfterChange_global(this, changes, configuracionColumnasG7A1);
            }
        },
    });

    hotLineaA1.render();
}

function crearTablaLineasHojaA2(data) {
    var grilla = document.getElementById("tablaLineasHojaA2");

    // Si no hay datos, inicializa con 50 filas vac�as
    if (data.length === 0) {
        for (var i = 0; i < 50; i++) {
            var row = new Array(9).fill("");
            data.push(row);
        }
    }

    if (typeof hotLineaA2 !== "undefined" && hotLineaA2 !== null) {
        hotLineaA2.destroy();
    }

    // Crear la tabla Handsontable
    hotLineaA2 = new Handsontable(grilla, {
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
                "Tramo",
                "R",
                "X",
                "B",
                "G",
                "R0",
                "X0",
                "B0",
                "G0"
            ],
            [
                "#",
                "ohm/km",
                "ohm/km",
                "mS/km",
                "mS/km",
                "ohm/km",
                "ohm/km",
                "mS/km",
                "mS/km",
            ]
        ],
        columns: [
            {
                data: 0,
                type: 'text'
            },
            {
                data: 1,
                type: 'text'
            },
            {
                data: 2,
                type: 'text'
            },
            {
                data: 3,
                type: 'text'
            },
            {
                data: 4,
                type: 'text'
            },
            {
                data: 5,
                type: 'text'
            },
            {
                data: 6,
                type: 'text'
            },
            {
                data: 7,
                type: 'text'
            },
            {
                data: 8,
                type: 'text'
            }
        ],
        beforeChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {

                return generalBeforeChange_4(this, changes, configuracionColumnasG7A2);
            }
        },
        afterChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
                generalAfterChange_global(this, changes, configuracionColumnasG7A2);
            }
        }
    });

    hotLineaA2.render();
}


function cargarFichaA() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetLFichaA',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            
            success: function (response) {
                console.log(response);
                var hojaaRes = response.responseResult;
                if (modoModel == "consultar") {
                    desactivarCamposFormulario('GLFichaA');
                    $('#btnGrabarG7A').hide();
                }
                if (hojaaRes.Proycodi == 0) {
                //  hojaaRes.FecPuestaServ=  obtenerFechaActualMesAnio()
                }
                setDataHojaALinea(hojaaRes);
                setDataHojaALineaDet1(hojaaRes.LineasFichaADet1DTO);
                setDataHojaALineaDet2(hojaaRes.LineasFichaADet2DTO);
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
    cambiosRealizados = false;
}


setDataHojaALinea = function (param) {
    $("#txtNombreLinea").val(param.NombreLinea);
    setFechaPickerGlobal("#txtFecPuestaServ", param.FecPuestaServ, "mm/aaaa");
    $("#txtSubInicio").val(param.SubInicio);
    $("#txtOtroSubInicio").val(param.OtroSubInicio);
    $("#txtSubFin").val(param.SubFin);
    $("#txtOtroSubFin").val(param.OtroSubFin);
    $("#txtEmpresaPropietaria").val(param.EmpPropietaria);
    $("#txtnivelTension").val(param.NivTension);
    $("#txtCapacidadCorriente").val(param.CapCorriente);
    $("#txtCapacidadCorrienteA").val(param.CapCorrienteA);
    $("#txtTiempoSobrecarga").val(param.TpoSobreCarga);
    $("#txtNumTemas").val(param.NumTemas);
    $("#txtLongitud").val(param.LongTotal);
    $("#txtLongitudVan").val(param.LongVanoPromedio);
    $("#txtTipoMateria").val(param.TipMatSop);
    $("#txtSistemProteccion").val(param.DesProtecPrincipal);
    $("#txtDescripcionSistema").val(param.DesProtecRespaldo);
    $("#txtDescripcionGeneral").val(param.DesGenProyecto);
    setupDropdownToggle('txtSubInicio', 'txtOtroSubInicio');
    setupDropdownToggle('txtSubFin', 'txtOtroSubFin');
    cargarArchivosRegistrados(seccion1LinA, '#tablaUbicacionLin');
    cargarArchivosRegistrados(seccion2LinA, '#tablaConfiguracionGeometrica');
    cargarArchivosRegistrados(seccion3LinA, '#tablaRutaGeografica');
    cargarArchivosRegistrados(seccion4LinA, '#tablaPerfilLongi');
}

function setDataHojaALineaDet1(lista1) {
    var hotInstance = hotLineaA1.getInstance(); // Obtener la instancia de Handsontable
    var data = hotInstance.getData(); // Obtener los datos actuales de la tabla

    // Iterar sobre los datos de seteo
    for (var i = 0; i < lista1.length; i++) {
        var dato = lista1[i];
            // Actualizar los valores en la fila correspondiente
            data[i][0] = dato.Tramo; // En Elaboraci�n
            data[i][1] = dato.Tipo;     // Presentado
            data[i][2] = dato.Longitud ? parseFloat(dato.Longitud).toFixed(4) : "";;    // En Tr�mite
            data[i][3] = dato.MatConductor ;      // Aprobado/Autorizado
            data[i][4] = dato.SecConductor ? parseFloat(dato.SecConductor).toFixed(4) : ""; 
            data[i][5] = dato.ConductorFase; 
            data[i][6] = dato.CapacidadTot ? parseFloat(dato.CapacidadTot).toFixed(4) : ""; 
            data[i][7] = dato.CabGuarda;
            data[i][8] = dato.ResistCabGuarda ? parseFloat(dato.ResistCabGuarda).toFixed(4) : "";
    }
    // Actualizar la tabla con los nuevos datos
    hotInstance.loadData(data);
}

function setDataHojaALineaDet2(lista2) {
    var hotInstance = hotLineaA2.getInstance(); // Obtener la instancia de Handsontable
    var data = hotInstance.getData(); // Obtener los datos actuales de la tabla

    // Iterar sobre los datos de seteo
    for (var i = 0; i < lista2.length; i++) {
        var dato = lista2[i];
        // Actualizar los valores en la fila correspondiente
        data[i][0] = dato.Tramo; // En Elaboraci�n
        data[i][1] = dato.R ? parseFloat(dato.R).toFixed(4) : "";     // Presentado
        data[i][2] = dato.X ? parseFloat(dato.X).toFixed(4) : "";    // En Tr�mite
        data[i][3] = dato.B ? parseFloat(dato.B).toFixed(4) : "";      // Aprobado/Autorizado
        data[i][4] = dato.G ? parseFloat(dato.G).toFixed(4) : "";
        data[i][5] = dato.R0 ? parseFloat(dato.R0).toFixed(4) : "";
        data[i][6] = dato.X0 ? parseFloat(dato.X0).toFixed(4) : "";
        data[i][7] = dato.B0 ? parseFloat(dato.B0).toFixed(4) : "";
        data[i][8] = dato.G0 ? parseFloat(dato.G0).toFixed(4) : "";
    }
    // Actualizar la tabla con los nuevos datos
    hotInstance.loadData(data);
}


function exportarTablaExcelLinea1() {
    var headers = [
        [
            "Tramo",
            "Tipo",
            "Longitud",
            "Material de Conductor",
            "Seccion del Conductor",
            "Conductor por Fase",
            "Capacidad Total",
            "Cables de Guarda",
            "Resistencia Cable de Guarde"
        ],
        [
            "#",
            "Aereo o Subterraneo",
            "(km)",
            "Tipo",
            "(mm2)",
            "N",
            "(A)",
            "N",
            "ohm/km"
        ]
    ];

    var datosArray = hotLineaA1.getData();
    var data = headers.concat(datosArray);

    var ws = XLSX.utils.aoa_to_sheet(data);
    aplicarBordes(ws);
    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "FichaA");

    XLSX.writeFile(wb, "G7A_06-LineasParametrosFisicos.xlsx");
}

function importarExcelLinea1() {
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

            updateTableFromExcelLinea1(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}

function updateTableFromExcelLinea1(jsonData) {
    var headers = jsonData.slice(0, 2); // Las dos primeras filas son los headers
    var data = jsonData.slice(2); // El resto son los datos

    // Validar y formatear los datos importados
    const datosValidados = validarYFormatearDatos2(data, configuracionColumnasG7A1, mostrarNotificacion, hotLineaA1);

    while (datosValidados.length < 50) {
        datosValidados.push(new Array(4).fill("")); // Agregar filas vacías
    }
    // Cargar los datos validados en la tabla, incluyendo errores
    hotLineaA1.loadData(datosValidados);

    // Verificar errores después de la validación
    const celdasConErrores = [];
    datosValidados.forEach((row, rowIndex) => {
        row.forEach((value, colIndex) => {
            const config = configuracionColumnasG7A1[colIndex];
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
                    hotLineaA1.setCellMeta(rowIndex, colIndex, "valid", false);
                    hotLineaA1.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "#ffcccc", // Rojo claro
                    });
                } else {
                    // Si es válido, limpiar estilo previo
                    hotLineaA1.setCellMeta(rowIndex, colIndex, "valid", true);
                    hotLineaA1.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "",
                    });
                }
            }
        });
    });

    hotLineaA1.render(); // Refrescar tabla

    if (celdasConErrores.length > 0) {
        mostrarNotificacion(
            `Se importaron datos con errores.<br>Errores detectados en:<br>${celdasConErrores.join("<br>")}`
        );
    } else {
        mostrarNotificacion("Los datos se importaron correctamente.", '#a3e352');
    }
}



function exportarTablaExcelLinea2() {
    var headers = [
        [
            "Tramo",
            "R",
            "X",
            "B",
            "G",
            "R0",
            "X0",
            "B0",
            "G0"
        ],
        [
            "#",
            "ohm/km",
            "ohm/km",
            "mS/km",
            "mS/km",
            "ohm/km",
            "ohm/km",
            "mS/km",
            "mS/km",
        ]
    ];

    var datosArray = hotLineaA2.getData();
    var data = headers.concat(datosArray);

    var ws = XLSX.utils.aoa_to_sheet(data);
    aplicarBordes(ws);
    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "FichaA");

    XLSX.writeFile(wb, "G7A_07-LineasParametrosElectricos.xlsx");
}

function importarExcelLinea2() {
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
            updateTableFromExcelLinea2(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}

function updateTableFromExcelLinea2(jsonData) {
    var headers = jsonData.slice(0, 2); // Las dos primeras filas son los headers
    var data = jsonData.slice(2); // El resto son los datos

    // Validar y formatear los datos importados
    const datosValidados = validarYFormatearDatos2(data, configuracionColumnasG7A2, mostrarNotificacion, hotLineaA2);

    while (datosValidados.length < 50) {
        datosValidados.push(new Array(4).fill("")); // Agregar filas vacías
    }
    // Cargar los datos validados en la tabla, incluyendo errores
    hotLineaA2.loadData(datosValidados);

    // Verificar errores después de la validación
    const celdasConErrores = [];
    datosValidados.forEach((row, rowIndex) => {
        row.forEach((value, colIndex) => {
            const config = configuracionColumnasG7A2[colIndex];
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
                    hotLineaA2.setCellMeta(rowIndex, colIndex, "valid", false);
                    hotLineaA2.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "#ffcccc", // Rojo claro
                    });
                } else {
                    // Si es válido, limpiar estilo previo
                    hotLineaA2.setCellMeta(rowIndex, colIndex, "valid", true);
                    hotLineaA2.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "",
                    });
                }
            }
        });
    });

    hotLineaA2.render(); // Refrescar tabla

    if (celdasConErrores.length > 0) {
        mostrarNotificacion(
            `Se importaron datos con errores.<br>Errores detectados en:<br>${celdasConErrores.join("<br>")}`
        );
    } else {
        mostrarNotificacion("Los datos se importaron correctamente.", '#a3e352');
    }
}


