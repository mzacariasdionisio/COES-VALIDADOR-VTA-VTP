var controlador = siteRoot + 'cortoplazo/equivalenciamodop/';
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
    document.location.href = controlador + "";
}

consultar = function () {
    var container = document.getElementById('contenedor');
    $(container).html("");

    cargarGrilla();
}


cargarGrilla = function () {


    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerDataEquivalencia',
        datatype: 'json',
        success: function (result) {
            
            var container = document.getElementById('contenedor');
            var data = result.Datos;

            var hotOptions = {
                data: data,
                colHeaders: true,
                colHeaders: ['N°', 'Cod', 'Modo', 'Nombre NCP'],
                columns: [
                 { type: 'numeric', readOnly: true },
                 { type: 'numeric', readOnly: true },
                 { type: 'text', readOnly: true },
                 { type: 'text' }
                ],
                rowHeaders: true,
                comments: true,

                height: 770,
                colWidths: [80, 80, 300, 150],
                cells: function (row, col, prop) {

                    var cellProperties = {};

                    return cellProperties;
                },
                afterLoadData: function () {
                    this.render();
                }
            };

            if (hot != null) {
                hot.destroy();
            }
            hot = new Handsontable(container, hotOptions);

        },
        error: function () {
            mostrarError();
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
        /*
        if (datos[i][1] != '') {
            if (datos[i][3] == '') {
                mensaje = mensaje + "<li>Complete los datos de la fila:" + (i + 1) + "</li>";
                flag = false;
            }
        }*/

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
                    document.location.href = controlador + "listaglobal";
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