using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_AYUDAAPP
    /// </summary>
    public class WbAyudaappHelper : HelperBase
    {
        public WbAyudaappHelper(): base(Consultas.WbAyudaappSql)
        {
        }

        public WbAyudaappDTO Create(IDataReader dr)
        {
            WbAyudaappDTO entity = new WbAyudaappDTO();

            int iAyuappcodi = dr.GetOrdinal(this.Ayuappcodi);
            if (!dr.IsDBNull(iAyuappcodi)) entity.Ayuappcodi = Convert.ToInt32(dr.GetValue(iAyuappcodi));

            int iAyuappcodigoventana = dr.GetOrdinal(this.Ayuappcodigoventana);
            if (!dr.IsDBNull(iAyuappcodigoventana)) entity.Ayuappcodigoventana = dr.GetString(iAyuappcodigoventana);

            int iAyuappdescripcionventana = dr.GetOrdinal(this.Ayuappdescripcionventana);
            if (!dr.IsDBNull(iAyuappdescripcionventana)) entity.Ayuappdescripcionventana = dr.GetString(iAyuappdescripcionventana);

            int iAyuappmensaje = dr.GetOrdinal(this.Ayuappmensaje);
            if (!dr.IsDBNull(iAyuappmensaje)) entity.Ayuappmensaje = dr.GetString(iAyuappmensaje);

            int iAyuappmensajeeng = dr.GetOrdinal(this.Ayuappmensajeeng);
            if (!dr.IsDBNull(iAyuappmensajeeng)) entity.Ayuappmensajeeng = dr.GetString(iAyuappmensajeeng);

            int iAyuappestado = dr.GetOrdinal(this.Ayuappestado);
            if (!dr.IsDBNull(iAyuappestado)) entity.Ayuappestado = dr.GetString(iAyuappestado);

            int iAyuappusucreacion = dr.GetOrdinal(this.Ayuappusucreacion);
            if (!dr.IsDBNull(iAyuappusucreacion)) entity.Ayuappusucreacion = dr.GetString(iAyuappusucreacion);

            int iAyuappfeccreacion = dr.GetOrdinal(this.Ayuappfeccreacion);
            if (!dr.IsDBNull(iAyuappfeccreacion)) entity.Ayuappfeccreacion = dr.GetDateTime(iAyuappfeccreacion);

            int iAyuappusumodificacion = dr.GetOrdinal(this.Ayuappusumodificacion);
            if (!dr.IsDBNull(iAyuappusumodificacion)) entity.Ayuappusumodificacion = dr.GetString(iAyuappusumodificacion);

            int iAyuappfecmodificacion = dr.GetOrdinal(this.Ayuappfecmodificacion);
            if (!dr.IsDBNull(iAyuappfecmodificacion)) entity.Ayuappfecmodificacion = dr.GetDateTime(iAyuappfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Ayuappcodi = "AYUAPPCODI";
        public string Ayuappcodigoventana = "AYUAPPCODIGOVENTANA";
        public string Ayuappdescripcionventana = "AYUAPPDESCRIPCIONVENTANA";
        public string Ayuappmensaje = "AYUAPPMENSAJE";
        public string Ayuappmensajeeng = "AYUAPPMENSAJEENG";
        public string Ayuappestado = "AYUAPPESTADO";
        public string Ayuappusucreacion = "AYUAPPUSUCREACION";
        public string Ayuappfeccreacion = "AYUAPPFECCREACION";
        public string Ayuappusumodificacion = "AYUAPPUSUMODIFICACION";
        public string Ayuappfecmodificacion = "AYUAPPFECMODIFICACION";

        #endregion
    }
}
