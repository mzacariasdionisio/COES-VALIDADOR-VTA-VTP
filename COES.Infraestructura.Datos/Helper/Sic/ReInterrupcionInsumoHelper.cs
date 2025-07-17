using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RE_INTERRUPCION_INSUMO
    /// </summary>
    public class ReInterrupcionInsumoHelper : HelperBase
    {
        public ReInterrupcionInsumoHelper(): base(Consultas.ReInterrupcionInsumoSql)
        {
        }

        public ReInterrupcionInsumoDTO Create(IDataReader dr)
        {
            ReInterrupcionInsumoDTO entity = new ReInterrupcionInsumoDTO();

            int iReinincodi = dr.GetOrdinal(this.Reinincodi);
            if (!dr.IsDBNull(iReinincodi)) entity.Reinincodi = Convert.ToInt32(dr.GetValue(iReinincodi));

            int iRepercodi = dr.GetOrdinal(this.Repercodi);
            if (!dr.IsDBNull(iRepercodi)) entity.Repercodi = Convert.ToInt32(dr.GetValue(iRepercodi));

            int iReinincorrelativo = dr.GetOrdinal(this.Reinincorrelativo);
            if (!dr.IsDBNull(iReinincorrelativo)) entity.Reinincorrelativo = Convert.ToInt32(dr.GetValue(iReinincorrelativo));

            int iRepentcodi = dr.GetOrdinal(this.Repentcodi);
            if (!dr.IsDBNull(iRepentcodi)) entity.Repentcodi = Convert.ToInt32(dr.GetValue(iRepentcodi));

            int iReininifecinicio = dr.GetOrdinal(this.Reininifecinicio);
            if (!dr.IsDBNull(iReininifecinicio)) entity.Reininifecinicio = dr.GetDateTime(iReininifecinicio);

            int iReininfecfin = dr.GetOrdinal(this.Reininfecfin);
            if (!dr.IsDBNull(iReininfecfin)) entity.Reininfecfin = dr.GetDateTime(iReininfecfin);

            int iReininprogifecinicio = dr.GetOrdinal(this.Reininprogifecinicio);
            if (!dr.IsDBNull(iReininprogifecinicio)) entity.Reininprogifecinicio = dr.GetDateTime(iReininprogifecinicio);

            int iReininprogfecfin = dr.GetOrdinal(this.Reininprogfecfin);
            if (!dr.IsDBNull(iReininprogfecfin)) entity.Reininprogfecfin = dr.GetDateTime(iReininprogfecfin);

            int iRetintcodi = dr.GetOrdinal(this.Retintcodi);
            if (!dr.IsDBNull(iRetintcodi)) entity.Retintcodi = Convert.ToInt32(dr.GetValue(iRetintcodi));

            int iReninitipo = dr.GetOrdinal(this.Reninitipo);
            if (!dr.IsDBNull(iReninitipo)) entity.Reninitipo = dr.GetString(iReninitipo);

            int iReninicausa = dr.GetOrdinal(this.Reninicausa);
            if (!dr.IsDBNull(iReninicausa)) entity.Reninicausa = dr.GetString(iReninicausa);

            int iRecintcodi = dr.GetOrdinal(this.Recintcodi);
            if (!dr.IsDBNull(iRecintcodi)) entity.Recintcodi = Convert.ToInt32(dr.GetValue(iRecintcodi));

            int iReinincodosi = dr.GetOrdinal(this.Reinincodosi);
            if (!dr.IsDBNull(iReinincodosi)) entity.Reinincodosi = dr.GetString(iReinincodosi);

            int iReinincliente = dr.GetOrdinal(this.Reinincliente);
            if (!dr.IsDBNull(iReinincliente)) entity.Reinincliente = Convert.ToInt32(dr.GetValue(iReinincliente));

            int iReininsuministrador = dr.GetOrdinal(this.Reininsuministrador);
            if (!dr.IsDBNull(iReininsuministrador)) entity.Reininsuministrador = Convert.ToInt32(dr.GetValue(iReininsuministrador));

            int iReininobservacion = dr.GetOrdinal(this.Reininobservacion);
            if (!dr.IsDBNull(iReininobservacion)) entity.Reininobservacion = dr.GetString(iReininobservacion);

            int iReininresponsable1 = dr.GetOrdinal(this.Reininresponsable1);
            if (!dr.IsDBNull(iReininresponsable1)) entity.Reininresponsable1 = Convert.ToInt32(dr.GetValue(iReininresponsable1));

            int iReininporcentaje1 = dr.GetOrdinal(this.Reininporcentaje1);
            if (!dr.IsDBNull(iReininporcentaje1)) entity.Reininporcentaje1 = dr.GetDecimal(iReininporcentaje1);

            int iReininresponsable2 = dr.GetOrdinal(this.Reininresponsable2);
            if (!dr.IsDBNull(iReininresponsable2)) entity.Reininresponsable2 = Convert.ToInt32(dr.GetValue(iReininresponsable2));

            int iReininporcentaje2 = dr.GetOrdinal(this.Reininporcentaje2);
            if (!dr.IsDBNull(iReininporcentaje2)) entity.Reininporcentaje2 = dr.GetDecimal(iReininporcentaje2);

            int iReininresponsable3 = dr.GetOrdinal(this.Reininresponsable3);
            if (!dr.IsDBNull(iReininresponsable3)) entity.Reininresponsable3 = Convert.ToInt32(dr.GetValue(iReininresponsable3));

            int iReininporcentaje3 = dr.GetOrdinal(this.Reininporcentaje3);
            if (!dr.IsDBNull(iReininporcentaje3)) entity.Reininporcentaje3 = dr.GetDecimal(iReininporcentaje3);

            int iReininresponsable4 = dr.GetOrdinal(this.Reininresponsable4);
            if (!dr.IsDBNull(iReininresponsable4)) entity.Reininresponsable4 = Convert.ToInt32(dr.GetValue(iReininresponsable4));

            int iReininporcentaje4 = dr.GetOrdinal(this.Reininporcentaje4);
            if (!dr.IsDBNull(iReininporcentaje4)) entity.Reininporcentaje4 = dr.GetDecimal(iReininporcentaje4);

            int iReininresponsable5 = dr.GetOrdinal(this.Reininresponsable5);
            if (!dr.IsDBNull(iReininresponsable5)) entity.Reininresponsable5 = Convert.ToInt32(dr.GetValue(iReininresponsable5));

            int iReininporcentaje5 = dr.GetOrdinal(this.Reininporcentaje5);
            if (!dr.IsDBNull(iReininporcentaje5)) entity.Reininporcentaje5 = dr.GetDecimal(iReininporcentaje5);

            int iReininusucreacion = dr.GetOrdinal(this.Reininusucreacion);
            if (!dr.IsDBNull(iReininusucreacion)) entity.Reininusucreacion = dr.GetString(iReininusucreacion);

            int iReininfeccreacion = dr.GetOrdinal(this.Reininfeccreacion);
            if (!dr.IsDBNull(iReininfeccreacion)) entity.Reininfeccreacion = dr.GetDateTime(iReininfeccreacion);

            int iReininusumodificacion = dr.GetOrdinal(this.Reininusumodificacion);
            if (!dr.IsDBNull(iReininusumodificacion)) entity.Reininusumodificacion = dr.GetString(iReininusumodificacion);

            int iReininfecmodificacion = dr.GetOrdinal(this.Reininfecmodificacion);
            if (!dr.IsDBNull(iReininfecmodificacion)) entity.Reininfecmodificacion = dr.GetDateTime(iReininfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Reinincodi = "REININCODI";
        public string Repercodi = "REPERCODI";
        public string Reinincorrelativo = "REININCORRELATIVO";
        public string Repentcodi = "REPENTCODI";
        public string Reininifecinicio = "REININIFECINICIO";
        public string Reininfecfin = "REININFECFIN";
        public string Reininprogifecinicio = "REININPROGIFECINICIO";
        public string Reininprogfecfin = "REININPROGFECFIN";
        public string Retintcodi = "RETINTCODI";
        public string Reninitipo = "RENINITIPO";
        public string Reninicausa = "RENINICAUSA";
        public string Recintcodi = "RECINTCODI";
        public string Reinincodosi = "REININCODOSI";
        public string Reinincliente = "REININCLIENTE";
        public string Reininsuministrador = "REININSUMINISTRADOR";
        public string Reininobservacion = "REININOBSERVACION";
        public string Reininresponsable1 = "REININRESPONSABLE1";
        public string Reininporcentaje1 = "REININPORCENTAJE1";
        public string Reininresponsable2 = "REININRESPONSABLE2";
        public string Reininporcentaje2 = "REININPORCENTAJE2";
        public string Reininresponsable3 = "REININRESPONSABLE3";
        public string Reininporcentaje3 = "REININPORCENTAJE3";
        public string Reininresponsable4 = "REININRESPONSABLE4";
        public string Reininporcentaje4 = "REININPORCENTAJE4";
        public string Reininresponsable5 = "REININRESPONSABLE5";
        public string Reininporcentaje5 = "REININPORCENTAJE5";
        public string Reininusucreacion = "REININUSUCREACION";
        public string Reininfeccreacion = "REININFECCREACION";
        public string Reininusumodificacion = "REININUSUMODIFICACION";
        public string Reininfecmodificacion = "REININFECMODIFICACION";

        #endregion
        
        public string SqlObtenerPorPeriodo
        {
            get { return base.GetSqlXml("ObtenerPorPeriodo"); }
        }

    }
}
