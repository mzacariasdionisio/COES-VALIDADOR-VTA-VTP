using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla NR_SUBMODULO
    /// </summary>
    public class NrSubmoduloHelper : HelperBase
    {
        public NrSubmoduloHelper(): base(Consultas.NrSubmoduloSql)
        {
        }

        public NrSubmoduloDTO Create(IDataReader dr)
        {
            NrSubmoduloDTO entity = new NrSubmoduloDTO();

            int iNrsmodcodi = dr.GetOrdinal(this.Nrsmodcodi);
            if (!dr.IsDBNull(iNrsmodcodi)) entity.Nrsmodcodi = Convert.ToInt32(dr.GetValue(iNrsmodcodi));

            int iNrsmodnombre = dr.GetOrdinal(this.Nrsmodnombre);
            if (!dr.IsDBNull(iNrsmodnombre)) entity.Nrsmodnombre = dr.GetString(iNrsmodnombre);

            return entity;
        }


        #region Mapeo de Campos

        public string Nrsmodcodi = "NRSMODCODI";
        public string Nrsmodnombre = "NRSMODNOMBRE";



        #endregion
    }
}
