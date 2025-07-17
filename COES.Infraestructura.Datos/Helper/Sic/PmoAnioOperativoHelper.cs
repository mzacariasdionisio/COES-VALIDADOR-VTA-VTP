using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PMO_ANIO_OPERATIVO
    /// </summary>
    public class PmoAnioOperativoHelper : HelperBase
    {
        public PmoAnioOperativoHelper() : base(Consultas.PmoAnioOperativoSql)
        {
        }

        public PmoAnioOperativoDTO Create(IDataReader dr)
        {
            PmoAnioOperativoDTO entity = new PmoAnioOperativoDTO();

            int iPmanopcodi = dr.GetOrdinal(this.Pmanopcodi);
            if (!dr.IsDBNull(iPmanopcodi)) entity.Pmanopcodi = Convert.ToInt32(dr.GetValue(iPmanopcodi));

            int iPmanopanio = dr.GetOrdinal(this.Pmanopanio);
            if (!dr.IsDBNull(iPmanopanio)) entity.Pmanopanio = Convert.ToInt32(dr.GetValue(iPmanopanio));

            int iPmanopfecini = dr.GetOrdinal(this.Pmanopfecini);
            if (!dr.IsDBNull(iPmanopfecini)) entity.Pmanopfecini = dr.GetDateTime(iPmanopfecini);

            int iPmanopfecfin = dr.GetOrdinal(this.Pmanopfecfin);
            if (!dr.IsDBNull(iPmanopfecfin)) entity.Pmanopfecfin = dr.GetDateTime(iPmanopfecfin);

            int iPmanopestado = dr.GetOrdinal(this.Pmanopestado);
            if (!dr.IsDBNull(iPmanopestado)) entity.Pmanopestado = Convert.ToInt32(dr.GetValue(iPmanopestado));

            int iPmanopnumversion = dr.GetOrdinal(this.Pmanopnumversion);
            if (!dr.IsDBNull(iPmanopnumversion)) entity.Pmanopnumversion = Convert.ToInt32(dr.GetValue(iPmanopnumversion));

            int iPmanopusucreacion = dr.GetOrdinal(this.Pmanopusucreacion);
            if (!dr.IsDBNull(iPmanopusucreacion)) entity.Pmanopusucreacion = dr.GetString(iPmanopusucreacion);

            int iPmanopfeccreacion = dr.GetOrdinal(this.Pmanopfeccreacion);
            if (!dr.IsDBNull(iPmanopfeccreacion)) entity.Pmanopfeccreacion = dr.GetDateTime(iPmanopfeccreacion);

            int iPmanopusumodificacion = dr.GetOrdinal(this.Pmanopusumodificacion);
            if (!dr.IsDBNull(iPmanopusumodificacion)) entity.Pmanopusumodificacion = dr.GetString(iPmanopusumodificacion);

            int iPmanopfecmodificacion = dr.GetOrdinal(this.Pmanopfecmodificacion);
            if (!dr.IsDBNull(iPmanopfecmodificacion)) entity.Pmanopfecmodificacion = dr.GetDateTime(iPmanopfecmodificacion);

            int iPmanopdesc = dr.GetOrdinal(this.Pmanopdesc);
            if (!dr.IsDBNull(iPmanopdesc)) entity.Pmanopdesc = dr.GetString(iPmanopdesc);

            int iPmanopprocesado = dr.GetOrdinal(this.Pmanopprocesado);
            if (!dr.IsDBNull(iPmanopprocesado)) entity.Pmanopprocesado = Convert.ToInt32(dr.GetValue(iPmanopprocesado));

            return entity;
        }


        #region Mapeo de Campos

        public string Pmanopcodi = "PMANOPCODI";
        public string Pmanopanio = "PMANOPANIO";
        public string Pmanopfecini = "PMANOPFECINI";
        public string Pmanopfecfin = "PMANOPFECFIN";
        public string Pmanopestado = "PMANOPESTADO";
        public string Pmanopnumversion = "PMANOPNUMVERSION";
        public string Pmanopusucreacion = "PMANOPUSUCREACION";
        public string Pmanopfeccreacion = "PMANOPFECCREACION";
        public string Pmanopusumodificacion = "PMANOPUSUMODIFICACION";
        public string Pmanopfecmodificacion = "PMANOPFECMODIFICACION";
        public string Pmanopdesc = "PMANOPDESC";
        public string Pmanopprocesado = "PMANOPPROCESADO";

        public string NumVersiones = "NUMVERSIONES";
        public string PmanopNumFeriados = "PMANOPNUMFERIADOS";

        #endregion

        public string SqlUpdateEstadoBaja
        {
            get { return GetSqlXml("UpdateEstadoBaja"); }
        }
        public string SqlUpdateAprobar
        {
            get { return base.GetSqlXml("UpdateAprobar"); }
        }
        public string SqlUpdateEstadoProcesado
        {
            get { return GetSqlXml("UpdateEstadoProcesado"); }
        }
    }
}
