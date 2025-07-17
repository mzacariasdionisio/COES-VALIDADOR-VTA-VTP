var hotPeMedio = null;
var hot2PeOptimista = null;
var hot3PePesimista = null;
var hot4InversEstim = null; 
var hot5Resumen = null;
var seccionUbicacionD11 = 27;
var seccionD12 = 28;
var seccionD13 = 29;
var seccionD14 = 30;
var catTipoCarga = 44;
var catSubSein = 45;
var catSubEmpElectro = 46;
var catvaloresListaFDA = 47;
var valoresListaFDA = [];

$(function () {
    //$('#txtFechaIngreso').val(obtenerFechaActualMesAnio());

    $('#txtFechaIngreso').Zebra_DatePicker({
        format: 'm/Y',
        onSelect: function () {
            cambiosRealizados = true;
        }
    });

    cargarDepartamentos();
    crearTablaPEMedio(2011, horizonteFin);
    crearTablaPEOptimista(2011, horizonteFin);
    crearTablaPEPesimista(2011, horizonteFin);
    crearTableInversEstim(2018, horizonteFin);
    cargarCatalogoTablAFDemanda1(catvaloresListaFDA);

    var ruta_interna = obtenerRutaProyecto();  
    crearUploaderGeneral('btnSubirUbicacionD11', '#tablaUbicacionD11', seccionUbicacionD11, null, ruta_interna);
    crearUploaderGeneral('btnSubirD12', '#tablaD12', seccionD12, null, ruta_interna);
    crearUploaderGeneral('btnSubirD13', '#tablaD13', seccionD13, null, ruta_interna);
    crearUploaderGeneral('btnSubirD14', '#tablaD14', seccionD14, null, ruta_interna);

});



const configuracionColumnasd1a = {
 
};
for (let i = 1; i <= 10; i++) {
    configuracionColumnasd1a[i] = { tipo: "decimal", decimales: 4, permitirNegativo: false };
}


function crearTablaPEMedio(inicioan, finan) {
    console.log("Entro excel");

    var data = [];
    for (var year = inicioan; year <= finan; year++) {
        var row = [year];
        for (var i = 0; i < 7; i++) {
            row.push('');
        }
        data.push(row);
    }

    var grilla = document.getElementById('tablePEMedio');

    if (typeof hotPeMedio !== 'undefined' && hotPeMedio !== null) {
        hotPeMedio.destroy();
    }

    hotPeMedio = new Handsontable(grilla, {
        data: data,
        // rowHeaders: true,
        colHeaders: true,
        colWidths: [100, 150, 100, 100, 120, 150, 100, 100, 150, 100, 100],
        manualColumnResize: true,
        autoColumnSize: true,
        autoRowSize: true,
        columnSorting: false,
        filters: false,
        contextMenu: true,
        nestedHeaders: [
            [
                { label: '', colspan: 1 },
                { label: 'Demanda', colspan: 4 },
                { label: 'Generaci&oacute;n', colspan: 3 },
                { label: 'Demanda Neta = Demanda - Generaci&oacute;n', colspan: 3 }
            ],
            [
                { label: '', colspan: 1 },
                { label: '', colspan: 1 },
                { label: 'POTENCIA(MW)', colspan: 2 },
                { label: '', colspan: 1 },
                { label: '', colspan: 1 },
                { label: 'POTENCIA(MW)', colspan: 2 },
                { label: '', colspan: 1 },
                { label: 'POTENCIA(MW)', colspan: 2 },
            ],
            [
                { label: 'Año', colspan: 1 },
                { label: 'ENERGIA(GWh)', colspan: 1 },
                { label: 'HP', colspan: 1 },
                { label: 'HFP', colspan: 1 },
                { label: 'FACTOR DE<br>CARGA (%)', colspan: 1 },
                { label: 'ENERGIA(GWh)', colspan: 1 },
                { label: 'HP', colspan: 1 },
                { label: 'HFP', colspan: 1 },
                { label: 'ENERGIA(GWh)', colspan: 1 },
                { label: 'HP', colspan: 1 },
                { label: 'HFP', colspan: 1 }
            ]
        ],
        columns: [
            { data: 0, readOnly:true , renderer: FormatDisabledRenderer},
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
                type: 'text',
                readOnly: true, 
                renderer: FormatDisabledRenderer
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
                readOnly: true,
                type: 'text',
                renderer: EnergiaColumnRenderer
            }, 
            {
                data: 9,
                type: 'text'
            },
            {
                data: 10,
                type: 'text'
            }
        ],
        allowInvalid: true,
        beforeChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {
              
                return generalBeforeChange_4(this, changes, configuracionColumnasd1a);
            }
        }
        ,
        afterChange: function (changes, source) {
            generalAfterChange_2(this, changes, source);
        }
    });

    hotPeMedio.render();
}


function setDataDFormatoDet1(dataObjects) {
    if (typeof hotPeMedio === 'undefined' || hotPeMedio === null) {
        console.error("La tabla Handsontable no está inicializada.");
        return;
    }

    var currentData = hotPeMedio.getData();

    dataObjects.forEach(function (obj) {
        var rowIndex = currentData.findIndex(row => row[0] === obj.Anio);
        if (rowIndex !== -1) {
            currentData[rowIndex][1] = obj.DemandaEnergia ?? ""; // Energía
            currentData[rowIndex][2] = obj.DemandaHP ?? ""; // HP
            currentData[rowIndex][3] = obj.DemandaHFP ?? ""; // HFP
            currentData[rowIndex][4] = obj.DemandaCarga ?? ""; // Energía Generación
            currentData[rowIndex][5] = obj.GeneracionEnergia ?? ""; // HP Generación
            currentData[rowIndex][6] = obj.GeneracionHP ?? ""; // HFP Generación
            currentData[rowIndex][7] = obj.GeneracionHFP ?? ""; // HFP Generación
            currentData[rowIndex][9] = obj.DemandaNetaHP ?? ""; // HFP Generación
            currentData[rowIndex][10] = obj.DemandaNetaHFP ?? ""; // HFP Generación
        }
    });

    hotPeMedio.loadData(currentData);
    hotPeMedio.render();
}

function EnergiaColumnRenderer(instance, td, row) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.background = '#fafafa';
    let total = 0;
    let energiaDemanda = parseFloat(instance.getDataAtCell(row, 1)) || 0;
    let energiaGeneracion = parseFloat(instance.getDataAtCell(row, 5)) || 0;
    total = energiaDemanda - energiaGeneracion;
    td.innerHTML = total.toFixed(4);
}

function crearTablaPEOptimista(inicioan, finan) {
    console.log("Entro excel");

    var data = [];
    for (var year = inicioan; year <= finan; year++) {
        var row = [year];
        for (var i = 0; i < 7; i++) {
            row.push('');
        }
        data.push(row);
    }

    var grilla = document.getElementById('tablePEOptimista');

    if (typeof hot2PeOptimista !== 'undefined' && hot2PeOptimista !== null) {
        hot2PeOptimista.destroy();
    }

    hot2PeOptimista = new Handsontable(grilla, {
        data: data,
        // rowHeaders: true,
        colHeaders: true,
        colWidths: [100, 150, 100, 100, 120, 150, 100, 100, 150, 100, 100],
        manualColumnResize: true,
        autoColumnSize: true,
        autoRowSize: true,
        columnSorting: false,
        filters: false,
        contextMenu: true,
        nestedHeaders: [
            [
                { label: '', colspan: 1 },
                { label: 'Demanda', colspan: 4 },
                { label: 'Generaci&oacute;n', colspan: 3 },
                { label: 'Demanda Neta = Demanda - Generaci&oacute;n', colspan: 3 }
            ],
            [
                { label: '', colspan: 1 },
                { label: '', colspan: 1 },
                { label: 'POTENCIA(MW)', colspan: 2 },
                { label: '', colspan: 1 },
                { label: '', colspan: 1 },
                { label: 'POTENCIA(MW)', colspan: 2 },
                { label: '', colspan: 1 },
                { label: 'POTENCIA(MW)', colspan: 2 },
            ],
            [
                { label: 'A&ntilde;o', colspan: 1 },
                { label: 'ENERGIA(GWh)', colspan: 1 },
                { label: 'HP', colspan: 1 },
                { label: 'HFP', colspan: 1 },
                { label: 'FACTOR DE </br> CARGA (%)', colspan: 1 },
                { label: 'ENERGIA(GWh)', colspan: 1 },
                { label: 'HP', colspan: 1 },
                { label: 'HFP', colspan: 1 },
                { label: 'ENERGIA(GWh)', colspan: 1 },
                { label: 'HP', colspan: 1 },
                { label: 'HFP', colspan: 1 }
            ]
        ],
        columns: [
            { data: 0, readOnly:true, renderer: FormatDisabledRenderer},
            {
                data: 1,
                type:'text'
            },
            {
                data: 2,
                type:'text'
            },
            {
                data: 3,
                type:'text'
            },
            {
                data: 4,
                type:'text',
                readOnly: true,
                renderer: FormatDisabledRenderer
            },
            {
                data: 5,
                type:'text'
            },
            {
                data: 6,
                type:'text'
            },
            {
                data: 7,
                type:'text'
            },
            {
                data: 8,
                readOnly: true,
                type:'text',
                renderer: EnergiaColumnRenderer
            }, 
            {
                data: 9,
                type:'text'
            },
            {
                data: 10,
                type:'text'
            }
        ],
        beforeChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {

                return generalBeforeChange_4(this, changes, configuracionColumnasd1a);
            }
        }
        ,
        afterChange: function (changes, source) {
            generalAfterChange_2(this, changes, source);
        }
    });

    hot2PeOptimista.render();
}
function crearTablaPEPesimista(inicioan, finan) {

    var data = [];
    for (var year = inicioan; year <= finan; year++) {
        var row = [year];
        for (var i = 0; i < 7; i++) {
            row.push('');
        }
        data.push(row);
    }

    var grilla = document.getElementById('tablePEPesimista');

    if (typeof hot3PePesimista !== 'undefined' && hot3PePesimista !== null) {
        hot3PePesimista.destroy();
    }

    hot3PePesimista = new Handsontable(grilla, {
        data: data,
        // rowHeaders: true,
        colHeaders: true,
        colWidths: [100, 150, 100, 100, 120, 150, 100, 100, 150, 100, 100],
        manualColumnResize: true,
        autoColumnSize: true,
        autoRowSize: true,
        columnSorting: false,
        filters: false,
        contextMenu: true,
        nestedHeaders: [
            [
                { label: '', colspan: 1 },
                { label: 'Demanda', colspan: 4 },
                { label: 'Generaci&oacute;n', colspan: 3 },
                { label: 'Demanda Neta = Demanda - Generaci&oacute;n', colspan: 3 }
            ],
            [
                { label: '', colspan: 1 },
                { label: '', colspan: 1 },
                { label: 'POTENCIA(MW)', colspan: 2 },
                { label: '', colspan: 1 },
                { label: '', colspan: 1 },
                { label: 'POTENCIA(MW)', colspan: 2 },
                { label: '', colspan: 1 },
                { label: 'POTENCIA(MW)', colspan: 2 },
            ],
            [
                { label: 'A&ntilde;o', colspan: 1 },
                { label: 'ENERGIA(GWh)', colspan: 1 },
                { label: 'HP', colspan: 1 },
                { label: 'HFP', colspan: 1 },
                { label: 'FACTOR DE </br> CARGA (%)', colspan: 1 },
                { label: 'ENERGIA(GWh)', colspan: 1 },
                { label: 'HP', colspan: 1 },
                { label: 'HFP', colspan: 1 },
                { label: 'ENERGIA(GWh)', colspan: 1 },
                { label: 'HP', colspan: 1 },
                { label: 'HFP', colspan: 1 }
            ]
        ],
        columns: [
            { data: 0, readOnly:true, renderer: FormatDisabledRenderer },
            {
                data: 1,
                type:'text'
            },
            {
                data: 2,
                type:'text'
            },
            {
                data: 3,
                type:'text'
            },
            {
                data: 4,
                type:'text',
                readOnly: true, 
                renderer: FormatDisabledRenderer
            },
            {
                data: 5,
                type:'text'
            },
            {
                data: 6,
                type:'text'
            },
            {
                data: 7,
                type:'text'
            },
            {
                data: 8,
                readOnly: true,
                type:'text',
                renderer: EnergiaColumnRenderer
            }, 
            {
                data: 9,
                type:'text'
            },
            {
                data: 10,
                type: 'text'
            }
        ],
        beforeChange: function (changes, source) {
            if (["edit", "paste", "autofill"].includes(source)) {

                return generalBeforeChange_4(this, changes, configuracionColumnasd1a);
            }
        }
        ,
        afterChange: function (changes, source) {
            generalAfterChange_2(this, changes, source);
        }
    });

    hot3PePesimista.render();
}
function crearTableInversEstim(inicioan, finan) {
    console.log("POrque no render")
    var data = [];
    for (var year = inicioan; year <= finan; year++) {
        var row = [year];
        for (var i = 0; i < 1; i++) {
            row.push('');
        }
        data.push(row);
    }

    var grilla = document.getElementById('tableInversEstim');

    if (typeof hot4InversEstim !== 'undefined' && hot4InversEstim !== null) {
        hot4InversEstim.destroy();
    }

    hot4InversEstim = new Handsontable(grilla, {
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
                { label: 'Monto de Inversi&oacute;n', colspan: 1 }
            ]
        ],
        // Definición de las columnas y validadores
        columns: [
            {
                data: 0,
                type: 'numeric',
                readOnly: true,
                renderer: FormatDisabledRenderer,
                validator: positiveNumberValidator,
                allowInvalid: false
            },
            {
                data: 1,
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

    hot4InversEstim.render();
}

function getDataDFormatoAInvEsti() {
    var datosArray = hot4InversEstim.getData();
    var datosObjetos = datosArray.map(function (row) {
        return {
            Anio: row[0],
            MontoInversion: row[1]
        };
    });
    var datosFiltrados = datosObjetos.filter(function (obj) {
        return (
            obj.Anio !== null && obj.Anio !== "" ||
            obj.MontoInversion !== null && obj.MontoInversion !== ""
        );
    });
    return datosFiltrados;


}

getDataDFormatoA = function () {
    var param = {};
    param.TipoCarga = $("#txtTipoCargaComb").val();
    param.Nombre = $("#txtNombre").val();
    param.EmpresaProp = $("#txtEmpresaProp").val();
    param.Distrito = $("#distritosSelect").val();
    param.ActDesarrollo = $("#txtActDesarrollo").val();
    param.SituacionAct = $("#txtSituacionAct").val();
    param.Exploracion = $("#txtExploracion").val();
    param.EstudioPreFactibilidad = $("#txtEstudioPreFactibilidad").val();
    param.EstudioFactibilidad = $("#txtEstudioFactibilidad").val();
    param.EstudioImpAmb = $("#txtEstudioImpAmb").val();
    param.Financiamiento1 = $("#txtFinanciamiento1").val();
    param.Ingenieria = $("#txtIngenieria").val();
    param.Construccion = $("#txtConstruccion").val();
    param.PuestaMarchar = $("#txtPuestaMarchar").val();
    param.TipoExtraccionMin = $("#txtTipoExtraccionMin").val();
    param.MetalesExtraer = $("#txtMetalesExtraer").val();
    param.TipoYacimiento = $("#txtTipoYacimiento").val();
    param.VidaUtil = $("#txtVidaUtil").val();
    param.Reservas = $("#txtReservas").val();
    param.EscalaProduccion = $("#txtEscalaProduccion").val();
    param.PlantaBeneficio = $("#txtPlantaBeneficio").val();
    param.RecuperacionMet = $("#txtRecuperacionMet").val();
    param.LeyesConcentrado = $("#txtLeyesConcentrado").val();
    param.CapacidadTrata = $("#txtCapacidadTrata").val();
    param.ProduccionAnual = $("#txtProduccionAnual").val();
    param.Item = $("#txtItem").val();
    param.ToneladaMetrica = $("#txtToneladaMetrica").val();
    param.Energia = $("#txtEnergia").val();
    param.Consumo = $("#txtConsumo").val();
    param.SubestacionCodi = $("#txtSubestacionCodiComb").val();
    param.SubestacionOtros = $("#txtSubestacionOtros").val();
    param.NivelTension = $("#txtNivelTension").val();
    param.EmpresaSuminicodi = $("#txtEmpresaSuminicodiComb").val();
    param.EmpresaSuminiOtro = $("#txtEmpresaSuminiOtro").val();
    param.FactorPotencia = $("#txtFactorPotencia").val();
    param.Inductivo = $("#txtInductivo").val();
    param.Capacitivo = $("#txtCapacitivo").val();
    param.PrimeraEtapa = $("#txtPrimeraEtapa").val();
    param.SegundaEtapa = $("#txtSegundaEtapa").val();
    param.Final = $("#txtFinal").val();
    param.CostoProduccion = $("#txtCostoProduccion").val();
    param.Metales = $("#txtMetales").val();
    param.Precio = $("#txtPrecio").val();
    param.Financiamiento2 = $("#txtFinanciamiento2").val();
    param.FacFavEjecuProy = $("#txtFacFavEjecuProy").val();
    param.FactDesEjecuProy = $("#txtFactDesEjecuProy").val();
    param.Comentarios = $("#txtComentarios").val();
    return param;

}

function cargarDepartamentos() {
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'ListarDepartamentos',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(''),
        success: function (eData) {
            console.log(eData);
            cargarListaDepartamento(eData);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaDepartamento(departamentos) {
    var selectDepartamentos = $('#departamentosSelect');
    $.each(departamentos, function (index, departamento) {
        // Crear la opci�n
        var option = $('<option>', {
            value: departamento.Id,
            text: departamento.Nombre
        });

        // Agregar la opci�n al select
        selectDepartamentos.append(option);
    });

    selectDepartamentos.change(function () {
        // Obtener el id del departamento seleccionado
        var idSeleccionado = $(this).val();
        // Llamar a la funci�n con el id del departamento
        console.log(idSeleccionado);
        cargarProvincia(idSeleccionado);
    });

}


function cargarProvincia(id, callback) {
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'ListarProvincias',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (eData) {
            console.log(eData);
            cargarListaProvincia(eData);
            if (typeof callback === 'function') {
                callback();
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaProvincia(provincias) {
    var selectProvincias = $('#provinciasSelect');
    selectProvincias.empty();
    var optionDefault = $('<option>', {
        value: "",
        text: "Seleccione una provincia"
    });
    selectProvincias.append(optionDefault); 
    cargarListaDistritos([]);
    $.each(provincias, function (index, provincia) {
        // Crear la opci�n
        var option = $('<option>', {
            value: provincia.Id,
            text: provincia.Nombre
        });

        // Agregar la opci�n al select
        selectProvincias.append(option);
    });

    selectProvincias.change(function () {
        // Obtener el id del departamento seleccionado
        var idSeleccionado = $(this).val();
        // Llamar a la funci�n con el id del departamento
        cargarDistrito(idSeleccionado);
    });

}

function cargarDistrito(id, callback) {
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'ListarDistritos',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (eData) {
            console.log(eData);
            cargarListaDistritos(eData);
            if (typeof callback === 'function') {
                callback();
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaDistritos(distritos) {
    var selectDistritos = $('#distritosSelect');
    selectDistritos.empty();
    var optionDefault = $('<option>', {
        value: "",
        text: "Seleccione un distrito"
    });
    selectDistritos.append(optionDefault);
    $.each(distritos, function (index, distrito) {
        // Crear la opci�n
        var option = $('<option>', {
            value: distrito.Id,
            text: distrito.Nombre
        });

        // Agregar la opci�n al select
        selectDistritos.append(option);
    });

}

function getDataDFormatoADet() {
    var datosArray = hotPeMedio.getData();


    var datosObjetos = datosArray.map(function (row) {
        return {
            Anio: row[0],
            DemandaEnergia: row[1],
            DemandaHP: row[2],
            DemandaHFP: row[3],
            DemandaCarga: row[4],
            GeneracionEnergia: row[5],
            GeneracionHP: row[6],
            GeneracionHFP: row[7],
            DemandaNetaHP: row[9],
            DemandaNetaHFP: row[10],
        };
    });
    var datosFiltrados = datosObjetos.filter(function (obj) {
        return (
            obj.DemandaEnergia !== null && obj.DemandaEnergia !== "" ||
            obj.DemandaHP !== null && obj.DemandaHP !== "" ||
            obj.DemandaHFP !== null && obj.DemandaHFP !== "" ||
            obj.DemandaCarga !== null && obj.DemandaCarga !== "" ||
            obj.GeneracionEnergia !== null && obj.GeneracionEnergia !== "" ||
            obj.GeneracionHP !== null && obj.GeneracionHP !== "" || 
            obj.GeneracionHFP !== null && obj.GeneracionHFP !== "" ||
            obj.DemandaNetaHP !== null && obj.DemandaNetaHP !== ""
        );
    });
    return datosFiltrados;
}

function getDataDFormatoADetOpt(){
    var datosArray = hot2PeOptimista.getData();
    var datosObjetos = datosArray.map(function (row) {
        return {
            Anio: row[0],
            DemandaEnergia: row[1],
            DemandaHP: row[2],
            DemandaHFP: row[3],
            DemandaCarga: row[4],
            GeneracionEnergia: row[5],
            GeneracionHP: row[6],
            GeneracionHFP: row[7],
            DemandaNetaHP: row[9],
            DemandaNetaHFP: row[10],
        };
    });
    var datosFiltrados = datosObjetos.filter(function (obj) {
        return (
            obj.DemandaEnergia !== null && obj.DemandaEnergia !== "" ||
            obj.DemandaHP !== null && obj.DemandaHP !== "" ||
            obj.DemandaHFP !== null && obj.DemandaHFP !== "" ||
            obj.DemandaCarga !== null && obj.DemandaCarga !== "" ||
            obj.GeneracionEnergia !== null && obj.GeneracionEnergia !== "" ||
            obj.GeneracionHP !== null && obj.GeneracionHP !== "" ||
            obj.GeneracionHFP !== null && obj.GeneracionHFP !== "" ||
            obj.DemandaNetaHP !== null && obj.DemandaNetaHP !== ""
        );
    });
    return datosFiltrados;
}

function getDataDFormatoADetPes() {
    var datosArray = hot3PePesimista.getData();
    var datosObjetos = datosArray.map(function (row) {
        return {
            Anio: row[0],
            DemandaEnergia: row[1],
            DemandaHP: row[2],
            DemandaHFP: row[3],
            DemandaCarga: row[4],
            GeneracionEnergia: row[5],
            GeneracionHP: row[6],
            GeneracionHFP: row[7],
            DemandaNetaHP: row[9],
            DemandaNetaHFP: row[10],
        };
    });
    var datosFiltrados = datosObjetos.filter(function (obj) {
        return (
            obj.DemandaEnergia !== null && obj.DemandaEnergia !== "" ||
            obj.DemandaHP !== null && obj.DemandaHP !== "" ||
            obj.DemandaHFP !== null && obj.DemandaHFP !== "" ||
            obj.DemandaCarga !== null && obj.DemandaCarga !== "" ||
            obj.GeneracionEnergia !== null && obj.GeneracionEnergia !== "" ||
            obj.GeneracionHP !== null && obj.GeneracionHP !== "" ||
            obj.GeneracionHFP !== null && obj.GeneracionHFP !== "" ||
            obj.DemandaNetaHP !== null && obj.DemandaNetaHP !== ""
        );
    });
    return datosFiltrados;
}


function grabarDForm() {

    if (!validarTablaAntesDeGuardar(hotPeMedio,'Medio')) {
        mostrarNotificacion("No se puede guardar porque hay datos inválidos en la tabla.");
        return; // Detener el guardado si hay errores
    }
    if (!validarTablaAntesDeGuardar(hot2PeOptimista,'Optimista')) {
        mostrarNotificacion("No se puede guardar porque hay datos inválidos en la tabla.");
        return; // Detener el guardado si hay errores
    }

    if (!validarTablaAntesDeGuardar(hot3PePesimista,'Pesimista')) {
        mostrarNotificacion("No se puede guardar porque hay datos inválidos en la tabla.");
        return; // Detener el guardado si hay errores
    }


    console.log("grabarDFormatoA");
    var param = {};
    param.TransmisionProyectoDTO = getCabeceraProyecto();
    var idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        const ValidMedio = verificarValoresValidos(hotPeMedio);
        const ValidOptimista = verificarValoresValidos(hot2PeOptimista);
        const ValidPesimista = verificarValoresValidos(hot3PePesimista);
        if(ValidMedio && ValidOptimista && ValidPesimista){
            param.ProyCodi = idProyecto;
            param.FormatoD1ADTO = getDataDFormatoA();
            param.ListaFormatoDet1A = getDataDFormatoADet();
            param.ListaFormatoDet2A = getDataDFormatoADetOpt();
            param.ListaFormatoDet3A = getDataDFormatoADetPes();
            param.ListaFormatoDet4A = getDataDFormatoAInvEsti();
            param.ListaFormatoDet5A = getDataDFormatoResumen();
            $.ajax({
                type: 'POST',
                url: controlador + 'GrabarDFormatoA',
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
        } else {
            mostrarMensaje('mensajeFicha', 'error', 'Existen celdas inválidas en la tabla. Por favor, corrige los valores antes de guardar.');
        }
    }
    cambiosRealizados = false;
}

function verificarValoresValidos(hotInstance) {
    let hayCeldasInvalidas = false;
    let celdasInvalidas = [];
    hotInstance.getData().forEach((row, rowIndex) => {
        row.forEach((value, colIndex) => {
            const cellMeta = hotInstance.getCellMeta(rowIndex, colIndex);
            if (cellMeta.valid === false) {
                hayCeldasInvalidas = true;
                celdasInvalidas.push({ row: rowIndex, col: colIndex });
            }
        });
    });

    if (hayCeldasInvalidas) {
        return false; 
    }
    return true;
}


function setDataDFormatoA(param) {
    $("#txtTipoCargaComb").val(param.TipoCarga);
    $("#txtNombre").val(param.Nombre);
    $("#txtEmpresaProp").val(param.EmpresaProp);
    //$("#distritosSelect").val(param.Distrito);
    cargarUbicacionD1A(param.Distrito);
    $("#txtActDesarrollo").val(param.ActDesarrollo);
    $("#txtSituacionAct").val(param.SituacionAct);
    $("#txtExploracion").val(param.Exploracion);
    $("#txtEstudioPreFactibilidad").val(param.EstudioPreFactibilidad);
    $("#txtEstudioFactibilidad").val(param.EstudioFactibilidad);
    $("#txtEstudioImpAmb").val(param.EstudioImpAmb);
    $("#txtFinanciamiento1").val(param.Financiamiento1);
    $("#txtIngenieria").val(param.Ingenieria);
    $("#txtConstruccion").val(param.Construccion);
    $("#txtPuestaMarchar").val(param.PuestaMarchar);
    $("#txtTipoExtraccionMin").val(param.TipoExtraccionMin);
    $("#txtMetalesExtraer").val(param.MetalesExtraer);
    $("#txtTipoYacimiento").val(param.TipoYacimiento);
    $("#txtVidaUtil").val(param.VidaUtil);
    $("#txtReservas").val(param.Reservas);
    $("#txtEscalaProduccion").val(param.EscalaProduccion);
    $("#txtPlantaBeneficio").val(param.PlantaBeneficio);
    $("#txtRecuperacionMet").val(param.RecuperacionMet);
    $("#txtLeyesConcentrado").val(param.LeyesConcentrado);
    $("#txtCapacidadTrata").val(param.CapacidadTrata);
    $("#txtProduccionAnual").val(param.ProduccionAnual);
    $("#txtItem").val(param.Item);
    $("#txtToneladaMetrica").val(param.ToneladaMetrica);
    $("#txtEnergia").val(param.Energia);
    $("#txtConsumo").val(param.Consumo);
    $("#txtSubestacionCodiComb").val(param.SubestacionCodi);
    $("#txtSubestacionOtros").val(param.SubestacionOtros);
    $("#txtNivelTension").val(param.NivelTension);
    $("#txtEmpresaSuminicodiComb").val(param.EmpresaSuminicodi);
    $("#txtEmpresaSuminiOtro").val(param.EmpresaSuminiOtro);
    $("#txtFactorPotencia").val(param.FactorPotencia);
    $("#txtInductivo").val(param.Inductivo);
    $("#txtCapacitivo").val(param.Capacitivo);
    $("#txtPrimeraEtapa").val(param.PrimeraEtapa);
    $("#txtSegundaEtapa").val(param.SegundaEtapa);
    $("#txtFinal").val(param.Final);
    $("#txtCostoProduccion").val(param.CostoProduccion);
    $("#txtMetales").val(param.Metales);
    $("#txtPrecio").val(param.Precio);
    $("#txtFinanciamiento2").val(param.Financiamiento2);
    $("#txtFacFavEjecuProy").val(param.FacFavEjecuProy);
    $("#txtFactDesEjecuProy").val(param.FactDesEjecuProy);
    $("#txtComentarios").val(param.Comentarios);

    cargarArchivosRegistrados(seccionUbicacionD11, '#tablaUbicacionD11');
    cargarArchivosRegistrados(seccionD12, '#tablaD12');
    cargarArchivosRegistrados(seccionD13, '#tablaD13');
    cargarArchivosRegistrados(seccionD14, '#tablaD14');

    if (modoModel == "consultar") {
        desactivarCamposFormulario('DGPFormatoD1A');
        $('#D1GrabarA').hide();
    }

}

function setDataDFormatoDet2(dataObjects) {
    if (typeof hot2PeOptimista === 'undefined' || hot2PeOptimista === null) {
        console.error("La tabla Handsontable no está inicializada.");
        return;
    }

    var currentData = hot2PeOptimista.getData();

    dataObjects.forEach(function (obj) {
        var rowIndex = currentData.findIndex(row => row[0] === obj.Anio);
        if (rowIndex !== -1) {
            currentData[rowIndex][1] = obj.DemandaEnergia ?? ""; // Energía
            currentData[rowIndex][2] = obj.DemandaHP ?? ""; // HP
            currentData[rowIndex][3] = obj.DemandaHFP ?? ""; // HFP
            currentData[rowIndex][4] = obj.DemandaCarga ?? ""; // Energía Generación
            currentData[rowIndex][5] = obj.GeneracionEnergia ?? ""; // HP Generación
            currentData[rowIndex][6] = obj.GeneracionHP ?? ""; // HFP Generación
            currentData[rowIndex][7] = obj.GeneracionHFP ?? ""; // HFP Generación
            currentData[rowIndex][9] = obj.DemandaNetaHP ?? ""; // HFP Generación
            currentData[rowIndex][10] = obj.DemandaNetaHFP ?? ""; // HFP Generación
        }
    });

    hot2PeOptimista.loadData(currentData);
    hot2PeOptimista.render();
}

function setDataDFormatoDet3(dataObjects) {
    if (typeof hot3PePesimista === 'undefined' || hot3PePesimista === null) {
        console.error("La tabla Handsontable no está inicializada.");
        return;
    }

    var currentData = hot3PePesimista.getData();

    dataObjects.forEach(function (obj) {
        var rowIndex = currentData.findIndex(row => row[0] === obj.Anio);
        if (rowIndex !== -1) {
            currentData[rowIndex][1] = obj.DemandaEnergia ?? ""; // Energía
            currentData[rowIndex][2] = obj.DemandaHP ?? ""; // HP
            currentData[rowIndex][3] = obj.DemandaHFP ?? ""; // HFP
            currentData[rowIndex][4] = obj.DemandaCarga ?? ""; // Energía Generación
            currentData[rowIndex][5] = obj.GeneracionEnergia ?? ""; // HP Generación
            currentData[rowIndex][6] = obj.GeneracionHP ?? ""; // HFP Generación
            currentData[rowIndex][7] = obj.GeneracionHFP ?? ""; // HFP Generación
            currentData[rowIndex][9] = obj.DemandaNetaHP ?? ""; // HFP Generación
            currentData[rowIndex][10] = obj.DemandaNetaHFP ?? ""; // HFP Generación
        }
    });

    hot3PePesimista.loadData(currentData);
    hot3PePesimista.render();
}

function setDataDFormatoDet4(listaDet1, inicioan, finan) {
    var datosArray = [];
    var añosPresentes = new Set(listaDet1.map(item => item.Anio));

    for (var year = inicioan; year <= finan; year++) {
        if (!añosPresentes.has(year)) {
            listaDet1.push({ Anio: year, MontoInversion: 0 });
        }
    }

    listaDet1.sort((a, b) => a.Anio - b.Anio);

    listaDet1.map(function (item) {
        var row = [
            item.Anio,
            item.MontoInversion ?? 0
        ];
        datosArray.push(row);
    });

    hot4InversEstim.loadData(datosArray);
}

function setDataDFormatoDet5(datosSeteo) {
    if (typeof hot5Resumen === 'undefined' || hot5Resumen === null) {
        return;
    }

    var hotInstance = hot5Resumen.getInstance();
    var data = hotInstance.getData();

    datosSeteo.forEach(function (dato) {
        var rowIndex = data.findIndex(row => row[0] == dato.DataCatCodi);
        if (rowIndex !== -1) {
            data[rowIndex][2] = dato.EnElaboracion ?? 0;
            data[rowIndex][3] = dato.Presentado ?? 0;
            data[rowIndex][4] = dato.EnTramite ?? 0;
            data[rowIndex][5] = dato.Aprobado ?? 0;
            data[rowIndex][6] = dato.Firmado ?? 0;
        }
    });

    hotInstance.loadData(data);
}

function getDataDFormatoResumen() {
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


function crearTablaResumenSitProy() {
    var data = [];

    for (var i = 0; i < valoresListaFDA.length; i++) {
        var row = [];
        row.push(valoresListaFDA[i].DataCatCodi);
        row.push(valoresListaFDA[i].DesDataCat);
        for (var j = 0; j < 5; j++) { // Cambié 'i' a 'j'
            row.push(false);
        }
        data.push(row);
    }

    var grilla = document.getElementById('tableResumenSitProy');

    if (typeof hot5Resumen !== 'undefined' && hot5Resumen !== null) {
        hot5Resumen.destroy();
    }

    hot5Resumen = new Handsontable(grilla, {
        data: data,
        // rowHeaders: true,
        colHeaders: true,
        colWidths: 170,
        manualColumnResize: true,
        autoRowSize: true,
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
    });
    
    hot5Resumen.render();
}


async function cargarCatalogosYAsignarValoresD1(param) {
    console.log("Verificando y cargando catálogos...");

    let promesas = [
        esperarCatalogo(catTipoCarga, "#txtTipoCargaComb"),
        esperarCatalogoSubestacion("#txtSubestacionCodiComb", true),
        esperarCatalogo(catSubEmpElectro, "#txtEmpresaSuminicodiComb")
    ];

    // Asegurar que todas las promesas de carga se completen antes de continuar
    await Promise.all(promesas);

    console.log("Todos los catálogos han sido cargados. Asignando valores...");

 

    asignarSiExisteGlobal("#txtTipoCargaComb", param.TipoCarga);
    asignarSiExisteGlobal("#txtSubestacionCodiComb", param.SubestacionCodi);
    asignarSiExisteGlobal("#txtEmpresaSuminicodiComb", param.EmpresaSuminicodi);
     

    console.log("Valores asignados correctamente.");
}


async function cargarDFormatoA() {
    limpiarCombos('#txtTipoCargaComb,#txtSubestacionCodiComb,#txtEmpresaSuminicodiComb');


    await cargarCatalogosYAsignarValoresD1({});


    limpiarMensaje('mensajeFicha');
    if (modoModel == "editar" || modoModel == "consultar") {
        var idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorFichas + 'GetDFormatoA',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: idProyecto }),
            
            success: function (response) {
                console.log(response);
                var hojaCRes = response.responseResult;




                setDataDFormatoA(hojaCRes);
                setDataDFormatoDet1(hojaCRes.ListaFormatoDet1A);
                setDataDFormatoDet2(hojaCRes.ListaFormatoDet2A);
                setDataDFormatoDet3(hojaCRes.ListaFormatoDet3A);
                setDataDFormatoDet4(hojaCRes.ListaFormatoDet4A, 2018, horizonteFin);
                setDataDFormatoDet5(hojaCRes.ListaFormatoDet5A);
                cambiosRealizados = false;
                setupDropdownToggle('txtSubestacionCodiComb', 'txtSubestacionOtros');
                setupDropdownToggle('txtEmpresaSuminicodiComb', 'txtEmpresaSuminiOtro');
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }

}
function cargarUbicacionD1A(idDistrito) {
    $.ajax({
        type: 'POST',
        url: controladorFichas + 'UbicacionByDistro',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: idDistrito }),
        success: function (ubicacion) {
            $('#departamentosSelect').val(ubicacion.DepartamentoId);

            cargarProvincia(ubicacion.DepartamentoId, function () {
                $('#provinciasSelect').val(ubicacion.ProvinciaId);

                cargarDistrito(ubicacion.ProvinciaId, function () {
                    $('#distritosSelect').val(ubicacion.DistritoId);

                });
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarCatalogoTablAFDemanda1(id) {
    console.log("Pruebaaa");
    $.ajax({
        type: 'POST',
        url: controlador + 'ListarCatalogo',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (eData) {
            valoresListaFDA = eData;
            crearTablaResumenSitProy();
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}


function aplicarEstiloCabeceras(ws, headerRows) {
    for (let r = 0; r < headerRows; r++) {
        let row = ws[r];
        for (let c in row) {
            let cellAddress = XLSX.utils.encode_cell({ r, c: parseInt(c) });
            if (!ws[cellAddress]) continue;
            if (!ws[cellAddress].s) ws[cellAddress].s = {};
            ws[cellAddress].s.alignment = { horizontal: "center", vertical: "center" };
        }
    }
}


function exportD1ADet4() {
    var headers = [["Periodo(Años)", "Monto Inversión(US$)"]];
    exportExcelGInvEstimada(hot4InversEstim, headers, "D1A_02-InversiónEstimada.xlsx");
}

function importD1ADet4() {
    importExcelGInvEstimada(hot4InversEstim, function (jsonData) {
        var data = jsonData.slice(1);
        hot4InversEstim.loadData(data);
        hot4InversEstim.render();
    });
}

function exportarTablaPEMedio() {
    exportExcelGSein(hotPeMedio, "D1A_01_01-DemandaBaseAnual","D1A_01_01");
}

function importarTablaPEMedio() {
    importGSein(hotPeMedio, function (jsonData) {
        const data = jsonData.slice(3);
        hotPeMedio.loadData(data);
        hotPeMedio.render();
    });
}

function exportarTablaPEOptimista() {
    exportExcelGSein(hot2PeOptimista, "D1A_01_02-DemandaOptimistaAnual","D1A_01_02");
}

function importarTablaPEOptimista() {
    importGSein(hot2PeOptimista, function (jsonData) {
        const data = jsonData.slice(3);
        hot2PeOptimista.loadData(data);
        hot2PeOptimista.render();
    });
}


function exportarTablaPEPesimista() {
    exportExcelGSein(hot3PePesimista, "D1A_01_03-DemandaPesimistaAnual","D1A_01_03");
}

function importarTablaPEPesimista() {
    importGSein(hot3PePesimista, function (jsonData) {
        const data = jsonData.slice(3);
        hot3PePesimista.loadData(data);
        hot3PePesimista.render();
    });
}
function exportarResumentProy() {
    exportarTablaResumenSitProy(hot5Resumen,"D1A_03-ResumenSituacionProyecto","D1A_03");
}
function importarResumentProy() {
    importarTablaResumenSitProy(hot5Resumen);
}

