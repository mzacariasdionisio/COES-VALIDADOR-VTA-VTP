using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RER_COMPARATIVO_DET
    /// </summary>
    public class RerComparativoDetHelper : HelperBase
    {
        #region Mapeo de Campos

        public string Rercdtcodi = "RERCDTCODI";
        public string Rerccbcodi = "RERCCBCODI";
        public string Rerevacodi = "REREVACODI";
        public string Reresecodi = "RERESECODI";
        public string Rereeucodi = "REREEUCODI";
        public string Rercdtfecha = "RERCDTFECHA";
        public string Rercdthora = "RERCDTHORA";
        public string Rercdtmedfpm = "RERCDTMEDFPM";
        public string Rercdtenesolicitada = "RERCDTENESOLICITADA";
        public string Rercdteneestimada = "RERCDTENEESTIMADA";
        public string Rercdtpordesviacion = "RERCDTPORDESVIACION";
        public string Rercdtflag = "RERCDTFLAG";
        public string Rercdtusucreacion = "RERCDTUSUCREACION";
        public string Rercdtfeccreacion = "RERCDTFECCREACION";
        public string Rercdtusumodificacion = "RERCDTUSUMODIFICACION";
        public string Rercdtfecmodificacion = "RERCDTFECMODIFICACION";

        public string Emprcodi = "EMPRCODI";
        public string Equicodi = "EQUICODI";
        #endregion

        public RerComparativoDetHelper() : base(Consultas.RerComparativoDetSql)
        {
        }

        public RerComparativoDetDTO Create(IDataReader dr)
        {
            RerComparativoDetDTO entity = new RerComparativoDetDTO();

            int iRercdtcodi = dr.GetOrdinal(this.Rercdtcodi);
            if (!dr.IsDBNull(iRercdtcodi)) entity.Rercdtcodi = Convert.ToInt32(dr.GetValue(iRercdtcodi));

            int iRerccbcodi = dr.GetOrdinal(this.Rerccbcodi);
            if (!dr.IsDBNull(iRerccbcodi)) entity.Rerccbcodi = Convert.ToInt32(dr.GetValue(iRerccbcodi));

            int iRerevacodi = dr.GetOrdinal(this.Rerevacodi);
            if (!dr.IsDBNull(iRerevacodi)) entity.Rerevacodi = Convert.ToInt32(dr.GetValue(iRerevacodi));

            int iReresecodi = dr.GetOrdinal(this.Reresecodi);
            if (!dr.IsDBNull(iReresecodi)) entity.Reresecodi = Convert.ToInt32(dr.GetValue(iReresecodi));

            int iRereeucodi = dr.GetOrdinal(this.Rereeucodi);
            if (!dr.IsDBNull(iRereeucodi)) entity.Rereeucodi = Convert.ToInt32(dr.GetValue(iRereeucodi));

            int iRercdtfecha = dr.GetOrdinal(this.Rercdtfecha);
            if (!dr.IsDBNull(iRercdtfecha)) entity.Rercdtfecha = dr.GetDateTime(iRercdtfecha);

            int iRercdthora = dr.GetOrdinal(this.Rercdthora);
            if (!dr.IsDBNull(iRercdthora)) entity.Rercdthora = dr.GetString(iRercdthora);

            int iRercdtmedfpm = dr.GetOrdinal(this.Rercdtmedfpm);
            if (!dr.IsDBNull(iRercdtmedfpm)) entity.Rercdtmedfpm = dr.GetDecimal(iRercdtmedfpm);

            int iRercdtenesolicitada = dr.GetOrdinal(this.Rercdtenesolicitada);
            if (!dr.IsDBNull(iRercdtenesolicitada)) entity.Rercdtenesolicitada = dr.GetDecimal(iRercdtenesolicitada);

            int iRercdteneestimada = dr.GetOrdinal(this.Rercdteneestimada);
            if (!dr.IsDBNull(iRercdteneestimada)) entity.Rercdteneestimada = dr.GetDecimal(iRercdteneestimada);

            int iRercdtpordesviacion = dr.GetOrdinal(this.Rercdtpordesviacion);
            if (!dr.IsDBNull(iRercdtpordesviacion)) entity.Rercdtpordesviacion = dr.GetDecimal(iRercdtpordesviacion);

            int iRercdtflag = dr.GetOrdinal(this.Rercdtflag);
            if (!dr.IsDBNull(iRercdtflag)) entity.Rercdtflag = dr.GetString(iRercdtflag);

            int iRercdtusucreacion = dr.GetOrdinal(this.Rercdtusucreacion);
            if (!dr.IsDBNull(iRercdtusucreacion)) entity.Rercdtusucreacion = dr.GetString(iRercdtusucreacion);

            int iRercdtfeccreacion = dr.GetOrdinal(this.Rercdtfeccreacion);
            if (!dr.IsDBNull(iRercdtfeccreacion)) entity.Rercdtfeccreacion = dr.GetDateTime(iRercdtfeccreacion);

            int iRercdtusumodificacion = dr.GetOrdinal(this.Rercdtusumodificacion);
            if (!dr.IsDBNull(iRercdtusumodificacion)) entity.Rercdtusumodificacion = dr.GetString(iRercdtusumodificacion);

            int iRercdtfecmodificacion = dr.GetOrdinal(this.Rercdtfecmodificacion);
            if (!dr.IsDBNull(iRercdtfecmodificacion)) entity.Rercdtfecmodificacion = dr.GetDateTime(iRercdtfecmodificacion);

            return entity;
        }

        public string SqlDeleteByRerccbcodi
        {
            get { return base.GetSqlXml("DeleteByRerccbcodi"); }
        }

        public string SqlGetEEDRByCriteria
        {
            get { return base.GetSqlXml("GetEEDRByCriteria"); }
        }

        public string SqlListComparativoAprobadaValidadaByMes
        {
            get { return base.GetSqlXml("ListComparativoAprobadaValidadaByMes"); }
        }
    }
}