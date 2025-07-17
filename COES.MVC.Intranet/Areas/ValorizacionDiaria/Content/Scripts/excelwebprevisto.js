var iFormatCodi = 129;//VTD - ENERGIA PREVISTA A ENTREGAR MENSUAL, Registro de la tabla ME_FORMATO
//Intranet
$(function () {
    inicializaPagina();
    /// Crea Objeto Pupload
    crearPupload();
});

function crearHojaWeb() {
    // Crea Objeto Handson,previamente inicializar evtHot
    getModelFormato(1);

}

function inicializaPagina() {
    $('#txtFecha').Zebra_DatePicker({
        direction: 0,
        onSelect: function () {
        }
    });

    $('#txtMes').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#btnEditarEnvio').click(function () {
        nuevoFormato(false);
    });

    $('#btnDescargarFormato').click(function () {
        descargarFormato();
    });

    $('#btnEnviarDatos').click(function () {

        if (formato == tipo1) {
            enviarExcelWeb();
        }
        if (formato == tipo2) {
            grabar();
        }
    });

    $('#btnVerEnvios').click(function () {
        popUpListaEnvios();
    });

    $('#btnMostrarErrores').click(function () {
        mostrarDetalleErrores();
    });

    $('#btnExpandirRestaurar').click(function () {
        if ($('#hfExpandirContraer').val() == "E") {
            expandirExcel();
            calculateSize2(1);
            $('#hfExpandirContraer').val("C");
            $('#spanExpandirContraer').text('Restaurar');

            var img = $('#imgExpandirContraer').attr('src');
            var newImg = img.replace('expandir.png', 'contraer.png');
            $('#imgExpandirContraer').attr('src', newImg);

        }
        else {
            restaurarExcel();
            calculateSize2(2);
            $('#hfExpandirContraer').val("E");
            $('#spanExpandirContraer').text('Expandir');

            var img = $('#imgExpandirContraer').attr('src');
            var newImg = img.replace('contraer.png', 'expandir.png');
            $('#imgExpandirContraer').attr('src', newImg);

        }
    });

    $('#btnAgregarFila').click(function () {
        $("#btnAceptarEmbalse").prop('disabled', false);
        abrirPopupEmbalse();
    });
    $("#btnAceptarEmbalse").click(function () {
        $("#btnAceptarEmbalse").prop('disabled', true);
        agregarFilaHandson();
        $('#popupEmbalses').bPopup().close();
    });
    $("#btnCerrarEmbalse").click(function () {
        $('#popupEmbalses').bPopup().close();
    });

    //Entrega 
    $('#btnEditarEnvioE').click(function () {
        nuevoFormatoE(false);
    });

    $('#btnDescargarFormatoE').click(function () {
        descargarFormato();
    });

    $('#btnEnviarDatosE').click(function () {

        if (formato == tipo1) {
            enviarExcelWebE();
        }
        if (formato == tipo2) {
            grabar2();
        }
    });

    $('#btnVerEnviosE').click(function () {
        popUpListaEnvios();
    });

    $('#btnMostrarErroresE').click(function () {
        mostrarDetalleErrores();
    });

    $('#btnExpandirRestaurarE').click(function () {
        if ($('#hfExpandirContraer').val() == "E") {
            expandirExcel();
            calculateSize2(1);
            $('#hfExpandirContraer').val("C");
            $('#spanExpandirContraerE').text('Restaurar');

            var img = $('#imgExpandirContraerE').attr('src');
            var newImg = img.replace('expandir.png', 'contraer.png');
            $('#imgExpandirContraerE').attr('src', newImg);

        }
        else {
            restaurarExcel();
            calculateSize2(2);
            $('#hfExpandirContraer').val("E");
            $('#spanExpandirContraerE').text('Expandir');

            var img = $('#imgExpandirContraerE').attr('src');
            var newImg = img.replace('contraer.png', 'expandir.png');
            $('#imgExpandirContraerE').attr('src', newImg);

        }
    });

    $('#btnAgregarFilaE').click(function () {
        $("#btnAceptarEmbalse").prop('disabled', false);
        abrirPopupEmbalse();
    });




    validaInicial = true;
    listErrores.splice(0, listErrores.length);
    var strFormatCodi = $('#hfFormatCodi').val();
    var strFormatPeriodo = $('#hfFormatPeriodo').val();
    $('#cbEmpresa').val($("#hfIdEmpresa").val());
    editable = $('#hfEditable').val();
}



function getModelFormato(accion) {
    $('#hfVersion').val($('#cbVersion').val());
    $('#hfHorizonte').val($('#cbHorizonte').val());
    $('#hfFecha').val($('#txtFecha').val());
    $('#hfSemana').val($('#cbSemana').val());
    $('#hfAnho').val($('#Anho').val());

    idEmpresa = $("#hfIdEmpresa").val();
    idFormato = $("#hfIdFormato").val();
    idEnvio = $("#hfIdEnvio").val();
    fecha = $("#hfFecha").val();
    fechaDia = $('#hfFechaDia').val();
    semana = "";
    mes = $("#txtMes").val();
    anho = "";
    semana = "";
    limpiarError();
    hideMensajeEvento();
    var inputVerUltimoEnvio = 0;
    VER_ULTIMO_ENVIO = false;

    $.ajax({
        type: 'POST',
        url: controlador + "MostrarHojaExcelWeb",
        dataType: 'json',
        data: {
            idEmpresa: idEmpresa,
            idFormato: idFormato,
            idEnvio: idEnvio,
            fecha: fecha,
            semana: semana,
            mes: mes,
            verUltimoEnvio: inputVerUltimoEnvio
        },
        success: function (evt) {
            console.log(evt);
            var habilitarEditar = false;
            var evtlastEnvio = parseInt(evt.IdEnvioLast) || 0;
            if (evtlastEnvio > 0 && VER_ULTIMO_ENVIO) {
                $("#hfIdEnvio").val(evtlastEnvio);
                idEnvio = evtlastEnvio;
                habilitarEditar = true;
            }

            if (VER_ENVIO) {
                habilitarEditar = true;
            }

            if (!evt.EsEmpresaVigente) {
                habilitarEditar = false;
            }

            evtHot = evt;
            switch (parseInt(idFormato)) {
                case formatoVertim:
                case formatoDescarg:
                    formato = tipo2;
                    crearGrillaFormatoTipo2(evt);
                    $('#celdaAgregar').css("display", "block");
                    $('#celdaDescargar').css("display", "none");
                    $('#celdaImportar').css("display", "none");
                    break;
                default:
                    formato = tipo1;
                    $('#celdaAgregar').css("display", "none");
                    $('#celdaDescargar').css("display", "block");
                    $('#celdaImportar').css("display", "block");
                    crearGrillaFormatoTipo1(evt, habilitarEditar);

                    break;
            }
            dibujaBarra(accion, evt.Handson.ReadOnly, habilitarEditar);
            switch (accion) {
                case 1:
                    mostrardetalleExcel(evt.Formato.Areaname, evt.Empresa, evt.Formato.Formatnombre, evt.Anho, evt.Mes, evt.Semana, evt.Dia, evt.Formato.Formatperiodo)
                    break;
                case 2:
                    mostrarExito("Los datos se enviaron correctamente");
                    break;
                case 4:
                    mostrarExito("Ya puede consultar el envío anterior");
                    break;
                case 5:
                    mostrarExito("<strong>Por favor presione el botón enviar para grabar los datos.</strong>");
                    break;
            }

            VER_ULTIMO_ENVIO = false;
            VER_ENVIO = false;
        },
        error: function () {
            alert("Error al cargar Excel Web");
            //evtHot = null;
        }
    });

}
/// Muestra los envios anteriores
function dibujarTablaEnvios(lista) {

    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablalenvio' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Id Envío</th><th>Fecha Hora</th><th>Usuario</th></tr></thead>";
    cadena += "<tbody>";

    for (key in lista) {
        var javaScriptDate = new Date(parseInt(lista[key].Enviofecha.substr(6)));
        cadena += "<tr onclick='mostrarEnvioExcelWeb(" + lista[key].Enviocodi + ");' style='cursor:pointer'><td>" + lista[key].Enviocodi + "</td>";
        cadena += "<td>" + getFormattedDate(javaScriptDate) + "</td>";
        cadena += "<td>" + lista[key].Lastuser + "</td></tr>";
    }
    cadena += "</tbody></table>";
    return cadena;
}

function dibujarTablaEnviosE(lista) {

    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablalenvio' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Id Envío</th><th>Fecha Hora</th><th>Usuario</th></tr></thead>";
    cadena += "<tbody>";

    for (key in lista) {
        var javaScriptDate = new Date(parseInt(lista[key].Enviofecha.substr(6)));
        cadena += "<tr onclick='mostrarEnvioExcelWebE(" + lista[key].Enviocodi + ");' style='cursor:pointer'><td>" + lista[key].Enviocodi + "</td>";
        cadena += "<td>" + getFormattedDate(javaScriptDate) + "</td>";
        cadena += "<td>" + lista[key].Lastuser + "</td></tr>";
    }
    cadena += "</tbody></table>";
    return cadena;
}

///Visualiza Div del Titulo del FormatonuevoFormato
function mostrardetalleExcel(areaNombre, empresaNombre, formatoNombre, anho, mes, semana, dia, periodo) {
    var area = areaNombre != null && areaNombre != undefined ? areaNombre : '';
    var empresa = empresaNombre != null && empresaNombre != undefined ? empresaNombre : '';
    var formato = formatoNombre != null && formatoNombre != undefined ? formatoNombre : '';
    anho = anho != null && anho != undefined ? anho : '';
    mes = mes != null && mes != undefined ? mes : '';
    semana = semana != null && semana != undefined ? semana : '';
    dia = dia != null && dia != undefined ? dia : '';
    var area = "<table><tr><td > <strong><font style='color:SteelBlue;'>&nbsp;&nbsp;&nbsp;Formato Seleccionado:</strong>&nbsp;&nbsp;&nbsp;" + formato + "</font></td></tr>";
    area += "<tr><td> <strong><font style='color:SteelBlue;'>&nbsp;&nbsp;&nbsp;Empresa:</strong>&nbsp;&nbsp;&nbsp;" + empresa + "</font><strong><font style='color:SteelBlue;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Año:</strong>&nbsp;&nbsp; " + anho + "</font>";
    switch (periodo) {
        case 1:
            area += "<strong><font style='color:SteelBlue;'>&nbsp;&nbsp;&nbsp;Mes:&nbsp;&nbsp;&nbsp;</strong>" + mes + " </font><font style='color:SteelBlue;'>&nbsp;&nbsp;&nbsp;<strong>Día:</strong>&nbsp;&nbsp; " + dia + "</font>";
            break;
        case 2:
            area += "<strong><font style='color:SteelBlue;'>&nbsp;&nbsp;&nbsp;Semana:</strong>&nbsp;&nbsp;&nbsp; " + semana + "</font>";
            break;
        case 3: case 5:
            area += "<strong><font style='color:SteelBlue;'&nbsp;&nbsp;&nbsp;>Mes:</strong>&nbsp;&nbsp;&nbsp; " + mes + "</font>";
            break;

    }
    area += "</td></tr></table>"
    $("#idArea").html(area);
}

function cargarDataExcelWeb(accion) {
    if (typeof hot != 'undefined') {
        hot.destroy();
    }


    getModelFormato(accion);
}

function horizonte() {
    var opcion = buscarPeriodo($('#cbFormato').val());
    switch (parseInt(opcion)) {
        case 1: //dia
            $('#cntFecha').css("display", "block");
            $('#cntSemana').css("display", "none");
            $('#fechasSemana').css("display", "none");
            $('#cntMes').css("display", "none");
            $('#cntFecha2').css("display", "block");
            $('#cntSemana2').css("display", "none");
            $('#fechasSemana2').css("display", "none");
            $('#cntMes2').css("display", "none");
            break;
        case 2: //semanal
            $('#cntFecha').css("display", "none");
            $('#cntSemana').css("display", "block");
            $('#fechasSemana').css("display", "block");
            $('#cntMes').css("display", "none");
            $('#cntFecha2').css("display", "none");
            $('#cntSemana2').css("display", "block");
            $('#fechasSemana2').css("display", "block");
            $('#cntMes2').css("display", "none");
            break;
        //mensual

        //break;
        case 3: case 5:
            $('#cntFecha').css("display", "none");
            $('#cntSemana').css("display", "none");
            $('#fechasSemana').css("display", "none");
            $('#cntMes').css("display", "block");
            $('#cntFecha2').css("display", "none");
            $('#cntSemana2').css("display", "none");
            $('#fechasSemana2').css("display", "none");
            $('#cntMes2').css("display", "block");
            break;
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'SetearFechasEnvio',
        dataType: 'json',
        data: {
            idFormato: $('#cbFormato').val()
        },
        success: function (result) {
            $('#txtMes').val(result.mes);
            $('#txtFecha').val(result.fecha);
            $('#cbSemana').val(result.semana);
            $('#Anho').val(result.anho);
        },
        error: function () {
            alert("Error");
        }
    });

}

function buscarPeriodo(valor) {// valor: formatCodi
    for (var i = 0; i < listFormatCodi.length; i++)
        if (listFormatCodi[i] == valor) return listFormatPeriodo[i];
}

function nuevoFormato(verUltimoEnvio) {

    VER_ULTIMO_ENVIO = verUltimoEnvio;
    $('#btnEditarEnvio').parent().css("display", "none");

    if (($("#cbFormato").val() != "0") && ($("#cbFormato").val() != "")) {
        $("#hfIdEmpresa").val($("#cbEmpresa").val());
        $("#hfIdEnvio").val(0);
        $("#hfFecha").val($("#txtFecha").val());
        $("#hfSemana").val($("#cbSemana").val());
        $("#hfMes").val($("#txtMes").val());

        cargarDataExcelWeb(1);

        $('#barraHidro').css("display", "block");
        mostrardetalleExcel();
    }
    else {
        alert("Error!.Seleccionar Formato");
    }
}

function actualizaDataExcel() {
    hot.loadData(evtHot.Handson.ListaExcelData);
}

function mostrarEnvioExcelWeb(envio) {
    $('#enviosanteriores').bPopup().close();
    $("#hfIdEnvio").val(envio);
    VER_ENVIO = true;
    cargarDataExcelWeb(4);

}

function mostrarEnvioExcelWebE(envio) {
    $('#enviosanteriores').bPopup().close();
    $("#hfIdEnvio").val(envio);
    VER_ENVIO = true;
    cargarDataExcelWebE(4);

}

function validarEnvio() {
    retorno = true;
    totalErrores = listErrores.length;
    getTotalErrores();
    if ((totalErrores) > 0) {
        mostrarError("Existen errores en las celdas, favor corregir y vuelva a envíar");
        mostrarDetalleErrores();
        retorno = false;
    }
    var data
    let dataTabActivo = $("ul.etabs li.active").data("tabindex");
    if (dataTabActivo == "0")
        data = hot.getData;
    else
        data = hotE.getData;
    var msjValid = validarConfigPlazoPto();
    if (retorno && msjValid != '')
        alert(msjValid);

    return retorno;
}

function formatFloat(num, casasDec, sepDecimal, sepMilhar) {

    if (num < 0) {
        num = -num;
        sinal = -1;
    } else
        sinal = 1;
    var resposta = "";
    var part = "";
    if (num != Math.floor(num)) // decimal values present
    {
        part = Math.round((num - Math.floor(num)) * Math.pow(10, casasDec)).toString(); // transforms decimal part into integer (rounded)
        while (part.length < casasDec)
            part = '0' + part;
        if (casasDec > 0) {
            resposta = sepDecimal + part;
            num = Math.floor(num);
        } else
            num = Math.round(num);
    } // end of decimal part
    else {
        while (part.length < casasDec)
            part = '0' + part;
        if (casasDec > 0) {
            resposta = sepDecimal + part;
        }
    }
    while (num > 0) // integer part
    {
        part = (num - Math.floor(num / 1000) * 1000).toString(); // part = three less significant digits
        num = Math.floor(num / 1000);
        if (num > 0)
            while (part.length < 3) // 123.023.123  if sepMilhar = '.'
                part = '0' + part; // 023
        resposta = part + resposta;
        if (num > 0)
            resposta = sepMilhar + resposta;
    }
    if (sinal < 0)
        resposta = '-' + resposta;
    return resposta;
}

function mostrarDetalleErrores() {
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

function popUpNewFormato() {
    setTimeout(function () {
        $('#nuevoformato').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}

function popUpListaEnvios() {

    let objMenu = "";
    let dataTabActivo = $("ul.etabs li.active").data("tabindex");
    (dataTabActivo == "0") ? objMenu = evtHot : objMenu = evtHotE;

    $('#idEnviosAnteriores').html(dibujarTablaEnvios(objMenu.ListaEnvios));
    setTimeout(function () {
        $('#enviosanteriores').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
        $('#tablalenvio').dataTable({
            "scrollY": 330,
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });
    }, 50);
}

function mostrarIngresoEnvio() {
    var modulo = $("#hfIdModulo").val();
    var index = "";
    switch (modulo) {
        case "3":
            index = "index";
            break;
        case "4":
            index = "indexmeddist";
            break;
        case "5":
            index = "indexdemcp";
            break;
    }
    window.location.href = controlador + index;
}

function dibujarTablaError() {

    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablaError' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Celda</th><th>Valor</th><th>Tipo Error</th></tr></thead>";
    cadena += "<tbody>";
    var len = listErrores.length;
    for (var i = 0; i < len; i++) {
        cadena += "<tr><td>" + listErrores[i].Celda + "</td>";
        cadena += "<td>" + listErrores[i].Valor + "</td>";
        cadena += "<td>" + errores[listErrores[i].Tipo].Descripcion + "</td></tr>";
    }
    cadena += "</tbody></table>";
    return cadena;
}

///////////////////Funciones para manejo de errores////////////////
function agregarError(row, col, valor, tipo, pto) {
    celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
    if (validarError(celda)) {
        var regError = {
            Celda: celda,
            Valor: valor,
            Tipo: tipo,
            Fila: row,
            Columna: col,
            Pto: pto
        };
        listErrores.push(regError);
        switch (tipo) {
            case errorNoNumero:
                errores[errorNoNumero].total++;
                break;
            case errorLimInferior:
                errores[errorLimInferior].total++;
                break;
            case errorLimSuperior:
                errores[errorLimSuperior].total++;
                break;
            case errorRangoFecha:
                errores[errorRangoFecha].total++;
                break;
            case errorCrucePeriodo:
                errores[errorCrucePeriodo].total++;
                break;
        }
    }
}

function validarError(celda) {
    for (var j in listErrores) {
        if (listErrores[j]['Celda'] == celda) {
            return false;
        }
    }
    return true;
}

function eliminarError(celda, tipoError) {
    var index = indexOfError(celda);
    if (index != -1) {
        listErrores.splice(index, 1);
        switch (tipoError) {
            case errorNoNumero:
                errores[errorNoNumero].total--;
                break;
            case errorLimInferior:
                errores[errorLimInferior].total--;
                break;
            case errorLimSuperior:
                errores[errorLimSuperior].total--;
                break;
            case errorRangoFecha:
                errores[errorRangoFecha].total--;
                break;
            case errorCrucePeriodo:
                errores[errorCrucePeriodo].total--;
                break;
        }
    }
}

function indexOfError(celda) {
    var index = -1;
    for (var i = 0; i < listErrores.length; i++) {
        if (listErrores[i].Celda == celda) {
            index = i;
            break;
        }
    }

    return index;
}

function indexOfPto(pto) {
    var index = -1;
    for (var i = 0; i < listaPtos.length; i++) {
        if (listaPtos[i].Ptomedicodi == pto) {
            index = i;
            break;
        }
    }

    return index;
}

function limpiarError() {
    nErrores = errores.length;
    total = 0;
    for (var i = 0; i < nErrores; i++) {
        errores[i].total = 0;
    }
    listErrores = [];
}

function getTotalErrores() {
    nErrores = errores.length;
    total = 0;
    for (var i = 0; i < nErrores; i++) {
        total += errores[i].total;
    }
    return total;
}

///////////////////Validar configuración de Plazo por Punto de medición del formato seleccionado////////////////
function validarConfigPlazoPto() {
    var msj = '';

    //Assetec
    let dataTabActivo = $("ul.etabs li.active").data("tabindex");
    if (dataTabActivo == "0") {  //PRIMER TAB (RETIRO)
		var listaPtos = evtHot.ListaHojaPto;
        if (hot.getInstance() !== undefined) {
            var data = hot.getData();
            var filasCab = evtHot.FilasCabecera - 1;
            var colCab = 0;
            var numFilasData = evtHot.Handson.ListaExcelData.length - evtHot.FilasCabecera;

            //obtener todos los errores
            for (i = 1; i <= listaPtos.length; i++) {
                var objPto = listaPtos[i - 1];
                var numFilasMinimo = objPto.ConfigPto != null && objPto.ConfigPto.Plzptominfila > 0 ? objPto.ConfigPto.Plzptominfila : 0;
                if (numFilasMinimo > 0) {
                    var valores = [];
                    for (k = 1; k <= numFilasData; k++) {//recorrer las siguientes filas del punto
                        //console.log((filasCab + k) + "-" + (colCab + i));
                        var valorCelda = parseFloat(data[filasCab + k][colCab + i]);
                        if (!isNaN(valorCelda))
                            valores.push({ 'Valor': valorCelda, 'Fila': filasCab + k, 'Col': colCab + i });
                    }

                    if (valores.length > 0 && valores.length < numFilasMinimo) {
                        var strPunto = objPto.Ptomedidesc; //objPto.Equinomb + ' - ' + objPto.Tipoptomedinomb;
                        strPunto = strPunto != null && strPunto.length > 46 ? strPunto.substring(0, 43) + "..." : strPunto;
                        msj += (" [Mín Celdas=" + numFilasMinimo + "] " + strPunto) + "\n";
                    }
                }
            }
        }
    } else {
		var listaPtos = evtHotE.ListaHojaPto;
        if (hotE.getInstance() !== undefined) {
            var data = hotE.getData();
            var filasCab = evtHotE.FilasCabecera - 1;
            var colCab = 0;
            var numFilasData = evtHotE.Handson.ListaExcelData.length - evtHotE.FilasCabecera;

            //obtener todos los errores
            for (i = 1; i <= listaPtos.length; i++) {
                var objPto = listaPtos[i - 1];
                var numFilasMinimo = objPto.ConfigPto != null && objPto.ConfigPto.Plzptominfila > 0 ? objPto.ConfigPto.Plzptominfila : 0;
                if (numFilasMinimo > 0) {
                    var valores = [];
                    for (k = 1; k <= numFilasData; k++) {//recorrer las siguientes filas del punto
                        //console.log((filasCab + k) + "-" + (colCab + i));
                        var valorCelda = parseFloat(data[filasCab + k][colCab + i]);
                        if (!isNaN(valorCelda))
                            valores.push({ 'Valor': valorCelda, 'Fila': filasCab + k, 'Col': colCab + i });
                    }

                    if (valores.length > 0 && valores.length < numFilasMinimo) {
                        var strPunto = objPto.Ptomedidesc; //objPto.Equinomb + ' - ' + objPto.Tipoptomedinomb;
                        strPunto = strPunto != null && strPunto.length > 46 ? strPunto.substring(0, 43) + "..." : strPunto;
                        msj += (" [Mín Celdas=" + numFilasMinimo + "] " + strPunto) + "\n";
                    }
                }
            }
        }
    }
    //Fin Assetec


    //if (hot.getInstance() !== undefined) {
    //    var data = hot.getData();
    //    var filasCab = evtHot.FilasCabecera - 1;
    //    var colCab = 0;
    //    var numFilasData = evtHot.Handson.ListaExcelData.length - evtHot.FilasCabecera;

    //    //obtener todos los errores
    //    for (i = 1; i <= listaPtos.length; i++) {
    //        var objPto = listaPtos[i - 1];
    //        var numFilasMinimo = objPto.ConfigPto != null && objPto.ConfigPto.Plzptominfila > 0 ? objPto.ConfigPto.Plzptominfila : 0;
    //        if (numFilasMinimo > 0) {
    //            var valores = [];
    //            for (k = 1; k <= numFilasData; k++) {//recorrer las siguientes filas del punto
    //                //console.log((filasCab + k) + "-" + (colCab + i));
    //                var valorCelda = parseFloat(data[filasCab + k][colCab + i]);
    //                if (!isNaN(valorCelda))
    //                    valores.push({ 'Valor': valorCelda, 'Fila': filasCab + k, 'Col': colCab + i });
    //            }

    //            if (valores.length > 0 && valores.length < numFilasMinimo) {
    //                var strPunto = objPto.Ptomedidesc; //objPto.Equinomb + ' - ' + objPto.Tipoptomedinomb;
    //                strPunto = strPunto != null && strPunto.length > 46 ? strPunto.substring(0, 43) + "..." : strPunto;
    //                msj += (" [Mín Celdas=" + numFilasMinimo + "] " + strPunto) + "\n";
    //            }
    //        }
    //    }
    //}

    if (msj != '')
        msj = "Los siguientes Puntos de medición no se consideran parte del envío ya que:" + "\n" + msj;

    return msj;
}

/////////////////////////////////////////////////////////////////////

function getExcelColumnName(pi_columnNumber) {
    var li_dividend = pi_columnNumber;
    var ls_columnName = "";
    var li_modulo;

    while (li_dividend > 0) {
        li_modulo = (li_dividend - 1) % 26;
        ls_columnName = String.fromCharCode(65 + li_modulo) + ls_columnName;
        li_dividend = Math.floor((li_dividend - li_modulo) / 26);
    }

    return ls_columnName;
}

function getFormattedDate(date) {
    if (date instanceof Date) {
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var seconds = date.getSeconds();
        var ampm = hours >= 12 ? 'pm' : 'am';
        hours = hours % 12;
        hours = hours ? hours : 12; // the hour '0' should be '12'
        hours = hours < 10 ? '0' + hours : hours;
        minutes = minutes < 10 ? '0' + minutes : minutes;
        seconds = seconds < 10 ? '0' + seconds : seconds;
        var strTime = hours + ':' + minutes + ':' + seconds + ' ' + ampm;

        return year + '/' + month + '/' + day + " " + strTime;
    }
    else {
        return "No es fecha";
    }
}

function descargarFormato() {

    let dataTabActivo = $("ul.etabs li.active").data("tabindex");
    let TabidEmpresa = null;
    let TabidFormato = null;

    if (dataTabActivo == "0") {
        TabidEmpresa = $('#cbEmpresa').val(),
            TabidFormato = 102

    } else {
        TabidEmpresa = $('#cbEmpresaE').val(),
            TabidFormato = iFormatCodi
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'generarformato',
        dataType: 'json',
        data: {
            idEmpresa: TabidEmpresa, //$('#hfIdEmpresa').val(),
            idFormato: TabidFormato, //$('#hfIdFormato').val(),
            fecha: $('#hfFecha').val(),
            semana: $('#Anho').val() + $('#hfSemana').val(), //$('#hfSemana').val(),
            mes: $('#hfMes').val()
        },
        success: function (result) {
            if (result == "1") {
                window.location = controlador + 'descargarformato';
            }
            else {
                alert(result);
            }
        },
        error: function () {
            alert("Error");
        }
    });
}

function leerFileUpExcel() {

    let dataTabActivo = $("ul.etabs li.active").data("tabindex");
    let TabidEmpresa = null;
    let TabidFormato = null;

    if (dataTabActivo == "0") {
        TabidEmpresa = $('#cbEmpresa').val(),
            TabidFormato = 102

    } else {
        TabidEmpresa = $('#cbEmpresaE').val(),
            TabidFormato = iFormatCodi
    }

    var retorno = 0;
    $.ajax({
        type: 'POST',
        url: controlador + 'LeerFileUpExcel',
        dataType: 'json',
        async: false,
        data: {
            idEmpresa: TabidEmpresa,//$('#hfIdEmpresa').val(),
            fecha: $('#hfFecha').val(),
            semana: $('#Anho').val() + $('#hfSemana').val(),
            mes: $('#hfMes').val(),
            idFormato: TabidFormato //$('#hfIdFormato').val()
        },
        success: function (res) {
            retorno = res;
        },
        error: function () {
            $('#percentValidacion').text("Ha ocurrido un error.");
        }
    });
    return retorno;
}

function expandirExcel() {
    $('#idpanel').addClass("divexcel");

    let dataTabActivo = $("ul.etabs li.active").data("tabindex");
    (dataTabActivo == "0") ? hot.render() : hotE.render();
   
}

function restaurarExcel() {

    $('#tophead').css("display", "none");
    $('#detExcel').css("display", "block");
    $('#idpanel').removeClass("divexcel");
    $('#itemExpandir').css("display", "block");
    $('#itemRestaurar').css("display", "none");

    //hot.render();
}

function getTipoError(value, limiteInf, limiteSup) {
    if (value == "")
        return 0; // blanco
    if (isNaN(value)) {
        return 2; // no numero
    }
    if (parseInt(value, 10) < limiteInf) {
        return 3; //Limite Inferior
    }
    if (parseInt(value, 10) > limiteSup) {
        return 4; //Limite Superior
    }
    return 1;//no hay error
}

function getTotBlancos(data, rowini, colini) {
    //var arreglo = data.split(",");
    var total = data.length;
    var totalBlancos = 0;
    var cadena = "";
    for (var i = rowini; i < total; i++) {
        for (var j = colini; j < data[i].length; j++) {
            //si es numero
            dato = data[i][j];
            if (!data[i][j]) {
                totalBlancos++;
            }
        }
    }
    return totalBlancos;
}

function enviarExcelWeb() {
    if (evtHot.Handson.ReadOnly) {
        alert("No se puede enviar información, solo diponible para  visualización");
        return
    }

    if (confirm("¿Desea enviar información a COES?")) {
        var $container = $('#detalleFormato');
        if (validarEnvio()) {
            $('#hfDataExcel').val((hot.getData()));
            $.ajax({
                type: 'POST',
                dataType: 'json',
                async: false,
                url: controlador + "GrabarExcelWeb",
                data: {
                    dataExcel: $('#hfDataExcel').val(),
                    idFormato: $('#hfIdFormato').val(),
                    idEmpresa: $('#hfIdEmpresa').val(),
                    fecha: $('#hfFecha').val(),
                    semana: $('#Anho').val() + $('#hfSemana').val(),
                    mes: $('#hfMes').val()
                },
                beforeSend: function () {
                    mostrarExito("Enviando Información ..");
                },
                success: function (evt) {
                    if (evt.Resultado == 1) {
                        $("#hfIdEnvio").val(evt.IdEnvio);
                        VER_ENVIO = true;
                        cargarDataExcelWeb(2);
                        mostrarExito("Los datos se enviaron correctamente");
                    }
                    else {
                        mostrarError("Error al Grabar");
                    }
                },
                error: function () {
                    mostrarError();
                }

            });

        }

    }

}

function crearPupload() {
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: ['btnSelectExcel', 'btnSelectExcelE'],
        url: siteRoot + 'valorizaciondiaria/envio/Upload',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '5mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx,xls" }
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                if (uploader.files.length == 2) {
                    uploader.removeFile(uploader.files[0]);
                }
                plupload.each(files, function (file) {
                    // loadInfoFile(file.name, plupload.formatSize(file.size));
                });
                uploader.start();
                mostrarEvento("Cargando");
                up.refresh();
            },
            UploadProgress: function (up, file) {
                //mostrarProgreso(file.percent);
            },
            UploadComplete: function (up, file) {
                mostrarAlerta("Subida completada <strong>Por favor espere</strong>");
                var retorno = leerFileUpExcel();
                let dataTabActivo = $("ul.etabs li.active").data("tabindex");
                if (dataTabActivo == "0" && !evtHot.Handson.ReadOnly) {
                    if (retorno == 1) {
                        limpiarError();
                        $("#hfIdEnvio").val(-1);//-1 indica que el handsonetable mostrara datos del archivo excel 
                        cargarDataExcelWeb(5);
                        mostrarExito("<strong>Por favor presione el botón 'Enviar' para grabar los datos</strong>");
                    }
                    else {
                        mostrarError("Error: Formato desconocido.");
                    }
                }
                else if (!evtHotE.Handson.ReadOnly) {
                    if (retorno == 1) {
                        limpiarError();
                        $("#hfIdEnvio").val(-1);//-1 indica que el handsonetable mostrara datos del archivo excel    
                        cargarDataExcelWebE(5);
                        mostrarExito("<strong>Por favor presione el botón enviar para grabar los datos.</strong>");
                    }
                    else {
                        mostrarError("Error: Formato desconocido.");
                    }
                }
                else {
                    alert("No se puede enviar información, solo diponible para  visualización");
                }


            },
            Error: function (up, err) {
                loadValidacionFile(err.code + "-" + err.message);
            }
        }
    });
    uploader.init();
}

function cargarSemanaAnho() {
    var anho = $('#Anho').val();
    $('#hfAnho').val(anho);
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarSemanas',

        data: { idAnho: $('#hfAnho').val() },

        success: function (aData) {
            $('#divSemana').html(aData);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });

}

/// Setea por defecto el año mes y dia al iniciar el aplicativo
function getPeriodoDefault() {
    var hoy = new Date();
    var dd = hoy.getDate();
    var mm = hoy.getMonth() + 1; //hoy es 0!
    var yyyy = hoy.getFullYear();

    if (dd < 10) {
        dd = '0' + dd
    }

    if (mm < 10) {
        mm = '0' + mm
    }
    periodo.anho = yyyy;
    periodo.mes = $("#hfMes").val();

}

function grabar() {
    if (confirm("¿Desea enviar información a COES?")) {
        if (validarEnvio()) {
            $.ajax({
                type: "POST",
                url: controlador + 'grabartipo2',
                dataType: "json",
                contentType: 'application/json',
                traditional: true,
                data: JSON.stringify({
                    data: hot.getData(),
                    idFormato: $('#hfIdFormato').val(),
                    idEmpresa: $('#hfIdEmpresa').val(),
                    fecha: $('#hfFecha').val()
                }),

                success: function (data) {
                    if (data == 1) {
                        mostrarExito("Los datos se enviaron correctamente");
                    }
                    else {
                        mostrarError("Error al grabar");
                    }
                },
                error: function () {
                    mostrarError("Error en el envío de datos");
                }
            });
        }
    }
}

function grabar2() {
    if (confirm("¿Desea enviar información a COES?")) {
        if (validarEnvio()) {
            $.ajax({
                type: "POST",
                url: controlador + 'grabartipo2',
                dataType: "json",
                contentType: 'application/json',
                traditional: true,
                data: JSON.stringify({
                    data: hot.getData(),
                    idFormato: iFormatCodi, //$('#hfIdFormato').val(),
                    idEmpresa: $("#cbEmpresaE").val(),//$('#hfIdEmpresa').val(),
                    fecha: $('#hfFecha').val()
                }),

                success: function (data) {
                    if (data == 1) {
                        mostrarExito("Los datos se enviaron correctamente");
                    }
                    else {
                        mostrarError("Error al grabar");
                    }
                },
                error: function () {
                    mostrarError("Error en el envío de datos");
                }
            });
        }
    }
}


function getHoraInicio() {
    var d = new Date();
    var hora = Math.floor(d.getHours() / 3) * 3;
    //var h = ("0" + hora).slice(-2);

    return "00:00:00";
}

function getHoraFin() {
    var d = new Date();
    var hora = Math.floor(d.getHours() / 3) * 3 + 3;
    var h = ("0" + hora).slice(-2);
    var horaFin = "";
    if (hora == 24) {
        horaFin = "23:59:59";
    }
    else {
        horaFin = ("0" + hora).slice(-2) + ":00:00";
    }
    horaFin = "23:59:59";
    return horaFin;
}

function getItemListaPtoMedicion(lista, iniFil) {
    item = -1;
    columnaPto = hot.getDataAtCol(1);
    nRows = columnaPto.length - 1;
    for (var j = 0; j < lista.length; j++) {
        encontrado = false;
        //for (var i = iniFil + 1; i <= nRows; i++) {
        for (var i = 1; i <= nRows; i++) {
            valor = columnaPto[i];
            if (valor == lista[j].id) {
                encontrado = true;
            }
        }
        if (!encontrado) {
            item = lista[j].id;
            break;
        }

    }
    return item;
}

function verificarRangoHoras(horaIni, horaFin) {
    var resultado = false;
    segundos1 = parseInt(horaIni.substr(0, 2)) * 60 * 60 + parseInt(horaIni.substr(3, 2)) * 60 + parseInt(horaIni.substr(6, 2));
    segundos2 = parseInt(horaFin.substr(0, 2)) * 60 * 60 + parseInt(horaFin.substr(3, 2)) * 60 + parseInt(horaFin.substr(6, 2));
    //console.log("Hora1 " + horaIni + " -- " + segundos1 + "  Hora2 " + horaFin + " -- " + segundos2);
    if (parseInt(segundos2) <= parseInt(segundos1)) {
        resultado = true;
    }
    return resultado;
}

function segundos(hora) {
    return parseInt(hora.substr(0, 2)) * 60 * 60 + parseInt(hora.substr(3, 2)) * 60 + parseInt(hora.substr(6, 2));
}

function verificaCruceRagoHora(grilla, fila) {
    HayCruce = false;
    columnaPto = grilla.getDataAtCol(1);
    columnaHoraIni = grilla.getDataAtCol(2);
    columnaHoraFin = grilla.getDataAtCol(3);
    nRows = columnaPto.length - 1;
    for (var i = 1; i <= nRows; i++) {
        if ((i != fila) && (columnaPto[i] == columnaPto[fila])) {
            if (columnaHoraIni[i] != null) {
                //console.log("Hora Ini: " + columnaHoraFin[i]);
                if ((segundos(columnaHoraIni[i]) > segundos(columnaHoraFin[fila])) ||
                    (segundos(columnaHoraIni[fila]) > segundos(columnaHoraFin[i]))) {

                }
                else {
                    HayCruce = true;
                }
            }
        }
    }
    return HayCruce;
}

function dibujaBarra(accion, isReadOnly, habilitarEditar) {

    let dataTabActivo = $("ul.etabs li.active").data("tabindex");
    (dataTabActivo == "0") ? objMenu = evtHot : objMenu = evtHotE;

    if (!isReadOnly) {
        switch (accion) {
            case 1://nuevo
                if (objMenu.OpGrabar) {
                    $('#idEditar').css("display", "none");
                    $('#idEnviar').css("display", "block");
                }
                else {
                    $('#idEditar').css("display", "none");
                    $('#idEnviar').css("display", "none");
                }
                $('#idErrores').css("display", "block");
                $('#idSubir').css("display", "block");
                break;
            case 2://grabo
                $('#idErrores').css("display", "none");
                $('#idEditar').css("display", "block");
                $('#idSubir').css("display", "none");
                $('#idEnviar').css("display", "none");
                break;
            case 3://editar
                $('#idErrores').css("display", "block");
                $('#idEditar').css("display", "none");
                $('#idSubir').css("display", "block");
                $('#idEnviar').css("display", "block");
                break;
            case 4://Envios
                $('#idErrores').css("display", "none");
                $('#idEditar').css("display", "none");
                $('#idSubir').css("display", "none");
                $('#idEnviar').css("display", "none");
                break;
            case 5://Subir
                $('#idErrores').css("display", "block");
                $('#idEditar').css("display", "none");
                $('#idSubir').css("display", "block");
                $('#idEnviar').css("display", "block");
                break;

        }

    }
    else {
        $('#idEnviar').css("display", "none");
        $('#idEditar').css("display", "none");
        $('#idSubir').css("display", "none");
        //$('#idErrores').css("display", "none");

    }

    if (habilitarEditar) {
        $('#btnEditarEnvio').parent().css("display", "table-cell");
    } else {
        $('#btnEditarEnvio').parent().css("display", "none");
    }
}

function abrirPopupEmbalse() {
    if (evtHot != undefined && evtHot.ListaPtoMedicion != undefined) {
        $('#embalses').html(getHtmlTablaEmbalses());

        setTimeout(function () {
            $('#popupEmbalses').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown',
                modalClose: false
            });
        }, 50);

    } else {
        alert("No hay puntos de medición.");
    }
}

function agregarFilaHandson() {
    var listaId = [];
    $('#tablaEmbalse input[name=checkembalse]:checked').each(function () {
        listaId.push($(this).attr('id'));
    });

    grillaBD = hot.getData();

    for (var i = 0; i < listaId.length; i++) {
        var id = listaId[i];
        index = grillaBD.length;
        grillaBD.push([fechaDia, id, getHoraInicio(), getHoraFin(), "0", "", "<button onclick='alert()' type='button'>X</button>"]);
        hot.updateSettings({
            maxRows: index + 1,
            data: grillaBD
        });
        hot.render();
    }

}

function getHtmlTablaEmbalses() {
    var html = '<table id="tablaEmbalse" class="pretty"><thead><tr><th> <input style="margin: 0;float: left;margin-top: 1px;" type="checkbox" id="allEmbalse" onclick="seleccionarAllEmbalse()" /> Todos </th><th>Seleccionar</th></tr></thead><tbody>';
    var listaData = evtHot.ListaPtoMedicion;

    var htmlCuerpo = "";
    for (var i = 0; i < listaData.length; i++) {
        var id = listaData[i].id;
        if (existeHorarioDisponible(id)) {
            var fila = "<tr>";
            fila += '<td><input type="checkbox" id="' + id + '" name="checkembalse" value="' + id + '"></td>';
            fila += '<td>' + listaData[i].text + '</td>';
            fila += "</tr>";
            htmlCuerpo += fila;
        }
    }

    html += htmlCuerpo
    if (htmlCuerpo == "") {
        html = html + "<tr><td colspan='2'>No están disponibles los registros para ser agregados</td></tr>";
    }

    html += "</tbody></table>";

    return html;
}

function existeHorarioDisponible(id) {
    grillaBD = hot.getData();
    for (var i = 0; i < grillaBD.length; i++) {
        if (grillaBD[i][1] == id && grillaBD[i][2] == "00:00:00" && grillaBD[i][3] == "23:59:59") {
            return false;
        }
    }
    return true;
}

function seleccionarAllEmbalse() {
    if ($('#allEmbalse').is(":checked")) {
        $('#tablaEmbalse input[name=checkembalse]').each(function () {
            $(this).prop("checked", true);
        });
    } else {
        $('#tablaEmbalse input[name=checkembalse]').each(function () {
            $(this).prop("checked", false);
        });
    }
}

function agregarColumna() {
    var j = 0;
    for (var i = 0; i < grillaBD.length; i++) {
        j = grillaBD[i].length;
        grillaBD[i][j] = "";
    }
}

function mostrarBotonAgregar() {
    alert(formato);
    if (formato == tipo1) {
        $('#celdaAgregar').css("display", "none");
    }
    if (formato == tipo2) {
        $('#celdaAgregar').css("display", "block");
    }
}

function eliminarFilaHandson() {
    grillaBD = hot.getData();
    index = grillaBD.length;
    console.log("Despues de Eliminar:" + index);
    hot.updateSettings({
        maxRows: index - 1
    });
    listErrores = [];
    //actualizaErrores();
}

function actualizaErrores() {
    grillaBD = hot.getData();
    maxRows = grillaBD.length;
    var len = listErrores.length;
    var i = 0;
    //for (var i = 0 ; i < len ; i++) {
    while (i < len) {
        for (var j = 1; j < maxRows; j++) {
            console.log("Total:" + len);
            console.log("Fila:" + grillaBD[j][1] + " Error:" + listErrores[i].Pto);
            if (grillaBD[j][1] == parseInt(listErrores[i].Pto)) {
                eliminarError(listErrores[i].Celda, listErrores[i].Tipo)
                len--;
            }

        }
        i++;
    }
}

function nuevoFormatoE(verUltimoEnvio) {
    VER_ULTIMO_ENVIO = verUltimoEnvio;
    $('#btnEditarEnvioE').parent().css("display", "none");

    if (($("#cbFormato").val() != "0") && ($("#cbFormato").val() != "")) {
        $("#hfIdEmpresa").val($("#cbEmpresaE").val());
        $("#hfIdEnvio").val(0);
        $("#hfFecha").val($("#txtFechaE").val());
        $("#hfSemana").val($("#cbSemana").val());
        $("#hfMes").val($("#txtMesE").val());

        cargarDataExcelWebE(1);

        $('#barraHidroE').css("display", "block");
        mostrardetalleExcel();
    }
    else {
        alert("Error!.Seleccionar Formato");
    }
}

function cargarDataExcelWebE(accion) {
          
    if (typeof hotE != 'undefined') {
        hotE.destroy();
    }
    getModelFormatoE(accion);
}

function getModelFormatoE(accion) {
    $('#hfVersion').val($('#cbVersion').val());
    $('#hfHorizonte').val($('#cbHorizonte').val());
    $('#hfFecha').val($('#txtFechaE').val());
    $('#hfSemana').val($('#cbSemana').val());
    $('#hfAnho').val($('#Anho').val());

    idEmpresa = $("#cbEmpresaE").val(); //$("#hfIdEmpresa").val();
    idFormato = iFormatCodi; //$("#hfIdFormato").val();
    idEnvio = $("#hfIdEnvio").val();
    fecha = $("#hfFecha").val();
    fechaDia = $('#hfFechaDia').val();
    semana = "";
    mes = $("#txtMesE").val();
    anho = "";
    semana = "";
    limpiarError();
    hideMensajeEvento();
    var inputVerUltimoEnvio = 0;
    VER_ULTIMO_ENVIO = false;

    $.ajax({
        type: 'POST',
        url: controlador + "MostrarHojaExcelWeb",
        dataType: 'json',
        data: {
            idEmpresa: idEmpresa,
            idFormato: idFormato,
            idEnvio: idEnvio,
            fecha: fecha,
            semana: semana,
            mes: mes,
            verUltimoEnvio: inputVerUltimoEnvio
        },
        success: function (evt) {

            var habilitarEditar = false;
            var evtlastEnvio = parseInt(evt.IdEnvioLast) || 0;
            if (evtlastEnvio > 0 && VER_ULTIMO_ENVIO) {
                $("#hfIdEnvio").val(evtlastEnvio);
                idEnvio = evtlastEnvio;
                habilitarEditar = true;
            }

            if (VER_ENVIO) {
                habilitarEditar = true;
            }

            if (!evt.EsEmpresaVigente) {
                habilitarEditar = false;
            }

            evtHotE = evt;
            switch (parseInt(idFormato)) {
                case formatoVertim:
                case formatoDescarg:
                    formato = tipo2;
                    crearGrillaFormatoTipo2(evt, 'detalleFormatoE');
                    $('#celdaAgregarE').css("display", "block");
                    $('#celdaDescargarE').css("display", "none");
                    $('#celdaImportarE').css("display", "none");
                    break;
                default:
                    formato = tipo1;
                    $('#celdaAgregarE').css("display", "none");
                    $('#celdaDescargarE').css("display", "block");
                    $('#celdaImportarE').css("display", "block");
                    crearGrillaFormatoTipo1(evt, habilitarEditar, 'detalleFormatoE');

                    break;
            }
            dibujaBarra(accion, evt.Handson.ReadOnly, habilitarEditar);
            switch (accion) {
                case 1:
                    mostrardetalleExcel(evt.Formato.Areaname, evt.Empresa, evt.Formato.Formatnombre, evt.Anho, evt.Mes, evt.Semana, evt.Dia, evt.Formato.Formatperiodo)
                    break;
                case 2:
                    mostrarExito("Los datos se enviaron correctamente");
                    break;
                case 4:
                    mostrarExito("Ya puede consultar el envío anterior");
                    break;
                case 5:
                    mostrarExito("<strong>Por favor presione el botón enviar para grabar los datos.</strong>");
                    break;
            }

            VER_ULTIMO_ENVIO = false;
            VER_ENVIO = false;
        },
        error: function () {
            alert("Error al cargar Excel Web");
            //evtHot = null;
        }

    });
}

function enviarExcelWebE() {

    if (evtHotE.Handson.ReadOnly) {
        alert("No se puede enviar información, solo diponible para  visualización");
        return
    }

    if (confirm("¿Desea enviar información a COES?")) {
        var $container = $('#detalleFormatoE');
        if (validarEnvio()) {
            $('#hfDataExcel').val((hotE.getData()));
            $.ajax({
                type: 'POST',
                dataType: 'json',
                async: false,
                url: controlador + "GrabarExcelWeb",
                data: {
                    dataExcel: $('#hfDataExcel').val(),
                    idFormato: iFormatCodi,//$('#hfIdFormato').val(),
                    idEmpresa: $("#cbEmpresaE").val(), //$('#hfIdEmpresa').val(),
                    fecha: $('#hfFecha').val(),
                    semana: $('#Anho').val() + $('#hfSemana').val(),
                    mes: $('#hfMes').val()
                },
                beforeSend: function () {
                    mostrarExito("Enviando Información ..");
                },
                success: function (evt) {
                    if (evt.Resultado == 1) {
                        $("#hfIdEnvio").val(evt.IdEnvio);
                        VER_ENVIO = true;
                        cargarDataExcelWebE(2);
                        mostrarExito("Los datos se enviaron correctamente");
                    }
                    else {
                        mostrarError("Error al Grabar");
                    }
                },
                error: function () {
                    mostrarError();
                }

            });

        }

    }

}