using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RER_COMPARATIVO_CAB
    /// </summary>
    public class RerComparativoCabHelper : HelperBase
    {
        #region Mapeo de Campos

        public string Rerccbcodi = "RERCCBCODI";
        public string Rerevacodi = "REREVACODI";
        public string Reresecodi = "RERESECODI";
        public string Rereeucodi = "REREEUCODI";
        public string Rerccboridatos = "RERCCBORIDATOS";
        public string Rerccbtotenesolicitada = "RERCCBTOTENESOLICITADA";
        public string Rerccbtoteneestimada = "RERCCBTOTENEESTIMADA";
        public string Rerccbusucreacion = "RERCCBUSUCREACION";
        public string Rerccbfeccreacion = "RERCCBFECCREACION";
        public string Rerccbusumodificacion = "RERCCBUSUMODIFICACION";
        public string Rerccbfecmodificacion = "RERCCBFECMODIFICACION";

        #endregion

        public RerComparativoCabHelper() : base(Consultas.RerComparativoCabSql)
        {
        }

        public RerComparativoCabDTO Create(IDataReader dr)
        {
            RerComparativoCabDTO entity = new RerComparativoCabDTO();

            int iRerccbcodi = dr.GetOrdinal(this.Rerccbcodi);
            if (!dr.IsDBNull(iRerccbcodi)) entity.Rerccbcodi = Convert.ToInt32(dr.GetValue(iRerccbcodi));

            int iRerevacodi = dr.GetOrdinal(this.Rerevacodi);
            if (!dr.IsDBNull(iRerevacodi)) entity.Rerevacodi = Convert.ToInt32(dr.GetValue(iRerevacodi));

            int iReresecodi = dr.GetOrdinal(this.Reresecodi);
            if (!dr.IsDBNull(iReresecodi)) entity.Reresecodi = Convert.ToInt32(dr.GetValue(iReresecodi));

            int iRereeucodi = dr.GetOrdinal(this.Rereeucodi);
            if (!dr.IsDBNull(iRereeucodi)) entity.Rereeucodi = Convert.ToInt32(dr.GetValue(iRereeucodi));

            int iRerccboridatos = dr.GetOrdinal(this.Rerccboridatos);
            if (!dr.IsDBNull(iRerccboridatos)) entity.Rerccboridatos = dr.GetString(iRerccboridatos);

            int iRerccbtotenesolicitada = dr.GetOrdinal(this.Rerccbtotenesolicitada);
            if (!dr.IsDBNull(iRerccbtotenesolicitada)) entity.Rerccbtotenesolicitada = dr.GetDecimal(iRerccbtotenesolicitada);

            int iRerccbtoteneestimada = dr.GetOrdinal(this.Rerccbtoteneestimada);
            if (!dr.IsDBNull(iRerccbtoteneestimada)) entity.Rerccbtoteneestimada = dr.GetDecimal(iRerccbtoteneestimada);

            int iRerccbusucreacion = dr.GetOrdinal(this.Rerccbusucreacion);
            if (!dr.IsDBNull(iRerccbusucreacion)) entity.Rerccbusucreacion = dr.GetString(iRerccbusucreacion);

            int iRerccbfeccreacion = dr.GetOrdinal(this.Rerccbfeccreacion);
            if (!dr.IsDBNull(iRerccbfeccreacion)) entity.Rerccbfeccreacion = dr.GetDateTime(iRerccbfeccreacion);

            int iRerccbusumodificacion = dr.GetOrdinal(this.Rerccbusumodificacion);
            if (!dr.IsDBNull(iRerccbusumodificacion)) entity.Rerccbusumodificacion = dr.GetString(iRerccbusumodificacion);

            int iRerccbfecmodificacion = dr.GetOrdinal(this.Rerccbfecmodificacion);
            if (!dr.IsDBNull(iRerccbfecmodificacion)) entity.Rerccbfecmodificacion = dr.GetDateTime(iRerccbfecmodificacion);

            return entity;
        }
    }
}