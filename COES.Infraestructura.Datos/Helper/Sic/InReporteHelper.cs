using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IN_REPORTE
    /// </summary>
    public class InReporteHelper : HelperBase
    {
        public InReporteHelper() : base(Consultas.InReporteSql)
        {
        }

        public InReporteDTO Create(IDataReader dr)
        {
            InReporteDTO entity = new InReporteDTO();

            int iInrepcodi = dr.GetOrdinal(this.Inrepcodi);
            if (!dr.IsDBNull(iInrepcodi)) entity.Inrepcodi = Convert.ToInt32(dr.GetValue(iInrepcodi));

            int iInrepnombre = dr.GetOrdinal(this.Inrepnombre);
            if (!dr.IsDBNull(iInrepnombre)) entity.Inrepnombre = dr.GetString(iInrepnombre);

            int iInrephorizonte = dr.GetOrdinal(this.Inrephorizonte);
            if (!dr.IsDBNull(iInrephorizonte)) entity.Inrephorizonte = Convert.ToInt32(dr.GetValue(iInrephorizonte));

            int iInreptipo = dr.GetOrdinal(this.Inreptipo);
            if (!dr.IsDBNull(iInreptipo)) entity.Inreptipo = Convert.ToInt32(dr.GetValue(iInreptipo));

            int iInrepusucreacion = dr.GetOrdinal(this.Inrepusucreacion);
            if (!dr.IsDBNull(iInrepusucreacion)) entity.Inrepusucreacion = dr.GetString(iInrepusucreacion);

            int iInrepfeccreacion = dr.GetOrdinal(this.Inrepfeccreacion);
            if (!dr.IsDBNull(iInrepfeccreacion)) entity.Inrepfeccreacion = dr.GetDateTime(iInrepfeccreacion);

            int iInrepusumodificacion = dr.GetOrdinal(this.Inrepusumodificacion);
            if (!dr.IsDBNull(iInrepusumodificacion)) entity.Inrepusumodificacion = dr.GetString(iInrepusumodificacion);

            int iInrepfecmodificacion = dr.GetOrdinal(this.Inrepfecmodificacion);
            if (!dr.IsDBNull(iInrepfecmodificacion)) entity.Inrepfecmodificacion = dr.GetDateTime(iInrepfecmodificacion);

            int iProgrcodi = dr.GetOrdinal(this.Progrcodi);
            if (!dr.IsDBNull(iProgrcodi)) entity.Progrcodi = Convert.ToInt32(dr.GetValue(iProgrcodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Inrepcodi = "INREPCODI";
        public string Inrepnombre = "INREPNOMBRE";
        public string Inrephorizonte = "INREPHORIZONTE";
        public string Inreptipo = "INREPTIPO";
        public string Inrepusucreacion = "INREPUSUCREACION";
        public string Inrepfeccreacion = "INREPFECCREACION";
        public string Inrepusumodificacion = "INREPUSUMODIFICACION";
        public string Inrepfecmodificacion = "INREPFECMODIFICACION";
        public string Progrcodi = "PROGRCODI";

        #endregion

        public string SqlObtenerReportePorTipo
        {
            get { return base.GetSqlXml("ObtenerReportePorTipo"); }
        }
    }
}
