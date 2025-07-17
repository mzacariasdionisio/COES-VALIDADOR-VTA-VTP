using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CPA_PORCENTAJE
    /// </summary>
    public class CpaPorcentajeHelper : HelperBase
    {
        #region Mapeo de Campos
        public string Cpapcodi = "CPAPCODI";
        public string Cparcodi = "CPARCODI";
        public string Cpaplog = "CPAPLOG";
        public string Cpapestpub = "CPAPESTPUB";
        public string Cpapusucreacion = "CPAPUSUCREACION";
        public string Cpapfeccreacion = "CPAPFECCREACION";
        public string Cpapusumodificacion = "CPAPUSUMODIFICACION";
        public string Cpapfecmodificacion = "CPAPFECMODIFICACION";
        #endregion

        public CpaPorcentajeHelper() : base(Consultas.CpaPorcentajeSql)
        {
        }

        public CpaPorcentajeDTO Create(IDataReader dr)
        {
            CpaPorcentajeDTO entity = new CpaPorcentajeDTO();

            int iCpapcodi = dr.GetOrdinal(Cpapcodi);
            if (!dr.IsDBNull(iCpapcodi)) entity.Cpapcodi = dr.GetInt32(iCpapcodi);

            int iCparcodi = dr.GetOrdinal(Cparcodi);
            if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = dr.GetInt32(iCparcodi);

            int iCpaplog = dr.GetOrdinal(Cpaplog);
            if (!dr.IsDBNull(iCpaplog)) entity.Cpaplog = dr.GetString(iCpaplog);

            int iCpapestpub = dr.GetOrdinal(Cpapestpub);
            if (!dr.IsDBNull(iCpapestpub)) entity.Cpapestpub = dr.GetString(iCpapestpub);

            int iCpapusucreacion = dr.GetOrdinal(Cpapusucreacion);
            if (!dr.IsDBNull(iCpapusucreacion)) entity.Cpapusucreacion = dr.GetString(iCpapusucreacion);

            int iCpapfeccreacion = dr.GetOrdinal(Cpapfeccreacion);
            if (!dr.IsDBNull(iCpapfeccreacion)) entity.Cpapfeccreacion = dr.GetDateTime(iCpapfeccreacion);

            int iCpapusumodificacion = dr.GetOrdinal(Cpapusumodificacion);
            if (!dr.IsDBNull(iCpapusumodificacion)) entity.Cpapusumodificacion = dr.GetString(iCpapusumodificacion);

            int iCpapfecmodificacion = dr.GetOrdinal(Cpapfecmodificacion);
            if (!dr.IsDBNull(iCpapfecmodificacion)) entity.Cpapfecmodificacion = dr.GetDateTime(iCpapfecmodificacion);

            return entity;
        }

        public string SqlUpdateEstadoPublicacion
        {
            get { return base.GetSqlXml("UpdateEstadoPublicacion"); }
        }

        public string SqlDeleteByRevision
        {
            get { return base.GetSqlXml("DeleteByRevision"); }
        }

    }

}

