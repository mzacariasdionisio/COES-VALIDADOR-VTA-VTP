using System;
using System.Data;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IIO_TABLA_SYNC
    /// </summary>
    public class IioTablaSyncHelper : HelperBase
    {
        public IioTablaSyncHelper() :base(Consultas.IioTablaSyncSql)
        {
            
        }

        public IioTablaSyncDTO Create(IDataReader dr)
        {
            IioTablaSyncDTO entity = new IioTablaSyncDTO();

            int iRtabCodi = dr.GetOrdinal(RtabCodi);
            if (!dr.IsDBNull(iRtabCodi)) entity.RtabCodi = dr.GetString(iRtabCodi);

            int iRtabDescripcionTabla = dr.GetOrdinal(RtabDescripcionTabla);
            if (!dr.IsDBNull(iRtabDescripcionTabla)) entity.RtabDescripcionTabla = dr.GetString(iRtabDescripcionTabla);

            int iRtabEstadoTabla = dr.GetOrdinal(RtabEstadoTabla);
            if (!dr.IsDBNull(iRtabEstadoTabla)) entity.RtabEstadoTabla = dr.GetString(iRtabEstadoTabla);

            int iRtabCodTablaOsig = dr.GetOrdinal(RtabCodTablaOsig);
            if (!dr.IsDBNull(iRtabCodTablaOsig)) entity.RtabCodTablaOsig = dr.GetString(iRtabCodTablaOsig);

            int iRtabEstRegistro = dr.GetOrdinal(RtabEstRegistro);
            if (!dr.IsDBNull(iRtabEstRegistro)) entity.RtabEstRegistro = dr.GetString(iRtabEstRegistro);

            int iRtabUsuCreacion = dr.GetOrdinal(RtabUsuCreacion);
            if (!dr.IsDBNull(iRtabUsuCreacion)) entity.RtabUsuCreacion = dr.GetString(iRtabUsuCreacion);

            int iRtabFecCreacion = dr.GetOrdinal(RtabFecCreacion);
            if (!dr.IsDBNull(iRtabFecCreacion)) entity.RtabFecCreacion = dr.GetDateTime(iRtabFecCreacion);

            int iRtabUsuModificacion = dr.GetOrdinal(RtabUsuModificacion);
            if (!dr.IsDBNull(iRtabUsuModificacion)) entity.RtabUsuModificacion = dr.GetString(iRtabUsuModificacion);

            int iRtabFecModificacion = dr.GetOrdinal(RtabFecModificacion);
            if (!dr.IsDBNull(iRtabFecModificacion)) entity.RtabFecModificacion = dr.GetDateTime(iRtabFecModificacion);

            int iRtabQuery = dr.GetOrdinal(RtabQuery);
            if (!dr.IsDBNull(iRtabQuery)) entity.RtabQuery = dr.GetString(iRtabQuery);

            int iRtabNombreTabla = dr.GetOrdinal(RtabNombreTabla);
            if (!dr.IsDBNull(iRtabNombreTabla)) entity.RtabNombreTabla = dr.GetString(iRtabNombreTabla);

            //Adiconales

            int iRccaNroRegistros = dr.GetOrdinal(RccaNroRegistros);
            if (!dr.IsDBNull(iRccaNroRegistros)) entity.RccaNroRegistros = dr.GetInt32(iRccaNroRegistros);

            int iRccaFecHorEnvio = dr.GetOrdinal(RccaFecHorEnvio);
            if (!dr.IsDBNull(iRccaFecHorEnvio)) entity.RccaFecHorEnvio = dr.GetDateTime(iRccaFecHorEnvio);

            int iRccaEstadoEnvio = dr.GetOrdinal(RccaEstadoEnvio);
            if (!dr.IsDBNull(iRccaEstadoEnvio)) entity.RccaEstadoEnvio = dr.GetString(iRccaEstadoEnvio);

            int iRccaUsuario = dr.GetOrdinal(RccaUsuario);
            if (!dr.IsDBNull(iRccaUsuario)) entity.RccaUsuario = dr.GetString(iRccaUsuario);

            return entity;
        }

        #region Mapeo de Campos

        public static string TableName = "IIO_TABLA_SYNC";
        public static string RtabCodi = "RTABCODI";
        public static string RtabDescripcionTabla = "RTABDESCRIPCIONTABLA";
        public static string RtabEstadoTabla = "RTABESTADOTABLA";
        public static string RtabCodTablaOsig = "RTABCODTABLAOSIG";
        public static string RtabEstRegistro = "RTABESTREGISTRO";
        public static string RtabUsuCreacion = "RTABUSUCREACION";
        public static string RtabFecCreacion = "RTABFECCREACION";
        public static string RtabUsuModificacion = "RTABUSUMODIFICACION";
        public static string RtabFecModificacion = "RTABFECMODIFICACION";
        public static string RtabQuery = "RTABQUERY";
        public static string RtabNombreTabla = "RTABNOMBRETABLA";

        //Adicionales
        public static string RccaNroRegistros = "RCCANROREGISTROS";
        public static string RccaFecHorEnvio = "RCCAFECHORENVIO";
        public static string RccaEstadoEnvio = "RCCAESTADOENVIO";
        public static string RccaUsuario = "RCCAUSUARIO";
        #endregion
    }
}