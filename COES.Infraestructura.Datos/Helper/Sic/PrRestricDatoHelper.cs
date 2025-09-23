using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_RESTRIC_DATO
    /// </summary>
    public class PrRestricDatoHelper : HelperBase
    {
        public PrRestricDatoHelper(): base(Consultas.PrRestricDatoSql)
        {
        }

        public PrRestricDatoDTO Create(IDataReader dr)
        {
            PrRestricDatoDTO entity = new PrRestricDatoDTO();

            int iRescfgcodi = dr.GetOrdinal(this.Rescfgcodi);
            if (!dr.IsDBNull(iRescfgcodi)) entity.Rescfgcodi = Convert.ToInt32(dr.GetValue(iRescfgcodi));

            int iResdatcodi = dr.GetOrdinal(this.Resdatcodi);
            if (!dr.IsDBNull(iResdatcodi)) entity.Resdatcodi = Convert.ToInt32(dr.GetValue(iResdatcodi));

            int iResdatdato = dr.GetOrdinal(this.Resdatdato);
            if (!dr.IsDBNull(iResdatdato)) entity.Resdatdato = dr.GetString(iResdatdato);

            int iResdatflag = dr.GetOrdinal(this.Resdatflag);
            if (!dr.IsDBNull(iResdatflag)) entity.Resdatflag = dr.GetString(iResdatflag);

            int iResdatfechaini = dr.GetOrdinal(this.Resdatfechaini);
            if (!dr.IsDBNull(iResdatfechaini)) entity.Resdatfechaini = dr.GetDateTime(iResdatfechaini);

            int iResenvcodi = dr.GetOrdinal(this.Resenvcodi);
            if (!dr.IsDBNull(iResenvcodi)) entity.Resenvcodi = Convert.ToInt32(dr.GetValue(iResenvcodi));

            int iResdatfechafin = dr.GetOrdinal(this.Resdatfechafin);
            if (!dr.IsDBNull(iResdatfechafin)) entity.Resdatfechafin = dr.GetDateTime(iResdatfechafin);

            return entity;
        }


        #region Mapeo de Campos

        public string Rescfgcodi = "RESCFGCODI";
        public string Resdatcodi = "RESDATCODI";
        public string Resdatdato = "RESDATDATO";
        public string Resdatflag = "RESDATFLAG";
        public string Resdatfechaini = "RESDATFECHAINI";
        public string Resenvcodi = "RESENVCODI";
        public string Resdatfechafin = "RESDATFECHAFIN";

        #endregion

        public string SqlObtenerDataEnvio
        {
            get { return base.GetSqlXml("ObtenerDataEnvio"); }
        }
        public string SqlObtenerPorEnvio
        {
            get { return base.GetSqlXml("ObtenerPorEnvio"); }
        }
    }
}
