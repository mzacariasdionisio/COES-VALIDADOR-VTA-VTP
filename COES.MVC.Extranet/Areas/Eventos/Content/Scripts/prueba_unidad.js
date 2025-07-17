var controlador = siteRoot + 'Eventos/pruebaunidad/';
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

$(function () {
    $('#txtFecha').Zebra_DatePicker({

    });

    $('#btnConsultar').click(function () {
        mostrarExcelWeb();
    });
    $('#cbEquipo').change(function () {
        mostrarFormulario(consulta);//limpiarBarra();
    });
    $('#txtFecha').click(function () {
        limpiarBarra();
    });

    $('#btnEnviarDatos').click(function () {
        if (evtHot.Handson.ReadOnly) {
            alert("No se puede enviar información, solo disponible para  visualización");
            return
        }
        else {
            enviarExcelWeb();
        }
    });

    $('#btnDescargarFormato').click(function () {
        if (validarSeleccionDatos()) {
            descargarFormato();
        }
        else {
            alert("Por favor seleccione la empresa correcta.");
        }
    });

    $('#btnMostrarErrores').click(function () {
        mostrarDetalleErrores();
    });

    $('#btnVerEnvios').click(function () {
        popUpListaEnvios();
    });


    $(document).ready(function () {

        //$("#txtFecha").val("18/01/2017");
        mostrarExcelWeb();

    });


    limpiarBarra();
    crearPupload();
});

function limpiarBarra() {
    $('#barra').css("display", "none");
    $('#detalleFormato').html("");
}
//Muestra la barra de herramemntas para administrar los datos de stock de combustible ingresados
function mostrarExcelWeb() {

    if ($("#txtFecha").val() != "") {

        $('#mensajeEvento').css("display", "none");
        $('#barra').css("display", "block");
        cargarUnidadSorteada();

    }
    else {
        alert("Error!.Ingresar fecha correcta");
    }

    //obtenerEmpresa("----");

}

cargarUnidadSorteada = function () {
    //alert(obtenerCadenaHf($("#hfIdEmprnomb").val()));
    $.ajax({
        type: 'POST',
        url: controlador + "unidadsorteada",
        data: {
            fecha: $('#txtFecha').val()
        },
        dataType: 'json',
        cache: false,
        success: function (evt) {
            /*
            var cad = evt;
            $('#cbEquipo').empty();
            var array = cad.split(',');
            var equicodi = array[0];
            var equinomb = array[1];
            var emprcodi = array[2];
            var emprnomb = array[3];
            */

            var _len = evt.length;
            var cad1 = _len + "\r\n";

            $('#cbEquipo').empty();

            limpiarBarra();

            if (_len == 1) {
                /*
                $("#hfIdEmprcodi").val(emprcodi);
                $("#hfIdEmprnomb").val(emprnomb);

                if (equicodi != "-1") {

                    var select2 = document.getElementById("cbEquipo");
                    select2.options[select2.options.length] = new Option(equinomb, equicodi);

                    if (equicodi != "0") {
                        $('#barra').css("display", "block");
                        $("#hfIdEnvio").val(0);
                        mostrarFormulario(consulta);
                    }
                }
                */

                for (i = 0; i < _len; i++) {

                    post = evt[i];

                    $("#hfIdEmprcodi").val(post.Emprcodi);
                    $("#hfIdEmprnomb").val(post.Emprnomb);
                    $("#hfIdEquicodi").val(post.Equicodi);


                    var select = document.getElementById("cbEquipo");
                    select.options[select.options.length] = new Option(post.Equinomb, post.Equicodi);

                    if (post.Equicodi != "0") {
                        $('#barra').css("display", "block");
                        $("#hfIdEnvio").val(0);
                        mostrarFormulario(consulta);
                    }
                }





            }
            else {

                var cadEmprcodi = ",";
                var cadEmprnomb = ",";
                var cadEquicodi = ",";

                for (i = 0; i < _len; i++) {

                    post = evt[i];

                    cadEmprcodi += post.Emprcodi + ",";
                    cadEmprnomb += post.Emprnomb + ",";
                    cadEquicodi += post.Equicodi + ",";

                    var select = document.getElementById("cbEquipo");
                    select.options[select.options.length] = new Option(post.Equinomb, post.Equicodi);

                }

                $('#barra').css("display", "block");
                $("#hfIdEnvio").val(0);
                mostrarFormulario(consulta);

                $("#hfIdEmprcodi").val(cadEmprcodi);
                $("#hfIdEmprnomb").val(cadEmprnomb);
                $("#hfIdEquicodi").val(cadEquicodi);

            }
        },
        error: function (xhr, textStatus, exceptionThrown) {
            mostrarError();
        }
    });
}

limpiarBarra = function () {
    $('#barra').css("display", "none");
    $('#filtro_grilla').css("display", "none");
    $('#detalleFormato').html("");
}


mostrarEnvioExcelWeb = function (envio) {
    $('#enviosanteriores').bPopup().close();
    $("#hfIdEnvio").val(envio);
    mostrarFormulario(envioAnterior);
    var mensaje = mostrarMensajeEnvio();
    mostrarExito(mensaje);
}

obtenerCadenaHf = function (valores) {
    //var n = cad.indexOf(",")
    //alert("x12")
    //alert($("#cbEquipo").val());
    //alert("x34")

    var equicodi = $("#cbEquipo").val();
    var equipo = $("#hfIdEquicodi").val().split(',');
    var cadarr = valores.split(',');
    var idx = equipo.indexOf(equicodi);

    if (idx >= 0) {
        return cadarr[idx];
    }
    else {
        return valores;
    }

}

mostrarFormulario = function (accion) {
    listErrores = [];
    var idEquipo = $("#cbEquipo").val();
    var fecha = $("#txtFecha").val();
    var idEnvio = $("#hfIdEnvio").val();
    $('#hfEquipo').val($('#cbEquipo').val());
    $('#hfFecha').val($('#txtFecha').val());
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    //alert($("#hfIdEmprcodi").val());
    $.ajax({
        type: 'POST',
        url: controlador + "MostrarDatos",
        dataType: 'json',
        data: {
            idEnvio: idEnvio,
            fecha: fecha,
            idEmpresa: obtenerCadenaHf($("#hfIdEmprcodi").val()),//$("#hfIdEmprcodi").val()
            equicodi: $("#cbEquipo").val()
        },
        success: function (evt) {

            if (evt != -1) {
                crearGrillaExcel(evt);
                evtHot = evt;
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
                        //var mensaje = mostrarMensajeEnvio();
                        //mostrarMensaje("Por favor complete los datos. <strong>Plazo del Envio: </strong>" + mensaje);
                        break;
                    case importarDatos:
                        mostrarExito("<strong>Los datos se cargaron correctamente, por favor presione el botón enviar para grabar.</strong>");
                        break;
                }
                if (evtHot.Handson.ReadOnly) {
                    $('#btnSelectExcel').css("display", "none");
                }
                else {
                    $('#btnSelectExcel').css("display", "block");
                }

            }
            else {
                alert("La empresa no tiene puntos de medición para cargar.");
                //document.location.href = controlador + 'envio';
            }
        },
        error: function () {
            alert("Error al cargar Excel Web");
        }
    });
}

enviarExcelWeb = function () {
    if (confirm("¿Desea enviar información a COES?")) {

        if (validarEnvio()) {
            var equipo = $('#cbEquipo').val();
            var fecha = $('#txtFecha').val();
            $('#hfEquipo').val(equipo);
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                traditional: true,
                url: controlador + "GrabarExcelWeb",
                data: JSON.stringify({
                    data: hot.getData(),
                    idEquipo: equipo,
                    fecha: fecha,
                    idEmpresa: obtenerCadenaHf($("#hfIdEmprcodi").val())//$('#hfIdEmprcodi').val()
                }),

                beforeSend: function () {
                    mostrarExito("Enviando Información ..");
                },
                success: function (evt) {
                    if (evt.Resultado == 1) {
                        $("#hfIdEnvio").val(evt.IdEnvio);
                        mostrarFormulario(envioDatos);
                        mostrarExito("Los datos se enviaron correctamente");
                    }
                    else {

                        if (evt.Resultado == -2) {
                            mostrarError("Usuario no pertenece a Empresa");
                        }
                        else {

                            if (evt.Resultado == -3) {
                                mostrarError("Punto de medición no registrado en formato. Solicitar configuración");
                            }
                            else {
                                mostrarError("Error al Grabar");
                            }
                        }
                    }
                },
                error: function () {
                    mostrarError();
                }

            });

        }

    }

}

validarEnvio = function () {
    retorno = true;
    totalErrores = listErrores.length;
    getTotalErrores();
    //valida si existen errores
    if ((totalErrores) > 0) {
        mostrarError("Existen errores en las celdas, favor corregir y vuelva a envíar");
        mostrarDetalleErrores();
        return false;
    }
    return retorno;

}

getTotalErrores = function () {
    nErrores = errores.length;
    total = 0;
    for (var i = 0; i < nErrores; i++) {
        total += errores[i].total;
    }
    return total;
}

descargarFormato = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'generarformato',
        async: false,
        data: {
            idEquipo: $('#hfEquipo').val(),
            dia: $('#hfFecha').val(),
            idEmpresa: obtenerCadenaHf($("#hfIdEmprcodi").val()),//$('#hfIdEmprcodi').val()
            equicodi: $("#cbEquipo").val()
        },
        success: function (result) {
            if (result.length > 0) {
                window.location.href = controlador + 'descargarformato?archivo=' + result;
            }
            else {
                alert("Error en descargar el archivo");
            }
        },
        error: function (result) {
            alert('ha ocurrido un error al descargar el archivo excel. ' + result.status + ' - ' + result.statusText + '.');
        }
    });
}

crearPupload = function () {

    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectExcel',
        url: siteRoot + 'StockCombustibles/envio/Upload',
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
                showMensaje();
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
                hideMensajeEvento();
            },
            UploadComplete: function (up, file) {
                hideMensaje();
                var retorno = leerFileUpExcel();
                if (retorno == 1) {
                    limpiarError();
                    $("#hfIdEnvio").val(-1);//-1 indica que el handsontable mostrara datos del archivo excel                    
                    mostrarFormulario(importarDatos);

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


mostrarMensajeEnvio = function () {

    var envio = $("#hfIdEnvio").val();

    if (envio > 0) {
        var plazo = (evtHot.EnPlazo) ? "<strong style='color:green'>en plazo</strong>" : "<strong style='color:red'>fuera de plazo</strong>";
        var mensaje = "<strong>Código de envío</strong> : " + envio + "   , <strong>Fecha de envío: </strong>" + evtHot.FechaEnvio + "   , <strong>Enviado </strong>" + plazo;
        return mensaje;
    }
    else {
        if (!evtHot.EnPlazo) {
            return "<strong style='color:red'>Fuera de plazo</strong>";
        }
        else return "<strong style='color:green'>En plazo</strong>";
    }
}

popUpListaEnvios = function () {
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
dibujarTablaEnvios = function (lista) {

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

getFormattedDate = function (date) {
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

mostrarDetalleErrores = function () {
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

dibujarTablaError = function () {

    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablaError' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Celda</th><th>Valor</th><th>Tipo Error</th></tr></thead>";
    cadena += "<tbody>";
    var len = listErrores.length;
    console.log("Dibujo:" + len);
    for (var i = 0; i < len; i++) {
        cadena += "<tr><td>" + listErrores[i].Celda + "</td>";
        cadena += "<td>" + listErrores[i].Valor + "</td>";
        cadena += "<td>" + errores[listErrores[i].Tipo].Descripcion + "</td></tr>";
    }
    cadena += "</tbody></table>";
    return cadena;
}

validarSeleccionDatos = function () {
    if (!($('#hfEquipo').val() == $('#cbEquipo').val() && $('#txtFecha').val() == $('#hfFecha').val())) {
        return false;
    }
    return true;
}

leerFileUpExcel = function () {
    //alert($('#hfEquipo').val() + "*" + $('#hfFecha').val() + "\n" + controlador + 'LeerFileUpExcel');
    var retorno = 0;
    $.ajax({
        type: 'POST',
        url: controlador + 'LeerFileUpExcel',
        dataType: 'json',
        async: false,
        data: {
            idEquipo: $('#hfEquipo').val(),
            dia: $('#hfFecha').val()
        },
        success: function (res) {
            retorno = res;
        },
        error: function () {
            mostrarError("Ha ocurrido un error");
        }
    });
    //alert(retorno);
    return retorno;
}

limpiarError = function () {
    totLimInf = 0;
    totLimSup = 0;
    totNoNumero = 0;
    listErrores = [];
}

indexOfPto = function (pto) {
    var index = -1;
    for (var i = 0; i < listaPtos.length; i++) {
        if (listaPtos[i].Ptomedicodi == pto) {
            index = i;
            break;
        }
    }

    return index;
}

formatFloat = function (num, casasDec, sepDecimal, sepMilhar) {

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
