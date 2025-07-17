var controlador = siteRoot + 'Combustibles/reporteGas/';

var IMG_DESCARGAR_EXCEL = '<img src="' + siteRoot + 'Content/Images/Document/raricon.png" alt="Descargar" width="19" height="19" style="">';
$(function () {

    $('#cbEmpresa').multipleSelect({
        width: '250px',
        filter: true,
    });

    $('#cbEmpresa').multipleSelect('checkAll');

    $('#FechaDesde').Zebra_DatePicker({
        format: 'd/m/Y',
        pair: $('#FechaHasta'),
        direction: -1,
    });

    $('#FechaHasta').Zebra_DatePicker({
        format: 'd/m/Y',
        pair: $('#FechaDesde'),
        direction: 1,
    });

    //$('#FechaConsulta').Zebra_DatePicker({
    //});

    $('#btnBuscar').click(function () {
        buscarEnvio();
    });

    $('#btnExpotar').click(function () {
        exportar();
    });

    buscarEnvio();
});

function buscarEnvio()
{
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);

    var msj = validarDatos();
    if (msj == "") {
        $.ajax({
            type: "POST",
            url: controlador + "ListarReporteEnvios",
            dataType: 'json',
            data: {
                empresas: $('#hfEmpresa').val(),
                finicios: $('#FechaDesde').val(),
                ffins: $('#FechaHasta').val()
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    limpiarBarraMensaje("mensaje");
                    var htmlEnvios = dibujarTablaReporte(evt.ListadoEnvios);
                    $("#listadoEnvios").html(htmlEnvios);
                    //$('#listadoEnvios').css("width", $('#mainLayout').width() + "px");
                    $('#tablaEnvios').dataTable({
                        "scrollY": 500,
                        "scrollX": true,
                        "sDom": 't',
                        "ordering": false,
                        "iDisplayLength": 50
                    });


                } else {
                    mostrarMensaje('mensaje', 'error', evt.Mensaje);
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', msj);
    }
}

function dibujarTablaReporte(lista) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaEnvios">
       <thead>
           <tr>
               <th>Código solicitud</th>
               <th>Fecha solicitud</th>
               <th>Nro envio</th>
               <th>Empresa</th>
               <th>Central termoeléctrica</th>
               <th>Estado</th>
               <th>Tipo Central</th>
               <th>Mes-Año</th>
               <th>Usuario envío</th>
               <th>Fecha - Hora envío</th>
               <th>Formato 3 e Inf. sustentatorio</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];
        cadena += `
                <tr>
                <td>${item.Cbenvcodi}</td>
                <td>${item.CbenvfecsolicitudDesc}</td>
                <td>${item.NumVersion}</td>
                <td>${item.Emprnomb}</td>
                <td>${item.Equinomb}</td>
                <td>${item.EstadoDesc}</td>
                <td>${item.CbenvtipocentralDesc}</td>
                <td>${item.MesVigenciaDesc}</td>
                <td>${item.Cbenvususolicitud}</td>
                <td>${item.CbenvfeccreacionDesc}</td>
        `;
        if (item.Estenvcodi == 11 || item.Estenvcodi == 12) //
        {
            cadena += `
               <td style='width:60px;'>
               </td>
                `;
        }
        else {
            cadena += `
               <td style='width:60px;'>
                   <a title="Descargar" href="JavaScript:descargarFormato(${item.Cbenvcodi}, ${item.Cbvercodi});">${IMG_DESCARGAR_EXCEL}</a>
               </td>
        `;
        }
        cadena += `
           </tr >           
        `;
    }
    cadena += `                   
        </tbody >
    </table >
    `;

    return cadena;
}


function exportar() {
    //limpiarBarraMensaje("mensaje");

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);

    var msj = validarDatos();

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'GenerarReporteEnvios',
            data: {
                empresas: $('#hfEmpresa').val(),
                finicios: $('#FechaDesde').val(),
                ffins: $('#FechaHasta').val()
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    limpiarBarraMensaje("mensaje");
                    window.location = controlador + "Exportar?file_name=" + evt.Resultado;
                } else {
                    mostrarMensaje('mensaje', 'error', 'Error: ' + evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', msj);
    }
}

function descargarFormato(idEnvio , idVersion) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarFormato3eInfSusXVersion',
        data: {
            idEnvio: idEnvio,
            idVersion: idVersion
        },
        dataType: 'json',
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "ExportarZip?file_name=" + evt.Resultado;
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

//validar consulta
function validarDatos() {
    var validacion = "<ul>";
    var flag = true;

    if (convertirFecha($('#FechaHasta').val()) <= convertirFecha($('#FechaDesde').val())) {
        validacion = validacion + "<li>Fecha Hasta: debe ser mayor a la fecha inicio .</li>";//Campo Requerido
        flag = false;
    }

    validacion = validacion + "</ul>";

    if (flag == true) validacion = "";

    return validacion;
}

function convertirFecha(fecha) {
    var arrayFecha = fecha.split('/');
    var dia = arrayFecha[0];
    var mes = arrayFecha[1];
    var anio = arrayFecha[2];

    var salida = anio + mes + dia;
    return salida;
}

function mostrarMensaje(id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

}

function limpiarBarraMensaje(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
}