var controlador = siteRoot + 'cortoplazo/comparativo/';

$(function () {

    $('#txtFecha').Zebra_DatePicker({
        direction: false
    });

    $('#btnConsultar').on('click', function () {
        consultar('N');
    });

    $('#btnExportar').on('click', function () {
        consultar('S');
    });

});

consultar = function (option) {

    var mensaje = validacion();

    if (mensaje == '') {

        $.ajax({
            type: 'POST',
            url: controlador + 'comparativocongestiones',
            data: {
                fecha: $('#txtFecha').val(),               
                option: option
            },
            dataType: 'json',
            success: function (result) {

                if (option == 'N') {
                    $('#contentTabla').html('');
                }

                if (result.Resultado == 1) {
                    $('#mensaje').removeClass();
                    $('#mensaje').html('');

                    if (option == 'N') {
                        pintarTabla(result.ListaCongestion);
                        $('#mensaje').removeClass();
                        $('#mensaje').html('');
                    }
                    else {
                        document.location.href = controlador + 'descargarcomparativocongestiones?fecha=' + $('#txtFecha').val();
                    }
                }
                else if (result.Resultado == 2) {
                    mostrarMensaje('mensaje', 'alert', 'No se encontraron procesos de costos marginales para el día seleccionado.');
                }
                else if (result.Resultado == 3) {
                    mostrarMensaje('mensaje', 'alert', 'No existen diferencias en las congestiones del SGOCOES y Resultados GAMS..');
                }
                else if (result.Resultado == -1) {
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
        $('#contentGrafico').hide();
        $('#contentTabla').html('');
    }
}

pintarTabla = function (result) {

    var html = '<table class="pretty tabla-adicional">';
    html = html + ' <thead>';
    html = html + '     <tr>';
    html = html + '         <th colspan="5">Periodo</th>';
    html = html + '         <th colspan="3">Congestiones en SGOCOES y no en resultados GAMS</th>';
    html = html + '         <th colspan="4">Congestiones en resultados GAMS y no en SGOCOES</th>';
    html = html + '     </tr>';
    html = html + '     <tr>';
    html = html + '         <th></th>';
    html = html + '         <th>Hora</th>';
    html = html + '         <th>Fecha Ejec.</th>';
    html = html + '         <th>Est.</th>';
    html = html + '         <th>PDO/RDO</th>';
    html = html + '         <th>Equipo</th>';
    html = html + '         <th>Desde</th>';
    html = html + '         <th>Hasta</th>';
    html = html + '         <th>Enlace</th>';
    html = html + '         <th>Límite</th>';
    html = html + '         <th>Envío</th>';
    html = html + '         <th>Recepción</th>';
    html = html + '     </tr>';
    html = html + ' </thead>';
    html = html + ' <tbody>';

    for (var i in result) {

        var count = result[i].ListaDetalle.length;
        var index = 0;
        for (var j in result[i].ListaDetalle) {
            var style = '';
            if (i % 2 == 0) style = 'background-color: #f2f5f7';

            html = html + '     <tr>';

            if (index == 0) {
                html = html + '         <td style="text-align:center;' + style + '" rowspan="' + count + '"><a href="JavaScript:descargarResultado(' + result[i].Correlativo + ')"><img src="' + siteRoot + 'content/images/csv.png" /></a></td>';
                html = html + '         <td style="text-align:center;' + style + '" rowspan="' + count + '">' + result[i].Hora + '</td>';
                html = html + '         <td style="text-align:center;' + style + '" rowspan="' + count + '">' + result[i].FechaEjecucion + '</td>';
                html = html + '         <td style="text-align:center;' + style + '" rowspan="' + count + '">' + result[i].Estimador + '</td>';
                html = html + '         <td style="text-align:center;' + style + '" rowspan="' + count + '">' + result[i].Programa + '</td>';
            }

            var equiposistema = '';
            var horainicio = '';
            var horafin = '';
            var equipoproceso = '';
            var limite = '';
            var envio = '';
            var recepcion = '';
            if (result[i].ListaDetalle[j].EquipoSistema != null) equiposistema = result[i].ListaDetalle[j].EquipoSistema;
            if (result[i].ListaDetalle[j].HoraInicio != null) horainicio = result[i].ListaDetalle[j].HoraInicio;
            if (result[i].ListaDetalle[j].HoraFin != null) horafin = result[i].ListaDetalle[j].HoraFin;
            if (result[i].ListaDetalle[j].EquipoProceso != null) equipoproceso = result[i].ListaDetalle[j].EquipoProceso;
            if (result[i].ListaDetalle[j].Limite != null) limite = $.number(result[i].ListaDetalle[j].Limite, 2, '.', '');
            if (result[i].ListaDetalle[j].Envio != null) envio = $.number(result[i].ListaDetalle[j].Envio, 2, '.', '');
            if (result[i].ListaDetalle[j].Recepcion != null) recepcion = $.number(result[i].ListaDetalle[j].Recepcion, 2, '.', '');

            html = html + '         <td style="text-align:center;' + style + '">' + equiposistema + '</td>';
            html = html + '         <td style="text-align:center;' + style + '">' + horainicio + '</td>';
            html = html + '         <td style="text-align:center;' + style + '">' + horafin + '</td>';
            html = html + '         <td style="text-align:center;' + style + '">' + equipoproceso + '</td>';
            html = html + '         <td style="text-align:center;' + style + '">' + limite + '</td>';
            html = html + '         <td style="text-align:center;' + style + '">' + envio + '</td>';
            html = html + '         <td style="text-align:center;' + style + '">' + recepcion + '</td>';

            html = html + '     </tr>';

            index++;
        }

    }

    html = html + ' </tbody>';
    html = html + '</table>';

    $('#contentTabla').html(html);
};

descargarResultado = function (correlativo) {
    
    document.location.href = controlador + 'descargararchivoresultado?fecha=' + $('#txtFecha').val() + "&correlativo=" + correlativo;
};

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

