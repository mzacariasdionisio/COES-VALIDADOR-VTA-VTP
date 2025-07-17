var controlador = siteRoot + 'Combustibles/reporteGas/';

$(function () {

    $('#cbEmpresa').multipleSelect({
        width: '250px',
        filter: true,
    });

    $('#cbEmpresa').multipleSelect('checkAll');

    $('#FechaDesde').Zebra_DatePicker({
        format: "m-Y",
        //pair: $('#FechaHasta'),
        direction: -1,
    });

    $('#FechaHasta').Zebra_DatePicker({
        format: "m-Y",
        //pair: $('#FechaDesde'),
        direction: -1,
    });


    //$('#FechaConsulta').Zebra_DatePicker({
    //});

    $('#btnBuscar').click(function () {
        buscarEnvio();
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
            url: controlador + "ListarReporteAmpliacion",
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
                        "scrollY": 430,
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
               <th>Fecha de Solicitud</th>
               <th>Empresa</th> 
               <th>Tipo de Central</th>
               <th>Fecha de ampliación</th>
               <th>Último Usuario</th>
               <th>Fecha modificación</th>
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
                <td>${item.Emprnomb}</td>
                <td>${item.CbenvtipocentralDesc}</td>
                <td>${item.CbenvfecamplDesc}</td>
                <td>${item.Cbenvususolicitud}</td>
                <td>${item.CbenvfecmodificacionDesc}</td>
        `;

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

//validar consulta
function validarDatos() {
    var validacion = "<ul>";
    var flag = true;

    if ($('#FechaHasta').val() <= $('#FechaDesde').val()) {
        validacion = validacion + "<li>Fecha Hasta: debe ser mayor a la fecha inicio .</li>";//Campo Requerido
        flag = false;
    }

    validacion = validacion + "</ul>";

    if (flag == true) validacion = "";

    return validacion;
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