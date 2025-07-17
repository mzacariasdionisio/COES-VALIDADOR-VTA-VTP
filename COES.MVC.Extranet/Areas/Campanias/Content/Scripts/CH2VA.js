
var catElectrolizador = 25;
var catObjProyecto = 26;
var catHidrogeno = 27;
var catTransporteH2 = 28;
var catSuministro = 29;
var catSituacion = 63;
var hot1 = null;
var hot3 = null;
var hot4 = null;
var hot5Resumen = null;
var seccionUbicacionH2A = 16;
var seccionEtapa1 = 17;
var seccionEtapa2 = 18;
var seccionEtapa3 = 19;
var valoresListaFDA = [];

var seccionResumenSitProy = 41;
const configuracionColumnasH2VA = {
    0: { tipo: "anio" },
    1: { tipo: "decimal"}

};

$(function () {
    
    var ruta_interna = obtenerRutaProyecto();  

    $('#curvaTurbinaCon').hide();

    cargarDepartamentosH2VA();
    //cargarDepartamentosGeneral();
    
    crearUploaderGeneral(
        'btnSubirUbicacionH2A',
        '#tablaUbicacionH2A',
        seccionUbicacionH2A
        , null, ruta_interna
    );
    
    crearUploaderGeneral('btnSubirEtapa1', '#tablaEtapa1', seccionEtapa1, null, ruta_interna);
    crearUploaderGeneral('btnSubirEtapa2', '#tablaEtapa2', seccionEtapa2, null, ruta_interna);
    crearUploaderGeneral('btnSubirEtapa3', '#tablaEtapa3', seccionEtapa3, null, ruta_interna);
    cargarCatalogoTablaResumSitProy(seccionResumenSitProy)
    crearTablaInverEstimPeri();
    //crearTablaDiagCargHorario(30, 1440, 30); // Valores en minutos

    var formularioa = document.getElementById('H2VA');
    formularioa.addEventListener('change', function () {
        cambiosRealizados = true;
        console.log(cambiosRealizados);
    });



});

function getCuestionarioH2VA() {
    var param = {};
    param.H2vaCodi = $("#txtH2VACodi").val();
    param.ProyCodi = $("#txtProyCodi").val();
    param.ProyNp = $('input[name="rbProy"]:checked').val();
    param.SocioOperador = $("#txtSocioOperador").val();
    param.SocioInversionista = $("#txtSocioInversionista").val();
    param.Distrito = $("#distritosSelect").val();
    param.Actdesarrollar = $("#txtActDesarrollar").val();
    param.Situacionact = $("#situacionActSelect").val();
    param.TipoElectrolizador = $("#tipElectrolizadorSelect").val();
    param.Otroelectrolizador = $("#txtOtroElectrolizador").val();
    param.VidaUtil = $("#txtVidaUtil").val();
    param.ProduccionAnual = $("#txtProduccionAnual").val();
    param.Objetivoproyecto = $("#objProyectoSelect").val();

    param.Otroobjetivo = $("#txtOtroObjetivo").val();
    param.Usoesperadohidro = $("#hidroSelect").val();
    param.Otrousoesperadohidro = $("#txtOtroUsoEsperadoHidro").val();
    param.Metodotransh2 = $("#metodoTransH2Select").val();
    param.Otrometodotransh2 = $("#txtOtroMetodoTransH2").val();
    param.Podercalorifico = $("#txtPoderCalorifico").val();
    param.Subestacionsein = $("#txtSubestacionSein").val();
    param.Niveltension = $("#txtNivelTension").val();
    param.Tiposuministro = $("#tipSuministroSelect").val();
    param.Otrosuministro = $("#txtOtroSuministro").val();
    param.Primeraetapa = $("#txtPrimeraEtapaAnio").val();
    param.Segundaetapa = $("#txtSegundaEtapaAnio").val();
    param.Final = $("#txtFinalAnio").val();
    param.Costoproduccion = $("#txtCostoProduccion").val();
    param.Precioventa = $("#txtPrecioVenta").val();
    param.Financiamiento = $("#txtFinanciamiento").val();
    param.Factfavorecenproy = $("#txtFactFavorecenProy").val();
    param.Factdesfavorecenproy = $("#txtFactDesfavorecenProy").val();
    param.Comentarios = $("#txtComentarios").val();
    return param;
}

function setCuestionarioH2VA(param) {
    $("#txtH2VACodi").val(param.H2vaCodi);
    $("#txtProyCodi").val(param.ProyCodi);
    $(`#rbProy${param.ProyNp}`).prop("checked", true);
    $("#txtSocioOperador").val(param.SocioOperador);
    $("#txtSocioInversionista").val(param.SocioInversionista);  
    cargarUbicacionH2VA(param.Distrito);
    $("#txtActDesarrollar").val(param.ActDesarrollar);
    $("#situacionActSelect").val(param.SituacionAct);
    $("#tipElectrolizadorSelect").val(param.TipoElectrolizador);
    $("#txtOtroElectrolizador").val(param.OtroElectrolizador);
    $("#txtVidaUtil").val(param.VidaUtil);
    $("#txtProduccionAnual").val(param.ProduccionAnual);
    $("#objProyectoSelect").val(param.ObjetivoProyecto);
    $("#txtOtroObjetivo").val(param.OtroObjetivo);
    $("#hidroSelect").val(param.UsoEsperadoHidro);
    $("#txtOtroUsoEsperadoHidro").val(param.OtroUsoEsperadoHidro);
    $("#metodoTransH2Select").val(param.MetodoTransH2);
    $("#txtOtroMetodoTransH2").val(param.OtroMetodoTransH2);
    $("#txtPoderCalorifico").val(param.PoderCalorifico);
    $("#txtSubestacionSein").val(param.SubEstacionSein);
    $("#txtNivelTension").val(param.NivelTension);
    $("#tipSuministroSelect").val(param.TipoSuministro);
    $("#txtOtroSuministro").val(param.OtroSuministro);
    $("#txtPrimeraEtapaAnio").val(param.PrimeraEtapa);
    $("#txtSegundaEtapaAnio").val(param.SegundaEtapa);
    $("#txtFinalAnio").val(param.Final);
    $("#txtCostoProduccion").val(param.CostoProduccion);
    $("#txtPrecioVenta").val(param.PrecioVenta);
    $("#txtFinanciamiento").val(param.Financiamiento);
    $("#txtFactFavorecenProy").val(param.FactFavorecenProy);
    $("#txtFactDesfavorecenProy").val(param.FactDesfavorecenProy);
    $("#txtComentarios").val(param.Comentarios);

    cargarArchivosRegistrados(seccionUbicacionH2A, '#tablaUbicacionH2A');
    cargarArchivosRegistrados(seccionEtapa1, '#tablaEtapa1');
    cargarArchivosRegistrados(seccionEtapa2, '#tablaEtapa2');
    cargarArchivosRegistrados(seccionEtapa3, '#tablaEtapa3');

    if (modoModel == "consultar") {
        desactivarCamposFormulario('H2VA');
        $("#btnGrabarH2VA").hide();
    }

}

function grabarCuestionarioH2VA() {
    if (!validarTablaAntesDeGuardar(hot1)) {
        mostrarNotificacion("No se puede guardar porque hay datos inválidos en la tabla.");
        return; // Detener el guardado si hay errores
    }
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        param.ProyCodi = idProyecto;
        param.CuestionarioH2VADTO = getCuestionarioH2VA();
        param.CuestionarioH2VADTO.ListCH2VADet1DTOs = getDatosDetA1();
        param.CuestionarioH2VADTO.ListCH2VADet2DTOs = getDatosDetA2();
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarCuestionarioH2VA',
            data: param,
            
            success: function (result) {
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

function cargarCatalogoTablaResumSitProy(id) {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListarCatalogo',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: id }),
            success: function (eData) {
                valoresListaFDA = eData;
                crearTablaResumSitProy();
                resolve(eData); // Resuelve la promesa
            },
            error: function (err) {
                alert("Ha ocurrido un error");
                reject(err); // Rechaza la promesa
            }
        });
    });
}

function crearTablaResumSitProy() {
    var data = [];

    // Generar la data basada en valoresListaFDA
    for (var i = 0; i < valoresListaFDA.length; i++) {
        var row = [];
        row.push(valoresListaFDA[i].DataCatCodi);
        row.push(valoresListaFDA[i].DesDataCat);
        for (var j = 0; j < 5; j++) {
            row.push(false);
        }
        data.push(row);
    }

    var grilla = document.getElementById('tableResumSitProy');

    // Verificar si hot5Resumen ya está inicializado
    if (typeof hot5Resumen !== 'undefined' && hot5Resumen !== null) {
        // Si ya está inicializado, solo actualiza los datos
        hot5Resumen.loadData(data);
    } else {
        // Si no está inicializado, crea una nueva instancia
        hot5Resumen = new Handsontable(grilla, {
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
                    { label: '', colspan: 2 },
                    { label: 'Estado Situacional', colspan: 5 }
                ],
                [
                    { label: '', colspan: 1 },
                    { label: 'Requisitos', colspan: 1 },
                    { label: 'En Elaboración', colspan: 1 },
                    { label: 'Presentado', colspan: 1 },
                    { label: 'En Trámite (evaluación)', colspan: 1 },
                    { label: 'Aprobado/ autorizado', colspan: 1 },
                    { label: 'Firmado', colspan: 1 }
                ]
            ],
            columns: [
                { readOnly: true },
                { readOnly: true },
                { type: "checkbox" },
                { type: "checkbox" },
                { type: "checkbox" },
                { type: "checkbox" },
                { type: "checkbox" },
            ],
            hiddenColumns: {
                columns: [0],
                indicators: true,
            },
            autoWrapRow: true,
            autoWrapCol: true,
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

        hot5Resumen.render();
    }
}

function crearTablaBalanceOD(inicioan, finan) {

    var data = [];
    var meses = ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Setiembre', 'Octubre', 'Noviembre', 'Diciembre'];
    for (var year = inicioan; year <= finan; year++) {
        meses.forEach(function (str) {
            var row = [year];
            row.push(str);
            for (var i = 0; i < 6; i++) {
                row.push('');
            }
            data.push(row);
        });
    }
    var grilla = document.getElementById('tableBalanceOD');

    if (typeof hot3 !== 'undefined' && hot3 !== null) {
        hot3.destroy();
    }

    hot3 = new Handsontable(grilla, {
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
                { label: '', colspan: 1 },
                { label: '', colspan: 1 },
                { label: 'DEMANDA ESTIM. PROY. DE PROD. HIDROG.', colspan: 3 },
                { label: 'GENERA. ESTIM. DE PROY. DE GENE. ASOC.', colspan: 3 }
            ],
            [
                { label: '', colspan: 1 },
                { label: '', colspan: 1 },
                { label: '', colspan: 1 },
                { label: 'POTENCIA(MW)', colspan: 2 },
                { label: '', colspan: 1 },
                { label: 'POTENCIA(MW)', colspan: 2 },
            ],
            [
                { label: 'A&ntilde;o', colspan: 1 },
                { label: 'Mes', colspan: 1 },
                { label: 'ENERGIA(GWh)', colspan: 1 },
                { label: 'HP', colspan: 1 },
                { label: 'HFP', colspan: 1 },
                { label: 'ENERGIA(GWh)', colspan: 1 },
                { label: 'HP', colspan: 1 },
                { label: 'HFP', colspan: 1 }
            ]
        ]
    });

    hot3.render();
}

function crearTablaDiagCargHorario(iniciot, fint, increm) {

    var data = [];
    for (var time = iniciot; time <= fint; time += increm) {
        var hora = Math.floor(time / 60).toString().padStart(2, '0');
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
        ]
    });

    hot4.render();
}
function crearTablaInverEstimPeri() {

    var data = [];
    for (var year = 0; year <= 20; year++) {
        var row = [];
        row.push('');;
        row.push('');
        data.push(row);
    }

    var grilla = document.getElementById('tableInverEstimPeri');

    if (typeof hot1 !== 'undefined' && hot1 !== null) {
        hot1.destroy();
    }

    hot1 = new Handsontable(grilla, {
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
                { label: 'Periodo (A&ntilde;os)', colspan: 1 },
                { label: 'Monto de inversión (US$)', colspan: 1 },
            ]
        ],
        // Definición de las columnas y validadores
        columns: [
            {
                data: 0,
                type: 'text', 
            },
            {
                data: 1,
                type: 'text'
            }
        ],
        beforeChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
                return generalBeforeChange_4(this, changes, configuracionColumnasH2VA);
            }
        },
        afterChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
                generalAfterChange_global(this, changes, configuracionColumnasH2VA);
            }
        }

    });
    hot1.render();
}


function getDatosDetA1() {
    var datosArray = hot1.getData();
    var datosObjetos = datosArray.map(function (row) {
        return {
            Anio: row[0], // ANIO
            MontoInversion: row[1] // MONTO INVERSIO
        };
    }).filter(function (row) {
        // Verificar si al menos una columna tiene un valor no vacío
        return Object.values(row).some(function (value) {
            return value !== "";
        });
    });
    // Ordenar los datos antes de enviarlos
    datosObjetos.sort((a, b) => a.Anio - b.Anio);

    return datosObjetos;
}

function setDatosDet1(listaDet1) {
    var datosArray = [];

    listaDet1.map(function (item) {
        var row = [
            item.Anio,
            item.MontoInversion
        ];
        datosArray.push(row);
    });
 
    // Ordenar por Anio en orden ascendente
    datosArray.sort((a, b) => a[0] - b[0]);

    // Completa con filas vacías hasta llegar a 20
    while (datosArray.length < 20) {
        var emptyRow = new Array(2).fill("");
        datosArray.push(emptyRow);
    }

    hot1.loadData(datosArray);
}

function getDatosDetA2() {
    var hotInstance = hot5Resumen.getInstance(); // Obtener la instancia de Handsontable
    var data = hotInstance.getData(); // Obtener los datos actuales de la tabla
    var valoresMarcados = [];
    
    // Recorrer las filas de la tabla
    for (var i = 0; i < data.length; i++) {
        var fila = data[i];
        var filaMarcada = {};
    
      
        // Recoger el estado de los checkboxes
        filaMarcada = {
            DataCatCodi: fila[0],
            EnElaboracion: fila[2],
            Presentado: fila[3],
            EnTramite: fila[4],
            Aprobado: fila[5],
            Firmado: fila[6],
        };
    
        // Solo agregar la fila si al menos un checkbox está marcado
        if (Object.values(filaMarcada).some(Boolean)) {
            valoresMarcados.push(filaMarcada);
        }
    }
    console.log(valoresMarcados);
    return valoresMarcados;
}

function setDataDetA2(datosSeteo) {
    if (!hot5Resumen) {
        crearTablaResumSitProy();
    }
    var hotInstance = hot5Resumen.getInstance(); // Obtener la instancia de Handsontable
    var data = hotInstance.getData(); // Obtener los datos actuales de la tabla
    
    // Iterar sobre los datos de seteo
    if(datosSeteo){
        for (var i = 0; i < datosSeteo.length; i++) {
            var dato = datosSeteo[i];
            var dataCatCodi = dato.DataCatCodi; // Convertir a string para comparación
            var enElaboracion = dato.EnElaboracion;
            var presentado = dato.Presentado;
            var enTramite = dato.EnTramite;
            var aprobado = dato.Aprobado;
            var firmado = dato.Firmado;
            // Buscar el índice de la fila correspondiente
            var rowIndex = data.findIndex(row => row[0] == dataCatCodi);
        
            if (rowIndex !== -1) {
                // Actualizar los valores en la fila correspondiente
                data[rowIndex][2] = enElaboracion; // En Elaboración
                data[rowIndex][3] = presentado;     // Presentado
                data[rowIndex][4] = enTramite;      // En Trámite
                data[rowIndex][5] = aprobado;       // Aprobado/Autorizado
                data[rowIndex][6] = firmado;        // Firmado
            }
        }
    }
    // Actualizar la tabla con los nuevos datos
    hotInstance.loadData(data);
}

function setDatosDet2(listaDet2) {
    var datosArray = [];
    listaDet2.map(function (item) {
        var row = [
            item.Anio,
            item.MontoInversion
        ];
        datosArray.push(row);
    });

    hot1.loadData(datosArray);
}


async function cargarCatalogosYAsignarValoresAGH2VA(param) {
    console.log("Verificando y cargando catálogos...");

    let promesas = [
        esperarCatalogo(catElectrolizador, "#tipElectrolizadorSelect"),
        esperarCatalogo(catObjProyecto, "#objProyectoSelect"),
        esperarCatalogo(catHidrogeno, "#hidroSelect"),
        esperarCatalogo(catTransporteH2, "#metodoTransH2Select"),
        esperarCatalogo(catSuministro, "#tipSuministroSelect"),
        esperarCatalogo(catSituacion, "#situacionActSelect")
      
    ];

    // Asegurar que todas las promesas de carga se completen antes de continuar
    await Promise.all(promesas);

    console.log("Todos los catálogos han sido cargados. Asignando valores...");


    asignarSiExisteGlobal("#tipElectrolizadorSelect", param.TipoElectrolizador);
    asignarSiExisteGlobal("#objProyectoSelect", param.Objetivoproyecto);
    asignarSiExisteGlobal("#hidroSelect", param.Usoesperadohidro);
    asignarSiExisteGlobal("#metodoTransH2Select", param.metodoTransH2Select);
    asignarSiExisteGlobal("#tipSuministroSelect", param.Tiposuministro);
    asignarSiExisteGlobal("#situacionActSelect", param.SituacionAct);
 

    console.log("Valores asignados correctamente.");
}



async function cargarDatosHojaA() {

    limpiarCombos('#tipElectrolizadorSelect,#objProyectoSelect,#hidroSelect,#metodoTransH2Select,#tipSuministroSelect,#situacionActSelect');

    await cargarCatalogosYAsignarValoresAGH2VA({});
    console.log("Editar");
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        cargarCatalogoTablaResumSitProy(seccionResumenSitProy)
            .then(() => {
                $.ajax({
                    type: 'POST',
                    url: controladorFichas + 'GetCuestionarioH2A',
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ id: idProyecto }),
                    success: function (response) {
                        console.log(response);
                        var hojaacues = response.responseResult;
                        setCuestionarioH2VA(hojaacues);
                        setDatosDet1(hojaacues.ListCH2VADet1DTOs);
                        setDataDetA2(hojaacues.ListCH2VADet2DTOs);
                        setupDropdownToggle('tipElectrolizadorSelect', 'txtOtroElectrolizador');
                        setupDropdownToggle('objProyectoSelect', 'txtOtroObjetivo');
                        setupDropdownToggle('hidroSelect', 'txtOtroUsoEsperadoHidro');
                        setupDropdownToggle('metodoTransH2Select', 'txtOtroMetodoTransH2');
                        setupDropdownToggle('tipSuministroSelect', 'txtOtroSuministro');
                        cambiosRealizados = false;
                    },
                    error: function () {
                        mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                    }
                });
            })
            .catch((err) => {
                console.error("Error cargando el catálogo de la tabla resumen:", err);
            });
    }
}



function exportH2VAInv() {
    var headers = [["Periodo(Años)", "Monto Inversión(US$)"]];
    exportExcelGInvEstimada(hot1, headers, "HVA_01-InversionEstimada.xlsx");
}
function importH2VAInv() {
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

            updateTableFromExcelH2VA(jsonData);
        };

        reader.readAsArrayBuffer(file);
    };

    fileInput.click();
}

function updateTableFromExcelH2VA(jsonData) {
    var headers = jsonData.slice(0, 2);
    var data = jsonData.slice(2);

    const datosValidados = validarYFormatearDatos2(data, configuracionColumnasH2VA, mostrarNotificacion, hot1);

    hot1.loadData(datosValidados);


    // Verificar errores después de la validación
    const celdasConErrores = [];
    datosValidados.forEach((row, rowIndex) => {
        row.forEach((value, colIndex) => {
            const config = configuracionColumnasH2VA[colIndex];
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
                } else if (config.tipo === "anio") {
        
                    if (!/^\d{4}$/.test(value)) {
                        esValido = false;
                    }
                }

                if (!esValido) {
                    celdasConErrores.push(`Fila ${rowIndex + 1}, Columna ${colIndex + 1}`);
                    hot1.setCellMeta(rowIndex, colIndex, "valid", false);
                    hot1.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "#ffcccc", // Rojo claro
                    });
                } else {
                    // Si es válido, limpiar estilo previo
                    hot1.setCellMeta(rowIndex, colIndex, "valid", true);
                    hot1.setCellMeta(rowIndex, colIndex, "style", {
                        backgroundColor: "",
                    });
                }
            }
        });
    });
 
    
    hot1.render();
}

function validarYFormatearDatos2(data, configuracionColumnas, mostrarNotificacion, hotInstance) {
    const tieneDatos = data.some(row => row.some(value => value !== ""));
    let celdasConErrores = [];
    let totalErrores = 0;

    if (!tieneDatos) {
        console.log("Los datos están vacíos, no se aplica validación ni formateo.");
        return data;
    }

    data.forEach((row, rowIndex) => {
        row.forEach((value, colIndex) => {
            const config = configuracionColumnas[colIndex];
            if (config && value !== "" && value !== null && value !== undefined) {
                let prev = String(value);
                if (config.tipo === "decimal" || config.tipo === "decimaltruncal" || config.tipo === "entero") {
                    prev = prev.replace(/,/g, "");
                }
                let parsedValue = parseFloat(prev);
                const stringValue = value.toString();
                let esValido = true;

                if (config.tipo === "texto" && stringValue.length > config.largo) {
                    esValido = false;
                    totalErrores++;
                } else if (isNaN(parsedValue)) {
                    esValido = false;
                    totalErrores++;
                } else if (!config.permitirNegativo && parsedValue < 0) {
                    esValido = false;
                    totalErrores++;
                } else if (config.tipo === "decimal") {
                    parsedValue = parsedValue.toFixed(config.decimales);
                    row[colIndex] = parsedValue;
                } else if (config.tipo === "decimaltruncal") {
                    const factor = Math.pow(10, config.decimales);
                    parsedValue = Math.floor(parsedValue * factor) / factor;
                    row[colIndex] = parsedValue;
                } else if (config.tipo === "entero" && (!/^-?\d+$/.test(stringValue) || !Number.isInteger(parsedValue))) {
                    esValido = false;
                    totalErrores++;
                } else if (config.tipo === "especial" && ![17, 18, 19].includes(parsedValue)) {
                    esValido = false;
                    totalErrores++;
                }

                if (!esValido) {
                    celdasConErrores.push(`Fila ${rowIndex + 1}, Columna ${colIndex + 1}`);
                    hotInstance.setCellMeta(rowIndex, colIndex, "valid", false);
                    hotInstance.setCellMeta(rowIndex, colIndex, "style", { backgroundColor: "#ffcccc" });
                } else {
                    hotInstance.setCellMeta(rowIndex, colIndex, "valid", true);
                    hotInstance.setCellMeta(rowIndex, colIndex, "style", { backgroundColor: "" });
                }
            }
        });
    });

    if (totalErrores > 0) {
        mostrarNotificacion(`Se importaron datos con errores.\nErrores detectados en:\n${celdasConErrores.slice(0, 10).join("\n")}`);
    }

    hotInstance.render();
    return data;
}


function exportExcelCH2VASitu() {
    exportarTablaResumenSitProy(hot5Resumen,"H2VA_02-ResumenSituacion","H2VA");
}
function importExcelCH2VASitu() {
    importarTablaResumenSitProy(hot5Resumen);
}

function desactivarCamH2VA() {
    var formulario = document.getElementById('H2VA');
    var elementos = formulario.querySelectorAll('input, button, textarea, select');

    elementos.forEach(function (elemento) {
        if (elemento.tagName === 'INPUT') {
            if (elemento.type === 'text' || elemento.type === 'email' || elemento.type === 'password' || elemento.type === 'number') {
                elemento.readOnly = true;
            } else {
                elemento.disabled = true;
            }
        } else if (elemento.tagName === 'TEXTAREA' || elemento.tagName === 'SELECT' || elemento.tagName === 'BUTTON') {
            elemento.disabled = true;
        } else if (elemento.tagName === 'A') {
            elemento.addEventListener('click', function (event) {
                event.preventDefault();
            });
            elemento.style.pointerEvents = 'none'; // Esto desactiva el click visualmente
            elemento.style.color = 'gray'; // Opcional: cambiar el color para mostrar que está desactivado
        }
    });
}

function cargarDepartamentosH2VA() {
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'ListarDepartamentos',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(''),
        success: function (eData) {
            cargarListaDepartamentoH2VA(eData);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
function cargarListaDepartamentoH2VA(departamentos) {
    var selectDepartamentos = $('#departamentosSelect');
    $.each(departamentos, function (index, departamento) {
        var option = $('<option>', {
            value: departamento.Id,
            text: departamento.Nombre
        });
        selectDepartamentos.append(option);
    });
    selectDepartamentos.change(function () {
        var idSeleccionado = $(this).val();
        cargarProvinciaH2VA(idSeleccionado);
    });

}
function cargarProvinciaH2VA(id, callback) {
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'ListarProvincias',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (eData) {
            cargarListaProvinciaH2VA(eData);
            if (typeof callback === 'function') {
                callback();
            }

        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
function cargarListaProvinciaH2VA(provincias) {
    var selectProvincias = $('#provinciasSelect');
    selectProvincias.empty();
    var optionDefault = $('<option>', {
        value: "",
        text: "Seleccione una provincia"
    });
    selectProvincias.append(optionDefault);
    cargarListaDistritosH2VA([]);
    $.each(provincias, function (index, provincia) {
        var option = $('<option>', {
            value: provincia.Id,
            text: provincia.Nombre
        });
        selectProvincias.append(option);
    });

    selectProvincias.change(function () {
        var idSeleccionado = $(this).val();
        cargarDistritoH2VA(idSeleccionado);
    });

}
function cargarDistritoH2VA(id, callback) {
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'ListarDistritos',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (eData) {
            console.log(eData);
            cargarListaDistritosH2VA(eData);
            if (typeof callback === 'function') {
                callback();
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
function cargarListaDistritosH2VA(distritos) {
    var selectDistritos = $('#distritosSelect');
    selectDistritos.empty();
    var optionDefault = $('<option>', {
        value: "",
        text: "Seleccione un distrito"
    });
    selectDistritos.append(optionDefault);
    $.each(distritos, function (index, distrito) {
        var option = $('<option>', {
            value: distrito.Id,
            text: distrito.Nombre
        });
        selectDistritos.append(option);
    });

}
function cargarUbicacionH2VA(idDistrito) {
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'UbicacionByDistro',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: idDistrito }),
        success: function (ubicacion) {
            $('#departamentosSelect').val(ubicacion.DepartamentoId);

            cargarProvinciaH2VA(ubicacion.DepartamentoId, function () {
                $('#provinciasSelect').val(ubicacion.ProvinciaId);
            
                cargarDistritoH2VA(ubicacion.ProvinciaId, function () {
                    $('#distritosSelect').val(ubicacion.DistritoId);
                    
                });
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

