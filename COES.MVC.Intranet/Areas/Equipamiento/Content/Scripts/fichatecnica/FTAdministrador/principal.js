var controlador = siteRoot + 'Equipamiento/FTAdministrador/';

var IMG_EDITAR = '<img src="' + siteRoot + 'Content/Images/btn-edit.png" alt="Editar Envío" title="Editar Envío" width="19" height="19" style="">';
var IMG_VER = '<img src="' + siteRoot + 'Content/Images/btn-open.png" alt="Ver Envío"  title="Ver Envío"width="19" height="19" style="">';
var IMG_AMPLIAR = '<img src="' + siteRoot + 'Content/Images/btn-add.png" alt="Ampliar Plazo" title="Ampliar Plazo" width="19" height="19" style="">';
var IMG_CARGO = '<img src="' + siteRoot + 'Content/Images/pegar.png" alt="Ver Cargo" title="Ver Cargo" width="19" height="19" style="">';
var IMG_DESCARGAR_EXCEL = '<img src="' + siteRoot + 'Content/Images/ExportExcel.png" alt="Descargar Detalle" title="Descargar Detalle" width="19" height="19" style="">';
var IMG_FECHA_SISTEMA = '<img src="' + siteRoot + 'Content/Images/calendar.png" alt="Habilitar Plazo" title="Fecha de sistema manual" width="15" height="19" style="">';
var IMG_CANCELAR = '<img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="Cancelar Envío" title="Cancelar Envío" width="15" height="19" style="">';
var IMG_HABILITAR_PLAZO_REV = '<img src="' + siteRoot + 'Content/Images/calendar-disabled.png" title="Habilitar plazo de revisión" width="15" height="19" style="">';
var IMG_OPCIONHAB_EQ = '<img src="' + siteRoot + 'Content/Images/btn-procesar.png" alt="Habilitar Equipos Proyecto" title="Habilitar/Deshabilitar Equipos Proyecto" width="19" height="19" style="">';
var IMG_DESHABILITAR_EQ = '<img src="' + siteRoot + 'Content/Images/btn-procesar2.jpg" alt="Habilitar Equipos Proyecto" title="Se ha deshabilitado la opción agregar equipos" width="19" height="19" style="">';
var IMG_HABILITAR_EQ = '<img src="' + siteRoot + 'Content/Images/btn-procesar3.jpg" alt="Habilitar Equipos Proyecto" title="Se habilitó la opción agregar equipos" width="19" height="19" style="">';

const EST_SOLICITUD = 1;
const EST_APROBADO = 3;
const EST_DESAPROBADO = 4;
const EST_OBSERVADO = 6;
const EST_SUBSANACION = 7;
const EST_CANCELADO = 8;
const EST_APROBADO_PARCIALMENTE = 10;

const ETAPA_CONEXION = 1;
const ETAPA_INTEGRACION = 2;
const ETAPA_OPERACION_COMERCIAL = 3;
const ETAPA_MODIFICACION = 4;

const EXTRANET = 1;
const INTRANET = 2;

const NUEVO = 1;
const EDITAR = 2;
const VER = 4;

var listaElementosCIO = [];
var listaElementosM = [];

var fechaSistemaManualActivo = false;

$(function () {


    $('#cbEmpresa').multipleSelect({
        width: '200px',
        filter: true,
    });
    $('#cbEmpresa').multipleSelect('checkAll');

    $('#FechaDesde').Zebra_DatePicker({
        format: "d/m/Y",
        pair: $('#FechaHasta'),
        direction: false,
    });

    $('#FechaHasta').Zebra_DatePicker({
        format: "d/m/Y",
        pair: $('#FechaDesde'),
        direction: true,
    });

    $('#btnBuscar').click(function () {
        buscarEnvio();
    });

    $('#btnExpotar').click(function () {
        exportar();
    });

    $('#btnReportes').click(function () {
        //Old
        //var url = siteRoot + 'Equipamiento/FTReporte/Index';
        var url = siteRoot + 'Equipamiento/FTReporte/IndexCumplimientoEnvio';
        window.open(url).focus();
    });

    $('#btnNuevoEnvio').click(function () {
        mostrarPopupNuevoEnvio();
    });

    //plazo de revision
    $('#btnGuardarFechaIniPlazo').click(function () {
        guardarHabilitarPlazo();
    });
    $("#btnCancelarFechaIniPlazo").click(function () {
        $('#idPopupPlazoRevision').bPopup().close();
    });

    //fecha de sistema manual
    $('#btnGuardarFecha').click(function () {
        guardarFechaSistema();
    });
    $("#btnCancelarFecha").click(function () {
        $('#idPopupFechaSistema').bPopup().close();
    });

    //ampliar plazo 
    $('#btnAmpliarPlazo').click(function () {
        GuardarAmpliarPlazo();
    });
    $("#btnCancelarAmpliar").click(function () {
        $('#idPopupAmpliarplazo').bPopup().close();
    });

    $('#cbEmpresaNE').change(function () {
        cargarListadoEtapasPorEmpresa();
    });

    $('#cbEtapaNE').change(function () {
        cargarListadoProyectosPorEmpresaYEtapa();
    });

    //Hbailitar equipos para agente
    $("#btnHabilitarEq").click(function () {
        habilitarEquipos("S");
    });

    $("#btnDeshabilitarEq").click(function () {
        habilitarEquipos("N");
    });

    buscarEnvio(); // muestra el listado de todos los envios de todas las empresas el filtro ingresado
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

    mostrarBloqueEnvios(idEstado);
}

function mostrarBloqueEnvios(idEstado) {
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
                ftetcodi: filtro.etapa,
                idEstado: parseInt(filtro.estado),
                finicios: filtro.finicio,
                ffins: filtro.ffin
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    fechaSistemaManualActivo = evt.UsarFechaSistemaManual;

                    var html = "<h3>Carpetas</h3>";
                    html += evt.HtmlCarpeta;
                    $("#div_carpetas").html(html);

                    var htmlEnvios = dibujarTablaReporte(evt.ListadoEnvios, idEstado, INTRANET);
                    $("#reporte").html(htmlEnvios);
                    //$('#listadoGeneralEnvios').css("width", $('#mainLayout').width() + "px");
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

function datosFiltro(idEstado) {
    var filtro = {};

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var etapa = parseInt($('#cbEtapa').val()) || 0;
    var estado = idEstado;
    var finicio = $('#FechaDesde').val();
    var ffin = $('#FechaHasta').val();
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    $('#hfEstado').val(estado);
    $('#hfEtapa').val(etapa);

    filtro.empresa = empresa;
    filtro.etapa = etapa;
    filtro.estado = estado;
    filtro.finicio = finicio;
    filtro.ffin = ffin;

    return filtro;
}

function validarDatosFiltroEnvios(datos) {
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

function convertirFecha(fecha) {
    var arrayFecha = fecha.split('/');
    var dia = arrayFecha[0];
    var mes = arrayFecha[1];
    var anio = arrayFecha[2];

    var salida = anio + mes + dia;
    return salida;
}

function HabilitarPlazo(id) {
    limpiarBarraMensaje("mensaje");
    limpiarBarraMensaje("mensaje_Relleno");

    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerDatosEnvio',
        dataType: 'json',
        data: {
            Ftenvcodi: id,
        },
        cache: false,
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#Env_Ftenvcodi').val(evt.Ftenvcodi);
                $('#FechaEnvioActual').val(evt.FechaPlazo);
                $('#FechaSistemaEnvio').val(evt.FechaPlazo);
                $('#FechaSistemaEnvio').Zebra_DatePicker({
                    format: "d/m/Y",
                    //direction: ['01/01/1900', '01/01/2050']
                    direction: [evt.FechaPlazo, '20/11/2050']
                });

                $("#cbHoraSistema").html(generarHtmlHora(evt.HoraPlazo));
                setTimeout(function () {
                    $('#idPopupFechaSistema').bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown',
                        modalClose: false
                    });
                }, 50);
            } else {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error: ' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', "Ha ocurrido un error");
        }
    });
}

function guardarFechaSistema() {
    limpiarBarraMensaje("mensaje");
    limpiarBarraMensaje("mensaje_Relleno");

    var ftenvcodi = parseInt($("#Env_Ftenvcodi").val()) || 0;
    var fechaoriginal = $("#FechaEnvioActual").val();
    var fechaSistema = $("#FechaSistemaEnvio").val();
    var horasistema = parseInt($("#cbHoraSistema").val()) || 0;

    if (fechaSistema != "") {
        if (confirm('¿Desea guardar la fecha del sistema para el envío?')) {
            $.ajax({
                type: 'POST',
                url: controlador + 'GrabarFechaSistema',
                data: {
                    ftenvcodi: ftenvcodi,
                    fechaoriginal: fechaoriginal,
                    fechasistema: fechaSistema,
                    horasistema: horasistema
                },
                dataType: 'json',
                success: function (evt) {
                    if (evt.Resultado != "-1") {
                        mostrarMensaje('mensaje', 'exito', "Fecha del sistema para el envío guardado de manera correcta.");
                        $('#idPopupFechaSistema').bPopup().close();
                    } else {
                        mostrarMensaje('mensaje_Relleno', 'error', 'Error: ' + evt.Mensaje);
                    }
                },
                error: function (err) {
                    mostrarMensaje('mensaje_Relleno', 'error', 'Ha ocurrido un error.');
                }
            });
        }
    } else {
        mostrarMensaje('mensaje_Relleno', 'error', "ingresa una fecha del sistema correcto.");
    }
}

function generarHtmlHora(horaActual) {
    var str = ``;

    for (var i = 0; i < 48; i++) {
        var hora = "0" + Math.floor(i / 2);
        hora = hora.substr((hora.length > 2) ? 1 : 0, 2);
        var minuto = "0" + ((i % 2) * 30);
        minuto = minuto.substr((minuto.length > 2) ? 1 : 0, 2);
        var horarmin = hora + ":" + minuto;

        var selected = i == horaActual ? "selected" : "";
        str += `<option value="${(i + 1)}" ${selected}>${horarmin}</option>`;
    }

    return str;
}

function mostrarAmpliarPlazo(id) {
    limpiarBarraMensaje("mensaje");
    limpiarBarraMensaje("mensaje_popupAmpliar");
    OcultarDatosAmpliarPlazo();

    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerDatosFinPlazo',
        dataType: 'json',
        data: {
            Ftenvcodi: id,
        },
        cache: false,
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#Env_FtenvcodiFP').val(evt.Ftenvcodi);
                $('#FechaAmpliarPlazo').val(evt.FechaPlazo);
                $('#FechaFinPlazo').val(evt.FechaPlazo);
                $('#FechaFinPlazo').Zebra_DatePicker({
                    format: "d/m/Y",
                    direction: [evt.FechaInicio, '20/11/2050']
                });

                //si es conexion, integracion y modifi(sin da baja MO)
                if (evt.Ftetcodi == 1 || evt.Ftetcodi == 2 || (evt.Ftetcodi == 4 && evt.EnvioTipoFormato != 3)) {
                    $("#trProyecto").css("display", "table-row");
                    $("#trEtapa").css("display", "table-row");
                    $("#trFSubs").css("display", "table-row");
                    $("#trFMaxSubs").css("display", "table-row");
                } else if (evt.Ftetcodi == 3) { //operacion comercial
                    $("#trProyecto").css("display", "table-row");
                    $("#trEquiposProy").css("display", "table-row");
                    $("#trEtapa").css("display", "table-row");
                    $("#trFSubs").css("display", "table-row");
                } else {
                    $("#trCategoria").css("display", "table-row");
                    $("#trUbicacion").css("display", "table-row");
                    $("#trNombre").css("display", "table-row");
                    $("#trFSubs").css("display", "table-row");
                }

                $("#campoCodEnvio").html(evt.Ftenvcodi);
                $("#campoEmpresa").html(evt.Emprnomb);
                $("#campoProyecto").html(evt.Ftprynombre);

                $("#campoCategoria").html(evt.ModoOperacion.Catenomb);
                $("#campoUbicacion").html(evt.ModoOperacion.Areanomb);
                $("#campoNombre").html(evt.ModoOperacion.Gruponomb);
                $("#campoEquipProy").html(evt.NombreEquipos);

                $("#campoEtapa").html(evt.Ftetnombre);
                $("#campoFechaSubs").html(evt.FechaSubsanacion);
                $("#campoFechaMaxSubs").html(evt.PlazoFinSubsanar);

                $("#cbHoraFinPlazo").html(generarHtmlHora(evt.HoraPlazo));

                setTimeout(function () {
                    $('#idPopupAmpliarplazo').bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown',
                        modalClose: false
                    });
                }, 50);
            } else {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error: ' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', "Ha ocurrido un error");
        }
    });
}

function GuardarAmpliarPlazo(Ftenvcodi) {
    limpiarBarraMensaje("mensaje_popupAmpliar");
    limpiarBarraMensaje("mensaje");

    var ftenvcodi = parseInt($("#Env_FtenvcodiFP").val()) || 0;
    var fechaoriginal = $("#FechaAmpliarPlazo").val();
    var fechaFinPlazo = $("#FechaFinPlazo").val();
    var horaFinPlazo = parseInt($("#cbHoraFinPlazo").val()) || 0;

    if (fechaFinPlazo != "") {
        if (confirm('¿Desea ampliar el plazo?')) {
            $.ajax({
                type: 'POST',
                url: controlador + 'GrabarAmpliarFecha',
                data: {
                    ftenvcodi: ftenvcodi,
                    fechaFinPlazo: fechaFinPlazo,
                    horaFinPlazo: horaFinPlazo
                },
                dataType: 'json',
                success: function (evt) {
                    if (evt.Resultado != "-1") {
                        $('#idPopupAmpliarplazo').bPopup().close();

                        alert('Se amplió el plazo para levantar observaciones..');
                        refrescarPrincipal(EST_OBSERVADO);
                    } else {
                        mostrarMensaje('mensaje_popupAmpliar', 'error', evt.Mensaje);

                    }
                },
                error: function (err) {
                    mostrarMensaje('mensaje_popupAmpliar', 'error', 'Ha ocurrido un error.');
                }
            });
        }
    } else {
        mostrarMensaje('mensaje_popupAmpliar', 'error', "ingresa una fecha del sistema correcto.");
    }
}

function refrescarPrincipal(estado) {
    var estadoEnvio = estado;
    document.location.href = controlador + "Index?carpeta=" + estadoEnvio;
}

function OcultarDatosAmpliarPlazo() {
    $("#trProyecto").css("display", "none");
    $("#trCategoria").css("display", "none");
    $("#trUbicacion").css("display", "none");
    $("#trNombre").css("display", "none");
    $("#trEquiposProy").css("display", "none");
    $("#trEtapa").css("display", "none");
    $("#trFSubs").css("display", "none");
    $("#trFMaxSubs").css("display", "none");
}


function mostrarHabilitarEq(id) {
    limpiarBarraMensaje("mensaje");

    setTimeout(function () {
        $("#popupHabilitarEquipos").bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);

    $('#ftenvcodiHabEq').val(id);
}

function habilitarEquipos(flag) {

    var id = parseInt($('#ftenvcodiHabEq').val());

    if (flag != ""){
        $.ajax({
            type: 'POST',
            url: controlador + 'HabilitarEdicionEquipos',
            dataType: 'json',
            data: {
                Ftenvcodi: id,
                flag: flag
            },
            cache: false,
            success: function (evt) {
                if (evt.Resultado != "-1") {

                    $('#popupHabilitarEquipos').bPopup().close();
                    buscarEnvio();

                    if (flag == "S")
                        mostrarMensaje('mensaje', 'exito', "Se habilitó la edición de equipos.");
                    else
                        mostrarMensaje('mensaje', 'exito', "Se deshabilitó la edición de equipos.");

                } else {
                    mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error: ' + evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', "Ha ocurrido un error");
            }
        });
    }
}

function dibujarTablaReporte(lista, estado, interfaz) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaEnvios" style="width:100%;">
       <thead>
           <tr>
               <th style='width:40px;white-space: inherit;'>Acciones</th>
               <th style="width:40px;white-space: inherit;">Código<br>del Envío</th>
               <th style="width:60px;white-space: inherit;">Empresa</th>
               <th style="width:50px;white-space: inherit;">Etapa</th>
               <th style="width:70px;white-space: inherit;">Nombre <br>Proyecto</th>
               <th style="width:70px;white-space: inherit;">Equipo(s) Proyecto</th>
               <th style="width:70px;white-space: inherit;">Nombre Equipo(s)</th>
               <th style="width:60px;white-space: inherit;">Usuario</th>
               <th style="width:60px;white-space: inherit;">Fecha de<br>Solicitud</th>                               
    `;
    if (estado == EST_SUBSANACION || estado == EST_APROBADO || estado == EST_APROBADO_PARCIALMENTE || estado == EST_DESAPROBADO || estado == EST_CANCELADO) {
        cadena += `
               <th style="width:60px;white-space: inherit;">Fecha de<br>Actualización</th>
        `;
    }
    if (estado == EST_APROBADO) // aprobados
    {
        cadena += `
               <th style="width:60px;white-space: inherit;">Fecha de<br>aprobación</th>
               <th style="width:60px;white-space: inherit;">Fecha de<br>vigencia</th>
        `;
    }
    if (estado == EST_APROBADO_PARCIALMENTE) // aprobados parciales
    {
        cadena += `
               <th style="width:60px;white-space: inherit;">Fecha de<br>aprobación parcial</th>
               <th style="width:60px;white-space: inherit;">Fecha de<br>vigencia</th>
        `;
    }
    if (estado == EST_DESAPROBADO) // desaprobados
    {
        cadena += `
               <th style="width:60px;white-space: inherit;">Fecha de<br>Denegación</th>
        `;
    }

    if (estado == EST_SOLICITUD || estado == EST_SUBSANACION) {
        cadena += `
               <th style="width:60px;white-space: inherit;">Habilitar plazo<br> de revisión</th>
        `;
    }
    if (fechaSistemaManualActivo) // para habilitar plazos
    {
        cadena += `
               <th style="width:60px;white-space: inherit;">Fecha Sistema <br/> para pruebas</th>
        `;
    }

    cadena += `
            </tr>
        </thead>
        <tbody>
    `;

    for (var key in lista) {
        var item = lista[key];

        cadena += `
            <tr>
                <td style='width:40px;white-space: inherit;'>
        `;

        if (interfaz == EXTRANET) {

            if (item.EsEditableExtranet) {
                cadena += `
                  <a title="Editar Envío" href="JavaScript:mostrarDetalle(${item.Ftenvcodi}, ${item.Ftetcodi}, ${item.Ftenvtipoformato},${EDITAR});">${IMG_EDITAR}</a>
            `;
            }
            else {
                cadena += `
                   <a title="Ver Envío" href="JavaScript:mostrarDetalle(${item.Ftenvcodi}, ${item.Ftetcodi}, ${item.Ftenvtipoformato}, ${VER});">${IMG_VER}</a>
            `;
            }
            if (estado == EST_SOLICITUD) {
                cadena += `
                     <a title="Cancelar Envío" href="JavaScript:cancelarEnvio(${item.Ftenvcodi});">${IMG_CANCELAR}</a>
                `;
            }
        } else {
            if (interfaz == INTRANET) {
                if (item.EsEditableIntranet) {

                    cadena += `
                        <a title="Editar Envío" href="JavaScript:mostrarDetalle(${item.Ftenvcodi},${item.Ftetcodi}, ${item.Ftenvtipoformato},${EDITAR});">${IMG_EDITAR}</a>
                     `;

                    if (estado == EST_SUBSANACION) {

                        cadena += `
                            <a title="Ampliar Plazo del Envío" href ="JavaScript:mostrarAmpliarPlazo(${item.Ftenvcodi});">${IMG_AMPLIAR}</a>
                        `;
                    }
                }
                else {
                    cadena += `
                   <a title="Ver Envío" href="JavaScript:mostrarDetalle(${item.Ftenvcodi},${item.Ftetcodi}, ${item.Ftenvtipoformato}, ${VER});">${IMG_VER}</a>
                `;

                    if (estado == EST_OBSERVADO && (item.Ftetcodi == ETAPA_CONEXION || item.Ftetcodi == ETAPA_INTEGRACION || item.Ftetcodi == ETAPA_OPERACION_COMERCIAL)) {

                        if (item.Ftenvflaghabeq == "N") {
                            cadena += `
                            <a title="Se ha deshabilitado la opción agregar equipos" href ="JavaScript:mostrarHabilitarEq(${item.Ftenvcodi});">${IMG_DESHABILITAR_EQ}</a>
                        `;
                        }
                        else {
                            if (item.Ftenvflaghabeq == "S") {
                                cadena += `
                                        <a title="Se ha habilitado la opción agregar equipos" href ="JavaScript:mostrarHabilitarEq(${item.Ftenvcodi});">${IMG_HABILITAR_EQ}</a>
                                    `;
                            } else {
                                cadena += `
                                        <a title="Habilitar /Deshabilitar Equipos Proyecto" href ="JavaScript:mostrarHabilitarEq(${item.Ftenvcodi});">${IMG_OPCIONHAB_EQ}</a>
                                        `;
                            }
                        }
                    }

                }
            }


        }

        cadena += `            
                </td>
                <td style="width:40px;white-space: inherit;">${item.Ftenvcodi}</td>
                <td style="width:60px;white-space: inherit;">${item.Emprnomb}</td>
                <td style="width:50px;white-space: inherit;">${item.Ftetnombre}</td>
                <td style="width:70px;white-space: inherit;">${item.Ftprynombre}</td>
        `;
        if (item.EquiposProyectoUnico.trim() != "") {
            var arrayNombEq = item.NombreEquipos.split(", ");
            var numEq = arrayNombEq.length;
            cadena += `
                <td style="width:70px;white-space: inherit;">
                    <div id="equiPyU_${item.Ftenvcodi}"> ${item.EquiposProyectoUnico}
            `;
            if (numEq > 1) {
                cadena += `
                        <a title="Ver Envío" href="JavaScript:mostrarMasProyecto(${item.Ftenvcodi});" style="float: right;color: green;font-weight: bold;">Ver Más</a>
                `;
            }
            cadena += `
                    </div>
                    <div id="equiPyT_${item.Ftenvcodi}" style="display:none;"> ${item.EquiposProyecto}
            `;
            if (numEq > 1) {
                cadena += `
                        <a title="Ver Envío" href="JavaScript:mostrarMenosProyecto(${item.Ftenvcodi});" style="float: right;color: green;font-weight: bold;">Ver Menos</a>
                `;
            }
            cadena += `
                    </div>                    
                </td>
            `;
        } else {
            cadena += `
                <td style="width:70px;white-space: inherit;">${item.EquiposProyecto}</td>
            `;
        }

        if (item.NombreEquiposUnico.trim() != "") {
            var arrayNombEq2 = item.NombreEquipos.split(", ");
            var numEq2 = arrayNombEq2.length;
            cadena += `
                <td style="width:70px;white-space: inherit;">
                    <div id="nomEquiU_${item.Ftenvcodi}"> ${item.NombreEquiposUnico}
            `;
            if (numEq2 > 1) {
                cadena += `
                        <a title="Ver Envío" href="JavaScript:mostrarMasEquipo(${item.Ftenvcodi});" style="float: right;color: green;font-weight: bold;">Ver Más</a>
                `;
            }
            cadena += `
                    </div>
                    <div id="nomEquiT_${item.Ftenvcodi}" style="display:none;"> ${item.NombreEquipos}
            `;
            if (numEq2 > 1) {
                cadena += `
                        <a title="Ver Envío" href="JavaScript:mostrarMenosEquipo(${item.Ftenvcodi});" style="float: right;color: green;font-weight: bold;">Ver Menos</a>
                `;
            }
            cadena += `
                    </div>
                </td>
            `;
        } else {
            cadena += `
                <td style="width:70px;white-space: inherit;">${item.NombreEquipos}</td>
            `;
        }
        cadena += `
                <td style="width:60px;white-space: inherit;">${item.Ftenvususolicitud}</td>
                <td style="width:60px;white-space: inherit;">${item.FtenvfecsolicitudDesc}</td>
        `;

        if (estado == EST_SUBSANACION || estado == EST_APROBADO || estado == EST_APROBADO_PARCIALMENTE || estado == EST_DESAPROBADO || estado == EST_CANCELADO) {
            //Fecha de Actualización
            cadena += `
               <td style="width:60px;white-space: inherit;">${item.FtenvfecmodificacionDesc}</td>
        `;
        }
        if (estado == EST_APROBADO) // aprobados
        {
            // Fecha de aprobación
            // Fecha de vigencia FALTA COMPLETAR
            cadena += `
               <td style="width:60px;white-space: inherit;">${item.FtenvfecaprobacionDesc}</td>
               <td style="width:60px;white-space: inherit;">${item.FechaVigenciaDesc}</td>
        `;
        }
        if (estado == EST_APROBADO_PARCIALMENTE) // aprobados parciales
        {
            // Fecha de aprobación parcial
            // Fecha de vigencia  FALTA COMPLETAR

            cadena += `
               <td style="width:60px;white-space: inherit;">${item.FechaAprobacionParcialDesc}</td>
               <td style="width:60px;white-space: inherit;">${item.FechaVigenciaDesc}</td> 
        `;
        }
        if (estado == EST_DESAPROBADO) // desaprobados
        {
            //Fecha de Denegación
            cadena += `
               <td style="width:60px;white-space: inherit;">${item.FechaDesaprobacionDesc}</td>
        `;
        }

        if (interfaz == INTRANET) {

            if (estado == EST_SOLICITUD || estado == EST_SUBSANACION) {

                cadena += `
                        <td style="width:60px;white-space: inherit;">
                            <a title="Habilitar plazo de revisión" href="JavaScript:habilitarPlazoRevision(${item.Ftenvcodi});">${IMG_HABILITAR_PLAZO_REV}</a>
                        </td>
                     `;
            }
        }

        if (fechaSistemaManualActivo) // para habilitar plazos
        {
            cadena += `
               <td style="width:60px;white-space: inherit;">
                    <a title="Fecha de sistema manual" href="JavaScript:HabilitarPlazo(${item.Ftenvcodi});">${IMG_FECHA_SISTEMA}</a>
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

function mostrarMasProyecto(id) {
    $("#equiPyU_" + id).css("display", "none");
    $("#equiPyT_" + id).css("display", "block");
}

function mostrarMenosProyecto(id) {
    $("#equiPyU_" + id).css("display", "block");
    $("#equiPyT_" + id).css("display", "none");
}

function mostrarMasEquipo(id) {
    $("#nomEquiU_" + id).css("display", "none");
    $("#nomEquiT_" + id).css("display", "block");
}

function mostrarMenosEquipo(id) {
    $("#nomEquiU_" + id).css("display", "block");
    $("#nomEquiT_" + id).css("display", "none");
}

//habilitar plazo de revision
function habilitarPlazoRevision(id) {
    limpiarBarraMensaje("mensaje");
    limpiarBarraMensaje("mensaje_relleno_habilitar_plazo");

    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerDatosEnvio',
        dataType: 'json',
        data: {
            Ftenvcodi: id,
        },
        cache: false,
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#ftenvcodi_habilitar_ini_plazo').val(evt.Ftenvcodi);
                $('#fecha_habilitar_ini_plazo').val(evt.FechaPlazoRevision);
                $('#fecha_habilitar_ini_plazo').Zebra_DatePicker({
                    format: "d/m/Y",
                });

                setTimeout(function () {
                    $('#idPopupPlazoRevision').bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown',
                        modalClose: false
                    });
                }, 50);
            } else {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error: ' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', "Ha ocurrido un error");
        }
    });
}

function guardarHabilitarPlazo() {
    limpiarBarraMensaje("mensaje");
    limpiarBarraMensaje("mensaje_relleno_habilitar_plazo");

    var ftenvcodi = parseInt($("#ftenvcodi_habilitar_ini_plazo").val()) || 0;
    var fechaIniPlazo = $("#fecha_habilitar_ini_plazo").val();

    if (fechaIniPlazo != "") {
        if (confirm('¿Desea guardar la fecha de inicio de plazo para el envío?')) {
            $.ajax({
                type: 'POST',
                url: controlador + 'GrabarFechaInicioRevision',
                data: {
                    ftenvcodi: ftenvcodi,
                    fechaIniPlazo: fechaIniPlazo,
                },
                dataType: 'json',
                success: function (evt) {
                    if (evt.Resultado != "-1") {
                        mostrarMensaje('mensaje', 'exito', "Fecha de inicio de plazo para el envío guardado de manera correcta.");
                        $('#idPopupPlazoRevision').bPopup().close();
                        buscarEnvio(); //actualizar listado de envios
                    } else {
                        mostrarMensaje('mensaje_relleno_habilitar_plazo', 'error', 'Error: ' + evt.Mensaje);
                    }
                },
                error: function (err) {
                    mostrarMensaje('mensaje_relleno_habilitar_plazo', 'error', 'Ha ocurrido un error.');
                }
            });
        }
    } else {
        mostrarMensaje('mensaje_relleno_habilitar_plazo', 'error', "ingresa una fecha correcta.");
    }
}

//exportar
function exportar() {
    limpiarBarraMensaje("mensaje");

    var idDelEstado = $("#hdIdEstado").val();

    var filtro = datosFiltro(idDelEstado);
    var msg = validarDatosFiltroEnvios(filtro);

    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'GenerarArchivoReporte',
            data: {
                empresas: $('#hfEmpresa').val(),
                ftetcodi: filtro.etapa,
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
    } else {
        mostrarMensaje('mensaje', 'error', msg);
    }
}

////////////////////////////////////////////////
// Mostrar Detalles del envio
////////////////////////////////////////////////
function mostrarDetalle(id, idEtapa, tipoFormato, accion) {
    var strDarBaja = "";
    if (tipoFormato == 3) //dar Baja
        strDarBaja = "XYZ";
    redireccionarFormulario(id, idEtapa, strDarBaja);
}

////////////////////////////////////////////////
// Ir a Formulario
////////////////////////////////////////////////
function redireccionarFormulario(idEnvio, idEtapa, strDarBaja) {

    var form_url = "";
    switch (idEtapa) {
        case ETAPA_CONEXION:
        case ETAPA_INTEGRACION:
            form_url = controlador + "EnvioFormato"; break;  //CU014
        case ETAPA_OPERACION_COMERCIAL:
            form_url = controlador + "EnvioFormatoOperacionComercial"; break; //CU015
        case ETAPA_MODIFICACION:
            if (strDarBaja == "") {
                form_url = controlador + "EnvioFormato"; break;  //CU014
            } else {
                form_url = controlador + "EnvioFormatoBajaModoOperacion"; break; //CU016
            }
    }

    document.location.href = form_url + "?codigoEnvio=" + idEnvio;
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