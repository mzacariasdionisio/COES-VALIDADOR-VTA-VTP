using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_VERSION_CONCEPTO
    /// </summary>
    public class SiVersionConceptoHelper : HelperBase
    {
        public SiVersionConceptoHelper() : base(Consultas.SiVersionConceptoSql)
        {
        }

        public SiVersionConceptoDTO Create(IDataReader dr)
        {
            SiVersionConceptoDTO entity = new SiVersionConceptoDTO();

            int iVercnpcodi = dr.GetOrdinal(this.Vercnpcodi);
            if (!dr.IsDBNull(iVercnpcodi)) entity.Vercnpcodi = Convert.ToInt32(dr.GetValue(iVercnpcodi));

            int iVercnpdesc = dr.GetOrdinal(this.Vercnpdesc);
            if (!dr.IsDBNull(iVercnpdesc)) entity.Vercnpdesc = dr.GetString(iVercnpdesc);

            int iVercnptipo = dr.GetOrdinal(this.Vercnptipo);
            if (!dr.IsDBNull(iVercnptipo)) entity.Vercnptipo = dr.GetString(iVercnptipo);

            return entity;
        }

        #region Mapeo de Campos

        public string Vercnpcodi = "VERCNPCODI";
        public string Vercnpdesc = "VERCNPDESC";
        public string Vercnptipo = "VERCNPTIPO";

        #endregion
    }
}
