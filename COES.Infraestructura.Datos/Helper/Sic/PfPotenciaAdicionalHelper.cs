using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PF_POTENCIA_ADICIONAL
    /// </summary>
    public class PfPotenciaAdicionalHelper : HelperBase
    {
        public PfPotenciaAdicionalHelper(): base(Consultas.PfPotenciaAdicionalSql)
        {
        }

        public PfPotenciaAdicionalDTO Create(IDataReader dr)
        {
            PfPotenciaAdicionalDTO entity = new PfPotenciaAdicionalDTO();

            int iPfpadicodi = dr.GetOrdinal(this.Pfpadicodi);
            if (!dr.IsDBNull(iPfpadicodi)) entity.Pfpadicodi = Convert.ToInt32(dr.GetValue(iPfpadicodi));

            int iFamcodi = dr.GetOrdinal(this.Famcodi);
            if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

            int iEquipadre = dr.GetOrdinal(this.Equipadre);
            if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iPfpadipe = dr.GetOrdinal(this.Pfpadipe);
            if (!dr.IsDBNull(iPfpadipe)) entity.Pfpadipe = dr.GetDecimal(iPfpadipe);

            int iPfpadifi = dr.GetOrdinal(this.Pfpadifi);
            if (!dr.IsDBNull(iPfpadifi)) entity.Pfpadifi = dr.GetDecimal(iPfpadifi);

            int iPfpadipf = dr.GetOrdinal(this.Pfpadipf);
            if (!dr.IsDBNull(iPfpadipf)) entity.Pfpadipf = dr.GetDecimal(iPfpadipf);

            int iPfpericodi = dr.GetOrdinal(this.Pfpericodi);
            if (!dr.IsDBNull(iPfpericodi)) entity.Pfpericodi = Convert.ToInt32(dr.GetValue(iPfpericodi));

            int iPfverscodi = dr.GetOrdinal(this.Pfverscodi);
            if (!dr.IsDBNull(iPfverscodi)) entity.Pfverscodi = Convert.ToInt32(dr.GetValue(iPfverscodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iPfpadiincremental = dr.GetOrdinal(this.Pfpadiincremental);
            if (!dr.IsDBNull(iPfpadiincremental)) entity.Pfpadiincremental = Convert.ToInt32(dr.GetValue(iPfpadiincremental));

            int iPfpadiunidadnomb = dr.GetOrdinal(this.Pfpadiunidadnomb);
            if (!dr.IsDBNull(iPfpadiunidadnomb)) entity.Pfpadiunidadnomb = dr.GetString(iPfpadiunidadnomb);

            return entity;
        }


        #region Mapeo de Campos

        public string Pfpadicodi = "PFPADICODI";
        public string Famcodi = "FAMCODI";
        public string Equipadre = "EQUIPADRE";
        public string Emprcodi = "EMPRCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Pfpadipe = "PFPADIPE";
        public string Pfpadifi = "PFPADIFI";
        public string Pfpadipf = "PFPADIPF";
        public string Pfpericodi = "PFPERICODI";
        public string Pfverscodi = "PFVERSCODI";
        public string Equicodi = "EQUICODI";
        public string Pfpadiincremental = "PFPADIINCREMENTAL";
        public string Pfpadiunidadnomb = "PFPADIUNIDADNOMB";

        //campos adicionales
        public string Central = "CENTRAL";
        public string Emprnomb = "EMPRNOMB";
        #endregion

        #region Mapeo de Consultas
        public string SqlListarPotAdicionalFiltro
        {
            get { return base.GetSqlXml("ListarPotAdicionalFiltro"); }
        }
        #endregion

    }
}
