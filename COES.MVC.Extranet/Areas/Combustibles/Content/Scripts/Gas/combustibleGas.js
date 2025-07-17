var controlador = siteRoot + 'Combustibles/EnvioGas/';

const EXTRANET = 1;
const INTRANET = 2;

const EST_SOLICITUD = 1;
const EST_APROBADO = 3;
const EST_DESAPROBADO = 4;
const EST_OBSERVADO = 6;
const EST_SUBSANACION = 7;
const EST_CANCELADO = 8;
const EST_APROBADO_PARCIALMENTE = 10;
const EST_SOLICITUD_ASIGNACION = 11;
const EST_ASIGNADO = 12;

var IMG_EDITAR = '<img src="' + siteRoot + 'Content/Images/btn-edit.png" alt="Editar Envío" width="19" height="19" style="">';
var IMG_VER = '<img src="' + siteRoot + 'Content/Images/btn-open.png" alt="Ver Envío" width="19" height="19" style="">';
var IMG_AMPLIAR = '<img src="' + siteRoot + 'Content/Images/btn-add.png" alt="Ampliar Plazo" width="19" height="19" style="">';
var IMG_CARGO = '<img src="' + siteRoot + 'Content/Images/pegar.png" alt="Ver Cargo" width="19" height="19" style="">';
var IMG_DESCARGAR_EXCEL = '<img src="' + siteRoot + 'Content/Images/ExportExcel.png" alt="Descargar Detalle" width="19" height="19" style="">';
var IMG_DESCARGAR_PDF = '<img src="' + siteRoot + 'Content/Images/pdf.png" alt="Descargar Detalle" width="15" height="19" style="">';
var IMG_CANCELAR = '<img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="Cancelar Envío" width="15" height="19" style="">';
$(function () {

    $('#cbEmpresa').multipleSelect({
        width: '200px',
        filter: true,        
    });
    $('#cbEmpresa').multipleSelect('checkAll');

    $('#FechaDesde').Zebra_DatePicker({
        format: "m-Y",
        pair: $('#FechaHasta'),
        direction: -1,
    });

    $('#FechaHasta').Zebra_DatePicker({
        format: "m-Y",
        pair: $('#FechaDesde'),
        direction: 1,
    });

    $('#btnBuscar').click(function () {
        buscarEnvio();
    });

    $('#btnExpotar').click(function () {
        exportar();
    });

    //Nuevo
    $('#btnNuevoEnvio').click(function () {
        var flagExist = parseInt($("#hfFlagCentralExistente").val()) || 0;
        var flagNuevo = parseInt($("#hfFlagCentralNuevo").val()) || 0;

        if (flagExist == 1 && flagNuevo == 1) {
            abrirPopup("popupSeleccionarCentral");
        } else {
            if (flagExist == 1)
                redireccionarFormulario(0,"E");
            if (flagNuevo == 1)
                abrirPopup("popupSeleccionarCentralNueva"); //redireccionarFormulario("N");
        }
    });
    $('#btnAceptarSeleccionarCentral').click(function () {
        var tipoCentral = $("input[name=rbTipoCentral]:checked").val();
        if(tipoCentral == "E")
            redireccionarFormulario(0,tipoCentral, '');
        else
            abrirPopup("popupSeleccionarCentralNueva");
    });
    $('#btnAceptarSeleccionarCentralNuevo').click(function () {
        var tipoOpcion = $("input[name=rbTipoOpcion]:checked").val();
        redireccionarFormulario(0,'N', tipoOpcion);
    });

    //cancelacion
    $('#btnAceptarCancelar').click(function () {
        realizarCancelarEnvio();
    });
    $('#btnCerrarCancelar').click(function () {
        cerrarPopup('popupCancelar');
    });

    buscarEnvio(); // muestra el listado de todos los envios de todas las empresas del tipo de combustible
});

////////////////////////////////////////////////
// Ir a Formulario
////////////////////////////////////////////////
function redireccionarFormulario(idEnvio, tipoCentral, tipoOpcion) {
    var form_url = controlador + "EnvioCombustibleGas";

    $("#frmEnvio").attr("action", form_url);
    $("#hdIdEnvioForm").val(idEnvio);
    $("#hdTipoCentralForm").val(tipoCentral);
    $("#hdTipoOpcionForm").val(tipoOpcion);

    $("#frmEnvio").submit();
}

////////////////////////////////////////////////
// Reporte Envio
////////////////////////////////////////////////
function buscarEnvio(idEstado) {
    idEstado = parseInt(idEstado) || 0;
    if (idEstado > 0) {
        $("#hdIdEstado").val(idEstado);
    }
    idEstado = $("#hdIdEstado").val();

    mostrarBloqueEnvios(1, idEstado);
}

function mostrarBloqueEnvios(nroPagina, idEstado) {
    limpiarBarraMensaje("mensaje");

    var filtro = datosFiltro(idEstado);
    var msg = validarDatosFiltroEnvios(filtro);
    
    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + "bloqueEnvios",
            dataType: 'json',
            data: {
                empresas: $('#hfEmpresa').val(),
                idEstado: filtro.estado,
                finicios: filtro.finicio,
                ffins: filtro.ffin
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    var html = "<h3>Carpetas</h3>";
                    html += evt.HtmlCarpeta;
                    $("#div_carpetas").html(html);

                    var htmlEnvios = dibujarTablaReporte(evt.ListadoEnvios, idEstado, EXTRANET);
                    $("#reporte").html(htmlEnvios);
                    $('#listado').css("width", $('#mainLayout').width() + "px");
                    $('#tablaEnvios').dataTable({
                        "scrollY": 430,
                        "scrollX": true,
                        "sDom": 't',
                        "ordering": false,
                        "iDisplayLength": -1
                    });


                } else {
                    mostrarMensaje('mensaje', 'error', evt.Mensaje);
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


function validarDatosFiltroEnvios(datos) {
    var msj = "";

    if (datos.empresa.length == 0) {
        msj += "<p>Debe escoger una empresa correcta.</p>";
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
            if (datos.ffin < datos.finicio) {
                //msj += "<p>El rango inicial no debe ser mayor al rango final.</p>";
            }
        }
    }

    return msj;
}

function dibujarTablaReporte(lista, estado, interfaz) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaEnvios">
       <thead>
           <tr>
               <th style='width:60px;'>Acciones</th>
               <th>Código<br>del Envío</th>
               <th>Empresa</th>
               <th>Tipo de<br>Combustible</th>
               <th>Usuario</th>
               <th>Fecha de<br>Solicitud</th>
    `;
    if (estado == EST_SOLICITUD_ASIGNACION || estado == EST_ASIGNADO) // asignados
    {
        cadena += `
               <th>Fecha de<br>Asignación</th>
               <th>Costo Vigente<br>Desde</th>
        `;
    }
    if (estado == EST_DESAPROBADO) // desaprobados
    {
        cadena += `
               <th>Fecha de<br>desaprobación</th>
        `;
    }
    if (estado == EST_APROBADO_PARCIALMENTE) // aprobados parciales
    {
        cadena += `
               <th>Fecha de<br>aprobación parcial</th>
               <th>Mes de<br>vigencia</th>
        `;
    }
    if (estado == EST_APROBADO) // aprobados
    {
        cadena += `
               <th>Fecha de<br>aprobación</th>
               <th>Mes de<br>vigencia</th>
        `;
    }
    cadena += `
               <th>Actualización</th>
    `;
    if (estado == EST_APROBADO || estado == EST_DESAPROBADO || estado == EST_OBSERVADO || estado == EST_APROBADO_PARCIALMENTE || estado == EST_ASIGNADO) //
    {
        cadena += `
               <th style='width:30px;'>Ver<br>Cargo</th>
        `;
    }
    if (interfaz == INTRANET && estado != EST_ASIGNADO) //
    {
        cadena += `
               <th style='width:60px;'>Descargar<br>Detalles</th>
        `;
    }
    cadena += `
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];        

        cadena += `
            <tr>
                <td style='width:60px;'>
        `;

        if (interfaz == EXTRANET) {
            if (item.EsEditableExtranet) {
                cadena += `
                   <a title="Editar Envío" href="JavaScript:mostrarDetalle(${item.Cbenvcodi}, '${item.Cbenvtipocentral}', '${item.TipoOpcionAsignado}');">${IMG_EDITAR}</a>
            `;
            }
            else {
                cadena += `
                   <a title="Ver Envío" href="JavaScript:mostrarDetalle(${item.Cbenvcodi}, '${item.Cbenvtipocentral}', '${item.TipoOpcionAsignado}');">${IMG_VER}</a>
            `;
            }
            if (estado == EST_SOLICITUD) {
                cadena += `
                    <a title="Cancelar Envío" href="JavaScript:cancelarEnvio(${item.Cbenvcodi});">${IMG_CANCELAR}</a>
                `;
            }
        } else {

            if (item.EsEditableIntranet) {
                if (estado == EST_OBSERVADO || estado == EST_SUBSANACION) {
                    if (estado == EST_SUBSANACION) {
                        cadena += `
                           <a title="Editar Envío" href="JavaScript:mostrarDetalle(${item.Cbenvcodi},'${item.Cbenvtipocentral}');">${IMG_EDITAR}</a>
                        `;
                        cadena += `
                            <a title="Ampliar Plazo del Envío" href ="JavaScript:mostrarAmpliarPlazo(${item.Cbenvcodi});">${IMG_AMPLIAR}</a>
                        `;
                    }
                    else {
                        cadena += `
                            <a title="Ver Envío" href="JavaScript:mostrarDetalle(${item.Cbenvcodi},'${item.Cbenvtipocentral}');">${IMG_VER}</a>
                         `;
                    }
                }
                else {
                    cadena += `
                        <a title="Editar Envío" href="JavaScript:mostrarDetalle(${item.Cbenvcodi},'${item.Cbenvtipocentral}');">${IMG_EDITAR}</a>
                     `;
                }
            }
            else {
                cadena += `
                   <a title="Ver Envío" href="JavaScript:mostrarDetalle(${item.Cbenvcodi},'${item.Cbenvtipocentral}');">${IMG_VER}</a>
                `;
            }

        }

        cadena += `            
                </td>
                <td>${item.Cbenvcodi}</td>
                <td>${item.Emprnomb}</td>
                <td>${item.Fenergnomb}</td>
                <td>${item.Cbenvususolicitud}</td>
                <td>${item.CbenvfecsolicitudDesc}</td>
        `;
        if (estado == EST_SOLICITUD_ASIGNACION || estado == EST_ASIGNADO) // asignados
        {
            cadena += `
               <td>${item.FechaAsignacionDesc}</td>
               <td>${item.CostoVigenteDesdeDesc}</td>
        `;
        }
        if (estado == EST_DESAPROBADO) // desaprobados
        {
            cadena += `
               <td>${item.FechaDesaprobacionDesc}</td>
        `;
        }
        if (estado == EST_APROBADO_PARCIALMENTE) // aprobados parciales
        {
            cadena += `
               <td>${item.CbenvfecmodificacionDesc}</td>
               <td>${item.MesVigenciaDesc}</td>
        `;
        }
        if (estado == EST_APROBADO) // aprobados
        {
            cadena += `
               <td>${item.FechaAprobacionDesc} </td>
               <td>${item.MesVigenciaDesc}</td>
        `;
        }
        cadena += `
               <td>${item.CbenvfecmodificacionDesc}</td>
    `;
        if (estado == EST_APROBADO || estado == EST_DESAPROBADO || estado == EST_OBSERVADO || estado == EST_APROBADO_PARCIALMENTE || estado == EST_ASIGNADO) //
        {
            cadena += `
               <td style='width:30px;'>
                   <a href="JavaScript:verCargo(${item.Cbenvcodi},${estado});">${IMG_CARGO}</a>
               </td>
        `;
        }
        if (interfaz == INTRANET && estado != EST_ASIGNADO) //
        {
            cadena += `
               <td style='width:60px;'>
                   <a href="#">${IMG_DESCARGAR_EXCEL}</a>
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


function verCargo(idEnvio, estado) {
    limpiarBarraMensaje("mensaje");

    $.ajax({
        type: 'POST',
        url: controlador + 'listarCargo',
        data: {
            idEnvio: idEnvio,
            estado: estado
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                var htmlCargo = dibujarTablaCargo(evt.ListadoEnvioLog);
                $("#seccionCargos").html(htmlCargo);

                abrirPopup("popupCargo");
                $('#tablaCargos').dataTable({
                    "sDom": 't',
                    "ordering": false,
                });
            }
            else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function dibujarTablaCargo(listaEnviosLog) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono1" id="tablaCargos">
       <thead>
           <tr style="height: 20px;">
               
               <th>Usuario de Recepción</th>
               <th>Fecha de Recepción</th>
               <th>Usuario de Lectura</th>
               <th>Fecha de Lectura</th>
               <th>Exportar</th>    
            </tr>
        </thead>
        <tbody>
    `;

    for (key in listaEnviosLog) {
        var item = listaEnviosLog[key];
        cadena += `
            <tr>                
                <td style='word-break: break-word;'>${item.Logenvusurecepcion}</td>
                <td>${item.LogenvfecrecepcionDesc}</td>
                <td style='word-break: break-word;'>${item.Logenvusulectura}</td>
                <td>${item.LogenvfeclecturaDesc}</td>
                <td><a href="JavaScript:descargarCargo(${item.Cbenvcodi},${item.Estenvcodi});">${IMG_DESCARGAR_PDF}</a></td>
           </tr >           
        `;
    }

    cadena += `                   
        </tbody >
    </table >
    `;

    return cadena;
}

function descargarCargo(idEnvio, estado) {
    limpiarBarraMensaje("mensaje_popupCargos");
    limpiarBarraMensaje("mensaje");
  
    $.ajax({
        type: 'POST',
        url: controlador + 'exportarCargo',
        data: {
            idEnvio: idEnvio,
            estado: estado
        },
        dataType: 'json',
        success: function (evt) {

            if (evt.Resultado != "-1") {
                cerrarPopup('popupCargo');
                window.location = controlador + "Exportar?file_name=" + evt.Resultado;

            } else {
                mostrarMensaje('mensaje_popupCargos', 'error', 'Error: ' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje_popupCargos', 'error', 'Ha ocurrido un error.');
        }
    });

}

function datosFiltro(idEstado) {
    var filtro = {};

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var estado = idEstado;
    var finicio = $('#FechaDesde').val();
    var ffin = $('#FechaHasta').val();
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    $('#hfEstado').val(estado);

    filtro.empresa = empresa;
    filtro.estado = estado;
    filtro.finicio = finicio;
    filtro.ffin = ffin;

    return filtro;
}

function exportar() {
    limpiarBarraMensaje("mensaje");
    
    var filtro = datosFiltro($('#hfEstado').val());

    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarArchivoReporte',
        data: {
            empresas: $('#hfEmpresa').val(),
            idEstado: filtro.estado,
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
}

////////////////////////////////////////////////
// Mostrar Detalles del envio
////////////////////////////////////////////////
function mostrarDetalle(id, tipoCentral, tipoOpcion) {
    redireccionarFormulario(id, tipoCentral, tipoOpcion);
}

////////////////////////////////////////////////
// Cancelar Envio
////////////////////////////////////////////////
function cancelarEnvio(idEnvio) {
    $('#txtMotivo').val('');
    $("#hfIdCancelarEnvio").val(idEnvio);

    abrirPopup("popupCancelar");
}

function realizarCancelarEnvio() {
    var idEnvio = parseInt($("#hfIdCancelarEnvio").val()) || 0;
    var motivo = ($('#txtMotivo').val()).trim();
    if (motivo == '')
        alert("Debe ingresar motivo para realizar la cancelación.");
    else {
        if (confirm('¿Desea cancelar el envío?')) {
            $.ajax({
                type: 'POST',
                url: controlador + 'CancelarEnvio',
                dataType: 'json',
                data: {
                    idEnvio: idEnvio,
                    motivo: motivo
                },
                cache: false,
                success: function (evt) {
                    if (evt.Resultado != "-1") {                        
                        alert('La cancelación del envío fue ejecutada correctamente.');
                        cerrarPopup('popupCancelar');
                        buscarEnvio(EST_CANCELADO);
                    } else {
                        mostrarMensaje('mensaje', 'error', 'Error: ' + evt.Mensaje);
                    }

                },
                error: function (err) {                    
                    mostrarMensaje('mensaje', 'error', 'Error al cancelar el envío');
                }
            });
        }
    }
}

////////////////////////////////////////////////
// Util
////////////////////////////////////////////////
function cerrarPopup(id) {
    $("#" + id).bPopup().close()
}

function abrirPopup(contentPopup) {
    setTimeout(function () {
        $("#" + contentPopup).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
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