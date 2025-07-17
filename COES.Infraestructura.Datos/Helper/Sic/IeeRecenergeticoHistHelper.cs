using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IEE_RECENERGETICO_HIST
    /// </summary>
    public class IeeRecenergeticoHistHelper : HelperBase
    {
        public IeeRecenergeticoHistHelper() : base(Consultas.IeeRecenergeticoHistSql)
        {
        }

        public IeeRecenergeticoHistDTO Create(IDataReader dr)
        {
            IeeRecenergeticoHistDTO entity = new IeeRecenergeticoHistDTO();

            int iRenercodi = dr.GetOrdinal(this.Renerhcodi);
            if (!dr.IsDBNull(iRenercodi)) entity.Renerhcodi = Convert.ToInt32(dr.GetValue(iRenercodi));

            int iRenerfecha = dr.GetOrdinal(this.Renerhfecha);
            if (!dr.IsDBNull(iRenerfecha)) entity.Renerhfecha = dr.GetDateTime(iRenerfecha);

            int iRenervalor = dr.GetOrdinal(this.Renerhvalor);
            if (!dr.IsDBNull(iRenervalor)) entity.Renerhvalor = Convert.ToDecimal(dr.GetValue(iRenervalor));

            int iRenertipcodi = dr.GetOrdinal(this.Renertipcodi);
            if (!dr.IsDBNull(iRenertipcodi)) entity.Renertipcodi = Convert.ToInt32(dr.GetValue(iRenertipcodi));

            int iRenerusucreacion = dr.GetOrdinal(this.Renerhusucreacion);
            if (!dr.IsDBNull(iRenerusucreacion)) entity.Renerhusucreacion = dr.GetString(iRenerusucreacion);

            int iRenerfeccreacion = dr.GetOrdinal(this.Renerhfeccreacion);
            if (!dr.IsDBNull(iRenerfeccreacion)) entity.Renerhfeccreacion = dr.GetDateTime(iRenerfeccreacion);

            int iRenerusumodificacion = dr.GetOrdinal(this.Renerhusumodificacion);
            if (!dr.IsDBNull(iRenerusumodificacion)) entity.Renerhusumodificacion = dr.GetString(iRenerusumodificacion);

            int iRenerfecmodificacion = dr.GetOrdinal(this.Renerhfecmodificacion);
            if (!dr.IsDBNull(iRenerfecmodificacion)) entity.Renerhfecmodificacion = dr.GetDateTime(iRenerfecmodificacion);


            return entity;
        }


        #region Mapeo de Campos

        public string Renerhcodi = "RENERHCODI";
        public string Renerhfecha = "RENERHFECHA";
        public string Renerhvalor = "RENERHVALOR";
        public string Renertipcodi = "RENERTIPCODI";
        public string Renerhusucreacion = "RENERHUSUCREACION";
        public string Renerhfeccreacion = "RENERHFECCREACION";
        public string Renerhusumodificacion = "RENERHUSUMODIFICACION";
        public string Renerhfecmodificacion = "RENERHFECMODIFICACION";

        public string Renertipnomb = "RENERTIPNOMB";

        #endregion
    }
}
