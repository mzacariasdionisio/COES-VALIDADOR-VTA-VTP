using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RE_EVENTO_PERIODO
    /// </summary>
    public class ReEventoPeriodoHelper : HelperBase
    {
        public ReEventoPeriodoHelper(): base(Consultas.ReEventoPeriodoSql)
        {
        }

        public ReEventoPeriodoDTO Create(IDataReader dr)
        {
            ReEventoPeriodoDTO entity = new ReEventoPeriodoDTO();

            int iReevecodi = dr.GetOrdinal(this.Reevecodi);
            if (!dr.IsDBNull(iReevecodi)) entity.Reevecodi = Convert.ToInt32(dr.GetValue(iReevecodi));

            int iRepercodi = dr.GetOrdinal(this.Repercodi);
            if (!dr.IsDBNull(iRepercodi)) entity.Repercodi = Convert.ToInt32(dr.GetValue(iRepercodi));

            int iReevedescripcion = dr.GetOrdinal(this.Reevedescripcion);
            if (!dr.IsDBNull(iReevedescripcion)) entity.Reevedescripcion = dr.GetString(iReevedescripcion);

            int iReevefecha = dr.GetOrdinal(this.Reevefecha);
            if (!dr.IsDBNull(iReevefecha)) entity.Reevefecha = dr.GetDateTime(iReevefecha);

            int iReeveempr1 = dr.GetOrdinal(this.Reeveempr1);
            if (!dr.IsDBNull(iReeveempr1)) entity.Reeveempr1 = Convert.ToInt32(dr.GetValue(iReeveempr1));

            int iReeveempr2 = dr.GetOrdinal(this.Reeveempr2);
            if (!dr.IsDBNull(iReeveempr2)) entity.Reeveempr2 = Convert.ToInt32(dr.GetValue(iReeveempr2));

            int iReeveempr3 = dr.GetOrdinal(this.Reeveempr3);
            if (!dr.IsDBNull(iReeveempr3)) entity.Reeveempr3 = Convert.ToInt32(dr.GetValue(iReeveempr3));

            int iReeveempr4 = dr.GetOrdinal(this.Reeveempr4);
            if (!dr.IsDBNull(iReeveempr4)) entity.Reeveempr4 = Convert.ToInt32(dr.GetValue(iReeveempr4));

            int iReeveempr5 = dr.GetOrdinal(this.Reeveempr5);
            if (!dr.IsDBNull(iReeveempr5)) entity.Reeveempr5 = Convert.ToInt32(dr.GetValue(iReeveempr5));

            int iReeveporc1 = dr.GetOrdinal(this.Reeveporc1);
            if (!dr.IsDBNull(iReeveporc1)) entity.Reeveporc1 = dr.GetDecimal(iReeveporc1);

            int iReeveporc2 = dr.GetOrdinal(this.Reeveporc2);
            if (!dr.IsDBNull(iReeveporc2)) entity.Reeveporc2 = dr.GetDecimal(iReeveporc2);

            int iReeveporc3 = dr.GetOrdinal(this.Reeveporc3);
            if (!dr.IsDBNull(iReeveporc3)) entity.Reeveporc3 = dr.GetDecimal(iReeveporc3);

            int iReeveporc4 = dr.GetOrdinal(this.Reeveporc4);
            if (!dr.IsDBNull(iReeveporc4)) entity.Reeveporc4 = dr.GetDecimal(iReeveporc4);

            int iReeveporc5 = dr.GetOrdinal(this.Reeveporc5);
            if (!dr.IsDBNull(iReeveporc5)) entity.Reeveporc5 = dr.GetDecimal(iReeveporc5);

            int iReevecomentario = dr.GetOrdinal(this.Reevecomentario);
            if (!dr.IsDBNull(iReevecomentario)) entity.Reevecomentario = dr.GetString(iReevecomentario);

            int iReeveestado = dr.GetOrdinal(this.Reeveestado);
            if (!dr.IsDBNull(iReeveestado)) entity.Reeveestado = dr.GetString(iReeveestado);

            int iReeveusucreacion = dr.GetOrdinal(this.Reeveusucreacion);
            if (!dr.IsDBNull(iReeveusucreacion)) entity.Reeveusucreacion = dr.GetString(iReeveusucreacion);

            int iReevefeccreacion = dr.GetOrdinal(this.Reevefeccreacion);
            if (!dr.IsDBNull(iReevefeccreacion)) entity.Reevefeccreacion = dr.GetDateTime(iReevefeccreacion);

            int iReeveusumodificacion = dr.GetOrdinal(this.Reeveusumodificacion);
            if (!dr.IsDBNull(iReeveusumodificacion)) entity.Reeveusumodificacion = dr.GetString(iReeveusumodificacion);

            int iReevefecmodificacion = dr.GetOrdinal(this.Reevefecmodificacion);
            if (!dr.IsDBNull(iReevefecmodificacion)) entity.Reevefecmodificacion = dr.GetDateTime(iReevefecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Reevecodi = "REEVECODI";
        public string Repercodi = "REPERCODI";
        public string Reevedescripcion = "REEVEDESCRIPCION";
        public string Reevefecha = "REEVEFECHA";
        public string Reeveempr1 = "REEVEEMPR1";
        public string Reeveempr2 = "REEVEEMPR2";
        public string Reeveempr3 = "REEVEEMPR3";
        public string Reeveempr4 = "REEVEEMPR4";
        public string Reeveempr5 = "REEVEEMPR5";
        public string Reeveporc1 = "REEVEPORC1";
        public string Reeveporc2 = "REEVEPORC2";
        public string Reeveporc3 = "REEVEPORC3";
        public string Reeveporc4 = "REEVEPORC4";
        public string Reeveporc5 = "REEVEPORC5";
        public string Reevecomentario = "REEVECOMENTARIO";
        public string Reeveestado = "REEVEESTADO";
        public string Reeveusucreacion = "REEVEUSUCREACION";
        public string Reevefeccreacion = "REEVEFECCREACION";
        public string Reeveusumodificacion = "REEVEUSUMODIFICACION";
        public string Reevefecmodificacion = "REEVEFECMODIFICACION";
        public string Responsablenomb1 = "RESPONSABLENOMB1";
        public string Responsablenomb2 = "RESPONSABLENOMB2";
        public string Responsablenomb3 = "RESPONSABLENOMB3";
        public string Responsablenomb4 = "RESPONSABLENOMB4";
        public string Responsablenomb5 = "RESPONSABLENOMB5";

        #endregion

        public string SqlObtenerEventosUtilizadosPorPeriodo
        {
            get { return base.GetSqlXml("ObtenerEventosUtilizadosPorPeriodo"); }
        }
    }
}
