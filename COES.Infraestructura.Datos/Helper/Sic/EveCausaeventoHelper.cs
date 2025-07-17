using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_CAUSAEVENTO
    /// </summary>
    public class EveCausaeventoHelper : HelperBase
    {
        public EveCausaeventoHelper(): base(Consultas.EveCausaeventoSql)
        {
        }

        public EveCausaeventoDTO Create(IDataReader dr)
        {
            EveCausaeventoDTO entity = new EveCausaeventoDTO();

            int iCausaevencodi = dr.GetOrdinal(this.Causaevencodi);
            if (!dr.IsDBNull(iCausaevencodi)) entity.Causaevencodi = Convert.ToInt32(dr.GetValue(iCausaevencodi));

            int iCausaevendesc = dr.GetOrdinal(this.Causaevendesc);
            if (!dr.IsDBNull(iCausaevendesc)) entity.Causaevendesc = dr.GetString(iCausaevendesc);

            int iCausaevenabrev = dr.GetOrdinal(this.Causaevenabrev);
            if (!dr.IsDBNull(iCausaevenabrev)) entity.Causaevenabrev = dr.GetString(iCausaevenabrev);

            return entity;
        }


        #region Mapeo de Campos

        public string Causaevencodi = "CAUSAEVENCODI";
        public string Causaevendesc = "CAUSAEVENDESC";
        public string Causaevenabrev = "CAUSAEVENABREV";

        #endregion
    }
}

