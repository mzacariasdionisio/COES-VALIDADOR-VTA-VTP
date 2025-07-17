using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_RESERVA
    /// </summary>
    public class PrReservaHelper : HelperBase
    {
        public PrReservaHelper(): base(Consultas.PrReservaSql)
        {
        }

        public PrReservaDTO Create(IDataReader dr)
        {
            PrReservaDTO entity = new PrReservaDTO();

            int iPrsvcodi = dr.GetOrdinal(this.Prsvcodi);
            if (!dr.IsDBNull(iPrsvcodi)) entity.Prsvcodi = Convert.ToInt32(dr.GetValue(iPrsvcodi));

            int iPrsvdato = dr.GetOrdinal(this.Prsvdato);
            if (!dr.IsDBNull(iPrsvdato)) entity.Prsvdato = dr.GetString(iPrsvdato);

            int iPrsvactivo = dr.GetOrdinal(this.Prsvactivo);
            if (!dr.IsDBNull(iPrsvactivo)) entity.Prsvactivo = Convert.ToInt32(dr.GetValue(iPrsvactivo));

            int iPrsvfechavigencia = dr.GetOrdinal(this.Prsvfechavigencia);
            if (!dr.IsDBNull(iPrsvfechavigencia)) entity.Prsvfechavigencia = dr.GetDateTime(iPrsvfechavigencia);

            int iPrsvfeccreacion = dr.GetOrdinal(this.Prsvfeccreacion);
            if (!dr.IsDBNull(iPrsvfeccreacion)) entity.Prsvfeccreacion = dr.GetDateTime(iPrsvfeccreacion);

            int iPrsvusucreacion = dr.GetOrdinal(this.Prsvusucreacion);
            if (!dr.IsDBNull(iPrsvusucreacion)) entity.Prsvusucreacion = dr.GetString(iPrsvusucreacion);

            int iPrsvfecmodificacion = dr.GetOrdinal(this.Prsvfecmodificacion);
            if (!dr.IsDBNull(iPrsvfecmodificacion)) entity.Prsvfecmodificacion = dr.GetDateTime(iPrsvfecmodificacion);

            int iPrsvusumodificacion = dr.GetOrdinal(this.Prsvusumodificacion);
            if (!dr.IsDBNull(iPrsvusumodificacion)) entity.Prsvusumodificacion = dr.GetString(iPrsvusumodificacion);

            int iPrsvtipo = dr.GetOrdinal(this.Prsvtipo);
            if (!dr.IsDBNull(iPrsvtipo)) entity.Prsvtipo = Convert.ToInt32(dr.GetValue(iPrsvtipo));

            return entity;
        }


        #region Mapeo de Campos

        public string Prsvcodi = "PRSVCODI";
        public string Prsvdato = "PRSVDATO";
        public string Prsvactivo = "PRSVACTIVO";
        public string Prsvfechavigencia = "PRSVFECHAVIGENCIA";
        public string Prsvfeccreacion = "PRSVFECCREACION";
        public string Prsvusucreacion = "PRSVUSUCREACION";
        public string Prsvfecmodificacion = "PRSVFECMODIFICACION";
        public string Prsvusumodificacion = "PRSVUSUMODIFICACION";
        public string Prsvtipo = "PRSVTIPO";

        #endregion

        public string SqlActualizarEstadoRegistro
        {
            get { return GetSqlXml("ActualizarEstadoRegistro"); }
        }

    }
}
