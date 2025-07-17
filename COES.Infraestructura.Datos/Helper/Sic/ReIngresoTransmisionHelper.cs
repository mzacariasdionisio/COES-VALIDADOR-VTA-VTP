using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RE_INGRESO_TRANSMISION
    /// </summary>
    public class ReIngresoTransmisionHelper : HelperBase
    {
        public ReIngresoTransmisionHelper(): base(Consultas.ReIngresoTransmisionSql)
        {
        }

        public ReIngresoTransmisionDTO Create(IDataReader dr)
        {
            ReIngresoTransmisionDTO entity = new ReIngresoTransmisionDTO();

            int iReingcodi = dr.GetOrdinal(this.Reingcodi);
            if (!dr.IsDBNull(iReingcodi)) entity.Reingcodi = Convert.ToInt32(dr.GetValue(iReingcodi));

            int iRepercodi = dr.GetOrdinal(this.Repercodi);
            if (!dr.IsDBNull(iRepercodi)) entity.Repercodi = Convert.ToInt32(dr.GetValue(iRepercodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iReingmoneda = dr.GetOrdinal(this.Reingmoneda);
            if (!dr.IsDBNull(iReingmoneda)) entity.Reingmoneda = dr.GetString(iReingmoneda);

            int iReingvalor = dr.GetOrdinal(this.Reingvalor);
            if (!dr.IsDBNull(iReingvalor)) entity.Reingvalor = dr.GetDecimal(iReingvalor);

            int iReingusucreacion = dr.GetOrdinal(this.Reingusucreacion);
            if (!dr.IsDBNull(iReingusucreacion)) entity.Reingusucreacion = dr.GetString(iReingusucreacion);

            int iReingfeccreacion = dr.GetOrdinal(this.Reingfeccreacion);
            if (!dr.IsDBNull(iReingfeccreacion)) entity.Reingfeccreacion = dr.GetDateTime(iReingfeccreacion);

            int iReingusumodificacion = dr.GetOrdinal(this.Reingusumodificacion);
            if (!dr.IsDBNull(iReingusumodificacion)) entity.Reingusumodificacion = dr.GetString(iReingusumodificacion);

            int iReingfecmodificacion = dr.GetOrdinal(this.Reingfecmodificacion);
            if (!dr.IsDBNull(iReingfecmodificacion)) entity.Reingfecmodificacion = dr.GetDateTime(iReingfecmodificacion);

            int iReingfuente = dr.GetOrdinal(this.Reingfuente);
            if (!dr.IsDBNull(iReingfuente)) entity.Reingfuente = dr.GetString(iReingfuente);

            int iReingsustento = dr.GetOrdinal(this.Reingsustento);
            if (!dr.IsDBNull(iReingsustento)) entity.Reingsustento = dr.GetString(iReingsustento);

            return entity;
        }

        #region Mapeo de Campos

        public string Reingcodi = "REINGCODI";
        public string Repercodi = "REPERCODI";
        public string Emprcodi = "EMPRCODI";
        public string Reingmoneda = "REINGMONEDA";
        public string Reingvalor = "REINGVALOR";
        public string Reingusucreacion = "REINGUSUCREACION";
        public string Reingfeccreacion = "REINGFECCREACION";
        public string Reingusumodificacion = "REINGUSUMODIFICACION";
        public string Reingfecmodificacion = "REINGFECMODIFICACION";
        public string Emprnomb = "EMPRNOMB";
        public string Tipoemprcodi = "TIPOEMPRCODI";
        public string Reingfuente = "REINGFUENTE";
        public string Reingsustento = "REINGSUSTENTO";
        
        #endregion

        public string SqlObtenerEmpresas
        {
            get { return base.GetSqlXml("ObtenerEmpresas"); }
        }

        public string SqlObtenerEmpresasSuministradoras
        {
            get { return base.GetSqlXml("ObtenerEmpresasSuministradoras"); }
        }

        public string SqlObtenerEmpresasSuministradorasTotal
        {
            get { return base.GetSqlXml("ObtenerEmpresasSuministradorasTotal"); }
        }
        public string SqlObtenerPorEmpresaPeriodo
        {
            get { return base.GetSqlXml("ObtenerPorEmpresaPeriodo"); }
        }

        public string SqlActualizarArchivo
        {
            get { return base.GetSqlXml("ActualizarArchivo"); }
        }

        public string SqlObtenerSuministradoresPorPeriodo
        {
            get { return base.GetSqlXml("ObtenerSuministradoresPorPeriodo"); }
        }

        public string SqlObtenerResponsablesPorPeriodo
        {
            get { return base.GetSqlXml("ObtenerResponsablesPorPeriodo"); }
        }
    }
}
