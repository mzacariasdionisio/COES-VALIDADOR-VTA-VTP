using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CO_CONFIGURACION_DET
    /// </summary>
    public class CoConfiguracionDetHelper : HelperBase
    {
        public CoConfiguracionDetHelper(): base(Consultas.CoConfiguracionDetSql)
        {
        }

        public CoConfiguracionDetDTO Create(IDataReader dr)
        {
            CoConfiguracionDetDTO entity = new CoConfiguracionDetDTO();

            int iCourdecodi = dr.GetOrdinal(this.Courdecodi);
            if (!dr.IsDBNull(iCourdecodi)) entity.Courdecodi = Convert.ToInt32(dr.GetValue(iCourdecodi));

            int iConurscodi = dr.GetOrdinal(this.Conurscodi);
            if (!dr.IsDBNull(iConurscodi)) entity.Conurscodi = Convert.ToInt32(dr.GetValue(iConurscodi));

            int iCourdetipo = dr.GetOrdinal(this.Courdetipo);
            if (!dr.IsDBNull(iCourdetipo)) entity.Courdetipo = dr.GetString(iCourdetipo);

            int iCourdeoperacion = dr.GetOrdinal(this.Courdeoperacion);
            if (!dr.IsDBNull(iCourdeoperacion)) entity.Courdeoperacion = dr.GetString(iCourdeoperacion);

            int iCourdereporte = dr.GetOrdinal(this.Courdereporte);
            if (!dr.IsDBNull(iCourdereporte)) entity.Courdereporte = dr.GetString(iCourdereporte);

            int iCourdeequipo = dr.GetOrdinal(this.Courdeequipo);
            if (!dr.IsDBNull(iCourdeequipo)) entity.Courdeequipo = dr.GetString(iCourdeequipo);

            int iCourderequip = dr.GetOrdinal(this.Courderequip);
            if (!dr.IsDBNull(iCourderequip)) entity.Courderequip = dr.GetDecimal(iCourderequip);

            int iCourdevigenciadesde = dr.GetOrdinal(this.Courdevigenciadesde);
            if (!dr.IsDBNull(iCourdevigenciadesde)) entity.Courdevigenciadesde = dr.GetDateTime(iCourdevigenciadesde);

            int iCourdevigenciahasta = dr.GetOrdinal(this.Courdevigenciahasta);
            if (!dr.IsDBNull(iCourdevigenciahasta)) entity.Courdevigenciahasta = dr.GetDateTime(iCourdevigenciahasta);

            int iCourdeusucreacion = dr.GetOrdinal(this.Courdeusucreacion);
            if (!dr.IsDBNull(iCourdeusucreacion)) entity.Courdeusucreacion = dr.GetString(iCourdeusucreacion);

            int iCourdefeccreacion = dr.GetOrdinal(this.Courdefeccreacion);
            if (!dr.IsDBNull(iCourdefeccreacion)) entity.Courdefeccreacion = dr.GetDateTime(iCourdefeccreacion);

            int iCourdeusumodificacion = dr.GetOrdinal(this.Courdeusumodificacion);
            if (!dr.IsDBNull(iCourdeusumodificacion)) entity.Courdeusumodificacion = dr.GetString(iCourdeusumodificacion);

            int iCourdefecmodificacion = dr.GetOrdinal(this.Courdefecmodificacion);
            if (!dr.IsDBNull(iCourdefecmodificacion)) entity.Courdefecmodificacion = dr.GetDateTime(iCourdefecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Courdecodi = "COURDECODI";
        public string Conurscodi = "CONURSCODI";
        public string Courdetipo = "COURDETIPO";
        public string Courdeoperacion = "COURDEOPERACION";
        public string Courdereporte = "COURDEREPORTE";
        public string Courdeequipo = "COURDEEQUIPO";
        public string Courderequip = "COURDEREQUIP";
        public string Courdevigenciadesde = "COURDEVIGENCIADESDE";
        public string Courdevigenciahasta = "COURDEVIGENCIAHASTA";
        public string Courdeusucreacion = "COURDEUSUCREACION";
        public string Courdefeccreacion = "COURDEFECCREACION";
        public string Courdeusumodificacion = "COURDEUSUMODIFICACION";
        public string Courdefecmodificacion = "COURDEFECMODIFICACION";

        public string Grupocodi = "grupocodi";
        public string FecInicioHabilitacion = "conursfecinicio";
        public string FecFinHabilitacion = "conursfecfin";
        public string ContadorSenial = "contador";

        public string Coverdesc = "COVERDESC";
        public string Copercodi = "COPERCODI";
        public string Covercodi = "COVERCODI";
        

        #endregion

        public string SqlObtenerConfiguracion
        {
            get { return base.GetSqlXml("ObtenerConfiguracion"); }
        }

        public string SqlObtenerInfoConfiguracionUrs
        {
            get { return base.GetSqlXml("ObtenerInfoConfiguracionUrs"); }
        }
        
    }
}
