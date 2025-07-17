using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_EQUIVALENCIAMODOP
    /// </summary>
    public class CmEquivalenciamodopHelper : HelperBase
    {
        public CmEquivalenciamodopHelper(): base(Consultas.CmEquivalenciamodopSql)
        {
        }

        public CmEquivalenciamodopDTO Create(IDataReader dr)
        {
            CmEquivalenciamodopDTO entity = new CmEquivalenciamodopDTO();

            int iEquimocodi = dr.GetOrdinal(this.Equimocodi);
            if (!dr.IsDBNull(iEquimocodi)) entity.Equimocodi = Convert.ToInt32(dr.GetValue(iEquimocodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iEquimonombrencp = dr.GetOrdinal(this.Equimonombrencp);
            if (!dr.IsDBNull(iEquimonombrencp)) entity.Equimonombrencp = dr.GetString(iEquimonombrencp);

            int iEquimousucreacion = dr.GetOrdinal(this.Equimousucreacion);
            if (!dr.IsDBNull(iEquimousucreacion)) entity.Equimousucreacion = dr.GetString(iEquimousucreacion);

            int iEquimofeccreacion = dr.GetOrdinal(this.Equimofeccreacion);
            if (!dr.IsDBNull(iEquimofeccreacion)) entity.Equimofeccreacion = dr.GetDateTime(iEquimofeccreacion);

            int iEquimousumodificacion = dr.GetOrdinal(this.Equimousumodificacion);
            if (!dr.IsDBNull(iEquimousumodificacion)) entity.Equimousumodificacion = dr.GetString(iEquimousumodificacion);

            int iEquimofecmodificacion = dr.GetOrdinal(this.Equimofecmodificacion);
            if (!dr.IsDBNull(iEquimofecmodificacion)) entity.Equimofecmodificacion = dr.GetDateTime(iEquimofecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Equimocodi = "EQUIMOCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Equimonombrencp = "EQUIMONOMBRENCP";
        public string Equimousucreacion = "EQUIMOUSUCREACION";
        public string Equimofeccreacion = "EQUIMOFECCREACION";
        public string Equimousumodificacion = "EQUIMOUSUMODIFICACION";
        public string Equimofecmodificacion = "EQUIMOFECMODIFICACION";
        public string Gruponomb = "GRUPONOMB";

        public string ObtenerListado
        {
            get { return base.GetSqlXml("ObtenerListado"); }
        }

        public string TotalRegistros
        {
            get { return base.GetSqlXml("TotalRegistros"); }
        }

        #endregion
    }
}
