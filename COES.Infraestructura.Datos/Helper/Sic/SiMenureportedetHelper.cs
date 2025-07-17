using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_MENUREPORTEDET
    /// </summary>
    public class SiMenureportedetHelper : HelperBase
    {
        public SiMenureportedetHelper(): base(Consultas.SiMenureportedetSql)
        {
        }

        public SiMenureportedetDTO Create(IDataReader dr)
        {
            SiMenureportedetDTO entity = new SiMenureportedetDTO();

            int iMrepdcodigo = dr.GetOrdinal(this.Mrepdcodigo);
            if (!dr.IsDBNull(iMrepdcodigo)) entity.Mrepdcodigo = Convert.ToInt32(dr.GetValue(iMrepdcodigo));

            int iMrepcodi = dr.GetOrdinal(this.Mrepcodi);
            if (!dr.IsDBNull(iMrepcodi)) entity.Mrepcodi = Convert.ToInt32(dr.GetValue(iMrepcodi));

            int iMrepdtitulo = dr.GetOrdinal(this.Mrepdtitulo);
            if (!dr.IsDBNull(iMrepdtitulo)) entity.Mrepdtitulo = dr.GetString(iMrepdtitulo);

            int iMrepdestado = dr.GetOrdinal(this.Mrepdestado);
            if (!dr.IsDBNull(iMrepdestado)) entity.Mrepdestado = Convert.ToInt32(dr.GetValue(iMrepdestado));

            int iMrepdorden = dr.GetOrdinal(this.Mrepdorden);
            if (!dr.IsDBNull(iMrepdorden)) entity.Mrepdorden = Convert.ToInt32(dr.GetValue(iMrepdorden));

            int iMrepdusucreacion = dr.GetOrdinal(this.Mrepdusucreacion);
            if (!dr.IsDBNull(iMrepdusucreacion)) entity.Mrepdusucreacion = dr.GetString(iMrepdusucreacion);

            int iMrepdfeccreacion = dr.GetOrdinal(this.Mrepdfeccreacion);
            if (!dr.IsDBNull(iMrepdfeccreacion)) entity.Mrepdfeccreacion = dr.GetDateTime(iMrepdfeccreacion);

            int iMrepdusumodificacion = dr.GetOrdinal(this.Mrepdusumodificacion);
            if (!dr.IsDBNull(iMrepdusumodificacion)) entity.Mrepdusumodificacion = dr.GetString(iMrepdusumodificacion);

            int iMrepdfecmodificacion = dr.GetOrdinal(this.Mrepdfecmodificacion);
            if (!dr.IsDBNull(iMrepdfecmodificacion)) entity.Mrepdfecmodificacion = dr.GetDateTime(iMrepdfecmodificacion);

            int iMrepddescripcion = dr.GetOrdinal(this.Mrepddescripcion);
            if (!dr.IsDBNull(iMrepddescripcion)) entity.Mrepddescripcion = dr.GetString(iMrepddescripcion);

            return entity;
        }


        #region Mapeo de Campos

        public string Mrepdcodigo = "MREPDCODIGO";
        public string Mrepcodi = "MREPCODI";
        public string Mrepdtitulo = "MREPDTITULO";
        public string Mrepdestado = "MREPDESTADO";
        public string Mrepdorden = "MREPDORDEN";
        public string Mrepdusucreacion = "MREPDUSUCREACION";
        public string Mrepdfeccreacion = "MREPDFECCREACION";
        public string Mrepdusumodificacion = "MREPDUSUMODIFICACION";
        public string Mrepdfecmodificacion = "MREPDFECMODIFICACION";
        public string Mrepddescripcion = "MREPDDESCRIPCION";
        public string Tmrepcodi = "TMREPCODI";
        #endregion
    }
}
