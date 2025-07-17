using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SEG_REGION
    /// </summary>
    public class SegRegionHelper : HelperBase
    {
        public SegRegionHelper(): base(Consultas.SegRegionSql)
        {
        }

        public SegRegionDTO Create(IDataReader dr)
        {
            SegRegionDTO entity = new SegRegionDTO();

            int iRegnombre = dr.GetOrdinal(this.Regnombre);
            if (!dr.IsDBNull(iRegnombre)) entity.Regnombre = dr.GetString(iRegnombre);

            int iRegusumodificacion = dr.GetOrdinal(this.Regusumodificacion);
            if (!dr.IsDBNull(iRegusumodificacion)) entity.Regusumodificacion = dr.GetString(iRegusumodificacion);

            int iRegfecmodificacion = dr.GetOrdinal(this.Regfecmodificacion);
            if (!dr.IsDBNull(iRegfecmodificacion)) entity.Regfecmodificacion = dr.GetDateTime(iRegfecmodificacion);

            int iRegestado = dr.GetOrdinal(this.Regestado);
            if (!dr.IsDBNull(iRegestado)) entity.Regestado = dr.GetString(iRegestado);

            int iRegcodi = dr.GetOrdinal(this.Regcodi);
            if (!dr.IsDBNull(iRegcodi)) entity.Regcodi = Convert.ToInt32(dr.GetValue(iRegcodi));

            int iRegusucreacion = dr.GetOrdinal(this.Regusucreacion);
            if (!dr.IsDBNull(iRegusucreacion)) entity.Regusucreacion = dr.GetString(iRegusucreacion);

            int iRegfeccreacion = dr.GetOrdinal(this.Regfeccreacion);
            if (!dr.IsDBNull(iRegfeccreacion)) entity.Regfeccreacion = dr.GetDateTime(iRegfeccreacion);


            return entity;
        }


        #region Mapeo de Campos

        public string Regnombre = "REGNOMBRE";
        public string Regusumodificacion = "REGUSUMODIFICACION";
        public string Regfecmodificacion = "REGFECMODIFICACION";
        public string Regestado = "REGESTADO";
        public string Regcodi = "REGCODI";
        public string Regusucreacion = "REGUSUCREACION";
        public string Regfeccreacion = "REGFECCREACION";

        #endregion

        public string SqlActualizarCongestion
        {
            get { return GetSqlXml("ActualizarCongestion"); }
        }
    }
}
