var controlador = siteRoot + 'eventos/rsf/';
var hot = null;
var arregloGeneral = [
    { id: 1, text: 'Asignar reserva a TV' },
    { id: 2, text: 'Asignación por unidad - Térmica' },
    { id: 3, text: 'Asignación por unidad - Hidro' },
    { id: 4, text: 'Dividir reserva entre unidades operativas' },
    { id: 5, text: 'Dividir reserva en base a los limites min y max' }
];

$(function () {
        
    $('#btnGrabar').on('click', function () {
        grabar();
    });     

    consultar();
});


$(function () {

    $('#btnConfAvanzada').on('click', function () {
        alert('hi');
        document.location.href = controlador + 'configuracionavanzada';
    });

   
});




consultar = function () {
    if (hot != null) {
        hot.destroy();
    }
    
    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerconfiguracion',        
        dataType: 'json',
        success: function (result) {
            cargarGrilla(result);
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

cargarGrilla = function (result) {

    var container = document.getElementById('contenedor');
    var data = result.Datos;    

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

    var totalSumRenderer = function (instance, td, row, col, prop, value, cellProperties) {

        td.style.backgroundColor = '#70AD47';
        td.style.textAlign = 'right';
        td.style.fontWeight = 'bold';
        td.style.color = '#ffffff';
        td.style.fontSize = '11px';
        td.style.verticalAlign = 'middle';
        var valor = obtenerTotal(col, instance);
        if ($.isNumeric(valor)) {
            value = valor;
            var comparacion = parseFloat($('#hfRA').val());
            if (parseFloat(valor) != comparacion) {
                td.style.backgroundColor = 'red';
            }
        }
        Handsontable.renderers.NumericRenderer.apply(this, arguments);
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

    var disbledRenderer2 = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#efe7e7';
        td.style.fontSize = '11px';
        td.style.verticalAlign = 'middle';
    };


    var disbledDerechaRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.NumericRenderer.apply(this, arguments);
        td.style.backgroundColor = '#FFDBA4';
        td.style.fontSize = '11px';
        td.style.verticalAlign = 'middle';
    };

    var dropdownListaRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        var selectedId;
        var tipo = "Línea";
        for (var index = 0; index < arregloGeneral.length; index++) {
            if (parseInt(value) === arregloGeneral[index].id) {
                selectedId = arregloGeneral[index].id;
                value = arregloGeneral[index].text;
            }
        }

        $(td).addClass("estilocombo");
        Handsontable.TextCell.renderer.apply(this, arguments);
        $('#selectedId').text(selectedId);

    };

    hot = new Handsontable(container, {
        data: data,
        rowHeaders: false,
        colHeaders: false,
        comments: true,
        height: 600,
        fixedRowsTop: 1,
        fixedColumnsLeft: 5,
        maxRows: result.Longitud + 3,
        colWidths: [75, 180, 120, 100, 80, 260, 100,100],
        cells: function (row, col, prop) {
            var cellProperties = {};

            if (row == 0) {
                cellProperties.renderer = tituloRenderer;              

            }
            if (row > 0 && row <= result.Longitud && col < 3) {
                cellProperties.renderer = subTituloRenderer;
            }
           
            if (col < 3) {
                cellProperties.readOnly = true;
            }
            if (row == result.Longitud + 1 && col <= 4) {
                cellProperties.readOnly = true;
                cellProperties.renderer = totalRenderer;
            }
            if (row == result.Longitud + 1 && col > 4) {
                cellProperties.readOnly = true;
                cellProperties.renderer = totalSumRenderer;
            }
            if (row > 0 && row <= result.Longitud + 1 && (col >= 6 && col <=7)) {
                cellProperties.format = '0,0.000';
                cellProperties.type = 'numeric';
            }
            if (row == result.Longitud + 2 && col == 0) {
                cellProperties.renderer = comentarioRenderer;
            }
            if (row == result.Longitud + 2 && col > 4) {
                cellProperties.renderer = textoRenderer;
            }

            if (result.Indices.indexOf(row) != -1 && col <= 2) {
                cellProperties.renderer = disbledRenderer;
                cellProperties.readOnly = true;
            }           

            if (row > 0 && col == 5) {

                if (result.Indices.indexOf(row)!=-1) {
                    cellProperties.editor = 'select2';
                    cellProperties.renderer = dropdownListaRenderer;
                    cellProperties.width = "400px";
                    cellProperties.select2Options = {
                        data: arregloGeneral,
                        dropdownAutoWidth: true,
                        width: 'resolve',
                        allowClear: false,
                    };
                }
                else {
                    cellProperties.renderer = disbledRenderer2;
                    cellProperties.readOnly = true;
                }                
            }

            return cellProperties;
        }      
    });  
   
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
            url: controlador + 'grabarconfiguracion',
            dataType: "json",
            contentType: 'application/json',
            traditional: true,
            data: JSON.stringify({
                datos: datos
            }),
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'Los datos se grabaron correctamente.');
                    consultar();
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
    console.log(data);
    var mensaje = "<ul>";

    var validacionFormato = true;
    var validacionRango = true;

    for (var j = 1; j < data.length; j++) {

        var min = data[j][6];
        var max = data[j][7];

        console.log(min);
        console.log(max);

        if (min != null && min != "") {
            if (!$.isNumeric(min)) {
                validacionFormato = false;
            }
        }
        if (max != null && max != "") {
            if (!$.isNumeric(max)) {
                validacionFormato = false;
            }
        }

        if ($.isNumeric(min) && $.isNumeric(max)) {

            if (parseFloat(min) > parseFloat(max)) {
                validacionRango = false;
            }
        }
    }


    if (!validacionFormato) {
        mensaje = mensaje + "<li>Ingrese valores correctos.</li>";
    }

    if (!validacionRango) {
        mensaje = mensaje + "<li>El límite máximo debe ser mayor que el mínimo.</li>";
    }

    mensaje = mensaje + "</ul>";

    if (validacionFormato && validacionRango) {
        mensaje = "";
    }
    return mensaje;
};


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
    var regExp = /(\d{1,2})\:(\d{1,2})\:(\d{1,2})/;
    if (parseInt(endTime.replace(regExp, "$1$2$3")) > parseInt(startTime.replace(regExp, "$1$2$3"))) {
        return true;
    }
    return false;
};

