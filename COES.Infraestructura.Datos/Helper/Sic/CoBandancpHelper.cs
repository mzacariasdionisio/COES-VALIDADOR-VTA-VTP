using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CO_BANDANCP
    /// </summary>
    public class CoBandancpHelper : HelperBase
    {
        public CoBandancpHelper(): base(Consultas.CoBandancpSql)
        {
        }

        public CoBandancpDTO Create(IDataReader dr)
        {
            CoBandancpDTO entity = new CoBandancpDTO();

            int iBandcodi = dr.GetOrdinal(this.Bandcodi);
            if (!dr.IsDBNull(iBandcodi)) entity.Bandcodi = Convert.ToInt32(dr.GetValue(iBandcodi));

            int iBandmin = dr.GetOrdinal(this.Bandmin);
            if (!dr.IsDBNull(iBandmin)) entity.Bandmin = dr.GetDecimal(iBandmin);

            int iBandmax = dr.GetOrdinal(this.Bandmax);
            if (!dr.IsDBNull(iBandmax)) entity.Bandmax = dr.GetDecimal(iBandmax);

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iBandfecha = dr.GetOrdinal(this.Bandfecha);
            if (!dr.IsDBNull(iBandfecha)) entity.Bandfecha = dr.GetDateTime(iBandfecha);

            int iBandusumodificacion = dr.GetOrdinal(this.Bandusumodificacion);
            if (!dr.IsDBNull(iBandusumodificacion)) entity.Bandusumodificacion = dr.GetString(iBandusumodificacion);

            int iBandfecmodificacion = dr.GetOrdinal(this.Bandfecmodificacion);
            if (!dr.IsDBNull(iBandfecmodificacion)) entity.Bandfecmodificacion = dr.GetDateTime(iBandfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Bandcodi = "BANDCODI";
        public string Bandmin = "BANDMIN";
        public string Bandmax = "BANDMAX";
        public string Grupocodi = "GRUPOCODI";
        public string Bandfecha = "BANDFECHA";
        public string Bandusumodificacion = "BANDUSUMODIFICACION";
        public string Bandfecmodificacion = "BANDFECMODIFICACION";

        #endregion

        public string SqlGetListBandaNCPxFecha
        {
            get { return base.GetSqlXml("GetListBandaNCPxFecha"); }
        }
    }
}
