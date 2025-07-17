using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_EQUIPODAT
    /// </summary>
    public class PrEquipodatHelper : HelperBase
    {
        public PrEquipodatHelper(): base(Consultas.PrEquipodatSql)
        {
        }

        public PrEquipodatDTO Create(IDataReader dr)
        {
            PrEquipodatDTO entity = new PrEquipodatDTO();

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iConcepcodi = dr.GetOrdinal(this.Concepcodi);
            if (!dr.IsDBNull(iConcepcodi)) entity.Concepcodi = Convert.ToInt32(dr.GetValue(iConcepcodi));

            int iFormuladat = dr.GetOrdinal(this.Formuladat);
            if (!dr.IsDBNull(iFormuladat)) entity.Formuladat = dr.GetString(iFormuladat);

            int iFechadat = dr.GetOrdinal(this.Fechadat);
            if (!dr.IsDBNull(iFechadat)) entity.Fechadat = dr.GetDateTime(iFechadat);

            int iDeleted = dr.GetOrdinal(this.Deleted);
            if (!dr.IsDBNull(iDeleted)) entity.Deleted = Convert.ToInt32(dr.GetValue(iDeleted));

            return entity;
        }


        #region Mapeo de Campos

        public string Equicodi = "EQUICODI";
        public string Grupocodi = "GRUPOCODI";
        public string Concepcodi = "CONCEPCODI";
        public string Formuladat = "FORMULADAT";
        public string Fechadat = "FECHADAT";
        public string Deleted = "DELETED";

        #endregion

        public string SqlValoresModoOperacionEquipoDat
        {
            get { return base.GetSqlXml("SqlValoresModoOperacionEquipoDat"); }
        }
    }
}

