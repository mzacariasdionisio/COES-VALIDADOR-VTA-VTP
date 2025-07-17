using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_TIPOMIGRAOPERACION
    /// </summary>
    public class SiTipomigraOperacionHelper : HelperBase
    {
        public SiTipomigraOperacionHelper(): base(Consultas.SiTipomigraoperacionSql)
        {
        }

        public SiTipomigraoperacionDTO Create(IDataReader dr)
        {
            SiTipomigraoperacionDTO entity = new SiTipomigraoperacionDTO();

            int iTmopercodi = dr.GetOrdinal(this.Tmopercodi);
            if (!dr.IsDBNull(iTmopercodi)) entity.Tmopercodi = Convert.ToInt32(dr.GetValue(iTmopercodi));

            int iTmoperdescripcion = dr.GetOrdinal(this.Tmoperdescripcion);
            if (!dr.IsDBNull(iTmoperdescripcion)) entity.Tmoperdescripcion = dr.GetString(iTmoperdescripcion);

            int iTmoperusucreacion = dr.GetOrdinal(this.Tmoperusucreacion);
            if (!dr.IsDBNull(iTmoperusucreacion)) entity.Tmoperusucreacion = dr.GetString(iTmoperusucreacion);

            int iTmoperfeccreacion = dr.GetOrdinal(this.Tmoperfeccreacion);
            if (!dr.IsDBNull(iTmoperfeccreacion)) entity.Tmoperfeccreacion = dr.GetDateTime(iTmoperfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Tmopercodi = "TMOPERCODI";
        public string Tmoperdescripcion = "TMOPERDESCRIPCION";
        public string Tmoperusucreacion = "TMOPERUSUCREACION";
        public string Tmoperfeccreacion = "TMOPERFECCREACION";

        #endregion
    }
}
