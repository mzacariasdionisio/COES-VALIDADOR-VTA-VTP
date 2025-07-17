using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PFR_ESCENARIO
    /// </summary>
    public class PfrEscenarioHelper : HelperBase
    {
        public PfrEscenarioHelper(): base(Consultas.PfrEscenarioSql)
        {
        }

        public PfrEscenarioDTO Create(IDataReader dr)
        {
            PfrEscenarioDTO entity = new PfrEscenarioDTO();

            int iPfresccodi = dr.GetOrdinal(this.Pfresccodi);
            if (!dr.IsDBNull(iPfresccodi)) entity.Pfresccodi = Convert.ToInt32(dr.GetValue(iPfresccodi));

            int iPfrrptcodi = dr.GetOrdinal(this.Pfrrptcodi);
            if (!dr.IsDBNull(iPfrrptcodi)) entity.Pfrrptcodi = Convert.ToInt32(dr.GetValue(iPfrrptcodi));

            int iPfrescdescripcion = dr.GetOrdinal(this.Pfrescdescripcion);
            if (!dr.IsDBNull(iPfrescdescripcion)) entity.Pfrescdescripcion = dr.GetString(iPfrescdescripcion);

            int iPfrescfecini = dr.GetOrdinal(this.Pfrescfecini);
            if (!dr.IsDBNull(iPfrescfecini)) entity.Pfrescfecini = dr.GetDateTime(iPfrescfecini);

            int iPfrescfecfin = dr.GetOrdinal(this.Pfrescfecfin);
            if (!dr.IsDBNull(iPfrescfecfin)) entity.Pfrescfecfin = dr.GetDateTime(iPfrescfecfin);

            int iPfrescfrf = dr.GetOrdinal(this.Pfrescfrf);
            if (!dr.IsDBNull(iPfrescfrf)) entity.Pfrescfrf = dr.GetDecimal(iPfrescfrf);

            int iPfrescfrfr = dr.GetOrdinal(this.Pfrescfrfr);
            if (!dr.IsDBNull(iPfrescfrfr)) entity.Pfrescfrfr = dr.GetDecimal(iPfrescfrfr);

            int iPfrescpfct = dr.GetOrdinal(this.Pfrescpfct);
            if (!dr.IsDBNull(iPfrescpfct)) entity.Pfrescpfct = dr.GetDecimal(iPfrescpfct);

            return entity;
        }


        #region Mapeo de Campos

        public string Pfresccodi = "PFRESCCODI";
        public string Pfrrptcodi = "PFRRPTCODI";
        public string Pfrescdescripcion = "PFRESCDESCRIPCION";
        public string Pfrescfecini = "PFRESCFECINI";
        public string Pfrescfecfin = "PFRESCFECFIN";
        public string Pfrescfrf = "PFRESCFRF";
        public string Pfrescfrfr = "PFRESCFRFR";
        public string Pfrescpfct = "PFRESCPFCT";

        #endregion
        
        public string SqlListByReportecodi
        {
            get { return base.GetSqlXml("ListByReportecodi"); }
        }
    }
}
