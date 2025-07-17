// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: alpha
//
// Fecha creacion: 25/10/2016
// Descripcion: Archivo para la atencion del requerimiento.
//
// Historial de cambios:
// 
// Correlativo	Fecha		Requerimiento		Comentario
//
// =======================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.IntercambioOsinergmin.Helper
{

    /// <summary>
    /// Clase utilizada para los parámetros necesarios para el intercambio de información con el Osinergmin.
    /// </summary>
    public class ConstantesIntercambio
    {

        /// <summary>
        /// Correo del administrador de la sincronización de maestros.
        /// </summary>
        public const string CorreoAdmSincronizaMaestro = "intercambioOsiCoes@coes.org.pe";

        /// <summary>
        /// Formato a aplicar para mostrar las fechas con el formato de 24 horas.
        /// </summary>
        public const string FormatoDotNETFechaHora24 = "dd/MM/yyyy HH:mm:ss";

        /// <summary>
        /// Idenficicador de la plantilla para realizar la notificación al responsable correspondiente.
        /// </summary>
        public const int IdPlantillaResultadoSincronizacion = 10;

        /// <summary>
        /// Valor resultante del servicio web de Osinergmin para Ok.
        /// </summary>
        public const int ValorResultanteOkOsiWS = 1;

        /// <summary>
        /// Dirección IP a usar para el registro de datos generados de manera automática.
        /// </summary>
        public const string TerminalTareaAutomatica = "127.0.0.1";

        /// <summary>
        /// Nombre del usuario a usar para el registro de datos generados de manera automática.
        /// </summary>
        public const string UsuarioTareaAutomatica = "coes_aut";

        /// <summary>
        /// Código de Activo para el estado de las empresas COES.
        /// </summary>
        public const string EmpresaCOESEstadoActivo = "A";

        /// <summary>
        /// Código de Activo para el estado de los equipos COES.
        /// </summary>
        public const string EquipoCOESEstadoActivo = "A";

        /// <summary>
        /// Código de Activo para el estado de los puntos de medición COES.
        /// </summary>
        public const string PtoMedicionCOESEstadoActivo = "A";

        /// <summary>
        /// Código de Baja para el estado de los equipos COES.
        /// </summary>
        public const string EquipoCOESEstadoBaja = "B";

        /// <summary>
        /// Código de Activo para el estado de las barras COES.
        /// </summary>
        public const string BarraCOESEstadoActivo = "ACT";

        /// <summary>
        /// Código de Activo para el estado de los grupos COES.
        /// </summary>
        public const string GrupoCOESEstadoActivo = "S";

        /// <summary>
        /// Excepción en el Código de central de generación para la generación de correlativos.
        /// </summary>
        public const string CentralGeneracionCodigoExcepcion = "C9999";

        /// <summary>
        /// Excepción en el Código de central de generación para la generación de correlativos.
        /// </summary>
        public const string GrupoGeneracionCodigoExcepcion = "G9999";

        /// <summary>
        /// Prefijo del código de la Central de Generación de Osinergmin.
        /// </summary>
        public const string CentralGeneracionOsiPrefijoCodigo = "C";

        /// <summary>
        /// Prefijo del código del Grupo de Generación de Osinergmin.
        /// </summary>
        public const string GrupoGeneracionOsiPrefijoCodigo = "G";

        /// <summary>
        /// Prefijo del código del Modo de Operación de Osinergmin.
        /// </summary>
        public const string ModoOperacionOsiPrefijoCodigo = "M";

        /// <summary>
        /// Prefijo del código del Embalse de Osinergmin.
        /// </summary>
        public const string EmbalseOsiPrefijoCodigo = "E";

        /// <summary>
        /// Prefijo del código del Lago de Osinergmin.
        /// </summary>
        public const string LagoOsiPrefijoCodigo = "L";

    }
}
