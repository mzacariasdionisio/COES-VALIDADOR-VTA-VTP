var controlador = siteRoot + 'Equipamiento/FTReporte/';

$(function () {

    $('#cbEmpresaAmpliaciones').multipleSelect({
        width: '250px',
        filter: true,
    });

    $('#cbEmpresaAmpliaciones').multipleSelect('checkAll');  

    $('#FechaDesdeAmpliaciones').Zebra_DatePicker({
        format: "d/m/Y",
        pair: $('#FechaHastaAmpliaciones'),
        direction: false,
    });

    $('#FechaHastaAmpliaciones').Zebra_DatePicker({
        format: "d/m/Y",
        pair: $('#FechaDesdeAmpliaciones'),
        direction: true,
    });

    $('#btnBuscarAmpliaciones').click(function () {
        buscarEnvioAmpliaciones();
    });

    $('#btnExpotarAmpliaciones').click(function () {
        exportarAmpliaciones();
    });

    buscarEnvioAmpliaciones();
});

function buscarEnvioAmpliaciones() {
    

    limpiarBarraMensaje("mensaje");
    var filtro = datosFiltroAmpliaciones();
    var msg = validarDatosFiltroAmpliaciones(filtro);
    var idEmpresa = filtro.todos ? "-1" : $('#hfEmpresaAmpliaciones').val();

    if (msg == "") {
        


        $.ajax({
            type: 'POST',
            url: controlador + "ListarReporteAmpliacion",
            dataType: 'json',
            data: {
                empresas: idEmpresa,
                etapas: filtro.etapa,
                finicios: filtro.finicio,
                ffins: filtro.ffin
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {

                    var htmlEnvios = dibujarTablaReporte(evt.ListadoEnvios);
                    $("#listadoEnvios").html(htmlEnvios);
                    var tamAnchoh = parseInt($('.header').width());
                    var tamAncho = parseInt($('#mainLayout').width());
                    var tamA = tamAnchoh - 240;
                    $('#listadoGeneralAmpliaciones').css("width", tamA + "px");
                    $('#listadoGeneralAmpliaciones').css("height", "430px");
                    $('#listadoGeneralAmpliaciones').css("overflow", "auto");

                } else {
                    mostrarMensaje('mensaje', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'error', msg);
    }
}


function datosFiltroAmpliaciones() {
    var filtro = {};

    var empresa = $('#cbEmpresaAmpliaciones').multipleSelect('getSelects');
    var etapa = parseInt($('#cbEtapaAmpliaciones').val()) || 0;

    var finicio = $('#FechaDesdeAmpliaciones').val();
    var ffin = $('#FechaHastaAmpliaciones').val();
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    $('#hfEmpresaAmpliaciones').val(empresa);

    //verifico si esta seleccionado TODOS
    var Todos = false;
    var lstSel = [];
    $("#CajaPrincipalAmpliaciones input:checkbox[name=selectAllIdEmpresaAmpliaciones]:checked").each(function () {
        var textoFiltrado = $('#CajaPrincipalAmpliaciones .ms-search input').val();
        if (textoFiltrado == "")
            lstSel.push($(this));
    });
    if (lstSel.length > 0)
        Todos = true;


    filtro.empresa = empresa;
    filtro.etapa = etapa;
    filtro.finicio = finicio;
    filtro.ffin = ffin;
    filtro.todos = Todos;

    return filtro;
}

function validarDatosFiltroAmpliaciones(datos) {
    var msj = "";

    if (datos.empresa.length == 0) {
        msj += "<p>Debe escoger una empresa correcta.</p>";
    }

    if (datos.etapa == 0) {
        msj += "<p>Debe escoger una etapa correcta.</p>";
    } else {
        if (datos.etapa < -1 || datos.etapa > 4) {
            msj += "<p>Debe escoger una etapa correcta.</p>";
        }
    }

    if (datos.finicio == "") {
        if (datos.ffin == "") {
            msj += "<p>Debe escoger un rango inicial y final correctos.</p>";
        } else {
            msj += "<p>Debe escoger un rango inicial correcto.</p>";
        }
    } else {
        if (datos.ffin == "") {
            msj += "<p>Debe escoger un rango final correcto.</p>";
        } else {
            if (convertirFecha(datos.finicio) > convertirFecha(datos.ffin)) {
                msj += "<p>Debe escoger un rango correcto, la fecha final debe ser posterior o igual a la fecha inicial.</p>";
            }

        }
    }

    return msj;
}

function dibujarTablaReporte(lista) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaEnviosAmpliaciones" style="">
       <thead>
           <tr>
               <th >Código<br> del Envío</th>
               <th >Fecha de Solicitud</th>
               <th >Empresa</th> 
               <th >Etapa</th>
               <th >Fin de Plazo</th>
               <th >Fecha de Ampliación</th>
               <th >Usuario Ampliación</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];
        var rowspan = item.NumeroAmpliaciones;
        cadena += `
                <tr>
                    <td style="white-space: inherit;" rowspan="${rowspan}">${item.Ftenvcodi}</td>
                    <td style="white-space: inherit;" rowspan="${rowspan}">${item.FtenvfecsolicitudDesc}</td>
                    <td style="white-space: inherit;" rowspan="${rowspan}">${item.Emprnomb}</td>
                    <td style="white-space: inherit;" rowspan="${rowspan}">${item.Ftetnombre}</td>
        `;

        $.each(item.ListaLog, function (index, object) {

            cadena += `
                <td style="white-space: inherit;">${object.FtelogfecampliacionDesc}</td>
                <td style="white-space: inherit;">${object.FtelogfeccreacionDesc}</td>
                <td style="white-space: inherit;">${object.Ftelogusucreacion}</td>
            `;

            cadena += `</tr>`;
        });

    }
    cadena += `                   
        </tbody >
    </table >
    `;

    return cadena;
}

function exportarAmpliaciones() {
    limpiarBarraMensaje("mensaje");
    var filtro = datosFiltroAmpliaciones();
    var msg = validarDatosFiltroAmpliaciones(filtro);
    var idEmpresa = filtro.todos ? "-1" : $('#hfEmpresaAmpliaciones').val();    

    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'GenerarArchivoReporte',
            data: {
                empresas: idEmpresa,
                etapas: filtro.etapa,
                finicios: filtro.finicio,
                ffins: filtro.ffin
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    window.location = controlador + "Exportar?file_name=" + evt.Resultado;
                } else {
                    mostrarMensaje('mensaje', 'error', 'Error: ' + evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    } else {
        mostrarMensaje('mensaje', 'error', msg);
    }
}


function convertirFecha(fecha) {    //PARA FECHAS TIPO “30/12/2023”
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