using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_GENERADOR_POTENCIAGEN
    /// </summary>
    public class CmGeneradorPotenciagenHelper : HelperBase
    {
        public CmGeneradorPotenciagenHelper(): base(Consultas.CmGeneradorPotenciagenSql)
        {
        }

        public CmGeneradorPotenciagenDTO Create(IDataReader dr)
        {
            CmGeneradorPotenciagenDTO entity = new CmGeneradorPotenciagenDTO();

            int iGenpotcodi = dr.GetOrdinal(this.Genpotcodi);
            if (!dr.IsDBNull(iGenpotcodi)) entity.Genpotcodi = Convert.ToInt32(dr.GetValue(iGenpotcodi));

            int iRelacioncodi = dr.GetOrdinal(this.Relacioncodi);
            if (!dr.IsDBNull(iRelacioncodi)) entity.Relacioncodi = Convert.ToInt32(dr.GetValue(iRelacioncodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iGenpotvalor = dr.GetOrdinal(this.Genpotvalor);
            if (!dr.IsDBNull(iGenpotvalor)) entity.Genpotvalor = dr.GetDecimal(iGenpotvalor);

            int iGenpotusucreacion = dr.GetOrdinal(this.Genpotusucreacion);
            if (!dr.IsDBNull(iGenpotusucreacion)) entity.Genpotusucreacion = dr.GetString(iGenpotusucreacion);

            int iGenpotfeccreacion = dr.GetOrdinal(this.Genpotfeccreacion);
            if (!dr.IsDBNull(iGenpotfeccreacion)) entity.Genpotfeccreacion = dr.GetDateTime(iGenpotfeccreacion);

            int iGenpotusumodificacion = dr.GetOrdinal(this.Genpotusumodificacion);
            if (!dr.IsDBNull(iGenpotusumodificacion)) entity.Genpotusumodificacion = dr.GetString(iGenpotusumodificacion);

            int iGenpotfecmodificacion = dr.GetOrdinal(this.Genpotfecmodificacion);
            if (!dr.IsDBNull(iGenpotfecmodificacion)) entity.Genpotfecmodificacion = dr.GetDateTime(iGenpotfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Genpotcodi = "GENPOTCODI";
        public string Relacioncodi = "RELACIONCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Genpotvalor = "GENPOTVALOR";
        public string Genpotusucreacion = "GENPOTUSUCREACION";
        public string Genpotfeccreacion = "GENPOTFECCREACION";
        public string Genpotusumodificacion = "GENPOTUSUMODIFICACION";
        public string Genpotfecmodificacion = "GENPOTFECMODIFICACION";

        #endregion
    }
}
