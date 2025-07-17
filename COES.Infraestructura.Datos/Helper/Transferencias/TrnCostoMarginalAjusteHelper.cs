using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    public class TrnCostoMarginalAjusteHelper : HelperBase
    {
        public TrnCostoMarginalAjusteHelper() : base(Consultas.TrnCostoMarginalAjusteSql)
        {
        }

        public TrnCostoMarginalAjusteDTO Create(IDataReader dr)
        {
            TrnCostoMarginalAjusteDTO entity = new TrnCostoMarginalAjusteDTO();

            int iTrncmacodi = dr.GetOrdinal(this.Trncmacodi);
            if (!dr.IsDBNull(iTrncmacodi)) entity.Trncmacodi = Convert.ToInt32(dr.GetValue(iTrncmacodi));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iRecacodi = dr.GetOrdinal(this.Recacodi);
            if (!dr.IsDBNull(iRecacodi)) entity.Recacodi = Convert.ToInt32(dr.GetValue(iRecacodi));

            int iTrncmafecha = dr.GetOrdinal(this.Trncmafecha);
            if (!dr.IsDBNull(iTrncmafecha)) entity.Trncmafecha = dr.GetDateTime(iTrncmafecha);

            int iTrncmausucreacion = dr.GetOrdinal(this.Trncmausucreacion);
            if (!dr.IsDBNull(iTrncmausucreacion)) entity.Trncmausucreacion = dr.GetString(iTrncmausucreacion);

            int iTrncmafeccreacion = dr.GetOrdinal(this.Trncmafeccreacion);
            if (!dr.IsDBNull(iTrncmafeccreacion)) entity.Trncmafeccreacion = dr.GetDateTime(iTrncmafeccreacion);

            int iTrncmausumodificacion = dr.GetOrdinal(this.Trncmausumodificacion);
            if (!dr.IsDBNull(iTrncmausumodificacion)) entity.Trncmausumodificacion = dr.GetString(iTrncmausumodificacion);

            int iTrncmafecmodificacion = dr.GetOrdinal(this.Trncmafecmodificacion);
            if (!dr.IsDBNull(iTrncmafecmodificacion)) entity.Trncmafecmodificacion = dr.GetDateTime(iTrncmafecmodificacion);

            entity.TrncmafechaStr = entity.Trncmafecha.ToString("dd/MM/yyyy HH:mm");

            entity.TrncmafeccreacionStr = entity.Trncmafeccreacion.ToString("dd/MM/yyyy HH:mm");

            if(entity.Trncmafecmodificacion != DateTime.MinValue)
                entity.TrncmafecmodificacionStr = entity.Trncmafecmodificacion.ToString("dd/MM/yyyy HH:mm");

            return entity;
        }

        #region Mapeo de Campos
        public string Trncmacodi = "TRNCMACODI";
        public string Pericodi = "PERICODI";
        public string Recacodi = "RECACODI";
        public string Trncmafecha = "TRNCMAFECHA";
        public string Trncmausucreacion = "TRNCMAUSUCREACION";
        public string Trncmafeccreacion = "TRNCMAFECCREACION";
        public string Trncmausumodificacion = "TRNCMAUSUMODIFICACION";
        public string Trncmafecmodificacion = "TRNCMAFECMODIFICACION";

        public string Perianio = "PERIANIO";
        public string Perimes = "PERIMES";
        public string Perinombre = "PERINOMBRE";
        #endregion

        #region Consultas BD
        public string SqlListByPeriodoVersion
        {
            get { return base.GetSqlXml("ListByPeriodoVersion"); }

        }
        public string SqlGetPeriodo
        {
            get { return base.GetSqlXml("GetPeriodo"); }

        }
        public string SqlCopiarAjustesCostosMarginales
        {
            get { return base.GetSqlXml("CopiarAjustesCostosMarginales"); }

        }
        public string SqlDeleteListaAjusteCostoMarginal
        {
            get { return base.GetSqlXml("DeleteListaAjusteCostoMarginal"); }

        }
        #endregion

    }
}
