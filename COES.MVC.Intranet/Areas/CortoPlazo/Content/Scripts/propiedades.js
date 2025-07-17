var controlador = siteRoot + 'cortoplazo/propiedades/';
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
    document.location.href = controlador + "lista";
}

consultar = function () {
    var container = document.getElementById('contenedor');
    $(container).html("");

    cargarGrilla();
}


cargarGrilla = function () {

    
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerData',
        datatype: 'json',        
        data: {            
            famcodi: $('#cbFamilia').val()
        },
        success: function (result) {
            
            var container = document.getElementById('contenedor');
            var data = result.Datos;

            var hotOptions = {
                data: data,
                colHeaders: true,
                colHeaders: ['Codigo','Unidad/Modo', 'Toma de carga', 'Reducción de carga'],
                columns: [
                 { type: 'numeric', readOnly: true },
                 { type: 'text' },
                 { type: 'text' },
                 { type: 'text' }
                ],
                rowHeaders: true,
                comments: true,
                height: 770,
                colWidths: [50, 450, 150, 150],
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


validarDetalle = function (datosDetalle) {
    var mensaje = "<ul>";
    var flag = true;
    var datos = datosDetalle;
  
    for (i = 0; i < datos.length; i++) {        
        if (datos[i][2] != '') {
            if (isNaN(datos[i][2])) {
                mensaje = mensaje + "<li>Toma de carga inválido. Fila:" + (i + 1) +". Valor: "+datos[i][2]+"</li>";
                flag = false;
            }
        }

        if (datos[i][3] != '') {
            if (isNaN(datos[i][3])) {
                mensaje = mensaje + "<li>Reducción de carga inválido. Fila:" + (i + 1) + ". Valor: " + datos[i][3] + "</li>";
                flag = false;
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
            url: controlador + "grabar",
            dataType: 'json',
            data: {
                famcodi: $('#cbFamilia').val(),
                dataExcel: $('#hfDatos').val(),
            },
            success: function (result) {
                if (result != "-1") {
                    mostrarExito();
                    //document.location.href = controlador + "lista";
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