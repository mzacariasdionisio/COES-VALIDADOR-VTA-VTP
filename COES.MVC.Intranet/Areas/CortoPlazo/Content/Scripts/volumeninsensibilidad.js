var controlador = siteRoot + 'cortoplazo/volumen/';
var hot = null;
var arregloRecurso = [];

$(function () {

    $('#FechaDesde').Zebra_DatePicker({
        onSelect: function () {
            consultar();
        }
    });

    $('#btnVer').on('click', function () {
        consultar();
    });

    $('#btnCancelar').on('click', function () {
        cancelar();
    });

    $('#btnGrabar').on('click', function () {
        grabar();
    });
      
    consultar();  
});

cancelar = function () {
    document.location.href = controlador + "index";
}

consultar = function () {
    var fecha = $('#FechaDesde').val();
    var container = document.getElementById('contenedor');
    $(container).html("");
    cargarGrilla(fecha);
}

var dropdownRecursoRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var selectedId;

    for (var index = 0; index < arregloRecurso.length; index++) {
        if (parseInt(value) === arregloRecurso[index].id) {
            selectedId = arregloRecurso[index].id;
            value = arregloRecurso[index].text;
        }
    }
    $(td).addClass("estilocombo");
    Handsontable.TextCell.renderer.apply(this, arguments);
    $('#selectedId').text(selectedId);
}

function delete_row_renderer(instance, td, row, col, prop, value, cellProperties) {
    var div;
    $(td).children('.btn').remove();
    div = document.createElement('div');
    div.className = 'btn';
    div.appendChild(document.createTextNode("."));
    td.appendChild(div);

    $(div).on('mouseup', function () {
        eliminar(value);
    });
    $(td).addClass("deleteIcon");
    return td;
}

eliminar = function (id) {
    if (id != "") {
        if (confirm('¿Está seguro de realizar esta operación?')) {
            $.ajax({
                type: 'POST',
                url: controlador + 'eliminar',
                data: {
                    id: id
                },
                dataType: 'json',
                success: function (result) {
                    if (result == 1) {
                        consultar();
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
    }
}

cargarGrilla = function (fecha) {

    $.ajax({
        type: 'POST',
        url: controlador + 'consultar',
        datatype: 'json',
        data: {            
            fecha: fecha
        },
        success: function (result) {

            var data = result.Data;
            var dataRecurso = result.ListaEmbalses;
           
            arregloRecurso = [];

            for (var j in dataRecurso) {
                arregloRecurso.push({ id: dataRecurso[j].Recurcodi, text: dataRecurso[j].Recurnombre });
            }

            var container = document.getElementById('contenedor');
            
            var hotOptions = {
                data: data,
                colHeaders: ['N°', 'Embalse', 'VI Inf', 'VI Sup', 'Desde', 'Hasta'],
                columns: [
                    { type: 'numeric', readOnly: true },
                    {
                    },
                    {  },
                    {  },
                    {
                        type: 'time',
                        timeFormat: 'HH:mm',
                        correctFormat: true
                    },
                    {
                        type: 'time',
                        timeFormat: 'HH:mm',
                        correctFormat: true
                    }
                ],
                rowHeaders: true,
                comments: true,

                height: 770,
                colWidths: [70, 400, 110, 110, 110, 110],
                cells: function (row, col, prop) {

                    var cellProperties = {};
                    if (col == 1) {
                        cellProperties.editor = 'select2';
                        cellProperties.renderer = dropdownRecursoRenderer;
                        cellProperties.width = "400px";
                        cellProperties.select2Options = {
                            data: arregloRecurso,
                            dropdownAutoWidth: true,
                            width: 'resolve',
                            allowClear: false,
                        };
                    }
                    if (col == 2 || col == 3) {
                        cellProperties.format = '0,0.00';
                        cellProperties.type = 'numeric';
                    }
                    return cellProperties;
                },
                afterLoadData: function () {
                    this.render();
                },
                beforeRemoveRow: function (index, amount) {
                    console.log('beforeRemove: index: %d, amount: %d', index, amount);
                    //eliminando filas
                    var codigos = "";
                    for (var i = amount; i > 0; i--) {

                        var valorCelda = hot.getDataAtCell(index + i - 1, 0);

                        if (valorCelda != "" && valorCelda != null) {
                            eliminar(valorCelda);
                            console.log('id eliminado: ' + valorCelda);
                        }
                    }
                }
            };

            if (hot != null) {
                hot.destroy();
            }

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
                            name: function () {
                                return " <div class= 'icon-agregar' >Agregar fila</div> ";
                            }
                        },
                        "remove_row": {
                            name: function () {
                                return " <div class= 'icon-eliminar' >Eliminar fila</div> ";
                            }
                        }
                    }
                }
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            mostrarError();
            console.log('Error:', jqXHR);
        }
    });
}

validarDetalle = function (datosDetalle) {

    var mensaje = "<ul>";
    var flag = true;
    var datos = datosDetalle;

    for (i = 0; i < datos.length; i++) {

        if (datos[i][1] != '' && datos[i][1] !=null) {
            var horaini = datos[i][4];
            var horafin = datos[i][5];
            var volinf = datos[i][2];
            var volsup = datos[i][3];

            if (!validarNumero(volinf) || !validarNumero(volsup)) {
                mensaje = "Ingrese valores válidos de VI Inf y VI Sup";
                flag = false;
                break;
            }

            if (!validarHoraMinuto(horaini) || !validarHoraMinuto(horafin) || horaini == "" || horafin == "") {
                mensaje = "Ingrese horas para los embalses seleccionados";
                flag = false;
                break;
            }
            else {
                var fechaini = "01/01/2016 " + horaini + ":00";
                var fechafin = "01/01/2016 " + horafin + ":00";

                if (horafin.localeCompare("00:00") != 0) {
                    if (Date.parse(fechaini) > Date.parse(fechafin)) {
                        mensaje = "La hora inicial supera la final";
                        flag = false;
                        break;
                    }
                }
            }
        }
    }

    if (flag) mensaje = "";
    return mensaje;

}

validarHoraMinuto = function (hhmm) {
    var isValid = /^([0-1]?[0-9]|2[0-4]):([0-5][0-9])(:[0-5][0-9])?$/.test(hhmm);
    return isValid;
}

function validarNumero(texto) {
    return /^-?[\d.]+(?:e-?\d+)?$/.test(texto);
};

grabar = function () {

    var datos = hot.getData();
    console.log(datos);
    var mensajeDetalle = validarDetalle(datos);
    eliminarAlerta();

    if (mensajeDetalle == "") {
        $.ajax({
            type: 'POST',
            url: controlador + "GrabarVolumen",            
            contentType: 'application/json',
            data: JSON.stringify({              
                data: datos,
                fecha: $('#FechaDesde').val()
            }),
            dataType: 'json',
            success: function (result) {
                if (result != "-1") {
                    mostrarExito();
                    consultar();
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
        mostrarAlerta(mensajeDetalle);
    }
}

eliminarAlerta = function () {
    $('#mensaje').removeClass();
    $('#mensaje').html("");
}

mostrarError = function () {
    alert('Ha ocurrido un error.');
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

