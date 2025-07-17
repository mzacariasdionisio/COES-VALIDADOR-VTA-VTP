var controlador = siteRoot + 'Equipamiento/Envio/';

var IMG_EDITAR = '<img src="' + siteRoot + 'Content/Images/btn-edit.png" alt="Editar Envío" width="19" height="19" style="">';
var IMG_VER = '<img src="' + siteRoot + 'Content/Images/btn-open.png" alt="Ver Envío" width="19" height="19" style="">';
var IMG_AMPLIAR = '<img src="' + siteRoot + 'Content/Images/btn-add.png" alt="Ampliar Plazo" width="19" height="19" style="">';
var IMG_CARGO = '<img src="' + siteRoot + 'Content/Images/pegar.png" alt="Ver Cargo" width="19" height="19" style="">';
var IMG_DESCARGAR_EXCEL = '<img src="' + siteRoot + 'Content/Images/ExportExcel.png" alt="Descargar Detalle" width="19" height="19" style="">';
var IMG_DESCARGAR_PDF = '<img src="' + siteRoot + 'Content/Images/pdf.png" alt="Descargar Detalle" width="15" height="19" style="">';
var IMG_CANCELAR = '<img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="Cancelar Envío" width="15" height="19" style="">';


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

    $('#btnNuevoEnvio').click(function () {
        mostrarPopupNuevoEnvio();
    });

    $('#cbEmpresaNE').change(function () {

        cargarListadoEtapasPorEmpresa();

    });

    $('#cbEtapaNE').change(function () {
        cargarListadoProyectosPorEmpresaYEtapa();
    });

    $('#cbProyectoNE').change(function () {
        var idEtapa = parseInt($("#cbEtapaNE").val());
        if (idEtapa != ETAPA_MODIFICACION) {

            mostrarMensajeEtapaEmpresaProyecto();
        }
    });

    $('#btnAceptarNE').click(function () {
        mostrarPopupListadoEquiposNE();
    });

    //cancelacion
    $('#btnAceptarCancelar').click(function () {
        realizarCancelarEnvio();
    });
    $('#btnCerrarCancelar').click(function () {
        cerrarPopup('popupCancelar');
    });

    //FORMULARIO NUEVO ENVIO
    $('#btnAceptarCIO').click(function () {
        crearNuevoEnvioCIO();
    });
    $('#btnAceptarM').click(function () {
        crearNuevoEnvioM();

    });

    $('#btnVerFT').click(function () {
        var url = siteRoot + 'Equipamiento/FichaTecnica/FichaTecnica';
        window.open(url, '_blank').focus();
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
                idEstado: filtro.estado,
                finicios: filtro.finicio,
                ffins: filtro.ffin
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {

                    //mostrar mensaje de validacion cuando no haya empresas
                    if (evt.Mensaje != null && evt.Mensaje != "")
                        mostrarMensaje('mensaje', 'error', 'Error: ' + evt.Mensaje);

                    var html = "<h3>Carpetas</h3>";
                    html += evt.HtmlCarpeta;
                    $("#div_carpetas").html(html);

                    var htmlEnvios = dibujarTablaReporte(evt.ListadoEnvios, idEstado, EXTRANET);
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



function dibujarTablaReporte(lista, estado, interfaz) {
    var esAgente = (parseInt($("#hfEsAgenteFT").val()) || 0) == 1;

    var cadena = '';
    cadena += `
    <table id="tablaEnvios" border="0" class="pretty tabla-icono"  style="width: 100%">
       <thead>
           <tr>
               <th style='white-space: inherit;'>Acciones</th>
               <th style="white-space: inherit;">Código<br>del <br>Envío</th>
               <th style="white-space: inherit;">Empresa</th>
               <th style="white-space: inherit;">Etapa</th>
               <th style="white-space: inherit;">Nombre <br>Proyecto</th>
               <th style="white-space: inherit;">Equipo(s) Proyecto</th>
               <th style="white-space: inherit;">Nombre Equipo(s)</th>
               <th style="white-space: inherit;">Usuario</th>
               <th style="white-space: inherit;">Fecha de<br>Solicitud</th>
                
               
    `;
    if (estado == EST_SUBSANACION || estado == EST_APROBADO || estado == EST_APROBADO_PARCIALMENTE || estado == EST_DESAPROBADO || estado == EST_CANCELADO) {
        cadena += `
               <th style="white-space: inherit;">Fecha de<br>Actualización</th>
        `;
    }
    if (estado == EST_APROBADO) // aprobados
    {
        cadena += `
               <th style="white-space: inherit;">Fecha de<br>aprobación</th>
               <th style="white-space: inherit;">Fecha de<br>vigencia</th>
        `;
    }
    if (estado == EST_APROBADO_PARCIALMENTE) // aprobados parciales
    {
        cadena += `
               <th style="white-space: inherit;">Fecha de<br>aprobación parcial</th>
               <th style="white-space: inherit;">Fecha de<br>vigencia</th>
        `;
    }
    if (estado == EST_DESAPROBADO) // desaprobados
    {
        cadena += `
               <th style="white-space: inherit;">Fecha de<br>Denegación</th>
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
                <td style='white-space: inherit;'>
        `;

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
        if (estado == EST_SOLICITUD && esAgente) {
            cadena += `
                     <a title="Cancelar Envío" href="JavaScript:cancelarEnvio(${item.Ftenvcodi});">${IMG_CANCELAR}</a>
                `;
        }

        cadena += `            
                </td>
                <td style="white-space: inherit;">${item.Ftenvcodi}</td>
                <td style="white-space: inherit;">${item.Emprnomb}</td>
                <td style="white-space: inherit;">${item.Ftetnombre}</td>
                <td style="white-space: inherit;">${item.Ftprynombre}</td>
        `;
        if (item.EquiposProyectoUnico.trim() != "") {
            var arrayNombEq = item.NombreEquipos.split(", ");
            var numEq = arrayNombEq.length;
            cadena += `
                <td style="white-space: inherit;">
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
                <td style="white-space: inherit;">${item.EquiposProyecto}</td>
            `;
        }

        if (item.NombreEquiposUnico.trim() != "") {
            var arrayNombEq2 = item.NombreEquipos.split(", ");
            var numEq2 = arrayNombEq2.length;
            cadena += `
                <td style="white-space: inherit;">
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
                <td style="white-space: inherit;">${item.NombreEquipos}</td>
            `;
        }
        cadena += `
                <td style="white-space: inherit;">${item.Ftenvususolicitud}</td>
                <td style="white-space: inherit;">${item.FtenvfecsolicitudDesc}</td>
        `;

        if (estado == EST_SUBSANACION || estado == EST_APROBADO || estado == EST_APROBADO_PARCIALMENTE || estado == EST_DESAPROBADO || estado == EST_CANCELADO) {
            cadena += `
               <td style="white-space: inherit;">${item.FtenvfecmodificacionDesc}</td>
        `;
        }
        if (estado == EST_APROBADO) // aprobados
        {
            cadena += `
               <td style="white-space: inherit;">${item.FtenvfecaprobacionDesc}</td>
               <td style="white-space: inherit;">${item.FechaVigenciaDesc}</td>
        `;
        }
        if (estado == EST_APROBADO_PARCIALMENTE) // aprobados parciales
        {
            cadena += `
               <td style="white-space: inherit;">${item.FechaAprobacionParcialDesc}</td>
               <td style="white-space: inherit;">${item.FechaVigenciaDesc}</td>
        `;
        }
        if (estado == EST_DESAPROBADO) // desaprobados
        {
            cadena += `
               <td style="white-space: inherit;">${item.FechaDesaprobacionDesc}</td>
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

function mostrarPopupNuevoEnvio() {

    inicializarPopuNuevo();
    abrirPopup("popupNuevoEnvio");
    $("#hdAccion").val(NUEVO);
}

function inicializarPopuNuevo() {
    $("#BloqueFilaProyectos").hide();
    $("#cbEmpresaNE").val("0");
    $("#cbEtapaNE").val("0");
    $("#cbProyectoNE").val("0");

    mostrarMensaje('mensaje_popupNuevo', 'message', 'Seleccione empresa y etapa.');
}

function mostrarMensajeEtapaEmpresaProyecto() {
    var idEmpresa = parseInt($("#cbEmpresaNE").val());
    var idEtapa = parseInt($("#cbEtapaNE").val());
    var idProyecto = parseInt($("#cbProyectoNE").val());
    var accion = parseInt($("#hdAccion").val());

    var mensaje = "";

    if (idEtapa != 0) {
        if (idEtapa != ETAPA_MODIFICACION) {
            if (idProyecto == 0) {
                if (idEmpresa == 0) {
                    if (idEtapa == 0) {
                        mensaje = "Seleccione empresa, etapa y proyecto.";
                    } else {
                        mensaje = "Seleccione empresa y proyecto.";
                    }
                } else {
                    if (idEtapa == 0) {
                        mensaje = "Seleccione etapa y proyecto.";
                    } else {
                        mensaje = "Seleccione proyecto.";
                    }
                }
            } else {
                if (idEmpresa == 0) {
                    if (idEtapa == 0) {
                        mensaje = "Seleccione empresa y etapa.";
                    } else {
                        mensaje = "Seleccione empresa.";
                    }
                } else {
                    if (idEtapa == 0) {
                        mensaje = "Seleccione etapa.";
                    }
                }
            }
        } else {
            if (idEmpresa == 0) {
                if (idEtapa == 0) {
                    mensaje = "Seleccione empresa y etapa.";
                } else {
                    mensaje = "Seleccione empresa.";
                }
            } else {
                if (idEtapa == 0) {
                    mensaje = "Seleccione etapa.";
                }
            }
        }
    } else {
        if (idEmpresa == 0) {
            if (idEtapa == 0) {
                mensaje = "Seleccione empresa y etapa.";
            } else {
                mensaje = "Seleccione empresa.";
            }
        } else {
            if (idEtapa == 0) {
                mensaje = "Seleccione etapa.";
            }
        }
    }

    if (mensaje == "") {
        limpiarBarraMensaje("mensaje_popupNuevo");
    }
    else {
        mostrarMensaje('mensaje_popupNuevo', 'message', mensaje);
    }
}

function cargarListadoEtapasPorEmpresa() {
    limpiarBarraMensaje("mensaje_popupNuevo");
    var idEmpresa = parseInt($("#cbEmpresaNE").val());
    $("#cbEtapaNE").empty();
    $("#cbProyectoNE").empty();
    $.ajax({
        type: 'POST',
        url: controlador + "CargarEtapasNE",
        data: { emprcodi: idEmpresa },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                var listaEtapas = evt.ListaEtapas;

                //Listamos etapas
                if (listaEtapas.length > 0) {
                    //usando for
                    $('#cbEtapaNE').get(0).options[0] = new Option("--  SELECCIONE  --", "0"); //obliga seleccionar para buscar tag
                    for (var i = 0; i < listaEtapas.length; i++) {
                        $('#cbEtapaNE').append('<option value=' + listaEtapas[i].Ftetcodi + '>' + listaEtapas[i].Ftetnombre + '</option>');  // listaEtapas es List<string>, si es objeto usar asi ListaIdentificador[i].Equicodi
                    }

                } else {
                    $('#cbEtapaNE').get(0).options[0] = new Option("--  SELECCIONE  --", "0");
                    $("#BloqueFilaProyectos").hide();
                }

                //Limpiamos combo proyectos
                $("#cbProyectoNE").empty();
                $('#cbProyectoNE').get(0).options[0] = new Option("--  SELECCIONE  --", "0");
                $("#BloqueFilaProyectos").hide();


            } else {
                mostrarMensaje('mensaje_popupNuevo', 'error', evt.Mensaje);
            }
            mostrarMensajeEtapaEmpresaProyecto();
        },
        error: function (err) {
            mostrarMensaje('mensaje_popupNuevo', 'error', 'Ha ocurrido un error al mostrar etapas.');
        }
    });
}


function cargarListadoProyectosPorEmpresaYEtapa() {
    limpiarBarraMensaje("mensaje_popupNuevo");
    var idEtapa = parseInt($("#cbEtapaNE").val());
    var idEmpresa = parseInt($("#cbEmpresaNE").val());
    $("#BloqueFilaProyectos").hide();
    if (idEtapa != ETAPA_MODIFICACION && idEtapa != 0) {
        limpiarBarraMensaje("mensaje_popupNuevo");
        $("#cbProyectoNE").empty();
        $.ajax({
            type: 'POST',
            url: controlador + "CargarProyectosNE",
            data: {
                emprcodi: idEmpresa,
                ftetcodi: idEtapa
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {

                    $("#BloqueFilaProyectos").show();
                    var listaProyectos = evt.ListaProyectos;

                    //Listamos proyectos
                    if (listaProyectos.length > 0) {

                        //usando for
                        $('#cbProyectoNE').get(0).options[0] = new Option("--  SELECCIONE  --", "0"); //obliga seleccionar para buscar tag
                        for (var i = 0; i < listaProyectos.length; i++) {
                            $('#cbProyectoNE').append('<option value=' + listaProyectos[i].Ftprycodi + '>' + listaProyectos[i].Ftprynombre + '</option>');  // listaProyectos es List<string>, si es objeto usar asi ListaIdentificador[i].Equicodi
                        }

                    } else {
                        $('#cbProyectoNE').get(0).options[0] = new Option("--  SELECCIONE  --", "0");

                    }


                } else {
                    mostrarMensaje('mensaje_popupNuevo', 'error', evt.Mensaje);
                }
                mostrarMensajeEtapaEmpresaProyecto();
            },
            error: function (err) {
                mostrarMensaje('mensaje_popupNuevo', 'error', 'Ha ocurrido un error al mostrar proyectos.');
            }
        });
    } else {
        $("#BloqueFilaProyectos").hide();
    }
}


function mostrarPopupListadoEquiposNE() {
    limpiarBarraMensaje("mensaje_popupNuevo");
    limpiarBarraMensaje("mensaje_popupEquiposCIO");
    limpiarBarraMensaje("mensaje_popupEquiposM");

    var filtro = datosFiltrosNE();
    var msg = validarFiltrosNE(filtro);

    if (msg == "") {
        if (filtro.etapaid != ETAPA_MODIFICACION) {
            mostrarListadoEquiposCIO(filtro);
        } else {
            mostrarListadoEquiposM(filtro);
        }
    } else {
        mostrarMensaje('mensaje_popupNuevo', 'alert', msg);
    }
}


function datosFiltrosNE() {
    var filtro = {};

    filtro.Numempresas = parseInt($('#hdNumEmpresas').val());
    filtro.idempresa = parseInt($('#cbEmpresaNE').val());
    filtro.etapaid = parseInt($("#cbEtapaNE").val());
    filtro.proyectoid = parseInt($("#cbProyectoNE").val());

    return filtro;
}


function validarFiltrosNE(filtro) {

    var msj = "";

    //validaciones generales
    if (filtro.Numempresas == 0) {
        msj += "<p>No existe empresa disponible para realizar el llenado o modificación de Ficha técnica.</p>";
    } else {
        if (filtro.idempresa == 0) {
            msj += "<p>Debe seleccionar una Empresa.</p>";
        }

        if (filtro.etapaid == 0) {
            msj += "<p>Debe seleccionar una Etapa.</p>";
        }

        if (filtro.etapaid != 0 && filtro.etapaid != ETAPA_MODIFICACION) {
            if (filtro.proyectoid == 0) {
                msj += "<p>Debe seleccionar un Proyecto.</p>";
            }
        }
    }

    return msj;
}

function mostrarListadoEquiposCIO(filtro) {
    $("#listadoEquiposCIO").html("");
    $("#tituloPopupCIO").html("<span></span>");
    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerListadoEquiposCIMO",
        data: {
            emprcodi: filtro.idempresa,
            ftetcodi: filtro.etapaid,
            ftprycodi: filtro.proyectoid
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $("#tituloPopupCIO").html("<span>" + evt.Proyecto.Ftprynombre.trim() + "</span>");

                var htmlL = dibujarTablaEquipoEnvioCIO(evt.ListaEquipoEnvio);
                $("#listadoEquiposCIO").html(htmlL);

                $('#tablaDetalleEquipoCIO').dataTable({
                    "scrollY": 300,
                    "scrollX": false,
                    "sDom": 'ft',
                    "ordering": false,
                    "iDisplayLength": -1
                });

                abrirPopup("popupEquiposCIO");

                //Toda la columna cambia (al escoger casilla cabecera)
                $('input[type=checkbox][name^="chkCIOTodo"]').unbind();
                $('input[type=checkbox][name^="chkCIOTodo"]').change(function () {
                    var valorCheck = $(this).prop('checked');
                    $("input[type=checkbox][id^=chkCIO_]").each(function () {
                        $("#" + this.id).prop("checked", valorCheck);
                    });
                });

                //Verifico si el check cabecera debe pintarse o no al editar cualquier check hijo
                $('input[type=checkbox][name^="chkCIO_"]').unbind();
                $('input[type=checkbox][name^="chkCIO_"]').change(function () {
                    verificarCheckGrupalCIO();
                });

            } else {
                mostrarMensaje('mensaje_popupNuevo', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje_popupNuevo', 'error', 'Ha ocurrido un error al mostrar equipos.');
        }
    });
}

function dibujarTablaEquipoEnvioCIO(lista) {
    var cadena = '';
    cadena += `
     <div class="seleccion_Equipos"> Seleccionar Equipos </div>
    `;
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaDetalleEquipoCIO" cellspacing="0"  >
        <thead>            
            <tr style="height: 30px;">
                <th style="width: 40px">Sel. Todos <br/> <input type="checkbox" id="chkCIOTodo" name="chkCIOTodo"> </th>
                
                <th style="width: 40px">Código</th>
                <th style="width: 160px">Empresa <br>Titular</th>
                <th style="width: 160px">Empresa <br>Copropietaria</th>
                <th style="width: 200px">Tipo de <br>equipo / categoría</th>
                <th style="width: 160px">Ubicación</th>
                <th style="width: 130px">Nombre</th>
            </tr>
        </thead>
        <tbody>
    `;

    var fila = 1;
    for (key in lista) {
        var item = lista[key];
        var seleccionable = item.CheckSeleccionableEnNuevo;
        var color = "";

        var tdSelec = `<input type="checkbox" value="${item.TipoYCodigo}" name="chkCIO_${item.TipoYCodigo}" id="chkCIO_${item.TipoYCodigo}" />`;

        cadena += `
           <tr >
        `;

        if (seleccionable) {
            cadena += `
                <td style="width: 40px; white-space: inherit;">
                    ${tdSelec}
                </td>
            `;
        } else {
            color = "#C0C1C2";
            cadena += `
                <td style="background: ${color}; white-space: inherit; width: 40px">
                    
                </td>
            `;
        }
        cadena += `
                <td style="background: ${color}; white-space: inherit; width: 40px">${item.Codigo}</td>
                <td style="background: ${color}; white-space: inherit; width: 160px">${item.EmpresaNomb}</td>
                <td style="background: ${color}; white-space: inherit; width: 160px">${item.EmpresaCoNomb}</td>
                <td style="background: ${color}; white-space: inherit; width: 200px">${item.Tipo}</td>
                <td style="background: ${color}; white-space: inherit; width: 160px">${item.Ubicacion}</td>
                <td style="background: ${color}; white-space: inherit; width: 130px">${item.EquipoNomb}</td>
           </tr >    
        `;

        fila++;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function verificarCheckGrupalCIO() {

    var val1 = 0;
    $("input[type=checkbox][id^=chkCIO_]").each(function () {
        var IsCheckedIS = $("#" + this.id)[0].checked;
        if (IsCheckedIS) {

        } else {
            val1 = val1 + 1;
        }
    });

    var v = true;
    if (val1 > 0)
        v = false;

    $("#chkCIOTodo").prop("checked", v);
}


function mostrarListadoEquiposM(filtro) {
    $("#listadoEquiposM").html("");

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerListadoEquiposCIMO",
        data: {
            emprcodi: filtro.idempresa,
            ftetcodi: filtro.etapaid,
            ftprycodi: filtro.proyectoid
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {


                var htmlL = dibujarTablaEquipoEnvioM(evt.ListaEquipoEnvio);
                $("#listadoEquiposM").html(htmlL);

                $('#tablaDetalleEquipoM').dataTable({
                    "scrollY": 300,
                    "scrollX": false,
                    "sDom": 'ft',
                    "ordering": false,
                    "iDisplayLength": -1
                });

                abrirPopup("popupEquiposM");

                //Toda la columna cambia (al escoger casilla cabecera)
                $('input[type=checkbox][name^="chkMTodo"]').unbind();
                $('input[type=checkbox][name^="chkMTodo"]').change(function () {
                    var valorCheck = $(this).prop('checked');
                    $("input[type=checkbox][id^=chkM_]").each(function () {
                        $("#" + this.id).prop("checked", valorCheck);
                    });

                    //Muestro/oculto todos los input de DAR BAJA 
                    evaluarColumnaDarBaja(this);
                });

                //Verifico si el check cabecera debe pintarse o no al editar cualquier check hijo
                $('input[type=checkbox][name^="chkM_"]').unbind();
                $('input[type=checkbox][name^="chkM_"]').change(function () {
                    verificarCheckGrupalM();
                    var esMarcado = $(this).prop('checked');
                    if (esMarcado)
                        mostrarInRaFila(this);
                    else
                        quitarInRaFila(this);
                });

            } else {
                mostrarMensaje('mensaje_popupNuevo', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje_popupNuevo', 'error', 'Ha ocurrido un error al mostrar equipos para la etapa Modificación.');
        }
    });
}


function dibujarTablaEquipoEnvioM(lista) {
    var cadena = '';

    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaDetalleEquipoM" cellspacing="0"  >
        <thead>            
            <tr style="height: 30px;">
                <th style="width: 40px">Sel. Todos <br/> <input type="checkbox" id="chkMTodo" name="chkMTodo"> </th>
                
                <th style="width: 40px">Código</th>
                <th style="width: 150px">Empresa <br>Titular</th>
                <th style="width: 150px">Empresa <br>Copropietaria</th>
                <th style="width: 180px">Tipo de <br>equipo / categoría</th>
                <th style="width: 160px">Ubicación</th>
                <th style="width: 130px">Nombre</th>
                <th style="width: 40px">Dar de baja</th>
            </tr>
        </thead>
        <tbody>
    `;

    var fila = 1;
    for (key in lista) {
        var item = lista[key];
        var seleccionable = item.CheckSeleccionableEnNuevo;
        var color = "";

        var tdSelec = `<input type="checkbox" value="${item.TipoYCodigo}" data-catecodifila="${item.Catecodi}" name="chkM_${item.TipoYCodigo}" id="chkM_${item.TipoYCodigo}" />`;

        cadena += `
           <tr >
        `;

        if (seleccionable) {
            cadena += `
                <td style="width: 40px; white-space: inherit;">
                    ${tdSelec}
                </td>
            `;
        } else {
            color = "#C0C1C2";
            cadena += `
                <td style="background: ${color}; white-space: inherit; width: 40px">
                    
                </td>
            `;
        }
        cadena += `
                
                <td style="background: ${color}; white-space: inherit; width: 40px">${item.Codigo}</td>
                <td style="background: ${color}; white-space: inherit; width: 150px">${item.EmpresaNomb}</td>
                <td style="background: ${color}; white-space: inherit; width: 150px">${item.EmpresaCoNomb}</td>
                <td style="background: ${color}; white-space: inherit; width: 180px">${item.Tipo}</td>
                <td style="background: ${color}; white-space: inherit; width: 160px">${item.Ubicacion}</td>
                <td style="background: ${color}; white-space: inherit; width: 130px">${item.EquipoNomb}</td>
                <td style="background: ${color}; white-space: inherit; width: 40px"><div id="InRa_${item.Codigo}"></div></td>
        `;

        cadena += `
           </tr >    
        `;

        fila++;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function verificarCheckGrupalM() {

    var val1 = 0;
    $("input[type=checkbox][id^=chkM_]").each(function () {
        var IsCheckedIS = $("#" + this.id)[0].checked;
        if (IsCheckedIS) {

        } else {
            val1 = val1 + 1;
        }
    });

    var v = true;
    if (val1 > 0)
        v = false;

    $("#chkMTodo").prop("checked", v);
}

function evaluarColumnaDarBaja(miobj) {
    var valorCheck = $(miobj).prop('checked');
    $("input[type=checkbox][id^=chkM_]").each(function () {
        //$("#" + this.id).prop("checked", valorCheck);

        //var esMarcado = $(this).prop('checked');
        var esMarcado = valorCheck;
        if (esMarcado)
            mostrarInRaFila(this);
        else
            quitarInRaFila(this);
    });
}

function mostrarInRaFila(miObj) {
    var idCheck = miObj.id;
    var catecodi = miObj.dataset.catecodifila;  //siempre es en minusculas (no usar mayusculas)
    const myArray = idCheck.split("_");
    let cod = myArray[1];
    var codElemento = cod.substring(1);

    var inp = "";
    if (catecodi == "2")  //Modo de operacion termico / Catecodi == 2
    {
        //creo el input radio
        var inp = '<input type="radio" id="IR_' + cod + '" name="IR_Darbaja" value="' + cod + '">';
    }
    $("#InRa_" + codElemento).html(inp);
}

function quitarInRaFila(miObj) {
    var idCheck = miObj.id;
    const myArray = idCheck.split("_");
    let cod = myArray[1];
    var codElemento = cod.substring(1);

    //creo el input radio    
    $("#InRa_" + codElemento).html("");
}



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
// Cancelar Envio
////////////////////////////////////////////////
function cancelarEnvio(idEnvio) {
    limpiarBarraMensaje("mensaje");
    limpiarBarraMensaje("mensaje_popupCancelarEnvio");
    $("#hfIdCancelarEnvio").val(idEnvio);
    $('#txtMotivo').val('');

    abrirPopup("popupCancelar");
}

function realizarCancelarEnvio() {
    limpiarBarraMensaje("mensaje");
    limpiarBarraMensaje("mensaje_popupCancelarEnvio");
    var idEnvio = parseInt($("#hfIdCancelarEnvio").val()) || 0;
    var motivo = ($('#txtMotivo').val()).trim();
    if (motivo == '') {
        mostrarMensaje('mensaje_popupCancelarEnvio', 'alert', 'Error: ' + 'Debe ingresar motivo para realizar la cancelación.');
    }
    else {
        if (motivo.length > 1000) {
            mostrarMensaje('mensaje_popupCancelarEnvio', 'alert', 'Error: ' + 'El motivo no debe exceder los 1000 caracteres');
        } else {
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
                            mostrarMensaje('mensaje', 'exito', "La cancelación del envío fue ejecutada correctamente.");
                            cerrarPopup('popupCancelar');
                            buscarEnvio(EST_CANCELADO);
                        } else {
                            mostrarMensaje('mensaje_popupCancelarEnvio', 'alert', 'Error: ' + evt.Mensaje);
                        }

                    },
                    error: function (err) {
                        mostrarMensaje('mensaje_popupCancelarEnvio', 'error', 'Error al cancelar el envío');
                    }
                });
            }
        }
    }
}


function crearNuevoEnvioCIO() {
    limpiarBarraMensaje("mensaje_popupEquiposCIO");
    var filtro = datosEnvioCIO();
    var msg = validarDatosEnvioCIO(filtro);

    if (msg == "") {
        redireccionarFormulario(0, filtro.idEmpresa, filtro.idEtapa, filtro.idProyecto, filtro.strSeleccionados, "", EDITAR);
    } else {
        mostrarMensaje('mensaje_popupEquiposCIO', 'alert', msg);
    }
}


function datosEnvioCIO() {
    var filtro = {};

    let arrSeleccionados = [];
    $('input[type=checkbox][name^="chkCIO_"]:checked').each(function () {
        arrSeleccionados.push(this.value);
    });
    let strSeleccionados = arrSeleccionados.join(",");

    filtro.NumeroElementosSeleccionados = arrSeleccionados.length;
    filtro.idEmpresa = parseInt($('#cbEmpresaNE').val());
    filtro.idEtapa = parseInt($('#cbEtapaNE').val());
    filtro.idProyecto = parseInt($("#cbProyectoNE").val());
    filtro.strSeleccionados = strSeleccionados;

    return filtro;
}


function validarDatosEnvioCIO(filtro) {

    var msj = "";

    //validaciones generales
    if (filtro.NumeroElementosSeleccionados == 0) {
        msj += "<p>Debe seleccionar como mínimo un equipo.</p>";
    } else {
        if (filtro.idEmpresa == 0) {
            msj += "<p>Debe seleccionar una Empresa.</p>";
        }

        if (filtro.idEtapa == 0) {
            msj += "<p>Debe seleccionar una Etapa.</p>";
        }

        if (filtro.idEtapa != 0 && filtro.idEtapa != ETAPA_MODIFICACION) {
            if (filtro.idProyecto == 0) {
                msj += "<p>Debe seleccionar un Proyecto.</p>";
            }
        }
    }

    return msj;
}

function crearNuevoEnvioM() {
    limpiarBarraMensaje("mensaje_popupEquiposM");
    var filtro = datosEnvioM();
    var msg = validarDatosEnvioM(filtro);

    if (msg == "") {
        redireccionarFormulario(0, filtro.idEmpresa, filtro.idEtapa, 0, filtro.strSeleccionados, filtro.strSelDarBaja, EDITAR);
    } else {
        mostrarMensaje('mensaje_popupEquiposM', 'alert', msg);
    }
}


function datosEnvioM() {
    var filtro = {};

    let arrSeleccionados = [];
    $('input[type=checkbox][name^="chkM_"]:checked').each(function () {
        arrSeleccionados.push(this.value);
    });
    let strSeleccionados = arrSeleccionados.join(",");

    let arrDarBaja = [];
    $('input[name="IR_Darbaja"]:checked').each(function () {
        arrDarBaja.push(this.value);
    });
    let strDarBaja = arrDarBaja.join(",");


    filtro.NumeroElementosSeleccionados = arrSeleccionados.length;
    filtro.NumeroElementosDarBajaSeleccionados = arrDarBaja.length;
    filtro.idEmpresa = parseInt($('#cbEmpresaNE').val());
    filtro.idEtapa = parseInt($('#cbEtapaNE').val());
    filtro.strSeleccionados = strSeleccionados;
    filtro.strSelDarBaja = strDarBaja;

    return filtro;
}


function validarDatosEnvioM(filtro) {

    var msj = "";

    //validaciones generales
    if (filtro.NumeroElementosSeleccionados == 0) {
        msj += "<p>Debe seleccionar como mínimo un equipo.</p>";
    } else {
        if (filtro.NumeroElementosDarBajaSeleccionados > 1) {
            msj += "<p>Solo se debe seleccionar como máximo un equipo para dar de baja.</p>";
        }

        if (filtro.idEmpresa == 0) {
            msj += "<p>Debe seleccionar una Empresa.</p>";
        }

        if (filtro.idEtapa == 0) {
            msj += "<p>Debe seleccionar una Etapa.</p>";
        }
    }

    return msj;
}

////////////////////////////////////////////////
// Mostrar Detalles del envio
////////////////////////////////////////////////
function mostrarDetalle(id, idEtapa, tipoFormato, accion) {
    var strDarBaja = "";
    if (tipoFormato == 3) //dar Baja
        strDarBaja = "XYZ";
    redireccionarFormulario(id, 0, idEtapa, 0, "", strDarBaja, accion);
}

////////////////////////////////////////////////
// Ir a Formulario
////////////////////////////////////////////////
function redireccionarFormulario(idEnvio, idEmpresa, idEtapa, idProyecto, strSelFilas, strSelDarBaja, accion) {

    var form_url = "";

    switch (idEtapa) {
        case ETAPA_CONEXION:
        case ETAPA_INTEGRACION:
            form_url = controlador + "EnvioFormato"; break;  //CU014
        case ETAPA_OPERACION_COMERCIAL:
            form_url = controlador + "EnvioFormatoOperacionComercial"; break; //CU015
        case ETAPA_MODIFICACION:
            if (strSelDarBaja == "") {
                form_url = controlador + "EnvioFormato"; break;  //CU014
            } else {
                form_url = controlador + "EnvioFormatoBajaModoOperacion"; break; //CU016
            }

    }

    $("#frmEnvio").attr("action", form_url);

    $("#hdidEnvioForm").val(idEnvio);
    $("#hdidEmpresaForm").val(idEmpresa);
    $("#hdidEtapaForm").val(idEtapa);
    $("#hdidProyectoForm").val(idProyecto);
    $("#hdidsEquiposForm").val(strSelFilas);
    $("#hdidsEquiposDarBajaForm").val(strSelDarBaja);
    $("#hdAccionForm").val(accion);

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