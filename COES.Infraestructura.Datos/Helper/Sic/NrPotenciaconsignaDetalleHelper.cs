using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla NR_POTENCIACONSIGNA_DETALLE
    /// </summary>
    public class NrPotenciaconsignaDetalleHelper : HelperBase
    {
        public NrPotenciaconsignaDetalleHelper(): base(Consultas.NrPotenciaconsignaDetalleSql)
        {
        }

        public NrPotenciaconsignaDetalleDTO Create(IDataReader dr)
        {
            NrPotenciaconsignaDetalleDTO entity = new NrPotenciaconsignaDetalleDTO();

            int iNrpcdcodi = dr.GetOrdinal(this.Nrpcdcodi);
            if (!dr.IsDBNull(iNrpcdcodi)) entity.Nrpcdcodi = Convert.ToInt32(dr.GetValue(iNrpcdcodi));

            int iNrpccodi = dr.GetOrdinal(this.Nrpccodi);
            if (!dr.IsDBNull(iNrpccodi)) entity.Nrpccodi = Convert.ToInt32(dr.GetValue(iNrpccodi));

            int iNrpcdfecha = dr.GetOrdinal(this.Nrpcdfecha);
            if (!dr.IsDBNull(iNrpcdfecha)) entity.Nrpcdfecha = dr.GetDateTime(iNrpcdfecha);

            int iNrpcdmw = dr.GetOrdinal(this.Nrpcdmw);
            if (!dr.IsDBNull(iNrpcdmw)) entity.Nrpcdmw = dr.GetDecimal(iNrpcdmw);

            int iNrpcdmaximageneracion = dr.GetOrdinal(this.Nrpcdmaximageneracion);
            if (!dr.IsDBNull(iNrpcdmaximageneracion)) entity.Nrpcdmaximageneracion = dr.GetString(iNrpcdmaximageneracion);

            int iNrpcdusucreacion = dr.GetOrdinal(this.Nrpcdusucreacion);
            if (!dr.IsDBNull(iNrpcdusucreacion)) entity.Nrpcdusucreacion = dr.GetString(iNrpcdusucreacion);

            int iNrpcdfeccreacion = dr.GetOrdinal(this.Nrpcdfeccreacion);
            if (!dr.IsDBNull(iNrpcdfeccreacion)) entity.Nrpcdfeccreacion = dr.GetDateTime(iNrpcdfeccreacion);

            int iNrpcdusumodificacion = dr.GetOrdinal(this.Nrpcdusumodificacion);
            if (!dr.IsDBNull(iNrpcdusumodificacion)) entity.Nrpcdusumodificacion = dr.GetString(iNrpcdusumodificacion);

            int iNrpcdfecmodificacion = dr.GetOrdinal(this.Nrpcdfecmodificacion);
            if (!dr.IsDBNull(iNrpcdfecmodificacion)) entity.Nrpcdfecmodificacion = dr.GetDateTime(iNrpcdfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Nrpcdcodi = "NRPCDCODI";
        public string Nrpccodi = "NRPCCODI";
        public string Nrpcdfecha = "NRPCDFECHA";
        public string Nrpcdmw = "NRPCDMW";
        public string Nrpcdmaximageneracion = "NRPCDMAXIMAGENERACION";
        public string Nrpcdusucreacion = "NRPCDUSUCREACION";
        public string Nrpcdfeccreacion = "NRPCDFECCREACION";
        public string Nrpcdusumodificacion = "NRPCDUSUMODIFICACION";
        public string Nrpcdfecmodificacion = "NRPCDFECMODIFICACION";
        public string Nrpceliminado = "NRPCELIMINADO";

        public string ObtenerListado
        {
            get { return base.GetSqlXml("ObtenerListado"); }
        }

        public string TotalRegistros
        {
            get { return base.GetSqlXml("TotalRegistros"); }
        }

        public string SqlDeleteTotal
        {
            get { return base.GetSqlXml("DeleteTotal"); }
        }

        


        #endregion
    }
}
