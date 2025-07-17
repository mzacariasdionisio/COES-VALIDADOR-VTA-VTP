using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PF_DISPCALORUTIL
    /// </summary>
    public class PfDispcalorutilHelper : HelperBase
    {
        public PfDispcalorutilHelper() : base(Consultas.PfDispcalorutilSql)
        {
        }

        public PfDispcalorutilDTO Create(IDataReader dr)
        {
            PfDispcalorutilDTO entity = new PfDispcalorutilDTO();

            int iPfcucodi = dr.GetOrdinal(this.Pfcucodi);
            if (!dr.IsDBNull(iPfcucodi)) entity.Pfcucodi = Convert.ToInt32(dr.GetValue(iPfcucodi));

            int iPfcufecha = dr.GetOrdinal(this.Pfcufecha);
            if (!dr.IsDBNull(iPfcufecha)) entity.Pfcufecha = dr.GetDateTime(iPfcufecha);

            int iPfcuh = dr.GetOrdinal(this.Pfcuh);
            if (!dr.IsDBNull(iPfcuh)) entity.Pfcuh = Convert.ToInt32(dr.GetValue(iPfcuh));

            int iPfcutienedisp = dr.GetOrdinal(this.Pfcutienedisp);
            if (!dr.IsDBNull(iPfcutienedisp)) entity.Pfcutienedisp = Convert.ToInt32(dr.GetValue(iPfcutienedisp));

            int iPfverscodi = dr.GetOrdinal(this.Irptcodi);
            if (!dr.IsDBNull(iPfverscodi)) entity.Irptcodi = Convert.ToInt32(dr.GetValue(iPfverscodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquipadre = dr.GetOrdinal(this.Equipadre);
            if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

            int iPfcumin = dr.GetOrdinal(this.Pfcumin);
            if (!dr.IsDBNull(iPfcumin)) entity.Pfcumin = Convert.ToInt32(dr.GetValue(iPfcumin));

            return entity;
        }


        #region Mapeo de Campos

        public string Pfcucodi = "PFCUCODI";
        public string Pfcufecha = "PFCUFECHA";
        public string Pfcuh = "PFCUH";
        public string Pfcutienedisp = "PFCUTIENEDISP";
        public string Irptcodi = "IRPTCODI";
        public string Emprcodi = "EMPRCODI";
        public string Equipadre = "EQUIPADRE";
        public string Grupocodi = "GRUPOCODI";
        public string Equicodi = "EQUICODI";
        public string Pfcumin = "PFCUMIN";

        #endregion
    }
}
