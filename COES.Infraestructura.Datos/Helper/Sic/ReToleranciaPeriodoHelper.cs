using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RE_TOLERANCIA_PERIODO
    /// </summary>
    public class ReToleranciaPeriodoHelper : HelperBase
    {
        public ReToleranciaPeriodoHelper(): base(Consultas.ReToleranciaPeriodoSql)
        {
        }

        public ReToleranciaPeriodoDTO Create(IDataReader dr)
        {
            ReToleranciaPeriodoDTO entity = new ReToleranciaPeriodoDTO();

            int iRetolninf = dr.GetOrdinal(this.Retolninf);
            if (!dr.IsDBNull(iRetolninf)) entity.Retolninf = Convert.ToInt32(dr.GetValue(iRetolninf));

            int iRetoldinf = dr.GetOrdinal(this.Retoldinf);
            if (!dr.IsDBNull(iRetoldinf)) entity.Retoldinf = Convert.ToInt32(dr.GetValue(iRetoldinf));

            int iRetolnsup = dr.GetOrdinal(this.Retolnsup);
            if (!dr.IsDBNull(iRetolnsup)) entity.Retolnsup = Convert.ToInt32(dr.GetValue(iRetolnsup));

            int iRetoldsup = dr.GetOrdinal(this.Retoldsup);
            if (!dr.IsDBNull(iRetoldsup)) entity.Retoldsup = Convert.ToInt32(dr.GetValue(iRetoldsup));

            int iRetolusucreacion = dr.GetOrdinal(this.Retolusucreacion);
            if (!dr.IsDBNull(iRetolusucreacion)) entity.Retolusucreacion = dr.GetString(iRetolusucreacion);

            int iRetolfeccreacion = dr.GetOrdinal(this.Retolfeccreacion);
            if (!dr.IsDBNull(iRetolfeccreacion)) entity.Retolfeccreacion = dr.GetDateTime(iRetolfeccreacion);

            int iRetolusumodificacion = dr.GetOrdinal(this.Retolusumodificacion);
            if (!dr.IsDBNull(iRetolusumodificacion)) entity.Retolusumodificacion = dr.GetString(iRetolusumodificacion);

            int iRetolfecmodificacion = dr.GetOrdinal(this.Retolfecmodificacion);
            if (!dr.IsDBNull(iRetolfecmodificacion)) entity.Retolfecmodificacion = dr.GetDateTime(iRetolfecmodificacion);

            int iRetolcodi = dr.GetOrdinal(this.Retolcodi);
            if (!dr.IsDBNull(iRetolcodi)) entity.Retolcodi = Convert.ToInt32(dr.GetValue(iRetolcodi));

            int iRepercodi = dr.GetOrdinal(this.Repercodi);
            if (!dr.IsDBNull(iRepercodi)) entity.Repercodi = Convert.ToInt32(dr.GetValue(iRepercodi));

            int iRentcodi = dr.GetOrdinal(this.Rentcodi);
            if (!dr.IsDBNull(iRentcodi)) entity.Rentcodi = Convert.ToInt32(dr.GetValue(iRentcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Retolninf = "RETOLNINF";
        public string Retoldinf = "RETOLDINF";
        public string Retolnsup = "RETOLNSUP";
        public string Retoldsup = "RETOLDSUP";
        public string Retolusucreacion = "RETOLUSUCREACION";
        public string Retolfeccreacion = "RETOLFECCREACION";
        public string Retolusumodificacion = "RETOLUSUMODIFICACION";
        public string Retolfecmodificacion = "RETOLFECMODIFICACION";
        public string Retolcodi = "RETOLCODI";
        public string Repercodi = "REPERCODI";
        public string Rentcodi = "RENTCODI";
        public string Rentabrev = "RENTABREV";

        #endregion

        public string SqlObtenerParaImportar
        {
            get { return base.GetSqlXml("ObtenerParaImportar"); }
        }
    }
}
