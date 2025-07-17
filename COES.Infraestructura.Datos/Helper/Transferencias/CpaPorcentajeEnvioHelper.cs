using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CPA_PORCENTAJE_ENVIO
    /// </summary>
    public class CpaPorcentajeEnvioHelper : HelperBase
    {
        #region Mapeo de Campos
        public string Cpapecodi = "CPAPECODI";
        public string Cpapcodi = "CPAPCODI";
        public string Cparcodi = "CPARCODI";
        public string Cpapetipo = "CPAPETIPO";
        public string Cpapemes = "CPAPEMES";
        public string Cpapenumenvio = "CPAPENUMENVIO";
        public string Cpapeusucreacion = "CPAPEUSUCREACION";
        public string Cpapefeccreacion = "CPAPEFECCREACION";
        #endregion

        public CpaPorcentajeEnvioHelper() : base(Consultas.CpaPorcentajeEnvioSql)
        {
        }

        public CpaPorcentajeEnvioDTO Create(IDataReader dr)
        {
            CpaPorcentajeEnvioDTO entity = new CpaPorcentajeEnvioDTO();

            int iCpapecodi = dr.GetOrdinal(Cpapecodi);
            if (!dr.IsDBNull(iCpapecodi)) entity.Cpapecodi = dr.GetInt32(iCpapecodi);

            int iCpapcodi = dr.GetOrdinal(Cpapcodi);
            if (!dr.IsDBNull(iCpapcodi)) entity.Cpapcodi = dr.GetInt32(iCpapcodi);

            int iCparcodi = dr.GetOrdinal(Cparcodi);
            if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = dr.GetInt32(iCparcodi);

            int iCpapetipo = dr.GetOrdinal(Cpapetipo);
            if (!dr.IsDBNull(iCpapetipo)) entity.Cpapetipo = dr.GetString(iCpapetipo);

            int iCpapemes = dr.GetOrdinal(Cpapemes);
            if (!dr.IsDBNull(iCpapemes)) entity.Cpapemes = dr.GetInt32(iCpapemes);

            int iCpapenumenvio = dr.GetOrdinal(Cpapenumenvio);
            if (!dr.IsDBNull(iCpapenumenvio)) entity.Cpapenumenvio = dr.GetInt32(iCpapenumenvio);

            int iCpapeusucreacion = dr.GetOrdinal(Cpapeusucreacion);
            if (!dr.IsDBNull(iCpapeusucreacion)) entity.Cpapeusucreacion = dr.GetString(iCpapeusucreacion);

            int iCpapefeccreacion = dr.GetOrdinal(Cpapefeccreacion);
            if (!dr.IsDBNull(iCpapefeccreacion)) entity.Cpapefeccreacion = dr.GetDateTime(iCpapefeccreacion);

            return entity;
        }

        public string SqlDeleteByRevision
        {
            get { return base.GetSqlXml("DeleteByRevision"); }
        }

        public string SqlListByRevision
        {
            get { return base.GetSqlXml("ListByRevision"); }
        }
    }

}
