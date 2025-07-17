using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CO_TIPO_DATO
    /// </summary>
    public class CoTipoDatoHelper : HelperBase
    {
        public CoTipoDatoHelper(): base(Consultas.CoTipoDatoSql)
        {
        }

        public CoTipoDatoDTO Create(IDataReader dr)
        {
            CoTipoDatoDTO entity = new CoTipoDatoDTO();

            int iCotidausumodificacion = dr.GetOrdinal(this.Cotidausumodificacion);
            if (!dr.IsDBNull(iCotidausumodificacion)) entity.Cotidausumodificacion = dr.GetString(iCotidausumodificacion);

            int iCotidafecmodificacion = dr.GetOrdinal(this.Cotidafecmodificacion);
            if (!dr.IsDBNull(iCotidafecmodificacion)) entity.Cotidafecmodificacion = dr.GetDateTime(iCotidafecmodificacion);

            int iCotidacodi = dr.GetOrdinal(this.Cotidacodi);
            if (!dr.IsDBNull(iCotidacodi)) entity.Cotidacodi = Convert.ToInt32(dr.GetValue(iCotidacodi));

            int iCotidaindicador = dr.GetOrdinal(this.Cotidaindicador);
            if (!dr.IsDBNull(iCotidaindicador)) entity.Cotidaindicador = dr.GetString(iCotidaindicador);

            int iCotidausucreacion = dr.GetOrdinal(this.Cotidausucreacion);
            if (!dr.IsDBNull(iCotidausucreacion)) entity.Cotidausucreacion = dr.GetString(iCotidausucreacion);

            int iCotidafeccreacion = dr.GetOrdinal(this.Cotidafeccreacion);
            if (!dr.IsDBNull(iCotidafeccreacion)) entity.Cotidafeccreacion = dr.GetDateTime(iCotidafeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Cotidausumodificacion = "COTIDAUSUMODIFICACION";
        public string Cotidafecmodificacion = "COTIDAFECMODIFICACION";
        public string Cotidacodi = "COTIDACODI";
        public string Cotidaindicador = "COTIDAINDICADOR";
        public string Cotidausucreacion = "COTIDAUSUCREACION";
        public string Cotidafeccreacion = "COTIDAFECCREACION";

        #endregion
    }
}
