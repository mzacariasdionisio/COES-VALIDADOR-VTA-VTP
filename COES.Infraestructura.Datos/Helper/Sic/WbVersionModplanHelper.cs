using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_VERSION_MODPLAN
    /// </summary>
    public class WbVersionModplanHelper : HelperBase
    {
        public WbVersionModplanHelper(): base(Consultas.WbVersionModplanSql)
        {
        }

        public WbVersionModplanDTO Create(IDataReader dr)
        {
            WbVersionModplanDTO entity = new WbVersionModplanDTO();

            int iVermplcodi = dr.GetOrdinal(this.Vermplcodi);
            if (!dr.IsDBNull(iVermplcodi)) entity.Vermplcodi = Convert.ToInt32(dr.GetValue(iVermplcodi));

            int iVermpldesc = dr.GetOrdinal(this.Vermpldesc);
            if (!dr.IsDBNull(iVermpldesc)) entity.Vermpldesc = dr.GetString(iVermpldesc);

            int iVermplestado = dr.GetOrdinal(this.Vermplestado);
            if (!dr.IsDBNull(iVermplestado)) entity.Vermplestado = dr.GetString(iVermplestado);

            int iVermplpadre = dr.GetOrdinal(this.Vermplpadre);
            if (!dr.IsDBNull(iVermplpadre)) entity.Vermplpadre = Convert.ToInt32(dr.GetValue(iVermplpadre));

            int iVermplusucreacion = dr.GetOrdinal(this.Vermplusucreacion);
            if (!dr.IsDBNull(iVermplusucreacion)) entity.Vermplusucreacion = dr.GetString(iVermplusucreacion);

            int iVermplfeccreacion = dr.GetOrdinal(this.Vermplfeccreacion);
            if (!dr.IsDBNull(iVermplfeccreacion)) entity.Vermplfeccreacion = dr.GetDateTime(iVermplfeccreacion);

            int iVermplusumodificacion = dr.GetOrdinal(this.Vermplusumodificacion);
            if (!dr.IsDBNull(iVermplusumodificacion)) entity.Vermplusumodificacion = dr.GetString(iVermplusumodificacion);

            int iVermplfecmodificacion = dr.GetOrdinal(this.Vermplfecmodificacion);
            if (!dr.IsDBNull(iVermplfecmodificacion)) entity.Vermplfecmodificacion = dr.GetDateTime(iVermplfecmodificacion);

            int iVermpltipo = dr.GetOrdinal(this.Vermpltipo);
            if (!dr.IsDBNull(iVermpltipo)) entity.Vermpltipo = Convert.ToInt32(dr.GetValue(iVermpltipo));

            return entity;
        }


        #region Mapeo de Campos

        public string Vermplcodi = "VERMPLCODI";
        public string Vermpldesc = "VERMPLDESC";
        public string Vermplestado = "VERMPLESTADO";
        public string Vermplpadre = "VERMPLPADRE";
        public string Vermplusucreacion = "VERMPLUSUCREACION";
        public string Vermplfeccreacion = "VERMPLFECCREACION";
        public string Vermplusumodificacion = "VERMPLUSUMODIFICACION";
        public string Vermplfecmodificacion = "VERMPLFECMODIFICACION";
        public string Vermpltipo = "VERMPLTIPO";

        #endregion

        public string SqlObtenerVersionPorPadre
        {
            get { return base.GetSqlXml("ObtenerVersionPorPadre"); }
        }
    }
}
