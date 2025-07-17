using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_PARAMETRO_VALOR
    /// </summary>
    public class SiParametroValorHelper : HelperBase
    {
        public SiParametroValorHelper(): base(Consultas.SiParametroValorSql)
        {
        }

        public SiParametroValorDTO Create(IDataReader dr)
        {
            SiParametroValorDTO entity = new SiParametroValorDTO();

            int iSiparvcodi = dr.GetOrdinal(this.Siparvcodi);
            if (!dr.IsDBNull(iSiparvcodi)) entity.Siparvcodi = Convert.ToInt32(dr.GetValue(iSiparvcodi));

            int iSiparcodi = dr.GetOrdinal(this.Siparcodi);
            if (!dr.IsDBNull(iSiparcodi)) entity.Siparcodi = Convert.ToInt32(dr.GetValue(iSiparcodi));

            int iSiparvfechainicial = dr.GetOrdinal(this.Siparvfechainicial);
            if (!dr.IsDBNull(iSiparvfechainicial)) entity.Siparvfechainicial = dr.GetDateTime(iSiparvfechainicial);

            int iSiparvfechafinal = dr.GetOrdinal(this.Siparvfechafinal);
            if (!dr.IsDBNull(iSiparvfechafinal)) entity.Siparvfechafinal = dr.GetDateTime(iSiparvfechafinal);

            int iSiparvvalor = dr.GetOrdinal(this.Siparvvalor);
            if (!dr.IsDBNull(iSiparvvalor)) entity.Siparvvalor = dr.GetDecimal(iSiparvvalor);

            int iSiparvnota = dr.GetOrdinal(this.Siparvnota);
            if (!dr.IsDBNull(iSiparvnota)) entity.Siparvnota = dr.GetString(iSiparvnota);

            int iSiparveliminado = dr.GetOrdinal(this.Siparveliminado);
            if (!dr.IsDBNull(iSiparveliminado)) entity.Siparveliminado = dr.GetString(iSiparveliminado);

            int iSiparvusucreacion = dr.GetOrdinal(this.Siparvusucreacion);
            if (!dr.IsDBNull(iSiparvusucreacion)) entity.Siparvusucreacion = dr.GetString(iSiparvusucreacion);

            int iSiparvfeccreacion = dr.GetOrdinal(this.Siparvfeccreacion);
            if (!dr.IsDBNull(iSiparvfeccreacion)) entity.Siparvfeccreacion = dr.GetDateTime(iSiparvfeccreacion);

            int iSiparvusumodificacion = dr.GetOrdinal(this.Siparvusumodificacion);
            if (!dr.IsDBNull(iSiparvusumodificacion)) entity.Siparvusumodificacion = dr.GetString(iSiparvusumodificacion);

            int iSiparvfecmodificacion = dr.GetOrdinal(this.Siparvfecmodificacion);
            if (!dr.IsDBNull(iSiparvfecmodificacion)) entity.Siparvfecmodificacion = dr.GetDateTime(iSiparvfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Siparvcodi = "SIPARVCODI";
        public string Siparcodi = "SIPARCODI";
        public string Siparvfechainicial = "SIPARVFECHAINICIAL";
        public string Siparvfechafinal = "SIPARVFECHAFINAL";
        public string Siparvvalor = "SIPARVVALOR";
        public string Siparvnota = "SIPARVNOTA";
        public string Siparveliminado = "SIPARVELIMINADO";
        public string Siparvusucreacion = "SIPARVUSUCREACION";
        public string Siparvfeccreacion = "SIPARVFECCREACION";
        public string Siparvusumodificacion = "SIPARVUSUMODIFICACION";
        public string Siparvfecmodificacion = "SIPARVFECMODIFICACION";
        public string Siparabrev = "SIPARABREV";

        public string ObtenerListado
        {
            get { return base.GetSqlXml("ObtenerListado"); }
        }

        public string TotalRegistros
        {
            get { return base.GetSqlXml("TotalRegistros"); }
        }

        public string SqlListByIdParametro
        {
            get { return base.GetSqlXml("ListByIdParametro"); }
        }

        public string SqlListByIdParametroAndFechaInicial
        {
            get { return base.GetSqlXml("ListByIdParametroAndFechaInicial"); }
        }

        public string SqlObtenerValorParametro
        {
            get { return base.GetSqlXml("ObtenerValorParametro"); }
        }

        #endregion
    }
}
