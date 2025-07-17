var controlador = siteRoot + 'StockCombustibles/envio/'

$(function () {

    $('#txtFecha').Zebra_DatePicker({
        direction: -1
    });

    $('#btnConsultar').click(function () {
        //mostrarBarraStock();
        mostrarExcelWeb();

    });

    $('#cbEmpresa').change(function () {
        limpiarBarra();
    });
    $('#txtFecha').click(function () {
        limpiarBarra();
    });

    $('#btnMostrarErrores').click(function () {
        mostrarDetalleErrores();
    });
    $('#btnEnviarDatos').click(function () {
        enviarExcelWeb();
    });
    $('#btnVerEnvios').click(function () {
        validaStock();
        popUpListaEnvios();
    });

});


function limpiarBarra() {
    $('#barraStock').css("display", "none");
    $('#detalleFormato').html("");
}

//Muestra la barra de herramemntas para administrar los datos de stock de combustible ingresados
function mostrarExcelWeb() {
    if ($("#txtFecha").val() != "") {

        $('#barraStock').css("display", "block");
        mostrarFormatoExcelWeb();
    }
    else {
        alert("Error!.Ingresar fecha correcta");
    }


}

function enviarExcelWeb() {
    if (confirm("¿Desea enviar información a COES?")) {
        //var $container = $('#detalleFormato');
        if (validarEnvio()) {
            //$('#hfDataExcel').val((hot.getData()));
            //$('#hfIdFormato').val(50);
            var empresa = $('#cbEmpresa').val();
            var fecha = $('#txtFecha').val();
            $('#hfEmpresa').val(empresa);
            $.ajax({
                type: 'POST',
                dataType: 'json',
                //async: false,
                contentType: 'application/json',
                traditional: true,
                url: controlador + "GrabarExcelWeb",
                data: JSON.stringify({
                    dataExcel: hot.getData(),
                    idEmpresa: empresa,
                    fecha: fecha
                }),


                beforeSend: function () {
                    mostrarExito("Enviando Información ..");
                },
                success: function (evt) {
                    if (evt.Resultado == 1) {
                        $("#hfIdEnvio").val(evt.IdEnvio);
                        //cargarDataExcelWeb(2);
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

function validarEnvio() {
    retorno = true;
    totalErrores = listErrores.length;
    getTotalErrores();
    //valida si existen errores
    if ((totalErrores) > 0) {
        mostrarError("Existen errores en las celdas, favor corregir y vuelva a envíar");
        mostrarDetalleErrores();
        return false;
    }
    //Valida los registro en blanco
    $('#hfDataExcel').val((hot.getData()));
    var data = $('#hfDataExcel').val();
    totalB = getTotBlancos(data);
    if (totalB > 0) {
        if (confirm("Total Blancos: " + totalB + " \n" + "¿Desea reemplazar registros en blanco por ceros?")) {
            return true;
        }
        return false;
    }

    //valida si hubo cambios en stock inicio y debe ingresar observaciones
    if (validaObservacionesStockinicio(data))
        return false;

    // Valida que stock final sea menor que stock inicial mas recepcion
    validaStock();
    return true;

}

function validaStock() {
    colStocIni = hot.getDataAtCol(2);
    colStocFin = hot.getDataAtCol(3);
    colRecepcion = hot.getDataAtCol(4);
    for (var i = 2; i < colStocIni.length; i++) {
        var resultado = -parseFloat(colStocFin[i]) + parseFloat(colStocIni[i]) + parseFloat(colRecepcion[i]);
        //alert(resultado);
        if (resultado < 0) {
            alert("Error : " + i);
        }
    }

}

function getTotBlancos(data) {
    var arreglo = data.split(",");
    var total = arreglo.length;
    var nfilas = total / 7 - 2; // sin cosiderar las 2 filas de titulos    
    var totalBlancos = 0;
    if (nfilas > 0) { // si  hay datos
        for (var i = 0 ; i < nfilas ; i++) {
            for (var j = 0; j < 3; j++) {
                dato = arreglo[17 + 7 * i + j - 1];
                //si no es numero
                if (!dato) {
                    totalBlancos++;
                }
            }
        }
    }
    return totalBlancos;
}

function getTotalErrores() {
    nErrores = errores.length;
    total = 0;
    for (var i = 0; i < nErrores; i++) {
        total += errores[i].total;
    }
    return total;
}

function mostrarEnvioExcelWeb(envio) {
    $('#enviosanteriores').bPopup().close();
    $("#hfIdEnvio").val(envio);
    mostrarFormatoExcelWeb()
    var mensaje = mostrarMensajeEnvio();
    mostrarExito(mensaje);
}

function mostrarFormatoExcelWeb() {
    var idEmpresa = $("#cbEmpresa").val();
    var fecha = $("#txtFecha").val();
    var idEnvio = $("#hfIdEnvio").val();
    $.ajax({
        type: 'POST',
        url: controlador + "MostrarHojaExcelWebStock",
        dataType: 'json',
        data: {
            idEmpresa: idEmpresa,
            idEnvio: idEnvio,
            fecha: fecha
        },
        success: function (evt) {
            evtHot = evt;
            crearGrillaExcelStock(evt);
            listaStockinicio = evt.ListaStockInicio;
        },
        error: function () {
            alert("Error al cargar Excel Web");
        }
    });
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

///////////////////Funciones para manejo de errores////////////////

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

function agregarError(celda, valor, tipo) {
    if (validarError(celda)) {
        var regError = {
            Celda: celda,
            Valor: valor,
            Tipo: tipo
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

//function indexOfPto(pto) {
//    var index = -1;
//    alert(pto);
//    for (var i = 0; i < listaPtos.length; i++) {
//        if (listaPtos[i].Ptomedicodi == pto) {
//            index = i;
//            break;
//        }
//    }
//    return index;
//}

