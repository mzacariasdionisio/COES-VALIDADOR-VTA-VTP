var controlador = siteRoot + "rechazocarga/EnvioArchivosMagnitud/";
var uploader;
var uploader2;
var totCampoVacio = 0;
var totNoNumero = 0;
var totValorNegatico = 0;
var totFormatoFecha = 0;
var totValidacionFecha = 0;

var listErrores = [];
var listDescripErrores = ["Campo vacío", "No es número", "Valor negativo", "Formato fecha debe ser dd/mm/aaaa hh:mm", "Fecha Fin debe ser mayor a Fecha Inicio"];

var listErrores2 = [];
var listDescripErrores2 = ["Campo vacío", "No es número", "Valor negativo", "Formato fecha debe ser dd/mm/aaaa hh:mm", "Fecha Hora Inicio debe ser menor o igual a " ,"Fecha Hora Final debe ser mayor o igual a "];

var validaInicial = true;
var hot;
var hotOptions;
var evtHot;

var hot2;
var hotOptions2;
var evtHot2;
var totCampoVacio2 = 0;
var totNoNumero2 = 0;
var totValorNegatico2 = 0;
var validaInicial2 = true;


$( /**
   * Llamadas iniciales
   * @returns {} 
   */
   $(document).ready(function () {
       //mostrarMensaje("Selecionar el periodo y la versión");
       $('#tab-container').easytabs({
           animate: false
       });


       $('#suministrador').change(function () {
           inicializarBusqueda();
       });

       $('#tipo').change(function () {
           obtenerProgramas();
       });

       $('#programa').change(function () {
           obtenerCuadros();
       });

       $('#btnConsultar').click(function () {
           $('#tab-container').easytabs('select', "#tabResumen");

           obtenerDatosArchivosMagnitud();
       });

       $("#btnDescargarFormato").click(function () {
           descargarFormato();
       });
       $('#btnMostrarErrores').click(function () {
           mostrarErrores(true);
       });

       $('#btnEnviarDatos').click(function () {
           guardarDatosArchivosMagnitud();
       });
       
       $('#cbClientes').change(function () {
           obtenerClienteDetalle();
       });

       $('#btnEnviarDatosDetalle').click(function () {
           guardarDatosArchivosMagnitudDetalle();
       });

       $('#btnMostrarErroresDetalle').click(function () {
           mostrarErroresDetalle(true);
       });

       $("#btnDescargarFormatoDetalle").click(function () {
           descargarFormatoDetalle();
       });

       crearPupload();
       crearPuploadDetalle();

   }));

function inicializarBusqueda() {
    $('#tipo').val('');    
    $("#programa").empty();
    $("#cuadro").empty();

    $("#programa").append('<option value="0" selected="selected">--SELECCIONE--</option>');
    $("#cuadro").append('<option value="0" selected="selected">--SELECCIONE--</option>');
}
function crearPupload() {
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectExcel',
        url: siteRoot + 'RechazoCarga/EnvioArchivosMagnitud/Subir',
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
                leerExcelSubido();
                limpiarError();
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploader.init();
}

function leerExcelSubido() {
    $.ajax({
        type: 'POST',
        url: controlador + 'LeerExcelSubido',
        dataType: 'json',
        async: false,
        data: {
        },
        success: function (respuesta) {
            if (respuesta.Exito == false) {
                mostrarError(respuesta.Mensaje);
            } else {
                if (typeof hot != 'undefined') {
                    hot.destroy();
                }
                crearHandsonTable(respuesta.Datos, true);
                evtHot = respuesta.Datos;
                mostrarExito("Archivo importado");
            }
        },
        error: function () {
            mostrarError("Ha ocurrido un error");
        }
    });
}

function limpiarError() {
    var totCampoVacio = 0;
    var totNoNumero = 0;
    var totValorNegatico = 0;
    
    var listErrores = [];
}

function mostrarErrores(activar) {
    totCampoVacio = 0;
    totNoNumero = 0;
    totValorNegatico = 0;
    totFormatoFecha = 0;
    totValidacionFecha = 0;
    validaInicial = true;
    listErrores.splice(0, listErrores.length);

    validarRegistros(evtHot);
    if (activar) {
        activarPopupErrores();
    }
}
function descargarFormato() {
    var programa = $("#programa").val();
    var cuadro = $("#cuadro").val();
    var suministrador = $("#suministrador").val();
    var tipo = $("#tipo").val();

    if (hot == undefined) {
        return false;
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarFormato',
        dataType: 'json',
        data: {
            programa: programa,
            cuadro: cuadro,
            suministrador: suministrador,
            tipo: tipo,
            datos: JSON.stringify(hot.getData())
        },
        success: function (result) {
            if (result == "1") {
                window.location = controlador + 'DescargarFormato';
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

function obtenerCuadros() {
    $("#cuadro").empty();
    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerCuadrosPorPrograma',
        dataType: 'json',
        data: {
            programa: $("#programa").val()
        },
        success: function (cuadros) {
            $.each(cuadros, function (i, cuadro) {
                $("#cuadro").append('<option value="' + cuadro.Value + '">' +
                     cuadro.Text + '</option>');
            });
        },
        error: function (ex) {
            mostrarError('Consultar cuadros por programa: Ha ocurrido un error');
        }
    });
};

function obtenerProgramas() {
    $("#programa").empty();
    $("#cuadro").empty();
    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerProgramas',
        dataType: 'json',
        data: {
            tipo: $("#tipo").val()
        },
        success: function (programas) {
            $.each(programas, function (i, programa) {
                $("#programa").append('<option value="' + programa.Value + '">' +
                     programa.Text + '</option>');
            });
            $("#cuadro").append('<option value="0" selected="selected">--SELECCIONE--</option>');
        },
        error: function (ex) {
            mostrarError('Consultar programas: Ha ocurrido un error');
        }
    });
};

function activarPopupErrores() {
    setTimeout(function () {
        $('#idTerrores').html(dibujarTablaError());

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

    }, 200);
}

function dibujarTablaError() {
    var totalErrores = listErrores.length;
    var valorLimite = 50;
    var totalErroresMostrar = totalErrores > valorLimite ? valorLimite : totalErrores;
    var cadena = "";
    if (totalErrores > valorLimite) {
        cadena += "<div>Se encontraron " + totalErrores + " errores</div>";
    }
    cadena += "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablaError' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Celda</th><th>Valor</th><th>Error</th></tr></thead>";
    cadena += "<tbody>";
    for (var i = 0 ; i < totalErroresMostrar ; i++) {
        cadena += "<tr><td>" + listErrores[i].Celda + "</td>";
        
        if (listErrores[i].Tipo >= 0) {
            cadena += "<td>" + listErrores[i].Valor + "</td>";
            cadena += "<td>" + listDescripErrores[listErrores[i].Tipo] + "</td></tr>";
        } else {
            cadena += "<td>" +  "</td>";
            cadena += "<td>" + listErrores[i].Valor + "</td></tr>";
        }
        
    }
    cadena += "</tbody></table>";
    return cadena;
}

function enviarDatos() {
    mostrarErrores(false);
    setTimeout(function () {
        if (listErrores.length > 0) {
            activarPopupErrores();
        } else {
            if (confirm("¿Desea enviar información a COES?")) {
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    url: controlador + "GrabarParametrosEsquema",
                    data: {
                        datos: JSON.stringify(hot.getData()),
                        anio: $('#anio').val()
                    },
                    beforeSend: function () {
                        mostrarAlerta("Enviando Información ..");
                    },
                    success: function (evt) {
                        mostrarMensaje("Información grabada correctamente.")
                        obtenerParametrosEsquema();
                    },
                    error: function () {
                        mostrarError("Ocurrió un error");
                    }
                });
            }

        }
    }, 100);
    
}

function obtenerDatosArchivosMagnitud() {
    
    obtenerModelo(false);
}
function guardarDatosArchivosMagnitud() {

    mostrarErrores(false);

    var programa = parseInt($("#programa").val());
    var cuadro = $("#cuadro").val();
    var suministrador = $("#suministrador").val();
    var tipo = $("#tipo").val();

    if (suministrador == '' || suministrador == '0') {
        mostrarAlerta('Debe seleccionar un suministrador');
        return false;
    }

    if (tipo == '' || tipo == '0') {
        mostrarAlerta('Debe seleccionar un tipo');
        return false;
    }

    if (programa == '' || programa == '0') {
        mostrarAlerta('Debe seleccionar un programa');
        return false;
    }

    if (cuadro == '' || cuadro == '0') {
        mostrarAlerta('Debe seleccionar un cuadro de programa');
        return false;
    }

    setTimeout(function () {

        //validarRegistros(evtHot);

        if (listErrores.length > 0) {
            activarPopupErrores();
        } else {
            if (confirm("¿Desea enviar información a COES?")) {
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    url: controlador + "GrabarDatosArchivosMagnitud",
                    data: {
                        datos: JSON.stringify(hot.getData()),
                        codigoSuministrador: suministrador,
                        tipo: tipo,
                        codigoPrograma: programa,
                        codigoCuadroPrograma: cuadro
                    },
                    beforeSend: function () {
                        mostrarAlerta("Enviando Información ..");
                    },
                    success: function (respuesta) {
                        if (respuesta.Exito) {                           
                            setTimeout(function () {
                                obtenerDatosArchivosMagnitud();
                            }, 2000);
                            mostrarMensaje("Información grabada correctamente. Cargando...");
                            //var empresas = $("#empresasSeleccionadas").val();
                        } else {
                            
                            for (var i = 0; i < respuesta.ListaErrores.length; i++) {
                                var fila = respuesta.ListaErrores[i];
                                agregarError(fila.Celda, fila.Error, -1);
                            }
                            mostrarMensaje("Hubieron errores, revisar.");
                            activarPopupErrores();                            
                        }
                    },
                    error: function () {
                        mostrarError("Ocurrió un error");
                    }
                });
            }
        }
    }, 100);
}
function obtenerModelo(validar) {
    var programa = parseInt($("#programa").val());
    var cuadro = $("#cuadro").val();
    var suministrador = $("#suministrador").val();
    var tipo = $("#tipo").val();

    if (suministrador == '' || suministrador == '0') {
        mostrarAlerta('Debe seleccionar un suministrador');
        return false;
    }

    if (tipo == '' || tipo == '0') {
        mostrarAlerta('Debe seleccionar un tipo');
        return false;
    }

    if (programa == '' || programa == '0') {
        mostrarAlerta('Debe seleccionar un programa');
        return false;
    }

    if (cuadro == '' || cuadro == '0') {
        mostrarAlerta('Debe seleccionar un cuadro de programa');
        return false;
    }    

    if (typeof hot != 'undefined') {
        hot.destroy();
    }

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerFormatoModelArchivosMagnitud",
        dataType: 'json',
        data: {
            programa: programa,
            cuadro: cuadro,
            suministrador: suministrador,
            tipo: tipo
        },
        success: function (evt) {
            mostrarMensaje('Consulta Generada');
            crearHandsonTable(evt.formatoClientes, validar);
            evtHot = evt.formatoClientes;

            CargarListaClientesDetalle(evt.ListClientes);
        },
        error: function () {
            alert("Error al cargar Excel Web");
        }
    });
}
var container;
function crearHandsonTable(evtHot, validar) {
    function celdaNumerica(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.textAlign = 'right';
    }

    function tituloAzul(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.color = 'white';
        td.style.background = 'darkblue';
    }

    function tituloVerdeClaro(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.color = 'white';
        td.style.background = 'lightgreen';
    }

    function tituloVerdeOscuro(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.color = 'white';
        td.style.background = 'green';
    }

    function errorRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.color = 'black';
        td.style.background = '#FF4C42';
    }

    function cambiosCellRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'right';
        td.style.background = '#FFFFD7';
        $(td).html(formatFloat(parseFloat(value), 3, '.', ','));
    }

    function negativeValueRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        if (parseInt(value, 10) < 0) {
            td.style.color = 'orange';
            td.style.fontStyle = 'italic';
        }
    }

    function limitesCellRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'right';
        if (Number(value) && value != "") {
            if (Number(value) < evtHot.ListaHojaPto[col - 1].Hojaptoliminf) {
                td.style.background = 'orange';
                $(td).html(formatFloat(Number(value), 3, '.', ','));
                var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                agregarError(celda, value, 3);

            }
            else {
                if (Number(value) > evtHot.ListaHojaPto[col - 1].Hojaptolimsup) {
                    td.style.background = 'yellow';
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    agregarError(celda, value, 4);
                }
                else {
                    td.style.background = 'white';
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                }
            }
        }
        else {
            if (value == "0")
                $(td).html("0.000");
            else if (value != "") {
                if (!Number(value)) {
                    td.style.background = 'red';
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    agregarError(celda, value, 2);
                }
            }
        }

    }

    function readOnlyValueRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'left';
        //td.style.color = 'DimGray';
        //td.style.background = 'Gainsboro';
        td.style.color = 'black';
        td.style.background = 'white';
        if (parseFloat(value)) {
            td.style.textAlign = 'right';
            //$(td).html(formatFloat(parseFloat(value), 3, '.', ','));
        }
        else {
            if (value == "0" || value == "0.00") {
                //$(td).html("0.000");
                td.style.textAlign = 'right';
            }
                
        }
    }

    function formatFloat(valor, numeroDecimales, separadorDecimalOriginal, separadorDecimalNuevo) {
        var redondeado = parseInt(Math.round(valor * Math.pow(10, numeroDecimales))) / Math.pow(10, numeroDecimales);
        var texto = redondeado.toString();
        var formateadoDecimal = texto.replace(separadorDecimalOriginal, separadorDecimalNuevo);
        return formateadoDecimal;
    }

    function calculateSize() {
        var offset;
        offset = Handsontable.Dom.offset(container);

        if (offset.top == 222) {
            availableHeight = $(window).height() - offset.top - 90;
        }
        else {
            availableHeight = $(window).height() - offset.top - 20;
        }

        //availableWidth = $(window).width() - 2 * offset.left;
        container.style.height = (availableHeight + 50) + 'px';
        hot.render();
    }

    container = document.getElementById('detalleFormato');
    Handsontable.renderers.registerRenderer('negativeValueRenderer', negativeValueRenderer);
    Handsontable.Dom.addEvent(window, 'resize', calculateSize);
    Handsontable.Dom.addEvent(container, 'click', function () {
        validaInicial = false;
    });

    hotOptions = {
        data: evtHot.Handson.ListaExcelData,
        height: 550,
        maxRows: evtHot.Handson.ListaExcelData.length,
        maxCols: evtHot.Handson.ListaExcelData[0].length,
        colHeaders: true,
        rowHeaders: true,
        fillHandle: true,
        columnSorting: false,
        className: "htCenter",
        readOnly: evtHot.Handson.ReadOnly,
        fixedRowsTop: evtHot.FilasCabecera + 1,
        //fixedColumnsLeft: evtHot.ColumnasCabecera,
        fixedColumnsLeft: 0,
        mergeCells: evtHot.Handson.ListaMerge,
        colWidths: evtHot.Handson.ListaColWidth,
        stretchH: 'all',
        contextMenu: false,
        afterLoadData: function () {
            this.render();
        },
        hiddenColumns: {
            columns: [9, 10, 11],
            indicators: false
        },
        beforeChange: function (changes, source) {
            for (var i = changes.length - 1; i >= 0; i--) {
                //var valorOld = changes[i][2];
                //var valorNew = changes[i][3];
                //var liminf = evtHot.ListaHojaPto[changes[i][1] - 1].Hojaptoliminf;
                //var limsup = evtHot.ListaHojaPto[changes[i][1] - 1].Hojaptolimsup;
                //var tipoOld = getTipoError(valorOld, liminf, limsup);//isNaN(changes[i][2]) ? true : false;
                //var tipoNew = getTipoError(valorNew, liminf, limsup); //isNaN(changes[i][3]) ? true : false;
                //var celda = getExcelColumnName(parseInt(changes[i][1]) + 1) + (parseInt(changes[i][0]) + 1).toString();
                //if (tipoOld > 1) {
                //    eliminarError(celda, tipoOld);
                //    if (tipoNew > 1) {
                //        agregarError(celda, changes[i][3], tipoNew);
                //    }
                //}
                //else {
                //    if (tipoNew > 1) {
                //        agregarError(celda, changes[i][3], tipoNew);
                //    }
                //}
            }
        },

        cells: function (row, col, prop) {
            var cellProperties = {};
            var formato = "";
            var render;
            var readOnly = true;
            var tipo;
            if (row == 0) {
                if (col == 6) {
                    render = tituloAzul;
                }
                readOnly = true;
            }
            if (row == 1) {
                render = tituloAzul;
                readOnly = true;
            }
            if (row > 1) {
                readOnly = col <= 5;

                if (col <= 5) {
                    render = readOnlyValueRenderer;
                }
                if (col == 6) {
                    render = celdaNumerica;
                }
            }

            cellProperties = {
                renderer: render,
                format: formato,
                type: tipo,
                readOnly: readOnly
            }

            return cellProperties;
        }
    };

    hot = new Handsontable(container, hotOptions);
    //calculateSize(1);
    hot.render();
    //if (validar == true) {
    //validarRegistros(evtHot);
    //}
}

function validarRegistros(data) {
    for (var row = 0; row < data.Handson.ListaExcelData.length; row++) {
        for (var col = 0; col < data.Handson.ListaExcelData[0].length; col++) {
            if ((row > data.FilasCabecera) && (col <= data.ColumnasCabecera) && (!data.Handson.ReadOnly)) {
                if (!data.Handson.ListaFilaReadOnly[row]) {
                    var value = data.Handson.ListaExcelData[row][col];
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();

                    if (col == 0) {
                        if (value == null || value == "") {
                            agregarError(celda, value, 0);
                        }
                    }
                    if (col == 6) {
                        if (!validarDecimal(value)) {
                            agregarError(celda, value, 1);
                        } else {
                            if (Number(value) < 0) {
                                agregarError(celda, value, 2);
                            }
                        }
                    }
                    if (col == 7 || col == 8) {
                        if (!validarFecha(value)) {
                            agregarError(celda, value, 3);
                        }

                        
                        if (col == 8) {                                            
                            var fechahoraInicio = data.Handson.ListaExcelData[row][col - 1];     
                            if (validarFecha(fechahoraInicio)) {
                                var fechahoraFin = value;
                                var fechahoraInicioFormateada = formatearFecha(fechahoraInicio);
                                var fechahoraFinFormateada = formatearFecha(fechahoraFin);
                                if (fechahoraFinFormateada < fechahoraInicioFormateada) {
                                    agregarError(celda, value, 4);
                                }
                            }
                            
                        }
                    }
                }
            }
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

validarFecha = function (valor) {
    if (valor == null || valor.length <= 0) return false;
    var expresionFecha = /^(0?[1-9]|[12][0-9]|3[01])[/](0?[1-9]|1[0-2])[/](19[5-9][0-9]|20[0-4][0-9]|2050)\s([0-1]?[0-9]|2?[0-3]):([0-5]\d)$/;
    return expresionFecha.test(valor);
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
            case 0:
                totCampoVacio++;
                break;
            case 1:
                totNoNumero++;
                break;
            case 2:
                totValorNegatico++;
            case 3:
                totFormatoFecha++;
            case 4:
                totValidacionFecha++;
                break;
        }
    }
}

function mostrarMensaje(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
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
function mostrarExito(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}

function CargarListaClientesDetalle(lista) {
    $('#cbClientes').empty();
    var cbPrueba = document.getElementById("cbClientes");
    for (i = 0; i < lista.length; i += 1) {

        var fila = lista[i];
        var option = document.createElement('option');
        option.value = fila.Rcproucodi;
        option.text = fila.Empresa;
        cbPrueba.add(option);
    };    

    if (typeof hot2 != 'undefined') {
        //$('#detalleCliente').destroy(); 
        //hot2.destroy();
        $('#detalleCliente').html('');
    }
    $('#mensajeFechas').html('');
}

function obtenerClienteDetalle() {
    
    var cliente = $('#cbClientes').val();
    if (cliente == '' || cliente == '0') {
        if (typeof hot2 != 'undefined') {
            //hot2.destroy();
            //hotOptions2.data = [];
            $('#detalleCliente').html('');
        }
        $('#mensajeFechas').html('');
    } else {
        if (typeof hot2 != 'undefined') {
            hot2.destroy();            
        }
        ObtenerModeloDetalle(false);
    }
}

function ObtenerModeloDetalle(validar) {
    var cuadro = $('#cuadro').val();
    if (cuadro == '' || cuadro == '0') {
        mostrarAlerta('Debe seleccionar un cuadro de programa');
        return false;
    }

    var cliente = $('#cbClientes').val();
    if (cliente == '' || cliente == '0') {
        mostrarAlerta('Debe seleccionar un cliente.');
        return false;
    }

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerFormatoModelArchivosMagnitudDetalle",
        dataType: 'json',
        data: {
            codigoCuadroPrograma: cuadro,
            clienteId: cliente
        },
        success: function (evt) {
            mostrarMensaje('Consulta Generada');
            crearHandsonTable2(evt.formatoClientes, validar);
            evtHot2 = evt.formatoClientes;

            $('#mensajeFechas').html(evt.RangoFechas);
            $('#fechaInicioDetalle').val(evt.fechaInicioDetalle);
            $('#fechaFinDetalle').val(evt.fechaFinDetalle);
        },
        error: function () {
            alert("Error al cargar Excel Web");
        }
    });
}

var container2;
function crearHandsonTable2(evtHot, validar) {
    function celdaNumerica(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.textAlign = 'right';
    }

    function tituloAzul(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.color = 'white';
        td.style.background = 'darkblue';
    }

    function tituloVerdeClaro(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.color = 'white';
        td.style.background = 'lightgreen';
    }

    function tituloVerdeOscuro(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.color = 'white';
        td.style.background = 'green';
    }

    function errorRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.color = 'black';
        td.style.background = '#FF4C42';
    }

    function cambiosCellRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'right';
        td.style.background = '#FFFFD7';
        $(td).html(formatFloat(parseFloat(value), 3, '.', ','));
    }

    function negativeValueRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        if (parseInt(value, 10) < 0) {
            td.style.color = 'orange';
            td.style.fontStyle = 'italic';
        }
    }

    function limitesCellRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'right';
        if (Number(value) && value != "") {
            if (Number(value) < evtHot.ListaHojaPto[col - 1].Hojaptoliminf) {
                td.style.background = 'orange';
                $(td).html(formatFloat(Number(value), 3, '.', ','));
                var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                agregarError(celda, value, 3);

            }
            else {
                if (Number(value) > evtHot.ListaHojaPto[col - 1].Hojaptolimsup) {
                    td.style.background = 'yellow';
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    agregarError(celda, value, 4);
                }
                else {
                    td.style.background = 'white';
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                }
            }
        }
        else {
            if (value == "0")
                $(td).html("0.000");
            else if (value != "") {
                if (!Number(value)) {
                    td.style.background = 'red';
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    agregarError(celda, value, 2);
                }
            }
        }

    }

    function readOnlyValueRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'left';
        //td.style.color = 'DimGray';
        //td.style.background = 'Gainsboro';
        td.style.color = 'black';
        td.style.background = 'white';
        if (parseFloat(value)) {
            td.style.textAlign = 'right';
            $(td).html(formatFloat(parseFloat(value), 3, '.', ','));
        }
        else {
            if (value == "0")
                $(td).html("0.000");
        }
    }

    function formatFloat(valor, numeroDecimales, separadorDecimalOriginal, separadorDecimalNuevo) {
        var redondeado = parseInt(Math.round(valor * Math.pow(10, numeroDecimales))) / Math.pow(10, numeroDecimales);
        var texto = redondeado.toString();
        var formateadoDecimal = texto.replace(separadorDecimalOriginal, separadorDecimalNuevo);
        return formateadoDecimal;
    }

    function calculateSize2() {
        var offset;
        offset = Handsontable.Dom.offset(container2);

        if (offset.top == 222) {
            availableHeight = $(window).height() - offset.top - 90;
        }
        else {
            availableHeight = $(window).height() - offset.top - 20;
        }

        //availableWidth = $(window).width() - 2 * offset.left;
        container2.style.height = (availableHeight + 50) + 'px';
        hot.render();
    }

    container2 = document.getElementById('detalleCliente');
    Handsontable.renderers.registerRenderer('negativeValueRenderer', negativeValueRenderer);
    Handsontable.Dom.addEvent(window, 'resize', calculateSizeDetalle);
    Handsontable.Dom.addEvent(container2, 'click', function () {
        validaInicial = false;
    });

    hotOptions2 = {
        data: evtHot.Handson.ListaExcelData,
        height: 550,
        maxRows: evtHot.Handson.ListaExcelData.length,
        maxCols: evtHot.Handson.ListaExcelData[0].length,
        colHeaders: true,
        rowHeaders: true,
        fillHandle: true,
        columnSorting: false,
        className: "htCenter",
        readOnly: evtHot.Handson.ReadOnly,
        fixedRowsTop: evtHot.FilasCabecera,
        //fixedColumnsLeft: evtHot.ColumnasCabecera,
        fixedColumnsLeft: 0,
        mergeCells: evtHot.Handson.ListaMerge,
        colWidths: evtHot.Handson.ListaColWidth,
        stretchH: 'none',
        contextMenu: false,        
        //hiddenColumns: {
        //    columns: [2],
        //    indicators: false
        //},
        beforeChange: function (changes, source) {
            for (var i = changes.length - 1; i >= 0; i--) {
                //var valorOld = changes[i][2];
                //var valorNew = changes[i][3];
                //var liminf = evtHot.ListaHojaPto[changes[i][1] - 1].Hojaptoliminf;
                //var limsup = evtHot.ListaHojaPto[changes[i][1] - 1].Hojaptolimsup;
                //var tipoOld = getTipoError(valorOld, liminf, limsup);//isNaN(changes[i][2]) ? true : false;
                //var tipoNew = getTipoError(valorNew, liminf, limsup); //isNaN(changes[i][3]) ? true : false;
                //var celda = getExcelColumnName(parseInt(changes[i][1]) + 1) + (parseInt(changes[i][0]) + 1).toString();
                //if (tipoOld > 1) {
                //    eliminarError(celda, tipoOld);
                //    if (tipoNew > 1) {
                //        agregarError(celda, changes[i][3], tipoNew);
                //    }
                //}
                //else {
                //    if (tipoNew > 1) {
                //        agregarError(celda, changes[i][3], tipoNew);
                //    }
                //}
            }
        },

        cells: function (row, col, prop) {
            var cellProperties = {};
            var formato = "";
            var render;
            var readOnly = true;
            var tipo;
           
            if (row == 0) {
                render = tituloAzul;
                readOnly = true;
            }
            if (row > 0) {
                readOnly = col < 1;
                                
                if (col == 1) {
                    render = celdaNumerica;
                }
            }

            cellProperties = {
                renderer: render,
                format: formato,
                type: tipo,
                readOnly: readOnly
            }

            return cellProperties;
        },
        afterLoadData: function () {
            this.render();
        }
    };

    hot2 = new Handsontable(container2, hotOptions2);
    //calculateSize2();
    hot2.render();
    //if (validar == true) {
    //validarRegistros(evtHot);
    //}
}
function calculateSizeDetalle() {
    var offset;
    offset = Handsontable.Dom.offset(container2);

    if (offset.top == 222) {
        availableHeight = $(window).height() - offset.top - 90;
    }
    else {
        availableHeight = $(window).height() - offset.top - 20;
    }

    availableWidth = $(window).width() - 2 * offset.left;
    container2.style.height = availableHeight + 'px';
    hot2.render();
}

function validarRegistrosDetalle(data) {

    var totalFilas = data.Handson.ListaExcelData.length;
    for (var row = 0; row < data.Handson.ListaExcelData.length; row++) {
        for (var col = 0; col < data.Handson.ListaExcelData[0].length; col++) {
            if ((row > data.FilasCabecera - 1) && (col <= data.ColumnasCabecera) && (!data.Handson.ReadOnly)) {
                if (!data.Handson.ListaFilaReadOnly[row]) {
                    var value = data.Handson.ListaExcelData[row][col];
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();

                    if (col == 0) {
                        if (value == null || value == "") {
                            agregarErrorDetalle(celda, value, 0);
                        } else {

                            if (row == 1) {
                                
                                var fechahoraInicioLista = formatearFecha(value);
                                var fechahoraInicioFormateada = formatearFecha($('#fechaInicioDetalle').val());

                                if (fechahoraInicioFormateada < fechahoraInicioLista) {
                                    agregarErrorDetalle(celda, value, 4);
                                }

                            }
                            if (row == (totalFilas - 1)) {
                                //console.log(value);
                                var fechahoraFinLista = formatearFecha(value);
                                var fechahoraFinFormateada = formatearFecha($('#fechaFinDetalle').val());

                                if (fechahoraFinFormateada > fechahoraFinLista) {
                                    agregarErrorDetalle(celda, value, 5);
                                }
                            }
                        }
                    }
                    if (col == 1) {
                        if (!validarDecimal(value)) {
                            agregarErrorDetalle(celda, value, 1);
                        } else {
                            if (Number(value) < 0) {
                                agregarErrorDetalle(celda, value, 2);
                            }
                        }
                    }

                }                
            }
        }
    }
}

function mostrarErroresDetalle(activar) {
    totCampoVacio2 = 0;
    totNoNumero2 = 0;
    totValorNegatico2 = 0;
    validaInicial2 = true;
    listErrores2.splice(0, listErrores2.length);

    validarRegistrosDetalle(evtHot2);
    if (activar) {
        activarPopupErroresDetalle();
    }
}
function agregarErrorDetalle(celda, valor, tipo) {
    if (validarErrorDetalle(celda)) {
        var regError = {
            Celda: celda,
            Valor: valor,
            Tipo: tipo
        };

        listErrores2.push(regError);
        switch (tipo) {
            case 0:
                totCampoVacio++;
                break;
            case 1:
                totNoNumero++;
                break;
            case 2:
                totValorNegatico++;
                break;
        }
    }
}


function validarErrorDetalle(celda) {
    for (var j in listErrores2) {
        if (listErrores2[j]['Celda'] == celda) {
            return false;
        }
    }
    return true;
}

function activarPopupErroresDetalle() {
    setTimeout(function () {
        $('#idTerroresDetalle').html(dibujarTablaErrorDetalle());

        $('#validacionesDetalle').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
        $('#tablaErrorDetalle').dataTable({
            "scrollY": 330,
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });

    }, 200);
}

function dibujarTablaErrorDetalle() {
    var totalErrores = listErrores2.length;
    var valorLimite = 50;
    var totalErroresMostrar = totalErrores > valorLimite ? valorLimite : totalErrores;
    var cadena = "";
    if (totalErrores > valorLimite) {
        cadena += "<div>Se encontraron " + totalErrores + " errores</div>";
    }
    cadena += "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablaErrorDetalle' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Celda</th><th>Valor</th><th>Error</th></tr></thead>";
    cadena += "<tbody>";
    for (var i = 0; i < totalErroresMostrar; i++) {
        cadena += "<tr><td>" + listErrores2[i].Celda + "</td>";

        if (listErrores2[i].Tipo >= 0) {
            cadena += "<td>" + listErrores2[i].Valor + "</td>";
            switch (listErrores2[i].Tipo) {
                case 4:
                    cadena += "<td>" + listDescripErrores2[listErrores2[i].Tipo] + $('#fechaInicioDetalle').val() + "</td></tr>";
                    break;
                case 5:
                    cadena += "<td>" + listDescripErrores2[listErrores2[i].Tipo] + $('#fechaFinDetalle').val() +"</td></tr>";
                    break;
               default:
                    cadena += "<td>" + listDescripErrores2[listErrores2[i].Tipo] + "</td></tr>";
                    break;
            }
            
        } else {
            cadena += "<td>" + "</td>";
            cadena += "<td>" + listErrores2[i].Valor + "</td></tr>";
        }

    }
    cadena += "</tbody></table>";
    return cadena;
}

function guardarDatosArchivosMagnitudDetalle() {

    mostrarErroresDetalle(false);

    var cliente = $("#cbClientes").val();
    var cuadro = $("#cuadro").val();    

    if (cliente == '' || cliente == '0') {
        mostrarAlerta('Debe seleccionar un cliente');
        return false;
    }

    if (cuadro == '' || cuadro == '0') {
        mostrarAlerta('Debe seleccionar un cuadro de programa');
        return false;
    }

    setTimeout(function () {
        if (listErrores2.length > 0) {
            activarPopupErroresDetalle();
        } else {
            if (confirm("¿Desea enviar información a COES?")) {
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    url: controlador + "GrabarDatosArchivosMagnitudDetalle",
                    data: {
                        datos: JSON.stringify(hot2.getData()),                       
                        codigoCuadroPrograma: cuadro,
                        clienteId: cliente
                    },
                    beforeSend: function () {
                        mostrarAlerta("Enviando Información ..");
                    },
                    success: function (respuesta) {
                        if (respuesta.Exito) {
                            mostrarMensaje("Información grabada correctamente.");
                            setTimeout(function () {
                                obtenerClienteDetalle();
                            }, 2000);
                           
                        } else {                           
                            mostrarMensaje("Hubieron errores, revisar.");                           
                        }
                    },
                    error: function () {
                        mostrarError("Ocurrió un error");
                    }
                });
            }
        }
    }, 100);
}

function descargarFormatoDetalle() {

    var cliente = $("#cbClientes").val();
    var cuadro = $("#cuadro").val();

    if (cliente == '' || cliente == '0') {
        mostrarAlerta('Debe seleccionar un cliente');
        return false;
    }

    if (cuadro == '' || cuadro == '0') {
        mostrarAlerta('Debe seleccionar un cuadro de programa');
        return false;
    }

    if (hot2 == undefined) {
        return false;
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarFormatoDetalle',
        dataType: 'json',
        data: {           
            datos: JSON.stringify(hot2.getData())
        },
        success: function (result) {
            if (result == "1") {
                window.location = controlador + 'DescargarFormatoDetalle';
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

function crearPuploadDetalle() {
    uploader2 = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectExcelDetalle',
        url: siteRoot + 'RechazoCarga/EnvioArchivosMagnitud/SubirDetalle',
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
                if (uploader2.files.length == 2) {
                    uploader2.removeFile(uploader2.files[0]);
                }
                uploader2.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                mostrarAlerta("Subida completada <strong>Por favor espere</strong>");
                leerExcelSubidoDetalle();
                limpiarErrorDetalle();
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploader2.init();
}

function leerExcelSubidoDetalle() {
    $.ajax({
        type: 'POST',
        url: controlador + 'LeerExcelSubidoDetalle',
        dataType: 'json',
        async: false,
        data: {
        },
        success: function (respuesta) {
            if (respuesta.Exito == false) {
                mostrarError(respuesta.Mensaje);
            } else {
                if (typeof hot2 != 'undefined') {
                    hot2.destroy();
                }
                crearHandsonTable2(respuesta.Datos, true);
                evtHot2 = respuesta.Datos;
                mostrarExito("Archivo importado");
            }
        },
        error: function () {
            mostrarError("Ha ocurrido un error");
        }
    });
}

function limpiarErrorDetalle() {
    var totCampoVacio2 = 0;
    var totNoNumero2 = 0;
    var totValorNegatico2 = 0;
    var listErrores2 = [];
}

function formatearFecha(cadena) {
    var partes = cadena.split("/");
    var partesAnioHoraMin = partes[2].split(" ");
    var partesHoraMin = partesAnioHoraMin[1].split(":");
    var dia = partes[0];
    var mes = parseInt(partes[1]) - 1;
    var anio = partesAnioHoraMin[0];
    var hora = partesHoraMin[0];
    var minuto = partesHoraMin[1];
    var fecha = new Date(anio, mes, dia, hora, minuto, 0, 0);
    return fecha;
};