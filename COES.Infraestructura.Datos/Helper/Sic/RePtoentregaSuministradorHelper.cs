using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RE_PTOENTREGA_SUMINISTRADOR
    /// </summary>
    public class RePtoentregaSuministradorHelper : HelperBase
    {
        public RePtoentregaSuministradorHelper(): base(Consultas.RePtoentregaSuministradorSql)
        {
        }

        public RePtoentregaSuministradorDTO Create(IDataReader dr)
        {
            RePtoentregaSuministradorDTO entity = new RePtoentregaSuministradorDTO();

            int iReptsmcodi = dr.GetOrdinal(this.Reptsmcodi);
            if (!dr.IsDBNull(iReptsmcodi)) entity.Reptsmcodi = Convert.ToInt32(dr.GetValue(iReptsmcodi));

            int iRepercodi = dr.GetOrdinal(this.Repercodi);
            if (!dr.IsDBNull(iRepercodi)) entity.Repercodi = Convert.ToInt32(dr.GetValue(iRepercodi));

            int iRepentcodi = dr.GetOrdinal(this.Repentcodi);
            if (!dr.IsDBNull(iRepentcodi)) entity.Repentcodi = Convert.ToInt32(dr.GetValue(iRepentcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iReptsmusucreacion = dr.GetOrdinal(this.Reptsmusucreacion);
            if (!dr.IsDBNull(iReptsmusucreacion)) entity.Reptsmusucreacion = dr.GetString(iReptsmusucreacion);

            int iReptsmfeccreacion = dr.GetOrdinal(this.Reptsmfeccreacion);
            if (!dr.IsDBNull(iReptsmfeccreacion)) entity.Reptsmfeccreacion = dr.GetDateTime(iReptsmfeccreacion);

            int iReptsmusumodificacion = dr.GetOrdinal(this.Reptsmusumodificacion);
            if (!dr.IsDBNull(iReptsmusumodificacion)) entity.Reptsmusumodificacion = dr.GetString(iReptsmusumodificacion);

            int iReptsmfecmodificacion = dr.GetOrdinal(this.Reptsmfecmodificacion);
            if (!dr.IsDBNull(iReptsmfecmodificacion)) entity.Reptsmfecmodificacion = dr.GetDateTime(iReptsmfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Reptsmcodi = "REPTSMCODI";
        public string Repercodi = "REPERCODI";
        public string Repentcodi = "REPENTCODI";
        public string Emprcodi = "EMPRCODI";
        public string Reptsmusucreacion = "REPTSMUSUCREACION";
        public string Reptsmfeccreacion = "REPTSMFECCREACION";
        public string Reptsmusumodificacion = "REPTSMUSUMODIFICACION";
        public string Reptsmfecmodificacion = "REPTSMFECMODIFICACION";
        public string Emprnomb = "EMPRNOMB";
        public string Repentnombre = "REPENTNOMBRE";
        
        #endregion

        public string SqlObtenerPorPuntoEntregaPeriodo
        {
            get { return base.GetSqlXml("ObtenerPorPuntoEntregaPeriodo"); }
        }

        public string SqlEliminarPorPeriodo
        {
            get { return base.GetSqlXml("EliminarPorPeriodo"); }
        }
    }
}
