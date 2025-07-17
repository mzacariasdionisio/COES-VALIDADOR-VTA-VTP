var controlador = siteRoot + 'StockCombustibles/envio/'
var dataSinFormula;
$(function () {

    $('#txtFecha').Zebra_DatePicker({
        direction: 0
    });

    $('#btnConsultar').click(function () {
        $("#hfIdEnvio").val("0");
        mostrarExcelWeb();
    });
    $('#cbEmpresa').change(function () {
        limpiarBarra();
    });
    $('#txtFecha').click(function () {
        limpiarBarra();
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

    $('#btnMostrarErrores').click(function () {
        mostrarDetalleErrores();
    });
    $('#btnVerEnvios').click(function () {
        popUpListaEnvios();
    });
    $('#btnAgregarFila').click(function () {
        if (!modoLectura) {
            agregarFilaHandson();
        }
        else {
            alert("Se encuentra en modo lectura.");
        }
    });
    $("#hfIdEnvio").val("0");


});


function limpiarBarra() {
    $('#barraDisponibilidad').css("display", "none");
    $('#detalleFormato').html("");
}

//Muestra la barra de herramemntas para administrar los datos de consumo de combustible ingresados
function mostrarExcelWeb() {
    if ($("#txtFecha").val() != "") {
        $('#mensajeEvento').css("display", "none");
        showMensaje();
        $('#barraDisponibilidad').css("display", "block");
        mostrarFormularioDisponibilidad(consulta);
    }
    else {
        alert("Error!.Ingresar fecha correcta");
    }
}

function mostrarFormularioDisponibilidad(accion) {
    listErrores = [];
    var idEmpresa = $("#cbEmpresa").val();
    var fecha = $("#txtFecha").val();
    var idEnvio = $("#hfIdEnvio").val();
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    $.ajax({
        type: 'POST',
        url: controlador + "MostrarGridExcelWebDisponibilidad",
        dataType: 'json',
        //async: false,
        data: {
            idEmpresa: idEmpresa,
            idEnvio: idEnvio,
            fecha: fecha
        },
        success: function (evt) {
            if (evt != -1) {
                evtHot = evt;
                crearGrillaExcelDisponibilidad(evt);
                switch (accion) {
                    case envioDatos:
                        var mensaje = mostrarMensajeEnvio(idEnvio);
                        mostrarExito("Los datos se enviaron correctamente. " + mensaje);
                        hideMensaje();
                        break;
                    case envioAnterior:
                        var mensaje = mostrarMensajeEnvio(idEnvio);
                        mostrarExito(mensaje);
                        hideMensaje();
                        break;
                    case consulta:
                        var mensaje = mostrarMensajeEnvio();
                        mostrarMensaje("Por favor complete los datos. <strong>Plazo del Envio: </strong>" + mensaje);
                        break;
                }
                mostrardetalleExcel(evt.Empresa, evt.Fecha, evt.FechaNext);
            }
            else {
                alert("La empresa no tiene puntos de medición para cargar.");
                /*document.location.href = controlador + 'Disponibilidad';*/
            }
        },
        error: function () {
            alert("Error al cargar Excel Web");
        }
    });
}

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
    calcularFormulas();
    if (confirm("¿Desea enviar información a COES?")) {
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
                url: controlador + "GrabarDisponibilidad",
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
                        mostrarFormularioDisponibilidad(2);
                        hideMensaje();
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
    if (!validarRegistrosRepetidos())
        return false;
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
    var data = hot.getData();
    totalB = getTotBlancos(data);
    if (totalB > 0) {
        alert("Existen Celdas en Blanco, favor completar la información");
        return false;
    }
    return true;

}

function validarRegistrosRepetidos() {

    colCentral = hot.getDataAtCol(0);
    colFecha = hot.getDataAtCol(1);
    //colHora = hot.getDataAtCol(2);
    nFilas = colCentral.length;
    for (var i = 1; i < nFilas; i++) {
        for (var j = 1 ; j < nFilas; j++) {
            if (i != j) {
                if ((colCentral[i] == colCentral[j]) && (colFecha[i] == colFecha[j])) {
                    alert("Informaciòn repetida, favor verificar que la central, la fecha y la hora no coincidan en mas de un registro");
                    return false;
                }
            }
        }
    }
    return true;
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
    var aCols = ['A', 'B', 'C', 'D', 'E', 'F', 'G'];
    var mydata = hot.getData();
    dataSinFormula = mydata.slice();
    for (i = 0; i < mydata.length; i++) {
        for (j = 0; j < mydata[i].length; j++) {
            if (mydata[i][j] != null) {
                if (mydata[i][j].toString().indexOf('=') > -1) {
                    dataSinFormula[i][j] = hot.plugin.helper.cellValue(aCols[j] + (i + 1));
                }
                else {
                    dataSinFormula[i][j] = mydata[i][j];
                }
            }
        }
    }

}

function getTotalErrores() {
    nErrores = errores.length;
    total = 0;
    for (var i = 0; i < nErrores; i++) {
        total += errores[i].total;
    }
    return total;
}

function getTotBlancos(data) {
    var total = data.length;
    var totalBlancos = 0;
    for (var i = 1 ; i < total ; i++) {
        for (var j = 0; j < 4; j++) {
            dato = data[i][j];
            //console.log("Matriz " + "i:" + i + "  j:" + j + " valor:" + dato);
            if (dato == null) {
                console.log("i:" + i + "  j:" + j + " valor:" + dato);
                totalBlancos++;
            }
            else {
                if (dato.length == 0) {
                    //console.log("i:" + i + "  j:" + j + " valor:" + dato);
                    totalBlancos++;
                }
            }
        }
    }
    return totalBlancos;
}

function mostrarEnvioExcelWeb(envio) {
    $('#enviosanteriores').bPopup().close();
    $("#hfIdEnvio").val(envio);
    mostrarFormularioDisponibilidad(2)
    var mensaje = mostrarMensajeEnvio();
    mostrarExito(mensaje);
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

function agregarColumna() {
    var j = 0;
    for (var i = 0; i < grillaBD.length; i++) {
        j = grillaBD[i].length;
        grillaBD[i][j] = "";
    }
}

function agregarFilaHandson() {
    index = grillaBD.length;
    grillaBD.push([evtHot.ListaPtoMedicion[0].id, getHoraInicio(), "0", listaEstado[0].id, "", ""]);
    // grillaBD.push([evtHot.ListaPtoMedicion[0].id, listaFecha[0], getHoraInicio(), "0", listaEstado[0].id, ""]);
    hot.updateSettings({
        maxRows: index + 1,
        data: grillaBD
    });
    hot.render();
}


function eliminarFilaHandson() {
    grillaBD = hot.getData();
    grillaBD.pop();
    index = grillaBD.length;
    hot.updateSettings({
        maxRows: index - 1
    });
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

function validaExtremoTime(hora, fecha) {
    var valido = true;
    if (listaFecha[0] == fecha) {
        if (parseInt(hora.substr(0, 2)) < 6) {
            valido = false;
            return valido;
        }
    }
    else {
        if (parseInt(hora.substr(0, 2)) >= 6) {
            valido = false;
            return valido;
        }
    }
    return valido;
}

function mostrardetalleExcel(empresaNombre, fecha, fechanext) {
    var area = "<P><font color='#4682B4'>DISPONIBILIDAD DE GAS NATURAL DESDE LAS <b> 06:00 AM DEL " + fecha + "</b> HASTA LAS <b> 06:00 AM DEL  " + fechanext + " </b> DE LA EMPRESA <b>" + empresaNombre + "</b></font></P>"
    $('#selecFormato').css("display", "block");
    $("#selecFormato").html(area);
}