using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RER_SDDP
    /// </summary>
    public class RerSddpHelper : HelperBase
    {
        public RerSddpHelper() : base(Consultas.RerSddpSql)
        {
        }

        public RerSddpDTO Create(IDataReader dr)
        {
            RerSddpDTO entity = new RerSddpDTO();

            int iResddpcodi = dr.GetOrdinal(this.Resddpcodi);
            if (!dr.IsDBNull(iResddpcodi)) entity.Resddpcodi = Convert.ToInt32(dr.GetValue(iResddpcodi));

            int iReravcodi = dr.GetOrdinal(this.Reravcodi);
            if (!dr.IsDBNull(iReravcodi)) entity.Reravcodi = Convert.ToInt32(dr.GetValue(iReravcodi));

            int iResddpnomarchivo = dr.GetOrdinal(this.Resddpnomarchivo);
            if (!dr.IsDBNull(iResddpnomarchivo)) entity.Resddpnomarchivo = dr.GetString(iResddpnomarchivo);

            int iResddpsemanaini = dr.GetOrdinal(this.Resddpsemanaini);
            if (!dr.IsDBNull(iResddpsemanaini)) entity.Resddpsemanaini = Convert.ToInt32(dr.GetValue(iResddpsemanaini));

            int iResddpanioini = dr.GetOrdinal(this.Resddpanioini);
            if (!dr.IsDBNull(iResddpanioini)) entity.Resddpanioini = Convert.ToInt32(dr.GetValue(iResddpanioini));

            int iResddpnroseries = dr.GetOrdinal(this.Resddpnroseries);
            if (!dr.IsDBNull(iResddpnroseries)) entity.Resddpnroseries = Convert.ToInt32(dr.GetValue(iResddpnroseries));

            int iResddpdiainicio = dr.GetOrdinal(this.Resddpdiainicio);
            if (!dr.IsDBNull(iResddpdiainicio)) entity.Resddpdiainicio = dr.GetDateTime(iResddpdiainicio);

            int iResddpusucreacion = dr.GetOrdinal(this.Resddpusucreacion);
            if (!dr.IsDBNull(iResddpusucreacion)) entity.Resddpusucreacion = dr.GetString(iResddpusucreacion);

            int iResddpfeccreacion = dr.GetOrdinal(this.Resddpfeccreacion);
            if (!dr.IsDBNull(iResddpfeccreacion)) entity.Resddpfeccreacion = dr.GetDateTime(iResddpfeccreacion);

            return entity;
        }

        #region Mapeo de Campos
        public string Resddpcodi = "RESDDPCODI";
        public string Reravcodi = "RERAVCODI";
        public string Resddpnomarchivo = "RESDDPNOMARCHIVO";
        public string Resddpsemanaini = "RESDDPSEMANAINI";
        public string Resddpanioini = "RESDDPANIOINI";
        public string Resddpnroseries = "RESDDPNROSERIES";
        public string Resddpdiainicio = "RESDDPDIAINICIO";
        public string Resddpusucreacion = "RESDDPUSUCREACION";
        public string Resddpfeccreacion = "RESDDPFECCREACION";
        #endregion
    }
}

