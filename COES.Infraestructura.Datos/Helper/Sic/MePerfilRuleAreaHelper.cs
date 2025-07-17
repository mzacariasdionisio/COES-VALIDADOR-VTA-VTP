using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_PERFIL_RULE_AREA
    /// </summary>
    public class MePerfilRuleAreaHelper : HelperBase
    {
        public MePerfilRuleAreaHelper(): base(Consultas.MePerfilRuleAreaSql)
        {
        }

        public MePerfilRuleAreaDTO Create(IDataReader dr)
        {
            MePerfilRuleAreaDTO entity = new MePerfilRuleAreaDTO();

            int iAreacode = dr.GetOrdinal(this.Areacode);
            if (!dr.IsDBNull(iAreacode)) entity.Areacode = Convert.ToInt32(dr.GetValue(iAreacode));

            int iPrrucodi = dr.GetOrdinal(this.Prrucodi);
            if (!dr.IsDBNull(iPrrucodi)) entity.Prrucodi = Convert.ToInt32(dr.GetValue(iPrrucodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Areacode = "AREACODE";
        public string Prrucodi = "PRRUCODI";

        #endregion
    }
}
