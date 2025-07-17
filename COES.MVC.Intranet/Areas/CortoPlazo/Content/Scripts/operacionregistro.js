var controlador = siteRoot + 'cortoplazo/operacionregistro/';
var hot = null;
var arregloGrupo = [];
var arregloSubcausa = [];
var hotProgramado = null;
var data = null;

$(function () {

    $('#FechaDesde').Zebra_DatePicker({
    });

    $('#FechaHasta').Zebra_DatePicker({
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
    
    $(document).ready(function () {        
        consultar();
    });

    
});

cancelar = function () {
    consultar();
}

consultar = function () {

    var fechaini = $('#FechaDesde').val();
    
    if (hot != null) {
        hot.destroy();
    }

    var container = document.getElementById('contenedor');   
    cargarGrilla(fechaini);
    cargarGrillaProgramado(fechaini);
}

var dropdownSubcausaRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var selectedId;

    for (var index = 0; index < arregloSubcausa.length; index++) {
        if (parseInt(value) === arregloSubcausa[index].id) {
            selectedId = arregloSubcausa[index].id;
            value = arregloSubcausa[index].text;
        }
    }

    $(td).addClass("estilocombo");
    Handsontable.TextCell.renderer.apply(this, arguments);
    $('#selectedId').text(selectedId);

}

var dropdownEquipoRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var selectedId;
    
    for (var index = 0; index < arregloGrupo.length; index++) {
        if (parseInt(value) === arregloGrupo[index].id) {
            selectedId = arregloGrupo[index].id;
            value = arregloGrupo[index].text;
        }
    }

    $(td).addClass("estilocombo");
    Handsontable.TextCell.renderer.apply(this, arguments);
    $('#selectedId').text(selectedId);

}

var fechahoraRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var selectedId;
    var r = /^(((0[1-9]|[12]\d|3[01])[\/\.-](0[13578]|1[02])[\/\.-]((19|[2-9]\d)\d{2})\s(0[0-9]|1[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9]))|((0[1-9]|[12]\d|30)[\/\.-](0[13456789]|1[012])[\/\.-]((19|[2-9]\d)\d{2})\s(0[0-9]|1[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9]))|((0[1-9]|1\d|2[0-8])[\/\.-](02)[\/\.-]((19|[2-9]\d)\d{2})\s(0[0-9]|1[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9]))|((29)[\/\.-](02)[\/\.-]((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))\s(0[0-9]|1[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9])))$/g;

    var value = instance.getDataAtCell(row, col);
    var td = instance.getCell(row, col);

    var esfechahora = r.test(value);

    if (!esfechahora) {
        td.style.backgroundColor = 'red';
    }
    else {
        td.style.background = '';
    }    
};

var validarFechaHora = function (fecha) {

    var r = /^(((0[1-9]|[12]\d|3[01])[\/\.-](0[13578]|1[02])[\/\.-]((19|[2-9]\d)\d{2})\s(0[0-9]|1[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9]))|((0[1-9]|[12]\d|30)[\/\.-](0[13456789]|1[012])[\/\.-]((19|[2-9]\d)\d{2})\s(0[0-9]|1[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9]))|((0[1-9]|1\d|2[0-8])[\/\.-](02)[\/\.-]((19|[2-9]\d)\d{2})\s(0[0-9]|1[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9]))|((29)[\/\.-](02)[\/\.-]((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))\s(0[0-9]|1[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9])))$/g;        
    var esfechahora = r.test(fecha);
    return esfechahora;
};

var eliminar = function (id) {

    $.ajax({
        type: 'POST',
        url: controlador + 'operacionregistrodelete',
        datatype: 'json',
        data: {
            id: id
        },
        success: function (result) {            
                        
        },
        error: function (jqXHR, textStatus, errorThrown) {
            mostrarError();
            console.log('Error:', jqXHR);
        }
    });

}

cargarGrilla = function (fechaini) {       
    
    $.ajax({
        type: 'POST',
        url: controlador + 'obteneroperacionregistro',
        datatype: 'json',
        data: {            
            fechaini: fechaini
        },
        success: function (result) {
                                                
            var dataGrupo = result.ListaPrGrupo;
            var dataSubcausa = result.ListaEveSubcausaevento;
          
            arregloGrupo = [];
            arregloSubcausa = [];
            
            for (var j in dataGrupo) {
                arregloGrupo.push({ id: dataGrupo[j].Grupocodi, text: dataGrupo[j].Gruponomb });
                //console.log("Grupo: " + j + "***" + dataGrupo[j].Relacioncodi + "***" + dataGrupo[j].Equinomb);
            }

            for (var j in dataSubcausa) {
                arregloSubcausa.push({ id: dataSubcausa[j].Subcausacodi, text: dataSubcausa[j].Subcausadesc });
                //console.log("Subcausa: " + j + "***" + dataSubcausa[j].Subcausacodi + "***" + dataSubcausa[j].Subcausadesc);
            }

            var container = document.getElementById('contenedor');

            //datos del listado procesado
            data = result.Datos;

            var hotOptions = {
                data: data,
                colHeaders: true,
                colHeaders: ['N°', 'Modo de Operación','Calificación', 'Desde', 'Hasta'],
                columns: [
                 { type: 'numeric', readOnly: true },                 
                 {
                 },
                 {
                 },
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
                height: 670,                
                colWidths: [1, 300,300, 60, 60],
                cells: function (row, col, prop) {

                    var cellProperties = {};                   
                    
                    if (col == 1) {
                        cellProperties.editor = 'select2';
                        cellProperties.renderer = dropdownEquipoRenderer;
                        cellProperties.width = "320px";
                        cellProperties.select2Options = {
                            data: arregloGrupo,
                            dropdownAutoWidth: true,
                            width: 'resolve',
                            allowClear: false,
                        };
                    }
                    if (col == 2) {
                        cellProperties.editor = 'select2';
                        cellProperties.renderer = dropdownSubcausaRenderer;
                        cellProperties.width = "300px";
                        cellProperties.select2Options = {
                            data: arregloSubcausa,
                            dropdownAutoWidth: true,
                            width: 'resolve',
                            allowClear: false,
                        };
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

cargarGrillaProgramado = function (fechaini) {
    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerregistroprogramado',
        datatype: 'json',
        data: {
            fechaini: fechaini
        },
        success: function (result) {

            var container = document.getElementById('contenedorProgramado');
            var dataProgramado = result;

            var hotOptions = {
                data: dataProgramado,
                colHeaders: true,
                colHeaders: ['N°', 'Modo de Operación', 'Calificación', 'Desde', 'Hasta'],
                columns: [
                  { type: 'numeric', readOnly: true },
                  {
                      readOnly:true
                  },
                  {
                      readOnly: true
                  },
                  {
                      type: 'time',
                      readOnly:true,
                      timeFormat: 'HH:mm',
                      correctFormat: true
                  },
                  {
                      type: 'time',
                      readOnly:true,
                      timeFormat: 'HH:mm',
                      correctFormat: true
                  }
                ],
                rowHeaders: true,
                comments: true,
                height: 770,
                colWidths: [1, 320, 1, 60, 60],
                cells: function (row, col, prop) {

                    var cellProperties = {};

                    if (col == 1) {
                        cellProperties.editor = 'select2';
                        cellProperties.renderer = dropdownEquipoRenderer;
                        cellProperties.width = "300px";
                        cellProperties.select2Options = {
                            data: arregloGrupo,
                            dropdownAutoWidth: true,
                            width: 'resolve',
                            allowClear: false,
                        };
                    }
                  

                    return cellProperties;
                },
            };

            hotProgramado = new Handsontable(container, hotOptions);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            mostrarError();
            console.log('Error:', jqXHR);
        }
    });
}

copiarContenido = function () {
    
    if (hotProgramado != null && hot != null) {
        for (var i = 0; i <= hotProgramado.countRows() - 1 ; i++) {
            data.push(hotProgramado.getDataAtRow(i));
        }

        var dataNew = [];
        for (var j in data)
        {
            if (!(
                (data[j][1] == '' || data[j][1] == null) &&
                (data[j][2] == '' || data[j][2] == null) &&
                (data[j][3] == '' || data[j][3] == null) &&
                (data[j][4] == '' || data[j][4] == null)
                ))
            {                
                dataNew.push(data[j]);                
            }
        }
        
        data = dataNew;        
        hot.updateSettings({
            data: data
        })
    }
}


convertDateTime = function (fechahora) {

    var fh = fechahora.toString().split(" ");
    var fecha = fh[0].split("/");
    var hora = fh[1].split(":");
    var dd = fecha[0];
    var mm = fecha[1] - 1;
    var yyyy = fecha[2];
    var h = hora[0];
    var m = hora[1];
    var s = parseInt(hora[2]);

    return new Date(yyyy, mm, dd, h, m, s);
}

obtenerFecha = function (fechahora) {
    var l1 = fechahora.getDate() + "/" + (fechahora.getMonth() + 1) + "/" + fechahora.getFullYear();
    return l1;
}

obtenerHora = function (fechahora) {
    var l1 = (fechahora.getHours() < 10 ? '0' : '') + fechahora.getHours() + ":" + (fechahora.getMinutes() < 10 ? '0' : '') + fechahora.getMinutes() + ":" + (fechahora.getSeconds() < 10 ? '0' : '') + fechahora.getSeconds();
    l1 = l1.toString();
    return l1;
}

validarDetalle = function (datosDetalle) {
    var mensaje = "<ul>";
    var flag = true;
        
    var datos = datosDetalle;

    var cad1 = "";
    console.log(datos.length);
    for (i = 0; i < datos.length; i++) {

        //revisión de simbolo

        if (datos[i][1] == '') {
            break;
        }
        else {

            var modo = datos[i][1];
            if (modo == '') {
                mensaje = "Ingrese modo de operación";
                flag = false;
            } else {

                //var calif = datos[i][2];
                //if (calif == '') {
                //    mensaje = "Ingrese calificación";
                //    flag = false;
                //} else {
                //revision de fecha
                var horaini = datos[i][3];
                var horafin = datos[i][4];
                if (!validarHoraMinuto(horaini) || !validarHoraMinuto(horafin) || horaini == "" || horafin == "") {
                    mensaje = "Ingrese horas";
                    flag = false;
                }
                else {
                    //Date.parse('01/01/2011 10:20:45') > Date.parse('01/01/2011 5:10:10')
                    var fechaini = "01/01/2016 " + horaini + ":00";
                    var fechafin = "01/01/2016 " + horafin + ":00";
                    console.log(fechaini + " " + fechafin);
                    if (horafin.localeCompare("00:00") != 0) {
                        if (Date.parse(fechaini) > Date.parse(fechafin)) {
                            mensaje = "Hora inicial supera la final";
                            flag = false;
                        }
                    }
                    //}
                }
            }

        }
    }

    $('#hfDatos').val(datos);

    if (flag) mensaje = "";
    return mensaje;

}

validarHoraMinuto = function (hhmm) {
    var isValid = /^([0-1]?[0-9]|2[0-4]):([0-5][0-9])(:[0-5][0-9])?$/.test(hhmm);
    return isValid;
}

grabar = function () {          
    var datos = hot.getData();    
    var mensajeDetalle = validarDetalle(datos);
    
    eliminarAlerta();
        
    if (mensajeDetalle == "") {

        //graba detalle
        var error = "";
        var mensaje = "<ul>";
        var flag = true;

        $.ajax({
            type: 'POST',
            url: controlador + "grabar",
            dataType: 'json',
            data: {                
                dataExcel: $('#hfDatos').val(),
                fecha:$('#FechaDesde').val()
            },
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

