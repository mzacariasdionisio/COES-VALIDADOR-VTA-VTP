using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SEG_ZONA
    /// </summary>
    public class SegZonaHelper : HelperBase
    {
        public SegZonaHelper(): base(Consultas.SegZonaSql)
        {
        }

        public SegZonaDTO Create(IDataReader dr)
        {
            SegZonaDTO entity = new SegZonaDTO();

            int iZoncodi = dr.GetOrdinal(this.Zoncodi);
            if (!dr.IsDBNull(iZoncodi)) entity.Zoncodi = Convert.ToInt32(dr.GetValue(iZoncodi));

            int iZonnombre = dr.GetOrdinal(this.Zonnombre);
            if (!dr.IsDBNull(iZonnombre)) entity.Zonnombre = dr.GetString(iZonnombre);

            return entity;
        }


        #region Mapeo de Campos

        public string Zoncodi = "ZONCODI";
        public string Zonnombre = "ZONNOMBRE";

        #endregion
    }
}
