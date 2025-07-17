var controlador = siteRoot + "Siosein/TablasPrieDeclaracionMen/";

$(function () {
    $("#btnVolverListado").click(function () {
        var periodo = $("#txtFecha").val();
        window.location.href = siteRoot + "Siosein/RemisionesOsinergmin/ListaTablasPrie?periodo=" + periodo;
    });

    mostrarLeyendaPrie();
});

/**
 * Permite obtener Object Array de un DataTable
 * @param {any} data DataTable rows data
 * @returns {any} array
 */
function GetDataDataTable(data) {
    var dataList = [];

    $.each(data, function (index, value) {
        dataList.push(value);
    });

    return dataList;
}

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

// #region SIOSEIN-PRIE-2021
/**
 * Invoca al controlador que realiza la exportación de los datos a un archivo excel
 * @param listaExcelHoja : objeto ListaExcelHoja que contiene todo el contenido de los datos a exportar
 * @param nombreArchivo : nombre del archivo a exportar, pero sin la extensión
 */
function ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo)
{
    $.ajax({
        type: 'POST',
        url: siteRoot + 'Siosein/TablasPrieDeclaracionMen/ExportaraExcel',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            listaExcelHoja: listaExcelHoja,
            nombreArchivo: nombreArchivo
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result == "-1") {
                alert("Ha ocurrido un error inesperado.");
            }
            else {
                window.location = controlador + 'abrirarchivo?tipo=' + 1 + '&nombreArchivo=' + result;
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}


/**
 * Invoca al controlador que realiza la exportación de los datos a un archivo de texto plano
 * @param listaRegistros : objeto listaRegistros que contiene todo el contenido de los datos a exportar
 * @param nombreArchivo : nombre del archivo a exportar, pero sin la extensión
 */
function ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo) {
    $.ajax({
        type: 'POST',
        url: siteRoot + 'Siosein/TablasPrieDeclaracionMen/ExportaraTexto',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            tpriecodi: tpriecodi,
            periodo: periodo,
            nombreArchivo: nombreArchivo
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if(result == "0") {
                alert("No existen registros a exportar. Por favor, ejecute primero la validación.");
            }
            else if (result == "-1") {
                alert("Ha ocurrido un error inesperado.");
            }
            else {
                window.location = controlador + 'abrirarchivo?tipo=' + 1 + '&nombreArchivo=' + result;
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}


/**
 * Prepara el contenido de una hoja excel para ser exportada
 * @param objeto: objeto que contiene los atributos a ser utilizados en la función
 * @returns {excelHoja}: un objeto excelHoja con todas los atributos ya seteados para ser utilizados en la exportación de datos
 */
function generarExcelHoja(objeto) {

    //Keys del parámetro objeto
    var idTabla = objeto["idTabla"];
    var datosTabla = objeto["datosTabla"];
    var titulo = objeto["titulo"];
    var nombreHoja = objeto["nombreHoja"];
    var defaultColumnaAtributos = objeto["defaultColumnaAtributos"];
    var listaColumnaAtributos = objeto["listaColumnaAtributos"];
    //

    var numCols = $(idTabla).dataTable().fnSettings().aoColumns.length;
    var periodo = $("#txtFecha").val();
    var mes = periodo.split(" ")[0];
    var anio = periodo.split(" ")[1];
    var titulo = titulo + " - Periodo " + ObtenerNombreMes(mes) + " " + anio;

    //Llenado de cabeceras
    var listaCabeceras = crearListaModelos(idTabla + " thead tr", "th", listaColumnaAtributos);

    //Llenado Ancho, Alineamiento y Tipo de Columnas
    var listaTipo = [];
    var listaAnchoColumna = [];
    var listaAlineaHorizontal = [];
    for (var col = 0; col < numCols; col++) {
        var esOmitido = omitir(col, listaColumnaAtributos);
        if (!(esOmitido)) {
            var ancho = (defaultColumnaAtributos.ancho != null) ? defaultColumnaAtributos.ancho : 50;
            var alinea = (defaultColumnaAtributos.alinea != null && defaultColumnaAtributos.alinea != "") ? defaultColumnaAtributos.alinea : "right";
            var tipo = (defaultColumnaAtributos.tipo != null && defaultColumnaAtributos.tipo != "") ? defaultColumnaAtributos.tipo : "string";
            for (var j = 0; j < listaColumnaAtributos.length; j++) {
                var columnaAtributos = listaColumnaAtributos[j];
                if (col == columnaAtributos.col) {
                    if (columnaAtributos.ancho != null && columnaAtributos.ancho != "") {
                        ancho = columnaAtributos.ancho;
                    }
                    if (columnaAtributos.alinea != null && columnaAtributos.alinea != "") {
                        alinea = columnaAtributos.alinea;
                    }
                    if (columnaAtributos.tipo != null && columnaAtributos.tipo != "") {
                        tipo = columnaAtributos.tipo;
                    }
                    break;
                }
            }
            listaAnchoColumna.push(ancho);
            listaAlineaHorizontal.push(alinea);
            listaTipo.push(tipo);
        }
    }

    //Llenado de registros
    var listaRegistros = [];
    for (var i = 0; i < datosTabla.length; i++) {
        var fila = [];
        for (var col = 0; col < numCols; col++) {
            var esOmitido = omitir(col, listaColumnaAtributos);
            if (!(esOmitido)) {
                fila.push(datosTabla[i][col]);
            }
        }
        listaRegistros.push(fila);
    }

    //Armado de cuerpo
    var cuerpo = {
        'ListaRegistros': listaRegistros,
        'ListaAlineaHorizontal': listaAlineaHorizontal,
        'ListaTipo': listaTipo
    };

    //Llenado de pies
    var listaPies = crearListaModelos(idTabla + " tfoot tr", "td", listaColumnaAtributos);

    //Llenado de la hoja excel
    var excelHoja = {
        'NombreHoja': nombreHoja,
        'Titulo': titulo,
        'ListaCabeceras': listaCabeceras,
        'ListaAnchoColumna': listaAnchoColumna,
        'Cuerpo': cuerpo,
        'ListaPies': listaPies
    };

    return excelHoja;
}


/**
 * Crea un objeto Modelo (que puede ser una cabecera o pie).
 * @param v: contiene una columna de una fila de una tabla HTML
 * @returns {any}: Devuelve las propiedades para el objeto Modelo
 */
function crearModelo(v) {
    var nombre = v.text();
    var contador = nombre.split("  ").length - 1;
    for (var i = 0; i <= contador; i++)
    {
        nombre = nombre.replace("  ", "\n");
    }
    var numColumnas = cantidadSpan(v.attr("colspan"));
    var numFilas = cantidadSpan(v.attr("rowspan"));
    var alineaHorizontal = alinea(v.css("text-align"));
    return { 'Nombre': nombre, 'NumColumnas': numColumnas, 'NumFilas': numFilas, 'AlineaHorizontal': alineaHorizontal };
}


/**
 * Devuelve un número en base al valor ingresado en el parámetro.
 * @param h: contiene un valor numérico
 * @returns {any}: Devuelve un número. En caso el parámetro v no esté definido, devuelve por default 1
 */
function cantidadSpan(v) {
    if (typeof v === "undefined") {
        return 1;
    } else {
        return parseInt(v);
    }
}


/**
 * Devuelve un alineamiento horizontal.
 * @param v: contiene un valor de alineamiento horizontal
 * @returns {any}: Devuelve un valor de alineamiento horizontal. En caso el parámetro v no esté definido, devuelve por default "right"
 */
function alinea(v) {
    if (typeof v === "undefined") {
        return "right";
    } else {
        return v;
    }
}


/**
 * Determinar si una columna debe ser omitida o no mostrada en la exportación de los datos
 * @param col: es el índice de la columna a evaluar 
 * @param listaColumnaAtributos: es la lista de atributos de las columnas
 * @returns {any}: Devuelve: true = si debe ser omitido; false = no debe ser omitido
 */
function omitir(col, listaColumnaAtributos) {
    var esOmitido = false;
    for (var j = 0; j < listaColumnaAtributos.length; j++) {
        esOmitido = false;
        var columnaAtributos = listaColumnaAtributos[j];
        if (col == columnaAtributos.col) {
            if (columnaAtributos.omitir != null && columnaAtributos.omitir == "si") {
                esOmitido = true;
                break;
            }
        }
    }
    return esOmitido;
}


/**
 * Crea un array de lista de objetos Modelo (que puede ser una cabecera o pie)
 * @param id: elemento del HTML a leer
 * @param tipoColumna: tipo de columna del HTML a leer
 * @returns {any}: Devuelve un array de lista de objetos Modelo (que puede ser una cabecera o pie)
 */
function crearListaModelos(id, tipoColumna, listaColumnaAtributos) {
    var listaModelos = [];
    $(id).each(function () {
        var modelos = [];
        var totalColumnas = 0;
        $(this).find(tipoColumna).each(function () {
            totalColumnas = totalColumnas + cantidadSpan($(this).attr("colspan"))
            var esOmitido = omitir(totalColumnas - 1, listaColumnaAtributos);
            if (!(esOmitido)) {
                modelos.push(crearModelo($(this)));
            }
        });
        listaModelos.push(modelos);
    });
    return listaModelos;
}

/**
 * Crea un nombre de archivo con formato para tabla prie
 * @param nombreArchivo: nombre de archivo base
 * @param periodo: periodo (MM YYYY)
 * @returns {any}: Devuelve un nombre de archivo con formato para tabla prie
 */
function nombreArchivoTablaPrie(nombreArchivo, periodo) {
    if (periodo != null &&
        periodo.split(" ").length > 1 &&
        periodo.split(" ")[1].substring(2) != null) {
        nombreArchivo = nombreArchivo + periodo.split(" ")[1].substring(2) + periodo.split(" ")[0];
    }
    return nombreArchivo;
}

/**
 * Obtiene el nombre del mes según el número del mes ingresado
 * @param numMes: número del mes
 * @returns {any}: Devuelve el nombre del mes
 */
function ObtenerNombreMes(numMes) {
    var meses = ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'];
    return meses[numMes - 1];
}
//#endregion

function mostrarLeyendaPrie() {
    var tpriecodi = parseInt($("#hdTpriecodi").val()) || 0;

    var htmlTexto = getTextoLeyenda(tpriecodi);
    $("#leyenda_prie").html(htmlTexto);

    $("#leyenda_prie").show();
}

function abrirVentanaAplicativo(urlAplicativo) {
    var url = siteRoot + urlAplicativo;
    window.open(url, '_blank').focus();
}

// #region Leyenda Formato txt

function getTextoLeyenda(tpriecodi) {
    var htmlAplicativo = '';
    var urlAplicativo = '';
    var urlAplicativoConfiguracion = '';

    switch (tpriecodi) {
        case 1: //1	POTENCIA FIRME
        case 3: //3	COSTOS MARGINALES
        case 7: //7	TRANSFERENCIAS DE ENERGIA
        case 8: //8	PAGOS POR VALOR DE TRANSFERENCIAS DE POTENCIA
        case 9: //9	BALANCE POR EMPRESAS
        case 10: //10	PAGOS POR VALOR DE TRANSFERENCIAS DE ENERGÍA
        case 11: //11	COMPENSACION INGRESO TARIFARIO
        case 12: //12	COMPENSACION POR TRANSMISIÓN PCSPT Y PCSGT
        case 13: //13	RECAUDACIÓN POR PEAJES CALCULADOS POR CONEX/TRANS
            htmlAplicativo = 'Transferencias';
            break;

        case 2://2	POTENCIA EFECTIVA
            htmlAplicativo = 'Potencia Firme';
            break;

        case 4://4	COSTOS VARIABLES
            htmlAplicativo = 'Costos Variables';
            break;

        case 5://5	PRODUCCIÓN DE ENERGÍA
        case 6://6	DESVIACIONES
            htmlAplicativo = 'Medidores de Generación';
            break;

        case 15://15	HORAS DE OPERACIÓN
            htmlAplicativo = 'Horas de Operación';
            break;
        case 16://16	ENERGIA NO SUMINISTRADA EJECUTADA MENSUAL
            htmlAplicativo = 'Eventos';
            break;
        case 17://17	FLUJOS DE INTERCONEXIÓN EJECUTADO
            htmlAplicativo = 'Intercambios Internacionales';
            break;

        case 18: //18	CAUDALES EJECUTADOS DIARIOS
        case 19: //19	VOLUMEN DE RESERVORIOS EJECUTADOS DIARIO
        case 20: //20	VOLUMEN LAGOS
        case 21: //21	VOLUMEN EMBALSES
        case 22: //22	HIDROLOGIA CUENCAS
            htmlAplicativo = 'Hidrología';
            break;

        case 23://23	VOLUMEN DE COMBUSTIBLE
            htmlAplicativo = 'Consumo de Combustible';
            break;
        case 24://24	HECHOS RELEVANTES
            htmlAplicativo = 'Mantenimientos';
            break;
        case 25://25	NUEVA INSTALACION, REPOTENCIACION Y/O RETIRO
            htmlAplicativo = 'Equipamientos';
            break;

        case 14: //14	COSTOS DE OPERACIÓN EJECUTADOS
        case 31: //31	COSTOS DE OPERACIÓN PROGRAMADO SEMANAL
        case 34: //34	COSTOS DE OPERACIÓN PROGRAMADO DIARIO
        case 30: //30	PROGRAMA DE OPERACIÓN SEMANAL
        case 33: //33	PROGRAMA DE OPERACIÓN DIARIO
        case 32: //32	COSTOS MARGINALES PROGRAMADO SEMANAL
        case 35: //35	COSTOS MARGINALES PROGRAMADO DIARIO
            htmlAplicativo = 'Cdisptach';
            break;

        case 26: //26	PROGRAMA DE OPERACIÓN MENSUAL
        case 27: //27	COSTOS MARGINALES PROGRAMADO MENSUAL
        case 28: //28	COSTOS DE OPERACIÓN PROGRAMADO MENSUAL
        case 29: //29	RESULTADOS EMBALSES ESTACIONALES PROGRAM MENSUAL
            htmlAplicativo = 'PMPO';
            break;
    }

    switch (tpriecodi) {
        case 1: //1	POTENCIA FIRME
            urlAplicativo = 'transfpotencia/ingresopotefr/index';
            break;
        case 3: //3	COSTOS MARGINALES
            urlAplicativo = 'transferencias/factorperdida/index/';
            break;
        case 7: //7	TRANSFERENCIAS DE ENERGIA
            urlAplicativo = 'transferencias/valortransferencia/index/#paso2';
            break;
        case 8: //8	PAGOS POR VALOR DE TRANSFERENCIAS DE POTENCIA
            urlAplicativo = 'transfpotencia/valortransfpc/index#paso2';
            break;
        case 9: //9	BALANCE POR EMPRESAS
            urlAplicativo = 'transferencias/valortransferencia/index/#paso3';
            break;
        case 10: //10	PAGOS POR VALOR DE TRANSFERENCIAS DE ENERGÍA
            urlAplicativo = 'transferencias/valortransferencia/index/#paso4';
            break;
        case 11: //11	COMPENSACION INGRESO TARIFARIO
            urlAplicativo = 'transfpotencia/valortransfpc/index/';
            break;
        case 12: //12	COMPENSACION POR TRANSMISIÓN PCSPT Y PCSGT
            urlAplicativo = 'transfpotencia/valortransfpc/index/';
            break;
        case 13: //13	RECAUDACIÓN POR PEAJES CALCULADOS POR CONEX/TRANS
            urlAplicativo = 'transfpotencia/valortransfpc/index/';
            break;

        case 2://2	POTENCIA EFECTIVA
            urlAplicativo = 'PotenciaFirme/General';
            break;

        case 4://4	COSTOS VARIABLES
            urlAplicativo = 'Despacho/CostosVariables/Index/';
            break;

        case 5://5	PRODUCCIÓN DE ENERGÍA
            urlAplicativo = 'Mediciones/MedidoresGeneracion/index/';
            break;
        case 6://6	DESVIACIONES
            urlAplicativo = 'Mediciones/MedidoresGeneracion/index/';
            break;

        case 15://15	HORAS DE OPERACIÓN
            urlAplicativo = 'IEOD/HorasOperacion/Reporte';
            break;
        case 16://16	ENERGIA NO SUMINISTRADA EJECUTADA MENSUAL
            urlAplicativo = 'eventos/evento/index/';
            break;
        case 17://17	FLUJOS DE INTERCONEXIÓN EJECUTADO
            urlAplicativo = 'Interconexiones/Reportes/IndexHistorico/';
            break;

        case 18: //18	CAUDALES EJECUTADOS DIARIOS
        case 19: //19	VOLUMEN DE RESERVORIOS EJECUTADOS DIARIO
        case 20: //20	VOLUMEN LAGOS
        case 21: //21	VOLUMEN EMBALSES
        case 22: //22	HIDROLOGIA CUENCAS
            urlAplicativo = 'Hidrologia/Reporte/Index/';
            break;

        case 23://23	VOLUMEN DE COMBUSTIBLE
            urlAplicativo = 'StockCombustibles/Reportes/Consumo/';
            break;
        case 24://24	HECHOS RELEVANTES
            urlAplicativo = 'eventos/mantenimiento/index/';
            break;
        case 25://25	NUEVA INSTALACION, REPOTENCIACION Y/O RETIRO
            urlAplicativo = 'Equipamiento/equipo/';
            break;

        case 14: //14	COSTOS DE OPERACIÓN EJECUTADOS
        case 31: //31	COSTOS DE OPERACIÓN PROGRAMADO SEMANAL
        case 34: //34	COSTOS DE OPERACIÓN PROGRAMADO DIARIO
        case 30: //30	PROGRAMA DE OPERACIÓN SEMANAL
        case 33: //33	PROGRAMA DE OPERACIÓN DIARIO
        case 32: //32	COSTOS MARGINALES PROGRAMADO SEMANAL
        case 35: //35	COSTOS MARGINALES PROGRAMADO DIARIO
            urlAplicativo = 'Migraciones/Despacho/Index/';
            break;

        case 26: //26	PROGRAMA DE OPERACIÓN MENSUAL
        case 27: //27	COSTOS MARGINALES PROGRAMADO MENSUAL
        case 28: //28	COSTOS DE OPERACIÓN PROGRAMADO MENSUAL
        case 29: //29	RESULTADOS EMBALSES ESTACIONALES PROGRAM MENSUAL
            urlAplicativo = 'PMPO/ProcesamientoResultadosSDDP/ProcesarResultados/';
            break;
    }

    switch (tpriecodi) {
        case 18: //18	CAUDALES EJECUTADOS DIARIOS
            urlAplicativoConfiguracion = 'ReportesMedicion/formatoreporte/IndexDetalle?id=63';
            break;
        case 19: //19	VOLUMEN DE RESERVORIOS EJECUTADOS DIARIO
            urlAplicativoConfiguracion = 'ReportesMedicion/formatoreporte/IndexDetalle?id=68';
            break;
        case 20: //20	VOLUMEN LAGOS
            urlAplicativoConfiguracion = 'ReportesMedicion/formatoreporte/IndexDetalle?id=65';
            break;
        case 21: //21	VOLUMEN EMBALSES
            urlAplicativoConfiguracion = 'ReportesMedicion/formatoreporte/IndexDetalle?id=64';
            break;
        case 22: //22	HIDROLOGIA CUENCAS
            urlAplicativoConfiguracion = 'ReportesMedicion/formatoreporte/IndexDetalle?id=63';
            break;
    }

    var htmlTexto = '';
    htmlTexto+= `
        Los datos son obtenidos del aplicativo <b><span style="color: blue;">${htmlAplicativo}<span></b>. <input type="button" value="Ir a aplicativo" onclick="abrirVentanaAplicativo('${urlAplicativo}');">
    `;
    if (urlAplicativoConfiguracion != '') {
        htmlTexto += `
        <br/>
        La configuración de los puntos de medición se encuentra en el Generador de Reporte PR5. <input type="button" value="Ir a aplicativo" onclick="abrirVentanaAplicativo('${urlAplicativoConfiguracion}');">
    `;
    }

    return htmlTexto;
}

//#endregion


// #region Historial
function abrirVentanaHistorialVerificacion(titulo, tpriecodi) {

    $("#popupHistorial .popup-title span").html("Historial de Verificación");
    $('#div_historial').html('');

    $.ajax({
        type: 'POST',
        url: controlador + "ListarHistorialVerificacion",
        data: {
            periodo: $("#txtFecha").val(),
            tpriecodi: tpriecodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                var html = dibujarTablaHistorialVerificacion(evt.ListaHistorialVerificacion);
                $('#div_historial').html(html);

                abrirPopup("popupHistorial");
            } else {

                alert('Ha ocurrido un error al listar las versiones :' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function dibujarTablaHistorialVerificacion(lista) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" width="100%" id="tabla_historial">
        <thead>
            <tr>
                <th>Versión</th>
                <th>Estado</th>
                <th>Fecha</th>
                <th>Usuario</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];

        cadena += `
            <tr class='id_${item.Cabpricodi}'>
                <td style="height: 24px">${item.Cabpriversion}</td>
                <td style="height: 24px">${item.EstadoDesc}</td>
                <td style="height: 24px">${item.CabprifeccreacionDesc}</td>
                <td style="height: 24px">${item.Cabpriusucreacion}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function abrirPopup(contentPopup) {
    setTimeout(function () {
        $("#" + contentPopup).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}

//#endregion