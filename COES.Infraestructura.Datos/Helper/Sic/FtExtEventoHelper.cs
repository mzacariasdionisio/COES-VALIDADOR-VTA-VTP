using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_EVENTO
    /// </summary>
    public class FtExtEventoHelper : HelperBase
    {
        public FtExtEventoHelper() : base(Consultas.FtExtEventoSql)
        {
        }

        public FtExtEventoDTO Create(IDataReader dr)
        {
            FtExtEventoDTO entity = new FtExtEventoDTO();

            int iFtevcodi = dr.GetOrdinal(this.Ftevcodi);
            if (!dr.IsDBNull(iFtevcodi)) entity.Ftevcodi = Convert.ToInt32(dr.GetValue(iFtevcodi));

            int iFtevnombre = dr.GetOrdinal(this.Ftevnombre);
            if (!dr.IsDBNull(iFtevnombre)) entity.Ftevnombre = dr.GetString(iFtevnombre);

            int iFtevfecvigenciaext = dr.GetOrdinal(this.Ftevfecvigenciaext);
            if (!dr.IsDBNull(iFtevfecvigenciaext)) entity.Ftevfecvigenciaext = dr.GetDateTime(iFtevfecvigenciaext);

            int iFtevestado = dr.GetOrdinal(this.Ftevestado);
            if (!dr.IsDBNull(iFtevestado)) entity.Ftevestado = dr.GetString(iFtevestado);

            int iFtevusucreacion = dr.GetOrdinal(this.Ftevusucreacion);
            if (!dr.IsDBNull(iFtevusucreacion)) entity.Ftevusucreacion = dr.GetString(iFtevusucreacion);

            int iFtevfeccreacion = dr.GetOrdinal(this.Ftevfeccreacion);
            if (!dr.IsDBNull(iFtevfeccreacion)) entity.Ftevfeccreacion = dr.GetDateTime(iFtevfeccreacion);

            int iFtevusumodificacion = dr.GetOrdinal(this.Ftevusumodificacion);
            if (!dr.IsDBNull(iFtevusumodificacion)) entity.Ftevusumodificacion = dr.GetString(iFtevusumodificacion);

            int iFtevfecmodificacion = dr.GetOrdinal(this.Ftevfecmodificacion);
            if (!dr.IsDBNull(iFtevfecmodificacion)) entity.Ftevfecmodificacion = dr.GetDateTime(iFtevfecmodificacion);

            int iFtevusumodificacionasig = dr.GetOrdinal(this.Ftevusumodificacionasig);
            if (!dr.IsDBNull(iFtevusumodificacionasig)) entity.Ftevusumodificacionasig = dr.GetString(iFtevusumodificacionasig);

            int iFtevfecmodificacionasig = dr.GetOrdinal(this.Ftevfecmodificacionasig);
            if (!dr.IsDBNull(iFtevfecmodificacionasig)) entity.Ftevfecmodificacionasig = dr.GetDateTime(iFtevfecmodificacionasig);

            return entity;
        }

        #region Mapeo de Campos

        public string Ftevcodi = "FTEVCODI";
        public string Ftevnombre = "FTEVNOMBRE";
        public string Ftevfecvigenciaext = "FTEVFECVIGENCIAEXT";
        public string Ftevestado = "FTEVESTADO";
        public string Ftevusucreacion = "FTEVUSUCREACION";
        public string Ftevfeccreacion = "FTEVFECCREACION";
        public string Ftevusumodificacion = "FTEVUSUMODIFICACION";
        public string Ftevfecmodificacion = "FTEVFECMODIFICACION";
        public string Ftevusumodificacionasig = "FTEVUSUMODIFICACIONASIG";
        public string Ftevfecmodificacionasig = "FTEVFECMODIFICACIONASIG";

        #endregion
    }
}
