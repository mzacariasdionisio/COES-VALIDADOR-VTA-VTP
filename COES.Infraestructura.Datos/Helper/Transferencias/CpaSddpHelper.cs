using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CPA_SDDP
    /// </summary>
    public class CpaSddpHelper : HelperBase
    {
        public CpaSddpHelper() : base(Consultas.CpaSddpSql)
        {
        }

        public CpaSddpDTO Create(IDataReader dr)
        {
            CpaSddpDTO entity = new CpaSddpDTO();

            int iCpsddpcodi = dr.GetOrdinal(Cpsddpcodi);
            if (!dr.IsDBNull(iCpsddpcodi)) entity.Cpsddpcodi = dr.GetInt32(iCpsddpcodi);

            int iCparcodi = dr.GetOrdinal(Cparcodi);
            if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = dr.GetInt32(iCparcodi);

            int iCpsddpcorrelativo = dr.GetOrdinal(Cpsddpcorrelativo);
            if (!dr.IsDBNull(iCpsddpcorrelativo)) entity.Cpsddpcorrelativo = dr.GetInt32(iCpsddpcorrelativo);

            int iCpsddpnomarchivo = dr.GetOrdinal(Cpsddpnomarchivo);
            if (!dr.IsDBNull(iCpsddpnomarchivo)) entity.Cpsddpnomarchivo = dr.GetString(iCpsddpnomarchivo);

            int iCpsddpsemanaini = dr.GetOrdinal(Cpsddpsemanaini);
            if (!dr.IsDBNull(iCpsddpsemanaini)) entity.Cpsddpsemanaini = dr.GetInt32(iCpsddpsemanaini);

            int iCpsddpanioini = dr.GetOrdinal(Cpsddpanioini);
            if (!dr.IsDBNull(iCpsddpanioini)) entity.Cpsddpanioini = dr.GetInt32(iCpsddpanioini);

            int iCpsddpnroseries = dr.GetOrdinal(Cpsddpnroseries);
            if (!dr.IsDBNull(iCpsddpnroseries)) entity.Cpsddpnroseries = dr.GetInt32(iCpsddpnroseries);

            int iCpsddpdiainicio = dr.GetOrdinal(Cpsddpdiainicio);
            if (!dr.IsDBNull(iCpsddpdiainicio)) entity.Cpsddpdiainicio = dr.GetDateTime(iCpsddpdiainicio);

            int iCpsddpusucreacion = dr.GetOrdinal(Cpsddpusucreacion);
            if (!dr.IsDBNull(iCpsddpusucreacion)) entity.Cpsddpusucreacion = dr.GetString(iCpsddpusucreacion);

            int iCpsddpfeccreacion = dr.GetOrdinal(Cpsddpfeccreacion);
            if (!dr.IsDBNull(iCpsddpfeccreacion)) entity.Cpsddpfeccreacion = dr.GetDateTime(iCpsddpfeccreacion);

            return entity;
        }

        #region Mapeo de Campos
        public string Cpsddpcodi = "CPSDDPCODI";
        public string Cparcodi = "CPARCODI";
        public string Cpsddpcorrelativo = "CPSDDPCORRELATIVO";
        public string Cpsddpnomarchivo = "CPSDDPNOMARCHIVO";
        public string Cpsddpsemanaini = "CPSDDPSEMANAINI";
        public string Cpsddpanioini = "CPSDDPANIOINI";
        public string Cpsddpnroseries = "CPSDDPNROSERIES";
        public string Cpsddpdiainicio = "CPSDDPDIAINICIO";
        public string Cpsddpusucreacion = "CPSDDPUSUCREACION";
        public string Cpsddpfeccreacion = "CPSDDPFECCREACION";

        public string SqlGetMaxCorrelativo
        {
            get { return base.GetSqlXml("GetMaxCorrelativo"); }
        }
        #endregion
    }
}
