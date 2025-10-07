var controlador = siteRoot + 'Yupana/CondicionInicial/';

var listaDataGeneral;
var listaRelaciones;
var tblHndModoOperacion;
var tblHndCaldero;
var tblHndInyeccion;
var listaErroresMOD = [];
var listaErroresCAL = [];
var listaErroresINY = [];
var listErrores = [];
var DATA_MOD;
var DATA_CAL;
var DATA_INY;

const MOD = 1;
const CAL = 4;
const INY = 5;

$(function () {

    $('#Anho').Zebra_DatePicker({
        format: 'Y',
        onSelect: function () {
            cargarSemanaAnho();
        }
    });
    $('#txtFecha').Zebra_DatePicker({
    });

    $('#cbHorizonte').change(function () {
        horizonte();
        //pintaDescripcion($('#cbFormato').val())
        cargarDatosCondicionInicial();
    });

    $('#btnConsultar').click(function () {
        cargarDatosCondicionInicial();
    });

    $('#btnConfigurar').on('click', function () {
        document.location.href = siteRoot + 'Yupana/CondicionInicial/ConfiguracionIndex';
    });

    $('#btnEnviarDatos').click(function () {
        enviarDatos();
    });

    $('#btnMostrarErrores').click(function () {
        mostrarListadoErrores();
    });

    $('#btnCargarfil').click(function () {

        const currentDataMOD = DATA_MOD.getSourceData(); //Modos de operación
        const currentDataCAL = DATA_CAL.getSourceData(); //Calderos
        const currentDataINY = DATA_INY.getSourceData(); //Inyección
        const idsFiltrarMOD = currentDataMOD.map(objeto => objeto.Rescfgcodi); //obtener data actual del handson
        const idsFiltrarCAL = currentDataCAL.map(objeto => objeto.Rescfgcodi); //obtener data actual del handson
        const idsFiltrarINY = currentDataINY.map(objeto => objeto.Rescfgcodi); //obtener data actual del handson

        var datosfaltanteMOD = listaDataGeneral.filter(obj => obj.Restipcodi == MOD && !idsFiltrarMOD.includes(obj.Rescfgcodi));
        var datosfaltanteCAL = listaDataGeneral.filter(obj => obj.Restipcodi == CAL && !idsFiltrarCAL.includes(obj.Rescfgcodi));
        var datosfaltanteINY = listaDataGeneral.filter(obj => obj.Restipcodi == INY && !idsFiltrarINY.includes(obj.Rescfgcodi));

        if (datosfaltanteMOD.length > 0 || datosfaltanteCAL.length > 0 || datosfaltanteINY.length > 0) {
            tblHndModoOperacion.loadData(currentDataMOD.concat(datosfaltanteMOD));
            tblHndCaldero.loadData(currentDataCAL.concat(datosfaltanteCAL));
            tblHndInyeccion.loadData(currentDataINY.concat(datosfaltanteINY));
        }
        else
            alert("No existen nuevos registros para completar");

    });

    horizonte();
    cargarSemanaAnho();

    cargarDatosCondicionInicial();
});

function cargarDatosCondicionInicial() {

    var idHorizonte = $("#cbHorizonte").val();
    var idEnvio = 0;
    var fecha = $("#txtFecha").val();
    var semana = $("#cbSemana").val();
    var anho = $("#Anho").val();
    var semana = anho.toString() + semana;

    limpiarBarraMensaje("mensaje");

    $.ajax({
        type: 'POST',
        url: controlador + "CargarCondicionesIniciales",
        data: {
            formato: idHorizonte,
            fecha: fecha,
            semana: semana
        },
        success: function (evt) {

            if (evt.Resultado == "1") {
                var data = evt.DataCondicionInicial;
                inicializarCondicionesIniciales(evt);
                iniciarHandsontables();
                cargarHansonTablas(data);
                $('.wtHolder').css("width", "750px");
                if (evt.IdEnvio > 0) {
                    var mensaje = "<strong>Código de envío</strong> : " + evt.IdEnvio + ", <strong>Fecha de envío: </strong>" + evt.FechaProceso;
                    mostrarMensaje('mensaje', 'exito', mensaje);
                }
                else
                    mostrarMensaje('mensaje', 'message', "Por favor complete la información");

            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (xhr, status) {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });

}

function inicializarCondicionesIniciales(evt) {
    listErrores = [];
    listaErroresMOD = [];
    listaErroresCAL = [];
    listaErroresINY = [];
    $('.leyenda-actividad-titulo').css("display", "block");

    if (evt.IdEnvio > 0)
        $('#btnCargarfil').css("display", "block");
    else
        $('#btnCargarfil').css("display", "none");

}

function iniciarHandsontables() {

    /************** MODOS DE OPERACIÓN *********************/
    $('#tblModosOperacion').html("");
    // Inicio datos
    Handsontable.renderers.registerRenderer('datosModosOperacionRenderer', datosModosOperacionRenderer);
    Handsontable.renderers.registerRenderer('datosCombosModosOperacionRenderer', datosCombosModosOperacionRenderer);

    //containerTablaModoOperacion = document.getElementById('tblModosOperacion');
    tblHndModoOperacion = new Handsontable(document.getElementById('tblModosOperacion'), {
        dataSchema: {
            Rescfgcodi: null,
            Rescfgnombre: null,
            Resdatflag: null,
            Resdatdato: null
        },
        colHeaders: ['CÓDIGO', 'MODO OPERACÍÓN', 'CONDICIÓN INICIAL', 'N° HORAS'],
        columns: [
            { data: 'Rescfgcodi', editor: false, className: 'fondo_soloLectura htCenter', readOnly: true },
            { data: 'Rescfgnombre', editor: false, className: 'fondo_soloLectura htCenter', readOnly: true },
            { data: 'Resdatflag', type: 'dropdown', "allowInvalid": false, source: ['Conectado', 'Desconectado'] },
            { data: 'Resdatdato', type: 'numeric', className: 'htCenter' }
        ],
        hiddenColumns: {
            columns: [4],
            indicators: false,
        },
        height: '300px',
        width: '45%',

        //Tamaño maximo de columna
        modifyColWidth: function (width, col) {

            if (col == 1) {
                return 380
            }
            if (col == 2) {
                return 200
            }
            if (col == 3) {
                return 80
            }
        }
    });

    tblHndModoOperacion.addHook('beforeChange', function (changes, [source]) {
        listaErroresMOD = [];
    });
    tblHndModoOperacion.addHook('afterChange', function (changes, [source]) {
        listaErroresMOD = [];
        if (['edit', 'e', 'paste', 'C'].includes(source)){
            tblHndModoOperacion.render();
        }
        listErrores = listaErroresCAL.concat(listaErroresINY.concat(listaErroresMOD));
    });

    /************** CALDEROS *********************/
    $('#tblCaldero').html("");
    // Inicio datos
    Handsontable.renderers.registerRenderer('datosCalderoRenderer', datosCalderoRenderer);
    Handsontable.renderers.registerRenderer('datosCombosCalderoRenderer', datosCombosCalderoRenderer);

    //containerTablaModoOperacion = document.getElementById('tblModosOperacion');
    tblHndCaldero = new Handsontable(document.getElementById('tblCaldero'), {

        dataSchema: {
            Rescfgcodi: null,
            Rescfgnombre: null,
            Resdatflag: null,
            Resdatdato: null
        },
        colHeaders: ['CÓDIGO', 'CALDERO', 'CONDICIÓN INICIAL', 'N° HORAS'],
        columns: [
            { data: 'Rescfgcodi', editor: false, className: 'fondo_soloLectura htCenter', readOnly: true },
            { data: 'Rescfgnombre', editor: false, className: 'fondo_soloLectura htCenter', readOnly: true },
            { data: 'Resdatflag', type: 'dropdown', "allowInvalid": false, source: ['Conectado', 'Desconectado'] },
            { data: 'Resdatdato', type: 'numeric', className: 'htCenter' }
        ],
        hiddenColumns: {
            columns: [4],
            indicators: false,
        },
        height: '150px',
        width: '45%',

        //Tamaño maximo de columna
        modifyColWidth: function (width, col) {
            if (col == 1) {
                return 380
            }
            if (col == 2) {
                return 200
            }
            if (col == 3) {
                return 80
            }
        }
    });

    tblHndCaldero.addHook('beforeChange', function (changes, [source]) {
        listaErroresCAL = [];
    });
    tblHndCaldero.addHook('afterChange', function (changes, [source]) {
        listaErroresCAL = [];
        if (['edit', 'e', 'paste', 'C'].includes(source)){
            tblHndCaldero.render();
        }
        listErrores = listaErroresMOD.concat(listaErroresINY.concat(listaErroresCAL));
    });


    /************** INYECCIÓN *********************/
    $('#tblInyeccion').html("");

    // Inicio datos
    Handsontable.renderers.registerRenderer('datosInyeccionRenderer', datosInyeccionRenderer);
    Handsontable.renderers.registerRenderer('datosCombosInyeccionRenderer', datosCombosInyeccionRenderer);

    //containerTablaModoOperacion = document.getElementById('tblModosOperacion');
    tblHndInyeccion = new Handsontable(document.getElementById('tblInyeccion'), {

        dataSchema: {
            Rescfgcodi: null,
            Rescfgnombre: null,
            Resdatflag: null,
            Resdatdato: null
        },
        colHeaders: ['CÓDIGO', 'INYECCIÓN', 'CONDICIÓN INICIAL', 'N° HORAS'],
        columns: [
            { data: 'Rescfgcodi', editor: false, className: 'fondo_soloLectura htCenter', readOnly: true },
            { data: 'Rescfgnombre', editor: false, className: 'fondo_soloLectura htCenter', readOnly: true },
            { data: 'Resdatflag', type: 'dropdown', "allowInvalid": false, source: ['Conectado', 'Desconectado'] },
            { data: 'Resdatdato', type: 'numeric', className: 'htCenter' }
        ],
        hiddenColumns: {
            columns: [4],
            indicators: false,
        },
        height: '150px',
        width: '45%',

        //Tamaño maximo de columna
        modifyColWidth: function (width, col) {
            if (col == 1) {
                return 380
            }
            if (col == 2) {
                return 200
            }
            if (col == 3) {
                return 80
            }
        }

    });

    tblHndInyeccion.addHook('beforeChange', function (changes, [source]) {
        listaErroresINY = [];
    });
    tblHndInyeccion.addHook('afterChange', function (changes, [source]) {
        listaErroresINY = [];
        if (['edit', 'e', 'paste', 'C'].includes(source)) {
            tblHndInyeccion.render();
        }
        listErrores = listaErroresMOD.concat(listaErroresCAL.concat(listaErroresINY));
    });

    DATA_MOD = tblHndModoOperacion;
    DATA_CAL = tblHndCaldero;
    DATA_INY = tblHndInyeccion;
    precargarDetallesHnd();

}

function datosModosOperacionRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);

    datosIngresadosRenderer(instance, td, row, col, prop, value, cellProperties, MOD);
}
function datosCalderoRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);

    datosIngresadosRenderer(instance, td, row, col, prop, value, cellProperties, CAL);
}

function datosInyeccionRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);

    datosIngresadosRenderer(instance, td, row, col, prop, value, cellProperties, INY);
}

function datosIngresadosRenderer(instance, td, row, col, prop, value, cellProperties, tipoInformacion) {

    var tipoInfo = "";
    var DATA;

    if (tipoInformacion == MOD) {
        tipoInfo = "Modo de operación";
        DATA = DATA_MOD;
    }
    if (tipoInformacion == CAL) {
        tipoInfo = "Calderos";
        DATA = DATA_CAL;
    }
    if (tipoInformacion == INY) {
        tipoInfo = "Inyección";
        DATA = DATA_INY;
    }

    var colNHoras = 3;

    var valCodigo = DATA.getDataAtRowProp(row, 'Rescfgcodi');

    if (valCodigo > 0) {
        if (col == colNHoras) {

            if (value != null && value != "") {

                //Valido si es NO numerico
                if (isNaN(value)) {
                    td.className = 'celda_error_roja htCenter';
                    agregarError(tipoInfo, valCodigo, value, "Solo se permiten valores numéricos (enteros positivos o negativos)");
                } else {

                    var numero = Number(value);
                    //valido si es decimal
                    if (!Number.isInteger(numero)) {
                        td.className = 'celda_error_roja htCenter';
                        agregarError(tipoInfo, valCodigo, value, "Solo está permitido el ingreso de valores enteros con máximo 3 cifras");
                    }
                    else {

                        if (numero < 0)
                            numero = numero * (-1);

                        var numPE = String(numero).length;
                        if (numPE > 3) {
                            td.className = 'celda_error_roja htCenter';
                            agregarError(tipoInfo, valCodigo, value, "Solo está permitido el ingreso de valores enteros con máximo 3 cifras");
                        }
                    }
                }
            }
        }
    }
}

function datosCombosModosOperacionRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.AutocompleteRenderer.apply(this, arguments);
}

function datosCombosCalderoRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.AutocompleteRenderer.apply(this, arguments);
}

function datosCombosInyeccionRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.AutocompleteRenderer.apply(this, arguments);
}

function precargarDetallesHnd() {

    if (tblHndModoOperacion != null) {
        tblHndModoOperacion.updateSettings({
            cells(row, col, prop) {
                const cellProperties = {};
                var colCondInicial = 2;
                var colNHoras = 3;

                var valCodigo = DATA_MOD.getDataAtRowProp(row, 'Rescfgcodi');

                switch (col) {
                    case colCondInicial:
                        cellProperties.renderer = "datosCombosModosOperacionRenderer";
                        break;
                    case colNHoras:
                        cellProperties.renderer = "datosModosOperacionRenderer"; // uses lookup map//
                        break;
                    default:
                }

                return cellProperties;
            },
        });
    }

    if (tblHndCaldero != null) {
        tblHndCaldero.updateSettings({
            cells(row, col, prop) {
                const cellProperties = {};

                var colCondInicial = 2;
                var colNHoras = 3;

                switch (col) {
                    case colCondInicial:
                        cellProperties.renderer = "datosCombosCalderoRenderer";
                        break;
                    case colNHoras:
                        cellProperties.renderer = "datosCalderoRenderer"; // uses lookup map//
                        break;
                    default:
                }

                return cellProperties;
            },
        });
    }

    if (tblHndInyeccion != null) {
        tblHndInyeccion.updateSettings({
            cells(row, col, prop) {
                const cellProperties = {};

                var colCondInicial = 2;
                var colNHoras = 3;

                switch (col) {
                    case colCondInicial:
                        cellProperties.renderer = "datosCombosInyeccionRenderer";
                        break;
                    case colNHoras:
                        cellProperties.renderer = "datosInyeccionRenderer"; // uses lookup map//
                        break;
                    default:
                }

                return cellProperties;
            },
        });
    }
}

function mostrarListadoErrores() {
    limpiarBarraMensaje("mensaje");

    setTimeout(function () {
        $('#tablaErrores').html(dibujarTablaErrores());

        $('#contenedorErrores').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
        $('#tablaErrorInd').dataTable({
            "scrollY": 330,
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });

    }, 200);
}

function cargarHansonTablas(datosEnBD) {

    listaDataGeneral = datosEnBD.DatosGneneral;
    listaRelaciones = datosEnBD.DatosRelaciones;

    //siempre debe haber al menos un modo de operación
    if (datosEnBD.DatosModoOperacion.length == 0) {
        datosEnBD.DatosModoOperacion = listaDataGeneral.filter(x => x.Restipcodi == MOD);
        datosEnBD.DatosCaldero = listaDataGeneral.filter(x => x.Restipcodi == CAL);
        datosEnBD.DatosInyeccion = listaDataGeneral.filter(x => x.Restipcodi == INY);
    }

    //Modo operación
    cargarDataAHanson(datosEnBD, MOD);

    //caldero
    cargarDataAHanson(datosEnBD, CAL);

    //inyección
    cargarDataAHanson(datosEnBD, INY);

}

function cargarDataAHanson(datosEnBd, tipoInfo) {
    var lstData = [];
    var datosHnd = [];
    var tablaHandson;

    if (tipoInfo == MOD) {
        tblHndModoOperacion.loadData(lstData);
        tablaHandson = tblHndModoOperacion;
        datosHnd = datosEnBd.DatosModoOperacion;
    }
    if (tipoInfo == CAL) {
        tblHndCaldero.loadData(lstData);
        tablaHandson = tblHndCaldero;
        datosHnd = datosEnBd.DatosCaldero;
    }
    if (tipoInfo == INY) {
        tblHndInyeccion.loadData(lstData);
        tablaHandson = tblHndInyeccion;
        datosHnd = datosEnBd.DatosInyeccion;
    }

    for (var index in datosHnd) {

        var item = datosHnd[index];

        var data = {
            Rescfgcodi: item.Rescfgcodi,
            Rescfgnombre: item.Rescfgnombre,
            Resdatflag: item.Resdatflag,
            Resdatdato: item.Resdatdato
        };

        lstData.push(data);
    }
    tablaHandson.loadData(lstData);
}

function limpiarBarraMensaje(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
}

function mostrarErrores() {
    $('#idTerrores').html(dibujarTablaError());
    setTimeout(function () {
        $('#validaciones').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
        $('#tablaError').dataTable({
            "scrollY": 330,
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });

    }, 50);
}

function dibujarTablaErrores() {
    var cadena = `
        <div style='clear:both; height:5px'></div>
            <table id='tablaErrorInd' border='1' class='pretty tabla-adicional' cellspacing='0' style='width: 850px;'>
                <thead>
                    <tr>
                        <th style='width: 80px;'>Tipo de información</th>
                        <th style='width: 120px;'>Código</th>
                        <th style='width: 250px;'>Valor</th>
                        <th style='width: 250px;'>Error</th>
                    </tr>
                </thead>
                <tbody>
    `;

    //ordeno el listado
    listErrores.sort(function (a, b) {
        return a.TipoInfo.localeCompare(b.TipoInfo);
    });


    for (var i = 0; i < listErrores.length; i++) {
        var item = listErrores[i];
        cadena += `
                    <tr>
                        <td style='width:  80px; text-align: center; white-space: break-spaces;'>${item.TipoInfo}</td>
                        <td style='width: 120px; text-align: left; white-space: break-spaces;'>${item.Codigo}</td>
                        <td style='width: 250px; text-align: left; white-space: break-spaces;'>${item.Valor}</td>
                        <td style='width: 250px; text-align: left; white-space: break-spaces;'>${item.Mensaje}</td>
                    </tr>
        `;
    }

    cadena += `
                </tbody>
            </table>
        </div>
    `;

    return cadena;
}

function agregarError(tipoInfo, codigo, valor, mensajeError) {
    //Agrega al array de tipo de informacion
    if (validarError(tipoInfo, codigo, valor, mensajeError)) {
        var regError = {
            TipoInfo: tipoInfo,
            Codigo: codigo,
            Valor: valor,
            Mensaje: mensajeError
        };

        if (tipoInfo === "Modo de operación")
            listaErroresMOD.push(regError);
        else {
            if (tipoInfo === "Calderos")
                listaErroresCAL.push(regError);
            else {
                listaErroresINY.push(regError);
            }
        }

        //listErrores.push(regError);
    }
}

function validarError(tipoInfo, codigo, valor, mensajeError) {
    var arrayData = [];
    //arrayData = listErrores.slice();

    if (tipoInfo === "Modo de operación")
        arrayData = listaErroresMOD.slice();
    else {
        if (tipoInfo === "Calderos")
            arrayData = listaErroresCAL.slice();
        else {
            arrayData = listaErroresINY.slice();
        }
    }

    for (var j in arrayData) {
        if (arrayData[j]['TipoInfo'] == tipoInfo && arrayData[j]['Codigo'] == codigo && arrayData[j]['Valor'] == valor && arrayData[j]['Mensaje'] == mensajeError) {
            return false;
        }
    }
    return true;
}

function enviarDatos() {
    $('#hfHorizonte').val($('#cbHorizonte').val());
    $('#hfFecha').val($('#txtFecha').val());
    $('#hfSemana').val($('#cbSemana').val());
    $('#hfAnho').val($('#Anho').val());
    semana = $("#hfSemana").val();
    anho = $("#hfAnho").val();
    semana = anho.toString() + semana;
    idHorizonte = $("#hfHorizonte").val();

    limpiarBarraMensaje("mensaje");

    if (listErrores.length == 0) {
        var dataGuardar = obtenerDataGuardar();
        var dataJson = {
            formato: idHorizonte,
            fecha: $('#hfFecha').val(),
            semana: semana,
            datosAGuardar: dataGuardar
        };

        if (confirm("¿Desea enviar información a COES?")) {
            $.ajax({
                url: controlador + "GuardarCondicionesIniciales",
                type: 'POST',
                contentType: 'application/json; charset=UTF-8',
                dataType: 'json',
                data: JSON.stringify(dataJson),
                success: function (evt) {
                    if (evt.Resultado == "1") {
                        alert("Los datos se enviaron correctamente");
                        //mostrarMensaje('mensaje', 'exito', 'La información fue registrada correctamente.');
                        cargarDatosCondicionInicial();
                    } else {
                        mostrarMensaje('mensaje', 'error', evt.Mensaje);
                    }
                },
                error: function () {
                    mostrarMensaje('mensaje', 'error', "No se ha podido realizar la acción");
                }
            });
        }
    }
    else
        mostrarListadoErrores();
}

function obtenerDataGuardar(dataHandson) {

    var dataHandsonMod, dataHandsonCal, dataHandsonIny = [];

    dataHandsonMod = tblHndModoOperacion.getSourceData();
    dataHandsonCal = tblHndCaldero.getSourceData();
    dataHandsonIny = tblHndInyeccion.getSourceData();

    var dataGuardar = {
        DatosModoOperacion: dataHandsonMod,
        DatosCaldero: dataHandsonCal,
        DatosInyeccion: dataHandsonIny
    };

    return dataGuardar;
}
function mostrarMensaje(id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

}

function horizonte() {

    var opcion = $('#cbHorizonte').val();
    switch (parseInt(opcion)) {
        case 1: //dia
        case 2: //dia
            $('#dDia').css("display", "block");
            $('#dSemana').css("display", "none");
            $('#selecFormato').css("display", "none");
            break;
        case 3: //semanal
            $('#dDia').css("display", "none");
            $('#dSemana').css("display", "block");
            $('#selecFormato').css("display", "none");
            break;

    }
}

function cargarSemanaAnho() {
    var anho = $('#Anho').val();
    $('#hfAnho').val(anho);
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarSemanas',

        data: { idAnho: $('#hfAnho').val() },

        success: function (aData) {
            $('#SemanaIni').html(aData);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });

}