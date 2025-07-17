using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla MAP_EMPRESAUL
    /// </summary>
    public class MapDemandaHelper : HelperBase
    {
        public MapDemandaHelper(): base(Consultas.MapDemandaSql)
        {
        }

        public MapDemandaDTO Create(IDataReader dr)
        {
            MapDemandaDTO entity = new MapDemandaDTO();

            int iMapdemcodi = dr.GetOrdinal(this.Mapdemcodi);
            if (!dr.IsDBNull(iMapdemcodi)) entity.Mapdemcodi = Convert.ToInt32(dr.GetValue(iMapdemcodi));

            int iMapdemtipo = dr.GetOrdinal(this.Mapdemtipo);
            if (!dr.IsDBNull(iMapdemtipo)) entity.Mapdemtipo = Convert.ToInt32(dr.GetValue(iMapdemtipo));

            int iVermcodi = dr.GetOrdinal(this.Vermcodi);
            if (!dr.IsDBNull(iVermcodi)) entity.Vermcodi = Convert.ToInt32(dr.GetValue(iVermcodi));

            int iMapdemvalor = dr.GetOrdinal(this.Mapdemvalor);
            if (!dr.IsDBNull(iMapdemvalor)) entity.Mapdemvalor = Convert.ToDecimal(dr.GetValue(iMapdemvalor));

            int iMapdemfechaperiodo = dr.GetOrdinal(this.Mapdemfechaperiodo);
            if (!dr.IsDBNull(iMapdemfechaperiodo)) entity.Mapdemfechaperiodo = dr.GetDateTime(iMapdemfechaperiodo);

            int iMapdemfecha = dr.GetOrdinal(this.Mapdemfecha);
            if (!dr.IsDBNull(iMapdemfecha)) entity.Mapdemfecha = dr.GetDateTime(iMapdemfecha);

            int iMapdemperiodo = dr.GetOrdinal(this.Mapdemperiodo);
            if (!dr.IsDBNull(iMapdemperiodo)) entity.Mapdemperiodo = Convert.ToInt32(dr.GetValue(iMapdemperiodo));

            int iMapdemfechafin = dr.GetOrdinal(this.Mapdemfechafin);
            if (!dr.IsDBNull(iMapdemfechafin)) entity.Mapdemfechafin = dr.GetDateTime(iMapdemfechafin);

            return entity;
        }


        #region Mapeo de Campos

        public string Mapdemcodi = "MAPDEMCODI";
        public string Mapdemtipo = "MAPDEMTIPO";
        public string Vermcodi = "VERMCODI";
        public string Mapdemvalor = "MAPDEMVALOR";
        public string Mapdemfechaperiodo = "MAPDEMFECHAPERIODO";
        public string Mapdemfecha = "MAPDEMFECHA";
        public string Mapdemperiodo = "MAPDEMPERIODO";
        public string Mapdemfechafin = "MAPDEMFECHAFIN";

        #endregion
    }
}
