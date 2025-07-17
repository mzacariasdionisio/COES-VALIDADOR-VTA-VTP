using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PF_PERIODO
    /// </summary>
    public class PfPeriodoHelper : HelperBase
    {
        public PfPeriodoHelper(): base(Consultas.PfPeriodoSql)
        {
        }

        public PfPeriodoDTO Create(IDataReader dr)
        {
            PfPeriodoDTO entity = new PfPeriodoDTO();

            int iPfpericodi = dr.GetOrdinal(this.Pfpericodi);
            if (!dr.IsDBNull(iPfpericodi)) entity.Pfpericodi = Convert.ToInt32(dr.GetValue(iPfpericodi));

            int iPfperinombre = dr.GetOrdinal(this.Pfperinombre);
            if (!dr.IsDBNull(iPfperinombre)) entity.Pfperinombre = dr.GetString(iPfperinombre);

            int iPfperianio = dr.GetOrdinal(this.Pfperianio);
            if (!dr.IsDBNull(iPfperianio)) entity.Pfperianio = Convert.ToInt32(dr.GetValue(iPfperianio));

            int iPfperimes = dr.GetOrdinal(this.Pfperimes);
            if (!dr.IsDBNull(iPfperimes)) entity.Pfperimes = Convert.ToInt32(dr.GetValue(iPfperimes));

            int iPfperianiomes = dr.GetOrdinal(this.Pfperianiomes);
            if (!dr.IsDBNull(iPfperianiomes)) entity.Pfperianiomes = Convert.ToInt32(dr.GetValue(iPfperianiomes));

            int iPfperiusucreacion = dr.GetOrdinal(this.Pfperiusucreacion);
            if (!dr.IsDBNull(iPfperiusucreacion)) entity.Pfperiusucreacion = dr.GetString(iPfperiusucreacion);

            int iPfperifeccreacion = dr.GetOrdinal(this.Pfperifeccreacion);
            if (!dr.IsDBNull(iPfperifeccreacion)) entity.Pfperifeccreacion = dr.GetDateTime(iPfperifeccreacion);

            int iPfperiusumodificacion = dr.GetOrdinal(this.Pfperiusumodificacion);
            if (!dr.IsDBNull(iPfperiusumodificacion)) entity.Pfperiusumodificacion = dr.GetString(iPfperiusumodificacion);

            int iPfperifecmodificacion = dr.GetOrdinal(this.Pfperifecmodificacion);
            if (!dr.IsDBNull(iPfperifecmodificacion)) entity.Pfperifecmodificacion = dr.GetDateTime(iPfperifecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Pfpericodi = "PFPERICODI";
        public string Pfperinombre = "PFPERINOMBRE";
        public string Pfperianio = "PFPERIANIO";
        public string Pfperimes = "PFPERIMES";
        public string Pfperianiomes = "PFPERIANIOMES";
        public string Pfperiusucreacion = "PFPERIUSUCREACION";
        public string Pfperifeccreacion = "PFPERIFECCREACION";
        public string Pfperiusumodificacion = "PFPERIUSUMODIFICACION";
        public string Pfperifecmodificacion = "PFPERIFECMODIFICACION";

        #endregion
    }
}
