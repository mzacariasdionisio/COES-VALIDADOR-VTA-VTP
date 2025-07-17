var controlador = siteRoot + 'Equipamiento/FTCorreo/';

const NOTIFICACION_CULMINACION_PLAZO_SUBSANAR_CONEXION = 221;
const NOTIFICACION_CULMINACION_PLAZO_SUBSANAR_INTEGRACION = 222;
const NOTIFICACION_CULMINACION_PLAZO_SUBSANAR_OPERACIONCOMERCIAL = 223;
const NOTIFICACION_CULMINACION_PLAZO_SUBSANAR_MFT = 224;
const NOTIFICACION_CULMINACION_PLAZO_SUBSANAR_MFT_DB = 225;

const RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_CONEXION = 229;
const RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_INTEGRACION = 230;
const RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_OPERACIONCOMERCIAL = 231;
const RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_MFT = 232;
const RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_MFT_DB = 233;

const RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_CONEXION = 234;
const RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_INTEGRACION = 235;
const RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_OPERACIONCOMERCIAL = 236;
const RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_MFT = 237;
const RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_MFT_DB = 238;

const RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_AREAS_CONEXION = 252;
const RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_AREAS_INTEGRACION = 253;
const RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_AREAS_OPERACIONCOMERCIAL = 254;
const RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_AREAS_MFT = 255;
const RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_AREAS_MFT_DB = 256;

const RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_AREAS_CONEXION = 267;
const RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_AREAS_INTEGRACION = 268;
const RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_AREAS_OPERACIONCOMERCIAL = 269;
const RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_AREAS_MFT = 270;
const RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_AREAS_MFT_DB = 271;

const NOTIFICACION_CULMINACION_PLAZO_REVISION_AREAS_SOLICITUD_CONEXION = 262;
const NOTIFICACION_CULMINACION_PLAZO_REVISION_AREAS_SOLICITUD_INTEGRACION = 263;
const NOTIFICACION_CULMINACION_PLAZO_REVISION_AREAS_SOLICITUD_OPERACIONCOMERCIAL = 264;
const NOTIFICACION_CULMINACION_PLAZO_REVISION_AREAS_SOLICITUD_MFT = 265;
const NOTIFICACION_CULMINACION_PLAZO_REVISION_AREAS_SOLICITUD_MFT_DB = 266;

const NOTIFICACION_CULMINACION_PLAZO_REVISION_AREAS_SUBSANACION_CONEXION = 277;
const NOTIFICACION_CULMINACION_PLAZO_REVISION_AREAS_SUBSANACION_INTEGRACION = 278;
const NOTIFICACION_CULMINACION_PLAZO_REVISION_AREAS_SUBSANACION_OPERACIONCOMERCIAL = 279;
const NOTIFICACION_CULMINACION_PLAZO_REVISION_AREAS_SUBSANACION_MFT = 280;
const NOTIFICACION_CULMINACION_PLAZO_REVISION_AREAS_SUBSANACION_MFT_DB = 281;

const VAR_FIRMA_COES = "{FIRMA_COES}";
const VAR_CODIGO_ENVIO = "{CODIGO_ENVIO}";  ////
const VAR_NOMBRE_EMPRESA_ENVIO = "{NOMBRE_EMPRESA_ENVIO}"; ////
const VAR_NOMBRE_PROYECTO = "{NOMBRE_PROYECTO}"; ////
const VAR_NOMBRE_EQUIPO_PROYECTO = "{NOMBRE_EQUIPO_PROYECTO}"; ////
const VAR_NOMBRE_ETAPA = "{NOMBRE_ETAPA}"; ////
const VAR_FECHA_SOLICITUD = "{FECHA_SOLICITUD}"; ////
const VAR_FECHA_CANCELACION = "{FECHA_CANCELACION}"; ////
const VAR_CORREO_USUARIO_SOLICITUD = "{CORREO_USUARIO_SOLICITUD}"; ////
const VAR_TABLA_EQUIPO_PARAMETRO_MODIF_FT = "{TABLA_EQUIPO_PARAMETRO_MODIF_FT}"; ////
const VAR_FECHA_REVISION = "{FECHA_REVISION}";//
const VAR_FECHA_MAX_RPTA = "{FECHA_MAXIMA_RESPUESTA}";//
const VAR_EQUIPOS_MODIFICADOS = "{EQUIPOS_MODIFICADOS}";//

const VAR_CORREO_USUARIO_ULTIMO_EVENTO = "{CORREO_USUARIO_ULTIMO_EVENTO}";//
const VAR_CORREOS_CC_AGENTES = "{CORREOS_CC_AGENTES}";//
const VAR_FECHA_DENEGACION = "{FECHA_DENEGACION}";//
const VAR_MENSAJE_AL_AGENTE = "{MENSAJE_AL_AGENTE}";//
const VAR_FECHA_APROBACION= "{FECHA_APROBACION}";//

const VAR_NOMBRE_EQUIPOS = "{NOMBRE_EQUIPOS}"; //
const VAR_FECHA_SUBSANACION_OBS = "{FECHA_SUBSANACION_OBS}";//

const VAR_FECHA_INICIO_DE_PLAZO = "{FECHA_INICIO_DE_PLAZO}";//
const VAR_FECHA_FINAL_DE_PLAZO = "{FECHA_FINAL_DE_PLAZO}";//

const VAR_TABLA_EQUIPO_PARAMETRO_MODIF_FT_APROBADO = "{TABLA_EQUIPO_PARAMETRO_MODIF_FT_APROBADO}";//
const VAR_TABLA_EQUIPO_PARAMETRO_MODIF_FT_DENEGADO = "{TABLA_EQUIPO_PARAMETRO_MODIF_FT_DENEGADO}";//
const VAR_FECHA_APROBACION_PARCIAL = "{FECHA_APROBACION_PARCIAL}";//

const VAR_CORREO_USUARIO_LT_OTRO_AGENTE = "{CORREO_USUARIO_LT_OTRO_AGENTE}"; //

const VAR_MODO_OPERACION_BAJA = "{MODO_OPERACION_BAJA}"; //

const VAR_NUMERO_DIAS_RECEPCION_SOLICITUD = "{NUMERO_DIAS_RECEPCION_SOLICITUD}"; //
const VAR_NUMERO_DIAS_RECEPCION_SUBSANACION = "{NUMERO_DIAS_RECEPCION_SUBSANACION}"; //

const VAR_CORREOS_ADMIN_FT = "{CORREOS_ADMIN_FT}"; //
const VAR_CORREOS_AREAS_COES_SOLICITUD = "{CORREOS_AREAS_COES_SOLICITUD}";//
const VAR_FECHA_MAX_RPTA_DERIVACION = "{FECHA_MAXIMA_RESPUESTA_DERIVACION}";//
const VAR_CORREOS_AREAS_COES_SUBSANADO = "{CORREOS_AREAS_COES_SUBSANADO}";//

const VAR_CORREOS_AREAS_ASIGNADOS_PENDIENTE_REVISION = "{CORREOS_AREAS_ASIGNADOS_PENDIENTE_REVISION}";//
const VAR_NOMBRE_AREA_ASIGNADA_PENDIENTE_REVISION = "{NOMBRE_AREA_ASIGNADA_PENDIENTE_REVISION}";//
const VAR_NUMERO_DIAS_FALTANTES_VENCIMIENTO_PLAZO_REVISION_AREAS = "{NUMERO_DIAS_FALTANTES_VENCIMIENTO_PLAZO_REVISION_AREAS}";//

const VAR_CORREOS_DEL_AREA_ASIGNADA_QUIEN_REALIZA_REVISION = "{CORREOS_DEL_AREA_ASIGNADA_QUIEN_REALIZA_REVISION}";//
const VAR_NOMBRE_AREA_ASIGNADA_QUIEN_REALIZA_REVISION = "{NOMBRE_AREA_ASIGNADA_QUIEN_REALIZA_REVISION}";//38

const VAR_CORREOS_DEL_AREA_ASIGNADA_QUIENES_DEBIERON_REVISAR = "{CORREOS_DEL_AREA_ASIGNADA_QUIENES_DEBIERON_REVISAR}";//
const VAR_NOMBRE_AREA_ASIGNADA_QUIEN_DEBIO_REVISAR = "{NOMBRE_AREA_ASIGNADA_QUIEN_DEBIO_REVISAR}";//

const VAR_FECHA_CARTA_SOLICITUD = "{FECHA_CARTA_SOLICITUD}";//
const VAR_FECHA_CARTA_SUBSANACION = "{FECHA_CARTA_SUBSANACION}";//

var lstVariablesCorreo = [
    VAR_CODIGO_ENVIO, VAR_NOMBRE_EMPRESA_ENVIO, VAR_NOMBRE_PROYECTO, VAR_NOMBRE_EQUIPO_PROYECTO,
    VAR_NOMBRE_ETAPA, VAR_FECHA_SOLICITUD, VAR_FECHA_CANCELACION, VAR_CORREO_USUARIO_SOLICITUD, VAR_TABLA_EQUIPO_PARAMETRO_MODIF_FT,
    VAR_FECHA_REVISION, VAR_FECHA_MAX_RPTA, VAR_EQUIPOS_MODIFICADOS, VAR_CORREO_USUARIO_ULTIMO_EVENTO, VAR_CORREOS_CC_AGENTES,
    VAR_FECHA_DENEGACION, VAR_MENSAJE_AL_AGENTE, VAR_FECHA_APROBACION, VAR_NOMBRE_EQUIPOS, VAR_FECHA_SUBSANACION_OBS,
    VAR_FECHA_INICIO_DE_PLAZO, VAR_FECHA_FINAL_DE_PLAZO, VAR_TABLA_EQUIPO_PARAMETRO_MODIF_FT_APROBADO, VAR_TABLA_EQUIPO_PARAMETRO_MODIF_FT_DENEGADO, VAR_FECHA_APROBACION_PARCIAL,

    VAR_CORREO_USUARIO_LT_OTRO_AGENTE, VAR_MODO_OPERACION_BAJA, VAR_NUMERO_DIAS_RECEPCION_SOLICITUD, VAR_NUMERO_DIAS_RECEPCION_SUBSANACION,
    VAR_CORREOS_ADMIN_FT, VAR_CORREOS_AREAS_COES_SOLICITUD, VAR_FECHA_MAX_RPTA_DERIVACION, VAR_CORREOS_AREAS_COES_SUBSANADO,

    VAR_CORREOS_AREAS_ASIGNADOS_PENDIENTE_REVISION, VAR_NOMBRE_AREA_ASIGNADA_PENDIENTE_REVISION, VAR_NUMERO_DIAS_FALTANTES_VENCIMIENTO_PLAZO_REVISION_AREAS,
    VAR_CORREOS_DEL_AREA_ASIGNADA_QUIEN_REALIZA_REVISION, VAR_NOMBRE_AREA_ASIGNADA_QUIEN_REALIZA_REVISION,
    VAR_CORREOS_DEL_AREA_ASIGNADA_QUIENES_DEBIERON_REVISAR, VAR_NOMBRE_AREA_ASIGNADA_QUIEN_DEBIO_REVISAR,

    VAR_FECHA_CARTA_SOLICITUD, VAR_FECHA_CARTA_SUBSANACION
];



function listarVariableCorreoTotales() {
    var lista = [];

    var logoEmail = $("#hfLogoEmail").val();

    lista.push({
        Valor: VAR_FIRMA_COES,
        Nombre: 'Firma - {FIRMA_COES}',
        ContenidoHtml: `<div>
                   <p class='MsoNormal'><u></u>&nbsp;<u></u></p>
                   
                   <p class='MsoNormal'>
		                <img alt='Logo Coes' width='127' height='66' src='${logoEmail}'  class='CToWUd'><u></u><u></u></p>
	  
                   <p class='MsoNormal'><span style='font-size:11.0pt;font-weight: bold;'>
                      <u></u><u></u></span>
                   </p>
   
                   <p class='MsoNormal'><span style='font-size:8.0pt'><u></u><u></u></span></p>
   
                   <p class='MsoNormal'><b><span style='font-size:8.0pt;color:#0077a5'>D:</span></b><span style='font-size:8.0pt'> Av. Los Conquistadores N° 1144, San Isidro, Lima - Perú
                      <u></u><u></u></span>
                   </p>
   
                   <p class='MsoNormal'><b><span style='font-size:8.0pt;color:#0077a5'>T:</span></b><span style='font-size:8.0pt;color:#0077a5'>
                      </span><span style='font-size:8.0pt'>+51 611 8585 - Anexo: 657 / 593 <u></u><u></u></span>
                   </p>
   
                   <p class='MsoNormal'><b><span style='font-size:8.0pt;color:#0077a5'>W:</span></b><span style='font-size:8.0pt;color:#0077a5'>
                      </span><a href='http://www.coes.org.pe' target='_blank'><span style='font-size:8.0pt;color:#0563c1'>www.coes.org.pe</span></a><span style='font-size:8.0pt'>
                      <u></u><u></u></span>
                   </p>
                </div>
        `
    });

    lista.push({ Valor: VAR_CODIGO_ENVIO, Nombre: 'CODIGO_ENVIO' });
    lista.push({ Valor: VAR_NOMBRE_EMPRESA_ENVIO, Nombre: 'NOMBRE_EMPRESA_ENVIO' });
    lista.push({ Valor: VAR_NOMBRE_PROYECTO, Nombre: 'NOMBRE_PROYECTO' });
    lista.push({ Valor: VAR_NOMBRE_EQUIPO_PROYECTO, Nombre: 'NOMBRE_EQUIPO_PROYECTO' });
    lista.push({ Valor: VAR_NOMBRE_ETAPA, Nombre: 'NOMBRE_ETAPA' });
    lista.push({ Valor: VAR_FECHA_SOLICITUD, Nombre: 'FECHA_SOLICITUD' });
    lista.push({ Valor: VAR_FECHA_CANCELACION, Nombre: 'FECHA_CANCELACION' });    
    lista.push({ Valor: VAR_CORREO_USUARIO_SOLICITUD, Nombre: 'CORREO_USUARIO_SOLICITUD' });
    lista.push({ Valor: VAR_TABLA_EQUIPO_PARAMETRO_MODIF_FT, Nombre: 'TABLA_EQUIPO_PARAMETRO_MODIF_FT' });
    lista.push({ Valor: VAR_FECHA_REVISION, Nombre: 'FECHA_REVISION' });
    lista.push({ Valor: VAR_FECHA_MAX_RPTA, Nombre: 'FECHA_MAXIMA_RESPUESTA' });
    lista.push({ Valor: VAR_EQUIPOS_MODIFICADOS, Nombre: 'EQUIPOS_MODIFICADOS' });
    lista.push({ Valor: VAR_CORREO_USUARIO_ULTIMO_EVENTO, Nombre: 'CORREO_USUARIO_ULTIMO_EVENTO' });
    lista.push({ Valor: VAR_CORREOS_CC_AGENTES, Nombre: 'CORREOS_CC_AGENTES' });
    lista.push({ Valor: VAR_FECHA_DENEGACION, Nombre: 'FECHA_DENEGACION' });
    lista.push({ Valor: VAR_MENSAJE_AL_AGENTE, Nombre: 'MENSAJE_AL_AGENTE' });
    lista.push({ Valor: VAR_FECHA_APROBACION, Nombre: 'FECHA_APROBACION' });

    lista.push({ Valor: VAR_NOMBRE_EQUIPOS, Nombre: 'NOMBRE_EQUIPOS' });
    lista.push({ Valor: VAR_FECHA_SUBSANACION_OBS, Nombre: 'FECHA_SUBSANACION_OBS' });

    lista.push({ Valor: VAR_FECHA_INICIO_DE_PLAZO, Nombre: 'FECHA_INICIO_DE_PLAZO' });
    lista.push({ Valor: VAR_FECHA_FINAL_DE_PLAZO, Nombre: 'FECHA_FINAL_DE_PLAZO' });
    lista.push({ Valor: VAR_TABLA_EQUIPO_PARAMETRO_MODIF_FT_APROBADO, Nombre: 'TABLA_EQUIPO_PARAMETRO_MODIF_FT_APROBADO' });
    lista.push({ Valor: VAR_TABLA_EQUIPO_PARAMETRO_MODIF_FT_DENEGADO, Nombre: 'TABLA_EQUIPO_PARAMETRO_MODIF_FT_DENEGADO' });
    lista.push({ Valor: VAR_FECHA_APROBACION_PARCIAL, Nombre: 'FECHA_APROBACION_PARCIAL' });

    lista.push({ Valor: VAR_CORREO_USUARIO_LT_OTRO_AGENTE, Nombre: 'CORREO_USUARIO_LT_OTRO_AGENTE' }); 
    
    lista.push({ Valor: VAR_MODO_OPERACION_BAJA, Nombre: 'MODO_OPERACION_BAJA' });

    lista.push({ Valor: VAR_NUMERO_DIAS_RECEPCION_SOLICITUD, Nombre: 'NUMERO_DIAS_RECEPCION_SOLICITUD' });
    lista.push({ Valor: VAR_NUMERO_DIAS_RECEPCION_SUBSANACION, Nombre: 'NUMERO_DIAS_RECEPCION_SUBSANACION' });
    
    lista.push({ Valor: VAR_FECHA_MAX_RPTA_DERIVACION, Nombre: 'FECHA_MAXIMA_RESPUESTA_DERIVACION' });

    lista.push({ Valor: VAR_NOMBRE_AREA_ASIGNADA_PENDIENTE_REVISION, Nombre: 'NOMBRE_AREA_ASIGNADA_PENDIENTE_REVISION' });
    lista.push({ Valor: VAR_NUMERO_DIAS_FALTANTES_VENCIMIENTO_PLAZO_REVISION_AREAS, Nombre: 'NUMERO_DIAS_FALTANTES_VENCIMIENTO_PLAZO_REVISION_AREAS' });
    lista.push({ Valor: VAR_NOMBRE_AREA_ASIGNADA_QUIEN_REALIZA_REVISION, Nombre: 'NOMBRE_AREA_ASIGNADA_QUIEN_REALIZA_REVISION' });
    lista.push({ Valor: VAR_NOMBRE_AREA_ASIGNADA_QUIEN_DEBIO_REVISAR, Nombre: 'NOMBRE_AREA_ASIGNADA_QUIEN_DEBIO_REVISAR' });

    lista.push({ Valor: VAR_FECHA_CARTA_SOLICITUD, Nombre: 'FECHA_CARTA_SOLICITUD' });
    lista.push({ Valor: VAR_FECHA_CARTA_SUBSANACION, Nombre: 'FECHA_CARTA_SUBSANACION' });

    return lista;
}

function listarVariableSoloCorreos() {
    var lista = [];
    lista.push({ Valor: VAR_CORREO_USUARIO_SOLICITUD, Nombre: 'CORREO_USUARIO_SOLICITUD' });
    //lista.push({ Valor: VAR_CORREO_AREA_COES, Nombre: 'CORREO_AREA_COES' });
    lista.push({ Valor: VAR_CORREOS_CC_AGENTES, Nombre: 'CORREOS_CC_AGENTES' });
    lista.push({ Valor: VAR_CORREO_USUARIO_ULTIMO_EVENTO, Nombre: 'CORREO_USUARIO_ULTIMO_EVENTO' });
    lista.push({ Valor: VAR_CORREO_USUARIO_LT_OTRO_AGENTE, Nombre: 'CORREO_USUARIO_LT_OTRO_AGENTE' });
    lista.push({ Valor: VAR_CORREOS_ADMIN_FT, Nombre: 'CORREOS_ADMIN_FT' });
    lista.push({ Valor: VAR_CORREOS_AREAS_COES_SOLICITUD, Nombre: 'CORREOS_AREAS_COES_SOLICITUD' });
    lista.push({ Valor: VAR_CORREOS_AREAS_COES_SUBSANADO, Nombre: 'CORREOS_AREAS_COES_SUBSANADO' });
    lista.push({ Valor: VAR_CORREOS_AREAS_ASIGNADOS_PENDIENTE_REVISION, Nombre: 'CORREOS_AREAS_ASIGNADOS_PENDIENTE_REVISION' });
    lista.push({ Valor: VAR_CORREOS_DEL_AREA_ASIGNADA_QUIEN_REALIZA_REVISION, Nombre: 'CORREOS_DEL_AREA_ASIGNADA_QUIEN_REALIZA_REVISION' });
    lista.push({ Valor: VAR_CORREOS_DEL_AREA_ASIGNADA_QUIENES_DEBIERON_REVISAR, Nombre: 'CORREOS_DEL_AREA_ASIGNADA_QUIENES_DEBIERON_REVISAR' });
    

    return lista;
}

var lstVariablesCC = [    
    VAR_CORREO_USUARIO_SOLICITUD, VAR_CORREO_USUARIO_LT_OTRO_AGENTE, VAR_CORREOS_CC_AGENTES, VAR_CORREO_USUARIO_ULTIMO_EVENTO, VAR_CORREOS_ADMIN_FT,
    VAR_CORREOS_AREAS_COES_SOLICITUD, VAR_CORREOS_AREAS_COES_SUBSANADO, VAR_CORREOS_AREAS_ASIGNADOS_PENDIENTE_REVISION, VAR_CORREOS_DEL_AREA_ASIGNADA_QUIEN_REALIZA_REVISION,
    VAR_CORREOS_DEL_AREA_ASIGNADA_QUIENES_DEBIERON_REVISAR
];

var lstVariablesPara = [
    VAR_CORREO_USUARIO_SOLICITUD, VAR_CORREO_USUARIO_LT_OTRO_AGENTE, VAR_CORREOS_CC_AGENTES, VAR_CORREO_USUARIO_ULTIMO_EVENTO, VAR_CORREOS_ADMIN_FT,
    VAR_CORREOS_AREAS_COES_SOLICITUD, VAR_CORREOS_AREAS_COES_SUBSANADO, VAR_CORREOS_AREAS_ASIGNADOS_PENDIENTE_REVISION, VAR_CORREOS_DEL_AREA_ASIGNADA_QUIEN_REALIZA_REVISION,
    VAR_CORREOS_DEL_AREA_ASIGNADA_QUIENES_DEBIERON_REVISAR
];

const NOTIFICACION = 1;
const RECORDATORIO = 2;

const TPCORRCODI_RECORDATORIO = 5;

const VER = 1;
const EDITAR = 2;

const ENVIO_AUTOMATICO = 0;
const ENVIO_MANUAL = 1;

const VARIABLE_ASUNTO = 0;
const VARIABLE_CC = 2;
const VARIABLE_PARA = 3;

const SEPARADOR_INI_VARIABLE = "{";// Debe coincidir con separadores de los valores en ConstantesCombustibles
const SEPARADOR_FIN_VARIABLE = "}";// Debe coincidir con separadores de los valores en ConstantesCombustibles

var IMG_EDITAR = '<img src="' + siteRoot + 'Content/Images/btn-edit.png" alt="Editar Plantilla" title="Editar Plantilla" width="19" height="19" style="">';
var IMG_VER = '<img src="' + siteRoot + 'Content/Images/btn-open.png" alt="Ver Plantilla" title="Ver Plantilla" width="19" height="19" style="">';
var tipoPlantillaEnListado = -1; //Notificacion o Recordatorio
var plantillaEnDetalle = -1; //iD
var tipoCentralEnListado = -1; //Existente o Nueva
var tipoEnvioEnDetalles = -1; // Automatico o Manual
var respondeATodos = "";
var listarVariableCorreoOrdenada = [];
var listarVariableSoloCorreosOrdenada = [];

$(function () {
    //ordeno lista variables por nombre    
    listarVariableCorreoOrdenada = listarVariableCorreoTotales().sort((a, b) => {
        let fa = a.Nombre.toLowerCase(),
            fb = b.Nombre.toLowerCase();

        if (fa < fb) {
            return -1;
        }
        if (fa > fb) {
            return 1;
        }
        return 0;
    });

    listarVariableSoloCorreosOrdenada = listarVariableSoloCorreos().sort((a, b) => {
        let fa = a.Nombre.toLowerCase(),
            fb = b.Nombre.toLowerCase();

        if (fa < fb) {
            return -1;
        }
        if (fa > fb) {
            return 1;
        }
        return 0;
    });

    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').easytabs('select', '#tabLista');

    $('#cbTipoPlantilla').change(function () {
        $('#tab-container').easytabs('select', '#tabLista');
        $("#div_detalle").html('');
        mostrarListadoPlantilla();
    });

    $('#cbEtapa').change(function () {
        $('#tab-container').easytabs('select', '#tabLista');
        $("#div_detalle").html('');
        mostrarListadoPlantilla();
    });

    mostrarListadoPlantilla();
});

function mostrarListadoPlantilla() {
    var tpcorrcodi = parseInt($("#cbTipoPlantilla").val()) || 0;
    var ftetcodi = parseInt($("#cbEtapa").val()) || 0;
    limpiarBarraMensaje("mensaje_Detalle");
    limpiarBarraMensaje("mensaje_Listado");

    tipoPlantillaEnListado = parseInt($("#cbTipoPlantilla").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "ListarPlantillaCorreo",
        dataType: 'json',
        data: {
            tpcorrcodi: tpcorrcodi,
            ftetcodi: ftetcodi,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                var htmlPlantillas = dibujarTablaPlantillas(evt.ListadoPlantillasCorreo, evt.AccionGrabar);
                $("#div_listado").html(htmlPlantillas);

                $('#tablaPlantillas').dataTable({
                    "scrollY": 480,
                    "scrollX": false,
                    "sDom": 't',
                    "ordering": false,
                    "iDisplayLength": -1
                });

            } else {
                mostrarMensaje('mensaje_Listado', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje_Listado', 'error', 'Ha ocurrido un error.');
        }
    });

}

function dibujarTablaPlantillas(listaPlantillas, esAdmin) {
    var num = 0;

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaPlantillas" >
       <thead>
           <tr style="height: 22px;">
               <th style='width:90px;'>Acciones</th>
               <th>N°</th>
               <th>Código de Plantilla</th>
               <th>Nombre de Plantilla</th>
               <th>Usuario Modificación</th>
               <th>Fecha de Modificación</th>        
            </tr>
        </thead>
        <tbody>
    `;

    for (key in listaPlantillas) {
        var item = listaPlantillas[key];
        num++;

        var htmlTdAccion = "";
        if (esAdmin && item.Plantcodi > 63) {
            htmlTdAccion = `
                    <a href="JavaScript:mostrarDetalle(${item.Plantcodi}, ${VER});">${IMG_VER}</a>
                    <a href="JavaScript:mostrarDetalle(${item.Plantcodi}, ${EDITAR});">${IMG_EDITAR}</a>
            `;
        } else {
            htmlTdAccion = `
                    <a href="JavaScript:mostrarDetalle(${item.Plantcodi}, ${VER});">${IMG_VER}</a>
            `;
        }

        cadena += `
            <tr>
                <td style='width:90px;'>   
                    ${htmlTdAccion}
                </td>
                <td>${num}</td>
                <td>${item.Plantcodi}</td>
                <td style="text-align:left;">${item.Plantnomb}</td>
                <td>${item.UltimaModificacionUsuarioDesc}</td>
                <td>${item.UltimaModificacionFechaDesc}</td>
           </tr >           
        `;
    }


    cadena += `                   
        </tbody >
    </table >
    `;

    return cadena;
}

function mostrarDetalle(plantillacodi, accion) {
    $("#div_detalle").html('');
    limpiarBarraMensaje("mensaje_Detalle");
    limpiarBarraMensaje("mensaje_Listado");
    $('#tab-container').easytabs('select', '#tabDetalle');
    plantillaEnDetalle = plantillacodi;
    
    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerDetalleCorreo",
        data: {
            plantillacodi: plantillacodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                respondeATodos = evt.PlantillaCorreo.RespondeAAgente;

                var htmlPlantilla = dibujarPlantilla(evt.PlantillaCorreo, plantillacodi, accion, ENVIO_AUTOMATICO, evt.TipoCorreo);
                $("#div_detalle").html(htmlPlantilla);

                //habilito/deshabilito edicion de contenido
                var readonly = 0;
                if (accion == VER) readonly = 1;

                //muestro editor de contenido
                evt.ListaVariables = listarVariableCorreoOrdenada;
                iniciarControlTexto('Contenido', evt.ListaVariables, readonly);

                $('#HoraR').Zebra_DatePicker({
                    format: "H:i",
                });

                $('#btnGuardarPC').click(function () {
                    guardarPlantilla();
                });

            } else {
                mostrarMensaje('mensaje_Listado', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje_Listado', 'error', 'Ha ocurrido un error.');
        }
    });
}

function dibujarPlantilla(objPlantilla, plantillacodi, accion, tipoEnvio, tipoCorreo) {

    var esEditable = true;
    var habilitacion = "";

    if (accion == VER) {
        esEditable = false;
        habilitacion = "disabled";
    }
    tipoEnvioEnDetalles = tipoEnvio;

    //horas y dias
    var val_DiasRecepcion = objPlantilla.ParametroDiaHora;  //Paso el mismo valor xq al pintarse solo uno lo hace (solo un plantcodi se muestra)
    //var val_HoraCulminacion = objPlantilla.ParametroDiaHora;  //Paso el mismo valor xq al pintarse solo uno lo hace (solo un plantcodi se muestra)

    //Formato 
    var val_To = objPlantilla.Planticorreos != null ? objPlantilla.Planticorreos : "";
    var val_CC = objPlantilla.PlanticorreosCc != null ? objPlantilla.PlanticorreosCc : "";
    var val_BCC = objPlantilla.PlanticorreosBcc != null ? objPlantilla.PlanticorreosBcc : "";
    var val_Asunto = objPlantilla.Plantasunto != null ? objPlantilla.Plantasunto : "";
    var val_Contenido = objPlantilla.Plantcontenido != null ? objPlantilla.Plantcontenido : "";

    //campo hora y estado
    var val_Hora = objPlantilla.Hora != null ? objPlantilla.Hora : "";
    var val_Estado = objPlantilla.EstadoRecordatorio != null ? objPlantilla.EstadoRecordatorio : "";
    var seleccionA = "";
    var seleccionI = "";
    if (val_Estado == "A")
        seleccionA = "selected";
    if (val_Estado == "I")
        seleccionI = "selected";

    //campo De
    var val_De = objPlantilla.PlanticorreoFrom != null ? objPlantilla.PlanticorreoFrom : "";
    var ftetcodi = parseInt($("#cbEtapa").val()) || 0;
    var cadena = '';

    
    //Agrego boton para ejecutar recordatorios
    //Tambien agrego boton para ejecutar recordatorios de alguns notificaciones
    if (plantillacodi == NOTIFICACION_CULMINACION_PLAZO_SUBSANAR_CONEXION || plantillacodi == NOTIFICACION_CULMINACION_PLAZO_SUBSANAR_INTEGRACION || plantillacodi == NOTIFICACION_CULMINACION_PLAZO_SUBSANAR_OPERACIONCOMERCIAL
        || plantillacodi == NOTIFICACION_CULMINACION_PLAZO_SUBSANAR_MFT || plantillacodi == NOTIFICACION_CULMINACION_PLAZO_SUBSANAR_MFT_DB ||
        plantillacodi == RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_CONEXION || plantillacodi == RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_INTEGRACION || plantillacodi == RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_OPERACIONCOMERCIAL
        || plantillacodi == RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_MFT || plantillacodi == RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_MFT_DB ||
        plantillacodi == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_CONEXION || plantillacodi == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_INTEGRACION || plantillacodi == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_OPERACIONCOMERCIAL
        || plantillacodi == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_MFT || plantillacodi == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_MFT_DB ||
        plantillacodi == RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_AREAS_CONEXION || plantillacodi == RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_AREAS_INTEGRACION || plantillacodi == RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_AREAS_OPERACIONCOMERCIAL
        || plantillacodi == RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_AREAS_MFT || plantillacodi == RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_AREAS_MFT_DB ||                
        plantillacodi == NOTIFICACION_CULMINACION_PLAZO_REVISION_AREAS_SOLICITUD_CONEXION || plantillacodi == NOTIFICACION_CULMINACION_PLAZO_REVISION_AREAS_SOLICITUD_INTEGRACION || plantillacodi == NOTIFICACION_CULMINACION_PLAZO_REVISION_AREAS_SOLICITUD_OPERACIONCOMERCIAL
        || plantillacodi == NOTIFICACION_CULMINACION_PLAZO_REVISION_AREAS_SOLICITUD_MFT || plantillacodi == NOTIFICACION_CULMINACION_PLAZO_REVISION_AREAS_SOLICITUD_MFT_DB ||
        plantillacodi == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_AREAS_CONEXION || plantillacodi == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_AREAS_INTEGRACION || plantillacodi == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_AREAS_OPERACIONCOMERCIAL
        || plantillacodi == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_AREAS_MFT || plantillacodi == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_AREAS_MFT_DB ||
        plantillacodi == NOTIFICACION_CULMINACION_PLAZO_REVISION_AREAS_SUBSANACION_CONEXION || plantillacodi == NOTIFICACION_CULMINACION_PLAZO_REVISION_AREAS_SUBSANACION_INTEGRACION || plantillacodi == NOTIFICACION_CULMINACION_PLAZO_REVISION_AREAS_SUBSANACION_OPERACIONCOMERCIAL
        || plantillacodi == NOTIFICACION_CULMINACION_PLAZO_REVISION_AREAS_SUBSANACION_MFT || plantillacodi == NOTIFICACION_CULMINACION_PLAZO_REVISION_AREAS_SUBSANACION_MFT_DB ) {
        cadena += `
        <fieldset style="margin-bottom: 10px; padding-bottom: revert;">
            <legend>Notificación Manual</legend>
            Notificación automática. <b>Presionar botón ejecutar para enviar recordatorio de forma manual. </b>
            <input style="float: right;" type="button" id="btnEjecutar" value="Ejecutar"  onclick="ejecutarRecordatorio( ${plantillacodi},${ftetcodi});"/>
            
        </fieldset>
        `;
    }
    

    //Agrego nombre del correo
    cadena += `
    <div id="informacionPlantilla" style="height:20px; padding-top: 8px; padding-bottom: 12px;" >
        <div class="tbform-label" style="width:120px; float: left; font-size:18px;">
            Plantilla:
        </div>
        <div id="valor" style="float: left; font-size:18px;">
            ${objPlantilla.Plantcodi} - ${objPlantilla.Plantnomb}
        </div>
    </div>
    <table style="width:100%">
    `;

    //Agrego un fila para gestionar hora de envio de recordatorios   
    if (objPlantilla.Prcscodi > 0) {
        cadena += `
            <tr> 
                <td class="tbform-label">Hora de Ejecución:</td>
                <td class="registro-control" style="width:1100px;">
                    <input type="text" name="Hora" id="HoraR" value="${val_Hora}"  style="width:65px;" ${habilitacion} title="Permite editar hora de los correos automáticos de recordatorios."/>
                    (hh:mm)

                    <span style='padding-left: 50px;'>Estado:</span>
                    <select id="cbEstadoR" style="width: 115px; margin-left: 25px;" ${habilitacion} value="I" >
                        <option value="A" ${seleccionA}>Activo</option>
                        <option value="I" ${seleccionI}>Inactivo</option>
                    </select>
                </td>
            </tr>
        `;
    }

    //Agrego un fila para gestionar parametros (horas/dias)
    if (tipoCorreo == RECORDATORIO) {

        if (objPlantilla.Plantcodi == RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_CONEXION || objPlantilla.Plantcodi == RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_INTEGRACION ||
            objPlantilla.Plantcodi == RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_OPERACIONCOMERCIAL || objPlantilla.Plantcodi == RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_MFT ||
            objPlantilla.Plantcodi == RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_MFT_DB ||
            objPlantilla.Plantcodi == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_CONEXION || objPlantilla.Plantcodi == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_INTEGRACION ||
            objPlantilla.Plantcodi == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_OPERACIONCOMERCIAL || objPlantilla.Plantcodi == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_MFT ||
            objPlantilla.Plantcodi == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_MFT_DB ) {
            cadena += `
                    <tr> 
                        <td class="tbform-label">Recibida hace:</td>
                        <td class="registro-control" style="width:1100px;">
                            <input type="number" name="Dia" id="parametroDR" value="${val_DiasRecepcion}"  style="width:35px;" ${habilitacion} title="Permite editar la cantidad de días de recepción de envío." onkeypress="return validarNumeroEntero(this,event)" />
                            (días)
                        </td>
                    </tr>
                `;
        }

        
        if (objPlantilla.Plantcodi == RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_AREAS_CONEXION || objPlantilla.Plantcodi == RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_AREAS_INTEGRACION ||
            objPlantilla.Plantcodi == RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_AREAS_OPERACIONCOMERCIAL || objPlantilla.Plantcodi == RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_AREAS_MFT ||
            objPlantilla.Plantcodi == RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_AREAS_MFT_DB ||
            objPlantilla.Plantcodi == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_AREAS_CONEXION || objPlantilla.Plantcodi == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_AREAS_INTEGRACION ||
            objPlantilla.Plantcodi == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_AREAS_OPERACIONCOMERCIAL || objPlantilla.Plantcodi == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_AREAS_MFT ||
            objPlantilla.Plantcodi == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_AREAS_MFT_DB) {
            cadena += `
                    <tr> 
                        <td class="tbform-label">Tiempo para vencimiento plazo revision:</td>
                        <td class="registro-control" style="width:1100px;">
                            <input type="number" name="Dia" id="parametroDR" value="${val_DiasRecepcion}"  style="width:35px;" ${habilitacion} title="Permite editar la cantidad de días de recepción de envío." onkeypress="return validarNumeroEntero(this,event)" />
                            (días)
                        </td>
                    </tr>
                `;
        }

    }
    cadena += `
                    <tr>
                        <td style="width:120px" class="registro-label"><input type="hidden" id="hfIdPlantillaCorreo" value="${objPlantilla.Plantcodi}"/></td>
                    </tr>
                    <tr>
                        <td style="width:120px" class="tbform-label">De:</td>
    `;
        

    //Habilito y deshabilito el campo De y Para
    cadena += `
                        <td class="registro-control" style="width:1100px;"><input type="text" name="From" id="From" value="${val_De}" maxlength="900" style="width:1090px;" /></td>
                    </tr>
        `;
    cadena += `
                    <tr>
                        <td class="tbform-label">Para (*):</td>
                        <td class="registro-control" style="width:1100px;"><input type="text" name="To" id="To" value="${val_To}" maxlength="100" style="width:1090px;" onfocus="activarBoton('botonAddPropPara');"  onblur="desactivarBoton('botonAddPropPara','To');" ${habilitacion}/></td>
                        <td class="registro-label"><input type="button" id="botonAddPropPara" value="Agregar variable" onclick="abrirListadoVariables( ${plantillacodi}, '${VARIABLE_PARA}');" style="visibility:hidden"  ></td>
                    </tr>
        `;

    //Otros (CC, BCC, ASUNTO y CONTENIDO)
    cadena += `                
            
            <tr>
                <td class="tbform-label">CC:</td>
                <td class="registro-control" style="width:1100px;"><input name="CC" id="CC" type="text" value="${val_CC}" maxlength="120" style="width:1090px;"  onfocus="activarBoton('botonAddPropCC');"  onblur="desactivarBoton('botonAddPropCC','CC');" ${habilitacion}/></td>
                <td class="registro-label"><input type="button" id="botonAddPropCC" value="Agregar variable" onclick="abrirListadoVariables( ${plantillacodi}, '${VARIABLE_CC}');" style="visibility:hidden"  ></td>
            </tr>
            <tr>
                <td class="tbform-label"> BCC:</td>
                <td class="registro-control" style="width:1100px;"><input name="BCC" id="BCC" type="text" value="${val_BCC}" maxlength="120" style="width:1090px;" ${habilitacion}/></td>
            </tr>
            <tr>
                <td class="tbform-label">Asunto (*):</td>
                <td class="registro-control" style="width:1100px;">
                    <textarea maxlength="450" name="Asunto" id="Asuntos" value=""  cols="" rows="3" style="resize: none;width:1090px;" onfocus="activarBoton('botonAddPropAsunto');"  onblur="desactivarBoton('botonAddPropAsunto','Asuntos');" ${habilitacion}>${val_Asunto}</textarea>
                </td>
                <td class="registro-label"><input type="button" id="botonAddPropAsunto" value="Agregar variable" onclick="abrirListadoVariables( ${plantillacodi}, '${VARIABLE_ASUNTO}');" style="visibility:hidden"  ></td>
            </tr>
            <tr>
                <td class="tbform-label"> Contenido (*):</td>
                <td class="registro-control" style="width:1100px;">
                    <textarea name="Contenido" id="Contenido" maxlength="2000" cols="180" rows="22"  ${habilitacion}>
                        ${val_Contenido}
                    </textarea>
                    (*): Campos obligatorios.
                </td>
                <td class="registro-label">
                    
                </td>
            </tr>
    `;

    if (esEditable) {
        cadena += `

            <tr>
                
                <td colspan="3" style="padding-top: 2px; text-align: center;">
                    <input type="button" id="btnGuardarPC" value="Guardar" />
                    
                </td>
            </tr>

        `;
    }

    cadena += `
    </table>    
    `;

    return cadena;
}

function ejecutarRecordatorio(plantcodi, ftetcodi) {
    limpiarBarraMensaje("mensaje_Detalle");
    limpiarBarraMensaje("mensaje_Listado");
    $.ajax({
        type: 'POST',
        url: controlador + 'EjecutarRecordatorio',
        data: {
            plantcodi: plantcodi,
            ftetcodi: ftetcodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                mostrarMensaje('mensaje_Detalle', 'exito', 'El proceso se ejecutó correctamente');

            } else {
                mostrarMensaje('mensaje_Detalle', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje_Detalle', 'error', 'Ha ocurrido un error...');

        }
    });
}

function iniciarControlTexto(id, listaVariables, sololectura) {

    tinymce.remove();

    tinymce.init({
        selector: '#' + id,
        plugins: [
            //'paste textcolor colorpicker textpattern link table image imagetools preview'
            'wordcount advlist anchor autolink codesample colorpicker contextmenu fullscreen image imagetools lists link media noneditable preview  searchreplace table template textcolor visualblocks wordcount'
        ],
        toolbar1:
            //'insertfile undo redo | fontsizeselect bold italic underline strikethrough | forecolor backcolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link | table | image | mybutton | preview',
            'insertfile undo redo | styleselect fontsizeselect | forecolor backcolor | bullist numlist outdent indent | link | table | mybutton | preview',

        menubar: false,
        readonly: sololectura ? 1 : 0,
        language: 'es',
        statusbar: false,
        convert_urls: false,
        plugin_preview_width: 1000,
        setup: function (editor) {
            editor.on('change',
                function () {
                    editor.save();
                });
            editor.addButton('mybutton', {
                type: 'menubutton',
                text: 'Agregar Variables',
                tooltip: "Ingrese una variable",
                icon: false,
                menu: llenarMenus(editor, listaVariables)
            });

        }
    });

}



function llenarMenus(e, lista) {
    var listaObj = [];
    var numVariables = lista.length;

    if (numVariables >= 1) {
        var regObj = {};
        var regVariable = lista[0];
        regObj.text = regVariable.Nombre;
        regObj.onclick = function () { if (regObj.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable.ContenidoHtml); else e.insertContent(regVariable.Valor);};
        listaObj.push(regObj);

        if (numVariables >= 2) {
            var regObj1 = {};
            var regVariable1 = lista[1];
            regObj1.text = regVariable1.Nombre;
            regObj1.onclick = function () { if (regObj1.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable1.ContenidoHtml); else e.insertContent(regVariable1.Valor); };
            listaObj.push(regObj1);

            if (numVariables >= 3) {
                var regObj2 = {};
                var regVariable2 = lista[2];
                regObj2.text = regVariable2.Nombre;
                regObj2.onclick = function () { if (regObj2.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable2.ContenidoHtml); else e.insertContent(regVariable2.Valor); };
                listaObj.push(regObj2);

                if (numVariables >= 4) {
                    var regObj3 = {};
                    var regVariable3 = lista[3];
                    regObj3.text = regVariable3.Nombre;
                    regObj3.onclick = function () { if (regObj3.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable3.ContenidoHtml); else e.insertContent(regVariable3.Valor); }; 
                    listaObj.push(regObj3);

                    if (numVariables >= 5) {
                        var regObj4 = {};
                        var regVariable4 = lista[4];
                        regObj4.text = regVariable4.Nombre;
                        regObj4.onclick = function () { if (regObj4.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable4.ContenidoHtml); else e.insertContent(regVariable4.Valor); };
                        listaObj.push(regObj4);

                        if (numVariables >= 6) {
                            var regObj5 = {};
                            var regVariable5 = lista[5];
                            regObj5.text = regVariable5.Nombre;
                            regObj5.onclick = function () { if (regObj5.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable5.ContenidoHtml); else e.insertContent(regVariable5.Valor); };
                            listaObj.push(regObj5);

                            if (numVariables >= 7) {
                                var regObj6 = {};
                                var regVariable6 = lista[6];
                                regObj6.text = regVariable6.Nombre;
                                regObj6.onclick = function () { if (regObj6.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable6.ContenidoHtml); else e.insertContent(regVariable6.Valor); };
                                listaObj.push(regObj6);

                                if (numVariables >= 8) {
                                    var regObj7 = {};
                                    var regVariable7 = lista[7];
                                    regObj7.text = regVariable7.Nombre;
                                    regObj7.onclick = function () { if (regObj7.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable7.ContenidoHtml); else e.insertContent(regVariable7.Valor); };
                                    listaObj.push(regObj7);

                                    if (numVariables >= 9) {
                                        var regObj8 = {};
                                        var regVariable8 = lista[8];
                                        regObj8.text = regVariable8.Nombre;
                                        regObj8.onclick = function () { if (regObj8.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable8.ContenidoHtml); else e.insertContent(regVariable8.Valor); };
                                        listaObj.push(regObj8);

                                        if (numVariables >= 10) {
                                            var regObj9 = {};
                                            var regVariable9 = lista[9];
                                            regObj9.text = regVariable9.Nombre;
                                            regObj9.onclick = function () { if (regObj9.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable9.ContenidoHtml); else e.insertContent(regVariable9.Valor); };
                                            listaObj.push(regObj9);

                                            if (numVariables >= 11) {
                                                var regObj10 = {};
                                                var regVariable10 = lista[10];
                                                regObj10.text = regVariable10.Nombre;
                                                regObj10.onclick = function () { if (regObj10.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable10.ContenidoHtml); else e.insertContent(regVariable10.Valor); };
                                                listaObj.push(regObj10);

                                                if (numVariables >= 12) {
                                                    var regObj11 = {};
                                                    var regVariable11 = lista[11];
                                                    regObj11.text = regVariable11.Nombre;
                                                    regObj11.onclick = function () { if (regObj11.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable11.ContenidoHtml); else e.insertContent(regVariable11.Valor); };
                                                    listaObj.push(regObj11);

                                                    if (numVariables >= 13) {
                                                        var regObj12 = {};
                                                        var regVariable12 = lista[12];
                                                        regObj12.text = regVariable12.Nombre;
                                                        regObj12.onclick = function () { if (regObj12.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable12.ContenidoHtml); else e.insertContent(regVariable12.Valor); };
                                                        listaObj.push(regObj12);

                                                        if (numVariables >= 14) {
                                                            var regObj13 = {};
                                                            var regVariable13 = lista[13];
                                                            regObj13.text = regVariable13.Nombre;
                                                            regObj13.onclick = function () { if (regObj13.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable13.ContenidoHtml); else e.insertContent(regVariable13.Valor); };
                                                            listaObj.push(regObj13);

                                                            if (numVariables >= 15) {
                                                                var regObj14 = {};
                                                                var regVariable14 = lista[14];
                                                                regObj14.text = regVariable14.Nombre;
                                                                regObj14.onclick = function () { if (regObj14.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable14.ContenidoHtml); else e.insertContent(regVariable14.Valor); };
                                                                listaObj.push(regObj14);

                                                                if (numVariables >= 16) {
                                                                    var regObj15 = {};
                                                                    var regVariable15 = lista[15];
                                                                    regObj15.text = regVariable15.Nombre;
                                                                    regObj15.onclick = function () { if (regObj15.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable15.ContenidoHtml); else e.insertContent(regVariable15.Valor); };
                                                                    listaObj.push(regObj15);

                                                                    if (numVariables >= 17) {
                                                                        var regObj16 = {};
                                                                        var regVariable16 = lista[16];
                                                                        regObj16.text = regVariable16.Nombre;
                                                                        regObj16.onclick = function () { if (regObj16.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable16.ContenidoHtml); else e.insertContent(regVariable16.Valor); };
                                                                        listaObj.push(regObj16);

                                                                        if (numVariables >= 18) {
                                                                            var regObj17 = {};
                                                                            var regVariable17 = lista[17];
                                                                            regObj17.text = regVariable17.Nombre;
                                                                            regObj17.onclick = function () { if (regObj17.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable17.ContenidoHtml); else e.insertContent(regVariable17.Valor); };
                                                                            listaObj.push(regObj17);

                                                                            if (numVariables >= 19) {
                                                                                var regObj18 = {};
                                                                                var regVariable18 = lista[18];
                                                                                regObj18.text = regVariable18.Nombre;
                                                                                regObj18.onclick = function () { if (regObj18.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable18.ContenidoHtml); else e.insertContent(regVariable18.Valor); };
                                                                                listaObj.push(regObj18);

                                                                                if (numVariables >= 20) {
                                                                                    var regObj19 = {};
                                                                                    var regVariable19 = lista[19];
                                                                                    regObj19.text = regVariable19.Nombre;
                                                                                    regObj19.onclick = function () { if (regObj19.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable19.ContenidoHtml); else e.insertContent(regVariable19.Valor); };
                                                                                    listaObj.push(regObj19);

                                                                                    if (numVariables >= 21) {
                                                                                        var regObj20 = {};
                                                                                        var regVariable20 = lista[20];
                                                                                        regObj20.text = regVariable20.Nombre;
                                                                                        regObj20.onclick = function () { if (regObj20.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable20.ContenidoHtml); else e.insertContent(regVariable20.Valor); };
                                                                                        listaObj.push(regObj20);

                                                                                        if (numVariables >= 22) {
                                                                                            var regObj21 = {};
                                                                                            var regVariable21 = lista[21];
                                                                                            regObj21.text = regVariable21.Nombre;
                                                                                            regObj21.onclick = function () { if (regObj21.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable21.ContenidoHtml); else e.insertContent(regVariable21.Valor); };
                                                                                            listaObj.push(regObj21);

                                                                                            if (numVariables >= 23) {
                                                                                                var regObj22 = {};
                                                                                                var regVariable22 = lista[22];
                                                                                                regObj22.text = regVariable22.Nombre;
                                                                                                regObj22.onclick = function () { if (regObj22.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable22.ContenidoHtml); else e.insertContent(regVariable22.Valor); };
                                                                                                listaObj.push(regObj22);

                                                                                                if (numVariables >= 24) {
                                                                                                    var regObj23 = {};
                                                                                                    var regVariable23 = lista[23];
                                                                                                    regObj23.text = regVariable23.Nombre;
                                                                                                    regObj23.onclick = function () { if (regObj23.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable23.ContenidoHtml); else e.insertContent(regVariable23.Valor); };
                                                                                                    listaObj.push(regObj23);

                                                                                                    if (numVariables >= 25) {
                                                                                                        var regObj24 = {};
                                                                                                        var regVariable24 = lista[24];
                                                                                                        regObj24.text = regVariable24.Nombre;
                                                                                                        regObj24.onclick = function () { if (regObj24.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable24.ContenidoHtml); else e.insertContent(regVariable24.Valor); };
                                                                                                        listaObj.push(regObj24);

                                                                                                        if (numVariables >= 26) {
                                                                                                            var regObj25 = {};
                                                                                                            var regVariable25 = lista[25];
                                                                                                            regObj25.text = regVariable25.Nombre;
                                                                                                            regObj25.onclick = function () { if (regObj25.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable25.ContenidoHtml); else e.insertContent(regVariable25.Valor); };
                                                                                                            listaObj.push(regObj25);

                                                                                                            if (numVariables >= 27) {
                                                                                                                var regObj26 = {};
                                                                                                                var regVariable26 = lista[26];
                                                                                                                regObj26.text = regVariable26.Nombre;
                                                                                                                regObj26.onclick = function () { if (regObj26.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable26.ContenidoHtml); else e.insertContent(regVariable26.Valor); };
                                                                                                                listaObj.push(regObj26);

                                                                                                                if (numVariables >= 28) {
                                                                                                                    var regObj27 = {};
                                                                                                                    var regVariable27 = lista[27];
                                                                                                                    regObj27.text = regVariable27.Nombre;
                                                                                                                    regObj27.onclick = function () { if (regObj27.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable27.ContenidoHtml); else e.insertContent(regVariable27.Valor); };
                                                                                                                    listaObj.push(regObj27);

                                                                                                                    if (numVariables >= 29) {
                                                                                                                        var regObj28 = {};
                                                                                                                        var regVariable28 = lista[28];
                                                                                                                        regObj28.text = regVariable28.Nombre;
                                                                                                                        regObj28.onclick = function () { if (regObj28.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable28.ContenidoHtml); else e.insertContent(regVariable28.Valor); };
                                                                                                                        listaObj.push(regObj28);

                                                                                                                        if (numVariables >= 30) {
                                                                                                                            var regObj29 = {};
                                                                                                                            var regVariable29 = lista[29];
                                                                                                                            regObj29.text = regVariable29.Nombre;
                                                                                                                            regObj29.onclick = function () { if (regObj29.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable29.ContenidoHtml); else e.insertContent(regVariable29.Valor); };
                                                                                                                            listaObj.push(regObj29);

                                                                                                                            if (numVariables >= 31) {
                                                                                                                                var regObj30 = {};
                                                                                                                                var regVariable30 = lista[30];
                                                                                                                                regObj30.text = regVariable30.Nombre;
                                                                                                                                regObj30.onclick = function () { if (regObj30.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable30.ContenidoHtml); else e.insertContent(regVariable30.Valor); };
                                                                                                                                listaObj.push(regObj30);

                                                                                                                                if (numVariables >= 32) {
                                                                                                                                    var regObj31 = {};
                                                                                                                                    var regVariable31 = lista[31];
                                                                                                                                    regObj31.text = regVariable31.Nombre;
                                                                                                                                    regObj31.onclick = function () { if (regObj31.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable31.ContenidoHtml); else e.insertContent(regVariable31.Valor); };
                                                                                                                                    listaObj.push(regObj31);

                                                                                                                                    if (numVariables >= 33) {
                                                                                                                                        var regObj32 = {};
                                                                                                                                        var regVariable32 = lista[32];
                                                                                                                                        regObj32.text = regVariable32.Nombre;
                                                                                                                                        regObj32.onclick = function () { if (regObj32.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable32.ContenidoHtml); else e.insertContent(regVariable32.Valor); };
                                                                                                                                        listaObj.push(regObj32);

                                                                                                                                        if (numVariables >= 34) {
                                                                                                                                            var regObj33 = {};
                                                                                                                                            var regVariable33 = lista[33];
                                                                                                                                            regObj33.text = regVariable33.Nombre;
                                                                                                                                            regObj33.onclick = function () { if (regObj33.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable33.ContenidoHtml); else e.insertContent(regVariable33.Valor); };
                                                                                                                                            listaObj.push(regObj33);

                                                                                                                                            if (numVariables >= 35) {
                                                                                                                                                var regObj34 = {};
                                                                                                                                                var regVariable34 = lista[34];
                                                                                                                                                regObj34.text = regVariable34.Nombre;
                                                                                                                                                regObj34.onclick = function () { if (regObj34.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable34.ContenidoHtml); else e.insertContent(regVariable34.Valor); };
                                                                                                                                                listaObj.push(regObj34);

                                                                                                                                                if (numVariables >= 36) {
                                                                                                                                                    var regObj35 = {};
                                                                                                                                                    var regVariable35 = lista[35];
                                                                                                                                                    regObj35.text = regVariable35.Nombre;
                                                                                                                                                    regObj35.onclick = function () { if (regObj35.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable35.ContenidoHtml); else e.insertContent(regVariable35.Valor); };
                                                                                                                                                    listaObj.push(regObj35);

                                                                                                                                                    if (numVariables >= 37) {
                                                                                                                                                        var regObj36 = {};
                                                                                                                                                        var regVariable36 = lista[36];
                                                                                                                                                        regObj36.text = regVariable36.Nombre;
                                                                                                                                                        regObj36.onclick = function () { if (regObj36.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable36.ContenidoHtml); else e.insertContent(regVariable36.Valor); };
                                                                                                                                                        listaObj.push(regObj36);

                                                                                                                                                        if (numVariables >= 38) {
                                                                                                                                                            var regObj37 = {};
                                                                                                                                                            var regVariable37 = lista[37];
                                                                                                                                                            regObj37.text = regVariable37.Nombre;
                                                                                                                                                            regObj37.onclick = function () { if (regObj37.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable37.ContenidoHtml); else e.insertContent(regVariable37.Valor); };
                                                                                                                                                            listaObj.push(regObj37);

                                                                                                                                                            if (numVariables >= 39) {
                                                                                                                                                                var regObj38 = {};
                                                                                                                                                                var regVariable38 = lista[38];
                                                                                                                                                                regObj38.text = regVariable38.Nombre;
                                                                                                                                                                regObj38.onclick = function () { if (regObj38.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable38.ContenidoHtml); else e.insertContent(regVariable38.Valor); };
                                                                                                                                                                listaObj.push(regObj38);

                                                                                                                                                                if (numVariables >= 40) {
                                                                                                                                                                    var regObj39 = {};
                                                                                                                                                                    var regVariable39 = lista[39];
                                                                                                                                                                    regObj39.text = regVariable39.Nombre;
                                                                                                                                                                    regObj39.onclick = function () { if (regObj39.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable39.ContenidoHtml); else e.insertContent(regVariable39.Valor); };
                                                                                                                                                                    listaObj.push(regObj39);

                                                                                                                                                                    if (numVariables >= 41) {
                                                                                                                                                                        var regObj40 = {};
                                                                                                                                                                        var regVariable40 = lista[40];
                                                                                                                                                                        regObj40.text = regVariable40.Nombre;
                                                                                                                                                                        regObj40.onclick = function () { if (regObj40.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable40.ContenidoHtml); else e.insertContent(regVariable40.Valor); };
                                                                                                                                                                        listaObj.push(regObj40);

                                                                                                                                                                        if (numVariables >= 42) {
                                                                                                                                                                            var regObj41 = {};
                                                                                                                                                                            var regVariable41 = lista[41];
                                                                                                                                                                            regObj41.text = regVariable41.Nombre;
                                                                                                                                                                            regObj41.onclick = function () { if (regObj41.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable41.ContenidoHtml); else e.insertContent(regVariable41.Valor); };
                                                                                                                                                                            listaObj.push(regObj41);

                                                                                                                                                                            if (numVariables >= 43) {
                                                                                                                                                                                var regObj42 = {};
                                                                                                                                                                                var regVariable42 = lista[42];
                                                                                                                                                                                regObj42.text = regVariable42.Nombre;
                                                                                                                                                                                regObj42.onclick = function () { if (regObj42.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable42.ContenidoHtml); else e.insertContent(regVariable42.Valor); };
                                                                                                                                                                                listaObj.push(regObj42);

                                                                                                                                                                                if (numVariables >= 44) {
                                                                                                                                                                                    var regObj43 = {};
                                                                                                                                                                                    var regVariable43 = lista[43];
                                                                                                                                                                                    regObj43.text = regVariable43.Nombre;
                                                                                                                                                                                    regObj43.onclick = function () { if (regObj43.text == "Firma - {FIRMA_COES}") e.insertContent(regVariable43.ContenidoHtml); else e.insertContent(regVariable43.Valor); };
                                                                                                                                                                                    listaObj.push(regObj43);
                                                                                                                                                                                }
                                                                                                                                                                            }
                                                                                                                                                                        }
                                                                                                                                                                    }
                                                                                                                                                                }
                                                                                                                                                            }
                                                                                                                                                        }
                                                                                                                                                    }
                                                                                                                                                }
                                                                                                                                            }
                                                                                                                                        }
                                                                                                                                    }
                                                                                                                                }
                                                                                                                            }
                                                                                                                        }
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    return listaObj;
}

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

function activarBoton(idBoton) {
    $("#" + idBoton).css("visibility", "visible");
}

async function desactivarBoton(idBoton, idCaja) {
    await sleep(800);

    var caja = document.getElementById(idCaja);

    if (document.activeElement != caja) {
        $("#" + idBoton).css("visibility", "hidden");
    }
}

function abrirListadoVariables(idPlantilla, tipoCampo) {

    var lista = [];

    if (tipoCampo == VARIABLE_CC || tipoCampo == VARIABLE_PARA)
        lista = listarVariableSoloCorreosOrdenada;
    else
        lista = listarVariableCorreoOrdenada;

    var htmlVariables = dibujarListadoVariables(lista);
    $("#seccionListadoVariables").html(htmlVariables);

    abrirPopup("popupAgregarVariable");

    $('#btnAceptarVar').click(function () {
        if (tipoCampo == VARIABLE_ASUNTO)
            agregarTextoA($("#cbVariable").val(), "Asuntos");
        if (tipoCampo == VARIABLE_CC)
            agregarTextoA($("#cbVariable").val(), "CC");
        if (tipoCampo == VARIABLE_PARA)
            agregarTextoA($("#cbVariable").val(), "To");

        cerrarPopup('popupAgregarVariable');
    });

    $('#btnCancelarVar').click(function () {
        cerrarPopup('popupAgregarVariable');
    });


}

function dibujarListadoVariables(listaVariables) {

    var cadena = '';
    cadena += `
    <table border="0"  id="tablaCargos">       
        <tr>
            <td>Variable :</td>
            <td>
                <select id="cbVariable" style="WIDTH: 450px;">
    `;

    for (key in listaVariables) {
        var item = listaVariables[key];
        cadena += `            
                    <option value="${item.Valor}">${item.Nombre}</option>
        `;
    }

    cadena += `            
                </select>
            </td>
         </tr >
         <tr style="text-align: center;">
                <td colspan="2" style="padding-top: 35px;">
                    <input type="button" id="btnAceptarVar" value="Agregar" />
                    <input type="button" id="btnCancelarVar" value="Cancelar" />
                </td>
            </tr>
    </table >
    `;

    return cadena;
}

function agregarTextoA(textoAIngresar, idCaja) {
    var caja = document.getElementById(idCaja);
    var inicio = caja.selectionStart;
    var fin = caja.selectionEnd;
    var textoAgregado = textoAIngresar;

    var texto = caja.value;
    var textIni = texto.substring(0, inicio);
    var textFin = texto.substring(inicio);
    var nuevoTexto = textIni + textoAgregado + textFin;
    $('#' + idCaja).val(nuevoTexto);

    caja.focus();
}

function guardarPlantilla() {
    limpiarBarraMensaje("mensaje_Detalle");
    limpiarBarraMensaje("mensaje_Listado");
    var correo = getPlantillaCorreo();

    var msg = validarCamposCorreoAGuardar(correo);

    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'GuardarPlantillaCorreo',
            contentType: "application/json",
            dataType: 'json',
            data: JSON.stringify({
                plantillaCorreo: correo,
            }),
            cache: false,
            success: function (evt) {
                if (evt.Resultado != "-1") {

                    //muestra el listado respectivo
                    $('#tab-container').easytabs('select', '#tabLista');
                    mostrarMensaje('mensaje_Detalle', 'exito', "Los cambios fueron guardados exitosamente...");
                    mostrarListadoPlantilla();
                    $("#div_detalle").html("");

                } else {
                    mostrarMensaje('mensaje_Detalle', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_Detalle', 'error', 'Ha ocurrido un error.' + evt.Mensaje);
            }
        });
    } else {
        mostrarMensaje('mensaje_Detalle', 'error', msg);
    }
}

function getPlantillaCorreo() {
    var obj = {};

    //Parametros Dia repecion (recordatorios)
    obj.ParametroDiaHora = "";
    if (tipoPlantillaEnListado == TPCORRCODI_RECORDATORIO) {
        obj.ParametroDiaHora = $("#parametroDR").val();
    }

    //Hora y Estado
    obj.Hora = $("#HoraR").val();
    obj.EstadoRecordatorio = $("#cbEstadoR").val();

    //otros
    obj.Planticorreos = $("#To").val();
    obj.Plantcodi = $("#hfIdPlantillaCorreo").val();
    obj.Plantcontenido = $("#Contenido").val();
    obj.Plantasunto = $("#Asuntos").val();
    obj.PlanticorreosCc = $("#CC").val();
    obj.PlanticorreosBcc = $("#BCC").val();
    obj.PlanticorreoFrom = $("#From").val();

    return obj;
}

function validarCamposCorreoAGuardar(correo) {
    var msj = "";

    /*********************************************** validacion del campo CC ***********************************************/
    var validaTo = 0;

    if ($('#From').val().trim() == "") {
        msj += "<p>Debe ingresar texto en 'De'.</p>";
    }

    if ($('#To').val().trim() == "") {
        msj += "<p>Debe ingresar texto en 'Para'.</p>";
    }

    validaTo = validarCorreo($('#To').val(), 0, -1, "To");
    if (validaTo < 0) {
        msj += "<p>Error encontrado en el campo Para. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
    }
    var para_ = correo.Planticorreos.trim();
    var tienenMismaCantidadPara = validarCantidadDeSeparadores(para_);
    if (!tienenMismaCantidadPara) {
        msj += "<p>Revisar campo Para, la cantidad de caracteres '{' no coincide con la cantidad de caracteres '}'. </p>";
    }
    //valido que las palabras dentro de un {} sean variables admitidas
    msj = validarVariablesCorrectas(para_, "To", lstVariablesPara, msj);

    //valido los (;) antes de los ({)
    msj = validarSeparacionConectores(para_, "To", msj);

    //valido cantidad de (;)
    msj = validarNumeroPuntoYComas(para_, "To", msj);



    /*********************************************** validacion del campo CC ***********************************************/
    var validaCc = validarCorreo($('#CC').val(), 0, -1, "Cc");
    if (validaCc < 0) {
        msj += "<p>Error encontrado en el campo CC. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
    }
    var micc_ = correo.PlanticorreosCc.trim();
    var tienenMismaCantidadCc = validarCantidadDeSeparadores(micc_);
    if (!tienenMismaCantidadCc) {
        msj += "<p>Revisar campo CC, la cantidad de caracteres '{' no coincide con la cantidad de caracteres '}'. </p>";
    }
    //valido que las palabras dentro de un {} sean variables admitidas
    msj = validarVariablesCorrectas(micc_, "CC", lstVariablesCC, msj);

    //valido los (;) antes de los ({)
    msj = validarSeparacionConectores(micc_, "CC", msj);

    //valido cantidad de (;)
    msj = validarNumeroPuntoYComas(micc_, "CC", msj);


    /*********************************************** validacion del campo BCC ***********************************************/
    var validaBcc = validarCorreo($('#BCC').val(), 0, -1, "Bcc");
    if (validaBcc < 0) {
        msj += "<p>Error encontrado en el campo BCC. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
    }


    /********************************************** validacion del campo Asunto ***********************************************/
    var asunto_ = correo.Plantasunto.trim();
    if (asunto_ == "") {
        msj += "<p>Debe ingresar texto en 'Asunto'.</p>";
    }

    var tienenMismaCantidadAsunto = validarCantidadDeSeparadores(asunto_);

    if (!tienenMismaCantidadAsunto) {
        msj += "<p>Revisar campo Asunto, la cantidad de caracteres '{' no coincide con la cantidad de caracteres '}'. </p>";
    }

    //valido que las palabras dentro de un {} sean variables admitidas
    msj = validarVariablesCorrectas(asunto_, "Asunto", lstVariablesCorreo, msj);




    /*********************************************** validacion del campo Contenido ***********************************************/

    var contenido_ = correo.Plantcontenido.trim();
    if (!validarContenidoHtml(contenido_)) {
        msj += "<p>Debe ingresar texto en 'Contenido'.</p>";
        contenido_ = '';
    }
    var tienenMismaCantidadContenido = validarCantidadDeSeparadores(contenido_);


    if (!tienenMismaCantidadContenido) {
        msj += "<p>Revisar campo Contenido, la cantidad de caracteres '{' no coincide con la cantidad de caracteres '}'. </p>";
    }

    //valido que las palabras dentro de un {} sean variables admitidas   
    msj = validarVariablesCorrectas(contenido_, "Contenido", lstVariablesCorreo, msj);



    /*********************************************** validacion parametros (dias) ***********************************************/
    if (tipoPlantillaEnListado == TPCORRCODI_RECORDATORIO) {

        if (plantillaEnDetalle == RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_CONEXION || plantillaEnDetalle == RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_INTEGRACION ||
            plantillaEnDetalle == RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_OPERACIONCOMERCIAL || plantillaEnDetalle == RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_MFT ||
            plantillaEnDetalle == RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_MFT_DB ||
            plantillaEnDetalle == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_CONEXION || plantillaEnDetalle == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_INTEGRACION ||
            plantillaEnDetalle == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_OPERACIONCOMERCIAL || plantillaEnDetalle == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_MFT ||
            plantillaEnDetalle == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_MFT_DB) {

            //verifico si se esta usando la variable        
            let resultadoDia_ContenidoSol = (contenido_).includes(VAR_NUMERO_DIAS_RECEPCION_SOLICITUD);
            let resultadoDia_AsuntoSol = (asunto_).includes(VAR_NUMERO_DIAS_RECEPCION_SOLICITUD);
            let resultadoDia_ContenidoSub = (contenido_).includes(VAR_NUMERO_DIAS_RECEPCION_SUBSANACION);
            let resultadoDia_AsuntoSub = (asunto_).includes(VAR_NUMERO_DIAS_RECEPCION_SUBSANACION);

            var dh = correo.ParametroDiaHora;

            if (resultadoDia_ContenidoSol || resultadoDia_AsuntoSol || resultadoDia_ContenidoSub || resultadoDia_AsuntoSub) {


                if (dh.trim() == "") {
                    msj += "<p>Revisar campo 'Recibida hace'. Ingrese valor.</p>";
                } else {
                    if (parseInt(dh.trim()) < 0) {
                        msj += "<p>El valor de 'Recibida hace' debe ser positivo.</p>";
                    }
                }


            } else {
                if (parseInt(dh.trim()) < 0) {
                    msj += "<p>El valor de 'Recibida hace' debe ser positivo.</p>";
                }
            }
        }

        
        if (plantillaEnDetalle == RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_AREAS_CONEXION || plantillaEnDetalle == RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_AREAS_INTEGRACION ||
            plantillaEnDetalle == RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_AREAS_OPERACIONCOMERCIAL || plantillaEnDetalle == RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_AREAS_MFT ||
            plantillaEnDetalle == RECORDATORIO_VENCIMIENTO_PLAZO_SOLICITUD_AREAS_MFT_DB ||
            plantillaEnDetalle == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_AREAS_CONEXION || plantillaEnDetalle == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_AREAS_INTEGRACION ||
            plantillaEnDetalle == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_AREAS_OPERACIONCOMERCIAL || plantillaEnDetalle == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_AREAS_MFT ||
            plantillaEnDetalle == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANACION_AREAS_MFT_DB ) {
            //verifico si se esta usando la variable   PARA recordatorio areas      
            let area_resultadoDia_Contenido = (contenido_).includes(VAR_NUMERO_DIAS_FALTANTES_VENCIMIENTO_PLAZO_REVISION_AREAS);
            let area_resultadoDia_Asunto = (asunto_).includes(VAR_NUMERO_DIAS_FALTANTES_VENCIMIENTO_PLAZO_REVISION_AREAS);

            var dh1 = correo.ParametroDiaHora;

            if (area_resultadoDia_Contenido || area_resultadoDia_Asunto) {


                if (dh1.trim() == "") {
                    msj += "<p>Revisar campo 'Tiempo para vencimiento plazo revisión'. Ingrese valor.</p>";
                } else {
                    if (parseInt(dh1.trim()) < 0) {
                        msj += "<p>El valor de 'Tiempo para vencimiento plazo revisión' debe ser positivo.</p>";
                    }
                }


            } else {
                if (parseInt(dh1.trim()) < 0) {
                    msj += "<p>El valor de 'Tiempo para vencimiento plazo revisión' debe ser positivo.</p>";
                }
            }
        }
    }


    return msj;
}

//Validación de campos
function borrarVacios(textoConVacios) {
    let textoSinVacios = textoConVacios.replace(/ /g, "");
    return textoSinVacios;
}

function validarCantidadDeSeparadores(campo) {

    //valido que la cantidad de { sea igual al de }
    let regexIni = new RegExp(SEPARADOR_INI_VARIABLE, 'g')
    let regexFin = new RegExp(SEPARADOR_FIN_VARIABLE, 'g')
    /*const regex = /{/g;*/
    const lstSeparadorIniCampo = campo.match(regexIni);
    const lstSeparadorFinCampo = campo.match(regexFin);

    var tienenMismaCantidad = false;
    if (lstSeparadorIniCampo != null) {
        if (lstSeparadorFinCampo != null) {
            if (lstSeparadorIniCampo.length == lstSeparadorFinCampo.length)
                tienenMismaCantidad = true;
        } else {
            tienenMismaCantidad = false;
        }
    } else {
        if (lstSeparadorFinCampo != null) {
            tienenMismaCantidad = false;
        } else {
            tienenMismaCantidad = true;
        }
    }

    return tienenMismaCantidad;
}

function validarNumeroPuntoYComas(valorCampo, campo, msj) {
    var cadena = valorCampo;

    if (cadena != "") {

        var arreglo = cadena.split(';');
        var nroCorreo = 0;
        var longitud = arreglo.length;

        if (longitud == 0) {
            arreglo = cadena;
            longitud = 1;
        }

        for (var i = 0; i < longitud; i++) {

            var email = arreglo[i].trim();
            var validacion = validarDirecccionCorreo(email);

            if (validacion) {
                nroCorreo++;
            }

        }

        //si hay correos valido la cantidad de  ;
        if (nroCorreo > 0) {
            //num de punto y comas
            const regex = /;/g;
            const lstTotalPuntoYComas = valorCampo.match(regex);

            //num de llaves
            const regex2 = /{/g;
            const lstTotalInicioConectores = valorCampo.match(regex2);

            //solo valido si hay variables
            if (lstTotalInicioConectores != null) {
                if (lstTotalPuntoYComas != null) {
                    if (lstTotalInicioConectores.length != lstTotalPuntoYComas.length) {
                        if (campo == "To")
                            campo = "Para";
                        msj += "<p>Revisar campo " + campo + ". Se detectó que el número de punto y comas (;) es incorrecto. </p>";
                    }
                } else {
                    if (campo == "To")
                        campo = "Para";
                    msj += "<p>Revisar campo " + campo + ". Se detectó que el número de punto y comas (;) es incorrectoo. </p>";
                }
            }
        }

    }
    return msj;
}

function validarSeparacionConectores(valorCampo, campo, msj) {
    //validando que no haya {var1}{var2} (sin ; entre ellos)

    //quito los vacios
    var valorSinVacios = borrarVacios(valorCampo);
    const regex = /}\w*{/g;
    const lstTotalLlavesSinPuntoYComa = valorSinVacios.match(regex);
    if (lstTotalLlavesSinPuntoYComa != null) {
        if (lstTotalLlavesSinPuntoYComa.length > 0) {
            if (campo == "To")
                campo = "Para";
            msj += "<p>Revisar campo " + campo + ". Se detectó la omisión de (;) entre dos variables continuas. </p>";
        }
    }

    return msj;
}

function validarVariablesCorrectas(valorCampo, campo, lstVariables, msj) {
    //validando textos tipo: {texto}
    //const regex = /{\w+}/g; //no sirve cuando hay espacios entre {} dado que w es word
    const regex = /{([^}]+)}/g;//atrapa los q inician con {, sigue mientras no haya }, y termina con un }. /g para que busque todas las coincidencias
    const lstPalabrasDentroParentesis = valorCampo.match(regex);
    if (lstPalabrasDentroParentesis != null) {
        let lstDiferentes = lstPalabrasDentroParentesis.filter(x => !lstVariables.includes(x));
        if (lstDiferentes.length > 0) {
            if (campo == "To")
                campo = "Para";
            msj += "<p>Revisar campo " + campo + ". Se detectó variables ( texto dentro de {} ) no reconocidas. </p>";
        }
    }

    //validando textos tipo: {}
    const regex2 = /{}/g;
    const lstPalabrasSoloParentesis = valorCampo.match(regex2);
    if (lstPalabrasSoloParentesis != null) {
        if (lstPalabrasSoloParentesis.length > 0) {
            if (campo == "To")
                campo = "Para";
            msj += "<p>Revisar campo " + campo + ". Se detectó texto ( {} ) no permitido. </p>";
        }
    }

    return msj;
}

function validarCorreo(valCadena, minimo, maximo, campo) {
    var cadena = valCadena;

    if (cadena != null) {
        for (var i = 0; i < lstVariablesCorreo.length; i++) {
            cadena = cadena.replace(lstVariablesCorreo[i], "");
        }
    }

    var arreglo = cadena.split(';');
    var nroCorreo = 0;
    var longitud = arreglo.length;

    if (longitud == 0) {
        arreglo = cadena;
        longitud = 1;
    }

    for (var i = 0; i < longitud; i++) {

        var email = arreglo[i].trim();
        var validacion = validarDirecccionCorreo(email);

        if (validacion) {
            nroCorreo++;
        } else {
            if (email != "")
                return -1;
        }

    }

    if (minimo > nroCorreo)
        return -1;

    if (maximo > 0 && nroCorreo > maximo)
        return -1;

    return 1;
}

function validarDirecccionCorreo(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

function validarNumeroEntero(item, evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;

    if (charCode == 46) {
        return false;
    }

    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }

    return true;
}

function validarContenidoHtml(texto) {
    $("#html_prueba").html(texto);

    var textoSinHtml = ($("#html_prueba").text()).trim();

    return textoSinHtml != "";
}

//Utiles
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