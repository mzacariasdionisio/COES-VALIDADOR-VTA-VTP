using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_MESCALENDARIO
    /// </summary>
    public class WbMescalendarioHelper : HelperBase
    {
        public WbMescalendarioHelper(): base(Consultas.WbMescalendarioSql)
        {
        }

        public WbMescalendarioDTO Create(IDataReader dr)
        {
            WbMescalendarioDTO entity = new WbMescalendarioDTO();

            int iMescalcodi = dr.GetOrdinal(this.Mescalcodi);
            if (!dr.IsDBNull(iMescalcodi)) entity.Mescalcodi = Convert.ToInt32(dr.GetValue(iMescalcodi));

            int iMescalcolorsat = dr.GetOrdinal(this.Mescalcolorsat);
            if (!dr.IsDBNull(iMescalcolorsat)) entity.Mescalcolorsat = dr.GetString(iMescalcolorsat);

            int iMescalmes = dr.GetOrdinal(this.Mescalmes);
            if (!dr.IsDBNull(iMescalmes)) entity.Mescalmes = Convert.ToInt32(dr.GetValue(iMescalmes));

            int iMescalanio = dr.GetOrdinal(this.Mescalanio);
            if (!dr.IsDBNull(iMescalanio)) entity.Mescalanio = Convert.ToInt32(dr.GetValue(iMescalanio));

            int iMescalcolorsun = dr.GetOrdinal(this.Mescalcolorsun);
            if (!dr.IsDBNull(iMescalcolorsun)) entity.Mescalcolorsun = dr.GetString(iMescalcolorsun);

            int iMescalcolor = dr.GetOrdinal(this.Mescalcolor);
            if (!dr.IsDBNull(iMescalcolor)) entity.Mescalcolor = dr.GetString(iMescalcolor);

            int iMescalinfo = dr.GetOrdinal(this.Mescalinfo);
            if (!dr.IsDBNull(iMescalinfo)) entity.Mescalinfo = dr.GetString(iMescalinfo);

            int iMescaltitulo = dr.GetOrdinal(this.Mescaltitulo);
            if (!dr.IsDBNull(iMescaltitulo)) entity.Mescaltitulo = dr.GetString(iMescaltitulo);

            int iMescaldescripcion = dr.GetOrdinal(this.Mescaldescripcion);
            if (!dr.IsDBNull(iMescaldescripcion)) entity.Mescaldescripcion = dr.GetString(iMescaldescripcion);

            int iMescalestado = dr.GetOrdinal(this.Mescalestado);
            if (!dr.IsDBNull(iMescalestado)) entity.Mescalestado = dr.GetString(iMescalestado);

            int iMescalusumodificacion = dr.GetOrdinal(this.Mescalusumodificacion);
            if (!dr.IsDBNull(iMescalusumodificacion)) entity.Mescalusumodificacion = dr.GetString(iMescalusumodificacion);

            int iMescalfecmodificacion = dr.GetOrdinal(this.Mescalfecmodificacion);
            if (!dr.IsDBNull(iMescalfecmodificacion)) entity.Mescalfecmodificacion = dr.GetDateTime(iMescalfecmodificacion);

            int iMescalcolortit = dr.GetOrdinal(this.Mescalcolortit);
            if (!dr.IsDBNull(iMescalcolortit)) entity.Mescalcolortit = dr.GetString(iMescalcolortit);

            int imescalcolorsubtit = dr.GetOrdinal(this.Mescalcolorsubtit);
            if (!dr.IsDBNull(imescalcolorsubtit)) entity.Mescalcolorsubtit = dr.GetString(imescalcolorsubtit);

            int iMesdiacolor = dr.GetOrdinal(this.Mesdiacolor);
            if (!dr.IsDBNull(iMesdiacolor)) entity.Mesdiacolor = dr.GetString(iMesdiacolor);

            return entity;
        }


        #region Mapeo de Campos

        public string Mescalcodi = "MESCALCODI";
        public string Mescalcolorsat = "MESCALCOLORSAT";
        public string Mescalmes = "MESCALMES";
        public string Mescalanio = "MESCALANIO";
        public string Mescalcolorsun = "MESCALCOLORSUN";
        public string Mescalcolor = "MESCALCOLOR";
        public string Mescalinfo = "MESCALINFO";
        public string Mescaltitulo = "MESCALTITULO";
        public string Mescaldescripcion = "MESCALDESCRIPCION";
        public string Mescalestado = "MESCALESTADO";
        public string Mescalusumodificacion = "MESCALUSUMODIFICACION";
        public string Mescalfecmodificacion = "MESCALFECMODIFICACION";
        public string Mescalcolortit = "MESCALCOLORTIT";
        public string Mescalcolorsubtit = "MESCALCOLORSUBTIT";
        public string Mesdiacolor = "MESDIACOLOR";

        public string SqlQuitarImagen
        {
            get { return base.GetSqlXml("QuitarImagen"); }
        }

        public string SqlActualizarInfografia
        {
            get { return base.GetSqlXml("ActualizarInfografia"); }
        }

        #endregion
    }
}
