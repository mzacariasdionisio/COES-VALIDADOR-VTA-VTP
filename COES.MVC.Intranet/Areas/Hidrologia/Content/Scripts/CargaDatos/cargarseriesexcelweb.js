var controlador = siteRoot + 'hidrologia/cargarseries/';
var uploader;
var totNoNumero = 0;
var totLimSup = 0;
var totLimInf = 0;
var totVacias = 0;
var totFilasVacias = 0;
var listErrores = [];
var listDescripErrores = ["BLANCO", "NÚMERO", "No es número", "Valor negativo", "Supera el valor máximo de 5 enteros", "Celda Vacía", "Fila Vacía"];
var listValInf = [];
var listValSup = [];
var validaInicial = true;
var hot;
var hotOptions;
var evtHot;

$(function () {
    $('#txtFecha').Zebra_DatePicker({
    });

    $('#cbHorizonte').change(function () {
        horizonte();
    });

    $('#btnConsultar').click(function () {
        $("#hfModoLectura").val('');
        $("#hfOpcion").val('');
        cargarExcelWeb();
        mostrarInformacionCabecera();
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

    $('#btnManualUsuario').click(function () {
        window.location = controlador + 'DescargarManualUsuario';
    });

    horizonte();
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
    totVacias = 0;
    totFilasVacias = 0;
    validaInicial = true;
    listErrores.splice(0, listErrores.length);

    crearHojaWeb();

    $('#divAcciones').css('display', 'block');
    $('#detalleCabecera').css('display', 'block');
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
    idEnvio = $("#hfIdEnvio").val();
    $.ajax({
        type: 'POST',
        url: controlador + "MostrarHojaExcelWeb",
        dataType: 'json',
        async: false,
        data: {
            idEnvio: idEnvio,
            EmpresaCodi: $('#cbEmpresa').val(),
            TipoSerie: $('#cbTipoSerie').val(),
            TipoPuntoMedicion: $('#cbTipoPuntoMedicion').val(),
            PuntoMedicion: $('#cbPuntoMedicion').val(),
            ModoLectura: $('#hfModoLectura').val()
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
        var javaScriptDate = new Date(parseInt(lista[key].FechaRegistro.substr(6)));
        cadena += "<tr onclick='mostrarEnvioExcelWeb(" + lista[key].IdCaudal + ");' style='cursor:pointer'><td>" + lista[key].IdCaudal + "</td>";
        cadena += "<td>" + getFormattedDate(javaScriptDate) + "</td>";
        cadena += "<td>" + lista[key].UsuarioRegistro + "</td></tr>";
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
    $("#selecFormato").html(area);
}

////// Carga el Formato con nueva informacion (ya sea despues de grabar la data,envio anterior, etc)
function CargarDataExcelWeb(accion) {
    hot.destroy();
    arrayFilasVacias = new Array;
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
    var valTipoPtoMedicion = $('#cbTipoPuntoMedicion').val();
    var valhdfAnioMinimo = $('#hdfAnioMinimo').val();
    var numAnios = 0;
    var blnTerminaAnio = true;
    
    if (valTipoPtoMedicion == '7') { //Volumen Util
        totFilasVacias = 0;
        if (arrayFilasVacias) {
            for (i = 1; i <= arrayFilasVacias.length; i++) {
                if ((arrayFilasVacias[i] == 1) && (blnTerminaAnio)) {
                    numAnios++
                } else if ((arrayFilasVacias[i] == 1) && (!blnTerminaAnio)) {
                    totFilasVacias++
                } else {
                    blnTerminaAnio = false;
                }
            }
        }

        if (totFilasVacias > 0) {         
            valhdfAnioMinimo = parseInt(valhdfAnioMinimo) + parseInt(numAnios);
            mostrarAlerta("No se realizó la carga de serie, debido a que una fila o celda entre los valores de  " + valhdfAnioMinimo + " hasta el año anterior actual está vacía.");
            mostrarErrores();
            retorno = false;
        } else if ((totLimInf + totLimSup + totNoNumero + totVacias) > 0) {
            mostrarAlerta("Existen errores en las celdas, favor corregir y vuelva a cargar.");
            mostrarErrores();
            retorno = false;
        }
    } else {
        totFilasVacias = 0;
        if (arrayFilasVacias) {
            for (i = 1; i <= arrayFilasVacias.length; i++) {
                if (arrayFilasVacias[i] == 1) {
                    totFilasVacias++
                }
            }
        }
        if ((totLimInf + totLimSup + totNoNumero + totVacias + totFilasVacias) > 0) {
            mostrarAlerta("Existen errores en las celdas, favor corregir y vuelva a cargar.");
            mostrarErrores();
            retorno = false;
        } else if (totFilasVacias > 0) {
            mostrarAlerta("Existen errores en las filas y celdas, favor corregir y vuelva a cargar.");
            mostrarErrores();
            retorno = false;
        }
    }
    

    return retorno;
}

function limpiarError() {
    totLimInf = 0;
    totLimSup = 0;
    totNoNumero = 0;
    totVacias = 0;
    totFilasVacias = 0;
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
    $('#idEnviosAnteriores').html(dibujarTablaEnvios(evtHot.ListaCaudal));
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
    //Mostrar las filas vacias
    var blnTerminaAnio = true;
    var numAnios = 0;
    if (arrayFilasVacias) {
        for (i = 1; i <= arrayFilasVacias.length; i++) {
            if ((arrayFilasVacias[i] == 1) && (blnTerminaAnio)) {
                numAnios++
                if ($('#cbTipoPuntoMedicion').val() != "7") {
                    cadena += "<tr><td>Fila " + parseInt(i + 1) + "</td>";
                    cadena += "<td></td>";
                    cadena += "<td>Fila vacía</td></tr>";
                }
            } else if ((arrayFilasVacias[i] == 1) && (!blnTerminaAnio)) {
                cadena += "<tr><td>Fila " + parseInt(i + 1)+ "</td>";
                cadena += "<td></td>";
                cadena += "<td>Fila vacía</td></tr>";
            } else {
                blnTerminaAnio = false;
            }
        }
    }

    var len = listErrores.length;
    for (var i = 0; i < len; i++) {
        cadena += "<tr><td>" + listErrores[i].Celda + "</td>";
        cadena += "<td>" + listErrores[i].Valor + "</td>";
        cadena += "<td>" + listDescripErrores[listErrores[i].Tipo] + "</td></tr>";
    }
    cadena += "</tbody></table>";
    return cadena;
}

function agregarError(celda, valor, tipo, fila) {
    if (validarError(celda)) {
        var regError = {
            Celda: celda,
            Valor: valor,
            Tipo: tipo,
            Fila: fila
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
            case 5:
                totVacias++;
                break;
            case 6:
                totFilasVacias++;
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
        case 5:
            totVacias--;
            break;
        case 6:
            totFilasVacias--;
            break;
    }
}

function eliminarErrorFila(fila) {
    var listNuevosErrores = [];
    for (var indexError = 0; indexError < listErrores.length; indexError++) {
        if (listErrores[indexError].Fila == fila) {
            listErrores.splice(indexError, 1);
            totVacias--;
        } else {
            listNuevosErrores.push(listErrores[indexError]);
        }
    }
}

function IndexOfError(celda) {
    var index = -1;
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
    idEnvio = $("#hfIdEnvio").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'generarformato',
        dataType: 'json',
        data: {
            EmpresaCodi: $('#cbEmpresa').val(),
            TipoSerie: $('#cbTipoSerie').val(),
            TipoPuntoMedicion: $('#cbTipoPuntoMedicion').val(),
            PuntoMedicion: $('#cbPuntoMedicion').val(),
            ModoLectura : ''
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
    idEnvio = $("#hfIdEnvio").val();
    if (confirm("¿Desea cargar información?")) {
        var $container = $('#detalleFormato');
        if (validarExcelWeb(hot.getData(), evtHot.FilasCabecera, evtHot.ColumnasCabecera)) {
            $('#hfDataExcel').val((hot.getData()));
            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: controlador + "GrabarExcelWeb",
                data: {
                    dataExcel: $('#hfDataExcel').val(),
                    dataExcel2: '',
                    EmpresaCodi: $('#cbEmpresa').val(),
                    TipoSerie: $('#cbTipoSerie').val(),
                    TipoPuntoMedicion: $('#cbTipoPuntoMedicion').val(),
                    PuntoMedicion: $('#cbPuntoMedicion').val()
                },
                beforeSend: function () {
                    mostrarAlerta("Enviando Información ..");
                },
                success: function (evt) {
                    if (evt.Resultado == 1) {
                        $("#hfIdEnvio").val(evt.IdEnvio);
                        $("#hfModoLectura").val(1);
                        CargarDataExcelWeb(2);
                        var mensaje = mostrarMensajeEnvio();
                        mostrarExito("Los datos se enviaron correctamente. " + mensaje);
                    }
                    else {
                        mostrarError("Existen errores en las celdas, favor corregir y vuelva a cargar.");
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
    $.ajax({
        type: 'POST',
        url: controlador + 'LeerFileUpExcel',
        dataType: 'json',
        async: false,
        data: {
            idEmpresa: -1,
            formatoReal: 0,
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
        return 5; // blanco
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
        url: siteRoot + 'hidrologia/cargarseries/Upload',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '15mb',
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
                    mostrarError("Error: Formato desconocido.");
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
        var mensaje = "<strong>Código de envío</strong> : " + evtHot.IdEnvio + "   , <strong>Fecha de envío: </strong>" + evtHot.FechaEnvio + " ";
        return mensaje;
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

function habilitarTipoSerie(elementEmpresa) {
    if (elementEmpresa.value) {
        $('#cbTipoSerie').prop('disabled', false);
        habilitarTipoPuntoMedicion(document.getElementById('cbTipoSerie'));
    } else {
        $('#cbTipoSerie').prop('disabled', true);
        $('#cbTipoSerie').val('');
    }
}

function habilitarTipoPuntoMedicion(elementTipoSerie) {
    if (elementTipoSerie.value) {        
        $('#cbTipoPuntoMedicion').prop('disabled', false);
        habilitarPuntoMedicion(document.getElementById('cbTipoPuntoMedicion'));
    } else {
        $('#cbTipoPuntoMedicion').prop('disabled', true);
        $('#cbTipoPuntoMedicion').val('');
    }
}

function habilitarPuntoMedicion(elementTipoPuntoMedicion) {
    if (elementTipoPuntoMedicion.value) {
        $('#cbPuntoMedicion').prop('disabled', false);
        $('#cbPuntoMedicion').find('option:not(:first-child)').remove();
        let loadCombobox = $('#cbPuntoMedicion');
        loadCombobox.find('option:first-child').text('Cargando....');
        loadCombobox.prop('disabled', true);
        let codEmpresa = $('#cbEmpresa').val();
        let codTipoSerie = $('#cbTipoSerie').val();
        let codTipoPuntoMedicion = $('#cbTipoPuntoMedicion').val();

        cargarComboPuntoMedicionPorEmpresa(codEmpresa, codTipoSerie, codTipoPuntoMedicion).then((data) => {
            $('#cbPuntoMedicion').find('option:not(:first-child)').remove();
            data.map((item) => {
                let option = $(`<option value="${item.Ptomedicodi}">${item.Ptomedibarranomb}</option>`);
                option.data('itemCentral', item)
                $('#cbPuntoMedicion').append(option);
            })

            loadCombobox.prop('disabled', false);
            loadCombobox.find('option:first-child').text('--Seleccione--');
        }).catch((error) => {
            loadCombobox.prop('disabled', false);
            loadCombobox.find('option:first-child').text('--Seleccione--');
            alert(error)
        })
        habilitarBotonConsultar($('#cbPuntoMedicion'));
    } else {
        $('#cbPuntoMedicion').prop('disabled', true);
        $('#cbPuntoMedicion').val('');
        habilitarBotonConsultar($('#cbPuntoMedicion'));
    }

}

function cargarComboPuntoMedicionPorEmpresa(codEmpresa, codTipoSerie, codTipoPuntoMedicion) {
    var pro = new Promise((resolve, reject) => {
        $.ajax({
            type: 'POST',
            url: controlador + "ListarPuntoMedicionPorEmpresa",
            data: {
                CodEmpresa: codEmpresa,
                CodTipoSerie: codTipoSerie,
                CodTipoPuntoMedicion: codTipoPuntoMedicion
            },
            success: function (evt) {
                resolve(evt)
            },
            error: function () {
                $('#loading').removeClass('cancel')
                reject("Lo sentimos, ha ocurrido un error inesperado");
            }
        });
    })
    return pro;
}

function habilitarBotonConsultar(elementPtoMedicion) {
    if (elementPtoMedicion.value) {
        $('#btnConsultar').prop('disabled', false);
        $('#btnConsultar').removeClass("btn_disabled").addClass("btn_enabled");
    } else {
        $('#btnConsultar').prop('disabled', true);
        $('#btnConsultar').removeClass("btn_enabled").addClass("btn_disabled");
    }
}

function mostrarInformacionCabecera() {
    codPuntoMedicion = $("#cbPuntoMedicion").val();
    $.ajax({
        type: 'POST',
        url: controlador + "ListarInformacionPuntoMedicionPorEmpresa",
        dataType: 'json',
        async: false,
        data: {
            CodPuntoMedicion: codPuntoMedicion
        },
        success: function (datos) {
            mostrarCabecera(datos);
        },
        error: function () {
            alert("Error al cargar Informacion de Cabecera.");
        }
    });
}

function mostrarCabecera(datos) {
    $('#divTipoPuntoMedicion').html(datos.Tipoptomedinomb);
    $('#divDescPuntoMedicion').html(datos.Ptomedidesc);
    $('#divEquipoPadreNombre').html(datos.EquiPadrenomb);
    
    $('#divNombreEquipoEstHidrologica').html(datos.Equinomb);
    $('#divNombreEmpresa').html(datos.Emprnomb);
    $('#divCoordenadaX').html(datos.CoordenadaX);
    if (datos.Tipoptomedinomb == 'CAUDAL NATURAL ESTIMADO') {
        $('#divTextEmbalseRio').html('<b>RIO</b>');
        $('#divDescEStacion').html('<b>ESTACIÓN</b>');
    } else {
        $('#divTextEmbalseRio').html('<b>EMBALSE</b>');
        $('#divDescEStacion').html('');
        $('#divNombreEquipoEstHidrologica').html('');
    }
    $('#divEmbalseRio').html(datos.Ptomedidesc);
    $('#divCoordenadaY').html(datos.CoordenadaY);
    $('#divPuntoMedicion').html(datos.Ptomedibarranomb);
    $('#divAltitud').html(datos.Altitud);

}