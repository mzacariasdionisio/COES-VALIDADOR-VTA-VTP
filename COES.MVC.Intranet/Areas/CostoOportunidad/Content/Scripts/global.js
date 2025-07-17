var controlador = siteRoot + 'DespachoDiario/envio/'

var hot;
var evtHot;
var listaObsrev = [];
var listaPtos = null;
var listaFecha = null;
var listErrores = [];
var grillaBD;
var modoLectura = false;
var consulta = 1;
var envioAnterior = 3;
var envioDatos = 2;
var importarDatos = 5;
var errorNoNumero = 2;
var errorLimInferior = 3;
var errorLimSuperior = 4;
var errorRangoFecha = 5;
var errorCrucePeriodo = 6;
var errorTime = 7;
var errorExtremoTime = 8
var maxCadena = 255;

var errores = [
    {
        tipo: 'BLANCO',
        Descripcion: 'BLANCO',
        total: 0,
        codigo: 0,
        Background_color: '',
        Color: ''
    },
    {
        tipo: 'NUMERO',
        Descripcion: 'NÚMERO',
        total: 0,
        codigo: 1,
        Background_color: 'white',
        Color: ''
    },
    {
        tipo: 'NONUMERO',
        Descripcion: 'NO NÚMERO',
        total: 0,
        codigo: 2,
        BackgroundColor: 'red',
        Color: ''
    },
    {
        tipo: 'LIMINF',
        Descripcion: "LIM. INFERIOR",
        total: 0,
        codigo: 3,
        BackgroundColor: 'orange',
        Color: ''
    },
    {
        tipo: 'LIMSUP',
        Descripcion: 'LIMITE SUPERIOR',
        total: 0,
        codigo: 4,
        BackgroundColor: 'yellow',
        Color: ''
    },
    {
        tipo: 'RANFEC',
        Descripcion: 'RANGO DE FECHA INVALIDO',
        total: 0,
        codigo: 5,
        BackgroundColor: '#FF4C42',
        Color: 'white'
    },
    {
        tipo: 'CRUPER',
        Descripcion: 'CRUCE EN PERIODOS',
        total: 0,
        codigo: 6,
        BackgroundColor: 'Wheat',
        Color: 'black'
    },
    {
        tipo: 'ERRTIME',
        Descripcion: 'TIME INVALIDO',
        total: 0,
        codigo: 7,
        BackgroundColor: 'SandyBrown',
        Color: 'black'
    },
        {
            tipo: 'ERREXTREMOTIME',
            Descripcion: 'FECHA FUERA DE RANGO',
            total: 0,
            codigo: 7,
            BackgroundColor: 'MediumTurquoise',
            Color: 'black'
        }
];

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

function getFormattedDate2(date) {
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

        //return year + '/' + month + '/' + day + " " + strTime;
        return year + '/' + month + '/' + day;
    }
    else {
        return "No es fecha";
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

function dibujarTablaError() {

    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablaError' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Celda</th><th>Valor</th><th>Tipo Error</th></tr></thead>";
    cadena += "<tbody>";
    var len = listErrores.length;
    console.log("Dibujo:" + len);
    for (var i = 0 ; i < len ; i++) {
        cadena += "<tr><td>" + listErrores[i].Celda + "</td>";
        cadena += "<td>" + listErrores[i].Valor + "</td>";
        cadena += "<td>" + errores[listErrores[i].Tipo].Descripcion + "</td></tr>";
    }
    cadena += "</tbody></table>";
    return cadena;
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
            case errorTime:
                errores[errorTime].total--;
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
            case errorTime:
                errores[errorTime].total++;
                break;
            case errorExtremoTime:
                errores[errorExtremoTime].total++;
                break;
        }
    }
}

function popUpListaEnvios() {
    $('#idEnviosAnteriores').html(dibujarTablaEnvios(evtHot.ListaEnvios));
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

function popUpListaReprogramados() {
    $('#idReprogramas').html(dibujarTablaReprogramas(evtHot.ListaReprograma));
    setTimeout(function () {
        $('#reprogramas').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
        $('#tablareprog').dataTable({
            "scrollY": 330,
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });
    }, 50);
}

function popUpListaPorcentajeRpf() {
    $('#idPorcentajeRpf').html(dibujarTablaPorcentajeRpf(evtHot.ListaPotenciaEfec));
    setTimeout(function () {
        $('#porcentajeRpf').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
        $('#tablaporcentajeRpf').dataTable({
            "scrollY": 330,
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });
    }, 50);
}

function dibujarTablaReprogramas(lista) {
    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablareprog' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Hora Inicio</th><th>Reprog</th><th>Causa</th></tr></thead>";
    cadena += "<tbody>";

    for (key in lista) {
       // var javaScriptDate = new Date(parseInt(lista[key].Enviofecha.substr(6)));
       // cadena += "<tr onclick='mostrarEnvioExcelWeb(" + lista[key].Enviocodi + ");' style='cursor:pointer'><td>" + lista[key].Enviocodi + "</td>";
        //cadena += "<td>" + getFormattedDate(javaScriptDate) + "</td>";
        cadena += "<td>" + convetirHoraMin(lista[key].Mailbloquehorario - 1) + "</td>";
        cadena += "<td>" + lista[key].Mailhoja + "</td>";
        cadena += "<td>" + lista[key].Mailreprogcausa + "</td></tr>";
    }
    cadena += "</tbody></table>";
    return cadena;
}

function dibujarTablaPorcentajeRpf(lista) {
    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablaporcentajeRpf' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Fecha</th><th>Porcentaje</th></tr></thead>";
    cadena += "<tbody>";
    
    for (key in lista) {
        var fechaDat = new Date(parseInt(lista[key].Fechadat.substr(6)));
        //cadena += "<td>" + lista[key].Centralnomb + "</td>";
        cadena += "<td>" + getFormattedDate2(fechaDat) + "</td>";
        cadena += "<td>" + lista[key].Formuladat + "</td></tr>";
    }
    cadena += "</tbody></table>";
    return cadena;
}

function convetirHoraMin(horamin) {
    var hora = "0" + Math.floor((parseInt(horamin) + 1) / 2).toString();
    hora = hora.substring((hora.length > 2) ? 1 : 0, ((hora.length > 2) ? 1 : 0) + 2);
    var minuto = "0" + (Math.floor((parseInt(horamin) + 1) % 2) * 30).toString();
    minuto = minuto.substring((minuto.length > 2) ? 1 : 0, ((minuto.length > 2) ? 1 : 0) + 2);
    return hora + ":" + minuto;
}