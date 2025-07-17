using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_DESPACHO_PRODGEN
    /// </summary>
    public class MeDespachoProdgenHelper : HelperBase
    {
        public MeDespachoProdgenHelper(): base(Consultas.MeDespachoProdgenSql)
        {
        }

        public MeDespachoProdgenDTO Create(IDataReader dr)
        {
            MeDespachoProdgenDTO entity = new MeDespachoProdgenDTO();

            int iDpgencodi = dr.GetOrdinal(this.Dpgencodi);
            if (!dr.IsDBNull(iDpgencodi)) entity.Dpgencodi = Convert.ToInt32(dr.GetValue(iDpgencodi));

            int iDpgenfecha = dr.GetOrdinal(this.Dpgenfecha);
            if (!dr.IsDBNull(iDpgenfecha)) entity.Dpgenfecha = dr.GetDateTime(iDpgenfecha);

            int iDpgentipo = dr.GetOrdinal(this.Dpgentipo);
            if (!dr.IsDBNull(iDpgentipo)) entity.Dpgentipo = Convert.ToInt32(dr.GetValue(iDpgentipo));

            int iDpgenvalor = dr.GetOrdinal(this.Dpgenvalor);
            if (!dr.IsDBNull(iDpgenvalor)) entity.Dpgenvalor = dr.GetDecimal(iDpgenvalor);

            int iDpgenintegrante = dr.GetOrdinal(this.Dpgenintegrante);
            if (!dr.IsDBNull(iDpgenintegrante)) entity.Dpgenintegrante = dr.GetString(iDpgenintegrante);

            int iDpgenrer = dr.GetOrdinal(this.Dpgenrer);
            if (!dr.IsDBNull(iDpgenrer)) entity.Dpgenrer = dr.GetString(iDpgenrer);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquipadre = dr.GetOrdinal(this.Equipadre);
            if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iTgenercodi = dr.GetOrdinal(this.Tgenercodi);
            if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt32(dr.GetValue(iTgenercodi));

            int iFenergcodi = dr.GetOrdinal(this.Fenergcodi);
            if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

            int iCtgdetcodi = dr.GetOrdinal(this.Ctgdetcodi);
            if (!dr.IsDBNull(iCtgdetcodi)) entity.Ctgdetcodi = Convert.ToInt32(dr.GetValue(iCtgdetcodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Dpgencodi = "DPGENCODI";
        public string Dpgenfecha = "DPGENFECHA";
        public string Dpgentipo = "DPGENTIPO";
        public string Dpgenvalor = "DPGENVALOR";
        public string Dpgenintegrante = "DPGENINTEGRANTE";
        public string Dpgenrer = "DPGENRER";
        public string Emprcodi = "EMPRCODI";
        public string Equipadre = "EQUIPADRE";
        public string Grupocodi = "GRUPOCODI";
        public string Tgenercodi = "TGENERCODI";
        public string Fenergcodi = "FENERGCODI";
        public string Ctgdetcodi = "CTGDETCODI";

        public string Tgenernomb = "TGENERNOMB";
        public string Emprnomb = "EMPRNOMB";
        public string Gruponomb = "GRUPONOMB";
        public string Fenergnomb = "FENERGNOMB";
        public string Central = "CENTRAL";

        #endregion
    }
}
