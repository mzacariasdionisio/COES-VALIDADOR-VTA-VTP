using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla C_POINTS
    /// </summary>
    public class CPointsHelper : HelperBase
    {
        public CPointsHelper(): base(Consultas.CPointsSql)
        {
        }

        public CPointsDTO Create(IDataReader dr)
        {
            CPointsDTO entity = new CPointsDTO();

            int iPointNumber = dr.GetOrdinal(this.PointNumber);
            if (!dr.IsDBNull(iPointNumber)) entity.PointNumber = Convert.ToInt32(dr.GetValue(iPointNumber));

            int iPointName = dr.GetOrdinal(this.PointName);
            if (!dr.IsDBNull(iPointName)) entity.PointName = dr.GetString(iPointName);

            int iPointType = dr.GetOrdinal(this.PointType);
            if (!dr.IsDBNull(iPointType)) entity.PointType = dr.GetString(iPointType);

            int iActive = dr.GetOrdinal(this.Active);
            if (!dr.IsDBNull(iActive)) entity.Active = dr.GetString(iActive);

            int iAorId = dr.GetOrdinal(this.AorId);
            if (!dr.IsDBNull(iAorId)) entity.AorId = Convert.ToInt32(dr.GetValue(iAorId));

            return entity;
        }


        #region Mapeo de Campos

        public string PointNumber = "POINT_NUMBER";
        public string PointName = "POINT_NAME";
        public string PointType = "POINT_TYPE";
        public string Active = "ACTIVE";
        public string AorId = "AOR_ID";

        #endregion
    }
}
