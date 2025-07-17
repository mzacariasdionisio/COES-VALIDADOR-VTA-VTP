using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CO_TIPOINFORMACION
    /// </summary>
    public class CoTipoinformacionHelper : HelperBase
    {
        public CoTipoinformacionHelper(): base(Consultas.CoTipoinformacionSql)
        {
        }

        public CoTipoinformacionDTO Create(IDataReader dr)
        {
            CoTipoinformacionDTO entity = new CoTipoinformacionDTO();

            int iCotinfestado = dr.GetOrdinal(this.Cotinfestado);
            if (!dr.IsDBNull(iCotinfestado)) entity.Cotinfestado = dr.GetString(iCotinfestado);

            int iCotinfusucreacion = dr.GetOrdinal(this.Cotinfusucreacion);
            if (!dr.IsDBNull(iCotinfusucreacion)) entity.Cotinfusucreacion = dr.GetString(iCotinfusucreacion);

            int iCotinffeccreacion = dr.GetOrdinal(this.Cotinffeccreacion);
            if (!dr.IsDBNull(iCotinffeccreacion)) entity.Cotinffeccreacion = dr.GetDateTime(iCotinffeccreacion);

            int iCotinfusumodificacion = dr.GetOrdinal(this.Cotinfusumodificacion);
            if (!dr.IsDBNull(iCotinfusumodificacion)) entity.Cotinfusumodificacion = dr.GetString(iCotinfusumodificacion);

            int iCotinffecmodificacion = dr.GetOrdinal(this.Cotinffecmodificacion);
            if (!dr.IsDBNull(iCotinffecmodificacion)) entity.Cotinffecmodificacion = dr.GetDateTime(iCotinffecmodificacion);

            int iCotinfcodi = dr.GetOrdinal(this.Cotinfcodi);
            if (!dr.IsDBNull(iCotinfcodi)) entity.Cotinfcodi = Convert.ToInt32(dr.GetValue(iCotinfcodi));

            int iCotinfdesc = dr.GetOrdinal(this.Cotinfdesc);
            if (!dr.IsDBNull(iCotinfdesc)) entity.Cotinfdesc = dr.GetString(iCotinfdesc);

            return entity;
        }


        #region Mapeo de Campos

        public string Cotinfestado = "COTINFESTADO";
        public string Cotinfusucreacion = "COTINFUSUCREACION";
        public string Cotinffeccreacion = "COTINFFECCREACION";
        public string Cotinfusumodificacion = "COTINFUSUMODIFICACION";
        public string Cotinffecmodificacion = "COTINFFECMODIFICACION";
        public string Cotinfcodi = "COTINFCODI";
        public string Cotinfdesc = "COTINFDESC";

        #endregion
    }
}
