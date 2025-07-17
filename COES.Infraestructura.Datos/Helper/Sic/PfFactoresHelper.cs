using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PF_FACTORES
    /// </summary>
    public class PfFactoresHelper : HelperBase
    {
        public PfFactoresHelper(): base(Consultas.PfFactoresSql)
        {
        }

        public PfFactoresDTO Create(IDataReader dr)
        {
            PfFactoresDTO entity = new PfFactoresDTO();

            int iPffactcodi = dr.GetOrdinal(this.Pffactcodi);
            if (!dr.IsDBNull(iPffactcodi)) entity.Pffactcodi = Convert.ToInt32(dr.GetValue(iPffactcodi));

            int iPfpericodi = dr.GetOrdinal(this.Pfpericodi);
            if (!dr.IsDBNull(iPfpericodi)) entity.Pfpericodi = Convert.ToInt32(dr.GetValue(iPfpericodi));

            int iPfverscodi = dr.GetOrdinal(this.Pfverscodi);
            if (!dr.IsDBNull(iPfverscodi)) entity.Pfverscodi = Convert.ToInt32(dr.GetValue(iPfverscodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iFamcodi = dr.GetOrdinal(this.Famcodi);
            if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

            int iEquipadre = dr.GetOrdinal(this.Equipadre);
            if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iPffacttipo = dr.GetOrdinal(this.Pffacttipo);
            if (!dr.IsDBNull(iPffacttipo)) entity.Pffacttipo = Convert.ToInt32(dr.GetValue(iPffacttipo));

            int iPffactfi = dr.GetOrdinal(this.Pffactfi);
            if (!dr.IsDBNull(iPffactfi)) entity.Pffactfi = dr.GetDecimal(iPffactfi);

            int iPffactfp = dr.GetOrdinal(this.Pffactfp);
            if (!dr.IsDBNull(iPffactfp)) entity.Pffactfp = dr.GetDecimal(iPffactfp);

            int iPffactincremental = dr.GetOrdinal(this.Pffactincremental);
            if (!dr.IsDBNull(iPffactincremental)) entity.Pffactincremental = Convert.ToInt32(dr.GetValue(iPffactincremental));

            int iPffactunidadnomb = dr.GetOrdinal(this.Pffactunidadnomb);
            if (!dr.IsDBNull(iPffactunidadnomb)) entity.Pffactunidadnomb = dr.GetString(iPffactunidadnomb);

            return entity;
        }


        #region Mapeo de Campos

        public string Pffactcodi = "PFFACTCODI";
        public string Pfpericodi = "PFPERICODI";
        public string Pfverscodi = "PFVERSCODI";
        public string Emprcodi = "EMPRCODI";
        public string Famcodi = "FAMCODI";
        public string Equipadre = "EQUIPADRE";
        public string Equicodi = "EQUICODI";
        public string Grupocodi = "GRUPOCODI";
        public string Pffacttipo = "PFFACTTIPO";
        public string Pffactfi = "PFFACTFI";
        public string Pffactfp = "PFFACTFP";
        public string Pffactincremental = "PFFACTINCREMENTAL";
        public string Pffactunidadnomb = "PFFACTUNIDADNOMB";

        //campos adicionales
        public string Central = "CENTRAL";
        public string Emprnomb = "EMPRNOMB";
        #endregion

        #region Mapeo de Consultas
        public string SqlListarFactorIndispFiltro
        {
            get { return base.GetSqlXml("ListarFactorIndispFiltro"); }
        }
        #endregion
    }
}
