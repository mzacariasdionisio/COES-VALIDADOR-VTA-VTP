using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EPO_ESTUDIO_ESTADO
    /// </summary>
    public class EpoEstudioEstadoHelper : HelperBase
    {
        public EpoEstudioEstadoHelper(): base(Consultas.EpoEstudioEstadoSql)
        {
        }

        public EpoEstudioEstadoDTO Create(IDataReader dr)
        {
            EpoEstudioEstadoDTO entity = new EpoEstudioEstadoDTO();

            int iEstacodi = dr.GetOrdinal(this.Estacodi);
            if (!dr.IsDBNull(iEstacodi)) entity.Estacodi = Convert.ToInt32(dr.GetValue(iEstacodi));

            int iEstadescripcion = dr.GetOrdinal(this.Estadescripcion);
            if (!dr.IsDBNull(iEstadescripcion)) entity.Estadescripcion = dr.GetString(iEstadescripcion);

            return entity;
        }


        #region Mapeo de Campos

        public string Estacodi = "ESTACODI";
        public string Estadescripcion = "ESTADESCRIPCION";

        #endregion
    }
}
