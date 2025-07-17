using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_MODELO_AGRUPACION
    /// </summary>
    public class CmModeloAgrupacionHelper : HelperBase
    {
        public CmModeloAgrupacionHelper() : base(Consultas.CmModeloAgrupacionSql)
        {
        }

        public CmModeloAgrupacionDTO Create(IDataReader dr)
        {
            CmModeloAgrupacionDTO entity = new CmModeloAgrupacionDTO();

            int iModagrcodi = dr.GetOrdinal(this.Modagrcodi);
            if (!dr.IsDBNull(iModagrcodi)) entity.Modagrcodi = Convert.ToInt32(dr.GetValue(iModagrcodi));

            int iModcomcodi = dr.GetOrdinal(this.Modcomcodi);
            if (!dr.IsDBNull(iModcomcodi)) entity.Modcomcodi = Convert.ToInt32(dr.GetValue(iModcomcodi));

            int iModagrorden = dr.GetOrdinal(this.Modagrorden);
            if (!dr.IsDBNull(iModagrorden)) entity.Modagrorden = Convert.ToInt32(dr.GetValue(iModagrorden));

            return entity;
        }

        #region Mapeo de Campos

        public string Modagrcodi = "MODAGRCODI";
        public string Modcomcodi = "MODCOMCODI";
        public string Modagrorden = "MODAGRORDEN";

        #endregion
    }
}
