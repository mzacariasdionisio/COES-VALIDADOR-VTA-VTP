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
        $('#cbEstado').val('ACTIVO');
        consultar();
    });

    
});

cancelar = function () {
    document.location.href = controlador + "linea";
}
consultar = function () {

    if (hot != null) {
        hot.destroy();
    }

    var empresa = $('#cbEmpresa').val();
    var estado = $('#cbEstado').val();
    var grupo = $('#cbGrupoFiltro').val();
    var familia = $('#cbFamilia').val();    

    var container = document.getElementById('contenedor');
    hot = null;
    $(container).html("");    
    cargarGrilla(empresa, estado, grupo, familia);
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

function render_vertical_align(ht, row, col) {
    $(ht.instance.getCell(row, col)).css(
    {
        "vertical-align": "middle"
    });
}

cargarGrilla = function (empresa, estado, grupo, familia) {
        
    var longTrafo = (familia==10?110:1);//((trafo == 2) ? 0 : 110);    

    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerDataLineaTrafoGrupo',
        datatype: 'json',
        data: {
            empresa: empresa,
            estado: estado,
            grupo: grupo,
            familia: familia
        },
        success: function (result) {

            var data = result.Datos;
            var dataLineaEquipoPrevio = result.ListLineaEquipo;            
            var dataListaGrupoTrafo2d = result.ListTrafo2dEquipo;
            var dataListaGrupoTrafo3d = result.ListTrafo3dEquipo;
            var dataListaGrupoLinea = result.ListConjuntoLinea;
            var dataEquipo;

            switch(familia)
            {
                case "9":                    
                    dataEquipo = dataListaGrupoTrafo2d;
                    break;
                case "10":                    
                    dataEquipo = dataListaGrupoTrafo3d;
                    break;
                default:                    
                    dataEquipo = dataLineaEquipoPrevio;
                    break;
            }
            
            arregloEquipo = [];          
            for (var j in dataEquipo)
            {                
                arregloEquipo.push({ id: dataEquipo[j].Equicodi, text: dataEquipo[j].Equinomb });                
            }
                       
            var container = document.getElementById('contenedor');

            var data = result.Datos;

            var hotOptions = {
                data: data,
                colHeaders: true,
                colHeaders: ['N°', 'Equipo', 'Barra (1)', 'Barra (2)', 'Barra (3)',
                             'ID', 'Código NCP', 'Nombre NCP', 'Grupo', 'Estado'],
                columns: [
                 { type: 'numeric', readOnly: true },
                 {},
                 { type: 'text' },
                 { type: 'text' },
                 { type: 'text' },

                 { type: 'text' },
                 { type: 'numeric' },
                 { type: 'text' },
                 {
                     type: 'dropdown',
                     source: dataListaGrupoLinea
                 },
                 {
                     type: 'dropdown',
                     source: ['ACTIVO', 'INACTIVO']
                 },

                ],
                rowHeaders: true,
                comments: true,
                
                height: 770,
                colWidths: [60, 450, 120, 120, longTrafo,
                            120, 120, 120, 1, 120],
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
                            eliminarLinea(valorCelda);
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
            alert(jqXHR);
            mostrarError();
            console.log('Error:', jqXHR);
        }
    });
}

eliminarLinea = function (id) {
    if (confirm('¿Está seguro de eliminar este registro?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'lineadelete',
            data: {
                idLinea: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    consultar();
                    mostrarMensaje('mensaje', 'exito', 'La operación se realizó correctamente.');
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

    var cad1 = "";
    for (i = 0; i < datos.length; i++) {

        if (datos[i][1] == '') {
            break;
        }
        else {
            if ((datos[i][9].localeCompare('ACTIVO') != 0) && (datos[i][9].localeCompare('INACTIVO')) != 0) {
                mensaje = "Revisar Estado";
                flag = false;
            }
        }
    }

    $('#hfDatos').val(datos);
    if (flag) mensaje = "";
    return mensaje;
}

grabar = function () {
     
    var datos = hot.getData();
    var mensajeDetalle = validarDetalle(datos);
    eliminarAlerta();

    if (mensajeDetalle == "") {      
        var error = "";
        var mensaje = "<ul>";
        var flag = true;

        $.ajax({
            type: 'POST',
            url: controlador + "grabarlineaglobal",
            dataType: 'json',
            data: {
                dataExcel: $('#hfDatos').val()
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

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}