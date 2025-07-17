var hot;
var evtHot;
var validaDataCongelada = false;
var listaPtos = null;
var listaFecha = null;
var listaSize = [];
var colsToHide = [];
var listaGrupo = [];
var listaGrupoAux = [];
var listaBloqueMantos = [];
var listErrores = [];
var manttos = [];
var eventos = [];
var filasCab = 0;
var grillaBD;
var modoLectura = false;
var consulta = 1;
var envioAnterior = 3;
var envioDatos = 2;
var importarDatos = 5;
var envioEstado = 6;
var errorNoNumero = 2;
var errorLimInferior = 3;
var errorLimSuperior = 4;
var errorRangoFecha = 5;
var errorCrucePeriodo = 6;
var errorTime = 7;
var errorExtremoTime = 8;
var errorUnidad = 9;
var errorDespacho = 10;

var errores = [
    {
        tipo: 'BLANCO',
        Descripcion: 'BLANCO',
        total: 0,
        codigo: 0,
        BackgroundColor: '',
        Color: ''
    },
    {
        tipo: 'NUMERO',
        Descripcion: 'NÚMERO',
        total: 0,
        codigo: 1,
        BackgroundColor: 'white',
        Color: ''
    },
    {
        tipo: 'NONUMERO',
        Descripcion: 'El dato no es númerico',
        total: 0,
        codigo: 2,
        BackgroundColor: 'red',
        Color: ''
    },
    {
        tipo: 'LIMINF',
        Descripcion: "El dato es menor que el límite inferior",
        total: 0,
        codigo: 3,
        BackgroundColor: 'orange',
        Color: ''
    },
    {
        tipo: 'LIMSUP',
        Descripcion: 'El dato es mayor que el límite superior',
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
        },
    {
        tipo: 'ERRUNIDAD',
        Descripcion: "Unidades de MW y MVar no estan completadas",
        total: 0,
        codigo: 9,
        BackgroundColor: '#86c2ad',
        Color: ''
    },
        {
            tipo: 'ERRDESPACHO',
            Descripcion: "El dato no esta completado",
            total: 0,
            codigo: 10,
            BackgroundColor: '#bfdfe8',
            Color: ''
        }
];

var listaCongelados = [];
var listaJustificacion = [
    {
        Id: -1,
        Justificacion: " -- Seleccionar --"
    },
    {
        Id: 0,
        Justificacion: "Por Mantenimiento"
    },
    {
        Id: 1,
        Justificacion: "Fuera de Servicio"
    },
    {
        Id: 2,
        Justificacion: "Causa Evento"
    }
];
imagenOk = "<img src='/Content/Images/ico-done.gif'/>"; //imagenOk = "<img src='" + initialURL + "Content/Images/ico-done.gif'/>";
imagenError = "<img src='/Content/Images/ico-delete.gif'/>";

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
            case errorUnidad:
                errores[errorUnidad].total++;
            case errorDespacho:
                errores[errorDespacho].total++;
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

function popUpListaJustificaciones() {
    $('#idJustificaciones').html(dibujarTablaJustificacion());
    setTimeout(function () {
        $('#justificaciones').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
        $('#tablaJustificaciones').dataTable({
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
        var mensaje = "<strong>Código de envío</strong> : " + idEnvio + "   , <strong>Fecha de envío: </strong>" + evtHot.FechaEnvio + "   , <strong>Enviado en </strong>" + plazo;
        return mensaje;
    }
    else {
        if (!evtHot.EnPlazo) {
            return "<strong style='color:red'>Fuera de plazo</strong>";
        }
        else return "<strong style='color:green'>En plazo</strong>";
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
            fecha: getFecha(),
            semana: getSemana(),
            mes: getMes(),
            idFormato: getIdFormato()
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
    if (num == 0) {
        var cerosDer = '';
        while (cerosDer.length < casasDec)
            cerosDer = '0' + cerosDer;

        return "0" + sepDecimal + cerosDer;
    }

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

function validarDataCongelada() {

    var columnas = listaPtos.length;
    var filas = evtHot.Handson.ListaExcelData.length;//48;
    var data = hot.getData();
    var valorAnterior = -100;
    var datosCongelados = 0;
    var inicioCongelado = 0;
    var finCongeladoFila = 0;
    var finCongeladoCol = 0;
    listaCongelados = [];
    for (var i = 1; i <= columnas ; i++) {
        for (var j = filasCab; j < filas; j++) {
            valor = data[j][i];
            if (valor == valorAnterior && valor != "") {
                datosCongelados++;
                if (datosCongelados == 1) {
                    inicioCongelado = j;
                    var congelado = {
                        Ptomedicodi: listaPtos[i - 1].Ptomedicodi,
                        Tipoinfocodi: listaPtos[i - 1].Tipoinfocodi,
                        Rango: getExcelColumnName(i + 1) + j + ":",
                        Periodo: data[j][0] + " hasta ",
                        Empresa: data[1][i],
                        Subestacion: data[3][i],
                        Equipo: data[4][i],
                        Unidad: data[6][i],
                        Justificacion: -1,
                        Texto: "",
                        Valor: valor,
                        FechaInicio: getDateCongelado(data[j][0], filas, j)
                    };
                }
            }
            else {
                if (datosCongelados > 3) {
                    finCongeladoCol = i;
                    finCongeladoFila = j - 1;
                    valorAnterior = -100;
                    congelado.Rango += getExcelColumnName(finCongeladoCol + 1) + finCongeladoFila;
                    congelado.Periodo += data[finCongeladoFila][0];
                    congelado.FechaFin = getDateCongelado(data[finCongeladoFila][0], filas, finCongeladoFila);
                    listaCongelados.push(congelado);
                }
                datosCongelados = 0;
            }
            valorAnterior = valor;

        }

        //si se termina el pto de medicion entonces guardar los congelados que hubiesen
        if (datosCongelados > 3) {
            finCongeladoCol = i;
            finCongeladoFila = j - 1;
            valorAnterior = -100;
            congelado.Rango += getExcelColumnName(finCongeladoCol + 1) + finCongeladoFila;
            congelado.Periodo += data[finCongeladoFila][0];
            congelado.FechaFin = getDateCongelado(data[finCongeladoFila][0], filas, finCongeladoFila);
            listaCongelados.push(congelado);
        }
        datosCongelados = 0;
    }

    return listaCongelados.length;
}

function dibujarTablaJustificacion() {
    listaData = evtHot.ListaCongeladoByEnvio;
    listaCombo = evtHot.ListaCausaJustificacion;
    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablaJustificaciones' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><th>Periodo</th><th>Empresa</th><th>Subestación</th><th>Equipo</th><th>Justificación</th><th style='width:260px'>Descripción de Justificación</th></tr></thead>";
    cadena += "<tbody>";
    var len = listaData.length;
    for (var i = 0 ; i < len ; i++) {
        cadena += "<tr>";
        cadena += "<td>" + listaData[i].Periodo + "</td>";
        cadena += "<td>" + listaData[i].Empresa + "</td>";
        cadena += "<td>" + listaData[i].Subestacion + "</td>";
        cadena += "<td>" + listaData[i].Equipo + "</td>";
        cadena += "<td><select disabled='disabled' id='justCmb" + i + "' style='width:200px;' >";
        for (var z = 0; z < listaCombo.length; z++) {
            if (listaData[i].Justificacion == listaCombo[z].Subcausacodi) {
                cadena += "<OPTION VALUE='" + listaCombo[z].Subcausacodi + "'>" + listaCombo[z].Subcausadesc + "</OPTION>";
            }
        }
        if (listaData[i].Justificacion == 0) {
            cadena += "<OPTION VALUE='0'>Otro</OPTION>";
            cadena += "</select></td>";
            cadena += '<td><input style="width:250px;" disabled="disabled" type="text" id="texto' + i + '" value="' + listaData[i].Texto + '"/></td>';
        } else {
            cadena += "</select></td>";
            cadena += '<td></td>';
        }
    }
    cadena += "</tbody></table>";

    return cadena;
}

function dibujarTablaCongelados() {

    lista = evtHot.ListaCausaJustificacion;
    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablaCongelados' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Rango</th><th>Periodo</th><th>Empresa</th><th>Subestación</th><th>Equipo</th><th>Dato Congelado</th><th>Unidad</th><th>Justificación</th><th style='width:260px'>Descripción de Justificación</th><th>Estado</th></tr></thead>";
    cadena += "<tbody>";
    var len = listaCongelados.length;
    for (var i = 0 ; i < len ; i++) {
        cadena += "<tr><td>" + listaCongelados[i].Rango + "</td>";
        cadena += "<td>" + listaCongelados[i].Periodo + "</td>";
        cadena += "<td>" + listaCongelados[i].Empresa + "</td>";
        cadena += "<td>" + listaCongelados[i].Subestacion + "</td>";
        cadena += "<td>" + listaCongelados[i].Equipo + "</td>";
        cadena += "<td>" + number_format(listaCongelados[i].Valor, 3) + "</td>";
        cadena += "<td>" + listaCongelados[i].Unidad + "</td>";
        cadena += "<td><select id='justCmb" + i + "' style='width:200px;' onChange='justificarCongelada(" + i + ",this.options[this.selectedIndex].value)'>";
        cadena += "<OPTION VALUE='-1'>  - Seleccionar - </OPTION>";
        for (var z = 0; z < lista.length; z++) {
            cadena += "<OPTION VALUE='" + lista[z].Subcausacodi + "'>" + lista[z].Subcausadesc + "</OPTION>";
        }
        cadena += "<OPTION VALUE='0'>Otro</OPTION>";
        cadena += "</select></td>";
        cadena += '<td><input style="width:250px;display: none" disabled="disabled" onchange="justificarCongelada2(' + i + ')" type="text" id="texto' + i + '"/></td>';
        cadena += "<td id='seljust" + i + "'>" + imagenError + "</td></tr>";
    }
    cadena += "</tbody></table>";
    cadena += "<div><input type='button' id='btnEnviar' value='Envíar' onclick='enviarCongelados();' />";
    cadena += "<input type='button' id='btnCancelar' onclick='offshowView();'  value='Cancelar' /></div>";

    return cadena;
}

function offshowView() {
    $('#congelados').bPopup().close();
}

function enviarCongelados() {
    var totalCongelados = listaCongelados.length;
    var justificados = 0;
    for (var i = 0 ; i < totalCongelados; i++) {
        if (listaCongelados[i].Justificacion >= 0) {
            justificados++;
        }
    }
    if (justificados == totalCongelados) {
        offshowView();
        envioData();
    }
    else {
        alert("Debe justificar todos los rangos de datos congelados para realizar el envío");
    }
}

function mostrarDetalleCongelados() {
    $('#idCongelados').html(dibujarTablaCongelados());
    setTimeout(function () {
        $('#congelados').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
        $('#tablaCongelados').dataTable({
            "scrollY": 330,
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });

    }, 50);
}

function justificarCongelada(fila, tipoJustif) {
    textoJust = '';
    $("#texto" + fila).hide();
    $("#texto" + fila).attr('disabled', 'disabled');
    if (tipoJustif == -1) {
        $("#seljust" + fila).html(imagenError);
    }
    else {
        if (tipoJustif == 0) {
            $("#texto" + fila).show();
            $("#texto" + fila).removeAttr('disabled');
            textoJust = $("#texto" + fila).val();
            textoJust = textoJust === undefined ? '' : textoJust.trim();
            if (textoJust == '') {
                $("#seljust" + fila).html(imagenError);
            } else {
                $("#seljust" + fila).html(imagenOk);
            }
        } else {
            $("#seljust" + fila).html(imagenOk);
        }
    }
    listaCongelados[fila].Justificacion = tipoJustif;
    listaCongelados[fila].Texto = textoJust;
}

function justificarCongelada2(fila) {
    textoJust = '';
    tipoJustif = $("#justCmb" + fila).val();
    $("#texto" + fila).hide();
    $("#texto" + fila).attr('disabled', 'disabled');

    if (tipoJustif == 0) {
        $("#texto" + fila).show();
        $("#texto" + fila).removeAttr('disabled');
        textoJust = $("#texto" + fila).val();
        textoJust = textoJust === undefined ? '' : textoJust.trim();
        if (textoJust == '') {
            $("#seljust" + fila).html(imagenError);
        } else {
            $("#seljust" + fila).html(imagenOk);
        }
    } else {
        $("#seljust" + fila).html(imagenOk);
    }
    listaCongelados[fila].Justificacion = tipoJustif;
    listaCongelados[fila].Texto = textoJust;
}

function number_format(number, decimal_pos, decimal_sep, thousand_sep) {
    //number = parseFloat(numberstr);
    var ts = (thousand_sep == null ? ',' : thousand_sep)
        , ds = (decimal_sep == null ? '.' : decimal_sep)
        , dp = (decimal_pos == null ? 2 : decimal_pos)

        , n = Math.abs(Math.ceil(number)).toString()

        , i = n.length % 3
        , f = n.substr(0, i)
    ;

    if (number < 0) f = '-' + f;

    for (; i < n.length; i += 3) {
        if (i != 0) f += ts;
        f += n.substr(i, 3);
    }

    if (dp > 0)
        f += ds + parseFloat(number).toFixed(dp).split('.')[1]

    return f;
}

//funciones para obtener los distintos campos
function getSemana() {
    $('#hfSemana').val($('#cbSemana').val());
    $('#hfAnho').val($('#Anho').val());
    semana = $("#hfSemana").val();
    anho = $('#Anho').val();

    if (semana !== undefined && anho !== undefined) {
        semana = anho.toString() + semana;
    } else {
        semana = '';
    }

    return semana;
}

function getFecha() {
    $('#hfFecha').val($('#txtFecha').val());
    fecha = $("#hfFecha").val();
    fecha = fecha !== undefined ? fecha : '';
    return fecha;
}

function getMes() {
    $("#hfMes").val($("#txtMes").val());
    mes = $("#hfMes").val();
    mes = mes !== undefined ? mes : '';
    return mes;
}

function getIdFormato() {
    formato = $("#cbTipoFormato").val();
    formato = formato !== undefined ? formato : 0;
    if (formato == 0) {
        formato2 = $("#hfFormato").val();
        formato = formato2 !== undefined ? formato2 : 0;
    }
    return formato;
}

function getDateCongelado(data, totalFila, fila) {
    minRestar = evtHot.Formato.Formatresolucion;

    anio = data.substring(6, 10);
    mes = data.substring(3, 5);
    dia = data.substring(0, 2);

    hora = data.substring(11, 16) + ':00';

    fechaDate = anio + '/' + mes + '/' + dia + ' ' + hora;
    res = new Date(fechaDate);

    return res;
}

//Leyenda
function mostrarDetalleLeyenda() {
    $('#idLeyenda').html(dibujarTablaLeyenda());
    setTimeout(function () {
        $('#leyenda').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
        $('#tablaLeyenda').dataTable({
            "scrollY": 330,
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });

    }, 50);
}

function dibujarTablaLeyenda() {

    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablaLeyenda' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Celda</th><th>Descripción</th></tr></thead>";
    cadena += "<tbody>";

    var erroresApp = getLeyenda();

    var len = erroresApp.length;
    for (var i = 0 ; i < len ; i++) {
        cadena += "<tr>";
        cadena += '     <td style="background-color: ' + erroresApp[i].BackgroundColor + ' !important; color: white;"></td>';
        cadena += "<td style='text-align: left;'>" + (erroresApp[i].codigo > 0 ? 'Error: ' : ' ') + erroresApp[i].Descripcion + "</td></tr>";
    }
    cadena += "</tbody></table>";
    return cadena;
}

function getLeyenda() {
    var listaErroresApp = [];

    for (var i = 0; i < errores.length; i++) {
        switch (errores[i].codigo) {
            case errorNoNumero:
            case errorLimInferior:
            case errorLimSuperior:
                listaErroresApp.push(errores[i]);
                break;
            case errorDespacho:
                if (typeof VALIDAR_CERO_BLANCO != 'undefined' && VALIDAR_CERO_BLANCO === true) {
                    listaErroresApp.push(errores[i]);
                }
                break;
            case errorUnidad:
                if (typeof VALIDAR_UNIDAD != 'undefined' && VALIDAR_UNIDAD === true) {
                    listaErroresApp.push(errores[i]);
                }
                break;
        }
    }

    if (evtHot != undefined && evtHot != null) {
        if ((evtHot.ValidaEventos != undefined && evtHot.ValidaEventos != null && evtHot.ValidaEventos)
            || (evtHot.ValidaRestricOperativa != undefined && evtHot.ValidaRestricOperativa != null && evtHot.ValidaRestricOperativa)) {
            var validacion = {
                Descripcion: 'VALIDACION DE EVENTOS: En este rango existe un evento que indispone la unidad de generación.',
                BackgroundColor: 'Salmon'
            };
            listaErroresApp.push(validacion);
        }

        if ((evtHot.ValidaMantenimiento != undefined && evtHot.ValidaMantenimiento != null && evtHot.ValidaMantenimiento)) {
            var validacion = {
                Descripcion: 'VALIDACION DE MANTENIMIENTOS: En este rango el equipo se encuentra en mantenimiento. ',
                BackgroundColor: 'MediumPurple'
            };
            listaErroresApp.push(validacion);
        }

        if (evtHot.ValidaHorasOperacion != undefined && evtHot.ValidaHorasOperacion != null && evtHot.ValidaHorasOperacion) {
            var validacion = {
                Descripcion: 'HORAS DE OPERACION: En este rango El equipo no tiene horas de operación registradas.',
                BackgroundColor: 'Silver'
            };
            listaErroresApp.push(validacion);
        }
    }

    return listaErroresApp;
}