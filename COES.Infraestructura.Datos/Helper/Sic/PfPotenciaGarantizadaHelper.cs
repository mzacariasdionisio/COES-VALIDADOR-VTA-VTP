using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PF_POTENCIA_GARANTIZADA
    /// </summary>
    public class PfPotenciaGarantizadaHelper : HelperBase
    {
        public PfPotenciaGarantizadaHelper() : base(Consultas.PfPotenciaGarantizadaSql)
        {
        }

        public PfPotenciaGarantizadaDTO Create(IDataReader dr)
        {
            PfPotenciaGarantizadaDTO entity = new PfPotenciaGarantizadaDTO();

            int iPfpgarcodi = dr.GetOrdinal(this.Pfpgarcodi);
            if (!dr.IsDBNull(iPfpgarcodi)) entity.Pfpgarcodi = Convert.ToInt32(dr.GetValue(iPfpgarcodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iEquipadre = dr.GetOrdinal(this.Equipadre);
            if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iPfpgarpe = dr.GetOrdinal(this.Pfpgarpe);
            if (!dr.IsDBNull(iPfpgarpe)) entity.Pfpgarpe = dr.GetDecimal(iPfpgarpe);

            int iPfpgarpg = dr.GetOrdinal(this.Pfpgarpg);
            if (!dr.IsDBNull(iPfpgarpg)) entity.Pfpgarpg = dr.GetDecimal(iPfpgarpg);

            int iPfpericodi = dr.GetOrdinal(this.Pfpericodi);
            if (!dr.IsDBNull(iPfpericodi)) entity.Pfpericodi = Convert.ToInt32(dr.GetValue(iPfpericodi));

            int iPfverscodi = dr.GetOrdinal(this.Pfverscodi);
            if (!dr.IsDBNull(iPfverscodi)) entity.Pfverscodi = Convert.ToInt32(dr.GetValue(iPfverscodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iPfpgarunidadnomb = dr.GetOrdinal(this.Pfpgarunidadnomb);
            if (!dr.IsDBNull(iPfpgarunidadnomb)) entity.Pfpgarunidadnomb = dr.GetString(iPfpgarunidadnomb);

            return entity;
        }

        #region Mapeo de Campos

        public string Pfpgarcodi = "PFPGARCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Equipadre = "EQUIPADRE";
        public string Emprcodi = "EMPRCODI";
        public string Pfpgarpe = "PFPGARPE";
        public string Pfpgarpg = "PFPGARPG";
        public string Pfpericodi = "PFPERICODI";
        public string Pfverscodi = "PFVERSCODI";
        public string Equicodi = "EQUICODI";
        public string Pfpgarunidadnomb = "PFPGARUNIDADNOMB";

        //campos adicionales
        public string Central = "CENTRAL";
        public string Emprnomb = "EMPRNOMB";
        #endregion

        #region Mapeo de Consultas
        public string SqlListarPotGarantizadaFiltro
        {
            get { return base.GetSqlXml("ListarPotGarantizadaFiltro"); }
        }
        #endregion

    }
}
