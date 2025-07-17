using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PF_ESCENARIO
    /// </summary>
    public class PfEscenarioHelper : HelperBase
    {
        public PfEscenarioHelper(): base(Consultas.PfEscenarioSql)
        {
        }

        public PfEscenarioDTO Create(IDataReader dr)
        {
            PfEscenarioDTO entity = new PfEscenarioDTO();

            int iPfescecodi = dr.GetOrdinal(this.Pfescecodi);
            if (!dr.IsDBNull(iPfescecodi)) entity.Pfescecodi = Convert.ToInt32(dr.GetValue(iPfescecodi));

            int iPfescefecini = dr.GetOrdinal(this.Pfescefecini);
            if (!dr.IsDBNull(iPfescefecini)) entity.Pfescefecini = dr.GetDateTime(iPfescefecini);

            int iPfescefecfin = dr.GetOrdinal(this.Pfescefecfin);
            if (!dr.IsDBNull(iPfescefecfin)) entity.Pfescefecfin = dr.GetDateTime(iPfescefecfin);

            int iPfescedescripcion = dr.GetOrdinal(this.Pfescedescripcion);
            if (!dr.IsDBNull(iPfescedescripcion)) entity.Pfescedescripcion = dr.GetString(iPfescedescripcion);

            int iPfrptcodi = dr.GetOrdinal(this.Pfrptcodi);
            if (!dr.IsDBNull(iPfrptcodi)) entity.Pfrptcodi = Convert.ToInt32(dr.GetValue(iPfrptcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Pfescecodi = "PFESCECODI";
        public string Pfescefecini = "PFESCEFECINI";
        public string Pfescefecfin = "PFESCEFECFIN";
        public string Pfescedescripcion = "PFESCEDESCRIPCION";
        public string Pfrptcodi = "PFRPTCODI";

        #endregion
    }
}
