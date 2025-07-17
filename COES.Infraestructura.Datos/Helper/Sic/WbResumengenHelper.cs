using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_RESUMENGEN
    /// </summary>
    public class WbResumengenHelper : HelperBase
    {
        public WbResumengenHelper(): base(Consultas.WbResumengenSql)
        {
        }

        public WbResumengenDTO Create(IDataReader dr)
        {
            WbResumengenDTO entity = new WbResumengenDTO();

            int iResgencodi = dr.GetOrdinal(this.Resgencodi);
            if (!dr.IsDBNull(iResgencodi)) entity.Resgencodi = Convert.ToInt32(dr.GetValue(iResgencodi));

            int iResgenactual = dr.GetOrdinal(this.Resgenactual);
            if (!dr.IsDBNull(iResgenactual)) entity.Resgenactual = dr.GetDecimal(iResgenactual);

            int iResgenanterior = dr.GetOrdinal(this.Resgenanterior);
            if (!dr.IsDBNull(iResgenanterior)) entity.Resgenanterior = dr.GetDecimal(iResgenanterior);

            int iResgenvariacion = dr.GetOrdinal(this.Resgenvariacion);
            if (!dr.IsDBNull(iResgenvariacion)) entity.Resgenvariacion = dr.GetDecimal(iResgenvariacion);

            int iResgenfecha = dr.GetOrdinal(this.Resgenfecha);
            if (!dr.IsDBNull(iResgenfecha)) entity.Resgenfecha = dr.GetDateTime(iResgenfecha);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            return entity;
        }


        #region Mapeo de Campos

        public string Resgencodi = "RESGENCODI";
        public string Resgenactual = "RESGENACTUAL";
        public string Resgenanterior = "RESGENANTERIOR";
        public string Resgenvariacion = "RESGENVARIACION";
        public string Resgenfecha = "RESGENFECHA";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";

        #endregion
    }
}
