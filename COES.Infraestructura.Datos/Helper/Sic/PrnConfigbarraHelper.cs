using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class PrnConfigbarraHelper : HelperBase
    {
        public PrnConfigbarraHelper() : base(Consultas.PrnConfigbarraSql)
        {
        }

        public PrnConfigbarraDTO Create(IDataReader dr)
        {
            PrnConfigbarraDTO entity = new PrnConfigbarraDTO();

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iCfgbarfecha = dr.GetOrdinal(this.Cfgbarfecha);
            if (!dr.IsDBNull(iCfgbarfecha)) entity.Cfgbarfecha = dr.GetDateTime(iCfgbarfecha);

            int iCfgbarpse = dr.GetOrdinal(this.Cfgbarpse);
            if (!dr.IsDBNull(iCfgbarpse)) entity.Cfgbarpse = Convert.ToDecimal(dr.GetValue(iCfgbarpse));

            int iCfgbarfactorf = dr.GetOrdinal(this.Cfgbarfactorf);
            if (!dr.IsDBNull(iCfgbarfactorf)) entity.Cfgbarfactorf = Convert.ToDecimal(dr.GetValue(iCfgbarfactorf));

            int iCfgbarusucreacion = dr.GetOrdinal(this.Cfgbarusucreacion);
            if (!dr.IsDBNull(iCfgbarusucreacion)) entity.Cfgbarusucreacion = dr.GetString(iCfgbarusucreacion);

            int iCfgbarfeccreacion = dr.GetOrdinal(this.Cfgbarfeccreacion);
            if (!dr.IsDBNull(iCfgbarfeccreacion)) entity.Cfgbarfeccreacion = dr.GetDateTime(iCfgbarfeccreacion);

            int iCfgbarusumodificacion = dr.GetOrdinal(this.Cfgbarusumodificacion);
            if (!dr.IsDBNull(iCfgbarusumodificacion)) entity.Cfgbarusumodificacion = dr.GetString(iCfgbarusumodificacion);

            int iCfgbarfecmodificacion = dr.GetOrdinal(this.Cfgbarfecmodificacion);
            if (!dr.IsDBNull(iCfgbarfecmodificacion)) entity.Cfgbarfecmodificacion = dr.GetDateTime(iCfgbarfecmodificacion);

            return entity;
        }

        #region Mapeo de Campos

        public string Grupocodi = "GRUPOCODI";
        public string Cfgbarfecha = "CFGBARFECHA";
        public string Cfgbarpse = "CFGBARPSE";
        public string Cfgbarfactorf = "CFGBARFACTORF";
        public string Cfgbarusucreacion = "CFGBARUSUCREACION";
        public string Cfgbarfeccreacion = "CFGBARFECCREACION";
        public string Cfgbarusumodificacion = "CFGBARUSUMODIFICACION";
        public string Cfgbarfecmodificacion = "CFGBARFECMODIFICACION";

        public string Gruponomb = "GRUPONOMB";
        public string Cfgbartiporeg = "CFGBARTIPOREG";
        #endregion

        #region Consultas a la BD

        public string SqlParametrosList
        {
            get { return base.GetSqlXml("ParametrosList"); }
        }

        public string SqlGetConfiguracion
        {
            get { return base.GetSqlXml("GetConfiguracion"); }
        }

        #endregion
    }
}
