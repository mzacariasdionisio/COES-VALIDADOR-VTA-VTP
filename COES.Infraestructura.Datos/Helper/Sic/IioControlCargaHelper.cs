using System;
using System.Data;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IIO_CONTROL_CARGA
    /// </summary>
    public class IioControlCargaHelper : HelperBase
    {
        public IioControlCargaHelper() : base(Consultas.IioControlCargaSql)
        {        
        }

        public IioControlCargaDTO Create(IDataReader dr)
        {
            int iRccaCodi = dr.GetOrdinal(RccaCodi);
            int iPeriodoCodi = dr.GetOrdinal(IioPeriodoSeinHelper.PseinCodi);
            int iTablaCodi = dr.GetOrdinal(IioTablaSyncHelper.RtabCodi);
            int iRccaNroRegistros = dr.GetOrdinal(RccaNroRegistros);
            int iRccaFecHorEnvio = dr.GetOrdinal(RccaFecHorEnvio);
            int iRccaEstadoEnvio = dr.GetOrdinal(RccaEstadoEnvio);
            int iRccaEstRegistro = dr.GetOrdinal(RccaEstRegistro);
            int iRccaUsuCreacion = dr.GetOrdinal(RccaUsuCreacion);
            int iRccaFecCreacion = dr.GetOrdinal(RccaFecCreacion);
            int iRccaUsuModificacion = dr.GetOrdinal(RccaUsuModificacion);
            int iRccaFecModificacion = dr.GetOrdinal(RccaFecModificacion);
            int iEnviocodi = dr.GetOrdinal(Enviocodi);

            var obj = new IioControlCargaDTO
            {
                RccaCodi = (!dr.IsDBNull(iRccaCodi) ? dr.GetInt32(iRccaCodi) : default(int)),
                RccaEstadoEnvio = (!dr.IsDBNull(iRccaEstadoEnvio) ? dr.GetString(iRccaEstadoEnvio) : null),
                RccaEstRegistro = (!dr.IsDBNull(iRccaEstRegistro) ? dr.GetString(iRccaEstRegistro) : null),
                RccaFecHorEnvio = (!dr.IsDBNull(iRccaFecHorEnvio) ? dr.GetDateTime(iRccaFecHorEnvio) : new DateTime()),
                RccaNroRegistros = (!dr.IsDBNull(iRccaNroRegistros) ? dr.GetInt32(iRccaNroRegistros) : default(int)),
                RccaUsuCreacion = (!dr.IsDBNull(iRccaUsuCreacion) ? dr.GetString(iRccaUsuCreacion) : null),
                RccaUsuModificacion = (!dr.IsDBNull(iRccaUsuModificacion) ? dr.GetString(iRccaUsuModificacion) : null),
                PseinCodi = (!dr.IsDBNull(iPeriodoCodi) ? dr.GetInt32(iPeriodoCodi) : default(int)),
                RtabCodi = (!dr.IsDBNull(iTablaCodi) ? dr.GetString(iTablaCodi) : null),
                Enviocodi = (!dr.IsDBNull(iEnviocodi) ? dr.GetInt32(iEnviocodi) : default(int))
            };

            if (!dr.IsDBNull(iRccaFecCreacion)) obj.RccaFecCreacion = dr.GetDateTime(iRccaFecCreacion);
            if (!dr.IsDBNull(iRccaFecModificacion)) obj.RccaFecModificacion = dr.GetDateTime(iRccaFecModificacion);

            return obj;
        }

        #region Mapeo de Campos

        public static string TableName = "IIO_CONTROL_CARGA";
        public static string RccaCodi = "RCCACODI";
        public static string RccaNroRegistros = "RCCANROREGISTROS";
        public static string RccaFecHorEnvio = "RCCAFECHORENVIO";
        public static string RccaEstadoEnvio = "RCCAESTADOENVIO";
        public static string RccaEstRegistro = "RCCAESTREGISTRO";
        public static string RccaUsuCreacion = "RCCAUSUCREACION";
        public static string RccaFecCreacion = "RCCAFECCREACION";
        public static string RccaUsuModificacion = "RCCAUSUMODIFICACION";
        public static string RccaFecModificacion = "RCCAFECMODIFICACION";
        public static string Enviocodi = "ENVIOCODI";

        #region Campos Paginacion
        public static string MaxRowToFetch = "MAXROWTOFETCH";
        public static string MinRowToFetch = "MINROWTOFETCH";
        #endregion

        #endregion


        #region SIOSEIN-PRIE-2021
        public string SqlGetByPeriodo
        {
            get { return base.GetSqlXml("GetByPeriodo"); }
        }
        #endregion

        public string SqlGetByCriteriaXTabla
        {
            get { return base.GetSqlXml("GetByCriteriaXTabla"); }
        }
    }
}