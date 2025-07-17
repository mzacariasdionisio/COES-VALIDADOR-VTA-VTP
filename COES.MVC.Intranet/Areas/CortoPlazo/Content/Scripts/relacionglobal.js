var controlador = siteRoot + 'cortoplazo/configuracion/';
var hot = null;
var arregloEquipo = [];

$(function () {

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
    document.location.href = controlador + "relacion";
}

consultar = function () {
    var container = document.getElementById('contenedor');
    $(container).html("");    

    var empresa = $('#cbEmpresa').val();
    var estado = $('#cbEstado').val();

    cargarGrilla(empresa, estado);
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

cargarGrilla = function (empresa, estado) {

    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerDataGenerador',
        data:{
            empresa:empresa,
            estado: estado
        },
        datatype: 'json',
        success: function (result) {

            $.ajax({
                type: 'POST',
                url: controlador + 'ObtenerListaGenerador',
                datatype: 'json',
                success: function (result2) {

                    var dataEquipo = result2.ListaEquipo;

                    arregloEquipo = [];
                    for (var j in dataEquipo) {
                        arregloEquipo.push({ id: dataEquipo[j].Equicodi, text: dataEquipo[j].Equinomb });                        
                    }

                    var container = document.getElementById('contenedor');
                    var data = result.Datos;

                    var hotOptions = {
                        data: data,
                        colHeaders: true,
                        colHeaders: ['N°', 'Equipo', 'Barra (PSSE)', 'ID Gen (PSSE)', 'Código NCP', 'Nombre NCP', 'Estado'],
                        columns: [
                         { type: 'numeric', readOnly: true },
                         {
                         },
                         { type: 'text' },
                         { type: 'text' },
                         { type: 'numeric' },
                         { type: 'text' },
                         {
                             type: 'dropdown',
                             source: ['ACTIVO', 'NO ACTIVO']
                         },
                        ],
                        rowHeaders: true,
                        comments: true,
                        
                        height: 770,
                        colWidths: [80, 500, 150, 150, 150, 150, 150],
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

                            var codigos = "";
                            for (var i = amount; i > 0; i--) {

                                var valorCelda = hot.getDataAtCell(index + i - 1, 0);

                                if (valorCelda != "" && valorCelda != null) {
                                    eliminarGenerador(valorCelda);
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
                error: function () {
                    mostrarError();
                }
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            mostrarError();
            console.log('Error:', jqXHR);
        }
    });
}

eliminarGenerador = function (id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'relaciondelete',
            data: {
                idRelacion: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'El registro se eliminó correctamente.');
                    consultar();
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

validarDetalle = function (datosDetalle) {
    var mensaje = "<ul>";
    var flag = true;
    var datos = datosDetalle;
  
    for (i = 0; i < datos.length; i++) {

        if (datos[i][0] != '') {
            if (datos[i][1] != '' && datos[i][6] != '') {

                if ((datos[i][2] != '' && (datos[i][3] != '')) || (datos[i][4] != '' && (datos[i][5] != ''))) {                    
                }
                else {
                    mensaje = mensaje + "<li>Complete los datos de la fila:" + (i+1) + "</li>";
                    flag = false;
                }

                if(!(datos[i][6] == 'ACTIVO' || datos[i][6] == 'NO ACTIVO'))
                {
                    mensaje = mensaje + "<li>Revise el campo estado de la fila:" + (i + 1) + "</li>";
                    flag = false;
                }
            }
            else {
                mensaje = mensaje + "<li>Complete los datos de la fila:" + (i + 1) + "</li>";
                flag = false;
            }
        }
        else {
            if (datos[i][1] != '') {
                if (datos[i][6] != '') {

                    if ((datos[i][2] != '' && (datos[i][3] != '')) || (datos[i][4] != '' && (datos[i][5] != ''))) {
                    }
                    else {
                        mensaje = mensaje + "<li>Complete los datos de la fila:" + (i + 1) + "</li>";
                        flag = false;
                    }

                    if (!(datos[i][6] == 'ACTIVO' || datos[i][6] == 'NO ACTIVO')) {
                        mensaje = mensaje + "<li>Revise el campo estado de la fila:" + (i + 1) + "</li>";
                        flag = false;
                    }
                }
                else {
                    mensaje = mensaje + "<li>Complete los datos de la fila:" + (i + 1) + "</li>";
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
    var mensajeDetalle = validarDetalle(datos);

    eliminarAlerta();

    if (mensajeDetalle == "") {

        //graba detalle
        var error = "";
        var mensaje = "<ul>";
        var flag = true;

        $.ajax({
            type: 'POST',
            url: controlador + "grabarlistglobal",
            dataType: 'json',
            data: {
                dataExcel: $('#hfDatos').val(),
            },
            success: function (result) {
                if (result != "-1") {
                    mostrarExito();
                    document.location.href = controlador + "relacionlistglobal";
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

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}