var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/';
var tblPotenciaFirme;

const FAMCODI_SOLAR = 37;
const FAMCODI_EOLICA = 39;

$(function () {        
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#btnValidar').click(function () {
        var dataTable = tblPotenciaFirme.rows().data();

        var data = GetDataDataTable(dataTable);
        // SIOSEIN-PRIE-2021
        //var listNoVal = obtenerCentralesNoValidosPotenciaFirme(data);
        //if (listNoVal.length > 0) {
        //    pintarCentralesNoValidos(listNoVal);
        //    alert("Existe Potencia Firme por central mayor que la Potencia Ejectiva [Marcado en color rojo]");
        //    return;
        //}

        //if (!validacionSumTotalPEyPF(data)) {
        //    alert("Potencia Firme Total no es menor que la Potencia Efectiva Total");
        //    return;
        //}

        guardarListaPFirmeADatosPrie(data);

    });

    $('#btnIrCargar').click(function () {
        var periodo = $("#txtFecha").val();
        document.location.href = controlador + 'index?periodo=' + periodo;
    });
    //#region Cambios SIOSEIN
    tblPotenciaFirme = $('#tblPotenciaFirme').DataTable({
        data: [],
        columns: [
            { data: "Emprnomb", width: "20%", className: "text" },
            { data: "Osinergcodi", width: "20%", className: "text" },       // SIOSEIN-PRIE-2021
            { data: "Central", width: "20%", className: "text" },
            { data: "ValorPfAct", width: "10%", className: "number" },
            { data: "ValorPfAnt", width: "10%", className: "number" },
            { data: "Variacion", width: "10%", className: "number" }
            //{ data: "Pefectivavalor", visible: false },                   // SIOSEIN-PRIE-2021
            //{ data: "Emprcodi", visible: false }                          // SIOSEIN-PRIE-2021
        ],
        rowCallback: function (row, data) { },
        filter: true,
        info: true,
        processing: true,
        scrollY: "400px",
        scrollCollapse: true,
        paging: false,
        order: [[1, 'asc']],
    });
    //#endregion

    // #region SIOSEIN-PRIE-2021
    $('#btnExportarExcel').on('click', function () {
        exportarExcel();
    });
    $('#btnExportarTexto').on('click', function () {
        exportarTexto();
    });
    //#endregion
    cargarListaPotenciaFirme();
});

function cargarListaPotenciaFirme() {

    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaPotenciaFirme',
        data: { mesAnio: mesAnio },
        success: function (aData) {

            //#region Cambios SIOSEIN
            if (aData.Estado === false) {
                alert("Ha ocurrido un error en la consulta");
                return;
            }

            tblPotenciaFirme.clear().draw();
            tblPotenciaFirme.rows.add(aData.listaPFirme).draw();

            setTimeout(function () {
                $($.fn.dataTable.tables(true)).DataTable().columns.adjust().draw();
            }, 5);

            //#endregion

            $('#msj_listado1').show();

            $('#listado2').html(aData.listaPFirmeEmpresaHTML);
            $('#tblPotenciaFirmeXEmpresa').dataTable({
                "ordering": false,
                "bInfo": false
            });

            $('#listado3').html(aData.listaPFirmeEmpresaCentralHTML);
            $('#tblPotenciaFirmeXEmpresaCentral').dataTable({
                "ordering": false,
                "bInfo": false
            });
            $('#msj_listado3').show();


        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function guardarListaPFirmeADatosPrie(data) {

    if (confirm("Se enviara la informacion a tablas PRIE. ¿Desea continuar?")) {

        var dataJson = {
            mesAnio: $('#txtFecha').val(),
            listaData: data
        };

        $.ajax({
            type: 'POST',
            url: controlador + 'SendListaPotenciaFirme',
            data: JSON.stringify(dataJson),
            contentType: 'application/json; charset=UTF-8',
            dataType: 'json',
            success: function (d) {
                if (d.ResultadoInt === 1) {
                    alert("Se enviaron los Datos Correctamente!..");
                    //document.location.href = controlador + 'DifusionPotenciaFirme?periodo=' + $('#txtFecha').val();       // SIOSEIN-PRIE-2021
                }
                else {
                    alert("Error!");
                }
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

//#region Cambios SIOSEIN

/**
 * Permite pintar celda de las centrales que tengan menor PE que PF
 * @param {any} listNoVal Lista centrales no valido
 */
function pintarCentralesNoValidos(listNoVal) {

    var data = tblPotenciaFirme.rows().data();
    $.each(data, function (index, value) {

        $node = tblPotenciaFirme.row(index).nodes().to$();
        $node.removeClass('invalido');

        if (listNoVal.includes(value.CodCentral)) {
            $node.addClass('invalido');
        }
    });

}

function obtenerCentralesNoValidosPotenciaFirme(dataList) {

    var dataGrp = dataList.groupBy('CodCentral');

    var listaCentrallesInval = [];
    
    $.each(dataGrp, function (index, value) {

        if (value[0].Famcodi === FAMCODI_EOLICA || value[0].Famcodi === FAMCODI_SOLAR) return;

        var pfirmevalor = value.sum("Pfirmevalor");
        var pefectivavalor = value.sum("Pefectivavalor");

        if (pefectivavalor < pfirmevalor) {
            listaCentrallesInval.push(parseInt(index));
        }
    });
    
    return listaCentrallesInval;
}

/**
 * Permite validad que la PE total sea mayor PF total
 * @param {any} data data data PRIE 01
 * @returns {any} bool
 */
function validacionSumTotalPEyPF(data) {
    var valPE = 0;
    var valPF = 0;
    $.each(data, function (index, value) {
        if (value.Pfirmevalor !== null) {
            valPE += value.Pefectivavalor;
            valPF += value.Pfirmevalor;
        }
    });

    return valPF < valPE ? true : false;
}

Array.prototype.groupBy = function (prop) {
    return this.reduce(function (groups, item) {
        const val = item[prop];
        groups[val] = groups[val] || [];
        groups[val].push(item);
        return groups;
    }, {});
};

Array.prototype.sum = function (prop) {
    var total = 0;
    for (var i = 0, _len = this.length; i < _len; i++) {
        total += this[i][prop];
    }
    return total;
};

//#endregion


// #region SIOSEIN-PRIE-2021
function exportarExcel() {

    var idTabla = "#tblPotenciaFirme";
    var dt = $(idTabla).DataTable().rows().data();
    var dataList = {};

    // Leer sólo los campos necesarios para la exportación.
    $.each(dt, function (index, value) {
        dataList[index.toString()] = [value.Emprnomb, value.Osinergcodi, value.Central, value.ValorPfAct,
            value.ValorPfAnt, value.Variacion];
    });
    //

    var datosTabla = GetDataDataTable(dataList);

    if (datosTabla == null || datosTabla.length < 1) {
        alert("No existen registros para exportar");
        return;
    }

    var nombreArchivo = nombreArchivoTablaPrie("01_PFIR", $("#txtFecha").val());
    var listaColumnaAtributos = [];
    listaColumnaAtributos.push({ "col": 0, "ancho": 50, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 1, "ancho": 20, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 2, "ancho": 30, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 3, "ancho": 45, "alinea": "right" });
    listaColumnaAtributos.push({ "col": 4, "ancho": 25, "alinea": "right" });
    listaColumnaAtributos.push({ "col": 5, "ancho": 25, "alinea": "right" });
    var defaultColumnaAtributos = { "ancho": 20, "alinea": "right", "tipo": "string", "omitir": "no" };

    var listaExcelHoja = [];
    var objeto = {
        "idTabla": idTabla,
        "datosTabla": datosTabla,
        "titulo": "Verificación Potencia Firme - Tabla 01",
        "nombreHoja": "TABLA N° 01",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    };

    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto));
    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);

}

function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "PFIR";
    var tpriecodi = 1;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}
//#endregion

