using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_TIPOIMPUGNACION
    /// </summary>
    public class WbTipoimpugnacionHelper : HelperBase
    {
        public WbTipoimpugnacionHelper(): base(Consultas.WbTipoimpugnacionSql)
        {
        }

        public WbTipoimpugnacionDTO Create(IDataReader dr)
        {
            WbTipoimpugnacionDTO entity = new WbTipoimpugnacionDTO();

            int iTimpgcodi = dr.GetOrdinal(this.Timpgcodi);
            if (!dr.IsDBNull(iTimpgcodi)) entity.Timpgcodi = Convert.ToInt32(dr.GetValue(iTimpgcodi));

            int iTimpgnombre = dr.GetOrdinal(this.Timpgnombre);
            if (!dr.IsDBNull(iTimpgnombre)) entity.Timpgnombre = dr.GetString(iTimpgnombre);

            int iTimpgnombdecision = dr.GetOrdinal(this.Timpgnombdecision);
            if (!dr.IsDBNull(iTimpgnombdecision)) entity.Timpgnombdecision = dr.GetString(iTimpgnombdecision);

            int iTimpgnombrefecha = dr.GetOrdinal(this.Timpgnombrefecha);
            if (!dr.IsDBNull(iTimpgnombrefecha)) entity.Timpgnombrefecha = dr.GetString(iTimpgnombrefecha);

            return entity;
        }


        #region Mapeo de Campos

        public string Timpgcodi = "TIMPGCODI";
        public string Timpgnombre = "TIMPGNOMBRE";
        public string Timpgnombdecision = "TIMPGNOMBDECISION";
        public string Timpgnombrefecha = "TIMPGNOMBREFECHA";

        #endregion
    }
}
