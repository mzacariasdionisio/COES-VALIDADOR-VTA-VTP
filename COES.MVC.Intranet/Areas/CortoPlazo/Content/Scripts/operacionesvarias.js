var controlador = siteRoot + 'cortoplazo/restriccionoperativa/';
var hot = null;
var arregloEquipo = [];

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
    
    $('#cbEmpresa').on('change', function () {
        consultar();
    });

    $(document).ready(function () {        
        consultar();
    });

});

cancelar = function () {
    document.location.href = controlador + "index";
}

consultar = function () {
    var fecha = $('#FechaDesde').val();
    var empresa = $('#cbEmpresa').val();

    var container = document.getElementById('contenedor');
    $(container).html("");    

    cargarGrilla(empresa,fecha);
}

var dropdownEquipoRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    var selectedId;

    for (var index = 0; index < arregloEquipo.length; index++) {
        if (parseInt(value) === arregloEquipo[index].id) {
            selectedId = arregloEquipo[index].id;
            value = arregloEquipo[index].text;
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

cargarGrilla = function (empresa,fecha) {        
    
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerDataOperacionNodal',
        datatype: 'json',
        data: {
            empresa: empresa,
            fecha: fecha            
        },
        success: function (result) {

            var data = result.Datos;
            var dataEquipo = result.ListGeneradorEquipo;
            var dataSubcausa = result.ListaEvensubcausa;
                                   
            arregloEquipo = [];

            for (var j in dataEquipo) {
                arregloEquipo.push({ id: dataEquipo[j].Equicodi, text: dataEquipo[j].Equinomb });                
            }
                        
            var container = document.getElementById('contenedor');
            var data = result.Datos;

            var hotOptions = {
                data: data,
                colHeaders: true,
                colHeaders: ['N°', 'Generador', 'Tipo', 'Valor', 'Desde','Hasta'],
                columns: [
                 { type: 'numeric', readOnly: true },
                 {                     
                 },
                 {
                     type: 'dropdown',
                     source: dataSubcausa
                 },
                 { type: 'numeric' },
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
                        cellProperties.renderer = dropdownEquipoRenderer;
                        cellProperties.width = "400px";
                        cellProperties.select2Options = {
                            data: arregloEquipo,
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

    var cad1 = "";
    for (i = 0; i < datos.length; i++) {
        
        if (datos[i][1] == '') {            
            break;
        }
        else {

            if (datos[i][2] == '') {
                mensaje = "Revise Tipo";
                flag = false;
            }
                
            ////alert(datos[i][3]);
            //if (datos[i][3] == '') {
            //    mensaje = "Ingrese valor";
            //    flag = false;
            //}
            //else {
            //    var numero = Number(datos[i][3]);
            //    if (numero < 0) {
            //        mensaje = "Revisar Valor negativo";
            //        flag = false;
            //    }
            //}
                       
            var horaini = datos[i][4];
            var horafin = datos[i][5];
            if (!validarHoraMinuto(horaini) || !validarHoraMinuto(horafin) || horaini == "" || horafin == "") {
                mensaje = "Ingrese horas";
                flag = false;
            }
            else
            {                
                var fechaini = "01/01/2016 " + horaini + ":00";
                var fechafin = "01/01/2016 " + horafin + ":00";
                
                if (horafin.localeCompare("00:00")!=0)
                {                    
                    if (Date.parse(fechaini) > Date.parse(fechafin))
                    {                        
                        mensaje = "Hora inicial supera la final";
                        flag = false;
                    }
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
    
    // GRABAR CABECERA    
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
            url: controlador + "grabaroperacionnodal",
            dataType: 'json',
            data: {
                dataExcel: $('#hfDatos').val(),
                fecha: $('#FechaDesde').val()
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

