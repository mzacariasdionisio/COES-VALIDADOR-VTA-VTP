using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_MENUREPORTE_TIPO
    /// </summary>
    public class SiMenureporteTipoHelper : HelperBase
    {
        public SiMenureporteTipoHelper(): base(Consultas.SiMenureporteTipoSql)
        {
        }

        public SiMenureporteTipoDTO Create(IDataReader dr)
        {
            SiMenureporteTipoDTO entity = new SiMenureporteTipoDTO();

            int iMreptipcodi = dr.GetOrdinal(this.Mreptipcodi);
            if (!dr.IsDBNull(iMreptipcodi)) entity.Mreptipcodi = Convert.ToInt32(dr.GetValue(iMreptipcodi));

            int iMreptipdescripcion = dr.GetOrdinal(this.Mreptipdescripcion);
            if (!dr.IsDBNull(iMreptipdescripcion)) entity.Mreptipdescripcion = dr.GetString(iMreptipdescripcion);

            int iMprojcodi = dr.GetOrdinal(this.Mprojcodi);
            if (!dr.IsDBNull(iMprojcodi)) entity.Mprojcodi = Convert.ToInt32(dr.GetValue(iMprojcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Mreptipcodi = "TMREPCODI";
        public string Mreptipdescripcion = "TMREPDESCRIPCION";
        public string Mprojcodi = "MPROJCODI";

        #endregion
    }
}
