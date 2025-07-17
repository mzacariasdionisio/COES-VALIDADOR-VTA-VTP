var controler = siteRoot + 'ind/cargapr25/';
const imageRoot = `${siteRoot}IND/Content/Images/`;
var objHtA2, objHtCDU, objHtCRD, objHtCCD, objHtPlazo, dataPlazo;
var popupEmpresa;
let estadoValidar = false;
let estadoImportar = false;
var error = [];

$(document).ready(function () {
    setMessage('#message',
        'Por favor seleccione empresa y el periodo',
        'info',
        false);

    $('.nt-header-option').click(function () {
        $(this).addClass('active')
            .siblings()
            .removeClass('active');
        var element = this;
        if (element.id == "c1") {
            document.getElementById('cdu').style.visibility = 'visible';
            document.getElementById('crd').style.visibility = 'visible';
            document.getElementById('ccd').style.visibility = 'visible';
            document.getElementById('a2').style.visibility = 'hidden';
        } else {
            document.getElementById('cdu').style.visibility = 'hidden';
            document.getElementById('crd').style.visibility = 'hidden';
            document.getElementById('ccd').style.visibility = 'hidden';
            document.getElementById('a2').style.visibility = 'visible';
        }
    });

    $("#selEmpresa").multipleSelect({
        filter: false,
        single: true,
        placeholder: 'Seleccione...',
        onClose: function () {
            console.log("_close");
        }
    });

    $("#selPeriodo").multipleSelect({
        filter: false,
        single: true,
        placeholder: 'Mes - Año',
        onClose: function () {
            console.log("_close");
        }
    });

    $('#pericodi').change(function () {
        let p = $('#pericodi').val();
        validarPeriodo(p);
    });

    $('#btnDescargarFormato').click(function () {
        //if (estadoValidar) {
        //    descargarFormato();
        //}
        descargarFormato();
    });

    $('#btnConsultar').click(function () {
        listarCuadros();
    });

    document.getElementById('btnSelecionarExcel').addEventListener('click', openDialog);

    $('#fileId').on("change", function () {
        importarArchivo();
    })
    //$('#fileId').click(function () {
    //    importarArchivo();
    //})

    $('#btnEnviarDatos').click(function () {
        if (estadoImportar) {
            enviarDatos();
        }
    });

    document.getElementById('cdu').style.visibility = 'visible';
    document.getElementById('crd').style.visibility = 'visible';
    document.getElementById('ccd').style.visibility = 'visible';
    document.getElementById('a2').style.visibility = 'hidden';

    listarCuadros();
    validarPeriodo($('#pericodi').val());
});

function validarPeriodo(d) {
    $.ajax({
        type: 'POST',
        url: controler + 'validarperiodo',
        dataType: 'json',
        data: {
            periodo: d
        },
        success: function (result) {
            mostrarMensaje(result.dataMsg, result.typeMsg);
            if (result.typeMsg == 'msg-warning') {
                estadoValidar = false;
            } else {
                estadoValidar = true;
            }
        },
        error: function () {
            alert("Error");
        }
    });
}

function enviarDatos() {
    pop = $('#popupPlazo').bPopup({
        easing: 'easeOutBack',
        speed: 350,
        transition: 'fadeIn',
        modalClose: false
    },
        function () {
            $.ajax({
                type: 'POST',
                url: controler + 'enviardatos',
                dataType: 'json',
                data: {
                    htCDU: objHtCDU.getData(),
                    htCRD: objHtCRD.getData(),
                    htCCD: objHtCCD.getData(),
                    htA2: objHtA2.getData(),
                    idPeriodo: $('#pericodi').val(),
                    inicioCDU: $('#cduInicio').val(),
                    finCDU: $('#cduFin').val(),
                    inicioCCD: $('#ccdInicio').val(),
                    finCCD: $('#ccdFin').val(),
                    enPlazo: dataPlazo
                },
                success: function (result) {
                    //listarCuadros();
                    var ht_modelPlazo = FormatoHandson(result.plz.dataPlazo);
                    obtenerHandsonPlazo(ht_modelPlazo);
                    mostrarMensaje(result.dataMsg, result.typeMsg);
                    estadoImportar = false;
                },
                error: function () {
                    alert("Ha ocurrido un problema...");
                }
            });
        });
}

//{Contenedor, mensaje(string), tipoMensaje(string), delay(bool)}
function setMessage(container, msg, tpo, del) {
    const new_class = "msg-" + tpo;//Identifica la nueva clase css
    $(container).removeClass($(container).attr('class'));//Quita la clase css previa
    $(container).addClass(new_class);
    $(container).html(msg);//Carga el mensaje
    
    //Focus to message
    $('html, body').animate({ scrollTop: 0 }, 5);

    //Mensaje con delay o no
    if (del) $(container).show(0).delay(5000).hide(0);//5 Segundos
    else $(container).show();
}

//Seleccionar / Cambiar Empresa
seleccionarEmpresa = function () {
    $.ajax({
        type: 'POST',
        url: controler + "EscogerEmpresa",
        success: function (evt) {
            $('#popup').html(evt);
            setTimeout(function () {
                $('#popup').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}
//------------------------------------------------------------------------------------------------------------------
//Expandir contraer DIV
function btnExpandirRestaurar() {
    if ($('#hfExpandirContraer').val() == "E") {
        expandirExcel();
        //calculateSize2(1);
        $('#hfExpandirContraer').val("C");
        $('#spanExpandirContraer').text('Restaurar');

        let img = $('#imgExpandirContraer').attr('src');
        let newImg = img.replace('expandir.png', 'contraer.png');
        $('#imgExpandirContraer').attr('src', newImg);

    }
    else {
        restaurarExcel();
        //calculateSize2(2);
        $('#hfExpandirContraer').val("E");
        $('#spanExpandirContraer').text('Expandir');

        let img = $('#imgExpandirContraer').attr('src');
        let newImg = img.replace('contraer.png', 'expandir.png');
        $('#imgExpandirContraer').attr('src', newImg);

    }
}

function expandirExcel() {
    $('#mainLayout').addClass("divexcel");
    //hot.render();
}

function restaurarExcel() {
    $('#mainLayout').removeClass("divexcel");
    //hot.render();
}

function calculateSize(opcion) {
    var offset;
    offset = Handsontable.Dom.offset(container);

    if (opcion == 1) {
        availableHeight = $(window).height() - offset.top - 10;
    }
    else {
        availableHeight = $(window).height() - offset.top - 80;
    }

    availableWidth = $(window).width() - 2 * offset.left;
    container.style.height = availableHeight + 'px';
    container.style.width = availableWidth + 'px';
    //hot.render();

}

//------------------------------------------------------------------------------------------------------------------
function btnGuardarSUGAD(pericodi, emprcodi) {
    if (parseInt(error.length) > 0) {
        abrirPopupErrores();
    } else {
        console.log(hotSUGAD.getData());
        $.ajax({
            type: 'POST',
            url: controler + "guardarsugad",
            data: { pericodi: pericodi, emprcodi: emprcodi, datos: hotSUGAD.getData() },
            dataType: 'json',
            success: function (result) {
                console.log("result:", result);
                if (result == "1") {
                    mostrarExcelWeb(pericodi, emprcodi);
                    $('#popupErrores').html("");
                    alert("La información se registro correctamente...!");
                }
                else {
                    alert("Lo sentimos, se ha producido un error: <br>" + result);
                }
            },
            error: function () {
                alert("Lo sentimos, ha ocurrido un error inesperado");
            }
        });
    }
}

abrirPopupErrores = function () {
    var html = '<span class="button b-close"><span>X</span></span>';
    html += '<p style="padding: 10px;"><b>Corregir los siguientes errores</b><p>';
    html += '<table border="0" class="pretty tabla-icono" id="tabla" cellspacing="2" cellpadding="5">'
    html += '<thead>'
    html += '<tr>'
    html += '<th>Fila</th>'
    html += '<th>Día</th>'
    html += '<th>Valor</th>'
    html += '<th>Comentario</th>'
    html += '</tr>'
    html += '</thead>'
    html += '<tbody>'
    for (var i = error.length - 1; i >= 0; i--) {
        var sStyle = "background : #ffffff;";
        var sBackground = "";
        if (i % 2)
            var sStyle = "background : #fbf4bf;";
        var SplitError = error[i].split("_-_");
        var sTipError = SplitError[3].substring(0, 3);
        if (sTipError === "[1]") {
            sBackground = " background-color: #F02211;";
        }
        else if (sTipError === "[2]") {
            sBackground = " background-color: #F3F554;";
        }
        else if (sTipError === "[3]") {
            sBackground = " background-color: #ECAFF0;";
        }
        var sMsgError = SplitError[3].substring(3);
        html += '<tr id="Fila_' + i + '">'
        html += '<td style="text-align:center;' + sBackground + '">' + (parseInt(SplitError[1]) + 1) + '</td>'
        html += '<td style="text-align:center;' + sStyle + '">' + SplitError[2] + '</td>'
        html += '<td style="text-align:center;' + sStyle + '">' + SplitError[0] + '</td>'
        html += '<td style="text-align:left;' + sStyle + '">' + sMsgError + '</td>'
        html += '</tr>'
    }
    html += '</tbody>'
    html += '</table>'

    $('#popupErrores').html(html);/*
    $('#popupErrores').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });*/
    alert("Lo sentimos, la hoja del cálculo tiene errores");
}

function btnMostrarSugad(id) {
    const pericodi = document.getElementById('pericodi').value;
    $.ajax({
        type: 'POST',
        url: controler + "MostrarSugad",
        data: { pericodi: pericodi, emprcodi: id },
        success: function (evt) {
            $('#popupsugad').html(evt); 
            $('#imgHandsontable').css('display', 'block');
            setTimeout(function () {
                $('#popupsugad').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            mostrarMensajeDiv("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

mostrarExcelWeb = function (pericodi, emprcodi) {
    $.ajax({
        type: 'POST',
        url: controler + 'mostrarexcelweb',
        data: { pericodi: pericodi, emprcodi: emprcodi },
        dataType: 'json',
        success: function (result) {
            console.log("NroColumnas:",result.NroColumnas);
            configurarExcelWeb(result);
            if (result.MensajeError) {
                alert("Lo sentimos, se ha producido un error: <br>" + result.MensajeError);
            }
        },
        error: function () {
            mostrarErrorDiv('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

configurarExcelWeb = function (result) {
    $('#htSUGAD').html('');
    const container = document.getElementById('htSUGAD');
    //var limiteMax = result.LimiteMax; //Maximo Limite 
    var columns = result.Columnas;
    var widths = result.Widths;
    var data = result.Data;
    hotSUGAD = new Handsontable(container, {
        data: data,
        //maxCols: result.Columnas.length,
        colHeaders: false,
        rowHeaders: true,
        colWidths: widths,
        contextMenu: false,
        minSpareRows: 0,
        columns: columns,
        fixedRowsTop: result.FixedRowsTop,
        fixedColumnsLeft: result.FixedColumnsLeft,
        currentRowClassName: 'currentRow',
        hiddenColumns: {
            columns: [1, 2, 3, 4],
            // Ocultamos las columnas 1, 2, 3 y 4
            indicators: true
        },
        cells: function (row, col, prop) {
            //console.log("col:" + col + " row:" + row + " prop" + prop);
            var cellProperties = {};
            if (row == 0) {
                cellProperties.renderer = firstRowRendererCabecera;
            }
            else if (row == 1) {
                cellProperties.renderer = firstRowRendererCeleste;
            }
            else if (col == 0) {
                cellProperties.renderer = firstRowRendererCeleste;
            }
            else if (col > 4) {
                cellProperties.renderer = negativeValueRenderer;
            }
            return cellProperties;
        },
    });
    hotSUGAD.render();
    $('#imgHandsontable').css('display', 'none');
}

firstRowRendererCabecera = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'white';
    td.style.background = '#3D8AB8';
    td.style.fontWeight = 'bold';
    td.style.fontFamily = 'sans - serif';
    td.style.fontSize = '12px';
    cellProperties.className = "htCenter",
    cellProperties.readOnly = true;
}

firstRowRendererCeleste = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'black';
    td.style.background = '#E8F6FF';
    td.style.fontFamily = 'sans - serif';
    td.style.fontSize = '12px';
    cellProperties.readOnly = true;
}

negativeValueRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    var sHeader = $(instance.getCell(0, col)).html();
    var sColumn = $(instance.getCell(row, 0)).html();
    var sMensaje;

    if (row == 2 && col == 5) {
        //Limpiamos la lista de errores
        error = [];
    }
    if (value) {
        if (isNaN(parseInt(value, 10))) {
            //console.log('Basura ' + value); //NO ES NUMERO
            td.style.backgroundColor = '#F02211';
            td.style.color = '#FFFFFF';
            td.style.fontWeight = 'bold';
            sMensaje = "[1]Valor " + value + " en el día " + sHeader + " para [" + sColumn + "] no es válido";
        }
        else if (parseInt(value, 10) > 10000 || parseInt(value, 10) < 0) {
            //console.log('>10000 ' + value); //"Maximo" pinta amarillo
            td.style.background = '#F3F554';
            sMensaje = "[2]El valor " + value.toString() + " en el día " + sHeader + " para [" + sColumn + "] supera el Limite Max/Min permitido: 10,000/0";
        }
    }
    else if (value != "0") {
        //console.log('Vacio/Nulo ' + value);
        td.style.background = '#ECAFF0'; //lila
        sMensaje = "[3]En el día " + sHeader + " para [" + sColumn + "] tiene un valor vacio.";
    }
    if (sMensaje) {
        if (!isNaN(value)) value = "";
        error.push(value.toString().concat("_-_" + row + "_-_" + sHeader + "_-_" + sMensaje));
    }
}

mostrarExito = function (mensaje) {
    $('#message').removeClass();
    $('#message').html(mensaje);
    $('#message').addClass('msg-success');
}

mostrarErrorDiv = function (mensaje) {
    $('#message').removeClass();
    $('#message').html(mensaje);
    $('#message').addClass('msg-error');
}

mostrarAlerta = function (mensaje) {
    $('#message').removeClass();
    $('#message').html(mensaje);
    $('#message').addClass('msg-info');
}

mostrarMensajeDiv = function (mensaje) {
    $('#message').removeClass();
    $('#message').html(mensaje);
    $('#message').addClass('msg-error');
}

mostrarMensaje = function (mensaje, tipo) {
    $('#message').removeClass();
    $('#message').html(mensaje);
    $('#message').addClass(tipo);
}

function listarCuadros() {
    $.ajax({
        type: 'POST',
        url: controler + 'listarcuadros',
        dataType: 'json',
        data: {
            periodo: $('#pericodi').val()
        },
        success: function (result) {
            //Cuadro CDU de A1
            var ht_modelCDU = FormatoHandson(result.dataCDU);
            obtenerHandsonCDU(ht_modelCDU);
            //Cuadro CRD de A1
            var ht_modelCRD = FormatoHandson(result.dataCRD);
            obtenerHandsonCRD(ht_modelCRD);
            //Cuadro CCD de A1
            var ht_modelCCD = FormatoHandson(result.dataCCD);
            obtenerHandsonCCD(ht_modelCCD);
            //Cuadro A2
            var ht_modelA2 = FormatoHandson(result.dataA2);
            obtenerHandsonA2(ht_modelA2);
            //Fechas
            document.getElementById("cduInicio").value = result.fechasVigencia.inicioCDU;
            document.getElementById("cduFin").value = result.fechasVigencia.finCDU;
            document.getElementById("ccdInicio").value = result.fechasVigencia.inicioCCD;
            document.getElementById("ccdFin").value = result.fechasVigencia.finCCD;
            mostrarMensaje('Consulta exitosa...', 'msg-success');
        },
        error: function () {
            alert("Error");
        }
    });
}

function descargarFormato() {
    $.ajax({
        type: 'POST',
        url: controler + 'generarformato',
        dataType: 'json',
        data: {
            periodo: $('#pericodi').val()
        },
        success: function (result) {
            if (result == "1") {
                window.location = controler + 'descargarformato';
            }
            else {
                if (result == "5") {
                    alert("No tiene habilitado ningun formato");
                }
                else {
                    alert(result);
                }
            }
        },
        error: function () {
            alert("Error");
        }
    });
}

function openDialog() {
    if (estadoValidar) {
        var e = document.getElementById('fileId');
        e.click();
    }
    //var e = document.getElementById('fileId');
    //e.click();
}

function importarArchivo() {
    $('#htCDU').html('');
    $('#htCRD').html('');
    $('#htCCD').html('');
    $('#dtValidaciones').html("");
    const archivoSeleccionado = ($('#fileId'))[0].files[0];
    let archivoData = new FormData();
    archivoData.append('archivo', archivoSeleccionado);
    archivoData.append('periodo', $('#pericodi').val());
    $.ajax({
        type: 'POST',
        url: controler + 'ImportarExcel',
        contentType: false,
        processData: false,
        data: archivoData,
        success: function (result) {
            console.log(result, 'result');
            if (result.validator != 0) {
                mostrarMensaje(result.dataMsg, result.typeMsg);
                //listarCuadros();
            } else {
                var data = result.datos;
                var validacion = result.validaciones;
                if (data.dataA2.length > 0) {
                    //Cuadro A2
                    var ht_modelA2 = FormatoHandson(data.dataA2);
                    obtenerHandsonA2(ht_modelA2);
                } else {
                    $('#htA2').html('');
                }
                if (data.dataCDU) {
                    //Cuadro CDU de A1
                    var ht_modelCDU = FormatoHandson(data.dataCDU);
                    obtenerHandsonCDU(ht_modelCDU);
                    //Cuadro CRD de A1
                    var ht_modelCRD = FormatoHandson(data.dataCRD);
                    var ht_oldCRD = FormatoHandson(data.valPlazo);
                    var crd_val = [];
                    $.each(ht_oldCRD.data, function (i, item) {
                        var crd_array = [];
                        $.each(item, function (x, y) {
                            crd_array.push(y)
                        });
                        crd_val.push(crd_array);
                    });
                    dataPlazo = crd_val;
                    obtenerHandsonCRD(ht_modelCRD, ht_oldCRD);
                    //Cuadro CCD de A1
                    var ht_modelCCD = FormatoHandson(data.dataCCD);
                    obtenerHandsonCCD(ht_modelCCD);
                } else {
                    $('#htCDU').html('');
                    $('#htCRD').html('');
                    $('#htCCD').html('');
                }
                //Fechas
                document.getElementById("cduInicio").value = data.fechasVigencia.inicioCDU;
                document.getElementById("cduFin").value = data.fechasVigencia.finCDU;
                document.getElementById("ccdInicio").value = data.fechasVigencia.inicioCCD;
                document.getElementById("ccdFin").value = data.fechasVigencia.finCCD;

                if (validacion.length > 0) {
                    dt = $('#dtValidaciones').DataTable({
                        data: validacion,
                        columns: [
                            { data: "Cuadro", title: "CUADRO" },
                            { data: "TipoCuadro", title: "TIPO" },
                            { data: "Descripcion", title: "DESCRIPCION" },
                        ],
                        initComplete: function () {
                            $('#dtValidaciones').css('width', '100%');
                            $('.dataTables_scrollHeadInner').css('width', '100%');
                            $('.dataTables_scrollHeadInner table').css('width', '98.84%');
                        },
                        searching: false,
                        bLengthChange: false,
                        bSort: true,
                        destroy: true,
                        paging: true,
                        pageLength: 25,
                        info: false,
                    });

                    $("#popupValidaciones").bPopup({
                        modalClose: false
                    });
                    mostrarMensaje('Error al importar, se deben resolver las observaciones mostradas', 'msg-warning');
                    estadoImportar = false;
                }
                else {
                    mostrarMensaje('No se encontraron errores al cargar el archivo excel', 'msg-success');
                    estadoImportar = true;
                }
            }

            const btn = document.getElementById('fileId');
            btn.value = '';
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        },
        complete: function () {
            //pop.close();
        }
    });
}

function obtenerHandsonA2(model) {
    console.log(model, 'modelA2');
    const fechas = FirstLastDate();
    //var array = [];
    $('#htA2').html('');
    var containerA2 = document.getElementById('htA2');
    objHtA2 = new Handsontable(containerA2, {
        data: model.data,
        fillHandle: true,
        stretchH: 'all',
        height: 300,
        width: 1020,
        maxCols: model.maxCols,
        maxRows: model.maxRows,
        minSpareCols: 0,
        minSpareRows: 0,
        columns: model.columns,
        colHeaders: model.colHeaders,
        colWidths: widthColumns2(model),
        nestedHeaders: [
            [{ label: 'Capacidad Reservada Diaria adquirida/vendida <br> mediante transferencias en el Mercado secundario de gas natural', colspan: 2 },
            { label: 'Periodos de vigencia entre el' + '<br>' + fechas[0] + ' y ' + fechas[1] + '', colspan: 4 }
            ],
            ['<br> Adquirida/Venta', '<br> Fecha', 'Empresa con quien <br> se transo',
                '<br> Punto de suministro', 'Cantidad <br> adquirida/vendida <br> (MMPCD)',
                'Precio de transferencia <br> del acuerdo <br> (US$ / Mm3)'
            ]
        ],
    });
}

function obtenerHandsonCDU(model) {
    $('#htCDU').html('');
    var containerCDU = document.getElementById('htCDU');
    objHtCDU = new Handsontable(containerCDU, {
        data: model.data,
        fillHandle: true,
        stretchH: 'all',
        height: 200,
        maxCols: model.maxCols,
        maxRows: model.maxRows,
        minSpareCols: 0,
        minSpareRows: 0,
        columns: model.columns,
        colHeaders: model.colHeaders,
        colWidths: widthColumns(model),
        nestedHeaders: [
            model.colHeaders,
            model.colHeaders1
        ],
        hiddenColumns: {
            columns: [0,1,2,3],
            // show UI indicators to mark hidden columns
            indicators: false
        }
    });
}

function obtenerHandsonCRD(model, model2) {
    $('#htCRD').html('');
    var containerCRD = document.getElementById('htCRD');
    objHtCRD = new Handsontable(containerCRD, {
        data: model.data,
        fillHandle: true,
        stretchH: 'all',
        height: 300,
        maxCols: model.maxCols,
        maxRows: model.maxRows,
        minSpareCols: 0,
        minSpareRows: 0,
        columns: model.columns,
        colHeaders: model.colHeaders,
        colWidths: widthColumns(model),
        nestedHeaders: [
            model.colHeaders,
            model.colHeaders1
        ],
        hiddenColumns: {
            columns: [0,1,2,3],
            // show UI indicators to mark hidden columns
            indicators: false
        },
        cells(row, col) {
            const cellProperties = {};
            if (model2) {
                if (col > 4 && row > 3) {
                    let plazo = model2.data[row - 4]['plz' + (col - 5)];
                    if (plazo == 'N') {
                        cellProperties.renderer = renderColor; // uses function directly
                    }
                }
            }
            if (row == 3) {
                cellProperties.renderer = backColor;
            }
            return cellProperties;
        }
    });
    objHtCRD.render();
}

function obtenerHandsonCCD(model) {
    $('#htCCD').html('');
    var containerCCD = document.getElementById('htCCD');
    objHtCCD = new Handsontable(containerCCD, {
        data: model.data,
        fillHandle: true,
        stretchH: 'all',
        height: 200,
        maxCols: model.maxCols,
        maxRows: model.maxRows,
        minSpareCols: 0,
        minSpareRows: 0,
        columns: model.columns,
        colHeaders: model.colHeaders,
        colWidths: widthColumns(model),
        nestedHeaders: [
            model.colHeaders,
            model.colHeaders1
        ],
        hiddenColumns: {
            columns: [0,1,2,3],
            indicators: true
        }
    });
}

function obtenerHandsonPlazo(model) {
    $('#htPlazo').html('');
    var containerPlazo = document.getElementById('htPlazo');
    objHtPlazo = new Handsontable(containerPlazo, {
        data: model.data,
        fillHandle: true,
        stretchH: 'all',
        height: 200,
        width: 800,
        maxCols: model.maxCols,
        maxRows: model.maxRows,
        minSpareCols: 0,
        minSpareRows: 0,
        columns: model.columns,
        colHeaders: model.colHeaders,
        //colWidths: widthColumns(model),
        nestedHeaders: [
            model.colHeaders
        ],
        cells(row, col) {
            const cellProperties = {};
            let plazo = model.data[row]['pval' + col];
            if (plazo == 'Fuera Plazo') {
                cellProperties.renderer = renderColor; // uses function directly
            }
            return cellProperties;
        }
    });
    objHtPlazo.render();
}

//Formatea el modelo de datos para el Handsontable
function FormatoHandson(model) {
    var handson = {};
    var data = [], columns = [], headers = [], headers1 = [];
    var rule = ['no'];

    $.each(model, function (i, item) {
        //crea la propiedad "colHeaders"
        if (!(rule.includes(item.htrender))) {
            headers.push(item.label);
        }

        //crea la propiedad "colHeaders1"
        if (!(rule.includes(item.htrender))) {
            headers1.push(item.label2);
        }

        //Crea la propiedad "columns"
        var col = {};
        col['data'] = item.id;

        switch (item.htrender) {
            case 'hora':
                col['type'] = 'text';
                col['className'] = 'htCenter';
                col['readOnly'] = true;
                col['renderer'] = HoraColumnRenderer;
                break;
            case 'normal':
                col['type'] = 'text';
                //col['format'] = '0.00';
                col['readOnly'] = true;
                break;
            case 'patron':
                col['type'] = 'numeric';
                col['format'] = '0.00';
                col['readOnly'] = true;
                col['renderer'] = PatronColumnRenderer;
                break;
            case 'edit':
                col['type'] = 'numeric';
                col['format'] = '0.00';
                col['readOnly'] = false;
                col['allowInvalid'] = false;
                col['allowEmpty'] = false;
                break;
            case 'final':
                col['type'] = 'numeric';
                col['format'] = '0.00';
                col['readOnly'] = true;
                col['renderer'] = PatronColumnRenderer;
        }

        if (!(rule.includes(item.htrender))) {
            columns.push(col);

            //Crea la propiedad "data"
            $.each(item.data, function (j, value) {
                var row = {};
                if (data[j]) {
                    data[j][item.id] = value;
                }
                else {
                    row[item.id] = value;
                    data.push(row);
                }
            });
        }
    });

    handson['colHeaders'] = headers;
    handson['colHeaders1'] = headers1;
    handson['columns'] = columns;
    handson['data'] = data;
    handson['maxCols'] = columns.length;
    handson['maxRows'] = data.length;
    return handson;
}

function FirstLastDate() {
    var today = new Date();
    //Fecha de inicio con formato dia/mes/anio
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = today.getFullYear();
    const inicio = '01' + '/' + mm + '/' + yyyy;

    //Fecha de fin con formato dia/mes/anio
    const currentYear = today.getFullYear();
    const currentMonth = today.getMonth() + 1
    const daysInCurrentMonth = getDaysInMonth(currentYear, currentMonth);
    const fin = daysInCurrentMonth + '/' + mm + '/' + yyyy;

    var fechas = [];
    fechas.push(inicio);
    fechas.push(fin);

    return fechas;
}

function getDaysInMonth(year, month) {
    return new Date(year, month, 0).getDate();
}

function renderColor(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontWeight = 'bold';
    td.style.color = '#F0481E';
}

function backColor(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontWeight = 'bold';
    td.style.backgroundColor = '#EEEEEE';
}

function widthColumns(model) {
    var med = [];
    med.push(10);
    med.push(10);
    med.push(10);
    med.push(10);
    med.push(750);
    for (var i = 5; i < model.columns.length; i++) {
        med.push(150);
    }
    return med;
}

function widthColumns2(model) {
    var med = [];
    med.push(200);
    med.push(200);
    med.push(200);
    med.push(200);
    med.push(200);
    med.push(200);
    //for (var i = 5; i < model.columns.length; i++) {
    //    med.push(250);
    //}
    return med;
} 