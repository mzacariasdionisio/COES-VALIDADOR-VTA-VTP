using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla NR_PERIODO_RESUMEN
    /// </summary>
    public class NrPeriodoResumenHelper : HelperBase
    {
        public NrPeriodoResumenHelper(): base(Consultas.NrPeriodoResumenSql)
        {
        }

        public NrPeriodoResumenDTO Create(IDataReader dr)
        {
            NrPeriodoResumenDTO entity = new NrPeriodoResumenDTO();

            int iNrperrcodi = dr.GetOrdinal(this.Nrperrcodi);
            if (!dr.IsDBNull(iNrperrcodi)) entity.Nrperrcodi = Convert.ToInt32(dr.GetValue(iNrperrcodi));

            int iNrpercodi = dr.GetOrdinal(this.Nrpercodi);
            if (!dr.IsDBNull(iNrpercodi)) entity.Nrpercodi = Convert.ToInt32(dr.GetValue(iNrpercodi));

            int iNrcptcodi = dr.GetOrdinal(this.Nrcptcodi);
            if (!dr.IsDBNull(iNrcptcodi)) entity.Nrcptcodi = Convert.ToInt32(dr.GetValue(iNrcptcodi));

            int iNrperrnumobservacion = dr.GetOrdinal(this.Nrperrnumobservacion);
            if (!dr.IsDBNull(iNrperrnumobservacion)) entity.Nrperrnumobservacion = Convert.ToInt32(dr.GetValue(iNrperrnumobservacion));

            int iNrperrobservacion = dr.GetOrdinal(this.Nrperrobservacion);
            if (!dr.IsDBNull(iNrperrobservacion)) entity.Nrperrobservacion = dr.GetString(iNrperrobservacion);

            int iNrperreliminado = dr.GetOrdinal(this.Nrperreliminado);
            if (!dr.IsDBNull(iNrperreliminado)) entity.Nrperreliminado = dr.GetString(iNrperreliminado);

            int iNrperrusucreacion = dr.GetOrdinal(this.Nrperrusucreacion);
            if (!dr.IsDBNull(iNrperrusucreacion)) entity.Nrperrusucreacion = dr.GetString(iNrperrusucreacion);

            int iNrperrfeccreacion = dr.GetOrdinal(this.Nrperrfeccreacion);
            if (!dr.IsDBNull(iNrperrfeccreacion)) entity.Nrperrfeccreacion = dr.GetDateTime(iNrperrfeccreacion);

            int iNrperrusumodificacion = dr.GetOrdinal(this.Nrperrusumodificacion);
            if (!dr.IsDBNull(iNrperrusumodificacion)) entity.Nrperrusumodificacion = dr.GetString(iNrperrusumodificacion);

            int iNrperrfecmodificacion = dr.GetOrdinal(this.Nrperrfecmodificacion);
            if (!dr.IsDBNull(iNrperrfecmodificacion)) entity.Nrperrfecmodificacion = dr.GetDateTime(iNrperrfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Nrperrcodi = "NRPERRCODI";
        public string Nrpercodi = "NRPERCODI";
        public string Nrcptcodi = "NRCPTCODI";
        public string Nrperrnumobservacion = "NRPERRNUMOBSERVACION";
        public string Nrperrobservacion = "NRPERROBSERVACION";
        public string Nrperreliminado = "NRPERRELIMINADO";
        public string Nrperrusucreacion = "NRPERRUSUCREACION";
        public string Nrperrfeccreacion = "NRPERRFECCREACION";
        public string Nrperrusumodificacion = "NRPERRUSUMODIFICACION";
        public string Nrperrfecmodificacion = "NRPERRFECMODIFICACION";
        public string Nrpermes = "NRPERMES";
        public string Nrcptabrev = "NRCPTABREV";


        public string Nrsmodcodi = "NRSMODCODI";
        public string Nrsmodnombre = "NRSMODNOMBRE";        
        public string Pendiente = "PENDIENTE";
        public string Observaciones = "OBSERVACIONES";
        public string Terminado = "TERMINADO";
        public string Proceso = "PROCESO";

        
        public string Nrcptdescripcion = "NRCPTDESCRIPCION";


        public string SqlListSubModuloPeriodo
        {
            get { return base.GetSqlXml("ListSubModuloPeriodo"); }
        }

        public string ObtenerListado
        {
            get { return base.GetSqlXml("ObtenerListado"); }
        }

        public string TotalRegistros
        {
            get { return base.GetSqlXml("TotalRegistros"); }
        }


        public string SqlGetByIdPeriodoConcepto
        {
            get { return GetSqlXml("GetByIdPeriodoConcepto"); }
        }

        #endregion
    }
}
