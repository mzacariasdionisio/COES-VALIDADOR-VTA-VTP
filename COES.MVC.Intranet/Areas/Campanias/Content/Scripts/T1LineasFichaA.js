
var seccion1LinA = 22;
var seccion2LinA = 23;
var seccion3LinA = 24;
var seccion4LinA = 25;
var catInicio = 48;
var catFin = 49;
var hotLineaA1 = null;
var hotLineaA2 = null;

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
});
function grabart1FichaLineaA() {
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.t1lineasFichaADTO = getDataHojaALinea();
        param.listaT1LineasFichaADet1DTO = getDataHojaALineaDet1();
        param.listaT1LineasFichaADet2DTO = getDataHojaALineaDet2()
        console.log(param);
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarT1LineasFichaA',
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
                type: 'numeric', readOnly: true
            },
            {
                data: 1, readOnly: true
            },
            {
                data: 2,
                type: 'numeric', format: '0.0000', readOnly: true
            },
            {
                data: 3, readOnly: true
            },
            {
                data: 4,
                type: 'numeric', format: '0.0000', readOnly: true
            },
            {
                data: 5,
                type: 'numeric', readOnly: true
            },
            {
                data: 6,
                type: 'numeric', format: '0.0000', readOnly: true
            },
            {
                data: 7,
                type: 'numeric', readOnly: true
            },
            {
                data: 8,
                type: 'numeric', format: '0.0000', readOnly: true
            }
        ],
        beforeChange: function (changes, source) {
            if (!changes) return;

            changes.forEach(change => {
                const [row, col, oldValue, newValue] = change;
                const numericColumns = [0, 2, 4, 5, 6, 7, 8];
                if (numericColumns.includes(col)) {
                    generalBeforeChange(this, [[row, col, oldValue, newValue]], source);
                }
            });
        },
        afterChange: function (changes, source) {
            if (!changes) return;

            changes.forEach(change => {
                const [row, col, oldValue, newValue] = change;
                const numericColumns = [0, 2, 4, 5, 6, 7, 8];
                if (numericColumns.includes(col)) {
                    generalAfterChange(this, [[row, col, oldValue, newValue]], source);
                }
            });

        }
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
                type: 'numeric'
            },
            {
                data: 1,
                type: 'numeric', format: '0.0000'
            },
            {
                data: 2,
                type: 'numeric', format: '0.0000'
            },
            {
                data: 3,
                type: 'numeric', format: '0.0000'
            },
            {
                data: 4,
                type: 'numeric', format: '0.0000'
            },
            {
                data: 5,
                type: 'numeric', format: '0.0000'
            },
            {
                data: 6,
                type: 'numeric', format: '0.0000'
            },
            {
                data: 7,
                type: 'numeric', format: '0.0000'
            },
            {
                data: 8,
                type: 'numeric', format: '0.0000'
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

    hotLineaA2.render();
}


function cargarFichaA() {
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetT1LFichaA',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),

            success: function (response) {
                console.log(response);
                var hojaaRes = response.responseResult;
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

    if (modoModel == "consultar") {
        desactivarCamposFormulario('TLFichaA');
        $('#btnLineaA').hide();
    }
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
        data[i][2] = dato.Longitud;    // En Tr�mite
        data[i][3] = dato.MatConductor;      // Aprobado/Autorizado
        data[i][4] = dato.SecConductor;
        data[i][5] = dato.ConductorFase;
        data[i][6] = dato.CapacidadTot;
        data[i][7] = dato.CabGuarda;
        data[i][8] = dato.ResistCabGuarda;
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
        data[i][1] = dato.R;     // Presentado
        data[i][2] = dato.X;    // En Tr�mite
        data[i][3] = dato.B;      // Aprobado/Autorizado
        data[i][4] = dato.G;
        data[i][5] = dato.R0;
        data[i][6] = dato.X0;
        data[i][7] = dato.B0;
        data[i][8] = dato.G0;
    }
    // Actualizar la tabla con los nuevos datos
    hotInstance.loadData(data);
}

function exportarTablaExcelTLinea1() {
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

    XLSX.writeFile(wb, "G6A_04-LineasParametrosFisicos.xlsx");
}

function importarExcelTLinea1() {
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

            updateTableFromExcelTLinea1(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}

function updateTableFromExcelTLinea1(jsonData) {
    var headers = jsonData.slice(0, 2); // Las dos primeras filas son los headers
    var data = jsonData.slice(2); // El resto son los datos
    hotLineaA1.loadData(data);
}

function exportarTablaExcelTLinea2() {
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

    XLSX.writeFile(wb, "G6A_05-LineasParametrosElectricos.xlsx");
}

function importarExcelTLinea2() {
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

            updateTableFromExcelTLinea2(jsonData);
            cambiosRealizados = true;
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}

function updateTableFromExcelTLinea2(jsonData) {
    var headers = jsonData.slice(0, 2); // Las dos primeras filas son los headers
    var data = jsonData.slice(2); // El resto son los datos
    hotLineaA2.loadData(data);
}
