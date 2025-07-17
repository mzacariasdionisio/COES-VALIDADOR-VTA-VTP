using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Scada
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TR_ZONA_SP7
    /// </summary>
    public class TrZonaSp7Helper : HelperBase
    {
        public TrZonaSp7Helper(): base(Consultas.TrZonaSp7Sql)
        {
        }

        public TrZonaSp7DTO Create(IDataReader dr)
        {
            TrZonaSp7DTO entity = new TrZonaSp7DTO();

            int iZonacodi = dr.GetOrdinal(this.Zonacodi);
            if (!dr.IsDBNull(iZonacodi)) entity.Zonacodi = Convert.ToInt32(dr.GetValue(iZonacodi));

            int iZonanomb = dr.GetOrdinal(this.Zonanomb);
            if (!dr.IsDBNull(iZonanomb)) entity.Zonanomb = dr.GetString(iZonanomb);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iZonaabrev = dr.GetOrdinal(this.Zonaabrev);
            if (!dr.IsDBNull(iZonaabrev)) entity.Zonaabrev = dr.GetString(iZonaabrev);

            int iZonasiid = dr.GetOrdinal(this.Zonasiid);
            if (!dr.IsDBNull(iZonasiid)) entity.Zonasiid = Convert.ToInt32(dr.GetValue(iZonasiid));

            int iZonausucreacion = dr.GetOrdinal(this.Zonausucreacion);
            if (!dr.IsDBNull(iZonausucreacion)) entity.Zonausucreacion = dr.GetString(iZonausucreacion);

            int iZonafeccreacion = dr.GetOrdinal(this.Zonafeccreacion);
            if (!dr.IsDBNull(iZonafeccreacion)) entity.Zonafeccreacion = dr.GetDateTime(iZonafeccreacion);

            int iZonausumodificacion = dr.GetOrdinal(this.Zonausumodificacion);
            if (!dr.IsDBNull(iZonausumodificacion)) entity.Zonausumodificacion = dr.GetString(iZonausumodificacion);

            int iZonafecmodificacion = dr.GetOrdinal(this.Zonafecmodificacion);
            if (!dr.IsDBNull(iZonafecmodificacion)) entity.Zonafecmodificacion = dr.GetDateTime(iZonafecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Zonacodi = "ZONACODI";
        public string Zonanomb = "ZONANOMB";
        public string Emprcodi = "EMPRCODI";
        public string Zonaabrev = "ZONAABREV";
        public string Zonasiid = "ZONASIID";
        public string Zonausucreacion = "ZONAUSUCREACION";
        public string Zonafeccreacion = "ZONAFECCREACION";
        public string Zonausumodificacion = "ZONAUSUMODIFICACION";
        public string Zonafecmodificacion = "ZONAFECMODIFICACION";
        public string Emprenomb = "EMPRENOMB";

        public string ObtenerListado
        {
            get { return base.GetSqlXml("ObtenerListado"); }
        }

        public string TotalRegistros
        {
            get { return base.GetSqlXml("TotalRegistros"); }
        }

        public string SqlListByEmpresa
        {
            get { return base.GetSqlXml("ListByEmpresa"); }
        }

        #endregion
    }
}
