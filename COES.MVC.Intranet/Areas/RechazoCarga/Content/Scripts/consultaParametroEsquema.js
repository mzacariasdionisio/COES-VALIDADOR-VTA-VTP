var controlador = siteRoot + "rechazocarga/ParametroEsquema/";
var uploader;
var totCampoVacio = 0;
var totNoNumero = 0;
var totValorNegatico = 0;

var listErrores = [];
var listDescripErrores = ["Campo vacío", "No es número", "Valor negativo"];

var validaInicial = true;
var hot;
var hotOptions;
var evtHot;
var tipoInstancia = [];

$(function () {   

    $('#btnConsultar').click(function () {
        obtenerParametrosEsquema();
    });

    $('#btnEnviarDatos').click(function () {
        if (validarSeleccionDatos()) {
            enviarDatos();
        }
        else {
            alert("Por favor seleccione un año");
        }
    });

    $('#btnMostrarErrores').click(function () {
        if (validarSeleccionDatos()) {
            mostrarErrores(true);
        }
        else {
            alert("Por favor seleccione un año");
        }
    });

    $("#btnDescargarFormato").click(function () {
        descargarFormato();
    });

    crearPupload();
});

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
                mostrarError("Ha ocurrido un error al leer el archivo");
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
function descargarFormato() {
    var anio = parseInt($("#anio").val());
    var tipoEmpresa = $("#empresa").val();
    if (anio == 0) {
        alert('Debe seleccionar al menos una periodo');
        return;
    } else {
        $.ajax({
            type: 'POST',
            url: controlador + 'GenerarFormato',
            dataType: 'json',
            data: {
                anio: anio,
                tipoEmpresa: tipoEmpresa,
                esConsulta: true
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
}
function validarSeleccionDatos() {
    if ($('#anio').val() == "") {
        return false;
    }
    return true;
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
    if (listErrores.length > 0) {
        activarPopupErrores();
        return;
    }

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

function obtenerParametrosEsquema() {
    if (typeof hot != 'undefined') {
        hot.destroy();
    }

    obtenerModelo(false);
}

function obtenerModelo(validar) {
    var anio = parseInt($("#anio").val());
    var tipoEmpresa = $("#empresa").val();

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerFormatoModelParametrosEsquema",
        dataType: 'json',
        data: {
            anio: anio,
            tipoEmpresa: tipoEmpresa,
        },
        success: function (evt) {

            tipoInstancia = [];

            for (var j in evt.ListaTipoInstancia) {
                tipoInstancia.push({ id: evt.ListaTipoInstancia[j].IipoInstanciaId, text: evt.ListaTipoInstancia[j].TipoInstanciaTexto });                
            }

            crearHandsonTable(evt.FormatoHandsonTable, validar);
            evtHot = evt.FormatoHandsonTable;
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
        td.style.background = 'blue';
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
        if (parseFloat(value))
            $(td).html(formatFloat(parseFloat(value), 3, '.', ','));
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
        fixedRowsTop: evtHot.FilasCabecera + 1,
        //manualColumnResize: true,
        //fixedColumnsLeft: evtHot.ColumnasCabecera,
        mergeCells: evtHot.Handson.ListaMerge,
        colWidths: evtHot.Handson.ListaColWidth,
        afterLoadData: function () {
            this.render();
        },
        hiddenColumns: {
            columns: [12, 13, 14],
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
                if (col == 3) {
                    render = tituloAzul
                }
                if (col == 7) {
                    render = tituloAzul
                }
                if (col == 9) {
                    render = tituloAzul
                }
            }
            if (row == 1) {
                if (col == 3) {
                    render = tituloVerdeClaro
                }
                if (col == 5) {
                    render = tituloVerdeOscuro
                }
                if (col == 7) {
                    render = tituloVerdeClaro
                }
                if (col == 8) {
                    render = tituloVerdeOscuro
                }
                if (col == 9) {
                    render = tituloVerdeClaro
                }
                if (col == 10) {
                    render = tituloVerdeOscuro
                }
                readOnly = true;
            }
            if (row == 2) {
                if (col <= 2) {
                    render = tituloAzul;
                } else {
                    if (col == 3 || col == 4) {
                        render = tituloVerdeClaro
                    }
                    if (col == 5 || col == 6) {
                        render = tituloVerdeOscuro
                    }
                    if (col == 7) {
                        render = tituloVerdeClaro
                    }
                    if (col == 8) {
                        render = tituloVerdeOscuro
                    }
                    if (col == 9) {
                        render = tituloVerdeClaro
                    }
                    if (col == 10) {
                        render = tituloVerdeOscuro
                    }
                }
                readOnly = true;
            }
            if (row > 2) {
                readOnly = col <= 2;

                if (col <= 2) {
                    render = readOnlyValueRenderer;
                } else {
                    render = celdaNumerica;
                }

                if (col == 11) {
                    cellProperties.editor = 'select2';
                    cellProperties.renderer = dropdownInstanciaRenderer;
                    cellProperties.width = "220px";
                    cellProperties.select2Options = {
                        data: tipoInstancia,
                        dropdownAutoWidth: true,
                        width: 'resolve',
                        allowClear: false,
                    };

                    return cellProperties;
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
    calculateSize();

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

                    if (col == 0 || col == 11) {
                        if (value == null || value == "") {
                            //agregarError(celda, value, 0);
                        }
                    }
                    if (col >= 3 && col != 11) {                        
                        if (value != "" && value != null) {
                            //alert('*' + value + '*');
                            if (!validarDecimal(value)) {
                                agregarError(celda, value, 1);
                            } else {
                                if (Number(value) < 0) {
                                    agregarError(celda, value, 2);
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

var dropdownInstanciaRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var selectedId;    
    for (var index = 0; index < tipoInstancia.length; index++) {
        if (parseInt(value) === tipoInstancia[index].id) {
            selectedId = tipoInstancia[index].id;
            value = tipoInstancia[index].text;
        }
    }

    $(td).addClass("estilocombo");
    Handsontable.TextCell.renderer.apply(this, arguments);
    $('#selectedId').text(selectedId);

};