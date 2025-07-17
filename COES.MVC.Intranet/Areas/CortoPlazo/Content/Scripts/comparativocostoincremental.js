var controlador = siteRoot + 'cortoplazo/comparativo/';

$(function () {

    $('#txtFecha').Zebra_DatePicker({
        direction: false,
        onSelect: function (date) {
            cargarFiltros();
        }
    });

    $('#btnConsultar').on('click', function () {
        consultar('N');
    });

    $('#btnExportar').on('click', function () {
        consultar('S');
    });

    cargarFiltros();

});

cargarFiltros = function () {
    $('#cbHora').get(0).options.length = 0;
    $('#cbHora').get(0).options[0] = new Option("--SELECCIONE--", "-1");
    $('#cbEquipo').get(0).options.length = 0;
    $('#cbEquipo').get(0).options[0] = new Option("--SELECCIONE--", "-1");

    if ($('#txtFecha').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerfiltrosci',
            data: {
                fecha: $('#txtFecha').val()
            },
            dataType: 'json',
            success: function (result) {
                $.each(result.ListaPeriodos, function (i, item) {
                    $('#cbHora').get(0).options[$('#cbHora').get(0).options.length] = new Option(item.FechaProceso, item.Cmgncodi);
                });
                
                $.each(result.ListaEquipos, function (i, item) {
                    $('#cbEquipo').get(0).options[$('#cbEquipo').get(0).options.length] = new Option(item.Equinomb, item.Equicodi);
                });
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
};

consultar = function (option) {

    var mensaje = validacion();

    if (mensaje == '') {

        $.ajax({
            type: 'POST',
            url: controlador + 'comparativocostosincrementales',
            data: {
                fecha: $('#txtFecha').val(),
                umbral: $('#txtUmbral').val(),
                equipo: $('#cbEquipo').val(),
                hora: $('#cbHora').val(),
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
                        pintarTabla(result.ListaCostoIncremental);
                        $('#mensaje').removeClass();
                        $('#mensaje').html('');
                    }
                    else {
                        document.location.href = controlador + 'descargarcomparativocostosincrementales?fecha=' + $('#txtFecha').val();
                    }
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

    var html = '<table class="pretty tabla-adicional" id="tabla">';
    html = html + ' <thead>';
    html = html + '     <tr>';
    html = html + '         <th rowspan="2">Periodo</th>';
    html = html + '         <th rowspan="2">Unidad de Generación</th>';
    html = html + '         <th colspan="3">Tramo 01</th>';
    html = html + '         <th colspan="3">Tramo 02</th>';
    html = html + '     </tr>';
    html = html + '     <tr>';
    html = html + '         <th>CI GAMS</th>';
    html = html + '         <th>CI SGOCOES</th>';
    html = html + '         <th>DIFERENCIA</th>';
    html = html + '         <th>CI GAMS</th>';
    html = html + '         <th>CI SGOCOES</th>';
    html = html + '         <th>DIFERENCIA</th>';

    html = html + '     </tr>';
    html = html + ' </thead>';
    html = html + ' <tbody>';   
    var index = 0;
    for (var i in result) {

        var style = '';
        var style1 = '';
        var style2 = '';
        
        style1 = style;
        style2 = style;

        if (result[i].IndDiferencia1 == 'S') {
            style1 = 'background-color: #ffb4b4';
        }

        if (result[i].IndDiferencia2 == 'S') {
            style2 = 'background-color: #ffb4b4';
        }
        
        html = html + '     <tr>';

        html = html + '         <td style="text-align:center;' + style + '">' + result[i].Hora + '</td>';
        html = html + '         <td style="text-align:center;' + style + '">' + result[i].Unidad + '</td>';
        html = html + '         <td style="text-align:center;' + style + '">' + $.number(result[i].Ci1Gams, 3, '.', '') + '</td>';
        html = html + '         <td style="text-align:center;' + style + '">' + $.number(result[i].Ci1Sicoes, 3, '.', '') + '</td>';
        html = html + '         <td style="text-align:center;' + style1 + '">' + $.number(result[i].Diferencia1, 3, '.', '')  + '</td>';
        html = html + '         <td style="text-align:center;' + style + '">' + $.number(result[i].Ci2Gams, 3, '.', '') + '</td>';
        html = html + '         <td style="text-align:center;' + style + '">' + $.number(result[i].Ci2Sicoes, 3, '.', '')  + '</td>';
        html = html + '         <td style="text-align:center;' + style2 + '">' + $.number(result[i].Diferencia2, 3, '.', '') + '</td>';

        html = html + '     </tr>';
        index++;
    }

    html = html + ' </tbody>';
    html = html + '</table>';

    $('#contentTabla').html(html);

    $('#tabla').dataTable({
        "iDisplayLength": 100
    });
};

descargarResultado = function (correlativo) {

};

validacion = function () {
    var mensaje = "<ul>";
    var flag = true;

    if ($('#txtFecha').val() == "") {
        mensaje = mensaje + "<li>Por favor seleccione una fecha.</li>";
        flag = false;
    }

    if ($('#txtUmbral').val() == "") {
        mensaje = mensaje + "<li>Ingrese umbral de costos incrementales.</li>";
        flag = false;
    }
    else {
        if (validarNumero($('#txtUmbral').val())) {
            var umbral = parseFloat($('#txtUmbral').val());

            if (umbral <= 0) {
                mensaje = mensaje + "<li>El umbral debe ser un número mayor a cero.</li>";
                flag = false;
            }
        }
        else {
            mensaje = mensaje + "<li>El umbral debe ser un número.</li>";
            flag = false;
        }
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

function validarNumero(texto) {
    return /^-?[\d.]+(?:e-?\d+)?$/.test(texto);
};