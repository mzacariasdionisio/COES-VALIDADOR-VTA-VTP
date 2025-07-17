using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_MODELO_COMPONENTE
    /// </summary>
    public class CmModeloComponenteHelper : HelperBase
    {
        public CmModeloComponenteHelper() : base(Consultas.CmModeloComponenteSql)
        {
        }

        public CmModeloComponenteDTO Create(IDataReader dr)
        {
            CmModeloComponenteDTO entity = new CmModeloComponenteDTO();

            int iModcomcodi = dr.GetOrdinal(this.Modcomcodi);
            if (!dr.IsDBNull(iModcomcodi)) entity.Modcomcodi = Convert.ToInt32(dr.GetValue(iModcomcodi));

            int iModembcodi = dr.GetOrdinal(this.Modembcodi);
            if (!dr.IsDBNull(iModembcodi)) entity.Modembcodi = Convert.ToInt32(dr.GetValue(iModembcodi));

            int iModcomtipo = dr.GetOrdinal(this.Modcomtipo);
            if (!dr.IsDBNull(iModcomtipo)) entity.Modcomtipo = dr.GetString(iModcomtipo);

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iModcomtviaje = dr.GetOrdinal(this.Modcomtviaje);
            if (!dr.IsDBNull(iModcomtviaje)) entity.Modcomtviaje = dr.GetDecimal(iModcomtviaje);

            return entity;
        }

        #region Mapeo de Campos

        public string Modcomcodi = "MODCOMCODI";
        public string Modembcodi = "MODEMBCODI";
        public string Modcomtipo = "MODCOMTIPO";
        public string Equicodi = "EQUICODI";
        public string Modcomtviaje = "MODCOMTVIAJE";

        public string Recurcodi = "RECURCODI";
        public string Recurnombre = "RECURNOMBRE";
        public string Equinomb = "EQUINOMB";

        #endregion
    }
}
