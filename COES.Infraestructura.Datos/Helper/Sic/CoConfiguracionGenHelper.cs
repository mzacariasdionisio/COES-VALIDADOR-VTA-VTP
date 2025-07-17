using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CO_CONFIGURACION_GEN
    /// </summary>
    public class CoConfiguracionGenHelper : HelperBase
    {
        public CoConfiguracionGenHelper(): base(Consultas.CoConfiguracionGenSql)
        {
        }

        public CoConfiguracionGenDTO Create(IDataReader dr)
        {
            CoConfiguracionGenDTO entity = new CoConfiguracionGenDTO();

            int iCourgecodi = dr.GetOrdinal(this.Courgecodi);
            if (!dr.IsDBNull(iCourgecodi)) entity.Courgecodi = Convert.ToInt32(dr.GetValue(iCourgecodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iCourdecodi = dr.GetOrdinal(this.Courdecodi);
            if (!dr.IsDBNull(iCourdecodi)) entity.Courdecodi = Convert.ToInt32(dr.GetValue(iCourdecodi));

            int iCourgeusucreacion = dr.GetOrdinal(this.Courgeusucreacion);
            if (!dr.IsDBNull(iCourgeusucreacion)) entity.Courgeusucreacion = dr.GetString(iCourgeusucreacion);

            int iCourgefeccreacion = dr.GetOrdinal(this.Courgefeccreacion);
            if (!dr.IsDBNull(iCourgefeccreacion)) entity.Courgefeccreacion = dr.GetDateTime(iCourgefeccreacion);

            int iCourgeusumodificacion = dr.GetOrdinal(this.Courgeusumodificacion);
            if (!dr.IsDBNull(iCourgeusumodificacion)) entity.Courgeusumodificacion = dr.GetString(iCourgeusumodificacion);

            int iCourgefecmodificacion = dr.GetOrdinal(this.Courgefecmodificacion);
            if (!dr.IsDBNull(iCourgefecmodificacion)) entity.Courgefecmodificacion = dr.GetDateTime(iCourgefecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Courgecodi = "COURGECODI";
        public string Equicodi = "EQUICODI";
        public string Courdecodi = "COURDECODI";
        public string Courgeusucreacion = "COURGEUSUCREACION";
        public string Courgefeccreacion = "COURGEFECCREACION";
        public string Courgeusumodificacion = "COURGEUSUMODIFICACION";
        public string Courgefecmodificacion = "COURGEFECMODIFICACION";
        public string Grupocodi = "GRUPOCODI";

        #endregion

        public string SqlGetUnidadesSeleccionadas
        {
            get { return base.GetSqlXml("GetUnidadesSeleccionadas"); }
        }
    }
}
