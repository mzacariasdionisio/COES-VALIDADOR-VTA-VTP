using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PF_REPORTE_DET
    /// </summary>
    public class PfReporteDetHelper : HelperBase
    {
        public PfReporteDetHelper() : base(Consultas.PfReporteDetSql)
        {
        }

        public PfReporteDetDTO Create(IDataReader dr)
        {
            PfReporteDetDTO entity = new PfReporteDetDTO();

            int iPfdetcodi = dr.GetOrdinal(this.Pfdetcodi);
            if (!dr.IsDBNull(iPfdetcodi)) entity.Pfdetcodi = Convert.ToInt32(dr.GetValue(iPfdetcodi));

            int iPftotcodi = dr.GetOrdinal(this.Pftotcodi);
            if (!dr.IsDBNull(iPftotcodi)) entity.Pftotcodi = Convert.ToInt32(dr.GetValue(iPftotcodi));

            int iPfdettipo = dr.GetOrdinal(this.Pfdettipo);
            if (!dr.IsDBNull(iPfdettipo)) entity.Pfdettipo = Convert.ToInt32(dr.GetValue(iPfdettipo));

            int iPfdetfechaini = dr.GetOrdinal(this.Pfdetfechaini);
            if (!dr.IsDBNull(iPfdetfechaini)) entity.Pfdetfechaini = dr.GetDateTime(iPfdetfechaini);

            int iPfdetfechafin = dr.GetOrdinal(this.Pfdetfechafin);
            if (!dr.IsDBNull(iPfdetfechafin)) entity.Pfdetfechafin = dr.GetDateTime(iPfdetfechafin);

            int iPfdetenergia = dr.GetOrdinal(this.Pfdetenergia);
            if (!dr.IsDBNull(iPfdetenergia)) entity.Pfdetenergia = dr.GetDecimal(iPfdetenergia);

            int iPfdetnumdiapoc = dr.GetOrdinal(this.Pfdetnumdiapoc);
            if (!dr.IsDBNull(iPfdetnumdiapoc)) entity.Pfdetnumdiapoc = Convert.ToInt32(dr.GetValue(iPfdetnumdiapoc));

            return entity;
        }


        #region Mapeo de Campos

        public string Pfdetcodi = "PFDETCODI";
        public string Pftotcodi = "PFTOTCODI";
        public string Pfdettipo = "PFDETTIPO";
        public string Pfdetfechaini = "PFDETFECHAINI";
        public string Pfdetfechafin = "PFDETFECHAFIN";
        public string Pfdetenergia = "PFDETENERGIA";
        public string Pfdetnumdiapoc = "PFDETNUMDIAPOC";

        public string Equipadre = "EQUIPADRE";

        #endregion
    }
}
