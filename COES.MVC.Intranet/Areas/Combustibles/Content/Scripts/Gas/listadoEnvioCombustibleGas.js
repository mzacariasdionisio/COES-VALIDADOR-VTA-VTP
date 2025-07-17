var controlador = siteRoot + 'Combustibles/GestionGas/';
var controladorReporte = siteRoot + 'Combustibles/reportegas/';

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
var IMG_DESCARGAR_EXCEL = '<img src="' + siteRoot + 'Content/Images/downloadExcel.png" alt="Descargar Detalle" width="19" height="19" style="">';
var IMG_DESCARGAR_PDF = '<img src="' + siteRoot + 'Content/Images/pdf.png" alt="Descargar Detalle" width="15" height="19" style="">';

$(function () {

    $('#cbEmpresa').multipleSelect({
        width: '360px',
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

    $('#btnReportes').click(function () {
        var url = siteRoot + 'Combustibles/ReporteGas/Index';
        window.open(url).focus();
    });

    $('#btnEnviarMensaje').click(function () {
        var url = siteRoot + 'Combustibles/ConfiguracionGas/IndexCorreo';
        window.open(url).focus();
    });

    $('#btnExpotar').click(function () {
        exportar();
    });


    $("#btnAmpliarPlazo").click(function () {
        grabarAmpliacion();
    });
   

    //Nuevo envio
    _nuevoEnvioForm();
    $("#btnPopupNuevoEnvio").click(function () {
        $("#hdIdEnvioTemporal").val(0);
        abrirPopup("popupNuevoEnvio");
    });
    $("#btnAceptarNuevoEnvio").click(function () {
        validarNuevoEnvio();
    });


    $('input[type=radio][name=radio_accion]').change(function () {
        $("#hdIdEnvioTemporal").val(0);

        var valorRadio = $('input[name=radio_accion]:checked').val();
        if (valorRadio == 'NE') { //nuevo envio
            $("#tr_check_primera_carga").show();
        } else {//subsanar
            $("#tr_generador").show();
            $("#tr_check_primera_carga").hide();
        }

        $("#email_generador_nuevo").val("");
        $('#check_primera_carga').prop('checked', false);
    });
    $('#check_primera_carga').change(function () {
        if (this.checked) {
            $("#tr_generador").hide();
        } else {
            $("#tr_generador").show();
        }
    });

    $('#btnAceptarSeleccionarCentral').click(function () {
        var tipoCentral = $("input[name=rbTipoCentral]:checked").val();
        var mesVigencia = $("#txtFechaVigNuevo").val();
        var idEmpresa = $("#cbEmpresaNuevo").val();
        var tipoAccionForm = $('input[name=radio_accion]:checked').val();
        var usuarioGenerador = $("#email_generador_nuevo").val();
        var esPrimeraCarga = $('#check_primera_carga').is(':checked') ? 1 : 0;
        var idEnvio = $("#hdIdEnvioTemporal").val();

        redireccionarFormulario(idEnvio, tipoCentral, idEmpresa, mesVigencia, tipoAccionForm, usuarioGenerador, esPrimeraCarga);
    });

    buscarEnvio(); // muestra el listado de todos los envios de todas las empresas del tipo de combustible
});

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

                    var htmlEnvios = dibujarTablaReporte(evt.ListadoEnvios, idEstado, INTRANET, evt.TienePermisoAdmin);
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

function dibujarTablaReporte(lista, estado, interfaz, esAdmin) {

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
    if (estado != EST_ASIGNADO) //
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

        if (item.EsEditableIntranet && esAdmin) {
            if (estado == EST_OBSERVADO || estado == EST_SUBSANACION) {
                if (estado == EST_SUBSANACION) {
                    cadena += `
                           <a title="Editar Envío" href="JavaScript:mostrarDetalle(${item.Cbenvcodi});">${IMG_EDITAR}</a>
                        `;
                    cadena += `
                            <a title="Ampliar Plazo del Envío" href ="JavaScript:mostrarAmpliarPlazo(${item.Cbenvcodi});">${IMG_AMPLIAR}</a>
                        `;
                }
                else {
                    cadena += `
                            <a title="Ver Envío" href="JavaScript:mostrarDetalle(${item.Cbenvcodi});">${IMG_VER}</a>
                         `;
                }
            }
            else {
                cadena += `
                        <a title="Editar Envío" href="JavaScript:mostrarDetalle(${item.Cbenvcodi});">${IMG_EDITAR}</a>
                     `;
            }
        }
        else {
            cadena += `
                   <a title="Ver Envío" href="JavaScript:mostrarDetalle(${item.Cbenvcodi});">${IMG_VER}</a>
                `;
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
        if (estado != EST_ASIGNADO) //
        {
            cadena += `
               <td style='width:60px;'>
                   <a href="JavaScript:descargarF3InfSust(${item.Cbenvcodi});">${IMG_DESCARGAR_EXCEL}</a>
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

function descargarF3InfSust(idEnvio) {

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarFormato',
        data: {
            idEnvio: idEnvio
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
                <td>${item.Logenvusurecepcion}</td>
                <td>${item.LogenvfecrecepcionDesc}</td>
                <td>${item.Logenvusulectura}</td>
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
function mostrarDetalle(id) {
    window.location = controlador + "EnvioCombustible?idEnvio=" + id;
}


////////////////////////////////////////////////
// Ampliar plazo del envio
////////////////////////////////////////////////
function mostrarAmpliarPlazo(idEnvio) {
    limpiarBarraMensaje("mensaje");
    limpiarBarraMensaje("mensaje_popupAmpliar");

    $("#hf_amplIdEnvio").val(-1);
    $("#ampl_enviocodi").html("");
    $("#ampl_fechaSolicitud").html("");
    $("#ampl_empresa").html("");
    $("#ampl_tipoComb").html("");

    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerDatosEnvio',
        dataType: 'json',
        data: {
            idEnvio: idEnvio,
        },
        cache: false,
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $("#hf_amplIdEnvio").val(evt.Envio.Cbenvcodi);
                $("#ampl_enviocodi").html(evt.Envio.Cbenvcodi);
                $("#ampl_fechaSolicitud").html(evt.Envio.CbenvfecsolicitudDesc);
                $("#ampl_empresa").html(evt.Envio.Emprnomb);
                $("#ampl_tipoComb").html(evt.Envio.Fenergnomb);

                $("#idFechaAmp").val(evt.FechaPlazo);
                $("#cbHora").html(generarHtmlHora(evt.HoraPlazo));

                $("#hf_strFinMes").val(evt.FechaFin);

                //si hoy pasa fin de mes (respecto a la fecha de solicitud): no hay ampliacion
                if (evt.PermiteAmpliacion == 0) {
                    $("#btnAmpliarPlazo").css("display", "none");
                    mostrarMensaje('mensaje_popupAmpliar', 'alert', 'Ampliación inhabilitada dado que el plazo máximo (' + evt.FechaFin + ') fue superado.');
                } else {
                    if (evt.PermiteAmpliacion == 1) {
                        $("#btnAmpliarPlazo").css("display", "initial");
                    }
                }

                $('#idFechaAmp').Zebra_DatePicker({
                    format: 'd/m/Y',
                    direction: [evt.Envio.CbenvfecsolicitudDateDesc, $("#hf_strFinMes").val()],
                });

                abrirPopup("popupAmpliar");


            } else {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error: ' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', "Ha ocurrido un error");
        }
    });
}

function generarHtmlHora(horaActual) {
    var str = ``;

    for (var i = 0; i < 48; i++) {
        var hora = "0" + Math.floor((i + 1) / 2);
        hora = hora.substr((hora.length > 2) ? 1 : 0, 2);
        var minuto = "0" + (((i + 1) % 2) * 30);
        minuto = minuto.substr((minuto.length > 2) ? 1 : 0, 2);
        var horarmin = hora + ":" + minuto;

        var selected = i == horaActual ? "selected" : "";
        str += `<option value="${(i + 1)}" ${selected}>${horarmin}</option>`;
    }

    return str;
}

function grabarAmpliacion() {
    limpiarBarraMensaje("mensaje_popupAmpliar");

    var id = parseInt($("#hf_amplIdEnvio").val()) || 0;
    var fecha = $("#idFechaAmp").val();
    var hora = parseInt($("#cbHora").val()) || 0;

    if (confirm("¿Desea habilitar el plazo?")) {
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarAmpliacion',
            dataType: 'json',
            data: {
                idEnvio: id,
                fecha: fecha,
                hora: hora
            },
            cache: false,
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    alert("Se amplió el plazo para levantar observaciones.");
                    document.location.href = controlador + "Index?carpeta=" + EST_OBSERVADO;
                } else {
                    mostrarMensaje('mensaje_popupAmpliar', 'error', 'Ha ocurrido un error: ' + evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_popupAmpliar', 'error', 'Ha ocurrido un error. ');
            }
        });
    }
}

////////////////////////////////////////////////
// Nuevo envio
////////////////////////////////////////////////
function _nuevoEnvioForm() {

    $('#txtFechaVigNuevo').Zebra_DatePicker({
        format: "m-Y",
    });
}

function validarNuevoEnvio() {

    var mesVigencia = $("#txtFechaVigNuevo").val();
    var idEmpresa = $("#cbEmpresaNuevo").val();
    var tipoAccionForm = $('input[name=radio_accion]:checked').val();
    var usuarioGenerador = ($("#email_generador_nuevo").val()) ?? "";
    var esPrimeraCarga = $('#check_primera_carga').is(':checked') ? 1 : 0;

    var msjAlerta = '';
    if (idEmpresa <= 0) msjAlerta += ("Debe seleccionar una empresa correcta para realizar un nuevo envío. ");
    if (esPrimeraCarga == 0) {
        if (usuarioGenerador == "") msjAlerta += ("Debe ingresar usuario generador.");
        if (usuarioGenerador.length < 5 || !usuarioGenerador.includes("@")) msjAlerta += ("Debe ingresar usuario generador válido.");
    }

    if (msjAlerta != '') {
        alert(msjAlerta);
    } else {
        $.ajax({
            type: 'POST',
            url: controlador + 'ValidarNuevoEnvio',
            dataType: 'json',
            data: {
                mesVigencia: mesVigencia,
                idEmpresa: idEmpresa,
                tipoAccionForm: tipoAccionForm,
                usuarioGenerador: usuarioGenerador,
                esPrimeraCarga: esPrimeraCarga,
            },
            cache: false,
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    var flagExist = parseInt(evt.FlagCentralExistente) || 0;
                    var flagNuevo = parseInt(evt.FlagCentralNuevo) || 0;

                    if (flagExist == 1 && flagNuevo == 1) {
                        $("#hdIdEnvioTemporal").val(evt.IdEnvio);
                        abrirPopup("popupSeleccionarCentral");
                    } else {
                        if (flagExist == 1)
                            redireccionarFormulario(evt.IdEnvio, "E", idEmpresa, mesVigencia, tipoAccionForm, usuarioGenerador, esPrimeraCarga);
                        if (flagNuevo == 1)
                            redireccionarFormulario(evt.IdEnvio, "N", idEmpresa, mesVigencia, tipoAccionForm, usuarioGenerador, esPrimeraCarga);
                    }
                } else {
                    alert(evt.Mensaje)
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_popupAmpliar', 'error', 'Ha ocurrido un error. ');
            }
        });
    }
}

function redireccionarFormulario(idEnvio, tipoCentral, idEmpresa, mesVigencia, tipoAccionForm, usuarioGenerador, esPrimeraCarga) {
    var form_url = controlador + "EnvioCombustibleGas";

    $("#frmEnvio").attr("action", form_url);
    $("#hdIdEnvioForm").val(idEnvio);
    $("#hdTipoCentralForm").val(tipoCentral);
    $("#hdMesVigenciaForm").val(mesVigencia);
    $("#hdIdEmpresaForm").val(idEmpresa);
    $("#hdTipoAccionForm").val(tipoAccionForm);
    $("#hdUsuarioGeneradorForm").val(usuarioGenerador);
    $("#hdEsPrimeraCargaForm").val(esPrimeraCarga);

    $("#frmEnvio").submit();
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