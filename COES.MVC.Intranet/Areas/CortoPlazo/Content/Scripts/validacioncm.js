var controlador = siteRoot + 'CortoPlazo/ValidacionCM/';


$(function () {

    $('#txtFecha').Zebra_DatePicker({
        direction: false
    });
      
    $('#btnConsultar').on('click', function () {
        consultar("N");
    });

    $('#btnExportar').on('click', function () {
        consultar("S");
    });
   
});

function consultar(option) {
    var mensaje = validacion();

    if (mensaje == '') {

        $.ajax({
            type: 'POST',
            url: controlador + 'ListarValidaciones',
            data: {
                fecha: $('#txtFecha').val(),
                option: option
            },
            dataType: 'json',
            success: function (result) {
                if (result.Resultado == 1 || result.Resultado == 2) {

                    $('#mensaje').removeClass();
                    $('#mensaje').html('');

                    if (result.Resultado == 2) {
                        mostrarMensaje('mensaje', 'exito', 'No se encontraron errores de validación en la fecha seleccionada.');
                        $('#contentTabla').html('');
                    }
                    else if (result.Resultado == 1) {

                        if (option == 'N') {
                            pintarTabla(result.ListaDetalle);                            
                        }
                        else {
                            document.location.href = controlador + 'DescargarValidacionCM?fecha=' + $('#txtFecha').val();
                        }
                       
                    }                    
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
        mostrarMensaje('mensaje', 'alert', mensaje);        
        $('#contentTabla').html('');
    }    
};

pintarTabla = function (result) {
    var html = '<table class="pretty tabla-adicional">';
    html = html + ' <thead>';
    html = html + '     <tr>';
    html = html + '         <th colspan="4">Periodo</th>';
    html = html + '         <th rowspan="2">Mensaje</th>';
    html = html + '     </tr>';
    html = html + '     <tr>';   
    html = html + '         <th>Hora</th>';
    html = html + '         <th>Fecha Ejec.</th>';
    html = html + '         <th>Est.</th>';
    html = html + '         <th>PDO/RDO</th>';   
    html = html + '     </tr>';
    html = html + ' </thead>';
    html = html + ' <tbody>';
    var index = 0;
    for (var i in result) {
        var mensaje = "<ul>";
        if (result[i].MensajePeriodo != null) {
            mensaje = mensaje + "<li>" + result[i].MensajePeriodo + "</li>";
            result[i].FechaEjecucion = '';
            result[i].Estimador = '';
            result[i].Programa = '';
        }
        else {
            if (result[i].MensajeTopologia != null) {
                mensaje = mensaje + "<li>" + result[i].MensajeTopologia + "</li>";
            }
            if (result[i].MensajeArhivo != null) {
                mensaje = mensaje + "<li>" + result[i].MensajeArhivo + "</li>";               
                mensaje = mensaje + "   <ul>";

                for (var j in result[i].Archivos) {
                    mensaje = mensaje + "<li>" + result[i].Archivos[j] + "</li>";                 
                }

                mensaje = mensaje + "   </ul>";
                
            }
        }
        mensaje = mensaje + "</ul>";
        


        var style = '';
        if (index % 2 == 0) style = 'background-color: #f2f5f7';

        html = html + '<tr>';
        html = html + '         <td style="text-align:center;' + style + '">' + result[i].Hora + '</td>';
        html = html + '         <td style="text-align:center;' + style + '">' + result[i].FechaEjecucion + '</td>';
        html = html + '         <td style="text-align:center;' + style + '">' + result[i].Estimador + '</td>';
        html = html + '         <td style="text-align:center;' + style + '">' + result[i].Programa + '</td>';
        html = html + '         <td style="text-align:left;' + style + '">' + mensaje + '</td>';
        html = html + '</tr>';

        index++;
    }

    html = html + ' </tbody>';
    html = html + '</table>';

    $('#contentTabla').html(html);
}

validacion = function () {
    var mensaje = "<ul>";
    var flag = true;

    if ($('#txtFecha').val() == "") {
        mensaje = mensaje + "<li>Por favor seleccione una fecha.</li>";
        flag = false;
    }


    mensaje = mensaje + "</ul>";

    if (flag) mensaje = "";

    return mensaje;
};

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

getFecha = function (date) {
    var parts = date.split("/");
    var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date.getTime();
};
