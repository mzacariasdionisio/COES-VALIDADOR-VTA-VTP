using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PF_REPORTE_TOTAL
    /// </summary>
    public class PfReporteTotalHelper : HelperBase
    {
        public PfReporteTotalHelper(): base(Consultas.PfReporteTotalSql)
        {
        }

        public PfReporteTotalDTO Create(IDataReader dr)
        {
            PfReporteTotalDTO entity = new PfReporteTotalDTO();

            int iPftotcodi = dr.GetOrdinal(this.Pftotcodi);
            if (!dr.IsDBNull(iPftotcodi)) entity.Pftotcodi = Convert.ToInt32(dr.GetValue(iPftotcodi));

            int iPftotpe = dr.GetOrdinal(this.Pftotpe);
            if (!dr.IsDBNull(iPftotpe)) entity.Pftotpe = dr.GetDecimal(iPftotpe);

            int iPftotpprom = dr.GetOrdinal(this.Pftotpprom);
            if (!dr.IsDBNull(iPftotpprom)) entity.Pftotpprom = dr.GetDecimal(iPftotpprom);

            int iPftotenergia = dr.GetOrdinal(this.Pftotenergia);
            if (!dr.IsDBNull(iPftotenergia)) entity.Pftotenergia = dr.GetDecimal(iPftotenergia);

            int iPftotminsincu = dr.GetOrdinal(this.Pftotminsincu);
            if (!dr.IsDBNull(iPftotminsincu)) entity.Pftotminsincu = Convert.ToInt32(dr.GetValue(iPftotminsincu));

            int iPftotpf = dr.GetOrdinal(this.Pftotpf);
            if (!dr.IsDBNull(iPftotpf)) entity.Pftotpf = dr.GetDecimal(iPftotpf);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquipadre = dr.GetOrdinal(this.Equipadre);
            if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

            int iFamcodi = dr.GetOrdinal(this.Famcodi);
            if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

            int iPftotfi = dr.GetOrdinal(this.Pftotfi);
            if (!dr.IsDBNull(iPftotfi)) entity.Pftotfi = dr.GetDecimal(iPftotfi);

            int iPftotfp = dr.GetOrdinal(this.Pftotfp);
            if (!dr.IsDBNull(iPftotfp)) entity.Pftotfp = dr.GetDecimal(iPftotfp);

            int iPftotpg = dr.GetOrdinal(this.Pftotpg);
            if (!dr.IsDBNull(iPftotpg)) entity.Pftotpg = dr.GetDecimal(iPftotpg);

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iPfescecodi = dr.GetOrdinal(this.Pfescecodi);
            if (!dr.IsDBNull(iPfescecodi)) entity.Pfescecodi = Convert.ToInt32(dr.GetValue(iPfescecodi));

            int iPftotincremental = dr.GetOrdinal(this.Pftotincremental);
            if (!dr.IsDBNull(iPftotincremental)) entity.Pftotincremental = Convert.ToInt32(dr.GetValue(iPftotincremental));

            int iPftotunidadnomb = dr.GetOrdinal(this.Pftotunidadnomb);
            if (!dr.IsDBNull(iPftotunidadnomb)) entity.Pftotunidadnomb = dr.GetString(iPftotunidadnomb);

            int iPftotnumdiapoc = dr.GetOrdinal(this.Pftotnumdiapoc);
            if (!dr.IsDBNull(iPftotnumdiapoc)) entity.Pftotnumdiapoc = Convert.ToInt32(dr.GetValue(iPftotnumdiapoc));

            return entity;
        }

        #region Mapeo de Campos

        public string Pftotcodi = "PFTOTCODI";
        public string Pftotpe = "PFTOTPE";
        public string Pftotpprom = "PFTOTPPROM";
        public string Pftotpf = "PFTOTPF";
        public string Emprcodi = "EMPRCODI";
        public string Equipadre = "EQUIPADRE";
        public string Famcodi = "FAMCODI";
        public string Pftotfi = "PFTOTFI";
        public string Pftotfp = "PFTOTFP";
        public string Pftotpg = "PFTOTPG";
        public string Grupocodi = "GRUPOCODI";
        public string Equicodi = "EQUICODI";
        public string Pfescecodi = "PFESCECODI";
        public string Pftotincremental = "PFTOTINCREMENTAL";
        public string Pftotunidadnomb = "PFTOTUNIDADNOMB";
        public string Pftotenergia = "PFTOTENERGIA";
        public string Pftotminsincu = "PFTOTMINSINCU";
        public string Pftotnumdiapoc = "PFTOTNUMDIAPOC";

        public string Emprnomb = "EMPRNOMB";
        public string Central = "CENTRAL";
        public string Equinomb = "EQUINOMB";
        public string Gruponomb = "Gruponomb";
        public string Grupotipocogen = "GRUPOTIPOCOGEN";
        public string Pfperianio = "PFPERIANIO";
        public string Pfperimes = "PFPERIMES";

        #endregion
    }
}
