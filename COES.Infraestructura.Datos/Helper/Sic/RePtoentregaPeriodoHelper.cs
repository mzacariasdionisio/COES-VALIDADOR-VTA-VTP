using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RE_PTOENTREGA_PERIODO
    /// </summary>
    public class RePtoentregaPeriodoHelper : HelperBase
    {
        public RePtoentregaPeriodoHelper(): base(Consultas.RePtoentregaPeriodoSql)
        {
        }

        public RePtoentregaPeriodoDTO Create(IDataReader dr)
        {
            RePtoentregaPeriodoDTO entity = new RePtoentregaPeriodoDTO();

            int iReptopcodi = dr.GetOrdinal(this.Reptopcodi);
            if (!dr.IsDBNull(iReptopcodi)) entity.Reptopcodi = Convert.ToInt32(dr.GetValue(iReptopcodi));

            int iRepentcodi = dr.GetOrdinal(this.Repentcodi);
            if (!dr.IsDBNull(iRepentcodi)) entity.Repentcodi = Convert.ToInt32(dr.GetValue(iRepentcodi));

            int iRepercodi = dr.GetOrdinal(this.Repercodi);
            if (!dr.IsDBNull(iRepercodi)) entity.Repercodi = Convert.ToInt32(dr.GetValue(iRepercodi));

            int iReptopusucreacion = dr.GetOrdinal(this.Reptopusucreacion);
            if (!dr.IsDBNull(iReptopusucreacion)) entity.Reptopusucreacion = dr.GetString(iReptopusucreacion);

            int iReptopfeccreacion = dr.GetOrdinal(this.Reptopfeccreacion);
            if (!dr.IsDBNull(iReptopfeccreacion)) entity.Reptopfeccreacion = dr.GetDateTime(iReptopfeccreacion);

            int iReptopusumodificacion = dr.GetOrdinal(this.Reptopusumodificacion);
            if (!dr.IsDBNull(iReptopusumodificacion)) entity.Reptopusumodificacion = dr.GetString(iReptopusumodificacion);

            int iReptopfecmodificacion = dr.GetOrdinal(this.Reptopfecmodificacion);
            if (!dr.IsDBNull(iReptopfecmodificacion)) entity.Reptopfecmodificacion = dr.GetDateTime(iReptopfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Reptopcodi = "REPTOPCODI";
        public string Repentcodi = "REPENTCODI";
        public string Repercodi = "REPERCODI";
        public string Reptopusucreacion = "REPTOPUSUCREACION";
        public string Reptopfeccreacion = "REPTOPFECCREACION";
        public string Reptopusumodificacion = "REPTOPUSUMODIFICACION";
        public string Reptopfecmodificacion = "REPTOPFECMODIFICACION";
        public string Repentnombre = "REPENTNOMBRE";
        public string Rentabrev = "RENTABREV";
        public string Rentcodi = "RENTCODI";
        #endregion

        public string SqlObtenerPorPtoEntrega
        {
            get { return base.GetSqlXml("ObtenerPorPtoEntrega"); }
        }

        public string SqlObtenerPtoEntregaUtilizadosPorPeriodo
        {
            get { return base.GetSqlXml("ObtenerPtoEntregaUtilizadosPorPeriodo"); }
        }
    }
}
