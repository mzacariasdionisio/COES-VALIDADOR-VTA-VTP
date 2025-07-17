using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RER_INSUMO_VTP
    /// </summary>
    public class RerInsumoVtpHelper : HelperBase
    {
        public RerInsumoVtpHelper() : base(Consultas.RerInsumoVtpSql)
        {
        }

        public RerInsumoVtpDTO Create(IDataReader dr)
        {
            RerInsumoVtpDTO entity = new RerInsumoVtpDTO();

            int iRerInpcodi = dr.GetOrdinal(this.Rerinpcodi);
            if (!dr.IsDBNull(iRerInpcodi)) entity.Rerinpcodi = Convert.ToInt32(dr.GetValue(iRerInpcodi));

            int iRerinscodi = dr.GetOrdinal(this.Rerinscodi);
            if (!dr.IsDBNull(iRerinscodi)) entity.Rerinscodi = Convert.ToInt32(dr.GetValue(iRerinscodi));

            int iRerPrcodi = dr.GetOrdinal(this.Rerpprcodi);
            if (!dr.IsDBNull(iRerPrcodi)) entity.Rerpprcodi = Convert.ToInt32(dr.GetValue(iRerPrcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iRerInpanio = dr.GetOrdinal(this.Rerinpanio);
            if (!dr.IsDBNull(iRerInpanio)) entity.Rerinpanio = Convert.ToInt32(dr.GetValue(iRerInpanio));

            int iRerInpmes = dr.GetOrdinal(this.Rerinpmes);
            if (!dr.IsDBNull(iRerInpmes)) entity.Rerinpmes = Convert.ToInt32(dr.GetValue(iRerInpmes));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iRecpotcodi = dr.GetOrdinal(this.Recpotcodi);
            if (!dr.IsDBNull(iRecpotcodi)) entity.Recpotcodi = Convert.ToInt32(dr.GetValue(iRecpotcodi));

            int iRerInpmestotal = dr.GetOrdinal(this.Rerinpmestotal);
            if (!dr.IsDBNull(iRerInpmestotal)) entity.Rerinpmestotal = dr.GetDecimal(iRerInpmestotal);

            int iRerInpmesusucreacion = dr.GetOrdinal(this.Rerinpmesusucreacion);
            if (!dr.IsDBNull(iRerInpmesusucreacion)) entity.Rerinpmesusucreacion = dr.GetString(iRerInpmesusucreacion);

            int iRerInpmesfeccreacion = dr.GetOrdinal(this.Rerinpmesfeccreacion);
            if (!dr.IsDBNull(iRerInpmesfeccreacion)) entity.Rerinpmesfeccreacion = dr.GetDateTime(iRerInpmesfeccreacion);

            return entity;
        }

        public RerInsumoVtpDTO CreateByPeriodo(IDataReader dr)
        {
            RerInsumoVtpDTO entity = Create(dr);

            //Additonal
            int iPerinombre = dr.GetOrdinal(this.Perinombre);
            if (!dr.IsDBNull(iPerinombre)) entity.Perinombre = dr.GetString(iPerinombre);

            int iRecpotnombre = dr.GetOrdinal(this.Recpotnombre);
            if (!dr.IsDBNull(iRecpotnombre)) entity.Recpotnombre = dr.GetString(iRecpotnombre);

            return entity;
        }

        #region Mapeo de Campos
        public string Rerinpcodi = "RERINPCODI";
        public string Rerinscodi = "RERINSCODI";
        public string Rerpprcodi = "RERPPRCODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodi = "EQUICODI";
        public string Rerinpanio = "RERINPANIO";
        public string Rerinpmes = "RERINPMES";
        public string Pericodi = "PERICODI";
        public string Recpotcodi = "RECPOTCODI";
        public string Rerinpmestotal = "RERINPMESTOTAL";
        public string Rerinpmesusucreacion = "RERINPMESUSUCREACION";
        public string Rerinpmesfeccreacion = "RERINPMESFECCREACION";

        //Additonal
        public string Perinombre = "PERINOMBRE";
        public string Recpotnombre = "RECPOTNOMBRE";
        #endregion

        public string SqlDeleteByParametroPrimaAndMes
        {
            get { return base.GetSqlXml("DeleteByParametroPrimaAndMes"); }
        }

        public string SqlGetByPeriodo
        {
            get { return base.GetSqlXml("GetByPeriodo"); }
        }

        public string SqlObtenerSaldoVtpByInsumoVTP
        {
            get { return base.GetSqlXml("ObtenerSaldoVtpByInsumoVTP"); }
        }
    }
}
