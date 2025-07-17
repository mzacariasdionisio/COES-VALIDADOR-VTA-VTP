var controlador = siteRoot + 'RDO/DisponibilidadCombustible/'

var hot;
var evtHot;
var listaStockinicio = [];
var listaObsrev = [];
var listaPtos = null;
var listaPtosStock = null;
var listaFecha = null;

var listErrores = [];
var nfilConsumo = 0;
var nfilStock = 0;
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
var columnas = [
    {
        central: 0,
        tipo: 1,
        unidad: 2,
        inicial: 3,
        recepcion: 4,
        total: 5,
        final: 6,
        declarado: 7,
        observacion: 8
    },
    {
        central: 0,
        tipo: 1,
        unidad: 2,
        medidor: 3,
        consumo: 4,
        total: 5,
        fin1: 6,
        fin2: 7
    }
];

var listaTipo = [
    {
        id: 1,
        text: "Quema de Gas"
    },
    {
        id: 2,
        text: "Venteo de Gas"
    }
];

var listaEstado = [
    {
        id: 1,
        text: "Declaró"
    },
    {
        id: 2,
        text: "Renominó"
    }
];
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

function validarSeleccionDatos() {
    if (!($('#hfEmpresa').val() == $('#cbEmpresa').val() && $('#txtFecha').val() == $('#hfFecha').val())) {
        return false;
    }
    return true;
}

function mostrarMensajeEnvio(idEnvio) {

    var envio = $("#hfIdEnvio").val();
    if (envio > 0) {
        var plazo = (evtHot.EnPlazo) ? "<strong style='color:green'>en plazo</strong>" : "<strong style='color:red'>fuera de plazo</strong>";
        var mensaje = "<strong>Código de envío</strong> : " + evtHot.IdEnvio + "   , <strong>Fecha de envío: </strong>" + evtHot.FechaEnvio + "   , <strong>Enviado en </strong>" + plazo;
        return mensaje;
    }
    else {
        var esEmpresaVigente = evtHot.EsEmpresaVigente;
        if (esEmpresaVigente) {
            if (evtHot.TipoPlazo != null && evtHot.TipoPlazo != "") {
                if (evtHot.TipoPlazo != "D") {
                    var mensaje = "<strong style='color:green'>En plazo</strong>";;
                    if (evtHot.TipoPlazo != "P") mensaje = "<strong style='color:red'>Fuera de plazo</strong>";
                    return "Por favor complete los datos. <strong>Plazo del Envio: </strong>" + mensaje;
                } else {
                    var mensaje = "<strong style='color:blue'>Deshabilitado</strong>";
                    return "No está permitido el envió de información, solo para Consulta. <strong>Plazo del Envio: </strong>" + mensaje;
                }

            } else {
                if (!evtHot.EnPlazo) {
                    return "<strong style='color:red'>Fuera de plazo</strong>";
                }
                else return "<strong style='color:green'>En plazo</strong>";
            }
        } else {
            var msjNoVigente = "La empresa se encuentra <strong style='color:blue'>No Vigente</strong>.";
            var mensaje = "<strong style='color:blue'>Deshabilitado</strong>";
            return msjNoVigente + " No está permitido el envió de información, solo para consulta.";
        }
    }
}

function leerFileUpExcel() {
    var retorno = 0;
    $.ajax({
        type: 'POST',
        url: controlador + 'LeerFileUpExcel',
        dataType: 'json',
        async: false,
        data: {
            idEmpresa: $('#hfEmpresa').val(),
            dia: $('#hfFecha').val()
        },
        success: function (res) {
            retorno = res;
        },
        error: function () {
            mostrarError("Ha ocurrido un error");
        }
    });
    return retorno;
}

function limpiarError() {
    totLimInf = 0;
    totLimSup = 0;
    totNoNumero = 0;
    listErrores = [];
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

function getTotalErrores() {
    nErrores = errores.length;
    total = 0;
    for (var i = 0; i < nErrores; i++) {
        total += errores[i].total;
    }
    return total;
}

function validaTime(str) {
    hora = str;//.value;
    if (hora == '') {
        return false;
    }
    if (hora.length > 8) {
        return false;
    }
    if (hora.length != 8) {
        return false;
    }
    a = hora.charAt(0); //<=2
    b = hora.charAt(1); //<4
    c = hora.charAt(2); //:
    d = hora.charAt(3); //<=5
    e = hora.charAt(5); //:
    f = hora.charAt(6); //<=5
    if ((a == 2 && b > 3) || (a > 2)) {
        //alert("El valor que introdujo en la Hora no corresponde, introduzca un digito entre 00 y 23");
        return false;
    }
    if (d > 5) {
        //alert("El valor que introdujo en los minutos no corresponde, introduzca un digito entre 00 y 59");
        return false;
    }
    if (f > 5) {
        //alert("El valor que introdujo en los segundos no corresponde");
        return false;
    }
    if (c != ':' || e != ':') {
        //alert("Introduzca el caracter ':' para separar la hora, los minutos y los segundos");
        return false;
    }
    return true;
}

function getHoraInicio() {
    var d = new Date();
    var hora = d.getHours();//Math.floor(d.getHours() / 3) * 3;
    var h = ("0" + hora).slice(-2);
    var minuto = d.getMinutes();// Math.floor(d.getMinutes() / 3) * 3;
    var m = ("0" + minuto).slice(-2);
    var segundo = d.getSeconds();
    var s = ("0" + segundo).slice(-2);
    return h + ":" + m + ":" + s;
}

function habilitarEdicionEnvio() {
    if (evtHot.TipoPlazo != null && evtHot.TipoPlazo != "") {
        if (evtHot.TipoPlazo == "D") {
            $("#btnEditarEnvio").parent().hide();
            $("#btnEnviarDatos").parent().hide();
            $("#btnMostrarErrores").parent().hide();            
        }
    }
}

function deshabilitarEdicionEnvio() {
    var idEnvio = parseInt($("#hfIdEnvio").val()) || 0;
    if (idEnvio == 0) {
        $("#btnEditarEnvio").parent().hide();
    }
}