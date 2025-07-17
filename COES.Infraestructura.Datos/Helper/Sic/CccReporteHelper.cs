using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CCC_REPORTE
    /// </summary>
    public class CccReporteHelper : HelperBase
    {
        public CccReporteHelper() : base(Consultas.CccReporteSql)
        {
        }

        public CccReporteDTO Create(IDataReader dr)
        {
            CccReporteDTO entity = new CccReporteDTO();

            int iCccrptcodi = dr.GetOrdinal(this.Cccrptcodi);
            if (!dr.IsDBNull(iCccrptcodi)) entity.Cccrptcodi = Convert.ToInt32(dr.GetValue(iCccrptcodi));

            int iCccvercodi = dr.GetOrdinal(this.Cccvercodi);
            if (!dr.IsDBNull(iCccvercodi)) entity.Cccvercodi = Convert.ToInt32(dr.GetValue(iCccvercodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquipadre = dr.GetOrdinal(this.Equipadre);
            if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iMogrupocodi = dr.GetOrdinal(this.Mogrupocodi);
            if (!dr.IsDBNull(iMogrupocodi)) entity.Mogrupocodi = Convert.ToInt32(dr.GetValue(iMogrupocodi));

            int iCccrptvalorreal = dr.GetOrdinal(this.Cccrptvalorreal);
            if (!dr.IsDBNull(iCccrptvalorreal)) entity.Cccrptvalorreal = dr.GetDecimal(iCccrptvalorreal);

            int iCccrptvalorteorico = dr.GetOrdinal(this.Cccrptvalorteorico);
            if (!dr.IsDBNull(iCccrptvalorteorico)) entity.Cccrptvalorteorico = dr.GetDecimal(iCccrptvalorteorico);

            int iCccrptvariacion = dr.GetOrdinal(this.Cccrptvariacion);
            if (!dr.IsDBNull(iCccrptvariacion)) entity.Cccrptvariacion = dr.GetDecimal(iCccrptvariacion);

            int iCccrptflagtienecurva = dr.GetOrdinal(this.Cccrptflagtienecurva);
            if (!dr.IsDBNull(iCccrptflagtienecurva)) entity.Cccrptflagtienecurva = Convert.ToInt32(dr.GetValue(iCccrptflagtienecurva));

            int iFenergcodi = dr.GetOrdinal(this.Fenergcodi);
            if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

            int iTipoinfocodi = dr.GetOrdinal(this.Tipoinfocodi);
            if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Cccrptcodi = "CCCRPTCODI";
        public string Cccvercodi = "CCCVERCODI";
        public string Emprcodi = "EMPRCODI";
        public string Equipadre = "EQUIPADRE";
        public string Equicodi = "EQUICODI";
        public string Grupocodi = "GRUPOCODI";
        public string Cccrptvalorreal = "CCCRPTVALORREAL";
        public string Cccrptvalorteorico = "CCCRPTVALORTEORICO";
        public string Cccrptvariacion = "CCCRPTVARIACION";
        public string Cccrptflagtienecurva = "CCCRPTFLAGTIENECURVA";
        public string Fenergcodi = "FENERGCODI";
        public string Tipoinfocodi = "TIPOINFOCODI";
        public string Mogrupocodi = "MOGRUPOCODI";

        public string Cccverfecha = "CCCVERFECHA";
        public string Emprabrev = "EMPRABREV";
        public string Emprnomb = "EMPRNOMB";
        public string Central = "CENTRAL";
        public string Equinomb = "EQUINOMB";
        public string Fenergnomb = "Fenergnomb";
        public string Tipoinfoabrev = "Tipoinfoabrev";
        public string Mogruponomb = "Mogruponomb";

        #endregion

    }
}
