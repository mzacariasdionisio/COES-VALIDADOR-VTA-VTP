using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_HORAOPERACION_EQUIPO
    /// </summary>
    public class EveHoraoperacionEquipoHelper : HelperBase
    {
        public EveHoraoperacionEquipoHelper(): base(Consultas.EveHoraoperacionEquipoSql)
        {
        }

        public EveHoraoperacionEquipoDTO Create(IDataReader dr)
        {
            EveHoraoperacionEquipoDTO entity = new EveHoraoperacionEquipoDTO();

            int iHopequcodi = dr.GetOrdinal(this.Hopequcodi);
            if (!dr.IsDBNull(iHopequcodi)) entity.Hopequcodi = Convert.ToInt32(dr.GetValue(iHopequcodi));

            int iHopcodi = dr.GetOrdinal(this.Hopcodi);
            if (!dr.IsDBNull(iHopcodi)) entity.Hopcodi = Convert.ToInt32(dr.GetValue(iHopcodi));

            int iConfigcodi = dr.GetOrdinal(this.Configcodi);
            if (!dr.IsDBNull(iConfigcodi)) entity.Configcodi = Convert.ToInt32(dr.GetValue(iConfigcodi));

            int iGrulincodi = dr.GetOrdinal(this.Grulincodi);
            if (!dr.IsDBNull(iGrulincodi)) entity.Grulincodi = Convert.ToInt32(dr.GetValue(iGrulincodi));

            int iRegsegcodi = dr.GetOrdinal(this.Regsegcodi);
            if (!dr.IsDBNull(iRegsegcodi)) entity.Regsegcodi = Convert.ToInt32(dr.GetValue(iRegsegcodi));

            int iHopequusucreacion = dr.GetOrdinal(this.Hopequusucreacion);
            if (!dr.IsDBNull(iHopequusucreacion)) entity.Hopequusucreacion = dr.GetString(iHopequusucreacion);

            int iHopequfeccreacion = dr.GetOrdinal(this.Hopequfeccreacion);
            if (!dr.IsDBNull(iHopequfeccreacion)) entity.Hopequfeccreacion = dr.GetDateTime(iHopequfeccreacion);

            int iHopequusumodificacion = dr.GetOrdinal(this.Hopequusumodificacion);
            if (!dr.IsDBNull(iHopequusumodificacion)) entity.Hopequusumodificacion = dr.GetString(iHopequusumodificacion);

            int iHopequfecmodificacion = dr.GetOrdinal(this.Hopequfecmodificacion);
            if (!dr.IsDBNull(iHopequfecmodificacion)) entity.Hopequfecmodificacion = dr.GetDateTime(iHopequfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Hopequcodi = "HOPEQUCODI";
        public string Hopcodi = "HOPCODI";
        public string Configcodi = "CONFIGCODI";
        public string Grulincodi = "GRULINCODI";
        public string Regsegcodi = "REGSEGCODI";
        public string Hopequusucreacion = "HOPEQUUSUCREACION";
        public string Hopequfeccreacion = "HOPEQUFECCREACION";
        public string Hopequusumodificacion = "HOPEQUUSUMODIFICACION";
        public string Hopequfecmodificacion = "HOPEQUFECMODIFICACION";

        #endregion

        public string SqlListarEquiposInv
        {
            get { return base.GetSqlXml("ListarEquiposInv"); }
        }
    }
}
