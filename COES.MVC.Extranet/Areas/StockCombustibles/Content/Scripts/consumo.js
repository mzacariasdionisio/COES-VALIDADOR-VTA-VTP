var controlador = siteRoot + 'StockCombustibles/envio/';
var dataSinFormula;
var fechaTmp = '';
$(function () {
    idFuenteDatos = 0;
    idFormato = idFormatoConsumoCombustible;
    tipoFormato = 0;
    dibujarPanelIeod(tipoFormato, 1, -1);

    $('#txtFecha').Zebra_DatePicker({
        direction: -1,
        onSelect: function () {
            $("#hfIdEnvio").val('0');
            limpiarBarra();
            fechaTmp = '';
        }
    });

    $('#btnConsultar').click(function () {
        mostrarExcelWeb();
    });
    $('#cbEmpresa').change(function () {
        $("#hfIdEnvio").val('0');
        limpiarBarra();
        fechaTmp = '';
        dibujarPanelIeod(tipoFormato, 1, -1);
    });
    $('#txtFecha').click(function () {
    });
    $('#btnEnviarDatos').click(function () {
        if (evtHot.Handson.ReadOnly) {
            alert("No se puede enviar información, solo diponible para  visualización");
            return
        }
        else {
            enviarExcelWeb();
        }
    });

    $('#btnDescargarFormato').click(function () {
        if (!validarSeleccionDatos()) {
            descargarFormato();
        }
        else {
            alert("Por favor seleccione la empresa correcta.");
        }
    });

    $('#btnMostrarErrores').click(function () {
        mostrarDetalleErrores();
    });
    $('#btnVerEnvios').click(function () {
        popUpListaEnvios();
    });
    $('#btnEditarEnvio').click(function () {
        $("#hfIdEnvio").val('0');
        fechaTmp = 'cambio';
        deshabilitarEdicionEnvio();
        mostrarFormularioConsumo();
    });
    $('#btnHabilitaStock').click(function () {
        habilitarStockInicio();
    });
});

function limpiarBarra() {
    $("#btnEnviarDatos").parent().hide();
    $("#btnMostrarErrores").parent().hide();
    $("#btnHabilitaStock").parent().parent().hide();
    $("#mensajeEvento").hide();
    $("#mensaje").html("Por favor seleccione la empresa y la fecha.");
    $("#mensaje").show();
    $('#barraStock').css("display", "none");
    $('#detalleFormato').html("");
    deshabilitarEdicionEnvio();
}

//Muestra la barra de herramemntas para administrar los datos de consumo de combustible ingresados
function mostrarExcelWeb() {
    $("#btnEnviarDatos").parent().hide();
    $("#btnMostrarErrores").parent().hide();
    $("#btnHabilitaStock").parent().parent().hide();
    deshabilitarEdicionEnvio();

    if ($("#txtFecha").val() != "") {
        $('#mensajeEvento').css("display", "none");
        $('#barraConsumo').css("display", "block");
        $("#hfIdEnvio").val(0);
        $('#hfEmpresa').val($('#cbEmpresa').val());
        mostrarFormularioConsumo();
    }
    else {
        alert("Error!.Ingresar fecha correcta");
    }
}

function mostrarFormularioConsumo(accion) {
    listErrores = [];
    var idEmpresa = $("#cbEmpresa").val();
    var fecha = $("#txtFecha").val();
    if (fechaTmp == '') {
        accion = 6;
        $("#hfIdEnvio").val('-1')
    }
    var idEnvio = $("#hfIdEnvio").val();
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    $.ajax({
        type: 'POST',
        url: controlador + "MostrarHojaExcelWebConsumo",
        dataType: 'json',
        data: {
            idEmpresa: idEmpresa,
            idEnvio: idEnvio,
            fecha: fecha
        },
        success: function (evt) {
            if (evt != -1) {
                evtHot = evt;
                if (!evtHot.Handson.ReadOnly) {
                    $("#btnEnviarDatos").parent().show();
                    $("#btnMostrarErrores").parent().show();
                    $("#btnHabilitaStock").parent().parent().show();
                }

                crearGrillaExcelConsumo(evt);
                if (accion == 2) {
                    var mensaje = mostrarMensajeEnvio(idEnvio);
                    mostrarExito("Los datos se enviaron correctamente. " + mensaje);
                    hideMensaje();
                    habilitarEdicionEnvio();
                }
                else if (accion == 5) {
                    mostrarExito("<strong>Por favor presione el botón enviar para grabar los datos</strong>");
                }
                else {
                    if (accion == 6) {
                        if (fechaTmp == '') {
                            fechaTmp = 'cambio';
                            idEnvio = evt.IdEnvioLast;
                            $("#hfIdEnvio").val(idEnvio);
                        }
                        if (idEnvio <= 0) {
                            var mensaje = mostrarMensajeEnvio();
                            mostrarMensaje(mensaje);
                            $('#divAcciones').css('display', 'block');
                        } else {
                            var mensaje = mostrarMensajeEnvio(idEnvio);
                            mostrarMensaje(mensaje);
                            $('#divAcciones').css('display', 'block');
                        }
                        habilitarEdicionEnvio();
                    } else {

                        var mensaje = mostrarMensajeEnvio();
                        mostrarMensaje(mensaje);
                        $('#divAcciones').css('display', 'block');
                        deshabilitarEdicionEnvio();
                    }
                }
            }
            else {
                alert("La empresa no tiene puntos de medición para cargar.");
                document.location.href = controlador + 'Consumo';
            }

            dibujarPanelIeod(tipoFormato, 1, -1);
        },
        error: function () {
            alert("Error al cargar Excel Web");
        }
    });
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

function enviarExcelWeb() {
    if (!calcularFormulas()) {
        return;
    }

    if (confirm("¿Desea enviar información a COES?")) {
        //var $container = $('#detalleFormato');

        if (validarEnvio()) {
            var empresa = $('#cbEmpresa').val();
            var fecha = $('#txtFecha').val();
            $('#hfEmpresa').val(empresa);
            $.ajax({
                type: 'POST',
                dataType: 'json',
                //async: false,
                contentType: 'application/json',
                traditional: true,
                url: controlador + "GrabarExcelWebConsumo",
                data: JSON.stringify({
                    data: dataSinFormula,
                    idEmpresa: empresa,
                    fecha: fecha
                }),


                beforeSend: function () {
                    mostrarExito("Enviando Información ..");
                },
                success: function (evt) {
                    if (evt.Resultado == 1) {
                        $("#hfIdEnvio").val(evt.IdEnvio);

                        $("#btnEnviarDatos").parent().hide();
                        $("#btnMostrarErrores").parent().hide();
                        $("#btnHabilitaStock").parent().parent().hide();
                        mostrarFormularioConsumo(2);

                        var mensaje = "Los datos se enviaron correctamente.";
                        mostrarExito(mensaje);
                        if (evt.Mensaje != null && evt.Mensaje != "")
                            alert(evt.Mensaje);
                        fechaTmp = '';
                    }
                    else {
                        mostrarError("Error al Grabar: " + evt.Mensaje);
                    }
                },
                error: function () {
                    mostrarError();
                }

            });

        }

    }

}

function validarEnvio() {
    retorno = true;
    if (!validarFinalStock()) {

        return false;
    }
    totalErrores = listErrores.length;
    getTotalErrores();

    //valida si existen errores
    if ((totalErrores) > 0) {
        mostrarError("Existen errores en las celdas, favor corregir y vuelva a envíar");
        mostrarDetalleErrores();
        return false;
    }

    //Valida los registro en blanco
    var data = hot.getData();//$('#hfDataExcel').val();
    totalB = getTotBlancos(data);
    if (totalB > 0) {
        //if (confirm("Total Blancos: " + totalB + " \n" + "¿Desea reemplazar registros en blanco por ceros?")) {
        //    return true;
        alert("Error: Hay " + totalB + " celdas vacias: ");
        return false;
    }

    //valida que el stock declarado no sea mayor que stock inicial mas recepcion
    totalE = getValStock(data);
    if (totalE > 0) {
        alert("El stock declarado no puede ser mayor que la suma del stock inicial más recepcion, ingresé observación.");
        /* alert("El stock declarado no puede ser mayor que el inicial si no se realizo una recepcion");*/
        return false;
    }

    //valida que Observacion no este vacio cuando el Stock Final declarado es menor que el Stock Final    
    if (validaStockFinal(data)) {
        return false;
    }

    //valida si hubo cambios en stock inicio y debe ingresar observaciones
    if (validaStockinicio(data)) {
        return false;
    }
    // Valida que stock final sea menor que stock inicial mas recepcion
    //validaStock();
    if (!validarStockFinalDeclarado(data)) {
        return false;
    }

    texto = validarComentario();
    if (texto.length > 0)
        return false;
    return true;

}

function validarFinalStock() {

    colStocFin = hot.getDataAtCol(columnas[0].final);

    var mensaje = "";
    var error = false;
    try {
        for (var i = nfilConsumo + 5; i < colStocFin.length; i++) {
            //console.log("F" + [i + 1]);
            var valor = hot.plugin.helper.cellValue(getExcelColumnName(columnas[0].final + 1) + [i + 1]);//hot.plugin.helper.cellValue("F" + [i + 1]);
            if (valor < 0) {
                mensaje += "Fila " + (i + 1).toString() + " : " + valor.toString() + "\n";
                error = true;
            }
        }
        if (error) {
            alert("Error en el Stock final, valor debe ser mayor que cero:\n" + mensaje);
            return false;
        }
        return true;
    }
    catch (err) {
        alert("Error, no se puede obtener el consumo final");
        return false
    }
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

function getFormattedDate(date) {
    if (date instanceof Date) {
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var ampm = hours >= 12 ? 'pm' : 'am';
        hours = hours % 12;
        hours = hours ? hours : 12; // the hour '0' should be '12'
        hours = hours < 10 ? '0' + hours : hours;
        minutes = minutes < 10 ? '0' + minutes : minutes;
        var strTime = hours + ':' + minutes + ' ' + ampm;

        return year + '/' + month + '/' + day + " " + strTime;
    }
    else {
        return "No es fecha";
    }
}

function calcularFormulas() {
    // var aCols = ['A', 'B', 'C', 'D', 'E', 'F', 'G'];
    var mydata = hot.getData();
    dataSinFormula = mydata.slice();
    try {
        for (i = 0; i < mydata.length; i++) {
            for (j = 0; j < mydata[i].length; j++) {
                if (mydata[i][j] != null) {
                    if (mydata[i][j].toString().indexOf('=') > -1) {
                        dataSinFormula[i][j] = hot.plugin.helper.cellValue(getExcelColumnName(j) + (i + 1));//hot.plugin.helper.cellValue(aCols[j] + (i + 1));
                    }
                    else {
                        dataSinFormula[i][j] = mydata[i][j];
                    }
                }
            }
        }
        return true;
    }
    catch (err) {
        alert("Error, Existen errores en los valores, favor corregir");
        return false;
    }

}

function getTotBlancos(data) {
    var totalBlancos = 0;
    for (var z = 0; z < nfilConsumo; z++) {
        fila = z + 2;
        valor = data[fila][4];
        if (valor == null) {
            totalBlancos++;
        }
        else {
            if (valor.length == 0) {
                totalBlancos++;
            }
        }
    }

    for (var z = 0; z < nfilStock; z++) {
        fila = z + nfilConsumo + 5;
        valor = data[fila][columnas[0].inicial];
        //if (!valor || 0 === valor.length) {
        if (valor != null) {
            if (valor.length <= 0) {
                totalBlancos++;
            }
        }
        else {
            totalBlancos++;
        }
        valor = data[fila][columnas[0].recepcion];
        if (valor != null) {
            if (valor.length <= 0) {
                totalBlancos++;
            }
        }
        else {
            totalBlancos++;
        }
    }


    return totalBlancos;
}

function getValStock(data) {
    var totalError = 0;
    for (var z = 0; z < nfilStock; z++) {
        fila = z + nfilConsumo + 5;
        valor1 = data[fila][columnas[0].inicial];
        valor2 = data[fila][columnas[0].recepcion];
        valor3 = data[fila][columnas[0].declarado];
        valor4 = data[fila][columnas[0].observacion];

        vali = parseFloat(valor1) + parseFloat(valor2);

        if (parseFloat(valor3) > vali && esBlancoVacio(valor4)) {
            totalError++;
        }
        else {
        }
    }
    return totalError;
}

function validaStockFinal(data) {
    var listaCeldaObs2 = [];
    var listaObsrev2 = [];
    var cadena = "";
    stockIni = 0;
    for (var z = 0; z < nfilStock; z++) {
        fila = z + nfilConsumo + 5;
        /*stockIni = data[fila][columnas[0].inicial];
        stockFin = data[fila][columnas[0].final];*/

        /*stockFin = parseFloat(data[fila][columnas[0].inicial]) + parseFloat(data[fila][columnas[0].recepcion]) - parseFloat(data[fila][columnas[1].consumo]);*/
        stockFin = hot.plugin.helper.cellValue(getExcelColumnName(columnas[0].final + 1) + (fila + 1));
        /*stockFin = parseFloat(data[fila][columnas[0].inicial]) + parseFloat(data[fila][columnas[0].recepcion]) - parseFloat(data[fila][columnas[0].total]);*/
        stockFinDec = data[fila][columnas[0].declarado];
        obs = data[fila][columnas[0].observacion];


        var reg = {
            /*Inicio: stockIni,*/
            Fin: stockFin,
            FinDec: stockFinDec,
            Observ: obs
        };
        listaObsrev2.push(reg);
    }

    for (var i = 0; i < nfilStock; i++) {
        /* console.log(" Ini:" + listaStockinicio[i] + "    Obser:" + listaObsrev2[i].Inicio);*/

        if (parseInt(listaObsrev2[i].Fin) > parseInt(listaObsrev2[i].FinDec)) {
            listaCeldaObs2.push(i);
            pos = nfilConsumo + 6 + i;
            str = listaObsrev2[i].Observ;
            if (esBlancoVacio(str)) {
                cadena = cadena + "Celda " + getExcelColumnName(columnas[0].observacion + 1) + pos + "; ";
            }
        }
    }
    if (listaCeldaObs2.length > 0) {
        if (cadena != "") {
            alert("Debe ingresar observaciones en las siguentes celdas: " + cadena);
            return true;
        }
    }
    return false;
}

function mostrarEnvioExcelWeb(envio) {
    $('#enviosanteriores').bPopup().close();
    $("#hfIdEnvio").val(envio);
    mostrarFormularioConsumo(6);
}


function validarComentario(cadena) {
    var regex1 = new RegExp(/([a-zA-Z0-9])\1{2,}/); //validación si encuentra una letra o número 3 o más veces consecutivamente
    var regex2 = new RegExp(/([\D])\1{2,}/); //validación si encuentra un caracter 3 o más veces consecutivamente
    var textoError = "";
    colObservacion = hot.getDataAtCol(columnas[0].observacion);
    var iniFil = nfilConsumo + 5;
    for (var i = iniFil; i < colObservacion.length; i++) {
        if (colObservacion[i] != null) {
            if ((colObservacion[i].length < 20) && (colObservacion[i].length > 0)) {
                textoError = "Observación debe tener mas de 20 caracteres : '" + colObservacion[i] + "'";
                alert(textoError);
            }
            var matchObs1 = colObservacion[i].toUpperCase().match(regex1);
            var matchObs2 = colObservacion[i].toUpperCase().match(regex2);
            if (matchObs1 != null || matchObs2 != null) {
                textoError = "Observación no válida: '" + colObservacion[i] + "'";
                alert(textoError);
            }
        }
    }
    return textoError;
}

function validarLengthPalabras(parrafo) {
    var vector = parrafo.split(' ');
    for (var i = 0; i < vector.lenght; i++) {
        if (vector[i].length > maxCadena) {
            return false;
        }
    }
    return true;
}

function validaStockinicio(data) {
    var listaCeldaObs = [];
    var listaObsrev = [];
    var cadena = "";
    stockIni = 0;
    for (var z = 0; z < nfilStock; z++) {
        fila = z + nfilConsumo + 5;
        stockIni = data[fila][columnas[0].inicial];
        obs = data[fila][columnas[0].observacion];
        var reg = {
            Inicio: stockIni,
            Observ: obs
        };
        listaObsrev.push(reg);
    }

    for (var i = 0; i < nfilStock; i++) {
        //console.log(" Ini:" + listaStockinicio[i] + "    Obser:" + listaObsrev[i].Inicio);
        if (parseInt(listaStockinicio[i]) != parseInt(listaObsrev[i].Inicio)) {
            listaCeldaObs.push(i);
            pos = nfilConsumo + 6 + i;
            str = listaObsrev[i].Observ;
            if (esBlancoVacio(str)) {
                cadena = cadena + "Celda " + getExcelColumnName(columnas[0].observacion + 1) + pos + "; ";
            }
        }
    }
    if (listaCeldaObs.length > 0) {
        if (cadena != "") {
            alert("Debe ingresar observaciones en las siguentes celdas: " + cadena);
            return true;
        }
    }
    return false;
}

function recuperaStockInicio(data) {
    stockIni = 0;
    listaStockinicio = [];
    for (var z = 0; z < nfilStock; z++) {
        fila = z + nfilConsumo + 5;
        if (fila <= data.length) {
            stockIni = data[fila][columnas[0].inicial];
            listaStockinicio.push(stockIni);
        }
    }
}

function validarStockFinalDeclarado(data) {
    mensaje = "";
    resultado = true;
    for (var z = 0; z < nfilStock; z++) {
        fila = z + nfilConsumo + 5;
        stockFinal = hot.plugin.helper.cellValue(getExcelColumnName(columnas[0].final + 1) + (fila + 1));
        stockFinalDec = data[fila][columnas[0].declarado];
        observacion = data[fila][columnas[0].observacion];
        //console.log(stockFinal + " " + stockFinalDec);
        if (stockFinal != stockFinalDec) {
            if (esBlancoVacio(observacion)) {
                indice = fila + 1;
                mensaje = mensaje + "Celda " + getExcelColumnName(columnas[0].observacion + 1) + indice + "; ";
                resultado = false;
            }
        }
    }

    if (!resultado) {
        alert("Error: Ingresar observacion debido a que no coinciden Stock Final con Stock Final Declarado ( " + mensaje + " )");
    }
    return resultado;
}

function esBlancoVacio(cadena) {
    if (cadena == null) {
        return true;
    }
    else {
        if (cadena.length == 0) {
            return true;
        }
    }
    return false;
}

function descargarFormato() {
    var empresa = $('#cbEmpresa').val();
    var fecha = $('#txtFecha').val();
    $('#hfEmpresa').val(empresa);
    $.ajax({
        type: 'POST',
        url: controlador + 'generarformatoconsumo',
        async: false,
        data: {
            idEmpresa: empresa,
            dia: fecha
        },
        success: function (result) {
            if (result.length > 0) {
                window.location.href = controlador + 'descargarformato?archivo=' + result;
            }
            else {
                alert("Error en descargar el archivo");
            }
        },
        error: function (result) {
            alert('ha ocurrido un error al descargar el archivo excel. ' + result.status + ' - ' + result.statusText + '.');
        }
    });
}

function habilitarStockInicio() {
    var empresa = $('#hfEmpresa').val();
    if (confirm("¿Desea habilitar registro de Stock Inicial?")) {
        $.ajax({
            type: 'POST',
            url: controlador + "HabilitarStockInicio",
            dataType: 'json',
            data: {
                idEmpresa: empresa
            },
            success: function (evt) {
                if (evt == 1) {
                    mostrarExcelWeb();
                }
                else {
                    alert("Error en eliminar datos para activar Stock Inicial");
                }
            },
            error: function () {
                alert("Ha ocurrido un error en guardar ensayo");
            }
        });
    }
}

