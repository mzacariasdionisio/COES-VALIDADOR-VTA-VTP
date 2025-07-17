var controlador = siteRoot + 'AporteIntegrantes/IngresoTransmision/';
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
    var Tipoemprcodi = document.getElementById('Tipoemprcodi').value;
    if (Tipoemprcodi == 1) {
        $.ajax({
            type: 'POST',
            url: controlador + "ObtenerUltimoEnvio",
            dataType: 'json',
            data: {
                idEmpresa: document.getElementById('Emprcodi').value,
                mes: $('#cbPeriodo').val(),
                Intervalo: document.getElementById('Intervalo').value
            },
            success: function (result) {
                $('#hfIdEnvio').val(result);
                cargarExcelWeb();
            },
            error: function () {
                alert("Error al cargar Excel Web");
            }
        });
    }

    //$('#btnConsultar').click(function () {

    //    $.ajax({
    //        type: 'POST',
    //        url: controlador + "ObtenerUltimoEnvio",
    //        dataType: 'json',
    //        data: {
    //            idEmpresa: document.getElementById('Emprcodi').value,
    //            mes: $('#cbPeriodo').val(),
    //            Tipoemprcodi: document.getElementById('Tipoemprcodi').value,
    //            Intervalo: document.getElementById('Intervalo').value
    //        },
    //        success: function (result) {
    //            $('#hfIdEnvio').val(result);
    //            cargarExcelWeb();
    //        },
    //        error: function () {
    //            alert("Error al cargar Excel Web");
    //        }
    //    });

    //});

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
    getModelFormato(1, false);
}

function cargarExcelWeb() {
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    $('#hfEmpresa').val(document.getElementById('Emprcodi').value);
    $('#hfMes').val($('#cbPeriodo').val());

    totNoNumero = 0;
    validaInicial = true;
    listErrores.splice(0, listErrores.length);

    crearHojaWeb();
}

function CargarDataExcelWeb(accion, flag) {

    hot.destroy();
    getModelFormato(accion, flag);
}

function validarSeleccionDatos() {
    if (!($('#hfEmpresa').val() == document.getElementById('Emprcodi').value && $('#cbPeriodo').val() == $('#hfMes').val())) {
        return false;
    }
    return true;
}

//////////////////////////////////////////// crearHojaWeb ////////////////////////////////////////////////
/// Llamada por Ajax para conseguir del servidor la informacion del Formato//////////////////////////
function getModelFormato(accion, flag) {

    var idEmpresa = $("#hfEmpresa").val();
    var idEnvio = $("#hfIdEnvio").val();
    var mes = $("#hfMes").val();
    var Tipoemprcodi = document.getElementById('Tipoemprcodi').value;
    var Intervalo = document.getElementById('Intervalo').value;
    var NroMeses = document.getElementById('NroMeses').value;
    $.ajax({
        type: 'POST',
        url: controlador + "MostrarHojaExcelWeb",
        dataType: 'json',

        data: {
            idEmpresa: idEmpresa,
            idEnvio: idEnvio,
            mes: mes,
            Intervalo: Intervalo,
            NroMeses: NroMeses
        },
        success: function (evt) {
            if (evt != -1) {
                $.ajax({
                    type: 'POST',
                    url: controlador + 'descargardatos',
                    dataType: 'json',
                    success: function (datos) {
                        evt.Handson.ListaExcelData = datos;
                        crearHandsonTable(evt, flag);
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
                            mostrarMensaje(mensaje);
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
    cadena += "<thead><tr><th>Id Envío</th><th>Fecha Hora</th><th>Usuario</th><th>Plazo</th></tr></thead>";
    cadena += "<tbody>";

    var plazo = "";
    for (key in lista) {

        if (lista[key].Envioplazo == "P") plazo = "En plazo";
        else if (lista[key].Envioplazo == "F") plazo = "Fuera de Plazo";

        var javaScriptDate = new Date(parseInt(lista[key].Enviofecha.substr(6)));
        cadena += "<tr onclick='mostrarEnvioExcelWeb(" + lista[key].Enviocodi + ");' style='cursor:pointer'><td>" + lista[key].Enviocodi + "</td>";
        cadena += "<td>" + getFormattedDate(javaScriptDate) + "</td>";
        cadena += "<td>" + lista[key].Lastuser + "</td>"
        cadena += "<td>" + plazo + "</td>"
        cadena += "</tr>";

    }
    cadena += "</tbody></table>";
    return cadena;

}

////// Carga el Formato con nueva informacion (ya sea despues de grabar la data,envio anterior, etc)

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
    getModelFormato();
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
    var len = (listErrores.length > 100) ? 50 : listErrores.length;
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
    var Tipoemprcodi = document.getElementById('Tipoemprcodi').value;
    var Intervalo = document.getElementById('Intervalo').value;
    var NroMeses = document.getElementById('NroMeses').value;
    $.ajax({
        type: 'POST',
        url: controlador + 'generarformato',
        dataType: 'json',
        data: {
            idEmpresa: $('#hfEmpresa').val(),
            mes: $('#hfMes').val(),
            Tipoemprcodi: Tipoemprcodi,
            Intervalo: Intervalo,
            NroMeses: NroMeses
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
        var Intervalo = document.getElementById('Intervalo').value;
        var NroMeses = document.getElementById('NroMeses').value;
        var $container = $('#detalleFormato');
        if (validarExcelWeb(hot.getData(), evtHot.FilasCabecera, evtHot.ColumnasCabecera)) {
            $('#hfDataExcel').val((hot.getData()));
            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: controlador + "GrabarExcelWeb",
                data: {
                    dataExcel: $('#hfDataExcel').val(),
                    idEmpresa: $('#hfEmpresa').val(),
                    mes: $('#hfMes').val(),
                    Intervalo: Intervalo,
                    NroMeses: NroMeses
                },
                beforeSend: function () {
                    mostrarAlerta("Enviando Información ..");
                },
                success: function (evt) {
                    if (evt.Resultado == 1) {
                        $("#hfIdEnvio").val(evt.IdEnvio);
                        CargarDataExcelWeb(2, false);
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
    var NroMeses = document.getElementById('NroMeses').value;
    var Intervalo = document.getElementById('Intervalo').value;
    $.ajax({
        type: 'POST',
        url: controlador + 'LeerFileUpExcel',
        dataType: 'json',
        async: false,
        data: {
            idEmpresa: $('#hfEmpresa').val(),
            mes: $('#hfMes').val(),
            Intervalo: Intervalo,
            NroMeses: NroMeses
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

function validarCarga(data) {

    for (var row = 0; row < data.Handson.ListaExcelData.length; row++) {
        for (var col = 0; col < data.Handson.ListaExcelData[0].length; col++) {
            if ((row >= data.FilasCabecera) && (col >= data.ColumnasCabecera) && (!data.Handson.ReadOnly)) {

                if (!data.Handson.ListaFilaReadOnly[row]) {

                    var value = data.Handson.ListaExcelData[row][col];

                    if (validarDecimal(value) && value != "") {
                        if (Number(value) < data.ListaHojaPto[col - 1].Hojaptoliminf) {
                            var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                            agregarError(celda, value, 3);
                        }
                        else {
                            if (Number(value) > data.ListaHojaPto[col - 1].Hojaptolimsup) {
                                var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                                agregarError(celda, value, 4);
                            }
                        }
                    }
                    else {
                        if (value != "") {
                            if (!validarDecimal(value)) {
                                var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                                agregarError(celda, value, 2);
                            }
                        }
                    }
                }
            }
        }
    }
}

validarDecimal = function (n) {
    if (n == "")
        return false;

    var count = 0;
    var strCheck = "0123456789.-E";
    var i;

    for (i in n) {
        if (strCheck.indexOf(n[i]) == -1)
            return false;
        else {
            if (n[i] == '.') {
                count = count + 1;
            }
        }
    }
    if (count > 1) return false;
    return true;
}

function getTotBlancos(data, rowini, colini) {
    //var arreglo = data.split(",");
    var total = data.length;
    var totalBlancos = 0;
    var cadena = "";
    for (var i = rowini ; i < total ; i++) {
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
        url: siteRoot + 'demandabarras/envio/Upload',
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
                    CargarDataExcelWeb(5, true);

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
        var mensaje = "<strong>Código de envío</strong> : " + evtHot.IdEnvio + "   , <strong>Fecha de envío: </strong>" + evtHot.FechaEnvio; "" + "   , <strong>Enviado en </strong>" + plazo;
        return mensaje;
    }
    else {
        if (!evtHot.EnPlazo) {
            return "Por favor complete los datos. <strong>Plazo del Envio: </strong> <strong style='color:red'>Fuera de plazo</strong>";
        }
        else return "Por favor complete los datos. <strong>Plazo del Envio: </strong> <strong style='color:green'>En plazo</strong>";
    }
}