var controlador = siteRoot + 'interconexiones/envio/';
var uploader;
var totNoNumero = 0;
var totLimSup = 0;
var totLimInf = 0;
var listErrores = [];
var listDescripErrores = ["BLANCO", "NÚMERO", "No es número", "Valor negativo", "Supera el valor máximo"];
var listValInf = [];
var listValSup = [];
var listaPeriodoMedidor = [];
var validaInicial = true;
var hot;
var hotOptions;
var evtHot;

$(function () {

    $('#txtFecha').Zebra_DatePicker({
    });

    $('#btnAgregarMedidores').click(function () {
        mostrarPeriodo();
    });

    $('#btnConsultar').click(function () {

        $.ajax({
            type: 'POST',
            url: controlador + "ObtenerUltimoEnvio",
            dataType: 'json',
            data: {
                idEmpresa: $('#cbEmpresa').val(),
                dia: $('#txtFecha').val()
            },
            success: function (result) {
                $('#hfIdEnvio').val(result);
                cargarExcelWeb();
            },
            error: function () {
                alert("Error al cargar Excel Web");
            }
        });

    });

    $('#btnDescargarFormato').click(function () {
        if (validarSeleccionDatos()) {
            descargarFormato();
        }
        else {
            alert("Por favor seleccione la empresa correcta.");
        }
    });

    $('#btnEnviarDatos').click(function () {
        if (validarSeleccionDatos()) {
            enviarDatos();
        }
        else {
            alert("Por favor seleccione la empresa correcta.");
        }
    });

    $('#btnMostrarErrores').click(function () {
        if (validarSeleccionDatos()) {
            mostrarErrores();
        }
        else {
            alert("Por favor seleccione la empresa correcta.");
        }
    });

    $('#btnVerEnvios').click(function () {
        if (validarSeleccionDatos()) {
            popUpListaEnvios();
        }
        else {
            alert("Por favor seleccione la empresa correcta.");
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

    crearPupload();
});

function crearHojaWeb() {
    getModelFormato();
}

function cargarExcelWeb() {
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    $('#hfEmpresa').val($('#cbEmpresa').val());
    $('#hfFecha').val($('#txtFecha').val());
    totNoNumero = 0;
    validaInicial = true;
    listErrores.splice(0, listErrores.length);
    crearHojaWeb();
}

function CargarDataExcelWeb(accion) {
    hot.destroy();
    getModelFormato(accion);
}

function validarSeleccionDatos() {
    if (!($('#hfEmpresa').val() == $('#cbEmpresa').val() && $('#txtFecha').val() == $('#hfFecha').val())) {
        return false;
    }
    return true;
}

//////////////////////////////////////////// crearHojaWeb ////////////////////////////////////////////////
/// Llamada por Ajax para conseguir del servidor la informacion del Formato//////////////////////////
function getModelFormato(accion) {
    listaPeriodoMedidor.length = 0;
    idEmpresa = $("#hfEmpresa").val();
    idEnvio = $("#hfIdEnvio").val();
    dia = $("#hfFecha").val();

    $.ajax({
        type: 'POST',
        url: controlador + "MostrarHojaExcelWeb",
        dataType: 'json',

        data: {
            idEmpresa: idEmpresa,
            idEnvio: idEnvio,
            dia: dia
        },
        success: function (evt) {
            if (evt != -1) {
                $.ajax({
                    type: 'POST',
                    url: controlador + 'descargardatos',
                    dataType: 'json',
                    success: function (datos) {
                        evt.Handson.ListaExcelData = datos;
                        generarListaPeriodoServer(evt.ListaPeriodoMedidor);
                        crearHandsonTable(evt);
                        evtHot = evt;

                        if (accion == 2) {

                            var mensaje = mostrarMensajeEnvio();
                            mostrarExito("Los datos se enviaron correctamente. " + mensaje);

                        }
                        else if (accion == 5) {
                            mostrarExito("<strong>Por favor presione el botón enviar para grabar los datos</strong>");

                        }
                        else {
                            var mensaje = mostrarMensajeEnvio();
                            mostrarMensaje("Por favor complete los datos. <strong>Plazo del Envio: </strong>" + mensaje);
                            $('#divAcciones').css('display', 'block');
                        }

                    },
                    error: function () {
                        alert("Ha ocurrido un error");
                    }
                });
            }
            else {
                alert("La empresa no tiene puntos de medición para cargar.");
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
    getModelFormato();
    crearHandsonTable();
}

function validarExcelWeb(data, rowini, colini) {
    var retorno = true;
    var totBlancos = parseInt(getTotBlancos(data, rowini, colini));

    if ((totLimInf + totLimSup + totNoNumero) > 0) {
        mostrarAlerta("Existen celdas con errores, favor corregir y vuelva a enviar");
        mostrarErrores();
        retorno = false;
    }
    else
        if (totBlancos > 0) {
            if (!confirm("Existen " + totBlancos + " celdas con valores en blanco, ¿desea reemplarlos con ceros?")) {
                mostrarAlerta("Existen " + totBlancos + " celdas con valores en blanco");
                retorno = false;
            }
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
    for (var i = 0 ; i < len ; i++) {
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

descargarFormato = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'generarformato',
        dataType: 'json',
        data: {
            idEmpresa: $('#hfEmpresa').val(),
            dia: $('#hfFecha').val()
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

function enviarDatos() {
    if (confirm("¿Desea enviar información a COES?")) {

        var $container = $('#detalleFormato');
        if (verificaPeriodo() > 0) {
            if (validarExcelWeb(hot.getData(), evtHot.FilasCabecera, evtHot.ColumnasCabecera)) {
                $('#hfDataExcel').val((hot.getData()));
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    url: controlador + "GrabarExcelWeb",
                    data: {
                        dataExcel: $('#hfDataExcel').val(),
                        idEmpresa: $('#hfEmpresa').val(),
                        dia: $('#hfFecha').val(),
                        periodos: vectorPeriodo()
                    },
                    beforeSend: function () {
                        mostrarAlerta("Enviando Información ..");
                    },
                    success: function (evt) {
                        if (evt.Resultado == 1) {
                            $("#hfIdEnvio").val(evt.IdEnvio);
                            CargarDataExcelWeb(2);
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
        else {
            alert("Favor indicar el detalle de medidores utilizados.");
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
    var total = data.length;
    var totalBlancos = 0;
    for (var i = rowini ; i < total ; i++) {
        for (var j = colini; j < data[i].length; j++) {
            //si es numero
            dato = data[i][j];
            if (dato == "0") {
                dato = "1";
            }
            if (dato == "") {
                totalBlancos++;
                console.log("blancos -" + dato + "-" + " fila: " + i + "  columna: " + j);
            }
        }
    }
    return totalBlancos;
}

function crearPupload() {
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectExcel',
        url: siteRoot + 'interconexiones/envio/Upload',
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

function mostrarPeriodo() {
    var perIni = 0;
    var len = listaPeriodoMedidor.length;
    if (len > 0) {
        perIni = parseInt(listaPeriodoMedidor[len - 1].Periodofin) + 1;
    }
    $.ajax({
        type: 'POST',
        url: controlador + "IndicarPeriodoMedidor",
        data: {
            periodoIni: perIni
        },
        success: function (evt) {
            nper = verificarPeriodo();
            $('#opcionPeriodo').html(evt);
            if (nper >= 96) {
                $("#opcionesPeriodo").css("display", "none");
            }
            $('#idTperiodo').html(dibujarTablaPeriodo());
            setTimeout(function () {
                $('#popup2').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false
                });
            }, 50);
        },
        error: function () {
            alert("Error en mostrar periodo");
        }
    });
}

function mostrarPeriodo2() {
    var perIni = 0;
    var len = listaPeriodoMedidor.length;
    if (len > 0) {
        perIni = parseInt(listaPeriodoMedidor[len - 1].Periodofin) + 1;
    }
    if (perIni < 96) {
        $.ajax({
            type: 'POST',
            url: controlador + "IndicarPeriodoMedidor",
            data: {
                periodoIni: perIni
            },
            success: function (evt) {

                $('#opcionPeriodo').html(evt);

                setTimeout(function () {
                    $('#popup2').bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown',
                        modalClose: false
                    });
                }, 50);
            },
            error: function () {
                alert("Error en mostrar periodo");
            }
        });
    }
    else {
        alert("Periodo ya se encuentra ingresado");
    }

}

function agregarPeriodo(medidor, pinicial, pfinal, nombreMedidor) {

    if (medidor != "") {
        var periodoMedidor = {
            Periodoini: pinicial,
            Periodofin: pfinal,
            Medicodi: medidor,
            Medinombre: nombreMedidor
        };

        listaPeriodoMedidor.push(periodoMedidor);
        //$('#popup2').bPopup().close();
        $('#idTperiodo').html(dibujarTablaPeriodo());
        var iniPer = parseInt(pfinal) + 1;
        var horamin = convetirHoraMin(iniPer);
        $("#hfpinicial").val(iniPer);
        $("#pinicial").val(horamin);
        generarListaPerFinal(iniPer);
        if (verificarPeriodo() >= 96) {
            $("#opcionesPeriodo").css("display", "none");
        }

    }
    else {
        alert("Seleccionar Medidor");
    }
}

function verificarPeriodo() {
    var perIni = 0;
    var len = listaPeriodoMedidor.length;
    if (len > 0) {
        perIni = parseInt(listaPeriodoMedidor[len - 1].Periodofin) + 1;
    }
    return perIni;
}

function eliminarPeriodo() {
    var totPeriodos = listaPeriodoMedidor.length;
    if (totPeriodos > 0) {
        if (confirm('¿Está seguro de eliminar el periodo?')) {
            var pIni = listaPeriodoMedidor[totPeriodos - 1].Periodoini;
            $("#hfpinicial").val(pIni);
            $("#pinicial").val(convetirHoraMin(pIni));
            listaPeriodoMedidor.pop();
            generarListaPerFinal(pIni);
            $('#idTperiodo').html(dibujarTablaPeriodo());
            $("#opcionesPeriodo").css("display", "block");
        }
    }
}

function dibujarTablaPeriodo() {

    var cadena = "<table border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Hora Inicio</th><th>Hora Fin</th><th>Medidor</th></tr></thead>";
    cadena += "<tbody>";
    var len = listaPeriodoMedidor.length;
    for (var i = 0 ; i < len ; i++) {
        cadena += "<tr><td>" + convetirHoraMin(listaPeriodoMedidor[i].Periodoini) + "</td>";
        cadena += "<td>" + convetirHoraMin(listaPeriodoMedidor[i].Periodofin) + "</td>";
        cadena += "<td>" + listaPeriodoMedidor[i].Medinombre + "</td></tr>";
    }
    cadena += "</tbody></table>";
    return cadena;
}

function convetirHoraMin(horamin) {
    var hora = "0" + Math.floor((parseInt(horamin) + 1) / 4).toString();
    hora = hora.substring((hora.length > 2) ? 1 : 0, ((hora.length > 2) ? 1 : 0) + 2);
    var minuto = "0" + (Math.floor((parseInt(horamin) + 1) % 4) * 15).toString();
    minuto = minuto.substring((minuto.length > 2) ? 1 : 0, ((minuto.length > 2) ? 1 : 0) + 2);
    return hora + ":" + minuto;
}

function verificaPeriodo() {
    var perFin = 0;
    var len = listaPeriodoMedidor.length;
    if (len > 0) {
        perFin = parseInt(listaPeriodoMedidor[len - 1].Periodofin);
    }
    if (perFin < 95) {
        return 0
    }
    else {
        return 1;
    }
}

function vectorPeriodo() {
    var cadena = "";
    var len = listaPeriodoMedidor.length;

    for (var i = 0; i < len; i++) {
        for (var j = parseInt(listaPeriodoMedidor[i].Periodoini) ; j <= parseInt(listaPeriodoMedidor[i].Periodofin) ; j++) {

            if (i == 0 && j == 0) {
                cadena += listaPeriodoMedidor[i].Medicodi;
            }
            else {
                cadena += "," + listaPeriodoMedidor[i].Medicodi;
            }
        }
    }

    return cadena;
}

function generarListaPerFinal(indiceIni) {
    $('#cbPeriodoFin')[0].options.length = 0;
    for (var i = indiceIni + 1 ; i <= 95; i++) {
        $('#cbPeriodoFin').append($('<option>', {
            value: i,
            text: convetirHoraMin(i)
        }));
    }
    $('#cbPeriodoFin').val(95);

}

function generarListaPeriodoServer(lista) {
    if (lista != null) {
        for (index = 0; index < lista.length; ++index) {
            periodoMedidor = {
                Periodoini: lista[index].PeriodoIni,
                Periodofin: lista[index].PeriodoFin,
                Medicodi: lista[index].IdMedidor,
                Medinombre: lista[index].NombreMedidor
            };
            listaPeriodoMedidor.push(periodoMedidor);
        }
    }
}



