using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_PLAZOENVIO
    /// </summary>
    public class SiPlazoenvioHelper : HelperBase
    {
        public SiPlazoenvioHelper()
            : base(Consultas.SiPlazoenvioSql)
        {
        }

        public SiPlazoenvioDTO Create(IDataReader dr)
        {
            SiPlazoenvioDTO entity = new SiPlazoenvioDTO();

            int iPlazcodi = dr.GetOrdinal(this.Plazcodi);
            if (!dr.IsDBNull(iPlazcodi)) entity.Plazcodi = Convert.ToInt32(dr.GetValue(iPlazcodi));

            int iFdatcodi = dr.GetOrdinal(this.Fdatcodi);
            if (!dr.IsDBNull(iFdatcodi)) entity.Fdatcodi = Convert.ToInt32(dr.GetValue(iFdatcodi));

            int iPlazperiodo = dr.GetOrdinal(this.Plazperiodo);
            if (!dr.IsDBNull(iPlazperiodo)) entity.Plazperiodo = Convert.ToInt32(dr.GetValue(iPlazperiodo));

            int iPlazinimin = dr.GetOrdinal(this.Plazinimin);
            if (!dr.IsDBNull(iPlazinimin)) entity.Plazinimin = Convert.ToInt32(dr.GetValue(iPlazinimin));

            int iPlazinidia = dr.GetOrdinal(this.Plazinidia);
            if (!dr.IsDBNull(iPlazinidia)) entity.Plazinidia = Convert.ToInt32(dr.GetValue(iPlazinidia));

            int iPlazfindia = dr.GetOrdinal(this.Plazfindia);
            if (!dr.IsDBNull(iPlazfindia)) entity.Plazfindia = Convert.ToInt32(dr.GetValue(iPlazfindia));

            int iPlazfinmin = dr.GetOrdinal(this.Plazfinmin);
            if (!dr.IsDBNull(iPlazfinmin)) entity.Plazfinmin = Convert.ToInt32(dr.GetValue(iPlazfinmin));

            int iPlazfueradia = dr.GetOrdinal(this.Plazfueradia);
            if (!dr.IsDBNull(iPlazfueradia)) entity.Plazfueradia = Convert.ToInt32(dr.GetValue(iPlazfueradia));

            int iPlazfueramin = dr.GetOrdinal(this.Plazfueramin);
            if (!dr.IsDBNull(iPlazfueramin)) entity.Plazfueramin = Convert.ToInt32(dr.GetValue(iPlazfueramin));

            int iPlazusucreacion = dr.GetOrdinal(this.Plazusucreacion);
            if (!dr.IsDBNull(iPlazusucreacion)) entity.Plazusucreacion = dr.GetString(iPlazusucreacion);

            int iPlazfeccreacion = dr.GetOrdinal(this.Plazfeccreacion);
            if (!dr.IsDBNull(iPlazfeccreacion)) entity.Plazfeccreacion = dr.GetDateTime(iPlazfeccreacion);

            int iPlazusumodificacion = dr.GetOrdinal(this.Plazusumodificacion);
            if (!dr.IsDBNull(iPlazusumodificacion)) entity.Plazusumodificacion = dr.GetString(iPlazusumodificacion);

            int iPlazfecmodificacion = dr.GetOrdinal(this.Plazfecmodificacion);
            if (!dr.IsDBNull(iPlazfecmodificacion)) entity.Plazfecmodificacion = dr.GetDateTime(iPlazfecmodificacion);

            return entity;
        }

        #region Mapeo de Campos

        public string Plazcodi = "PLAZCODI";
        public string Fdatcodi = "FDATCODI";
        public string Plazperiodo = "PLAZPERIODO";
        public string Plazinimin = "PLAZINIMIN";
        public string Plazinidia = "PLAZINIDIA";
        public string Plazfindia = "PLAZFINDIA";
        public string Plazfinmin = "PLAZFINMIN";
        public string Plazfueradia = "PLAZFUERADIA";
        public string Plazfueramin = "PLAZFUERAMIN";
        public string Plazusucreacion = "PLAZUSUCREACION";
        public string Plazfeccreacion = "PLAZFECCREACION";
        public string Plazusumodificacion = "PLAZUSUMODIFICACION";
        public string Plazfecmodificacion = "PLAZFECMODIFICACION";

        public string Fdatnombre = "FDATNOMBRE";

        #endregion

        #region Consultas

        public string SqlGetByFdatcodi
        {
            get { return base.GetSqlXml("GetByFdatcodi"); }
        }

        #endregion
    }
}
