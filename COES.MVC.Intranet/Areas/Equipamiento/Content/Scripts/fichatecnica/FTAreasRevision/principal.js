var controlador = siteRoot + 'Equipamiento/FTAreasRevision/';

var IMG_EDITAR = '<img src="' + siteRoot + 'Content/Images/btn-edit.png" alt="Editar Envío" title="Editar Envío" width="19" height="19" style="">';
var IMG_VER = '<img src="' + siteRoot + 'Content/Images/btn-open.png" alt="Ver Envío"  title="Ver Envío"width="19" height="19" style="">';
var IMG_DESCARGAR_EXCEL = '<img src="' + siteRoot + 'Content/Images/ExportExcel.png" alt="Descargar Detalle" title="Descargar Detalle" width="19" height="19" style="">';
var IMG_CANCELAR = '<img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="Cancelar Envío" title="Cancelar Envío" width="15" height="19" style="">';

var IDS_AREAS_DEL_USUARIO;

const EST_SOLICITUD = 1;
const EST_SUBSANACION = 7;

const EST_PENDIENTE = 1;
const EST_ATENDIDO = 2;

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
    
    var areasUser = $('#hdIdsAreaDelUsuario').val();
    if (areasUser == "") {
        IDS_AREAS_DEL_USUARIO = "0"; //para evitar mostrar mensajes de validacion cuando el usuario no tenga areas asociados
    } else {
        IDS_AREAS_DEL_USUARIO = areasUser;
    }

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

    $("#btnManualUsuario").click(function () {
        window.location = controlador + 'DescargarManualUsuario';
    });


    buscarEnvio(); // muestra el listado de todos los envios de todas las empresas el filtro ingresado
});

////////////////////////////////////////////////
// Reporte Envio
////////////////////////////////////////////////
function buscarEnvio(idCarpeta) {
    idCarpeta = parseInt(idCarpeta) || 0;
    if (idCarpeta > 0) {
        $("#hdIdEstado").val(idCarpeta);
    }
    idCarpeta = $("#hdIdEstado").val() != undefined ? parseInt($("#hdIdEstado").val()) : 1;

    mostrarBloqueEnvios(idCarpeta);
}


function mostrarBloqueEnvios(idCarpeta) {
    limpiarBarraMensaje("mensaje");

    var filtro = datosFiltro(idCarpeta);
    var msg = validarDatosFiltroEnvios(filtro);

    if (msg == "") {
        

        $.ajax({
            type: 'POST',
            url: controlador + "bloqueEnviosAreas",
            dataType: 'json',
            data: {
                areasUsuario: IDS_AREAS_DEL_USUARIO,
                empresas: $('#hfEmpresa').val(),
                ftetcodi: filtro.etapa,
                idCarpeta: parseInt(filtro.idCarpeta),
                finicios: filtro.finicio,
                ffins: filtro.ffin
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {

                    var html = "<h3>Carpetas</h3>";
                    html += evt.HtmlCarpeta;
                    $("#div_carpetas").html(html);

                    var htmlEnvios = dibujarTablaReporte(evt.ListadoEnvios, idCarpeta);
                    $("#reporte").html(htmlEnvios);
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

function datosFiltro(idCarpeta) {
    var filtro = {};

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var etapa = parseInt($('#cbEtapa').val()) || 0;    
    var finicio = $('#FechaDesde').val();
    var ffin = $('#FechaHasta').val();
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    $('#hfEstado').val(idCarpeta);
    $('#hfEtapa').val(etapa);

    filtro.empresa = empresa;
    filtro.etapa = etapa;
    filtro.idCarpeta = idCarpeta;
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


function dibujarTablaReporte(lista, estado) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaEnvios" style="width:100%;">
       <thead>
           <tr>
               <th style='width:40px;white-space: inherit;'>Acciones</th>
               <th style="width:40px;white-space: inherit;">Área</th>
               <th style="width:40px;white-space: inherit;">Código<br>del Envío</th>
               <th style="width:40px;white-space: inherit;">Tipo<br>de Envío</th>
               <th style="width:60px;white-space: inherit;">Empresa</th>
               <th style="width:50px;white-space: inherit;">Etapa</th>
               <th style="width:70px;white-space: inherit;">Nombre <br>Proyecto</th>
               <th style="width:70px;white-space: inherit;">Equipo(s) Proyecto</th>
               <th style="width:70px;white-space: inherit;">Nombre Equipo(s)</th>
               <th style="width:60px;white-space: inherit;">Usuario</th>
               <th style="width:60px;white-space: inherit;">Fecha de<br>Solicitud</th>
               <th style="width:60px;white-space: inherit;">Fecha de<br>Actualización</th>    
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

       
        if (item.EsEditableParaAreas) {

            cadena += `
                        <a title="Editar Envío" href="JavaScript:mostrarDetalle(${item.Ftenvcodi}, ${item.Faremcodi}, ${item.Ftetcodi}, ${item.Ftenvtipoformato}, ${item.Estenvcodiversion});">${IMG_EDITAR}</a>
                     `;
        }
        else {
            cadena += `
                   <a title="Ver Envío" href="JavaScript:mostrarDetalle(${item.Ftenvcodi}, ${item.Faremcodi}, ${item.Ftetcodi}, ${item.Ftenvtipoformato}, ${item.Estenvcodiversion});">${IMG_VER}</a>
                `;
        }        

        cadena += `            
                </td>
                <td style="width:40px;white-space: inherit;">${item.Faremnombre}</td>
                <td style="width:40px;white-space: inherit;">${item.Ftenvcodi}</td>
                <td style="width:40px;white-space: inherit;">${item.Estenvnombversion}</td>
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

        //Fecha de Actualización
        if (item.Estenvcodiversion == EST_SUBSANACION) {            
            cadena += `
               <td style="width:60px;white-space: inherit;">${item.FtenvfecmodificacionDesc}</td>
        `;
        } else {
            cadena += `
               <td style="width:60px;white-space: inherit;"></td>
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



//exportar
function exportar() {
    limpiarBarraMensaje("mensaje");

    var idCarpeta = $("#hdIdEstado").val() != undefined ? parseInt($("#hdIdEstado").val()) : 1;

    var filtro = datosFiltro(idCarpeta);
    var msg = validarDatosFiltroEnvios(filtro);

    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'GenerarArchivoReporte',
            data: {
                areasUsuario: IDS_AREAS_DEL_USUARIO,
                empresas: $('#hfEmpresa').val(),
                ftetcodi: filtro.etapa,
                idCarpeta: parseInt(filtro.idCarpeta),
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
function mostrarDetalle(idEnvio, faremcodi, idEtapa, tipoFormato, estenvcodiversion) {
    var strDarBaja = "";
    if (tipoFormato == 3) //dar Baja
        strDarBaja = "XYZ";
    redireccionarFormulario(idEnvio, faremcodi, idEtapa, strDarBaja, estenvcodiversion);
}

////////////////////////////////////////////////
// Ir a Formulario
////////////////////////////////////////////////
function redireccionarFormulario(idEnvio, faremcodi, idEtapa, strDarBaja, estenvcodiversion) {

    var form_url = "";
    switch (idEtapa) {
        case ETAPA_CONEXION:
        case ETAPA_INTEGRACION:
            form_url = controlador + "AreaEnvioFormato"; break;  //CU0
        case ETAPA_OPERACION_COMERCIAL:
            form_url = controlador + "AreaEnvioFormatoOperacionComercial"; break; //CU0
        case ETAPA_MODIFICACION:
            if (strDarBaja == "") {
                form_url = controlador + "AreaEnvioFormato"; break;  //CU0
            } else {
                form_url = controlador + "AreaEnvioFormatoBajaModoOperacion"; break; //CU0
            }
    } 

    document.location.href = form_url + "?codigoEnvio=" + idEnvio + '&area=' + faremcodi + '&tipoEnvio=' + estenvcodiversion;
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