using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RER_CENTRAL_LVTP
    /// </summary>
    public class RerCentralLvtpHelper : HelperBase
    {
        public RerCentralLvtpHelper() : base(Consultas.RerCentralLvtpSql)
        {
        }

        public RerCentralLvtpDTO Create(IDataReader dr)
        {
            RerCentralLvtpDTO entity = new RerCentralLvtpDTO();

            int iRerctpcodi = dr.GetOrdinal(this.Rerctpcodi);
            if (!dr.IsDBNull(iRerctpcodi)) entity.Rerctpcodi = Convert.ToInt32(dr.GetValue(iRerctpcodi));

            int iRercencodi = dr.GetOrdinal(this.Rercencodi);
            if (!dr.IsDBNull(iRercencodi)) entity.Rercencodi = Convert.ToInt32(dr.GetValue(iRercencodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iRerctpusucreacion = dr.GetOrdinal(this.Rerctpusucreacion);
            if (!dr.IsDBNull(iRerctpusucreacion)) entity.Rerctpusucreacion = dr.GetString(iRerctpusucreacion);

            int iRerctpfeccreacion = dr.GetOrdinal(this.Rerctpfeccreacion);
            if (!dr.IsDBNull(iRerctpfeccreacion)) entity.Rerctpfeccreacion = dr.GetDateTime(iRerctpfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Rerctpcodi = "RERCTPCODI";
        public string Rercencodi = "RERCENCODI";
        public string Equicodi = "EQUICODI";
        public string Rerctpusucreacion = "RERCTPUSUCREACION";
        public string Rerctpfeccreacion = "RERCTPFECCREACION";

        public string Equinomb = "EQUINOMB"; 
        #endregion

        public string SqlListByRercencodi
        {
            get { return base.GetSqlXml("ListByRercencodi"); }
        }
        public string SqlDeleteAllByRercencodi
        {
            get { return base.GetSqlXml("DeleteAllByRercencodi"); }
        }

    }
}