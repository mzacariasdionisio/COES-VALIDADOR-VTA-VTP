using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_VERSIONAPP
    /// </summary>
    public class WbVersionappHelper : HelperBase
    {
        public WbVersionappHelper(): base(Consultas.WbVersionappSql)
        {
        }

        public WbVersionappDTO Create(IDataReader dr)
        {
            WbVersionappDTO entity = new WbVersionappDTO();

            int iVerappcodi = dr.GetOrdinal(this.Verappcodi);
            if (!dr.IsDBNull(iVerappcodi)) entity.Verappcodi = Convert.ToInt32(dr.GetValue(iVerappcodi));

            int iVerappios = dr.GetOrdinal(this.Verappios);
            if (!dr.IsDBNull(iVerappios)) entity.Verappios = dr.GetDecimal(iVerappios);

            int iVerappandroid = dr.GetOrdinal(this.Verappandroid);
            if (!dr.IsDBNull(iVerappandroid)) entity.Verappandroid = dr.GetDecimal(iVerappandroid);

            int iVerappdescripcion = dr.GetOrdinal(this.Verappdescripcion);
            if (!dr.IsDBNull(iVerappdescripcion)) entity.Verappdescripcion = dr.GetString(iVerappdescripcion);

            int iVerappusucreacion = dr.GetOrdinal(this.Verappusucreacion);
            if (!dr.IsDBNull(iVerappusucreacion)) entity.Verappusucreacion = dr.GetString(iVerappusucreacion);

            int iVerappfeccreacion = dr.GetOrdinal(this.Verappfeccreacion);
            if (!dr.IsDBNull(iVerappfeccreacion)) entity.Verappfeccreacion = dr.GetDateTime(iVerappfeccreacion);

            int iVerappusumodificacion = dr.GetOrdinal(this.Verappusumodificacion);
            if (!dr.IsDBNull(iVerappusumodificacion)) entity.Verappusumodificacion = dr.GetString(iVerappusumodificacion);

            int iVerappfecmodificacion = dr.GetOrdinal(this.Verappfecmodificacion);
            if (!dr.IsDBNull(iVerappfecmodificacion)) entity.Verappfecmodificacion = dr.GetDateTime(iVerappfecmodificacion);

            int iVerappvigente = dr.GetOrdinal(this.Verappvigente);
            if (!dr.IsDBNull(iVerappvigente)) entity.Verappvigente = dr.GetString(iVerappvigente);

            int iVerappupdate = dr.GetOrdinal(this.Verappupdate);
            if (!dr.IsDBNull(iVerappupdate)) entity.Verappupdate = Convert.ToInt32(dr.GetValue(iVerappupdate));

            return entity;
        }


        #region Mapeo de Campos

        public string Verappcodi = "VERAPPCODI";
        public string Verappios = "VERAPPIOS";
        public string Verappandroid = "VERAPPANDROID";
        public string Verappdescripcion = "VERAPPDESCRIPCION";
        public string Verappusucreacion = "VERAPPUSUCREACION";
        public string Verappfeccreacion = "VERAPPFECCREACION";
        public string Verappusumodificacion = "VERAPPUSUMODIFICACION";
        public string Verappfecmodificacion = "VERAPPFECMODIFICACION";
        public string Verappvigente = "VERAPPVIGENTE";
        public string Verappupdate = "VERAPPUPDATE";

        public string SqlObtenerVersionActual
        {
            get { return base.GetSqlXml("ObtenerVersionActual"); }
        }


        #endregion
    }
}
