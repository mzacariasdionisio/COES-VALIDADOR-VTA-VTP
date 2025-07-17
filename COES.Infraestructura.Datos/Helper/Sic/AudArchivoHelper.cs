using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla AUD_ARCHIVO
    /// </summary>
    public class AudArchivoHelper : HelperBase
    {
        public AudArchivoHelper(): base(Consultas.AudArchivoSql)
        {
        }

        public AudArchivoDTO Create(IDataReader dr)
        {
            AudArchivoDTO entity = new AudArchivoDTO();

            int iArchcodi = dr.GetOrdinal(this.Archcodi);
            if (!dr.IsDBNull(iArchcodi)) entity.Archcodi = Convert.ToInt32(dr.GetValue(iArchcodi));

            int iArchnombre = dr.GetOrdinal(this.Archnombre);
            if (!dr.IsDBNull(iArchnombre)) entity.Archnombre = dr.GetString(iArchnombre);

            int iArchruta = dr.GetOrdinal(this.Archruta);
            if (!dr.IsDBNull(iArchruta)) entity.Archruta = dr.GetString(iArchruta);

            int iArchactivo = dr.GetOrdinal(this.Archactivo);
            if (!dr.IsDBNull(iArchactivo)) entity.Archactivo = dr.GetString(iArchactivo);

            int iArchusucreacion = dr.GetOrdinal(this.Archusucreacion);
            if (!dr.IsDBNull(iArchusucreacion)) entity.Archusucreacion = dr.GetString(iArchusucreacion);

            int iArchfechacreacion = dr.GetOrdinal(this.Archfechacreacion);
            if (!dr.IsDBNull(iArchfechacreacion)) entity.Archfechacreacion = dr.GetDateTime(iArchfechacreacion);

            int iArchusumodificacion = dr.GetOrdinal(this.Archusumodificacion);
            if (!dr.IsDBNull(iArchusumodificacion)) entity.Archusumodificacion = dr.GetString(iArchusumodificacion);

            int iArchfechamodificacion = dr.GetOrdinal(this.Archfechamodificacion);
            if (!dr.IsDBNull(iArchfechamodificacion)) entity.Archfechamodificacion = dr.GetDateTime(iArchfechamodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Archcodi = "ARCHCODI";
        public string Archnombre = "ARCHNOMBRE";
        public string Archruta = "ARCHRUTA";
        public string Archactivo = "ARCHACTIVO";
        public string Archusucreacion = "ARCHUSUCREACION";
        public string Archfechacreacion = "ARCHFECHACREACION";
        public string Archusumodificacion = "ARCHUSUMODIFICACION";
        public string Archfechamodificacion = "ARCHFECHAMODIFICACION";

        #endregion
    }
}
