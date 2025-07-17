var controlador = siteRoot + 'coordinacion/registroobservacion/';
var hot = null;

$(function () {

    $('#btnAgregarCanal').on('click', function () {
        openBusquedaCanal();
    });

    $('#btnGrabar').on('click', function () {
        grabar();
    });

    $('#btnCancelar').on('click', function () {
        document.location.href = controlador + 'index';
    });

    $('#cbEmpresa').val($('#hfIdEmpresa').val());

    if ($('#hfIndEdicion').val() == "N") {
        $('#btnGrabar').hide();
        $('#btnAgregarCanal').hide();
    }

    $("#cbTipoObservacion").val($("#hfTipo").val());
    validatipoobservacion();
    $('#cbEstado').val($('#hfEstado').val());

    cargarGrilla();
});

openBusquedaCanal = function () {
    if ($('#cbEmpresa').val() != "") {
        $.ajax({
            type: "POST",
            url: controlador + "busquedacanal",
            data: {
                emprcodi: $('#cbEmpresa').val()
            },
            success: function (evt) {
                $('#contenidoBusquedaCanal').html(evt);

                $('#btnSeleccionar').on('click', function () {
                    seleccionarCanales();
                });

                buscarCanal();

                setTimeout(function () {
                    $('#popupBusquedaCanal').bPopup({
                        autoClose:false
                    });
                }, 50);

                mostrarMensaje('mensajeEditar', 'message', 'Por favor complete los datos.');
            },
            error: function (req, status, error) {
                mostrarMensaje('mensajeEditar', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeEditar', 'alert', 'Por favor seleccione un empresa.');
    }
}

seleccionarCanales = function () {
    var canales = "";
    var countCanal = "";
    var contador = 0;
    $('#tablaCanal input:checked').each(function () {
        var constante = (contador > 0) ? "," : "";
        canales = canales + constante + $(this).val();
        countCanal++;
        contador++;
    });

    var count = 0;
    var items = "";
    $('#tablaEquipo>tbody tr').each(function (i) {
        $punto = $(this).find('#hfEquipoItem');
        var constante = (count > 0) ? "," : "";
        items = items + constante + $punto.val();
        count++;
    });

    if (countCanal > 0) {

        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerdatoscanal',
            data: {
                canales: canales
            },
            dataType: 'json',
            success: function (result) {
                agregarFilas(result);
                $('#popupBusquedaCanal').bPopup().close();
            },
            error: function () {
                mostrarMensajePopup('messageCanal', 'error', 'Se ha produccido un error.');
            }
        });
    }
    else
    {
        mostrarMensajePopup('messageCanal', 'alert', 'Seleccione al menos un registro.');
    }
}

cargarGrilla = function () {
    var idObservacion = $('#hfIdObservacion').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'cargargrilla',
        data: {
            idObservacion: idObservacion
        },
        dataType: 'json',
        success: function (result) {
            configurarGrilla(result);
        },
        error: function () {
            mostrarMensaje('mensajeEditar', 'error', 'Se ha producido un error.');
        }
    })
}

configurarGrilla = function (result) {

    var flagEdicion = ($('#hfIndEdicion').val() == "N") ? false : true;
    var longitud = (result.Datos.length > 0) ? result.Datos.length : 2;
    var container = document.getElementById('contenidoItems');
                


    var widths = [1, 180, 250, 220, 100, 250, 250];
    var columns = [
            { readOnly: true },
            { readOnly: true },
            { readOnly: true },
            { readOnly: true },
            { readOnly: !flagEdicion },
            { readOnly: !flagEdicion },
            { readOnly: true }
        ];
  
    hot = new Handsontable(container, {
        data: result.Datos,
        rowHeaders: false,
        colHeaders: false,        
        colWidths: widths,
        columns: columns,
        width: 1038,
        height: 400,
        fixedRowsTop: 1,
        fixedColumnsLeft: 4,
        fillHandle: {
            autoInsertRow: false
        },
        cells: function (row, col, prop) {
            var cellProperties = {};

            if (row == 0) {
                cellProperties.renderer = tituloRenderer;
                cellProperties.readOnly = true;
            }
            else if (col < 4 || col == 6) {
                cellProperties.renderer = disabledRenderer;
            }

            if (row > 0 && col == 4) {
                cellProperties.type = 'dropdown';
                cellProperties.source = result.Estados;
            }

            if (!flagEdicion) {
                if (row> 0 && (col == 4 || col == 5)) {
                    cellProperties.renderer = disabledRenderer;
                }
            }

            return cellProperties;
        }
    });

    hot.updateSettings({
        contextMenu: {
            callback: function (key, options) {
                if (key === "history_row") {
                    var selection = this.getSelectedRange();
                    var row = Math.min(selection.from.row, selection.to.row);                    
                    var canal = hot.getDataAtCell(row, 0);
                    verHistorialCambios(canal);
                    return false;
                }
            },
            items: {               
                "history_row": {
                    name: function () {
                        return " <div class= 'icon-history' >Ver histórico</div> ";
                    },
                    disabled: function () {
                        return hot.getSelected()[0] === 0;
                    }
                }

            }
        }
    });
}

agregarFilas = function (data) {

    var dataActual = hot.getData();    
    
    if (dataActual.length == 2 && dataActual[1][0] == "") {
        var dataActualFinal = [];
        dataActualFinal.push(hot.getDataAtRow(0));

        hot.updateSettings({
            data: dataActualFinal.concat(data)
        })        
    }
    else {                
        //-Solo agregamos los elementos que no hayan sido agregado anteriormente
        var arreglo = [];
        
        for (var j in data) {
            var flag = false;
            for (var i = 1; i < hot.countRows() ; i++) {
                if (data[j][0] == hot.getDataAtCell(i, 0)) {
                    flag = true;
                    break;
                }
            }

            if (!flag) {
                arreglo.push(data[j]);
            }
        }
        
        hot.updateSettings({
            data: hot.getData().concat(arreglo)
        })
    }
}

grabar = function () {
    var mensaje = validacion();

    if (mensaje == "") {
        $.ajax({
            type: "POST",
            url: controlador + 'grabar',
            dataType: "json",
            contentType: 'application/json',
            traditional: true,
            data: JSON.stringify({
                canales: hot.getData(),
                idEmpresa:$('#cbEmpresa').val(),
                observacion: $('#txtObservacion').val(),
                idObservacion: $('#hfIdObservacion').val(),                
                tipo: $('#cbTipoObservacion').val(),
                estado: $('#cbEstado').val(),
                observacionAgente: $('#txtObservacionAgente').val()
            }),
            success: function (result) {
                if (result.Id > 0) {
                    $('#hfIdObservacion').val(result.Id);
                    $('#txtEstado').val(result.Estado)
                    mostrarMensaje('mensajeEditar', 'exito', 'Los datos se grabaron correctamente.');
                }
                else {
                    mostrarMensaje('mensajeEditar', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEditar', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeEditar', 'alert', mensaje);
    }
}

validacion = function () {
    
    var mensaje = "";
    if ($("#cbTipoObservacion").val() == "O") {
        if (hot.countRows() == 1) {
            mensaje = mensaje + "Por favor agregue al menos un ICCP.";
        }
    }
    return mensaje;
}

verHistorialCambios = function (canal){
    $.ajax({
        type: 'POST',
        url: controlador + 'historiacanal',
        data: {
            canal: canal,
            idObservacion: $('#hfIdObservacion').val()
        },
        success: function (evt) {
            $('#contenidoHistoria').html(evt);
            setTimeout(function () {
                $('#popupHistoria').bPopup({

                });
            }, 100)
        },
        error: function () {
            mostrarMensaje('mensajeEditar', 'error', 'Se ha producido un error.');
        }
    })
}

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

var tituloRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontWeight = 'bold';
    td.style.textAlign = 'center';
    td.style.color = '#fff';
    td.style.backgroundColor = '#2980B9';
};

var disabledRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = '#3B83C1';
    td.style.backgroundColor = '#F5F5F5';
};

validatipoobservacion = function () {

    if ($('#cbTipoObservacion').val() == "O") {
        $("#contenidoItems").show();
        $("#botonera").show();
        $("#title").show();
        $('#btnAgregarCanal').show();
        $('#trObservacionAgente').hide();
        $('#trEstadoObservacion').show();
        $('#trEstadoEnlace').hide();

    } else {
        $("#contenidoItems").hide();
        $("#botonera").hide();
        $("#title").hide();
        $('#btnAgregarCanal').hide();
        $('#trObservacionAgente').show();
        $('#trEstadoObservacion').hide();
        $('#trEstadoEnlace').show();
        if ($('#hfIdObservacion').val() == "0") {
            $('#trObservacionAgente').hide();
            $('#trEstadoEnlace').hide();
        }
    }
}
