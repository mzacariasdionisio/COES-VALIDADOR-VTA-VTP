using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PF_RECURSO
    /// </summary>
    public class PfRecursoHelper : HelperBase
    {
        public PfRecursoHelper(): base(Consultas.PfRecursoSql)
        {
        }

        public PfRecursoDTO Create(IDataReader dr)
        {
            PfRecursoDTO entity = new PfRecursoDTO();

            int iPfrecucodi = dr.GetOrdinal(this.Pfrecucodi);
            if (!dr.IsDBNull(iPfrecucodi)) entity.Pfrecucodi = Convert.ToInt32(dr.GetValue(iPfrecucodi));

            int iPfrecunomb = dr.GetOrdinal(this.Pfrecunomb);
            if (!dr.IsDBNull(iPfrecunomb)) entity.Pfrecunomb = dr.GetString(iPfrecunomb);

            int iPfrecudescripcion = dr.GetOrdinal(this.Pfrecudescripcion);
            if (!dr.IsDBNull(iPfrecudescripcion)) entity.Pfrecudescripcion = dr.GetString(iPfrecudescripcion);

            int iPfrecutipo = dr.GetOrdinal(this.Pfrecutipo);
            if (!dr.IsDBNull(iPfrecutipo)) entity.Pfrecutipo = Convert.ToInt32(dr.GetValue(iPfrecutipo));

            return entity;
        }


        #region Mapeo de Campos

        public string Pfrecucodi = "PFRECUCODI";
        public string Pfrecunomb = "PFRECUNOMB";
        public string Pfrecudescripcion = "PFRECUDESCRIPCION";
        public string Pfrecutipo = "PFRECUTIPO";

        #endregion
    }
}
