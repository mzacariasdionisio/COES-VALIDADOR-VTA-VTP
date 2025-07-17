using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CB_REPORTE
    /// </summary>
    public class CbReporteHelper : HelperBase
    {
        public CbReporteHelper(): base(Consultas.CbReporteSql)
        {
        }

        public CbReporteDTO Create(IDataReader dr)
        {
            CbReporteDTO entity = new CbReporteDTO();

            int iCbrepcodi = dr.GetOrdinal(this.Cbrepcodi);
            if (!dr.IsDBNull(iCbrepcodi)) entity.Cbrepcodi = Convert.ToInt32(dr.GetValue(iCbrepcodi));

            int iCbrepmesvigencia = dr.GetOrdinal(this.Cbrepmesvigencia);
            if (!dr.IsDBNull(iCbrepmesvigencia)) entity.Cbrepmesvigencia = dr.GetDateTime(iCbrepmesvigencia);

            int iCbrepversion = dr.GetOrdinal(this.Cbrepversion);
            if (!dr.IsDBNull(iCbrepversion)) entity.Cbrepversion = Convert.ToInt32(dr.GetValue(iCbrepversion));

            int iCbrepnombre = dr.GetOrdinal(this.Cbrepnombre);
            if (!dr.IsDBNull(iCbrepnombre)) entity.Cbrepnombre = dr.GetString(iCbrepnombre);

            int iCbrepusucreacion = dr.GetOrdinal(this.Cbrepusucreacion);
            if (!dr.IsDBNull(iCbrepusucreacion)) entity.Cbrepusucreacion = dr.GetString(iCbrepusucreacion);

            int iCbrepfeccreacion = dr.GetOrdinal(this.Cbrepfeccreacion);
            if (!dr.IsDBNull(iCbrepfeccreacion)) entity.Cbrepfeccreacion = dr.GetDateTime(iCbrepfeccreacion);

            int iCbrepusumodificacion = dr.GetOrdinal(this.Cbrepusumodificacion);
            if (!dr.IsDBNull(iCbrepusumodificacion)) entity.Cbrepusumodificacion = dr.GetString(iCbrepusumodificacion);

            int iCbrepfecmodificacion = dr.GetOrdinal(this.Cbrepfecmodificacion);
            if (!dr.IsDBNull(iCbrepfecmodificacion)) entity.Cbrepfecmodificacion = dr.GetDateTime(iCbrepfecmodificacion);

            int iCbreptipo = dr.GetOrdinal(this.Cbreptipo);
            if (!dr.IsDBNull(iCbreptipo)) entity.Cbreptipo = Convert.ToInt32(dr.GetValue(iCbreptipo));

            return entity;
        }


        #region Mapeo de Campos

        public string Cbrepcodi = "CBREPCODI";
        public string Cbrepmesvigencia = "CBREPMESVIGENCIA";
        public string Cbrepversion = "CBREPVERSION";
        public string Cbrepnombre = "CBREPNOMBRE";
        public string Cbrepusucreacion = "CBREPUSUCREACION";
        public string Cbrepfeccreacion = "CBREPFECCREACION";
        public string Cbrepusumodificacion = "CBREPUSUMODIFICACION";
        public string Cbrepfecmodificacion = "CBREPFECMODIFICACION";
        public string Cbreptipo = "CBREPTIPO";

        #endregion

        public string SqlGetByTipoYMesVigencia
        {
            get { return base.GetSqlXml("GetByTipoYMesVigencia"); }
        }
    }
}
