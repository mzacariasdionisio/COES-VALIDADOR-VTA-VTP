using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RE_INTERRUPCION_SUMINISTRO_DET
    /// </summary>
    public class ReInterrupcionSuministroDetHelper : HelperBase
    {
        public ReInterrupcionSuministroDetHelper(): base(Consultas.ReInterrupcionSuministroDetSql)
        {
        }

        public ReInterrupcionSuministroDetDTO Create(IDataReader dr)
        {
            ReInterrupcionSuministroDetDTO entity = new ReInterrupcionSuministroDetDTO();

            int iReintdevidenciaresp = dr.GetOrdinal(this.Reintdevidenciaresp);
            if (!dr.IsDBNull(iReintdevidenciaresp)) entity.Reintdevidenciaresp = dr.GetString(iReintdevidenciaresp);

            int iReintdconformidadsumi = dr.GetOrdinal(this.Reintdconformidadsumi);
            if (!dr.IsDBNull(iReintdconformidadsumi)) entity.Reintdconformidadsumi = dr.GetString(iReintdconformidadsumi);

            int iReintdcomentariosumi = dr.GetOrdinal(this.Reintdcomentariosumi);
            if (!dr.IsDBNull(iReintdcomentariosumi)) entity.Reintdcomentariosumi = dr.GetString(iReintdcomentariosumi);

            int iReintdevidenciasumi = dr.GetOrdinal(this.Reintdevidenciasumi);
            if (!dr.IsDBNull(iReintdevidenciasumi)) entity.Reintdevidenciasumi = dr.GetString(iReintdevidenciasumi);

            int iReintdcodi = dr.GetOrdinal(this.Reintdcodi);
            if (!dr.IsDBNull(iReintdcodi)) entity.Reintdcodi = Convert.ToInt32(dr.GetValue(iReintdcodi));

            int iReintcodi = dr.GetOrdinal(this.Reintcodi);
            if (!dr.IsDBNull(iReintcodi)) entity.Reintcodi = Convert.ToInt32(dr.GetValue(iReintcodi));

            int iReintdestado = dr.GetOrdinal(this.Reintdestado);
            if (!dr.IsDBNull(iReintdestado)) entity.Reintdestado = dr.GetString(iReintdestado);

            int iReintdorden = dr.GetOrdinal(this.Reintdorden);
            if (!dr.IsDBNull(iReintdorden)) entity.Reintdorden = Convert.ToInt32(dr.GetValue(iReintdorden));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iReintdorcentaje = dr.GetOrdinal(this.Reintdorcentaje);
            if (!dr.IsDBNull(iReintdorcentaje)) entity.Reintdorcentaje = dr.GetDecimal(iReintdorcentaje);

            int iReintdconformidadresp = dr.GetOrdinal(this.Reintdconformidadresp);
            if (!dr.IsDBNull(iReintdconformidadresp)) entity.Reintdconformidadresp = dr.GetString(iReintdconformidadresp);

            int iReintdobservacionresp = dr.GetOrdinal(this.Reintdobservacionresp);
            if (!dr.IsDBNull(iReintdobservacionresp)) entity.Reintdobservacionresp = dr.GetString(iReintdobservacionresp);

            int iReintddetalleresp = dr.GetOrdinal(this.Reintddetalleresp);
            if (!dr.IsDBNull(iReintddetalleresp)) entity.Reintddetalleresp = dr.GetString(iReintddetalleresp);

            int iReintdcomentarioresp = dr.GetOrdinal(this.Reintdcomentarioresp);
            if (!dr.IsDBNull(iReintdcomentarioresp)) entity.Reintdcomentarioresp = dr.GetString(iReintdcomentarioresp);

            int iReintddisposicion = dr.GetOrdinal(this.Reintddisposicion);
            if (!dr.IsDBNull(iReintddisposicion)) entity.Reintddisposicion = dr.GetString(iReintddisposicion);

            int iReintdcompcero = dr.GetOrdinal(this.Reintdcompcero);
            if (!dr.IsDBNull(iReintdcompcero)) entity.Reintdcompcero = dr.GetString(iReintdcompcero);

            return entity;
        }


        #region Mapeo de Campos

        public string Reintdevidenciaresp = "REINTDEVIDENCIARESP";
        public string Reintdconformidadsumi = "REINTDCONFORMIDADSUMI";
        public string Reintdcomentariosumi = "REINTDCOMENTARIOSUMI";
        public string Reintdevidenciasumi = "REINTDEVIDENCIASUMI";
        public string Reintdcodi = "REINTDCODI";
        public string Reintcodi = "REINTCODI";
        public string Reintdestado = "REINTDESTADO";
        public string Reintdorden = "REINTDORDEN";
        public string Emprcodi = "EMPRCODI";
        public string Reintdorcentaje = "REINTDORCENTAJE";
        public string Reintdconformidadresp = "REINTDCONFORMIDADRESP";
        public string Reintdobservacionresp = "REINTDOBSERVACIONRESP";
        public string Reintddetalleresp = "REINTDDETALLERESP";
        public string Reintdcomentarioresp = "REINTDCOMENTARIORESP";
        public string Emprnomb = "EMPRNOMB";
        public string SumId = "SUMID";
        public string SumNomb = "SUMNOMB";
        public string Reintddisposicion = "REINTDDISPOSICION";
        public string Reintdcompcero = "REINTDCOMPCERO";

        #endregion

        public string SqlObtenerPorEmpresaPeriodo
        {
            get { return base.GetSqlXml("ObtenerPorEmpresaPeriodo"); }
        }

        public string SqlObtenerInterrupcionesPorResponsable
        {
            get { return base.GetSqlXml("ObtenerInterrupcionesPorResponsable"); }
        }

        public string SqlActualizarObservacion 
        {
            get { return base.GetSqlXml("ActualizarObservacion"); }
        }

        public string SqlActualizarRespuesta
        {
            get { return base.GetSqlXml("ActualizarRespuesta"); }
        }

        public string SqlActualizarDatosAdicionales
        {
            get { return base.GetSqlXml("ActualizarDatosAdicionales"); }
        }

        public string SqlObtenerPorOrden
        {
            get { return base.GetSqlXml("ObtenerPorOrden"); }
        }

        public string SqlGetResponsablesFinalPorPeriodo
        {
            get { return base.GetSqlXml("GetResponsablesFinalPorPeriodo"); }
        }

        public string SqlGetConformidadResponsableNO
        {
            get { return base.GetSqlXml("GetConformidadResponsableNO"); }
        }
        
        public string SqlActualizarArchivoObservacion
        {
            get { return base.GetSqlXml("ActualizarArchivoObservacion"); }
        }

        public string SqlActualizarArchivoRespuesta
        {
            get { return base.GetSqlXml("ActualizarArchivoRespuesta"); }
        }

        public string SqlActualizarDesdeTrimestral
        {
            get { return base.GetSqlXml("ActualizarDesdeTrimestral"); }
        }

        public string SqlObtenerRegistrosConSustento
        {
            get { return base.GetSqlXml("ObtenerRegistrosConSustento"); }
        }

    }
}
