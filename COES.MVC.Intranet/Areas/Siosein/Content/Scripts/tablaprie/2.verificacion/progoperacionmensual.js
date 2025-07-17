var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

var datos1 = null;
var datos2 = null;
var datos3 = null;

$(function () {        
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#btnConsultar').click(function () {
        $("#hfIdEnvio").val("0");
        mostrarExcelWeb();
    });

    $('#btnMostrarErrores').click(function () {
        mostrarDetalleErrores02();
    });
    $('#btnEnviarDatos').click(function () {
        if (typeof hot1 != 'undefined' || typeof hot2 != 'undefined' || typeof hot3 != 'undefined') {
            enviarExcelWeb();
        }
    });

    $('#btnVerEnvios').click(function () {
        popUpListaEnvios();
    });

    $("#hfIdEnvio").val("0");
    mostrarExcelWeb();
});

function mostrarEnvioTablaPrie01(envio) {
    $('#enviosanteriores').bPopup().close();
    $("#hfIdEnvio").val(envio);

  /*  mostrarFormulario(envioAnterior);*/
    mostrarExcelWeb();
}

function enviarExcelWeb() {
    var H1 = hot1.getData();
    var H2 = hot2.getData();
    var H3 = hot3.getData();
    if (H1[0][1] != "" || H2[0][1] != "" || H3[0][1] != "") {
        
        if (confirm("¿Desea enviar información a COES?")) {
            if (validarEnvio()) {
                
                var fecha = $('#txtFecha').val();

                if (H1[0][1] != "") {
                    datos1 = Obtenerdatos(hot1, datos1);
                }
                if (H2[0][1] != "") {
                    datos2 = Obtenerdatos(hot2, datos2);
                }
                if (H3[0][1] != "") {
                    datos3 = Obtenerdatos(hot3, datos3);
                }

                    $.ajax({
                        type: 'POST',
                        dataType: 'json',
                        //async: false,
                        contentType: 'application/json',
                        traditional: true,
                        url: controlador + "GrabarDatosTablaPrie26dec",
                        data: JSON.stringify({
                            data1: datos1,
                            data2: datos2,
                            data3: datos3,
                            fecha: fecha
                        }),
                        beforeSend: function () {
                            mostrarExito("Enviando Información ..");
                        },
                        success: function (evt) {
                            if (evt.Resultado == 1) {
                                $("#hfIdEnvio").val(evt.Resultado);
                                mostrarExcelWeb();
                                //hideMensaje();
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
    } else { alert("Excel Web vacio"); }
}
function getTotalErrores() {
    nErrores = errores.length;
    total = 0;
    for (var i = 0; i < nErrores; i++) {
        total += errores[i].total;
    }
    return total;
}

function Obtenerdatos(Tmp, datos) {
    var aCols = ['A'];
    var mydata = Tmp.getData();
    datos = mydata.slice();
    for (i = 0; i < mydata.length; i++) {
        for (j = 0; j < mydata[i].length; j++) {
            if (mydata[i][j] != null) {

                if (j == 0) {
                    /*datos[i][0] = Tmp.plugin.helper.cellValue(aCols[j] + (i + 1));*/
                  //  var celda = hot1.plugin.helper.cellValue(aCols[j] + (i + 1));
                    datos[i][0] = Tmp.getDataAtCell(i, 0);
                }
                else {
                    datos[i][j] = mydata[i][j];
                }
            }
        }
    }
    return datos;

}


function validarEnvio() {
    retorno = true;
    /*if (!validarRegistrosRepetidos())
        return false;*/
    totalErrores = listErrores.length;
    getTotalErrores();
    //valida si existen errores
    if ((totalErrores) > 0) {
        mostrarError("Existen errores en las celdas, favor corregir y vuelva a envíar");
        mostrarDetalleErrores02();
        return false;
    }
    //Valida los registro en blanco
  /*  $('#hfDataExcel').val((hot.getData()));
    var data = hot.getData();
    totalB = getTotBlancos(data);
    if (totalB > 0) {
        alert("Existen Celdas en Blanco, favor completar la información");
        return false;
    }*/
    return true;

}

function mostrarDetalleErrores02() {
    $('#idTerrores').html(NewdibujarTablaError());
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

//Muestra la barra de herramemntas para administrar los datos de declaración mensual ingresados
function mostrarExcelWeb() {
    if ($("#txtFecha").val() != "") {
        $('#mensajeEvento').css("display", "none");
        showMensaje();
        $('#barraDeclaracion').css("display", "block");
        for (var x = 1; x < 4; x++) {
            mostrarFormulario(consulta,x);
        }
        
    }
    else {
        alert("Error!.Ingresar fecha correcta");
    }
}

function mostrarFormulario(accion, tipo) {
    listErrores = [];    
    var fecha = $("#txtFecha").val();
    var idEnvio = $("#hfIdEnvio").val();

    if (tipo == 1) {
        if (typeof hot1 != 'undefined') {
            hot1.destroy();
        }
    }
    if (tipo == 2) {
        if (typeof hot2 != 'undefined') {
            hot2.destroy();
        }
    }
    if (tipo == 3) {
        if (typeof hot3 != 'undefined') {
            hot3.destroy();
        }
    }


    
    $.ajax({
        type: 'POST',
        url: controlador + "MostrarGridExcelWeb",
        dataType: 'json',
        //async: false,
        data: {            
            idEnvio: idEnvio,
            fecha: fecha,
            tabla: 26,
            tipo:tipo
        },
        success: function (evt) {
            if (evt != -1) {
                evtHot = evt;
                /*alert(evt);*/
                crearGrillaExcelProgOperacionMensual(evt, tipo);
                if (tipo == 1) {
                    hot1.render();
                }
                if (tipo == 2) {
                    hot2.render();
                }
                if (tipo == 3) {
                    hot3.render();
                }
                
                
                switch (accion) {
                    case envioDatos:
                        //var mensaje = mostrarMensajeEnvio(idEnvio);
                        mostrarExito("Los datos se enviaron correctamente.");
                        hideMensaje();
                        break;
                    case envioAnterior:
                        var mensaje = mostrarMensajeEnvio(idEnvio);
                        mostrarExito(mensaje);
                        hideMensaje();
                        break;
                    case consulta:
                        mostrarExito("Informacion Exitosa!!");
                        //var mensaje = mostrarMensajeEnvio();
                        //mostrarMensaje("Por favor complete los datos. <strong>Plazo del Envio: </strong>" + mensaje);
                        break;
                    case importarDatos:
                        mostrarExito("<strong>Los datos se cargaron correctamente, por favor presione el botón enviar para grabar.</strong>");
                        break;
                }
            }
            else {
                /*alert("La empresa no tiene puntos de medición para cargar.");*/
               /* document.location.href = controlador + 'ProgOperacionMensual';*/
            }
        },
        error: function () {
            alert("Error al cargar Excel Web");
        }
    });
}

function crearPupload() {
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnImportar',
        url: siteRoot + 'Siosein/TablasPrieDeclaracionMen/Upload',
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
                if (retorno.Retorno == 1) {
                    limpiarError();
                    $("#hfIdEnvio").val(-1);//-1 indica que el handsonetable mostrara datos del archivo excel                    
                    mostrarFormulario(importarDatos);
                    //crearGrillaExcelDeclaracion(retorno);
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

function mostrarAlerta(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
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

function limpiarError() {
    totLimInf = 0;
    totLimSup = 0;
    totNoNumero = 0;
    listErrores = [];
}

function NewvalidarError(Tab, celda) {
    for (var j in listErrores) {
        if (listErrores[j]['Celda'] == celda && listErrores[j]['Tab']) {
            return false;
        }
    }
    return true;
}

function NewdibujarTablaError() {

    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablaError' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Pestaña</th><th>Celda</th><th>Valor</th><th>Tipo Error</th></tr></thead>";
    cadena += "<tbody>";
    var len = listErrores.length;
    console.log("Dibujo:" + len);
    for (var i = 0 ; i < len ; i++) {
        cadena += "<tr><td>" + listErrores[i].Tab + "</td>";
        cadena += "<td>" + listErrores[i].Celda + "</td>";
        cadena += "<td>" + listErrores[i].Valor + "</td>";
        cadena += "<td>" + errores[listErrores[i].Tipo].Descripcion + "</td></tr>";
    }
    cadena += "</tbody></table>";
    return cadena;
}

function NewindexOfError(Tab, celda) {
    var index = -1;
    for (var i = 0; i < listErrores.length; i++) {
        if (listErrores[i].Celda == celda && listErrores[i].Tab == Tab) {
            index = i;
            break;
        }
    }

    return index;
}

function NeweliminarError(Tab, celda, tipoError) {
    var index = NewindexOfError(Tab, celda);
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
            case errorNoOsinergCodi:
                errores[errorNoOsinergCodi].total--;
                break;
            case errorNoUnidadOsinergCodi:
                errores[errorNoUnidadOsinergCodi].total--;
                break;
        }
    }
}

function NewagregarError(Tab, celda, valor, tipo) {
    if (NewvalidarError(Tab, celda)) {
        var regError = {
            Tab: Tab,
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
            case errorNoOsinergCodi:
                errores[errorNoOsinergCodi].total++;
                break;
            case errorNoUnidadOsinergCodi:
                errores[errorNoUnidadOsinergCodi].total++;
                break;
        }
    }
}

function NeweliminarFilaHandson(tipo) {

    if (tipo == 1) {
        grillaBD = hot1.getData();
        grillaBD.pop();
        index = grillaBD.length;
        hot1.updateSettings({
            maxRows: index - 1
        });
    }
    if (tipo == 2) {
        grillaBD = hot2.getData();
        grillaBD.pop();
        index = grillaBD.length;
        hot2.updateSettings({
            maxRows: index - 1
        });
    }
    if (tipo == 3) {
        grillaBD = hot3.getData();
        grillaBD.pop();
        index = grillaBD.length;
        hot3.updateSettings({
            maxRows: index - 1
        });
    }
   
    
}