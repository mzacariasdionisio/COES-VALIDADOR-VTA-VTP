function loadInfoFile(id, fileName, fileSize) {
    $('#' + id).html(fileName + " (" + fileSize + ")");
    $('#' + id).removeClass('action-alert');
    $('#' + id).addClass('action-exito');
    $('#' + id).css('margin-bottom', '10px');
}

function loadValidacionFile(id, mensaje) {
    $('#' + id).html(mensaje);
    $('#' + id).removeClass('action-exito');
    $('#' + id).addClass('action-alert');
    $('#' + id).css('margin-bottom', '10px');
}

function mostrarProgreso(id, porcentaje) {
    $('#' + id).text(porcentaje + "%");
}

function mostrarMensajeDefecto() {
    mostrarMensaje('mensaje', 'message', 'Complete los datos solicitados.');
}

function mostrarMensaje(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

function limpiarMensaje(id) {
    $('#' + id).removeClass();
    $('#' + id).html('');
}

function getFecha(date) {
    var parts = date.split("/");
    var date1 = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date1.getTime();
}

function getDateTime(date) {
    var parts = date.split(' ');
    var fecha = parts[0].split("/");;
    var hora = parseInt(parts[1].split(":")[0]);
    var minuto = parseInt(parts[1].split(":")[1]);
    var segundo = parseInt(parts[1].split(":")[2]);

    var date1 = new Date(fecha[1] + "/" + fecha[0] + "/" + fecha[2]);
    return date1.getTime() + (segundo + minuto * 60 + 60 * 60 * hora) * 1000;
}

function getDatePart(date) {
    var parts = date.split(' ');
    return parts[0];
}

function validarNumero(texto) {
    return /^-?[\d.]+(?:e-?\d+)?$/.test(texto);
}

function validarEntero(texto) {
    return /^-?[0-9]+$/.test(texto);
}

function validarFecha(fecha) {
    return /^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$/.test(fecha);
}

function validarExcelEntero(texto) {
    if (texto == "" || validarEntero(texto)) return true;
    return false;
}

function validarExcelDecimalRango(texto) {
    if (texto == "") return true;
    if (validarNumero(texto) && (parseFloat(texto) >= 0 && parseFloat(texto) <= 1)) return true;
    return false;
}

function validarExcelEnteroRango(texto) {
    if (texto == "") return true;
    if (validarEntero(texto) && (parseInt(texto) >= 1 && parseInt(texto) <= 6)) return true;
    return false;
}

function validarRangoDecimal(texto) {    
    if ((parseFloat(texto) >= 0 && parseFloat(texto) <= 1)) return true;
    return false;
}

function validarEnteroRango(texto) {
    if (validarEntero(texto) && (parseInt(texto) >= 1 && parseInt(texto) <= 6)) return true;
    return false;
}

function validarExcelCorrelativoRango(texto) {
    if (texto == "" || validarCorrelativoRango(texto)) return true;
    return false;
}

function validarCorrelativoRango(texto) {
    if (validarEntero(texto) && (parseInt(texto) >= 0 && parseInt(texto) <= 99999)) return true;
    return false;
}

function validarExcelNumeroConDecimales(texto, decimales) {
    if (texto == "") return true;
    if (validarNumero(texto)) {
        if (contarDecimales(texto) <= decimales) {
            return true;
        }
    }
    return false;
}

function validarExcelNumeroConDecimalesPositivo(texto, decimales) {
    if (texto == "") return true;
    if (validarNumero(texto)) {
        if (contarDecimales(texto) <= decimales) {
            if (parseFloat(texto) < 0) return false;
            return true;
        }
    }
    return false;
}

function validarExcelNumeroConDecimalesEnteroPositivo(texto, enteros, decimales) {
    if (texto == "") return true;
    if (validarNumero(texto)) {
        if (contarDecimales(texto) <= decimales && contarEnteros(texto) <= enteros) {
            if (parseFloat(texto) < 0) return false;
            return true;
        }
    }
    return false;
}

function validarCantidadDecimales(texto, decimales) {
    if (contarDecimales(texto) <= decimales) {
        return true;
    }
    return false;
}

function validarNegativo(texto) {
    if (validarNumero(texto)) {     
        if (parseFloat(texto) < 0) return false;
        return true;
    }
    return false;
}

function validarExcelFecha(texto) {
    if (texto == "") return true;
    if (validarFechaHora(texto)) return true;
    return false;
}

function contarDecimales(value) {
    if (Math.floor(value) === value) return 0;    
    if (value.toString().split(".").length == 1) return 0;
    return value.toString().split(".")[1].length || 0;
}

function contarEnteros(value) {
    //if (Math.floor(value) === value) return 0;
    //if (value.toString().split(".").length == 1) return 0;
    return value.toString().split(".")[0].length || 0;
}

function validarExcelPorcentaje(texto) {
    if (texto == "") return true;
    if (validarNumero(texto)) {
        if (parseFloat(texto) >= 0 && parseFloat(texto) <= 100)
            return true;
    }
    return false;
}

function validarPorcentaje(texto) {
    if (validarNumero(texto)) {
        if (parseFloat(texto) >= 0 && parseFloat(texto) <= 100)
            return true;
    }
    return false;
}

function validarTexto(texto, longitud) {
    if (texto == "") return true;
    if (texto.length > longitud) return false;
    return true;
}

function validarTextoInsumo(texto, longitud) {
    if (texto == null || texto == "") return true;
    if (texto.length > longitud) return false;
    return true;
}

function validarFechaHora(fecha) {
    return /^([0-2][0-9]|3[0-1])(\/|-)(0[1-9]|1[0-2])\2(\d{4})(\s)([0-1][0-9]|2[0-3])(:)([0-5][0-9])(:)([0-5][0-9])$/.test(fecha);
}

function obtenerColumnas(data) {   
    var html = "<table class='pretty tabla-adicional' id='tablaColumnas'>";
    html = html + " <thead>";
    html = html + "     <tr>";
    html = html + "         <th><input type='checkbox' id='cbSelectAll' checked='checked'></th>";
    html = html + "         <th>Columna</th>";
    html = html + "     </tr>";
    html = html + " </thead>";
    html = html + " <tbody>";
    for (var k in data) {
        if (k > 1 && data[k] != '') {
            html = html + "     <tr>";
            html = html + "         <td><input type='checkbox' checked='checked' value='" + k + "' /></td>";
            html = html + "         <td>" + data[k] + "</td>";           
            html = html + "     </tr>";
        }
    }
    html = html + " </tbody>";
    html = html + "</table>";

    return html;
}

function obtenerHtmlEnvios(result) {
    var html = "<table class='pretty tabla-adicional' id='tablaEnvios'>";
    html = html + " <thead>";
    html = html + "     <tr>";
    html = html + "         <th>ID Envío</th>";
    html = html + "         <th>Usuario</th>";
    html = html + "         <th>Fecha</th>";
    html = html + "         <th>Plazo</th>";
    html = html + "         <th>Comentario</th>";
    html = html + "     </tr>";
    html = html + " </thead>";
    html = html + " <tbody>";
    for (var k in result) {
        var comentario = "";
        var enPlazo = "En plazo";
        if (result[k].Reenvplazo == "No") enPlazo = "Fuera de plazo";
        if (result[k].Reenvcomentario != null) comentario = result[k].Reenvcomentario;
        html = html + "     <tr>";       
        html = html + "         <td>" + result[k].Reenvcodi + "</td>";
        html = html + "         <td>" + result[k].Reenvusucreacion + "</td>";
        html = html + "         <td>" + result[k].ReenvfechaDesc + "</td>";
        html = html + "         <td>" + enPlazo + "</td>";
        html = html + "         <td>" + comentario + "</td>";
        html = html + "     </tr>";
    }
    html = html + " </tbody>";
    html = html + "</table>";

    return html;
}

function compararArreglo(arregloA, arregloB) {
    //- Quitamos id y evidencia   
    for (var k = 2; k < arregloA.length - 1; k++) {        
        if (arregloA[k] != arregloB[k]) {
            return false;
        }
    }
    return true;
}

var Validacion = {
    DatoObligatorio: "El dato es obligatorio",
    FormatoEntero: "El dato debe ser un valor entero",
    FormatoEnteroRango: "El valor debe encontrarse entre 1 y 6",
    FormatoNumero: "El dato debe ser numérico",
    FormatoDecimal: "La cantidad máxima de decimales es ",
    FormatoDecimalRango: "Los valores deben estar entre 0 y 1",
    FormatoFechaHora: "El dato debe tener el formato dd/mm/yyyy hh:mm:ss",
    FormatoPorcentaje: "El dato debe tener formato numérico y debe estar entre 0 y 100",
    ValorNiIncorrecto: "El valor de Ni no corresponde a la causa de interrupción seleccionada",
    ValorKiIncorrecto: "El valor de Ki no corresponde a la causa de interrupción seleccionada",
    ValorNTIncorrecto: "El nivel de tensión no corresponde al punto de entrega",
    TextoCausaResumida: "La longitud máxima es de 300 caracteres",
    TextoNroSuministro: "La longitud máxima es de 30 caracteres",
    TextoPuntoEntrega: "La longitud máxima es de 100 caracteres",
    SumaPorcentajes: "Los porcentajes deben sumar 100%",
    ValidacionFechaProgramada: "Debe ingresar el tiempo programado inicio y fin",
    FechaEjecutadaFinal: "La fecha final ejecutada debe ser menor o igual a la fecha final programada",
    FechaEjecutadaInicial: "La fecha inicial ejecutada debe ser mayor o igual a la fecha inicial programada",
    DuracionEvento: "La duracion del evento debe ser superior a 3 minutos",
    DuracionEventoPE: "La duracion del evento debe ser mayor a 0 minutos",
    DuracionEventoRC: "La duracion del evento debe ser mayor o igual a 0 minutos",
    ResarcimimientoRCCero: "El resarcimiento debe ser 0 para los eventos con duración menores a 3 minutos",
    DuracionProgramada: "La fecha final programada debe ser mayor a la fecha inicial programada",
    FechaProgramadaInicio: "Debe ingresar la fecha de inicio programada",
    FechaProgramadaFin: "Debe ingresar la fecha de inicio programada",
    TipoCausaInterrupcion: "La causa no corresponde al tipo de interrupción",
    RegistroDuplicado: "Los valores de la fila se repiten con los valores de la fila ",
    ValidacionPtoEntrega: "Los valores de las columnas Nivel de Tensión, Aplicación Literal, Energía Semestral e Incremento de tolerancias deben ser iguales a la fila ",
    ValidacionArchivo: "Debe agregar archivo de sustento para las causas FM FUNDADO o FM TRAMITE",
    IngresoReponsablePorcentaje: "Debe ingresar responsable y su respectivo porcentaje",
    TextoLongitudAlimentadorSED: "La longitud máxima es de 200 caracteres",
    TextoLongitudComentario: "La longitud máxima es de 300 caracteres",
    FechaInicioEvento: "La fecha debe coincidir con la fecha del Evento COES",
    ArchivoSustentoObligatorio: "Debe cargar el archivo de sustento",
    TextoIngresoObservacion: "Debe ingresar la observación",
    ConformidadSuministradorObligatorio: "El dato de Conformidad del Suministrador es obligatorio",
    ComentarioSuministradorObligatorio: "El comentario del Suministrador es obligatorio",
    IngresoResponsablesOrden: "Debe ingresar los responsables de forma ordenada",
    FormatoDecimalNegativo: "No se permiten valores negativos",
    LongitudEntero: "El campo supera los doce dígitos",
    LongitudDecimal: "El campo supera el máximo permitido",
    DatoObligatorioCliente: "Dato debe existir en la base de datos del COES y es obligatorio",
    TraslapeInterrupcion: "Existe traslape con la interrupción de la fila ",
    ValidarCorrelativo: "Ingresar valores entre 0 y 99999"
};