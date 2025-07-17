using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RE_NIVEL_TENSION
    /// </summary>
    public class ReNivelTensionHelper : HelperBase
    {
        public ReNivelTensionHelper(): base(Consultas.ReNivelTensionSql)
        {
        }

        public ReNivelTensionDTO Create(IDataReader dr)
        {
            ReNivelTensionDTO entity = new ReNivelTensionDTO();

            int iRentcodi = dr.GetOrdinal(this.Rentcodi);
            if (!dr.IsDBNull(iRentcodi)) entity.Rentcodi = Convert.ToInt32(dr.GetValue(iRentcodi));

            int iRentabrev = dr.GetOrdinal(this.Rentabrev);
            if (!dr.IsDBNull(iRentabrev)) entity.Rentabrev = dr.GetString(iRentabrev);

            int iRentnombre = dr.GetOrdinal(this.Rentnombre);
            if (!dr.IsDBNull(iRentnombre)) entity.Rentnombre = dr.GetString(iRentnombre);

            int iRentusucreacion = dr.GetOrdinal(this.Rentusucreacion);
            if (!dr.IsDBNull(iRentusucreacion)) entity.Rentusucreacion = dr.GetString(iRentusucreacion);

            int iRentfeccreacion = dr.GetOrdinal(this.Rentfeccreacion);
            if (!dr.IsDBNull(iRentfeccreacion)) entity.Rentfeccreacion = dr.GetDateTime(iRentfeccreacion);

            int iRentusumodificacion = dr.GetOrdinal(this.Rentusumodificacion);
            if (!dr.IsDBNull(iRentusumodificacion)) entity.Rentusumodificacion = dr.GetString(iRentusumodificacion);

            int iRentfecmodificacion = dr.GetOrdinal(this.Rentfecmodificacion);
            if (!dr.IsDBNull(iRentfecmodificacion)) entity.Rentfecmodificacion = dr.GetDateTime(iRentfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Rentcodi = "RENTCODI";
        public string Rentabrev = "RENTABREV";
        public string Rentnombre = "RENTNOMBRE";
        public string Rentusucreacion = "RENTUSUCREACION";
        public string Rentfeccreacion = "RENTFECCREACION";
        public string Rentusumodificacion = "RENTUSUMODIFICACION";
        public string Rentfecmodificacion = "RENTFECMODIFICACION";

        #endregion
    }
}
