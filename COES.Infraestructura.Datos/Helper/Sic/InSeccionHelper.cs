using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IN_SECCION
    /// </summary>
    public class InSeccionHelper : HelperBase
    {
        public InSeccionHelper() : base(Consultas.InSeccionSql)
        {
        }

        public InSeccionDTO Create(IDataReader dr)
        {
            InSeccionDTO entity = new InSeccionDTO();

            int iInseccodi = dr.GetOrdinal(this.Inseccodi);
            if (!dr.IsDBNull(iInseccodi)) entity.Inseccodi = Convert.ToInt32(dr.GetValue(iInseccodi));

            int iInsecnombre = dr.GetOrdinal(this.Insecnombre);
            if (!dr.IsDBNull(iInsecnombre)) entity.Insecnombre = dr.GetString(iInsecnombre);

            int iInseccontenido = dr.GetOrdinal(this.Inseccontenido);
            if (!dr.IsDBNull(iInseccontenido)) entity.Inseccontenido = dr.GetString(iInseccontenido);

            int iInsecusumodificacion = dr.GetOrdinal(this.Insecusumodificacion);
            if (!dr.IsDBNull(iInsecusumodificacion)) entity.Insecusumodificacion = dr.GetString(iInsecusumodificacion);

            int iInsecfeccracion = dr.GetOrdinal(this.Insecfeccracion);
            if (!dr.IsDBNull(iInsecfeccracion)) entity.Insecfeccracion = dr.GetDateTime(iInsecfeccracion);

            int iInsecusucreacion = dr.GetOrdinal(this.Insecusucreacion);
            if (!dr.IsDBNull(iInsecusucreacion)) entity.Insecusucreacion = dr.GetString(iInsecusucreacion);

            int iInsecfeccreacion = dr.GetOrdinal(this.Insecfeccreacion);
            if (!dr.IsDBNull(iInsecfeccreacion)) entity.Insecfeccreacion = dr.GetDateTime(iInsecfeccreacion);

            int iInrepcodi = dr.GetOrdinal(this.Inrepcodi);
            if (!dr.IsDBNull(iInrepcodi)) entity.Inrepcodi = Convert.ToInt32(dr.GetValue(iInrepcodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Inseccodi = "INSECCODI";
        public string Insecnombre = "INSECNOMBRE";
        public string Inseccontenido = "INSECCONTENIDO";
        public string Insecusumodificacion = "INSECUSUMODIFICACION";
        public string Insecfeccracion = "INSECFECCRACION";
        public string Insecusucreacion = "INSECUSUCREACION";
        public string Insecfeccreacion = "INSECFECCRACION";
        public string Inrepcodi = "INREPCODI";

        #endregion


        public string SqlUpdateSeccion
        {
            get { return GetSqlXml("UpdateSeccion"); }
        }
    }
}
