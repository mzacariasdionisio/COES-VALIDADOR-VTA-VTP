using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PFR_REPORTE_TOTAL
    /// </summary>
    public class PfrReporteTotalHelper : HelperBase
    {
        public PfrReporteTotalHelper(): base(Consultas.PfrReporteTotalSql)
        {
        }

        public PfrReporteTotalDTO Create(IDataReader dr)
        {
            PfrReporteTotalDTO entity = new PfrReporteTotalDTO();

            int iPfrtotcodi = dr.GetOrdinal(this.Pfrtotcodi);
            if (!dr.IsDBNull(iPfrtotcodi)) entity.Pfrtotcodi = Convert.ToInt32(dr.GetValue(iPfrtotcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquipadre = dr.GetOrdinal(this.Equipadre);
            if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iFamcodi = dr.GetOrdinal(this.Famcodi);
            if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iPfrtotunidadnomb = dr.GetOrdinal(this.Pfrtotunidadnomb);
            if (!dr.IsDBNull(iPfrtotunidadnomb)) entity.Pfrtotunidadnomb = dr.GetString(iPfrtotunidadnomb);

            int iPfresccodi = dr.GetOrdinal(this.Pfresccodi);
            if (!dr.IsDBNull(iPfresccodi)) entity.Pfresccodi = Convert.ToInt32(dr.GetValue(iPfresccodi));

            int iPfrtotcv = dr.GetOrdinal(this.Pfrtotcv);
            if (!dr.IsDBNull(iPfrtotcv)) entity.Pfrtotcv = dr.GetDecimal(iPfrtotcv);

            int iPfrtotpe = dr.GetOrdinal(this.Pfrtotpe);
            if (!dr.IsDBNull(iPfrtotpe)) entity.Pfrtotpe = dr.GetDecimal(iPfrtotpe);

            int iPfrtotpea = dr.GetOrdinal(this.Pfrtotpea);
            if (!dr.IsDBNull(iPfrtotpea)) entity.Pfrtotpea = dr.GetDecimal(iPfrtotpea);

            int iPfrtotfi = dr.GetOrdinal(this.Pfrtotfi);
            if (!dr.IsDBNull(iPfrtotfi)) entity.Pfrtotfi = dr.GetDecimal(iPfrtotfi);

            int iPfrtotpf = dr.GetOrdinal(this.Pfrtotpf);
            if (!dr.IsDBNull(iPfrtotpf)) entity.Pfrtotpf = dr.GetDecimal(iPfrtotpf);

            int iPfrtotpfc = dr.GetOrdinal(this.Pfrtotpfc);
            if (!dr.IsDBNull(iPfrtotpfc)) entity.Pfrtotpfc = dr.GetDecimal(iPfrtotpfc);

            int iPfrtotpd = dr.GetOrdinal(this.Pfrtotpd);
            if (!dr.IsDBNull(iPfrtotpd)) entity.Pfrtotpd = dr.GetDecimal(iPfrtotpd);

            int iPfrtotcvf = dr.GetOrdinal(this.Pfrtotcvf);
            if (!dr.IsDBNull(iPfrtotcvf)) entity.Pfrtotcvf = dr.GetDecimal(iPfrtotcvf);

            int iPfrtotpdd = dr.GetOrdinal(this.Pfrtotpdd);
            if (!dr.IsDBNull(iPfrtotpdd)) entity.Pfrtotpdd = dr.GetDecimal(iPfrtotpdd);

            int iPfrtotpfr = dr.GetOrdinal(this.Pfrtotpfr);
            if (!dr.IsDBNull(iPfrtotpfr)) entity.Pfrtotpfr = dr.GetDecimal(iPfrtotpfr);
			
			int iPfrtotcrmesant = dr.GetOrdinal(this.Pfrtotcrmesant);
            if (!dr.IsDBNull(iPfrtotcrmesant)) entity.Pfrtotcrmesant = Convert.ToInt32(dr.GetValue(iPfrtotcrmesant));

            int iPfrtotfkmesant = dr.GetOrdinal(this.Pfrtotfkmesant);
            if (!dr.IsDBNull(iPfrtotfkmesant)) entity.Pfrtotfkmesant = dr.GetDecimal(iPfrtotfkmesant);

            int iPfrtotficticio = dr.GetOrdinal(this.Pfrtotficticio);
            if (!dr.IsDBNull(iPfrtotficticio)) entity.Pfrtotficticio = Convert.ToInt32(dr.GetValue(iPfrtotficticio));

            return entity;
        }


        #region Mapeo de Campos

        public string Pfrtotcodi = "PFRTOTCODI";
        public string Emprcodi = "EMPRCODI";
        public string Equipadre = "EQUIPADRE";
        public string Equicodi = "EQUICODI";
        public string Famcodi = "FAMCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Pfrtotunidadnomb = "PFRTOTUNIDADNOMB";
        public string Pfresccodi = "PFRESCCODI";
        public string Pfrtotcv = "PFRTOTCV";
        public string Pfrtotpe = "PFRTOTPE";
        public string Pfrtotpea = "PFRTOTPEA";
        public string Pfrtotfi = "PFRTOTFI";
        public string Pfrtotpf = "PFRTOTPF";
        public string Pfrtotpfc = "PFRTOTPFC";
        public string Pfrtotpd = "PFRTOTPD";
        public string Pfrtotcvf = "PFRTOTCVF";
        public string Pfrtotpdd = "PFRTOTPDD";
        public string Pfrtotpfr = "PFRTOTPFR";
		public string Pfrtotcrmesant = "PFRTOTCRMESANT";
        public string Pfrtotfkmesant = "PFRTOTFKMESANT";
        public string Pfrtotficticio = "PFRTOTFICTICIO";

        public string Emprnomb = "EMPRNOMB";
        public string Central = "CENTRAL";
        public string Equinomb = "EQUINOMB";
        public string Gruponomb = "Gruponomb";
        public string Grupotipocogen = "GRUPOTIPOCOGEN";

        #endregion

        public string SqlGetByReportecodi
        {
            get { return base.GetSqlXml("ListByReportecodi"); }
        }
    }
}
