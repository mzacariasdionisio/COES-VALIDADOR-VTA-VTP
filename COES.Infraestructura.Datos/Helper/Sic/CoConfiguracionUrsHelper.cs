using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CO_CONFIGURACION_URS
    /// </summary>
    public class CoConfiguracionUrsHelper : HelperBase
    {
        public CoConfiguracionUrsHelper(): base(Consultas.CoConfiguracionUrsSql)
        {
        }

        public CoConfiguracionUrsDTO Create(IDataReader dr)
        {
            CoConfiguracionUrsDTO entity = new CoConfiguracionUrsDTO();

            int iConurscodi = dr.GetOrdinal(this.Conurscodi);
            if (!dr.IsDBNull(iConurscodi)) entity.Conurscodi = Convert.ToInt32(dr.GetValue(iConurscodi));

            int iCopercodi = dr.GetOrdinal(this.Copercodi);
            if (!dr.IsDBNull(iCopercodi)) entity.Copercodi = Convert.ToInt32(dr.GetValue(iCopercodi));

            int iCovercodi = dr.GetOrdinal(this.Covercodi);
            if (!dr.IsDBNull(iCovercodi)) entity.Covercodi = Convert.ToInt32(dr.GetValue(iCovercodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iConursfecinicio = dr.GetOrdinal(this.Conursfecinicio);
            if (!dr.IsDBNull(iConursfecinicio)) entity.Conursfecinicio = dr.GetDateTime(iConursfecinicio);

            int iConursfecfin = dr.GetOrdinal(this.Conursfecfin);
            if (!dr.IsDBNull(iConursfecfin)) entity.Conursfecfin = dr.GetDateTime(iConursfecfin);

            int iConursusucreacion = dr.GetOrdinal(this.Conursusucreacion);
            if (!dr.IsDBNull(iConursusucreacion)) entity.Conursusucreacion = dr.GetString(iConursusucreacion);

            int iConursfeccreacion = dr.GetOrdinal(this.Conursfeccreacion);
            if (!dr.IsDBNull(iConursfeccreacion)) entity.Conursfeccreacion = dr.GetDateTime(iConursfeccreacion);

            int iConursusumodificacion = dr.GetOrdinal(this.Conursusumodificacion);
            if (!dr.IsDBNull(iConursusumodificacion)) entity.Conursusumodificacion = dr.GetString(iConursusumodificacion);

            int iConursfecmodificacion = dr.GetOrdinal(this.Conursfecmodificacion);
            if (!dr.IsDBNull(iConursfecmodificacion)) entity.Conursfecmodificacion = dr.GetDateTime(iConursfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Conurscodi = "CONURSCODI";
        public string Copercodi = "COPERCODI";
        public string Covercodi = "COVERCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Conursfecinicio = "CONURSFECINICIO";
        public string Conursfecfin = "CONURSFECFIN";
        public string Conursusucreacion = "CONURSUSUCREACION";
        public string Conursfeccreacion = "CONURSFECCREACION";
        public string Conursusumodificacion = "CONURSUSUMODIFICACION";
        public string Conursfecmodificacion = "CONURSFECMODIFICACION";

        #endregion

        public string SqlGetPorVersion
        {
            get { return base.GetSqlXml("GetPorVersion"); }
        }
    }
}
