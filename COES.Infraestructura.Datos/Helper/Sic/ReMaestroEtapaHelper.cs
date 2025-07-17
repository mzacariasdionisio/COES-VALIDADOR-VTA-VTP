using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RE_MAESTRO_ETAPA
    /// </summary>
    public class ReMaestroEtapaHelper : HelperBase
    {
        public ReMaestroEtapaHelper(): base(Consultas.ReMaestroEtapaSql)
        {
        }

        public ReMaestroEtapaDTO Create(IDataReader dr)
        {
            ReMaestroEtapaDTO entity = new ReMaestroEtapaDTO();

            int iReetacodi = dr.GetOrdinal(this.Reetacodi);
            if (!dr.IsDBNull(iReetacodi)) entity.Reetacodi = Convert.ToInt32(dr.GetValue(iReetacodi));

            int iReetanombre = dr.GetOrdinal(this.Reetanombre);
            if (!dr.IsDBNull(iReetanombre)) entity.Reetanombre = dr.GetString(iReetanombre);

            int iReetaorden = dr.GetOrdinal(this.Reetaorden);
            if (!dr.IsDBNull(iReetaorden)) entity.Reetaorden = Convert.ToInt32(dr.GetValue(iReetaorden));

            int iReetaregistro = dr.GetOrdinal(this.Reetaregistro);
            if (!dr.IsDBNull(iReetaregistro)) entity.Reetaregistro = dr.GetString(iReetaregistro);

            int iReetausucreacion = dr.GetOrdinal(this.Reetausucreacion);
            if (!dr.IsDBNull(iReetausucreacion)) entity.Reetausucreacion = dr.GetString(iReetausucreacion);

            int iReetafeccreacion = dr.GetOrdinal(this.Reetafeccreacion);
            if (!dr.IsDBNull(iReetafeccreacion)) entity.Reetafeccreacion = dr.GetDateTime(iReetafeccreacion);

            int iReetausumodificacion = dr.GetOrdinal(this.Reetausumodificacion);
            if (!dr.IsDBNull(iReetausumodificacion)) entity.Reetausumodificacion = dr.GetString(iReetausumodificacion);

            int iReetafecmodificacion = dr.GetOrdinal(this.Reetafecmodificacion);
            if (!dr.IsDBNull(iReetafecmodificacion)) entity.Reetafecmodificacion = dr.GetDateTime(iReetafecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Reetacodi = "REETACODI";
        public string Reetanombre = "REETANOMBRE";
        public string Reetaorden = "REETAORDEN";
        public string Reetaregistro = "REETAREGISTRO";
        public string Reetausucreacion = "REETAUSUCREACION";
        public string Reetafeccreacion = "REETAFECCREACION";
        public string Reetausumodificacion = "REETAUSUMODIFICACION";
        public string Reetafecmodificacion = "REETAFECMODIFICACION";
        public string Repercodi = "REPERCODI";
        public string Repeetfecha = "REPEETFECHA";
        public string Repeetestado = "REPEETESTADO";


        #endregion
    }
}
