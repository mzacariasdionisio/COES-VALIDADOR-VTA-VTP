var controlador = siteRoot + "rechazocarga/EnvioArchivosMagnitud/";
var uploader;
var totCampoVacio = 0;
var totNoNumero = 0;
var totValorNegatico = 0;

var listErrores = [];
var listDescripErrores = ["Campo vacío", "No es número", "Valor negativo", "Formato fecha debe ser dd/mm/aaaa hh:mm"];

var validaInicial = true;
var hot;
var hotOptions;
var evtHot;

$( /**
   * Llamadas iniciales
   * @returns {} 
   */
   $(document).ready(function () {
       //mostrarMensaje("Selecionar el periodo y la versión");
       $('#tipo').change(function () {
           obtenerProgramas();
       });

       $('#programa').change(function () {
           obtenerCuadros();
       });

       $('#btnConsultar').click(function () {
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

       crearPupload();

   }));

function crearPupload() {
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectExcel',
        url: siteRoot + 'RechazoCarga/ProgramaRechazoCarga/Subir',
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
        },
        error: function (ex) {
            mostrarError('Consultar programas: Ha ocurrido un error');
        }
    });
};

function mostrarErrores(activar) {
    totCampoVacio = 0;
    totNoNumero = 0;
    totValorNegatico = 0;
    validaInicial = true;
    listErrores.splice(0, listErrores.length);

    validarRegistros(evtHot);
    if (activar) {
        activarPopupErrores();
    }
}

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
        cadena += "<td>" + listErrores[i].Valor + "</td>";
        cadena += "<td>" + listDescripErrores[listErrores[i].Tipo] + "</td></tr>";
    }
    cadena += "</tbody></table>";
    return cadena;
}

function enviarDatos() {
    mostrarErrores(false);
    setTimeout(function () {
        if (listErrores.length > 0) {
            activarPopupErrores();
        }
    }, 100);
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

function obtenerDatosArchivosMagnitud() {
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    obtenerModelo(false);
}
function guardarDatosArchivosMagnitud() {

    //mostrarErrores(false);

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
                            mostrarMensaje("Información grabada correctamente.");
                            setTimeout(function () {
                                obtenerDatosArchivosMagnitud();
                            }, 2000);
                            //var empresas = $("#empresasSeleccionadas").val();
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
            crearHandsonTable(evt, validar);
            evtHot = evt;
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

    function calculateSize() {
        var offset;
        offset = Handsontable.Dom.offset(container);

        if (offset.top == 222) {
            availableHeight = $(window).height() - offset.top - 90;
        }
        else {
            availableHeight = $(window).height() - offset.top - 20;
        }

        availableWidth = $(window).width() - 2 * offset.left;
        container.style.height = availableHeight + 'px';
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
        height: 500,
        maxRows: evtHot.Handson.ListaExcelData.length,
        maxCols: evtHot.Handson.ListaExcelData[0].length,
        colHeaders: true,
        rowHeaders: true,
        fillHandle: true,
        columnSorting: false,
        className: "htCenter",
        readOnly: evtHot.Handson.ReadOnly,
        fixedRowsTop: evtHot.FilasCabecera,
        fixedColumnsLeft: evtHot.ColumnasCabecera,
        mergeCells: evtHot.Handson.ListaMerge,
        colWidths: evtHot.Handson.ListaColWidth,
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
                    render = tituloAzul
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
    calculateSize(1);

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
                    if (col == 5 || col == 6) {
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