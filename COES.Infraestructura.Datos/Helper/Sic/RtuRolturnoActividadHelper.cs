using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RTU_ROLTURNO_ACTIVIDAD
    /// </summary>
    public class RtuRolturnoActividadHelper : HelperBase
    {
        public RtuRolturnoActividadHelper() : base(Consultas.RtuRolturnoActividadSql)
        {
        }

        public RtuRolturnoActividadDTO Create(IDataReader dr)
        {
            RtuRolturnoActividadDTO entity = new RtuRolturnoActividadDTO();

            int iRtudetcodi = dr.GetOrdinal(this.Rtudetcodi);
            if (!dr.IsDBNull(iRtudetcodi)) entity.Rtudetcodi = Convert.ToInt32(dr.GetValue(iRtudetcodi));

            int iRtuactcodi = dr.GetOrdinal(this.Rtuactcodi);
            if (!dr.IsDBNull(iRtuactcodi)) entity.Rtuactcodi = Convert.ToInt32(dr.GetValue(iRtuactcodi));

            int iRturaccodi = dr.GetOrdinal(this.Rturaccodi);
            if (!dr.IsDBNull(iRturaccodi)) entity.Rturaccodi = Convert.ToInt32(dr.GetValue(iRturaccodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Rtudetcodi = "RTUDETCODI";
        public string Rtuactcodi = "RTUACTCODI";
        public string Rturaccodi = "RTURACCODI";

        #endregion
    }
}
