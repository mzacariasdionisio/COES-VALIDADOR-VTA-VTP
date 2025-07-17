var controlador = siteRoot + 'eventos/rsf/';
var hot = null;
var limites = null;
var tableHoldFire = null;
var tableHoldFireValueOld = null;
var bkpdatos = null;
var oper = null;
var del = 0;

$(function () {

    $('#txtFecha').Zebra_DatePicker({
        onSelect: function () {
            consultar(0);
        }
    });

    $('#txtFechaInicio').Zebra_DatePicker({
    });

    $('#txtFechaFin').Zebra_DatePicker({
    });

    $('#btnGrabar').on('click', function () {
        grabar2();
    });

    $('#btnExportar').on('click', function () {
        exportar();
    });

    $('#btnExportar30').on('click', function () {
        exportarMediaHora();
    });

    $('#btnExportarRA').on('click', function () {
        $('#popupExportacion').css('display', 'block');
    });

    $('#btnAceptarExportacion').on('click', function () {
        exportarReporte();
    });

    $('#btnGrabarRA').on('click', function () {
        grabarRA();
    });

    $('#btnCargarXML').on('click', function () {
        $('#divImportar').show();
    });

    $('#btnConfiguracion').on('click', function () {
        document.location.href = controlador + 'configuracion';
    });

    $('#btnExportarXML').on('click', function () {
        generarXML();
    });

    $('#btnGrabarHora').on("click", function () {
        consultar(0);
        $('#popupHoras').bPopup().close();
    });

    $('#btnCancelarHora').on("click", function () {
        $('#popupHoras').bPopup().close();
    });

    $('#btnCerrarHora').on("click", function () {
        if (del == 1) {
            consultar(0);
        }
        $('#popupHoras').bPopup().close();
    });

    $('#btnImportarYupana').on('click', function () {
        consultar(1);
    });

    $('#btnGrafico').on('click', function () {
        graficar();
    });

    consultar(0);
});

closeImportar = function () {
    $('#divImportar').hide();
};

closePopupExportacion = function () {
    $('#popupExportacion').css('display', 'none');
};

modificarHoras = function () {

    setTimeout(function () {
        $('#popupHoras').bPopup({
            autoClose: false
        });
    }, 50);
    del = 0;
    cargarHoras();
};

cargarHoras = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'horas',
        data: {
            fecha: $('#txtFecha').val()
        },
        global: false,
        success: function (evt) {
            $('#contenidoHoras').html(evt);           

        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
};

eliminarHora = function (id) {
    if (confirm('¿Está seguro de realizar esta acción?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'eliminarhora',
            data: {
                id:id
            },
            global: false,
            dataType:'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensajeHora', 'exito', 'La operación se realizó correctamente.');
                    del = 1;
                    cargarHoras();
                }
                else {
                    mostrarMensaje('mensajeHora', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeHora', 'error', 'Se ha producido un error.');
            }
        });
    }
};

agregarHora = function (indice) {

    $('#tablaHoras> tbody').append(
        '<tr>' +
        '    <td>' +
        '        <input type="text" style="width:50px"  />' +
        '   </td>' +
        '    <td>' +
        '        <input type="text" style="width:50px" />' +
        '   </td>' +
        '   <td style="text-align:center">' +
        '       <img src="' + siteRoot + 'Content/Images/btn-ok.png" alt="" onclick="addHora($(this).parent().parent(), 0)" style="cursor:pointer" />' +
        '       <img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="" onclick="$(this).parent().parent().remove();" style="cursor:pointer" />' +       
        '   </td>' +
        '</tr>'
    );
};

addHora = function (evt, id) {
    console.log(evt);
    if (confirm('¿Está seguro de realizar esta acción?')) {
        var inicio = $(evt).find("td:eq(0) input").val();
        var fin = $(evt).find("td:eq(1) input").val();
        console.log(inicio);
        console.log(fin);
        if (validateTime(inicio) && validateTime(fin)) {
            if (validarRango(inicio, fin)) {
                mostrarMensaje('mensajeHora', 'info', 'Por favor complete los datos.');

                $.ajax({
                    type: 'POST',
                    url: controlador + 'grabarHora',
                    data: {
                        fecha: $('#txtFecha').val(),
                        inicio: inicio,
                        fin: fin,
                        id:id
                    },
                    global: false,
                    dataType: 'json',
                    success: function (result) {
                        if (result == 1) {
                            mostrarMensaje('mensajeHora', 'exito', 'Los datos se grabaron correctamente.');    
                            cargarHoras();
                        }
                        else {
                            mostrarMensaje('mensajeHora', 'error', 'Se ha producido un error.');                        
                        }
                    },
                    error: function () {
                        mostrarMensaje('mensajeHora', 'error', 'Se ha producido un error.');
                    }
                });
            }
            else {
                mostrarMensaje('mensajeHora', 'alert', 'Hora inicial debe ser menor a la final');
            }
        }
        else {
            mostrarMensaje('mensajeHora', 'alert', 'Formato incorrecto');

        }
    }
};

consultar = function (indicador) {
    if (hot != null) {
        hot.destroy();
    }
    var operacion = 0;
    if (indicador == 1) {
        operacion = $("input:radio[name='Operacion']:checked").val();
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerestructura',
        data: {
            fecha: $('#txtFecha').val(),
            indicador: indicador,
            operacion: operacion
        },
        dataType: 'json',

        success: function (resultData) {
            bkpdatos = resultData.DatosBkp;
            oper = resultData.Oper;
            cargarGrilla(resultData, operacion);
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error');
        }
    });
};

rangoValidator = function (value, callback) {
    if (value != "") {
        var arreglo = value.split("-");
        if (arreglo.length == 2) {
            var tiempo1 = arreglo[0].replace(/\s/g, "");
            var tiempo2 = arreglo[1].replace(/\s/g, "");

            if (validateTime(tiempo1) && validateTime(tiempo2)) {
                if (validarRango(tiempo1, tiempo2)) {
                    limpiarMensaje();
                    callback(true);
                }
                else {
                    mostrarMensaje('mensaje', 'alert', 'Hora inicial debe ser menor a la final');
                    callback(false);
                }
            }
            else {
                mostrarMensaje('mensaje', 'alert', 'Formato incorrecto');
                callback(false);
            }
        }
        else {
            mostrarMensaje('mensaje', 'alert', 'Formato incorrecto');
            callback(false);
        }
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Ingrese valor');
        callback(false);
    }
};


cargarGrilla = function (result2, operacion) { 
    limites = result2.ListaLimite;    
   
    var container = document.getElementById('contenedor');
    var data = result2.Datos;
    
    var indiceAdicional = result2.CountAdicional;
    var countGrupos = result2.Columnas;
    var obtenerTotal = function (col, instance, colope) {
        var suma = 0;
        for (var row = 2; row <= result2.Longitud + 1; row++) {

            if (result2.Indices.indexOf(row) == -1 ) {
                var valor = instance.getDataAtCell(row, col);
                var ope = instance.getDataAtCell(row, col - colope);
                var indope = 0;

                if (ope != "" && ope != null) {
                    indope = parseInt(ope);
                }

                if (valor != null) {
                    if (valor != "") {
                        suma = suma + parseFloat(valor) * indope;
                    }
                }
            }
            
            
        }
        return suma;
    };

    calculateSize = function () {
        var offset;
        offset = Handsontable.Dom.offset(container);
        availableHeight = $(window).height() - offset.top - 10;
        availableWidth = $(window).width() - 2 * offset.left;
        container.style.height = availableHeight + 'px';
        container.style.width = availableWidth + 'px';
        hot.render();
    };

    var tituloRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '11px';
        td.style.textAlign = 'center';
        td.style.color = '#fff';
        td.style.backgroundColor = '#4C97C3';
    };

    var tituloRendererAdicional = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '11px';
        td.style.textAlign = 'center';
        td.style.color = '#fff';
        td.style.backgroundColor = '#FF9900';
    };

    var subTituloRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#D7EFEF';
        td.style.textAlign = 'left';
        td.style.fontWeight = 'normal';
        td.style.color = '#1C91AE';
        td.style.fontSize = '11px';
        td.style.verticalAlign = 'middle';
    };

    var totalRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.NumericRenderer.apply(this, arguments);
        td.style.backgroundColor = '#70AD47';
        td.style.textAlign = 'right';
        td.style.fontWeight = 'bold';
        td.style.color = '#ffffff';
        td.style.fontSize = '11px';
        td.style.verticalAlign = 'middle';
    };

    var totalSumRenderer1 = function (instance, td, row, col, prop, value, cellProperties) {

        td.style.backgroundColor = '#70AD47';
        td.style.textAlign = 'right';
        td.style.fontWeight = 'bold';
        td.style.color = '#ffffff';
        td.style.fontSize = '11px';
        td.style.verticalAlign = 'middle';
        var valor = obtenerTotal(col, instance, 1);
        
        if ($.isNumeric(valor)) {
            value = valor;
            //- Hacer cambio para pintar datos
            var comparacion = parseFloat($('#hfRaUp').val());
            if (parseFloat(valor) != comparacion) {
                td.style.backgroundColor = 'red';
            }
        }
        Handsontable.renderers.NumericRenderer.apply(this, arguments);
    };

    var totalSumRenderer2 = function (instance, td, row, col, prop, value, cellProperties) {

        td.style.backgroundColor = '#70AD47';
        td.style.textAlign = 'right';
        td.style.fontWeight = 'bold';
        td.style.color = '#ffffff';
        td.style.fontSize = '11px';
        td.style.verticalAlign = 'middle';
        var valor = obtenerTotal(col, instance, 2);
       
        if ($.isNumeric(valor)) {
            value = valor;
            //- Hacer cambio para pintar datos
            var comparacion = parseFloat($('#hfRaDown').val());
            if (parseFloat(valor) != comparacion) {
                td.style.backgroundColor = 'red';
            }
        }
        Handsontable.renderers.NumericRenderer.apply(this, arguments);
    };

    indisponibilidadRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        cellProperties.type = 'dropdown';
        cellProperties.source = ['0', '1'];
        Handsontable.renderers.TextRenderer.apply(this, arguments);
    };

    var comentarioRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.textAlign = 'right';
        td.style.fontWeight = 'bold';
        td.style.height = "40px";
        td.style.color = '#AD6500';
        td.style.backgroundColor = '#FFEB9C';
        td.style.fontSize = '11px';
        td.style.verticalAlign = 'middle';
    };

    var textoRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.height = "40px";
        td.style.backgroundColor = '#EBEBEB';
        td.style.verticalAlign = 'middle';
    };

    var disbledRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#FFDBA4';
        td.style.fontSize = '11px';
        td.style.verticalAlign = 'middle';
    };

    var disbledDerechaRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.NumericRenderer.apply(this, arguments);
        td.style.backgroundColor = '#FFDBA4';
        td.style.fontSize = '11px';
        td.style.verticalAlign = 'middle';
    };

   

    if ((operacion == 1 || operacion == 3)) {

        if (indiceAdicional > 0) {

            mostrarMensaje('mensaje', 'alert', 'Se han actualizado las columnas con cabecera de color naranja. Por favor revise antes de grabar');
        }
    }

    var merges = [
        { row: 0, col: 0, rowspan: 1, colspan: 5 },
        { row: result2.Longitud + 2, col: 0, rowspan: 1, colspan: 5 },
        { row: result2.Longitud + 3, col: 0, rowspan: 1, colspan: 5 }
    ];

    var widths = [75, 1, 160, 140, 110];

    for (var i = 0; i <= countGrupos; i++) {
        var columna = 5 + i * 3;
        merges.push({ row: 0, col: columna, rowspan: 1, colspan: 3 });
        //merges.push({ row: result.Longitud + 2, col: columna, rowspan: 1, colspan: 3 });
        merges.push({ row: result2.Longitud + 3, col: columna, rowspan: 1, colspan: 3 });

        widths.push(35);
        widths.push(60);
        widths.push(60);
    }   

    hot = new Handsontable(container, {
        data: data,
        rowHeaders: false,
        colHeaders: false,
        comments: true,
        height: 600,
        fixedRowsTop: 2,
        fixedColumnsLeft: 5,
        maxRows: result2.Longitud + 4,
        colWidths: widths,
        afterChange: onChange,
        cells: function (row, col, prop) {
            var cellProperties = {};

            //- Headers
            if (row == 0 || row == 1) {
                cellProperties.renderer = tituloRenderer;  
                cellProperties.readOnly = true;

                if ((operacion == 1 || operacion == 3)) {

                    if (indiceAdicional > 0) {

                        if (col > indiceAdicional) {
                            cellProperties.renderer = tituloRendererAdicional;
                        }
                    }
                }
            }

            if ((row == 0 ) && col > 4) {
                cellProperties.validator = rangoValidator;
                //cellProperties.comment = "hh:mm - hh:mm";
            }

            //- Filas normales
            if (row > 1 && row <= result2.Longitud + 1 && col < 5) {
                cellProperties.renderer = subTituloRenderer;
            }
          
            if (col < 5) {
                cellProperties.readOnly = true;
            }
            
            if (row > 1 && row <= result2.Longitud + 2 && col >= 5) {


                //- Aquí debemos hacer la jugada para los totales

                for (var i = 0; i <= countGrupos; i++) {

                    if (col == 5 + i * 3) {
                        cellProperties.type = 'dropdown';
                        cellProperties.source = ['0', '1'];
                        //cellProperties.renderer = indisponibilidadRenderer;


                    }
                    if (col == 6 + i * 3 || col == 7 + i * 3) {
                        cellProperties.format = '0,0.000';
                        cellProperties.type = 'numeric';
                    }

                }
            }

            //- centrales

            if (result2.Indices.indexOf(row) != -1 && col <= 4) {
                cellProperties.renderer = disbledRenderer;
                cellProperties.readOnly = true;
            }

            if (result2.Indices.indexOf(row) != -1 && col > 4) {
                cellProperties.renderer = disbledDerechaRenderer;
                cellProperties.readOnly = true;
            }

            //- ultimas dos filas
            if (row == result2.Longitud + 2 && col <= 4) {
                cellProperties.readOnly = true;
                cellProperties.renderer = totalRenderer;
            }                     

            //- Aquí debemos hacer la jugada para los totales
            if (row == result2.Longitud + 2 && col > 4) {

                for (var i = 0; i <= countGrupos; i++) {

                    if (col == 5 + i * 3) {
                        cellProperties.readOnly = true;
                        //cellProperties.renderer = totalSumRenderer;
                    }
                    if (col == 6 + i * 3) {
                        cellProperties.readOnly = true;
                        cellProperties.renderer = totalSumRenderer1;

                        
                    }
                    if (col == 7 + i * 3) {
                        cellProperties.readOnly = true;
                        cellProperties.renderer = totalSumRenderer2;
                    }
                }
            }

            if (row == result2.Longitud + 3 && col == 0) {
                cellProperties.renderer = comentarioRenderer;
            }
            if (row == result2.Longitud + 3 && col > 4) {
                cellProperties.renderer = textoRenderer;
            }

            return cellProperties;
        },
        mergeCells: merges
    });

    tableHoldFire = hot;

    /*
    document.querySelectorAll(".htNumeric").forEach(function (e) {
        //if (e.innerHTML == 44.000) console.log(e.innerHTML)
        e.addEventListener("dblclick", function (e) {
            console.log(this.innerHTML);
        });
    });*/
    var tableHoldFireValueOldDblclick = false;
    $(document).on("dblclick", ".htAutocomplete", function () {
        tableHoldFireValueOld = parseFloat($(this).text());
        tableHoldFireValueOldDblclick = true;
        console.log("doble click: " + tableHoldFireValueOld);
        console.log($(this).text());
    });

    $(document).on("mousedown", ".htAutocompleteArrow", function () {
        tableHoldFireValueOldDblclick = true;
    });

    //htAutocompleteArrow


    $("#btnPrueba").click(function () {
        console.log("Handler for .click() called.");
        let row = 3;
        let column = 5;//+7;
        let value = 0;
        let source = 'edit';
        tableHoldFire.setDataAtCell(row, column, value, source);
    });



    /*
    var dataRowSimulateStartA = 6;
    var dataRowSimulateStartB = 7;
    for (let i = 0; i < 48; i++) {
        let celdaValorA = hot.getDataAtCell(68, dataRowSimulateStartA);
        let celdaValorB = hot.getDataAtCell(68, dataRowSimulateStartB);
        //hot.setDataAtCell(71, dataRowSimulateStartA, celdaValorA);
        //hot.setDataAtCell(71, dataRowSimulateStartB, celdaValorB);
        hot.setDataAtCell(72, dataRowSimulateStartA, celdaValorA);
        hot.setDataAtCell(72, dataRowSimulateStartB, celdaValorB);
        dataRowSimulateStartA = dataRowSimulateStartA + 3;
        dataRowSimulateStartB = dataRowSimulateStartB + 3;
        console.log("Calculando Fenix");
    }*/


    function onChange(changes, source) {
        if (!changes) {
            return;
        }




        let contadorCambiosBool = true;
        let contadorCambios = 0;

        var listHorariosPosColumnValueInitial = 5;
        var listHorariosPosColumn = [];
        for (i = 0; i < 48; i++) {
            listHorariosPosColumn.push(listHorariosPosColumnValueInitial);
            listHorariosPosColumnValueInitial = listHorariosPosColumnValueInitial + 3;
        }

        changes.forEach(function (change) {
            try {
                if (contadorCambiosBool && tableHoldFireValueOldDblclick && $('#checkboxActualizacionAutomaticaDerecha').is(':checked')) {
                    //console.log("iniciando");
                    /*$('#loading').bPopup({
                        fadeSpeed: 'fast',
                        opacity: 0.4,
                        followSpeed: 500,
                        modalColor: '#000000',
                        modalClose: false
                    });*/
                    tableHoldFireValueOldDblclick = false;
                    let pRow = change[0];
                    let pCol = change[1];
                    let pOldValue = parseFloat(change[2]).toFixed(2);//change[2];
                    let pNewValue = change[3];
                    //tableHoldFireValueOld == pOldValue && 
                    if (listHorariosPosColumn.includes(pCol)) {
                        //console.log("procesando...");
                        /*console.log("aaaaaa__" + contadorCambiosBool);
                        console.log("bbbbbb__" + contadorCambios);
                        console.log(changes);
                        console.log(source);*/

                        let listHorariosChangePosColumnValueInitial = pCol + 3;
                        for (i = 0; i < 48; i++) {
                            //console.log(i + "-" + listHorariosChangePosColumnValueInitial);
                            if (listHorariosPosColumn.includes(listHorariosChangePosColumnValueInitial)) {
                                tableHoldFire.setDataAtCell(pRow, listHorariosChangePosColumnValueInitial, pNewValue, 'edit');
                                listHorariosChangePosColumnValueInitial = listHorariosChangePosColumnValueInitial + 3;

                            }
                        }
                    }
                    //setTimeout(function () { $('#loading').bPopup().close(); }, 2000);

                }
            } catch{
                //setTimeout(function () { $('#loading').bPopup().close(); }, 2000);
            }
            contadorCambios++;
            contadorCambiosBool = false;
        });

        var instance = this;
        changes.forEach(function (change) {
            var row = change[0];
            var col = change[1];
            var newValue = change[3];
            var elemento = limites[row - 2];

            if ((col - 5) % 3 == 0) {
                try {

                    if (elemento.Tipo == 4 || elemento.Tipo == 5) {

                        var rsfup = hot.getDataAtCell(elemento.Indice, col + 1);
                        var rsfdown = hot.getDataAtCell(elemento.Indice, col + 2);

                        if (rsfup != "" && rsfup != null && rsfdown != "" && rsfdown != null) {


                            // Division de totales dependiendo del numero de unidades operativas
                            if (elemento.Tipo == 4) {
                                var contador = 0;
                                for (var i = elemento.Indice + 1; i <= elemento.Indice + elemento.Contador - 1; i++) {
                                    var dato = "";
                                    if (row != i) {
                                        dato = hot.getDataAtCell(i, col);
                                    }
                                    else {
                                        dato = newValue;
                                    }
                                    if (dato == "1") {
                                        contador++;
                                    }
                                }

                                if (contador > 0) {
                                    var rsfupitem = parseFloat(rsfup) / contador;
                                    var rsfdownitem = parseFloat(rsfdown) / contador;

                                    for (var j = elemento.Indice + 1; j <= elemento.Indice + elemento.Contador - 1; j++) {

                                        var dato1 = "";
                                        if (row != j) {
                                            dato1 = hot.getDataAtCell(j, col);
                                        }
                                        else {
                                            dato1 = newValue;
                                        }
                                        if (dato1 == "1") {
                                            hot.setDataAtCell(j, col + 1, rsfupitem);
                                            hot.setDataAtCell(j, col + 2, rsfdownitem);
                                        }
                                        else {
                                            hot.setDataAtCell(j, col + 1, 0);
                                            hot.setDataAtCell(j, col + 2, 0);
                                        }
                                    }
                                }
                            }
                            if (elemento.Tipo == 5) {
                                if (parseFloat(rsfup) + parseFloat(rsfdown)) {
                                    var acumuladoAnterior = 0;

                                    //- Division tomando en cuenta el limite mínimo y máximo
                                    for (var k = elemento.Indice + 1; k <= elemento.Indice + elemento.Contador - 1; k++) {

                                        var operativo = "";
                                        var acumulado = 0;
                                        if (row != k) {
                                            operativo = hot.getDataAtCell(k, col);
                                        }
                                        else {
                                            operativo = newValue;
                                        }
                                        var diferencia = parseInt(operativo) * (limites[k - 2].LimiteMax - limites[k - 2].LimiteMin);

                                        if (k == elemento.Indice + 1) {
                                            acumulado = parseFloat(rsfup) + parseFloat(rsfdown);

                                            if (acumulado - diferencia < 0) {
                                                acumuladoAnterior = 0;
                                            }
                                            else {
                                                acumuladoAnterior = acumulado - diferencia;
                                            }
                                        }
                                        else {
                                            acumulado = acumuladoAnterior;

                                            if (acumulado - diferencia < 0) {
                                                acumuladoAnterior = 0;
                                            }
                                            else {
                                                acumuladoAnterior = acumulado - diferencia;
                                            }
                                        }

                                        var nuevoRsfUp = 0;
                                        var nuevoRsfDown = 0;

                                        if (diferencia < acumulado) {
                                            nuevoRsfUp = parseInt(operativo) * diferencia * parseFloat(rsfup) / (parseFloat(rsfup) + parseFloat(rsfdown));
                                            nuevoRsfDown = parseInt(operativo) * diferencia * parseFloat(rsfdown) / (parseFloat(rsfup) + parseFloat(rsfdown));
                                        }
                                        else {
                                            nuevoRsfUp = parseInt(operativo) * acumulado * parseFloat(rsfup) / (parseFloat(rsfup) + parseFloat(rsfdown));
                                            nuevoRsfDown = parseInt(operativo) * acumulado * parseFloat(rsfdown) / (parseFloat(rsfup) + parseFloat(rsfdown));
                                        }

                                        //- Cambio Movisoft 20012021
                                        if (rsfup + rsfdown == 0) {
                                            nuevoRsfUp = 0;
                                            nuevoRsfDown = 0;
                                        }
                                        //- Fin cambio Movisoft 20012021

                                        //console.log("Nuevo UP: " + nuevoRsfUp);
                                        //console.log("Nuevo DOWN: " + nuevoRsfDown);
                                        hot.setDataAtCell(k, col + 1, nuevoRsfUp);
                                        hot.setDataAtCell(k, col + 2, nuevoRsfDown);
                                    }
                                }
                            }
                        }
                    }        
                } catch (e) {

                }        
            }
        });
    }

    closeImportar();
};

procesarArchivo = function () {
    consultar(1);
};

grabar = function () {
    var datos = hot.getData(0, 0, hot.countRows() - 1, hot.countCols() - 1);   
    var validacion = validarDatos(datos);

    if (validacion == "") {
        $.ajax({
            type: "POST",
            url: controlador + 'grabar',
            dataType: "json",
            contentType: 'application/json',
            traditional: true,
            data: JSON.stringify({
                fecha: $('#txtFecha').val(),
                datos: datos
            }),
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'Los datos se grabaron correctamente.');
                    consultar(0);
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', validacion);
    }
};

grabar2 = function () {
    var datos = hot.getData(0, 0, hot.countRows() - 1, hot.countCols() - 1);
    var validacion = validarDatos(datos);

    if (validacion == "") {
        $.ajax({
            type: "POST",
            url: controlador + 'grabar',
            dataType: "json",
            contentType: 'application/json',
            traditional: true,
            data: JSON.stringify({
                fecha: $('#txtFecha').val(),
                datos: datos,
                bkp: bkpdatos,
                op: oper,
            }),
            success: function (result) {
                if (result > -1) {
                    mostrarMensaje('mensaje', 'exito', 'Los datos se grabaron correctamente. Se modificaron ' + result + ' datos.');
                    consultar(0);
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', validacion);
    }
};

validarDatos = function (data) {
    var mensaje = "<ul>";
    var validacionHeader = true;
    var validacionColumnas = true;
    var validacionFormato = true;
    for (var i = 5; i < data[0].length; i++) {
        //if (data[0][i] == null || data[0][i] == "") {
        //    validacionHeader = false;
        //}
        var validacionItem = false;
        for (var j = 2; j < data.length - 2; j++) {
            if (data[j][i] != null && data[j][i] != "") {
                validacionItem = true;

                if (!$.isNumeric(data[j][i])) {
                    validacionFormato = false;
                }
            }
        }
        validacionColumnas = validacionColumnas && validacionItem;
    }

    if (!validacionHeader) {
        mensaje = mensaje + "<li>Ingrese todos los rangos de horas.</li>";
    }
    if (!validacionColumnas) {
        mensaje = mensaje + "<li>Ingrese al menos un valor correcto de cada columna.</li>";
    }
    if (!validacionFormato) {
        mensaje = mensaje + "<li>Ingrese valores correctos.</li>";
    }

    mensaje = mensaje + "</ul>";

    if (validacionHeader && validacionColumnas && validacionFormato) {
        mensaje = "";
    }
    return mensaje;
}

exportar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'generar',
        data: {
            fecha: $('#txtFecha').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                document.location.href = controlador + "exportar";
            }
            if (result == -1) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
};

generarXML = function () {
    document.location.href = controlador + 'generarxml?fecha=' + $('#txtFecha').val();
};

graficar = function () {
    document.location.href = controlador + 'graficar?fecha=' + $('#txtFecha').val();
};

exportarMediaHora = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'generarmediahora',
        data: { fecha: $('#txtFecha').val() },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                document.location.href = controlador + "exportarmediahora";
            }
            if (result == -1) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

exportarReporte = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'generarreservaasignada',
        data: {
            fechaInicio: $('#txtFechaInicio').val(),
            fechaFin: $('#txtFechaFin').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                document.location.href = controlador + "exportarreservaasignada";
            }
            if (result == -1) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

modificarRA = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'editarra',
        global: false,
        dataType: 'json',
        success: function (result) {

            $('#txtValorUp').val(result.ValorRaUp);
            $('#txtValorDown').val(result.ValorRaDown);
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    autoClose: false
                });
            }, 50);

        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

grabarRA = function () {

    if ($('#txtValorUp').val() != "" && $('#txtValorDown').val() != "") {
        if ($.isNumeric($('#txtValorUp').val()) && $.isNumeric($('#txtValorDown').val())) {
            $.ajax({
                type: 'POST',
                url: controlador + 'grabarra',
                data: {
                    raUp: $('#txtValorUp').val(),
                    raDown: $('#txtValorDown').val()
                },
                dataType: 'json',
                success: function (result) {
                    if (result == 1) {
                        $('#popupEdicion').bPopup().close();
                        $('#spanRaUp').text($('#txtValorUp').val());
                        $('#spanRaDown').text($('#txtValorDown').val());
                        $('#hfRaUp').val($('#txtValorUp').val());
                        $('#hfRaDown').val($('#txtValorDown').val());
                        consultar(0);
                        mostrarMensaje('mensaje', 'exito', 'El valor valor fué cambiado correctamente.');
                    }
                    else {
                        mostrarMensaje('mensajePopup', 'error', 'Ha ocurrido un error.');
                    }
                },
                error: function () {
                    mostrarMensaje('mensajePopup', 'error', 'Ha ocurrido un error.');
                }
            });
        }
        else {
            mostrarMensaje('mensajePopup', 'alert', 'Ingrese un valor numérico');
        }
    }
    else {
        mostrarMensaje('mensajePopup', 'alert', 'Ingrese el valor de la Reserva Asignada');
    }
}

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};

limpiarMensaje = function () {
    $('#mensaje').removeClass();
    $('#mensaje').html("Completa los datos");
    $('#mensaje').addClass('action-message');
};

validateTime = function (inputStr) {
    if (!inputStr || inputStr.length < 1) { return false; }
    var time = inputStr.split(':');
    return (time.length === 2
        && parseInt(time[0], 10) >= 0
        && parseInt(time[0], 10) <= 23
        && parseInt(time[1], 10) >= 0
        && parseInt(time[1], 10) <= 59) ||
        (time.length === 3
            && parseInt(time[0], 10) >= 0
            && parseInt(time[0], 10) <= 23
            && parseInt(time[1], 10) >= 0
            && parseInt(time[1], 10) <= 59
            && parseInt(time[2], 10) >= 0
            && parseInt(time[2], 10) <= 59)
}

validarRango = function (startTime, endTime) {
    var regExp = /(\d{1,2})\:(\d{1,2})/;
    if (parseInt(endTime.replace(regExp, "$1$2")) > parseInt(startTime.replace(regExp, "$1$2"))) {
        return true;
    }
    return false;
};

function loadInfoFile(fileName, fileSize) {
    $('#fileInfo').html(fileName + " (" + fileSize + ")");
    $('#fileInfo').removeClass('action-alert');
    $('#fileInfo').addClass('action-exito');
    $('#fileInfo').css('margin-bottom', '10px');
}

function loadValidacionFile(mensaje) {
    $('#fileInfo').html(mensaje);
    $('#fileInfo').removeClass('action-exito');
    $('#fileInfo').addClass('action-alert');
    $('#fileInfo').css('margin-bottom', '10px');
}

function mostrarProgreso(porcentaje) {
    $('#progreso').text(porcentaje + "%");
}