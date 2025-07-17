var controlador = siteRoot + 'hidrologia/cargadatos/';
var uploader;
var totNoNumero = 0;
var totLimSup = 0;
var totLimInf = 0;
var listErrores = [];
var listDescripErrores = ["BLANCO", "NÚMERO", "No es número", "Valor negativo", "Supera el valor máximo"];
var listValInf = [];
var listValSup = [];
var validaInicial = true;
var hot;
var hotOptions;
var evtHot;

$(function () {

    $('#Anho').Zebra_DatePicker({
        format: 'Y',
        onSelect: function () {
            cargarSemanaAnho()
        }
    });
    $('#txtFecha').Zebra_DatePicker({
    });

    $('#cbHorizonte').change(function () {
        horizonte();
        //pintaDescripcion($('#cbFormato').val())
    });

    $('#btnConsultar').click(function () {
        cargarExcelWeb();
    });

    $('#btnDescargarFormato').click(function () {
        if (validarSeleccionDatos()) {
            descargarFormato();
        }
        else {
            alert("Por favor no cambie las opciones.");
        }
    });


    $('#btnEnviarDatos').click(function () {
        if (validarSeleccionDatos()) {
            enviarDatos();
        }
        else {
            alert("Por favor no cambie las opciones.");
        }
    });

    $('#btnMostrarErrores').click(function () {
        if (validarSeleccionDatos()) {
            mostrarErrores();
        }
        else {
            alert("Por favor no cambie las opciones.");
        }
    });

    $('#btnVerEnvios').click(function () {
        if (validarSeleccionDatos()) {
            popUpListaEnvios();
        }
        else {
            alert("Por favor no cambie las opciones.");
        }
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

    horizonte();
    cargarSemanaAnho();
    crearPupload();
});

function crearHojaWeb() {

    getModelFormato(function (evt) {
        evtHot = $.extend(true, [], evt);
    });
    crearHandsonTable();
}

function cargarExcelWeb() {
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    $('#hfEmpresa').val($('#cbEmpresa').val());
    $('#hfIdEnvio').val(0);

    totNoNumero = 0;
    validaInicial = true;
    listErrores.splice(0, listErrores.length);

    crearHojaWeb();

    var mensaje = mostrarMensajeEnvio();
    mostrarMensaje("Por favor complete los datos. <strong>Plazo del Envio: </strong>" + mensaje);
    $('#divAcciones').css('display', 'block');
}

function validarSeleccionDatos() {
    if (!($('#hfHorizonte').val() == $('#cbHorizonte').val())) {
        return false;
    }
    return true;
}

//////////////////////////////////////////// crearHojaWeb ////////////////////////////////////////////////
/// Llamada por Ajax para conseguir del servidor la informacion del Formato//////////////////////////
function getModelFormato(callback) {
    $('#hfHorizonte').val($('#cbHorizonte').val());
    $('#hfFecha').val($('#txtFecha').val());
    $('#hfSemana').val($('#cbSemana').val());
    $('#hfAnho').val($('#Anho').val());
    idHorizonte = $("#hfHorizonte").val();
    idEnvio = $("#hfIdEnvio").val();
    fecha = $("#hfFecha").val();
    semana = $("#hfSemana").val();
    anho = $("#hfAnho").val();
    semana = anho.toString() + semana;
    $.ajax({
        type: 'POST',
        url: controlador + "MostrarHojaExcelWeb",
        dataType: 'json',
        async: false,
        data: {
            idEmpresa: -1,
            idEnvio: idEnvio,
            formatoReal: idHorizonte,
            fecha: fecha,
            semana: semana
        },
        success: function (evt) {
            if (evt != -1) {
                $.ajax({
                    type: 'POST',
                    url: controlador + 'descargardatos',
                    dataType: 'json',
                    async: false,
                    success: function (datos) {
                        evt.Handson.ListaExcelData = datos;
                        if (idHorizonte == 115 || idHorizonte == 116 || idHorizonte == 117) {
                            $('#selecFormato').css("display", "block");
                            mostrardetalleExcel(evt.Formato.Areaname, evt.Empresa, evt.Formato.Formatnombre, evt.Anho, evt.Mes, evt.Semana, evt.Dia, evt.Formato.Formatperiodo);
                        }
                    },
                    error: function () {
                        alert("Ha ocurrido un error");
                    }
                });

                callback(evt);
            }
            else {
                alert("El formato no tiene puntos de medición para cargar.");
                document.location.href = controlador + 'index';
            }
        },
        error: function () {
            alert("Error al cargar Excel Web");
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

///Visualiza Div del Titulo del FormatonuevoFormato
function mostrardetalleExcel(areaNombre, empresaNombre, formatoNombre, anho, mes, semana, dia, periodo) {
    semana = semana.substr(4);
    var area = areaNombre != null && areaNombre != undefined ? areaNombre : '';
    //var empresa = empresaNombre != null && empresaNombre != undefined ? empresaNombre : '';
    var formato = formatoNombre != null && formatoNombre != undefined ? formatoNombre : '';
    anho = anho != null && anho != undefined ? anho : '';
    mes = mes != null && mes != undefined ? mes : '';
    semana = semana != null && semana != undefined ? semana : '';
    dia = dia != null && dia != undefined ? dia : '';
    var area = "<table><tr><td > <strong><font style='color:SteelBlue;'>&nbsp;&nbsp;&nbsp;Formato Seleccionado:</strong>&nbsp;&nbsp;&nbsp;" + formato + "</font></td></tr>";
    area += "<tr><td><strong><font style='color:SteelBlue;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Año:</strong>&nbsp;&nbsp; " + anho + "</font>";
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
    //$("#idArea").html(area);
    $("#selecFormato").html(area);
}

////// Carga el Formato con nueva informacion (ya sea despues de grabar la data,envio anterior, etc)
function CargarDataExcelWeb(accion) {
    hot.destroy();
    getModelFormato(function (evt) {
        evtHot = $.extend(true, [], evt); //Crea un clone de evt
    });
    crearHandsonTable();
}

// Actualiza datos de web excel
function actualizaDataExcel() {
    hot.loadData(evtHot.Handson.ListaExcelData);
}

function mostrarEnvioExcelWeb(envio) {
    $('#enviosanteriores').bPopup().close();
    $("#hfIdEnvio").val(envio);


    CargarDataExcelWeb(4);


    var mensaje = mostrarMensajeEnvio();
    mostrarExito(mensaje);

}

function mostrarFormatoExcelWeb() {
    getModelFormato(function (evt) {
        evtHot = $.extend(true, [], evt); //Crea un clone de evt
    });
    crearHandsonTable();
}

validarExcelWeb = function (data, rowini, colini) {
    var retorno = true;
    var totBlancos = getTotBlancos(data, rowini, colini);
    if ((totLimInf + totLimSup + totNoNumero) > 0) {
        mostrarAlerta("Existen celdas con valores no nùmericos, favor corregir y vuelva a envíar");
        mostrarErrores();
        retorno = false;
    }

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

function dibujarTablaError() {
    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablaError' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Celda</th><th>Valor</th><th>Error</th></tr></thead>";
    cadena += "<tbody>";
    var len = listErrores.length;
    for (var i = 0; i < len; i++) {
        cadena += "<tr><td>" + listErrores[i].Celda + "</td>";
        cadena += "<td>" + listErrores[i].Valor + "</td>";
        cadena += "<td>" + listDescripErrores[listErrores[i].Tipo] + "</td></tr>";
    }
    cadena += "</tbody></table>";
    return cadena;
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
            case 2:
                totNoNumero++;
                break;
            case 3:
                totLimInf++;
                break;
            case 4:
                totLimSup++;
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
    var index = IndexOfError(celda);
    if (index != -1) {
        listErrores.splice(index, 1);
    }
    switch (tipoError) {
        case 2:
            totNoNumero--;
            break;
        case 3:
            totLimInf--;
            break;
        case 4:
            totLimSup--;
            break;
    }
}

function IndexOfError(celda) {
    var index = -1;
    //alert(celda);
    for (var i = 0; i < listErrores.length; i++) {
        if (listErrores[i].Celda == celda) {
            index = i;
            break;
        }
    }

    return index;
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

function pad(n, width, z) {
    z = z || '0';
    n = n + '';
    return n.length >= width ? n : new Array(width - n.length + 1).join(z) + n;
}

descargarFormato = function () {
    $('#hfHorizonte').val($('#cbHorizonte').val());
    $('#hfFecha').val($('#txtFecha').val());
    $('#hfSemana').val($('#cbSemana').val());
    $('#hfAnho').val($('#Anho').val());
    semana = $("#hfSemana").val();
    anho = $("#hfAnho").val();
    console.log(anho);
    semana = anho.toString() + pad(semana, 2, '0');
    console.log(semana);
    idHorizonte = $("#hfHorizonte").val();
    idEnvio = $("#hfIdEnvio").val();
    fecha = $("#hfFecha").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'generarformato',
        dataType: 'json',
        data: {
            idEmpresa: -1,
            formatoReal: idHorizonte,
            fecha: $('#hfFecha').val(),
            semana: semana
        },
        success: function (result) {
            if (result.Resultado == "1") {
                window.location = controlador + 'descargarformato';
            }
            else {
                alert(result.Mensaje);
            }
        },
        error: function () {
            alert("Error");
        }
    });
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
    idEnvio = $("#hfIdEnvio").val();
    if (confirm("¿Desea enviar información a COES?")) {

        var $container = $('#detalleFormato');
        if (validarExcelWeb(hot.getData(), evtHot.FilasCabecera, evtHot.ColumnasCabecera)) {
            $('#hfDataExcel').val((hot.getData()));
            if (idHorizonte == 115 || idHorizonte == 116 || idHorizonte == 117) {
                var matriz = JSON.stringify(formatJavaScriptSerializerVI(evtHot.ListaHojaPto));
            } else {
                var matriz = JSON.stringify(formatJavaScriptSerializer(evtHot.ListaHojaPto));
            }
            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: controlador + "GrabarExcelWeb",
                data: {
                    dataExcel: $('#hfDataExcel').val(),
                    idEmpresa: -1,
                    formatoReal: idHorizonte,
                    fecha: $('#hfFecha').val(),
                    semana: semana,
                    dataExcel2: matriz
                },
                beforeSend: function () {
                    mostrarAlerta("Enviando Información ..");
                },
                success: function (evt) {
                    if (evt.Resultado == 1) {
                        $("#hfIdEnvio").val(evt.IdEnvio);
                        CargarDataExcelWeb(2);
                        var mensaje = mostrarMensajeEnvio();
                        mostrarExito("Los datos se enviaron correctamente. " + mensaje);
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

function leerFileUpExcel() {
    var retorno = 0;
    $('#hfHorizonte').val($('#cbHorizonte').val());
    $('#hfFecha').val($('#txtFecha').val());
    $('#hfSemana').val($('#cbSemana').val());
    $('#hfAnho').val($('#Anho').val());
    semana = $("#hfSemana").val();
    anho = $("#hfAnho").val();
    semana = anho.toString() + semana;
    idHorizonte = $("#hfHorizonte").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'LeerFileUpExcel',
        dataType: 'json',
        async: false,
        data: {
            idEmpresa: -1,
            formatoReal: idHorizonte,
            fecha: $('#hfFecha').val(),
            semana: semana
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

function expandirExcel() {
    $('#idpanel').addClass("divexcel");
    hot.render();
}

function restaurarExcel() {
    $('#idpanel').removeClass("divexcel");
    hot.render();
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

function crearPupload() {
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectExcel',
        url: siteRoot + 'hidrologia/cargadatos/Upload',
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
                uploader.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                mostrarAlerta("Subida completada <strong>Por favor espere</strong>");
                var retorno = leerFileUpExcel();
                if (retorno == 1) {
                    limpiarError();
                    $("#hfIdEnvio").val(-1);//-1 indica que el handsonetable mostrara datos del archivo excel                    
                    CargarDataExcelWeb(5);
                    mostrarExito("<strong>Por favor presione el botón enviar para grabar los datos</strong>");
                }
                else {
                    mostrarError("Error: Formato descnocido.");
                }

            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploader.init();
}

function findIndiceMerge(col, lista) {
    for (key in lista) {
        if ((col >= lista[key].col) && (col < (lista[key].col + lista[key].colspan))) {
            return key;
        }
    }
    return -1;
}


function mostrarExito(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}

function mostrarError(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}

function mostrarAlerta(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}

function mostrarMensaje(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
}

function mostrarMensajeEnvio() {

    var envio = $("#hfIdEnvio").val();
    if (envio > 0) {
        var plazo = (evtHot.EnPlazo) ? "<strong style='color:green'>en plazo</strong>" : "<strong style='color:red'>fuera de plazo</strong>";
        var mensaje = "<strong>Código de envío</strong> : " + evtHot.IdEnvio + "   , <strong>Fecha de envío: </strong>" + evtHot.FechaEnvio + "   , <strong>Enviado en </strong>" + plazo;
        return mensaje;
    }
    else {
        if (!evtHot.EnPlazo) {
            return "<strong style='color:red'>Fuera de plazo</strong>";
        }
        else return "<strong style='color:green'>En plazo</strong>";
    }
}

horizonte = function () {

    var opcion = $('#cbHorizonte').val();
    switch (parseInt(opcion)) {
        case 109: //dia
        case 110: //dia
        case 115: //dia
        case 116: //dia
            $('#dDia').css("display", "block");
            $('#dSemana').css("display", "none");
            $('#selecFormato').css("display", "none");
            break;
        case 111: //semanal
        case 117: //semanal
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