using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SMA_OFERTA_DETALLE
    /// </summary>
    public class SmaOfertaDetalleHelper : HelperBase
    {
        public SmaOfertaDetalleHelper()
            : base(Consultas.SmaOfertaDetalleSql)
        {
        }

        public SmaOfertaDetalleDTO Create(IDataReader dr)
        {
            SmaOfertaDetalleDTO entity = new SmaOfertaDetalleDTO();

            int iUrscodi = dr.GetOrdinal(this.Urscodi);
            if (!dr.IsDBNull(iUrscodi)) entity.Urscodi = Convert.ToInt32(dr.GetValue(iUrscodi));

            int iOfdehorainicio = dr.GetOrdinal(this.Ofdehorainicio);
            if (!dr.IsDBNull(iOfdehorainicio)) entity.Ofdehorainicio = dr.GetString(iOfdehorainicio);

            int iOfdehorafin = dr.GetOrdinal(this.Ofdehorafin);
            if (!dr.IsDBNull(iOfdehorafin)) entity.Ofdehorafin = dr.GetString(iOfdehorafin);

            int iOfdeprecio = dr.GetOrdinal(this.Ofdeprecio);
            if (!dr.IsDBNull(iOfdeprecio)) entity.Ofdeprecio = dr.GetString(iOfdeprecio);

            int iOfdedusucreacion = dr.GetOrdinal(this.Ofdedusucreacion);
            if (!dr.IsDBNull(iOfdedusucreacion)) entity.Ofdedusucreacion = dr.GetString(iOfdedusucreacion);

            int iOfdefeccreacion = dr.GetOrdinal(this.Ofdefeccreacion);
            if (!dr.IsDBNull(iOfdefeccreacion)) entity.Ofdefeccreacion = dr.GetDateTime(iOfdefeccreacion);

            int iOfdemoneda = dr.GetOrdinal(this.Ofdemoneda);
            if (!dr.IsDBNull(iOfdemoneda)) entity.Ofdemoneda = dr.GetString(iOfdemoneda);

            int iOfdeusumodificacion = dr.GetOrdinal(this.Ofdeusumodificacion);
            if (!dr.IsDBNull(iOfdeusumodificacion)) entity.Ofdeusumodificacion = dr.GetString(iOfdeusumodificacion);

            int iOfdefecmodificacion = dr.GetOrdinal(this.Ofdefecmodificacion);
            if (!dr.IsDBNull(iOfdefecmodificacion)) entity.Ofdefecmodificacion = dr.GetDateTime(iOfdefecmodificacion);

            int iOfdecodi = dr.GetOrdinal(this.Ofdecodi);
            if (!dr.IsDBNull(iOfdecodi)) entity.Ofdecodi = Convert.ToInt32(dr.GetValue(iOfdecodi));

            int iOfercodi = dr.GetOrdinal(this.Ofercodi);
            if (!dr.IsDBNull(iOfercodi)) entity.Ofercodi = Convert.ToInt32(dr.GetValue(iOfercodi));

            int iOfdepotmaxofer = dr.GetOrdinal(this.Ofdepotmaxofer);
            if (!dr.IsDBNull(iOfdepotmaxofer)) entity.Ofdepotmaxofer = dr.GetDecimal(iOfdepotmaxofer);

            int iOfdepotofer = dr.GetOrdinal(this.Ofdepotofer);
            if (!dr.IsDBNull(iOfdepotofer)) entity.Ofdepotofertada = dr.GetDecimal(iOfdepotofer);

            int iOfdetipo = dr.GetOrdinal(this.Ofdetipo);
            if (!dr.IsDBNull(iOfdetipo)) entity.Ofdetipo = Convert.ToInt32(dr.GetValue(iOfdetipo));

            return entity;
        }


        #region Mapeo de Campos

        public string Urscodi = "OFDEDURS";
        public string Ofdehorainicio = "OFDEHORAINICIO";
        public string Ofdehorafin = "OFDEHORAFIN";
        public string Ofdeprecio = "OFDEPRECIO";
        public string Ofdedusucreacion = "OFDEDUSUCREACION";
        public string Ofdefeccreacion = "OFDEFECCREACION";
        public string Ofdemoneda = "OFDEMONEDA";
        public string Ofdeusumodificacion = "OFDEUSUMODIFICACION";
        public string Ofdefecmodificacion = "OFDEFECMODIFICACION";
        public string Ofdecodi = "OFDECODI";
        public string Ofercodi = "OFERCODI";
        public string Ofdepotmaxofer = "OFDEPOTMAXOFER";
        public string Ofdepotofer = "OFDEPOTOFER";
        public string Ofdetipo = "OFDETIPO";
        public string Gruponomb = "GRUPONOMB";
        public string Oferfuente = "OFERFUENTE";
        #endregion

        public string SqlUpdatePrecio
        {
            get { return base.GetSqlXml("UpdatePrecio"); }
        }

        #region FIT - Aplicativo VTP
        public string urscodivtdvalorizacion = "URSCODI";

        public string SqlListByDate
        {
            get { return GetSqlXml("ListByDate"); }
        }
        public string SqlListByDateTipo
        {
            get { return GetSqlXml("ListByDateTipo"); }
        }

        public string SqlListarPorOfertas
        {
            get { return GetSqlXml("ListarPorOfertas"); }
        }
        

        #endregion
    }
}
