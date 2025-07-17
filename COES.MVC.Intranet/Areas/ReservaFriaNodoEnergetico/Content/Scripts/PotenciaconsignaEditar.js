var controlador = siteRoot + 'reservafrianodoenergetico/potenciaconsigna/';
var hot = null;
var titulos = null;
var subTitulos = null;
var mantenimientos = null;
var agrupaciones = null;
var comentarios = null;
var adicionales = null;
var finales = null;
var merge = null;
var intervalo = null;
var errors = null;


$(function () {

    $('#txtNrpcFecha').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#txtNrpcFecha').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtNrpcFecha').val(date);
        }
    });
    
    $('#cbNrsmodCodi').on('change', function () {
        cargarModoOperacion(this.value, 'cbGrupoCodi', false);
    });

    $('#btnCancelar').click(function () {
        document.location.href = controlador;
    });
    
    $('#rbNrpcEliminadoN').prop('checked', true);
    
    $(document).ready(function () {

        if ($('#hfNrpcEliminado').val() == 'S') { $('#rbNrpcEliminadoS').prop('checked', true); }
        if ($('#hfNrpcEliminado').val() == 'N') { $('#rbNrpcEliminadoN').prop('checked', true); }
        
        $('#cbNrsmodCodi').val($('#hfNrsmodCodi').val());
        $('#cbGrupoCodi').val($('#hfGrupoCodi').val());
        

        if ($('#hfAccion').val() == 0) {
            $('#btnGrabar').hide();
        }

    });

    $('#btnGrabar').click(function () {
        grabar();
    });

    cargarGrilla();
    
});

ObtenerPotenciaDetalle = function () {
    cargarGrilla();
}


cargarGrilla = function () {

    var id = $('#hfNrpcCodi').val();

    if (true)
    {
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerpotenciadetalle',
            datatype: 'json',
            data: {
                id: id
            },
            success: function (result) {
                errors = [];
                clearInterval(intervalo);

                var container = document.getElementById('contenedor');

                merge = result.Merge;
                titulos = result.IndicesTitulo;
                subTitulos = result.IndicesSubtitulo;
                validaciones = result.Validaciones;
                agrupaciones = result.IndicesAgrupacion;
                comentarios = result.IndicesComentario;

                var tituloRenderer = function (instance, td, row, col, prop, value, cellProperties) {
                    Handsontable.renderers.TextRenderer.apply(this, arguments);
                    td.style.fontWeight = 'bold';
                    td.style.textAlign = 'left';
                    td.style.color = '#fff';
                    td.style.backgroundColor = '#1991B5';
                };

                var subTituloRenderer = function (instance, td, row, col, prop, value, cellProperties) {
                    Handsontable.renderers.TextRenderer.apply(this, arguments);
                    td.style.backgroundColor = '#D7EFEF';
                    td.style.textAlign = 'center';
                    td.style.fontWeight = 'bold';
                    td.style.color = '#1C91AE';
                    td.style.verticalAlign = 'middle';
                };

                var data = result.Datos;
                var titulos = result.IndicesTitulo;

                var defaultRenderer = function (instance, td, row, col, prop, value, cellProperties) {
                    Handsontable.renderers.TextRenderer.apply(this, arguments);
                    td.style.backgroundColor = '#FFF';
                    td.style.fontSize = '11px';
                };
                                
                var hotOptions = {
                    data: data,
                    colHeaders: true,
                    colHeaders: ['Hora', 'MW', 'Máxima Gen.'],
                    columns: [
     {
         type: 'time',
         timeFormat: 'HH:mm',
         correctFormat: true
     },
     { type: 'numeric' },
     {
         // type: 'checkbox'
         //, checkedTemplate: true
         //, uncheckedTemplate: false
     }
                    ],
                    rowHeaders: true,
                    comments: true,
                    colWidths: [150, 110, 110, 110, 110, 110, 110],
                    cells: function (row, col, prop) {

                        var cellProperties = {};

                        cellProperties.renderer = defaultRenderer;

                        if (titulos.indexOf(row) != -1) {
                            cellProperties.renderer = tituloRenderer;
                            cellProperties.readOnly = true;
                        }

                        if (subTitulos.indexOf(row) != -1) {
                            cellProperties.renderer = subTituloRenderer;
                            cellProperties.readOnly = true;
                        }
                        

                        for (var i in validaciones) {
                            if (validaciones[i]['Row'] == row && validaciones[i]['Column'] == col) {
                                cellProperties.type = 'dropdown';
                                cellProperties.source = validaciones[i].Elementos;
                                break;
                            }
                        }
                        return cellProperties;

                    },
                    mergeCells: merge,
                    afterCreateRow: function (index, amount) {
                        hotOptions.mergeCells = merge;
                        hot.mergeCells = new Handsontable.MergeCells(hotOptions.mergeCells);
                        hot.updateSettings(hotOptions);
                    },
                    afterRemoveRow: function (index, amount) {

                        for (var i in subTitulos) {
                            if (index < subTitulos[i] && index > 2) {
                                subTitulos[i] = subTitulos[i] - 1;
                            }
                        }
                    },
                    afterLoadData: function () {
                        this.render();
                    },
                    afterValidate: function (isValid, value, row, prop, source) {
                        if (value != "") {
                            if (!isValid) {
                                errors.push({ row: row, column: prop });
                            }
                            else {
                                for (var j in errors) {
                                    if (errors[j]['row'] == row && errors[j]['column'] == prop) {
                                        errors.splice(j, 1);
                                    }
                                }
                            }
                        }
                        else {
                            for (var j in errors) {
                                if (errors[j]['row'] == row && errors[j]['column'] == prop) {
                                    errors.splice(j, 1);
                                }
                            }
                            return true;
                        }
                    }
                };

                hot = new Handsontable(container, hotOptions);

                hot.updateSettings({
                    contextMenu: {
                        callback: function (key, options) {
                            if (key === 'about') {
                                setTimeout(function () {
                                    alert("This is a context menu with default and custom options mixed");
                                }, 100);
                            }
                        },
                        items: {
                            "row_below": {
                                name: 'Agregar fila',
                                disabled: function () {
                                    return (subTitulos.indexOf(hot.getSelected()[0]) > -1 || titulos.indexOf(hot.getSelected()[0]) > -1
                                        || agrupaciones.indexOf(hot.getSelected()[0]) > -1 || comentarios.indexOf(hot.getSelected()[0]) > -1);
                                    
                                }
                            },
                            "remove_row": {
                                name: 'Eliminar fila',
                                disabled: function () {
                                    return (subTitulos.indexOf(hot.getSelected()[0]) > -1 || titulos.indexOf(hot.getSelected()[0]) > -1
                                        || agrupaciones.indexOf(hot.getSelected()[0]) > -1 || comentarios.indexOf(hot.getSelected()[0]) > -1
                                        || hot.getSelected().length == 1);
                                }
                            }
                        }
                    }
                });
                
                if (result.Indicador == 0) {
                    hot.setDataAtCell(hot.countRows() - 4, 8, obtenerHora());
                    intervalo = setInterval(function () {
                        if (hot != null) {
                            hot.setDataAtCell(hot.countRows() - 4, 8, obtenerHora());
                        }

                    }, 1000);
                }

            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {        
    }
}

cargarModoOperacion = function (nrsmodcodi, control, todos) {
    
    $.ajax({
        type: 'POST',
        url: controlador + "cargarmodooperacion",
        dataType: 'json',
        cache: false,
        data: {
            nrsmodCodi: nrsmodcodi
        },
        success: function (evt) {
            
            var _len = evt.length;

            var cad1 = _len + "\r\n";

            $('#' + control).empty();

            if (todos) {
                var select = document.getElementById(control);
                select.options[0] = new Option("TODOS", 0);
            }

            for (i = 0; i < _len; i++) {

                post = evt[i];

                var select = document.getElementById(control);
                select.options[select.options.length] = new Option(post.Gruponomb, post.Grupocodi);
            }
        },
        error: function () {
            mostrarError();
        }
    });
}


validarHoraMinuto = function (horamin) {
    var result = false, m;
    var re = /^\s*([01]?\d|2[0-3]):?([0-5]\d)\s*$/;

    if ((m = horamin.match(re))) {
        result = (m[1].length === 2 ? "" : "0") + m[1] + ":" + m[2];
    }

    return result;
}


obtenerHora = function () {
    var d = new Date();
    var h = ("0" + d.getHours()).slice(-2);
    var m = ("0" + d.getMinutes()).slice(-2);
    var s = ("0" + d.getSeconds()).slice(-2);

    return h + ":" + m + ":" + s;
}

mostrarAlerta = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-alert');
    $('#mensaje').html(mensaje);
}

mostrarExito = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-exito');
    $('#mensaje').html('La operación se realizó con éxito.');
}

eliminarAlerta = function () {
    $('#mensaje').removeClass();
    $('#mensaje').html("");
}

validarRegistro = function () {

    var mensaje = "<ul>";
    var flag = true;

    $('#hfNrsmodCodi').val($('#cbNrsmodCodi').val());
    $('#hfGrupoCodi').val($('#cbGrupoCodi').val());

    if ($('#rbNrpcEliminadoS').is(':checked')) {
        $('#hfNrpcEliminado').val('S');
    }

    if ($('#rbNrpcEliminadoN').is(':checked')) {
        $('#hfNrpcEliminado').val('N');
    }
   
    if ($('#cbNrsmodCodi').val() == "-1" || $('#cbNrsmodCodi').val() == null || $('#cbNrsmodCodi').val() == undefined) {
        mensaje += "<li>Debe seleccionar módulo</li>";//\n";
        flag = false;
    }

    if ($('#cbGrupoCodi').val() == "-1" || $('#cbGrupoCodi').val() == null || $('#cbGrupoCodi').val() == undefined) {
        mensaje += "<li>Debe seleccionar modo de operación</li>";//\n";
        flag = false;
    }

    if (flag) mensaje = "";
    return mensaje;
}


validarDetalle = function (datosDetalle) {
    var mensaje = "<ul>";
    var flag = true;

    var datos = datosDetalle;

    var cad1 = "";
    for (i = 0; i < datos.length; i++) {

        if (datos[i][0] == '' && datos[i][1] == '' && datos[i][2] == '') {
            //no validar
        }
        else {
            if (datos[i][0] == '') {
                mensaje += "<li>Debe ingresar hora (" + "Fila: " + (i + 1) + ")</li>";//\n";
                flag = false;
            }
            else {
                if (!validarHoraMinuto(datos[i][0])) {
                    mensaje += "<li>Formato hora no válido (" + "Fila: " + (i + 1) + ")</li>";//\n";
                    flag = false;
                }
            }

            if (datos[i][1] != '') {
                var num = parseFloat(datos[i][1]);

                if (isNaN(num)) {
                    mensaje += "<li>Debe ingresar MW válido (" + "Fila: " + (i + 1) + ")</li>";//\n";
                    flag = false;
                }

                if (num <= 0) {
                    mensaje += "<li>Debe ingresar MW mayor a cero (" + "Fila: " + (i + 1) + ")</li>";//\n";
                    flag = false;
                }
            }
            else {
                mensaje += "<li>Debe ingresar MW mayor a cero (" + "Fila: " + (i + 1) + ")</li>";//\n";
                flag = false;
            }

            if (datos[i][2] != 'S' && datos[i][2] != 'N') {

                if (datos[i][2] == 's') {
                    datos[i][2] = 'S';
                    continue;
                }

                if (datos[i][2] == 'n') {
                    datos[i][2] = 'N';
                    continue;
                }

                if (datos[i][2] == '') {
                    datos[i][2] = 'N';
                }
                else {
                    mensaje += "<li>Debe ingresar Máxima generación (S/N) (" + "Fila: " + (i + 1) + ")</li>";//\n";
                    flag = false;
                }
            }
        }
    }

    $('#hfDatos').val(datos);

    if (flag) mensaje = "";
    return mensaje;
}

grabar = function () {

    // GRABAR CABECERA    
    var datos = hot.getData();

    var mensaje = validarRegistro();
    var mensajeDetalle = validarDetalle(datos);
   

    eliminarAlerta();

    if (mensaje == "" && mensajeDetalle == "") {

        $.ajax({
            type: 'POST',
            url: controlador + "grabar",
            dataType: 'json',
            data: $('#form').serialize(),
            success: function (result) {
                if (result != "-1") {

                    //mostrarExito();
                    $('#hfNrpcCodi').val(result);
                    
                    //graba detalle
                    var error = "";
                    var mensaje = "<ul>";
                    var flag = true;

                    $.ajax({
                        type: 'POST',
                        url: controlador + "grabarConsigna",
                        dataType: 'json',
                        data: {
                            id: $('#hfNrpcCodi').val(),
                            dataExcel: $('#hfDatos').val(),
                            fecha: $('#txtNrpcFecha').val()
                        },
                        success: function (result) {
                            if (result != "-1") {
                                mostrarExito();
                                document.location.href = controlador;
                            }
                            else {
                                mostrarError();
                            }
                        },
                        error: function () {
                            mostrarError();
                        }
                    });                  
                }
                else {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        mostrarAlerta(mensaje + '\n' + mensajeDetalle);
    }    
}

mostrarError = function () {
    alert("Ha ocurrido un error");
}



