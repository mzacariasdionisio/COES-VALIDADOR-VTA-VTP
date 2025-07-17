using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EN_ESTADOS
    /// </summary>
    public class EnEstadoHelper : HelperBase
    {
        public EnEstadoHelper(): base(Consultas.EnEstadosSql)
        {
        }

        public EnEstadoDTO Create(IDataReader dr)
        {
            EnEstadoDTO entity = new EnEstadoDTO();

            int iEstadocodi = dr.GetOrdinal(this.Estadocodi);
            if (!dr.IsDBNull(iEstadocodi)) entity.Estadocodi = Convert.ToInt32(dr.GetValue(iEstadocodi));

            int iEstadonombre = dr.GetOrdinal(this.Estadonombre);
            if (!dr.IsDBNull(iEstadonombre)) entity.Estadonombre = dr.GetString(iEstadonombre);

            int iEstadocolor = dr.GetOrdinal(this.Estadocolor);
            if (!dr.IsDBNull(iEstadocolor)) entity.Estadocolor = dr.GetString(iEstadocolor);

            return entity;
        }


        #region Mapeo de Campos

        public string Estadocodi = "ESTADOCODI";
        public string Estadonombre = "ESTADONOMBRE";
        public string Estadocolor = "ESTADOCOLOR";

        #endregion
    }
}
